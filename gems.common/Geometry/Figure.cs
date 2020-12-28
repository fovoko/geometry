using gems.common.DataSerializers;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace gems.common.Geometry
{
	[JsonConverter( typeof( GeometryJsonConverter ) )]
	[SwaggerSubTypes( typeof( Triangle ), typeof( Circle ) )]
	public abstract class Figure
	{
		public abstract double CalculateSquare();
	}

}

