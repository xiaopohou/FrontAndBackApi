﻿namespace Script.I200.Entity.Enum
{
    public enum ResponseErrorcode
    {
        /// <summary>
        /// http200 成功
        /// </summary>
        C200 = 200,

        /// <summary>
        /// http400 请求错误
        /// </summary>
        C400 = 400,

        /// <summary>
        /// http500 服务器错误(内部数据处理失败)
        /// </summary>
        C500 = 500,

        /// <summary>
        /// 未找到对应的资源
        /// </summary>
        C404 = 404
    }
}