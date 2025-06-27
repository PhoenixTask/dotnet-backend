using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Users.Register;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using SharedKernel;
using Shouldly;
using Xunit;

namespace ApplicationTests.Users;

public class RegisterUserCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContextMock;
    private readonly IPasswordHasher _passwordHasherMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _dbContextMock = Substitute.For<IApplicationDbContext>();
        _passwordHasherMock = Substitute.For<IPasswordHasher>();
        _handler = new RegisterUserCommandHandler(_dbContextMock, _passwordHasherMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEmailAlreadyExists()
    {
        // Arrange
        DbSet<User> existingUsers = new List<User>
    {
        new() { Email = "existing@email.com" }
    }.AsQueryable().BuildMockDbSet();

        _dbContextMock.Users.Returns(existingUsers);

        var command = new RegisterUserCommand("NewUser","existing@email.com", "Password123",string.Empty,string.Empty);

        // Act
        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(UserErrors.EmailNotUnique);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUsernameAlreadyExists()
    {
        // Arrange
        DbSet<User> existingUsers = new List<User>
    {
        new() { NormalizedUserName = "TAKENUSERNAME" }
    }.AsQueryable().BuildMockDbSet();

        _dbContextMock.Users.Returns(existingUsers);

        var command = new RegisterUserCommand("TakenUsername", "newuser@example.com", "Password123", string.Empty, string.Empty);

        // Act
        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(UserErrors.UsernameNotUnique);
    }

    [Fact]
    public async Task Handle_ShouldRegisterUser_WhenDataIsValid()
    {
        // Arrange
        DbSet<User> usersDbSet = new List<User>().AsQueryable().BuildMockDbSet();
        _dbContextMock.Users.Returns(usersDbSet);

        var command = new RegisterUserCommand("NewUser","newuser@example.com", "SecretPassword", string.Empty, string.Empty);

        _passwordHasherMock.Hash("SecretPassword").Returns("hashed-password");

        // Act
        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBe(Guid.Empty);

        await _dbContextMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
