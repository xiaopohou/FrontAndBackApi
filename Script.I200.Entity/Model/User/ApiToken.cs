using System.ComponentModel.DataAnnotations.Schema;

namespace Script.I200.Entity.Model.User
{
    [Table("T_Token_Api")]
    public class ApiToken:BaseToken
    {
        public string AppKey { get; set; }

        public string DeviceId { get; set; }
    }
}
