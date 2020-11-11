using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure;
using Microsoft.Extensions.Options;
using JointOffice.Models;
using System.IO;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using JointOffice.Core;

namespace JointOffice.Controllers
{
    [Route("api/[controller]")]
    public class WorkFlowController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        private readonly IWorkFlow _IWorkFlow;
        ExceptionMessage em;
        IOptions<Root> config;
        string SasKey;
        public WorkFlowController(IOptions<Root> config, IWorkFlow IWorkFlow, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _IWorkFlow = IWorkFlow;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
            SasKey = this.config.Value.ConnectionStrings.SasKey;
        }
        /// <summary>
        /// 获取审批流列表
        /// </summary>
        [HttpPost("GetWorkFlowList")]
        public Showapi_Res_List<WorkFlowList> GetWorkFlowList()
        {
            Showapi_Res_List<WorkFlowList> res = new Showapi_Res_List<WorkFlowList>();
            try
            {
                return _IWorkFlow.GetWorkFlowList();
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
        /// 新建审批流
        /// </summary>
        [HttpPost("CreateWorkFlow")]
        public Showapi_Res_Meaasge CreateWorkFlow([FromBody]CreateWorkFlowInPara para)
        {
            try
            {
                return _IWorkFlow.CreateWorkFlow(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 删除审批流
        /// </summary>
        [HttpPost("DeleteWorkFlow")]
        public Showapi_Res_Meaasge DeleteWorkFlow([FromBody]GetWorkFlowInPara para)
        {
            try
            {
                return _IWorkFlow.DeleteWorkFlow(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 获取审批流
        /// </summary>
        [HttpPost("GetWorkFlow")]
        public Showapi_Res_List<WorkFlowShow> GetWorkFlow([FromBody]GetWorkFlowInPara para)
        {
            Showapi_Res_List<WorkFlowShow> res = new Showapi_Res_List<WorkFlowShow>();
            try
            {
                return _IWorkFlow.GetWorkFlow(para);
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
        /// 添加分支
        /// </summary>
        [HttpPost("AddBranch")]
        public Showapi_Res_Meaasge AddBranch([FromBody]AddBranchInPara para)
        {
            try
            {
                return _IWorkFlow.AddBranch(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 添加分支路径
        /// </summary>
        [HttpPost("AddBranchPath")]
        public Showapi_Res_Meaasge AddBranchPath([FromBody]AddBranchInPara para)
        {
            try
            {
                return _IWorkFlow.AddBranchPath(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 添加审批节点
        /// </summary>
        [HttpPost("AddApprovalNode")]
        public Showapi_Res_Meaasge AddApprovalNode([FromBody]AddBranchInPara para)
        {
            try
            {
                return _IWorkFlow.AddApprovalNode(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 删除分支
        /// </summary>
        [HttpPost("DeleteBranch")]
        public Showapi_Res_Meaasge DeleteBranch([FromBody]DeleteBranchInPara para)
        {
            try
            {
                return _IWorkFlow.DeleteBranch(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 删除分支路径
        /// </summary>
        [HttpPost("DeleteBranchPath")]
        public Showapi_Res_Meaasge DeleteBranchPath([FromBody]DeleteBranchInPara para)
        {
            try
            {
                return _IWorkFlow.DeleteBranchPath(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 删除审批节点
        /// </summary>
        [HttpPost("DeleteApprovalNode")]
        public Showapi_Res_Meaasge DeleteApprovalNode([FromBody]DeleteBranchInPara para)
        {
            try
            {
                return _IWorkFlow.DeleteApprovalNode(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 编辑节点
        /// </summary>
        [HttpPost("EditNode")]
        public Showapi_Res_Meaasge EditNode([FromBody]AddBranchInPara para)
        {
            try
            {
                return _IWorkFlow.EditNode(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改判断条件
        /// </summary>
        [HttpPost("UpdateCondition")]
        public Showapi_Res_Meaasge UpdateCondition([FromBody]UpdateConditionInPara para)
        {
            try
            {
                return _IWorkFlow.UpdateCondition(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }

        /// <summary>
        /// 开始审批流
        /// </summary>
        [HttpPost("GoWorkFlow")]
        public Showapi_Res_Meaasge GoWorkFlow()
        {
            try
            {
                return _IWorkFlow.GoWorkFlow();
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 提交
        /// </summary>
        [HttpPost("SubmitWorkFlow")]
        public Showapi_Res_Meaasge SubmitWorkFlow([FromBody]GoWorkFlowInPara para)
        {
            try
            {
                return _IWorkFlow.SubmitWorkFlow(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
    }
}
