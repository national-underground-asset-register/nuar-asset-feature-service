using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers
{
    public class FallbackTypeMapper : SqlMapper.ITypeMap
    {
        private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;

        public FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers) => _mappers = mappers;

        public ConstructorInfo FindConstructor(string[] names, Type[] types) =>
            _mappers.Select(mapper => mapper.FindConstructor(names, types)).FirstOrDefault(ci => ci != null);

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName) =>
            _mappers.Select(mapper => mapper.GetConstructorParameter(constructor, columnName)).FirstOrDefault(m => m != null);

        public SqlMapper.IMemberMap GetMember(string columnName) =>
            _mappers.Select(mapper => mapper.GetMember(columnName)).FirstOrDefault(m => m != null);

        public ConstructorInfo FindExplicitConstructor() =>
            _mappers.Select(mapper => mapper.FindExplicitConstructor()).FirstOrDefault(ci => ci != null);
    }
}
