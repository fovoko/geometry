using System;
using gems.common.Geometry.Figures;

namespace gems.common.Geometry.Calculators
{
    public class CalculatorTriangle : ICalculator<Triangle>
    {
        public double CalculateSquare(IFigure figure)
        {
            Triangle triangle = figure as Triangle;
            return Math.Abs(triangle.A.X * (triangle.B.Y - triangle.C.Y)
                + triangle.B.X * (triangle.C.Y - triangle.A.Y)
                + triangle.C.X * (triangle.A.Y - triangle.B.Y)) / 2;
        }
    }
}
