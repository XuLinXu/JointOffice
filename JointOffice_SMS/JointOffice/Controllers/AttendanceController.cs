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
    public class AttendanceController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        private readonly IAttendance _IAttendance;
        ExceptionMessage em;
        IOptions<Root> config;
        public AttendanceController(IOptions<Root> config, IAttendance IAttendance, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _IAttendance = IAttendance;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
        }
        /// <summary>
        /// 考勤详情
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetCheckRecordList")]
        public Showapi_Res_Single<CheckRecord> GetCheckRecordList([FromBody]CheckRecordInPara para)
        {
            Showapi_Res_Single<CheckRecord> res = new Showapi_Res_Single<CheckRecord>();
            try
            {
                if (string.IsNullOrEmpty(para.time))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IAttendance.GetCheckRecordList(para);
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
        /// 签到
        /// </summary>
        /// <returns></returns>
        [HttpPost("CheckIn")]
        public Showapi_Res_Meaasge CheckIn([FromBody]CheckInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.map) || string.IsNullOrEmpty(para.address) || string.IsNullOrEmpty(para.checkType.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IAttendance.CheckIn(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 备注  写入
        /// </summary>
        /// <returns></returns>
        [HttpPost("RemarksIn")]
        public Showapi_Res_Meaasge RemarksIn([FromBody]RemarksInPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.checkDate) || string.IsNullOrEmpty(para.remarks))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IAttendance.RemarksIn(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 考勤统计
        /// </summary>
        [HttpPost("GetCheckCountList")]
        public Showapi_Res_Single<CheckCountPara> GetCheckCountList([FromBody]CheckCountInPara para)
        {
            Showapi_Res_Single<CheckCountPara> res = new Showapi_Res_Single<CheckCountPara>();
            try
            {
                if (string.IsNullOrEmpty(para.type.ToString()) || string.IsNullOrEmpty(para.beginTime) || string.IsNullOrEmpty(para.stopTime))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IAttendance.GetCheckCountList(para);
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
        /// 显示考勤统计人员和次数页面  团队
        /// </summary>
        [HttpPost("GetCheckCountTeamList")]
        public Showapi_Res_List<CheckCountTeamListPara> GetCheckCountTeamList([FromBody]CheckCountTeamListParaInPara para)
        {
            Showapi_Res_List<CheckCountTeamListPara> res = new Showapi_Res_List<CheckCountTeamListPara>();
            try
            {
                if (string.IsNullOrEmpty(para.mark.ToString()) || string.IsNullOrEmpty(para.beginTime) || string.IsNullOrEmpty(para.stopTime))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IAttendance.GetCheckCountTeamList(para);
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
        /// 显示某个人员具体签到类型的明细列表
        /// </summary>
        [HttpPost("GetCheckCountTypeList")]
        public Showapi_Res_List<CheckCountTypeList> GetCheckCountTypeList([FromBody]CheckCountTypeListInPara para)
        {
            Showapi_Res_List<CheckCountTypeList> res = new Showapi_Res_List<CheckCountTypeList>();
            try
            {
                if (string.IsNullOrEmpty(para.mark.ToString()) || string.IsNullOrEmpty(para.beginTime) || string.IsNullOrEmpty(para.stopTime) || string.IsNullOrEmpty(para.type.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                if (para.type == 1)
                {
                    if (string.IsNullOrEmpty(para.memberid))
                    {
                        throw new BusinessException("参数不正确。");
                    }
                }
                return _IAttendance.GetCheckCountTypeList(para);
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
        /// 某个人员具体签到类型的明细列表进考勤详情
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetCheckCountTypeRecordList")]
        public Showapi_Res_Single<CheckRecord> GetCheckCountTypeRecordList([FromBody]CheckCountTypeRecordInPara para)
        {
            Showapi_Res_Single<CheckRecord> res = new Showapi_Res_Single<CheckRecord>();
            try
            {
                if (string.IsNullOrEmpty(para.time) || string.IsNullOrEmpty(para.memberid))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IAttendance.GetCheckCountTypeRecordList(para);
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
