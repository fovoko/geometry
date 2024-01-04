using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using gems.common.Geometry.Figures;
using gems.common.Geometry.Calculators;
using gems.common.Geometry.Models;
using gems.Commands;
using gems.CQRS;
using gems.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace gems.Controllers
{
    /// <summary>
    /// Implements WebApi controllers
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="calculateDispatcher"></param>
    /// <param name="commandDispatcher"></param>
    /// <param name="queryDispatcher"></param>
    [Route("api/figure")]
    [ApiController]
    public class FiguresController(ILogger<FiguresController> logger, ICalculateDispatcher calculateDispatcher, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) : ControllerBase
    {

        /// <summary>
        /// Returns all figures
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FigureDto>>> GetFigures()
        {
            var figures = await queryDispatcher.Execute<GetFiguresQuery, Task<IEnumerable<FigureDto>>>(new GetFiguresQuery());
            return Ok(figures);
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
            GetFigureQuery getFigureQuery = new()
            {
                Id = id
            };

            FigureDto figure = await queryDispatcher.Execute<GetFigureQuery, ValueTask<FigureDto>>(getFigureQuery);

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
            var putFigureCommand = new PutFigureCommand()
            {
                Id = id,
                Geometry = figure
            };

            try
            {
                await commandDispatcher.Execute(putFigureCommand);
            }
            catch (DbUpdateException ex)
            {
                logger.LogWarning(ex, "Failed to update figure with id {id}", id);
                return BadRequest($"Failed to update figure with id {id}");
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
            PostFigureCommand postFigureCommand = new()
            { 
                Geometry = figure
            };

            long newId = await commandDispatcher.Execute(postFigureCommand);

            return Ok(newId);
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
            GetFigureQuery getFigureQuery = new()
            {
                Id = id
            };
            FigureDto figure = await queryDispatcher.Execute<GetFigureQuery, ValueTask<FigureDto>>(getFigureQuery);

            if (figure == null)
            {
                return NotFound();
            }

            DeleteFigureCommand deleteFigureCommand = new()
            {
                Id = id
            };

            await commandDispatcher.Execute(deleteFigureCommand);

            return Ok(figure);
        }
    }
}
