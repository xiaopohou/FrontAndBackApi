using System;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using Script.I200.Application.Users;
using Script.I200.Application.UserStoreMoneyCard;
using Script.I200.Application.UserTimesCard;
using Script.I200.Core.Config;
using Script.I200.Data;
using Script.I200.Data.MicroOrm.SqlGenerator;
using Script.I200.Entity.Api.UserTimesCard;
using Script.I200.Entity.API;
using Script.I200.Entity.Enum;
using Script.I200.Entity.Model.Goods;
using Script.I200.Entity.Model.Sales;
using Script.I200.Entity.Model.User;

namespace Script.I200.Application.Sales
{
    /// <summary>
    /// 货品销售
    /// </summary>
    public class SalesService : ISalesService
    {
        private readonly DapperRepository<SalesInfo> _salesDapperRepository;
        private readonly DapperRepository<SalesList> _salesListDapperRepository;
        private readonly DapperRepository<SaleLogInfo> _salesLogDapperRepository;

        private readonly DapperRepository<GoodsInfoSummary> _goodsSummaryDapperRepository;

        private readonly IUserTimesCardService _iUserTimesCardService;
        private readonly IUserStoreMoneyCardService _iUserStoreMoneyCardService;
        private readonly IUserService _iUserService;

        private readonly DapperRepository<Entity.Model.Account.Account> _accountDapperRepository;
        private readonly DapperRepository<UserInfoDetail> _userInfoDapperRepository;
        private readonly DapperRepository<UserLogInfo> _userLogRepository;

        /// <summary>
        /// 初始化
        /// </summary>
        public SalesService()
        {
            var dapperContext =
                new DapperDbContext(new SqlConnection(WebConfigSetting.Instance.I200DbConnectionString));

            _salesDapperRepository = new DapperRepository<SalesInfo>(dapperContext);
            _salesListDapperRepository = new DapperRepository<SalesList>(dapperContext);
            _salesLogDapperRepository = new DapperRepository<SaleLogInfo>(dapperContext);

            _goodsSummaryDapperRepository = new DapperRepository<GoodsInfoSummary>(dapperContext);

            _iUserTimesCardService = new UserTimesCardService();
            _iUserStoreMoneyCardService = new UserStoreMoneyCardService();
            _iUserService = new UserService();

            _accountDapperRepository = new DapperRepository<Entity.Model.Account.Account>(dapperContext);
            _userInfoDapperRepository = new DapperRepository<UserInfoDetail>(dapperContext);
            _userLogRepository = new DapperRepository<UserLogInfo>(dapperContext);
        }

        /// <summary>
        /// 销售还款
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="salesId"></param>
        /// <param name="money"></param>
        /// <param name="remark"></param>
        /// <param name="sendsms"></param>
        /// <returns></returns>
        public int SalesRepayment(UserContext userContext, int salesId, decimal money, string remark, int sendsms,
            int operatorID)
        {
            var salesInfo = _salesDapperRepository.Find(x => x.saleID == salesId && x.accID == userContext.AccId);

            if (salesInfo == null)
            {
                return -1;
            }
            else if (salesInfo.UnpaidMoney <= 0)
            {
                return -2;
            }
            else
            {
                var updateSalesInfo = new SalesInfo();
                updateSalesInfo.accID = userContext.AccId;
                updateSalesInfo.saleID = salesId;
                updateSalesInfo.UnpaidMoney = salesInfo.UnpaidMoney - money;
                updateSalesInfo.CashMoney = salesInfo.CashMoney + money;

                bool result = _salesDapperRepository.Update<SalesInfo>(updateSalesInfo,
                    item => new {item.UnpaidMoney, item.CashMoney});

                if (result)
                {
                    var logModel = new SaleLogInfo
                    {
                        accID = userContext.AccId,
                        saleID = salesId,
                        saleListID = 0,
                        logType = 3,
                        itemType = 0,
                        OriginalVal = salesInfo.UnpaidMoney,
                        EditVal = money
                    };
                    logModel.FinalVal = logModel.OriginalVal - logModel.EditVal;
                    logModel.LogTime = DateTime.Now;
                    logModel.operatorTime = DateTime.Now;
                    logModel.operatorID = userContext.Operater;
                    logModel.operatorIP = userContext.IpAddress;
                    logModel.addedLgUserId = operatorID == 0 ? userContext.Operater : operatorID;
                    logModel.Remark = remark;
                    logModel.Flag = "";

                    _salesLogDapperRepository.Insert(logModel);

                    if (sendsms == 1)
                    {
                        //TODO 接统一短信接口
                    }
                }

                return result ? 1 : 0;
            }
        }

