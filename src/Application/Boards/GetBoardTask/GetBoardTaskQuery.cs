using Application.Abstractions.Messaging;
using Application.Common;

namespace Application.Boards.GetBoardTask;
public sealed record GetBoardTaskQuery(Guid ProjectId,int Page, int PageSize) : PaginatedRequest(Page,PageSize) , IQuery<PaginatedResponse<BoardResponse>> ;
