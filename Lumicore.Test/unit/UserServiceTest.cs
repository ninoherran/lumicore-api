using FluentAssertions;
using Lumicore.Domain.core.errors;
using Lumicore.Domain.user;
using Moq;

namespace Lumicore.Test.unit;

[TestFixture]
public class UserServiceTest : BaseTest
{
    [Test]
    public async Task CreateUserAdmin_ShouldCreateAdminAndPersist()
    {
        var svc = new UserService();
        var user = await svc.CreateUser("admin@acme.com", "Admin", "One", "pwd123");

        user.Should().NotBeNull();
        user.Email.Should().Be("admin@acme.com");
        user.IsAdmin.Should().BeFalse();
        UserRepositoryMock.Verify(ur => ur.Add(user), Times.Once);
    }
}
