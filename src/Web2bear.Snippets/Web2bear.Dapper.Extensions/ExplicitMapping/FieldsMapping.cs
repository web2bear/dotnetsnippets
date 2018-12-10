using System;
using System.Collections.Generic;
using Dapper;

namespace Web2bear.Dapper.Extensions.ExplicitMapping
{
    public class FieldsMapping
    {
        public static FieldsMapping For<T>() where T:class 
        {
            return new FieldsMapping(typeof(T), new Dictionary<string, string>());
        }

        private readonly Type _type;
        private readonly Dictionary<string, string> _map;

        private FieldsMapping(Type type, Dictionary<string, string> map)
        {
            _type = type;
            _map = map;
        }

        public FieldsMapping Field(string fieldName, string propertyName)
        {
            _map.Add(fieldName,propertyName);
            return this;
        }

        public void Register()
        {
            var typeMap = new ExplicitTypeMap(_type,_map);
            SqlMapper.SetTypeMap(_type,typeMap);
        }
    }
}