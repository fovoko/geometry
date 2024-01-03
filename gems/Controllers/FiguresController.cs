using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gems.SqliteDb.Db;
using Swashbuckle.AspNetCore.Filters;
using gems.common.Geometry.Figures;
using gems.common.Geometry.Calculators;
using gems.common.Geometry.Models;

namespace gems.Controllers
{
    /// <summary>
    /// Implements WebApi controllers
    /// </summary>
    /// <param name="context"></param>
    /// <param name="calculateDispatcher"></param>
    [Route("api/figure")]
    [ApiController]
    public class FiguresController(FigureDbContext context, ICalculateDispatcher calculateDispatcher) : ControllerBase
    {

        /// <summary>
        /// Returns all figures
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FigureDto>>> GetFigures()
        {
            return await context.Figures.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Calculates square for figure with requested Id.
        /// </summary>
        ///<remarks>
        /// 
        ///     GET /api/figure/2
        ///
        /// </remarks>
        /// <response code="200">Identificator of new figure</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFigure(long id)
        {
            FigureDto figure = await context.Figures.FindAsync(id).ConfigureAwait(false);

            if (figure == null)
            {
                return NotFound();
            }

            return Ok(calculateDispatcher.CalculateSquare(figure.Geometry));
        }

        /// <summary>
        /// Accepts request body content as figure ("circle" and "triangle") and updates data in persistent storage.
        /// </summary>
        ///<remarks>
        /// 
        /// Circle:
        ///
        ///     POST /api/figure/1
        ///     {
        ///         "circle": 
        ///         {
        ///             "center": {"x": 2, "y": 2},
        ///             "radius": 10
        ///         }
        ///     }
        ///
        /// Triangle:
        ///
        ///     POST /api/figure/2
        ///     {
        ///         "triangle": 
        ///         {
        ///             "a": {"x": 0, "y": 0},
        ///             "b": {"x": 0, "y": 3},
        ///             "c": {"x": 4, "y": 0}
        ///         }
        ///     }
        ///
        /// </remarks>
        /// <response code="204"></response>
        [HttpPut("{id}")]
        [SwaggerRequestExample(typeof(IFigure), typeof(TriangleExample))]
        public async Task<IActionResult> PutFigure(long id, [FromBody] IFigure figure)
        {
            var figureDto = new FigureDto()
            {
                Id = id,
                Geometry = figure
            };

            context.Add(figureDto);

            context.Entry(figureDto).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FigureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Accepts request body content as figure ("circle" and "triangle") and saves data in persistent storage.
        /// </summary>
        ///<remarks>
        /// 
        /// Circle:
        ///
        ///     POST /api/figure
        ///     {
        ///         "circle": 
        ///         {
        ///             "center": {"x": 2, "y": 2},
        ///             "radius": 10
        ///         }
        ///     }
        ///
        /// Triangle:
        ///
        ///     POST /api/figure
        ///     {
        ///         "triangle": 
        ///         {
        ///             "a": {"x": 0, "y": 0},
        ///             "b": {"x": 0, "y": 3},
        ///             "c": {"x": 4, "y": 0}
        ///         }
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Identificator of new figure</response>
        [HttpPost]
        [SwaggerRequestExample(typeof(IFigure), typeof(CircleExample))]
        public async Task<IActionResult> PostFigure([FromBody] IFigure figure)
        {
            var figureParsed = figure as IFigure;

            var figureDto = new FigureDto()
            {
                Geometry = figureParsed
            };

            await context.Figures.AddAsync(figureDto).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return Ok(figureDto.Id);
        }

        /// <summary>
        /// Deletes figure with requested Id from persistent storage.
        /// </summary>
        ///<remarks>
        /// 
        ///     DELETE /api/figure/2
        ///
        /// </remarks>
        /// <response code="200">Identificator of new figure</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFigure(long id)
        {
            var figure = await context.Figures.FindAsync(id).ConfigureAwait(false);
            if (figure == null)
            {
                return NotFound();
            }

            context.Figures.Remove(figure);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return Ok(figure);
        }


        private bool FigureExists(long id)
        {
            return context.Figures.Any(x => x.Id == id);
        }

    }
}
