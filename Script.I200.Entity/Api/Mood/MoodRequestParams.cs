namespace Script.I200.Entity.Api.Mood
{
    /// <summary>
    ///     提交心情的参数实体
    /// </summary>
    public class MoodRequestParams
    {
        /// <summary>
        /// 心情图片
        /// </summary>
        public string pic { get; set; }

        /// <summary>
        /// 心情内容
        /// </summary>
        public string mood { get; set; }
    }
}