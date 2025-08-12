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
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;
    private readonly GetWorkspacesQueryHandler _handler;

    public GetWorkspacesQueryHandlerTests()
    {
        _context = Substitute.For<IApplicationDbContext>();
        _userContext = Substitute.For<IUserContext>();
        _handler = new GetWorkspacesQueryHandler(_context, _userContext);
    }

    [Fact]
    public async Task Handle_Should_Return_User_Workspaces()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userContext.UserId.Returns(userId);

        var workspace1 = new Workspace
        {
            Id = Guid.NewGuid(),
            Name = "Workspace A",
            Color = "Red"
        };

        var workspace2 = new Workspace
        {
            Id = Guid.NewGuid(),
            Name = "Workspace B",
            Color = "Blue"
        };

        DbSet<TeamMember> members = new List<TeamMember>
        {
            new() { UserId = userId, Workspace = workspace1 },
            new() { UserId = userId, Workspace = workspace2 }
        }
        .BuildMockDbSet();

        _context.Members.Returns(members);

        var query = new GetWorkspacesQuery();

        // Act
        Result<List<WorkspaceResponse>> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBe(2);

        result.Value.ShouldContain(w => w.Id == workspace1.Id && w.Name == "Workspace A" && w.Color == "Red");
        result.Value.ShouldContain(w => w.Id == workspace2.Id && w.Name == "Workspace B" && w.Color == "Blue");
    }
}
