using System.Web.Http;
using Script.I200.Application.Feedback;
using Script.I200.Entity.Api.Feedback;
using Script.I200.Entity.API;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    ///     用户反馈（B端用户）
    /// </summary>
    [RoutePrefix("v0")]
    public class FeedbackController : BaseApiController
    {
        private readonly IFeedbackService _feedbackService;

        /// <summary>
        ///     初始化
        /// </summary>
        public FeedbackController()
        {
            _feedbackService = new FeedbackService();
        }

        /// <summary>
        ///     提交反馈
        /// </summary>
        /// <param name="feedbackParams"></param>
        /// <returns></returns>
        [Route("feedbacks")]
        [HttpPost, HttpOptions]
        public ResponseModel CommmitFeedback(FeedbackRequestParams feedbackParams)
        {
            //校验参数是否为空
            ResponseModel checkResult;
            var result = CheckRequestParamIsNull(feedbackParams, out checkResult);
            return !result ? checkResult : FunctionReturn(_feedbackService.CommmitFeedback(GetUserContext(), feedbackParams));
        }
    }
}