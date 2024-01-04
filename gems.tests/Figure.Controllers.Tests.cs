using gems.Controllers;
using System;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Gems.SqliteDb.Db;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gems.common.Geometry.Figures;
using gems.common.Geometry.Calculators;
using gems.CQRS;
using gems.common.Geometry.Models;
using gems.Queries;
using System.Collections.Generic;
using gems.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.HttpResults;

namespace gems.tests
{
    /// <summary>
    /// Integration tests of the contrller FiguresController
    /// </summary>
    public class GeometryControllersTests : IDisposable
    {
        private readonly Mock<IServiceProvider> mockServiceProvider;
        private readonly DbContextOptions<FigureDbContext> options;
        private readonly FigureDbContext dbContext;
        private readonly Mock<ILogger<FiguresController>> mockLogger;
        private readonly FiguresController geometryController;

        public const long circleUpdate = 1;
        public const long triangleUpdate = 1;

        public GeometryControllersTests()
        {
            options = new DbContextOptionsBuilder<FigureDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging(true)
                .EnableDetailedErrors(true)
                .Options;

            dbContext = new FigureDbContext(options);

            mockLogger = new Mock<ILogger<FiguresController>>();

            mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(o => o.GetService(typeof(ICalculator<Circle>))).Returns(new CalculatorCircle());
            mockServiceProvider.Setup(o => o.GetService(typeof(ICalculator<Triangle>))).Returns(new CalculatorTriangle());

            mockServiceProvider.Setup(o => o.GetService(typeof(ICommandHandler<DeleteFigureCommand>))).Returns(new DeleteFigureCommandHandler(dbContext));
            mockServiceProvider.Setup(o => o.GetService(typeof(ICommandHandler<PostFigureCommand>))).Returns(new PostFigureCommandHandler(dbContext));
            mockServiceProvider.Setup(o => o.GetService(typeof(ICommandHandler<PutFigureCommand>))).Returns(new PutFigureCommandHandler(dbContext));

            mockServiceProvider.Setup(o => o.GetService(typeof(IQueryHandler<GetFiguresQuery, Task<IEnumerable<FigureDto>>>))).Returns(new GetFiguresQueryHandler(dbContext));
            mockServiceProvider.Setup(o => o.GetService(typeof(IQueryHandler<GetFigureQuery, ValueTask<FigureDto>>))).Returns(new GetFigureQueryHandler(dbContext));

            geometryController = new FiguresController(
                mockLogger.Object,
                new CalculatorDispatcher(mockServiceProvider.Object),
                new CommandDispatcher(mockServiceProvider.Object),
                new QueryDispatcher(mockServiceProvider.Object));
        }

        [Fact]
        public async Task CirclePostAndGetRunsOk()
        {
            const double Radius = 10;
            const double CircleSquare = Radius * Radius * Math.PI;

            IFigure circle = new Circle()
            {
                Center = new Point(0, 0),
                Radius = Radius
            };

            IActionResult postFigureResponse = await geometryController.PostFigure(circle);

            OkObjectResult okObjectResultId = Assert.IsType<OkObjectResult>(postFigureResponse);
            long figureId = Assert.IsType<long>(okObjectResultId.Value);

            IActionResult getFigureResponse = await geometryController.GetFigure(figureId);

            OkObjectResult okObjectResultSquare = Assert.IsType<OkObjectResult>(getFigureResponse);
            double calculatedSquare = Assert.IsType<double>(okObjectResultSquare.Value);
            Assert.Equal(CircleSquare, calculatedSquare);
        }

        [Fact]
        public async Task CirclePutAndGetRunsOk()
        {
            const long figureUpdateId = 4;
            const double Radius = 10;
            const double CircleSquare = Radius * Radius * Math.PI;

            dbContext.Figures.Add(new FigureDto()
            {
                Id = figureUpdateId,
                Geometry = new Triangle() { A = new Point(0, 0), B = new Point(1, 0), C = new Point(0, 1) }
            });
            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();

            IFigure circleUpdate = new Circle()
            {
                Center = new Point(0, 0),
                Radius = Radius * 3
            };
            IActionResult putFigureResponse = await geometryController.PutFigure(figureUpdateId, circleUpdate);

            Assert.IsType<NoContentResult>(putFigureResponse);

            IActionResult getFigureResponse = await geometryController.GetFigure(figureUpdateId);

            OkObjectResult okObjectResultSquare = Assert.IsType<OkObjectResult>(getFigureResponse);
            double calculatedSquare = Assert.IsType<double>(okObjectResultSquare.Value);
            Assert.Equal(CircleSquare * 9, calculatedSquare);
        }

        [Fact]
        public async Task CirclePutNonExistentReturnsBadRequest()
        {
            const long figureUpdateId = int.MaxValue;
            const double Radius = 10;

            IFigure circleUpdate = new Circle()
            {
                Center = new Point(0, 0),
                Radius = Radius * 3
            };
            IActionResult putFigureResponse = await geometryController.PutFigure(figureUpdateId, circleUpdate);

            Assert.IsType<BadRequestObjectResult>(putFigureResponse);
        }

        [Fact]
        public async Task TrianglePostAndGetRunsOk()
        {
            IFigure triangle = new Triangle()
            {
                A = new Point(0, 0),
                B = new Point(0, 3),
                C = new Point(4, 0),
            };

            IActionResult postFigureResponse = await geometryController.PostFigure(triangle);

            OkObjectResult okObjectResultId = Assert.IsType<OkObjectResult>(postFigureResponse);
            long figureId = Assert.IsType<long>(okObjectResultId.Value);

            IActionResult getFigureResponse = await geometryController.GetFigure(figureId);

            OkObjectResult okObjectResultSquare = Assert.IsType<OkObjectResult>(getFigureResponse);
            double calculatedSquare = Assert.IsType<double>(okObjectResultSquare.Value);
            Assert.Equal(6, calculatedSquare);
        }

        [Fact]
        public async Task TrianglePutAndGetRunsOk()
        {
            const long figureUpdateId = 3;

            dbContext.Add(new FigureDto()
            {
                Id = figureUpdateId,
                Geometry = new Circle() { Center = new Point(1, 1), Radius = 10 }
            });
            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();

            IFigure triangleUpdate = new Triangle()
            {
                A = new Point(0, 0),
                B = new Point(0, 6),
                C = new Point(8, 0),
            };

            IActionResult putFigureResponse = await geometryController.PutFigure(figureUpdateId, triangleUpdate);
            Assert.IsType<NoContentResult>(putFigureResponse);

            IActionResult getFigureResponse = await geometryController.GetFigure(figureUpdateId);

            OkObjectResult okObjectResultSquare = Assert.IsType<OkObjectResult>(getFigureResponse);
            double calculatedSquare = Assert.IsType<double>(okObjectResultSquare.Value);
            Assert.Equal(24, calculatedSquare);
        }

        [Fact]
        public async Task TriangleGetNonExistentReturnsNotFound()
        {
            const long figureGetId = int.MaxValue;

            IActionResult getFigureResponse = await geometryController.GetFigure(figureGetId);

            Assert.IsType<NotFoundResult>(getFigureResponse);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext?.Dispose();
            }
        }
    }
}
