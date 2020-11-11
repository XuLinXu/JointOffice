using JointOffice.DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public interface ISaleRiQing
    {
        /// <summary>
        /// 获取当前用户职位信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<LoginUserInfor> GetLoginUserInfor(Job para);
        /// <summary>
        /// 获取日清列表
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<RiQingDayPlanList_Result> GetDayPlanListData(RiQingDayPlanListSeachPara para);
        /// <summary>
        /// 获取日清详情
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<DayPlanChaXunMXResult> GetDayPlanChaXunMX(DayPlanChaXunMX para);
        /// <summary>
        /// 获取日清总览
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<RiQingZongLanXinXiResult> GetRiQingZongLanXinXi(RiQingZongLanXinXiPara para);
        /// <summary>
        /// 获取日计划临时项
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<DayPlanChaXunMXResult> GetDayPlanLinShiXiangByPerson(DayPlanLinShiXiangByPersonInPara para);
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<GetAllKeHuXinXiForSelect_Res> GetAllKeHuXinXiForSelect(GetAllKeHuXinXiForSelect_Para para);
		/// <summary>
		/// 获取拜访目的
		/// </summary>
		/// <returns></returns>
		Showapi_Res_List<GetBaiFangMuDi_Res> GetBaiFangMuDi(Job job);
		/// <summary>
		/// 获取费用类别
		/// </summary>
		/// <returns></returns>
		Showapi_Res_List<GetFeiYongLeiBie_Res> GetFeiYongLeiBie(Job job);
        /// <summary>
        /// 获取全部客户分类
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<KeHuFenLei_Res> GetAllKeHuFenLei(Job para);
        /// <summary>
        /// 获取全部地区分类
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<DiQuFenLei_Res> GetAllDiQuFenLei(Job para);
        /// <summary>
        /// 日清填写
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge SaveRiQingDayPlan(SaveRiQingPara para);
        /// <summary>
        /// 日清审核
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge RiQingReview(RiQingReviewPara para);
        /// <summary>
        /// 日清签到列表
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<QianDaoTypeResult> GetQianDaoType(Job para);
        /// <summary>
        /// 日清签到列表
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<RiQingQianDaoListResult> GetRiQingQianDaoList(RiQingQianDaoListInPara para);
        /// <summary>
        /// 日清签到
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge AddOneQianDao(AddOneQianDaoInPara para);
        /// <summary>
        /// 日清签到统计
        /// </summary>
        Showapi_Res_List<QianDaoTongJi> GetQianDaoTongJiList(QianDaoTongJiInPara para);
        /// <summary>
        /// 签到统计  数据tab页
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<QianDaoTongJiData> GetQianDaoTongJiDataList(QianDaoTongJiInPara para);
        /// <summary>
        /// 获取SMS系统全部人员信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<AllPersonList> GetAllPersonList(Job para);
        /// <summary>
        /// 月计划填写
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge AddRiQingMonthPlan(AddRiQingMonthPlan_Para para);
		/// <summary>
		/// 保存周总结
		/// </summary>
		/// <returns></returns>
		Showapi_Res_Single<AddRiQingPlan_Res> AddWeekPlanZongJie(AddWeekPlanZongJie_Para para);
		/// <summary>
		/// 保存月总结
		/// </summary>
		/// <returns></returns>
		Showapi_Res_Single<AddRiQingPlan_Res> AddMonthPlanZongJie(AddMonthPlanZongJie_Para para);
	}
    public class Job
    {
        public string job { get; set; }
    }
    /// <summary>
    /// 获取日清列表  入参
    /// </summary>
    /// <returns></returns>
    public class RiQingDayPlanListSeachPara
    {
        public string ChanPinXian { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CreateJsDate { get; set; }
        public string CreatePerson { get; set; }
        public string DepCode { get; set; }
        /// <summary>
        /// ""全部  1已审核  0未审核
        /// </summary>
        public string ShenPiZhuangTai { get; set; }
        /// <summary>
        /// 分页信息和排序信息
        /// </summary>
        public PageingRequestParameter PageingRequestParameter { get; set; }
        public string job { get; set; }
        public string searchBody { get; set; }
    }
    /// <summary>
    /// 日清列表  分页信息和排序信息
    /// </summary>
    public class PageingRequestParameter
    {
        public int UpPageIndex { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int NewPageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
        public bool ClosePageing { get; set; }
        /// <summary>
        /// 分页信息
        /// </summary>
        public List<SortUnity> SortUnities { get; set; }
        //int DataItemCount { get; set; }
        //bool EnableSort { get; set; }
        //public List<SeachUnity> SeachUnities { get; set; }
        //public string SortDirection { get; set; }
        //public string SortExpression { get; set; }
    }
    /// <summary>
    /// 日清列表  分页信息
    /// </summary>
    public class SortUnity
    {
        public string SortKey { get; set; }
        public int SortDirection { get; set; }
        public string SortDescript { get; set; }
    }
    /// <summary>
    /// 获取日清列表  出参
    /// </summary>
    /// <returns></returns>
    public class RiQingDayPlanList_Result
    {
        public string ChanPinXian { get; set; }
        public string createcode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatePerson { get; set; }
        public string DepCode { get; set; }
        public string DepName { get; set; }
        public string ID { get; set; }
        public string JobID { get; set; }
        public string JobName { get; set; }
        public DateTime? ShenHeDate { get; set; }
        public string ShenHePerson { get; set; }
        public string ShenHePingJia { get; set; }
        public string ShenHeZhuangTaiEx { get; set; }
        public string zhangtaoId { get; set; }
    }
    /// <summary>
    /// 获取当前用户职位信息  出参
    /// </summary>
    /// <returns></returns>
    public class LoginUserInfor
    {
        public UserSource UserSource { get; set; }
        public List<JosSet> jobSet { get; set; }
    }
    public class UserSource
    {
        public string 人员编码 { get; set; }
        public string 姓名 { get; set; }
        public string 所属大区 { get; set; }
        public string 岗位类型编码 { get; set; }
        public string 岗位类型名称 { get; set; }
        public string 总部名称 { get; set; }
        public string 总部编码 { get; set; }
        public string 状态 { get; set; }
        public string 产品线名称 { get; set; }
        public string 产品线编码 { get; set; }
    }
    public class JosSet
    {
        public string 登录账号 { get; set; }
        public string 登录账号状态 { get; set; }
        public string 组织编码 { get; set; }
        public string 组织名称 { get; set; }
        public string 组织描述 { get; set; }
        public string 上级职位编码 { get; set; }
        public string 职位编码 { get; set; }
        public string 职位名称 { get; set; }
        public string 职位描述 { get; set; }
        public string 角色编码 { get; set; }
        public string 角色名称 { get; set; }
        public string 角色有效标志 { get; set; }
        public string 所属工厂 { get; set; }
        public string 账套名称 { get; set; }
	}
	/// <summary>
	/// 获取客户列表  出参
	/// </summary>
	/// <returns></returns>
	public class GetAllKeHuXinXiForSelect_Res
	{
		public int TID { get; set; }
		public string 客户编码 { get; set; }
		public string 客户全称 { get; set; }
		public string 客户简称 { get; set; }
		public string 客户类别编码 { get; set; }
		public string 客户类别 { get; set; }
		public string 地区编码 { get; set; }
		public string 地区名称 { get; set; }
		public string 客户地址 { get; set; }
		public string 发货地址 { get; set; }
		public string 客户银行 { get; set; }
		public string 客户账户 { get; set; }
	}
	/// <summary>
	/// 获取客户列表  入参
	/// </summary>
	/// <returns></returns>
	public class GetAllKeHuXinXiForSelect_Para
	{
		public string KehuDiqu { get; set; }
		public string KehuFullName { get; set; }
		public string KehuHangYeLeiBie { get; set; }
		public PageingRequestParameter PageingRequestParameter { get; set; }
		public string job { get; set; }
	}
	/// <summary>
	/// 获取拜访目的  出参
	/// </summary>
	/// <returns></returns>
	public class GetBaiFangMuDi_Res
	{
		public int ID { get; set; }
		public string ValueSetCode { get; set; }
		public string ValueSetName { get; set; }
		public string Value { get; set; }
		public string ValueMeaning { get; set; }
		public string Delete_flag { get; set; }
		public string Active_flag { get; set; }
		public string VID { get; set; }
	}
	/// <summary>
	/// 获取费用类别  出参
	/// </summary>
	/// <returns></returns>
	public class GetFeiYongLeiBie_Res
	{
		public int ID { get; set; }
		public string ValueSetCode { get; set; }
		public string ValueSetName { get; set; }
		public string Value { get; set; }
		public string ValueMeaning { get; set; }
		public string Delete_flag { get; set; }
		public string Active_flag { get; set; }
		public string VID { get; set; }
	}
    /// <summary>
    /// 日清详情  入参
    /// </summary>
    public class DayPlanChaXunMX
    {
        public string id { get; set; } = "";
        public string idSec { get; set; } = "";
        public string idThi { get; set; } = "";
        public string idFour { get; set; } = "";
        public string idFive { get; set; } = "";
        public int intid { get; set; } = 0;
        public string dateid { get; set; } = DateTime.Now.ToString();
        public string job { get; set; } = "";
    }
    /// <summary>
    /// 日清详情  出参
    /// </summary>
    public class DayPlanChaXunMXResult
    {
        public List<GongZuoXiangMu> gongZuoXiangMu { get; set; }
        public List<LinShiXiangMu> linShiXiangMu { get; set; }
        public List<FeiYongXinXi> feiYongXinXi { get; set; }
        public List<RiQingGuanLi> riQingGuanLi { get; set; }
        public List<QianDaoXinXi> qianDaoXinXi { get; set; }
        public RiQingXinXi RiQingXinXi { get; set; }
        public string ZhiWuName { get; set; }
        public string LoginName { get; set; }
        public string TiXing { get; set; }
        public string CreatePerson { get; set; }
    }
    /// <summary>
    /// 日清详情  工作项目
    /// </summary>
    public class GongZuoXiangMu
    {
        public int xuhao { get; set; }
        public string ID { get; set; }
        public string DayPlanID { get; set; }
        public string MonthPlanID { get; set; }
        public string MonthPlanItem { get; set; }
        public string DayPlanContent { get; set; }
        public string DayChaYifenXi { get; set; }
        public string DayFactContent { get; set; }
        public string DayXiaBuJiHua { get; set; }
    }
    /// <summary>
    /// 日清详情  临时项目
    /// </summary>
    public class LinShiXiangMu
    {
        public int xuhao { get; set; }
        public string ID { get; set; }
        public string DayPlanID { get; set; }
        public string LinShiXiangMuName { get; set; }
        public string LinShiXiangMuDayPlan { get; set; }
        public string LinShiXiangMuDayFact { get; set; }
        public string ChaYiFenXi { get; set; }
        public string XiaBuJIHuaContent { get; set; }
    }
    /// <summary>
    /// 日清详情  费用明细
    /// </summary>
    public class FeiYongXinXi
    {
        public int xuhao { get; set; }
        public string ID { get; set; }
        public string DayPlanID { get; set; }
        public string FeiYongLeiBiebm { get; set; }
        public string FeiYongLeiBiemc { get; set; }
        public string FeiYongKeHuCode { get; set; }
        public string FeiYongKeHuName { get; set; }
        public string LianXiRenName { get; set; }
        public string LianXiRenCode { get; set; }
        public decimal FeiYongJine { get; set; }
        public string BeiZhu { get; set; }
    }
    /// <summary>
    /// 日清详情  日清管理
    /// </summary>
    public class RiQingGuanLi
    {
        public int xuhao { get; set; }
        public string ID { get; set; }
        public string DayPlanID { get; set; }
        public string ckeHuCode { get; set; }
        public string cKeHuMingCheng { get; set; }
        public string cKeHuAddress { get; set; }
        public string cBaiFangMuDi
        {
            get
            {
                return BaiFangMuDiBiaoMa;
            }
        }
        public string BaiFangMuDiBiaoMa { get; set; }
        public string BaiFangMuDiMingCheng { get; set; }
        public string cGongZuoXiaoJie { get; set; }
        public string cBeiZhu { get; set; }
        public string distance { get; set; }
        public string QianDaoTime { get; set; }
    }
    /// <summary>
    /// 日清详情  签到信息
    /// </summary>
    public class QianDaoXinXi
    {
        public string QianDaoTime { get; set; }
        public string QianDaoTimeApp { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string JingDu { get; set; }
        public string WeiDu { get; set; }
        public string Address { get; set; }
        public string 签到类型 { get; set; }
        public string 是否在工厂 { get; set; }
        public string 工厂名称 { get; set; }
        public string 市区 { get; set; }
        public string 备注 { get; set; }
        public string 客户编码 { get; set; }
        public string 客户名称 { get; set; }
    }
    /// <summary>
    /// 日清详情  日清信息
    /// </summary>
    public class RiQingXinXi
    {
        public string ID { get; set; }
        public string ChanPinXian { get; set; }
        public string DepCode { get; set; }
        public string DepName { get; set; }
        public string CreatePerson { get; set; }
        public string CreatePersonName { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public string ModifyTime { get; set; }
        public string ShenHeZhuangTai { get; set; }
        public string ShenHePingJia { get; set; }
        public string ShenHePiYu { get; set; }
        public string ShenHePerson { get; set; }
        public string ShenHeDate { get; set; }
        public string JobID { get; set; }
        public string job_Name { get; set; }
        public string ZhangTao_Id { get; set; }
    }
    /// <summary>
    /// 日清总览入参
    /// </summary>
    public class RiQingZongLanXinXiPara
    {
        public string bumen { get; set; }
        public string jieshushijian { get; set; }
        public string kaishishijian { get; set; }
        public string renyuanxingming { get; set; }
        public string userID { get; set; }
        public string job { get; set; }
        public PageingRequestParameter PageingRequestParmeter { get; set; }
    }
    /// <summary>
    /// 日清总览出参
    /// </summary>
    public class RiQingZongLanXinXiResult
    {
        public string TID { get; set; }
        public string ID { get; set; }
        public string 部门编号 { get; set; }
        public string 部门名称 { get; set; }
        public string JobID { get; set; }
        public string CreatePerson { get; set; }
        public string 姓名 { get; set; }
        public string 计划月份 { get; set; }
        public string 工作项目 { get; set; }
        public string 项目类别 { get; set; }
        public string 工作项目内容 { get; set; }
        public string 临时项目内容 { get; set; }
        public string 日清ID { get; set; }
        public string 人员Job { get; set; }
        public string 职位名称 { get; set; }
        public string zhangTaoid { get; set; }
        public string 人员编码 { get; set; }
        public string 日清日期 { get; set; }
        public string 临时项目ID { get; set; }
        public string 日清ID2 { get; set; }
        public string 客情维护措施 { get; set; }
        public string 客户名称 { get; set; }
        public string 技术交流情况 { get; set; }
        public string 旧钢芯回收情况 { get; set; }
        public string 竞争对手信息 { get; set; }
        public string 客户投诉内容 { get; set; }
        public string 个人小结 { get; set; }
        public string 其他 { get; set; }
        public string 贷款情况 { get; set; }
        public string tbMiaoShu { get; set; }
        public string tbSheBeiXinXi { get; set; }
        public string 签到信息 { get; set; }
        public string 费用明细 { get; set; }
        public string 日清管理 { get; set; }
    }
    /// <summary>
    /// 全部客户分类  出参
    /// </summary>
    /// <returns></returns>
    public class KeHuFenLei_Res
    {
        public string cCCCode { get; set; }
        public string cCCName { get; set; }
        public string iCCGrade { get; set; }
        public string bCCEnd { get; set; }
    }
    /// <summary>
    /// 全部地区分类  出参
    /// </summary>
    /// <returns></returns>
    public class DiQuFenLei_Res
    {
        public string cCCCode { get; set; }
        public string cCCName { get; set; }
        public string iCCGrade { get; set; }
    }
    /// <summary>
    /// 日清填写
    /// </summary>
    public class SaveRiQingPara
    {
        public string ChanPinXian { get; set; } = "";
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string CreatePerson { get; set; } = "";
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string DepCode { get; set; } = "";
        public string ID { get; set; } = "";
        public string JobID { get; set; } = "";
        public string ModifyPerson { get; set; } = "";
        public DateTime ModifyTime { get; set; } = DateTime.Now;
        public DateTime ShenHeDate { get; set; } = DateTime.Now;
        public string ShenHePerson { get; set; } = "";
        public string ShenHePingJia { get; set; } = "";
        public string ShenHePiYu { get; set; } = "";
        public int ShenHeZhuangTai { get; set; } = 0;
        public string Job { get; set; } = "";
        public List<RiQing_FeiYongMingXi> RiQing_DayPlan_FeiYongMingXi { get; set; }
        public List<RiQing_LinShiXiangMu> RiQing_DayPlan_LinShiXiangMu { get; set; }
        public List<RiQing_MonthPlanXiangMu> RiQing_DayPlan_MonthPlanXiangMu { get; set; }
        public List<RiQing_QiTaNeiRong> RiQing_DayPlan_QiTaNeiRong { get; set; }
        public List<RiQing_RiQingGuanLi> RiQing_RiQingGuanLi { get; set; }
    }
    /// <summary>
    /// 日清填写  费用明细
    /// </summary>
    public class RiQing_FeiYongMingXi
    {
        public int xuhao { get; set; } = 0;
        public string BeiZhu { get; set; } = "";
        public string CreatePerson { get; set; } = "";
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string DayPlanID { get; set; } = "";
        public string FeiYongLeiBie { get; set; } = "";
        public string FeiYongLeiBiebm { get; set; } = "";
        public string FeiYongLeiBiemc { get; set; } = "";
        public string ID { get; set; } = "";
        public decimal JinE { get; set; } = 0;
        public decimal FeiYongJine { get; set; } = 0;
        public string ModifyPerson { get; set; } = "";
        public DateTime ModifyTime { get; set; } = DateTime.Now;
    }
    /// <summary>
    /// 日清填写  临时项目
    /// </summary>
    public class RiQing_LinShiXiangMu
    {
        public int xuhao { get; set; } = 0;
        public string ChaYiFenXi { get; set; } = "";
        public string CreatePerson { get; set; } = "";
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string DayPlanID { get; set; } = "";
        public string ID { get; set; } = "";
        public string LinShiXiangMuDayFact { get; set; } = "";
        public string LinShiXiangMuDayPlan { get; set; } = "";
        public string LinShiXiangMuName { get; set; } = "";
        public string MidifyPerson { get; set; } = "";
        public DateTime MidifyTime { get; set; } = DateTime.Now;
        public string XiaBuJIHuaContent { get; set; } = "";
        public DateTime XiaBuJiHuaDate { get; set; } = DateTime.Now;
    }
    /// <summary>
    /// 日清填写  月计划项目
    /// </summary>
    public class RiQing_MonthPlanXiangMu
    {
        public string ChaYiFenXiJieJueCuoShi { get; set; } = "";
        public string CreatePerson { get; set; } = "";
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string DayFactContent { get; set; } = "";
        public string DayPlanContent { get; set; } = "";
        public string DayPlanID { get; set; } = "";
        public string ID { get; set; } = "";
        public string ModifyPerson { get; set; } = "";
        public DateTime ModifyTime { get; set; } = DateTime.Now;
        public string MonthPlanID { get; set; } = "";
        public string NextPlanContent { get; set; } = "";
        public DateTime NextPlanDate { get; set; } = DateTime.Now;
        public string MonthPlanItem { get; set; } = "";
    }
    /// <summary>
    /// 日清填写  其他内容
    /// </summary>
    public class RiQing_QiTaNeiRong
    {

    }
    /// <summary>
    /// 日清填写  日清管理
    /// </summary>
    public class RiQing_RiQingGuanLi
    {
        public int xuhao { get; set; } = 0;
        public string cBaiFangMuDi { get; set; } = "";
        public string BaiFangMuDiBiaoMa { get; set; } = "";
        public string BaiFangMuDiMingCheng { get; set; } = "";
        public string cBeiZhu { get; set; } = "";
        public string cCreatePerson { get; set; } = "";
        public string cGongZuoXiaoJie { get; set; } = "";
        public string cKeHuAddress { get; set; } = "";
        public string ckeHuCode { get; set; } = "";
        public string cKeHuMingCheng { get; set; } = "";
        public string DayPlanID { get; set; } = "";
        public DateTime dCreateDate { get; set; } = DateTime.Now;
        public string ID { get; set; } = "";
        public DateTime ModifyDate { get; set; } = DateTime.Now;
        public string ModifyPerson { get; set; } = "";
    }
    /// <summary>
    /// 获取日计划临时项  入参
    /// </summary>
    /// <returns></returns>
    public class DayPlanLinShiXiangByPersonInPara
    {
        public string job { get; set; }
        public string date { get; set; }
    }
    public class SMSLogin
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
    }
    public class MessageSuccess
    {
        public bool ChengGong { get; set; }
        public string Message { get; set; }
    }
    /// <summary>
    /// 日清审核入参
    /// </summary>
    public class RiQingReviewPara
    {
        /// <summary>
        /// 对方的人员编码  0000100058
        /// </summary>
        public string CreateCode { get; set; } = "";
        public string CreateRiQi { get; set; } = DateTime.Now.ToString();
        public string DayPlanID { get; set; } = "";
        /// <summary>
        /// 对方的jobid
        /// </summary>
        public string RqJobID { get; set; } = "";
        public DateTime ShenHeDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 自己的人员编码
        /// </summary>
        public string ShenHePerson { get; set; } = "";
        public string ShenHePingJia { get; set; } = "";
        public string ShenHePiYu { get; set; } = "";
        public int ShenHeZhuangTai { get; set; } = 0;
        /// <summary>
        /// 对方的所属工厂
        /// </summary>
        public string ZhangTou_id { get; set; } = "";
        public string job { get; set; } = "";
    }
    /// <summary>
    /// 日清签到类别出参
    /// </summary>
    public class QianDaoTypeResult
    {
        public int ID { get; set; }
        public string ValueSetCode { get; set; }
        public string ValueSetName { get; set; }
        public string Value { get; set; }
        public string ValueMeaning { get; set; }
        public string Delete_flag { get; set; }
        public string Active_flag { get; set; }
        public string VID { get; set; }
    }
    /// <summary>
    /// 日清签到列表入参
    /// </summary>
    public class RiQingQianDaoListInPara
    {
        public string BuMen { get; set; }
        public DateTime QianDaoDate1 { get; set; }
        public DateTime QianDaoDate2 { get; set; }
        public string XingMing { get; set; }
        public string XingMingName { get; set; }
        public string ZhiWu { get; set; }
        public string job { get; set; }
        public string JobID { get; set; }
        public PageingRequestParameter PageingRequestParameter { get; set; }
    }
    /// <summary>
    /// 日清签到列表出参
    /// </summary>
    public class RiQingQianDaoListResult
    {
        public string 部门 { get; set; }
        public string 职务 { get; set; }
        public string 姓名 { get; set; }
        public string 签到时间 { get; set; }
        public string 签到时间App { get; set; }
        public string 经度 { get; set; }
        public string 纬度 { get; set; }
        public string 地址 { get; set; }
        public string JobID { get; set; }
        public string 签到类型 { get; set; }
        public string 是否在工厂 { get; set; }
        public string 工厂名称 { get; set; }
        public string 备注 { get; set; }
        public string 客户编码 { get; set; }
        public string 客户名称 { get; set; }
    }
    /// <summary>
    /// 日清列表筛选获取人员入参
    /// </summary>
    public class GetPersonInfoListPara
    {
        public string PersonCode { get; set; }
        public string PersonName { get; set; }
        public string job { get; set; }
        public PageingRequestParameter PageingRequestParameter { get; set; }
    }
    /// <summary>
    /// 日清列表筛选获取人员出参
    /// </summary>
    public class GetPersonInfoListResult
    {
        public string PersonCode { get; set; }
        public string PersonName { get; set; }
        public int TID { get; set; }
    }
    /// <summary>
    /// 日清签到  入参
    /// </summary>
    /// <returns></returns>
    public class AddOneQianDaoInPara
    {
        public string Address { get; set; } = "";
        public string area { get; set; } = "";
        public string city { get; set; } = "";
        public decimal? distance { get; set; } = 0;
        public string GongChangCoordinate { get; set; } = "";
        public string GongChangName { get; set; } = "";
        public int Id { get; set; } = 0;
        public bool? isGongChang { get; set; } = false;
        public string JingDu { get; set; } = "";
        public string local { get; set; } = "";
        public string province { get; set; } = "";
        public DateTime QianDaoTime { get; set; } = DateTime.Now;
        public string QianDaoType { get; set; } = "";
        public string UserId { get; set; } = "";
        public string WeiDu { get; set; } = "";
        public string job { get; set; } = "";
        public string KeHuCode { get; set; } = "";
        public string Remark { get; set; } = "";
        public string UserJob { get; set; } = "";
    }
    /// <summary>
    /// 日清签到统计
    /// </summary>
    public class QianDaoTongJi
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string userJob { get; set; }
        public string jobName { get; set; }
        public string organName { get; set; }
        public int count { get; set; }
    }
    /// <summary>
    /// 日清签到统计  入参
    /// </summary>
    public class QianDaoTongJiInPara
    {
        public DateTime time1 { get; set; }
        public DateTime time2 { get; set; }
        public string userList { get; set; }
        public PageingRequestParameter PageingRequestParameter { get; set; }
        public string job { get; set; }
    }
    /// <summary>
    /// 签到统计  数据tab页
    /// </summary>
    /// <returns></returns>
    public class QianDaoTongJiData
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string userJob { get; set; }
        public string jobName { get; set; }
        public string organName { get; set; }
        public int count { get; set; }
        public List<QianDaoTongJiDataList> qianDaoTongJiDataList { get; set; }
    }
    public class QianDaoTongJiDataList
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string userJob { get; set; }
        public int count { get; set; }
        public string qiandaotype { get; set; }
        public string qiandaotypecount { get; set; }
    }
    /// <summary>
    /// 获取SMS系统全部人员信息
    /// </summary>
    /// <returns></returns>
    public class AllPersonList
    {
        public string userId { get; set; }
        public string userName { get; set; }
    }
	/// <summary>
	/// 月计划填写 入参
	/// </summary>
	/// <returns></returns>
	public class AddRiQingMonthPlan_Para
	{
        public string AppMonthTarget { get; set; } = "";
        public string ID { get; set; } = "";
        public DateTime YearMonth { get; set; } = DateTime.Now;
        public string WorkPlanContent { get; set; } = "";
        public string WorkPlanClass { get; set; } = "";
        public string MonthTarget { get; set; } = "";
        public string ChanPinXian { get; set; } = "";
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime CreateTime { get; set; } = DateTime.Now;
		public string CreatePerson { get; set; } = "";
        public string JobID { get; set; } = "";
        public string depCode { get; set; } = "";
        public string ModifyPerson { get; set; } = "";
        public DateTime ModifyTime { get; set; } = DateTime.Now;
		public List<PlanRes> PlanRes { get; set; }
		public string job { get; set; }
	}
	public class PlanRes
	{
		public string key { get; set; }
		public string value { get; set; }
	}
	/// <summary>
	/// 月计划填写 保存周总结 保存月总结 出参
	/// </summary>
	/// <returns></returns>
	public class AddRiQingPlan_Res
	{
		public Boolean ChengGong { get; set; }
		public string Message { get; set; }
	}
	/// <summary>
	/// 保存周总结 入参
	/// </summary>
	/// <returns></returns>
	public class AddWeekPlanZongJie_Para
	{
		public List<YueJiHuaList> YueJiHuaResList { get; set; }
		public string job { get; set; }
	}
	/// <summary>
	/// 保存月总结 入参
	/// </summary>
	/// <returns></returns>
	public class AddMonthPlanZongJie_Para
	{
		public List<YueJiHuaList> yueJiHuaZongJie { get; set; }
		public string job { get; set; }
	}
	public class YueJiHuaList
	{
		public string ID { get; set; }
		public string YearMonth { get; set; }
		public string WorkPlanContent { get; set; }
		public string WorkPlanClass { get; set; }
		public string MonthTarget { get; set; }
		public string AppMonthTarget { get; set; }
		public string Problem { get; set; }
		public string SolveProblemMode { get; set; }
		public string FirstWeekPlan { get; set; }
		public string FirstWeekShiJi { get; set; }
		public string FirstWeekChaYi { get; set; }
		public string FirstWeekWenTi { get; set; }
		public string FirstWeekZongJie { get; set; }
		public string FirstWeekZiPing { get; set; }
		public string SeconDWeekPlan { get; set; }
		public string SeconDWeekShiJi { get; set; }
		public string SeconDWeekChaYi { get; set; }
		public string SeconDWeekWenTi { get; set; }
		public string SeconDWeekZongJie { get; set; }
		public string SeconDWeekZiPing { get; set; }
		public string ThirdWeekPlan { get; set; }
		public string ThirdWeekShiJi { get; set; }
		public string ThirdWeekChaYi { get; set; }
		public string ThirdWeekWenTi { get; set; }
		public string ThirdWeekZongJie { get; set; }
		public string ThirdWeekZiPing { get; set; }
		public string FourthWeekPlan { get; set; }
		public string FourthWeekShiJi { get; set; }
		public string FourthWeekChaYi { get; set; }
		public string FourthWeekWenTi { get; set; }
		public string FourthWeekZongJie { get; set; }
		public string FourthWeekZiPing { get; set; }
		public string FifthWeekPlan { get; set; }
		public string ChanPinXian { get; set; }
		public string CreateDate { get; set; }
		public string CreateTime { get; set; }
		public string CreatePerson { get; set; }
		public string CreatePersonName { get; set; }
		public string depCode { get; set; }
		public string depName { get; set; }
		public string JobID { get; set; }
		public string JobName { get; set; }
		public string ModifyPerson { get; set; }
		public string ModifyTime { get; set; }
		public string ZongJieDate { get; set; }
		public string ZongJieZiPingClass { get; set; }
		public string ZongJieShiJi { get; set; }
		public string ZongJieChaYi { get; set; }
		public string ZongJieProblem { get; set; }
		public string ZongJieZongJie { get; set; }
		public string PingJiaDate { get; set; }
		public string PingJiaClass { get; set; }
		public string PingJiaPingYu { get; set; }
		public string PingJiaPerson { get; set; }
		public string PingJiaPersonName { get; set; }
		public string APPCreate { get; set; }
	}
}
