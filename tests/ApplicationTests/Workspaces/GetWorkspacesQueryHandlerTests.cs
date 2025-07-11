using System.Reflection;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Workspaces.Get;
using Domain.Subscriptions;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using SharedKernel;
using Shouldly;
using Xunit;

namespace ApplicationTests.Workspaces;

public class GetWorkspacesQueryHandlerTests
{
    private readonly IApplicationDbContext _dbContextMock;
    private readonly IUserContext _userContextMock;
    private readonly GetWorkspacesQueryHandler _handler;
    private readonly Guid _userId = Guid.NewGuid();

    public GetWorkspacesQueryHandlerTests()
    {
        _dbContextMock = Substitute.For<IApplicationDbContext>();
        _userContextMock = Substitute.For<IUserContext>();
        _userContextMock.UserId.Returns(_userId);
        _handler = new GetWorkspacesQueryHandler(_dbContextMock, _userContextMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnOwnedWorkspaces()
    {
        // Arrange
        var ownedWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "Owned", Color = "copper" };
        FieldInfo? fieldInfo = ownedWorkspace.GetType().GetField("<CreatedById>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic);
        fieldInfo!.SetValue(ownedWorkspace, _userId);
        DbSet<Workspace> workspaces = new List<Workspace> { ownedWorkspace }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Workspaces.Returns(workspaces);
        DbSet<TeamMember> teamMemberDbSet = new List<TeamMember>().AsQueryable().BuildMockDbSet();
        _dbContextMock.Members.Returns(teamMemberDbSet);

        // Act
        Result<List<WorkspaceResponse>> result = await _handler.Handle(new GetWorkspacesQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldContain(x => x.Id == ownedWorkspace.Id && x.Name == "Owned");
    }

    [Fact]
    public async Task Handle_ShouldReturnSharedWorkspaces()
    {
        // Arrange
        var sharedWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "Shared", Color = "gold" };
        var teamMember = new TeamMember { UserId = _userId, Workspace = sharedWorkspace };
        DbSet<Workspace> workspaceDbSet = new List<Workspace>().AsQueryable().BuildMockDbSet();
        _dbContextMock.Workspaces.Returns(workspaceDbSet);
        DbSet<TeamMember> teamMemberDbSet = new List<TeamMember> { teamMember }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Members.Returns(teamMemberDbSet);

        // Act
        Result<List<WorkspaceResponse>> result = await _handler.Handle(new GetWorkspacesQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldContain(x => x.Id == sharedWorkspace.Id && x.Name == "Shared");
    }
}
