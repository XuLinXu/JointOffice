using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.WorkFlow
{
    [Serializable]
    public class WF_ConditionPath : WF_ArraySetp
    {
        WF_Setps _WF_Setps = null;
        public WF_ConditionPath()
            : base()
        {
            _WF_Setps = new WF_Setps();
            WF_ConditionPath_EndSetp = new WF_ConditionPath_EndSetp();
            WF_ConditionPath_EndSetp.Parent_Id = this.Parent_Id;
            WF_ConditionPath_EndSetp.SetpName = "路径End";
            WF_ConditionPath_EndSetp.SetpDesc = "路径End";
        }

        //public new string SetpId
        //{
        //    get { return base.SetpId; }
        //    set
        //    {
        //        //if (string.IsNullOrEmpty(SetpId))
        //        //{
        //        base.SetpId = SetpId;
        //        _NextSetps = new WF_Setps(SetpId);
        //        //}
        //        //else
        //        //{
        //        //    throw new Exception("已经存在了SetpId值,不能重复赋值.");
        //        //}
        //    }
        //}
        public virtual string Condition { get; set; }
        //public List<WF_Setp> NextSetps
        //{
        //    get { return _NextSetps; }
        //}

        public override WF_Setps WF_Setps
        {
            get { return _WF_Setps; }
        }

        public override WF_Setp GetStartWF_Setp
        {
            get
            {
                //return WF_Setps.FirstOrDefault();
                var findSetp = WF_Setps.FirstOrDefault();
                if (findSetp == null)
                {
                    return this.Get_EndSetp();
                }
                else
                {
                    return findSetp;
                }
            }
        }

        public WF_ConditionPath_EndSetp WF_ConditionPath_EndSetp { get; set; }
        #region 行为处理
        public override void Add(WF_Setp item)
        {
            if (_WF_Setps != null && item != null)
            {
                var find = _WF_Setps.Where(t => t.SetpId == item.SetpId).FirstOrDefault();
                if (find != null)
                {
                    throw new Exception(string.Format("Id是{0}的节点已经存在于节点集合中", item.SetpId));
                }

                if (_WF_Setps.Count == 0)  ////如果起始在开始和结束间插入审批节点
                {
                    if (item is WF_ArraySetp)
                    {
                        WF_ArraySetp setp = item as WF_ArraySetp;
                        var endSetp = setp.Get_EndSetp();
                        item.Parent_Id = this.SetpId;
                        item.Pervious_SetpId = this.SetpId;
                        item.Next_SetpId = "";//endSetp.SetpId;
                        endSetp.Next_SetpId = this.Get_EndSetp().SetpId;
                    }
                    else
                    {
                        item.Parent_Id = this.SetpId;
                        item.Pervious_SetpId = this.SetpId;
                        //item.Next_SetpId = this.SetpId;
                        item.Next_SetpId = WF_ConditionPath_EndSetp.SetpId;
                    }
                }
                else  ////否则,添加到最后一个节点和WF_End节点之前
                {
                    var lastItem = _WF_Setps[_WF_Setps.Count - 1];
                    if (item is WF_ArraySetp)
                    {
                        WF_ArraySetp setp = item as WF_ArraySetp;
                        var endSetp = setp.Get_EndSetp();
                        if (lastItem is WF_ArraySetp)
                        {
                            var temS = lastItem as WF_ArraySetp;
                            temS.Get_EndSetp().Next_SetpId = item.SetpId;
                        }
                        else
                        {
                            lastItem.Next_SetpId = item.SetpId;
                        }
                        item.Pervious_SetpId = lastItem.SetpId;
                        item.Next_SetpId = "";//endSetp.SetpId;
                        endSetp.Next_SetpId = this.Get_EndSetp().SetpId;
                    }
                    else
                    {
                        item.Parent_Id = this.SetpId;

                        if (lastItem is WF_ArraySetp)
                        {
                            var temS = lastItem as WF_ArraySetp;
                            temS.Get_EndSetp().Next_SetpId = item.SetpId;
                        }
                        else
                        {
                            lastItem.Next_SetpId = item.SetpId;
                        }
                        item.Pervious_SetpId = lastItem.SetpId;
                        //item.Next_SetpId = this.SetpId;
                        item.Next_SetpId = WF_ConditionPath_EndSetp.SetpId;
                    }
                }
                _WF_Setps.Add(item);
            }
        }

        public override void AddRange(IEnumerable<WF_Setp> collection)
        {
            if (_WF_Setps != null && collection != null)
            {
                foreach (var item in collection)
                {
                    Add(item);
                }
            }
        }
        public override void Insert(WF_Setp sourceItem, WF_Setp item)
        {
            if (_WF_Setps != null && item != null && sourceItem != null)
            {
                var find = _WF_Setps.Where(t => t.SetpId == item.SetpId).FirstOrDefault();
                if (find != null)
                {
                    throw new Exception(string.Format("Id是{0}的节点已经存在于节点集合中", item.SetpId));
                }

                var findItem = _WF_Setps.Where(t => t.SetpId == sourceItem.SetpId).FirstOrDefault();
                int index = _WF_Setps.IndexOf(findItem);
                if (index < _WF_Setps.Count)
                {
                    if (index == 0)
                    {
                        item.Parent_Id = this.SetpId;
                        item.Pervious_SetpId = this.SetpId;
                        if (item is WF_ArraySetp)
                        {
                            WF_ArraySetp setp = item as WF_ArraySetp;
                            var endSetp = setp.Get_EndSetp();
                            //item.Next_SetpId = findItem.SetpId;
                            endSetp.Next_SetpId = findItem.SetpId;
                        }
                        else
                        {
                            item.Next_SetpId = findItem.SetpId;
                        }
                        findItem.Pervious_SetpId = item.SetpId;
                        _WF_Setps.Insert(index, item);
                    }
                    else
                    {
                        var PSetpId = findItem.Pervious_SetpId;
                        var PSetp = _WF_Setps.Where(t => t.SetpId == PSetpId).FirstOrDefault();/////寻找下一个节点
                        if (PSetp != null)
                        {
                            item.Parent_Id = this.SetpId;
                            if (PSetp is WF_ArraySetp)
                            {
                                //PSetp.Next_SetpId = item.SetpId;
                                var temPsetp = PSetp as WF_ArraySetp;
                                temPsetp.Get_EndSetp().Next_SetpId = item.SetpId;
                            }
                            else
                            {
                                PSetp.Next_SetpId = item.SetpId;
                            }
                            item.Pervious_SetpId = PSetp.SetpId;

                            if (item is WF_ArraySetp)
                            {
                                var temsetp = item as WF_ArraySetp;
                                temsetp.Get_EndSetp().Next_SetpId = findItem.SetpId;
                            }
                            else
                            {
                                item.Next_SetpId = findItem.SetpId;
                            }
                            findItem.Pervious_SetpId = item.SetpId;
                            _WF_Setps.Insert(index, item);
                        }
                        else
                        {
                            throw new Exception(string.Format("在审批节点集合中没有发现要在其后插入的Id是{0}的节点的后节点信息", item.SetpId));
                        }
                    }
                }
            }
        }

        public override void Delete(WF_Setp item)
        {
            if (_WF_Setps != null && item != null)
            {
                var findItem = _WF_Setps.Where(t => t.SetpId == item.SetpId).FirstOrDefault();
                if (findItem == null)
                {
                    throw new Exception(string.Format("在审批节点集合中没有发现要删除的Id是{0}的节点信息", item.SetpId));
                }
                else//////寻找此节点的上一个节点和下一个节点进行处理
                {
                    int index = _WF_Setps.IndexOf(findItem);
                    if (index == 0)/////如果是第一个
                    {
                        var NSetpId = findItem.Next_SetpId;
                        var NSetp = _WF_Setps.Where(t => t.SetpId == NSetpId).FirstOrDefault();/////寻找下一个节点
                        if (NSetp != null)  ////第一个还有下一个节点
                        {
                            NSetp.Pervious_SetpId = "";
                        }
                        _WF_Setps.Remove(findItem);
                    }
                    else if (index == _WF_Setps.Count - 1 && index > 0) ////如果是最后一个
                    {
                        var PSetpId = findItem.Pervious_SetpId;
                        var PSetp = _WF_Setps.Where(t => t.SetpId == PSetpId).FirstOrDefault();/////寻找上一个节点
                        if (PSetp != null)  ////第一个还有下一个节点
                        {
                            PSetp.Next_SetpId = this.WF_ConditionPath_EndSetp.SetpId;
                        }
                        _WF_Setps.Remove(findItem);
                    }
                    else
                    {
                        var PSetpId = findItem.Pervious_SetpId;
                        var NSetpId = findItem.Next_SetpId;
                        var PSetp = _WF_Setps.Where(t => t.SetpId == PSetpId).FirstOrDefault();  /////寻找上一个节点
                        var NSetp = _WF_Setps.Where(t => t.SetpId == NSetpId).FirstOrDefault();/////寻找下一个节点
                        if (PSetp != null && NSetp != null)
                        {
                            PSetp.Next_SetpId = NSetp.SetpId;
                            NSetp.Pervious_SetpId = PSetp.SetpId;
                            _WF_Setps.Remove(findItem);  ////从集合中移除.
                        }
                        else
                        {
                            throw new Exception(string.Format("在审批节点集合中没有发现要删除的Id是{0}的前节点和后节点信息", item.SetpId));
                        }
                    }
                }
            }
        }
        #endregion

        public override bool Container(WF_Setp item)
        {
            return this.WF_Setps.Where(t => t.SetpId == item.SetpId).FirstOrDefault() == null ? false : true;
        }

        public override WF_Setp Get_EndSetp()
        {
            return this.WF_ConditionPath_EndSetp;
        }
    }
    [Serializable]
    public class WF_ConditionPath_EndSetp : WF_Setp
    {

    }
}
