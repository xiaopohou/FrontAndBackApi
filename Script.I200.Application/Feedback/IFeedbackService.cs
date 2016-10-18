using Script.I200.Entity.Api.Feedback;
using Script.I200.Entity.API;

namespace Script.I200.Application.Feedback
{
    /// <summary>
    ///     用户反馈（B端用户）
    /// </summary>
    public interface IFeedbackService
    {
        /// <summary>
        /// 提交反馈
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="feedbackParams"></param>
        /// <returns></returns>
        ResponseModel CommmitFeedback(UserContext userContext, FeedbackRequestParams feedbackParams);
    }
}