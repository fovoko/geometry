using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gems.SqliteDb.Db;
using Gems.SqliteDb.Models;
using gems.common.Geometry;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace gems.Controllers
{
	[Route( "api/figure" )]
	[ApiController]
	public class FiguresController : ControllerBase
	{
		private readonly FigureDbContext _context;

		public FiguresController( FigureDbContext context )
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<FigureDto>>> GetFigures()
		{
			return await _context.Figures.ToListAsync().ConfigureAwait( false );
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
		[HttpGet( "{id}" )]
		public async Task<IActionResult> GetFigure( long id )
		{
			var figure = await _context.Figures.FindAsync( id ).ConfigureAwait( false );

			if ( figure == null )
			{
				return NotFound();
			}

			return Ok( figure.Geometry.CalculateSquare() );
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
		[HttpPut( "{id}" )]
		[SwaggerRequestExample( typeof( Figure ), typeof( TriangleExample ) )]
		public async Task<IActionResult> PutFigure( long id, [FromBody] Figure figure )
		{
			var figureDto = new FigureDto()
			{
				Id = id,
				Geometry = figure
			};

			_context.Add( figureDto );

			_context.Entry( figureDto ).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync().ConfigureAwait( false );
			}
			catch ( DbUpdateConcurrencyException )
			{
				if ( !FigureExists( id ) )
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
		[SwaggerRequestExample( typeof( Figure ), typeof( CircleExample ) )]
		public async Task<IActionResult> PostFigure( [FromBody] Figure figure )
		{
			var figureParsed = figure as Figure;

			var figureDto = new FigureDto()
			{
				Geometry = figureParsed
			};

			await _context.Figures.AddAsync( figureDto ).ConfigureAwait( false );
			await _context.SaveChangesAsync().ConfigureAwait( false );

			return Ok( figureDto.Id );
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
		[HttpDelete( "{id}" )]
		public async Task<IActionResult> DeleteFigure( long id )
		{
			var figure = await _context.Figures.FindAsync( id ).ConfigureAwait( false );
			if ( figure == null )
			{
				return NotFound();
			}

			_context.Figures.Remove( figure );
			await _context.SaveChangesAsync().ConfigureAwait( false );

			return Ok( figure );
		}


		private bool FigureExists( long id )
		{
			return _context.Figures.Any( x => x.Id == id );
		}

	}
}
