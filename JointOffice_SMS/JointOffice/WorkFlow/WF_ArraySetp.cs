using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.WorkFlow
{
    [Serializable]
    public abstract class WF_ArraySetp : WF_Setp
    {
        public abstract WF_Setps WF_Setps { get; }
        public abstract WF_Setp GetStartWF_Setp { get; }
        public abstract void Add(WF_Setp item);
        public abstract void AddRange(IEnumerable<WF_Setp> collection);
        public abstract void Insert(WF_Setp sourceItem, WF_Setp item);
        //public abstract void InsertRange(int index, IEnumerable<WF_Setp> collection);
        public abstract void Delete(WF_Setp item);
        public abstract bool Container(WF_Setp item);
        public abstract WF_Setp Get_EndSetp();
    }
}
