using gems.CQRS;

namespace gems.Commands
{
    /// <summary>
    /// Command to delete figure
    /// </summary>
    public class DeleteFigureCommand : ICommand
    {
        /// <summary>
        /// Id of the figure to delete
        /// </summary>
        public long Id { get; set; }
    }
}
