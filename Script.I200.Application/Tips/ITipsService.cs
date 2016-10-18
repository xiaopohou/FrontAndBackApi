using Script.I200.Entity.API;

namespace Script.I200.Application.Tips
{
    /// <summary>
    ///     提示相关接口
    /// </summary>
    public interface ITipsService
    {
        /// <summary>
        ///     判断是否显示消息提示层
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="tipsType"></param>
        /// <returns></returns>
        ResponseModel IsHideElement(UserContext userContext, int tipsType);

        /// <summary>
        ///     更新消息提示层
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="tipsType"></param>
        /// <returns></returns>
        ResponseModel AddOrUpdateUserBehavior(UserContext userContext, int tipsType);
    }
}