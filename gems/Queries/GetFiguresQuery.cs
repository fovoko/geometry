using gems.common.Geometry.Models;
using gems.CQRS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gems.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public class GetFiguresQuery : IQuery<Task<IEnumerable<FigureDto>>>
    {
    }
}
