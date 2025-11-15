using FluentAssertions;
using Lumicore.Domain.user;
using Lumicore.Endpoint.controller;
using Lumicore.Endpoint.controller.dto;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Lumicore.Test.unit.setup.isInit;

[TestFixture]
public class IsInitTest : BaseTest
{
    [Test]
    public void ShouldReturnBadRequestIfSetupNotCompleted()
    {
        UserRepositoryMock.Setup(ur => ur.HasAnyUser()).Returns(Task.FromResult(false));
        var setupController = new SetupController();
        var result = setupController.IsInit().Result;
        
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public void ShouldReturnOkIfSetupCompleted()
    {
        UserRepositoryMock.Setup(ur => ur.HasAnyUser()).Returns(Task.FromResult(true));
        var setupController = new SetupController();
        var result = setupController.IsInit().Result;
        
        result.Should().BeOfType<OkResult>();
    }

    [Test]
    public void ShouldBeAbleToSetupIfNoUsers()
    {
        var fakeAdmin = new UserRegisterDto{Email = "admin@admin.com", Firstname = "Admin Admin", Password = "123456"};
        UserRepositoryMock.Setup(ur => ur.HasAnyUser()).Returns(Task.FromResult(false));
        
        var setupController = new SetupController();
        var result = setupController.Init(fakeAdmin).Result;
        
        result.Should().BeOfType<OkResult>();
        UserRepositoryMock.Verify(ur => ur.Add(It.IsAny<User>()), Times.Once);
    }

    [Test]
    public void CreatedUserShouldBeAdmin()
    {
        var fakeAdmin = new UserRegisterDto{Email = "admin@admin.com", Firstname = "Admin", Lastname = "Admin", Password = "123456"};
        User createdUser = null;
        UserRepositoryMock.Setup(ur => ur.HasAnyUser()).Returns(Task.FromResult(false));
        UserRepositoryMock.Setup(ur => ur.Add(It.IsAny<User>()))
            .Callback<User>(u => createdUser = u);
        
        var setupController = new SetupController();
        var result = setupController.Init(fakeAdmin).Result;
        
        result.Should().BeOfType<OkResult>();
        createdUser.IsAdmin.Should().BeTrue();
        createdUser.Email.Should().Be(fakeAdmin.Email);
        createdUser.FirstName.Should().Be(fakeAdmin.Firstname);
        createdUser.LastName.Should().Be(fakeAdmin.Lastname);
        createdUser.PasswordHash.Should().NotBeNullOrEmpty();
    }
}