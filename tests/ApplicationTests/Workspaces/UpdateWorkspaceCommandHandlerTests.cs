using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Users.AccessAction;
using Application.Workspaces.Update;
using Domain.Users;
using Domain.Workspaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using SharedKernel;
using Shouldly;
using Xunit;

namespace ApplicationTests.Workspaces;

public class UpdateWorkspaceCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContextMock;
    private readonly IUserContext _userContextMock;
    private readonly ISender _senderMock;
    private readonly UpdateWorkspaceCommandHandler _handler;
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _workspaceId = Guid.NewGuid();

    public UpdateWorkspaceCommandHandlerTests()
    {
        _dbContextMock = Substitute.For<IApplicationDbContext>();
        _userContextMock = Substitute.For<IUserContext>();
        _senderMock = Substitute.For<ISender>();
        _userContextMock.UserId.Returns(_userId);
        _handler = new UpdateWorkspaceCommandHandler(_dbContextMock, _userContextMock, _senderMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenWorkspaceDoesNotExist()
    {
        // Arrange
        DbSet<Workspace> workspaceDbSet = new List<Workspace>().AsQueryable().BuildMockDbSet();
        _dbContextMock.Workspaces.Returns(workspaceDbSet);
        var command = new UpdateWorkspaceCommand(_workspaceId, "I'm Not Exist", "hollow");

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(WorkspaceErrors.NotFound(Arg.Any<Guid>()).Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserHasNoPermission()
    {
        // Arrange
        var workspace = new Workspace { Id = _workspaceId, Name = "Secret of School", Color = "orange" };
        DbSet<Workspace> workspaceDbSet = new List<Workspace> { workspace }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Workspaces.Returns(workspaceDbSet);
        _senderMock.Send(Arg.Any<UserAccessCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure(UserErrors.InvalidPermission));
        var command = new UpdateWorkspaceCommand(_workspaceId, "Secret", "grass");

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(UserErrors.InvalidPermission.Code);
    }

    [Fact]
    public async Task Handle_ShouldUpdateWorkspace_WhenUserHasPermission()
    {
        // Arrange
        var workspace = new Workspace { Id = _workspaceId, Name = "Secret of School", Color = "orange" };
        DbSet<Workspace> dbSet = new List<Workspace> { workspace }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Workspaces.Returns(dbSet);
        _senderMock.Send(Arg.Any<UserAccessCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        var command = new UpdateWorkspaceCommand(_workspaceId, "Secret", "grass");

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        workspace.Name.ShouldBe("Secret");
        workspace.Color.ShouldBe("grass");
        await _dbContextMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
