using Application.Abstractions.Data;
using Application.Workspaces.Create;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using SharedKernel;
using Shouldly;
using Xunit;

namespace ApplicationTests.Workspaces;

public class CreateWorkspaceCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContextMock;
    private readonly CreateWorkspaceCommandHandler _handler;

    public CreateWorkspaceCommandHandlerTests()
    {
        _dbContextMock = Substitute.For<IApplicationDbContext>();
        _handler = new CreateWorkspaceCommandHandler(_dbContextMock);
    }

    [Fact]
    public async Task Handle_ShouldCreateWorkspace_WhenDataIsValid()
    {
        // Arrange
        DbSet<Workspace> workspaces = new List<Workspace>().AsQueryable().BuildMockDbSet();
        _dbContextMock.Workspaces.Returns(workspaces);
        var command = new CreateWorkspaceCommand("My Universe", "sky");

        // Act
        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        await _dbContextMock.Workspaces.Received(1).AddAsync(Arg.Any<Workspace>(), Arg.Any<CancellationToken>());
        await _dbContextMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
