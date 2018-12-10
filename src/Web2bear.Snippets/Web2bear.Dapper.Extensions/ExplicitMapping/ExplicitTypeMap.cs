using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper;

namespace Web2bear.Dapper.Extensions.ExplicitMapping
{
    internal sealed class ExplicitTypeMap : SqlMapper.ITypeMap
    {
        private readonly IReadOnlyDictionary<string, string> _columnsMap;
        private readonly DefaultTypeMap _defaultMapper;
        private readonly Type _type;


        public ExplicitTypeMap(Type type, IReadOnlyDictionary<string, string> columnsMap)
        {
            _columnsMap = columnsMap;
            _type = type;
            _defaultMapper = new DefaultTypeMap(_type);
        }
        
        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            return _defaultMapper.FindConstructor(names, types);
        }

        public ConstructorInfo FindExplicitConstructor()
        {
            return _defaultMapper.FindExplicitConstructor();
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            var parameters = constructor.GetParameters();
            var parameterName = GetMappedColumnName(columnName);
            var selectedParameter = parameters.FirstOrDefault(p =>
                string.Equals(p.Name, parameterName, StringComparison.OrdinalIgnoreCase));
            return new CustomMemberMap(columnName, selectedParameter);
        }

        /// <summary>
        ///     Returns property based on selector strategy
        /// </summary>
        /// <param name="columnName">DataReader column name</param>
        /// <returns>Poperty member map</returns>
        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            var propertyInfos = _type.GetProperties();
            var propertyName = GetMappedColumnName(columnName);
            var prop = propertyInfos.FirstOrDefault(p =>
                string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));

            return prop != null ? new CustomMemberMap(columnName, prop) : null;
        }

        private string GetMappedColumnName(string columnName)
        {
            _columnsMap.TryGetValue(columnName, out var mappedColumnName);
            return string.IsNullOrWhiteSpace(mappedColumnName) ? columnName : mappedColumnName;
        }
    }
}