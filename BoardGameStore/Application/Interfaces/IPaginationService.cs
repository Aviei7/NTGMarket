using Application.DTO.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPaginationService
    {
        public Task<IReadOnlyList<T>> PaginationResult<T>(IQueryable<T> query, ProductQuery pq, CancellationToken ct);
    }
}
