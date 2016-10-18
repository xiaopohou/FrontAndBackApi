using Script.I200.Entity.API;

namespace Script.I200.Application.Mood
{
    public interface IMoodService
    {
        /// <summary>
        /// 店铺店员签到
        /// </summary>
        /// <param name="userContext"></param>
        int AddSignin(UserContext userContext, int signType);

        /// <summary>
        /// 提交一条心情
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="pic"></param>
        /// <param name="mood"></param>
        /// <returns></returns>
        int AddMood(UserContext userContext, string pic, string mood);
    }
}
