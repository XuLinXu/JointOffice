using JointOffice.Models;
using MailKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTool.MailKit
{
    /// <summary>
    /// 邮件内容实体
    /// </summary>
    public class MailBodyEntity
    {
        public string host { get; set; }
        /// <summary>
        /// 邮箱名
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string password { get; set; }
        ///// <summary>
        ///// 邮件文本内容
        ///// </summary>
        //public string MailTextBody { get; set; }

        /// <summary>
        /// 邮件内容类型
        /// </summary>
        public TextFormat MailBodyType { get; set; } = TextFormat.Html;

        /// <summary>
        /// 邮件附件集合
        /// </summary>
        public List<MailFile> MailFiles { get; set; } = new List<MailFile>();

        /// <summary>
        /// 邮件内容附件集合
        /// </summary>
        public List<MailFile> HtmlFiles { get; set; } = new List<MailFile>();
        /// <summary>
        /// 文件附件
        /// </summary>
        public List<FileList> fileList { get; set; } = new List<FileList>();
        /// <summary>
        /// 文件附件(转发)
        /// </summary>
        public List<FileList> oldFileList { get; set; } = new List<FileList>();

        /// <summary>
        /// 收件人
        /// </summary>
        public List<string> Recipients { get; set; } = new List<string>();

        /// <summary>
        /// 抄送
        /// </summary>
        public List<string> Cc { get; set; } = new List<string>();

        /// <summary>
        /// 密送
        /// </summary>
        public List<string> Bcc { get; set; } = new List<string>();

        /// <summary>
        /// 发件人
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 发件人地址
        /// </summary>
        public string SenderAddress { get; set; }

        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Body { get; set; }
        public int oldid { get; set; } = -1;
        public string folderType { get; set; }
        public bool reply { get; set; }
        public int draftId { get; set; } = -1;
    }

    public class FileList
    {
        public string name { get; set; }
        public string uuid { get; set; }
        public string url { get; set; }
        public string size { get; set; }
        public string file { get; set; }
        public string path { get; set; }
        public int index { get; set; }//附件位置
    }
    /// <summary>
    /// 邮件内容实体
    /// </summary>
    public class ReceiveMailBodyEntity
    {
        public string id { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public List<PersonBean> To { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 抄送
        /// </summary>
        public List<PersonBean> Cc { get; set; }

        /// <summary>
        /// 密送
        /// </summary>
        public List<PersonBean> Bcc { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// 发件人地址
        /// </summary>
        public string SenderAddress { get; set; }

        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// 邮件类型
        /// </summary>
        public string FolderType { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<AttachmentBean> Attachments { get; set; } = new List<AttachmentBean>();

        /// <summary>
        /// 附件数量
        /// </summary>
        public int AttaCount { get; set; }

        /// <summary>
        /// 加星
        /// </summary>
        public bool Star { get; set; }

        /// <summary>
        /// 已读、回复
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 邮箱未读数量
        /// </summary>
        public int unread { get; set; }

    }
    public class NewIMReceiveMailBodyEntity
    {
        public int id { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string Tostr { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 抄送
        /// </summary>
        public string Cc { get; set; }

        /// <summary>
        /// 密送
        /// </summary>
        public string Bcc { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public string Fromstr { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// 发件人地址
        /// </summary>
        public string SenderAddress { get; set; }

        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// 邮件类型
        /// </summary>
        public string FolderType { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<NewAttachmentBean> Attachments { get; set; } = new List<NewAttachmentBean>();

        /// <summary>
        /// 附件数量
        /// </summary>
        public int AttaCount { get; set; }

        /// <summary>
        /// 加星
        /// </summary>
        public bool Star { get; set; }

        /// <summary>
        /// 已读、回复
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 邮箱未读数量
        /// </summary>
        public int unread { get; set; }
        /// <summary>
        /// 二进制
        /// </summary>
        public byte[] rich { get; set; }

    }



    /// <summary>
    /// 发送人，接收人，抄送人，密送人 对象
    /// </summary>
    public class PersonBean
    {   
        /// <summary>
        /// 邮件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Mail { get; set; }
    }

    /// <summary>
    /// 删除，加星
    /// </summary>
    public class EmailIds
    {
        /// <summary>
        /// 邮箱类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// id 集合
        /// </summary>
        public List<uint> idArr { get; set; }
        /// <summary>
        /// 状态值
        /// </summary>
        public bool value { get; set; } = true;
        /// <summary>
        /// 标识代码 1=已读 2=已回复 8=删除 4=加星
        /// </summary>
        public int flag { get; set; }
    }
    /// <summary>
    /// 邮件附件对象
    /// </summary>
    public class NewAttachmentBean
    {   /// <summary>
        /// 3.附件名(只在AttaList里的对象有值)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性位置
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// 附件大小(只在AttaList里的对象有值)
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 附件流(只在AttaList里的对象有值)
        /// </summary>
        public Stream AttaStream { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }
    }
    /// <summary>
    /// 邮件附件对象
    /// </summary>
    public class AttachmentBean
    {   /// <summary>
        /// 3.附件名(只在AttaList里的对象有值)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性位置
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// 附件大小(只在AttaList里的对象有值)
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 附件流(只在AttaList里的对象有值)
        /// </summary>
        public Stream AttaStream { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }
    }

    public class MailFile
    {
        /// <summary>
        /// 邮件附件文件类型 例如：图片 MailFileType="image"
        /// </summary>
        public string MailFileType { get; set; }

        /// <summary>
        /// 邮件附件文件子类型 例如：图片 MailFileSubType="png"
        /// </summary>
        public string MailFileSubType { get; set; }

        /// <summary>
        /// 邮件附件文件路径  例如：图片 MailFilePath=@"C:\Files\123.png"
        /// </summary>
        public string MailFilePath { get; set; }
        public string cid { get; set; }
        public string filename { get; set; }
    }

    public class EmailById
    {
        public string host { get; set; }
        /// <summary>
        /// 邮箱类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// id 集合
        /// </summary>
        public uint id { get; set; }
        /// <summary>
        /// 发送类型 ： 恢复、转发、正常发送
        /// </summary>
        public bool falg { get; set; }
    }
    public class ImEmailById
    {
        /// <summary>
        /// 邮件id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 附件id
        /// </summary>
        public string attrid { get; set; }
        public string host { get; set; }
        public string token { get; set; }

    }
    /// <summary>
    /// 下载附件
    /// </summary>
    public class AttaById
    {
        public string host { get; set; }
        /// <summary>
        /// 邮箱类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// id 集合
        /// </summary>
        public uint id { get; set; }
        /// <summary>
        /// id 集合
        /// </summary>
        public uint attrid { get; set; }
    }
    /// <summary>
    /// 下载附件para
    /// </summary>
    public class downloadPara
    {
        public string host { get; set; }
        /// <summary>
        /// 邮箱类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// id 集合
        /// </summary>
        public string index { get; set; }
        /// <summary>
        /// id 集合
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// id 集合
        /// </summary>
        public byte[] content { get; set; }
    }
    /// <summary>
    /// 返回附件路径
    /// </summary>
    public class AttaFile
    {
        public string path { get; set; }
    }
    /// <summary>
    /// 邮件服务器基础信息
    /// </summary>
    public class MailServerInformation
    {
        /// <summary>
        /// SMTP服务器支持SASL机制类型
        /// </summary>
        public bool Authentication { get; set; }

        /// <summary>
        /// SMTP服务器对消息的大小
        /// </summary>
        public uint Size { get; set; }

        /// <summary>
        /// SMTP服务器支持传递状态通知
        /// </summary>
        public bool Dsn { get; set; }

        /// <summary>
        /// SMTP服务器支持Content-Transfer-Encoding
        /// </summary>
        public bool EightBitMime { get; set; }

        /// <summary>
        /// SMTP服务器支持Content-Transfer-Encoding
        /// </summary>
        public bool BinaryMime { get; set; }

        /// <summary>
        /// SMTP服务器在消息头中支持UTF-8
        /// </summary>
        public string UTF8 { get; set; }
    }

    /// <summary>
    /// 邮件发送结果
    /// </summary>
    public class SendResultEntity
    {
        /// <summary>
        /// 结果信息
        /// </summary>
        public string ResultInformation { get; set; } = "发送成功！";

        /// <summary>
        /// 结果状态
        /// </summary>
        public bool ResultStatus { get; set; } = true;
    }
    /// <summary>
    /// 邮件保存成功
    /// </summary>
    public class SaveResultEntity
    {
        /// <summary>
        /// 结果信息
        /// </summary>
        public string ResultInformation { get; set; } = "保存成功！";

        /// <summary>
        /// 结果状态
        /// </summary>
        public bool ResultStatus { get; set; } = true;

        /// <summary>
        /// 保存ID
        /// </summary>
        public int id { get; set; }
    }

    /// <summary>
    /// 邮件发送服务器配置
    /// </summary>
    public class SendServerConfigurationEntity
    {
        /// <summary>
        /// 邮箱SMTP服务器地址
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// 邮箱SMTP服务器端口
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// 是否启用IsSsl
        /// </summary>
        public bool IsSsl { get; set; }

        /// <summary>
        /// 邮件编码
        /// </summary>
        public string MailEncoding { get; set; }

        /// <summary>
        /// 邮箱账号
        /// </summary>
        public string SenderAccount { get; set; }

        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string SenderPassword { get; set; }

    }

    /// <summary>
    /// 邮件发送服务器配置
    /// </summary>
    public class ReceiveServerConfigurationEntity
    {
        /// <summary>
        /// 邮箱SMTP服务器地址
        /// </summary>
        public string ImapHost { get; set; }

        /// <summary>
        /// 邮箱SMTP服务器端口
        /// </summary>
        public int ImapPort { get; set; }

        /// <summary>
        /// 邮箱账号
        /// </summary>
        public string SenderAccount { get; set; }

        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string SenderPassword { get; set; }

    }

    /// <summary>
    /// 登录入参
    /// </summary>
    public class EmailLoginBean
    {
        /// <summary>
        /// 邮箱类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 邮箱名
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 主题搜索
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 状态：0：未读；1：已读；2：已回复
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string sort { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string order { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 条数
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string receiveName { get; set; }
    }
    /// <summary>
    /// 登录返回参数
    /// </summary>
    public class EmailResult
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string email { get; set; }
        /// <summary>
        /// 返回未读数量
        /// </summary>
        public int unread { get; set; }
    }
    public class EmailInfo
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class UnReadPara
    {
        public string email { get; set; }
        public string mailtoken { get; set; }
        public string id { get; set; }
    }
    public class UnReadBean
    {
        public int number { get; set; }
        public string mailtoken { get; set; } = "";
        public List<NewsRemindNum> NewsRemindNumList { get; set; }
    }
    public class MailResult
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public int count { get; set; } = 0;
    }

    ///// <summary>
    ///// 邮件内容实体
    ///// </summary>
    //public class MailBodyEntity
    //{
    //    /// <summary>
    //    /// 邮件文本内容
    //    /// </summary>
    //    public string MailTextBody { get; set; }

    //    /// <summary>
    //    /// 邮件内容类型
    //    /// </summary>
    //    public string MailBodyType { get; set; }

    //    /// <summary>
    //    /// 邮件附件文件类型
    //    /// </summary>
    //    public string MailFileType { get; set; }

    //    /// <summary>
    //    /// 邮件附件文件子类型
    //    /// </summary>
    //    public string MailFileSubType { get; set; }

    //    /// <summary>
    //    /// 邮件附件文件路径
    //    /// </summary>
    //    public string MailFilePath { get; set; }

    //    /// <summary>
    //    /// 收件人
    //    /// </summary>
    //    public List<string> Recipients { get; set; }

    //    /// <summary>
    //    /// 抄送
    //    /// </summary>
    //    public List<string> Cc { get; set; }

    //    /// <summary>
    //    /// 发件人
    //    /// </summary>
    //    public string Sender { get; set; }

    //    /// <summary>
    //    /// 发件人地址
    //    /// </summary>
    //    public string SenderAddress { get; set; }

    //    /// <summary>
    //    /// 邮件主题
    //    /// </summary>
    //    public string Subject { get; set; }

    //    /// <summary>
    //    /// 邮件内容
    //    /// </summary>
    //    public string Body { get; set; }
    //}

    ///// <summary>
    ///// 邮件服务器基础信息
    ///// </summary>
    //public class MailServerInformation
    //{
    //    /// <summary>
    //    /// SMTP服务器支持SASL机制类型
    //    /// </summary>
    //    public bool Authentication { get; set; }

    //    /// <summary>
    //    /// SMTP服务器对消息的大小
    //    /// </summary>
    //    public uint Size { get; set; }

    //    /// <summary>
    //    /// SMTP服务器支持传递状态通知
    //    /// </summary>
    //    public bool Dsn { get; set; }

    //    /// <summary>
    //    /// SMTP服务器支持Content-Transfer-Encoding
    //    /// </summary>
    //    public bool EightBitMime { get; set; }

    //    /// <summary>
    //    /// SMTP服务器支持Content-Transfer-Encoding
    //    /// </summary>
    //    public bool BinaryMime { get; set; }

    //    /// <summary>
    //    /// SMTP服务器在消息头中支持UTF-8
    //    /// </summary>
    //    public string UTF8 { get; set; }
    //}

    ///// <summary>
    ///// 邮件发送结果
    ///// </summary>
    //public class SendResultEntity
    //{
    //    /// <summary>
    //    /// 结果信息
    //    /// </summary>
    //    public string ResultInformation { get; set; } = "发送成功！";

    //    /// <summary>
    //    /// 结果状态
    //    /// </summary>
    //    public bool ResultStatus { get; set; } = true;
    //}

    ///// <summary>
    ///// 邮件发送服务器配置
    ///// </summary>
    //public class SendServerConfigurationEntity
    //{
    //    /// <summary>
    //    /// 邮箱SMTP服务器地址
    //    /// </summary>
    //    public string SmtpHost { get; set; }

    //    /// <summary>
    //    /// 邮箱SMTP服务器端口
    //    /// </summary>
    //    public int SmtpPort { get; set; }

    //    /// <summary>
    //    /// 是否启用IsSsl
    //    /// </summary>
    //    public bool IsSsl { get; set; }

    //    /// <summary>
    //    /// 邮件编码
    //    /// </summary>
    //    public string MailEncoding { get; set; }

    //    /// <summary>
    //    /// 发件人账号
    //    /// </summary>
    //    public string SenderAccount { get; set; }

    //    /// <summary>
    //    /// 发件人密码
    //    /// </summary>
    //    public string SenderPassword { get; set; }

    //}
}