using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public interface IBlobCunChu
    {
        /// <summary>
        /// 创建新容器
        /// </summary>
        /// <param name="容器名"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge Create(string Name);
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="文件内容List"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge ShangChuanBlob(List<filepara> para);
        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <param name="文件夹名，文件夹ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge XinJianWenJianJia(XinJianWenJianJiaPara para);
        ////新建共享文件夹
        //Showapi_Res_Meaasge XinJianGongXiangWenJianJia(XinJianGongXiangWenJianJiaPara para);
        ////新建企业文件夹
        //Showapi_Res_Meaasge XinJianQiYeWenJianJia(XinJianQiYeWenJianJiaPara para);
        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="文件夹ID，文件夹名，文件类型"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge RenameWenJianJia(RenameWenJianJiaPara para);
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="文件夹ID"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge DeleteWenJianJia(DeleteWenJianJiaPara para);
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="文件夹ID，文件类型"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge DeleteBlob(List<DeletePara> para);
        /// <summary>
        /// 获取我的网盘页面信息
        /// </summary>
        /// <param name="页数，总数，文件夹ID，顺序，文件类型"></param>
        /// <returns></returns>
        Showapi_Res_List<ListPara> WangPanList(WangPanListPara para);
        /// <summary>
        /// 移动文件或文件夹
        /// </summary>
        /// <param name="之前文件夹，现在文件夹，文件或文件夹List"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge MoveWenJian(MoveWenJianJiaPara para);
        /// <summary>
        /// 获取我的网盘所有文件夹
        /// </summary>
        /// <param name="页数，总数，文件夹ID，顺序，文件类型"></param>
        /// <returns></returns>
        Showapi_Res_List<filelist> GetAllWangPanList(WangPanListPara para);
        /// <summary>
        /// 获取共享文件所有文件夹
        /// </summary>
        /// <param name="页数，总数，文件夹ID，顺序，文件类型"></param>
        /// <returns></returns>
        Showapi_Res_List<filelist> GetGongXiangWangPanList(WangPanListPara para);
    }
}
