using gems.common.Geometry.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace gems.common.DataSerializers
{
    public class GeometryJsonConverter : JsonConverter<IFigure>
    {
        private readonly Dictionary<string, Type> typeMapping;
        private readonly Dictionary<Type, string> typeMappingWrite;

        public GeometryJsonConverter()
        {
            typeMapping = [];
            typeMappingWrite = [];
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IFigure).IsAssignableFrom(t) && t != typeof(IFigure));
            foreach (var t in types)
            {
                var typeName = t.Name.ToLower();
                typeMapping.Add(typeName, t);
                typeMappingWrite.Add(t, typeName);
            }
        }

        public override IFigure Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of object");
            }

            var originalDepth = reader.CurrentDepth;

            Type deserializedType = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var typeName = reader.GetString();

                    if (!typeMapping.TryGetValue(typeName, out deserializedType))
                    {
                        throw new JsonException($"Invalid type in json : {typeName}");
                    }
                    break;
                }
            }

            if (deserializedType == null)
            {
                throw new JsonException($"Known type class name not found.");
            }
            reader.Read();
            var res = (IFigure)JsonSerializer.Deserialize(ref reader, deserializedType, options);

            while (reader.TokenType != JsonTokenType.EndObject || reader.CurrentDepth != originalDepth)
                reader.Read();

            return res;
        }

        public override void Write(Utf8JsonWriter writer, IFigure value, JsonSerializerOptions options)
        {
            typeMappingWrite.TryGetValue(value.GetType(), out string typeName);

            if (string.IsNullOrEmpty(typeName))
            {
                return;
            }

            writer.WriteStartObject();

            writer.WritePropertyName(typeName);

            JsonSerializer.Serialize(writer, (dynamic)value);

            writer.WriteEndObject();
        }
    }
}
