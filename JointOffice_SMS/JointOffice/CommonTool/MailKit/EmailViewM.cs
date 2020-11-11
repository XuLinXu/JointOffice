using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.CommonTool.MailKit
{
    public class EmailViewM
    {
        /// <summary>
        /// 1.从服务器上获取的邮件的UniqueId
        /// </summary>
        public uint UniqueId { get; set; }
        /// <summary>
        /// 1.发件人名字,这个名字可能为null.因为发件人可以不设名字
        /// 2.收件人名(只在ToList里的对象有值)
        /// 3.附件名(只在AttaList里的对象有值)
        /// 4.文件夹名字(只在FolderList里的对象有值)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 1.发件人地址
        /// 2.收件人地址(只在ToList里的对象有值)
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 发件人邮箱授权码
        /// </summary>
        public string AuthCode { get; set; }
        /// <summary>
        /// 收件人列表
        /// </summary>
        public List<EmailViewM> ToList { get; set; }
        /// <summary>
        /// 邮件主题(标题)
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 邮件时间
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 1.附件个数
        /// 2.文件夹内邮件个数(只在FolderList里的对象有值)
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 附件标识ID在保存附件在本地时设置(只在AttaList里的对象有值)
        /// 当附件从邮件服务器下载到本地后,需要向客户端提供下载时,用这个ID找到该附件.
        /// </summary>
        public string AttaGuid { get; set; }
        /// <summary>
        /// 附件大小(只在AttaList里的对象有值)
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 附件流(只在AttaList里的对象有值)
        /// </summary>
        public System.IO.Stream AttaStream { get; set; }
        /// <summary>
        /// 附件列表
        /// </summary>
        public List<EmailViewM> AttaList { get; set; }
        /// <summary>
        /// 是否已经读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 是否已经回复
        /// </summary>
        public bool IsAnswered { get; set; }
        /// <summary>
        /// 邮件正文的纯文本形式
        /// </summary>
        public string BodyText { get; set; }
        /// <summary>
        /// 邮件正文的HTML形式.
        /// </summary>
        public string BodyHTML { get; set; }

        /// <summary>
        /// 邮箱的文件夹列表
        /// </summary>
        public List<EmailViewM> FolderList { get; set; }
        /// <summary>
        /// 文件夹类型名
        /// 1.表示当前邮件所处文件夹名字
        /// 2.在FolderList里的对象,表示文件夹名字
        ///inbox(收件箱),
        ///archive(档案箱),
        ///drafts(草稿箱),
        ///flagged(标记的),
        ///junk(垃圾箱),
        ///sent(发件箱),
        ///trash(回收箱)
        /// </summary>
        public string FolderType { get; set; }
        /// <summary>
        /// 邮件标识,需要修改邮件标识时,传入此值
        /// 1=Seen(设为已读),
        /// 2=Answered(设为已经回复),
        /// 8=Deleted(设为删除),
        /// </summary>
        public int Flag { get; set; }
    }
}
