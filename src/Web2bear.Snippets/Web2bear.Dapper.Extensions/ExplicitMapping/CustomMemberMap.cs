using System;
using System.Reflection;
using Dapper;

namespace Web2bear.Dapper.Extensions.ExplicitMapping
{
    public sealed class CustomMemberMap : SqlMapper.IMemberMap
    {
        /// <summary>Creates instance for simple property mapping</summary>
        /// <param name="columnName">DataReader column name</param>
        /// <param name="property">Target property</param>
        public CustomMemberMap(string columnName, PropertyInfo property)
        {
            if (columnName == null)
                throw new ArgumentNullException(nameof(columnName));
            if ((object)property == null)
                throw new ArgumentNullException(nameof(property));
            this.ColumnName = columnName;
            this.Property = property;
        }

        public CustomMemberMap(string columnName, ParameterInfo parameter)
        {
            if (columnName == null)
                throw new ArgumentNullException(nameof(columnName));
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            this.ColumnName = columnName;
            this.Parameter = parameter;
        }

        /// <summary>DataReader column name</summary>
        public string ColumnName { get; }

        /// <summary>Target member type</summary>
        public Type MemberType
        {
            get
            {
                FieldInfo field = this.Field;
                Type type1 = (object)field != null ? field.FieldType : (Type)null;
                if ((object)type1 != null)
                    return type1;
                PropertyInfo property = this.Property;
                Type type2 = (object)property != null ? property.PropertyType : (Type)null;
                if ((object)type2 != null)
                    return type2;
                ParameterInfo parameter = this.Parameter;
                if (parameter == null)
                    return (Type)null;
                return parameter.ParameterType;
            }
        }

        /// <summary>Target property</summary>
        public PropertyInfo Property { get; }

        /// <summary>Target field</summary>
        public FieldInfo Field { get; }

        /// <summary>Target constructor parameter</summary>
        public ParameterInfo Parameter { get; }
    }
}