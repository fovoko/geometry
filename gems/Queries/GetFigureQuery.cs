using gems.common.Geometry.Models;
using gems.CQRS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gems.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public class GetFigureQuery : IQuery<ValueTask<FigureDto>>
    {
        /// <summary>
        ///  
        /// </summary>
        public long Id { get; set; }
    }
}
