namespace Script.I200.Entity.Enum
{
    /// <summary>
    /// 店员权限
    /// </summary>
    public static class AccountUserPowerEnum
    {
        /// <summary>
        /// 权限 原版
        /// Author：白磊成
        /// Email: xbfighitng@hotmail.com
        /// Date : 2015-10-28 整理
        /// </summary>
        public enum UserPowerV1
        {
            /// <summary>
            /// 查看分店的信息
            /// </summary>
            BranchManage = 1,

            /// <summary>
            /// 删除会员 充值记录 积分记录
            /// </summary>
            DeleteUser = 2,

            /// <summary>
            /// 商品盘点
            /// </summary>
            GoodsCheck = 4,

            /// <summary>
            /// 未知已删除权限
            /// </summary>
            X = 8,

            /// <summary>
            /// 销售记录查阅 修改
            /// </summary>
            SalesView = 16,

            /// <summary>
            /// 系统设置
            /// </summary>
            SystemManage = 32,

            /// <summary>
            /// 智能分析
            /// </summary>
            AnalysisView = 64,

            /// <summary>
            /// 支出管理
            /// </summary>
            ExpenditureManage = 128,

            /// <summary>
            /// 新增商品（商品添加、修改、出入库）
            /// </summary>
            GoodsManage = 256,

            /// <summary>
            /// 显示完整的会员电话
            /// </summary>
            ShowVipPhoneNumber = 512,

            /// <summary>
            /// 禁止在移动端登录
            /// </summary>
            DenyMobileLogin = 1024
        }

        /// <summary>
        /// 权限 新版1.0
        /// Author：白磊成
        /// Email: whiteking5120@gmail.com
        /// Date : 2015-10-28
        /// </summary>
        public enum UserPowerV2Enum
        {
            #region v1版本权限不变部分
            /// <summary>
            /// 查看分店的信息
            /// </summary>
            BranchManage = 1,

            /// <summary>
            /// 删除会员 充值记录 积分记录
            /// </summary>
            DeleteUser = 2,

            /// <summary>
            /// 商品盘点
            /// </summary>
            GoodsCheck = 4,

            /// <summary>
            /// 未知已删除权限
            /// </summary>
            X = 8,

            #region 权限修改 降低了查询销售的范围 只能获取自己的销售列表 其他销售只能通过搜索
            /// <summary>
            /// 销售记录查阅 修改
            /// </summary>
            SalesView = 16,
            #endregion

            /// <summary>
            /// 系统设置
            /// </summary>
            SystemManage = 32,

            /// <summary>
            /// 智能分析
            /// </summary>
            AnalysisView = 64,

            /// <summary>
            /// 支出管理
            /// </summary>
            ExpenditureManage = 128,

            /// <summary>
            /// 新增商品（商品添加、修改、出入库）
            /// </summary>
            GoodsManage = 256,

            /// <summary>
            /// 显示完整的会员电话
            /// </summary>
            ShowVipPhoneNumber = 512,

            /// <summary>
            /// 禁止在移动端登录
            /// </summary>
            DenyMobileLogin = 1024,
            #endregion

            #region AnalysisView=64 智能分析 拆分
            /// <summary>
            /// 基本分析（排除利润和店员业绩）
            /// </summary>
            AnalysisViewBase = 2048,
            /// <summary>
            /// 毛利润和净利润查看
            /// </summary>
            AnalysisProfitView = 4096,
            #endregion

            #region GoodsManage=256 原商品新增 拆分
            /// <summary>
            /// 商品新增
            /// </summary>
            AddGoods = 8192,
            /// <summary>
            /// 商品修改
            /// </summary>
            EditGoods = 16384,
            /// <summary>
            /// 商品出库
            /// </summary>
            StockOut = 32768,
            /// <summary>
            /// 商品入库
            /// </summary>
            StockIn = 65536,
            #endregion

            #region 新增细分权限
            /// <summary>
            /// 销售 金额相关修改（单价，折扣，实收两处）
            /// </summary>
            AmountModification = 131072,
            /// <summary>
            /// 添加 修改用户
            /// </summary>
            AddUpdateVip = 262144,
            /// <summary>
            /// 储值计次
            /// </summary>
            StoreAndTimes = 524288,
            /// <summary>
            /// 会员配置 系统设置开启同步开启
            /// </summary>
            VipConfig = 1048576,
            /// <summary>
            /// 打印配置
            /// </summary>
            PrintConfig = 2097152,
            /// <summary>
            /// 进价查看
            /// </summary>
            PurchasePriceView = 4194304,
            #endregion

            #region 新增功能权限
            /// <summary>
            /// 短信
            /// </summary>
            Sms = 8388608,
            /// <summary>
            /// 橱窗
            /// </summary>
            MobileShowCase = 16777216,
            /// <summary>
            /// 优惠券
            /// </summary>
            Coupon = 33554432,
            /// <summary>
            /// 微信营销
            /// </summary>
            WeChatMarkting = 67108864,
            /// <summary>
            /// 供应商管理
            /// </summary>
            SuppilerManage = 134217728,
            /// <summary>
            /// 销售金额
            /// </summary>
            SaleMoney = 268435456
            #endregion
        }
    }
}
