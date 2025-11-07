using System;
using System.Threading.Tasks;
using FluentAssertions;
using Lumicore.Domain.core.errors;
using Lumicore.Domain.user;
using Lumicore.Endpoint.auth;
using Lumicore.Endpoint.controller;
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
}
