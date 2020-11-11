using JointOffice.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public  interface IStatistical
    {
        /// <summary>
        /// 区域列表
        /// </summary>
        Showapi_Res_List<Area> GetAreaList();
        /// <summary>
        /// 客户转换
        /// </summary>
        Showapi_Res_Single<KeHuChange> GetKeHuChange(KeHuChangeInPara para);
        /// <summary>
        /// 订单追踪
        /// </summary>
        Showapi_Res_Single<OrderTrack> GetOrderTrack(KeHuChangeInPara para);
        /// <summary>
        /// 订单列表
        /// </summary>
        Showapi_Res_Single<OrderInfo> GetOrderList(OrderListInpara para);
        /// <summary>
        /// 订单详情
        /// </summary>
        Showapi_Res_Single<OrderDetails> GetOrderDetails(DetailsID para);
        /// <summary>
        /// 销售订单
        /// </summary>
        Showapi_Res_Single<SalesOrder> GetSalesOrder(KeHuChangeInPara para);
        /// <summary>
        /// 应收款统计
        /// </summary>
        Showapi_Res_Single<ReceiptCount> GetReceiptCount(KeHuChangeInPara para);
        /// <summary>
        /// 回款统计
        /// </summary>
        Showapi_Res_Single<BackMoney> GetBackMoney(KeHuChangeInPara para);
        /// <summary>
        /// 物流信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<WuliuXinXi> GetWuliuXinXi(DetailsID para);
        ///// <summary>
        ///// 目标管理
        ///// </summary>
        //Showapi_Res_Single<TargetManage> GetTargetManage(TargetManageInPara para);
        ///// <summary>
        ///// 片区销售任务额
        ///// </summary>
        //Showapi_Res_List<AreaSalesMoney> GetAreaSalesMoney(KeHuChangeInPara para);
        /// <summary>
        /// 片区详情
        /// </summary>
        Showapi_Res_Single<AreaDetails> GetAreaDetails(DetailsID para);
        /// <summary>
        /// 销售人员详情
        /// </summary>
        Showapi_Res_Single<SalesPersonDetails> GetSalesPersonDetails(DetailsID para);
        /// <summary>
        /// 个人信息
        /// </summary>
        Showapi_Res_Single<MemberInfo> GetMemberInfo(DetailsID para);
        /// <summary>
        /// 个人信息  Odoo
        /// </summary>
        Showapi_Res_Single<MemberInfoOdoo> GetMemberInfoOdoo(GetInfoOdooAPI para);
        /// <summary>
        /// HR设置
        /// </summary>
        Showapi_Res_Single<HRSet> GetHRSet(DetailsID para);
        /// <summary>
        /// 业务情况
        /// </summary>
        Showapi_Res_Single<BusinessDetails> GetBusinessDetails(DetailsID para);
        /// <summary>
        /// 关联销售 销售总监
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<GetAreaAllListInfo> GetAreaAllList(DetailsID para);
        /// <summary>
        /// 关联销售 销售总监 查看片区
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_List<GetExtensionPersonByOneList> GetExtensionPersonByAreaList(GetExtensionPersonListPara para);
        /// <summary>
        /// 关联销售 区域经理
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<GetExtensionPersonList> GetExtensionPersonList(GetExtensionPersonListPara para);
        /// <summary>
        /// 关联销售 销售人员
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<GetExtensionPersonByOneList> GetExtensionPersonByOneList(GetExtensionPersonByOneListPara para);
        /// <summary>
        /// 查看投诉订单
        /// </summary>
        Showapi_Res_List<GetComplaintListInfo> GetComplaintList(GetComplaintListPara para);
    }
    /// <summary>
    /// 区域列表
    /// </summary>
    public class Area
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public string areaID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string areaName { get; set; }
    }
    /// <summary>
    /// 客户转换  入参
    /// </summary>
    public class KeHuChangeInPara
    {
        /// <summary>
        /// 1销售总监页面  2片区经理页面  3销售人员页面
        /// </summary>
        public string order { get; set; }
        /// <summary>
        /// 1销售任务额  2客户回款额
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        public string year { get; set; }
        /// <summary>
        /// 季度
        /// </summary>
        public string season { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public string month { get; set; }
    }
    public class GetComplaintListPara
    {
        /// <summary>
        /// 年
        /// </summary>
        public string year { get; set; }
        /// <summary>
        /// 季度
        /// </summary>
        public string season { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public string month { get; set; }
        /// <summary>
        /// 订单号 投诉单号
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 全部0   待处理 10  处理中 20 已处理 30
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
    }
    /// <summary>
    /// 客户转换
    /// </summary>
    public class KeHuChange
    {
        /// <summary>
        /// 圆柱线上客户数
        /// </summary>
        public string chartOnlineKeHuNum { get; set; }
        /// <summary>
        /// 圆柱线下客户数
        /// </summary>
        public string chartOfflineKeHuNum { get; set; }
        /// <summary>
        /// 圆柱客户总数
        /// </summary>
        public string chartKeHuNum { get; set; }
        /// <summary>
        /// 圆柱转换率
        /// </summary>
        public string chartRate { get; set; }
        /// <summary>
        /// 柱状图统计
        /// </summary>
        public List<ColumnCount> cloumnCount { get; set; }
        /// <summary>
        /// 区域列表
        /// </summary>
        public List<Area> areaList { get; set; }
    }
    /// <summary>
    /// 柱状图  统计
    /// </summary>
    public class ColumnCount
    {
        /// <summary>
        /// 片区
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 总客户数
        /// </summary>
        public string keHuNum { get; set; }
        ///// <summary>
        ///// 目标线上客户数/目标销售总额/目标回款总额
        ///// </summary>
        //public string onlineKeHuNum { get; set; }
        ///// <summary>
        ///// 完成客户上线数/完成销售总额/完成回款总额
        ///// </summary>
        //public string completeKeHuNum { get; set; }
        /// <summary>
        /// 目标达成率
        /// </summary>
        public string rate { get; set; }
        /// <summary>
        /// 已转换客户数/已完成销售订单额/已回款/已完成回款
        /// </summary>
        public string yesChangeKeHuNum { get; set; }
        /// <summary>
        /// 未转换客户数/未完成销售订单额/应收款/未完成回款
        /// </summary>
        public string noChangeKeHuNum { get; set; }
    }
    /// <summary>
    /// 订单追踪
    /// </summary>
    public class OrderTrack
    {
        /// <summary>
        /// 区域
        /// </summary>
        public List<Area> area { get; set; }
        /// <summary>
        /// 订单概况
        /// </summary>
        public List<OrderProfile> orderProfile { get; set; }
    }
    /// <summary>
    /// 订单概况
    /// </summary>
    public class OrderProfile
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 目标
        /// </summary>
        public string target { get; set; }
        /// <summary>
        /// 实际
        /// </summary>
        public string real { get; set; }
    }
    /// <summary>
    /// 订单列表  入参
    /// </summary>
    public class OrderListInpara
    {
        /// <summary>
        /// 1全部订单 2退货订单 3投诉订单
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        public string year { get; set; }
        /// <summary>
        /// 季度
        /// </summary>
        public string season { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public string month { get; set; }
        /// <summary>
        /// 要搜索的关键字
        /// </summary>
        public string body { get; set; }
    }
    public class OrderInfo
    {
        /// <summary>
        /// 订单列表
        /// </summary>
        public List<OrderList> orderList { get; set; }
        /// <summary>
        /// 片区人员列表
        /// </summary>
        public List<Area> area { get; set; }
    }
    /// <summary>
    /// 订单列表
    /// </summary>
    public class OrderList
    {
        public string id { get; set; }
        /// <summary>
        /// 片区名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string orderCode { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string keHuName { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public string orderDate { get; set; }
        /// <summary>
        /// 客户性质
        /// </summary>
        public string keHuProperty { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public string payMoney { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string payMethod { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string orderState { get; set; }
        /// <summary>
        /// 订单状态 int
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string reason { get; set; }
    }
    /// <summary>
    /// 订单ID  入参
    /// </summary>
    public class DetailsID
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string id { get; set; }
    }
    /// <summary>
    /// 订单详情
    /// </summary>
    public class OrderDetails
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string orderCode { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 公司logo
        /// </summary>
        public string companyLogo { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        public string companyAddress { get; set; }
        /// <summary>
        /// 公司电话
        /// </summary>
        public string companyPhone { get; set; }
        /// <summary>
        /// 片区
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 销售人员
        /// </summary>
        public string salesPerson { get; set; }
        /// <summary>
        /// 交货
        /// </summary>
        public string delivery { get; set; }
        /// <summary>
        /// 发票
        /// </summary>
        public string invoice { get; set; }
        /// <summary>
        /// 发票地址
        /// </summary>
        public string invoiceAddress { get; set; }
        /// <summary>
        /// 送货地址
        /// </summary>
        public string orderAddress { get; set; }
        /// <summary>
        /// 确认日期
        /// </summary>
        public string sureDate { get; set; }
        /// <summary>
        /// 付款条款
        /// </summary>
        public string payTerms { get; set; }
        /// <summary>
        /// 交货方式
        /// </summary>
        public string deliveryMethod { get; set; }
        /// <summary>
        /// 交货金额
        /// </summary>
        public string deliveryMoney { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public string payDate { get; set; }
        /// <summary>
        /// 未含税金额
        /// </summary>
        public string noGeldMoney { get; set; }
        /// <summary>
        /// 税金
        /// </summary>
        public string zongGeld { get; set; }
        /// <summary>
        /// 总计
        /// </summary>
        public string payMoney { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public string payMethod { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public List<CommodityDetails> commodityDetails { get; set; }
    }
    /// <summary>
    /// 商品详情
    /// </summary>
    public class CommodityDetails
    {
        /// <summary>
        /// 商品图片
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 商品属性
        /// </summary>
        public string property { get; set; }
        /// <summary>
        /// 现价
        /// </summary>
        public string nowPrice { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public string formerPrice { get; set; }
        /// <summary>
        /// 订购数
        /// </summary>
        public string buyNum { get; set; }
        /// <summary>
        /// 已送数
        /// </summary>
        public string giftNum { get; set; }
        /// <summary>
        /// 税金
        /// </summary>
        public string geld { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public string rebate { get; set; }
        /// <summary>
        /// 已开票
        /// </summary>
        public string doInvoice { get; set; }
        /// <summary>
        /// 小计
        /// </summary>
        public string subtotal { get; set; }
    }
    /// <summary>
    /// 销售订单
    /// </summary>
    public class SalesOrder
    {
        /// <summary>
        /// 柱状图统计
        /// </summary>
        public List<ColumnCount> cloumnCount { get; set; }
        /// <summary>
        /// 区域列表
        /// </summary>
        public List<Area> areaList { get; set; }
    }
    /// <summary>
    /// 应收款统计
    /// </summary>
    public class ReceiptCount
    {
        /// <summary>
        /// 柱状图统计
        /// </summary>
        public List<ColumnCount> cloumnCount { get; set; }
        /// <summary>
        /// 区域列表
        /// </summary>
        public List<Area> areaList { get; set; }
    }
    /// <summary>
    /// 回款统计
    /// </summary>
    public class BackMoney
    {
        /// <summary>
        /// 最大值
        /// </summary>
        public string maxNum { get; set; }
        /// <summary>
        /// 柱状图统计
        /// </summary>
        public List<ColumnCount> cloumnCount { get; set; }
        /// <summary>
        /// 区域列表
        /// </summary>
        public List<Area> areaList { get; set; }
    }
    /// <summary>
    /// 目标管理  入参
    /// </summary>
    public class TargetManageInPara
    {
        /// <summary>
        /// 1销售任务额  2客户回款额
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 1销售总监访问所有片区数据  2销售总监访问一个片区数据
        /// </summary>
        public string order { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        public string year { get; set; }
        /// <summary>
        /// 季度
        /// </summary>
        public string season { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public string month { get; set; }
    }
    /// <summary>
    /// 目标管理
    /// </summary>
    public class TargetManage
    {
        /// <summary>
        /// 区域列表
        /// </summary>
        public List<Area> areaList { get; set; }
        /// <summary>
        /// 目标管理详情
        /// </summary>
        public List<TargetManageBody> targetManageBody { get; set; }
    }
    /// <summary>
    /// 目标管理详情
    /// </summary>
    public class TargetManageBody
    {
        /// <summary>
        /// 片区Id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 片区
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string money { get; set; }
    }
    /// <summary>
    /// 片区销售任务额
    /// </summary>
    public class AreaSalesMoney
    {
        /// <summary>
        /// 季度
        /// </summary>
        public string season { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string money { get; set; }
        /// <summary>
        /// 片区销售任务额列表
        /// </summary>
        public List<AreaSalesMoneyList> areaSalesMoneyList { get; set; }
    }
    /// <summary>
    /// 片区销售任务额列表
    /// </summary>
    public class AreaSalesMoneyList
    {
        ///// <summary>
        ///// 季度
        ///// </summary>
        //public string season { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public string month { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string money { get; set; }
    }
    /// <summary>
    /// 片区详情
    /// </summary>
    public class AreaDetails
    {
        /// <summary>
        /// 区域名称
        /// </summary>
        public string areaName { get; set; }
        /// <summary>
        /// 片区负责人
        /// </summary>
        public string areaFuZeRen { get; set; }
        /// <summary>
        /// 人员list
        /// </summary>
        public List<MemberList> memberList { get; set; }
    }
    /// <summary>
    /// 人员list
    /// </summary>
    public class MemberList
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string jobName { get; set; }
    }
    /// <summary>
    /// 销售人员详情
    /// </summary>
    public class SalesPersonDetails
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 办公地点
        /// </summary>
        public string workAddress { get; set; }
        /// <summary>
        /// 办公手机
        /// </summary>
        public string workMobilePhone { get; set; }
        /// <summary>
        /// 工作地点
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 工作email
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 办公电话
        /// </summary>
        public string workPhone { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 工作头衔
        /// </summary>
        public string jobName { get; set; }
        /// <summary>
        /// 管理员
        /// </summary>
        public string admin { get; set; }
        /// <summary>
        /// 师傅
        /// </summary>
        public string master { get; set; }
        /// <summary>
        /// 工作时间
        /// </summary>
        public string workTime { get; set; }
    }
    /// <summary>
    /// 个人信息
    /// </summary>
    public class MemberInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string jobname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string birthday { get; set; }
        /// <summary>
        /// 出生地
        /// </summary>
        public string birthplace { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string marital { get; set; }
        /// <summary>
        /// 子女数
        /// </summary>
        public string sonNum { get; set; }
        /// <summary>
        /// 家庭地址
        /// </summary>
        public string familyAddress { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string idCard { get; set; }
        /// <summary>
        /// 护照号
        /// </summary>
        public string passportNum { get; set; }
        /// <summary>
        /// 银行账户号码
        /// </summary>
        public string bankNum { get; set; }
    }
    /// <summary>
    /// HR设置
    /// </summary>
    public class HRSet
    {
        /// <summary>
        /// 工时表成本
        /// </summary>
        public string workHourTable { get; set; }
        /// <summary>
        /// 科目
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 相关用户
        /// </summary>
        public string relatedUser { get; set; }
        /// <summary>
        /// badgeID
        /// </summary>
        public string badgeID { get; set; }
        /// <summary>
        /// pin
        /// </summary>
        public string pin { get; set; }
        /// <summary>
        /// 手动出勤
        /// </summary>
        public string attendance { get; set; }
        /// <summary>
        /// 体验
        /// </summary>
        public string experience { get; set; }
        /// <summary>
        /// 公司汽车
        /// </summary>
        public string companyCar { get; set; }
        /// <summary>
        /// 家到公司的距离
        /// </summary>
        public string distance { get; set; }
        /// <summary>
        /// 护照号
        /// </summary>
        public string number { get; set; }
    }
    /// <summary>
    /// 业务情况
    /// </summary>
    public class BusinessDetails
    {
        /// <summary>
        /// 客户总数
        /// </summary>
        public string keHuNum { get; set; }
        /// <summary>
        /// 线上客户数
        /// </summary>
        public string onlineKeHuNum { get; set; }
        /// <summary>
        /// 线下客户数
        /// </summary>
        public string offlineKeHuNum { get; set; }
        /// <summary>
        /// 客户转换率
        /// </summary>
        public string rate { get; set; }
        /// <summary>
        /// 年订单任务额
        /// </summary>
        public string yearOrderMoney { get; set; }
        /// <summary>
        /// 目前完成订单任务额
        /// </summary>
        public string carryOrderMoney { get; set; }
        /// <summary>
        /// 未完成订单任务额
        /// </summary>
        public string noCarryOrderMoney { get; set; }
        /// <summary>
        /// 本月订单任务额
        /// </summary>
        public string monthOrderMoney { get; set; }
        /// <summary>
        /// 本月已完成订单任务额
        /// </summary>
        public string monthCarryOrderMoney { get; set; }
        /// <summary>
        /// 客户欠款金额
        /// </summary>
        public string keHuOweMoney { get; set; }
        /// <summary>
        /// 客户欠款订单
        /// </summary>
        public string keHuOweOrder { get; set; }
        /// <summary>
        /// 今年计划追欠
        /// </summary>
        public string yearPlanMoney { get; set; }
        /// <summary>
        /// 本月计划追欠
        /// </summary>
        public string monthPlanMoney { get; set; }
    }
    /// <summary>
    /// 关联销售列表
    /// </summary>
    public class GetAreaAllListInfo
    {
        /// <summary>
        /// 推广片区列表
        /// </summary>
        public List<Area> area { get; set; }
        public List<GetAreaAllList> info { get; set; }
    }
    /// <summary>
    /// 关联销售列表
    /// </summary>
    public class GetAreaAllList
    {
        /// <summary>
        /// 推广片区名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 推广片区Id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 片区列表
        /// </summary>
        public List<Area> area { get; set; }
    }
    public class GetExtensionPersonListPara
    {
        /// <summary>
        /// 片区Id
        /// </summary>
        public string id { get; set; }
    }
    public class GetExtensionPersonList
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string jobName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string telephone { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        public string memberId { get; set; }
        /// <summary>
        /// 人员列表
        /// </summary>
        public List<MemberList> personList { get; set; }

    }
    public class GetExtensionPersonByOneListPara
    {
        /// <summary>
        /// 人员id
        /// </summary>
        public string memberId { get; set; }
    }
    public class GetExtensionPersonByOneList
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string jobName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string telephone { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        public string memberId { get; set; }
    }


    /// <summary>
    /// Odoo  获取人员信息接口返回值
    /// </summary>
    public class OdooMemberInfoAPIResult
    {
        public string message { get; set; }
        public List<MemberInfoOdoo> result { get; set; }
        public bool success { get; set; }
    }
    /// <summary>
    /// 解析人员基本信息
    /// </summary>
    public class MemberInfoOdoo
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 员工头像
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string jobID { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string birthday { get; set; }
        /// <summary>
        /// 出生地
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 家庭地址
        /// </summary>
        public string home { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string idCard { get; set; }
        /// <summary>
        /// 银行卡
        /// </summary>
        public string bankID { get; set; }
    }
    /// <summary>
    /// Odoo接口  传参
    /// </summary>
    public class GetInfoOdooAPI
    {
        public string id { get; set; }
        public string token { get; set; }
        public string mark { get; set; }
    }
    public class GetComplaintListInfo
    {
        /// <summary>
        /// 投诉单号
        /// </summary>
        public string complaintCode { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderCode { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 投诉时间
        /// </summary>
        public string complaintDate { get; set; }
        /// <summary>
        /// 公司名
        /// </summary>
        public string complaintCompany { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 投诉id
        /// </summary>
        public string id { get; set; }
    }
    public class GetComplaintListInfoPara
    {
        public string type { get; set; }
        public string createdate { get; set; }
        public string enddate { get; set; }
    }
}
