using System.Collections.Generic;

namespace Script.I200.Core.Config
{
    public interface IRemoteSetting
    {
        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetConfig(string key);

        void UpdateLocalConfig(string key);

        void UpdateLocalConfig(IEnumerable<string> keys);

    }
}
