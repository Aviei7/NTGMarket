using Application.DTO.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Application.Services.Pagination
{
    public class PaginationService : IPaginationService
    {
        public async Task<IReadOnlyList<T>> PaginationResult<T>(IQueryable<T> query, ProductQuery pq, CancellationToken ct)
        {
            var page = pq.Page < 1 ? 1 : pq.Page;
            var pageSize = pq.PageSize < 1 ? 10 : pq.PageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;

            var skip = (page - 1) * pageSize;

            return await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }
    }
}
