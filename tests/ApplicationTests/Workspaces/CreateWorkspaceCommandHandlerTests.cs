using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Workspaces.Create;
using Domain.Subscriptions;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ApplicationTests.Workspaces;
public class CreateWorkspaceCommandHandlerTests
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;
    private readonly CreateWorkspaceCommandHandler _handler;

    public CreateWorkspaceCommandHandlerTests()
    {
        _context = Substitute.For<IApplicationDbContext>();
        _userContext = Substitute.For<IUserContext>();
        _handler = new CreateWorkspaceCommandHandler(_context, _userContext);
    }

    [Fact]
    public async Task Handle_Should_Create_Workspace_And_Owner()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userContext.UserId.Returns(userId);

        var command = new CreateWorkspaceCommand(
            Name: "  Test Workspace  ",
            Color: "  Blue  "
        );

        DbSet<Workspace> workspaces = new List<Workspace>().BuildMockDbSet();
        DbSet<TeamMember> members = new List<TeamMember>().BuildMockDbSet();

        _context.Workspaces.Returns(workspaces);
        _context.Members.Returns(members);

        // Act
        SharedKernel.Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        // The AddAsync calls are captured here
        await _context.Workspaces.Received(1)
            .AddAsync(Arg.Is<Workspace>(w =>
                w.Name == "Test Workspace" &&
                w.Color == "Blue"
            ), Arg.Any<CancellationToken>());

        await _context.Members.Received(1)
            .AddAsync(Arg.Is<TeamMember>(m =>
                m.UserId == userId &&
                m.Role == Role.Owner &&
                m.Workspace != null
            ), Arg.Any<CancellationToken>());

        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        result.IsSuccess.ShouldBeTrue();
    }
}
