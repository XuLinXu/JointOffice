using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.DbModel;
using JointOffice.Models;
using JointOffice.WorkFlow;
using JointOffice.WorkFlowHelper;

namespace JointOffice.WorkFlowModels
{
    public class Test1
    {
        JointOfficeContext _JointOfficeContext;
        public Test1(JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
        }
        public void BJiaoYinKeSuTiJiao(GoWorkFlowInPara para)
        {
            WF_WorkFlowTemplate wf_template = null;
            wf_template = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateCategoryCode == "111").FirstOrDefault();
            var Instance_Jiu = _JointOfficeContext.WF_WorkFlowInstance.Where(t => t.WF_ObjectId == para.id && (t.WF_InstanceState == 1 || t.WF_InstanceState == 0) && t.WF_SourceObject == "111").FirstOrDefault();
            if (Instance_Jiu == null)
            {
                if (wf_template != null)
                {
                    //报价单单方案提交到审批流
                    BWorkFlowManger manger = new BWorkFlowManger();
                    WF_WorkFlowInstance instance = new WF_WorkFlowInstance();
                    instance.WF_InstanceID = Guid.NewGuid().ToString();
                    //instance.WF_InstanceState = 1;
                    instance.WF_ObjectId = para.id;
                    instance.WF_SourceObject = wf_template.WF_TemplateCategoryCode;
                    instance.WF_TemplateID = wf_template.WF_TemplateID;
                    //根据人员找到账套
                    //para.TJ_JOBID = "ZB-ZB-CJGLY";

                    var paras = new List<KeyValuePair<string, string>>();
                    paras.Add(new KeyValuePair<string, string>("type", "aaa"));
                    WF_WorkFlowInstanceHelper instance1 = new WF_WorkFlowInstanceHelper();
                    instance1.WF_Flow_Parameter = paras;
                    AddEF_Instance(instance, instance1);
                    //int Wf_instanceState = DBMete.WF_WorkFlowInstance.GetWorkFlowInstanceState(instance.WF_InstanceID);
                    int Wf_instanceState = 0;
                    var DanJuWfInstance = _JointOfficeContext.WF_WorkFlowInstance.Where(t => t.WF_InstanceID == instance.WF_InstanceID).FirstOrDefault();
                    if (DanJuWfInstance != null)
                    {
                        Wf_instanceState = DanJuWfInstance.WF_InstanceState;
                    }
                    //para.ShenPiState = Wf_instanceState;
                    ////改状态
                    //DBMete.KSCL_JYKS.UpdateShenPiStateByID(para);
                }
                else
                {
                    throw new Exception("系统未找到合同协议工作流模版，请联系系统管理员");
                }
            }
            else
            {
                throw new Exception("此操作无效，客诉单正在审批或已审批完。");
            }
        }
        /// <summary>
        /// 添加一个审批
        /// </summary>
        /// <param name="Instance"></param>
        public void AddEF_Instance(WF_WorkFlowInstance Instance, WF_WorkFlowInstanceHelper Instance1, bool AlowStartDirectEnd = true, string RiQingCreater = "", string RiQingCreater_Job = "")
        {
            Instance.creater = RiQingCreater;
            Instance.Creater_Job = RiQingCreater_Job;
            Instance.createtime = DateTime.Now;

            WF_Flow flow = null;
            var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == Instance.WF_TemplateID).FirstOrDefault();
            flow = WF_Flow.LoadFormByte(temp.WF_TemplateContent);

            flow.WF_Flow_Parameter = Instance1.WF_Flow_Parameter;
            //循环Setp设置Flow参数
            LoopFlowSetPara(flow, flow.WF_Setps);

            Instance.WF_InstanceState = (short)WF_InstanceState.Submit;
            Instance.active_flag = 1;

            WF_WorkFlowInstanceDetial detial = new WF_WorkFlowInstanceDetial();
            detial.creater = Instance.creater;
            detial.createtime = Instance.createtime;
            detial.WF_AuditJobName = "销售总监";
            detial.WF_AuditUserName = "jyl";
            detial.WF_AuditTime = DateTime.Now;
            detial.creater = "111222333";
            detial.createtime = DateTime.Now;
            detial.ID = string.IsNullOrEmpty(detial.ID) ? Guid.NewGuid().ToString() : detial.ID;

