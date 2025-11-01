using System;
using System.Threading.Tasks;
using FluentAssertions;
using Lumicore.Domain.core.errors;
using Lumicore.Domain.user;
using Lumicore.Endpoint.controller.dto;
using Lumicore.Endpoint.controller.user;
using Lumicore.Endpoint.controller.user.dto;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Lumicore.Test.unit;

[TestFixture]
public class UserControllerTest : BaseTest
{
    [Test]
    public void Whitelist_ShouldAddEmail_AndReturnOk()
    {
        var controller = new UserController();
        var dto = new WhitelistDto { Email = "whitelist@acme.com" };

        var result = controller.Post(dto);

        result.Should().BeOfType<OkResult>();
        UserRepositoryMock.Verify(r => r.AddToWhiteList("whitelist@acme.com"), Times.Once);
    }

    [Test]
    public async Task Create_ShouldReturnOk_AndPersistUser_WhenWhitelisted()
    {
        UserRepositoryMock.Setup(r => r.IsUserWhitelisted("user@acme.com"))
            .ReturnsAsync(true);

        var controller = new UserController();
        var dto = new UserRegisterDto
        {
            Email = "user@acme.com",
            Firstname = "User",
            Lastname = "One",
            Password = "pwd123"
        };

        var result = await controller.Get(dto);

        result.Should().BeOfType<OkResult>();
        UserRepositoryMock.Verify(r => r.Add(It.Is<User>(u => u.Email == "user@acme.com")), Times.Once);
    }

    [Test]
    public async Task Create_ShouldThrow_WhenNotWhitelisted()
    {
        UserRepositoryMock.Setup(r => r.IsUserWhitelisted("blocked@acme.com"))
            .ReturnsAsync(false);

        var controller = new UserController();
        var dto = new UserRegisterDto
        {
            Email = "blocked@acme.com",
            Firstname = "User",
            Lastname = "Two",
            Password = "pwd123"
        };

        var act = async () => await controller.Get(dto);
        var ex = await act.Should().ThrowAsync<DomainException>();
        ex.Which.Code.Should().Be(ErrorCode.UserNotWhitelisted);

        UserRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
    }
}
