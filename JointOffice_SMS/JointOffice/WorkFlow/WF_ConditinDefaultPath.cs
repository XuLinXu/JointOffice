using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.WorkFlow
{
    [Serializable]
    public class WF_ConditinDefaultPath : WF_ConditionPath
    {
        public override string Condition
        {
            get
            {
                return "1==1";
            }
            set
            {
                base.Condition = value;
            }
        }
    }
}
