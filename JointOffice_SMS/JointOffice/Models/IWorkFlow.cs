using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public interface IWorkFlow
    {
        /// <summary>
        /// 获取审批流列表
        /// </summary>
        Showapi_Res_List<WorkFlowList> GetWorkFlowList();
        /// <summary>
        /// 新建审批流
        /// </summary>
        Showapi_Res_Meaasge CreateWorkFlow(CreateWorkFlowInPara para);
        /// <summary>
        /// 删除审批流
        /// </summary>
        Showapi_Res_Meaasge DeleteWorkFlow(GetWorkFlowInPara para);
        /// <summary>
        /// 获取审批流
        /// </summary>
        Showapi_Res_List<WorkFlowShow> GetWorkFlow(GetWorkFlowInPara para);
        /// <summary>
        /// 添加分支
        /// </summary>
        Showapi_Res_Meaasge AddBranch(AddBranchInPara para);
        /// <summary>
        /// 添加分支路径
        /// </summary>
        Showapi_Res_Meaasge AddBranchPath(AddBranchInPara para);
        /// <summary>
        /// 添加审批节点
        /// </summary>
        Showapi_Res_Meaasge AddApprovalNode(AddBranchInPara para);
        /// <summary>
        /// 删除分支
        /// </summary>
        Showapi_Res_Meaasge DeleteBranch(DeleteBranchInPara para);
        /// <summary>
        /// 删除分支路径
        /// </summary>
        Showapi_Res_Meaasge DeleteBranchPath(DeleteBranchInPara para);
        /// <summary>
        /// 删除审批节点
        /// </summary>
        Showapi_Res_Meaasge DeleteApprovalNode(DeleteBranchInPara para);
        /// <summary>
        /// 编辑节点
        /// </summary>
        Showapi_Res_Meaasge EditNode(AddBranchInPara para);
        /// <summary>
        /// 修改判断条件
        /// </summary>
        Showapi_Res_Meaasge UpdateCondition(UpdateConditionInPara para);
        /// <summary>
        /// 开始审批流
        /// </summary>
        Showapi_Res_Meaasge GoWorkFlow();
        /// <summary>
        /// 提交
        /// </summary>
        Showapi_Res_Meaasge SubmitWorkFlow(GoWorkFlowInPara para);
    }
    /// <summary>
    /// 审批流列表
    /// </summary>
    public class WorkFlowList
    {
        /// <summary>
        /// 审批流id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 审批流编码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 审批流版本
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 审批流名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 审批流描述
        /// </summary>
        public string desc { get; set; }
    }
    /// <summary>
    /// 新建审批流  入参
    /// </summary>
    public class CreateWorkFlowInPara
    {
        /// <summary>
        /// 审批流编号
        /// </summary>
        public string templateCategoryCode { get; set; }
        /// <summary>
        /// 审批流名字
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 审批流描述
        /// </summary>
        public string desc { get; set; }
    }
    /// <summary>
    /// 获取审批流  入参
    /// </summary>
    public class GetWorkFlowInPara
    {
        /// <summary>
        /// 审批流模板id
        /// </summary>
        public string tempId { get; set; }
    }
    /// <summary>
    /// 增加分支  入参
    /// </summary>
    public class AddBranchInPara
    {
        /// <summary>
        /// 审批流id
        /// </summary>
        public string tempId { get; set; }
        /// <summary>
        /// 当前点击的节点id
        /// </summary>
        public string stepId { get; set; }
        /// <summary>
        /// 添加节点的名字
        /// </summary>
        public string newStepName { get; set; }
        /// <summary>
        /// 添加节点的描述
        /// </summary>
        public string newStepDesc { get; set; }
        /// <summary>
        /// 审批人memberid
        /// </summary>
        public string personid { get; set; }
    }
    /// <summary>
    /// 修改判断条件  入参
    /// </summary>
    public class UpdateConditionInPara
    {
        /// <summary>
        /// 审批流id
        /// </summary>
        public string tempId { get; set; }
        /// <summary>
        /// 当前点击的节点id
        /// </summary>
        public string stepId { get; set; }
        /// <summary>
        /// 新判断条件
        /// </summary>
        public string newCondition { get; set; }
    }
    /// <summary>
    /// 审批流树形返回类
    /// </summary>
    public class WorkFlowShow
    {
        /// <summary>
        /// 节点id
        /// </summary>
        public string stepId { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string stepName { get; set; }
        /// <summary>
        /// 节点描述
        /// </summary>
        public string stepDesc { get; set; }
        /// <summary>
        /// 节点标记
        /// 1审批流  可加分支和审批节点
        /// 2开始
        /// 3结束
        /// 4判断  可加分支路径
        /// 5条件  可加分支和审批节点
        /// 6审批节点
        /// </summary>
        public int stepTag { get; set; }
        /// <summary>
        /// 判断条件
        /// </summary>
        public string condition { get; set; }
        /// <summary>
        /// 审批人memberid
        /// </summary>
        public string jobid { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public List<WorkFlowShow> children { get; set; }
    }
    /// <summary>
    /// 删除分支+分支路径  入参
    /// </summary>
    public class DeleteBranchInPara
    {
        /// <summary>
        /// 审批流模板id
        /// </summary>
        public string tempId { get; set; }
        /// <summary>
        /// 节点id
        /// </summary>
        public string stepId { get; set; }
    }
    public class GoWorkFlowInPara
    {
        public string id { get; set; }
        public string type { get; set; }
    }
    public class WF_Instance_AuditParameter
    {
        /// <summary>获取或设置审批人职务ID
        /// </summary>
        public string JobId { get; set; }
        /// <summary>获取或设置审批人编码ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>获取或设置审批流实例Id
        /// </summary>
        public string WF_InstanceId { get; set; }
        /// <summary>获取或设置审批是否通过
        /// </summary>
        public bool IsPass { get; set; }
        /// <summary>获取或设置审批内容
        /// </summary>
        public string WF_AuditContent { get; set; }
        /// <summary>工作流审批有关的参数
        /// </summary>
        public List<KeyValuePair<string, string>> WF_FlowParameter { get; set; }
    }
}
