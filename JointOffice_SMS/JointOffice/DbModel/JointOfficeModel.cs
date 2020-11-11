using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.DbModel
{
    public class JointOfficeContext : DbContext
    {

        public JointOfficeContext(DbContextOptions<JointOfficeContext> options)
    : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region
            ////主键
            //modelBuilder.Entity<TRsServiceOrder>().HasKey("TRsServiceOrderId");
            ////索引
            //modelBuilder.Entity<TRsServiceOrder>().HasIndex(p => p.TRsQuoteOrderId);
            ////唯一约束
            //modelBuilder.Entity<TRsServiceOrder>().HasIndex(p => p.TRsQuoteOrderId).IsUnique();
            ////字段长度
            //modelBuilder.Entity<TRsServiceOrder>().HasIndex(p => p.TRsQuoteOrderId).HasAnnotation("MaxLength", 450);
            ////关系
            //modelBuilder.Entity<TRsAttachment>()
            //    .HasOne(p => p.Inquiryorder)
            //    .WithMany(p => p.TRsAttachments)
            //    .HasForeignKey(t => t.TRsInquiryOrderId);

            #endregion
        }
        public DbSet<Member> Member { get; set; }
        public DbSet<Member_Token> Member_Token { get; set; }
        public DbSet<Member_RYToken> Member_RYToken { get; set; }
        public DbSet<Member_Token_New> Member_Token_New { get; set; }
        public DbSet<Member_Code> Member_Code { get; set; }
        public DbSet<Member_Info> Member_Info { get; set; }
        public DbSet<Member_Company> Member_Company { get; set; }
        public DbSet<Member_Team> Member_Team { get; set; }
        public DbSet<Member_Group> Member_Group { get; set; }
        public DbSet<Member_Model> Member_Model { get; set; }
        public DbSet<Member_Contact> Member_Contact { get; set; }
        public DbSet<Member_Job> Member_Job { get; set; }
        public DbSet<Member_Info_Company> Member_Info_Company { get; set; }
        public DbSet<Member_Often> Member_Often { get; set; }
        public DbSet<Member_Role> Member_Role { get; set; }
        public DbSet<Member_Permission> Member_Permission { get; set; }
        public DbSet<Member_Other_System_Token> Member_Other_System_Token { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<WangPan_ZuiJin> WangPan_ZuiJin { get; set; }
        public DbSet<WangPan_Member> WangPan_Member { get; set; }
        public DbSet<WangPan_Jurisdiction> WangPan_Jurisdiction { get; set; }
        public DbSet<WangPan_FileJiLu> WangPan_FileJiLu { get; set; }
        public DbSet<WangPan_Menu> WangPan_Menu { get; set; }
        public DbSet<WangPan_QiYeMenu> WangPan_QiYeMenu { get; set; }
        public DbSet<WangPan_WenJian> WangPan_WenJian { get; set; }
        public DbSet<WangPan_GongXiangMenu> WangPan_GongXiangMenu { get; set; }
        public DbSet<WangPan_GongXiangWenJian> WangPan_GongXiangWenJian { get; set; }
        public DbSet<WangPan_QiYeWenJian> WangPan_QiYeWenJian { get; set; }
        public DbSet<WangPan_Download> WangPan_Download { get; set; }
        public DbSet<Contacts_Star> Contacts_Star { get; set; }
        public DbSet<Work_Log> Work_Log { get; set; }
        public DbSet<Work_Task> Work_Task { get; set; }
        public DbSet<Work_Program> Work_Program { get; set; }
        public DbSet<Work_Order> Work_Order { get; set; }
        public DbSet<Work_Approval> Work_Approval { get; set; }
        public DbSet<Work_Announcement> Work_Announcement { get; set; }
        public DbSet<Work_Share> Work_Share { get; set; }
        public DbSet<WorkListTag> WorkListTag { get; set; }
        public DbSet<Receipts> Receipts { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<Approval_Content> Approval_Content { get; set; }
        public DbSet<Execute_Content> Execute_Content { get; set; }
        public DbSet<Comment_Body> Comment_Body { get; set; }
        public DbSet<DianPing_Body> DianPing_Body { get; set; }
        public DbSet<EscProgram> EscProgram { get; set; }
        public DbSet<TotalNum> TotalNum { get; set; }
        public DbSet<Agree> Agree { get; set; }
        public DbSet<News_News> News_News { get; set; }
        public DbSet<News_GroupNotice> News_GroupNotice { get; set; }
        public DbSet<News_GroupNotice_Content> News_GroupNotice_Content { get; set; }
        public DbSet<News_Collection> News_Collection { get; set; }
        public DbSet<News_Mark> News_Mark { get; set; }
        public DbSet<News_Focus> News_Focus { get; set; }
        public DbSet<Attendance_Check> Attendance_Check { get; set; }
        public DbSet<Remark> Remark { get; set; }
        public DbSet<CountryCode> CountryCode { get; set; }
        public DbSet<News_Member> News_Member { get; set; }
        public DbSet<Mail_Info> Mail_Info { get; set; }
        public DbSet<System_Message> System_Message { get; set; }
        public DbSet<WF_WorkFlowTemplate> WF_WorkFlowTemplate { get; set; }
        public DbSet<WF_WorkFlowInstance> WF_WorkFlowInstance { get; set; }
        public DbSet<WF_WorkFlowInstanceDetial> WF_WorkFlowInstanceDetial { get; set; }
    }
}
