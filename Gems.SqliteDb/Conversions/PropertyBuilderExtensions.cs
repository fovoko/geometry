using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gems.SqliteDb.Conversions
{
    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>( this PropertyBuilder<T> propertyBuilder ) where T : class
        {
            ValueConverter<T, string> converter = new ValueConverter<T, string>
            (
                v => JsonSerializer.Serialize( v, null ),
                v => JsonSerializer.Deserialize<T>( v, null )
            );

            ValueComparer<T> comparer = new ValueComparer<T>
            (
                ( l, r ) => JsonSerializer.Serialize( l, null ) == JsonSerializer.Serialize( r, null ),
                v => v == null ? 0 : JsonSerializer.Serialize( v, null ).GetHashCode(),
                v => JsonSerializer.Deserialize<T>( JsonSerializer.Serialize( v, null ), null )
            );

            propertyBuilder.HasConversion( converter );
            propertyBuilder.Metadata.SetValueConverter( converter );
            propertyBuilder.Metadata.SetValueComparer( comparer );
            propertyBuilder.HasColumnType( "jsonb" );

            return propertyBuilder;
        }
    }
}
