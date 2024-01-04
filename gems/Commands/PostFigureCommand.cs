using gems.common.Geometry.Figures;
using gems.CQRS;

namespace gems.Commands
{
    /// <summary>
    /// Command to create figure
    /// </summary>
    public class PostFigureCommand : ICommand
    {
        /// <summary>
        /// Holds geometry object
        /// </summary>
        public IFigure Geometry { get; set; }
    }
}
