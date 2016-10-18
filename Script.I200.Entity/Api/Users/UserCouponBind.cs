using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script.I200.Entity.Api.Users
{
    public class UserCouponBind
    {
        public int[] Userlist { get; set; }
        public int CouponId { get; set; }
        public bool IsSendMsg { get; set; }
        public string SmsContent { get; set; }
    }
}
