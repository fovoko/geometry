using gems.CQRS;
using Gems.SqliteDb.Db;
using System.Threading.Tasks;

namespace gems.Commands
{
    /// <summary>
    /// Class implementing figure deleting command
    /// </summary>
    public class DeleteFigureCommandHandler(FigureDbContext context) : ICommandHandler<DeleteFigureCommand>
    {
        /// <summary>
        /// Deletes figure by id from command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<long> Execute(DeleteFigureCommand command)
        {
            var figure = await context.Figures.FindAsync(command.Id).ConfigureAwait(false);
            context.Figures.Remove(figure);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return -1;
        }
    }
}
