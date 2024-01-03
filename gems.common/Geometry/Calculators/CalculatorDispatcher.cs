using gems.common.Geometry.Figures;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace gems.common.Geometry.Calculators
{
    public class CalculatorDispatcher(IServiceProvider serviceProvider) : ICalculateDispatcher
    {
        public double CalculateSquare<TFigure>(TFigure figure) where TFigure : IFigure
        {
            var typeCalculator = typeof(ICalculator<>).MakeGenericType(figure.GetType());
            ICalculator<IFigure> calculator = (ICalculator<IFigure>)serviceProvider.GetRequiredService(typeCalculator);
            return calculator.CalculateSquare(figure);
        }
    }
}
