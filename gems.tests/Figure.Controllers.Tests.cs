using gems.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Gems.SqliteDb.Db;
using gems.common.Geometry;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace gems.tests
{
	public class GeometryControllersTests
	{
		[Fact]
		public async Task CirclePostAndGet()
		{
			const double Radius = 10;
			const double CircleSquare = Radius * Radius * Math.PI;

			Figure circle = new Circle()
			{
				Center = new Point(0, 0),
				Radius = Radius
			};

			var options = new DbContextOptionsBuilder<FigureDbContext>()
				.UseInMemoryDatabase( Guid.NewGuid().ToString() )
				.Options;

			using ( var dbContext = new FigureDbContext(options) )
			{
				var geometryController = new FiguresController( dbContext );

				var responseId = await geometryController.PostFigure( circle ).ConfigureAwait( false );

				OkObjectResult okObjectResultId = Assert.IsType<OkObjectResult>( responseId );
				long figureId = Assert.IsType<long>( okObjectResultId.Value );

				var responseSquare = await geometryController.GetFigure( figureId ).ConfigureAwait( false );

				OkObjectResult okObjectResultSquare = Assert.IsType<OkObjectResult>( responseSquare );
				double calculatedSquare = Assert.IsType<double>( okObjectResultSquare.Value );
				Assert.Equal( CircleSquare, calculatedSquare );
			}
		}

		[Fact]
		public async Task TrianglePostAndGet()
		{
			Figure triangle = new Triangle()
			{
				A = new Point( 0, 0 ),
				B = new Point( 0, 3 ),
				C = new Point( 4, 0 ),
			};

			var options = new DbContextOptionsBuilder<FigureDbContext>()
				.UseInMemoryDatabase( Guid.NewGuid().ToString() )
				.Options;

			using ( var dbContext = new FigureDbContext( options ) )
			{
				var geometryController = new FiguresController( dbContext );

				var responseId = await geometryController.PostFigure( triangle ).ConfigureAwait( false );

				OkObjectResult okObjectResultId = Assert.IsType<OkObjectResult>( responseId );
				long figureId = Assert.IsType<long>( okObjectResultId.Value );

				var responseSquare = await geometryController.GetFigure( figureId ).ConfigureAwait( false );

				OkObjectResult okObjectResultSquare = Assert.IsType<OkObjectResult>( responseSquare );
				double calculatedSquare = Assert.IsType<double>( okObjectResultSquare.Value );
				Assert.Equal( 6, calculatedSquare );
			}
		}
	}
}
