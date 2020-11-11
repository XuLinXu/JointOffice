using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.DbModel;

namespace JointOffice.Models
{
    public interface INews
    {
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="群名称,群组人员ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge CreateGroup(CreateGroupPara para);
        /// <summary>
        /// 修改群组人员
        /// </summary>
        /// <param name="群组ID,群组类型,群组人员ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateGroup(UpdateGroupPara para);
        /// <summary>
        /// 修改群头像
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateGroupPicture(UpdateGroupPicturePara para);
        /// <summary>
        /// 修改群名称
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateGroupName(UpdateGroupNamePara para);
        /// <summary>
        /// 存储会话信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge SaveConversation(News_News para);
        /// <summary>
        /// 删除会话消息
        /// </summary>
        /// <param name="单条消息的ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge DeleteConversation(DeleteConversationInPara para);
        /// <summary>
        /// 清空会话消息
        /// </summary>
        /// <param name="会话ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge CleanConversation(CleanConversationInPara para);
        /// <summary>
        /// 显示   消息列表
        /// </summary>
        Showapi_Res_List<ConversationList> GetConversationList(GetConversationList para);
        /// <summary>
        /// 发群通知
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge SendGroupNotice(News_GroupNotice para);
        /// <summary>
        /// 删除群通知
        /// </summary>
        Showapi_Res_Meaasge DeleteGroupNotice(DeleteGroupNoticeInPara para);
        /// <summary>
        /// 确认读取群通知
        /// </summary>
        /// <param name="群通知ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge ConfirmReadGroupNotice(ConfirmReadGroupNoticeInPara para);
        /// <summary>
        /// 群通知列表
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<GroupNoticeList> GetGroupNoticeList(GroupNoticeListInPara para);
        /// <summary>
        /// 群通知详情
        /// </summary>
        Showapi_Res_Single<GroupNoticeBody> GetGroupNoticeBodyList(GroupNoticeBodyInPara para);
        /// <summary>
        /// 群通知读取/确认人数
        /// </summary>
        Showapi_Res_List<GroupNoticeConfirmNum> GetGroupNoticeConfirmNumList(GroupNoticeBodyInPara para);
        /// <summary>
        /// 收藏
        /// </summary>
        Showapi_Res_Meaasge Collection(CollectionInPara para);
        /// <summary>
        /// 修改收藏标签
        /// </summary>
        Showapi_Res_Meaasge UpdateCollection(UpdateCollectionInPara para);
        /// <summary>
        /// 收藏列表
        /// </summary>
        Showapi_Res_List<CollectionList> GetCollectionList();
        /// <summary>
        /// 我的收藏 Web
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<PersonDynamic_info> GetMyCollectionList(GetMyFocusInPara para);
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge NoCollection(NoCollectionInPara para);
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge InsertMark(InsertMarkInPara para);
        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="ID，name"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateMark(UpdateMarkInPara para);
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge DeleteMark(DeleteMarkInPara para);
        /// <summary>
        /// 标签列表
        /// </summary>
        Showapi_Res_List<MarkList> GetMarkList();
        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="类型,ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge Focus(FocusInPara para);
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge NoFocus(NoFocusInPara para);
        /// <summary>
        /// 我关注的
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<PersonDynamic_info> GetMyFocusList(GetMyFocusInPara para);
        /// <summary>
        /// @我的回复
        /// </summary>
        Showapi_Res_List<WorkReply> GetMyReplyList(GetATMyInPara para);
        /// <summary>
        /// 工作回复  Web
        /// </summary>
        Showapi_Res_List<WorkReply> GetWorkReplyWebList(AllSearch para);
        /// <summary>
        /// @我的工作
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<PersonDynamic_info> GetATMyWorkList(GetATMyInPara para);
        /// <summary>
        /// 我收到的赞
        /// </summary>
        Showapi_Res_List<MyZan> GetMyZanList(MyZanInPara para);
        /// <summary>
        /// 我的赞  收到+发出
        /// </summary>
        Showapi_Res_List<MyZan> GetMyZanWebList(AllSearch para);
        /// <summary>
        /// 我的回执
        /// </summary>
        Showapi_Res_List<PersonDynamic_info> GetMyReceiptList(AllSearch para);
        /// <summary>
        /// 各种提醒的数量
        /// </summary>
        Showapi_Res_List<NewsRemindNum> GetNewsRemindNumList();
        /// <summary>
        /// 审批提醒
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<DaiShenPi> GetApprovalRemindList(ApprovalRemindInPara para);
        /// <summary>
        /// 日志提醒
        /// </summary>
        /// <param name="类型"></param>
        /// <returns></returns>
        Showapi_Res_List<DaiDianPingDeRiZhi> GetLogRemindList(LogRemindInPara para);
        /// <summary>
        /// 我执行的任务
        /// </summary>
        Showapi_Res_List<DaiZhiXingDeRenWu> GetIDoTaskList(IDoTaskInPara para);
        /// <summary>
        /// 我发出的任务
        /// </summary>
        Showapi_Res_List<DaiZhiXingDeRenWu> GetIPublishTaskList(IPublishTaskInPara para);
        /// <summary>
        /// 日程列表
        /// </summary>
        /// <param name="年月"></param>
        /// <returns></returns>
        Showapi_Res_Single<ProgramList> GetProgramList(ProgramListInPara para);
        /// <summary>
        /// 日程通知
        /// </summary>
        /// <param name="日程状态"></param>
        /// <returns></returns>
        Showapi_Res_List<RiChengDetail> GetProgramNoticeList(ProgramNoticeInPara para);
        /// <summary>
        /// 指令提醒
        /// </summary>
        /// <param name="类型"></param>
        /// <returns></returns>
        Showapi_Res_List<ZhiLingDetail> GetOrderRemindList(OrderRemindInPara para);
        /// <summary>
        /// 取消(指令,审批,任务)
        /// </summary>
        Showapi_Res_Meaasge Cancel(CancelInPara para);
        /// <summary>
        /// 删除(1审批 2日志 3任务 4日程 5指令)
        /// </summary>
        Showapi_Res_Meaasge Delete(DeleteInPara para);
        /// <summary>
        /// 群文档
        /// </summary>
        Showapi_Res_List<GroupDocument> GetGroupDocumentList(GroupDocumentInPara para);
        /// <summary>
        /// 全图片
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<AllPicture> GetAllPictureList(GroupDocumentInPara para);
        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_List<NewsList> GetNewsList(GetNewsListPara para);
        /// <summary>
        /// 群组信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_Single<NewsList> GetNewsInfo(CleanConversationInPara para);
        /// <summary>
        /// 获取聊天信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_List<NewsInfoList> GetNewsInfoList(GetNewsInfoListPara para);
        /// <summary>
        /// 删除会话窗口
        /// </summary>
        /// <param name="单条消息的ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge DeleteNewsList(DeleteNewsListPara para);
        /// <summary>
        /// 获取人员和权限信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_Single<GetNewsPersonInfoList> GetNewsPersonInfo(CleanConversationInPara para);
        /// <summary>
        /// 解散群
        /// </summary>
        /// <param name="单条消息的ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge DissolutionGroup(CleanConversationInPara para);
        /// <summary>
        /// 搜索聊天记录
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_List<SearchNewsInfoList> SearchNewsInfoList(SearchNewsInfoListPara para);
        /// <summary>
        /// 根据memberid获取头像名称
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_Single<GetNamePictureInfo> GetNamePicture(GetNamePicturePara para);
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge NewCreateGroup(CreateGroupPara para);
        /// <summary>
        /// 分公司显示各种提醒的数量
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<GetNewsRemindNumListCompany> GetNewsRemindNumListCompany(GetNewsRemindNumListCompanyInPara para);
        /// <summary>
        /// 获取当前用户某聊天未读数量
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<GetPersonNewsNotReadNum> GetPersonNewsNotReadNum(GetPersonNewsNotReadNumInPara para);
        /// <summary>
        /// 获取当前用户全部聊天未读数量
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<GetPersonNewsNotReadAllNum> GetPersonNewsNotReadAllNum();
        /// <summary>
        /// 各种提醒的数量  web
        /// </summary>
        Showapi_Res_List<NewsRemindNum> GetNewsRemindNumListCompanyWeb(UpdateMember_info para);
    }
    /// <summary>
    /// 创建群组
    /// </summary>
    public class CreateGroupPara
    {
        /// <summary>
        /// 群组名
        /// </summary>
        public string name { get; set; }
        ///// <summary>
        ///// 群组名
        ///// </summary>
        //public string groupId { get; set; }
        /// <summary>
        /// 群组人员id
        /// </summary>
        public List<string> memberidlist { get; set; }
    }
    /// <summary>
    /// 修改群组人员
    /// </summary>
    public class UpdateGroupPara
    {
        public string groupid { get; set; }
        /// <summary>
        /// 增加人员还是减少人员  1增加 2减少
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 要添加或删除的人的ID
        /// </summary>
        public List<string> memberidlist { get; set; }
    }
    /// <summary>
    /// 修改群名称,群头像  入参
    /// </summary>
    public class UpdateGroupPicturePara
    {
        /// <summary>
        /// 群ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 群新头像
        /// </summary>
        public string picture { get; set; }
    }
    public class UpdateGroupNamePara
    {
        /// <summary>
        /// 群ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 群新名称
        /// </summary>
        public string name { get; set; }
    }
    /// <summary>
    /// 存储聊天记录  入参
    /// </summary>
    /// <returns></returns>
    public class SaveConversationInPara
    {
        /// <summary>
        /// 会话类型  1个人 2 群组
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 群组ID
        /// </summary>
        public string groupId { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 我的坐标
        /// </summary>
        public string map { get; set; }
        /// <summary>
        /// 我的位置
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 1 普通文本  2 图片  3文档  3 录音  4视频
        /// </summary>
        public string infoType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string name { get; set; }
    }
    /// <summary>
    /// 删除聊天记录  入参
    /// </summary>
    /// <param name="单条消息的ID"></param>
    /// <returns></returns>
    public class DeleteConversationInPara
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public string id { get; set; }
    }
    public class DeleteNewsListPara
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 个人1  群组2
        /// </summary>
        public string type { get; set; }
    }
    /// <summary>
    /// 清空聊天记录  入参
    /// </summary>
    /// <returns></returns>
    public class CleanConversationInPara
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        public string id { get; set; }
    }
    /// <summary>
    /// 显示   消息列表
    /// </summary>
    public class GetConversationList
    {
        public string id { get; set; }
        public int page { get; set; }
        public int count { get; set; }
    }
    public class GetNewsInfoListPara
    {
        public string id { get; set; }
        public int type { get; set; }
        public int page { get; set; }
        public int count { get; set; }
        public string time { get; set; }
        public string body { get; set; }
    }
    /// <summary>
    /// 显示   消息列表
    /// </summary>
    public class ConversationList
    {
        public string id { get; set; }
        public string body { get; set; }
        public string date { get; set; }
    }
    /// <summary>
    /// 删除群通知  入参
    /// </summary>
    public class DeleteGroupNoticeInPara
    {
        public string id { get; set; }
    }
    /// <summary>
    /// 确认读取群通知  入参
    /// </summary>
    public class ConfirmReadGroupNoticeInPara
    {
        /// <summary>
        /// 群通知ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 确认/读取 标记  1已读 2已确认
        /// </summary>
        public string mark { get; set; }
    }
    /// <summary>
    /// 群通知列表  入参
    /// </summary>
    /// <returns></returns>
    public class GroupNoticeListInPara
    {
        /// <summary>
        /// 1我收到的  2我发出的  3我未读的
        /// </summary>
        public string type { get; set; }
        public int page { get; set; }
        public int count { get; set; }
    }
    /// <summary>
    /// 群通知列表
    /// </summary>
    /// <returns></returns>
    public class GroupNoticeList
    {
        /// <summary>
        /// 群通知创建人ID
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 群通知创建人姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 群通知创建人头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 群通知ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 群通知标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 群通知内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 群通知创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 已确认/已读(1/2)
        /// </summary>
        public string confirmed { get; set; }
    }
    /// <summary>
    /// 群通知详情  入参
    /// </summary>
    public class GroupNoticeBodyInPara
    {
        /// <summary>
        /// 群通知ID
        /// </summary>
        public string id { get; set; }
    }
    /// <summary>
    /// 群通知详情
    /// </summary>
    public class GroupNoticeBody
    {
        /// <summary>
        /// 群通知创建人ID
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 群通知创建人姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 群通知创建人头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 群通知创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 是否需要确认 1需要 0不需要
        /// </summary>
        public string isConfirm { get; set; }
        /// <summary>
        /// 已确认(1/2)
        /// </summary>
        public string confirmed { get; set; }
        /// <summary>
        /// 群通知内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 群通知标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 点评数量
        /// </summary>
        public int? pingLunNum { get; set; }
        /// <summary>
        /// 是否赞 1是 0否
        /// </summary>
        public int? isZan { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? dianZanNum { get; set; }
        /// <summary>
        /// 抄送范围
        /// </summary>
        public string range { get; set; }
        /// <summary>
        /// 附加图片
        /// </summary>
        public List<PersonDynamic_info_url> appendPicture { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<PersonDynamic_info_url> annex { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string voiceLength { get; set; }
    }
    /// <summary>
    /// 群通知读取/确认人数
    /// </summary>
    public class GroupNoticeConfirmNum
    {
        /// <summary>
        /// 群通知接受人的ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 群通知接受人的姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 群通知接受人的头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 读取/确认标记  1已读 2已确认 
        /// </summary>
        public string confirmMark { get; set; }
        /// <summary>
        /// 读取/确认时间
        /// </summary>
        public string confirmTime { get; set; }
    }
    /// <summary>
    /// 收藏  入参
    /// </summary>
    public class CollectionInPara
    {
        /// <summary>
        /// 类型   1审批 2日志 3任务 4日程 5指令 6群通知 8公告 9分享
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 收藏类型的ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 主表创建人ID
        /// </summary>
        public string otherMemberid { get; set; }
        /// <summary>
        /// 标签信息
        /// </summary>
        public List<MarkInfoPara> markInfo { get; set; }
    }
    /// <summary>
    /// 修改收藏列表  入参
    /// </summary>
    public class UpdateCollectionInPara
    {
        /// <summary>
        /// 收藏类型的ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 1添加标签  2删除标签
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 收藏添加的标签
        /// </summary>
        public List<MarkInfoPara> addMarkInfo { get; set; }
        /// <summary>
        /// 收藏删除的标签
        /// </summary>
        public List<MarkInfoPara> deleteMarkInfo { get; set; }
    }
    /// <summary>
    /// 标签信息
    /// </summary>
    public class MarkInfoPara
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        public string markId { get; set; }
        /// <summary>
        /// 标签内容
        /// </summary>
        public string markName { get; set; }
    }
    /// <summary>
    /// 标签标记的主表信息
    /// </summary>
    public class MarkUId
    {
        /// <summary>
        /// 主表ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 主表类型
        /// </summary>
        public string type { get; set; }
    }
    /// <summary>
    /// 收藏列表
    /// </summary>
    public class CollectionList
    {
        /// <summary>
        /// 类型(审批 日志 任务 日程 指令 群通知)
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 对应类型的ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 对应类型的内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 主表创建人ID
        /// </summary>
        public string otherMemberId { get; set; }
        /// <summary>
        /// 主表创建人头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 主表创建人姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 收藏时间
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 标签信息
        /// </summary>
        public List<MarkInfoPara> markInfo { get; set; }
    }
    /// <summary>
    /// 取消收藏  入参
    /// </summary>
    public class NoCollectionInPara
    {
        /// <summary>
        /// 类型(1审批 2日志 3任务 4日程 5指令 6群通知 8公告 9分享)的ID
        /// </summary>
        public string id { get; set; }
    }
    /// <summary>
    /// 添加标签  入参
    /// </summary>
    public class InsertMarkInPara
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 标签标记的类型ID
        /// </summary>
        public string id { get; set; }
    }
    /// <summary>
    /// 修改标签  入参
    /// </summary>
    public class UpdateMarkInPara
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 标签新名称
        /// </summary>
        public string name { get; set; }
    }
    /// <summary>
    /// 删除标签  入参
    /// </summary>
    public class DeleteMarkInPara
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        public string id { get; set; }
    }
    /// <summary>
    /// 标签列表
    /// </summary>
    public class MarkList
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string name { get; set; }
    }
    /// <summary>
    /// 关注  入参
    /// </summary>
    public class FocusInPara
    {
        /// <summary>
        /// 类型(1审批 2日志 3任务 4日程 5指令 6群通知 8公告 9分享)
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 类型(1审批 2日志 3任务 4日程 5指令 6群通知 8公告 9分享)的ID
        /// </summary>
        public string id { get; set; }
    }
    /// <summary>
    /// 取消关注  入参
    /// </summary>
    public class NoFocusInPara
    {
        /// <summary>
        /// 类型(1审批 2日志 3任务 4日程 5指令 6群通知)的ID
        /// </summary>
        public string id { get; set; }
    }
    /// <summary>
    /// 我关注的  入参
    /// </summary>
    public class GetMyFocusInPara
    {
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 当前公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// @我的  入参
    /// </summary>
    public class GetATMyInPara
    {
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 在消息界面调用此接口时,此字段传""
        /// 在别人信息主页里调用此接口时,此字段传对方memberid
        /// </summary>
        public string memberid { get; set; }
    }
    public class MyZanInPara
    {
        /// <summary>
        /// 当前公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 我收到的赞
    /// </summary>
    public class MyZan
    {
        /// <summary>
        /// 点赞人ID
        /// </summary>
        public string zannerId { get; set; }
        /// <summary>
        /// 点赞人姓名
        /// </summary>
        public string zannerName { get; set; }
        /// <summary>
        /// 点赞人头像
        /// </summary>
        public string zannerPicture { get; set; }
        /// <summary>
        /// 被点赞人姓名
        /// </summary>
        public string beizannerName { get; set; }
        /// <summary>
        /// 被点赞的动态的类型(审批 任务 日程 日志 指令 群通知 评论 公告 分享)
        /// </summary>
        public string zanType { get; set; }
        /// <summary>
        /// 1 2 3 4 5 8 9
        /// </summary>
        public string zanTypeNum { get; set; }
        /// <summary>
        /// 被点赞的动态的ID
        /// </summary>
        public string zanTypeId { get; set; }
        /// <summary>
        /// 被点赞的动态的内容
        /// </summary>
        public string zanTypeBody { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 点赞时间
        /// </summary>
        public string time { get; set; }
    }
    /// <summary>
    /// 回复我的  未读
    /// </summary>
    public class ReplyMeIsRead
    {
        public string id { get; set; }
        public bool isRead { get; set; }
        public string pingtype { get; set; }
    }
    /// <summary>
    /// 各种提醒的数量
    /// </summary>
    public class NewsRemindNum
    {
        /// <summary>
        /// 类型(1审批 2日志 3任务 4日程 5指令)
        /// </summary>
        public string type { get; set; }
        public int num1 { get; set; }
        public int num2 { get; set; }
    }
    /// <summary>
    /// 审批提醒  入参
    /// </summary>
    public class ApprovalRemindInPara
    {
        /// <summary>
        /// 1待批复  2已批复
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 当前公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 日志提醒  入参
    /// </summary>
    public class LogRemindInPara
    {
        /// <summary>
        /// 类型 1待点评 2已点评
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 当前公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 我执行的任务  入参
    /// </summary>
    public class IDoTaskInPara
    {
        /// <summary>
        /// 1执行中,2已完成,3已取消
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 当前公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 我发出的任务  入参
    /// </summary>
    public class IPublishTaskInPara
    {
        /// <summary>
        /// 1执行中,2已完成,3已取消
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 当前公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 日程列表  入参
    /// </summary>
    public class ProgramListInPara
    {
        /// <summary>
        /// 1请求 年月   2请求 年月日
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 年月   2017-01
        /// </summary>
        public string yy_mm { get; set; }
        /// <summary>
        /// 年月日   2017-01-01
        /// </summary>
        public string yy_mm_dd { get; set; }
        /// <summary>
        /// 当前公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 日程列表
    /// </summary>
    public class ProgramList
    {
        /// <summary>
        /// 这个月当中哪些天有日程
        /// </summary>
        public List<string> programList { get; set; }
        /// <summary>
        /// 日程列表
        /// </summary>
        public List<ProgramListDetail> programListDetail { get; set; }
    }
    /// <summary>
    /// 日程列表  详情
    /// </summary>
    public class ProgramListDetail
    {
        /// <summary>
        /// 时分
        /// </summary>
        public string hh_mm { get; set; }
        /// <summary>
        /// 日程ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 日程内容
        /// </summary>
        public string body { get; set; }
    }
    /// <summary>
    /// 日程通知  入参
    /// </summary>
    public class ProgramNoticeInPara
    {
        /// <summary>
        /// 日程状态(1已开始,2未开始)
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 当前公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 删除(1审批 2日志 3任务 4日程 5指令)   入参
    /// </summary>
    public class DeleteInPara
    {
        /// <summary>
        /// ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 类型(1审批 2日志 3任务 4日程 5指令 8公告 9分享)
        /// </summary>
        public string type { get; set; }
    }
    /// <summary>
    /// 指令提醒  入参
    /// </summary>
    public class OrderRemindInPara
    {
        /// <summary>
        /// 类型 1待处理(待执行) 2已处理(已执行)
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 当前公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 取消(指令,审批,任务)  入参
    /// </summary>
    public class CancelInPara
    {
        /// <summary>
        /// 审批/任务/指令的ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 类型  1审批 3任务 5指令
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 机型
        /// </summary>
        public string phoneModel { get; set; }
    }
    /// <summary>
    /// 群文档
    /// </summary>
    public class GroupDocument
    {
        /// <summary>
        /// 文档上传人姓名
        /// </summary>
        public string personName { get; set; }
        /// <summary>
        /// 文档名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// url
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 文档大小
        /// </summary>
        public string size { get; set; }
        /// <summary>
        /// 文档上传时间
        /// </summary>
        public string time { get; set; }
    }
    /// <summary>
    /// 群文档 全图片   入参
    /// </summary>
    public class GroupDocumentInPara
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        public string id { get; set; }
        public int page { get; set; }
        public int count { get; set; }
    }
    /// <summary>
    /// 全图片
    /// </summary>
    public class AllPicture
    {
        public string time { get; set; }
        public string url { get; set; }
    }
    public class NewsList
    {
        /// <summary>
        /// 会话id
        /// </summary>
        public string id { get; set; }
        public string groupId { get; set; }
        /// <summary>
        /// 1 个人 2群组
        /// </summary>
        public string type { get; set; }
        public int typeNum { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 是否还是个群 1  是   0不是
        /// </summary>
        public int isGroup { get; set; }
        public string personNum { get; set; }
        /// <summary>
        /// 时分或几天前
        /// </summary>
        public string time1 { get; set; }
    }
    public class NewsInfoList
    {
        /// <summary>
        /// 1 个人 2群组
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 1 普通文本  2 图片 3文档   4  录音  5 视频  6位置
        /// </summary>
        public string fileType { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public string map { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 标志 1 右面 2 左面
        /// </summary>
        public string newType { get; set; }
        /// <summary>
        /// 发送人id
        /// </summary>
        public string newSenderId { get; set; }
        /// <summary>
        /// 接收人Id  群组的返回群组id
        /// </summary>
        public string newReceiveId { get; set; }
        /// <summary>
        /// base64位
        /// </summary>
        public string baseurl { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string filename { get; set; }

    }
    public class SearchNewsInfoList
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        public string id { get; set; }

    }
    public class SearchNewsInfoListPara
    {
        /// <summary>
        /// 个人 1  群组 2
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 群组id
        /// </summary>
        public string id { get; set; }
    }
    public class NewsInfo
    {
        public string id { get; set; }
        public string type { get; set; }
        public string picture { get; set; }
        public string body { get; set; }
        public string time { get; set; }
        public string name { get; set; }
        public string groupName { get; set; }
        public string groupPicture { get; set; }
        public string infotype { get; set; }
        public string weidu { get; set; }
        public string grouppersonid { get; set; }
        /// <summary>
        /// 是否还是个群 1  是   0不是
        /// </summary>
        public int State { get; set; }

    }
    public class MessagePara
    {
        public string message { get; set; }
        public string extra { get; set; }
    }
    public class GroupMessagePara
    {
        public string operatorUserId { get; set; }
        public string operation { get; set; }
        public string message { get; set; }
        public string extra { get; set; }
        public dataPara data { get; set; }
    }
    public class GroupMessagePersonPara
    {
        public string operatorUserId { get; set; }
        public string operation { get; set; }
        public string message { get; set; }
        public string extra { get; set; }
        public dataPersonPara data { get; set; }
    }
    public class dataPara
    {
        public string operatorNickname { get; set; }
        public string targetGroupName { get; set; }
    }
    public class dataPersonPara
    {
        public string operatorNickname { get; set; }
        public List<string> targetUserIds { get; set; }
        public List<string> targetUserDisplayNames { get; set; }
    }
    public class GetNewsListPara
    {
        public string time { get; set; }
    }
    public class GetNewsPersonInfoList
    {
        /// <summary>
        /// 会话id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 权限 1 群主   0普通人员
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public string number { get; set; }
        /// <summary>
        /// 群头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 群名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 是否还是个群 1  是   0不是
        /// </summary>
        public int isGroup { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public List<PersonInfo> personList { get; set; }
    }
    public class PersonInfo
    {
        /// <summary>
        /// 人员id
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 权限 1 群主   0普通人员
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string jobName { get; set; }
    }
    public class GetNamePictureInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 修改数量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 是否在群组 1 在  0不在
        /// </summary>
        public int inGroup { get; set; }
    }
    public class GetNamePicturePara
    {
        /// <summary>
        /// 群组id
        /// </summary>
        public string groupId { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        public string memberId { get; set; }
        /// <summary>
        /// 1 个人头像个人聊天记录  2 群组头像   3 个人头像 群聊天记录
        /// </summary>
        public int type  { get; set; }
        /// <summary>
        /// 是否修改已读
        /// </summary>
        public bool state { get; set; }
    }
    public class WeiDuInfo
    {
        /// <summary>
        /// 人员id
        /// </summary>
        public string memberId { get; set; }
        /// <summary>
        /// 未读数量
        /// </summary>
        public int count { get; set; }

    }
    /// <summary>
    /// 分公司显示各种提醒的数量  入参
    /// </summary>
    /// <returns></returns>
    public class GetNewsRemindNumListCompanyInPara
    {
        /// <summary>
        /// 1审批 2日志 3任务 4日程 5指令 21工作提醒 22我关注的
        /// </summary>
        public string type { get; set; }
    }
    /// <summary>
    /// 分公司显示各种提醒的数量
    /// </summary>
    /// <returns></returns>
    public class GetNewsRemindNumListCompany
    {
        public string companyId { get; set; }
        public string companyName { get; set; }
        public int num1 { get; set; }
        public int num2 { get; set; }
    }
    /// <summary>
    /// 获取当前用户某聊天未读数量  入参
    /// </summary>
    /// <returns></returns>
    public class GetPersonNewsNotReadNumInPara
    {
        public string token { get; set; }
    }
    /// <summary>
    /// 获取当前用户某聊天未读数量
    /// </summary>
    /// <returns></returns>
    public class GetPersonNewsNotReadNum
    {
        public string id { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public int num { get; set; }
        /// <summary>
        /// 聊天类型  1个人 2群组
        /// </summary>
        public int newsType { get; set; }
    }
    /// <summary>
    /// 获取当前用户全部聊天未读数量
    /// </summary>
    /// <returns></returns>
    public class GetPersonNewsNotReadAllNum
    {
        public int num { get; set; }
    }
}
