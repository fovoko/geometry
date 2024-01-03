using gems.common.Geometry.Calculators;
using gems.common.Geometry.Figures;
using System;
using Xunit;

namespace gems.tests
{
    public class GeometryTests
	{

		[Fact]
		public void CircleCalculateSquare()
		{
			const double R = 100;

			var circle = new Circle() { Center = new Point(0, 0), Radius = R };

			var square = new CalculatorCircle().CalculateSquare(circle); 

			Assert.Equal( R * R * Math.PI, square );
		}

        [Fact]
        public void CircleCalculateTriangle()
        {
            var circle = new Triangle() { 
				A = new Point(0, 0), 
				B = new Point(0, 3),
                C = new Point(4, 0)
            };

            var square = new CalculatorTriangle().CalculateSquare(circle);

            Assert.Equal(3 * 4 / 2, square);
        }
    }
}
