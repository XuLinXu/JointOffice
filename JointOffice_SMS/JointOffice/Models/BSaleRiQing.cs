using Dapper;
using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public class BSaleRiQing : ISaleRiQing
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        string SMSUrl;
        private readonly IPrincipalBase _PrincipalBase;
        IMemoryCache _memoryCache;
        public BSaleRiQing(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase, IMemoryCache memoryCache)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
            SMSUrl = this.config.Value.ConnectionStrings.SMSUrl;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// 获取当前用户职位信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<LoginUserInfor> GetLoginUserInfor(Job para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<LoginUserInfor>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<LoginUserInfor> res = new Showapi_Res_Single<LoginUserInfor>();
            LoginUserInfor LoginUserInfor = new LoginUserInfor();
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }
            LoginUserInfor = SaleRiQingToSMS.GetAsynctMethod<LoginUserInfor>(SMSUrl + "api/UsersResource/GetLoginUserInfor", para.job, smsToken.Token);
            res.showapi_res_body = LoginUserInfor;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 获取日清列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<RiQingDayPlanList_Result> GetDayPlanListData(RiQingDayPlanListSeachPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<RiQingDayPlanList_Result>();
                return Return.Return();
            }
            Showapi_Res_List<RiQingDayPlanList_Result> res = new Showapi_Res_List<RiQingDayPlanList_Result>();
            List<RiQingDayPlanList_Result> list = new List<RiQingDayPlanList_Result>();
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(para);
            list = SaleRiQingToSMS.PostAsynctMethod<List<RiQingDayPlanList_Result>>(SMSUrl + "api/RiQing/GetDayPlanListData", str, para.job, smsToken.Token);
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<RiQingDayPlanList_Result>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 获取日清详情
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<DayPlanChaXunMXResult> GetDayPlanChaXunMX(DayPlanChaXunMX para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<DayPlanChaXunMXResult>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<DayPlanChaXunMXResult> res = new Showapi_Res_Single<DayPlanChaXunMXResult>();
            DayPlanChaXunMXResult DayPlanChaXunMXResult = new DayPlanChaXunMXResult();
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }

            DayPlanChaXunMX DayPlanChaXunMX = new DayPlanChaXunMX();
            DayPlanChaXunMX.idSec = para.idSec;
            DayPlanChaXunMX.dateid = para.dateid;

            var str = Newtonsoft.Json.JsonConvert.SerializeObject(DayPlanChaXunMX);
            DayPlanChaXunMXResult = SaleRiQingToSMS.PostAsynctMethod<DayPlanChaXunMXResult>(SMSUrl + "api/RiQing/GetDayPlanChaXunMX", str, para.job, smsToken.Token);
            res.showapi_res_code = "200";
            res.showapi_res_body = DayPlanChaXunMXResult;
            return res;
        }
        /// <summary>
        /// 获取日清总览
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<RiQingZongLanXinXiResult> GetRiQingZongLanXinXi(RiQingZongLanXinXiPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<RiQingZongLanXinXiResult>();
                return Return.Return();
            }
            Showapi_Res_List<RiQingZongLanXinXiResult> res = new Showapi_Res_List<RiQingZongLanXinXiResult>();
            List<RiQingZongLanXinXiResult> RiQingZongLanXinXiResult = new List<RiQingZongLanXinXiResult>();
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(para);
            RiQingZongLanXinXiResult = SaleRiQingToSMS.PostAsynctMethod<List<RiQingZongLanXinXiResult>>(SMSUrl + "api/RiQing/GetRiQingZongLanXinXi", str, para.job, smsToken.Token);
            res.showapi_res_body = new Showapi_res_body_list<RiQingZongLanXinXiResult>();
            res.showapi_res_body.contentlist = RiQingZongLanXinXiResult;
            res.showapi_res_code = "200";

            return res;
        }
        /// <summary>
        /// 获取日计划临时项
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<DayPlanChaXunMXResult> GetDayPlanLinShiXiangByPerson(DayPlanLinShiXiangByPersonInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnSingle<DayPlanChaXunMXResult>();
                return Return.Return();
            }
            Showapi_Res_Single<DayPlanChaXunMXResult> res = new Showapi_Res_Single<DayPlanChaXunMXResult>();
            DayPlanChaXunMXResult DayPlanChaXunMXResult = new DayPlanChaXunMXResult();
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(para);
            DayPlanChaXunMXResult = SaleRiQingToSMS.PostAsynctMethod<DayPlanChaXunMXResult>(SMSUrl + "api/RiQing/GetDayPlanLinShiXiangByPersonPost", str, para.job, smsToken.Token);
            res.showapi_res_body = DayPlanChaXunMXResult;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<GetAllKeHuXinXiForSelect_Res> GetAllKeHuXinXiForSelect(GetAllKeHuXinXiForSelect_Para para)
		{
			var memberid = _PrincipalBase.GetMemberId();
			if (memberid == null || memberid == "")
			{
				var Return = new ReturnList<GetAllKeHuXinXiForSelect_Res>();
				return Return.Return();
			}
			Showapi_Res_List<GetAllKeHuXinXiForSelect_Res> res = new Showapi_Res_List<GetAllKeHuXinXiForSelect_Res>();
			List<GetAllKeHuXinXiForSelect_Res> GetAllKeHuXinXiForSelect_Res = new List<GetAllKeHuXinXiForSelect_Res>();
			var str = Newtonsoft.Json.JsonConvert.SerializeObject(para);
			var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
			if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
			{
				throw new BusinessTureException("获取SMSToken失败");
			}
			GetAllKeHuXinXiForSelect_Res = SaleRiQingToSMS.PostAsynctMethod<List<GetAllKeHuXinXiForSelect_Res>>(SMSUrl + "api/CustomerSelect/GetAllKeHuXinXiForSelect", str, para.job, smsToken.Token);
			res.showapi_res_code = "200";
			res.showapi_res_body = new Showapi_res_body_list<GetAllKeHuXinXiForSelect_Res>();
			res.showapi_res_body.contentlist = GetAllKeHuXinXiForSelect_Res;
			return res;
		}
		/// <summary>
		/// 获取拜访目的
		/// </summary>
		/// <returns></returns>
		public Showapi_Res_List<GetBaiFangMuDi_Res> GetBaiFangMuDi(Job para)
		{
			var memberid = _PrincipalBase.GetMemberId();
			if (memberid == null || memberid == "")
			{
				var Return = new ReturnList<GetBaiFangMuDi_Res>();
				return Return.Return();
			}
			Showapi_Res_List<GetBaiFangMuDi_Res> res = new Showapi_Res_List<GetBaiFangMuDi_Res>();
			List<GetBaiFangMuDi_Res> GetBaiFangMuDi_Res = new List<GetBaiFangMuDi_Res>();
			var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
			if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
			{
				throw new BusinessTureException("获取SMSToken失败");
			}
			GetBaiFangMuDi_Res = SaleRiQingToSMS.GetAsynctMethod<List<GetBaiFangMuDi_Res>>(SMSUrl + "api/RiQing/GetBaiFangMuDi", para.job, smsToken.Token);
			res.showapi_res_body = new Showapi_res_body_list<GetBaiFangMuDi_Res>();
			res.showapi_res_body.contentlist = GetBaiFangMuDi_Res;
			res.showapi_res_code = "200";
			return res;
		}
		/// <summary>
		/// 获取费用类别
		/// </summary>
		/// <returns></returns>
		public Showapi_Res_List<GetFeiYongLeiBie_Res> GetFeiYongLeiBie(Job para)
		{
			var memberid = _PrincipalBase.GetMemberId();
			if (memberid == null || memberid == "")
			{
				var Return = new ReturnList<GetFeiYongLeiBie_Res>();
				return Return.Return();
			}
			Showapi_Res_List<GetFeiYongLeiBie_Res> res = new Showapi_Res_List<GetFeiYongLeiBie_Res>();
			List<GetFeiYongLeiBie_Res> GetFeiYongLeiBie_Res = new List<GetFeiYongLeiBie_Res>();
			var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
			if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
			{
				throw new BusinessTureException("获取SMSToken失败");
			}
			GetFeiYongLeiBie_Res = SaleRiQingToSMS.GetAsynctMethod<List<GetFeiYongLeiBie_Res>>(SMSUrl + "api/RiQing/GetFeiYongLeiBie", para.job, smsToken.Token);
			res.showapi_res_body = new Showapi_res_body_list<GetFeiYongLeiBie_Res>();
			res.showapi_res_body.contentlist = GetFeiYongLeiBie_Res;
			res.showapi_res_code = "200";
			return res;
		}
        /// <summary>
        /// 获取全部客户分类
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<KeHuFenLei_Res> GetAllKeHuFenLei(Job para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<KeHuFenLei_Res>();
                return Return.Return();
            }
            Showapi_Res_List<KeHuFenLei_Res> res = new Showapi_Res_List<KeHuFenLei_Res>();
            List<KeHuFenLei_Res> KeHuFenLei_Res = new List<KeHuFenLei_Res>();
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }
            KeHuFenLei_Res = SaleRiQingToSMS.GetAsynctMethod<List<KeHuFenLei_Res>>(SMSUrl + "api/Base_PotentialCusTomer/GetAllKeHuFenLei", para.job, smsToken.Token);
            res.showapi_res_body = new Showapi_res_body_list<KeHuFenLei_Res>();
            res.showapi_res_body.contentlist = KeHuFenLei_Res;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 获取全部地区分类
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<DiQuFenLei_Res> GetAllDiQuFenLei(Job para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<DiQuFenLei_Res>();
                return Return.Return();
            }
            Showapi_Res_List<DiQuFenLei_Res> res = new Showapi_Res_List<DiQuFenLei_Res>();
            List<DiQuFenLei_Res> DiQuFenLei_Res = new List<DiQuFenLei_Res>();
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }
            DiQuFenLei_Res = SaleRiQingToSMS.GetAsynctMethod<List<DiQuFenLei_Res>>(SMSUrl + "api/Base_PotentialCusTomer/GetAllDiQuFenLei", para.job, smsToken.Token);
            res.showapi_res_body = new Showapi_res_body_list<DiQuFenLei_Res>();
            res.showapi_res_body.contentlist = DiQuFenLei_Res;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 日清填写
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge SaveRiQingDayPlan(SaveRiQingPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }

            SaveRiQingPara SaveRiQingPara = new SaveRiQingPara();
            SaveRiQingPara.CreatePerson = para.CreatePerson;
            List<RiQing_FeiYongMingXi> RiQing_DayPlan_FeiYongMingXi = new List<RiQing_FeiYongMingXi>();
            List<RiQing_LinShiXiangMu> RiQing_DayPlan_LinShiXiangMu = new List<RiQing_LinShiXiangMu>();
            List<RiQing_MonthPlanXiangMu> RiQing_DayPlan_MonthPlanXiangMu = new List<RiQing_MonthPlanXiangMu>();
            List<RiQing_QiTaNeiRong> RiQing_DayPlan_QiTaNeiRong = new List<RiQing_QiTaNeiRong>();
            List<RiQing_RiQingGuanLi> RiQing_RiQingGuanLi_List = new List<RiQing_RiQingGuanLi>();
            if (para.RiQing_DayPlan_FeiYongMingXi != null)
            {
                foreach (var item in para.RiQing_DayPlan_FeiYongMingXi)
                {
                    RiQing_FeiYongMingXi RiQing_FeiYongMingXi = new RiQing_FeiYongMingXi();
                    RiQing_FeiYongMingXi.xuhao = item.xuhao;
                    RiQing_FeiYongMingXi.BeiZhu = item.BeiZhu;
                    RiQing_FeiYongMingXi.FeiYongLeiBie = item.FeiYongLeiBie;
                    RiQing_FeiYongMingXi.FeiYongLeiBiebm = item.FeiYongLeiBiebm;
                    RiQing_FeiYongMingXi.JinE = item.JinE;
                    RiQing_FeiYongMingXi.FeiYongJine = item.FeiYongJine;
                    RiQing_FeiYongMingXi.ID = item.ID;
                    RiQing_FeiYongMingXi.DayPlanID = item.DayPlanID;
                    RiQing_DayPlan_FeiYongMingXi.Add(RiQing_FeiYongMingXi);
                }
            }
            if (para.RiQing_DayPlan_LinShiXiangMu != null)
            {
                foreach (var item in para.RiQing_DayPlan_LinShiXiangMu)
                {
                    RiQing_LinShiXiangMu RiQing_LinShiXiangMu = new RiQing_LinShiXiangMu();
                    RiQing_LinShiXiangMu.xuhao = item.xuhao;
                    RiQing_LinShiXiangMu.ChaYiFenXi = item.ChaYiFenXi;
                    RiQing_LinShiXiangMu.LinShiXiangMuDayFact = item.LinShiXiangMuDayFact;
                    RiQing_LinShiXiangMu.LinShiXiangMuDayPlan = item.LinShiXiangMuDayPlan;
                    RiQing_LinShiXiangMu.LinShiXiangMuName = item.LinShiXiangMuName;
                    RiQing_LinShiXiangMu.ID = item.ID;
                    RiQing_LinShiXiangMu.DayPlanID = item.DayPlanID;
                    RiQing_DayPlan_LinShiXiangMu.Add(RiQing_LinShiXiangMu);
                }
            }
            if (para.RiQing_DayPlan_MonthPlanXiangMu != null)
            {
                foreach (var item in para.RiQing_DayPlan_MonthPlanXiangMu)
                {
                    RiQing_MonthPlanXiangMu RiQing_MonthPlanXiangMu = new RiQing_MonthPlanXiangMu();
                    RiQing_MonthPlanXiangMu.ChaYiFenXiJieJueCuoShi = item.ChaYiFenXiJieJueCuoShi;
                    RiQing_MonthPlanXiangMu.DayFactContent = item.DayFactContent;
                    RiQing_MonthPlanXiangMu.DayPlanContent = item.DayPlanContent;
                    RiQing_MonthPlanXiangMu.MonthPlanID = item.MonthPlanID;
                    RiQing_MonthPlanXiangMu.NextPlanContent = item.NextPlanContent;
                    RiQing_MonthPlanXiangMu.MonthPlanItem = item.MonthPlanItem;
                    RiQing_MonthPlanXiangMu.ID = item.ID;
                    RiQing_MonthPlanXiangMu.DayPlanID = item.DayPlanID;
                    RiQing_DayPlan_MonthPlanXiangMu.Add(RiQing_MonthPlanXiangMu);
                }
            }
            if (para.RiQing_RiQingGuanLi != null)
            {
                foreach (var item in para.RiQing_RiQingGuanLi)
                {
                    RiQing_RiQingGuanLi RiQing_RiQingGuanLi = new RiQing_RiQingGuanLi();
                    RiQing_RiQingGuanLi.xuhao = item.xuhao;
                    RiQing_RiQingGuanLi.cBaiFangMuDi = item.cBaiFangMuDi;
                    RiQing_RiQingGuanLi.BaiFangMuDiBiaoMa = item.BaiFangMuDiBiaoMa;
                    RiQing_RiQingGuanLi.cBeiZhu = item.cBeiZhu;
                    RiQing_RiQingGuanLi.cGongZuoXiaoJie = item.cGongZuoXiaoJie;
                    RiQing_RiQingGuanLi.cKeHuAddress = item.cKeHuAddress;
                    RiQing_RiQingGuanLi.ckeHuCode = item.ckeHuCode;
                    RiQing_RiQingGuanLi.cKeHuMingCheng = item.cKeHuMingCheng;
                    RiQing_RiQingGuanLi.ID = item.ID;
                    RiQing_RiQingGuanLi.DayPlanID = item.DayPlanID;
                    RiQing_RiQingGuanLi_List.Add(RiQing_RiQingGuanLi);
                }
            }
            SaveRiQingPara.RiQing_DayPlan_FeiYongMingXi = RiQing_DayPlan_FeiYongMingXi;
            SaveRiQingPara.RiQing_DayPlan_LinShiXiangMu = RiQing_DayPlan_LinShiXiangMu;
            SaveRiQingPara.RiQing_DayPlan_MonthPlanXiangMu = RiQing_DayPlan_MonthPlanXiangMu;
            SaveRiQingPara.RiQing_DayPlan_QiTaNeiRong = RiQing_DayPlan_QiTaNeiRong;
            SaveRiQingPara.RiQing_RiQingGuanLi = RiQing_RiQingGuanLi_List;

            var str = Newtonsoft.Json.JsonConvert.SerializeObject(SaveRiQingPara);
            var SaveRiQing = SaleRiQingToSMS.PostAsynctMethod<MessageSuccess>(SMSUrl + "api/RiQing/SaveRiQingDayPlan", str, para.Job, smsToken.Token); ;
            if (!SaveRiQing.ChengGong)
            {
                throw new BusinessTureException("保存失败");
            }
            return Message.SuccessMeaasge("保存成功");
        }
        /// <summary>
        /// 日清审核
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge RiQingReview(RiQingReviewPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }

            RiQingReviewPara RiQingReviewPara = new RiQingReviewPara();
            RiQingReviewPara.CreateCode = para.CreateCode;
            RiQingReviewPara.DayPlanID = para.DayPlanID;
            RiQingReviewPara.RqJobID = para.RqJobID;
            RiQingReviewPara.ShenHePerson = para.ShenHePerson;
            RiQingReviewPara.ShenHePingJia = para.ShenHePingJia;
            RiQingReviewPara.ShenHePiYu = para.ShenHePiYu;
            RiQingReviewPara.ZhangTou_id = para.ZhangTou_id;

            var str = Newtonsoft.Json.JsonConvert.SerializeObject(RiQingReviewPara);
            var Review = SaleRiQingToSMS.PostAsynctMethod<MessageSuccess>(SMSUrl + "api/RiQing/ShenPiRiQingDayPlan", str, para.job, smsToken.Token);
            if (!Review.ChengGong)
            {
                throw new BusinessTureException("审核失败");
            }
            return Message.SuccessMeaasge("审核成功");
        }
        /// <summary>
        /// 日清签到类别
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<QianDaoTypeResult> GetQianDaoType(Job para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<QianDaoTypeResult>();
                return Return.Return();
            }
            Showapi_Res_List<QianDaoTypeResult> res = new Showapi_Res_List<QianDaoTypeResult>();
            List<QianDaoTypeResult> QianDaoTypeResult = new List<QianDaoTypeResult>();
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }
            QianDaoTypeResult = SaleRiQingToSMS.GetAsynctMethod<List<QianDaoTypeResult>>(SMSUrl + "api/RiQing/GetQiaoDaoType", para.job, smsToken.Token);
            res.showapi_res_body = new Showapi_res_body_list<QianDaoTypeResult>();
            res.showapi_res_body.contentlist = QianDaoTypeResult;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 日清签到列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<RiQingQianDaoListResult> GetRiQingQianDaoList(RiQingQianDaoListInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<RiQingQianDaoListResult>();
                return Return.Return();
            }
            Showapi_Res_List<RiQingQianDaoListResult> res = new Showapi_Res_List<RiQingQianDaoListResult>();
            List<RiQingQianDaoListResult> RiQingQianDaoListResult = new List<RiQingQianDaoListResult>();
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(para);
            RiQingQianDaoListResult = SaleRiQingToSMS.PostAsynctMethod<List<RiQingQianDaoListResult>>(SMSUrl + "api/RiQing/GetRiQingQianDao_List", str, para.job, smsToken.Token);
            res.showapi_res_body = new Showapi_res_body_list<RiQingQianDaoListResult>();
            res.showapi_res_body.contentlist = RiQingQianDaoListResult;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 日清签到
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge AddOneQianDao(AddOneQianDaoInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }

            AddOneQianDaoInPara AddOneQianDaoInPara = new AddOneQianDaoInPara();
            AddOneQianDaoInPara.JingDu = para.JingDu;
            AddOneQianDaoInPara.WeiDu = para.WeiDu;
            AddOneQianDaoInPara.QianDaoType = para.QianDaoType;
            AddOneQianDaoInPara.KeHuCode = para.KeHuCode;
            AddOneQianDaoInPara.Remark = para.Remark;
            AddOneQianDaoInPara.UserJob = para.job;

            var str = Newtonsoft.Json.JsonConvert.SerializeObject(AddOneQianDaoInPara);
            var Review = SaleRiQingToSMS.PostAsynctMethod<MessageSuccess>(SMSUrl + "api/RiQing/AddOneQianDao", str, para.job, smsToken.Token);
            if (!Review.ChengGong)
            {
                throw new BusinessTureException("签到失败");
            }
            return Message.SuccessMeaasge("签到成功");
        }
        /// <summary>
        /// 日清签到统计
        /// </summary>
        public Showapi_Res_List<QianDaoTongJi> GetQianDaoTongJiList(QianDaoTongJiInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<QianDaoTongJi>();
                return Return.Return();
            }
            Showapi_Res_List<QianDaoTongJi> res = new Showapi_Res_List<QianDaoTongJi>();
            List<QianDaoTongJi> QianDaoTongJi = new List<QianDaoTongJi>();
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(para);
            QianDaoTongJi = SaleRiQingToSMS.PostAsynctMethod<List<QianDaoTongJi>>(SMSUrl + "api/RiQing/GetQianDaoTongJiList", str, para.job, smsToken.Token);
            res.showapi_res_body = new Showapi_res_body_list<QianDaoTongJi>();
            res.showapi_res_body.contentlist = QianDaoTongJi;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 签到统计  数据tab页
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<QianDaoTongJiData> GetQianDaoTongJiDataList(QianDaoTongJiInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<QianDaoTongJiData>();
                return Return.Return();
            }
            Showapi_Res_List<QianDaoTongJiData> res = new Showapi_Res_List<QianDaoTongJiData>();
            List<QianDaoTongJiData> QianDaoTongJiData = new List<QianDaoTongJiData>();
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(para);
            QianDaoTongJiData = SaleRiQingToSMS.PostAsynctMethod<List<QianDaoTongJiData>>(SMSUrl + "api/RiQing/GetQianDaoTongJiDataList", str, para.job, smsToken.Token);
            res.showapi_res_body = new Showapi_res_body_list<QianDaoTongJiData>();
            res.showapi_res_body.contentlist = QianDaoTongJiData;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 获取SMS系统全部人员信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<AllPersonList> GetAllPersonList(Job para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<AllPersonList>();
                return Return.Return();
            }
            Showapi_Res_List<AllPersonList> res = new Showapi_Res_List<AllPersonList>();
            List<AllPersonList> AllPersonList = new List<AllPersonList>();
            var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
            if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
            {
                throw new BusinessTureException("获取SMSToken失败");
            }
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(para);
            AllPersonList = SaleRiQingToSMS.PostAsynctMethod<List<AllPersonList>>(SMSUrl + "api/RiQing/GetAllPersonList", str, para.job, smsToken.Token);
            res.showapi_res_body = new Showapi_res_body_list<AllPersonList>();
            res.showapi_res_body.contentlist = AllPersonList;
            res.showapi_res_code = "200";
            return res;
        }
		/// <summary>
		/// 月计划填写
		/// </summary>
		/// <returns></returns>
		public Showapi_Res_Meaasge AddRiQingMonthPlan(AddRiQingMonthPlan_Para para)
		{
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
			var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
			if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
			{
				throw new BusinessTureException("获取SMSToken失败");
			}

            AddRiQingMonthPlan_Para AddRiQingMonthPlan_Para = new AddRiQingMonthPlan_Para();
            AddRiQingMonthPlan_Para.WorkPlanContent = "测试12321";
            AddRiQingMonthPlan_Para.WorkPlanClass = "A";
            AddRiQingMonthPlan_Para.MonthTarget = "测试45654";
            AddRiQingMonthPlan_Para.PlanRes = para.PlanRes;

            var str = Newtonsoft.Json.JsonConvert.SerializeObject(AddRiQingMonthPlan_Para);
            var Review = SaleRiQingToSMS.PostAsynctMethod<MessageSuccess>(SMSUrl + "api/RiQing/AddRiQingMonthPlan", str, para.job, smsToken.Token);
            if (!Review.ChengGong)
            {
                throw new BusinessTureException("保存失败");
            }
            return Message.SuccessMeaasge("保存成功");
        }
		/// <summary>
		/// 保存周总结
		/// </summary>
		/// <returns></returns>
		public Showapi_Res_Single<AddRiQingPlan_Res> AddWeekPlanZongJie(AddWeekPlanZongJie_Para para)
		{
			var memberid = _PrincipalBase.GetMemberId();
			if (memberid == null || memberid == "")
			{
				var ReturnSingle = new ReturnSingle<AddRiQingPlan_Res>();
				return ReturnSingle.Return();
			}
			Showapi_Res_Single<AddRiQingPlan_Res> res = new Showapi_Res_Single<AddRiQingPlan_Res>();
			AddRiQingPlan_Res AddRiQingPlan_Res = new AddRiQingPlan_Res();
			var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
			var str = Newtonsoft.Json.JsonConvert.SerializeObject(para);
			if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
			{
				throw new BusinessTureException("获取SMSToken失败");
			}
			AddRiQingPlan_Res = SaleRiQingToSMS.PostAsynctMethod<AddRiQingPlan_Res>(SMSUrl + "api/WeekPlanZongJie/AddWeekPlanZongJie", str, para.job, smsToken.Token);
			res.showapi_res_body = AddRiQingPlan_Res;
			res.showapi_res_code = "200";
			return res;
		}
		/// <summary>
		/// 保存月总结
		/// </summary>
		/// <returns></returns>
		public Showapi_Res_Single<AddRiQingPlan_Res> AddMonthPlanZongJie(AddMonthPlanZongJie_Para para)
		{
			var memberid = _PrincipalBase.GetMemberId();
			if (memberid == null || memberid == "")
			{
				var ReturnSingle = new ReturnSingle<AddRiQingPlan_Res>();
				return ReturnSingle.Return();
			}
			Showapi_Res_Single<AddRiQingPlan_Res> res = new Showapi_Res_Single<AddRiQingPlan_Res>();
			AddRiQingPlan_Res AddRiQingPlan_Res = new AddRiQingPlan_Res();
			var smsToken = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == memberid && t.System == "SMS").FirstOrDefault();
			var str = Newtonsoft.Json.JsonConvert.SerializeObject(para);
			if (smsToken == null || string.IsNullOrEmpty(smsToken.Token))
			{
				throw new BusinessTureException("获取SMSToken失败");
			}
			AddRiQingPlan_Res = SaleRiQingToSMS.PostAsynctMethod<AddRiQingPlan_Res>(SMSUrl + "api/RiQing/AddMonthPlanZongJie", str, para.job, smsToken.Token);
			res.showapi_res_body = AddRiQingPlan_Res;
			res.showapi_res_code = "200";
			return res;
		}
	}
}
