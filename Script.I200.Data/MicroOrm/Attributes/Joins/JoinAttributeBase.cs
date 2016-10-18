using System;

namespace Script.I200.Data.MicroOrm.Attributes.Joins
{
    public abstract class JoinAttributeBase : Attribute
    {
        protected JoinAttributeBase()
        {
        }

        protected JoinAttributeBase(string tableName, string key, string externalKey)
        {
            TableName = tableName;
            Key = key;
            ExternalKey = externalKey;
        }

        public string TableName { get; set; }

        public string Key { get; set; }

        public string ExternalKey { get; set; }
    }
}