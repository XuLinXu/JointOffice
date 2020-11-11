using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.DbModel
{
    public class WF_WorkFlowTemplate
    {
        [Key]
        [MaxLength(450)]
        public string WF_TemplateID { get; set; }
        public string WF_TemplateCategoryCode { get; set; }
        public int WF_Version { get; set; }
        public string WF_Name { get; set; }
        public string WF_DESC { get; set; }
        public byte[] WF_TemplateContent { get; set; }
        public int active_flag { get; set; }
        public int delete_flag { get; set; }
        public string creater { get; set; }
        public DateTime createtime { get; set; }
        public string modifyer { get; set; }
        public DateTime modefytime { get; set; }
        public bool isDelete { get; set; }
    }
    public class WF_WorkFlowInstance
    {
        [Key]
        [MaxLength(450)]
        public string WF_InstanceID { get; set; }
        public string WF_SourceObject { get; set; }
        public string WF_ObjectId { get; set; }
        public string WF_CurrentSetp { get; set; }
        public int WF_CurrentSetpState { get; set; }
        public int WF_InstanceState { get; set; }
        public string WF_TemplateID { get; set; }
        public int active_flag { get; set; }
        public int delete_flag { get; set; }
        public string creater { get; set; }
        public DateTime createtime { get; set; }
        public string modifyer { get; set; }
        public DateTime modefytime { get; set; }
        public byte[] TST { get; set; }
        public string WF_CurrentSetp_AuditJobs { get; set; }
        public string WF_CurrentSetp_AuditUserId { get; set; }
        public string Creater_Job { get; set; }
        public string WF_CurrentStepMiaoShu { get; set; }
    }
    public class WF_WorkFlowInstanceDetial
    {
        [Key]
        [MaxLength(450)]
        public string ID { get; set; }
        public string WF_InstanceID { get; set; }
        public string WF_SetpID { get; set; }
        public short WF_AuditState { get; set; }
        public string WF_AuditContent { get; set; }
        public string WF_AuditJobName { get; set; }
        public string WF_AuditUserName { get; set; }
        public DateTime WF_AuditTime { get; set; }
        public string creater { get; set; }
        public DateTime createtime { get; set; }
        public string modifyer { get; set; }
        public DateTime modefytime { get; set; }
    }
}
