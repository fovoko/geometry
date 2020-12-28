using System.Text.Json.Serialization;

namespace gems.common.Geometry
{
	
	public struct Point
	{
		public double X { get; set; }
		public double Y { get; set; }

		public Point( double X, double Y )
		{
			this.X = X;
			this.Y = Y;
		}
	}
}
