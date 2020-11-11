using JointOffice.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public interface IWork
    {
        /// <summary>
        /// 待审批
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<DaiShenPi> GetDaiShenPiList(GetDaiDianPingDeRiZhiListPara para);
        /// <summary>
        /// 待点评的日志
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<DaiDianPingDeRiZhi> GetDaiDianPingDeRiZhiList(GetDaiDianPingDeRiZhiListPara para);
        /// <summary>
        /// 待执行的任务
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<DaiZhiXingDeRenWu> GetDaiZhiXingDeRenWuList(GetDaiDianPingDeRiZhiListPara para);
        /// <summary>
        /// 待执行的指令
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<ZhiLingDetail> GetZhiLingDetailList(GetDaiDianPingDeRiZhiListPara para);
        /// <summary>
        /// 待回执的公告
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<GongGaoDetail> GetGongGaoDetailList(GetDaiDianPingDeRiZhiListPara para);
        /// <summary>
        /// 待回执的分享
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<FenXiangDetail> GetFenXiangDetailList(GetDaiDianPingDeRiZhiListPara para);
        /// <summary>
        /// 个人动态主页
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<PersonDynamic_info> GetPersonDynamic_infoList(GetPersonDynamic_infoListPara para);
        /// <summary>
        /// 部门信息中个人动态主页
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<PersonDynamic_info> GetDept_PersonDynamic_infoList(GetPersonDynamic_infoListPara para);
        /// <summary>
        /// 工作回复  回复我的
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<WorkReply> GetWorkReplyList(GetReplyListPara para);
        /// <summary>
        /// 创建审批
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge CreateWorkApproval(Work_Approval para);
        /// <summary>
        /// 创建日志
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge CreateWorkLog(Work_Log para);
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge CreateWorkTask(Work_Task para);
        /// <summary>
        /// 创建日程
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge CreateWorkProgram(Work_Program para);
        /// <summary>
        /// 创建指令
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge CreateWorkOrder(Work_Order para);
        /// <summary>
        /// 创建公告
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge CreateWorkAnnouncement(Work_Announcement para);
        /// <summary>
        /// 创建分享
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge CreateWorkShare(Work_Share para);
        /// <summary>
        /// 审批详情
        /// </summary>
        /// <param name="审批ID"></param>
        /// <returns></returns>
        Showapi_Res_Single<DaiShenPi> GetShenPiDetail(DetailID para);
        /// <summary>
        /// 日志详情
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<DaiDianPingDeRiZhi> GetRiZhiDetail(DetailID para);
        /// <summary>
        /// 任务详情
        /// </summary>
        /// <param name="任务ID"></param>
        /// <returns></returns>
        Showapi_Res_Single<DaiZhiXingDeRenWu> GetRenWuDetail(DetailID para);
        /// <summary>
        /// 日程详情
        /// </summary>
        /// <param name="日程ID"></param>
        /// <returns></returns>
        Showapi_Res_Single<RiChengDetail> GetRiChengDetail(DetailID para);
        /// <summary>
        /// 指令详情
        /// </summary>
        /// <param name="指令ID"></param>
        /// <returns></returns>
        Showapi_Res_Single<ZhiLingDetail> GetZhiLingDetail(DetailID para);
        /// <summary>
        /// 公告详情
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<GongGaoDetail> GetGongGaoDetail(DetailID para);
        /// <summary>
        /// 分享详情
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<FenXiangDetail> GetFenXiangDetail(DetailID para);
        /// <summary>
        /// 工作列表中每个工作的详情页
        /// </summary>
        Showapi_Res_Single<AllDetails> GetAllDetails(FocusInPara para);
        /// <summary>
        /// 工作附件
        /// </summary>
        /// <param name="人id,工作文档类型"></param>
        /// <returns></returns>
        Showapi_Res_List<WorkDoc_Annex> GetWorkDoc_AnnexList(WorkDoc_Para para);
        /// <summary>
        /// 工作图片
        /// </summary>
        /// <param name="人id,工作文档类型"></param>
        /// <returns></returns>
        Showapi_Res_List<WorkDoc_Picture> GetWorkDoc_PictureList(WorkDoc_Para para);
        /// <summary>
        /// 工作录音
        /// </summary>
        /// <param name="人id,工作文档类型"></param>
        /// <returns></returns>
        Showapi_Res_List<WorkDoc_Voice> GetWorkDoc_VoiceList(WorkDoc_Para para);
        /// <summary>
        /// 工作列表
        /// </summary>
        Showapi_Res_Single<WorkListAll> GetWorkListAll(WorkListAllInPara para);
        /// <summary>
        /// 工作列表分类
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<WorkList> GetWorkList();
        /// <summary>
        /// 审批  同意/不同意
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge Approval(ApprovalInPara para);
        /// <summary>
        /// 审批人  取消审批
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge EscApproval(EscApprovalInPara para);
        /// <summary>
        /// 重新选择审批人
        /// </summary>
        Showapi_Res_Meaasge AgainChooseApprovalPerson(AgainChooseApprovalPersonInPara para);
        /// <summary>
        /// 执行人  同意执行
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge Exe(ExeInPara para);
        /// <summary>
        /// 执行人  取消任务/取消指令    参与人  取消日程
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge NoExe(NoExeInPara para);
        /// <summary>
        /// 指令  继续执行
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge ContinueExe(ExeInPara para);
        /// <summary>
        /// 评论
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge PingLun(Comment_Body para);
        /// <summary>
        /// 点评
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge DianPing(DianPing_Body para);
        /// <summary>
        /// 回执
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge Rece(ReceInPara para);
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<PersonDynamic_info> GetAllSearchList(AllSearch para);
        /// <summary>
        /// 普通搜索
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<PersonDynamic_info> GetSearchList(Search para);
        /// <summary>
        /// 高级搜索
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<PersonDynamic_info> GetHighSearchList(HighSearch para);
        /// <summary>
        /// 高级搜索   列表
        /// </summary>
        Showapi_Res_List<HighSearchType> GetHighSearchTypeList();
        /// <summary>
        /// 日志周计划  周列表
        /// </summary>
        Showapi_Res_List<GetOneDay> GetOneDay(GetOneDayInPara para);
        /// <summary>
        /// 点击更多返回的状态值
        /// </summary>
        Showapi_Res_Single<FocusCollectionDeleteState> GetFocusCollectionDeleteState(FocusCollectionDeleteStateInPara para);
        /// <summary>
        /// 获取中国城市列表
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<ChinaCity> GetChinaCity();
        /// <summary>
        /// 获取中国城市列表
        /// </summary>
        Showapi_Res_List<ChinaCity_Province> GetChinaCityNew();
        /// <summary>
        /// 搜索城市
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<ChinaCitySearch> GetChinaCitySearch(ChinaCitySearchInPara para);
        /// <summary>
        /// 获取抄送范围/参与人详情
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<RangeInfo> GetRangeInfo(FocusInPara para);
        /// <summary>
        /// WorkTest
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge WorkTest(WorkTestInPara para);
        /// <summary>
        /// 工作列表标签顺序  写入
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge WorkListTagDES(WorkListTagDESInPara para);
        /// <summary>
        /// 工作列表标签顺序  获取
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<WorkListTagDES> GetWorkListTagDES();
        /// <summary>
        /// 单个工作刷新
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<PersonDynamic_info> GetOneWorkInfo(GetOneWorkInfopara para);
        /// <summary>
        /// 获取签到类型
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<QianDaoType> GetQianDaoType();
        /// <summary>
        /// 修改抄送范围
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateWorkRange(UpdateWorkRangeInPara para);
    }
    /// <summary>
    /// 待我处理  入参
    /// </summary>
    public class GetDaiDianPingDeRiZhiListPara
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 对方ID
        /// </summary>
        public string memberid { get; set; }
    }
    /// <summary>
    /// 个人/部门  动态列表  入参
    /// </summary>
    public class GetPersonDynamic_infoListPara
    {
        /// <summary>
        /// 个人(全部 审批等)   部门(1发出  2收到)
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
        /// 查看个人信息主页时:查看的是自己的传入自己的memberid  查看别人的传入对方memberid
        /// 查看部门信息主页时:传入部门ID
        /// </summary>
        public string memberid { get; set; }
        public string companyId { get; set; }
    }
    /// <summary>
    /// 工作回复 回复我的   入参
    /// </summary>
    /// <returns></returns>
    public class GetReplyListPara
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
        /// '工作回复'接口分为:我回复别人 和 别人回复我,前者此字段传当前用户的memberid,后者此字段传对方memberid
        /// '回复我的'接口:此字段传 ""
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 当前公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 加班
    /// </summary>
    public class OverTime
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public string hour { get; set; }
    }
    /// <summary>
    /// 出差总详情
    /// </summary>
    public class TravelAll
    {
        /// <summary>
        /// 出差人  id  name  type
        /// </summary>
        public string travelPerson { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 出差天数
        /// </summary>
        public string allDays { get; set; }
        /// <summary>
        /// 出差原因
        /// </summary>
        public string reason { get; set; }
        /// <summary>
        /// 预算
        /// </summary>
        public string budget { get; set; }
        /// <summary>
        /// 预支
        /// </summary>
        public string advance { get; set; }
    }
    /// <summary>
    /// 出差分详情
    /// </summary>
    public class Travel
    {
        /// <summary>
        /// 出发地
        /// </summary>
        public string goArea { get; set; }
        /// <summary>
        /// 目的地
        /// </summary>
        public string toArea { get; set; }
        /// <summary>
        /// 出发时间
        /// </summary>
        public string goDate { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        public string toDate { get; set; }
        /// <summary>
        /// 交通工具
        /// </summary>
        public string tran { get; set; }
        /// <summary>
        /// 住宿天数
        /// </summary>
        public string stay { get; set; }
    }
    /// <summary>
    /// 请假
    /// </summary>
    public class Leave
    {
        /// <summary>
        /// 请假事项
        /// </summary>
        public string leaveItems { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 请假时长
        /// </summary>
        public string hour { get; set; }
    }
    /// <summary>
    /// 出差报销
    /// </summary>
    public class TravelReb
    {
        /// <summary>
        /// 报销项目
        /// </summary>
        public string travelRebType { get; set; }
        /// <summary>
        /// 出发地
        /// </summary>
        public string goArea { get; set; }
        /// <summary>
        /// 目的地
        /// </summary>
        public string toArea { get; set; }
        /// <summary>
        /// 出发时间
        /// </summary>
        public string goDate { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        public string toDate { get; set; }
        /// <summary>
        /// 报销金额
        /// </summary>
        public string money { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string proPicture { get; set; }
    }
    /// <summary>
    /// 普通报销
    /// </summary>
    public class Reb
    {
        /// <summary>
        /// 报销事项
        /// </summary>
        public string reimbursementMatters { get; set; }
        /// <summary>
        /// 报销金额
        /// </summary>
        public string money { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 关联客户
        /// </summary>
        public string associatedCustomer { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string proPicture { get; set; }
    }
    public class DetailID
    {
        public string id { get; set; }
    }
    /// <summary>
    /// 待审批
    /// </summary>
    public class DaiShenPi
    {
        public string dyTypeName { get; set; }
        /// <summary>
        /// 审批类型 1加班 2出差 3请假 4出差报销 5普通报销
        /// </summary>
        public string approvalType { get; set; }
        /// <summary>
        /// 审批类型 1加班 2出差 3请假 4出差报销 5普通报销
        /// </summary>
        public string approvalTypeNum { get; set; }
        /// <summary>
        /// 审批ID
        /// </summary>
        public string shenpiid { get; set; }
        /// <summary>
        /// 审批编号
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 审批内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 审批创建人ID
        /// </summary>
        public string shenpiMemberid { get; set; }
        /// <summary>
        /// 审批创建人的头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 审批创建人的名字
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 审批创建日期
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        public List<Executor> executor { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 抄送范围
        /// </summary>
        public string range { get; set; }
        /// <summary>
        /// 加班总时长
        /// </summary>
        public string workDuration { get; set; }
        /// <summary>
        /// 加班
        /// </summary>
        public List<OverTime> overTime { get; set; }
        /// <summary>
        /// 出差总详情
        /// </summary>
        public string travelAll { get; set; }
        /// <summary>
        /// 出差分详情
        /// </summary>
        public List<Travel> travel { get; set; }
        /// <summary>
        /// 请假
        /// </summary>
        public List<Leave> leave { get; set; }
        /// <summary>
        /// 出差报销
        /// </summary>
        public List<TravelReb> travelReb { get; set; }
        /// <summary>
        /// 报销
        /// </summary>
        public List<Reb> reb { get; set; }
        /// <summary>
        /// 请假总时长
        /// </summary>
        public string leaveDuration { get; set; }
        /// <summary>
        /// 出差报销总金额
        /// </summary>
        public string travelMoney { get; set; }
        /// <summary>
        /// 普通报销总金额
        /// </summary>
        public string rebMoney { get; set; }
        /// <summary>
        /// 点评数量
        /// </summary>
        public int? pingLunNum { get; set; }
        /// <summary>
        /// 是否赞   1是 0否
        /// </summary>
        public int? isZan { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? dianZanNum { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 我的坐标
        /// </summary>
        public string map { get; set; }
        /// <summary>
        /// 我的位置
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 附加图片
        /// </summary>
        public List<PersonDynamic_info_url> appendPicture { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<Work_File> annex { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public string annexLength { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// 审批进度(谁提交 待谁审批)
        /// </summary>
        public string percentage { get; set; }
        /// <summary>
        /// 审批详情中的审批列表
        /// </summary>
        public List<ExeCommentList> exeCommentList { get; set; }
        /// <summary>
        /// 是否该当前用户审批  1是 0不是
        /// </summary>
        public string isExecutor { get; set; }
        /// <summary>
        /// 是否已关注  1是 0否
        /// </summary>
        public string isFocus { get; set; }
        /// <summary>
        /// 是否已收藏  1是 0否
        /// </summary>
        public string isCollection { get; set; }
    }
    /// <summary>
    /// 待点评的日志
    /// </summary>
    public class DaiDianPingDeRiZhi
    {
        public string dyTypeName { get; set; }
        /// <summary>
        /// 日志ID
        /// </summary>
        public string rizhiid { get; set; }
        /// <summary>
        /// 日志创建人ID
        /// </summary>
        public string rizhimemberid { get; set; }
        /// <summary>
        /// 日志创建人的头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 日志创建人的名字
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 日志创建日期
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 点评人姓名
        /// </summary>
        public string reviewPersonName { get; set; }
        /// <summary>
        /// 点评模板(日计划,周计划,月计划)
        /// </summary>
        public string moban { get; set; }
        /// <summary>
        /// 模板对应的时间
        /// </summary>
        public string mobanTime { get; set; }
        /// <summary>
        /// 点评状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 点评等级
        /// </summary>
        public string grade { get; set; }
        /// <summary>
        /// 点评等级
        /// </summary>
        public int gradeNum { get; set; }
        /// <summary>
        /// 执行详情中的执行列表
        /// </summary>
        public List<ExeCommentList> exeCommentList { get; set; }
        /// <summary>
        /// 工作总结
        /// </summary>
        public string workSummary { get; set; }
        /// <summary>
        /// 工作计划
        /// </summary>
        public string workPlan { get; set; }
        /// <summary>
        /// 心得体会
        /// </summary>
        public string experience { get; set; }
        /// <summary>
        /// 日志费用明细
        /// </summary>
        public string moneyInfo { get; set; }
        /// <summary>
        /// 日志总费用
        /// </summary>
        public string money { get; set; }
        /// <summary>
        /// 抄送范围
        /// </summary>
        public string range { get; set; }
        /// <summary>
        /// 回执范围
        /// </summary>
        public string receipt { get; set; }
        /// <summary>
        /// 回执进度概况
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 点评数量
        /// </summary>
        public int? pingLunNum { get; set; }
        /// <summary>
        /// 是否赞   1是 0否
        /// </summary>
        public int? isZan { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? dianZanNum { get; set; }
        /// <summary>
        /// 我的坐标
        /// </summary>
        public string map { get; set; }
        /// <summary>
        /// 我的位置
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 附加图片
        /// </summary>
        public List<PersonDynamic_info_url> appendPicture { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<Work_File> annex { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public string annexLength { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 当前用户是否为点评人  1是 0不是
        /// </summary>
        public string isExecutor { get; set; }
        /// <summary>
        /// 是否已关注  1是 0否
        /// </summary>
        public string isFocus { get; set; }
        /// <summary>
        /// 是否已收藏  1是 0否
        /// </summary>
        public string isCollection { get; set; }
        /// <summary>
        /// 是否需要当前用户回执  1是 0否
        /// </summary>
        public string isReceipt { get; set; }
        /// <summary>
        /// 当前用户是否回执完  1是 0否
        /// </summary>
        public string isReceiptOk { get; set; }
    }
    public class DaiDianPingDeRiZhi_url
    {
        public string url { get; set; }
    }
    /// <summary>
    /// 待执行的任务
    /// </summary>
    public class DaiZhiXingDeRenWu
    {
        public string dyTypeName { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public string renwuid { get; set; }
        /// <summary>
        /// 任务标题
        /// </summary>
        public string taskTitle { get; set; }
        /// <summary>
        /// 任务备注
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 任务创建人ID
        /// </summary>
        public string renwuMemberid { get; set; }
        /// <summary>
        /// 任务创建人的头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 任务创建人的名字
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 任务创建日期
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 执行状态  数字
        /// </summary>
        public string stateNum { get; set; }
        /// <summary>
        /// 星级
        /// </summary>
        public string grade { get; set; }
        /// <summary>
        /// 点评等级
        /// </summary>
        public int gradeNum { get; set; }
        /// <summary>
        /// 抄送范围
        /// </summary>
        public string range { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        public List<Executor> executor { get; set; }
        /// <summary>
        /// 执行详情中的执行列表
        /// </summary>
        public List<ExeCommentList> exeCommentList { get; set; }
        /// <summary>
        /// 执行人数   0/2
        /// </summary>
        public string executorNum { get; set; }
        /// <summary>
        /// 全部执行人数
        /// </summary>
        public string executorNumAll { get; set; }
        /// <summary>
        /// 已执行人数
        /// </summary>
        public string executorNumYes { get; set; }
        /// <summary>
        /// 执行进度  %
        /// </summary>
        public string percentage { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 提醒时间
        /// </summary>
        public string remindTime { get; set; }
        /// <summary>
        /// 点评数量
        /// </summary>
        public int? pingLunNum { get; set; }
        /// <summary>
        /// 是否赞   1是 0否
        /// </summary>
        public int? isZan { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? dianZanNum { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 我的坐标
        /// </summary>
        public string map { get; set; }
        /// <summary>
        /// 我的位置
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 附加图片
        /// </summary>
        public List<PersonDynamic_info_url> appendPicture { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<Work_File> annex { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public string annexLength { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// 当前用户是否为执行人  1是 0不是
        /// </summary>
        public string isExecutor { get; set; }
        /// <summary>
        /// 此任务谁完成了
        /// </summary>
        public string exePerson { get; set; }
        /// <summary>
        /// 是否已关注  1是 0否
        /// </summary>
        public string isFocus { get; set; }
        /// <summary>
        /// 是否已收藏  1是 0否
        /// </summary>
        public string isCollection { get; set; }
        /// <summary>
        /// 执行人执行完没  0没执行完 1执行完成 2拒绝任务
        /// </summary>
        public string isExe { get; set; }
    }
    public class DaiZhiXingDeRenWu_url
    {
        public string url { get; set; }
    }
    public class Work_File
    {
        public string url { get; set; }
        public long length { get; set; }
        public string lengthStr { get; set; }
        public string name { get; set; }
        public string fileType { get; set; }
    }
    /// <summary>
    /// 日程详情
    /// </summary>
    public class RiChengDetail
    {
        public string dyTypeName { get; set; }
        /// <summary>
        /// 日程ID
        /// </summary>
        public string richengid { get; set; }
        /// <summary>
        /// 日程内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 日程创建人ID
        /// </summary>
        public string richengMemberid { get; set; }
        /// <summary>
        /// 日程创建人的头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 日程创建人的名字
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 日程创建日期
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 抄送范围
        /// </summary>
        public string range { get; set; }
        /// <summary>
        /// 参与人
        /// </summary>
        public string joinPerson { get; set; }
        /// <summary>
        /// 回执范围
        /// </summary>
        public string receipt { get; set; }
        /// <summary>
        /// 回执进度概况
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 执行详情中的执行列表
        /// </summary>
        public List<ExeCommentList> exeCommentList { get; set; }
        /// <summary>
        /// 日程开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 提醒时间
        /// </summary>
        public string remindTime { get; set; }
        /// <summary>
        /// 点评数量
        /// </summary>
        public int? pingLunNum { get; set; }
        /// <summary>
        /// 是否赞   1是 0否
        /// </summary>
        public int? isZan { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? dianZanNum { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 我的坐标
        /// </summary>
        public string map { get; set; }
        /// <summary>
        /// 我的位置
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 附加图片
        /// </summary>
        public List<PersonDynamic_info_url> appendPicture { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<Work_File> annex { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public string annexLength { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// 是否已关注  1是 0否
        /// </summary>
        public string isFocus { get; set; }
        /// <summary>
        /// 是否已收藏  1是 0否
        /// </summary>
        public string isCollection { get; set; }
        /// <summary>
        /// 是否需要当前用户回执  1是 0否
        /// </summary>
        public string isReceipt { get; set; }
        /// <summary>
        /// 当前用户是否回执完  1是 0否
        /// </summary>
        public string isReceiptOk { get; set; }
    }
    /// <summary>
    /// 指令详情
    /// </summary>
    public class ZhiLingDetail
    {
        public string dyTypeName { get; set; }
        /// <summary>
        /// 指令ID
        /// </summary>
        public string zhilingid { get; set; }
        /// <summary>
        /// 指令内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 指令创建人ID
        /// </summary>
        public string zhilingMemberid { get; set; }
        /// <summary>
        /// 指令创建人的头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 指令创建人的名字
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 指令创建日期
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 指令截止时间
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 超时标志  1超时  2没超时
        /// </summary>
        public string overTimeMark { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 执行状态   数字
        /// </summary>
        public string stateNum { get; set; }
        /// <summary>
        /// 星级
        /// </summary>
        public string grade { get; set; }
        /// <summary>
        /// 点评等级
        /// </summary>
        public int gradeNum { get; set; }
        /// <summary>
        /// 抄送范围
        /// </summary>
        public string range { get; set; }
        /// <summary>
        /// 点评数量
        /// </summary>
        public int? pingLunNum { get; set; }
        /// <summary>
        /// 是否赞   1是 0否
        /// </summary>
        public int? isZan { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? dianZanNum { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 我的坐标
        /// </summary>
        public string map { get; set; }
        /// <summary>
        /// 我的位置
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 附加图片
        /// </summary>
        public List<PersonDynamic_info_url> appendPicture { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<Work_File> annex { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public string annexLength { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// 当前用户是否为执行人  1是 0不是
        /// </summary>
        public string isExecutor { get; set; }
        /// <summary>
        /// 由谁执行  信息
        /// </summary>
        public string exeBody { get; set; }
        /// <summary>
        /// 执行详情中的执行列表
        /// </summary>
        public List<ExeCommentList> exeCommentList { get; set; }
        /// <summary>
        /// 是否已关注  1是 0否
        /// </summary>
        public string isFocus { get; set; }
        /// <summary>
        /// 是否已收藏  1是 0否
        /// </summary>
        public string isCollection { get; set; }
    }
    /// <summary>
    /// 公告详情
    /// </summary>
    public class GongGaoDetail
    {
        public string dyTypeName { get; set; }
        /// <summary>
        /// 公告ID
        /// </summary>
        public string gonggaoid { get; set; }
        /// <summary>
        /// 公告标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 公告内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 公告创建人ID
        /// </summary>
        public string gonggaoMemberid { get; set; }
        /// <summary>
        /// 公告创建人的头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 公告创建人的名字
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 公告创建日期
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 公告公示开始日期
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 公告公示结束日期
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 执行状态   数字
        /// </summary>
        public string stateNum { get; set; }
        /// <summary>
        /// 星级
        /// </summary>
        public string grade { get; set; }
        /// <summary>
        /// 点评等级
        /// </summary>
        public int gradeNum { get; set; }
        /// <summary>
        /// 抄送范围
        /// </summary>
        public string range { get; set; }
        /// <summary>
        /// 回执范围
        /// </summary>
        public string receipt { get; set; }
        /// <summary>
        /// 回执进度概况
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 点评数量
        /// </summary>
        public int? pingLunNum { get; set; }
        /// <summary>
        /// 是否赞   1是 0否
        /// </summary>
        public int? isZan { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? dianZanNum { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 我的坐标
        /// </summary>
        public string map { get; set; }
        /// <summary>
        /// 我的位置
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 附加图片
        /// </summary>
        public List<PersonDynamic_info_url> appendPicture { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<Work_File> annex { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public string annexLength { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// 是否已关注  1是 0否
        /// </summary>
        public string isFocus { get; set; }
        /// <summary>
        /// 是否已收藏  1是 0否
        /// </summary>
        public string isCollection { get; set; }
        /// <summary>
        /// 是否需要当前用户回执  1是 0否
        /// </summary>
        public string isReceipt { get; set; }
        /// <summary>
        /// 当前用户是否回执完  1是 0否
        /// </summary>
        public string isReceiptOk { get; set; }
    }
    /// <summary>
    /// 分享详情
    /// </summary>
    public class FenXiangDetail
    {
        public string dyTypeName { get; set; }
        /// <summary>
        /// 分享ID
        /// </summary>
        public string fenxiangid { get; set; }
        /// <summary>
        /// 分享内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 分享创建人ID
        /// </summary>
        public string fenxiangMemberid { get; set; }
        /// <summary>
        /// 分享创建人的头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 分享创建人的名字
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 分享创建日期
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 执行状态   数字
        /// </summary>
        public string stateNum { get; set; }
        /// <summary>
        /// 星级
        /// </summary>
        public string grade { get; set; }
        /// <summary>
        /// 点评等级
        /// </summary>
        public int gradeNum { get; set; }
        /// <summary>
        /// 抄送范围
        /// </summary>
        public string range { get; set; }
        /// <summary>
        /// 回执范围
        /// </summary>
        public string receipt { get; set; }
        /// <summary>
        /// 回执进度概况
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 点评数量
        /// </summary>
        public int? pingLunNum { get; set; }
        /// <summary>
        /// 是否赞   1是 0否
        /// </summary>
        public int? isZan { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? dianZanNum { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 我的坐标
        /// </summary>
        public string map { get; set; }
        /// <summary>
        /// 我的位置
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 附加图片
        /// </summary>
        public List<PersonDynamic_info_url> appendPicture { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<Work_File> annex { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public string annexLength { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// 是否已关注  1是 0否
        /// </summary>
        public string isFocus { get; set; }
        /// <summary>
        /// 是否已收藏  1是 0否
        /// </summary>
        public string isCollection { get; set; }
        /// <summary>
        /// 是否需要当前用户回执  1是 0否
        /// </summary>
        public string isReceipt { get; set; }
        /// <summary>
        /// 当前用户是否回执完  1是 0否
        /// </summary>
        public string isReceiptOk { get; set; }
    }
    /// <summary>
    /// 工作列表中每个工作的详情页
    /// </summary>
    public class AllDetails
    {
        public string type { get; set; }
        /// <summary>
        /// 审批详情页
        /// </summary>
        public DaiShenPi daiShenPi { get; set; }
        /// <summary>
        /// 日志详情页
        /// </summary>
        public DaiDianPingDeRiZhi daiDianPingDeRiZhi { get; set; }
        /// <summary>
        /// 任务详情页
        /// </summary>
        public DaiZhiXingDeRenWu daiZhiXingDeRenWu { get; set; }
        /// <summary>
        /// 日程详情页
        /// </summary>
        public RiChengDetail riChengDetail { get; set; }
        /// <summary>
        /// 指令详情页
        /// </summary>
        public ZhiLingDetail zhiLingDetail { get; set; }
        /// <summary>
        /// 公告详情页
        /// </summary>
        public GongGaoDetail gongGaoDetail { get; set; }
        /// <summary>
        /// 分享详情页
        /// </summary>
        public FenXiangDetail fenXiangDetail { get; set; }
    }
    /// <summary>
    /// 执行人List
    /// </summary>
    public class Executor
    {
        /// <summary>
        /// 执行人ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 执行人姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 执行人头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public string type { get; set; }
        public string num { get; set; }
        /// <summary>
        /// 执行人完成时间
        /// </summary>
        public string executorCompleteTime { get; set; }
        /// <summary>
        /// 执行人完成后备注
        /// </summary>
        public string executorCompleteRemarks { get; set; }
    }
    /// <summary>
    /// 执行详情中的执行列表
    /// </summary>
    public class ExeCommentList
    {
        /// <summary>
        /// 执行人或创建人 name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        public string exeDate { get; set; }
        /// <summary>
        /// 操作过程的回复的内容
        /// </summary>
        public string exeBody { get; set; }
        /// <summary>
        /// 该操作的行为
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 该操作的行为 数字   1点评 2完成 3取消 4继续执行
        /// </summary>
        public string stateNum { get; set; }
        /// <summary>
        /// 机型
        /// </summary>
        public string phoneModel { get; set; }
    }
    /// <summary>
    /// 评论内容
    /// </summary>
    public class PingLun
    {
        /// <summary>
        /// 此条评论ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 评论人ID
        /// </summary>
        public string reviewPersonID { get; set; }
        /// <summary>
        /// 评论人姓名
        /// </summary>
        public string reviewPersonName { get; set; }
        /// <summary>
        /// 评论人头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 此条评论内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 此条评论上一级的评论的人的ID
        /// </summary>
        public string previousMemberId { get; set; }
        /// <summary>
        /// 此条评论上一级的评论的人的姓名
        /// </summary>
        public string previousName { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public string pingLunTime { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 附加图片
        /// </summary>
        public List<PersonDynamic_info_url> appendPicture { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<Work_File> annex { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public string annexLength { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// 当前用户能否删除此条评论  1能  0不能
        /// </summary>
        public string isDelete { get; set; }
        /// <summary>
        /// 当前用户是否点赞  1是 0不是
        /// </summary>
        public string isZan { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int pingLunNum { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int dianZanNum { get; set; }
    }
    /// <summary>
    /// 个人动态信息
    /// </summary>
    public class PersonDynamic_info
    {
        public string dyTypeName { get; set; }
        /// <summary>
        /// 个人动态的类型(审批 日志 任务 日程 指令 公告 分享)
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 1审批 2日志 3任务 4日程 5指令 8公告 9分享
        /// </summary>
        public string typeNum { get; set; }
        /// <summary>
        /// 此动态的当前状态(待执行,已执行,待审批,已审批,待点评,已点评,已删除,已取消)
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 此动态的ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 此动态的附加图片
        /// </summary>
        public List<PersonDynamic_info_url> appendPicture { get; set; }
        /// <summary>
        /// 此动态的附件
        /// </summary>
        public List<Work_File> annex { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public string annexLength { get; set; }
        /// <summary>
        /// 此动态的录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 此动态的录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// 此动态创建人的ID
        /// </summary>
        public string memberId { get; set; }
        /// <summary>
        /// 此动态创建人的姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 此动态创建人的头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 此动态创建时间
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 点评数量
        /// </summary>
        public int? pingLunNum { get; set; }
        /// <summary>
        /// 是否赞
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
        public string rangeNew { get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string aTPerson { get; set; }
        /// <summary>
        /// 发布此动态时的位置的横纵坐标
        /// </summary>
        public string map { get; set; }
        /// <summary>
        /// 发布此动态时的位置的具体地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 审批 日程 指令 公告 分享   内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 任务的执行人  审批的审批人
        /// </summary>
        public List<Executor> executor { get; set; }
        /// <summary>
        /// 任务 指令 公告  截止时间
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 任务标题
        /// </summary>
        public string taskTitle { get; set; }
        /// <summary>
        /// 任务备注  回执进度概况
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 任务 执行人数   0/2
        /// </summary>
        public string executorNum { get; set; }
        /// <summary>
        /// 全部执行人数
        /// </summary>
        public string executorNumAll { get; set; }
        /// <summary>
        /// 已执行人数
        /// </summary>
        public string executorNumYes { get; set; }
        /// <summary>
        /// 执行进度(%)   审批进度(谁提交 待谁审批)
        /// </summary>
        public string percentage { get; set; }
        /// <summary>
        /// 执行详情中的执行列表
        /// </summary>
        public List<ExeCommentList> exeCommentList { get; set; }
        /// <summary>
        /// 任务 执行人执行完没   1执行完成  0没执行完
        /// </summary>
        public string isExe { get; set; }
        /// <summary>
        /// 任务 日程  提醒时间
        /// </summary>
        public string remindTime { get; set; }
        /// <summary>
        /// 点评人ID
        /// </summary>
        public string reviewPersonId { get; set; }
        /// <summary>
        /// 点评人姓名
        /// </summary>
        public string reviewPersonName { get; set; }
        /// <summary>
        /// 日志模板
        /// </summary>
        public string moban { get; set; }
        /// <summary>
        /// 模板对应的时间
        /// </summary>
        public string mobanTime { get; set; }
        /// <summary>
        /// 日志工作总结
        /// </summary>
        public string workSummary { get; set; }
        /// <summary>
        /// 日志工作计划
        /// </summary>
        public string workPlan { get; set; }
        /// <summary>
        /// 日志心得体会
        /// </summary>
        public string experience { get; set; }
        /// <summary>
        /// 日志费用明细
        /// </summary>
        public string moneyInfo { get; set; }
        /// <summary>
        /// 日志总费用
        /// </summary>
        public string money { get; set; }
        /// <summary>
        /// 日志 任务 指令 点评等级
        /// </summary>
        public string grade { get; set; }
        /// <summary>
        /// 日志 任务 指令 点评等级
        /// </summary>
        public int gradeNum { get; set; }
        /// <summary>
        /// 任务 指令 执行状态   数字
        /// </summary>
        public string stateNum { get; set; }
        /// <summary>
        /// 超时标志  1超时  0没超时
        /// </summary>
        public string overTimeMark { get; set; }
        /// <summary>
        /// 参与人
        /// </summary>
        public string joinPerson { get; set; }
        /// <summary>
        /// 日程 公告  开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 公告标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 回执
        /// </summary>
        public string receipt { get; set; }
        /// <summary>
        /// 审批编号
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 审批类型 1加班 2出差 3请假 4出差报销 5普通报销
        /// </summary>
        public string approvalType { get; set; }
        /// <summary>
        /// 审批类型 1加班 2出差 3请假 4出差报销 5普通报销
        /// </summary>
        public string approvalTypeNum { get; set; }
        /// <summary>
        /// 加班总时长
        /// </summary>
        public string workDuration { get; set; }
        /// <summary>
        /// 加班
        /// </summary>
        public List<OverTime> overTime { get; set; }
        /// <summary>
        /// 出差总详情
        /// </summary>
        public string travelAll { get; set; }
        /// <summary>
        /// 出差分详情
        /// </summary>
        public List<Travel> travel { get; set; }
        /// <summary>
        /// 请假
        /// </summary>
        public List<Leave> leave { get; set; }
        /// <summary>
        /// 出差报销
        /// </summary>
        public List<TravelReb> travelReb { get; set; }
        /// <summary>
        /// 报销
        /// </summary>
        public List<Reb> reb { get; set; }
        /// <summary>
        /// 请假总时长
        /// </summary>
        public string leaveDuration { get; set; }
        /// <summary>
        /// 出差报销总金额
        /// </summary>
        public string travelMoney { get; set; }
        /// <summary>
        /// 普通报销总金额
        /// </summary>
        public string rebMoney { get; set; }
        /// <summary>
        /// 指令 由谁执行  信息
        /// </summary>
        public string exeBody { get; set; }
        /// <summary>
        /// 此动态执行人/审批人   用来判断 当前用户是否为此动态的审批人 执行人 点评人
        /// </summary>
        public string typeExecutor { get; set; }
        /// <summary>
        /// 当前用户是否为执行人/审批人/点评人  1是 0不是
        /// </summary>
        public string isExecutor { get; set; }
        /// <summary>
        /// 此任务谁完成了
        /// </summary>
        public string exePerson { get; set; }
        /// <summary>
        /// 是否已关注  1是 0否
        /// </summary>
        public string isFocus { get; set; }
        /// <summary>
        /// 是否已收藏  1是 0否
        /// </summary>
        public string isCollection { get; set; }
        /// <summary>
        /// 是否需要当前用户回执  1是 0否
        /// </summary>
        public string isReceipt { get; set; }
        /// <summary>
        /// 当前用户是否回执完  1是 0否
        /// </summary>
        public string isReceiptOk { get; set; }

        /// <summary>
        /// 加班
        /// </summary>
        public string overTimeStr { get; set; }
        /// <summary>
        /// 出差总详情
        /// </summary>
        public string travelAllStr { get; set; }
        /// <summary>
        /// 出差分详情
        /// </summary>
        public string travelStr { get; set; }
        /// <summary>
        /// 请假
        /// </summary>
        public string leaveStr { get; set; }
        /// <summary>
        /// 出差报销
        /// </summary>
        public string travelRebStr { get; set; }
        /// <summary>
        /// 报销
        /// </summary>
        public string rebStr { get; set; }
        public string picturelist { get; set; }
        public string annexlist { get; set; }
    }
    public class PersonDynamic_info_url
    {
        public string url { get; set; }
    }
    /// <summary>
    /// 工作回复
    /// </summary>
    public class WorkReply
    {
        /// <summary>
        ///  被回复的工作类型
        /// </summary>
        public string replyType { get; set; }
        /// <summary>
        /// 1 2 3 4 5
        /// </summary>
        public string replyTypeNum { get; set; }
        /// <summary>
        /// 被回复的工作类型的ID
        /// </summary>
        public string replyWorkID { get; set; }
        /// <summary>
        /// 被回复的工作类型的创建人ID
        /// </summary>
        public string replyWorkNameID { get; set; }
        /// <summary>
        /// 被回复的工作类型的创建人姓名
        /// </summary>
        public string replyWorkName { get; set; }
        /// <summary>
        /// 被回复的工作类型的内容
        /// </summary>
        public string replyWorkBody { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        public string replyTime { get; set; }
        /// <summary>
        /// 此条回复的创建人的ID
        /// </summary>
        public string memberId { get; set; }
        /// <summary>
        /// 此条回复的创建人的姓名
        /// </summary>
        public string memberName { get; set; }
        /// <summary>
        /// 此条回复的创建人的头像
        /// </summary>
        public string memberPicture { get; set; }
        /// <summary>
        /// 此条回复的ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 回复的附加图片
        /// </summary>
        public List<PersonDynamic_info_url> appendPicture { get; set; }
        /// <summary>
        /// 回复的附件
        /// </summary>
        public List<Work_File> annex { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public string annexLength { get; set; }
        /// <summary>
        /// 回复的录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 回复的录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 点评数量
        /// </summary>
        public int? pingLunNum { get; set; }
        /// <summary>
        /// 是否赞
        /// </summary>
        public int? isZan { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? dianZanNum { get; set; }
    }
    /// <summary>
    /// 工作附件
    /// </summary>
    public class WorkDoc_Annex
    {
        /// <summary>
        /// 附件路径
        /// </summary>
        public string annexurl { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        public string annexName { get; set; }
        /// <summary>
        /// 附件创建时间
        /// </summary>
        public string annexTime { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public string annexSize { get; set; }
    }
    /// <summary>
    /// 工作图片 第一层List
    /// </summary>
    public class WorkDoc_Picture
    {
        public List<WorkDoc_PictureList> pictureList { get; set; }
    }
    /// <summary>
    /// 工作图片 第二层List
    /// </summary>
    public class WorkDoc_PictureList
    {
        public string url { get; set; }
    }
    /// <summary>
    /// 工作录音
    /// </summary>
    public class WorkDoc_Voice
    {
        /// <summary>
        /// 录音路径
        /// </summary>
        public string annexurl { get; set; }
        /// <summary>
        /// 录音创建时间
        /// </summary>
        public string annexTime { get; set; }
        /// <summary>
        /// 录音大小
        /// </summary>
        public string annexSize { get; set; }
    }
    /// <summary>
    /// 工作文档  传参
    /// </summary>
    public class WorkDoc_Para
    {
        /// <summary>
        /// 当前用户所查看的人的ID
        /// </summary>
        public string memberid { get; set; }
        public int count { get; set; }
        public int page { get; set; }
        public string companyId { get; set; }
    }
    /// <summary>
    /// 执行人信息
    /// </summary>
    public class PeoPleInfo
    {
        /// <summary>
        /// 1个人 2部门 3群组的ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 0最近 1个人 2部门 3群组
        /// </summary>
        public string type { get; set; }
    }
    /// <summary>
    /// 审批人信息
    /// </summary>
    public class ApprovalPeoPleInfo
    {
        /// <summary>
        /// 1个人 2部门 3群组的ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 0最近 1个人 2部门 3群组
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 审批人顺序
        /// </summary>
        public string order { get; set; }
    }
    /// <summary>
    /// 执行人  选择群组时  解析
    /// </summary>
    public class PeoPleInfo2
    {
        public string memberid { get; set; }
    }
    /// <summary>
    /// 工作列表分类
    /// </summary>
    public class WorkList
    {
        public int type { get; set; }
        public string name { get; set; }
    }
    /// <summary>
    /// 工作列表  入参
    /// </summary>
    public class WorkListAllInPara
    {
        /// <summary>
        /// 类型  0全部 1审批 2日志 3任务 4日程 5指令 8公告 9分享
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 所在公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 工作列表
    /// </summary>
    public class WorkListAll
    {
        public string dyTypeName { get; set; }
        /// <summary>
        /// 类型  0全部 1审批 2日志 3任务 4日程 5指令
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int allPage { get; set; }
        /// <summary>
        /// 全部
        /// </summary>
        public List<WorkAll> WorkGetAllList { get; set; }
        /// <summary>
        /// 审批列表
        /// </summary>
        public List<DaiShenPi> WorkGetApprovalRemindList { get; set; }
        /// <summary>
        /// 日志列表
        /// </summary>
        public List<DaiDianPingDeRiZhi> WorkGetLogRemindList { get; set; }
        /// <summary>
        /// 任务列表
        /// </summary>
        public List<DaiZhiXingDeRenWu> WorkGetIPublishTaskList { get; set; }
        /// <summary>
        /// 日程列表
        /// </summary>
        public List<RiChengDetail> WorkGetProgramNoticeList { get; set; }
        /// <summary>
        /// 指令列表
        /// </summary>
        public List<ZhiLingDetail> WorkGetOrderRemindList { get; set; }
        /// <summary>
        /// 公告列表
        /// </summary>
        public List<GongGaoDetail> WorkGetAnnouncementRemindList { get; set; }
        /// <summary>
        /// 分享列表
        /// </summary>
        public List<FenXiangDetail> WorkGetShareRemindList { get; set; }
    }
    /// <summary>
    /// 工作列表  全部   SQL
    /// </summary>
    public class WorkAllSQL
    {
        /// <summary>
        /// 个人动态的类型(审批 日志 任务 日程 指令)
        /// </summary>
        public string dynamicType { get; set; }
        /// <summary>
        /// 1 2 3 4 5
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 此动态创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 动态ID
        /// </summary>
        public string id { get; set; }
    }
    /// <summary>
    /// 工作列表  全部
    /// </summary>
    public class WorkAll
    {
        public string dyTypeName { get; set; }
        /// <summary>
        /// 1 2 3 4 5
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 动态ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 审批列表
        /// </summary>
        public List<DaiShenPi> WorkGetApprovalRemindList { get; set; }
        /// <summary>
        /// 日志列表
        /// </summary>
        public List<DaiDianPingDeRiZhi> WorkGetLogRemindList { get; set; }
        /// <summary>
        /// 任务列表
        /// </summary>
        public List<DaiZhiXingDeRenWu> WorkGetIPublishTaskList { get; set; }
        /// <summary>
        /// 日程列表
        /// </summary>
        public List<RiChengDetail> WorkGetProgramNoticeList { get; set; }
        /// <summary>
        /// 指令列表
        /// </summary>
        public List<ZhiLingDetail> WorkGetOrderRemindList { get; set; }
        /// <summary>
        /// 公告列表
        /// </summary>
        public List<GongGaoDetail> WorkGetAnnouncementRemindList { get; set; }
        /// <summary>
        /// 分享列表
        /// </summary>
        public List<FenXiangDetail> WorkGetShareRemindList { get; set; }
    }
    /// <summary>
    /// 审批   入参
    /// </summary>
    public class ApprovalInPara
    {
        /// <summary>
        /// 审批ID
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 审批创建人ID
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 审批回复内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 是否同意  1是 0否
        /// </summary>
        public int isAgree { get; set; }
        /// <summary>
        /// 附加图片
        /// </summary>
        public string appendPicture { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string annex { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string _person { get; set; }
    }
    /// <summary>
    /// 取消审批   入参
    /// </summary>
    public class EscApprovalInPara
    {
        /// <summary>
        /// 审批ID
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 审批创建人ID
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
    }
    /// <summary>
    /// 重新选择审批人   入参
    /// </summary>
    public class AgainChooseApprovalPersonInPara
    {
        /// <summary>
        /// 审批ID
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 新的审批人
        /// </summary>
        public string newApprovalPerson { get; set; }
    }
    /// <summary>
    /// 执行  入参
    /// </summary>
    public class ExeInPara
    {
        /// <summary>
        /// 执行类型 3任务 4日程 5指令
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 任务 日程 指令 ID
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 主表创建人ID
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 附加图片
        /// </summary>
        public string appendPicture { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string annex { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string _person { get; set; }
    }
    /// <summary>
    /// 取消执行  入参
    /// </summary>
    public class NoExeInPara
    {
        /// <summary>
        /// 执行类型 3任务 4日程 5指令
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 任务 日程 指令 ID
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 主表创建人ID
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
    }
    /// <summary>
    /// 回执  入参
    /// </summary>
    public class ReceInPara
    {
        /// <summary>
        /// 回执类型 8公告 9分享
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 主表ID
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 主表创建人ID
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string phoneModel { get; set; }
        /// <summary>
        /// 附加图片
        /// </summary>
        public string appendPicture { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string annex { get; set; }
        /// <summary>
        /// 录音
        /// </summary>
        public string voice { get; set; }
        /// <summary>
        /// 录音时长
        /// </summary>
        public string voiceLength { get; set; }
        /// <summary>
        /// @某人
        /// </summary>
        public string _person { get; set; }
    }
    /// <summary>
    /// 提醒时间
    /// </summary>
    public class RemindTime
    {
        public string timeWord { get; set; }
        public string time { get; set; }
    }
    /// <summary>
    /// 搜索  入参
    /// </summary>
    public class AllSearch
    {
        /// <summary>
        /// 要搜索的内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 所在公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 普通搜索  入参
    /// </summary>
    public class Search
    {
        /// <summary>
        /// 要搜索的内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 所在公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 高级搜索  入参
    /// </summary>
    public class HighSearch
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 所在公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 高级搜索   类型列表
    /// </summary>
    public class HighSearchType
    {
        /// <summary>
        /// 类型   数字
        /// </summary>
        public string typeNum { get; set; }
        /// <summary>
        /// 类型   汉字
        /// </summary>
        public string typeName { get; set; }
        /// <summary>
        /// 状态列表
        /// </summary>
        public List<HighSearchTypeState> highSearchTypeState { get; set; }
        public string highSearchTypeStateStr { get; set; }
    }
    /// <summary>
    /// 高级搜索   状态列表
    /// </summary>
    public class HighSearchTypeState
    {
        /// <summary>
        /// 主表类型   数字
        /// </summary>
        public string typeNum { get; set; }
        /// <summary>
        /// 状态  数字
        /// </summary>
        public string stateNum { get; set; }
        /// <summary>
        /// 状态  汉字
        /// </summary>
        public string stateName { get; set; }
    }
    /// <summary>
    /// 日志周计划  周列表    入参
    /// </summary>
    public class GetOneDayInPara
    {
        public string oneDay { get; set; }
    }
    /// <summary>
    /// 日志周计划  周列表
    /// </summary>
    public class GetOneDay
    {
        public string mondayStr { get; set; }
        public string sundayStr { get; set; }
    }
    /// <summary>
    /// 点击更多返回的状态值  入参
    /// </summary>
    public class FocusCollectionDeleteStateInPara
    {
        /// <summary>
        /// 主表类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 主表ID
        /// </summary>
        public string uid { get; set; }
    }
    /// <summary>
    /// 点击更多返回的状态值
    /// </summary>
    public class FocusCollectionDeleteState
    {
        /// <summary>
        /// 当前用户是否关注此动态  1是 0否
        /// </summary>
        public string isFocus { get; set; }
        /// <summary>
        /// 当前用户是否收藏此动态  1是 0否
        /// </summary>
        public string isCollection { get; set; }
        /// <summary>
        /// 1可删除日志/可删除任务/可删除日程/可删除指令/可删除公告/可删除分享      调用News/Delete接口
        /// 2执行人可取消任务/日程/指令                       调用Work/NoExe接口
        /// 3创建人可取消审批/可取消任务/可取消指令           调用News/Cancel接口
        /// 4可删除审批/更换审批人                            删除审批调用News/Delete接口,更换审批人调用Work/AgainChooseApprovalPerson接口
        /// 5审批人可取消审批                                 调用Work/EscApproval接口
        /// ""不可删除审批/不可删除日志/不可删除任务/不可删除日程/不可删除指令/执行人不可取消审批/任务/日程/指令
        /// </summary>
        public string isDelete { get; set; }
    }
    /// <summary>
    /// 获取中国城市列表
    /// </summary>
    public class ChinaCity
    {
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public List<string> city { get; set; }
    }
    /// <summary>
    /// 获取中国城市列表
    /// </summary>
    public class ChinaCity_Province
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    /// <summary>
    /// 搜索城市  入参
    /// </summary>
    public class ChinaCitySearchInPara
    {
        /// <summary>
        /// 要搜索的内容
        /// </summary>
        public string body { get; set; }
    }
    /// <summary>
    /// 搜索城市
    /// </summary>
    public class ChinaCitySearch
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        public string cityName { get; set; }
    }
    /// <summary>
    /// 抄送范围/参与人详情
    /// </summary>
    /// <returns></returns>
    public class RangeInfo
    {
        public string memberid { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string jobName { get; set; }
        public string type { get; set; }
    }
    public class WorkTestInPara
    {
        public string date { get; set; }
    }
    /// <summary>
    /// 工作列表标签顺序  写入  传参
    /// </summary>
    /// <returns></returns>
    public class WorkListTagDESInPara
    {
        public string workListTagDES { get; set; }
    }
    /// <summary>
    /// 工作列表标签顺序  解析类
    /// </summary>
    /// <returns></returns>
    public class WorkListTagDES
    {
        /// <summary>
        /// 顺序
        /// </summary>
        public string des { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 标签id  0全部 1审批 2日志 3任务 4日程 5指令 8公告 9分享
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool isUse { get; set; }
    }
    public class GetOneWorkInfopara
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
    }
    /// <summary>
    /// 签到类型
    /// </summary>
    /// <returns></returns>
    public class QianDaoType
    {
        /// <summary>
        /// 签到类型
        /// </summary>
        public string type { get; set; }
    }
    /// <summary>
    /// 修改抄送范围  入参
    /// </summary>
    /// <returns></returns>
    public class UpdateWorkRangeInPara
    {
        /// <summary>
        /// 类型(1审批 2日志 3任务 4日程 5指令 6群通知 8公告 9分享)
        /// </summary>
        public string type { get; set; }
        public string id { get; set; }
        public string range { get; set; }
    }
}
