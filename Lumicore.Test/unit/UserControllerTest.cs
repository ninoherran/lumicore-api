using FluentAssertions;
using Lumicore.Endpoint.controller.user;
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

        var result = controller.AddToWhitelist("whitelist@nino.com");

        result.Should().BeOfType<OkResult>();
        UserRepositoryMock.Verify(r => r.AddToWhiteList("whitelist@nino.com"), Times.Once);
    }
}
