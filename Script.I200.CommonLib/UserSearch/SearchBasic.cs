using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using I200.ElasticSearch;

namespace CommonLib.UserSearch
{
    public class SearchBasic
    {
        private static readonly string ElasticsearchHost = ConfigurationManager.AppSettings["ElasticsearchHost"];
        private readonly ElasticSearch _search;

        public SearchBasic()
        {
            if (string.IsNullOrEmpty(ElasticsearchHost))
            {
                throw new Exception("ElasticSearch appSetting is empty!");
            }
            _search = new ElasticSearch(ElasticsearchHost);
        }

        /// <summary>
        /// 会员搜索
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="accountId">店铺id</param>
        /// <param name="masterId">总店id</param>
        /// <returns></returns>
        public List<UserSearchModel> UserBasic(string keyword, long accountId, long masterId)
        {
            var dict = new List<UserSearchModel>();

            var oResult = _search.Search<UserBasic>(50, keyword, accountId, masterId);

            var userBasics = oResult as UserBasic[] ?? oResult.ToArray();
            if (oResult != null && userBasics.Any())
            {
                dict.AddRange(userBasics.Select(item => new UserSearchModel
                {
                    uName = item.user_name, uNumber = item.user_cardno, uPhone = item.user_phone, uid = item.user_id,
                    masterId=item.master_id
                }));
            }

            return dict;
        }

        /// <summary>
        /// 会员更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UserUpdate(UserBasic model)
        {
            return _search.DocumentPut(model.user_id.ToString(), model);
        }

        /// <summary>
        /// 会员更新(批量)
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool UserUpdateBluk(List<UserBasic> models)
        {
            return _search.BlukDocumentPut(models);
        }

        /// <summary>
        /// 会员删除
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UserDelete(long userId)
        {
            return _search.DocumentDelete(userId.ToString());
        }

        /// <summary>
        /// 会员删除(批量)
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public bool UserDeleteBluk(List<long> userIds)
        {
            return _search.BlukDocumentDelete(userIds.Select(x=>x.ToString()));
        }
    }
}
