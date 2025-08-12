using Application.Abstractions.Data;
using Application.Users.Access;
using Application.Workspaces.GetById;
using Domain.Subscriptions;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ApplicationTests.Workspaces;

public class GetWorkspaceByIdQueryHandlerTests
{
    private readonly IUserAccess _userAccess;
    private readonly IApplicationDbContext _context;
    private readonly GetWorkspaceByIdQueryHandler _handler;

    public GetWorkspaceByIdQueryHandlerTests()
    {
        _userAccess = Substitute.For<IUserAccess>();
        _context = Substitute.For<IApplicationDbContext>();
        _handler = new GetWorkspaceByIdQueryHandler(_userAccess, _context);
    }

    [Fact]
    public async Task Handle_Should_Return_Workspace_When_Found_And_Authorized()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        var workspace = new Workspace { Id = workspaceId, Name = "Workspace A", Color = "Red" };
        var member = new TeamMember { WorkspaceId = workspaceId, Workspace = workspace };

        DbSet<TeamMember> members = new List<TeamMember> { member }
            .BuildMockDbSet();

        _context.Members.Returns(members);
        _userAccess.IsAuthenticatedAsync(workspaceId, Role.Owner).Returns(Task.FromResult(true));

        var query = new GetWorkspaceByIdQuery(workspaceId);

        // Act
        SharedKernel.Result<Application.Workspaces.Get.WorkspaceResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldBe(workspaceId);
        result.Value.Name.ShouldBe("Workspace A");
        result.Value.Color.ShouldBe("Red");
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

        var query = new GetWorkspaceByIdQuery(workspaceId);

        // Act
        SharedKernel.Result<Application.Workspaces.Get.WorkspaceResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Not_Authorized()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        var workspace = new Workspace { Id = workspaceId, Name = "Workspace A", Color = "Red" };
        var member = new TeamMember { WorkspaceId = workspaceId, Workspace = workspace };

        DbSet<TeamMember> members = new List<TeamMember> { member }
            .BuildMockDbSet();

        _context.Members.Returns(members);
        _userAccess.IsAuthenticatedAsync(workspaceId, Role.Owner).Returns(Task.FromResult(false));

        var query = new GetWorkspaceByIdQuery(workspaceId);

        // Act
        SharedKernel.Result<Application.Workspaces.Get.WorkspaceResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
    }
}
