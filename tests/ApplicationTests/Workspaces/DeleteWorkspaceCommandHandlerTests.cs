using System.Reflection;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Workspaces.Delete;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using SharedKernel;
using Shouldly;
using Xunit;

namespace ApplicationTests.Workspaces;

public class DeleteWorkspaceCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContextMock;
    private readonly IUserContext _userContextMock;
    private readonly DeleteWorkspaceCommandHandler _handler;
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _workspaceId = Guid.NewGuid();

    public DeleteWorkspaceCommandHandlerTests()
    {
        _dbContextMock = Substitute.For<IApplicationDbContext>();
        _userContextMock = Substitute.For<IUserContext>();
        _userContextMock.UserId.Returns(_userId);
        _handler = new DeleteWorkspaceCommandHandler(_dbContextMock, _userContextMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenWorkspaceDoesNotExist()
    {
        // Arrange
        DbSet<Workspace> workspaceDbSet = new List<Workspace>().AsQueryable().BuildMockDbSet();
        _dbContextMock.Workspaces.Returns(workspaceDbSet);
        var command = new DeleteWorkspaceCommand(_workspaceId);

        // Act
        var handler = new DeleteWorkspaceCommandHandler(_dbContextMock, _userContextMock);
        Result result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(WorkspaceErrors.NotFound(Arg.Any<Guid>()).Code);
    }

    [Fact]
    public async Task Handle_ShouldDeleteWorkspace_WhenExistsAndUserIsOwner()
    {
        // Arrange
        var workspace = new Workspace { Id = _workspaceId, Name = "Milky Way", Color = "milky" };
        FieldInfo? fieldInfo = workspace.GetType().GetField("<CreatedById>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic);
        fieldInfo!.SetValue(workspace, _userId);
        DbSet<Workspace> dbSet = new List<Workspace> { workspace }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Workspaces.Returns(dbSet);
        _dbContextMock.Workspaces.When(x => x.Remove(workspace)).Do(_ => { });
        _dbContextMock.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);
        var command = new DeleteWorkspaceCommand(_workspaceId);

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        await _dbContextMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