        /// <summary>
        /// 销售退货
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="salesId"></param>
        /// <param name="remark"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int SalesReturnGoods(UserContext userContext, int salesListId, string remark, int type)
        {
            var salesReturnGoodsType = (SalesReturnGoodsType) type;

            //销售列表信息
            var salesList = _salesListDapperRepository.Find(x => x.saleListID == salesListId);
            //销售基本信息
            var salesInfo = _salesDapperRepository.Find(x => x.saleID == salesList.saleID);

            if (salesList == null)
            {
                return -1;
            }

            var returnNumber = salesList.GoodsNum; //退货数量
            decimal returnAbleMoney = salesList.AbleMoney; //应收金额
            decimal returnRealMoney = salesList.RealMoney; //实收金额

            var itemDesc = "";
            switch (type)
            {
                case 1:
                    itemDesc = "商品质量原因";
                    break;
                case 2:
                    itemDesc = "商品不一致";
                    break;
                case 3:
                    itemDesc = "其他原因";
                    break;
                case 4:
                    itemDesc = "输入错误，删除本条记录！";
                    break;
            }

            #region 会员信息处理

            if (salesInfo.isRetail == 0 && salesInfo.uid > 0)
            {
                var userInfo =
                    _userInfoDapperRepository.Find(x => x.Uid == salesInfo.uid && x.AccId == userContext.AccId);

                //是否记次
                if (salesInfo.payType == 3)
                {
                    var userTimesResponse = _iUserTimesCardService.GetUserTimesCardList(userContext, new UserTimesCardSearchParam() { UserId = salesInfo.uid.ToString() });
                    var returnBackTimes = 0;
                    decimal returnBackTimesMoney = 0;

                    if (userTimesResponse.Code == 0)
                    {
                        var userTimesCard = (UserTimesCardSearchParamResult) userTimesResponse.Data;
                        returnBackTimesMoney = (userTimesCard.TotalStoreMoney/userTimesCard.TotalAvaiable)*returnNumber;

                        StringBuilder strSqlUserTimes = new StringBuilder();
                        //实际金额大于扣除储值金额(部分为现金支付)
                        strSqlUserTimes.Append(
                            " UPDATE T_SaleInfo SET RealMoney =ISNULL(RealMoney,0)-cast(@reRealMoney as decimal(18, 2)), StoreMoney =ISNULL(StoreMoney,0)-@ToBackStoreMoney,CashMoney =ISNULL(CashMoney,0)-cast(@ToBackCash as decimal(18, 2)), ReturnBackMoney =ISNULL(ReturnBackMoney,0)+cast(@reRealMoney as decimal(18, 2)) where accID=@accID and saleID=@saleID; ");
                        //退回储值金额	
                        strSqlUserTimes.Append(
                            " UPDATE T_UserInfo SET uStoreMoney =ISNULL(uStoreMoney,0)+cast(@ToBackStoreMoney as decimal(18, 2)) where  [uid]=@userID and  accID IN ( SELECT id FROM dbo.T_Account  WHERE max_shop IN (SELECT  max_shop FROM dbo.T_Account WHERE  ID= @accID));");

                        var sqlParamsUserTimes = new
                        {
                            accID = userContext.AccId,
                            uid = salesInfo.uid,
                            reRealMoney = returnRealMoney,
                            reNumber = returnNumber,
                            timesCardId = salesList.TimeCardId,
                            timesMoneyPay = returnBackTimesMoney,
                            saleID = salesInfo.saleID,
                        };
                        var dapperParamUserTimes = new DynamicParameters(sqlParamsUserTimes);
                        var sqlQueryUserTimes = new SqlQuery(strSqlUserTimes.ToString(), dapperParamUserTimes);
                        _salesDapperRepository.Find(sqlQueryUserTimes);

                        //写日志
                        #region 记次日志

                        var timesLogInfo = new UserLogInfo
                        {
                            AccId = userContext.AccId,
                            OriginalAccId = userContext.AccId,
                            UId = salesInfo.uid,
                            LogType = (int) UserLogTypeEnum.TimesCardChange,
                            ItemType = (int) UserLogItemTypeEnum.ModifyIntegral,
                            LogTime = DateTime.Now,
                            OperatorTime = DateTime.Now,
                            OperatorId = userContext.UserId,
                            OperatorIp = userContext.IpAddress,
                            Remark = remark,
                            Flag = string.Empty,
                            FlagStatus = 0,
                            FlagStatusTime = DateTime.Now,
                            EditMoney = 0,
                            EditMoneyType = 0,
                            AddedLgUserId = userContext.UserId,
                            BindCardId = salesList.TimeCardId,
                            OriginalVal = userTimesCard.TotalAvaiable,
                            EditVal = returnNumber,
                            FinalVal = userTimesCard.TotalAvaiable + returnNumber,
                        };

                        _userLogRepository.Insert(timesLogInfo);

                        #endregion

                    }

                }

                //是否储值
                if (salesInfo.payType == 4)
                {
                    //如果实际金额大于扣除储值金额(部分为现金支付)
                    var returnBackCash = returnRealMoney - salesInfo.StoreMoney;
                    var returnBackStoreMoney = salesInfo.StoreMoney;

                    StringBuilder strSqlStoreMoney = new StringBuilder();
                    //实际金额大于扣除储值金额(部分为现金支付)
                    strSqlStoreMoney.Append(" UPDATE T_SaleInfo SET RealMoney =ISNULL(RealMoney,0)-cast(@reRealMoney as decimal(18, 2)), StoreMoney =ISNULL(StoreMoney,0)-@ToBackStoreMoney,CashMoney =ISNULL(CashMoney,0)-cast(@ToBackCash as decimal(18, 2)), ReturnBackMoney =ISNULL(ReturnBackMoney,0)+cast(@reRealMoney as decimal(18, 2)) where accID=@accID and saleID=@saleID; ");
                    //退回储值金额	
                    strSqlStoreMoney.Append(" UPDATE T_UserInfo SET uStoreMoney =ISNULL(uStoreMoney,0)+cast(@ToBackStoreMoney as decimal(18, 2)) where  [uid]=@userID and  accID IN ( SELECT id FROM dbo.T_Account  WHERE max_shop IN (SELECT  max_shop FROM dbo.T_Account WHERE  ID= @accID));");

                    var sqlParamsStoreMoney = new
                    {
                        accID = userContext.AccId,
                        uid = salesInfo.uid,
                        reRealMoney = returnRealMoney,
                        ToBackCash = returnBackStoreMoney,
                        ToBackStoreMoney = returnBackCash,
                        saleID = salesInfo.saleID,
                    };
                    var dapperParamStoreMoney = new DynamicParameters(sqlParamsStoreMoney);
                    var sqlQueryStoreMoney = new SqlQuery(strSqlStoreMoney.ToString(), dapperParamStoreMoney);
                    _salesDapperRepository.Find(sqlQueryStoreMoney);

                    //写日志
                    #region 储值日志

                    var storeMoneyLogInfo = new UserLogInfo
                    {
                        AccId = userContext.AccId,
                        OriginalAccId = userContext.AccId,
                        UId = salesInfo.uid,
                        LogType = (int)UserLogTypeEnum.StoreChange,
                        ItemType = (int)UserLogItemTypeEnum.ModifyIntegral,
                        LogTime = DateTime.Now,
                        OperatorTime = DateTime.Now,
                        OperatorId = userContext.UserId,
                        OperatorIp = userContext.IpAddress,
                        Remark = remark,
                        Flag = string.Empty,
                        FlagStatus = 0,
                        FlagStatusTime = DateTime.Now,
                        EditMoney = returnRealMoney,
                        EditMoneyType = 0,
                        AddedLgUserId = userContext.UserId,
                        BindCardId = salesList.TimeCardId,
                        OriginalVal =userInfo.UStoreMoney,
                        EditVal = returnRealMoney,
                        FinalVal = userInfo.UStoreMoney + returnRealMoney,
                    };

                    _userLogRepository.Insert(storeMoneyLogInfo);

                    #endregion
                }

                //会员积分(等级)
                if (salesList.isIntegral == 1)
                {
                    //积分配置
                    var accountInfo = _accountDapperRepository.Find(x => x.Id == userContext.AccId);
                    var integralConfig = accountInfo.Proportion;

                    //会员积分处理
                    int nowIntegral = 0;
                    int newIntrgral = 0;
                    decimal propMoney = 0;
                    int propIntegral = 0;
                    decimal tempPropIntegral = 0;
                    string tempStrPropIntegral = "0";

                    //获取积分比例
                    var propList = integralConfig.Split('/');
                    decimal.TryParse(propList[0], out propMoney);
                    decimal.TryParse(propList[1], out tempPropIntegral);
                    tempStrPropIntegral = Math.Round(tempPropIntegral).ToString();
                    int.TryParse(tempStrPropIntegral, out propIntegral);

                    if (propMoney != 0)
                    {
                        nowIntegral = Convert.ToInt32(Math.Floor((returnRealMoney / propMoney) * propIntegral));
                    }
                    else
                    {
                        nowIntegral = 0;
                    }

                    newIntrgral = userInfo.UIntegral - nowIntegral;
                    if (newIntrgral < 0)
                    {
                        newIntrgral = 0;
                    }

                    //操作积分、等级
                    _iUserService.SetUserIntegral(UserIntegralSetTypeEnum.Add, userContext, userInfo.Uid, newIntrgral,
                        "");
                }
            }

            #endregion

            #region 调整库存

            var goodsInfo = _goodsSummaryDapperRepository.Find(x => x.Id == salesList.GoodsID);
            var returnedRemark = "退货入库";
            if (!string.IsNullOrEmpty(remark))
            {
                returnedRemark += remark;
            }
            if (goodsInfo.IsService == 1)
            {
                StockInto(userContext.AccId, -2, salesList.CostPrice, goodsInfo.Id, 0M,
                                DateTime.Now, returnedRemark, userContext.Operater, userContext.IpAddress);
            }
            else
            {
                //颜色尺码入库
                if (goodsInfo.IsExtend == 1)
                {
                    StockIntoEx(userContext.AccId, salesList.CostPrice, -2, goodsInfo.Id,
                                    salesList.SkuId, salesList.GoodsName, returnNumber,
                                    DateTime.Now, returnedRemark, userContext.Operater, userContext.IpAddress);
                }
                else
                {
                    StockInto(userContext.AccId, -2, salesList.CostPrice, goodsInfo.Id,
                                    returnNumber, DateTime.Now, returnedRemark, userContext.Operater, userContext.IpAddress);
                }
            }

            #endregion

            #region 操作销售表

            if (salesReturnGoodsType == SalesReturnGoodsType.InputErrorOfGoods)
            {
                //删除：操作销售表
                StringBuilder strSqlSalesDelete = new StringBuilder();
                strSqlSalesDelete.Append(" DELETE FROM T_Sale_List where accID=@accID and saleListID=@saleListID; ");
                strSqlSalesDelete.Append(
                    " UPDATE T_SaleInfo SET saleKind =ISNULL(saleKind,0)-1, saleNum =ISNULL(saleNum,0)-cast(@GoodsNum as decimal(18, 2)) where accID=@accID and saleID=@saleID; ");

                var sqlParamsSalesDelete = new
                {
                    accID = userContext.AccId,
                    uid = salesInfo.uid,
                    saleListID = salesListId,
                    saleID = salesInfo.saleID,
                    GoodsNum = salesList.GoodsNum,
                };
                var dapperParamSalesDelete = new DynamicParameters(sqlParamsSalesDelete);
                var sqlQuerySalesDelete = new SqlQuery(strSqlSalesDelete.ToString(), dapperParamSalesDelete);
                _salesDapperRepository.Find(sqlQuerySalesDelete);

                return 1;
            }
            else
            {
                //退货：操作销售表
                StringBuilder strSqlSalesUpdate = new StringBuilder();
                strSqlSalesUpdate.Append(
                    " UPDATE T_Sale_List SET returnStatus =1, returnFlag =@itemType, returnDesc =@itemDesc, returnRemark =@Remark,returnTime=GETDATE() where accID=@accID and saleListID=@saleListID; ");
                strSqlSalesUpdate.Append(
                    " UPDATE T_SaleInfo SET saleNum=saleNum-isnull(@reNumber,0) where accID=@accID and saleID=@saleId; ");

                var sqlParamsSalesUpdate = new
                {
                    accID = userContext.AccId,
                    saleListID = salesListId,
                    saleID = salesInfo.saleID,
                    itemType = type,
                    itemDesc = itemDesc,
                    Remark = remark,
                    reNumber = returnNumber,
                };
                var dapperParamSalesUpdate = new DynamicParameters(sqlParamsSalesUpdate);
                var sqlQuerySalesUpdate = new SqlQuery(strSqlSalesUpdate.ToString(), dapperParamSalesUpdate);
                _salesDapperRepository.Find(sqlQuerySalesUpdate);

                return 1;
            }

            #endregion

            return 1;
        }

