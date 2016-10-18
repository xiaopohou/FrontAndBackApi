using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.Onlinemall
{
    [Table("T_Receiving_Address")]
    public class T_Receiving_Address
    {
        /// <summary> 
        ///   
        /// </summary> 
        [Key]
        [Identity]
        public int AddressId { get; set; }
        /// <summary>
        /// AccId
        /// </summary>
        public int AccId { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ConsigneeName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string TelNumber { get; set; }
        /// <summary>
        /// 省id
        /// </summary>
        public int ProviceId { get; set; }
        /// <summary>
        /// 省名称
        /// </summary>
        public string ProviceName { get; set; }
        /// <summary>
        /// 市id
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 区 / 县 id
        /// </summary>
        public int CountryId { get; set; }
        /// <summary>
        /// 区 / 县 名
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// 镇 / 街道 id
        /// </summary>
        public int TownId { get; set; }
        /// <summary>
        /// 镇 / 街道 名
        /// </summary>
        public string TownName { get; set; }
        /// <summary>
        /// 地址详细
        /// </summary>
        public string AddressDetail { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 物流公司
        /// </summary>
        public string ExpressCompany { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
