using Swashbuckle.AspNetCore.Filters;
using System;

namespace gems.common.Geometry
{

    public class Circle : Figure
	{
        public Point Center { get; set; }

        public double Radius { get; set; }

		public override double CalculateSquare()
		{
			return Math.PI * Radius * Radius;
		}
	}

	public class CircleExample : IExamplesProvider<Object>
	{
		public Object GetExamples()
		{
			return new
			{
				Circle = new Circle
				{
					Center = new Point( 2, 3 ),
					Radius = 10
				}
			};
		}
	}

}
