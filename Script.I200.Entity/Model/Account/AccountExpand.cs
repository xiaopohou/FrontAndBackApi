using System.ComponentModel.DataAnnotations.Schema;

namespace Script.I200.Entity.Model.Account
{
    /// <summary>
    /// 店铺扩展信息
    /// </summary>
    [Table("tb_user_infor")]
    public  class AccountExpand
    {
        /// <summary>
        /// ID
        /// </summary>		
        public int ID { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>
        [Column("UserId")]		
        public int AccountId{ get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>		
        public string UserName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>		
        public string UserSex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>		
        public int UserAge { get; set; }

        /// <summary>
        /// 电话
        /// </summary>		
        public string UserNumber { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>		
        public string UserPhone { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>		
        public string UserEmail { get; set; }

        /// <summary>
        /// 店铺行业
        /// </summary>		
        public string ShopType { get; set; }

        /// <summary>
        /// 开店时间
        /// </summary>		
        public int ShopAge { get; set; }

        /// <summary>
        /// 店铺地址
        /// </summary>		
        public string ShopAddress { get; set; }

        /// <summary>
        /// 店主生日
        /// </summary>		
        public string UserBirthday { get; set; }

        /// <summary>
        /// 血型
        /// </summary>		
        public string BooType { get; set; }

        /// <summary>
        /// 星座
        /// </summary>		
        public string Constellations { get; set; }

        /// <summary>
        /// 生肖
        /// </summary>		
        public string ChineseAnimal { get; set; }

        /// <summary>
        /// 微博
        /// </summary>		
        public string MicroBold { get; set; }

        /// <summary>
        /// 学历
        /// </summary>		
        public string Enducation { get; set; }

        /// <summary>
        /// 个人签名
        /// </summary>		
        public string Personality { get; set; }

        /// <summary>
        /// 头像
        /// </summary>		
        public string UserPic { get; set; }

        /// <summary>
        /// 系统录入QQ号
        /// </summary>		
        public string sys_qqNum { get; set; }

        /// <summary>
        /// 系统备注
        /// </summary>		
        public string sys_Remark { get; set; }

        /// <summary>
        /// 经度
        /// </summary>		
        public string t_longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>		
        public string t_latitude { get; set; }

        /// <summary>
        /// HTML5 店铺banner
        /// </summary>		
        public string t_StoreBanner { get; set; }

        /// <summary>
        /// 店主头像
        /// </summary>		
        public string t_Avatar { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        public string t_Range { get; set; }
    }
}
