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
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _workspaceId = Guid.NewGuid();

    public DeleteWorkspaceCommandHandlerTests()
    {
        _dbContextMock = Substitute.For<IApplicationDbContext>();
        _userContextMock = Substitute.For<IUserContext>();
        _userContextMock.UserId.Returns(_userId);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenWorkspaceDoesNotExist()
    {
        // Arrange
        DbSet<Workspace> workspaceDbSet = new List<Workspace>().BuildMockDbSet();
        _dbContextMock.Workspaces.Returns(workspaceDbSet);
        var command = new DeleteWorkspaceCommand(_workspaceId);

        // Act
        var handler = new DeleteWorkspaceCommandHandler(_dbContextMock, _userContextMock);
        Result result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(WorkspaceErrors.NotFound(Arg.Any<Guid>()).Code);
    }
}
