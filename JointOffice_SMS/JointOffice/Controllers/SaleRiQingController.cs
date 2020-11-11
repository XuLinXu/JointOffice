using JointOffice.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.Configuration;
using Microsoft.Extensions.Options;
using JointOffice.DbModel;

namespace JointOffice.Controllers
{
    [Route("api/[controller]")]
    public class SaleRiQingController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        private readonly ISaleRiQing _ISaleRiQing;
        ExceptionMessage em;
        IOptions<Root> config;
        public SaleRiQingController(IOptions<Root> config, ISaleRiQing ISaleRiQing, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _ISaleRiQing = ISaleRiQing;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
        }
        /// <summary>
        /// 获取当前用户职位信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetLoginUserInfor")]
        public Showapi_Res_Single<LoginUserInfor> GetLoginUserInfor([FromBody]Job para)
        {
            Showapi_Res_Single<LoginUserInfor> res = new Showapi_Res_Single<LoginUserInfor>();
            try
            {
                return _ISaleRiQing.GetLoginUserInfor(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取日清列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDayPlanListData")]
        public Showapi_Res_List<RiQingDayPlanList_Result> GetDayPlanListData([FromBody]RiQingDayPlanListSeachPara para)
        {
            Showapi_Res_List<RiQingDayPlanList_Result> res = new Showapi_Res_List<RiQingDayPlanList_Result>();
            try
            {
                return _ISaleRiQing.GetDayPlanListData(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取日清详情
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDayPlanChaXunMX")]
        public Showapi_Res_Single<DayPlanChaXunMXResult> GetDayPlanChaXunMX([FromBody]DayPlanChaXunMX para)
        {
            Showapi_Res_Single<DayPlanChaXunMXResult> res = new Showapi_Res_Single<DayPlanChaXunMXResult>();
            try
            {
                return _ISaleRiQing.GetDayPlanChaXunMX(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取日清总览
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetRiQingZongLanXinXi")]
        public Showapi_Res_List<RiQingZongLanXinXiResult> GetRiQingZongLanXinXi([FromBody]RiQingZongLanXinXiPara para)
        {
            Showapi_Res_List<RiQingZongLanXinXiResult> res = new Showapi_Res_List<RiQingZongLanXinXiResult>();
            try
            {
                return _ISaleRiQing.GetRiQingZongLanXinXi(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取日计划临时项
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDayPlanLinShiXiangByPerson")]
        public Showapi_Res_Single<DayPlanChaXunMXResult> GetDayPlanLinShiXiangByPerson([FromBody]DayPlanLinShiXiangByPersonInPara para)
        {
            Showapi_Res_Single<DayPlanChaXunMXResult> res = new Showapi_Res_Single<DayPlanChaXunMXResult>();
            try
            {
                return _ISaleRiQing.GetDayPlanLinShiXiangByPerson(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllKeHuXinXiForSelect")]
		public Showapi_Res_List<GetAllKeHuXinXiForSelect_Res> GetAllKeHuXinXiForSelect([FromBody]GetAllKeHuXinXiForSelect_Para para)
		{
			Showapi_Res_List<GetAllKeHuXinXiForSelect_Res> res = new Showapi_Res_List<GetAllKeHuXinXiForSelect_Res>();
			try
			{
				return _ISaleRiQing.GetAllKeHuXinXiForSelect(para);
			}
			catch (Exception ex)
			{
				em.ReturnMeaasge(ex);
				res.showapi_res_code = "508";
				res.showapi_res_error = ex.Message;
				return res;
			}
		}
		/// <summary>
		/// 获取拜访目的
		/// </summary>
		/// <returns></returns>
		[HttpPost("GetBaiFangMuDi")]
		public Showapi_Res_List<GetBaiFangMuDi_Res> GetBaiFangMuDi([FromBody]Job job)
		{
			Showapi_Res_List<GetBaiFangMuDi_Res> res = new Showapi_Res_List<GetBaiFangMuDi_Res>();
			try
			{
				return _ISaleRiQing.GetBaiFangMuDi(job);
			}
			catch (Exception ex)
			{
				em.ReturnMeaasge(ex);
				res.showapi_res_code = "508";
				res.showapi_res_error = ex.Message;
				return res;
			}
		}
		/// <summary>
		/// 获取费用类别
		/// </summary>
		/// <returns></returns>
		[HttpPost("GetFeiYongLeiBie")]
		public Showapi_Res_List<GetFeiYongLeiBie_Res> GetFeiYongLeiBie([FromBody]Job job)
		{
			Showapi_Res_List<GetFeiYongLeiBie_Res> res = new Showapi_Res_List<GetFeiYongLeiBie_Res>();
			try
			{
				return _ISaleRiQing.GetFeiYongLeiBie(job);
			}
			catch (Exception ex)
			{
				em.ReturnMeaasge(ex);
				res.showapi_res_code = "508";
				res.showapi_res_error = ex.Message;
				return res;
			}
		}
        /// <summary>
        /// 获取全部客户分类
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllKeHuFenLei")]
        public Showapi_Res_List<KeHuFenLei_Res> GetAllKeHuFenLei([FromBody]Job job)
        {
            Showapi_Res_List<KeHuFenLei_Res> res = new Showapi_Res_List<KeHuFenLei_Res>();
            try
            {
                return _ISaleRiQing.GetAllKeHuFenLei(job);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取全部地区分类
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllDiQuFenLei")]
        public Showapi_Res_List<DiQuFenLei_Res> GetAllDiQuFenLei([FromBody]Job job)
        {
            Showapi_Res_List<DiQuFenLei_Res> res = new Showapi_Res_List<DiQuFenLei_Res>();
            try
            {
                return _ISaleRiQing.GetAllDiQuFenLei(job);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 日清填写
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveRiQingDayPlan")]
        public Showapi_Res_Meaasge SaveRiQingDayPlan([FromBody]SaveRiQingPara para)
        {
            try
            {
                return _ISaleRiQing.SaveRiQingDayPlan(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 日清审核
        /// </summary>
        /// <returns></returns>
        [HttpPost("RiQingReview")]
        public Showapi_Res_Meaasge RiQingReview([FromBody]RiQingReviewPara para)
        {
            try
            {
                return _ISaleRiQing.RiQingReview(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 日清签到类别
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetSignType")]
        public Showapi_Res_List<QianDaoTypeResult> GetQianDaoType([FromBody]Job para)
        {
            Showapi_Res_List<QianDaoTypeResult> res = new Showapi_Res_List<QianDaoTypeResult>();
            try
            {
                return _ISaleRiQing.GetQianDaoType(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 日清签到列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetRiQingQianDaoList")]
        public Showapi_Res_List<RiQingQianDaoListResult> GetRiQingQianDaoList([FromBody]RiQingQianDaoListInPara para)
        {
            Showapi_Res_List<RiQingQianDaoListResult> res = new Showapi_Res_List<RiQingQianDaoListResult>();
            try
            {
                return _ISaleRiQing.GetRiQingQianDaoList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 日清签到
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddOneQianDao")]
        public Showapi_Res_Meaasge AddOneQianDao([FromBody]AddOneQianDaoInPara para)
        {
            try
            {
                return _ISaleRiQing.AddOneQianDao(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 日清签到统计
        /// </summary>
        [HttpPost("GetQianDaoTongJiList")]
        public Showapi_Res_List<QianDaoTongJi> GetQianDaoTongJiList([FromBody]QianDaoTongJiInPara para)
        {
            Showapi_Res_List<QianDaoTongJi> res = new Showapi_Res_List<QianDaoTongJi>();
            try
            {
                return _ISaleRiQing.GetQianDaoTongJiList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 签到统计  数据tab页
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetQianDaoTongJiDataList")]
        public Showapi_Res_List<QianDaoTongJiData> GetQianDaoTongJiDataList([FromBody]QianDaoTongJiInPara para)
        {
            Showapi_Res_List<QianDaoTongJiData> res = new Showapi_Res_List<QianDaoTongJiData>();
            try
            {
                return _ISaleRiQing.GetQianDaoTongJiDataList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取SMS系统全部人员信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllPersonList")]
        public Showapi_Res_List<AllPersonList> GetAllPersonList([FromBody]Job para)
        {
            Showapi_Res_List<AllPersonList> res = new Showapi_Res_List<AllPersonList>();
            try
            {
                return _ISaleRiQing.GetAllPersonList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
		/// <summary>
		/// 月计划填写
		/// </summary>
		/// <returns></returns>
		[HttpPost("AddRiQingMonthPlan")]
        public Showapi_Res_Meaasge AddRiQingMonthPlan([FromBody]AddRiQingMonthPlan_Para para)
        {
            try
            {
                return _ISaleRiQing.AddRiQingMonthPlan(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 保存周总结
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddWeekPlanZongJie")]
		public Showapi_Res_Single<AddRiQingPlan_Res> AddWeekPlanZongJie(AddWeekPlanZongJie_Para para)
		{
			Showapi_Res_Single<AddRiQingPlan_Res> res = new Showapi_Res_Single<AddRiQingPlan_Res>();
			try
			{
				return _ISaleRiQing.AddWeekPlanZongJie(para);
			}
			catch (Exception ex)
			{
				em.ReturnMeaasge(ex);
				res.showapi_res_code = "508";
				res.showapi_res_error = ex.Message;
				return res;
			}
		}
		/// <summary>
		/// 保存月总结
		/// </summary>
		/// <returns></returns>
		[HttpPost("AddMonthPlanZongJie")]
		public Showapi_Res_Single<AddRiQingPlan_Res> AddMonthPlanZongJie(AddMonthPlanZongJie_Para para)
		{
			Showapi_Res_Single<AddRiQingPlan_Res> res = new Showapi_Res_Single<AddRiQingPlan_Res>();
			try
			{
				return _ISaleRiQing.AddMonthPlanZongJie(para);
			}
			catch (Exception ex)
			{
				em.ReturnMeaasge(ex);
				res.showapi_res_code = "508";
				res.showapi_res_error = ex.Message;
				return res;
			}
		}

	}
}
