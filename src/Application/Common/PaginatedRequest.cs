using Microsoft.EntityFrameworkCore;

namespace Application.Common;
public abstract record PaginatedRequest(int Page, int PageSize);
public record PaginatedResponse<T>(List<T> Data, int Page, int Total) where T : class;

internal static class PaginationHelper
{
    public static async Task<PaginatedResponse<T>> ToPagedAsync<T>(this IQueryable<T> query, PaginatedRequest paginatedRequest, CancellationToken cancellationToken) where T : class
    {
        int total = await query.CountAsync(cancellationToken);

        List<T> result = await query
            .AsNoTracking()
            .Skip((paginatedRequest.Page - 1) * paginatedRequest.PageSize)
            .Take(paginatedRequest.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResponse<T>(result, paginatedRequest.Page, total);
    }
}
