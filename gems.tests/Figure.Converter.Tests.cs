using gems.common.DataSerializers;
using gems.common.Geometry;
using System;
using System.Text.Json;
using Xunit;

namespace gems.tests
{
	public class GeometryConverterTests
	{

		private readonly JsonSerializerOptions _jsonSerializerOptions;
		public GeometryConverterTests()
		{
			_jsonSerializerOptions = new JsonSerializerOptions()
			{
				AllowTrailingCommas = false,
				MaxDepth = 64,
				WriteIndented = false,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				PropertyNameCaseInsensitive = true,
			};
		}

		[Theory]
		[InlineData( @"{""circle"": {""center"": {""x"": 2, ""y"": 2},""radius"": 10}}", 10*10*Math.PI )]
		[InlineData( @"{ ""triangle"" : { ""a"": {""x"": 0, ""y"": 0}, ""b"": {""x"": 0, ""y"": 3}, ""c"": {""x"": 4, ""y"": 0} } }", 6 )]
		[InlineData( @"{ ""triangle"" : { ""b"": {""x"": 0, ""y"": 0}, ""c"": {""x"": 0, ""y"": 3}, ""a"": {""x"": 4, ""y"": 0} } }", 6 )]
		[InlineData( @"{ ""triangle"" : { ""c"": {""x"": 0, ""y"": 0}, ""a"": {""x"": 0, ""y"": 3}, ""b"": {""x"": 4, ""y"": 0} } }", 6 )]
		public void FiguresDeserializeAndCalculateSquare( string data, double estimatedSquare )
		{
			var figure = JsonSerializer.Deserialize<Figure>( data, _jsonSerializerOptions );

			var square = figure.CalculateSquare();

			Assert.Equal( estimatedSquare, square );
		}
	}
}
