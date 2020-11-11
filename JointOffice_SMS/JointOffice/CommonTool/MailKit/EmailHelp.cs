using MailKit;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using MimeKit;
using System.Collections.Generic;

public class EmailHelp
{
    ///// <summary>
    ///// Smtp服务器地址
    ///// </summary>
    //private static readonly string SmtpServer = "";

    ///// <summary>
    ///// Pop服务器地址
    ///// </summary>
    //private static readonly string PopServer ="";

    ///// <summary>
    ///// Imap服务器地址
    ///// </summary>
    //private static readonly string ImapServer = "";

    ///// <summary>
    ///// SMTP端口
    ///// </summary>
    //private static readonly int SmtpPort = 0;

    ///// <summary>
    ///// POP端口
    ///// </summary>
    //private static readonly int PopPort = 0;

    ///// <summary>
    ///// IMAP端口
    ///// </summary>
    //private static readonly int ImapPort = 0;

    ///// <summary>
    ///// 邮件发送
    ///// </summary>
    ///// <param name="mailFromAccount">发送邮箱账号</param>
    ///// <param name="mailPassword">发送邮箱密码</param>
    ///// <param name="message">邮件</param>
    //public static void SendEmali(string mailFromAccount, string mailPassword, MimeMessage message)
    //{
    //    using (var client = new MailKit.Net.Smtp.SmtpClient())
    //    {
    //        client.Connect(SmtpServer, SmtpPort, false);

    //        // Note: since we don't have an OAuth2 token, disable
    //        // the XOAUTH2 authentication mechanism.
    //        client.AuthenticationMechanisms.Remove("XOAUTH2");

    //        // Note: only needed if the SMTP server requires authentication
    //        client.Authenticate(mailFromAccount, mailPassword);
    //        client.Send(message);
    //        client.Disconnect(true);
    //    }
    //}

    ///// <summary>
    ///// 创建文本消息
    ///// </summary>
    ///// <param name="fromAddress">发件地址</param>
    ///// <param name="toAddressList">收件地址</param>
    ///// <param name="title">标题</param>
    ///// <param name="content">内容</param>
    ///// <param name="IsPostFiles">是否将POST上传文件加为附件</param>
    ///// <returns></returns>
    //public static MimeMessage CreateTextMessage(MailboxAddress fromAddress, IList<MailboxAddress> toAddressList
    //    , string title, string content, bool IsPostFiles = false)
    //{
    //    var message = new MimeMessage();
    //    message.From.Add(fromAddress);
    //    message.To.AddRange(toAddressList);
    //    message.Subject = title; //设置消息的主题

    //    var html = new TextPart("html")
    //    {
    //        Text = content,
    //    };
    //    var alternative = new Multipart("alternative");
    //    alternative.Add(html);

    //    var multipart = new Multipart("mixed");
    //    multipart.Add(alternative);
    //    if (IsPostFiles)
    //    {
    //        IList<MimePart> multiPartList = GetMimePartList();
    //        foreach (var item in multiPartList)
    //        {
    //            multipart.Add(item);
    //        }
    //    }

    //    message.Body = multipart;
    //    return message;
    //}

    ///// <summary>
    ///// 收邮件
    ///// </summary>
    ///// <param name="mailFromAccount">发送邮箱账号</param>
    ///// <param name="mailPassword">发送邮箱密码</param>
    ///// <param name="searchQuery">查询条件</param>
    ///// <param name="folderName">文件夹名称</param>
    ///// <returns></returns>
    //public static IList<IMessageSummary> ReceiveEmail(string mailFromAccount, string mailPassword, string folderName, SearchQuery searchQuery = null)
    //{
    //    //打开收件箱
    //    var folder = OpenFolder(mailFromAccount, mailPassword, folderName);

    //    //IList<OrderBy> orderByList = new List<OrderBy> { OrderBy.Date };
    //    //查询所有的邮件
    //    var uidss = folder.Search(searchQuery);

