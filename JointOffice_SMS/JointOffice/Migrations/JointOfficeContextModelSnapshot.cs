using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using JointOffice.DbModel;

namespace JointOffice.Migrations
{
    [DbContext(typeof(JointOfficeContext))]
    partial class JointOfficeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JointOffice.DbModel.Agree", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Body");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsRead");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("OtherMemberId");

                    b.Property<string>("PId");

                    b.Property<string>("P_UId");

                    b.Property<string>("PhoneModel");

                    b.Property<string>("Type");

                    b.Property<string>("UId");

                    b.HasKey("Id");

                    b.ToTable("Agree");
                });

            modelBuilder.Entity("JointOffice.DbModel.Approval_Content", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ApprovalTime");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("IsMeApproval");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("OtherMemberId")
                        .HasMaxLength(500);

                    b.Property<string>("OtherMemberName")
                        .HasMaxLength(500);

                    b.Property<int>("OtherMemberOrder");

                    b.Property<string>("OtherMemberPicture");

                    b.Property<string>("PhoneModel");

                    b.Property<string>("Picture");

                    b.Property<int>("State");

                    b.Property<string>("Type");

                    b.Property<string>("UId")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.ToTable("Approval_Content");
                });

            modelBuilder.Entity("JointOffice.DbModel.Attendance_Check", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Address");

                    b.Property<DateTime>("CheckDate");

                    b.Property<string>("Map");

                    b.Property<int>("Mark");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("OffWorkTime");

                    b.Property<string>("Remarks");

                    b.Property<string>("RemarksTime");

                    b.Property<string>("ToWorkTime");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Attendance_Check");
                });

            modelBuilder.Entity("JointOffice.DbModel.Comment_Body", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ATPerson");

                    b.Property<string>("Annex");

                    b.Property<string>("Body");

                    b.Property<int>("IsExeComment");

                    b.Property<bool>("IsRead");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("OtherBody");

                    b.Property<string>("PId")
                        .HasMaxLength(450);

                    b.Property<string>("PersonId")
                        .HasMaxLength(500);

                    b.Property<string>("PersonName")
                        .HasMaxLength(500);

                    b.Property<string>("PhoneModel");

                    b.Property<string>("Picture");

                    b.Property<string>("PictureList");

                    b.Property<string>("PingLunMemberId")
                        .HasMaxLength(500);

                    b.Property<DateTime>("PingLunTime");

                    b.Property<string>("Type");

                    b.Property<string>("UId")
                        .HasMaxLength(450);

                    b.Property<string>("Voice");

                    b.Property<string>("VoiceLength");

                    b.HasKey("Id");

                    b.ToTable("Comment_Body");
                });

            modelBuilder.Entity("JointOffice.DbModel.Contacts_Star", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("IsStar");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("OtherMemberId")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("Contacts_Star");
                });

            modelBuilder.Entity("JointOffice.DbModel.CountryCode", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Code")
                        .HasMaxLength(500);

                    b.Property<string>("EnglishName")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("ShortName")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("CountryCode");
                });

            modelBuilder.Entity("JointOffice.DbModel.DianPing_Body", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ATPerson");

                    b.Property<string>("Annex");

                    b.Property<string>("Body");

                    b.Property<string>("DianPingMemberId")
                        .HasMaxLength(500);

                    b.Property<DateTime>("DianPingTime");

                    b.Property<string>("Grade");

                    b.Property<bool>("IsRead");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("PhoneModel");

                    b.Property<string>("Picture");

                    b.Property<string>("PictureList");

                    b.Property<int>("State");

                    b.Property<string>("Type");

                    b.Property<string>("UId")
                        .HasMaxLength(450);

                    b.Property<string>("Voice");

                    b.Property<string>("VoiceLength");

                    b.HasKey("Id");

                    b.ToTable("DianPing_Body");
                });

            modelBuilder.Entity("JointOffice.DbModel.EscProgram", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("AfterEscJoinPerson");

                    b.Property<string>("BeforeEscJoinPerson");

                    b.Property<DateTime>("EscTime");

                    b.Property<string>("JoinPersonMemberId")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("UId")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.ToTable("EscProgram");
                });

            modelBuilder.Entity("JointOffice.DbModel.Execute_Content", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("ExecuteDate");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("OtherMemberId")
                        .HasMaxLength(500);

                    b.Property<string>("OtherMemberName")
                        .HasMaxLength(500);

                    b.Property<string>("OtherMemberPicture");

                    b.Property<string>("PhoneModel");

                    b.Property<int>("State");

                    b.Property<int>("Type");

                    b.Property<string>("UId")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.ToTable("Execute_Content");
                });

            modelBuilder.Entity("JointOffice.DbModel.File", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("FileName")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Type")
                        .HasMaxLength(500);

                    b.Property<string>("UId")
                        .HasMaxLength(450);

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("File");
                });

            modelBuilder.Entity("JointOffice.DbModel.Logs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Createperson")
                        .HasMaxLength(500);

                    b.Property<string>("Exception");

                    b.Property<string>("Origin")
                        .HasMaxLength(500);

                    b.Property<string>("Track");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("JointOffice.DbModel.Mail_Info", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("LoginTime");

                    b.Property<string>("Mail")
                        .HasMaxLength(500);

                    b.Property<string>("Mid")
                        .HasMaxLength(450);

                    b.Property<string>("Passwrod")
                        .HasMaxLength(500);

                    b.Property<int>("State");

                    b.HasKey("Id");

                    b.ToTable("Mail_Info");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("IsDel");

                    b.Property<int>("IsUse");

                    b.Property<string>("LoginName")
                        .HasMaxLength(500);

                    b.Property<string>("LoginPwd")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("Member");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Code", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Code")
                        .HasMaxLength(500);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("DiQuCode")
                        .HasMaxLength(500);

                    b.Property<string>("Mobile")
                        .HasMaxLength(500);

                    b.Property<string>("Type")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("Member_Code");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Company", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("FuZeRen")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("ParentId")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("Member_Company");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Contact", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Contacts");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("Member_Contact");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Group", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("GroupKingMemberId")
                        .HasMaxLength(500);

                    b.Property<string>("GroupPersonId");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("Picture");

                    b.Property<int>("State");

                    b.HasKey("Id");

                    b.ToTable("Member_Group");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Info", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("BuMenFuZeRen")
                        .HasMaxLength(500);

                    b.Property<string>("CompanyIDS");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("FuBuMen")
                        .HasMaxLength(500);

                    b.Property<int>("Gender");

                    b.Property<string>("GongZuoJieShao");

                    b.Property<string>("HuiBaoDuiXiang")
                        .HasMaxLength(500);

                    b.Property<int>("JobGrade");

                    b.Property<string>("JobID");

                    b.Property<string>("JobName")
                        .HasMaxLength(500);

                    b.Property<string>("Mail")
                        .HasMaxLength(500);

                    b.Property<string>("MemberCode");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Mobile")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("Phone")
                        .HasMaxLength(500);

                    b.Property<string>("Picture");

                    b.Property<string>("QQ")
                        .HasMaxLength(500);

                    b.Property<string>("QRCodeURL");

                    b.Property<string>("Roles");

                    b.Property<string>("SMSLoginCode");

                    b.Property<string>("WeChat")
                        .HasMaxLength(500);

                    b.Property<string>("ZhuBuMen")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("Member_Info");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Info_Company", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("CompanyId");

                    b.Property<string>("DeptId");

                    b.Property<string>("DeptMemberId");

                    b.Property<string>("GongZuoJieShao");

                    b.Property<string>("HuiBaoDuiXiangId");

                    b.Property<string>("JobId");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<int>("Num");

                    b.HasKey("Id");

                    b.ToTable("Member_Info_Company");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Job", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Code");

                    b.Property<string>("MemberCompanyId");

                    b.Property<string>("MemberDeptId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Member_Job");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Model", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Cid")
                        .HasMaxLength(500);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Device")
                        .HasMaxLength(500);

                    b.Property<string>("IP")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Model")
                        .HasMaxLength(500);

                    b.Property<int>("ShiFouKeYong");

                    b.Property<string>("Token")
                        .HasMaxLength(500);

                    b.Property<string>("Type")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("Member_Model");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Often", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("OtherMemberId")
                        .HasMaxLength(500);

                    b.Property<DateTime>("WriteDate");

                    b.HasKey("Id");

                    b.ToTable("Member_Often");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Other_System_Token", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("System");

                    b.Property<string>("Token");

                    b.Property<DateTime?>("WriteDate");

                    b.HasKey("Id");

                    b.ToTable("Member_Other_System_Token");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Permission", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Member_Permission");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<int>("IsDel");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Member_Role");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_RYToken", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<int>("Effective");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Token")
                        .HasMaxLength(500);

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Member_RYToken");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Team", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("TeamPerson");

                    b.HasKey("Id");

                    b.ToTable("Member_Team");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Token", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("Effective");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Token")
                        .HasMaxLength(500);

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Member_Token");
                });

            modelBuilder.Entity("JointOffice.DbModel.Member_Token_New", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("FailDate");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Token")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("Member_Token_New");
                });

            modelBuilder.Entity("JointOffice.DbModel.News_Collection", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Body");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("MarkInfo");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("OtherMemberID")
                        .HasMaxLength(500);

                    b.Property<string>("Type");

                    b.Property<string>("UId")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.ToTable("News_Collection");
                });

            modelBuilder.Entity("JointOffice.DbModel.News_Focus", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Type");

                    b.Property<string>("UId")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.ToTable("News_Focus");
                });

            modelBuilder.Entity("JointOffice.DbModel.News_GroupNotice", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Annex");

                    b.Property<string>("AtPerson");

                    b.Property<string>("Body");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("IsConfirm");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("PhoneModel");

                    b.Property<string>("Picture");

                    b.Property<string>("Range");

                    b.Property<string>("Title");

                    b.Property<string>("Voice");

                    b.Property<string>("VoiceLength");

                    b.HasKey("Id");

                    b.ToTable("News_GroupNotice");
                });

            modelBuilder.Entity("JointOffice.DbModel.News_GroupNotice_Content", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ConfirmTime");

                    b.Property<string>("GroupNoticeId")
                        .HasMaxLength(450);

                    b.Property<string>("IfYesIsConfirm");

                    b.Property<string>("IsConfirm");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("News_GroupNotice_Content");
                });

            modelBuilder.Entity("JointOffice.DbModel.News_Mark", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("MarkUId");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("News_Mark");
                });

            modelBuilder.Entity("JointOffice.DbModel.News_Member", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("DeleteGroupPersonId");

                    b.Property<string>("GroupId");

                    b.Property<string>("GroupPersonId");

                    b.Property<string>("MemberId");

                    b.Property<string>("WeiDuGroupPersonId");

                    b.HasKey("Id");

                    b.ToTable("News_Member");
                });

            modelBuilder.Entity("JointOffice.DbModel.News_News", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Address");

                    b.Property<string>("BaseUrl");

                    b.Property<string>("Body");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("FileName");

                    b.Property<string>("GroupId");

                    b.Property<string>("InfoType");

                    b.Property<string>("Length");

                    b.Property<string>("Map");

                    b.Property<string>("NewsSenderId")
                        .HasMaxLength(500);

                    b.Property<string>("NoSeePerson");

                    b.Property<string>("SeePerson");

                    b.Property<string>("Time");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("News_News");
                });

            modelBuilder.Entity("JointOffice.DbModel.Receipts", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Body");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("OtherMemberId");

                    b.Property<string>("PhoneModel");

                    b.Property<int>("Type");

                    b.Property<string>("UId");

                    b.HasKey("Id");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("JointOffice.DbModel.Remark", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("CheckDate");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Remarks");

                    b.Property<DateTime>("RemarksTime");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Remark");
                });

            modelBuilder.Entity("JointOffice.DbModel.System_Message", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Code");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Message");

                    b.Property<string>("Params");

                    b.Property<DateTime>("SendDate");

                    b.Property<int>("ShiFouYiDu");

                    b.Property<string>("Title");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("System_Message");
                });

            modelBuilder.Entity("JointOffice.DbModel.TotalNum", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("DianZanNum");

                    b.Property<string>("PId");

                    b.Property<string>("P_UId");

                    b.Property<int>("PingLunNum");

                    b.Property<string>("Type");

                    b.Property<string>("UId");

                    b.Property<int>("ZhuanFaNum");

                    b.HasKey("Id");

                    b.ToTable("TotalNum");
                });

            modelBuilder.Entity("JointOffice.DbModel.WangPan_Download", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("FileName")
                        .HasMaxLength(500);

                    b.Property<string>("Length")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Type")
                        .HasMaxLength(500);

                    b.Property<string>("Url")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("WangPan_Download");
                });

            modelBuilder.Entity("JointOffice.DbModel.WangPan_FileJiLu", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<int>("BlobType");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("FileName")
                        .HasMaxLength(500);

                    b.Property<int>("IsDelete");

                    b.Property<long>("Length");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("Tid")
                        .HasMaxLength(500);

                    b.Property<int>("Type");

                    b.Property<string>("Uid")
                        .HasMaxLength(500);

                    b.Property<string>("Url");

                    b.Property<string>("WenJianId")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("WangPan_FileJiLu");
                });

            modelBuilder.Entity("JointOffice.DbModel.WangPan_GongXiangMenu", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ChaKan");

                    b.Property<string>("ChuanJian")
                        .HasMaxLength(500);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("GuanLi");

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("ParentId")
                        .HasMaxLength(500);

                    b.Property<string>("ShangChuan");

                    b.Property<string>("TeamId")
                        .HasMaxLength(500);

                    b.Property<string>("Uid")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("WangPan_GongXiangMenu");
                });

            modelBuilder.Entity("JointOffice.DbModel.WangPan_GongXiangWenJian", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("FileName")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("MenuId")
                        .HasMaxLength(500);

                    b.Property<string>("UId")
                        .HasMaxLength(500);

                    b.Property<long>("length");

                    b.Property<int>("type");

                    b.Property<string>("url");

                    b.HasKey("Id");

                    b.ToTable("WangPan_GongXiangWenJian");
                });

            modelBuilder.Entity("JointOffice.DbModel.WangPan_Jurisdiction", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("IsTrue");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("WangPan_Jurisdiction");
                });

            modelBuilder.Entity("JointOffice.DbModel.WangPan_Member", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("WangPan_Member");
                });

            modelBuilder.Entity("JointOffice.DbModel.WangPan_Menu", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("ParentId")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("WangPan_Menu");
                });

            modelBuilder.Entity("JointOffice.DbModel.WangPan_QiYeMenu", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("ParentId")
                        .HasMaxLength(500);

                    b.Property<string>("TeamId")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("WangPan_QiYeMenu");
                });

            modelBuilder.Entity("JointOffice.DbModel.WangPan_QiYeWenJian", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("FileName")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("MenuId")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("UId")
                        .HasMaxLength(500);

                    b.Property<long>("length");

                    b.Property<int>("type");

                    b.Property<string>("url");

                    b.HasKey("Id");

                    b.ToTable("WangPan_QiYeWenJian");
                });

            modelBuilder.Entity("JointOffice.DbModel.WangPan_WenJian", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("FileName")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("MenuId")
                        .HasMaxLength(500);

                    b.Property<string>("UId")
                        .HasMaxLength(500);

                    b.Property<long>("length");

                    b.Property<int>("type");

                    b.Property<string>("url");

                    b.HasKey("Id");

                    b.ToTable("WangPan_WenJian");
                });

            modelBuilder.Entity("JointOffice.DbModel.WangPan_ZuiJin", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("DiZhi")
                        .HasMaxLength(500);

                    b.Property<string>("Length")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<DateTime>("SeeDate");

                    b.Property<string>("Url");

                    b.Property<string>("WenJianId")
                        .HasMaxLength(500);

                    b.Property<int>("type");

                    b.HasKey("Id");

                    b.ToTable("WangPan_ZuiJin");
                });

            modelBuilder.Entity("JointOffice.DbModel.WF_WorkFlowInstance", b =>
                {
                    b.Property<string>("WF_InstanceID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("Creater_Job");

                    b.Property<byte[]>("TST");

                    b.Property<string>("WF_CurrentSetp");

                    b.Property<int>("WF_CurrentSetpState");

                    b.Property<string>("WF_CurrentSetp_AuditJobs");

                    b.Property<string>("WF_CurrentSetp_AuditUserId");

                    b.Property<string>("WF_CurrentStepMiaoShu");

                    b.Property<int>("WF_InstanceState");

                    b.Property<string>("WF_ObjectId");

                    b.Property<string>("WF_SourceObject");

                    b.Property<string>("WF_TemplateID");

                    b.Property<int>("active_flag");

                    b.Property<string>("creater");

                    b.Property<DateTime>("createtime");

                    b.Property<int>("delete_flag");

                    b.Property<DateTime>("modefytime");

                    b.Property<string>("modifyer");

                    b.HasKey("WF_InstanceID");

                    b.ToTable("WF_WorkFlowInstance");
                });

            modelBuilder.Entity("JointOffice.DbModel.WF_WorkFlowInstanceDetial", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("WF_AuditContent");

                    b.Property<string>("WF_AuditJobName");

                    b.Property<short>("WF_AuditState");

                    b.Property<DateTime>("WF_AuditTime");

                    b.Property<string>("WF_AuditUserName");

                    b.Property<string>("WF_InstanceID");

                    b.Property<string>("WF_SetpID");

                    b.Property<string>("creater");

                    b.Property<DateTime>("createtime");

                    b.Property<DateTime>("modefytime");

                    b.Property<string>("modifyer");

                    b.HasKey("ID");

                    b.ToTable("WF_WorkFlowInstanceDetial");
                });

            modelBuilder.Entity("JointOffice.DbModel.WF_WorkFlowTemplate", b =>
                {
                    b.Property<string>("WF_TemplateID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("WF_DESC");

                    b.Property<string>("WF_Name");

                    b.Property<string>("WF_TemplateCategoryCode");

                    b.Property<byte[]>("WF_TemplateContent");

                    b.Property<int>("WF_Version");

                    b.Property<int>("active_flag");

                    b.Property<string>("creater");

                    b.Property<DateTime>("createtime");

                    b.Property<int>("delete_flag");

                    b.Property<bool>("isDelete");

                    b.Property<DateTime>("modefytime");

                    b.Property<string>("modifyer");

                    b.HasKey("WF_TemplateID");

                    b.ToTable("WF_WorkFlowTemplate");
                });

            modelBuilder.Entity("JointOffice.DbModel.Work_Announcement", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ATPerson");

                    b.Property<string>("Address")
                        .HasMaxLength(500);

                    b.Property<string>("Annex");

                    b.Property<DateTime>("BeginTime");

                    b.Property<string>("Body");

                    b.Property<string>("CompanyId");

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("IsDraft");

                    b.Property<string>("Map")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("PhoneModel");

                    b.Property<string>("Picture");

                    b.Property<string>("Range");

                    b.Property<string>("RangeNew");

                    b.Property<string>("Receipt");

                    b.Property<int>("State");

                    b.Property<DateTime>("StopTime");

                    b.Property<string>("Title");

                    b.Property<string>("Voice");

                    b.Property<string>("VoiceLength");

                    b.Property<string>("WangPanJson");

                    b.HasKey("Id");

                    b.ToTable("Work_Announcement");
                });

            modelBuilder.Entity("JointOffice.DbModel.Work_Approval", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ATPerson");

                    b.Property<string>("Address")
                        .HasMaxLength(500);

                    b.Property<string>("Annex");

                    b.Property<string>("ApprovalPerson");

                    b.Property<int>("ApprovalPersonNum");

                    b.Property<string>("Body");

                    b.Property<string>("Code");

                    b.Property<string>("CompanyId");

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("IsDraft");

                    b.Property<string>("Leave");

                    b.Property<string>("LeaveDuration");

                    b.Property<string>("Map")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("OverTime");

                    b.Property<string>("PhoneModel");

                    b.Property<string>("Picture");

                    b.Property<string>("Range");

                    b.Property<string>("RangeNew");

                    b.Property<string>("Reb");

                    b.Property<string>("RebMoney");

                    b.Property<int>("State");

                    b.Property<string>("Travel");

                    b.Property<string>("TravelAll");

                    b.Property<string>("TravelMoney");

                    b.Property<string>("TravelReb");

                    b.Property<string>("Type")
                        .HasMaxLength(500);

                    b.Property<string>("Voice");

                    b.Property<string>("VoiceLength");

                    b.Property<string>("WangPanJson");

                    b.Property<string>("WorkDuration");

                    b.HasKey("Id");

                    b.ToTable("Work_Approval");
                });

            modelBuilder.Entity("JointOffice.DbModel.Work_Log", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ATPerson");

                    b.Property<string>("Address")
                        .HasMaxLength(500);

                    b.Property<string>("Annex");

                    b.Property<string>("CompanyId");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Experience");

                    b.Property<int>("IsDraft");

                    b.Property<string>("Map")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("MoBan")
                        .HasMaxLength(500);

                    b.Property<string>("MoBanTime");

                    b.Property<string>("Money");

                    b.Property<string>("MoneyInfo");

                    b.Property<string>("PhoneModel");

                    b.Property<string>("Picture");

                    b.Property<string>("Range");

                    b.Property<string>("RangeNew");

                    b.Property<string>("Receipt");

                    b.Property<string>("ReviewPersonId")
                        .HasMaxLength(500);

                    b.Property<string>("ReviewPersonName")
                        .HasMaxLength(500);

                    b.Property<int>("State");

                    b.Property<int?>("TMSstate");

                    b.Property<string>("Voice");

                    b.Property<string>("VoiceLength");

                    b.Property<string>("WangPanJson");

                    b.Property<string>("WorkPlan");

                    b.Property<string>("WorkSummary");

                    b.HasKey("Id");

                    b.ToTable("Work_Log");
                });

            modelBuilder.Entity("JointOffice.DbModel.Work_Order", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ATPerson");

                    b.Property<string>("Address")
                        .HasMaxLength(500);

                    b.Property<string>("Annex");

                    b.Property<string>("Body");

                    b.Property<string>("CompanyId");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Executor");

                    b.Property<int>("IsDraft");

                    b.Property<string>("Map")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("PhoneModel");

                    b.Property<string>("Picture");

                    b.Property<string>("Range");

                    b.Property<string>("RangeNew");

                    b.Property<int>("State");

                    b.Property<string>("StopTime");

                    b.Property<string>("Voice");

                    b.Property<string>("VoiceLength");

                    b.Property<string>("WangPanJson");

                    b.HasKey("Id");

                    b.ToTable("Work_Order");
                });

            modelBuilder.Entity("JointOffice.DbModel.Work_Program", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ATPerson");

                    b.Property<string>("Address")
                        .HasMaxLength(500);

                    b.Property<string>("Annex");

                    b.Property<string>("Body");

                    b.Property<string>("CompanyId");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Hour")
                        .HasMaxLength(500);

                    b.Property<int>("IsDraft");

                    b.Property<string>("JoinPerson");

                    b.Property<string>("Map")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("PhoneModel");

                    b.Property<string>("Picture");

                    b.Property<string>("Range");

                    b.Property<string>("Receipt");

                    b.Property<string>("RemindTime");

                    b.Property<int>("State");

                    b.Property<string>("Voice");

                    b.Property<string>("VoiceLength");

                    b.Property<string>("WangPanJson");

                    b.Property<string>("Year")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("Work_Program");
                });

            modelBuilder.Entity("JointOffice.DbModel.Work_Share", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ATPerson");

                    b.Property<string>("Address")
                        .HasMaxLength(500);

                    b.Property<string>("Annex");

                    b.Property<string>("Body");

                    b.Property<string>("CompanyId");

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("IsDraft");

                    b.Property<string>("Map")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("PhoneModel");

                    b.Property<string>("Picture");

                    b.Property<string>("Range");

                    b.Property<string>("RangeNew");

                    b.Property<string>("Receipt");

                    b.Property<int>("State");

                    b.Property<string>("Voice");

                    b.Property<string>("VoiceLength");

                    b.Property<string>("WangPanJson");

                    b.HasKey("Id");

                    b.ToTable("Work_Share");
                });

            modelBuilder.Entity("JointOffice.DbModel.Work_Task", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ATPerson");

                    b.Property<string>("Address")
                        .HasMaxLength(500);

                    b.Property<string>("Annex");

                    b.Property<string>("CompanyId");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Executor");

                    b.Property<int>("IsDraft");

                    b.Property<string>("Map")
                        .HasMaxLength(500);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("PhoneModel");

                    b.Property<string>("Picture");

                    b.Property<string>("Range");

                    b.Property<string>("RangeNew");

                    b.Property<string>("Remarks");

                    b.Property<string>("RemindTime");

                    b.Property<int>("State");

                    b.Property<string>("StopTime");

                    b.Property<string>("TaskTitle")
                        .HasMaxLength(500);

                    b.Property<string>("Voice");

                    b.Property<string>("VoiceLength");

                    b.Property<string>("WangPanJson");

                    b.HasKey("Id");

                    b.ToTable("Work_Task");
                });

            modelBuilder.Entity("JointOffice.DbModel.WorkListTag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("MemberId")
                        .HasMaxLength(500);

                    b.Property<string>("WorkListTagDES");

                    b.HasKey("Id");

                    b.ToTable("WorkListTag");
                });
        }
    }
}
