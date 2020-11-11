using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.WorkFlow
{
    [Serializable]
    public abstract class WF_Setp
    {
        public WF_Setp()
        {
            this.SetpId = Guid.NewGuid().ToString();
        }
        [NonSerialized]
        List<KeyValuePair<string, string>> _WF_Flow_Parameter = new List<KeyValuePair<string, string>>();
        List<string> _AuditJobsCode = new List<string>();
        List<string> _AuditPersonCode = new List<string>();

        public List<KeyValuePair<string, string>> WF_Flow_Parameter { get; set; }
        string _SetpId = "";
        public string SetpId
        {
            get
            {
                return _SetpId;
            }
            set
            {
                _SetpId = value;
            }
        }
        public string SetpName { get; set; }
        public string SetpDesc { get; set; }
        //public WF_Setp Next_Setp { get; set; }
        public string Parent_Id { get; set; }
        public string Pervious_SetpId { get; set; }
        public string Next_SetpId { get; set; }
        public List<string> AuditJobsCode { get { return _AuditJobsCode; } }
        public List<string> AuditPersonCode { get { return _AuditPersonCode; } }
        public bool IsEndAudit { get; set; }
    }

    [Serializable]
    public class WF_Setps : List<WF_Setp>
    {
        //string parentId = "";
        ///// <summary>获取WF_ArraySetp类型的标示ID
        ///// </summary>
        //public string ParentId
        //{
        //    get { return parentId; }
        //    set { parentId = value; }
        //}
        //public WF_Setps()
        //{
        //    this.parentId = parentId;
        //}
        //public new void Add(WF_Setp item)
        //{
        //    if (item != null)
        //    {
        //        item.Parent_Id = parentId;
        //        base.Add(item);
        //    }
        //}

        //public new void AddRange(IEnumerable<WF_Setp> collection)
        //{
        //    if (collection != null)
        //    {
        //        foreach (var m in collection)
        //        {
        //            m.Parent_Id = parentId;
        //        }
        //        base.AddRange(collection);
        //    }
        //}

        //public new void Insert(int index, WF_Setp item)
        //{
        //    if (item != null)
        //    {
        //        item.Parent_Id = parentId;
        //        base.Insert(index, item);
        //    }
        //}

        //public new void InsertRange(int index, IEnumerable<WF_Setp> collection)
        //{
        //    if (collection != null)
        //    {
        //        foreach (var m in collection)
        //        {
        //            m.Parent_Id = parentId;
        //        }
        //        base.InsertRange(index, collection);
        //    }
        //}
    }
}
