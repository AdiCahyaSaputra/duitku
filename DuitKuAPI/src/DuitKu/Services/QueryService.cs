using DuitKu.DTOs;

namespace DuitKu.Services
{
    public class QueryService<TEntity>
    {
        public delegate IQueryable<TEntity> SearchQuery(IQueryable<TEntity> query, string searchString);
        public delegate IQueryable<TEntity> WhenCallback(IQueryable<TEntity> query);

        public IQueryable<TEntity> When(IQueryable<TEntity> query, bool condition, WhenCallback callback)
        {
            return condition ? callback.Invoke(query) : query;
        }

        public IQueryable<TEntity> PaginateWithSearchFilter(
            IQueryable<TEntity> query,
            BaseParamFilterDto filterDto,
            SearchQuery? searchQuery
        )
        {
            int pageNumber = filterDto.pageNumber ?? 1;
            int pageSize = filterDto.limit ?? -1; // No Limit

            query = When(query, filterDto.search != null && searchQuery != null, (query) => searchQuery!.Invoke(query, filterDto.search!));
            query = When(query, filterDto.paginate, (query) => query.Skip((pageNumber - 1) * (pageSize > 0 ? pageSize : 0)));
            query = When(query, pageSize > 0, (query) => query.Take(pageSize));

            return query;
        }
    }
}