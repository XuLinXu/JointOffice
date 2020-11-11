using JointOffice.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.Configuration;
using Microsoft.Extensions.Options;
using JointOffice.DbModel;

namespace JointOffice.Controllers
{
    [Route("api/[controller]")]
    public class WangPanController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        private readonly IWangPan _IWangPan;
        ExceptionMessage em;
        IOptions<Root> config;
        public WangPanController(IOptions<Root> config, IWangPan IWangPan, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _IWangPan = IWangPan;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
        }
        /// <summary>
        /// 访问文件
        /// </summary>
        /// <param name="文件夹ID"></param>
        /// <returns></returns>
        [HttpPost("FangWenWenJian")]
        public Showapi_Res_Meaasge FangWenWenJian([FromBody]FangWenWenJianPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.wenJianJiaId) || string.IsNullOrEmpty(para.length))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IWangPan.FangWenWenJian(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 最近文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetZuiJinWenJianList")]
        public Showapi_Res_List<ZuiJinWenJianList> GetZuiJinWenJianList()
        {
            Showapi_Res_List<ZuiJinWenJianList> res = new Showapi_Res_List<ZuiJinWenJianList>();
            try
            {
                return _IWangPan.GetZuiJinWenJianList();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }       
        /// <summary>
        /// 我的网盘
        /// </summary>
        /// <returns></returns>
        [HttpPost("WoDeWangPan")]
        public Showapi_Res_Single<WoDeWangPanList> WoDeWangPan()
        {
            Showapi_Res_Single<WoDeWangPanList> res = new Showapi_Res_Single<WoDeWangPanList>();
            try
            {
                return _IWangPan.WoDeWangPan();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 文件动态
        /// </summary>
        /// <param name="页数，总数"></param>
        /// <returns></returns>
        [HttpPost("GetWenJianDongTaiList")]
        public Showapi_Res_List<WenJianDongTaiList> GetWenJianDongTaiList([FromBody]GetWenJianDongTaiListPara para)
        {
            Showapi_Res_List<WenJianDongTaiList> res = new Showapi_Res_List<WenJianDongTaiList>();
            try
            {
                if (string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IWangPan.GetWenJianDongTaiList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 修改共享文件夹权限
        /// </summary>
        /// <param name="文件夹ID，文件类型，个人IDList"></param>
        /// <returns></returns>
        [HttpPost("UpdateGongXiangQuanXian")]
        public Showapi_Res_Meaasge UpdateGongXiangQuanXian([FromBody]UpdateGongXiangQuanXianPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.wenJianJiaId) || string.IsNullOrEmpty(para.type.ToString()) || string.IsNullOrEmpty(para.shiFouDelete.ToString()) || string.IsNullOrEmpty(para.memberidlist.Count().ToString()))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IWangPan.UpdateGongXiangQuanXian(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 查看共享文件夹的权限人数
        /// </summary>
        /// <param name="文件夹ID"></param>
        /// <returns></returns>
        [HttpPost("ChaKanGongXiangQuanXian")]
        public Showapi_Res_Single<GongXiangQuanXian> ChaKanGongXiangQuanXian([FromBody]ChaKanGongXiangQuanXianPara para)
        {
            Showapi_Res_Single<GongXiangQuanXian> res = new Showapi_Res_Single<GongXiangQuanXian>();
            try
            {
                if (string.IsNullOrEmpty(para.wenJianJiaId))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IWangPan.ChaKanGongXiangQuanXian(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 查看共享文件夹的权限人员
        /// </summary>
        /// <param name="文件夹ID，文件类型"></param>
        /// <returns></returns>
        [HttpPost("GongXiangQuanXianRenYuan")]
        public Showapi_Res_List<GongXiangQuanXianRenYuanList> GongXiangQuanXianRenYuan([FromBody]GongXiangQuanXianRenYuanPara para)
        {
            Showapi_Res_List<GongXiangQuanXianRenYuanList> res = new Showapi_Res_List<GongXiangQuanXianRenYuanList>();
            try
            {
                if (string.IsNullOrEmpty(para.wenJianJiaId) || string.IsNullOrEmpty(para.type.ToString()))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IWangPan.GongXiangQuanXianRenYuan(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 获取共享文件列表
        /// </summary>
        /// <param name="顺序"></param>
        /// <returns></returns>
        [HttpPost("GetGongXiangWenJianList")]
        public Showapi_Res_List<GongXiangWenJianList> GetGongXiangWenJianList([FromBody]GetGongXiangWenJianListPara para)
        {
            Showapi_Res_List<GongXiangWenJianList> res = new Showapi_Res_List<GongXiangWenJianList>();
            try
            {
                if (string.IsNullOrEmpty(para.shunxu))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IWangPan.GetGongXiangWenJianList( para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 获取企业文件列表
        /// </summary>
        /// <param name="文件夹ID，顺序"></param>
        /// <returns></returns>
        [HttpPost("GetQiYeWenJianList")]
        public Showapi_Res_List<QiYeWenJianList> GetQiYeWenJianList([FromBody]GetQiYeWenJianListPara para)
        {
            Showapi_Res_List<QiYeWenJianList> res = new Showapi_Res_List<QiYeWenJianList>();
            try
            {
                if (string.IsNullOrEmpty(para.wenJianJiaId) || string.IsNullOrEmpty(para.shunxu))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IWangPan.GetQiYeWenJianList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }       
        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetBuMenList")]
        public Showapi_Res_List<BuMenList> GetBuMenList()
        {
            Showapi_Res_List<BuMenList> res = new Showapi_Res_List<BuMenList>();
            try
            {
                return _IWangPan.GetBuMenList();
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 全局搜索
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllList")]
        public Showapi_Res_List<AllList> GetAllList([FromBody]GetAllListPara para)
        {
            Showapi_Res_List<AllList> res = new Showapi_Res_List<AllList>();
            try
            {
                if (string.IsNullOrEmpty(para.name) || string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IWangPan.GetAllList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="文件夹ID"></param>
        /// <returns></returns>
        [HttpPost("DownloadBlob")]
        public Showapi_Res_Meaasge DownloadBlob([FromBody]List<DownloadBlobPara> para)
        {
            try
            {
                foreach (var item in para)
                {
                    if (string.IsNullOrEmpty(item.url) || string.IsNullOrEmpty(item.name) || string.IsNullOrEmpty(item.type) || string.IsNullOrEmpty(item.length))
                    {
                        throw new BusinessException("参数不正确.");
                    }
                }
                return _IWangPan.DownloadBlob(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 下载列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDownloadBlobList")]
        public Showapi_Res_List<GetDownloadBlobList> GetDownloadBlobList([FromBody]GetDownloadBlobListPara para)
        {
            Showapi_Res_List<GetDownloadBlobList> res = new Showapi_Res_List<GetDownloadBlobList>();
            try
            {
                if (string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IWangPan.GetDownloadBlobList(para);
            }
            catch (Exception ex)
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                em.XieLogs(ex);
                return res;
            }
        }
        /// <summary>
        /// 下载列表删除记录
        /// </summary>
        /// <param name="文件夹ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteDownloadBlobList")]
        public Showapi_Res_Meaasge DeleteDownloadBlobList([FromBody]List<DeleteDownloadBlobListPara> para)
        {
            try
            {
                foreach (var item in para)
                {
                    if (string.IsNullOrEmpty(item.id))
                    {
                        throw new BusinessException("参数不正确.");
                    }
                }
                return _IWangPan.DeleteDownloadBlobList(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 重新保存共享文件夹权限
        /// </summary>
        /// <param name="文件夹ID，文件类型，个人IDList"></param>
        /// <returns></returns>
        [HttpPost("SaveGongXiangQuanXian")]
        public Showapi_Res_Meaasge SaveGongXiangQuanXian([FromBody]SaveGongXiangQuanXianPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.wenJianJiaId) || string.IsNullOrEmpty(para.type.ToString()) || string.IsNullOrEmpty(para.memberidlist.Count().ToString()))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _IWangPan.SaveGongXiangQuanXian(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
    }
}
