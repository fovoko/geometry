using gems.common.Geometry.Models;
using gems.CQRS;
using Gems.SqliteDb.Db;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace gems.Commands
{
    /// <summary>
    /// Class implementing figure updating
    /// </summary>
    public class PutFigureCommandHandler(FigureDbContext context) : ICommandHandler<PutFigureCommand>
    {
        /// <summary>
        /// Updates figure by id with figure from command
        /// </summary>
        /// <param name="command"></param>
        public async Task<long> Execute(PutFigureCommand command)
        {
            context.Figures.Update(command);

            await context.SaveChangesAsync();

            return command.Id;
        }
    }
}
