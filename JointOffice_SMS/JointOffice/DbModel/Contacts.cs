using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.DbModel
{
    public class Contacts_Star
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 被设为星标的人
        /// </summary>
        [MaxLength(500)]
        public string OtherMemberId { get; set; }
        /// <summary>
        /// 是否为星标  1是 0否
        /// </summary>
        public int IsStar { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
