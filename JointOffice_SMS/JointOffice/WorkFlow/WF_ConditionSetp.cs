using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.WorkFlow
{
    [Serializable]
    public class WF_ConditionSetp : WF_ArraySetp
    {
        //WF_ConditinDefaultPath _WF_ConditinDefaultPath = new WF_ConditinDefaultPath();
        WF_ConditionPaths _WF_ConditionPath = null;
        //new WF_ConditionPaths();

        //public new string SetpId
        //{
        //    get { return base.SetpId; }
        //    set
        //    {
        //        //if (string.IsNullOrEmpty(SetpId))
        //        //{
        //        base.SetpId = SetpId;
        //        _WF_ConditionPath = new WF_ConditionPaths(SetpId);
        //        //}
        //        //else
        //        //{
        //        //    throw new Exception("已经存在了SetpId值,不能重复赋值.");
        //        //}
        //    }
        //}
        public WF_ConditionPaths WF_ConditionPaths
        {
            get { return _WF_ConditionPath; }
            set { _WF_ConditionPath = value; }
        }

        public WF_ConditionSetp()
            : base()
        {
            _WF_ConditionPath = new WF_ConditionPaths();

            WF_ConditionSetp_EndSetp = new WF_ConditionSetp_EndSetp();
            WF_ConditionSetp_EndSetp.Parent_Id = this.SetpId;
            WF_ConditionSetp_EndSetp.SetpDesc = "分支路径End";
            WF_ConditionSetp_EndSetp.SetpName = "分支路径End";

            WF_ConditinDefaultPath = new WF_ConditinDefaultPath();
            WF_ConditinDefaultPath.Parent_Id = this.SetpId;
            WF_ConditinDefaultPath.SetpDesc = "默认分支";
            WF_ConditinDefaultPath.SetpName = "默认分支";
            //WF_ConditinDefaultPath.Next_SetpId = WF_ConditinDefaultPath.WF_ConditionPath_EndSetp.SetpId;
            WF_ConditinDefaultPath.Get_EndSetp().Next_SetpId = WF_ConditionSetp_EndSetp.SetpId;
        }
        // public WF_ConditinDefaultPath WF_ConditinDefaultPath { get { return _WF_ConditinDefaultPath; } }
        public WF_ConditinDefaultPath WF_ConditinDefaultPath { get; set; }
        public override WF_Setps WF_Setps
        {
            get
            {
                if (string.IsNullOrEmpty(this.SetpId))
                {
                    throw new Exception("WF_ConditionSetp的SetpId是空值,不合法,不能获取WF_Setps属性.");
                }
                else
                {
                    WF_Setps setps = new WorkFlow.WF_Setps();
                    setps.AddRange(WF_ConditionPaths);
                    setps.Add(WF_ConditinDefaultPath);
                    return setps;
                    //return WF_ConditionPaths.Union(new List<WF_ConditionPath>() {WF_ConditinDefaultPath}
                }
            }
        }

        public override WF_Setp GetStartWF_Setp
        {
            get { return GetStartSetp(); }
        }

        protected WF_Setp GetStartSetp()
        {
            foreach (var m in _WF_ConditionPath)
            {
                string tj = m.Condition;
                if (WF_Expression.ExecuteBoolenExpression(tj))
                {
                    return m.GetStartWF_Setp;
                }
            }
            return WF_ConditinDefaultPath;
        }

        #region  行为处理
        public override void Add(WF_Setp item)
        {
            WF_ConditionPath path = item as WF_ConditionPath;

            if (_WF_ConditionPath != null && path != null)
            {
                var find = _WF_ConditionPath.Where(t => t.SetpId == item.SetpId).FirstOrDefault();
                if (find != null)
                {
                    throw new Exception(string.Format("Id是{0}的分支路径已经存在于路径集合中", item.SetpId));
                }
                path.Parent_Id = this.SetpId;
                //path.Next_SetpId = this.WF_ConditionSetp_EndSetp.SetpId;
                path.Pervious_SetpId = this.SetpId;
                path.WF_ConditionPath_EndSetp.Next_SetpId = this.WF_ConditionSetp_EndSetp.SetpId;
                //item.Parent_Id = this.SetpId;
                _WF_ConditionPath.Add(path);
                //foreach(var m in _WF_ConditionPath)
                //{
                //    m.Parent_Id = this.SetpId;
                //}
            }
            else
            {
                throw new Exception("WF_ConditionSet只允许添加WF_ConditionPath对象.");
            }
        }

        public override void AddRange(IEnumerable<WF_Setp> collection)
        {
            if (_WF_ConditionPath != null && collection != null)
            {
                foreach (var item in collection)
                {
                    Add(item);
                }
            }
        }

        public override void Insert(WF_Setp sourceItem, WF_Setp item)
        {
            if (_WF_ConditionPath != null && item != null && sourceItem != null)
            {
                WF_ConditionPath path = item as WF_ConditionPath;
                if (path != null)
                {
                    var find = _WF_ConditionPath.Where(t => t.SetpId == item.SetpId).FirstOrDefault();
                    if (find != null)
                    {
                        throw new Exception(string.Format("Id是{0}的分支路径已经存在于路径集合中", item.SetpId));
                    }

                    var findItem = _WF_ConditionPath.Where(t => t.SetpId == sourceItem.SetpId).FirstOrDefault();
                    int index = _WF_ConditionPath.IndexOf(findItem);
                    if (index < _WF_ConditionPath.Count)
                    {
                        item.Parent_Id = this.SetpId;
                        _WF_ConditionPath.Insert(index, path);
                        //foreach (var m in _WF_ConditionPath)
                        //{
                        //    m.Parent_Id = this.SetpId;
                        //}
                    }
                }
                else
                {
                    throw new Exception("WF_ConditionSet只允许插入WF_ConditionPath对象.");
                }
            }
        }

        public override void Delete(WF_Setp item)
        {
            if (_WF_ConditionPath != null && item != null)
            {
                var findItem = _WF_ConditionPath.Where(t => t.SetpId == item.SetpId).FirstOrDefault();
                if (findItem == null)
                {
                    throw new Exception(string.Format("在分支路径集合中没有发现要删除的Id是{0}的分支路径信息", item.SetpId));
                }
                else//////寻找此节点的上一个节点和下一个节点进行处理
                {
                    _WF_ConditionPath.Remove(findItem);  ////从集合中移除.                
                }
            }
        }
        #endregion

        public override bool Container(WF_Setp item)
        {
            return this.WF_Setps.Where(t => t.SetpId == item.SetpId).FirstOrDefault() == null ? false : true;
        }

        public WF_ConditionSetp_EndSetp WF_ConditionSetp_EndSetp { get; set; }

        public override WF_Setp Get_EndSetp()
        {
            return this.WF_ConditionSetp_EndSetp;
        }
    }

    [Serializable]
    public class WF_ConditionPaths : List<WF_ConditionPath>
    {
        public WF_ConditionPaths()
            : base()
        {

        }
        //string parentId = "";
        ///// <summary>获取WF_ArraySetp类型的标示ID
        ///// </summary>
        //public string ParentId
        //{
        //    get { return parentId; }
        //    private set { parentId = value; }
        //}
        //public WF_ConditionPaths(string parentId)
        //{
        //    this.ParentId = parentId;
        //}
        //public new void Add(WF_ConditionPath item)
        //{
        //    item.Parent_Id = parentId;
        //    base.Add(item);
        //}

        //public new void AddRange(IEnumerable<WF_ConditionPath> collection)
        //{
        //    foreach (var m in collection)
        //    {
        //        m.Parent_Id = parentId;
        //    }
        //    base.AddRange(collection);
        //}

        //public new void Insert(int index, WF_ConditionPath item)
        //{
        //    item.Parent_Id = parentId;
        //    base.Insert(index, item);
        //}

        //public new void InsertRange(int index, IEnumerable<WF_ConditionPath> collection)
        //{
        //    foreach (var m in collection)
        //    {
        //        m.Parent_Id = parentId;
        //    }
        //    base.InsertRange(index, collection);
        //}
    }
    [Serializable]
    public class WF_ConditionSetp_EndSetp : WF_Setp
    {

    }
}
