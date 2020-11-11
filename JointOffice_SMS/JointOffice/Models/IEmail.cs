using CommonTool.MailKit;
using MimeKit;

namespace JointOffice.Models
{
    public interface IEmail
    {
        /// <summary>
        /// 邮箱登录验证
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<EmailResult> EmailLogin(EmailLoginBean para);
        /// <summary>
        /// 邮箱登录验证
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<EmailResult> AutoLogin();
        /// <summary>
        /// 邮箱登录验证
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<EmailResult> UnLogin();
        /// <summary>
        /// 获取邮箱列表
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<ReceiveMailBodyEntity> GetEmailList(EmailLoginBean emailLoginBean);
        /// <summary>
        /// 获取邮箱列表IM
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<NewIMReceiveMailBodyEntity> GetIMOfficeEmailList(EmailLoginBean emailLoginBean);
        /// <summary>
        /// 获取发件箱邮箱列表IM
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<NewIMReceiveMailBodyEntity> GetIMOfficeOutEmailList(EmailLoginBean emailLoginBean);
        /// <summary>
        /// 获取邮箱明细
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<NewIMReceiveMailBodyEntity> GetIMOfficeEmailInfo(ImEmailById emailById);
        /// <summary>
        /// 发送邮箱
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<SendResultEntity> SendEmail(MailBodyEntity mailBodyEntity);
        /// <summary>
        /// 保存草稿
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<SaveResultEntity> SaveDraft(MailBodyEntity mailBodyEntity);
        /// <summary>
        /// 删除,已读，加星，已回复邮箱
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<EmailResult> UpdateEmail(EmailIds emailIds);
        /// <summary>
        /// 邮箱明细
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<ReceiveMailBodyEntity> EmailDetail(EmailById emailById);
        /// <summary>
        /// 下载附件
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<AttaFile> DownloadBodyParts(AttaById attaById);
        /// <summary>
        /// IM下载附件
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<AttaFile> IMDownloadBodyParts(ImEmailById attaById);
        /// <summary>
        /// 获取未读邮件
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<UnReadBean> UnReadMail(UnReadPara attaById);
    }
    public class xlx_token
    {
        public string token { get; set; }
    }
}
