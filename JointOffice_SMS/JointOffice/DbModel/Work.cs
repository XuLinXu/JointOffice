using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.DbModel
{
    /// <summary>
    /// 审批
    /// </summary>
    public class Work_Approval
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 审批类型 1加班 2出差 3请假 4出差报销 5普通报销
        /// </summary>
        [MaxLength(500)]
        public string Type { get; set; }
        /// <summary>
        /// 审批内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        public string ApprovalPerson { get; set; }
        /// <summary>
        /// 抄送范围  全部的人  存储过程筛选用
        /// </summary>
        public string Range { get; set; }
        /// <summary>
        /// 抄送范围  人和部门  前端展示用
        /// </summary>
        public string RangeNew { get; set; }
        /// <summary>
        /// 地图
        /// </summary>
        [MaxLength(500)]
        public string Map { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(500)]
        public string Address { get; set; }
        /// <summary>
        /// 加班总时长
        /// </summary>
        public string WorkDuration { get; set; }
        /// <summary>
        /// 加班
        /// </summary>
        public string OverTime { get; set; }
        /// <summary>
        /// 出差总详情
        /// </summary>
        public string TravelAll { get; set; }
        /// <summary>
        /// 出差分详情
        /// </summary>
        public string Travel { get; set; }
        /// <summary>
        /// 请假
        /// </summary>
        public string Leave { get; set; }
        /// <summary>
        /// 出差报销
        /// </summary>
        public string TravelReb { get; set; }
        /// <summary>
        /// 普通报销
        /// </summary>
        public string Reb { get; set; }
        /// <summary>
        /// 请假总时长
        /// </summary>
        public string LeaveDuration { get; set; }
        /// <summary>
        /// 出差报销总金额
        /// </summary>
        public string TravelMoney { get; set; }
        /// <summary>
        /// 普通报销总金额
        /// </summary>
        public string RebMoney { get; set; }
        /// <summary>
        /// 是否是草稿 1是 0否
        /// </summary>
        public int IsDraft { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string Voice { get; set; }
        /// <summary>
        /// 录音长度
        /// </summary>
        public string VoiceLength { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string Annex { get; set; }
        /// <summary>
        /// 网盘
        /// </summary>
        public string WangPanJson { get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string ATPerson { get; set; }
        /// <summary>
        /// 主表状态  0审批中 1已批准 2已取消 3未批准 4可重新指定审批人
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 轮到第几个审批人了
        /// </summary>
        public int ApprovalPersonNum { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
        /// <summary>
        /// 审批编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 发布人发布此工作时所选择的公司
        /// </summary>
        public string CompanyId { get; set; }
    }
    /// <summary>
    /// 日志
    /// </summary>
    public class Work_Log
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 日志模板  1日计划 2周计划 3月计划
        /// </summary>
        [MaxLength(500)]
        public string MoBan { get; set; }
        /// <summary>
        /// 模板所对应的时间
        /// </summary>
        public string MoBanTime { get; set; }
        /// <summary>
        /// 工作总结
        /// </summary>
        public string WorkSummary { get; set; }
        /// <summary>
        /// 工作计划
        /// </summary>
        public string WorkPlan { get; set; }
        /// <summary>
        /// 体会
        /// </summary>
        public string Experience { get; set; }
        /// <summary>
        /// 抄送范围  全部的人  存储过程筛选用
        /// </summary>
        public string Range { get; set; }
        /// <summary>
        /// 抄送范围  人和部门  前端展示用
        /// </summary>
        public string RangeNew { get; set; }
        /// <summary>
        /// 回执范围
        /// </summary>
        public string Receipt { get; set; }
        /// <summary>
        /// 点评人姓名
        /// </summary>
        [MaxLength(500)]
        public string ReviewPersonName { get; set; }
        /// <summary>
        /// 点评人ID
        /// </summary>
        [MaxLength(500)]
        public string ReviewPersonId { get; set; }
        /// <summary>
        /// 地图
        /// </summary>
        [MaxLength(500)]
        public string Map { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(500)]
        public string Address { get; set; }
        /// <summary>
        /// 是否是草稿 1是 0否
        /// </summary>
        public int IsDraft { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string Voice { get; set; }
        /// <summary>
        /// 录音长度
        /// </summary>
        public string VoiceLength { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string Annex { get; set; }
        /// <summary>
        /// 网盘
        /// </summary>
        public string WangPanJson { get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string ATPerson { get; set; }
        /// <summary>
        /// 主表状态  0未点评 1已点评 2已删除
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
        /// <summary>
        /// 费用明细
        /// </summary>
        public string MoneyInfo { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>
        public string Money { get; set; }
        /// <summary>
        /// TMS推送状态
        /// </summary>
        public int? TMSstate { get; set; }
        /// <summary>
        /// 发布人发布此工作时所选择的公司
        /// </summary>
        public string CompanyId { get; set; }
    }
    /// <summary>
    /// 任务
    /// </summary>
    public class Work_Task
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 任务标题
        /// </summary>
        [MaxLength(500)]
        public string TaskTitle { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public string StopTime { get; set; }
        /// <summary>
        /// 提醒时间
        /// </summary>
        public string RemindTime { get; set; }
        /// <summary>
        /// 地图
        /// </summary>
        [MaxLength(500)]
        public string Map { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(500)]
        public string Address { get; set; }
        /// <summary>
        /// 抄送范围  全部的人  存储过程筛选用
        /// </summary>
        public string Range { get; set; }
        /// <summary>
        /// 抄送范围  人和部门  前端展示用
        /// </summary>
        public string RangeNew { get; set; }
        /// <summary>
        /// 是否是草稿 1是 0否
        /// </summary>
        public int IsDraft { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string Voice { get; set; }
        /// <summary>
        /// 录音长度
        /// </summary>
        public string VoiceLength { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string Annex { get; set; }
        /// <summary>
        /// 网盘
        /// </summary>
        public string WangPanJson { get; set; }
        /// <summary>
        /// 执行人信息
        /// </summary>
        public string Executor{ get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string ATPerson { get; set; }
        /// <summary>
        /// 主表状态  0待执行 1已执行 2已取消
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
        /// <summary>
        /// 发布人发布此工作时所选择的公司
        /// </summary>
        public string CompanyId { get; set; }
    }
    /// <summary>
    /// 日程
    /// </summary>
    public class Work_Program
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 日程内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 年月日
        /// </summary>
        [MaxLength(500)]
        public string Year { get; set; }
        /// <summary>
        /// 时分
        /// </summary>
        [MaxLength(500)]
        public string Hour { get; set; }
        /// <summary>
        /// 参与人
        /// </summary>
        public string JoinPerson { get; set; }
        /// <summary>
        /// 回执范围
        /// </summary>
        public string Receipt { get; set; }
        /// <summary>
        /// 提醒时间
        /// </summary>
        public string RemindTime { get; set; }
        /// <summary>
        /// 抄送范围
        /// </summary>
        public string Range { get; set; }
        /// <summary>
        /// 地图
        /// </summary>
        [MaxLength(500)]
        public string Map { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(500)]
        public string Address { get; set; }
        /// <summary>
        /// 是否是草稿 1是 0否
        /// </summary>
        public int IsDraft { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string Voice { get; set; }
        /// <summary>
        /// 录音长度
        /// </summary>
        public string VoiceLength { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string Annex { get; set; }
        /// <summary>
        /// 网盘
        /// </summary>
        public string WangPanJson { get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string ATPerson { get; set; }
        /// <summary>
        /// 主表状态  0未开始 1已开始 2已取消
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
        /// <summary>
        /// 发布人发布此工作时所选择的公司
        /// </summary>
        public string CompanyId { get; set; }
    }
    /// <summary>
    /// 指令
    /// </summary>
    public class Work_Order
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 指令内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public string StopTime { get; set; }
        /// <summary>
        /// 抄送范围  全部的人  存储过程筛选用
        /// </summary>
        public string Range { get; set; }
        /// <summary>
        /// 抄送范围  人和部门  前端展示用
        /// </summary>
        public string RangeNew { get; set; }
        /// <summary>
        /// 地图
        /// </summary>
        [MaxLength(500)]
        public string Map { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(500)]
        public string Address { get; set; }
        /// <summary>
        /// 是否是草稿 1是 0否
        /// </summary>
        public int IsDraft { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string Voice { get; set; }
        /// <summary>
        /// 录音长度
        /// </summary>
        public string VoiceLength { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string Annex { get; set; }
        /// <summary>
        /// 网盘
        /// </summary>
        public string WangPanJson { get; set; }
        /// <summary>
        /// 执行人信息
        /// </summary>
        public string Executor { get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string ATPerson { get; set; }
        /// <summary>
        /// 主表状态  0执行中 1已执行 2已取消
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
        /// <summary>
        /// 发布人发布此工作时所选择的公司
        /// </summary>
        public string CompanyId { get; set; }
    }
    /// <summary>
    /// 公告
    /// </summary>
    public class Work_Announcement
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 公告标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 公告内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 公示开始日期
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 公示结束日期
        /// </summary>
        public DateTime StopTime { get; set; }
        /// <summary>
        /// 抄送范围  全部的人  存储过程筛选用
        /// </summary>
        public string Range { get; set; }
        /// <summary>
        /// 抄送范围  人和部门  前端展示用
        /// </summary>
        public string RangeNew { get; set; }
        /// <summary>
        /// 回执范围
        /// </summary>
        public string Receipt { get; set; }
        /// <summary>
        /// 地图
        /// </summary>
        [MaxLength(500)]
        public string Map { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(500)]
        public string Address { get; set; }
        /// <summary>
        /// 是否是草稿 1是 0否
        /// </summary>
        public int IsDraft { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string Voice { get; set; }
        /// <summary>
        /// 录音长度
        /// </summary>
        public string VoiceLength { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string Annex { get; set; }
        /// <summary>
        /// 网盘
        /// </summary>
        public string WangPanJson { get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string ATPerson { get; set; }
        /// <summary>
        /// 主表状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
        /// <summary>
        /// 发布人发布此工作时所选择的公司
        /// </summary>
        public string CompanyId { get; set; }
    }
    /// <summary>
    /// 分享
    /// </summary>
    public class Work_Share
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 分享内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 抄送范围  全部的人  存储过程筛选用
        /// </summary>
        public string Range { get; set; }
        /// <summary>
        /// 抄送范围  人和部门  前端展示用
        /// </summary>
        public string RangeNew { get; set; }
        /// <summary>
        /// 回执范围
        /// </summary>
        public string Receipt { get; set; }
        /// <summary>
        /// 地图
        /// </summary>
        [MaxLength(500)]
        public string Map { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(500)]
        public string Address { get; set; }
        /// <summary>
        /// 是否是草稿 1是 0否
        /// </summary>
        public int IsDraft { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string Voice { get; set; }
        /// <summary>
        /// 录音长度
        /// </summary>
        public string VoiceLength { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string Annex { get; set; }
        /// <summary>
        /// 网盘
        /// </summary>
        public string WangPanJson { get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string ATPerson { get; set; }
        /// <summary>
        /// 主表状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
        /// <summary>
        /// 发布人发布此工作时所选择的公司
        /// </summary>
        public string CompanyId { get; set; }
    }
    /// <summary>
    /// 回执详情
    /// </summary>
    public class Receipts
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 回执人ID
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 被回执的主表的创建人的ID
        /// </summary>
        public string OtherMemberId { get; set; }
        /// <summary>
        /// 被回执的动态类型(2日志 4日程 8公告 9分享)
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 被回执的主表ID
        /// </summary>
        public string UId { get; set; }
        /// <summary>
        /// 回执内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
    }
    /// <summary>
    /// 文件流
    /// </summary>
    public class File
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(450)]
        public string UId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string Url { get; set; }
        [MaxLength(500)]
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        [MaxLength(500)]
        /// <summary>
        /// 文件类型
        /// </summary>
        public string Type { get; set; }
    }
    /// <summary>
    /// 创建审批详情
    /// </summary>
    public class Approval_Content
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 审批创建人
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 审批ID
        /// </summary>
        [MaxLength(450)]
        public string UId { get; set; }
        /// <summary>
        /// 审批人ID
        /// </summary>
        [MaxLength(500)]
        public string OtherMemberId { get; set; }
        /// <summary>
        /// 审批人姓名
        /// </summary>
        [MaxLength(500)]
        public string OtherMemberName { get; set; }
        /// <summary>
        /// 审批人头像
        /// </summary>
        public string OtherMemberPicture { get; set; }
        /// <summary>
        /// 审批人在此条审批中所处的次序
        /// </summary>
        public int OtherMemberOrder { get; set; }
        /// <summary>
        /// 是否轮到当前审批人审批  1是 0否
        /// </summary>
        public string IsMeApproval { get; set; }
        /// <summary>
        /// 审批类型 1加班 2出差 3请假 4出差报销 5普通报销
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 此条审批创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 审批状态 0审批中 1已批准 2已取消 3未批准 5待审批 6已失效
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 审批人回复内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 审批人手签名
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        public string ApprovalTime { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
    }
    /// <summary>
    /// 创建执行详情
    /// </summary>
    public class Execute_Content
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 执行创建人
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 执行类型  3任务 5指令
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 任务 指令 ID
        /// </summary>
        [MaxLength(450)]
        public string UId { get; set; }
        /// <summary>
        /// 执行人id
        /// </summary>
        [MaxLength(500)]
        public string OtherMemberId { get; set; }
        /// <summary>
        /// 执行人姓名
        /// </summary>
        [MaxLength(500)]
        public string OtherMemberName { get; set; }
        /// <summary>
        /// 执行人头像
        /// </summary>
        public string OtherMemberPicture{ get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 执行状态 0执行中 1已执行 2已取消 3已失效
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 执行时间  拒绝执行时间
        /// </summary>
        public string ExecuteDate { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
    }
    /// <summary>
    /// 评论内容详情
    /// </summary>
    public class Comment_Body
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 评论人ID
        /// </summary>
        [MaxLength(500)]
        public string PingLunMemberId { get; set; }
        /// <summary>
        /// 评论人姓名
        /// </summary>
        [MaxLength(500)]
        public string Name { get; set; }
        /// <summary>
        /// 评论人头像
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime PingLunTime { get; set; }
        /// <summary>
        /// 主表ID
        /// </summary>
        [MaxLength(450)]
        public string UId { get; set; }
        /// <summary>
        /// 主表创建人ID
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 主表的评论的ID
        /// </summary>
        [MaxLength(450)]
        public string PId { get; set; }
        /// <summary>
        /// PID的内容
        /// </summary>
        public string OtherBody { get; set; }
        /// <summary>
        /// PID的人的ID
        /// </summary>
        [MaxLength(500)]
        public string PersonId { get; set; }
        /// <summary>
        /// PID的人的姓名
        /// </summary>
        [MaxLength(500)]
        public string PersonName { get; set; }
        /// <summary>
        /// 评论的对象的类型 (1审批 2日志 3任务 4日程 5指令 6群通知 7普通评论 8公告 9分享)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 是否为执行人或创建人或回执人对此条动态的执行行为的回复  1是 0不是
        /// </summary>
        public int IsExeComment { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string PictureList { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string Annex { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string Voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string VoiceLength { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string ATPerson { get; set; }
        /// <summary>
        /// 是否已读标记
        /// </summary>
        public bool IsRead { get; set; }
    }
    /// <summary>
    /// 点评详情
    /// </summary>
    public class DianPing_Body
    {
        /// <summary>
        /// 点评ID
        /// </summary>
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 点评人ID
        /// </summary>
        [MaxLength(500)]
        public string DianPingMemberId { get; set; }
        /// <summary>
        /// 点评人姓名
        /// </summary>
        [MaxLength(500)]
        public string Name { get; set; }
        /// <summary>
        /// 点评人头像
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 点评内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 点评时间
        /// </summary>
        public DateTime DianPingTime { get; set; }
        /// <summary>
        /// 主表ID
        /// </summary>
        [MaxLength(450)]
        public string UId { get; set; }
        /// <summary>
        /// 主表创建人ID
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 评论的对象的类型 (2日志 3任务 5指令)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 星级
        /// </summary>
        public string Grade { get; set; }
        /// <summary>
        /// 状态  0未点评 1已点评 2已取消
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string PictureList { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string Voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string VoiceLength { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string Annex { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string ATPerson { get; set; }
        /// <summary>
        /// 是否已读标记
        /// </summary>
        public bool IsRead { get; set; }
    }
    /// <summary>
    /// 日程取消记录
    /// </summary>
    public class EscProgram
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 参与人ID==当前用户
        /// </summary>
        [MaxLength(500)]
        public string JoinPersonMemberId { get; set; }
        /// <summary>
        /// 主表ID
        /// </summary>
        [MaxLength(450)]
        public string UId { get; set; }
        /// <summary>
        /// 主表创建人ID
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 日程取消时间
        /// </summary>
        public DateTime EscTime { get; set; }
        /// <summary>
        /// 取消前参与人
        /// </summary>
        public string BeforeEscJoinPerson { get; set; }
        /// <summary>
        /// 取消后参与人
        /// </summary>
        public string AfterEscJoinPerson { get; set; }
    }
    /// <summary>
    /// 统计点赞,评论,转发数
    /// </summary>
    public class TotalNum
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 类型(1审批 2日志 3任务 4日程 5指令 6群通知 7评论 8公告 9分享)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 类型对应的主表ID
        /// </summary>
        public string UId { get; set; }
        /// <summary>
        /// 评论ID
        /// </summary>
        public string PId { get; set; }
        /// <summary>
        /// 评论的UID
        /// </summary>
        public string P_UId { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int DianZanNum { get; set; }
        /// <summary>
        /// 转发数
        /// </summary>
        public int ZhuanFaNum { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int PingLunNum { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
    /// <summary>
    /// 点赞详情
    /// </summary>
    public class Agree
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 点赞人ID
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 被点赞人的ID
        /// </summary>
        public string OtherMemberId { get; set; }
        /// <summary>
        /// 被点赞的动态类型(1审批 2日志 3任务 4日程 5指令 6群通知 7评论 8公告 9分享)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 被点赞的动态类型对应的主表ID
        /// </summary>
        public string UId { get; set; }
        /// <summary>
        /// 普通评论ID
        /// </summary>
        public string PId { get; set; }
        /// <summary>
        /// 评论的UID
        /// </summary>
        public string P_UId { get; set; }
        /// <summary>
        /// 被点赞的动态的内容   前20个字+...
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
        /// <summary>
        /// 是否已读标记
        /// </summary>
        public bool IsRead { get; set; }
    }
    /// <summary>
    /// 工作列表标签顺序
    /// </summary>
    public class WorkListTag
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 人ID
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 标签顺序
        /// </summary>
        public string WorkListTagDES { get; set; }
    }
}
