using gems.common.DataSerializers;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace gems.common.Geometry.Figures
{
    [JsonConverter(typeof(GeometryJsonConverter))]
    [SwaggerSubType(typeof(Triangle))]
    [SwaggerSubType(typeof(Circle))]
    public interface IFigure
    {
    }

}

