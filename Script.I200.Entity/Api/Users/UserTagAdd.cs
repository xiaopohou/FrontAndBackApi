
using System.ComponentModel.DataAnnotations;

namespace Script.I200.Entity.Api.Users
{
    public class UserTagAdd
    {
        public int[] UserIds { get; set; }
        /// <summary>
        /// 标签内容
        /// </summary>		
        [Required(ErrorMessage = "会员标签内容不能为空")]
        public string TagName { get; set; }
        /// <summary>
        /// 标签颜色
        /// </summary>
        [Required(ErrorMessage = "会员标签颜色不能为空")]
        public string TagColor { get; set; }
    }
}
