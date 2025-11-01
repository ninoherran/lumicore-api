using System;
using System.Threading.Tasks;
using FluentAssertions;
using Lumicore.Domain.core.errors;
using Lumicore.Domain.user;
using Moq;

namespace Lumicore.Test.unit;

[TestFixture]
public class UserServiceTest : BaseTest
{
    [Test]
    public void CreateUserAdmin_ShouldCreateAdminAndPersist()
    {
        var svc = new UserService();
        var user = svc.CreateUserAdmin("admin@acme.com", "Admin", "One", "pwd123");

        user.Should().NotBeNull();
        user.Email.Should().Be("admin@acme.com");
        user.IsAdmin.Should().BeTrue();
        UserRepositoryMock.Verify(r => r.Add(It.Is<User>(u => u.Email == "admin@acme.com" && u.IsAdmin)), Times.Once);
    }

    [Test]
    public async Task CreateUser_ShouldThrow_WhenNotWhitelisted()
    {
        UserRepositoryMock.Setup(r => r.IsUserWhitelisted("user@acme.com"))
            .ReturnsAsync(false);

        var svc = new UserService();
        var act = () => svc.CreateUser("user@acme.com", "User", "One", "pwd123");

        var ex = await act.Should().ThrowAsync<DomainException>();
        ex.Which.Code.Should().Be(ErrorCode.UserNotWhitelisted);

        UserRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
    }

    [Test]
    public async Task CreateUser_ShouldPersist_WhenWhitelisted()
    {
        UserRepositoryMock.Setup(r => r.IsUserWhitelisted("user@acme.com"))
            .ReturnsAsync(true);

        var svc = new UserService();
        var user = await svc.CreateUser("user@acme.com", "User", "One", "pwd123");

        user.Should().NotBeNull();
        user.Email.Should().Be("user@acme.com");
        UserRepositoryMock.Verify(r => r.Add(It.Is<User>(u => u.Email == "user@acme.com")), Times.Once);
    }

    [Test]
    public async Task Authenticate_ShouldReturnUser_WhenPasswordMatches()
    {
        var existing = new User("john@acme.com", "John", "Doe", "correct");
        UserRepositoryMock.Setup(r => r.GetByEmail("john@acme.com"))
            .ReturnsAsync(existing);

        var svc = new UserService();
        var user = await svc.Authenticate("john@acme.com", "correct");

        user.Should().BeSameAs(existing);
    }

    [Test]
    public async Task Authenticate_ShouldThrow_WhenPasswordInvalid()
    {
        var existing = new User("john@acme.com", "John", "Doe", "correct");
        UserRepositoryMock.Setup(r => r.GetByEmail("john@acme.com"))
            .ReturnsAsync(existing);

        var svc = new UserService();
        var act = () => svc.Authenticate("john@acme.com", "wrong");

        var ex = await act.Should().ThrowAsync<DomainException>();
        ex.Which.Code.Should().Be(ErrorCode.InvalidPassword);
    }

    [Test]
    public void AddToWhiteList_ShouldDelegateToRepository()
    {
        var svc = new UserService();
        svc.AddToWhiteList("guest@acme.com");

        UserRepositoryMock.Verify(r => r.AddToWhiteList("guest@acme.com"), Times.Once);
    }
}
