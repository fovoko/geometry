using gems.common.Geometry.Models;
using gems.CQRS;
using Gems.SqliteDb.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gems.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public class GetFiguresQueryHandler(FigureDbContext context) : IQueryHandler<GetFiguresQuery, Task<IEnumerable<FigureDto>>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<IEnumerable<FigureDto>> Execute(GetFiguresQuery query)
        {
            return Task.FromResult((IEnumerable<FigureDto>)context.Figures);
        }
    }
}
