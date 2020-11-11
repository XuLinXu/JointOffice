using JointOffice.CommonTool.MailKit;
using JointOffice.Models;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Extensions.FileSystemGlobbing;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTool.MailKit
{
    /// <summary>
    /// 跟投邮件服务API
    /// </summary>
    public static class ReceiveEmailHelper
    {

        /// <summary>
        /// 接收邮件
        /// </summary>
        public static Showapi_res_body_list<ReceiveMailBodyEntity> ReceiveEmail(ReceiveServerConfigurationEntity receiveServerConfiguration, EmailLoginBean emailLoginBean)
        {
            if (receiveServerConfiguration == null)
            {
                throw new ArgumentNullException();
            }
            Showapi_res_body_list<ReceiveMailBodyEntity> showapi_Res_Body_List = new Showapi_res_body_list<ReceiveMailBodyEntity>();

            using (var client = new ImapClient())
            {
                #region 连接到邮件服务器
                try
                {
                    //一、创建获取邮件客户端并连接到邮件服务器。
                    //带端口号和协议的连接方式
                    client.Connect(receiveServerConfiguration.ImapHost, receiveServerConfiguration.ImapPort, SecureSocketOptions.SslOnConnect);
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
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    // 二、验证登录信息，输入账号和密码登录。
                    client.Authenticate(receiveServerConfiguration.SenderAccount, receiveServerConfiguration.SenderPassword);
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

                int page = emailLoginBean.page - 1;
                int size = emailLoginBean.size;
                string type = emailLoginBean.type;

                var folder = client.GetFolder(type);

                folder.Open(FolderAccess.ReadOnly);

                string FolderType = folder.Name;

                // 获取邮箱的所有文件夹列表
                //List<IMailFolder> mailFolderList = client.GetFolders(client.PersonalNamespaces[0]).ToList();
                //只获取收件箱文件加

                //获取大于2016-9-1时间的所有邮件的唯一Id  SearchQuery.DeliveredAfter(DateTime.Parse("2016-9-1"))

                // SearchQuery sq = SearchQuery.DeliveredAfter(DateTime.Today.AddDays(-30));
                string title = emailLoginBean.title;
                string receiveName = emailLoginBean.receiveName;
                SearchQuery sq = null;
                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(emailLoginBean.status))
                {

                    //if ("0".Equals(emailLoginBean.status))
                    //{
                    //    //0未读
                    //    sq = SearchQuery.SubjectContains(title).And(SearchQuery.NotSeen);
                    //}
                    //else if ("1".Equals(emailLoginBean.status))
                    //{
                    //    //1已读
                    //    sq = SearchQuery.SubjectContains(title).And(SearchQuery.Seen); ;
                    //}
                    //else if ("2".Equals(emailLoginBean.status))
                    //{
                    //    //2已回复
                    //    sq = SearchQuery.SubjectContains(title).And(SearchQuery.Answered);
                    //}
                    if ("0".Equals(emailLoginBean.status))
                    {
                        //0未读
                        sq = SearchQuery.NotSeen;
                    }
                    else if ("1".Equals(emailLoginBean.status))
                    {
                        //1已读
                        sq = SearchQuery.Seen;
                    }
                    else if ("2".Equals(emailLoginBean.status))
                    {
                        //2已回复
                        sq = SearchQuery.Answered;
                    }
                }
                else if (!string.IsNullOrEmpty(title))
                {
                    sq = SearchQuery.SubjectContains(title);
                }
                else if (!string.IsNullOrEmpty(emailLoginBean.status))
                {
                    if ("0".Equals(emailLoginBean.status))
                    {
                        //0未读
                        sq = SearchQuery.NotSeen;
                    }
                    else if ("1".Equals(emailLoginBean.status))
                    {
                        //1已读
                        sq = SearchQuery.Seen;
                    }
                    else if ("2".Equals(emailLoginBean.status))
                    {
                        //2已回复
                        sq = SearchQuery.Answered;
                    }
                }
                else
                {
                    sq = SearchQuery.All;
                }
                
                var uidss = folder.Search(sq);
                IList<IMessageSummary> messageSummaries = new List<IMessageSummary>();
                List<ReceiveMailBodyEntity> receiveMailBodyEntitys = new List<ReceiveMailBodyEntity>();
                if (!string.IsNullOrEmpty(title)||!string.IsNullOrEmpty(receiveName))
                {

                    if (uidss.Count > 0)
                    {
                        messageSummaries = folder.Fetch(uidss, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope);

                        if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(receiveName))
                        {
                            messageSummaries = messageSummaries.Where(t => t.NormalizedSubject.ToLower().Contains(title.ToLower()) && t.Envelope.To.ToString().ToLower().Contains(receiveName.ToLower())).ToList();
                        }
                        else if (!string.IsNullOrEmpty(title))
                        {
                            messageSummaries = messageSummaries.Where(t => t.NormalizedSubject.ToLower().Contains(title.ToLower())).ToList();
                        }
                        else if (!string.IsNullOrEmpty(receiveName))
                        {
                            messageSummaries = messageSummaries.Where(t => t.Envelope.To.ToString().ToLower().Contains(receiveName.ToLower())).ToList();
                        }



                        if (messageSummaries.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(emailLoginBean.order) && !string.IsNullOrEmpty(emailLoginBean.sort))
                            {
                                if ("descending".Equals(emailLoginBean.order))
                                {
                                    if ("date".Equals(emailLoginBean.sort))
                                    {
                                        messageSummaries = messageSummaries.OrderByDescending(t => t.UniqueId).ToList();
                                    }
                                    else
                                    {
                                        messageSummaries = messageSummaries.OrderByDescending(t => t.UniqueId).ToList();
                                    }
                                }
                                else
                                {
                                    if ("date".Equals(emailLoginBean.sort))
                                    {
                                        messageSummaries = messageSummaries.OrderBy(t => t.UniqueId).ToList();
                                    }
                                    else
                                    {
                                        messageSummaries = messageSummaries.OrderBy(t => t.UniqueId).ToList();
                                    }
                                }
                            }
                            else
                            {
                                messageSummaries = messageSummaries.OrderByDescending(t => t.UniqueId).ToList();
                            }
                        }
                    }

                    showapi_Res_Body_List.allNum = messageSummaries.Count;
                    folder.Status(StatusItems.Count | StatusItems.Unread);
                    //int total = folder.Count;
                    showapi_Res_Body_List.unread = folder.Unread;
                }
                else
                {
                    if (uidss.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(emailLoginBean.order) && !string.IsNullOrEmpty(emailLoginBean.sort))
                        {
                            if ("descending".Equals(emailLoginBean.order))
                            {
                                if ("date".Equals(emailLoginBean.sort))
                                {
                                    uidss = uidss.OrderByDescending(t => t.Id).ToList();
                                }
                                else
                                {
                                    uidss = uidss.OrderByDescending(t => t.Id).ToList();
                                }
                            }
                            else
                            {
                                if ("date".Equals(emailLoginBean.sort))
                                {
                                    uidss = uidss.OrderBy(t => t.Id).ToList();
                                }
                                else
                                {
                                    uidss = uidss.OrderBy(t => t.Id).ToList();
                                }
                            }
                        }
                        else
                        {
                            uidss = uidss.OrderByDescending(t => t.Id).ToList();
                        }
                    }

                    showapi_Res_Body_List.allNum = uidss.Count;
                    folder.Status(StatusItems.Count | StatusItems.Unread);
                    //int total = folder.Count;
                    showapi_Res_Body_List.unread = folder.Unread;
                }

                //获取完整邮件
                if (!string.IsNullOrEmpty(title)||!string.IsNullOrEmpty(receiveName))
                {
                    for (int i = 0; i < messageSummaries.Count; i++)
                    {
                        try
                        {
                            if (i >= (page * size) && i < ((page + 1) * size))
                            {

                                // 获取这些邮件的摘要信息
                                List<UniqueId> uids = new List<UniqueId>();
                                uids.Add(new UniqueId(messageSummaries[i].UniqueId.Id));
                                var emaills = folder.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure | MessageSummaryItems.Full);
                                if (emaills.Count > 0)
                                {
                                    var emhead = emaills[0];

                                    ReceiveMailBodyEntity receiveMailBodyEntity = new ReceiveMailBodyEntity();
                                    receiveMailBodyEntity.id = messageSummaries[i].UniqueId.Id.ToString();

                                    receiveMailBodyEntity.Subject = emhead.Envelope.Subject;

                                    if (emhead.Envelope.From.Count > 0)
                                    {
                                        receiveMailBodyEntity.From = emhead.Envelope.From.Mailboxes.ElementAt(0).Name;
                                        receiveMailBodyEntity.FromAddress = emhead.Envelope.From.Mailboxes.ElementAt(0).Address;
                                    }

                                    receiveMailBodyEntity.FolderType = FolderType;

                                    // 收件人可能有多个
                                    receiveMailBodyEntity.To = new List<PersonBean>();
                                    foreach (var to in emhead.Envelope.To.Mailboxes)
                                    {
                                        receiveMailBodyEntity.To.Add(new PersonBean { Name = to.Name, Mail = to.Address });
                                    }
                                    // 收件人可能有多个 - 加到收件人显示
                                    receiveMailBodyEntity.Cc = new List<PersonBean>();
                                    foreach (var cc in emhead.Envelope.Cc.Mailboxes)
                                    {
                                        receiveMailBodyEntity.To.Add(new PersonBean { Name = cc.Name, Mail = cc.Address });
                                    }
                                    // 收件人可能有多个 - 加到收件人显示
                                    receiveMailBodyEntity.Bcc = new List<PersonBean>();
                                    foreach (var bcc in emhead.Envelope.Bcc.Mailboxes)
                                    {
                                        receiveMailBodyEntity.To.Add(new PersonBean { Name = bcc.Name, Mail = bcc.Address });
                                    }

                                    receiveMailBodyEntity.Date = emhead.Envelope.Date.Value.DateTime;
                                    // 附件
                                    // 附件个数(传embody时,包含有附件完整信息)
                                    receiveMailBodyEntity.AttaCount = emhead.Attachments.Count();


                                    // 邮件状态,加星
                                    if (emhead.Flags.HasValue)
                                    {
                                        receiveMailBodyEntity.Star = emhead.Flags.Value.HasFlag(MessageFlags.Flagged);
                                    }
                                    //未读
                                    receiveMailBodyEntity.Status = 0;

                                    // 邮件状态,已读未读等等
                                    if (emhead.Flags.HasValue)
                                    {
                                        if (emhead.Flags.Value.HasFlag(MessageFlags.Seen))
                                        {
                                            receiveMailBodyEntity.Status = 1;
                                        }
                                    }

                                    // 邮件状态,回复
                                    if (emhead.Flags.HasValue)
                                    {
                                        if (emhead.Flags.Value.HasFlag(MessageFlags.Answered))
                                        {
                                            receiveMailBodyEntity.Status = 2;
                                        }
                                    }

                                    //if (emhead.TextBody != null)
                                    //{
                                    //    // this will download *just* the text/plain part  
                                    //    var text = folder.GetBodyPart(emhead.UniqueId, emhead.TextBody);
                                    //}

                                    //if (emhead.HtmlBody != null)
                                    //{
                                    //    // this will download *just* the text/html part  
                                    //    var html = folder.GetBodyPart(emhead.UniqueId, emhead.HtmlBody);
                                    //}

                                    //// if you'd rather grab, say, an image attachment... it might look something like this:  
                                    //if (emhead.Body is BodyPartMultipart)
                                    //{
                                    //    var multipart = (BodyPartMultipart)emhead.Body;

                                    //    var attachment = multipart.BodyParts.OfType<BodyPartBasic>().First();
                                    //    if (attachment != null)
                                    //    {
                                    //        // this will download *just* the attachment  
                                    //        var part = folder.GetBodyPart(emhead.UniqueId, attachment);
                                    //    }
                                    //}

                                    //MimeMessage message = folder.GetMessage(messageSummaries[i].UniqueId);
                                    //receiveMailBodyEntity.Body = message.TextBody;
                                    //receiveMailBodyEntity.Html = message.HtmlBody;

                                    //// 附件信息
                                    //if (receiveMailBodyEntity.AttaCount > 0)
                                    //{
                                    //    receiveMailBodyEntity.Attachments = new List<AttachmentBean>();
                                    //    // 这里要转成mimepart类型
                                    //    foreach (MimePart attachment in message.Attachments)
                                    //    {
                                    //        var atta = new AttachmentBean();
                                    //        atta.Name = attachment.ContentDisposition.FileName;
                                    //        Stream stream = new MemoryStream(); 
                                    //        attachment.Content.DecodeTo(stream);
                                    //        StreamReader reader = new StreamReader(stream);
                                    //        string text = reader.ReadToEnd();
                                    //        atta.Size = Math.Round((double)stream.Length / 1024, 1).ToString();
                                    //        receiveMailBodyEntity.Attachments.Add(atta);
                                    //    }
                                    //}
                                    receiveMailBodyEntitys.Add(receiveMailBodyEntity);
                                }
                                else
                                {
                                    MimeMessage message = folder.GetMessage(messageSummaries[i].UniqueId);

                                }
                            }
                            if (i >= ((page + 1) * size) - 1)
                            {
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            #region 连接到邮件服务器
                            try
                            {
                                //一、创建获取邮件客户端并连接到邮件服务器。
                                //带端口号和协议的连接方式
                                client.Connect(receiveServerConfiguration.ImapHost, receiveServerConfiguration.ImapPort, SecureSocketOptions.SslOnConnect);
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
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                // 二、验证登录信息，输入账号和密码登录。
                                client.Authenticate(receiveServerConfiguration.SenderAccount, receiveServerConfiguration.SenderPassword);
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

                            #region 打开并搜索邮件
                            folder = client.GetFolder(type);

                            folder.Open(FolderAccess.ReadWrite);

                            sq = null;
                            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(emailLoginBean.status))
                            {

                                //if ("0".Equals(emailLoginBean.status))
                                //{
                                //    //0未读
                                //    sq = SearchQuery.SubjectContains(title).And(SearchQuery.NotSeen);
                                //}
                                //else if ("1".Equals(emailLoginBean.status))
                                //{
                                //    //1已读
                                //    sq = SearchQuery.SubjectContains(title).And(SearchQuery.Seen); ;
                                //}
                                //else if ("2".Equals(emailLoginBean.status))
                                //{
                                //    //2已回复
                                //    sq = SearchQuery.SubjectContains(title).And(SearchQuery.Answered);
                                //}
                                if ("0".Equals(emailLoginBean.status))
                                {
                                    //0未读
                                    sq = SearchQuery.NotSeen;
                                }
                                else if ("1".Equals(emailLoginBean.status))
                                {
                                    //1已读
                                    sq = SearchQuery.Seen;
                                }
                                else if ("2".Equals(emailLoginBean.status))
                                {
                                    //2已回复
                                    sq = SearchQuery.Answered;
                                }
                            }
                            else if (!string.IsNullOrEmpty(title))
                            {
                                sq = SearchQuery.SubjectContains(title);
                            }
                            else if (!string.IsNullOrEmpty(emailLoginBean.status))
                            {
                                if ("0".Equals(emailLoginBean.status))
                                {
                                    //0未读
                                    sq = SearchQuery.NotSeen;
                                }
                                else if ("1".Equals(emailLoginBean.status))
                                {
                                    //1已读
                                    sq = SearchQuery.Seen;
                                }
                                else if ("2".Equals(emailLoginBean.status))
                                {
                                    //2已回复
                                    sq = SearchQuery.Answered;
                                }
                            }
                            else
                            {
                                sq = SearchQuery.All;
                            }

                            uidss = folder.Search(sq);
                            messageSummaries = new List<IMessageSummary>();
                            if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(receiveName))
                            {

                                if (uidss.Count > 0)
                                {
                                    messageSummaries = folder.Fetch(uidss, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope);

                                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(receiveName))
                                    {
                                        messageSummaries = messageSummaries.Where(t => t.NormalizedSubject.ToLower().Contains(title.ToLower()) && t.Envelope.To.ToString().ToLower().Contains(receiveName.ToLower())).ToList();
                                    }
                                    else if (!string.IsNullOrEmpty(title))
                                    {
                                        messageSummaries = messageSummaries.Where(t => t.NormalizedSubject.ToLower().Contains(title.ToLower())).ToList();
                                    }
                                    else if (!string.IsNullOrEmpty(receiveName))
                                    {
                                        messageSummaries = messageSummaries.Where(t => t.Envelope.To.ToString().ToLower().Contains(receiveName.ToLower())).ToList();
                                    }



                                    if (messageSummaries.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(emailLoginBean.order) && !string.IsNullOrEmpty(emailLoginBean.sort))
                                        {
                                            if ("descending".Equals(emailLoginBean.order))
                                            {
                                                if ("date".Equals(emailLoginBean.sort))
                                                {
                                                    messageSummaries = messageSummaries.OrderByDescending(t => t.UniqueId).ToList();
                                                }
                                                else
                                                {
                                                    messageSummaries = messageSummaries.OrderByDescending(t => t.UniqueId).ToList();
                                                }
                                            }
                                            else
                                            {
                                                if ("date".Equals(emailLoginBean.sort))
                                                {
                                                    messageSummaries = messageSummaries.OrderBy(t => t.UniqueId).ToList();
                                                }
                                                else
                                                {
                                                    messageSummaries = messageSummaries.OrderBy(t => t.UniqueId).ToList();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            messageSummaries = messageSummaries.OrderByDescending(t => t.UniqueId).ToList();
                                        }
                                    }
                                }

                                showapi_Res_Body_List.allNum = messageSummaries.Count;
                                folder.Status(StatusItems.Count | StatusItems.Unread);
                                //int total = folder.Count;
                                showapi_Res_Body_List.unread = folder.Unread;
                            }
                            else
                            {
                                if (uidss.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(emailLoginBean.order) && !string.IsNullOrEmpty(emailLoginBean.sort))
                                    {
                                        if ("descending".Equals(emailLoginBean.order))
                                        {
                                            if ("date".Equals(emailLoginBean.sort))
                                            {
                                                uidss = uidss.OrderByDescending(t => t.Id).ToList();
                                            }
                                            else
                                            {
                                                uidss = uidss.OrderByDescending(t => t.Id).ToList();
                                            }
                                        }
                                        else
                                        {
                                            if ("date".Equals(emailLoginBean.sort))
                                            {
                                                uidss = uidss.OrderBy(t => t.Id).ToList();
                                            }
                                            else
                                            {
                                                uidss = uidss.OrderBy(t => t.Id).ToList();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        uidss = uidss.OrderByDescending(t => t.Id).ToList();
                                    }
                                }

                                showapi_Res_Body_List.allNum = uidss.Count;
                                folder.Status(StatusItems.Count | StatusItems.Unread);
                                //int total = folder.Count;
                                showapi_Res_Body_List.unread = folder.Unread;
                            }
                            #endregion

                            continue;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < uidss.Count; i++)
                    {
                        try
                        {
                            if (i >= (page * size) && i < ((page + 1) * size))
                            {

                                // 获取这些邮件的摘要信息
                                List<UniqueId> uids = new List<UniqueId>();
                                uids.Add(new UniqueId(uidss[i].Id));
                                var emaills = folder.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure | MessageSummaryItems.Full);
                                if (emaills.Count > 0)
                                {
                                    var emhead = emaills[0];

                                    ReceiveMailBodyEntity receiveMailBodyEntity = new ReceiveMailBodyEntity();
                                    receiveMailBodyEntity.id = uidss[i].Id.ToString();

                                    receiveMailBodyEntity.Subject = emhead.Envelope.Subject;

                                    if (emhead.Envelope.From.Count > 0)
                                    {
                                        receiveMailBodyEntity.From = emhead.Envelope.From.Mailboxes.ElementAt(0).Name;
                                        receiveMailBodyEntity.FromAddress = emhead.Envelope.From.Mailboxes.ElementAt(0).Address;
                                    }

                                    receiveMailBodyEntity.FolderType = FolderType;

                                    // 收件人可能有多个
                                    receiveMailBodyEntity.To = new List<PersonBean>();
                                    foreach (var to in emhead.Envelope.To.Mailboxes)
                                    {
                                        receiveMailBodyEntity.To.Add(new PersonBean { Name = to.Name, Mail = to.Address });
                                    }
                                    // 收件人可能有多个 - 加到收件人显示
                                    receiveMailBodyEntity.Cc = new List<PersonBean>();
                                    foreach (var cc in emhead.Envelope.Cc.Mailboxes)
                                    {
                                        receiveMailBodyEntity.To.Add(new PersonBean { Name = cc.Name, Mail = cc.Address });
                                    }
                                    // 收件人可能有多个 - 加到收件人显示
                                    receiveMailBodyEntity.Bcc = new List<PersonBean>();
                                    foreach (var bcc in emhead.Envelope.Bcc.Mailboxes)
                                    {
                                        receiveMailBodyEntity.To.Add(new PersonBean { Name = bcc.Name, Mail = bcc.Address });
                                    }

                                    receiveMailBodyEntity.Date = emhead.Envelope.Date.Value.DateTime;
                                    // 附件
                                    // 附件个数(传embody时,包含有附件完整信息)
                                    receiveMailBodyEntity.AttaCount = emhead.Attachments.Count();


                                    // 邮件状态,加星
                                    if (emhead.Flags.HasValue)
                                    {
                                        receiveMailBodyEntity.Star = emhead.Flags.Value.HasFlag(MessageFlags.Flagged);
                                    }
                                    //未读
                                    receiveMailBodyEntity.Status = 0;

                                    // 邮件状态,已读未读等等
                                    if (emhead.Flags.HasValue)
                                    {
                                        if (emhead.Flags.Value.HasFlag(MessageFlags.Seen))
                                        {
                                            receiveMailBodyEntity.Status = 1;
                                        }
                                    }

                                    // 邮件状态,回复
                                    if (emhead.Flags.HasValue)
                                    {
                                        if (emhead.Flags.Value.HasFlag(MessageFlags.Answered))
                                        {
                                            receiveMailBodyEntity.Status = 2;
                                        }
                                    }

                                    //if (emhead.TextBody != null)
                                    //{
                                    //    // this will download *just* the text/plain part  
                                    //    var text = folder.GetBodyPart(emhead.UniqueId, emhead.TextBody);
                                    //}

                                    //if (emhead.HtmlBody != null)
                                    //{
                                    //    // this will download *just* the text/html part  
                                    //    var html = folder.GetBodyPart(emhead.UniqueId, emhead.HtmlBody);
                                    //}

                                    //// if you'd rather grab, say, an image attachment... it might look something like this:  
                                    //if (emhead.Body is BodyPartMultipart)
                                    //{
                                    //    var multipart = (BodyPartMultipart)emhead.Body;

                                    //    var attachment = multipart.BodyParts.OfType<BodyPartBasic>().First();
                                    //    if (attachment != null)
                                    //    {
                                    //        // this will download *just* the attachment  
                                    //        var part = folder.GetBodyPart(emhead.UniqueId, attachment);
                                    //    }
                                    //}

                                    //MimeMessage message = folder.GetMessage(new UniqueId(uidss[i].Id));
                                    //receiveMailBodyEntity.Body = message.TextBody;
                                    //receiveMailBodyEntity.Html = message.HtmlBody;

                                    //// 附件信息
                                    //if (receiveMailBodyEntity.AttaCount > 0)
                                    //{
                                    //    receiveMailBodyEntity.Attachments = new List<AttachmentBean>();
                                    //    // 这里要转成mimepart类型
                                    //    foreach (MimePart attachment in message.Attachments)
                                    //    {
                                    //        var atta = new AttachmentBean();
                                    //        atta.Name = attachment.ContentDisposition.FileName;
                                    //        Stream stream = new MemoryStream(); 
                                    //        attachment.Content.DecodeTo(stream);
                                    //        StreamReader reader = new StreamReader(stream);
                                    //        string text = reader.ReadToEnd();
                                    //        atta.Size = Math.Round((double)stream.Length / 1024, 1).ToString();
                                    //        receiveMailBodyEntity.Attachments.Add(atta);
                                    //    }
                                    //}
                                    receiveMailBodyEntitys.Add(receiveMailBodyEntity);
                                }
                                else
                                {
                                    MimeMessage message = folder.GetMessage(new UniqueId(uidss[i].Id));

                                }
                            }
                            if (i >= ((page + 1) * size) - 1)
                            {
                                break;
                            }
                        }
                        catch (Exception e)
                        {

                            #region 连接到邮件服务器
                            try
                            {
                                //一、创建获取邮件客户端并连接到邮件服务器。
                                //带端口号和协议的连接方式
                                client.Connect(receiveServerConfiguration.ImapHost, receiveServerConfiguration.ImapPort, SecureSocketOptions.SslOnConnect);
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
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                // 二、验证登录信息，输入账号和密码登录。
                                client.Authenticate(receiveServerConfiguration.SenderAccount, receiveServerConfiguration.SenderPassword);
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

                            #region 打开并搜索邮件
                            folder = client.GetFolder(type);

                            folder.Open(FolderAccess.ReadWrite);

                            sq = null;
                            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(emailLoginBean.status))
                            {

                                //if ("0".Equals(emailLoginBean.status))
                                //{
                                //    //0未读
                                //    sq = SearchQuery.SubjectContains(title).And(SearchQuery.NotSeen);
                                //}
                                //else if ("1".Equals(emailLoginBean.status))
                                //{
                                //    //1已读
                                //    sq = SearchQuery.SubjectContains(title).And(SearchQuery.Seen); ;
                                //}
                                //else if ("2".Equals(emailLoginBean.status))
                                //{
                                //    //2已回复
                                //    sq = SearchQuery.SubjectContains(title).And(SearchQuery.Answered);
                                //}
                                if ("0".Equals(emailLoginBean.status))
                                {
                                    //0未读
                                    sq = SearchQuery.NotSeen;
                                }
                                else if ("1".Equals(emailLoginBean.status))
                                {
                                    //1已读
                                    sq = SearchQuery.Seen;
                                }
                                else if ("2".Equals(emailLoginBean.status))
                                {
                                    //2已回复
                                    sq = SearchQuery.Answered;
                                }
                            }
                            else if (!string.IsNullOrEmpty(title))
                            {
                                sq = SearchQuery.SubjectContains(title);
                            }
                            else if (!string.IsNullOrEmpty(emailLoginBean.status))
                            {
                                if ("0".Equals(emailLoginBean.status))
                                {
                                    //0未读
                                    sq = SearchQuery.NotSeen;
                                }
                                else if ("1".Equals(emailLoginBean.status))
                                {
                                    //1已读
                                    sq = SearchQuery.Seen;
                                }
                                else if ("2".Equals(emailLoginBean.status))
                                {
                                    //2已回复
                                    sq = SearchQuery.Answered;
                                }
                            }
                            else
                            {
                                sq = SearchQuery.All;
                            }

                            uidss = folder.Search(sq);
                            messageSummaries = new List<IMessageSummary>();
                            if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(receiveName))
                            {

                                if (uidss.Count > 0)
                                {
                                    messageSummaries = folder.Fetch(uidss, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope);

                                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(receiveName))
                                    {
                                        messageSummaries = messageSummaries.Where(t => t.NormalizedSubject.ToLower().Contains(title.ToLower()) && t.Envelope.To.ToString().ToLower().Contains(receiveName.ToLower())).ToList();
                                    }
                                    else if (!string.IsNullOrEmpty(title))
                                    {
                                        messageSummaries = messageSummaries.Where(t => t.NormalizedSubject.ToLower().Contains(title.ToLower())).ToList();
                                    }
                                    else if (!string.IsNullOrEmpty(receiveName))
                                    {
                                        messageSummaries = messageSummaries.Where(t => t.Envelope.To.ToString().ToLower().Contains(receiveName.ToLower())).ToList();
                                    }



                                    if (messageSummaries.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(emailLoginBean.order) && !string.IsNullOrEmpty(emailLoginBean.sort))
                                        {
                                            if ("descending".Equals(emailLoginBean.order))
                                            {
                                                if ("date".Equals(emailLoginBean.sort))
                                                {
                                                    messageSummaries = messageSummaries.OrderByDescending(t => t.UniqueId).ToList();
                                                }
                                                else
                                                {
                                                    messageSummaries = messageSummaries.OrderByDescending(t => t.UniqueId).ToList();
                                                }
                                            }
                                            else
                                            {
                                                if ("date".Equals(emailLoginBean.sort))
                                                {
                                                    messageSummaries = messageSummaries.OrderBy(t => t.UniqueId).ToList();
                                                }
                                                else
                                                {
                                                    messageSummaries = messageSummaries.OrderBy(t => t.UniqueId).ToList();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            messageSummaries = messageSummaries.OrderByDescending(t => t.UniqueId).ToList();
                                        }
                                    }
                                }

                                showapi_Res_Body_List.allNum = messageSummaries.Count;
                                folder.Status(StatusItems.Count | StatusItems.Unread);
                                //int total = folder.Count;
                                showapi_Res_Body_List.unread = folder.Unread;
                            }
                            else
                            {
                                if (uidss.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(emailLoginBean.order) && !string.IsNullOrEmpty(emailLoginBean.sort))
                                    {
                                        if ("descending".Equals(emailLoginBean.order))
                                        {
                                            if ("date".Equals(emailLoginBean.sort))
                                            {
                                                uidss = uidss.OrderByDescending(t => t.Id).ToList();
                                            }
                                            else
                                            {
                                                uidss = uidss.OrderByDescending(t => t.Id).ToList();
                                            }
                                        }
                                        else
                                        {
                                            if ("date".Equals(emailLoginBean.sort))
                                            {
                                                uidss = uidss.OrderBy(t => t.Id).ToList();
                                            }
                                            else
                                            {
                                                uidss = uidss.OrderBy(t => t.Id).ToList();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        uidss = uidss.OrderByDescending(t => t.Id).ToList();
                                    }
                                }

                                showapi_Res_Body_List.allNum = uidss.Count;
                                folder.Status(StatusItems.Count | StatusItems.Unread);
                                //int total = folder.Count;
                                showapi_Res_Body_List.unread = folder.Unread;
                            }
                            #endregion


                            continue;
                        }
                    }
                }
                if (folder.IsOpen)
                {
                    folder.Close();
                }
                
                client.Disconnect(true);

                showapi_Res_Body_List.currentPage = page;
                showapi_Res_Body_List.contentlist = receiveMailBodyEntitys;

                return showapi_Res_Body_List;
            }
        }

        /// <summary>
        /// 邮件明细
        /// </summary>
        public static ReceiveMailBodyEntity ReceiveEmailById(ReceiveServerConfigurationEntity receiveServerConfiguration, EmailById emailById)
        {
            if (receiveServerConfiguration == null)
            {
                throw new ArgumentNullException();
            }

            using (var client = new ImapClient())
            {
                #region 连接到邮件服务器
                try
                {
                    //一、创建获取邮件客户端并连接到邮件服务器。
                    //带端口号和协议的连接方式
                    client.Connect(receiveServerConfiguration.ImapHost, receiveServerConfiguration.ImapPort, SecureSocketOptions.SslOnConnect);
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
                    client.Authenticate(receiveServerConfiguration.SenderAccount, receiveServerConfiguration.SenderPassword);
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

                var folder = client.GetFolder(emailById.type);

                folder.Open(FolderAccess.ReadWrite);

                //获取完整邮件
                ReceiveMailBodyEntity receiveMailBodyEntity = new ReceiveMailBodyEntity();
                // 获取这些邮件的摘要信息
                List<UniqueId> uids = new List<UniqueId>();
                uids.Add(new UniqueId(emailById.id));
                var emaills = folder.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure | MessageSummaryItems.Full);
                if (emaills.Count > 0)
                {
                    var emhead = emaills[0];

                    receiveMailBodyEntity.id = emailById.id.ToString();

                    receiveMailBodyEntity.Subject = string.IsNullOrEmpty(emhead.Envelope.Subject) ? "" : emhead.Envelope.Subject;

                    if (emhead.Envelope.From.Count > 0)
                    {
                        receiveMailBodyEntity.From = emhead.Envelope.From.Mailboxes.ElementAt(0).Name;
                        receiveMailBodyEntity.FromAddress = emhead.Envelope.From.Mailboxes.ElementAt(0).Address;
                    }

                    // 收件人可能有多个
                    receiveMailBodyEntity.To = new List<PersonBean>();
                    foreach (var to in emhead.Envelope.To.Mailboxes)
                    {
                        receiveMailBodyEntity.To.Add(new PersonBean { Name = to.Name, Mail = to.Address });
                    }
                    // 收件人可能有多个
                    receiveMailBodyEntity.Cc = new List<PersonBean>();
                    foreach (var cc in emhead.Envelope.Cc.Mailboxes)
                    {
                        receiveMailBodyEntity.Cc.Add(new PersonBean { Name = cc.Name, Mail = cc.Address });
                    }
                    // 收件人可能有多个
                    receiveMailBodyEntity.Bcc = new List<PersonBean>();
                    foreach (var bcc in emhead.Envelope.Bcc.Mailboxes)
                    {
                        receiveMailBodyEntity.Bcc.Add(new PersonBean { Name = bcc.Name, Mail = bcc.Address });
                    }

                    receiveMailBodyEntity.Date = emhead.Envelope.Date.Value.DateTime;
                    // 附件
                    // 附件个数(传embody时,包含有附件完整信息)
                    receiveMailBodyEntity.AttaCount = emhead.Attachments.Count();

                    // 邮件状态,加星
                    if (emhead.Flags.HasValue)
                    {
                        receiveMailBodyEntity.Star = emhead.Flags.Value.HasFlag(MessageFlags.Flagged);
                    }

                    //未读
                    receiveMailBodyEntity.Status = 0;

                    // 邮件状态,已读未读等等
                    if (emhead.Flags.HasValue)
                    {
                        if (emhead.Flags.Value.HasFlag(MessageFlags.Seen))
                        {
                            receiveMailBodyEntity.Status = 1;
                        }
                    }

                    // 邮件状态,回复
                    if (emhead.Flags.HasValue)
                    {
                        if (emhead.Flags.Value.HasFlag(MessageFlags.Answered))
                        {
                            receiveMailBodyEntity.Status = 2;
                        }
                    }

                    MimeMessage message = folder.GetMessage(new UniqueId(emailById.id));

                    // 附件信息
                    if (receiveMailBodyEntity.AttaCount > 0)
                    {
                        receiveMailBodyEntity.Attachments = new List<AttachmentBean>();

                        // 这里要转成mimepart类型
                        int index = 0;
                        foreach (var attachment in emhead.Attachments)
                        {
                            //查看不下载
                            var atta = new AttachmentBean();
                            atta.Name = attachment.ContentDisposition.FileName;
                            atta.index = index;
                            receiveMailBodyEntity.Attachments.Add(atta);
                            index++;
                        }
                    }

                    if (!string.IsNullOrEmpty(message.HtmlBody))
                    {
                        receiveMailBodyEntity.Html = message.HtmlBody;

                        var directory = "";
                        var htmlAtta = emhead.BodyParts.Where(t => t.IsAttachment == false).ToList();
                        if (htmlAtta.Count > 0)
                        {
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

                            foreach (var attachment in emhead.BodyParts.Where(t => t.IsAttachment == false).ToList())
                            {
                                if (!string.IsNullOrEmpty(attachment.ContentId))
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

                                        string guid = Guid.NewGuid().ToString("D");
                                        //扩展名 + 新名称
                                        string extension = System.IO.Path.GetExtension(part.FileName);
                                        string newFileName = guid + extension;

                                        // 注意：这可能是空的，但大多数会指定一个文件名
                                        var fileName = string.IsNullOrEmpty(part.FileName) ? newFileName : part.FileName;

                                        string path = Path.Combine(directory, newFileName);
                                        string backpath = emailById.host + filePathString + "/" + newFileName;

                                        // decode and save the content to a file
                                        using (var stream = File.Create(path))
                                            part.Content.DecodeTo(stream);
                                        receiveMailBodyEntity.Html = receiveMailBodyEntity.Html.Replace("cid:" + attachment.ContentId, backpath);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        receiveMailBodyEntity.Html = message.TextBody;
                    }

                    // 已读状态
                    if ("INBOX".Equals(emailById.type))
                    {
                        MessageFlags messageFlags = (MessageFlags)1;
                        folder.AddFlags(new UniqueId(emailById.id), messageFlags, true);
                    }
                }
                else
                {
                    receiveMailBodyEntity = null;
                }

                folder.Close();
                client.Disconnect(true);

                return receiveMailBodyEntity;
            }
        }

        /// <summary>
        /// 下载邮件内容
        /// </summary>
        public static AttaFile DownloadBodyParts(ReceiveServerConfigurationEntity receiveServerConfiguration, AttaById attaById)
        {

            using (var client = new ImapClient())
            {
                #region 连接到邮件服务器
                try
                {
                    //一、创建获取邮件客户端并连接到邮件服务器。
                    //带端口号和协议的连接方式
                    client.Connect(receiveServerConfiguration.ImapHost, receiveServerConfiguration.ImapPort, SecureSocketOptions.SslOnConnect);
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
                    client.Authenticate(receiveServerConfiguration.SenderAccount, receiveServerConfiguration.SenderPassword);
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

                var folder = client.GetFolder(attaById.type);

                folder.Open(FolderAccess.ReadWrite);
                //返回地址
                string backpath = "";
                // 获取这些邮件的摘要信息
                List<UniqueId> uids = new List<UniqueId>();
                uids.Add(new UniqueId(attaById.id));
                var emaills = folder.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure | MessageSummaryItems.Full);
                if (emaills.Count > 0)
                {
                    var item = emaills[0];
                    var filePathString = "/AttachmentFile/Download";

                    // 确定一个目录来保存内容
                    var directory = Directory.GetCurrentDirectory() + filePathString;

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

                    int index = 0;
                    // 现在遍历所有附件并将其保存到磁盘
                    foreach (var attachment in item.Attachments)
                    {
                        if (index == attaById.attrid)
                        {
                            // 像我们对内容所做的那样下载附件
                            var entity = folder.GetBodyPart(item.UniqueId, attachment);

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

                                string guid = Guid.NewGuid().ToString("D");
                                //扩展名 + 新名称
                                string extension = System.IO.Path.GetExtension(part.FileName);
                                string newFileName = guid + extension;

                                // 注意：这可能是空的，但大多数会指定一个文件名
                                var fileName = string.IsNullOrEmpty(part.FileName) ? newFileName : part.FileName;

                                string path = Path.Combine(directory, newFileName);
                                backpath = filePathString + "/" + newFileName;

                                // decode and save the content to a file
                                using (var stream = File.Create(path))
                                    part.Content.DecodeTo(stream);
                            }
                            break;
                        }
                        index++;
                    }
                }
                folder.Close();

                client.Disconnect(true);

                AttaFile attaFile = new AttaFile();
                attaFile.path = attaById.host + backpath;
                return attaFile;
            }
        }
        /// <summary>
        /// 下载邮件内容
        /// </summary>
        public static AttaFile IMDownloadBodyParts(downloadPara para)
        {
            try
            {
                var filePathString = "/AttachmentFile/Download";

                // 确定一个目录来保存内容
                var directory = Directory.GetCurrentDirectory() + filePathString;

                bool exists = Directory.Exists(directory);

                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(directory);
                }
                string guid = Guid.NewGuid().ToString("D");
                string extension = System.IO.Path.GetExtension(para.name);
                string newFileName = guid + extension;


                System.IO.File.WriteAllBytes(directory + "/" + newFileName, para.content);

                string path = Path.Combine(directory, newFileName);

                var backpath = para.host + filePathString + "/" + newFileName;
                AttaFile attaFile = new AttaFile();
                attaFile.path = backpath;
                return attaFile;
            }
            catch (Exception ex)
            {
                throw new Exception("邮箱未登录");
            }
        }
    }
}