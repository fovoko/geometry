using Swashbuckle.AspNetCore.Filters;
using System;

namespace gems.common.Geometry
{
	public class Triangle : Figure
	{
		public Point A { get; set; }
		public Point B { get; set; }
		public Point C { get; set; }

		public override double CalculateSquare()
		{
			return Math.Abs( A.X * ( B.Y - C.Y ) + B.X * ( C.Y - A.Y ) + C.X * ( A.Y - B.Y ) ) / 2;
		}
	}

	public class TriangleExample : IExamplesProvider<object>
	{
		public object GetExamples()
		{
			return new
			{
				Triangle = new Triangle
				{
					A = new Point( 1, 1 ),
					B = new Point( 5, 5 ),
					C = new Point( 0, 5 )
				}
			};
		}
	}

}
