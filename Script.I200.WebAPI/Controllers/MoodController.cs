using System.Web.Http;
using Script.I200.Application.Mood;
using Script.I200.Entity.Api.Mood;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;

namespace Script.I200.WebAPI.Controllers
{
    /// <summary>
    /// 心情相关功能
    /// </summary>
    [RoutePrefix("v0/mood")]
    public class MoodController : BaseApiController
    {
        private readonly IMoodService _moodService;

        /// <summary>
        /// 初始化
        /// </summary>
        public MoodController()
        {
            _moodService = new MoodService();
        }

        /// <summary>
        /// 店铺店员签到
        /// </summary>
        /// <returns></returns>
        [Route("signin/{type}")]
        [HttpPost, HttpOptions]
        public ResponseModel AddSignin(int type)
        {
            var resultData = _moodService.AddSignin(GetUserContext(), type);

            if (resultData == -1)
                return Fail(ErrorCodeEnum.MoodLessThanOneTimes);

            if (resultData == 0)
                return Fail(ErrorCodeEnum.MoodFail);

            return Success(resultData);
        }

        /// <summary>
        /// 提交一条心情
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost,HttpOptions]
        public ResponseModel AddMood(MoodRequestParams moodRequestParams)
        {
            var resultData = _moodService.AddMood(GetUserContext(), moodRequestParams.pic, moodRequestParams.mood);

            if (resultData == -1)
                return Fail(ErrorCodeEnum.MoodLessThanOneTimes);

            if (resultData == 0)
                return Fail(ErrorCodeEnum.MoodFail);

            return Success(resultData);
        }
    }
}