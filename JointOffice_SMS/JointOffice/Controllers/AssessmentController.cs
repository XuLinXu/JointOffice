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
using JointOffice.Models;


namespace JointOffice.Controllers
{
    [Route("api/[controller]")]
    public class AssessmentController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        private readonly IAssessment _IAssessment;
        ExceptionMessage em;
        IOptions<Root> config;
        public AssessmentController(IOptions<Root> config, IAssessment IAssessment, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _IAssessment = IAssessment;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
        }
        /// <summary>
        /// 获取客户授信列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("GetKeHuCreditList")]
        public Showapi_Res_List<GetKeHuCreditList> GetKeHuCreditList([FromBody]GetKeHuCreditListPara para)
        {
            Showapi_Res_List<GetKeHuCreditList> res = new Showapi_Res_List<GetKeHuCreditList>();
            try
            {
                //if (string.IsNullOrEmpty(para.state) || string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                return _IAssessment.GetKeHuCreditList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        ///获取公司信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("GetCompanyInfo")]
        public Showapi_Res_Single<GetCompanyInfo> GetCompanyInfo([FromBody]GetCompanyInfoPara para)
        {
            Showapi_Res_Single<GetCompanyInfo> res = new Showapi_Res_Single<GetCompanyInfo>();
            try
            {
                //if (string.IsNullOrEmpty(para.kehuId))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                return _IAssessment.GetCompanyInfo(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 获取客户信息  Odoo
        /// </summary>
        [HttpPost("GetCompanyInfoOdoo")]
        public Showapi_Res_Single<CompanyInfoOdoo> GetCompanyInfoOdoo([FromBody]GetInfoOdooAPI para)
        {
            Showapi_Res_Single<CompanyInfoOdoo> res = new Showapi_Res_Single<CompanyInfoOdoo>();
            try
            {
                //if (string.IsNullOrEmpty(para.id) || string.IsNullOrEmpty(para.token) || string.IsNullOrEmpty(para.mark))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                return _IAssessment.GetCompanyInfoOdoo(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 获取公司联系人信息  Odoo
        /// </summary>
        [HttpPost("GetCompanyPersonInfoOdoo")]
        public Showapi_Res_List<CompanyPersonInfoOdoo> GetCompanyPersonInfoOdoo([FromBody]GetInfoOdooAPI para)
        {
            Showapi_Res_List<CompanyPersonInfoOdoo> res = new Showapi_Res_List<CompanyPersonInfoOdoo>();
            try
            {
                return _IAssessment.GetCompanyPersonInfoOdoo(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        ///获取公司金额信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("GetCompanyMoneyInfo")]
        public Showapi_Res_Single<GetCompanyMoneyInfo> GetCompanyMoneyInfo([FromBody]GetCompanyInfoPara para)
        {
            Showapi_Res_Single<GetCompanyMoneyInfo> res = new Showapi_Res_Single<GetCompanyMoneyInfo>();
            try
            {
                //if (string.IsNullOrEmpty(para.kehuId))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                return _IAssessment.GetCompanyMoneyInfo(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        ///提交授信 修改授信
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("UpdateCredit")]
        public Showapi_Res_Meaasge UpdateCredit([FromBody]UpdateCreditPara para)
        {
            try
            {
                //if (string.IsNullOrEmpty(para.kehuId))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                return _IAssessment.UpdateCredit(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        ///审核授信
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("AuditCredit")]
        public Showapi_Res_Meaasge AuditCredit([FromBody]AuditCreditPara para)
        {
            try
            {
                //if (string.IsNullOrEmpty(para.kehuId))
                //{
                //    throw new BusinessException("参数不正确。");
                //}
                return _IAssessment.AuditCredit(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        ///获取销售人员
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("GetSalePersonList")]
        public Showapi_Res_List<GetSalePersonList> GetSalePersonList(GetSalePersonListPara para)
        {
            Showapi_Res_List<GetSalePersonList> res = new Showapi_Res_List<GetSalePersonList>();
            try
            {
                return _IAssessment.GetSalePersonList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        ///绑定销售人员
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("BinDingSalePerson")]
        public Showapi_Res_Meaasge BinDingSalePerson([FromBody]BinDingSalePerson para)
        {
            try
            {
                return _IAssessment.BinDingSalePerson(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        ///获取应收账款统计信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("GetAccountsReceivableInfo")]
        public Showapi_Res_Single<GetAccountsReceivableInfo> GetAccountsReceivableInfo([FromBody]GetAccountsReceivableInfoPara para)
        {
            Showapi_Res_Single<GetAccountsReceivableInfo> res = new Showapi_Res_Single<GetAccountsReceivableInfo>();
            try
            {
                return _IAssessment.GetAccountsReceivableInfo(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        ///我的钱包
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("GetMyPurse")]
        public Showapi_Res_Single<GetMyPurse> GetMyPurse(GetMyPursePara para)
        {
            Showapi_Res_Single<GetMyPurse> res = new Showapi_Res_Single<GetMyPurse>();
            try
            {
                return _IAssessment.GetMyPurse(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        ///我的钱包月份明细
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("GetMyPurseByMonth")]
        public Showapi_Res_Single<GetMyPurse> GetMyPurseByMonth([FromBody]GetMyPurseByMonthPara para)
        {
            Showapi_Res_Single<GetMyPurse> res = new Showapi_Res_Single<GetMyPurse>();
            try
            {
                return _IAssessment.GetMyPurseByMonth(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        ///客户工商信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost("GetGongShangInfo")]
        public Showapi_Res_Single<GetGongShangInfo> GetGongShangInfo([FromBody]GetGongShangInfoPara para)
        {
            Showapi_Res_Single<GetGongShangInfo> res = new Showapi_Res_Single<GetGongShangInfo>();
            try
            {
                return _IAssessment.GetGongShangInfo(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        ///客户工商信息  Odoo
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetGongShangInfoOdoo")]
        public Showapi_Res_Single<CompanyBusInfoOdoo> GetGongShangInfoOdoo([FromBody]GetInfoOdooAPI para)
        {
            Showapi_Res_Single<CompanyBusInfoOdoo> res = new Showapi_Res_Single<CompanyBusInfoOdoo>();
            try
            {
                return _IAssessment.GetGongShangInfoOdoo(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
    }
}

