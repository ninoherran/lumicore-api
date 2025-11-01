using Lumicore.Domain;
using Lumicore.Domain.core.ioc;
using Lumicore.Domain.user;
using Lumicore.Domain.user.repository;
using Moq;
using NUnit.Framework;

namespace Lumicore.Test;

public class BaseTest
{
    public static TestInjector Injector { get; private set; } = new();
    
    public Mock<UserRepository> UserRepositoryMock { get; set; } = Injector.UserRepositoryMock;
    public Mock<SetupService> SetupServiceMock { get; set; } = Injector.SetupServiceMock;

    public BaseTest()
    {
        Locator.Load(Injector);
    }

    [SetUp]
    public void BeforeEach()
    {
        UserRepositoryMock.Reset();
        SetupServiceMock.Reset();
        // Ensure Locator is pointing to the test injector for each test
        Locator.Load(Injector);
    }
}

public class TestInjector : IInjector
{
    public Mock<UserRepository> UserRepositoryMock { get; set; } = new() { CallBase = false };
    public Mock<SetupService> SetupServiceMock { get; set; } = new() { CallBase = false };

    public SetupService SetupService()
    {
        return SetupServiceMock.Object;
    }

    public UserRepository UserRepository()
    {
        return UserRepositoryMock.Object;
    }


    public UserService UserService()
    {
        return new();   
    }
}