using System;
using gems.common.Geometry.Figures;

namespace gems.common.Geometry.Calculators
{
    public class CalculatorCircle : ICalculator<Circle>
    {
        public double CalculateSquare(IFigure figure)
        {
            Circle circle = figure as Circle;
            return Math.PI * circle.Radius * circle.Radius;
        }
    }
}
