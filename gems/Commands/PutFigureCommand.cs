using gems.common.Geometry.Models;
using gems.CQRS;

namespace gems.Commands
{
    /// <summary>
    /// Command to update figure
    /// </summary>
    public class PutFigureCommand : FigureDto, ICommand
    {
    }
}
