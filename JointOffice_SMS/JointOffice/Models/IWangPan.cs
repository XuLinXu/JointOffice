using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public interface IWangPan
    {
        /// <summary>
        /// 访问文件
        /// </summary>
        /// <param name="文件夹ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge FangWenWenJian(FangWenWenJianPara para);
        /// <summary>
        /// 最近文件
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<ZuiJinWenJianList> GetZuiJinWenJianList();
        /// <summary>
        /// 我的网盘
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<WoDeWangPanList> WoDeWangPan();
        /// <summary>
        /// 文件动态
        /// </summary>
        /// <param name="页数，总数"></param>
        /// <returns></returns>
        Showapi_Res_List<WenJianDongTaiList> GetWenJianDongTaiList(GetWenJianDongTaiListPara para);
        /// <summary>
        /// 修改共享文件夹权限
        /// </summary>
        /// <param name="文件夹ID，文件类型，个人IDList"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdateGongXiangQuanXian(UpdateGongXiangQuanXianPara para);
        /// <summary>
        /// 查看共享文件夹的权限人数
        /// </summary>
        /// <param name="文件夹ID"></param>
        /// <returns></returns>
        Showapi_Res_Single<GongXiangQuanXian> ChaKanGongXiangQuanXian(ChaKanGongXiangQuanXianPara para);
        /// <summary>
        /// 查看共享文件夹的权限人员
        /// </summary>
        /// <param name="文件夹ID，文件类型"></param>
        /// <returns></returns>
        Showapi_Res_List<GongXiangQuanXianRenYuanList> GongXiangQuanXianRenYuan(GongXiangQuanXianRenYuanPara para);
        /// <summary>
        /// 获取共享文件列表
        /// </summary>
        /// <param name="顺序"></param>
        /// <returns></returns>
        Showapi_Res_List<GongXiangWenJianList> GetGongXiangWenJianList(GetGongXiangWenJianListPara para);
        /// <summary>
        /// 获取企业文件列表
        /// </summary>
        /// <param name="文件夹ID，顺序"></param>
        /// <returns></returns>
        Showapi_Res_List<QiYeWenJianList> GetQiYeWenJianList(GetQiYeWenJianListPara para);
        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<BuMenList> GetBuMenList();
        /// <summary>
        /// 网盘搜索
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<AllList> GetAllList(GetAllListPara para);
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge DownloadBlob(List<DownloadBlobPara> para);
        /// <summary>
        /// 下载列表
        /// </summary>
        /// <returns></returns>
        Showapi_Res_List<GetDownloadBlobList> GetDownloadBlobList(GetDownloadBlobListPara para);
        /// <summary>
        /// 下载列表删除记录
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge DeleteDownloadBlobList(List<DeleteDownloadBlobListPara> para);
        /// <summary>
        /// 重新保存共享文件夹权限
        /// </summary>
        /// <param name="文件夹ID，文件类型，个人IDList"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge SaveGongXiangQuanXian(SaveGongXiangQuanXianPara para);
    }
    public class DeleteDownloadBlobListPara
    {
        public string id { get; set; }
    }
    public class GetDownloadBlobListPara
    {
        public int count { get; set; }
        public int page { get; set; }
    }
    public class DownloadBlobPara
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 文件url
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string length { get; set; }
    }
    public class GetDownloadBlobList
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 文件url
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// 下载时间
        /// </summary>
        public string date { get; set; }
    }
    public class AllList
    {
        public string wenJianId { get; set; }
        public string name { get; set; }
        public string dizhi { get; set; }
        public string url { get; set; }
        /// <summary>
        /// 1 文件夹 2 文件
        /// </summary>
        public string type { get; set; }
        public string length { get; set; }
    }
    public class GetAllListPara
    {
        public int page { get; set; }
        public string name { get; set; }
        public int count { get; set; }
    }
    public class BuMenList
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public string teamid { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string name { get; set; }
    }
    public class QiYeWenJianList
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int  type { get; set; }
    }
    public class GetQiYeWenJianListPara
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public string shunxu { get; set; }
    }
    public class GetGongXiangWenJianListPara
    {
        /// <summary>
        /// 顺序
        /// </summary>
        public string shunxu { get; set; }
    }
    public class FangWenWenJianPara
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
        public string length { get; set; }
    }
    public class ZuiJinWenJianList
    {        
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dizhi { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string length { get; set; }
    }
    public class GetWenJianDongTaiListPara
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int count { get; set; }
    }
    public class GongXiangQuanXianRenYuanList
    {
        /// <summary>
        /// 个人ID
        /// </summary>
        public string membrid { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }
    public class GongXiangWenJianList
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
        public string blobtype { get; set; }
        
        /// <summary>
        /// 文件大小
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        public string renyuanname { get; set; }
        /// <summary>
        /// 群组名称
        /// </summary>
        public string qunzuname { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 管理权限 1 有 2 可上传 3 查看
        /// </summary>
        public int type { get; set; }

    }
    public class WenJianDongTaiList
    {
        public string person { get; set; }
        public string personurl { get; set; }
        public string name { get; set; }
        public string tid { get; set; }
        public int type { get; set; }

        public int qita { get; set; }
        public int count { get; set; }
        public string date { get; set; }
        public List<WenJianInfo>  list { get; set; }
    }
    public class ZuiJinWenJianInfo
    {
        public string tid { get; set; }
        public string date { get; set; }
    }
    public class WenJianInfo
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int isdelete { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int blobType { get; set; }
    }
    public class WangPanListPara
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
        //public int type { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public string shunxu { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int type { get; set; }
    }
}
