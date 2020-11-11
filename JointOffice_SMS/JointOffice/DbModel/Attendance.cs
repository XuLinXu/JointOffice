using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.DbModel
{    
    /// <summary>
    /// 签到  签退  详情
    /// </summary>
    public class Attendance_Check
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 当前用户ID
        /// </summary>
        [MaxLength(500)]

        public string MemberId { get; set; }

        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime CheckDate { get; set; }
        /// <summary>
        /// 类型 1出发 2交通中转 3到达客户 4离开客户 5上班签到 6下班签到
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 我的坐标
        /// </summary>
        public string Map { get; set; }
        /// <summary>
        /// 我的位置
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 备注内容
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 备注时间
        /// </summary>
        public string RemarksTime { get; set; }
        public int Mark { get; set; }
        public string OffWorkTime { get; set; }
        public string ToWorkTime { get; set; }
    }
    /// <summary>
    /// 备注详情
    /// </summary>
    public class Remark
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 当前用户ID
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }

        /// <summary>
        /// 用户查看日期  yyyy-MM-dd
        /// </summary>
        public string CheckDate { get; set; }
        /// <summary>
        /// 备注内容
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 备注时间
        /// </summary>
        public DateTime RemarksTime { get; set; }
        /// <summary>
        /// 类型 0无 1出发 2交通中转 3到达客户 4离开客户 5上班签到 6下班签到
        /// </summary>
        public int Type { get; set; }
    }
}
