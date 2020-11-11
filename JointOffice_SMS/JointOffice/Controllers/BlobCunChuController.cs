using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.Extensions.Options;
using JointOffice.Models;
using System.IO;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using JointOffice.Core;
using Microsoft.AspNetCore.Http;

namespace JointOffice.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class BlobCunChuController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        private readonly IPrincipalBase _PrincipalBase;
        IOptions<Root> config;
        ExceptionMessage em;
        CloudBlobClient blobClient = null;
        private readonly IBlobCunChu _IBlobCunChu;
        string SasKey;
        //private readonly IVerification _IVerification;
        public BlobCunChuController(IOptions<Root> config, IPrincipalBase IPrincipalBase, IBlobCunChu IBlobCunChu, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _IBlobCunChu = IBlobCunChu;
            this.config = config;
            //    _IVerification = IVerification;
            em = new ExceptionMessage(_JointOfficeContext);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.config.Value.ConnectionStrings.StorageConnectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            SasKey = this.config.Value.ConnectionStrings.SasKey;
            _PrincipalBase = IPrincipalBase;
        }
        //[HttpPost("SendVerificationCode")]
        //public ActionResult Upload(string name, HttpPostedFileBase[] files)
        //{
        //    if (files == null)
        //    {
        //        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        //    }
        //    CloudBlobContainer container = blobClient.GetContainerReference(name);
        //    foreach (HttpPostedFileBase item in files)
        //    {
        //        string filename = Guid.NewGuid().ToString() + "." + item.FileName.Split('.').Last();
        //        CloudBlockBlob blob = container.GetBlockBlobReference(filename);
        //        blob.UploadFromStreamAsync(item.InputStream);
        //    }
        //    return RedirectToAction("List", new { name = name });
        //}

        //[HttpPost("Index")]
        //public ActionResult Index()
        //{
        //    BlobContinuationToken currentToken = new BlobContinuationToken();


        //    //currentToken.NextMarker
        //    var containes = blobClient.ListContainersSegmentedAsync(currentToken);

        //    return View(containes);
        //}
        /// <summary>
        /// 创建新容器
        /// </summary>
        /// <param name="容器名"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public Showapi_Res_Meaasge Create([FromBody]string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    throw new BusinessException("请确认名称填写.");
                }
                return _IBlobCunChu.Create(Name);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }

        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("ShangChuanBlob")]
        public Showapi_Res_Meaasge ShangChuanBlob()
        {
            try
            {
                var wenJianJiaId = string.IsNullOrEmpty(Request.Form["wenJianJiaId"]) ? "" : Request.Form["wenJianJiaId"].ToString();
                if (string.IsNullOrEmpty(wenJianJiaId))
                {
                    throw new BusinessException("请确认名称和文件夹Id填写.");
                }
                var files = Request.Form.Files;
                CloudBlobContainer container = blobClient.GetContainerReference("spjointoffice");
                var memberid = _PrincipalBase.GetMemberId();
                var name = "";

                if(wenJianJiaId=="0")
                {
                    var OneMenu = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == "0" && t.MemberId == memberid).FirstOrDefault();
                    if (OneMenu != null)
                    {
                        name = _JointOfficeContext.WangPan_Member.Where(t => t.MemberId == memberid).FirstOrDefault().Name;
                        wenJianJiaId = OneMenu.Id;
                    }
                }
                else
                {
                    var OneMenu = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == wenJianJiaId).FirstOrDefault();
                    if (OneMenu != null)
                    {
                        name = _JointOfficeContext.WangPan_Member.Where(t => t.MemberId == memberid).FirstOrDefault().Name;
                    }
                }
               
                var WangPan_QiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == wenJianJiaId).FirstOrDefault();
                if (WangPan_QiYeMenu != null)
                {
                    name = WangPan_QiYeMenu.TeamId;
                }
                var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == wenJianJiaId).FirstOrDefault();
                if (WangPan_GongXiangMenu != null)
                {
                    if(WangPan_GongXiangMenu.ParentId=="0")
                    {
                        name = WangPan_GongXiangMenu.Id;
                    }
                    else
                    {
                        var WangPan_GongXiangMenuqqq = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == WangPan_GongXiangMenu.Uid ).FirstOrDefault();
                        name = WangPan_GongXiangMenuqqq.Id;
                    }
                }
                CloudBlobHelper.DeleteDicFiles();
                var blobFiles = CloudBlobHelper.SelectBlobFiles(files, name);
                List<filepara> list = new List<filepara>();
                CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);
                foreach (var item in blobFiles)
                {
                    filepara filepara = new filepara();
                    filepara.filename = item.fileYName;
                    filepara.url = item.fileurl;
                    filepara.wenJianJiaId = wenJianJiaId;
                    filepara.name= name;
                    filepara.type = item.filetype;
                    filepara.length = item.filelength;
                    list.Add(filepara);
                }
                return _IBlobCunChu.ShangChuanBlob(list);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <param name="文件夹名，文件夹ID"></param>
        /// <returns></returns>
        [HttpPost("XinJianWenJianJia")]
        public Showapi_Res_Meaasge XinJianWenJianJia([FromBody]XinJianWenJianJiaPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.wenJianJiaName)|| string.IsNullOrEmpty(para.wenJianJiaId))
                {
                    throw new BusinessException("请确认我的文件名,文件夹Id都填写.");
                }
                return _IBlobCunChu.XinJianWenJianJia(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        ////新建共享文件夹
        //[HttpPost("XinJianGongXiangWenJianJia")]
        //public Showapi_Res_Meaasge XinJianGongXiangWenJianJia([FromBody]XinJianGongXiangWenJianJiaPara para)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(para.wenJianJiaName) || string.IsNullOrEmpty(para.teamid) || string.IsNullOrEmpty(para.wenJianJiaId))
        //        {
        //            throw new BusinessException("请确认文件夹名,文件夹Id和团队名都填写.");
        //        }
        //        return _IBlobCunChu.XinJianGongXiangWenJianJia(para);
        //    }
        //    catch (Exception ex)
        //    {
        //        return em.ReturnMeaasge(ex);
        //    }
        //}
        ////新建企业文件夹
        //[HttpPost("XinJianQiYeWenJianJia")]
        //public Showapi_Res_Meaasge XinJianQiYeWenJianJia([FromBody]XinJianQiYeWenJianJiaPara para)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(para.wenJianJiaName)|| string.IsNullOrEmpty(para.wenJianJiaId))
        //        {
        //            throw new BusinessException("请确认文件夹名,文件夹Id都填写.");
        //        }
        //        return _IBlobCunChu.XinJianQiYeWenJianJia(para);
        //    }
        //    catch (Exception ex)
        //    {
        //        return em.ReturnMeaasge(ex);
        //    }
        //}
        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="文件夹ID，文件夹名，文件类型"></param>
        /// <returns></returns>
        [HttpPost("RenameWenJianJia")]
        public Showapi_Res_Meaasge RenameWenJianJia([FromBody]RenameWenJianJiaPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.wenJianJiaId)||string.IsNullOrEmpty(para.wenJianJiaName))
                {
                    throw new BusinessException("请确认文件夹名或文件名填写.");
                }
                return _IBlobCunChu.RenameWenJianJia(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="文件夹ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteWenJianJia")]
        public Showapi_Res_Meaasge DeleteWenJianJia([FromBody]DeleteWenJianJiaPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.wenJianJiaId))
                {
                    throw new BusinessException("请确认文件夹Id填写.");
                }
                return _IBlobCunChu.DeleteWenJianJia(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="文件夹ID，文件类型"></param>
        /// <returns></returns>
        [HttpPost("DeleteBlob")]
        public Showapi_Res_Meaasge DeleteBlob([FromBody]List<DeletePara> para)
        {
            try
            {
                foreach (var item in para)
                {
                    if ( item.type==0 ||  string.IsNullOrEmpty(item.wenJianJiaId))
                    {
                        throw new BusinessException("请确认文件类型和文件夹Id填写.");
                    }
                }
                return _IBlobCunChu.DeleteBlob(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 获取我的网盘页面信息
        /// </summary>
        /// <param name="页数，总数，文件夹ID，顺序，文件类型"></param>
        /// <returns></returns>
        [HttpPost("WangPanList")]
        public Showapi_Res_List<ListPara> WangPanList([FromBody]WangPanListPara para)
        {
            Showapi_Res_List<ListPara> res = new Showapi_Res_List<ListPara>();
            try
            {
                if (string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString())  || string.IsNullOrEmpty(para.shunxu) || string.IsNullOrEmpty(para.type.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IBlobCunChu.WangPanList(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        //public async Task<List<ListPara>> ListAsync([FromBody] string name)
        //{
        //    //CloudBlobContainer container = blobClient.GetContainerReference(name);
        //    //BlobContinuationToken currentToken = new BlobContinuationToken();
        //    //var result = container.ListBlobsSegmentedAsync(currentToken).Where(x => x.GetType() == typeof(CloudBlockBlob)).Cast<CloudBlockBlob>();
        //    //return View(result);
        //    CloudBlobContainer container = blobClient.GetContainerReference("jointoffice");
        //    BlobContinuationToken currentToken = new BlobContinuationToken();
        //    BlobContinuationToken continuationToken = null;
        //    var memberid = _PrincipalBase.GetMemberId();

        //    //var WangPan_Member = _JointOfficeContext.WangPan_Member.Where(t => t.MemberId == memberid).FirstOrDefault();
        //    //var name = WangPan_Member.Name;
        //    //Loop over items within the container and output the length and URI.
        //    var resultSegment = await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 10, continuationToken, null, null);

        //    List<ListPara> ListParaList = new List<ListPara>();
        //    foreach (IListBlobItem item in resultSegment.Results)
        //    {
        //        if (item.GetType() == typeof(CloudBlockBlob))
        //        {
        //            CloudBlockBlob blob = (CloudBlockBlob)item;
        //            string[] b = blob.Uri.ToString().Split(new Char[] { '/' });
        //            if (b[4]==name)
        //            {
        //                ListPara ListPara = new ListPara();
        //                ListPara.length = blob.Properties.Length.ToString() + "KB";
        //                ListPara.name = blob.Name;
        //                ListPara.url = blob.Uri.ToString() + SasKey;
        //                ListPara.blobtype = blob.BlobType.ToString();
        //                ListPara.date = blob.Properties.LastModified.Value.DateTime.AddHours(8).ToString();
        //                ListParaList.Add(ListPara);
        //            }
        //        }
        //        else if (item.GetType() == typeof(CloudPageBlob))
        //        {
        //            CloudPageBlob pageBlob = (CloudPageBlob)item;
        //            string[] b = pageBlob.Uri.ToString().Split(new Char[] { '/' });
        //            if (b[4] == name)
        //            {
        //                ListPara ListPara = new ListPara();
        //                ListPara.length = pageBlob.Properties.Length.ToString() + "KB";
        //                ListPara.name = pageBlob.Name;
        //                ListPara.url = pageBlob.Uri.ToString() + SasKey;
        //                ListPara.date = pageBlob.Properties.LastModified.Value.DateTime.AddHours(8).ToString();
        //                ListParaList.Add(ListPara);
        //            }

        //        }
        //        else if (item.GetType() == typeof(CloudBlobDirectory))
        //        {
        //            CloudBlobDirectory directory = (CloudBlobDirectory)item;

        //            ListPara ListPara = new ListPara();
        //            ListPara.url = directory.Uri.ToString();
        //            ListParaList.Add(ListPara);
        //        }
        //    }
        //    return ListParaList;
        //}
        /// <summary>
        /// 移动文件或文件夹
        /// </summary>
        /// <param name="之前文件夹，现在文件夹，文件或文件夹List"></param>
        /// <returns></returns>
        [HttpPost("MoveWenJian")]
        public Showapi_Res_Meaasge MoveWenJian([FromBody]MoveWenJianJiaPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.oldwenJianJiaId) || string.IsNullOrEmpty(para.wenJianJiaId) || string.IsNullOrEmpty(para.MovePara.Count().ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                if (para.oldwenJianJiaId==para.wenJianJiaId)
                {
                    throw new BusinessException("该文件已在当前文件夹.");
                }
                return _IBlobCunChu.MoveWenJian(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 获取我的网盘所有文件夹
        /// </summary>
        /// <param name="页数，总数，文件夹ID，顺序，文件类型"></param>
        /// <returns></returns>
        [HttpPost("GetAllWangPanList")]
        public Showapi_Res_List<filelist> GetAllWangPanList([FromBody]WangPanListPara para)
        {
            Showapi_Res_List<filelist> res = new Showapi_Res_List<filelist>();
            try
            {
                if (string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()) || string.IsNullOrEmpty(para.shunxu) || string.IsNullOrEmpty(para.type.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IBlobCunChu.GetAllWangPanList(para);
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
        /// 获取共享文件所有文件夹
        /// </summary>
        /// <param name="页数，总数，文件夹ID，顺序，文件类型"></param>
        /// <returns></returns>
        [HttpPost("GetGongXiangWangPanList")]
        public Showapi_Res_List<filelist> GetGongXiangWangPanList([FromBody]WangPanListPara para)
        {
            Showapi_Res_List<filelist> res = new Showapi_Res_List<filelist>();
            try
            {
                if (string.IsNullOrEmpty(para.page.ToString()) || string.IsNullOrEmpty(para.count.ToString()) || string.IsNullOrEmpty(para.shunxu) || string.IsNullOrEmpty(para.type.ToString()))
                {
                    throw new BusinessException("参数不正确。");
                }
                return _IBlobCunChu.GetGongXiangWangPanList(para);
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
        /// 上传附件
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost("PostAttachment")]
        public AttachmentUpload PostAttachment()
        {
            AttachmentUpload attachmentUpload = new AttachmentUpload();
            try
            {
                string host = "";
                if (Request.IsHttps)
                {
                    host = "https://" + Request.Host.Value;
                }
                else
                {
                    host = "http://" + Request.Host.Value;
                }
                var files = Request.Form.Files;
                attachmentUpload = BusinessHelper.FileSave(files,host).Result;
                return attachmentUpload;

            }catch(Exception ex)
            {
                attachmentUpload.status = false;
                attachmentUpload.message = ex.Message;
                return attachmentUpload;
            }
        }
    }
}
