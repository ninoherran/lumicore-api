using FluentAssertions;
using Lumicore.Domain.user;
using Lumicore.Endpoint.controller;
using Lumicore.Endpoint.controller.dto;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Lumicore.Test.unit;

[TestFixture]
public class SetupControllerTest : BaseTest
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
        var fakeAdmin = new SetupDto{Email = "admin@admin.com", Fullname = "Admin Admin", Password = "123456"};
        UserRepositoryMock.Setup(ur => ur.HasAnyUser()).Returns(Task.FromResult(false));
        
        var setupController = new SetupController();
        var result = setupController.Init(fakeAdmin).Result;
        
        result.Should().BeOfType<OkResult>();
        UserRepositoryMock.Verify(ur => ur.Add(It.IsAny<User>()), Times.Once);
    }
    
    [Test]
    public void ShouldNotBeAbleToSetupIfUsersExist()
    {
        UserRepositoryMock.Setup(ur => ur.HasAnyUser()).Returns(Task.FromResult(true));
        var fakeAdmin = new SetupDto{Email = "admin@admin.com", Fullname = "Admin Admin", Password = "123456"};
        
        var setupController = new SetupController();
        var result = setupController.Init(fakeAdmin).Result;
        
        result.Should().BeOfType<BadRequestResult>();
    }
        
}