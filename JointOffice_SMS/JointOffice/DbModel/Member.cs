using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.DbModel
{
    public class Member
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string LoginName { get; set; }
        [MaxLength(500)]
        public string LoginPwd { get; set; }
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 1没删除  0已删除
        /// </summary>
        public int IsDel { get; set; }
        /// <summary>
        /// 1启用  0禁用
        /// </summary>
        public int IsUse { get; set; }
    }
    public class Member_Token
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string Token { get; set; }
        /// <summary>
        /// 1有效  0失效
        /// </summary>
        public int Effective { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class Member_Token_New
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string Token { get; set; }
        /// <summary>
        /// 失效日期
        /// </summary>
        public string FailDate { get; set; }
    }
    public class Member_Team
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        public string TeamPerson { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }
    /// <summary>
    /// 群
    /// </summary>
    public class Member_Group
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        /// <summary>
        /// 群组创建人ID
        /// </summary>
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 群主ID
        /// </summary>
        [MaxLength(500)]
        public string GroupKingMemberId { get; set; }
        /// <summary>
        /// 群成员ID  List
        /// </summary>
        public string GroupPersonId { get; set; }
        /// <summary>
        /// 群名称
        /// </summary>
        [MaxLength(500)]
        public string Name { get; set; }
        /// <summary>
        /// 群头像
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 群创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 群状态
        /// </summary>
        public int State { get; set; }
    }
    public class Member_Info
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string Mobile { get; set; }
        /// <summary>
        /// 0保密  1男  2女
        /// </summary>
        public int Gender { get; set; }
        [MaxLength(500)]
        public string Mail { get; set; }
        [MaxLength(500)]
        public string ZhuBuMen { get; set; }
        [MaxLength(500)]
        public string FuBuMen { get; set; }
        [MaxLength(500)]
        public string HuiBaoDuiXiang { get; set; }
        [MaxLength(500)]
        public string BuMenFuZeRen { get; set; }
        [MaxLength(500)]
        public string WeChat { get; set; }
        [MaxLength(500)]
        public string QQ { get; set; }
        public string GongZuoJieShao { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string JobName { get; set; }
        [MaxLength(500)]
        public string Phone { get; set; }
        public string Picture { get; set; }
        public DateTime CreateDate { get; set; }
        public string JobID { get; set; }
        public int JobGrade { get; set; }
        public string MemberCode { get; set; }
        public string CompanyIDS { get; set; }
        public string QRCodeURL { get; set; }
        public string SMSLoginCode { get; set; }
        public string Roles { get; set; }
    }
    public class Member_Code
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string Mobile { get; set; }
        [MaxLength(500)]
        public string Code { get; set; }
        [MaxLength(500)]
        public string DiQuCode { get; set; }
        [MaxLength(500)]
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
    }
    /// <summary>
    /// 部门
    /// </summary>
    public class Member_Company
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string ParentId { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string FuZeRen { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class Logs
    {
        [Key]
        public int Id { get; set; }
        public string Exception { get; set; }
        public string Track { get; set; }
        [MaxLength(500)]
        public string Origin { get; set; }
        [MaxLength(500)]
        public string Createperson { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class Member_Model
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string Model { get; set; }
        [MaxLength(500)]
        public string Device { get; set; }
        [MaxLength(500)]
        public string Cid { get; set; }
        [MaxLength(500)]
        public string IP { get; set; }
        [MaxLength(500)]
        public string Type { get; set; }
        [MaxLength(500)]
        public string Token { get; set; }
        public int ShiFouKeYong { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class CountryCode
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string EnglishName { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string ShortName { get; set; }
        [MaxLength(500)]
        public string Code { get; set; }
    }
    public class Member_RYToken
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string Token { get; set; }
        public int Effective { get; set; }
        public string Type { get; set; }
    }
    public class Mail_Info
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string Mail { get; set; }
        [MaxLength(500)]
        public string Passwrod { get; set; }
        public DateTime LoginTime { get; set; }
        public int State { get; set; }
        [MaxLength(450)]
        public string Mid { get; set; }
    }
    public class System_Message
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
        public int ShiFouYiDu { get; set; }
        public string Code { get; set; }
        public string Params { get; set; }
    }
    public class Member_Contact
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        public string Contacts { get; set; }
    }
    public class Member_Job
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MemberCompanyId { get; set; }
        public string MemberDeptId { get; set; }
    }
    public class Member_Info_Company
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        /// <summary>
        /// 所在公司
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 所在部门
        /// </summary>
        public string DeptId { get; set; }
        /// <summary>
        /// 当前岗位
        /// </summary>
        public string JobId { get; set; }
        /// <summary>
        /// 部门负责人
        /// </summary>
        public string DeptMemberId { get; set; }
        /// <summary>
        /// 汇报对象
        /// </summary>
        public string HuiBaoDuiXiangId { get; set; }
        /// <summary>
        /// 工作介绍
        /// </summary>
        public string GongZuoJieShao { get; set; }
        public int Num { get; set; }
    }
    public class Member_Often
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string OtherMemberId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime WriteDate { get; set; }
    }
    /// <summary>
    /// 角色表
    /// </summary>
    public class Member_Role
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 是否删除  1是  0否
        /// </summary>
        public int IsDel { get; set; }
    }
    /// <summary>
    /// 角色权限表
    /// </summary>
    public class Member_Permission
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class Member_Other_System_Token
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        public string Token { get; set; }
        public string System { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? WriteDate { get; set; }
    }
}
