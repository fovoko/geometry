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

namespace gems.tests
{
    public class GeometryControllersTests
    {
        private readonly Mock<IServiceProvider> mockServiceProvider;

        public GeometryControllersTests() 
        {
            mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(o => o.GetService(typeof(ICalculator<Circle>))).Returns(new CalculatorCircle());
            mockServiceProvider.Setup(o => o.GetService(typeof(ICalculator<Triangle>))).Returns(new CalculatorTriangle());
        }

        [Fact]
        public async Task CirclePostAndGet()
        {
            const double Radius = 10;
            const double CircleSquare = Radius * Radius * Math.PI;

            IFigure circle = new Circle()
            {
                Center = new Point(0, 0),
                Radius = Radius
            };

            var options = new DbContextOptionsBuilder<FigureDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new FigureDbContext(options))
            {
                var geometryController = new FiguresController(dbContext, new CalculatorDispatcher(mockServiceProvider.Object));

                var responseId = await geometryController.PostFigure(circle);

                OkObjectResult okObjectResultId = Assert.IsType<OkObjectResult>(responseId);
                long figureId = Assert.IsType<long>(okObjectResultId.Value);

                var responseSquare = await geometryController.GetFigure(figureId);

                OkObjectResult okObjectResultSquare = Assert.IsType<OkObjectResult>(responseSquare);
                double calculatedSquare = Assert.IsType<double>(okObjectResultSquare.Value);
                Assert.Equal(CircleSquare, calculatedSquare);
            }
        }

        [Fact]
        public async Task TrianglePostAndGet()
        {
            IFigure triangle = new Triangle()
            {
                A = new Point(0, 0),
                B = new Point(0, 3),
                C = new Point(4, 0),
            };

            var options = new DbContextOptionsBuilder<FigureDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new FigureDbContext(options))
            {
                var geometryController = new FiguresController(dbContext, new CalculatorDispatcher(mockServiceProvider.Object));

                var responseId = await geometryController.PostFigure(triangle);

                OkObjectResult okObjectResultId = Assert.IsType<OkObjectResult>(responseId);
                long figureId = Assert.IsType<long>(okObjectResultId.Value);

                var responseSquare = await geometryController.GetFigure(figureId);

                OkObjectResult okObjectResultSquare = Assert.IsType<OkObjectResult>(responseSquare);
                double calculatedSquare = Assert.IsType<double>(okObjectResultSquare.Value);
                Assert.Equal(6, calculatedSquare);
            }
        }
    }
}