    //    IList<IMessageSummary> msgList = new List<IMessageSummary>();
    //    if (uidss.Count > 0)//判断是否查询到邮件
    //    {
    //        //获取邮件头
    //        msgList = folder.Fetch(uidss, MessageSummaryItems.UniqueId | MessageSummaryItems.Full);
    //    }

    //    folder.Close();
    //    return msgList;
    //}


    ///// <summary>
    ///// 根据唯一号查询信件
    ///// </summary>
    ///// <param name="mailFromAccount">邮箱账号</param>
    ///// <param name="mailPassword">邮箱密码</param>
    ///// <param name="id">唯一号</param>
    ///// <param name="folderName">文件夹名称</param>
    ///// <returns></returns>
    //public static MimeMessage GetEmailByUniqueId(string mailFromAccount, string mailPassword, uint id, string folderName)
    //{
    //    //打开收件箱
    //    var folder = OpenFolder(mailFromAccount, mailPassword, folderName);
    //    UniqueId emailUniqueId = new UniqueId(id);
    //    MimeMessage message = folder.GetMessage(emailUniqueId);
    //    /*将邮件设为已读*/
    //    MessageFlags flags = MessageFlags.Seen;
    //    folder.SetFlags(emailUniqueId, flags, true);
    //    return message;
    //}

    ///// <summary>
    ///// 读取上传的文件
    ///// </summary>
    ///// <returns></returns>
    //public static IList<MimePart> GetMimePartList()
    //{
    //    IList<MimePart> mimePartList = new List<MimePart>();
    //    var current = HttpContext.Current;
    //    if (current != null)
    //    {
    //        HttpRequest request = current.Request;
    //        HttpFileCollection files = request.Files;
    //        int filesCount = files.Count;
    //        for (int i = 0; i < filesCount; i++)
    //        {
    //            HttpPostedFile item = files[i];
    //            MimePart attachment = new MimePart(item.ContentType)
    //            {
    //                ContentObject = new ContentObject(item.InputStream, ContentEncoding.Default),
    //                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
    //                ContentTransferEncoding = ContentEncoding.Base64,
    //                FileName = item.FileName
    //            };

    //            mimePartList.Add(attachment);
    //        }
    //    }
    //    return mimePartList;
    //}


    ///// <summary>
    ///// 打开邮箱文件夹
    ///// </summary>
    ///// <param name="mailFromAccount">邮箱账号</param>
    ///// <param name="mailPassword">邮箱密码</param>
    ///// <param name="folderName">文件夹名称(INBOX:收件箱名称)</param>
    ///// <returns></returns>
    //public static IMailFolder OpenFolder(string mailFromAccount, string mailPassword, string folderName)
    //{
    //    ImapClient client = new ImapClient();
    //    client.Connect(ImapServer, ImapPort);
    //    client.Authenticate(mailFromAccount, mailPassword);
    //    //获取所有文件夹
    //    //List<IMailFolder> mailFolderList = client.GetFolders(client.PersonalNamespaces[0]).ToList();

    //    var folder = client.GetFolder(folderName);

    //    //打开文件夹并设置为读的方式
    //    folder.Open(MailKit.FolderAccess.ReadWrite);
    //    return folder;
    //}

    ///// <summary>
    ///// 下载邮件附件
    ///// </summary>
    ///// <param name="mimePart"></param>
    //public static void DownFile(MimePart mimePart)
    //{
    //    HttpContext context = HttpContext.Current;

    //    // 设置编码和附件格式
    //    context.Response.ContentType = mimePart.ContentType.ToString();
    //    //context.Response.ContentEncoding = Encoding.UTF8;

    //    context.Response.Charset = "";
    //    string fileName = HttpUtility.UrlEncode(mimePart.FileName, Encoding.UTF8);
    //    context.Response.AppendHeader("Content-Disposition",
    //        "attachment;filename=" + fileName);
    //    using (MemoryStream ms = new MemoryStream())
    //    {
    //        mimePart.ContentObject.DecodeTo(ms);
    //        ms.Flush();
    //        ms.Position = 0;
    //        context.Response.BinaryWrite(ms.GetBuffer());

    //        context.Response.End();
    //    }
    //}
}