        #region 辅助方法

        /// <summary>
        /// 商品入库处理
        /// </summary>
        /// <param name="accId">店铺ID</param>
        /// <param name="gid">商品ID</param>
        /// <param name="currentPrice">当前进价</param>
        /// <param name="stockPrice">最终进价</param>
        /// <param name="gQuantity">商品数量</param>
        /// <param name="stockDate">入库时间</param>
        /// <param name="gcRemark">入库备注</param>
        /// <param name="operatorID">操作人员ID</param>
        /// <param name="operatorIP">操作ID</param>
        /// <returns></returns>
        private void StockInto(int accId, decimal currentPrice, decimal stockPrice, int gid, decimal gQuantity, DateTime stockDate, string gcRemark, int operatorID, string operatorIP)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Declare @maxName nvarchar(200),@minName nvarchar(200),@gName nvarchar(200),@gNum decimal(18,4),@gCost decimal(18,4);");
            strSql.Append(" SELECT @maxName=gMaxName, @minName=gMinName, @gName=gName, @gNum=isnull(gQuantity,0),@gCost=isnull(gCostPrice,0)");
            strSql.Append(" FROM T_GoodsInfo");
            strSql.Append(" where gid=@gid and accID=@accID ;");
            strSql.Append(" UPDATE T_GoodsInfo");
            strSql.Append(stockPrice == -1 || stockPrice == -3
                ? " SET gQuantity =isnull(gQuantity,0)+@gQuantity"
                : " SET gQuantity =isnull(gQuantity,0)+@gQuantity,gCostPrice = @stockPrice");
            strSql.Append(" where gid=@gid and accID=@accID ;");
            if (currentPrice == -2)
            {
                strSql.Append(" INSERT INTO T_Goods_StockLog(accID, LogType, ItemType, gid, gMaxName, gMinName, gName, addTime, OriginalVal, EditVal, FinalVal, Remark, Flag, OperatorID, OperatorDate, OperatorIP, MoneyCharge,Price)");
                strSql.Append(" VALUES (@accID,1,2,@gid,@maxName,@minName,@gName,@addTime,@gNum,@gQuantity,(@gNum+@gQuantity),@gcRemark,'',@operatorID,getdate(),@operatorIP,(0-(@gCost*@gQuantity)), @stockPrice)");
            }
            else if (stockPrice == -1)
            {
                strSql.Append(" INSERT INTO T_Goods_StockLog(accID, LogType, ItemType, gid, gMaxName, gMinName, gName, addTime, OriginalVal, EditVal, FinalVal, Remark, Flag, OperatorID, OperatorDate, OperatorIP, MoneyCharge,Price,CurrentPrice)");
                strSql.Append(" VALUES (@accID,1,2,@gid,@maxName,@minName,@gName,@addTime,@gNum,@gQuantity,(@gNum+@gQuantity),@gcRemark,'',@operatorID,getdate(),@operatorIP,(0-(@gCost*@gQuantity)), @gCost,@gCost)");
            }
            else if (stockPrice == -3)
            {
                strSql.Append(" INSERT INTO T_Goods_StockLog(accID, LogType, ItemType, gid, gMaxName, gMinName, gName, addTime, OriginalVal, EditVal, FinalVal, Remark, Flag, OperatorID, OperatorDate, OperatorIP, MoneyCharge,Price,CurrentPrice)");
                strSql.Append(" VALUES (@accID,1,2,@gid,@maxName,@minName,@gName,@addTime,@gNum,@gQuantity,(@gNum+@gQuantity),@gcRemark,'',@operatorID,getdate(),@operatorIP,(0-(@gCost*@gQuantity)), @gCost,@gCost)");
            }
            else
            {
                strSql.Append(" INSERT INTO T_Goods_StockLog(accID, LogType, ItemType, gid, gMaxName, gMinName, gName, addTime, OriginalVal, EditVal, FinalVal, Remark, Flag, OperatorID, OperatorDate, OperatorIP, MoneyCharge,Price,CurrentPrice)");
                strSql.Append(" VALUES (@accID,1,2,@gid,@maxName,@minName,@gName,@addTime,@gNum,@gQuantity,(@gNum+@gQuantity),@gcRemark,'',@operatorID,getdate(),@operatorIP,(0-(@CurrentPrice*@gQuantity)), @stockPrice,@CurrentPrice)");
            }

