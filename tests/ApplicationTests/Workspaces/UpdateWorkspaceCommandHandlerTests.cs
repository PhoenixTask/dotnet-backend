using Application.Abstractions.Data;
using Application.Users.Access;
using Application.Workspaces.Update;
using Domain.Subscriptions;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using SharedKernel;
using Shouldly;
using Xunit;

namespace ApplicationTests.Workspaces;

public class UpdateWorkspaceCommandHandlerTests
{
    private readonly IApplicationDbContext _context;
    private readonly IUserAccess _userAccess;
    private readonly UpdateWorkspaceCommandHandler _handler;

    public UpdateWorkspaceCommandHandlerTests()
    {
        _context = Substitute.For<IApplicationDbContext>();
        _userAccess = Substitute.For<IUserAccess>();
        _handler = new UpdateWorkspaceCommandHandler(_context, _userAccess);
    }

    [Fact]
    public async Task Handle_Should_Update_Workspace_When_Authorized()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        var workspace = new Workspace { Id = workspaceId, Name = "Old", Color = "OldColor" };
        var member = new TeamMember { Workspace = workspace };

        DbSet<TeamMember> members = new List<TeamMember> { member }
            .BuildMockDbSet();

        _context.Members.Returns(members);
        _userAccess.IsAuthenticatedAsync(workspaceId, Role.Owner).Returns(Task.FromResult(true));

        var command = new UpdateWorkspaceCommand(workspaceId, "  New Name  ", "  New Color  ");

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        workspace.Name.ShouldBe("New Name");
        workspace.Color.ShouldBe("New Color");

        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Workspace_Not_Found()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();

        DbSet<TeamMember> members = new List<TeamMember>()
            .BuildMockDbSet();

        _context.Members.Returns(members);
        _userAccess.IsAuthenticatedAsync(workspaceId, Role.Owner).Returns(Task.FromResult(true));

        var command = new UpdateWorkspaceCommand(workspaceId, "Name", "Color");

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Not_Authorized()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        var workspace = new Workspace { Id = workspaceId };
        var member = new TeamMember { Workspace = workspace };

        DbSet<TeamMember> members = new List<TeamMember> { member }
            .BuildMockDbSet();

        _context.Members.Returns(members);
        _userAccess.IsAuthenticatedAsync(workspaceId, Role.Owner).Returns(Task.FromResult(false));

        var command = new UpdateWorkspaceCommand(workspaceId, "Name", "Color");

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
    }
}
