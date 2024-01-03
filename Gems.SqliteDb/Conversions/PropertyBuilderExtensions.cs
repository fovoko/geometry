using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gems.SqliteDb.Conversions
{
    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class
        {
            ValueConverter<T, string> converter = new (
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<T>(v, (JsonSerializerOptions)null)
            );

            ValueComparer<T> comparer = new (
                (l, r) => JsonSerializer.Serialize(l, (JsonSerializerOptions)null) == JsonSerializer.Serialize(r, (JsonSerializerOptions)null),
                v => v == null ? 0 : JsonSerializer.Serialize(v, (JsonSerializerOptions)null).GetHashCode(),
                v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, (JsonSerializerOptions)null), (JsonSerializerOptions)null)
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            propertyBuilder.HasColumnType("jsonb");

            return propertyBuilder;
        }
    }
}
