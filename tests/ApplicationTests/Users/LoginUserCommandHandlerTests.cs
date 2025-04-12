using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Users.Login;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using SharedKernel;
using Shouldly;
using Xunit;

namespace ApplicationTests.Users;

public class LoginUserCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContextMock;
    private readonly IPasswordHasher _passwordHasherMock;
    private readonly ITokenProvider _tokenProviderMock;
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        _dbContextMock = Substitute.For<IApplicationDbContext>();
        _passwordHasherMock = Substitute.For<IPasswordHasher>();
        _tokenProviderMock = Substitute.For<ITokenProvider>();
        _handler = new LoginUserCommandHandler(_dbContextMock, _passwordHasherMock, _tokenProviderMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        DbSet<User> emptyUsers = new List<User>().AsQueryable().BuildMockDbSet();
        _dbContextMock.Users.Returns(emptyUsers);

        var command = new LoginUserCommand("NoErfan", "password");

        // Act
        Result<string> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(UserErrors.NotFoundByUserName);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = new User
        {
            UserName = "Erfan",
            NormalizedUserName = "ERFAN",
            PasswordHash = "some-hashed-password"
        };

        DbSet<User> mockUsers = new List<User> { user }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Users.Returns(mockUsers);

        var command = new LoginUserCommand("Erfan", "wrong-password");

        _passwordHasherMock.Verify("wrong-password", "some-hashed-password").Returns(false);

        // Act
        Result<string> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(UserErrors.NotFoundByUserName);
    }

    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialsAreCorrect()
    {
        // Arrange
        var user = new User
        {
            UserName = "Erfan",
            NormalizedUserName = "ERFAN",
            PasswordHash = "correct-hash"
        };

        DbSet<User> mockUsers = new List<User> { user }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Users.Returns(mockUsers);

        var command = new LoginUserCommand("Erfan", "secret");

        _passwordHasherMock.Verify("secret", "correct-hash").Returns(true);
        _tokenProviderMock.Create(user).Returns("secure-token");

        // Act
        Result<string> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe("secure-token");
    }

    [Fact]
    public async Task Handle_ShouldNormalizeUsername_BeforeLookup()
    {
        // Arrange
        var user = new User
        {
            UserName = "mixedcasE",
            NormalizedUserName = "MIXEDCASE",
            PasswordHash = "hash"
        };

        DbSet<User> mockUsers = new List<User> { user }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Users.Returns(mockUsers);

        var command = new LoginUserCommand("MiXeDcAsE", "pw");

        _passwordHasherMock.Verify("pw", "hash").Returns(true);
        _tokenProviderMock.Create(user).Returns("token");

        // Act
        Result<string> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe("token");
    }
}
