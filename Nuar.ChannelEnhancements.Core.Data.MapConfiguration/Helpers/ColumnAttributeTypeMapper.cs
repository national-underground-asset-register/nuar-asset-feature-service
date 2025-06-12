namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;

using Dapper;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]

public class ColumnAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}

public class ColumnAttributeTypeMapper<T> : FallbackTypeMapper
{
    public ColumnAttributeTypeMapper() : base(new SqlMapper.ITypeMap[]
    {
        new CustomPropertyTypeMap(typeof(T),
            (type, columnName) =>
                type.GetProperties().FirstOrDefault(prop =>
                    prop.GetCustomAttributes(false)
                        .OfType<ColumnAttribute>()
                        .Any(attr => attr.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                )!
        ),
        new DefaultTypeMap(typeof(T))
    })
    { }
}