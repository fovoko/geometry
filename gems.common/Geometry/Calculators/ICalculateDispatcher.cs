using gems.common.Geometry.Figures;

namespace gems.common.Geometry.Calculators
{
    public interface ICalculateDispatcher
    {
        double CalculateSquare<TFigure>(TFigure figure) where TFigure : IFigure;
    }
}