            var sqlParams = new
            {
                gid = gid,
                accID = accId,
                gQuantity = gQuantity,
                gcRemark = (gcRemark == "" ? "商品入库" : "商品入库-" + gcRemark),
                operatorID = operatorID,
                operatorIP = operatorIP,
                addTime = stockDate,
                stockPrice = stockPrice,
                CurrentPrice = currentPrice,
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery= new SqlQuery(strSql.ToString(), dapperParam);
            _salesDapperRepository.Find(sqlQuery);
        }

        /// <summary>
        /// 扩展商品入库
        /// </summary>
        /// <param name="accId">店铺Id</param>
        /// <param name="stockPrice">商品进价</param>
        /// <param name="currentPrice">当前进价</param>
        /// <param name="gid">商品Id</param>
        /// <param name="skuId">SkuId</param>
        /// <param name="skuName">商品名称</param>
        /// <param name="gQuantity">入库数量</param>
        /// <param name="stockDate">入库日期</param>
        /// <param name="gcRemark">备注信息</param>
        /// <param name="operatorID">操作人员Id</param>
        /// <param name="operatorIP">操作人员Ip</param>
        /// <returns></returns>
        private void StockIntoEx(int accId, decimal stockPrice, decimal currentPrice, int gid, int skuId, string skuName, decimal gQuantity, DateTime stockDate, string gcRemark, int operatorID, string operatorIP)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Declare @maxName nvarchar(200),@minName nvarchar(200),@gName nvarchar(200),@gNum decimal(18,4),@gCost decimal(18,4);");
            strSql.Append(" SELECT @maxName=gMaxName, @minName=gMinName, @gName=gName, @gNum=isnull(T_Goods_Sku.gsQuantity,0),@gCost=isnull(T_Goods_Sku.gsCostPrice,0)");
            strSql.Append(" FROM T_GoodsInfo");
            strSql.Append(" left outer join T_Goods_Sku");
            strSql.Append(" on T_Goods_Sku.gid=T_GoodsInfo.gid");
            strSql.Append(" where T_GoodsInfo.gid=@gid and T_GoodsInfo.accID=@accID and T_GoodsInfo.isDown=0 and T_Goods_Sku.gsId=@skuId;");
            strSql.Append(" UPDATE T_Goods_Sku");
            strSql.Append(stockPrice == -1 || stockPrice == -3
                ? " SET gsQuantity =isnull(gsQuantity,0)+@gQuantity"
                : " SET gsQuantity =isnull(gsQuantity,0)+@gQuantity,gsCostPrice=@stockPrice");
            strSql.Append(" where gid=@gid and accId=@accID and gsId=@skuId;");

