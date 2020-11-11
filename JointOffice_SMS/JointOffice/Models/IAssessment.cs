using JointOffice.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public interface IAssessment
    {
        /// <summary>
        /// 获取客户授信列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_List<GetKeHuCreditList> GetKeHuCreditList(GetKeHuCreditListPara para);
        /// <summary>
        /// 获取客户授信列表  Odoo
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<GetKeHuCreditListOdoo> GetKeHuCreditListOdoo(GetInfoOdooAPI para);
        /// <summary>
        /// 获取客户信息
        /// </summary>
        Showapi_Res_Single<GetCompanyInfo> GetCompanyInfo(GetCompanyInfoPara para);
        /// <summary>
        /// 获取客户信息  Odoo
        /// </summary>
        Showapi_Res_Single<CompanyInfoOdoo> GetCompanyInfoOdoo(GetInfoOdooAPI para);
        /// <summary>
        /// 获取公司联系人信息  Odoo
        /// </summary>
        Showapi_Res_List<CompanyPersonInfoOdoo> GetCompanyPersonInfoOdoo(GetInfoOdooAPI para);
        /// <summary>
        /// 获取客户金钱信息
        /// </summary>
        Showapi_Res_Single<GetCompanyMoneyInfo> GetCompanyMoneyInfo(GetCompanyInfoPara para);
        /// <summary>
        /// 提交授信 修改授信
        /// </summary>
        Showapi_Res_Meaasge UpdateCredit(UpdateCreditPara para);
        /// <summary>
        /// 审核授信
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge AuditCredit(AuditCreditPara para);
        /// <summary>
        /// 销售人员
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_List<GetSalePersonList> GetSalePersonList(GetSalePersonListPara para);
        /// <summary>
        /// 绑定销售人员
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge BinDingSalePerson(BinDingSalePerson para);
        /// <summary>
        /// 获取应收账款统计信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_Single<GetAccountsReceivableInfo> GetAccountsReceivableInfo(GetAccountsReceivableInfoPara para);
        /// <summary>
        /// 我的钱包
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_Single<GetMyPurse> GetMyPurse(GetMyPursePara para);
        /// <summary>
        ///我的钱包月份明细
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_Single<GetMyPurse> GetMyPurseByMonth(GetMyPurseByMonthPara para);
        /// <summary>
        ///客户工商信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_Single<GetGongShangInfo> GetGongShangInfo(GetGongShangInfoPara para);
        /// <summary>
        ///客户工商信息  Odoo
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<CompanyBusInfoOdoo> GetGongShangInfoOdoo(GetInfoOdooAPI para);
    }
    /// <summary>
    /// 获取客户授信列表入参
    /// </summary>
    public class GetKeHuCreditListPara
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 状态 0全部 1未授信 2已授信 3待审核授信
        /// </summary>
        public string state { get; set; }
    }
    /// <summary>
    /// 客户授信列表
    /// </summary>
    public class GetKeHuCreditList
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 状态 1未授信 2已授信 3待审核授信
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string info { get; set; }
        /// <summary>
        /// 人名
        /// </summary>
        public string person { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string money { get; set; }
        /// <summary>
        /// 客户id
        /// </summary>
        public string kehuId { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string contact { get; set; }
        /// <summary>
        /// 是否绑定销售
        /// </summary>
        public string binDingSale { get; set; }
    }
    /// <summary>
    /// 获取客户信息入参
    /// </summary>
    public class GetCompanyInfoPara
    {
        /// <summary>
        /// 公司id
        /// </summary>
        public string kehuId { get; set; }
    }
    /// <summary>
    /// 公司金钱基本信息
    /// </summary>
    public class GetCompanyMoneyInfo
    {
        /// <summary>
        /// 公司id
        /// </summary>
        public string kehuId { get; set; }
        /// <summary>
        /// 公司名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 订单数
        /// </summary>
        public string orderNum { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string money { get; set; }
        /// <summary>
        /// 欠费
        /// </summary>
        public string arrears { get; set; }
        ///// <summary>
        ///// 额度权限
        ///// </summary>
        //public string quotaMoney { get; set; }
        /// <summary>
        /// 客户等级
        /// </summary>
        public string grade { get; set; }
        /// <summary>
        /// 授信金额
        /// </summary>
        public string creditMoney { get; set; }
        /// <summary>
        /// 客户等级
        /// </summary>
        public List<KeHuGrade> keHuGrade { get; set; }
    }
    /// <summary>
    /// 客户等级
    /// </summary>
    public class KeHuGrade
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 级别名称
        /// </summary>
        public string name { get; set; }
    }
    /// <summary>
    /// 公司基本信息
    /// </summary>
    public class GetCompanyInfo
    {
        /// <summary>
        /// 客户id
        /// </summary>
        public string kehuId { get; set; }
        /// <summary>
        /// 客户名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string url { get; set; }
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
        /// 传真
        /// </summary>
        public string fax { get; set; }
        /// <summary>
        /// 网站
        /// </summary>
        public string website { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string label { get; set; }
        public List<ContactsInfo> contactsInfolist { get; set; }
    }
    /// <summary>
    /// 公司联系人基本信息
    /// </summary>
    public class ContactsInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string realname { get; set; }
        /// <summary>
        /// 称谓
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public string jobname { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string telephone { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picture { get; set; }
    }
    /// <summary>
    /// 提交授信 修改授信入参
    /// </summary>
    public class UpdateCreditPara
    {
        /// <summary>
        /// 公司id
        /// </summary>
        public string kehuId { get; set; }
        /// <summary>
        /// 客户级别
        /// </summary>
        public string grade { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string money { get; set; }
    }
    /// <summary>
    /// 审核授信入参
    /// </summary>
    public class AuditCreditPara
    {
        /// <summary>
        /// 客户id
        /// </summary>
        public string kehuId { get; set; }
        /// <summary>
        /// 审批操作 审批通过 yes  不通过 no
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 审批不通过原因
        /// </summary>
        public string remarks { get; set; }
    }
    /// <summary>
    /// 销售人员
    /// </summary>
    /// <param name="para"></param>
    /// <returns></returns>
    public class GetSalePersonList
    {
        /// <summary>
        /// 人员id
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picture { get; set; }
    }
    /// <summary>
    /// 绑定销售人员入参
    /// </summary>
    /// <param name="para"></param>
    /// <returns></returns>
    public class BinDingSalePerson
    {
        /// <summary>
        /// 客户id
        /// </summary>
        public string kehuId { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        public string memberid { get; set; }
    }
    /// <summary>
    /// 获取应收账款统计信息入参
    /// </summary>
    public class GetAccountsReceivableInfoPara
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
        /// 片区
        /// </summary>
        public string area { get; set; }
    }
    /// <summary>
    /// 应收账款统计信息
    /// </summary>
    public class GetAccountsReceivableInfo
    {
        /// <summary>
        /// 片区列表
        /// </summary>
        public List<AreaList> arealist { get; set; }
        /// <summary>
        /// 统计明细
        /// </summary>
        public List<AccountsReceivableInfo> accountsReceivableList { get; set; }
    }
    /// <summary>
    /// 片区列表
    /// </summary>
    public class AreaList
    {
        /// <summary>
        /// 片区id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 片区名称
        /// </summary>
        public string name { get; set; }
    }
    /// <summary>
    /// 应收账款统计信息明细
    /// </summary>
    public class AccountsReceivableInfo
    {
        /// <summary>
        /// 已回款
        /// </summary>
        public string payment { get; set; }
        /// <summary>
        /// 未回款
        /// </summary>
        public string nopayment { get; set; }
        /// <summary>
        /// 片区
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 订单额
        /// </summary>
        public string order { get; set; }
    }
    public class WuliuPara
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 请求状态
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 物流信息
        /// </summary>
        public WuliuXinXi result { get; set; }
    }
    public class GongShangPara
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 请求状态
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 工商信息
        /// </summary>
        public List<GongShangInfo> result { get; set; }//创建日期
    }
    public class GongShang
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 请求状态
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 工商明细
        /// </summary>
        public GongShangMingXi result { get; set; }
    }
    public class GongShangInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string companyKey { get; set; }
        /// <summary>
        /// 公司名
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 注册号
        /// </summary>
        public string regNumber { get; set; }
        /// <summary>
        /// 地区号
        /// </summary>
        public string areaCode { get; set; }
        /// <summary>
        /// 地区名
        /// </summary>
        public string areaName { get; set; }
        /// <summary>
        /// 法人
        /// </summary>
        public string faRen { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 网站
        /// </summary>
        public string webSite { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 经营项目
        /// </summary>
        public string bussinessDes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string urlLink { get; set; }

    }
    public class GongShangMingXi
    {
        /// <summary>
        /// 
        /// </summary>
        public string companyKey { get; set; }
        /// <summary>
        /// 信用代码
        /// </summary>
        public string creditCode { get; set; }
        /// <summary>
        /// 公司名
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 公司代码
        /// </summary>
        public string companyCode { get; set; }
        /// <summary>
        /// 注册号
        /// </summary>
        public string regNumber { get; set; }
        /// <summary>
        /// 地区号
        /// </summary>
        public string areaCode { get; set; }
        /// <summary>
        /// 地区名
        /// </summary>
        public string areaName { get; set; }
        /// <summary>
        /// 法人
        /// </summary>
        public string faRen { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 网站
        /// </summary>
        public string webSite { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 经营项目
        /// </summary>
        public string bussinessDes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string businessStatus { get; set; }
        /// <summary>
        /// 注册类型
        /// </summary>
        public string regType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string issueTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bussiness { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string regOrgName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string regMoney { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cerValidityPeriod { get; set; }
        /// <summary>
        /// 股东列表
        /// </summary>
        public List<gdlist> gdList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PersonList> memberList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<branch> branch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<investment> investment { get; set; }

    }
    public class PersonList
    {
        /// <summary>
        /// 
        /// </summary>
        public string memberName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string position { get; set; }
    }
    public class branch
    {
        /// <summary>
        /// 
        /// </summary>
        public string companyKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string companyCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string companyName { get; set; }
    }
    public class investment
    {
        /// <summary>
        /// 
        /// </summary>
        public string companyKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string companyCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string companyName { get; set; }
    }
    public class gdlist
    {
        /// <summary>
        /// 股东名称
        /// </summary>
        public string gdName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string money { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string property { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string proportion { get; set; }
    }
    public class WuliuXinXi
    {
        public List<WuliuInfo> list { get; set; }
        //弃用
        //public int issign { get; set; }
        /// <summary>
        /// 1在途中 2派件中 3已签收 4派送失败 （拒签等）
        /// </summary>
        public int deliverystatus { get; set; }
        /// <summary>
        /// 在途中 派件中 已签收 派送失败 （拒签等）
        /// </summary>
        public string deliverystatusStr { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public string waybillNum { get; set; }
        /// <summary>
        /// 信息来源
        /// </summary>
        public string infoSource { get; set; }
    }
    public class WuliuInfo
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string status { get; set; }
    }
    /// <summary>
    /// 我的钱包 入参
    /// </summary>
    public class GetMyPursePara
    {
        /// <summary>
        /// 年
        /// </summary>
        public string year { get; set; }
    }
    /// <summary>
    /// 我的钱包
    /// </summary>
    public class GetMyPurse
    {
        /// <summary>
        /// 提成总金额
        /// </summary>
        public string sumMoney { get; set; }
        /// <summary>
        /// 已发提成
        /// </summary>
        public string payMoney { get; set; }
        /// <summary>
        /// 未发提成
        /// </summary>
        public string nopayMoney { get; set; }
        public List<YingShouBaoBiao> monthlist { get; set; }
    }
    public class YingShouBaoBiao
    {
        /// <summary>
        /// 月份 
        /// </summary>
        public string month { get; set; }
        /// <summary>
        /// 提成总金额
        /// </summary>
        public string sumMoney { get; set; }
        /// <summary>
        /// 已发提成
        /// </summary>
        public string payMoney { get; set; }
        /// <summary>
        /// 未发提成
        /// </summary>
        public string nopayMoney { get; set; }
    }
    /// <summary>
    /// 我的钱包月份明细 入参
    /// </summary>
    public class GetMyPurseByMonthPara
    {
        /// <summary>
        /// 年
        /// </summary>
        public string year { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public string month { get; set; }
    }
    public class GetSalePersonListPara
    {
        public int page { get; set; }
        public int count { get; set; }
    }
    public class GetGongShangInfoPara
    {
        public string name { get; set; }
    }
    /// <summary>
    ///客户工商信息
    /// </summary>
    /// <returns></returns>
    public class GetGongShangInfo
    {
        /// <summary>
        /// 信用代码
        /// </summary>
        public string creditCode { get; set; }
        /// <summary>
        /// 公司名
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 法人
        /// </summary>
        public string faRen { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 经营项目
        /// </summary>
        public string bussinessDes { get; set; }
        /// <summary>
        /// 注册类型
        /// </summary>
        public string regType { get; set; }
        /// <summary>
        /// 成立日期
        /// </summary>
        public string issueTime { get; set; }
        /// <summary>
        /// 登记机关
        /// </summary>
        public string regOrgName { get; set; }
        /// <summary>
        ///  营业期限
        /// </summary>
        public string cerValidityPeriod { get; set; }
        /// <summary>
        /// 注册资本
        /// </summary>
        public string regMoney { get; set; }
        
    }


    /// <summary>
    /// Odoo  获取客户信息接口返回值
    /// </summary>
    public class OdooCompanyInfoAPIResult
    {
        public string message { get; set; }
        public List<CompanyInfoOdoo> result { get; set; }
        public bool success { get; set; }
    }
    /// <summary>
    /// 解析客户基本信息
    /// </summary>
    public class CompanyInfoOdoo
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 客户头像
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string contact_address { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 固话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string fax { get; set; }
        /// <summary>
        /// 网页
        /// </summary>
        public string web { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string category_id { get; set; }
        /// <summary>
        /// 授信金额
        /// </summary>
        public string credit_limit { get; set; }
        /// <summary>
        /// 客户等级
        /// </summary>
        public string client_grade { get; set; }
    }
    /// <summary>
    /// Odoo  获取客户工商信息接口返回值
    /// </summary>
    public class OdooCompanyBusInfoAPIResult
    {
        public string message { get; set; }
        public List<CompanyBusInfoOdoo> result { get; set; }
        public bool success { get; set; }
    }
    public class OdooReturn<T> where T : class
    {
        public string message { get; set; }
        public List<T> result { get; set; } = new List<T>();
        public bool success { get; set; }
    }
    /// <summary>
    /// 解析客户工商信息
    /// </summary>
    public class CompanyBusInfoOdoo
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 信用代码
        /// </summary>
        public string credit_code { get; set; }
        /// <summary>
        /// 法人
        /// </summary>
        public string legalperson { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string contact_address { get; set; }
        /// <summary>
        /// 经营项目
        /// </summary>
        public string operateproject { get; set; }
        /// <summary>
        /// 注册类型
        /// </summary>
        public string registertype { get; set; }
        /// <summary>
        /// 注册资本
        /// </summary>
        public string registermoney { get; set; }
        /// <summary>
        /// 成立日期
        /// </summary>
        public string setdate { get; set; }
        /// <summary>
        /// 登记机关
        /// </summary>
        public string registerarea { get; set; }
        /// <summary>
        /// 营业期限
        /// </summary>
        public string businessdate { get; set; }
    }
    /// <summary>
    /// Odoo  获取公司联系人信息接口返回值
    /// </summary>
    public class OdooCompanyPersonInfoAPIResult
    {
        public string message { get; set; }
        public List<CompanyPersonInfoOdoo> result { get; set; }
        public bool success { get; set; }
    }
    /// <summary>
    /// 解析公司联系人信息
    /// </summary>
    public class CompanyPersonInfoOdoo
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// 称谓
        /// </summary>
        public string appellation { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public string function { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 固话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string comment { get; set; }
    }
    /// <summary>
    /// Odoo  获取客户授信信息接口返回值
    /// </summary>
    public class OdooClientCreditInfoAPIResult
    {
        public string message { get; set; }
        public List<GetKeHuCreditListOdoo> result { get; set; }
        public bool success { get; set; }
    }
    /// <summary>
    /// 解析客户授信信息
    /// </summary>
    public class GetKeHuCreditListOdoo
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string clientID { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string controller { get; set; }
        /// <summary>
        /// 授信额度
        /// </summary>
        public string credit_limit { get; set; }
        /// <summary>
        /// 授信状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 是否绑定销售
        /// </summary>
        public string isBindSales { get; set; }
    }
}
