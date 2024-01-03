using Swashbuckle.AspNetCore.Filters;

namespace gems.common.Geometry.Figures
{
    public class Circle : IFigure
    {
        public Point Center { get; set; }

        public double Radius { get; set; }
    }

    public class CircleExample : IExamplesProvider<object>
    {
        public object GetExamples()
        {
            return new
            {
                Circle = new Circle
                {
                    Center = new Point(2, 3),
                    Radius = 10
                }
            };
        }
    }
}
