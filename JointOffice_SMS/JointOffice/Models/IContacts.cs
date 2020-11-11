using JointOffice.DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public interface IContacts
    {
        /// <summary>
        /// 获取人员资料
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<Memberinfo> GetMemberinfo(MemberID para);
        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<LianXiRenInfo> GetLianXiRenList(GetLianXiRenListInPara para);
        ////获取组织架构列表
        //Showapi_Res_List<LianXiRenInfo> GetDepartmentList();
        /// <summary>
        /// 按组织架构查看联系人列表
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<ContactsList> GetLianXiRenListByBuMen(UpdateMember_info para);
        /// <summary>
        /// 查看部门列表
        /// </summary>
        Showapi_Res_List<BuMenInfo> GetBuMenList(CompanyPersonListInPara para);
        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="邮箱"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateMail(UpdateMemberinfo_Mail para);
        /// <summary>
        /// 修改QQ
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateQQ(UpdateMemberinfo_QQ para);
        /// <summary>
        /// 修改微信
        /// </summary>
        /// <param name="微信"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateWeChat(UpdateMemberinfo_WeChat para);
        /// <summary>
        /// 修改职务
        /// </summary>
        /// <param name="职务"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateJobName(UpdateMemberinfo_JobName para);
        /// <summary>
        /// 修改性别
        /// </summary>
        /// <param name="性别"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateGender(UpdateMemberinfo_Gender para);
        /// <summary>
        /// 修改汇报对象
        /// </summary>
        /// <param name="汇报对象"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateHuiBaoDuiXiang(UpdateMemberinfo_HuiBaoDuiXiang para);
        /// <summary>
        /// 修改电话
        /// </summary>
        /// <param name="电话"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdatePhone(UpdateMemberinfo_Phone para);
        /// <summary>
        /// 修改工作介绍
        /// </summary>
        /// <param name="工作介绍"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateGongZuoJieShao(UpdateMemberinfo_GongZuoJieShao para);
        /// <summary>
        /// 修改头像
        /// </summary>
        Showapi_Res_Meaasge UpdateInfoUrl(BlobFilePara para);
        /// <summary>
        /// 修改个人信息
        /// </summary>
        Showapi_Res_Meaasge UpdatePersonInfo(UpdatePersonInfoInPara para);
        /// <summary>
        /// 设为星标联系人
        /// </summary>
        /// <param name="对方ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge SetStar(UpdateMemberinfo_SetStar para);
        /// <summary>
        /// 取消星标联系人
        /// </summary>
        /// <param name="对方ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge NoSetStar(UpdateMemberinfo_NoSetStar para);
        /// <summary>
        /// 点赞  取消点赞
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge SetAgree(SetAgreePara para);
        /// <summary>
        /// 删除评论
        /// </summary>
        Showapi_Res_Meaasge NoSetComment(NoSetCommentPara para);
        /// <summary>
        /// 查看赞人员
        /// </summary>
        Showapi_Res_List<ZanRenYuanList> GetZanRenYuanList(GetZanRenYuanListPara para);
        /// <summary>
        /// 查看评论列表
        /// </summary>
        Showapi_Res_List<PingLun> GetPingLunList(GetZanRenYuanListPara para);
        /// <summary>
        /// 搜索联系人
        /// </summary>
        Showapi_Res_List<LianXiRenInfo> GetLianXiRenListByName(GetLianXiRenListByNamePara para);
        /// <summary>
        /// 获取当前用户通讯录首页  所属公司信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<PersonAddressBook> GetPersonAddressBook();
        /// <summary>
        /// 获取某公司下所有人员
        /// </summary>
        Showapi_Res_List<SearchPersonList> GetCompanyPersonList(CompanyPersonListInPara para);
        /// <summary>
        /// 搜索联系人
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<SearchPersonList> GetSearchPersonList(SearchPersonInPara para);
        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge AddContact(UpdateMember_info para);
        /// <summary>
        /// 我的联系人列表
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<SearchPersonList> GetMyContactList(MyContactListInPara para);
        /// <summary>
        /// 获取Member_Info表中全部信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<GetAllPerson> GetAllPerson();
        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<CompanyList> GetCompanyList();
        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<DeptList> GetDeptList(UpdateMember_info para);
        /// <summary>
        /// 获取job信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<CompanyJobList> GetJobList(GetJobListInPara para);
        /// <summary>
        /// 获取部门负责人
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<CompanyList> GetDeptBossList(UpdateMember_info para);
        /// <summary>
        /// 编辑人员公司信息  后台
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge AddPersonCompany(AddPersonCompanyInPara para);
        /// <summary>
        /// 编辑人员公司信息  两端
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdatePersonComInfo(UpdatePersonComInfoInPara para);
        /// <summary>
        /// 获取我的群组列表
        /// </summary>
        Showapi_Res_List<MyGroupList> GetMyGroupList(MyGroupListInPara para);
        /// <summary>
        /// 后台修改人员公司信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdatePersonComInfoBack(UpdatePersonComInfoBackInPara para);
        /// <summary>
        /// 生成二维码并上传
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge CreateQRCode(UpdateMember_info para);
        /// <summary>
        /// 获取某人的二维码
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<PersonDynamic_info_url> GetPersonQRCode(UpdateMember_info para);
        /// <summary>
        /// 常用联系人
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<SearchPersonList> GetOftenContactList();
        /// <summary>
        /// web左上角切换公司时人员job随之变化
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<WebMemberInfo> GetWebMemberInfo(UpdateMember_info para);
        /// <summary>
        /// 后台新建+修改个人基本信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdatePersonInfoBack(GetAllPerson para);
        /// <summary>
        /// 后台组织架构管理  获取全部公司部门信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<AllDeptListBack> GetAllDeptListBack();
        /// <summary>
        /// 后台新建+编辑组织
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge AddDeptBack(AddDeptBackInPara para);
        /// <summary>
        /// 后台禁用启用删除账户
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge OpenCloseUser(OpenCloseUserInPara para);
        /// <summary>
        /// 后台删除某人公司信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge DeletePersonCompanyInfo(UpdateMember_info para);
        /// <summary>
        /// 后台新建+编辑职务
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge AddJobBack(AddDeptBackInPara para);
        /// <summary>
        /// 后台根据部门id获取job信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<CompanyJobList> GetJobListByDeptId(UpdateMember_info para);
        /// <summary>
        /// 后台删除组织
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge DeleteDeptBack(UpdateMember_info para);
        /// <summary>
        /// 后台删除职务
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge DeleteJobBack(UpdateMember_info para);
        /// <summary>
        /// 后台新建角色
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge AddRoleBack(GetAllRoleBack para);
        /// <summary>
        /// 后台获取所有角色
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<GetAllRoleBack> GetAllRoleBack();


        ///// <summary>
        ///// 根据部门获取父公司  并写入
        ///// </summary>
        ///// <returns></returns>
        //Showapi_Res_Meaasge DeptToCompany();
        ///// <summary>
        ///// 同步job
        ///// </summary>
        ///// <returns></returns>
        //Showapi_Res_Meaasge AddMemberJob();
        ///// <summary>
        ///// 同步jobid
        ///// </summary>
        ///// <returns></returns>
        //Showapi_Res_Meaasge UpdateMemberJobId();
        ///// <summary>
        ///// 修改工作模块表的CompanyId字段
        ///// </summary>
        ///// <returns></returns>
        //Showapi_Res_Meaasge UpdateWorkCompanyId();
        ///// <summary>
        ///// 同步Member_Info_Company表数据
        ///// </summary>
        ///// <returns></returns>
        //Showapi_Res_Meaasge UpdateMemberInfoCompany();
        ///// <summary>
        ///// 同步正式库job表的MemberDeptId字段
        ///// </summary>
        ///// <returns></returns>
        //Showapi_Res_Meaasge UpdateJobDeptId();
    }
    /// <summary>
    /// 个人资料
    /// </summary>
    public class Memberinfo
    {
        /// <summary>
        /// 个人ID
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int gender { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string mail { get; set; }
        /// <summary>
        /// 汇报对象id
        /// </summary>
        public string huiBaoDuiXiang { get; set; }
        /// <summary>
        /// 汇报对象name
        /// </summary>
        public string huiBaoDuiXiangName { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string weChat { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string qq { get; set; }
        /// <summary>
        /// 工作介绍
        /// </summary>
        public string gongZuoJieShao { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string jobName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 主部门
        /// </summary>
        public string zhubumen { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 副部门
        /// </summary>
        public string fubumen { get; set; }
        /// <summary>
        /// 部门负责人
        /// </summary>
        public string bumenfuzeren { get; set; }
        public string bumenfuzerenName { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string createdate { get; set; }
        /// <summary>
        /// 主部门名称
        /// </summary>
        public string zhubumenName { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public string jurisdiction { get; set; }
        /// <summary>
        /// 是否星标
        /// </summary>
        public bool isStar { get; set; }
        /// <summary>
        /// 个人公司信息
        /// </summary>
        public List<MemberCompanyInfo> memberCompanyInfo { get; set; }
    }
    /// <summary>
    /// 个人公司信息
    /// </summary>
    public class MemberCompanyInfo
    {
        /// <summary>
        /// Member_Info_Company表id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string company { get; set; }
        /// <summary>
        /// 公司id
        /// </summary>
        public string companyId { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string job { get; set; }
        /// <summary>
        /// 汇报对象id
        /// </summary>
        public string huiBaoDuiXiangID { get; set; }
        /// <summary>
        /// 汇报对象
        /// </summary>
        public string huiBaoDuiXiang { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string dept { get; set; }
        /// <summary>
        /// 部门负责人
        /// </summary>
        public string deptMember { get; set; }
        /// <summary>
        /// 工作介绍
        /// </summary>
        public string gongZuoJieShao { get; set; }
    }
    public class LianXiRenInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 个人ID
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// 主部门
        /// </summary>
        public string zhubumen { get; set; }
        /// <summary>
        /// 主部门名称
        /// </summary>
        public string zhubumenName { get; set; }
        /// <summary>
        /// 是否设为星标
        /// </summary>
        public int isStar { get; set; }
        public int gender { get; set; }
        public string mail { get; set; }
        public string huiBaoDuiXiang { get; set; }
        public string huiBaoDuiXiangName { get; set; }       
        public string weChat { get; set; }
        public string qq { get; set; }
        public string gongZuoJieShao { get; set; }
        public string jobName { get; set; }
        public string fubumen { get; set; }
        public string bumenfuzeren { get; set; }
        public string bumenfuzerenName { get; set; }
        public string phone { get; set; }
        public string jobid { get; set; }
        public string membercode { get; set; }
    }
    public class BuMenInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public int count { get; set; }
        public List<ZanRenYuanList> personList { get; set; }
    }
    public class ContactsList
    {
        public string nowCompanyName { get; set; }
        public List<LianXiRenInfo> lianXiRenList { get; set; }
        public List<BuMenInfo> buMenList { get; set; }
    }
    /// <summary>
    /// 点赞参数
    /// </summary>
    public class SetAgreePara
    {
        /// <summary>
        /// 1点赞  2取消点赞
        /// </summary>
        public int isDian { get; set; }
        /// <summary>
        /// 主表id
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 普通评论ID
        /// </summary>
        public string pid { get; set; }
        /// <summary>
        /// 主表创建人ID
        /// </summary>
        public string otherMemberId { get; set; }
        /// <summary>
        /// 类型(1审批 2日志 3任务 4日程 5指令 6群通知 7评论 8公告 9分享)
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 对应类型的内容  前20个字+...
        /// </summary>
        public string body { get; set; }
        public string phoneModel { get; set; }
    }
    /// <summary>
    /// 删除评论
    /// </summary>
    public class NoSetCommentPara
    {
        /// <summary>
        /// 评论id
        /// </summary>
        public string id { get; set; }
    }
    public class MemberID
    {
        public string memberid { get; set; }
    }
    public class UpdateMember_info
    {
        public string id { get; set; }
    }
    /// <summary>
    /// 邮箱
    /// </summary>
    public class UpdateMemberinfo_Mail
    {
        public string mail { get; set; }
    }
    /// <summary>
    /// QQ
    /// </summary>
    public class UpdateMemberinfo_QQ
    {
        public string qq { get; set; }
    }
    /// <summary>
    /// 微信
    /// </summary>
    public class UpdateMemberinfo_WeChat
    {
        public string wechat { get; set; }
    }
    /// <summary>
    /// 职位
    /// </summary>
    public class UpdateMemberinfo_JobName
    {
        public string jobname { get; set; }
    }
    /// <summary>
    /// 性别
    /// </summary>
    public class UpdateMemberinfo_Gender
    {
        public int gender { get; set; }
    }
    /// <summary>
    /// 汇报对象
    /// </summary>
    public class UpdateMemberinfo_HuiBaoDuiXiang
    {
        public string huibaoduixiang { get; set; }
    }
    /// <summary>
    /// 电话
    /// </summary>
    public class UpdateMemberinfo_Phone
    {
        public string phone { get; set; }
    }
    /// <summary>
    /// 工作介绍
    /// </summary>
    public class UpdateMemberinfo_GongZuoJieShao
    {
        public string gongzuojieshao { get; set; }
    }
    /// <summary>
    /// 修改个人信息  入参
    /// </summary>
    public class UpdatePersonInfoInPara
    {
        /// <summary>
        /// 性别
        /// </summary>
        public int gender { get; set; }
        /// <summary>
        /// 工作介绍
        /// </summary>
        public string gongzuojieshao { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string wechat { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string mail { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string qq { get; set; }
    }
    public class UpdateMemberinfo_SetStar
    {
        public string othermemberid { get; set; }
    }
    public class UpdateMemberinfo_NoSetStar
    {
        public string othermemberid { get; set; }
    }
    /// <summary>
    /// 查看 点赞人员 评论 列表  入参
    /// </summary>
    public class GetZanRenYuanListPara
    {
        /// <summary>
        /// 主表ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
    }
    /// <summary>
    /// 查看赞人员
    /// </summary>
    public class ZanRenYuanList
    {
        public string memberid { get; set; }
        public string picture { get; set; }
        public string name { get; set; }
    }
    /// <summary>
    /// 搜索联系人
    /// </summary>
    /// <returns></returns>
    public class GetLianXiRenListByNamePara
    {
        public string name { get; set; }
    }
    /// <summary>
    /// 当前用户通讯录首页
    /// </summary>
    public class PersonAddressBook
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    /// <summary>
    /// 获取某公司下所有人员  入参
    /// </summary>
    public class CompanyPersonListInPara
    {
        public string id { get; set; }
        public string body { get; set; }
    }
    /// <summary>
    /// 我的联系人列表  入参
    /// </summary>
    /// <returns></returns>
    public class MyContactListInPara
    {
        public string body { get; set; }
    }
    /// <summary>
    /// 搜索联系人  入参
    /// </summary>
    public class SearchPersonInPara
    {
        /// <summary>
        /// 1在我的联系人里搜索  2添加时搜索
        /// </summary>
        public string type { get; set; }
        public string body { get; set; }
    }
    /// <summary>
    /// 返回搜索到的联系人信息
    /// </summary>
    public class SearchPersonList
    {
        public string id { get; set; }
        public string memberid { get; set; }
        public string name { get; set; }
        public string job { get; set; }
        public string company { get; set; }
        public string picture { get; set; }
        public string mobile { get; set; }
        /// <summary>
        /// 是否已添加
        /// </summary>
        public bool isAdd { get; set; }
        /// <summary>
        /// 是否星标
        /// </summary>
        public bool isStar { get; set; }
    }
    /// <summary>
    /// 获取公司信息
    /// </summary>
    /// <returns></returns>
    public class CompanyList
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class CompanyJobList
    {
        public string id { get; set; }
        public string name { get; set; }
        public string nameEx { get; set; }
    }
    /// <summary>
    /// 获取部门信息
    /// </summary>
    /// <returns></returns>
    public class DeptList
    {
        public string id { get; set; }
        public string name { get; set; }
        /// <summary>
        /// 获取部门信息  递归
        /// </summary>
        public List<DeptList> children { get; set; }
    }
    /// <summary>
    /// 编辑人员公司信息  后台  入参
    /// </summary>
    public class AddPersonCompanyInPara
    {
        /// <summary>
        /// 编辑的人的memberid
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 所属公司信息
        /// </summary>
        public List<AddPersonCompanyInParaList> list { get; set; }
    }
    /// <summary>
    /// 所属公司信息
    /// </summary>
    public class AddPersonCompanyInParaList
    {
        public string comId { get; set; }
        public string deptId { get; set; }
        public string jobId { get; set; }
        public string deptBossId { get; set; }
    }
    /// <summary>
    /// 编辑人员公司信息  两端  入参
    /// </summary>
    /// <returns></returns>
    public class UpdatePersonComInfoInPara
    {
        public string id { get; set; }
        public string huiBaoDuiXiang { get; set; }
        public string gongZuoJieShao { get; set; }
    }
    /// <summary>
    /// 获取我的群组列表   入参
    /// </summary>
    public class MyGroupListInPara
    {
        public string body { get; set; }
        /// <summary>
        /// 1我创建的群   2我加入的群
        /// </summary>
        public string type { get; set; }
    }
    /// <summary>
    /// 获取我的群组列表
    /// </summary>
    public class MyGroupList
    {
        public string id { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string personNum { get; set; }
    }
    /// <summary>
    /// 后台修改人员公司信息  入参
    /// </summary>
    /// <returns></returns>
    public class UpdatePersonComInfoBackInPara
    {
        /// <summary>
        /// Member_Info_Company表id
        /// </summary>
        public string id { get; set; }
        public string comId { get; set; }
        public string deptId { get; set; }
        public string jobId { get; set; }
        public string deptBossId { get; set; }
    }
    /// <summary>
    /// 获取联系人列表  入参
    /// </summary>
    /// <returns></returns>
    public class GetLianXiRenListInPara
    {
        /// <summary>
        /// 所在公司id
        /// </summary>
        public string companyId { get; set; }
    }
    /// <summary>
    /// 获取Member_Info表中全部信息
    /// </summary>
    /// <returns></returns>
    public class GetAllPerson
    {
        public string memberid { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string company { get; set; }
        public string job { get; set; }
        public string dept { get; set; }
        public string deptMember { get; set; }
        public string mail { get; set; }
        public string wechat { get; set; }
        public string qq { get; set; }
        public string role { get; set; }
        /// <summary>
        /// 1启用  0禁用  2删除
        /// </summary>
        public int isUse { get; set; }
        /// <summary>
        /// 所属公司信息
        /// </summary>
        public List<AddPersonCompanyInParaList> list { get; set; }
    }
    /// <summary>
    /// web左上角切换公司时人员job随之变化
    /// </summary>
    /// <returns></returns>
    public class WebMemberInfo
    {
        public string memberid { get; set; }
        public string name { get; set; }
        public string jobName { get; set; }
        public string mobile { get; set; }
        public string picture { get; set; }
    }
    /// <summary>
    /// 后台组织架构管理  获取全部公司部门信息
    /// </summary>
    /// <returns></returns>
    public class AllDeptListBack
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<AllDeptListBack> children { get; set; }
    }
    /// <summary>
    /// 后台新建+编辑组织  入参
    /// </summary>
    /// <returns></returns>
    public class AddDeptBackInPara
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string name { get; set; }
    }
    /// <summary>
    /// 后台禁用启用删除账户  入参
    /// </summary>
    /// <returns></returns>
    public class OpenCloseUserInPara
    {
        /// <summary>
        /// 1启用  0禁用  2删除
        /// </summary>
        public int type { get; set; }
        public List<string> memberList { get; set; }
    }
    /// <summary>
    /// 获取job信息  入参
    /// </summary>
    /// <returns></returns>
    public class GetJobListInPara
    {
        public string comId { get; set; }
        public string deptId { get; set; }
    }
    /// <summary>
    /// 后台获取所有角色
    /// </summary>
    /// <returns></returns>
    public class GetAllRoleBack
    {
        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }
}
