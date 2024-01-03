using gems.common.Geometry.Figures;

namespace gems.common.Geometry.Calculators
{
    public interface ICalculator<out TFigure> where TFigure : IFigure
    {
        double CalculateSquare(IFigure figure);
    }
}
