using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.DbModel
{
    /// <summary>
    /// 聊天记录
    /// </summary>
    public class News_News
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 聊天类型  1个人 2群组
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 群组ID
        /// </summary>
        public string GroupId { get; set; }
        /// <summary>
        /// 消息发送方
        /// </summary>
        [MaxLength(500)]
        public string NewsSenderId { get; set; }
        ///// <summary>
        ///// 消息接收方
        ///// </summary>
        //public string NewsReceiverId { get; set; }
        /// <summary>
        /// 此条消息谁不可见
        /// </summary>
        public string NoSeePerson { get; set; }
        /// <summary>
        /// 我的坐标
        /// </summary>
        public string Map { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 我的位置
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 消息类型 1 普通文本  2 图片 3文档   4  录音  5 视频  6位置
        /// </summary>
        public string InfoType { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public string Length { get; set; }
        /// <summary>
        /// base64wei
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// 能看到这条消息的人员
        /// </summary>
        public string SeePerson { get; set; }
        ///// <summary>
        ///// 1 日志  2通知
        ///// </summary>
        //public string Mold { get; set; }
        //public string MoldId { get; set; }
    }
    public class News_Member
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 人员id 
        /// </summary>
        public string MemberId { get; set; }
        /// <summary>
        /// 群组id
        /// </summary>
        public string GroupId { get; set; }
        /// <summary>
        /// 成员MemberID
        /// </summary>
        public string GroupPersonId { get; set; }
        /// <summary>
        /// 成员MemberID
        /// </summary>
        public string DeleteGroupPersonId { get; set; }
        /// <summary>
        /// 未读数量
        /// </summary>
        public string WeiDuGroupPersonId { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateDate { get; set; }

    }
    /// <summary>
    /// 群通知
    /// </summary>
    public class News_GroupNotice
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 群通知创建人ID
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 抄送范围
        /// </summary>
        public string Range { get; set; }
        /// <summary>
        /// 群标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 群内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 是否需要确认  1需要 0不需要
        /// </summary>
        public string IsConfirm { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }
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
        /// @某人
        /// </summary>
        public string AtPerson { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string PhoneModel { get; set; }
    }
    /// <summary>
    /// 个人  群通知详情
    /// </summary>
    public class News_GroupNotice_Content
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 群通知接受人ID
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 群通知ID
        /// </summary>
        [MaxLength(450)]
        public string GroupNoticeId { get; set; }
        /// <summary>
        /// 此群通知是否需要确认  1需要 0不需要
        /// </summary>
        public string IsConfirm { get; set; }
        /// <summary>
        /// 是否已确认  1已读 2已确认
        /// </summary>
        public string IfYesIsConfirm { get; set; }
        /// <summary>
        /// 确认/读取时间
        /// </summary>
        public string ConfirmTime { get; set; }
    }
    /// <summary>
    /// 收藏
    /// </summary>
    public class News_Collection
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 当前用户ID  收藏人
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 收藏对象的ID
        /// </summary>
        [MaxLength(450)]
        public string UId { get; set; }
        /// <summary>
        /// 主表创建人ID
        /// </summary>
        [MaxLength(500)]
        public string OtherMemberID { get; set; }
        /// <summary>
        /// 收藏对象的类型 1审批 2日志 3任务 4日程 5指令 6群通知
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 收藏对象的内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 收藏时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 标签信息  id  name
        /// </summary>
        public string MarkInfo { get; set; }
    }
    /// <summary>
    /// 标签列表
    /// </summary>
    public class News_Mark
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
        /// 标签名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 标签标记的主表   id  type
        /// </summary>
        public string MarkUId { get; set; }
    }
    /// <summary>
    /// 关注
    /// </summary>
    public class News_Focus
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
        /// 关注对象的ID
        /// </summary>
        [MaxLength(450)]
        public string UId { get; set; }
        /// <summary>
        /// 关注对象的类型 1审批 2日志 3任务 4日程 5指令 6群通知
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
