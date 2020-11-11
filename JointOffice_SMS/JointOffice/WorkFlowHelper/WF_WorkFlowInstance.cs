using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.DbModel;

namespace JointOffice.WorkFlowHelper
{
    public partial class WF_WorkFlowInstanceHelper
    {
        public List<KeyValuePair<string, string>> WF_Flow_Parameter = new List<KeyValuePair<string, string>>();
        public virtual ICollection<WF_WorkFlowInstanceDetial> WF_WorkFlowInstanceDetial { get; set; }
    }
    public enum WF_InstanceState
    {
        /// <summary>
        /// 提交
        /// </summary>
        Submit = 0,
        /// <summary>
        /// 审核中
        /// </summary>
        Auditing = 1,
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 2,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 3,
        /// <summary>
        /// 撤销
        /// </summary>
        Revoke = 4
    }
    public enum WF_AuditState
    {
        /// <summary>
        /// 提交
        /// </summary>
        Submit = -1,
        /// <summary>
        /// 同意
        /// </summary>
        Agree = 1,
        /// <summary>
        /// 不同意
        /// </summary>
        DisAgree = 0,
        /// <summary>
        /// 结束
        /// </summary>
        End = 2
    }
}
