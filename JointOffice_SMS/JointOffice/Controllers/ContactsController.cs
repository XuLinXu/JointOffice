using JointOffice.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.Configuration;
using Microsoft.Extensions.Options;
using JointOffice.DbModel;

namespace JointOffice.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        private readonly IContacts _IContacts;
        ExceptionMessage em;
        IOptions<Root> config;
        public ContactsController(IOptions<Root> config, IContacts IContacts, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _IContacts = IContacts;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
        }
        /// <summary>
        /// 获取人员资料
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetMemberinfo")]
        public Showapi_Res_Single<Memberinfo> GetMemberinfo([FromBody]MemberID para)
        {
            Showapi_Res_Single<Memberinfo> res = new Showapi_Res_Single<Memberinfo>();
            try
            {
                if (string.IsNullOrEmpty(para.memberid))
                {
                    throw new BusinessException("确认用户ID是否正确.");
                }
                return _IContacts.GetMemberinfo(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 获取人员资料
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetMemberinfoSecond")]
        public Showapi_Res_Single<Memberinfo> GetMemberinfoSecond()
        {
            Showapi_Res_Single<Memberinfo> res = new Showapi_Res_Single<Memberinfo>();
            try
            {
                var memberid = string.IsNullOrEmpty(Request.Form["memberid"]) ? "" : Request.Form["memberid"].ToString();
                if (string.IsNullOrEmpty(memberid))
                {
                    throw new BusinessException("确认用户ID是否正确.");
                }
                MemberID MemberID = new MemberID();
                MemberID.memberid = memberid;
                return _IContacts.GetMemberinfo(MemberID);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetLianXiRenList")]
        public Showapi_Res_List<LianXiRenInfo> GetLianXiRenList([FromBody]GetLianXiRenListInPara para)
        {
            Showapi_Res_List<LianXiRenInfo> res = new Showapi_Res_List<LianXiRenInfo>();
            try
            {
                return _IContacts.GetLianXiRenList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 按组织架构查看联系人列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetLianXiRenListByBuMen")]
        public Showapi_Res_Single<ContactsList> GetLianXiRenListByBuMen([FromBody]UpdateMember_info id)
        {
            Showapi_Res_Single<ContactsList> res = new Showapi_Res_Single<ContactsList>();
            try
            {
                //if (string.IsNullOrEmpty(id.id))
                //{
                //    throw new BusinessException("参数不正确.");
                //}
                return _IContacts.GetLianXiRenListByBuMen(id);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 查看部门列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetBuMenList")]
        public Showapi_Res_List<BuMenInfo> GetBuMenList([FromBody]CompanyPersonListInPara para)
        {
            Showapi_Res_List<BuMenInfo> res = new Showapi_Res_List<BuMenInfo>();
            try
            {
                return _IContacts.GetBuMenList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;

            }
        }
        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="邮箱"></param>
        /// <returns></returns>
        [HttpPost("UpdateMail")]
        public Showapi_Res_Meaasge UpdateMail([FromBody]UpdateMemberinfo_Mail mail)
        {
            try
            {
                if (string.IsNullOrEmpty(mail.mail))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.UpdateMail(mail);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改QQ
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        [HttpPost("UpdateQQ")]
        public Showapi_Res_Meaasge UpdateQQ([FromBody]UpdateMemberinfo_QQ qq)
        {
            try
            {
                if (string.IsNullOrEmpty(qq.qq))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.UpdateQQ(qq);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改微信
        /// </summary>
        /// <param name="微信"></param>
        /// <returns></returns>
        [HttpPost("UpdateWeChat")]
        public Showapi_Res_Meaasge UpdateWeChat([FromBody]UpdateMemberinfo_WeChat wechat)
        {
            try
            {
                if (string.IsNullOrEmpty(wechat.wechat))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.UpdateWeChat(wechat);
            }
            catch(Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        ///// <summary>
        ///// 修改职务
        ///// </summary>
        ///// <param name="职务"></param>
        ///// <returns></returns>
        //[HttpPost("UpdateJobName")]
        //public Showapi_Res_Meaasge UpdateJobName([FromBody]UpdateMemberinfo_JobName jobname)
        //{
        //    try
        //    {
        //        return _IContacts.UpdateJobName(jobname);
        //    }
        //    catch (Exception ex)
        //    {
        //        return em.ReturnMeaasge(ex);
        //    }
        //}
        /// <summary>
        /// 修改性别
        /// </summary>
        /// <param name="性别"></param>
        /// <returns></returns>
        [HttpPost("UpdateGender")]
        public Showapi_Res_Meaasge UpdateGender([FromBody]UpdateMemberinfo_Gender gender)
        {
            try
            {
                if (string.IsNullOrEmpty(gender.gender.ToString()))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.UpdateGender(gender);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改汇报对象
        /// </summary>
        /// <param name="汇报对象"></param>
        /// <returns></returns>
        [HttpPost("UpdateHuiBaoDuiXiang")]
        public Showapi_Res_Meaasge UpdateHuiBaoDuiXiang([FromBody]UpdateMemberinfo_HuiBaoDuiXiang huibaoduixiang)
        {
            try
            {
                if (string.IsNullOrEmpty(huibaoduixiang.huibaoduixiang))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.UpdateHuiBaoDuiXiang(huibaoduixiang);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改电话
        /// </summary>
        /// <param name="电话"></param>
        /// <returns></returns>
        [HttpPost("UpdatePhone")]
        public Showapi_Res_Meaasge UpdatePhone([FromBody]UpdateMemberinfo_Phone phone)
        {
            try
            {
                if (string.IsNullOrEmpty(phone.phone))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.UpdatePhone(phone);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改工作介绍
        /// </summary>
        /// <param name="工作介绍"></param>
        /// <returns></returns>
        [HttpPost("UpdateGongZuoJieShao")]
        public Showapi_Res_Meaasge UpdateGongZuoJieShao([FromBody]UpdateMemberinfo_GongZuoJieShao gongzuojieshao)
        {
            try
            {
                if (string.IsNullOrEmpty(gongzuojieshao.gongzuojieshao))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.UpdateGongZuoJieShao(gongzuojieshao);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="头像"></param>
        /// <returns></returns>
        [HttpPost("UpdateInfoUrl")]
        public Showapi_Res_Meaasge UpdateInfoUrl()
        {
            try
            {
                var files = Request.Form.Files;
                var blobFiles = CloudBlobHelper.SelectBlobFiles(files);
                CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                return _IContacts.UpdateInfoUrl(blobFiles.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改个人信息
        /// </summary>
        [HttpPost("UpdatePersonInfo")]
        public Showapi_Res_Meaasge UpdatePersonInfo([FromBody]UpdatePersonInfoInPara para)
        {
            try
            {
                return _IContacts.UpdatePersonInfo(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 设为星标联系人
        /// </summary>
        /// <param name="对方ID"></param>
        /// <returns></returns>
        [HttpPost("SetStar")]
        public Showapi_Res_Meaasge SetStar([FromBody]UpdateMemberinfo_SetStar othermemberid)
        {
            try
            {
                if (string.IsNullOrEmpty(othermemberid.othermemberid))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.SetStar(othermemberid);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 取消星标联系人
        /// </summary>
        /// <param name="对方ID"></param>
        /// <returns></returns>
        [HttpPost("NoSetStar")]
        public Showapi_Res_Meaasge NoSetStar([FromBody]UpdateMemberinfo_NoSetStar othermemberid)
        {
            try
            {
                if (string.IsNullOrEmpty(othermemberid.othermemberid))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.NoSetStar(othermemberid);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 点赞  取消点赞
        /// </summary>
        /// <returns></returns>
        [HttpPost("SetAgree")]
        public Showapi_Res_Meaasge SetAgree([FromBody]SetAgreePara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.isDian.ToString()) || string.IsNullOrEmpty(para.uid) || string.IsNullOrEmpty(para.otherMemberId) || string.IsNullOrEmpty(para.type))
                {
                    throw new BusinessException("参数不正确.");
                }
                if (string.IsNullOrEmpty(para.body) || string.IsNullOrEmpty(para.phoneModel))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.SetAgree(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id,类型"></param>
        /// <returns></returns>
        [HttpPost("NoSetComment")]
        public Showapi_Res_Meaasge NoSetComment([FromBody]NoSetCommentPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.NoSetComment(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 查看赞人员
        /// </summary>
        /// <param name="id,类型"></param>
        /// <returns></returns>
        [HttpPost("GetZanRenYuanList")]
        public Showapi_Res_List<ZanRenYuanList> GetZanRenYuanList([FromBody]GetZanRenYuanListPara para)
        {
            Showapi_Res_List<ZanRenYuanList> res = new Showapi_Res_List<ZanRenYuanList>();
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.GetZanRenYuanList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 查看评论列表
        /// </summary>
        /// <param name="id,类型"></param>
        /// <returns></returns>
        [HttpPost("GetPingLunList")]
        public Showapi_Res_List<PingLun> GetPingLunList([FromBody]GetZanRenYuanListPara para)
        {
            Showapi_Res_List<PingLun> res = new Showapi_Res_List<PingLun>();
            try
            {
                if (string.IsNullOrEmpty(para.id) || string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.GetPingLunList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 搜索联系人
        /// </summary>
        /// <param name="id,类型"></param>
        /// <returns></returns>
        [HttpPost("GetLianXiRenListByName")]
        public Showapi_Res_List<LianXiRenInfo> GetLianXiRenListByName([FromBody]GetLianXiRenListByNamePara para)
        {
            Showapi_Res_List<LianXiRenInfo> res = new Showapi_Res_List<LianXiRenInfo>();
            try
            {
                if (string.IsNullOrEmpty(para.name))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.GetLianXiRenListByName(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 获取当前用户通讯录首页  所属公司信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetPersonAddressBook")]
        public Showapi_Res_List<PersonAddressBook> GetPersonAddressBook()
        {
            Showapi_Res_List<PersonAddressBook> res = new Showapi_Res_List<PersonAddressBook>();
            try
            {
                return _IContacts.GetPersonAddressBook();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取某公司下所有人员
        /// </summary>
        [HttpPost("GetCompanyPersonList")]
        public Showapi_Res_List<SearchPersonList> GetCompanyPersonList([FromBody]CompanyPersonListInPara para)
        {
            Showapi_Res_List<SearchPersonList> res = new Showapi_Res_List<SearchPersonList>();
            try
            {
                return _IContacts.GetCompanyPersonList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 搜索联系人
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetSearchPersonList")]
        public Showapi_Res_List<SearchPersonList> GetSearchPersonList([FromBody]SearchPersonInPara para)
        {
            Showapi_Res_List<SearchPersonList> res = new Showapi_Res_List<SearchPersonList>();
            try
            {
                return _IContacts.GetSearchPersonList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddContact")]
        public Showapi_Res_Meaasge AddContact([FromBody]UpdateMember_info para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.id))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IContacts.AddContact(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 我的联系人列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetMyContactList")]
        public Showapi_Res_List<SearchPersonList> GetMyContactList([FromBody]MyContactListInPara para)
        {
            Showapi_Res_List<SearchPersonList> res = new Showapi_Res_List<SearchPersonList>();
            try
            {
                return _IContacts.GetMyContactList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取Member_Info表中全部信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllPerson")]
        public Showapi_Res_List<GetAllPerson> GetAllPerson()
        {
            Showapi_Res_List<GetAllPerson> res = new Showapi_Res_List<GetAllPerson>();
            try
            {
                return _IContacts.GetAllPerson();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetCompanyList")]
        public Showapi_Res_List<CompanyList> GetCompanyList()
        {
            Showapi_Res_List<CompanyList> res = new Showapi_Res_List<CompanyList>();
            try
            {
                return _IContacts.GetCompanyList();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDeptList")]
        public Showapi_Res_List<DeptList> GetDeptList([FromBody]UpdateMember_info para)
        {
            Showapi_Res_List<DeptList> res = new Showapi_Res_List<DeptList>();
            try
            {
                return _IContacts.GetDeptList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取job信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetJobList")]
        public Showapi_Res_List<CompanyJobList> GetJobList([FromBody]GetJobListInPara para)
        {
            Showapi_Res_List<CompanyJobList> res = new Showapi_Res_List<CompanyJobList>();
            try
            {
                return _IContacts.GetJobList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取部门负责人
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDeptBossList")]
        public Showapi_Res_List<CompanyList> GetDeptBossList([FromBody]UpdateMember_info para)
        {
            Showapi_Res_List<CompanyList> res = new Showapi_Res_List<CompanyList>();
            try
            {
                return _IContacts.GetDeptBossList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 编辑人员公司信息  后台
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddPersonCompany")]
        public Showapi_Res_Meaasge AddPersonCompany([FromBody]AddPersonCompanyInPara para)
        {
            try
            {
                return _IContacts.AddPersonCompany(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 编辑人员公司信息  两端
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdatePersonComInfo")]
        public Showapi_Res_Meaasge UpdatePersonComInfo([FromBody]UpdatePersonComInfoInPara para)
        {
            try
            {
                return _IContacts.UpdatePersonComInfo(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 获取我的群组列表
        /// </summary>
        [HttpPost("GetMyGroupList")]
        public Showapi_Res_List<MyGroupList> GetMyGroupList([FromBody]MyGroupListInPara para)
        {
            Showapi_Res_List<MyGroupList> res = new Showapi_Res_List<MyGroupList>();
            try
            {
                return _IContacts.GetMyGroupList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 后台修改人员公司信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdatePersonComInfoBack")]
        public Showapi_Res_Meaasge UpdatePersonComInfoBack([FromBody]UpdatePersonComInfoBackInPara para)
        {
            try
            {
                return _IContacts.UpdatePersonComInfoBack(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 生成二维码并上传
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateQRCode")]
        public Showapi_Res_Meaasge CreateQRCode([FromBody]UpdateMember_info para)
        {
            try
            {
                return _IContacts.CreateQRCode(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 获取某人的二维码
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetPersonQRCode")]
        public Showapi_Res_Single<PersonDynamic_info_url> GetPersonQRCode([FromBody]UpdateMember_info para)
        {
            Showapi_Res_Single<PersonDynamic_info_url> res = new Showapi_Res_Single<PersonDynamic_info_url>();
            try
            {
                return _IContacts.GetPersonQRCode(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 常用联系人
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetOftenContactList")]
        public Showapi_Res_List<SearchPersonList> GetOftenContactList()
        {
            Showapi_Res_List<SearchPersonList> res = new Showapi_Res_List<SearchPersonList>();
            try
            {
                return _IContacts.GetOftenContactList();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// web左上角切换公司时人员job随之变化
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetWebMemberInfo")]
        public Showapi_Res_Single<WebMemberInfo> GetWebMemberInfo([FromBody]UpdateMember_info para)
        {
            Showapi_Res_Single<WebMemberInfo> res = new Showapi_Res_Single<WebMemberInfo>();
            try
            {
                return _IContacts.GetWebMemberInfo(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 后台新建+修改个人基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdatePersonInfoBack")]
        public Showapi_Res_Meaasge UpdatePersonInfoBack([FromBody]GetAllPerson para)
        {
            try
            {
                return _IContacts.UpdatePersonInfoBack(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 后台组织架构管理  获取全部公司部门信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllDeptListBack")]
        public Showapi_Res_List<AllDeptListBack> GetAllDeptListBack()
        {
            Showapi_Res_List<AllDeptListBack> res = new Showapi_Res_List<AllDeptListBack>();
            try
            {
                return _IContacts.GetAllDeptListBack();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 后台新建+编辑组织
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddDeptBack")]
        public Showapi_Res_Meaasge AddDeptBack([FromBody]AddDeptBackInPara para)
        {
            try
            {
                return _IContacts.AddDeptBack(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 后台禁用启用删除账户
        /// </summary>
        /// <returns></returns>
        [HttpPost("OpenCloseUser")]
        public Showapi_Res_Meaasge OpenCloseUser([FromBody]OpenCloseUserInPara para)
        {
            try
            {
                return _IContacts.OpenCloseUser(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 后台删除某人公司信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeletePersonCompanyInfo")]
        public Showapi_Res_Meaasge DeletePersonCompanyInfo([FromBody]UpdateMember_info para)
        {
            try
            {
                return _IContacts.DeletePersonCompanyInfo(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 后台新建+编辑职务
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddJobBack")]
        public Showapi_Res_Meaasge AddJobBack([FromBody]AddDeptBackInPara para)
        {
            try
            {
                return _IContacts.AddJobBack(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 后台根据部门id获取job信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetJobListByDeptId")]
        public Showapi_Res_List<CompanyJobList> GetJobListByDeptId([FromBody]UpdateMember_info para)
        {
            Showapi_Res_List<CompanyJobList> res = new Showapi_Res_List<CompanyJobList>();
            try
            {
                return _IContacts.GetJobListByDeptId(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 后台删除组织
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteDeptBack")]
        public Showapi_Res_Meaasge DeleteDeptBack([FromBody]UpdateMember_info para)
        {
            try
            {
                return _IContacts.DeleteDeptBack(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 后台删除职务
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteJobBack")]
        public Showapi_Res_Meaasge DeleteJobBack([FromBody]UpdateMember_info para)
        {
            try
            {
                return _IContacts.DeleteJobBack(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 后台新建角色
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddRoleBack")]
        public Showapi_Res_Meaasge AddRoleBack([FromBody]GetAllRoleBack para)
        {
            try
            {
                return _IContacts.AddRoleBack(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 后台获取所有角色
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllRoleBack")]
        public Showapi_Res_List<GetAllRoleBack> GetAllRoleBack()
        {
            Showapi_Res_List<GetAllRoleBack> res = new Showapi_Res_List<GetAllRoleBack>();
            try
            {
                return _IContacts.GetAllRoleBack();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }


        ///// <summary>
        ///// 根据部门获取父公司  并写入
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("DeptToCompany")]
        //public Showapi_Res_Meaasge DeptToCompany()
        //{
        //    try
        //    {
        //        return _IContacts.DeptToCompany();
        //    }
        //    catch (Exception ex)
        //    {
        //        return em.ReturnMeaasge(ex);
        //    }
        //}
        ///// <summary>
        ///// 同步job
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("AddMemberJob")]
        //public Showapi_Res_Meaasge AddMemberJob()
        //{
        //    try
        //    {
        //        return _IContacts.AddMemberJob();
        //    }
        //    catch (Exception ex)
        //    {
        //        return em.ReturnMeaasge(ex);
        //    }
        //}
        ///// <summary>
        ///// 同步jobid
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("UpdateMemberJobId")]
        //public Showapi_Res_Meaasge UpdateMemberJobId()
        //{
        //    try
        //    {
        //        return _IContacts.UpdateMemberJobId();
        //    }
        //    catch (Exception ex)
        //    {
        //        return em.ReturnMeaasge(ex);
        //    }
        //}
        ///// <summary>
        ///// 修改工作模块表的CompanyId字段
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("UpdateWorkCompanyId")]
        //public Showapi_Res_Meaasge UpdateWorkCompanyId()
        //{
        //    try
        //    {
        //        return _IContacts.UpdateWorkCompanyId();
        //    }
        //    catch (Exception ex)
        //    {
        //        return em.ReturnMeaasge(ex);
        //    }
        //}
        ///// <summary>
        ///// 同步Member_Info_Company表数据
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("UpdateMemberInfoCompany")]
        //public Showapi_Res_Meaasge UpdateMemberInfoCompany()
        //{
        //    try
        //    {
        //        return _IContacts.UpdateMemberInfoCompany();
        //    }
        //    catch (Exception ex)
        //    {
        //        return em.ReturnMeaasge(ex);
        //    }
        //}
        ///// <summary>
        ///// 同步正式库job表的MemberDeptId字段
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("UpdateJobDeptId")]
        //public Showapi_Res_Meaasge UpdateJobDeptId()
        //{
        //    try
        //    {
        //        return _IContacts.UpdateJobDeptId();
        //    }
        //    catch (Exception ex)
        //    {
        //        return em.ReturnMeaasge(ex);
        //    }
        //}
    }
}
