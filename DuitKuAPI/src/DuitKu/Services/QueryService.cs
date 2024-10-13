using DuitKu.DTOs;

namespace DuitKu.Services
{
    public class QueryService<TEntity>
    {
        public delegate IQueryable<TEntity> SearchQuery(IQueryable<TEntity> query, string searchString);

        public IQueryable<TEntity> PaginateWithSearchFilter(
            IQueryable<TEntity> query,
            BaseParamFilterDto filterDto,
            SearchQuery? searchQuery
        )
        {
            int pageNumber = filterDto.pageNumber ?? 1;
            int pageSize = filterDto.limit ?? -1; // No Limit

            if (filterDto.paginate)
            {
                query = query.Skip((pageNumber - 1) * pageSize);

                if (pageSize > 0)
                {
                    query = query.Take(pageSize);
                }
            }

            if (filterDto.search != null && !filterDto.paginate && searchQuery != null)
            {
                string searchString = filterDto.search!;

                query = searchQuery.Invoke(query, searchString);

                if (pageSize > 0)
                {
                    query = query.Take(pageSize);
                }
            }

            return query;
        }
    }
}