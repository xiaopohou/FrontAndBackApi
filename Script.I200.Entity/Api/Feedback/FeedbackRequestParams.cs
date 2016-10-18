namespace Script.I200.Entity.Api.Feedback
{
    /// <summary>
    ///     提交反馈的参数实体
    /// </summary>
    public class FeedbackRequestParams
    {
        /// <summary>
        ///     反馈建议内容
        /// </summary>
        public string Suggestion { get; set; }

        /// <summary>
        ///     反馈人的电话号码
        /// </summary>
        public string FeedbackTelPhone { get; set; }

        /// <summary>
        ///     反馈类型
        /// </summary>
        public int FeedbackType { get; set; }
    }
}