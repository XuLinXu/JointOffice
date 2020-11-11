using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.WorkFlow;

namespace JointOffice.Models
{
    public class BWorkFlowManger
    {
        public WF_Setp GetWordFlowInstanceSetp(WF_Flow wf_Flow, string setpId)
        {
            return GetInstanceSetp(wf_Flow, setpId);
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
        public List<WorkFlowShow> InitWFTree(WF_Flow WF)
        {
            List<WorkFlowShow> list = new List<WorkFlowShow>();
            try
            {
                if (WF != null)
                {
                    WorkFlowShow wfs0 = new WorkFlowShow();
                    wfs0.stepId = WF.SetpId;
                    wfs0.stepName = WF.SetpName;
                    wfs0.stepDesc = WF.SetpDesc;
                    wfs0.stepTag = 1;
                    wfs0.condition = "";
                    list.Add(wfs0);

                    WorkFlowShow wfs1 = new WorkFlowShow();
                    wfs1.stepId = WF.WF_Begin.SetpId;
                    wfs1.stepName = WF.WF_Begin.SetpName;
                    wfs1.stepDesc = WF.WF_Begin.SetpDesc;
                    wfs1.stepTag = 2;
                    wfs1.condition = "";
                    WorkFlowShow wfs2 = new WorkFlowShow();
                    wfs2.stepId = WF.WF_End.SetpId;
                    wfs2.stepName = WF.WF_End.SetpName;
                    wfs2.stepDesc = WF.WF_End.SetpDesc;
                    wfs2.stepTag = 3;
                    wfs2.condition = "";
                    wfs0.children = new List<WorkFlowShow>();
                    wfs0.children.Add(wfs1);

                    var m = WF;
                    if (m.WF_Setps != null && m.WF_Setps.Count > 0)
                    {
                        foreach (var tx in m.WF_Setps)
                        {
                            WorkFlowShow pathSingleNode = new WorkFlowShow();
                            pathSingleNode.stepId = tx.SetpId;
                            pathSingleNode.stepName = tx.SetpName;
                            pathSingleNode.stepDesc = tx.SetpDesc;
                            if (tx is WF_ConditionSetp)
                            {
                                pathSingleNode.stepTag = 4;
                            }
                            if (tx is WF_ConditionPath)
                            {
                                pathSingleNode.stepTag = 5;
                            }
                            if (tx is WF_SingleSetp)
                            {
                                pathSingleNode.stepTag = 6;
                                pathSingleNode.jobid = "";
                                foreach (var item in tx.AuditJobsCode)
                                {
                                    pathSingleNode.jobid += item + ",";
                                }
                                pathSingleNode.jobid = pathSingleNode.jobid.Remove(pathSingleNode.jobid.LastIndexOf(","));
                            }
                            pathSingleNode.condition = "";
                            wfs0.children.Add(pathSingleNode);
                        }
                        foreach (var tx in m.WF_Setps)
                        {
                            var temPContionSetp = tx as WF_ConditionSetp;
                            if (temPContionSetp != null)
                            {
                                WorkFlowShow temtn = new WorkFlowShow();
                                for (int i = 0; i < wfs0.children.Count; i++)
                                {
                                    if (wfs0.children[i].stepId == tx.SetpId)
                                    {
                                        temtn = wfs0.children[i];
                                        break;
                                    }
                                }
                                if (temtn != null)
                                {
                                    LoopNode(WF, temtn, tx);
                                }
                            }
                        }
                    }
                    wfs0.children.Add(wfs2);
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        protected void LoopNode(WF_Flow WF, WorkFlowShow treeNode, WF_Setp setp)
        {
            try
            {
                var singSetp = setp as WF_SingleSetp;
                var ContionSetp = setp as WF_ConditionSetp;
                //如果是单个步骤
                if (singSetp != null)
                {
                    WorkFlowShow setpNode = new WorkFlowShow();
                    setpNode.stepId = singSetp.SetpId;
                    setpNode.stepName = singSetp.SetpName;
                    setpNode.stepDesc = singSetp.SetpDesc;
                    setpNode.stepTag = 6;
                    setpNode.condition = "";
                    setpNode.jobid = "";
                    foreach (var item in singSetp.AuditJobsCode)
                    {
                        setpNode.jobid += item + ",";
                    }
                    setpNode.jobid = setpNode.jobid.Remove(setpNode.jobid.LastIndexOf(","));
                    treeNode.children = new List<WorkFlowShow>();
                    treeNode.children.Add(setpNode);
                }
                //如果是分支
                if (ContionSetp != null)
                {
                    treeNode.children = new List<WorkFlowShow>();
                    foreach (WF_ConditionPath m in ContionSetp.WF_Setps)
                    {
                        WorkFlowShow pathNode = new WorkFlowShow();
                        pathNode.stepId = m.SetpId;
                        pathNode.stepName = m.SetpName;
                        pathNode.stepDesc = m.SetpDesc;
                        pathNode.stepTag = 5;
                        pathNode.condition = m.Condition;
                        treeNode.children.Add(pathNode);
                        if (m.WF_Setps != null && m.WF_Setps.Count > 0)
                        {
                            pathNode.children = new List<WorkFlowShow>();
                            foreach (var tx in m.WF_Setps)
                            {
                                WorkFlowShow pathSingleNode = new WorkFlowShow();
                                pathSingleNode.stepId = tx.SetpId;
                                pathSingleNode.stepName = tx.SetpName;
                                pathSingleNode.stepDesc = tx.SetpDesc;
                                if (tx is WF_ConditionSetp)
                                {
                                    pathSingleNode.stepTag = 4;
                                }
                                if (tx is WF_ConditionPath)
                                {
                                    pathSingleNode.stepTag = 5;
                                }
                                if (tx is WF_SingleSetp)
                                {
                                    pathSingleNode.stepTag = 6;
                                    pathSingleNode.jobid = "";
                                    foreach (var item in tx.AuditJobsCode)
                                    {
                                        pathSingleNode.jobid += item + ",";
                                    }
                                    pathSingleNode.jobid = pathSingleNode.jobid.Remove(pathSingleNode.jobid.LastIndexOf(","));
                                }
                                pathSingleNode.condition = "";
                                pathNode.children.Add(pathSingleNode);
                            }
                            foreach (var tx in m.WF_Setps)
                            {
                                var temPContionSetp = tx as WF_ConditionSetp;
                                if (temPContionSetp != null)
                                {
                                    WorkFlowShow temtn = null;
                                    for (int i = 0; i < pathNode.children.Count; i++)
                                    {
                                        if (pathNode.children[i].stepId == tx.SetpId)
                                        {
                                            temtn = pathNode.children[i];
                                            break;
                                        }
                                    }
                                    if (temtn != null)
                                    {
                                        LoopNode(WF, temtn, tx);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }
    }
}
