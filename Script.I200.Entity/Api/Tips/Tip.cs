namespace Script.I200.Entity.Api.Tips
{
    public class Tip
    {
        /// <summary>
        /// 提示层id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 提示层名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 提示层是否呈现	0=隐藏，1=呈现
        /// </summary>
        public int isShow { get; set; }
    }
}
