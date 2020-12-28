using gems.common.DataSerializers;
using gems.common.Geometry;
using System;
using System.Text.Json;
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

			var square = circle.CalculateSquare();

			Assert.Equal( R * R * Math.PI, square );
		}
	}
}
