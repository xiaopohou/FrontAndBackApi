namespace Script.I200.Data.MicroOrm.Attributes.Joins
{
    public class RightJoinAttribute : JoinAttributeBase
    {
        public RightJoinAttribute()
        {
        }

        public RightJoinAttribute(string tableName, string key, string externalKey)
            : base(tableName, key, externalKey)
        {
        }
    }
}
