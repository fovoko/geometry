using gems.common.Geometry.Figures;
using gems.common.Geometry.Models;
using gems.CQRS;
using Gems.SqliteDb.Db;
using System.Threading.Tasks;

namespace gems.Commands
{
    /// <summary>
    /// Class implementing figure creation
    /// </summary>
    public class PostFigureCommandHandler(FigureDbContext context) : ICommandHandler<PostFigureCommand>
    {
        /// <summary>
        /// Creates new figure in repository from command figure
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<long> Execute(PostFigureCommand command)
        {
            var figureDto = new FigureDto()
            {
                Geometry = command.Geometry
            };

            await context.Figures.AddAsync(figureDto).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return figureDto.Id;
        }
    }
}
