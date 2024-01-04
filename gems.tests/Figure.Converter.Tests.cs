using gems.common.Geometry.Calculators;
using gems.common.Geometry.Figures;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Text.Json;
using Xunit;

namespace gems.tests
{
    public class GeometryConverterTests
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;
        private readonly Mock<IServiceProvider> mockServiceProvider;

        public GeometryConverterTests()
        {
            jsonSerializerOptions = new JsonSerializerOptions()
            {
                AllowTrailingCommas = false,
                MaxDepth = 64,
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };

            mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(o => o.GetService(typeof(ICalculator<Circle>))).Returns(new CalculatorCircle());
            mockServiceProvider.Setup(o => o.GetService(typeof(ICalculator<Triangle>))).Returns(new CalculatorTriangle());
        }

        [Theory]
        [InlineData(@"{""circle"": {""center"": {""x"": 2, ""y"": 2},""radius"": 10}}", 10 * 10 * Math.PI)]
        [InlineData(@"{ ""triangle"" : { ""a"": {""x"": 0, ""y"": 0}, ""b"": {""x"": 0, ""y"": 3}, ""c"": {""x"": 4, ""y"": 0} } }", 6)]
        [InlineData(@"{ ""triangle"" : { ""b"": {""x"": 0, ""y"": 0}, ""c"": {""x"": 0, ""y"": 3}, ""a"": {""x"": 4, ""y"": 0} } }", 6)]
        [InlineData(@"{ ""triangle"" : { ""c"": {""x"": 0, ""y"": 0}, ""a"": {""x"": 0, ""y"": 3}, ""b"": {""x"": 4, ""y"": 0} } }", 6)]
        public void FiguresDeserializeAndCalculateSquare(string data, double estimatedSquare)
        {
            var figure = JsonSerializer.Deserialize<IFigure>(data, jsonSerializerOptions);


            var calculatorDispatcher = new CalculatorDispatcher(mockServiceProvider.Object);
            var square = calculatorDispatcher.CalculateSquare(figure);

            Assert.Equal(estimatedSquare, square);
        }
    }
}