            strSql.Append(" update T_GoodsInfo set gQuantity=a.num from( ");
            strSql.Append(" select @gid gid,SUM(gsQuantity) num from T_Goods_Sku where gid=@gid and gsQuantity>0) a where a.gid=T_GoodsInfo.gid; ");
            if (currentPrice == -2)
            {
                strSql.Append(" INSERT INTO T_Goods_StockLog(accID, LogType, ItemType, gid, gMaxName, gMinName, gName, addTime, OriginalVal, EditVal, FinalVal, Remark, Flag, OperatorID, OperatorDate, OperatorIP, MoneyCharge,IsExtend,SkuId,Price)");
                strSql.Append(" VALUES (@accID,1,2,@gid,@maxName,@minName,@skuName,@addTime,@gNum,@gQuantity,(@gNum+@gQuantity),@gcRemark,'',@operatorID,getdate(),@operatorIP,(0-(@gCost*@gQuantity)),1,@skuId, @stockPrice)");
            }
            else if (stockPrice == -1)
            {
                strSql.Append(" INSERT INTO T_Goods_StockLog(accID, LogType, ItemType, gid, gMaxName, gMinName, gName, addTime, OriginalVal, EditVal, FinalVal, Remark, Flag, OperatorID, OperatorDate, OperatorIP, MoneyCharge,IsExtend,SkuId,Price,CurrentPrice)");
                strSql.Append(" VALUES (@accID,1,2,@gid,@maxName,@minName,@skuName,@addTime,@gNum,@gQuantity,(@gNum+@gQuantity),@gcRemark,'',@operatorID,getdate(),@operatorIP,(0-(@gCost*@gQuantity)),1,@skuId, @gCost,@gCost)");
            }
            else if (stockPrice == -3)
            {
                strSql.Append(" INSERT INTO T_Goods_StockLog(accID, LogType, ItemType, gid, gMaxName, gMinName, gName, addTime, OriginalVal, EditVal, FinalVal, Remark, Flag, OperatorID, OperatorDate, OperatorIP, MoneyCharge,IsExtend,SkuId,Price,CurrentPrice)");
                strSql.Append(" VALUES (@accID,1,2,@gid,@maxName,@minName,@skuName,@addTime,@gNum,@gQuantity,(@gNum+@gQuantity),@gcRemark,'',@operatorID,getdate(),@operatorIP,(0-(@gCost*@gQuantity)),1,@skuId, @gCost,@gCost)");
            }
            else
            {
                strSql.Append(" INSERT INTO T_Goods_StockLog(accID, LogType, ItemType, gid, gMaxName, gMinName, gName, addTime, OriginalVal, EditVal, FinalVal, Remark, Flag, OperatorID, OperatorDate, OperatorIP, MoneyCharge,IsExtend,SkuId,Price,CurrentPrice)");
                strSql.Append(" VALUES (@accID,1,2,@gid,@maxName,@minName,@skuName,@addTime,@gNum,@gQuantity,(@gNum+@gQuantity),@gcRemark,'',@operatorID,getdate(),@operatorIP,(0-(@currentPrice*@gQuantity)),1,@skuId, @stockPrice,@currentPrice)");
            }

            var sqlParams = new
            {
                gid = gid,
                accID = accId,
                gQuantity = gQuantity,
                gcRemark = (gcRemark == "" ? "商品入库" : "商品入库-" + gcRemark),
                operatorID = operatorID,
                operatorIP = operatorIP,
                addTime = stockDate,
                stockPrice = stockPrice,
                CurrentPrice = currentPrice,
                skuId = skuId,
                skuName = skuName,
            };
            var dapperParam = new DynamicParameters(sqlParams);
            var sqlQuery = new SqlQuery(strSql.ToString(), dapperParam);
            _salesDapperRepository.Find(sqlQuery);
        }


        #endregion
    }
}
