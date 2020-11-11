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
using CommonTool.MailKit;
using MimeKit;
using System.IO;
using System.Text.RegularExpressions;
using MimeKit.Text;

namespace JointOffice.Controllers
{
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        private readonly IEmail _IEmail;
        ExceptionMessage em;
        IOptions<Root> config;
        public MailController(IOptions<Root> config, IEmail IEmail, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _IEmail = IEmail;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
        }

        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <returns></returns>
        [HttpPost("UnLogin")]
        public Showapi_Res_Single<EmailResult> UnLogin()
        {
            try
            {
                return _IEmail.UnLogin();
            }
            catch (Exception ex)
            {
                Showapi_Res_Single<EmailResult> res = new Showapi_Res_Single<EmailResult>();
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 邮箱登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("AutoLogin")]
        public Showapi_Res_Single<EmailResult> AutoLogin()
        {
            try
            {
                return _IEmail.AutoLogin();
            }
            catch (Exception ex)
            {
                Showapi_Res_Single<EmailResult> res = new Showapi_Res_Single<EmailResult>();
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 自动邮箱登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("EmailLogin")]
        public Showapi_Res_Single<EmailResult> EmailLogin([FromBody]EmailLoginBean para)
        {
            try
            {
                return _IEmail.EmailLogin(para);
            }
            catch (Exception ex)
            {
                Showapi_Res_Single<EmailResult> res = new Showapi_Res_Single<EmailResult>();
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 获取邮箱列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetEmailList")]
        public Showapi_Res_List<ReceiveMailBodyEntity> GetEmailList([FromBody]EmailLoginBean emailLoginBean)
        {
            Showapi_Res_List<ReceiveMailBodyEntity> res = new Showapi_Res_List<ReceiveMailBodyEntity>();
            try
            {
                return _IEmail.GetEmailList(emailLoginBean);
            }
            catch (Exception ex)
            {
                em.XieLogs(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取IM邮箱列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetIMOfficeEmailList")]
        public Showapi_Res_List<NewIMReceiveMailBodyEntity> GetIMOfficeEmailList([FromBody]EmailLoginBean emailLoginBean)
        {
            Showapi_Res_List<NewIMReceiveMailBodyEntity> res = new Showapi_Res_List<NewIMReceiveMailBodyEntity>();
            try
            {
                return _IEmail.GetIMOfficeEmailList(emailLoginBean);
            }
            catch (Exception ex)
            {
                em.XieLogs(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取IM发件箱邮箱列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetIMOfficeOutEmailList")]
        public Showapi_Res_List<NewIMReceiveMailBodyEntity> GetIMOfficeOutEmailList([FromBody]EmailLoginBean emailLoginBean)
        {
            Showapi_Res_List<NewIMReceiveMailBodyEntity> res = new Showapi_Res_List<NewIMReceiveMailBodyEntity>();
            try
            {
                return _IEmail.GetIMOfficeOutEmailList(emailLoginBean);
            }
            catch (Exception ex)
            {
                em.XieLogs(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 获取IM邮箱明细
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetIMOfficeEmailInfo")]
        public Showapi_Res_Single<NewIMReceiveMailBodyEntity> GetIMOfficeEmailInfo([FromBody]ImEmailById emailById)
        {
            Showapi_Res_Single<NewIMReceiveMailBodyEntity> res = new Showapi_Res_Single<NewIMReceiveMailBodyEntity>();
            try
            {
                return _IEmail.GetIMOfficeEmailInfo(emailById);
            }
            catch (Exception ex)
            {
                em.XieLogs(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <returns></returns>
        [HttpPost("SendEmail")]
        public Showapi_Res_Single<SendResultEntity> SendEmail()
        {
            try
            {
                MailBodyEntity mailBodyEntity = new MailBodyEntity();

                mailBodyEntity.email = Request.Form["email"].ToString(); // 发件人
                if (!string.IsNullOrEmpty(Request.Form["oldid"]) && !"undefined".Equals(Request.Form["reply"].ToString()))
                {
                    mailBodyEntity.oldid = int.Parse(Request.Form["oldid"].ToString()); // 
                }
                mailBodyEntity.folderType = Request.Form["folderType"].ToString(); // 
                if (!string.IsNullOrEmpty(Request.Form["reply"])&& !"undefined".Equals(Request.Form["reply"].ToString()))
                {
                    mailBodyEntity.reply = Boolean.Parse(Request.Form["reply"].ToString()); // 
                }
                mailBodyEntity.password = Request.Form["password"].ToString(); // 密码
                mailBodyEntity.Subject = Request.Form["subject"].ToString(); // 标题
                string content = Request.Form["body"].ToString();
                if (Request.IsHttps)
                {
                    mailBodyEntity.host = "https://" + Request.Host.Value;
                }
                else
                {
                    mailBodyEntity.host = "http://" + Request.Host.Value;
                }
                if (!string.IsNullOrEmpty(content))
                {
                    mailBodyEntity.Body = content; // 内容
                }
                else
                {
                    mailBodyEntity.Body = ""; // 内容
                }
                
                mailBodyEntity.Recipients = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(Request.Form["target"].ToString()); // 收件人

                if(mailBodyEntity.Recipients == null || mailBodyEntity.Recipients.Count == 0)
                {
                    throw new Exception("请选择收件人.");
                }

                string fileList = Request.Form["fileList"].ToString();
                if (!string.IsNullOrEmpty(fileList))
                {
                    mailBodyEntity.fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileList>>(fileList);
                }
                string oldFileList = Request.Form["oldFileList"].ToString();
                if (!string.IsNullOrEmpty(oldFileList))
                {
                    mailBodyEntity.oldFileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileList>>(oldFileList);
                }
                string copy = Request.Form["copy"].ToString();
                if (!string.IsNullOrEmpty(copy))
                {
                    mailBodyEntity.Cc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(copy); // 抄送人
                }
                return _IEmail.SendEmail(mailBodyEntity);
            }
            catch (Exception ex)
            {
                Showapi_Res_Single<SendResultEntity> res = new Showapi_Res_Single<SendResultEntity>();
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }


        /// <summary>
        /// 保存草稿
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveDraft")]
        public Showapi_Res_Single<SaveResultEntity> SaveDraft()
        {
            try
            {
                MailBodyEntity mailBodyEntity = new MailBodyEntity();

                mailBodyEntity.email = Request.Form["email"].ToString(); // 发件人
                if (!string.IsNullOrEmpty(Request.Form["oldid"]) && !"undefined".Equals(Request.Form["reply"].ToString()))
                {
                    mailBodyEntity.oldid = int.Parse(Request.Form["oldid"].ToString()); // 
                }
                mailBodyEntity.folderType = Request.Form["folderType"].ToString(); // 
                if (!string.IsNullOrEmpty(Request.Form["reply"]) && !"undefined".Equals(Request.Form["reply"].ToString()))
                {
                    mailBodyEntity.reply = Boolean.Parse(Request.Form["reply"].ToString()); // 
                }
                mailBodyEntity.password = Request.Form["password"].ToString(); // 密码
                mailBodyEntity.Subject = Request.Form["subject"].ToString(); // 标题
                string content = Request.Form["body"].ToString();
                if (Request.IsHttps)
                {
                    mailBodyEntity.host = "https://" + Request.Host.Value;
                }
                else
                {
                    mailBodyEntity.host = "http://" + Request.Host.Value;
                }
                if (!string.IsNullOrEmpty(content))
                {
                    mailBodyEntity.Body = content; // 内容
                }
                else
                {
                    mailBodyEntity.Body = ""; // 内容
                }

                mailBodyEntity.Recipients = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(Request.Form["target"].ToString()); // 收件人

                string fileList = Request.Form["fileList"].ToString();
                if (!string.IsNullOrEmpty(fileList))
                {
                    mailBodyEntity.fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileList>>(fileList);
                }
                string oldFileList = Request.Form["oldFileList"].ToString();
                if (!string.IsNullOrEmpty(oldFileList))
                {
                    mailBodyEntity.oldFileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileList>>(oldFileList);
                }
                string copy = Request.Form["copy"].ToString();
                if (!string.IsNullOrEmpty(copy))
                {
                    mailBodyEntity.Cc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(copy); // 抄送人
                }
                if(!string.IsNullOrEmpty(Request.Form["draftId"]) && !"undefined".Equals(Request.Form["draftId"].ToString()))
                {
                    mailBodyEntity.draftId = int.Parse(Request.Form["draftId"].ToString());
                }
                return _IEmail.SaveDraft(mailBodyEntity);
            }
            catch (Exception ex)
            {
                Showapi_Res_Single<SaveResultEntity> res = new Showapi_Res_Single<SaveResultEntity>();
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }

        /// <summary>
        /// 删除邮件
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateEmail")]
        public Showapi_Res_Single<EmailResult> UpdateEmail([FromBody]EmailIds emailIds)
        {
            try
            {
                return _IEmail.UpdateEmail(emailIds);
            }
            catch (Exception ex)
            {
                Showapi_Res_Single<EmailResult> res = new Showapi_Res_Single<EmailResult>();
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }

        /// <summary>
        /// 邮件明细
        /// </summary>
        /// <returns></returns>
        [HttpPost("EmailDetail")]
        public Showapi_Res_Single<ReceiveMailBodyEntity> EmailDetail([FromBody]EmailById emailById)
        {
            try
            {
                if (Request.IsHttps)
                {
                    emailById.host = "https://" + Request.Host.Value;
                }
                else
                {
                    emailById.host = "http://" + Request.Host.Value;
                }
                return _IEmail.EmailDetail(emailById);
            }
            catch (Exception ex)
            {
                Showapi_Res_Single<ReceiveMailBodyEntity> res = new Showapi_Res_Single<ReceiveMailBodyEntity>();
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <returns></returns>
        [HttpPost("DownloadMailAtta")]
        public Showapi_Res_Single<AttaFile> DownloadBodyParts([FromBody]AttaById attaById)
        {
            try
            {
                if (Request.IsHttps)
                {
                    attaById.host = "https://" + Request.Host.Value;
                }
                else
                {
                    attaById.host = "http://" + Request.Host.Value;
                }
                return _IEmail.DownloadBodyParts(attaById);
            }
            catch (Exception ex)
            {
                Showapi_Res_Single<AttaFile> res = new Showapi_Res_Single<AttaFile>();
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 下载附件
        /// </summary>
        /// <returns></returns>
        [HttpPost("IMDownloadBodyParts")]
        public Showapi_Res_Single<AttaFile> IMDownloadBodyParts([FromBody]ImEmailById attaById)
        {
            try
            {
                if (Request.IsHttps)
                {
                    attaById.host = "https://" + Request.Host.Value;
                }
                else
                {
                    attaById.host = "http://" + Request.Host.Value;
                }
                return _IEmail.IMDownloadBodyParts(attaById);
            }
            catch (Exception ex)
            {
                Showapi_Res_Single<AttaFile> res = new Showapi_Res_Single<AttaFile>();
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        ///  获取未读邮件
        /// </summary>
        /// <returns></returns>
        [HttpPost("UnReadMail")]
        public Showapi_Res_Single<UnReadBean> UnReadMail([FromBody]UnReadPara unReadPara)
        {
            try
            {
                return _IEmail.UnReadMail(unReadPara);
            }
            catch (Exception ex)
            {
                Showapi_Res_Single<UnReadBean> res = new Showapi_Res_Single<UnReadBean>();
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
    }
}
