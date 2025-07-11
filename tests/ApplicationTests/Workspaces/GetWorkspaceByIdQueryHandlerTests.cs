using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Users.AccessAction;
using Application.Workspaces.Get;
using Application.Workspaces.GetById;
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

public class GetWorkspaceByIdQueryHandlerTests
{
    private readonly IUserContext _userContextMock;
    private readonly ISender _senderMock;
    private readonly IApplicationDbContext _dbContextMock;
    private readonly GetWorkspaceByIdQueryHandler _handler;
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _workspaceId = Guid.NewGuid();

    public GetWorkspaceByIdQueryHandlerTests()
    {
        _userContextMock = Substitute.For<IUserContext>();
        _senderMock = Substitute.For<ISender>();
        _dbContextMock = Substitute.For<IApplicationDbContext>();
        _userContextMock.UserId.Returns(_userId);
        _handler = new GetWorkspaceByIdQueryHandler(_userContextMock, _senderMock, _dbContextMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenWorkspaceDoesNotExist()
    {
        // Arrange
        DbSet<Workspace> workspaceDbSet = new List<Workspace>().AsQueryable().BuildMockDbSet();
        _dbContextMock.Workspaces.Returns(workspaceDbSet);
        var query = new GetWorkspaceByIdQuery(_workspaceId);

        // Act
        Result result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(WorkspaceErrors.NotFound(Arg.Any<Guid>()).Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserHasNoAccess()
    {
        // Arrange
        var workspace = new Workspace { Id = _workspaceId, Name = "Important Workspace", Color = "tomato" };
        DbSet<Workspace> workspaceDbSet = new List<Workspace> { workspace }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Workspaces.Returns(workspaceDbSet);
        _senderMock.Send(Arg.Any<UserAccessCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure(UserErrors.InvalidPermission));
        var query = new GetWorkspaceByIdQuery(_workspaceId);

        // Act
        Result<WorkspaceResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe(UserErrors.InvalidPermission.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnWorkspace_WhenUserHasAccess()
    {
        // Arrange
        var workspace = new Workspace { Id = _workspaceId, Name = "Important Workspace", Color = "tomato" };
        DbSet<Workspace> workspaceDbSet = new List<Workspace> { workspace }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Workspaces.Returns(workspaceDbSet);
        _senderMock.Send(Arg.Any<UserAccessCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        var query = new GetWorkspaceByIdQuery(_workspaceId);

        // Act
        Result<WorkspaceResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldBe(_workspaceId);
        result.Value.Name.ShouldBe("Important Workspace");
        result.Value.Color.ShouldBe("tomato");
    }
}
