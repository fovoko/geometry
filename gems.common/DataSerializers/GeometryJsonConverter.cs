using gems.common.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace gems.common.DataSerializers
{
	public class GeometryJsonConverter : JsonConverter<Figure>
	{
        private readonly Dictionary<string, Type> _typeMapping;
        private readonly Dictionary<Type, string> _typeMappingWrite;

        public GeometryJsonConverter() 
        {
            _typeMapping = new Dictionary<string, Type>();
            _typeMappingWrite = new Dictionary<Type, string>();
            var types = Assembly.GetExecutingAssembly().GetTypes().Where( t => typeof( Figure ).IsAssignableFrom( t ) && t != typeof(Figure) );
            foreach ( var t in types )
            {
                var typeName = t.Name.ToLower();
                _typeMapping.Add( typeName, t );
                _typeMappingWrite.Add( t, typeName );
            }
        }

        public override Figure? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
        {
            if ( reader.TokenType != JsonTokenType.StartObject )
            {
                throw new JsonException( "Expected start of object" );
            }

            var originalDepth = reader.CurrentDepth;

            Type? deserializedType = null;
            while ( reader.Read() )
            {
                if ( reader.TokenType == JsonTokenType.PropertyName )
                {
                    var typeName = reader.GetString();

                    if ( !_typeMapping.TryGetValue( typeName, out deserializedType ) )
                    {
                        throw new JsonException( $"Invalid type in json : {typeName}" );
                    }
                    break;
                }
            }

            if ( deserializedType == null )
            {
                throw new JsonException( $"Known type class name not found." );
            }
            reader.Read();
            var res = (Figure?)JsonSerializer.Deserialize( ref reader, deserializedType, options );

            while ( reader.TokenType != JsonTokenType.EndObject || reader.CurrentDepth != originalDepth )
                reader.Read();

            return res;
        }

        public override void Write( Utf8JsonWriter writer, Figure value, JsonSerializerOptions options )
        {
            _typeMappingWrite.TryGetValue( value.GetType(), out string typeName );

            if ( string.IsNullOrEmpty( typeName ) ) {
                return;
            }

            writer.WriteStartObject();

            writer.WritePropertyName( typeName );

            JsonSerializer.Serialize( writer, (dynamic)value );
            //writer.WriteStringValue( JsonSerializer.Serialize<object>( value ) );

            writer.WriteEndObject();
        }
    }
}
