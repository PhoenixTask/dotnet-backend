using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Users.Access;
using Application.Workspaces.Delete;
using Domain.Subscriptions;
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
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;
    private readonly IUserAccess _userAccess;
    private readonly DeleteWorkspaceCommandHandler _handler;

    public DeleteWorkspaceCommandHandlerTests()
    {
        _context = Substitute.For<IApplicationDbContext>();
        _userContext = Substitute.For<IUserContext>();
        _userAccess = Substitute.For<IUserAccess>();

        _handler = new DeleteWorkspaceCommandHandler(_context, _userContext, _userAccess);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_WorkspaceNotFound()
    {
        // Arrange
        var command = new DeleteWorkspaceCommand(Guid.NewGuid());

        DbSet<Workspace> workspaces = new List<Workspace>().BuildMockDbSet();
        _context.Workspaces.Returns(workspaces);

        _userAccess.IsAuthenticatedAsync(command.Id, Role.Owner)
            .Returns(Task.FromResult(true));

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_UserNotOwner()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        var workspace = new Workspace { Id = workspaceId };

        DbSet<Workspace> workspaces = new List<Workspace> { workspace }
            .BuildMockDbSet();

        _context.Workspaces.Returns(workspaces);

        _userAccess.IsAuthenticatedAsync(workspaceId, Role.Owner)
            .Returns(Task.FromResult(false));

        var command = new DeleteWorkspaceCommand(workspaceId);

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task Handle_Should_Delete_Workspace_And_Member_When_Authorized()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var workspace = new Workspace { Id = workspaceId };
        var member = new TeamMember { WorkspaceId = workspaceId, UserId = userId };

        DbSet<Workspace> workspaces = new List<Workspace> { workspace }
            .BuildMockDbSet();

        DbSet<TeamMember> members = new List<TeamMember> { member }
            .BuildMockDbSet();

        _context.Workspaces.Returns(workspaces);
        _context.Members.Returns(members);

        _userContext.UserId.Returns(userId);

        _userAccess.IsAuthenticatedAsync(workspaceId, Role.Owner)
            .Returns(Task.FromResult(true));

        var command = new DeleteWorkspaceCommand(workspaceId);

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        _context.Members.Received(1).Remove(Arg.Is<TeamMember>(m => m == member));
        _context.Workspaces.Received(1).Remove(Arg.Is<Workspace>(w => w == workspace));
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
