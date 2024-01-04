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
    public class GetFigureQueryHandler(FigureDbContext context) : IQueryHandler<GetFigureQuery, ValueTask<FigureDto>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ValueTask<FigureDto> Execute(GetFigureQuery query)
        {
            return context.Figures.FindAsync(query.Id);
        }
    }
}
