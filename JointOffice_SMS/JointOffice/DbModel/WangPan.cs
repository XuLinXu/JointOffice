using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.DbModel
{
    public class WangPan_ZuiJin
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string WenJianId { get; set; }
        [MaxLength(500)]
        public string DiZhi { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Length { get; set; }
        public int type { get; set; }
        public string Url { get; set; }
        public DateTime SeeDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class WangPan_Member
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class WangPan_Jurisdiction
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public int IsTrue{ get; set; }
    }
    public class WangPan_FileJiLu
    {
        [Key]
        [MaxLength(450)]        
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        //分组Id
        public string Tid { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string FileName { get; set; }
        [MaxLength(500)]
        //主文件夹Id
        public string Uid { get; set; }
        [MaxLength(500)]
        //文件Id
        public string WenJianId { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
        public int BlobType { get; set; }
        public long Length { get; set; }
        public DateTime CreateDate { get; set; }
        public int IsDelete { get; set; }
    }
    public class WangPan_Menu
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string ParentId { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class WangPan_GongXiangMenu
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string TeamId { get; set; }
        [MaxLength(500)]
        public string ChuanJian { get; set; }
        public string GuanLi { get; set; }
        public string ShangChuan { get; set; }
        public string ChaKan { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string ParentId { get; set; }
        [MaxLength(500)]
        public string Uid { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class WangPan_QiYeMenu
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string TeamId { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string ParentId { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class WangPan_QiYeWenJian
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string UId { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        public long length { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string FileName { get; set; }
        [MaxLength(500)]
        public string MenuId { get; set; }
        public int type { get; set; }
        public string url { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class WangPan_GongXiangWenJian
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string UId { get; set; }
        [MaxLength(500)]
        public string FileName { get; set; }
        public long length { get; set; }
        [MaxLength(500)]
        public string MenuId { get; set; }
        public int type { get; set; }
        public string url { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class WangPan_WenJian
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string UId { get; set; }
        [MaxLength(500)]
        public string MenuId { get; set; }
        [MaxLength(500)]
        public string FileName { get; set; }
        public long length { get; set; }
        public int type { get; set; }
        public string url { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class WangPan_Download
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string MemberId { get; set; }
        [MaxLength(500)]
        public string Url { get; set; }
        [MaxLength(500)]
        public string FileName { get; set; }
        [MaxLength(500)]
        public string Length { get; set; }
        [MaxLength(500)]
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
