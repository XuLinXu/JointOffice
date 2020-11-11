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
using System.Threading.Tasks;
using JointOffice.WorkFlow;
using JointOffice.WorkFlowModels;

namespace JointOffice.Models
{
    public class BWorkFlow : IWorkFlow
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        private readonly IPrincipalBase _PrincipalBase;
        IMemoryCache _memoryCache;
        public BWorkFlow(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase, IMemoryCache memoryCache)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// 获取审批流列表
        /// </summary>
        public Showapi_Res_List<WorkFlowList> GetWorkFlowList()
        {
            Showapi_Res_List<WorkFlowList> res = new Showapi_Res_List<WorkFlowList>();
            List<WorkFlowList> list = new List<WorkFlowList>();
            var wflist = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.isDelete == false).OrderByDescending(t => t.createtime).ToList();
            foreach (var item in wflist)
            {
                WorkFlowList WorkFlowList = new WorkFlowList();
                WorkFlowList.id = item.WF_TemplateID;
                WorkFlowList.code = item.WF_TemplateCategoryCode;
                WorkFlowList.version = item.WF_Version.ToString();
                WorkFlowList.name = item.WF_Name;
                WorkFlowList.desc = item.WF_DESC;
                list.Add(WorkFlowList);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<WorkFlowList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 新建审批流
        /// </summary>
        public Showapi_Res_Meaasge CreateWorkFlow(CreateWorkFlowInPara para)
        {
            Message Message = new Message();
            WF_Flow flow = new WF_Flow();
            WF_WorkFlowTemplate temp = new WF_WorkFlowTemplate();
            temp.WF_TemplateID = Guid.NewGuid().ToString();
            temp.WF_TemplateCategoryCode = para.templateCategoryCode;
            temp.WF_Version = 1;
            temp.WF_Name = para.name;
            temp.WF_DESC = para.desc;
            temp.WF_TemplateContent = WF_Flow.SaveToByte(flow);
            temp.active_flag = 1;
            temp.delete_flag = 0;
            temp.creater = "admin";
            temp.createtime = DateTime.Now;
            _JointOfficeContext.WF_WorkFlowTemplate.Add(temp);
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 删除审批流
        /// </summary>
        public Showapi_Res_Meaasge DeleteWorkFlow(GetWorkFlowInPara para)
        {
            Message Message = new Message();
            var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == para.tempId).FirstOrDefault();
            temp.isDelete = true;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 获取审批流
        /// </summary>
        public Showapi_Res_List<WorkFlowShow> GetWorkFlow(GetWorkFlowInPara para)
        {
            Showapi_Res_List<WorkFlowShow> res = new Showapi_Res_List<WorkFlowShow>();
            List<WorkFlowShow> list = new List<WorkFlowShow>();
            var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == para.tempId).FirstOrDefault();
            //var WF = WF_Flow.LoadFormJson(temp.WF_TemplateContent);
            var WF = WF_Flow.LoadFormByte(temp.WF_TemplateContent);
            BWorkFlowManger bfm = new BWorkFlowManger();
            list = bfm.InitWFTree(WF);
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<WorkFlowShow>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 添加分支
        /// </summary>
        public Showapi_Res_Meaasge AddBranch(AddBranchInPara para)
        {
            Message Message = new Message();
            var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == para.tempId).FirstOrDefault();
            var WF = WF_Flow.LoadFormByte(temp.WF_TemplateContent);
            BWorkFlowManger bfm = new BWorkFlowManger();
            WF_Setp findSetp = null;
            if (para.stepId == "审批流")
            {
                findSetp = WF;
            }
            else
            {
                //找此节点
                findSetp = bfm.GetWordFlowInstanceSetp(WF, para.stepId);
            }
            if (findSetp != null)
            {
                //分支头
                if (findSetp is WF_ArraySetp && (!(findSetp is WF_ConditionSetp)))
                {
                    var conditionSetp = findSetp as WF_ArraySetp;
                    if (conditionSetp != null)
                    {
                        WF_ConditionSetp setp = new WF_ConditionSetp();
                        setp.SetpDesc = para.newStepDesc;
                        setp.SetpName = para.newStepName;
                        conditionSetp.Add(setp);

                        var sb = WF.Check();
                        if (sb != null && sb.Length == 0)
                        {
                            var tempWFStr = WF_Flow.SaveToByte(WF);
                            temp.WF_TemplateContent = tempWFStr;
                        }
                        else
                        {
                            return Message.SuccessMeaasge(sb.ToString());
                        }
                    }
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 添加分支路径
        /// </summary>
        public Showapi_Res_Meaasge AddBranchPath(AddBranchInPara para)
        {
            Message Message = new Message();
            var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == para.tempId).FirstOrDefault();
            var WF = WF_Flow.LoadFormByte(temp.WF_TemplateContent);
            BWorkFlowManger bfm = new BWorkFlowManger();
            var findSetp = bfm.GetWordFlowInstanceSetp(WF, para.stepId);
            if (findSetp != null)
            {
                //分支头
                if (findSetp is WF_ConditionSetp)
                {
                    var conditionSetp = findSetp as WF_ConditionSetp;
                    if (conditionSetp != null)
                    {
                        var p = new WF_ConditionPath();
                        p.Condition = "{[ 1==1 ]}";
                        //p.Condition = "";
                        p.SetpDesc = para.newStepDesc;
                        p.SetpName = para.newStepName;
                        conditionSetp.Add(p);

                        var sb = WF.Check();
                        if (sb != null && sb.Length == 0)
                        {
                            var tempWFStr = WF_Flow.SaveToByte(WF);
                            temp.WF_TemplateContent = tempWFStr;
                        }
                        else
                        {
                            return Message.SuccessMeaasge(sb.ToString());
                        }
                    }
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 添加审批节点
        /// </summary>
        public Showapi_Res_Meaasge AddApprovalNode(AddBranchInPara para)
        {
            Message Message = new Message();
            var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == para.tempId).FirstOrDefault();
            var WF = WF_Flow.LoadFormByte(temp.WF_TemplateContent);
            BWorkFlowManger bfm = new BWorkFlowManger();
            WF_Setp findSetp = null;
            if (para.stepId == "审批流")
            {
                findSetp = WF;
            }
            else
            {
                findSetp = bfm.GetWordFlowInstanceSetp(WF, para.stepId);
            }
            if (findSetp != null)
            {
                //分支路径
                if (findSetp is WF_Flow || findSetp is WF_ConditionPath || findSetp is WF_ConditinDefaultPath)
                {
                    var setp = findSetp as WF_ArraySetp;
                    if (setp != null)
                    {
                        WF_SingleSetp sSetp = new WF_SingleSetp();
                        sSetp.SetpDesc = para.newStepDesc;
                        sSetp.SetpName = para.newStepName;
                        //添加消息
                        //AddDefaultMessageToSetp(tempId, sSetp.SetpId);
                        sSetp.AuditJobsCode.Add(para.personid);
                        setp.Add(sSetp);

                        var sb = WF.Check();
                        if (sb != null && sb.Length == 0)
                        {
                            var tempWFStr = WF_Flow.SaveToByte(WF);
                            temp.WF_TemplateContent = tempWFStr;
                        }
                        else
                        {
                            return Message.SuccessMeaasge(sb.ToString());
                        }
                    }
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 删除分支
        /// </summary>
        public Showapi_Res_Meaasge DeleteBranch(DeleteBranchInPara para)
        {
            Message Message = new Message();
            var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == para.tempId).FirstOrDefault();
            var WF = WF_Flow.LoadFormByte(temp.WF_TemplateContent);
            BWorkFlowManger bfm = new BWorkFlowManger();
            var findSetp = bfm.GetWordFlowInstanceSetp(WF, para.stepId);
            if (findSetp != null)
            {
                //分支头
                if (findSetp is WF_ConditionSetp)
                {
                    var conditionSetp = findSetp as WF_ConditionSetp;
                    if (conditionSetp != null)
                    {
                        //如果ParnetId是Flow
                        if (conditionSetp.Parent_Id == WF.SetpId)
                        {
                            WF.Delete(conditionSetp);
                            var sb = WF.Check();
                            if (sb != null && sb.Length == 0)
                            {
                                var tempWFStr = WF_Flow.SaveToByte(WF);
                                temp.WF_TemplateContent = tempWFStr;
                            }
                            else
                            {
                                return Message.SuccessMeaasge(sb.ToString());
                            }
                        }
                        else
                        {
                            var findConditionSetpParent = bfm.GetWordFlowInstanceSetp(WF, conditionSetp.Parent_Id);
                            if (findConditionSetpParent != null && findConditionSetpParent is WF_ArraySetp)
                            {
                                var findConditionSetpParentArray = findConditionSetpParent as WF_ArraySetp;
                                findConditionSetpParentArray.Delete(findSetp);
                                var sb = WF.Check();
                                if (sb != null && sb.Length == 0)
                                {
                                    var tempWFStr = WF_Flow.SaveToByte(WF);
                                    temp.WF_TemplateContent = tempWFStr;
                                }
                                else
                                {
                                    return Message.SuccessMeaasge(sb.ToString());
                                }
                            }
                        }
                    }
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 删除分支路径
        /// </summary>
        public Showapi_Res_Meaasge DeleteBranchPath(DeleteBranchInPara para)
        {
            Message Message = new Message();
            var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == para.tempId).FirstOrDefault();
            var WF = WF_Flow.LoadFormByte(temp.WF_TemplateContent);
            BWorkFlowManger bfm = new BWorkFlowManger();
            var findSetp = bfm.GetWordFlowInstanceSetp(WF, para.stepId);
            if (findSetp != null)
            {
                //分支路径
                if (findSetp is WF_ConditionPath)
                {
                    var path = findSetp as WF_ConditionPath;
                    if (path != null)
                    {
                        var findParentSetp = bfm.GetWordFlowInstanceSetp(WF, path.Parent_Id);
                        if (findParentSetp != null && findParentSetp is WF_ConditionSetp)
                        {
                            WF_ConditionSetp s = findParentSetp as WF_ConditionSetp;
                            if (s != null)
                            {
                                s.Delete(findSetp);
                                var sb = WF.Check();
                                if (sb != null && sb.Length == 0)
                                {
                                    var tempWFStr = WF_Flow.SaveToByte(WF);
                                    temp.WF_TemplateContent = tempWFStr;
                                }
                                else
                                {
                                    return Message.SuccessMeaasge(sb.ToString());
                                }
                            }
                        }
                    }
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 删除审批节点
        /// </summary>
        public Showapi_Res_Meaasge DeleteApprovalNode(DeleteBranchInPara para)
        {
            Message Message = new Message();
            var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == para.tempId).FirstOrDefault();
            var WF = WF_Flow.LoadFormByte(temp.WF_TemplateContent);
            BWorkFlowManger bfm = new BWorkFlowManger();
            var findSetp = bfm.GetWordFlowInstanceSetp(WF, para.stepId);
            if (findSetp != null && findSetp is WF_SingleSetp)
            {
                var ss = findSetp as WF_SingleSetp;
                if (ss != null)
                {
                    var findParentSetp = bfm.GetWordFlowInstanceSetp(WF, ss.Parent_Id);
                    if (findParentSetp != null)
                    {
                        var array = findParentSetp as WF_ArraySetp;
                        if (array != null)
                        {
                            array.Delete(ss);
                            //DeleteDefalutMessageFormSetp(tempId, setpId);
                            var sb = WF.Check();
                            if (sb != null && sb.Length == 0)
                            {
                                var tempWFStr = WF_Flow.SaveToByte(WF);
                                temp.WF_TemplateContent = tempWFStr;
                            }
                            else
                            {
                                return Message.SuccessMeaasge(sb.ToString());
                            }
                        }
                    }
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 编辑节点
        /// </summary>
        public Showapi_Res_Meaasge EditNode(AddBranchInPara para)
        {
            Message Message = new Message();
            var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == para.tempId).FirstOrDefault();
            var WF = WF_Flow.LoadFormByte(temp.WF_TemplateContent);
            BWorkFlowManger bfm = new BWorkFlowManger();
            var findSetp = bfm.GetWordFlowInstanceSetp(WF, para.stepId);
            if (findSetp != null)
            {
                findSetp.SetpDesc = para.newStepDesc;
                findSetp.SetpName = para.newStepName;

                var sb = WF.Check();
                if (sb != null && sb.Length == 0)
                {
                    var tempWFStr = WF_Flow.SaveToByte(WF);
                    temp.WF_TemplateContent = tempWFStr;
                }
                else
                {
                    return Message.SuccessMeaasge(sb.ToString());
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 修改判断条件
        /// </summary>
        public Showapi_Res_Meaasge UpdateCondition(UpdateConditionInPara para)
        {
            Message Message = new Message();
            var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == para.tempId).FirstOrDefault();
            var WF = WF_Flow.LoadFormByte(temp.WF_TemplateContent);
            BWorkFlowManger bfm = new BWorkFlowManger();
            var findSetp = bfm.GetWordFlowInstanceSetp(WF, para.stepId);
            if (findSetp != null)
            {
                var oneStep = findSetp as WF_ConditionPath;
                oneStep.Condition = para.newCondition;

                var sb = WF.Check();
                if (sb != null && sb.Length == 0)
                {
                    var tempWFStr = WF_Flow.SaveToByte(WF);
                    temp.WF_TemplateContent = tempWFStr;
                }
                else
                {
                    return Message.SuccessMeaasge(sb.ToString());
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }

        /// <summary>
        /// 开始审批流
        /// </summary>
        public Showapi_Res_Meaasge GoWorkFlow()
        {
            Message Message = new Message();
            GoWorkFlowInPara para = new GoWorkFlowInPara();
            para.id = "0d9788b5-fab5-4098-8310-648637a9b676";
            para.type = "1";
            Test1 test1 = new Test1(_JointOfficeContext);
            test1.BJiaoYinKeSuTiJiao(para);
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 提交
        /// </summary>
        public Showapi_Res_Meaasge SubmitWorkFlow(GoWorkFlowInPara para)
        {
            Message Message = new Message();
            BWorkFlowManger manger = new BWorkFlowManger();
            var instance = _JointOfficeContext.WF_WorkFlowInstance.Where(t => t.WF_InstanceID == para.id).FirstOrDefault();
            if (instance != null)
            {
                //获取工作流模版
                var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == instance.WF_TemplateID).FirstOrDefault();
                //var WF_flow = manger.GetWF_FlowByTemplateId(JiaoYinKeSuShiLi.WF_TemplateID);
                var WF_flow = WF_Flow.LoadFormByte(temp.WF_TemplateContent);
                if (WF_flow != null)
                {
                    //获取工作流模版节点
                    var WF_flowStep = manger.GetWordFlowInstanceSetp(WF_flow, instance.WF_CurrentSetp);
                    
                    var wfpara = new WF_Instance_AuditParameter();
                    //var JSFWQueRenYuanYin = "";
                    //if (para.JSFWQueRenYuanYin == "JiShuYuanYin")
                    //{
                    //    JSFWQueRenYuanYin = "技术原因";
                    //}
                    //else
                    //{
                    //    JSFWQueRenYuanYin = (para.JSFWQueRenYuanYin == "GongChang" ? "工厂原因" : "客户原因");
                    //}
                    //var JSFUGongChangShiFouTongYi = (para.JSFWQueRenYuanYin == "GongChang" ? "同意" : "不同意");
                    //var ZLJLQueRenYuanYin = (para.ZLJLQueRenYuanYin == "GongChang" ? "工厂原因" : "客户原因");
                    //var ZLJLGongChangShiFouTongYi = (para.JSFWQueRenYuanYin == "GongChang" ? "同意" : "不同意");
                    //var ZLJLPanDuanKeHuYuanYin = (para.JSFWQueRenYuanYin == "KeHu" ? "同意" : "不同意");
                    //var ZJLQueRenYuanYin = (para.ZJLQueRenYuanYin == "GongChang" ? "工厂原因" : "客户原因");

                    wfpara.IsPass = true;
                    wfpara.JobId = "9bf1d5cd-4ffc-4a8d-8a2a-35855da2d18f";
                    //wfpara.UserId = para.TJ_JOBID;
                    wfpara.WF_AuditContent = "同意";
                    wfpara.WF_InstanceId = para.id;

                    var paras = new List<KeyValuePair<string, string>>();
                    paras.Add(new KeyValuePair<string, string>("type", "aaa"));
                    wfpara.WF_FlowParameter = paras;
                    if (wfpara != null)
                    {
                        Test1 test1 = new Test1(_JointOfficeContext);
                        test1.Audit_Instance(wfpara);
                    }
                    //根据实例状态更新报价单方案状态
                    //int Wf_instanceState = DBMete.WF_WorkFlowInstance.GetWorkFlowInstanceState(JiaoYinKeSuShiLi.WF_InstanceID);
                    int Wf_instanceState = 0;
                    var DanJuWfInstance = _JointOfficeContext.WF_WorkFlowInstance.Where(t => t.WF_InstanceID == instance.WF_InstanceID).FirstOrDefault();
                    if (DanJuWfInstance != null)
                    {
                        Wf_instanceState = DanJuWfInstance.WF_InstanceState;
                    }
                    //para.ShenPiState = Wf_instanceState;
                    //DBMete.KSCL_JYKS.UpdateShenPiStateByID(para);

                }
                else
                {
                    throw new Exception("未找到客诉工作流模版.");
                }
            }
            else
            {
                throw new Exception("未找到审批项目.");
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
    }
}
