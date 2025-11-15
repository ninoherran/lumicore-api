using Lumicore.Domain;
using Lumicore.Domain.core.ioc;
using Lumicore.Domain.role;
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
        Locator.Load(Injector);
    }
}

public class TestInjector : IInjector
{
    public Mock<UserService> UserServiceMock { get; set; } = new() { CallBase = false };
    public Mock<RoleService> RoleServiceMock { get; set; } = new() { CallBase = false };
    public Mock<UserRepository> UserRepositoryMock { get; set; } = new() { CallBase = false };
    public Mock<SetupService> SetupServiceMock { get; set; } = new() { CallBase = false };
    public Mock<RoleRepository> RoleRepositoryMock { get; set; } = new() { CallBase = false };

    public SetupService SetupService()
    {
        return SetupServiceMock.Object;
    }

    public UserRepository UserRepository()
    {
        return UserRepositoryMock.Object;
    }


    public UserService UserService() => UserServiceMock.Object;

    public RoleService RoleService() => RoleServiceMock.Object;
    public RoleRepository RoleRepository() => RoleRepositoryMock.Object;
}