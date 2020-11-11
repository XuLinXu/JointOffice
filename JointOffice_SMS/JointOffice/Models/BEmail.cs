using CommonTool.MailKit;
using Dapper;
using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public class BEmail : IEmail
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        int SmtpPort;
        string SmtpHost;
        int ImapPort;
        string ImapHost;
        string corpid;
        string corpsecret;
        string Imurl;
        string constr;
        private readonly IPrincipalBase _PrincipalBase;
        //private static readonly EndpointAddress Endpoint = new EndpointAddress(Configurations.CalculatorServiceEndPoint);
        private static readonly BasicHttpBinding Binding = new BasicHttpBinding
        {
            MaxReceivedMessageSize = 2147483647,
            MaxBufferSize = 2147483647
        };
        public BEmail(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            SmtpPort = this.config.Value.ConnectionStrings.SmtpPort;
            SmtpHost = this.config.Value.ConnectionStrings.SmtpHost;
            ImapPort = this.config.Value.ConnectionStrings.ImapPort;
            ImapHost = this.config.Value.ConnectionStrings.ImapHost;
            corpid = this.config.Value.ConnectionStrings.corpid;
            corpsecret = this.config.Value.ConnectionStrings.corpsecret;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            Imurl = this.config.Value.ConnectionStrings.Imurl;
        }


        public List<EmailInfo> getEmailInfo(string memberid)
        {
            List<EmailInfo> emailInfos = new List<EmailInfo>();
            var mail = _JointOfficeContext.Mail_Info.Where(t => t.Mid == memberid && t.State == 1).FirstOrDefault();
            if (mail != null)
            {
                EmailInfo emailInfo = new EmailInfo();
                emailInfo.email = mail.Mail;
                string ps = BusinessHelper.AESDecrypt(mail.Passwrod, "8ca6e1b3bbb54dbfa014223271975b07");
                emailInfo.password = ps;
                emailInfos.Add(emailInfo);

            }
            return emailInfos;
        }
        /// <summary>
        /// 判断是否绑定
        /// </summary>
        /// <param name="memberid"></param>
        /// <returns></returns>
        public string isLogin(string memberid)
        {
            var mail = _JointOfficeContext.Mail_Info.Where(t => t.Mid == memberid && t.State == 1).FirstOrDefault();
            if (mail != null)
            {
                mail.LoginTime = new DateTime();
                _JointOfficeContext.SaveChanges();
                return mail.Mail;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <param name="memberid"></param>
        /// <returns></returns>
        public bool relieveLogin(string memberid)
        {
            var mail = _JointOfficeContext.Mail_Info.Where(t => t.Mid == memberid && t.State == 1).FirstOrDefault();
            if (mail != null)
            {
                mail.State = 0;
                _JointOfficeContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否第一次登陆 保存 或 修改登陆状态
        /// </summary>
        public void updateEmailInfo(EmailLoginBean para, string memberid)
        {
            List<EmailInfo> emailInfos = new List<EmailInfo>();
            var mail = _JointOfficeContext.Mail_Info.Where(t => t.Mail == para.email && t.Mid == memberid).FirstOrDefault();
            if (mail != null)
            {
                mail.State = 1;
                string ps = BusinessHelper.AESEncrypt(para.password, "8ca6e1b3bbb54dbfa014223271975b07");
                mail.Passwrod = ps;
                mail.LoginTime = DateTime.Now;
            }
            else
            {
                // string a = Guid.NewGuid().ToString("N");
                Mail_Info mail_Info = new Mail_Info();
                mail_Info.Id = Guid.NewGuid().ToString("D");
                mail_Info.Mail = para.email;
                string ps = BusinessHelper.AESEncrypt(para.password, "8ca6e1b3bbb54dbfa014223271975b07");
                mail_Info.Passwrod = ps.ToString();
                mail_Info.LoginTime = DateTime.Now;
                mail_Info.State = 1;
                mail_Info.Mid = memberid;
                _JointOfficeContext.Mail_Info.Add(mail_Info);

            }
            _JointOfficeContext.SaveChanges();
        }

        /// <summary>
        /// 修改邮件
        /// </summary>
        /// <param name="para"></param>
        /// <param name="flag">标识代码 1=已读 2=已回复 8=删除 4=加星</param>
        /// <returns></returns>
        public Showapi_Res_Single<EmailResult> UpdateEmail(EmailIds para)
        {
            Showapi_Res_Single<EmailResult> res = new Showapi_Res_Single<EmailResult>();
            try
            {
                var memberid = _PrincipalBase.GetMemberId();
                if (memberid == null || memberid == "")
                {
                    var ReturnSingle = new ReturnSingle<EmailResult>();
                    return ReturnSingle.Return();
                }

                #region 获取用户和密码
                string account = "";
                string passWord = "";
                List<EmailInfo> emailInfos = this.getEmailInfo(memberid);
                if (emailInfos.Count <= 0)
                {
                    throw new Exception("邮箱未登录");
                }
                else
                {
                    account = emailInfos[0].email;
                    passWord = emailInfos[0].password;
                }
                #endregion

                string folderType = para.type;
                int flag = para.flag;

                //准备工作结束
                ImapClient client = new ImapClient();

                #region 连接到邮件服务器
                try
                {
                    //一、创建获取邮件客户端并连接到邮件服务器。
                    //带端口号和协议的连接方式
                    client.Connect(ImapHost, ImapPort, true);
                }
                catch (ImapCommandException ex)
                {
                    throw new Exception("尝试连接时出错");
                }
                catch (ImapProtocolException ex)
                {
                    throw new Exception("尝试连接时的协议错误");
                }
                catch (Exception ex)
                {
                    throw new Exception("服务器连接错误:{0}");
                }

                try
                {
                    // 二、验证登录信息，输入账号和密码登录。
                    client.Authenticate(account, passWord);
                }
                catch (AuthenticationException ex)
                {
                    throw new Exception("无效的用户名或密码");
                }
                catch (ImapCommandException ex)
                {
                    throw new Exception("尝试验证错误");
                }
                catch (ImapProtocolException ex)
                {
                    throw new Exception("尝试验证时的协议错误");
                }
                catch (Exception ex)
                {
                    throw new Exception("账户认证错误");
                }

                List<UniqueId> uniqueids = para.idArr.Select(o => new UniqueId(o)).ToList();
                MessageFlags messageFlags = (MessageFlags)flag;
                if (folderType == null)
                    folderType = client.Inbox.Name;
                IMailFolder folder = client.GetFolder(folderType);
                folder.Open(FolderAccess.ReadWrite);

                if (flag == 4)
                {
                    if (para.value)
                    {
                        folder.SetFlags(uniqueids, messageFlags, true);
                    }
                    else
                    {
                        folder.RemoveFlags(uniqueids, messageFlags, true);
                    }
                }
                else
                {
                    folder.SetFlags(uniqueids, messageFlags, true);
                }
                if (uniqueids.Count > 0 && flag == 8)
                {
                    //主要针对Exchange 让删除指令执行
                    folder.Expunge(uniqueids);
                }
                //
                folder.Close();
                client.Disconnect(true);


                #endregion
                EmailResult emailResult = new EmailResult();
                //1=已读 2=已回复 8=删除 4=加星
                if (flag == 8)
                {
                    emailResult.message = "删除成功";
                }
                else if (flag == 4)
                {
                    emailResult.message = "加星成功";
                }

                emailResult.status = true;
                res.showapi_res_body = emailResult;
                res.showapi_res_code = "200";
                return res;
            }
            catch (Exception ex)
            {
                res.showapi_res_error = ex.Message;
                res.showapi_res_code = "508";
                return res;
            }
        }

        /// <summary>
        /// 邮箱登录
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_Single<EmailResult> EmailLogin(EmailLoginBean para)
        {
            Showapi_Res_Single<EmailResult> res = new Showapi_Res_Single<EmailResult>();
            EmailResult emailResult = new EmailResult();
            try
            {
                var memberid = _PrincipalBase.GetMemberId();
                if (memberid == null || memberid == "")
                {
                    var ReturnSingle = new ReturnSingle<EmailResult>();
                    return ReturnSingle.Return();
                }


                string account = para.email;
                string passWord = para.password;//获得的授权码

                using (var client = new ImapClient())
                {

                    #region 连接到邮件服务器
                    try
                    {
                        //一、创建获取邮件客户端并连接到邮件服务器。
                        //带端口号和协议的连接方式
                        client.Connect(ImapHost, ImapPort, true);
                    }
                    catch (ImapCommandException ex)
                    {
                        throw new Exception("尝试连接时出错");
                    }
                    catch (ImapProtocolException ex)
                    {
                        throw new Exception("尝试连接时的协议错误");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("服务器连接错误:{0}");
                    }

                    try
                    {
                        // 二、验证登录信息，输入账号和密码登录。
                        client.Authenticate(account, passWord);
                    }
                    catch (AuthenticationException ex)
                    {
                        throw new Exception("无效的用户名或密码");
                    }
                    catch (ImapCommandException ex)
                    {
                        throw new Exception("尝试验证错误");
                    }
                    catch (ImapProtocolException ex)
                    {
                        throw new Exception("尝试验证时的协议错误");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("账户认证错误");
                    }
                    #endregion

                    #region 获取未读数量
                    var folder = client.GetFolder(client.Inbox.Name);
                    folder.Open(FolderAccess.ReadOnly);
                    folder.Status(StatusItems.Count | StatusItems.Unread);
                    int total = folder.Count;
                    int unread = folder.Unread;
                    #endregion

                    folder.Close();
                    client.Disconnect(true);

                    #region 是否第一次登陆 保存 或 修改登陆状态

                    updateEmailInfo(para, memberid);

                    #endregion

                    emailResult.email = para.email;
                    emailResult.unread = unread;
                    emailResult.message = "绑定成功";
                    emailResult.status = true;
                    res.showapi_res_body = emailResult;
                    res.showapi_res_code = "200";
                    return res;
                }
            }
            catch (Exception e)
            {
                res.showapi_res_error = e.Message;
                res.showapi_res_body = emailResult;
                res.showapi_res_code = "508";
                return res;
            }
        }

        /// <summary>
        /// 拉取邮件列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<ReceiveMailBodyEntity> GetEmailList(EmailLoginBean emailLoginBean)
        {
            Showapi_Res_List<ReceiveMailBodyEntity> res = new Showapi_Res_List<ReceiveMailBodyEntity>();
            try
            {
                var memberid = _PrincipalBase.GetMemberId();
                if (memberid == null || memberid == "")
                {
                    var Return = new ReturnList<ReceiveMailBodyEntity>();
                    return Return.Return();
                }

                #region 获取用户和密码
                string account = "";
                string passWord = "";
                List<EmailInfo> emailInfos = this.getEmailInfo(memberid);
                if (emailInfos.Count <= 0)
                {
                    throw new Exception("邮箱未登录");
                }
                else
                {
                    account = emailInfos[0].email;
                    passWord = emailInfos[0].password;
                }
                #endregion



                var receiveServerConfigurationEntity = new ReceiveServerConfigurationEntity()
                {
                    SenderPassword = passWord,
                    ImapPort = ImapPort,
                    SenderAccount = account,
                    ImapHost = ImapHost,
                };

                Showapi_res_body_list<ReceiveMailBodyEntity> mimeMessages = ReceiveEmailHelper.ReceiveEmail(receiveServerConfigurationEntity, emailLoginBean);
                res.showapi_res_body = mimeMessages;
                res.showapi_res_code = "200";
                return res;
            }
            catch (Exception ex)
            {
                res.showapi_res_error = ex.Message;
                res.showapi_res_code = "508";
                return res;
            }
        }
        /// <summary>
        /// 拉取IM邮件列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<NewIMReceiveMailBodyEntity> GetIMOfficeEmailList(EmailLoginBean emailLoginBean)
        {
            Showapi_Res_List<NewIMReceiveMailBodyEntity> res = new Showapi_Res_List<NewIMReceiveMailBodyEntity>();
            try
            {
                var result = new List<NewIMReceiveMailBodyEntity>();
                var memberid = _PrincipalBase.GetMemberId();
                var allnum = 0;
                //memberid = "2de62cc3-cc32-48b8-9511-cee872f79534";
                if (memberid == null || memberid == "")
                {
                    var Return = new ReturnList<NewIMReceiveMailBodyEntity>();
                    return Return.Return();
                }
                var member_info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                if (member_info == null)
                {
                    throw new BusinessTureException("未找到该人员");
                }
                else
                {
                    if (!string.IsNullOrEmpty(member_info.MemberCode))
                    {
                        var date = DateTime.Now.ToString("yyyy-MM-dd");
                        var sql3 = "select token from xlx_token where date='" + date + "'";
                        var xlx_token = new xlx_token();
                        using (SqlConnection conText = new SqlConnection(constr))
                        {
                            xlx_token = conText.Query<xlx_token>(sql3).FirstOrDefault();
                        }
                        GetMailListRequest GetMailListRequest = new GetMailListRequest();
                        GetMailListRequest.Body = new GetMailListRequestBody();
                        GetMailListRequest.Body.para = new tempuri.org.GetMailPara();
                        GetMailListRequest.Body.para.token = xlx_token.token;
                        GetMailListRequest.Body.para.name = member_info.MemberCode;
                        GetMailListRequest.Body.para.page = emailLoginBean.page - 1;
                        GetMailListRequest.Body.para.congt = emailLoginBean.size;
                        GetMailListRequest.Body.para.str = emailLoginBean.title;
                        //SendEmailSoapClient client = new SendEmailSoapClient();
                        //var list = client.GetMailList(GetMailPara);
                        //foreach (var item in list)
                        //{
                        //    NewIMReceiveMailBodyEntity NewIMReceiveMailBodyEntity = new NewIMReceiveMailBodyEntity();
                        //    NewIMReceiveMailBodyEntity.Body = item.Body;
                        //    NewIMReceiveMailBodyEntity.Date = item.Date;
                        //    NewIMReceiveMailBodyEntity.Fromstr = item.Fromstr;
                        //    NewIMReceiveMailBodyEntity.Tostr = item.Tostr;
                        //    NewIMReceiveMailBodyEntity.Subject = item.Subject;
                        //    result.Add(NewIMReceiveMailBodyEntity);
                        //}

                        EndpointAddress Endpoint = new EndpointAddress(Imurl);
                        using (var proxy = new GenericProxy<SendEmailSoap>(Binding, Endpoint))
                        {
                            try
                            {

                                var info = proxy.Execute(c => c.GetMailList(GetMailListRequest));
                                allnum = info.Body.GetMailListResult.allNum;
                                foreach (var item in info.Body.GetMailListResult.contentlist)
                                {
                                    NewIMReceiveMailBodyEntity NewIMReceiveMailBodyEntity = new NewIMReceiveMailBodyEntity();
                                    NewIMReceiveMailBodyEntity.id = item.id;
                                    NewIMReceiveMailBodyEntity.Body = item.Body;
                                    NewIMReceiveMailBodyEntity.Date = item.Date;
                                    NewIMReceiveMailBodyEntity.Fromstr = item.Fromstr;
                                    NewIMReceiveMailBodyEntity.Tostr = item.Tostr;
                                    NewIMReceiveMailBodyEntity.Subject = item.Subject;
                                    result.Add(NewIMReceiveMailBodyEntity);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new BusinessTureException(ex.Message);
                            }
                        }
                    }

                }
                res.showapi_res_body = new Showapi_res_body_list<NewIMReceiveMailBodyEntity>();
                res.showapi_res_body.contentlist = result;
                res.showapi_res_body.allNum = allnum;
                res.showapi_res_code = "200";
                return res;
            }
            catch (Exception ex)
            {
                res.showapi_res_error = ex.Message;
                res.showapi_res_code = "508";
                return res;
            }
        }
        /// <summary>
        /// 拉取IM发件箱邮件列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<NewIMReceiveMailBodyEntity> GetIMOfficeOutEmailList(EmailLoginBean emailLoginBean)
        {
            Showapi_Res_List<NewIMReceiveMailBodyEntity> res = new Showapi_Res_List<NewIMReceiveMailBodyEntity>();
            try
            {
                var result = new List<NewIMReceiveMailBodyEntity>();
                var memberid = _PrincipalBase.GetMemberId();
                var allnum = 0;
                //memberid = "2de62cc3-cc32-48b8-9511-cee872f79534";
                if (memberid == null || memberid == "")
                {
                    var Return = new ReturnList<NewIMReceiveMailBodyEntity>();
                    return Return.Return();
                }
                var member_info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                if (member_info == null)
                {
                    throw new BusinessTureException("未找到该人员");
                }
                else
                {
                    if (!string.IsNullOrEmpty(member_info.MemberCode))
                    {
                        var date = DateTime.Now.ToString("yyyy-MM-dd");
                        var sql3 = "select token from xlx_token where date='" + date + "'";
                        var xlx_token = new xlx_token();
                        using (SqlConnection conText = new SqlConnection(constr))
                        {
                            xlx_token = conText.Query<xlx_token>(sql3).FirstOrDefault();
                        }
                        GetOutMailListRequest GetMailListRequest = new GetOutMailListRequest();
                        GetMailListRequest.Body = new GetOutMailListRequestBody();
                        GetMailListRequest.Body.para = new tempuri.org.GetMailPara();
                        GetMailListRequest.Body.para.token = xlx_token.token;
                        GetMailListRequest.Body.para.name = member_info.MemberCode;
                        GetMailListRequest.Body.para.page = emailLoginBean.page - 1;
                        GetMailListRequest.Body.para.congt = emailLoginBean.size;
                        GetMailListRequest.Body.para.str = emailLoginBean.title;
                        //SendEmailSoapClient client = new SendEmailSoapClient();
                        //var list = client.GetMailList(GetMailPara);
                        //foreach (var item in list)
                        //{
                        //    NewIMReceiveMailBodyEntity NewIMReceiveMailBodyEntity = new NewIMReceiveMailBodyEntity();
                        //    NewIMReceiveMailBodyEntity.Body = item.Body;
                        //    NewIMReceiveMailBodyEntity.Date = item.Date;
                        //    NewIMReceiveMailBodyEntity.Fromstr = item.Fromstr;
                        //    NewIMReceiveMailBodyEntity.Tostr = item.Tostr;
                        //    NewIMReceiveMailBodyEntity.Subject = item.Subject;
                        //    result.Add(NewIMReceiveMailBodyEntity);
                        //}

                        EndpointAddress Endpoint = new EndpointAddress(Imurl);
                        using (var proxy = new GenericProxy<SendEmailSoap>(Binding, Endpoint))
                        {
                            try
                            {

                                var info = proxy.Execute(c => c.GetOutMailList(GetMailListRequest));
                                allnum = info.Body.GetOutMailListResult.allNum;
                                foreach (var item in info.Body.GetOutMailListResult.contentlist)
                                {
                                    NewIMReceiveMailBodyEntity NewIMReceiveMailBodyEntity = new NewIMReceiveMailBodyEntity();
                                    NewIMReceiveMailBodyEntity.id = item.id;
                                    NewIMReceiveMailBodyEntity.Body = item.Body;
                                    NewIMReceiveMailBodyEntity.Date = item.Date;
                                    NewIMReceiveMailBodyEntity.Fromstr = item.Fromstr;
                                    NewIMReceiveMailBodyEntity.Tostr = item.Tostr;
                                    NewIMReceiveMailBodyEntity.Subject = item.Subject;
                                    result.Add(NewIMReceiveMailBodyEntity);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new BusinessTureException(ex.Message);
                            }
                        }
                    }

                }
                res.showapi_res_body = new Showapi_res_body_list<NewIMReceiveMailBodyEntity>();
                res.showapi_res_body.contentlist = result;
                res.showapi_res_body.allNum = allnum;
                res.showapi_res_code = "200";
                return res;
            }
            catch (Exception ex)
            {
                res.showapi_res_error = ex.Message;
                res.showapi_res_code = "508";
                return res;
            }
        }
        /// <summary>
        /// 拉取IM邮件列明细
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<NewIMReceiveMailBodyEntity> GetIMOfficeEmailInfo(ImEmailById emailById)
        {
            Showapi_Res_Single<NewIMReceiveMailBodyEntity> res = new Showapi_Res_Single<NewIMReceiveMailBodyEntity>();
            try
            {
                var result = new NewIMReceiveMailBodyEntity();
                var memberid = _PrincipalBase.GetMemberId();
                if (memberid == null || memberid == "")
                {
                    var ReturnSingle = new ReturnSingle<NewIMReceiveMailBodyEntity>();
                    return ReturnSingle.Return();
                }
                var member_info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                if (member_info == null)
                {
                    throw new BusinessTureException("未找到该人员");
                }
                else
                {
                    var date = DateTime.Now.ToString("yyyy-MM-dd");
                    var sql3 = "select token from xlx_token where date='" + date + "'";
                    var xlx_token = new xlx_token();
                    using (SqlConnection conText = new SqlConnection(constr))
                    {
                        xlx_token = conText.Query<xlx_token>(sql3).FirstOrDefault();
                    }
                    GetMailInfoRequest GetMailInfoPara = new GetMailInfoRequest();
                    GetMailInfoPara.Body = new GetMailInfoRequestBody();
                    GetMailInfoPara.Body.para = new tempuri.org.GetMailInfoPara();
                    GetMailInfoPara.Body.para.token = xlx_token.token;
                    GetMailInfoPara.Body.para.id = emailById.id;
                    EndpointAddress Endpoint = new EndpointAddress(Imurl);

                    using (var proxy = new GenericProxy<SendEmailSoap>(Binding, Endpoint))
                    {
                        try
                        {
                            var info = proxy.Execute(c => c.GetMailInfo(GetMailInfoPara));
                            //result.rich = info.Body.GetMailInfoResult.rich;//二进制

                            //try
                            //{

                            //    var filePathString = "/AttachmentFile/OfficeIM";
                            //    string directory = Directory.GetCurrentDirectory() + filePathString;
                            //    string guid = Guid.NewGuid().ToString("D");
                            //    string name = directory + "\\" + 123 + ".rvf";

                            //    //将创建文件流对象的过程写在using当中，会自动的帮助我们释放流所占用的资源
                            //    using (FileStream fsWrite = new FileStream(name, FileMode.OpenOrCreate, FileAccess.Write))
                            //    {
                            //        byte[] buffer = result.rich;
                            //        fsWrite.Write(buffer, 0, buffer.Length);
                            //    }


                            //   FileStream fs = null;
                            //    BinaryWriter bw = null;
                            //    try
                            //    {
                            //        //二进制流转换成文件
                            //        byte[] content = result.rich;
                            //        fs = new FileStream(name, FileMode.CreateNew);
                            //        bw = new BinaryWriter(fs);
                            //        bw.Write(content, 0, content.Length);
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        //文件下载失败
                            //    }
                            //    finally
                            //    {
                            //        if (bw != null)
                            //        {
                            //            bw.Close();
                            //            bw = null;
                            //        }
                            //        if (fs != null)
                            //        {
                            //            fs.Close();
                            //            fs = null;
                            //        }
                            //    }

                            //    //if (File.Exists(path))
                            //    //{
                            //    //    System.IO.FileInfo file = new System.IO.FileInfo(path);
                            //    //    context.Response.Clear();
                            //    //    context.Response.ClearHeaders();
                            //    //    context.Response.Buffer = false;
                            //    //    context.Response.ContentType = "application/octet-stream";
                            //    //    context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(newsfile.Title + newsfile.Filetype, System.Text.Encoding.UTF8));
                            //    //    context.Response.AppendHeader("Content-Length", file.Length.ToString());
                            //    //    context.Response.WriteFile(file.FullName);
                            //    //    context.Response.Flush();
                            //    //    context.Response.End();
                            //    //}


                            //    //var filePathString = "/AttachmentFile/OfficeIM";
                            //    //// 确定一个目录来保存内容
                            //    //string directory = Directory.GetCurrentDirectory() + filePathString;

                            //    //bool exists = Directory.Exists(directory);

                            //    //if (!exists)
                            //    //{
                            //    //    System.IO.Directory.CreateDirectory(directory);
                            //    //}
                            //    //else
                            //    //{
                            //    //    //foreach (var file in Directory.GetFiles(directory))
                            //    //    //{
                            //    //    //    try
                            //    //    //    {
                            //    //    //        FileInfo fileInfo = new FileInfo(file);

                            //    //    //        fileInfo.Delete();
                            //    //    //    }
                            //    //    //    catch (Exception e)
                            //    //    //    {

                            //    //    //    }
                            //    //    //}
                            //    //}

                            //    ////存储
                            //    //string guid = Guid.NewGuid().ToString("D");
                            //    //string name = directory + "\\" + guid + ".rtf";
                            //    //FileStream fs;
                            //    //fs = new FileStream(name, FileMode.CreateNew);
                            //    //BinaryWriter br = new BinaryWriter(fs);
                            //    //br.Write(result.rich, 0, result.rich.Length);
                            //    //br.Close();
                            //    //fs.Close();


                            //    ////创建读取器
                            //    //StreamReader reader = new StreamReader(fs);
                            //    ////读取内容到变量
                            //    //string content = reader.ReadToEnd();
                            //    ////关闭读取器
                            //    //reader.Close();


                            //    ////删除
                            //    //FileInfo file = new FileInfo(name);//指定文件路径
                            //    //if (file.Exists)//判断文件是否存在
                            //    //{
                            //    //    file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                            //    //    file.Delete();//删除文件
                            //    //}


                            //}
                            //catch(Exception e)
                            //{
                            //    result.Body = info.Body.GetMailInfoResult.Body.Replace("\n", "<br>");
                            //}
                            if(info.Body.GetMailInfoResult.Body==null)
                            {
                                result.Body = "";
                            }
                            else
                            {
                                result.Body = info.Body.GetMailInfoResult.Body.Replace("\n", "<br>");
                            }
                            result.Tostr = info.Body.GetMailInfoResult.Tostr;
                            result.Fromstr = info.Body.GetMailInfoResult.Fromstr;
                            result.Subject = info.Body.GetMailInfoResult.Subject;
                            result.Date = info.Body.GetMailInfoResult.Date;
                            result.Cc = info.Body.GetMailInfoResult.Cc;
                            var Attachmentslist = new List<NewAttachmentBean>();
                            foreach (var item in info.Body.GetMailInfoResult.Attachments)
                            {
                                NewAttachmentBean Attachment = new NewAttachmentBean();
                                //Stream stream = new MemoryStream(item.AttaStream);
                                //Attachment.AttaStream = stream;
                                Attachment.Name = item.Name;
                                Attachment.index = item.index;
                                //Attachment.Size = stream.Length.ToString();
                                Attachment.path = item.path;
                                Attachmentslist.Add(Attachment);
                            }
                            result.Attachments = Attachmentslist;
                        }
                        catch (Exception ex)
                        {
                            throw new BusinessTureException(ex.Message);
                        }
                    }
                }

                res.showapi_res_body = result;
                res.showapi_res_code = "200";
                return res;
            }
            catch (Exception ex)
            {
                res.showapi_res_error = ex.Message;
                res.showapi_res_code = "508";
                return res;
            }
        }
        /// <summary> 
        /// 获取字符串中img的url集合 
        /// </summary> 
        /// <param name="content">字符串</param> 
        /// <returns></returns> 
        public static List<string> GetImgUrl(string content)
        {
            Regex rg = new Regex("data-mce-src=\"([^\"]+)\"", RegexOptions.IgnoreCase);
            var img = rg.Match(content);
            List<string> imgUrl = new List<string>();
            while (img.Success)
            {
                content = content.Replace("data-mce-src=\"" + img.Groups[1].Value + "\"", "");
                img = img.NextMatch();
            }

            rg = new Regex("src=\"([^\"]+)\"", RegexOptions.IgnoreCase);
            img = rg.Match(content);
            imgUrl = new List<string>();
            while (img.Success)
            {
                imgUrl.Add(img.Groups[1].Value);
                img = img.NextMatch();
            }

            return imgUrl;
        }

        /// <summary>
        /// 发送邮箱
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<SendResultEntity> SendEmail(MailBodyEntity para)
        {
            Showapi_Res_Single<SendResultEntity> res = new Showapi_Res_Single<SendResultEntity>();
            try
            {
                var memberid = _PrincipalBase.GetMemberId();
                if (memberid == null || memberid == "")
                {
                    var ReturnSingle = new ReturnSingle<SendResultEntity>();
                    return ReturnSingle.Return();
                }

                #region 获取用户和密码
                string account = "";
                string passWord = "";
                List<EmailInfo> emailInfos = this.getEmailInfo(memberid);
                if (emailInfos.Count <= 0)
                {
                    throw new Exception("邮箱未登录");
                }
                else
                {
                    account = emailInfos[0].email;
                    passWord = emailInfos[0].password;
                }
                #endregion

                var mailFiles = new List<MailFile>();
                if (para.oldid != -1)
                {
                    using (var client = new ImapClient())
                    {
                        #region 连接到邮件服务器
                        try
                        {
                            //一、创建获取邮件客户端并连接到邮件服务器。
                            //带端口号和协议的连接方式
                            client.Connect(ImapHost, ImapPort, true);
                        }
                        catch (ImapCommandException ex)
                        {
                            throw new Exception("尝试连接时出错");
                        }
                        catch (ImapProtocolException ex)
                        {
                            throw new Exception("尝试连接时的协议错误");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("服务器连接错误:{0}");
                        }

                        try
                        {
                            // 二、验证登录信息，输入账号和密码登录。
                            client.Authenticate(account, passWord);
                        }
                        catch (AuthenticationException ex)
                        {
                            throw new Exception("无效的用户名或密码");
                        }
                        catch (ImapCommandException ex)
                        {
                            throw new Exception("尝试验证错误");
                        }
                        catch (ImapProtocolException ex)
                        {
                            throw new Exception("尝试验证时的协议错误");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("账户认证错误");
                        }
                        #endregion

                        var folder = client.GetFolder(para.folderType);

                        folder.Open(FolderAccess.ReadWrite);

                        // 获取这些邮件的摘要信息
                        List<UniqueId> uids = new List<UniqueId>();
                        uids.Add(new UniqueId((uint)para.oldid));
                        var emaills = folder.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure | MessageSummaryItems.Full);
                        if (emaills.Count > 0)
                        {
                            var emhead = emaills[0];

                            if (para.oldFileList.Count > 0)
                            {
                                // 附件信息
                                if (emhead.Attachments.Count() > 0)
                                {
                                    var directory = "";
                                    var filePathString = "/AttachmentFile/MailDetail";

                                    // 确定一个目录来保存内容
                                    directory = Directory.GetCurrentDirectory() + filePathString;

                                    bool exists = Directory.Exists(directory);

                                    if (!exists)
                                    {
                                        System.IO.Directory.CreateDirectory(directory);
                                    }
                                    else
                                    {
                                        //foreach (var file in Directory.GetFiles(directory))
                                        //{
                                        //    try
                                        //    {
                                        //        FileInfo fileInfo = new FileInfo(file);

                                        //        fileInfo.Delete();
                                        //    }
                                        //    catch (Exception e)
                                        //    {

                                        //    }
                                        //}
                                    }

                                    foreach (var oldfile in para.oldFileList)
                                    {
                                        // 这里要转成mimepart类型
                                        int index = 0;
                                        foreach (var attachment in emhead.Attachments)
                                        {
                                            if (index == oldfile.index)
                                            {
                                                // 像我们对内容所做的那样下载附件
                                                var entity = folder.GetBodyPart(emhead.UniqueId, attachment);

                                                // 附件可以是message / rfc822部件或常规MIME部件
                                                var messagePart = entity as MessagePart;
                                                if (messagePart != null)
                                                {
                                                    var rfc822 = messagePart;

                                                    string path = Path.Combine(directory, attachment.PartSpecifier + ".eml");

                                                    rfc822.Message.WriteTo(path);
                                                }
                                                else
                                                {
                                                    var part = (MimePart)entity;


                                                    #region 扩展名
                                                    string extension = System.IO.Path.GetExtension(part.FileName);
                                                    //扩展名 + 新名称
                                                    string newFileName = Guid.NewGuid().ToString("D") + extension;

                                                    if (!string.IsNullOrEmpty(extension) && ".exe".Equals(extension))
                                                    {
                                                        throw new Exception("出于安全性考虑，不允许添加以下可执行文件文件。");
                                                    }
                                                    if (string.IsNullOrEmpty(extension))
                                                    {
                                                        extension = "jpeg";
                                                    }
                                                    else
                                                    {
                                                        extension = extension.Substring(1, extension.Length - 1);
                                                    }
                                                    #endregion

                                                    // 注意：这可能是空的，但大多数会指定一个文件名
                                                    var fileName = string.IsNullOrEmpty(part.FileName) ? newFileName : part.FileName;

                                                    string path = Path.Combine(directory, newFileName);

                                                    // decode and save the content to a file
                                                    using (var stream = System.IO.File.Create(path))
                                                        part.Content.DecodeTo(stream);

                                                    var atta = new AttachmentBean();


                                                    string filetype = "application";
                                                    if (extension == "bmp" || extension == "gif" || extension == "jpeg" || extension == "png" || extension == "jpg")
                                                    {
                                                        filetype = "image";
                                                    }

                                                    mailFiles.Add(new MailFile { MailFilePath = path, MailFileSubType = extension, MailFileType = filetype, filename = fileName });
                                                }
                                                break;
                                            }
                                            index++;
                                        }
                                    }

                                }
                            }


                            // 邮件状态,已读未读等等
                            if (emhead.Flags.HasValue)
                            {
                                if (!emhead.Flags.Value.HasFlag(MessageFlags.Seen))
                                {
                                    // 已读状态
                                    if ("INBOX".Equals(para.folderType))
                                    {
                                        MessageFlags messageFlags = (MessageFlags)1;
                                        folder.AddFlags(new UniqueId((uint)para.oldid), messageFlags, true);
                                    }
                                }
                            }
                            // 邮件状态,回复
                            if (emhead.Flags.HasValue)
                            {
                                if (!emhead.Flags.Value.HasFlag(MessageFlags.Answered))
                                {
                                    if (para.reply && "INBOX".Equals(para.folderType))
                                    {
                                        MessageFlags messageFlags = (MessageFlags)2;
                                        folder.AddFlags(new UniqueId((uint)para.oldid), messageFlags, true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("未找到要处理要邮件。");
                        }
                        folder.Close();
                        client.Disconnect(true);
                    }
                }

                // 新添加
                if (para.fileList != null && para.fileList.Count > 0)
                {
                    foreach (var file in para.fileList)
                    {
                        #region 扩展名
                        string extension = System.IO.Path.GetExtension(file.name);

                        if (!string.IsNullOrEmpty(extension) && ".exe".Equals(extension))
                        {
                            throw new Exception("出于安全性考虑，不允许添加以下可执行文件文件。");
                        }
                        if (string.IsNullOrEmpty(extension))
                        {
                            extension = "jpeg";
                        }
                        else
                        {
                            extension = extension.Substring(1, extension.Length - 1);
                        }

                        #endregion
                        string filetype = "application";
                        if (extension == "bmp" || extension == "gif" || extension == "jpeg" || extension == "png" || extension == "jpg")
                        {
                            filetype = "image";
                        }

                        mailFiles.Add(new MailFile { MailFilePath = Directory.GetCurrentDirectory() + file.path.Replace(para.host, ""), MailFileSubType = extension, MailFileType = filetype, filename = file.name });
                    }
                }

                // 文本中图片
                var htmlFiles = new List<MailFile>();
                List<string> contentUrl = GetImgUrl(para.Body);
                if (contentUrl.Count > 0)
                {
                    foreach (var file in contentUrl)
                    {
                        if (file.IndexOf("AttachmentFile\\Upload") == -1)
                        {
                            continue;
                        }
                        #region 扩展名
                        MailFile mailFile = new MailFile();
                        string extension = System.IO.Path.GetExtension(file);
                        if (!string.IsNullOrEmpty(extension) && ".exe".Equals(extension))
                        {
                            throw new Exception("出于安全性考虑，不允许添加以下可执行文件文件。");
                        }
                        if (string.IsNullOrEmpty(extension))
                        {
                            extension = "file";
                        }
                        else
                        {
                            extension = extension.Substring(1, extension.Length - 1);
                        }
                        #endregion
                        string filetype = "application";
                        if (extension == "bmp" || extension == "gif" || extension == "jpeg" || extension == "png" || extension == "jpg")
                        {
                            filetype = "image";
                        }


                        string guid = Guid.NewGuid().ToString("D");

                        htmlFiles.Add(new MailFile { MailFilePath = Directory.GetCurrentDirectory() + file.Replace(para.host, ""), MailFileSubType = extension, MailFileType = filetype, cid = guid });
                        para.Body = para.Body.Replace(file, "cid:" + guid);

                    }

                }

                var mailBodyEntity = new MailBodyEntity()
                {
                    Body = para.Body,
                    Cc = para.Cc,
                    HtmlFiles = htmlFiles,
                    //MailBodyType = "html",
                    MailFiles = mailFiles,
                    Recipients = para.Recipients,
                    // Sender = "hxx",
                    SenderAddress = account,
                    Subject = para.Subject,
                };

                var sendServerConfiguration = new SendServerConfigurationEntity()
                {
                    SenderPassword = passWord,
                    SmtpPort = SmtpPort,
                    IsSsl = true,
                    MailEncoding = "utf-8",
                    SenderAccount = account,
                    SmtpHost = SmtpHost,

                };

                var result = SeedMailHelper.SendMail(mailBodyEntity, sendServerConfiguration);

                res.showapi_res_body = result;
                res.showapi_res_code = "200";
                return res;
            }
            catch (Exception e)
            {
                res.showapi_res_error = e.Message;
                res.showapi_res_code = "508";
                return res;
            }

        }

        /// <summary>
        /// 保存草稿
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<SaveResultEntity> SaveDraft(MailBodyEntity para)
        {
            Showapi_Res_Single<SaveResultEntity> res = new Showapi_Res_Single<SaveResultEntity>();
            try
            {
                var memberid = _PrincipalBase.GetMemberId();
                if (memberid == null || memberid == "")
                {
                    var ReturnSingle = new ReturnSingle<SaveResultEntity>();
                    return ReturnSingle.Return();
                }

                #region 获取用户和密码
                string account = "";
                string passWord = "";
                List<EmailInfo> emailInfos = this.getEmailInfo(memberid);
                if (emailInfos.Count <= 0)
                {
                    throw new Exception("邮箱未登录");
                }
                else
                {
                    account = emailInfos[0].email;
                    passWord = emailInfos[0].password;
                }
                #endregion

                var mailFiles = new List<MailFile>();
                if (para.oldid != -1)
                {
                    using (var client = new ImapClient())
                    {
                        #region 连接到邮件服务器
                        try
                        {
                            //一、创建获取邮件客户端并连接到邮件服务器。
                            //带端口号和协议的连接方式
                            client.Connect(ImapHost, ImapPort, true);
                        }
                        catch (ImapCommandException ex)
                        {
                            throw new Exception("尝试连接时出错");
                        }
                        catch (ImapProtocolException ex)
                        {
                            throw new Exception("尝试连接时的协议错误");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("服务器连接错误:{0}");
                        }

                        try
                        {
                            // 二、验证登录信息，输入账号和密码登录。
                            client.Authenticate(account, passWord);
                        }
                        catch (AuthenticationException ex)
                        {
                            throw new Exception("无效的用户名或密码");
                        }
                        catch (ImapCommandException ex)
                        {
                            throw new Exception("尝试验证错误");
                        }
                        catch (ImapProtocolException ex)
                        {
                            throw new Exception("尝试验证时的协议错误");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("账户认证错误");
                        }
                        #endregion

                        var folder = client.GetFolder(para.folderType);

                        folder.Open(FolderAccess.ReadWrite);

                        // 获取这些邮件的摘要信息
                        List<UniqueId> uids = new List<UniqueId>();
                        uids.Add(new UniqueId((uint)para.oldid));
                        var emaills = folder.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure | MessageSummaryItems.Full);
                        if (emaills.Count > 0)
                        {
                            var emhead = emaills[0];

                            if (para.oldFileList.Count > 0)
                            {
                                // 附件信息
                                if (emhead.Attachments.Count() > 0)
                                {
                                    var directory = "";
                                    var filePathString = "/AttachmentFile/MailDetail";

                                    // 确定一个目录来保存内容
                                    directory = Directory.GetCurrentDirectory() + filePathString;

                                    bool exists = Directory.Exists(directory);

                                    if (!exists)
                                    {
                                        System.IO.Directory.CreateDirectory(directory);
                                    }
                                    else
                                    {
                                        //foreach (var file in Directory.GetFiles(directory))
                                        //{
                                        //    try
                                        //    {
                                        //        FileInfo fileInfo = new FileInfo(file);

                                        //        fileInfo.Delete();
                                        //    }
                                        //    catch (Exception e)
                                        //    {

                                        //    }
                                        //}
                                    }

                                    foreach (var oldfile in para.oldFileList)
                                    {
                                        // 这里要转成mimepart类型
                                        int index = 0;
                                        foreach (var attachment in emhead.Attachments)
                                        {
                                            if (index == oldfile.index)
                                            {
                                                // 像我们对内容所做的那样下载附件
                                                var entity = folder.GetBodyPart(emhead.UniqueId, attachment);

                                                // 附件可以是message / rfc822部件或常规MIME部件
                                                var messagePart = entity as MessagePart;
                                                if (messagePart != null)
                                                {
                                                    var rfc822 = messagePart;

                                                    string path = Path.Combine(directory, attachment.PartSpecifier + ".eml");

                                                    rfc822.Message.WriteTo(path);
                                                }
                                                else
                                                {
                                                    var part = (MimePart)entity;


                                                    #region 扩展名
                                                    string extension = System.IO.Path.GetExtension(part.FileName);
                                                    //扩展名 + 新名称
                                                    string newFileName = Guid.NewGuid().ToString("D") + extension;

                                                    if (!string.IsNullOrEmpty(extension) && ".exe".Equals(extension))
                                                    {
                                                        throw new Exception("出于安全性考虑，不允许添加以下可执行文件文件。");
                                                    }
                                                    if (string.IsNullOrEmpty(extension))
                                                    {
                                                        extension = "jpeg";
                                                    }
                                                    else
                                                    {
                                                        extension = extension.Substring(1, extension.Length - 1);
                                                    }
                                                    #endregion

                                                    // 注意：这可能是空的，但大多数会指定一个文件名
                                                    var fileName = string.IsNullOrEmpty(part.FileName) ? newFileName : part.FileName;

                                                    string path = Path.Combine(directory, newFileName);

                                                    // decode and save the content to a file
                                                    using (var stream = System.IO.File.Create(path))
                                                        part.Content.DecodeTo(stream);

                                                    var atta = new AttachmentBean();


                                                    string filetype = "application";
                                                    if (extension == "bmp" || extension == "gif" || extension == "jpeg" || extension == "png" || extension == "jpg")
                                                    {
                                                        filetype = "image";
                                                    }

                                                    mailFiles.Add(new MailFile { MailFilePath = path, MailFileSubType = extension, MailFileType = filetype, filename = fileName });
                                                }
                                                break;
                                            }
                                            index++;
                                        }
                                    }

                                }
                            }


                            // 邮件状态,已读未读等等
                            if (emhead.Flags.HasValue)
                            {
                                if (!emhead.Flags.Value.HasFlag(MessageFlags.Seen))
                                {
                                    // 已读状态
                                    if ("INBOX".Equals(para.folderType))
                                    {
                                        MessageFlags messageFlags = (MessageFlags)1;
                                        folder.AddFlags(new UniqueId((uint)para.oldid), messageFlags, true);
                                    }
                                }
                            }
                            // 邮件状态,回复
                            if (emhead.Flags.HasValue)
                            {
                                if (!emhead.Flags.Value.HasFlag(MessageFlags.Answered))
                                {
                                    if (para.reply && "INBOX".Equals(para.folderType))
                                    {
                                        MessageFlags messageFlags = (MessageFlags)2;
                                        folder.AddFlags(new UniqueId((uint)para.oldid), messageFlags, true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("未找到要处理要邮件。");
                        }
                        folder.Close();

                        client.Disconnect(true);
                    }
                }

                // 新添加
                if (para.fileList != null && para.fileList.Count > 0)
                {
                    foreach (var file in para.fileList)
                    {
                        #region 扩展名
                        string extension = System.IO.Path.GetExtension(file.name);

                        if (!string.IsNullOrEmpty(extension) && ".exe".Equals(extension))
                        {
                            throw new Exception("出于安全性考虑，不允许添加以下可执行文件文件。");
                        }
                        if (string.IsNullOrEmpty(extension))
                        {
                            extension = "jpeg";
                        }
                        else
                        {
                            extension = extension.Substring(1, extension.Length - 1);
                        }

                        #endregion
                        string filetype = "application";
                        if (extension == "bmp" || extension == "gif" || extension == "jpeg" || extension == "png" || extension == "jpg")
                        {
                            filetype = "image";
                        }

                        mailFiles.Add(new MailFile { MailFilePath = Directory.GetCurrentDirectory() + file.path.Replace(para.host, ""), MailFileSubType = extension, MailFileType = filetype, filename = file.name });
                    }
                }

                // 文本中图片
                var htmlFiles = new List<MailFile>();
                List<string> contentUrl = GetImgUrl(para.Body);
                if (contentUrl.Count > 0)
                {
                    foreach (var file in contentUrl)
                    {
                        if (file.IndexOf("AttachmentFile\\Upload") == -1)
                        {
                            continue;
                        }
                        #region 扩展名
                        MailFile mailFile = new MailFile();
                        string extension = System.IO.Path.GetExtension(file);
                        if (!string.IsNullOrEmpty(extension) && ".exe".Equals(extension))
                        {
                            throw new Exception("出于安全性考虑，不允许添加以下可执行文件文件。");
                        }
                        if (string.IsNullOrEmpty(extension))
                        {
                            extension = "file";
                        }
                        else
                        {
                            extension = extension.Substring(1, extension.Length - 1);
                        }
                        #endregion
                        string filetype = "application";
                        if (extension == "bmp" || extension == "gif" || extension == "jpeg" || extension == "png" || extension == "jpg")
                        {
                            filetype = "image";
                        }


                        string guid = Guid.NewGuid().ToString("D");

                        htmlFiles.Add(new MailFile { MailFilePath = Directory.GetCurrentDirectory() + file.Replace(para.host, ""), MailFileSubType = extension, MailFileType = filetype, cid = guid });
                        para.Body = para.Body.Replace(file, "cid:" + guid);

                    }

                }

                var mailBodyEntity = new MailBodyEntity()
                {
                    Body = para.Body,
                    Cc = para.Cc,
                    HtmlFiles = htmlFiles,
                    //MailBodyType = "html",
                    MailFiles = mailFiles,
                    Recipients = para.Recipients,
                    // Sender = "hxx",
                    SenderAddress = account,
                    Subject = para.Subject,
                };


                //保存草稿
                using (var client = new ImapClient())
                {
                    #region 连接到邮件服务器
                    try
                    {
                        //一、创建获取邮件客户端并连接到邮件服务器。
                        //带端口号和协议的连接方式
                        client.Connect(ImapHost, ImapPort, true);
                    }
                    catch (ImapCommandException ex)
                    {
                        throw new Exception("尝试连接时出错");
                    }
                    catch (ImapProtocolException ex)
                    {
                        throw new Exception("尝试连接时的协议错误");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("服务器连接错误:{0}");
                    }

                    try
                    {
                        // 二、验证登录信息，输入账号和密码登录。
                        client.Authenticate(account, passWord);
                    }
                    catch (AuthenticationException ex)
                    {
                        throw new Exception("无效的用户名或密码");
                    }
                    catch (ImapCommandException ex)
                    {
                        throw new Exception("尝试验证错误");
                    }
                    catch (ImapProtocolException ex)
                    {
                        throw new Exception("尝试验证时的协议错误");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("账户认证错误");
                    }
                    #endregion

                    var folder = client.GetFolder(SpecialFolder.Drafts);

                    folder.Open(FolderAccess.ReadWrite);

                    MimeMessage message = MailMessage.AssemblyMailMessage(mailBodyEntity);

                    // 如果保存的是已经有的草稿邮件,则删除它再保存新的草稿.(没找到保存已有草稿的办法)
                    if (para.draftId > -1)
                    {
                        List<UniqueId> uidls = new List<UniqueId>();
                        uidls.Add(new UniqueId((uint)para.draftId));
                        folder.SetFlags(uidls, MessageFlags.Seen | MessageFlags.Deleted, true);
                        folder.Expunge(uidls);
                    }

                    UniqueId? uid = folder.Append(message, MessageFlags.Seen | MessageFlags.Draft);

                    folder.Close();

                    client.Disconnect(true);

                    SaveResultEntity result = new SaveResultEntity();
                    result.id = uid.HasValue ? (int)uid.Value.Id : -1;
                    res.showapi_res_body = result;
                    res.showapi_res_code = "200";
                    return res;
                }

            }
            catch (Exception e)
            {
                res.showapi_res_error = e.Message;
                res.showapi_res_code = "508";
                return res;
            }

        }

        /// <summary>
        /// 邮件明细
        /// </summary>
        /// <param name="emailById"></param>
        /// <returns></returns>
        public Showapi_Res_Single<ReceiveMailBodyEntity> EmailDetail(EmailById emailById)
        {
            Showapi_Res_Single<ReceiveMailBodyEntity> res = new Showapi_Res_Single<ReceiveMailBodyEntity>();
            try
            {
                var memberid = _PrincipalBase.GetMemberId();
                if (memberid == null || memberid == "")
                {
                    var ReturnSingle = new ReturnSingle<ReceiveMailBodyEntity>();
                    return ReturnSingle.Return();
                }

                #region 获取用户和密码
                string account = "";
                string passWord = "";
                List<EmailInfo> emailInfos = this.getEmailInfo(memberid);
                if (emailInfos.Count <= 0)
                {
                    throw new Exception("邮箱未登录");
                }
                else
                {
                    account = emailInfos[0].email;
                    passWord = emailInfos[0].password;
                }
                #endregion

                var receiveServerConfigurationEntity = new ReceiveServerConfigurationEntity()
                {
                    SenderPassword = passWord,
                    ImapPort = ImapPort,
                    SenderAccount = account,
                    ImapHost = ImapHost,
                };
                res.showapi_res_body = ReceiveEmailHelper.ReceiveEmailById(receiveServerConfigurationEntity, emailById);
                res.showapi_res_code = "200";
                return res;
            }
            catch (Exception e)
            {
                res.showapi_res_error = e.Message;
                res.showapi_res_code = "508";
                return res;
            }
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="attaById"></param>
        /// <returns></returns>
        public Showapi_Res_Single<AttaFile> DownloadBodyParts(AttaById attaById)
        {
            Showapi_Res_Single<AttaFile> res = new Showapi_Res_Single<AttaFile>();
            try
            {
                var memberid = _PrincipalBase.GetMemberId();
                if (memberid == null || memberid == "")
                {
                    var ReturnSingle = new ReturnSingle<AttaFile>();
                    return ReturnSingle.Return();
                }

                #region 获取用户和密码
                string account = "";
                string passWord = "";
                List<EmailInfo> emailInfos = this.getEmailInfo(memberid);
                if (emailInfos.Count <= 0)
                {
                    throw new Exception("邮箱未登录");
                }
                else
                {
                    account = emailInfos[0].email;
                    passWord = emailInfos[0].password;
                }
                #endregion

                var receiveServerConfigurationEntity = new ReceiveServerConfigurationEntity()
                {
                    SenderPassword = passWord,
                    ImapPort = ImapPort,
                    SenderAccount = account,
                    ImapHost = ImapHost,
                };
                res.showapi_res_body = ReceiveEmailHelper.DownloadBodyParts(receiveServerConfigurationEntity, attaById);
                res.showapi_res_code = "200";
                return res;
            }
            catch (Exception e)
            {
                res.showapi_res_error = e.Message;
                res.showapi_res_code = "508";
                return res;
            }
        }
        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="attaById"></param>
        /// <returns></returns>
        public Showapi_Res_Single<AttaFile> IMDownloadBodyParts(ImEmailById attaById)
        {
            Showapi_Res_Single<AttaFile> res = new Showapi_Res_Single<AttaFile>();
            try
            {
                var memberid = _PrincipalBase.GetMemberId();
                if (memberid == null || memberid == "")
                {
                    var ReturnSingle = new ReturnSingle<AttaFile>();
                    return ReturnSingle.Return();
                }
                var date = DateTime.Now.ToString("yyyy-MM-dd");
                var sql3 = "select token from xlx_token where date='" + date + "'";
                var xlx_token = new xlx_token();
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    xlx_token = conText.Query<xlx_token>(sql3).FirstOrDefault();
                }
                GetAttInfoRequest GetMailInfoPara = new GetAttInfoRequest();
                GetMailInfoPara.Body = new GetAttInfoRequestBody();
                GetMailInfoPara.Body.para = new tempuri.org.GetMailInfoPara();
                GetMailInfoPara.Body.para.id = attaById.attrid;
                GetMailInfoPara.Body.para.token = xlx_token.token;
                EndpointAddress Endpoint = new EndpointAddress(Imurl);
                downloadPara downloadPara = new downloadPara();
                using (var proxy = new GenericProxy<SendEmailSoap>(Binding, Endpoint))
                {
                    try
                    {
                        var info = proxy.Execute(c => c.GetAttInfo(GetMailInfoPara));
                        if (info.Body.GetAttInfoResult != null)
                        {
                            downloadPara.content = info.Body.GetAttInfoResult.AttaStream;
                            downloadPara.name = info.Body.GetAttInfoResult.Name;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new BusinessTureException(ex.Message);
                    }
                }
                downloadPara.host = attaById.host;
                res.showapi_res_body = ReceiveEmailHelper.IMDownloadBodyParts(downloadPara);
                res.showapi_res_code = "200";
                return res;
            }
            catch (Exception e)
            {
                res.showapi_res_error = e.Message;
                res.showapi_res_code = "508";
                return res;
            }
        }

        public Showapi_Res_Single<EmailResult> AutoLogin()
        {
            Showapi_Res_Single<EmailResult> res = new Showapi_Res_Single<EmailResult>();
            EmailResult emailResult = new EmailResult();
            try
            {
                var memberid = _PrincipalBase.GetMemberId();
                if (memberid == null || memberid == "")
                {
                    var ReturnSingle = new ReturnSingle<EmailResult>();
                    return ReturnSingle.Return();
                }

                emailResult.message = "";
                emailResult.email = this.isLogin(memberid);
                emailResult.status = string.IsNullOrEmpty(emailResult.email) ? false : true;
                res.showapi_res_body = emailResult;
                res.showapi_res_code = "200";
                return res;
            }
            catch (Exception e)
            {
                res.showapi_res_error = e.Message;
                res.showapi_res_code = "200";
                return res;
            }

        }

        public Showapi_Res_Single<EmailResult> UnLogin()
        {
            Showapi_Res_Single<EmailResult> res = new Showapi_Res_Single<EmailResult>();
            EmailResult emailResult = new EmailResult();
            try
            {
                var memberid = _PrincipalBase.GetMemberId();
                if (memberid == null || memberid == "")
                {
                    var ReturnSingle = new ReturnSingle<EmailResult>();
                    return ReturnSingle.Return();
                }

                if (this.relieveLogin(memberid))
                {
                    emailResult.message = "解除绑定成功";
                    emailResult.status = true;
                }
                else
                {
                    emailResult.message = "解除绑定失败";
                    emailResult.status = false;
                }

                res.showapi_res_body = emailResult;
                res.showapi_res_code = "200";
                return res;
            }
            catch (Exception e)
            {
                res.showapi_res_error = e.Message;
                res.showapi_res_code = "200";
                return res;
            }

        }

        public Showapi_Res_Single<UnReadBean> UnReadMail(UnReadPara unReadPara)
        {
            Showapi_Res_Single<UnReadBean> unReadBeans = new Showapi_Res_Single<UnReadBean>();
            UnReadBean unReadBean = new UnReadBean();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                unReadBeans.showapi_res_code = "200";
                unReadBeans.showapi_res_body = unReadBean;
                return unReadBeans;
            }
            else
            {
                unReadPara.email = isLogin(memberid);
            }

            #region 消息提醒
            var para = unReadPara;
            List<NewsRemindNum> list = new List<NewsRemindNum>();
            string[] array = new string[] { "1", "2", "3", "4", "5", "21" };
            //审批待处理
            var num1 = 0;
            var sql1 = @"select count(*) from Approval_Content a
                        inner join Work_Approval b on b.Id=a.UId
                        where a.OtherMemberId='" + memberid + @"' 
                        and a.State=0 and (b.CompanyId='" + para.id + @"' or b.CompanyId is null or b.CompanyId='')";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                num1 = conText.Query<int>(sql1).FirstOrDefault();
            }
            //日志待点评
            var workLog = _JointOfficeContext.Work_Log.Where(t => t.ReviewPersonId == memberid && t.State == 0 && (t.CompanyId == para.id || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
            var num2 = workLog.Count();
            //我执行的任务
            var num3 = 0;
            var sql3 = @"select count(*) from Execute_Content a
                        inner join Work_Task b on b.Id=a.UId
                        where a.OtherMemberId='" + memberid + @"' 
                        and a.Type=3 and a.State=0 and (b.CompanyId='" + para.id + @"' or b.CompanyId is null or b.CompanyId='')";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                num3 = conText.Query<int>(sql3).FirstOrDefault();
            }
            //日程未开始
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            var programNo = _JointOfficeContext.Work_Program.Where(t => (t.JoinPerson.Contains(memberid) || t.MemberId == memberid) && t.State == 0).ToList();
            foreach (var item in programNo)
            {
                var beginTime = item.Year + " " + item.Hour;
                if (DateTime.Now >= Convert.ToDateTime(beginTime))
                {
                    item.State = 1;
                }
            }
            _JointOfficeContext.SaveChanges();
            var workProgram = _JointOfficeContext.Work_Program.Where(t => (t.JoinPerson.Contains(memberid) || t.JoinPerson.Contains(memberInfo.ZhuBuMen)) && t.State == 0 && (t.CompanyId == para.id || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
            var num4 = workProgram.Count();
            //指令待处理
            var num5 = 0;
            var sql5 = @"select count(*) from Execute_Content a
                        inner join Work_Order b on b.Id=a.UId
                        where a.OtherMemberId='" + memberid + @"' 
                        and a.Type=5 and a.State=0 and (b.CompanyId='" + para.id + @"' or b.CompanyId is null or b.CompanyId='')";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                num5 = conText.Query<int>(sql5).FirstOrDefault();
            }
            //我的回执  未回执
            var num21 = 0;
            var sql21 = @"exec MyReceiptsNoCount '','0','1901-01-01 00:00:00','2200-12-31 23:59:59','" + memberid + "','" + para.id + "'";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                num21 = conText.Query<int>(sql21).FirstOrDefault();
            }

            for (int i = 0; i < array.Length; i++)
            {
                NewsRemindNum NewsRemindNum = new NewsRemindNum();
                switch (array[i])
                {
                    case "1":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = num1;
                        break;
                    case "2":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = num2;
                        break;
                    case "3":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = num3;
                        break;
                    case "4":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = num4;
                        break;
                    case "5":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = num5;
                        break;
                    case "21":
                        NewsRemindNum.type = array[i];
                        NewsRemindNum.num1 = num21;
                        break;
                }
                list.Add(NewsRemindNum);
            }
            unReadBean.NewsRemindNumList = list;
            #endregion

            if (string.IsNullOrEmpty(unReadPara.email))
            {
                unReadBeans.showapi_res_code = "200";
                unReadBeans.showapi_res_body = unReadBean;
                return unReadBeans;
            }

            if (string.IsNullOrEmpty(unReadPara.mailtoken))
            {
                string urltoken = "https://api.exmail.qq.com/cgi-bin/gettoken?corpid=" + corpid + "&corpsecret=" + corpsecret;
                string result_01 = WebApiHelper.HttpGet(urltoken);
                MailResult mailResult_01 = Newtonsoft.Json.JsonConvert.DeserializeObject<MailResult>(result_01);
                if (!string.IsNullOrEmpty(mailResult_01.access_token))
                {
                    unReadPara.mailtoken = mailResult_01.access_token;
                    string unreadtoken = "https://api.exmail.qq.com/cgi-bin/mail/newcount?access_token=" + unReadPara.mailtoken + "&userid=" + unReadPara.email;
                    string result_02 = WebApiHelper.HttpGet(unreadtoken);
                    MailResult mailResult_02 = Newtonsoft.Json.JsonConvert.DeserializeObject<MailResult>(result_02);
                    if ("0".Equals(mailResult_02.errcode))
                    {
                        unReadBean.number = mailResult_02.count;
                        unReadBean.mailtoken = unReadPara.mailtoken;
                    }
                }
            }
            else
            {
                string unreadtoken = "https://api.exmail.qq.com/cgi-bin/mail/newcount?access_token=" + unReadPara.mailtoken + "&userid=" + unReadPara.email;
                string result = WebApiHelper.HttpGet(unreadtoken);
                MailResult mailResult = Newtonsoft.Json.JsonConvert.DeserializeObject<MailResult>(result);
                if ("0".Equals(mailResult.errcode))
                {
                    try
                    {
                        unReadBean.number = mailResult.count;
                    }
                    catch
                    {

                    }
                }
                else
                {
                    string urltoken = "https://api.exmail.qq.com/cgi-bin/gettoken?corpid=" + corpid + "&corpsecret=" + corpsecret;
                    string result_01 = WebApiHelper.HttpGet(urltoken);
                    MailResult mailResult_01 = Newtonsoft.Json.JsonConvert.DeserializeObject<MailResult>(result_01);
                    if (!string.IsNullOrEmpty(mailResult_01.access_token))
                    {
                        unReadPara.mailtoken = mailResult_01.access_token;
                        unreadtoken = "https://api.exmail.qq.com/cgi-bin/mail/newcount?access_token=" + unReadPara.mailtoken + "&userid=" + unReadPara.email;
                        string result_02 = WebApiHelper.HttpGet(unreadtoken);
                        MailResult mailResult_02 = Newtonsoft.Json.JsonConvert.DeserializeObject<MailResult>(result_02);
                        if ("0".Equals(mailResult_02.errcode))
                        {
                            unReadBean.number = mailResult_02.count;
                            unReadBean.mailtoken = unReadPara.mailtoken;
                        }
                    }
                }
            }

            unReadBeans.showapi_res_code = "200";
            unReadBeans.showapi_res_body = unReadBean;
            return unReadBeans;
        }

    }
}