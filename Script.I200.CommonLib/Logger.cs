namespace CommonLib
{
    /// <summary>  
    /// 日志记录器  
    /// </summary>  
    public  static class Logger
    {
        //委托Login处理其他事务
        private delegate void MGDBAddRunTimeDelegate(string sql, long time);


        public static void AddRunTime(string sql, long time)
        {
            var tokenDelegate = new MGDBAddRunTimeDelegate(MonGoDBAddRunTime);
            tokenDelegate.BeginInvoke(sql, time, null, null);
        }

        private static void MonGoDBAddRunTime(string sql, long time)
        {
            //MongoDBAPI mgApi = new MongoDBAPI();
            //mgApi.RunTime(sql, time);
        } 
    }
}
