using Swashbuckle.AspNetCore.Filters;

namespace gems.common.Geometry.Figures
{
    public class Triangle : IFigure
    {
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }
    }

    public class TriangleExample : IExamplesProvider<object>
    {
        public object GetExamples()
        {
            return new
            {
                Triangle = new Triangle
                {
                    A = new Point(1, 1),
                    B = new Point(5, 5),
                    C = new Point(0, 5)
                }
            };
        }
    }

}