            detial.WF_AuditContent = "提交审批";
            detial.WF_AuditState = (int)WF_AuditState.Submit;
            detial.WF_InstanceID = Instance.WF_InstanceID;
            detial.WF_SetpID = flow.WF_Begin.SetpId;

            //获取下一个审批节点
            var FindStartRun_Setp = GetInstanceSetp(flow, flow.WF_Begin.Next_SetpId);
            if (FindStartRun_Setp == null)
            {
                throw new Exception(string.Format("没有发现编码是{0}的审批流节点.", flow.WF_Begin.Next_SetpId));
            }
            //var Next_Setp = FindNextAuditSetp(flow, flow.WF_Begin);
            var Next_Setp = FindNextAuditSetp(flow, FindStartRun_Setp);
            if (Next_Setp != null)
            {
                //结束
                if (Next_Setp is WF_End)
                {
                    //AlowStartDirectEnd==false 提醒没有找到审批节点
                    if (!AlowStartDirectEnd)
                    {
                        throw new Exception("当前职务没有发现可审批的工作流节点.本流程不允许开始直接到结束过程.");
                    }
                    Instance.WF_CurrentSetp = Next_Setp.SetpId;
                    //Instance.WF_CurrentStepMiaoShu = Next_Setp.;
                    //成功
                    Instance.WF_CurrentSetpState = (short)WF_AuditState.End;
                    //成功
                    Instance.WF_InstanceState = (short)WF_InstanceState.Succeed;
                    Instance.WF_CurrentSetp_AuditJobs = "";
                    Instance.WF_CurrentSetp_AuditUserId = "";

                    WF_WorkFlowInstanceDetial detial2 = new WF_WorkFlowInstanceDetial();
                    detial2.ID = Guid.NewGuid().ToString();
                    detial2.creater = Instance.creater;
                    detial2.createtime = Instance.createtime;
                    detial2.WF_AuditJobName = "销售总监";
                    detial2.WF_AuditUserName = "jyl";
                    detial2.WF_AuditTime = DateTime.Now;
                    detial2.creater = "111222333";
                    detial2.createtime = DateTime.Now;

                    detial2.WF_AuditContent = "审批结束";
                    //审核
                    detial2.WF_AuditState = (int)WF_AuditState.End;
                    detial2.WF_InstanceID = Instance.WF_InstanceID;
                    detial2.WF_SetpID = Next_Setp.SetpId;

                    //Instance1.WF_WorkFlowInstanceDetial.Add(detial2);
                    _JointOfficeContext.WF_WorkFlowInstanceDetial.Add(detial2);

                    //增加日志提醒
                    //WorkFowMessagAction_End start = new WorkFowMessagAction_End(Instance, flow, flow.WF_Begin, Next_Setp, WorkFowMessageSendAction.完成);
                    //start.WriteMessage();
                }
                else
                {
                    Instance.WF_CurrentSetp = Next_Setp.SetpId;
                    Instance.WF_CurrentStepMiaoShu = Next_Setp.SetpName;
                    Instance.WF_CurrentSetpState = (int)WF_AuditState.Submit;//审核中
                    Instance.WF_InstanceState = (short)WF_InstanceState.Submit;//审核中
                    //2014-02-11 增加当前审批节点的审批角色和UserID
                    Instance.WF_CurrentSetp_AuditJobs = (Next_Setp.AuditJobsCode != null && Next_Setp.AuditJobsCode.Count > 0) ? string.Join(";", Next_Setp.AuditJobsCode) : "";
                    Instance.WF_CurrentSetp_AuditUserId = (Next_Setp.AuditPersonCode != null && Next_Setp.AuditPersonCode.Count > 0) ? string.Join(";", Next_Setp.AuditPersonCode) : "";
                    //增加日志提醒
                    //WorkFowMessagAction_Start start = new WorkFowMessagAction_Start(Instance, flow, flow.WF_Begin, Next_Setp, WorkFowMessageSendAction.开始);
                    //start.WriteMessage();
                }
                //Instance1.WF_WorkFlowInstanceDetial.Add(detial);
                _JointOfficeContext.WF_WorkFlowInstanceDetial.Add(detial);
                _JointOfficeContext.WF_WorkFlowInstance.Add(Instance);

            }
            else
            {
                throw new Exception(string.Format("没有发现编码是{0}的审批流节点可移动到的下一个审批点.", flow.WF_Begin.Next_SetpId));
            }
        }
        protected void LoopFlowSetPara(WF_Flow flow, List<WF_Setp> setps)
        {
            foreach (var m in setps)
            {
                // m.WF_Flow_Parameter = flow.WF_Flow_Parameter;
                if (m is WF_ConditionPath)
                {
                    WF_ConditionPath path = (WF_ConditionPath)m;
                    path.Condition = ReplacePythonScriptPara(path.Condition, flow.WF_Flow_Parameter);
                }

                if (m is WF_ArraySetp)
                {
                    LoopFlowSetPara(flow, ((WF_ArraySetp)m).WF_Setps);
                }
            }
        }
        protected string ReplacePythonScriptPara(string pythonScript, List<KeyValuePair<string, string>> WF_Flow_Parameter)
        {
            var parts = pythonScript.Split(new char[] { '{', '[' });
            for (int i = 0; i < parts.Length; i++)
            {
                var findIndex = parts[i].IndexOf("]}");
                if (findIndex > 0)
                {
                    parts[i] = "{[" + parts[i];
                    string paraName = parts[i].Substring(0, findIndex + 4);
                    string thinParaName = paraName.TrimStart(new char[] { '{', '[' }).TrimEnd(new char[] { ']', '}' });

                    var findPara = WF_Flow_Parameter.Where(t => t.Key == thinParaName).FirstOrDefault();
                    if (string.Equals(findPara.Key, thinParaName, StringComparison.OrdinalIgnoreCase))
                    {
                        var newValue = "\"" + findPara.Value + "\"";
                        parts[i] = parts[i].Replace(paraName, newValue);
                    }
                    else
                    {
                        throw new Exception(string.Format("在审批流参数列表中,没有找到审批流参数{0}", thinParaName));
                    }
                }
            }
            return string.Concat(parts);
        }
        protected WF_Setp GetInstanceSetp(WF_Flow flow, string SetpId)
        {
            var setpList = flow.WF_Setps.Union(new List<WF_Setp>() { flow.WF_End, flow.WF_Begin, flow }).ToList();
            WF_Setp setp = null;
            LoopFlowFindSetp(setpList, SetpId, ref setp);
            return setp;
        }
        protected void LoopFlowFindSetp(List<WF_Setp> setps, string SetpId, ref WF_Setp findSetp)
        {
            foreach (var m in setps)
            {
                if (string.Equals(m.SetpId, SetpId, StringComparison.OrdinalIgnoreCase))
                {
                    findSetp = m;
                    return;
                }
                else
                {
                    if (m is WF_ArraySetp)
                    {
                        var tm = m as WF_ArraySetp;
                        var tm2 = tm.Get_EndSetp();
                        if (string.Equals(tm2.SetpId, SetpId, StringComparison.OrdinalIgnoreCase))
                        {
                            findSetp = tm2;
                            return;
                        }
                        LoopFlowFindSetp(((WF_ArraySetp)m).WF_Setps, SetpId, ref findSetp);
                    }
                }
            }
        }
        protected WF_Setp FindNextAuditSetp(WF_Flow flow, WF_Setp currentSetp)
        {
            if (currentSetp is WF_ArraySetp)
            {
                WF_ArraySetp tempSetp = currentSetp as WF_ArraySetp;
                var nextFindSetp = tempSetp.GetStartWF_Setp;
                if (nextFindSetp != null)
                {
                    return FindNextAuditSetp(flow, nextFindSetp);
                }
                else
                {
                    throw new Exception(string.Format("没有发现编码是{0}的审批节点的后续步骤.", currentSetp.SetpId));
                }
            }
            else if (currentSetp is WF_ConditionPath_EndSetp || currentSetp is WF_ConditionSetp_EndSetp)
            {
                var findnextSetp = GetInstanceSetp(flow, currentSetp.Next_SetpId);
                if (findnextSetp != null)
                {
                    return FindNextAuditSetp(flow, findnextSetp);
                }
                else
                {
                    throw new Exception(string.Format("没有发现编码是{0}的审批分支节点的后续步骤.", currentSetp.SetpId));
                }
            }
            else // (Next_Setp != null)
            {
                return currentSetp;
            }
        }


        /// <summary>
        /// 进行一个审批
        /// </summary>
        /// <param name="instanceId"></param>
        public void Audit_Instance(WF_Instance_AuditParameter auditPara)
        {
            var wfInstance = _JointOfficeContext.WF_WorkFlowInstance.Where(t => t.WF_InstanceID == auditPara.WF_InstanceId).FirstOrDefault();
            //var wfInstance = DBMete.WF_WorkFlowInstance.Select().Where(t => t.WF_InstanceID == auditPara.WF_InstanceId).FirstOrDefault();
            if (wfInstance != null)
            {
                //检查当前审批实例的状态,如果是Fail和Sucess或撤销,就不能继续审批
                if (wfInstance.WF_InstanceState >= 2)
                {
                    throw new Exception(string.Format("编号是{0}的审批流已经结束.不能继续操作.", auditPara.WF_InstanceId));
                }
                string setpId = wfInstance.WF_CurrentSetp;
                var fw = GetWF_FlowByTemplateId(wfInstance.WF_TemplateID);
                fw.WF_Flow_Parameter = auditPara.WF_FlowParameter;
                //循环Setp设置Flow参数
                LoopFlowSetPara(fw, fw.WF_Setps);
                //获取当前实例的当前Setp,进行处理.
                var setp = GetInstanceSetp(fw, setpId);
                if (setp == null)
                {
                    throw new Exception(string.Format("没有找到编号是{0}的工作流步骤.", setpId));
                }
                else
                {
                    //如果指定了审核人,就使用审核人审核
                    if (setp.AuditPersonCode != null && setp.AuditPersonCode.Count > 0)
                    {
                        var findPerson = setp.AuditPersonCode.Where(t => string.Equals(t, auditPara.UserId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (findPerson != null)
                        {
                            SaveAudit(auditPara, wfInstance, fw, setp);
                        }
                        else
                        {
                            throw new Exception(string.Format("编号是{0}的工作流步骤,编号是{1}人员没有审核权限", setpId, auditPara.UserId));
                        }
                    }
                    //否则指定了审核职务,就使用审核职务进行审核
                    else if (setp.AuditJobsCode != null && setp.AuditJobsCode.Count > 0)
                    {
                        var findJobCode = setp.AuditJobsCode.Where(t => string.Equals(t, auditPara.JobId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (findJobCode != null)
                        {
                            //修改工作流状态,进入下一步
                            SaveAudit(auditPara, wfInstance, fw, setp);
                        }
                        else
                        {
                            throw new Exception(string.Format("编号是{0}的工作流步骤,编号是{1}人员职务没有审核权限", setpId, auditPara.JobId));
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("编号是{0}的工作流步骤,没有指定有效的审核职务或人员", setpId));
                    }
                }
            }
            else
            {
                throw new Exception(string.Format("没有找到要审批的审批流实例", auditPara.WF_InstanceId));
            }
        }
        /// <summary>
        /// 根据审批流模板ID获取审批流实例
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public WF_Flow GetWF_FlowByTemplateId(string templateId)
        {
            WF_Flow flow = null;
            var temp = _JointOfficeContext.WF_WorkFlowTemplate.Where(t => t.WF_TemplateID == templateId).FirstOrDefault();
            //var temp = WF_WorkFlowTemplate.Select().Where(t => t.WF_TemplateID == templateId).FirstOrDefault();
            if (temp != null)
            {
                flow = WF_Flow.LoadFormByte(temp.WF_TemplateContent);
                //string xml = temp.WF_TemplateContent;
                //flow = LEPUSoft.Core.WorkFlow.WF_Flow.LoadFormXML(xml);
                if (flow == null)
                {
                    throw new Exception(string.Format("编号是{0}的审批模板,反序列失败,请检查XML文件内容是否正确.", templateId));
                }
            }
            else
            {
                throw new Exception(string.Format("没有找到编号是{0}的审批模板", templateId));
            }
            return flow;
        }
        /// <summary>
        /// 完成一个审批节点的审批
        /// </summary>
        /// <param name="auditPara">审批参数</param>
        /// <param name="wfInstance">工作流实例</param>
        /// <param name="wf">工作流模板对象</param>
        /// <param name="setp">当前正在审批的审批节点,也就是要完成的审批节点</param>
        private void SaveAudit(WF_Instance_AuditParameter auditPara, WF_WorkFlowInstance wfInstance, WF_Flow wf, WF_Setp setp)
        {
            //var userInfo = LEPUSoft.Environment.Cls_MoreUserInfo.CurrentCls_MoreUserInfo;
            List<WF_WorkFlowInstanceDetial> detials = new List<WF_WorkFlowInstanceDetial>();

            WF_WorkFlowInstanceDetial detial = new WF_WorkFlowInstanceDetial();
            detials.Add(detial);
            detial.ID = Guid.NewGuid().ToString();
            detial.WF_InstanceID = wfInstance.WF_InstanceID;
            detial.WF_SetpID = setp.SetpId;

            detial.WF_AuditJobName = "销售总监";
            detial.WF_AuditUserName = "jyl";
            detial.WF_AuditTime = DateTime.Now;
            detial.creater = "111222333";
            detial.createtime = DateTime.Now;

            var FindStartRun_Setp = GetInstanceSetp(wf, setp.Next_SetpId);
            if (FindStartRun_Setp == null)
            {
                throw new Exception(string.Format("没有发现编码是{0}的审批流节点.", setp.Next_SetpId));
            }
            var Next_Setp = FindNextAuditSetp(wf, FindStartRun_Setp);
            //var Next_Setp = FindNextAuditSetp(wf, setp);
            if (Next_Setp == null)
            {
                throw new Exception(string.Format("没有发现编码是{0}的审批流节点步骤.", setp.Next_SetpId));
            }

            if (auditPara.IsPass)
            {
                detial.WF_AuditContent = auditPara.WF_AuditContent;
                detial.WF_AuditState = (short)WF_AuditState.Agree;
                //修改工作流状态,进入下一步.
                if (Next_Setp is WF_End)//结束
                {
                    wfInstance.WF_CurrentSetp = Next_Setp.SetpId;
                    wfInstance.WF_CurrentSetpState = (short)WF_AuditState.End;//成功.   
                    wfInstance.WF_InstanceState = (short)WF_InstanceState.Succeed;//成功.   
                    wfInstance.WF_CurrentSetp_AuditJobs = "";
                    wfInstance.WF_CurrentSetp_AuditUserId = "";

                    WF_WorkFlowInstanceDetial detial2 = new WF_WorkFlowInstanceDetial();
                    detial2.ID = Guid.NewGuid().ToString();
                    detial2.creater = wfInstance.creater;
                    detial2.createtime = wfInstance.createtime;
                    detial2.WF_AuditJobName = "销售总监";
                    detial2.WF_AuditUserName = "jyl";
                    detial2.WF_AuditTime = DateTime.Now;
                    detial2.creater = "111222333";
                    detial2.createtime = DateTime.Now;

                    detial2.WF_AuditContent = "审批结束";
                    detial2.WF_AuditState = (int)WF_AuditState.End;//审核
                    detial2.WF_InstanceID = wfInstance.WF_InstanceID;
                    detial2.WF_SetpID = Next_Setp.SetpId;

                    detials.Add(detial2);

                    //增加日志提醒
                    //WorkFowMessagAction_End start = new WorkFowMessagAction_End(wfInstance, wf, setp, Next_Setp, WorkFowMessageSendAction.完成);
                    //start.WriteMessage();
                }
                else
                {
                    wfInstance.WF_CurrentSetp = Next_Setp.SetpId;
                    wfInstance.WF_CurrentSetpState = (int)WF_AuditState.Submit;//审核中
                    wfInstance.WF_InstanceState = (short)WF_InstanceState.Auditing;//审核中.
                    //2014-02-11 增加当前审批节点的审批角色和UserID
                    wfInstance.WF_CurrentSetp_AuditJobs = (Next_Setp.AuditJobsCode != null && Next_Setp.AuditJobsCode.Count > 0) ? string.Join(";", Next_Setp.AuditJobsCode) : "";
                    wfInstance.WF_CurrentSetp_AuditUserId = (Next_Setp.AuditPersonCode != null && Next_Setp.AuditPersonCode.Count > 0) ? string.Join(";", Next_Setp.AuditPersonCode) : "";

                    //增加日志提醒
                    //WorkFowMessagAction_Aduit start = new WorkFowMessagAction_Aduit(wfInstance, wf, setp, Next_Setp, WorkFowMessageSendAction.同意);
                    //start.WriteMessage();

                    ////20161205如果审批中出现不同意则审批结束
                    //wfInstance.WF_CurrentSetp = Next_Setp.SetpId;
                    //wfInstance.WF_CurrentSetpState = (short)WF_AuditState.End;////成功.   
                    //wfInstance.WF_InstanceState = (short)WF_InstanceState.Succeed;////成功.   
                    //wfInstance.WF_CurrentSetp_AuditJobs = "";
                    //wfInstance.WF_CurrentSetp_AuditUserId = "";

                    //WF_WorkFlowInstanceDetial detial2 = new WF_WorkFlowInstanceDetial();
                    //detial2.ID = Guid.NewGuid().ToString();
                    //detial2.creater = wfInstance.creater;
                    //detial2.createtime = wfInstance.createtime;
                    //detial2.WF_AuditJobName = userInfo.当前职位名称;
                    //detial2.WF_AuditUserName = userInfo.姓名;
                    //detial2.WF_AuditTime = DateTime.Now;
                    //detial2.creater = userInfo.人员编码;
                    //detial2.createtime = DateTime.Now;

                    //detial2.WF_AuditContent = "审批结束";
                    //detial2.WF_AuditState = (int)WF_AuditState.End;////审核
                    //detial2.WF_InstanceID = wfInstance.WF_InstanceID;
                    //detial2.WF_SetpID = Next_Setp.SetpId;

                    //detials.Add(detial2);
                }
            }
            else
            {
                wfInstance.WF_CurrentSetpState = (short)WF_AuditState.DisAgree;
                wfInstance.WF_InstanceState = (short)WF_InstanceState.Fail;

                detial.WF_AuditContent = auditPara.WF_AuditContent;
                detial.WF_AuditState = (short)WF_AuditState.DisAgree;

                //增加日志提醒
                //WorkFowMessagAction_Aduit start = new WorkFowMessagAction_Aduit(wfInstance, wf, setp, Next_Setp, WorkFowMessageSendAction.不同意);
                //start.WriteMessage();
            }
            DoNextStep(wfInstance, detials);
            //WF_WorkFlowInstance.DoNextStep(wfInstance, detials);
        }
        public void DoNextStep(WF_WorkFlowInstance Instance, List<WF_WorkFlowInstanceDetial> detials)
        {
            string instanceId = Instance.WF_InstanceID;
            var findInstance = _JointOfficeContext.WF_WorkFlowInstance.Where(t => t.WF_InstanceID == instanceId && t.TST == Instance.TST).FirstOrDefault();
            if (findInstance != null)
            {
                findInstance.WF_CurrentSetp = Instance.WF_CurrentSetp;
                findInstance.WF_CurrentSetpState = Instance.WF_CurrentSetpState;
                findInstance.WF_InstanceState = Instance.WF_InstanceState;
                findInstance.WF_CurrentSetp_AuditJobs = Instance.WF_CurrentSetp_AuditJobs;
                findInstance.WF_CurrentSetp_AuditUserId = Instance.WF_CurrentSetp_AuditUserId;

                //var userInfo = LEPUSoft.Environment.Cls_MoreUserInfo.CurrentCls_MoreUserInfo;

                findInstance.modefytime = DateTime.Now;
                findInstance.modifyer = "111222333";

                foreach (var m in detials)
                {
                    _JointOfficeContext.WF_WorkFlowInstanceDetial.Add(m);
                }

                _JointOfficeContext.SaveChanges();
            }
            else
            {
                throw new Exception(string.Format("编号是{0}的审批流实例没有找到.或是已经被修改,请重新处理审批.", instanceId));
            }
        }
    }
}
