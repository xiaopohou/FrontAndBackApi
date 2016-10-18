using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace CommonLib.UserSearch
{
    public class SearchService
    {
        private static string _indexPath;
        private const string _userIndexFoldName = "~\\$UserIndexData$";


        public SearchService()
        { 
        }

        public SearchService(string indexPath)
        {
            _indexPath = indexPath;
        }

        public static string InitIndexPath(HttpContext context)
        {
            if (!string.IsNullOrEmpty(_indexPath))
                return _indexPath;

            _indexPath = context.Server.MapPath(_userIndexFoldName);
            return _indexPath;
        }

        /// <summary>
        /// 移除某个用户的索引
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveIndex(string userId, string maxshop)
        {
            var isNew = false;
            var path = _indexPath + "\\" + maxshop;

            // 从磁盘directory 中移除
            var directory = FSDirectory.Open(path);
            isNew = !IndexReader.IndexExists(directory);
            RemoveIndexFromDirectory(directory, userId, isNew);

            // 从内存directory中移除
            var ramDirectory = RamDirectoryFactory.GetRamDirectory(path, directory);
            isNew = !IndexReader.IndexExists(ramDirectory);
            RemoveIndexFromDirectory(ramDirectory, userId, isNew);
        }

        /// <summary>
        /// 移除某个用户的索引
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveIndex(List<int> userIds, string maxshop)
        {
            var isNew = false;
            var path = _indexPath + "\\" + maxshop;

            // 从磁盘directory 中移除
            var directory = FSDirectory.Open(path);
            isNew = !IndexReader.IndexExists(directory);
            var maxFieldLength = new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH);
            
            using (var writer = new IndexWriter(directory, new StandardAnalyzer(Version.LUCENE_30), isNew, maxFieldLength))
            {
                foreach (var userId in userIds)
                {
                    writer.DeleteDocuments(new Term("UserId", userId.ToString()));
                }
                writer.Optimize();
            }

            // 从内存directory中移除
            var ramDirectory = RamDirectoryFactory.GetRamDirectory(path, directory);
            isNew = !IndexReader.IndexExists(ramDirectory);
            using (var writer = new IndexWriter(ramDirectory, new StandardAnalyzer(Version.LUCENE_30), isNew, maxFieldLength))
            {
                foreach (var userId in userIds)
                {
                    writer.DeleteDocuments(new Term("UserId", userId.ToString()));
                }
                writer.Optimize();
            }
        }

        private void RemoveIndexFromDirectory(Directory directory, string userId, bool isNew)
        {
            var maxFieldLength = new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH);
            using (var writer = new IndexWriter(directory, new StandardAnalyzer(Version.LUCENE_30), isNew, maxFieldLength))
            {
                writer.DeleteDocuments(new Term("UserId", userId));
                writer.Optimize();
            }
        }

        private void AddUserIndexToDirectory(UserInfo user, Directory directory, bool isNew)
        {
            var maxFieldLength = new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH);
            using (var writer = new IndexWriter(directory, new StandardAnalyzer(Version.LUCENE_30), isNew, maxFieldLength))
            {
                var document = new Document();

                document.Add(new Field("UserId", user.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("ShopId", user.ShopId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("Name", user.Name.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("UPinYinShort", user.UPinYinShort.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("UPinYinFull", user.UPinYinFull.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                if (!string.IsNullOrWhiteSpace(user.Phone))
                {
                    document.Add(new Field("Phone", user.Phone.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
                if (!string.IsNullOrWhiteSpace(user.Number))
                {
                    document.Add(new Field("Number", user.Number.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
                
                writer.AddDocument(document);
                writer.Optimize();
            }
        }

        private Document GetUserFromIndexDirectoryByUid(string accId, string maxShopId)
        {
            // 搜索用户是否存在
            var indexPath = _indexPath + "\\" + maxShopId;
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            var ramDirectory = RamDirectoryFactory.GetRamDirectory(indexPath, directory);
            IndexReader reader = IndexReader.Open(ramDirectory, true);
            IndexSearcher searcher = new IndexSearcher(reader);

            var idTerm = new Term("UserId", accId);
            var idQuery = new TermQuery(idTerm);

            var result = searcher.Search(idQuery, 1);
            var userInfo = new UserInfo() { ShopId = accId, MaxShopId = maxShopId };

            if (result.TotalHits == 0)
            {
                return null;
            }

            var document = searcher.Doc(result.ScoreDocs[0].Doc);
            return document;
        }


        /// <summary>
        /// 新增某个用户的索引 
        /// </summary>
        /// <param name="user"></param>
        public void AddUserIndex(UserInfo user)
        {
            if (string.IsNullOrEmpty(user.MaxShopId))
            {
                throw new ArgumentNullException("max_shop");
            }

            var document = GetUserFromIndexDirectoryByUid(user.UserId, user.MaxShopId);
            if (document != null)
            {
                RemoveIndex(user.UserId, user.MaxShopId);
            }

            var isNew = false;
            var path = _indexPath + "\\" + user.MaxShopId;
            var directory = FSDirectory.Open(path);

            isNew = !IndexReader.IndexExists(directory);
            AddUserIndexToDirectory(user, directory, isNew);

            var ramDirectory = RamDirectoryFactory.GetRamDirectory(path, directory);
            isNew = !IndexReader.IndexExists(ramDirectory);
            AddUserIndexToDirectory(user, ramDirectory, isNew);
        }

        /// <summary>
        /// 新增某个用户的索引 
        /// </summary>
        /// <param name="user"></param>
        public void AddUserIndex(List<UserInfo> users, int maxshopId)
        {
            var isNew = false;
            var path = _indexPath + "\\" + maxshopId;
            var directory = FSDirectory.Open(path);
            var maxFieldLength = new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH);
            isNew = !IndexReader.IndexExists(directory);

            // 添加到文件索引
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(directory, analyzer, isNew, maxFieldLength))
            {
                foreach (var user in users)
                {
                    var document = new Document();

                    document.Add(new Field("UserId", user.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("ShopId", user.ShopId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("UPinYinShort", user.UPinYinShort.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("UPinYinFull", user.UPinYinFull.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("Name", user.Name.ToString(), Field.Store.YES, Field.Index.ANALYZED));

                    if (!string.IsNullOrWhiteSpace(user.Phone))
                    {
                        document.Add(new Field("Phone", user.Phone.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    }
                    if (!string.IsNullOrWhiteSpace(user.Number))
                    {
                        document.Add(new Field("Number", user.Number.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    }
                    

                    writer.AddDocument(document);
                }

                writer.Optimize();
            }

            // 添加到内存索引
            var ramDirectory = RamDirectoryFactory.GetRamDirectory(path, directory);
            using (var writer = new IndexWriter(ramDirectory, analyzer, isNew, maxFieldLength))
            {
                foreach (var user in users)
                {
                    var document = new Document();

                    document.Add(new Field("UserId", user.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("ShopId", user.ShopId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("UPinYinShort", user.UPinYinShort.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("UPinYinFull", user.UPinYinFull.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("Name", user.Name.ToString(), Field.Store.YES, Field.Index.ANALYZED));

                    if (!string.IsNullOrWhiteSpace(user.Phone))
                    {
                        document.Add(new Field("Phone", user.Phone.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    }
                    if (!string.IsNullOrWhiteSpace(user.Number))
                    {
                        document.Add(new Field("Number", user.Number.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    }

                    writer.AddDocument(document);
                }

                writer.Optimize();
            }

        }
        
         /// <summary>
        /// 更新某个用户的索引 
        /// </summary>
        /// <param name="user"></param>
        public void UpdateIndex(UserInfo user, int maxShopId)
        {
            var document = GetUserFromIndexDirectoryByUid(user.UserId, maxShopId.ToString());
            if (document == null)
            {
                return;
            }
            user.MaxShopId = maxShopId.ToString();
            RemoveIndex(user.UserId ,maxShopId.ToString());
            AddUserIndex(user);
        }

        /// <summary>
        /// 更新某个用户的索引 
        /// </summary>
        /// <param name="user"></param>
        public void UpdateIndex(int uid, int accId, int maxShopId, string name, string phone)
        {
            var document = GetUserFromIndexDirectoryByUid(uid.ToString(), maxShopId.ToString());
            if (document == null)
            {
                return;
            }

            var userInfo = new UserInfo();
            userInfo.UserId = uid.ToString();
            userInfo.ShopId = accId.ToString();
            userInfo.MaxShopId = maxShopId.ToString();
            userInfo.Number = document.Get("Number");

            if (!string.IsNullOrEmpty(name))
            {
                userInfo.Name = name;
                userInfo.UPinYinFull = Helper.GetPinyin(name);
                userInfo.UPinYinShort = Helper.GetInitials(name);
            }
            else
            {
                userInfo.Name = document.Get("Name");
                userInfo.UPinYinFull = document.Get("UPinYinFull");
                userInfo.UPinYinShort = document.Get("UPinYinShort");
            }

            if (!string.IsNullOrEmpty(phone))
            {
                userInfo.Phone = phone;
            }
            else
            {
                userInfo.Phone = document.Get("Phone");
            }

            RemoveIndex(uid.ToString(),maxShopId.ToString());
            AddUserIndex(userInfo);
        }

        
        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="shopIds">店铺Id列表</param>
        /// <param name="term">关键字</param>
        /// <returns></returns>
        public List<int> Search(int maxShop, string term)
        {
            var indexPath = _indexPath + "\\" + maxShop;
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());

            var ramDirectory = RamDirectoryFactory.GetRamDirectory(indexPath, directory);
            IndexReader reader = IndexReader.Open(ramDirectory, true);
            IndexSearcher searcher = new IndexSearcher(reader);

            var nameParse = new QueryParser(Version.LUCENE_30, "Name", new StandardAnalyzer( Version.LUCENE_30));
            var nameQuery = nameParse.Parse(term);

            var phoneTerm = new Term("Phone", string.Format("*{0}*", term));
            var phoneQuery = new WildcardQuery(phoneTerm);

            var numberTerm = new Term("Number", string.Format("*{0}*", term));
            var numberQuery = new WildcardQuery(numberTerm);

            var uPinYinShortTerm = new Term("UPinYinShort", string.Format("*{0}*", term.ToUpper()));
            var uPinYinShortQuery = new WildcardQuery(uPinYinShortTerm);

            var uPinYinFullTerm = new Term("UPinYinFull", string.Format("*{0}*", term.ToUpper()));
            var uPinYinFullQuery = new WildcardQuery(uPinYinFullTerm);

            var query = new BooleanQuery();

            query.Add(uPinYinShortQuery, Occur.SHOULD);
            query.Add(uPinYinFullQuery, Occur.SHOULD);
            query.Add(phoneQuery, Occur.SHOULD);
            query.Add(numberQuery, Occur.SHOULD);
            query.Add(nameQuery, Occur.SHOULD);

            var docs = searcher.Search(query, 20);
            
            var result = new List<int>();
            foreach (var doc in docs.ScoreDocs)
            {
                var document = searcher.Doc(doc.Doc);
                result.Add(Convert.ToInt32(document.Get("UserId")));
            }

            return result;
        }

        /// <summary>
        /// 初始化索引到磁盘
        /// </summary>
        /// <param name="users"></param>
        public void InitIndex(List<UserInfo> users)
        {
            var groupedUsers = users.GroupBy(u => u.MaxShopId);
            foreach (var gUsers in groupedUsers)
            {
                var isNew = false;
                var directory = FSDirectory.Open(_indexPath + "\\" + gUsers.Key);
                var maxFieldLength = new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH);
                isNew = !IndexReader.IndexExists(directory);

                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(directory, analyzer, isNew, maxFieldLength))
                {
                    foreach (var user in gUsers)
                    {
                        var document = new Document();

                        document.Add(new Field("UserId", user.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("ShopId", user.ShopId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("Phone", user.Phone.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("UPinYinShort", user.UPinYinShort.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("UPinYinFull", user.UPinYinFull.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("Number", user.Number.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("Name", user.Name.ToString(), Field.Store.YES, Field.Index.ANALYZED));

                        writer.AddDocument(document);
                    }

                    writer.Optimize();
                }   
            }
        }



        /// <summary>
        /// ES会员索引更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateElasticIndex(UserInfo model)
        {
            bool bResult = false;

            var oUserBase = GetUserSearchModel(model);
            var searchBasic = new SearchBasic();
            bResult = searchBasic.UserUpdate(oUserBase);

            return bResult;
        }

        /// <summary>
        /// ES会员索引更新(批量)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateElasticIndexBluk(List<UserInfo> model)
        {
            if (model == null || !model.Any()) return false;
            var oUserBasics = new List<UserBasic>();
            foreach (var item in model)
            {
                var oUserBase = new UserBasic
                {
                    account_id = Convert.ToInt64(item.ShopId),
                    master_id = Convert.ToInt64(item.MaxShopId),
                    user_id = Convert.ToInt64(item.UserId),
                    user_name = string.IsNullOrEmpty(item.Number) ? "" : item.Number.ToLower()
                };
                oUserBase.user_name = string.IsNullOrEmpty(item.Name) ? "" : item.Name.ToLower();
                oUserBase.user_phone = string.IsNullOrEmpty(item.Phone) ? "" : item.Phone.ToLower();
                oUserBase.user_pinyin = string.IsNullOrEmpty(item.UPinYinFull) ? "" : item.UPinYinFull.ToLower();
                oUserBase.user_initials = string.IsNullOrEmpty(item.UPinYinShort) ? "" : item.UPinYinShort.ToLower();
                oUserBasics.Add(oUserBase);
            }

            var searchBasic = new SearchBasic();
            var bResult = searchBasic.UserUpdateBluk(oUserBasics);

            return bResult;
        }


        /// <summary>
        /// 获取会员搜索model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserBasic GetUserSearchModel(UserInfo model)
        {
            var userBasic = new UserBasic
            {
                account_id = Convert.ToInt64(model.ShopId),
                master_id = Convert.ToInt64(model.MaxShopId),
                user_id = Convert.ToInt64(model.UserId),
                user_cardno = model.Number.ToLower(),
                user_name = model.Name.ToLower(),
                user_phone = model.Phone.ToLower(),
                user_pinyin = model.UPinYinFull.ToLower(),
                user_initials = model.UPinYinShort.ToLower()
            };

            return userBasic;
        }
    }
}
