using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Common;
public static class OrderableBehaviour
{
    public static int PutInOrder<T>(this IQueryable<T> query, ref T entity, int newOrder) where T : Entity, IOrderable
    {
        entity.Order = newOrder;
        if (!query.Any()) // Set zero for empty list
        {
            return entity.Order = 0;
        }

        IQueryable<int> sortedList = query
            .OrderBy(x => x.Order)
            .Select(x => x.Order);

        int firstOrder = sortedList.FirstOrDefault();
        int lastOrder = sortedList.LastOrDefault();
        if (newOrder < firstOrder)
        {
            entity.Order = firstOrder;
        }
        if (newOrder > lastOrder)
        {
            entity.Order = lastOrder + 1;
        }

        int actualOrder = entity.Order;

        return query
            .Where(x => x.Order >= actualOrder)
            .ExecuteUpdate(x => x.SetProperty(x => x.Order, x => x.Order + 1));

    }
    public static int DropFromOrderedList<T>(this IQueryable<T> query, int entityOrder) where T : Entity, IOrderable
    {
        return query
            .Where(x => x.Order > entityOrder)
            .ExecuteUpdate(x => x.SetProperty(x => x.Order, x => x.Order - 1));
    }
}
