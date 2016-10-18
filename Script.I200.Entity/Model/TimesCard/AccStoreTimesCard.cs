using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Script.I200.Data.MicroOrm.Attributes;

namespace Script.I200.Entity.Model.TimesCard
{
    /// <summary>
    ///     店铺计次卡
    /// </summary>
    [Table("T_Act_TimesCard")]
    public class AccStoreTimesCard
    {
        /// <summary>
        ///     Id
        /// </summary>
        [Identity]
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///     店铺Id
        /// </summary>
        public int AccId { get; set; }

        /// <summary>
        ///     绑定商品的Id
        /// </summary>
        public int BindGoodsId { get; set; }

        /// <summary>
        ///     计次卡名称
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        ///     编辑时间
        /// </summary>
        public DateTime EditTime { get; set; }

        /// <summary>
        ///     添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}