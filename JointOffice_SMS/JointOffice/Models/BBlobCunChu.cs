using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public class BBlobCunChu : IBlobCunChu
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        private readonly IPrincipalBase _PrincipalBase;
        CloudBlobClient blobClient = null;
        string SasKey;
        public BBlobCunChu(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.config.Value.ConnectionStrings.StorageConnectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            SasKey = this.config.Value.ConnectionStrings.SasKey;

        }
        /// <summary>
        /// 创建新容器
        /// </summary>
        /// <param name="容器名"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge Create(string Name)
        {
            //CloudBlobClient cloudBlobClient = blobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("spjointoffice");
            //container.CreateAsync();
            //CloudBlobHelper CloudBlobHelper = new CloudBlobHelper();
            //var url = CloudBlobHelper.GetBlobSasUri(container, "005a84b8458b484c8981b1100eee0aa4/ceshi.txt");
            string sasBlobToken;

            // Get a reference to a blob within the container.
            // Note that the blob may not exist yet, but a SAS can still be created for it.
            CloudBlockBlob blob = container.GetBlockBlobReference(@"005a84b8458b484c8981b1100eee0aa4\ceshi.txt");
            SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
            {
                // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(5),
                Permissions = SharedAccessBlobPermissions.Read
            };

            // Generate the shared access signature on the blob, setting the constraints directly on the signature.
            sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);
            // Return the URI string for the container, including the SAS token.
            var url = blob.Uri + sasBlobToken;

            //CloudBlobContainer container = blobClient.GetContainerReference(Name);
            //container.CreateIfNotExistsAsync();
            //var memberid = _PrincipalBase.GetMemberId();
            //WangPan_Member WangPan_Member = new WangPan_Member();
            //WangPan_Member.Id = Guid.NewGuid().ToString();
            //WangPan_Member.MemberId = memberid;
            //WangPan_Member.Name = Name;
            //WangPan_Member.CreateDate = DateTime.Now;
            //_JointOfficeContext.WangPan_Member.Add(WangPan_Member);
            //_JointOfficeContext.SaveChanges();
            Message Message = new Message();
            return Message.SuccessMeaasge("创建成功");
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="文件内容List"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge ShangChuanBlob(List<filepara> para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var OneMenu = _JointOfficeContext.WangPan_Menu.Where(t => t.MemberId == memberid && t.ParentId == "0").FirstOrDefault();
            //var WangPan_QiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.MemberId == memberid && t.ParentId == "0").FirstOrDefault();
            //var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ChuanJian == memberid && (t.ParentId == "0" || t.ParentId == "1")).FirstOrDefault();
            var tid = Guid.NewGuid().ToString();
            var WangPan_Menu1 = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == para.FirstOrDefault().wenJianJiaId).FirstOrDefault();

            var WangPan_QiYeMenu1 = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == para.FirstOrDefault().wenJianJiaId).FirstOrDefault();
            var WangPan_GongXiangMenu1 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == para.FirstOrDefault().wenJianJiaId).FirstOrDefault();
            if (WangPan_Menu1 != null)
            {
                //var oldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Type == 1).OrderByDescending(t => t.CreateDate).FirstOrDefault();
                //if (oldWangPan_FileJiLu == null)
                //{
                foreach (var item in para)
                {


                    WangPan_WenJian WangPan_WenJian = new WangPan_WenJian();
                    WangPan_WenJian.Id = Guid.NewGuid().ToString();
                    WangPan_WenJian.MemberId = memberid;
                    WangPan_WenJian.url = item.url;
                    WangPan_WenJian.length = item.length;
                    WangPan_WenJian.FileName = item.filename;
                    WangPan_WenJian.MenuId = item.wenJianJiaId;
                    WangPan_WenJian.UId = OneMenu.Id;
                    WangPan_WenJian.type = item.type;
                    WangPan_WenJian.CreateDate = DateTime.Now;
                    _JointOfficeContext.WangPan_WenJian.Add(WangPan_WenJian);

                    var WangPan_FileJiLu = new WangPan_FileJiLu();
                    WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                    WangPan_FileJiLu.Tid = tid;
                    WangPan_FileJiLu.MemberId = memberid;
                    WangPan_FileJiLu.Name = "我的文件";
                    WangPan_FileJiLu.Type = 1;
                    WangPan_FileJiLu.Url = item.url;
                    WangPan_FileJiLu.Length = item.length;
                    WangPan_FileJiLu.BlobType = item.type;
                    WangPan_FileJiLu.Uid = OneMenu.Id;
                    WangPan_FileJiLu.FileName = item.filename;
                    WangPan_FileJiLu.CreateDate = DateTime.Now;
                    WangPan_FileJiLu.IsDelete = 0;
                    WangPan_FileJiLu.WenJianId = WangPan_WenJian.Id;
                    _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);
                }
                //}
                //else
                //{
                //    if (oldWangPan_FileJiLu.MemberId == memberid && oldWangPan_FileJiLu.Uid == OneMenu.Id)
                //    {
                //        foreach (var item in para)
                //        {
                //            var WangPan_FileJiLu = new WangPan_FileJiLu();
                //            WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                //            WangPan_FileJiLu.Tid = oldWangPan_FileJiLu.Tid;
                //            WangPan_FileJiLu.MemberId = memberid;
                //            WangPan_FileJiLu.Name = "我的文件";
                //            WangPan_FileJiLu.Type = 1;
                //            WangPan_FileJiLu.Url = item.url;
                //            WangPan_FileJiLu.Length = item.length;
                //            WangPan_FileJiLu.Uid = OneMenu.Id;
                //            WangPan_FileJiLu.FileName = item.filename;
                //            WangPan_FileJiLu.CreateDate = DateTime.Now;
                //            WangPan_FileJiLu.IsDelete = 0;
                //            _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);

                //            WangPan_WenJian WangPan_WenJian = new WangPan_WenJian();
                //            WangPan_WenJian.Id = Guid.NewGuid().ToString();
                //            WangPan_WenJian.MemberId = memberid;
                //            WangPan_WenJian.url = item.url;
                //            WangPan_WenJian.length = item.length;
                //            WangPan_WenJian.FileName = item.filename;
                //            WangPan_WenJian.MenuId = item.wenJianJiaId;
                //            WangPan_WenJian.UId = OneMenu.Id;
                //            WangPan_WenJian.type = item.type;
                //            WangPan_WenJian.CreateDate = DateTime.Now;
                //            _JointOfficeContext.WangPan_WenJian.Add(WangPan_WenJian);
                //        }
                //    }
                //    else
                //    {
                //        foreach (var item in para)
                //        {
                //            var WangPan_FileJiLu = new WangPan_FileJiLu();
                //            WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                //            WangPan_FileJiLu.Tid = tid;
                //            WangPan_FileJiLu.MemberId = memberid;
                //            WangPan_FileJiLu.Name = "我的文件";
                //            WangPan_FileJiLu.Type = 1;
                //            WangPan_FileJiLu.Url = item.url;
                //            WangPan_FileJiLu.FileName = item.filename;
                //            WangPan_FileJiLu.Uid = OneMenu.Id;
                //            WangPan_FileJiLu.Length = item.length;
                //            WangPan_FileJiLu.CreateDate = DateTime.Now;
                //            WangPan_FileJiLu.IsDelete = 0;
                //            _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);

                //            WangPan_WenJian WangPan_WenJian = new WangPan_WenJian();
                //            WangPan_WenJian.Id = Guid.NewGuid().ToString();
                //            WangPan_WenJian.MemberId = memberid;
                //            WangPan_WenJian.url = item.url;
                //            WangPan_WenJian.FileName = item.filename;
                //            WangPan_WenJian.MenuId = item.wenJianJiaId;
                //            WangPan_WenJian.UId = OneMenu.Id;
                //            WangPan_WenJian.type = item.type;
                //            WangPan_WenJian.length = item.length;
                //            WangPan_WenJian.CreateDate = DateTime.Now;
                //            _JointOfficeContext.WangPan_WenJian.Add(WangPan_WenJian);
                //        }

                //    }
                //}
            }
            else if (WangPan_QiYeMenu1 != null)
            {
                //var oldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Type == 1).OrderByDescending(t => t.CreateDate).FirstOrDefault();
                //if (oldWangPan_FileJiLu == null)
                //{
                foreach (var item in para)
                {
                    var name = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.TeamId == item.name).FirstOrDefault().Name;


                    WangPan_QiYeWenJian WangPan_QiYeWenJian = new WangPan_QiYeWenJian();
                    WangPan_QiYeWenJian.Id = Guid.NewGuid().ToString();
                    WangPan_QiYeWenJian.MemberId = memberid;
                    WangPan_QiYeWenJian.url = item.url;
                    WangPan_QiYeWenJian.length = item.length;
                    WangPan_QiYeWenJian.FileName = item.filename;
                    WangPan_QiYeWenJian.type = item.type;
                    WangPan_QiYeWenJian.MenuId = item.wenJianJiaId;
                    WangPan_QiYeWenJian.UId = item.name;
                    WangPan_QiYeWenJian.CreateDate = DateTime.Now;
                    _JointOfficeContext.WangPan_QiYeWenJian.Add(WangPan_QiYeWenJian);

                    var WangPan_FileJiLu = new WangPan_FileJiLu();
                    WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                    WangPan_FileJiLu.Tid = tid;
                    WangPan_FileJiLu.MemberId = memberid;
                    WangPan_FileJiLu.Name = name;
                    WangPan_FileJiLu.Type = 1;
                    WangPan_FileJiLu.BlobType = item.type;
                    WangPan_FileJiLu.Url = item.url;
                    WangPan_FileJiLu.Uid = item.name;
                    WangPan_FileJiLu.Length = item.length;
                    WangPan_FileJiLu.FileName = item.filename;
                    WangPan_FileJiLu.CreateDate = DateTime.Now;
                    WangPan_FileJiLu.IsDelete = 0;
                    WangPan_FileJiLu.WenJianId = WangPan_QiYeWenJian.Id;
                    _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);
                }
                //}
                //else
                //{
                //    if (oldWangPan_FileJiLu.MemberId == memberid && oldWangPan_FileJiLu.Uid == WangPan_QiYeMenu.Id)
                //    {
                //        foreach (var item in para)
                //        {
                //            var WangPan_FileJiLu = new WangPan_FileJiLu();
                //            WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                //            WangPan_FileJiLu.Tid = oldWangPan_FileJiLu.Tid;
                //            WangPan_FileJiLu.MemberId = memberid;
                //            WangPan_FileJiLu.Name = WangPan_QiYeMenu.Name;
                //            WangPan_FileJiLu.Type = 1;
                //            WangPan_FileJiLu.Url = item.url;
                //            WangPan_FileJiLu.Uid = WangPan_QiYeMenu.Id;
                //            WangPan_FileJiLu.Length = item.length;
                //            WangPan_FileJiLu.FileName = item.filename;
                //            WangPan_FileJiLu.CreateDate = DateTime.Now;
                //            WangPan_FileJiLu.IsDelete = 0;
                //            _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);

                //            WangPan_QiYeWenJian WangPan_QiYeWenJian = new WangPan_QiYeWenJian();
                //            WangPan_QiYeWenJian.Id = Guid.NewGuid().ToString();
                //            WangPan_QiYeWenJian.MemberId = memberid;
                //            WangPan_QiYeWenJian.length = item.length;
                //            WangPan_QiYeWenJian.url = item.url;
                //            WangPan_QiYeWenJian.FileName = item.filename;
                //            WangPan_QiYeWenJian.MenuId = item.wenJianJiaId;
                //            WangPan_QiYeWenJian.type = item.type;
                //            WangPan_QiYeWenJian.UId = WangPan_QiYeMenu.TeamId;
                //            WangPan_QiYeWenJian.CreateDate = DateTime.Now;
                //            _JointOfficeContext.WangPan_QiYeWenJian.Add(WangPan_QiYeWenJian);
                //        }
                //    }
                //    else
                //    {
                //        foreach (var item in para)
                //        {
                //            var WangPan_FileJiLu = new WangPan_FileJiLu();
                //            WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                //            WangPan_FileJiLu.Tid = tid;
                //            WangPan_FileJiLu.MemberId = memberid;
                //            WangPan_FileJiLu.Name = WangPan_QiYeMenu.Name;
                //            WangPan_FileJiLu.Type = 1;
                //            WangPan_FileJiLu.Url = item.url;
                //            WangPan_FileJiLu.FileName = item.filename;
                //            WangPan_FileJiLu.Length = item.length;
                //            WangPan_FileJiLu.Uid = WangPan_QiYeMenu.Id;
                //            WangPan_FileJiLu.CreateDate = DateTime.Now;
                //            WangPan_FileJiLu.IsDelete = 0;
                //            _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);

                //            WangPan_QiYeWenJian WangPan_QiYeWenJian = new WangPan_QiYeWenJian();
                //            WangPan_QiYeWenJian.Id = Guid.NewGuid().ToString();
                //            WangPan_QiYeWenJian.MemberId = memberid;
                //            WangPan_QiYeWenJian.url = item.url;
                //            WangPan_QiYeWenJian.type = item.type;
                //            WangPan_QiYeWenJian.MenuId = item.wenJianJiaId;
                //            WangPan_QiYeWenJian.UId = WangPan_QiYeMenu.TeamId;
                //            WangPan_QiYeWenJian.FileName = item.filename;
                //            WangPan_QiYeWenJian.length = item.length;
                //            WangPan_QiYeWenJian.CreateDate = DateTime.Now;
                //            _JointOfficeContext.WangPan_QiYeWenJian.Add(WangPan_QiYeWenJian);
                //        }
                //    }
                //}
            }
            else if (WangPan_GongXiangMenu1 != null)
            {
                //var oldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Type == 1).OrderByDescending(t => t.CreateDate).FirstOrDefault();
                //if (oldWangPan_FileJiLu == null)
                //{


                foreach (var item in para)
                {
                    var name = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == item.name).FirstOrDefault().Name;


                    WangPan_GongXiangWenJian WangPan_GongXiangWenJian = new WangPan_GongXiangWenJian();
                    WangPan_GongXiangWenJian.Id = Guid.NewGuid().ToString();
                    WangPan_GongXiangWenJian.MemberId = memberid;
                    WangPan_GongXiangWenJian.url = item.url;
                    WangPan_GongXiangWenJian.MenuId = item.wenJianJiaId;
                    WangPan_GongXiangWenJian.length = item.length;
                    WangPan_GongXiangWenJian.type = item.type;
                    WangPan_GongXiangWenJian.FileName = item.filename;
                    WangPan_GongXiangWenJian.UId = item.name;
                    WangPan_GongXiangWenJian.CreateDate = DateTime.Now;
                    _JointOfficeContext.WangPan_GongXiangWenJian.Add(WangPan_GongXiangWenJian);

                    var WangPan_FileJiLu = new WangPan_FileJiLu();
                    WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                    WangPan_FileJiLu.Tid = tid;
                    WangPan_FileJiLu.MemberId = memberid;
                    WangPan_FileJiLu.Name = name;
                    WangPan_FileJiLu.Type = 1;
                    WangPan_FileJiLu.Url = item.url;
                    WangPan_FileJiLu.Length = item.length;
                    WangPan_FileJiLu.BlobType = item.type;
                    WangPan_FileJiLu.Uid = item.name;
                    WangPan_FileJiLu.FileName = item.filename;
                    WangPan_FileJiLu.CreateDate = DateTime.Now;
                    WangPan_FileJiLu.IsDelete = 0;
                    WangPan_FileJiLu.WenJianId = WangPan_GongXiangWenJian.Id;
                    _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);
                }
                //}
                //else
                //{
                //    if (oldWangPan_FileJiLu.MemberId == memberid && oldWangPan_FileJiLu.Uid == WangPan_GongXiangMenu.Id)
                //    {
                //        foreach (var item in para)
                //        {
                //            var WangPan_FileJiLu = new WangPan_FileJiLu();
                //            WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                //            WangPan_FileJiLu.Tid = oldWangPan_FileJiLu.Tid;
                //            WangPan_FileJiLu.MemberId = memberid;
                //            WangPan_FileJiLu.Name = WangPan_GongXiangMenu.Name;
                //            WangPan_FileJiLu.Type = 1;
                //            WangPan_FileJiLu.Url = item.url;
                //            WangPan_FileJiLu.Length = item.length;
                //            WangPan_FileJiLu.Uid = WangPan_GongXiangMenu.Id;
                //            WangPan_FileJiLu.FileName = item.filename;
                //            WangPan_FileJiLu.CreateDate = DateTime.Now;
                //            WangPan_FileJiLu.IsDelete = 0;
                //            _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);

                //            WangPan_GongXiangWenJian WangPan_GongXiangWenJian = new WangPan_GongXiangWenJian();
                //            WangPan_GongXiangWenJian.Id = Guid.NewGuid().ToString();
                //            WangPan_GongXiangWenJian.MemberId = memberid;
                //            WangPan_GongXiangWenJian.url = item.url;
                //            WangPan_GongXiangWenJian.length = item.length;
                //            WangPan_GongXiangWenJian.FileName = item.filename;
                //            WangPan_GongXiangWenJian.type = item.type;
                //            WangPan_GongXiangWenJian.MenuId = item.wenJianJiaId;
                //            WangPan_GongXiangWenJian.UId = WangPan_GongXiangMenu.Id;
                //            WangPan_GongXiangWenJian.CreateDate = DateTime.Now;
                //            _JointOfficeContext.WangPan_GongXiangWenJian.Add(WangPan_GongXiangWenJian);
                //        }
                //    }
                //    else
                //    {
                //        foreach (var item in para)
                //        {
                //            var WangPan_FileJiLu = new WangPan_FileJiLu();
                //            WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                //            WangPan_FileJiLu.Tid = tid;
                //            WangPan_FileJiLu.MemberId = memberid;
                //            WangPan_FileJiLu.Name = WangPan_GongXiangMenu.Name;
                //            WangPan_FileJiLu.Type = 1;
                //            WangPan_FileJiLu.Url = item.url;
                //            WangPan_FileJiLu.FileName = item.filename;
                //            WangPan_FileJiLu.Length = item.length;
                //            WangPan_FileJiLu.Uid = WangPan_GongXiangMenu.Id;
                //            WangPan_FileJiLu.CreateDate = DateTime.Now;
                //            WangPan_FileJiLu.IsDelete = 0;
                //            _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);

                //            WangPan_GongXiangWenJian WangPan_GongXiangWenJian = new WangPan_GongXiangWenJian();
                //            WangPan_GongXiangWenJian.Id = Guid.NewGuid().ToString();
                //            WangPan_GongXiangWenJian.MemberId = memberid;
                //            WangPan_GongXiangWenJian.url = item.url;
                //            WangPan_GongXiangWenJian.MenuId = item.wenJianJiaId;
                //            WangPan_GongXiangWenJian.length = item.length;
                //            WangPan_GongXiangWenJian.FileName = item.filename;
                //            WangPan_GongXiangWenJian.type = item.type;
                //            WangPan_GongXiangWenJian.UId = WangPan_GongXiangMenu.Id;
                //            WangPan_GongXiangWenJian.CreateDate = DateTime.Now;
                //            _JointOfficeContext.WangPan_GongXiangWenJian.Add(WangPan_GongXiangWenJian);
                //        }
                //    }
                //}
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("上传成功");
        }
        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <param name="文件夹名，文件夹ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge XinJianWenJianJia(XinJianWenJianJiaPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            CloudBlobContainer container = blobClient.GetContainerReference("spjointoffice");
            if (para.wenJianJiaId == "0")
            {
                var info = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == "0" && t.MemberId == memberid).FirstOrDefault();
                if (info != null)
                {
                    var WangPan_Menu1 = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == info.Id).ToList();
                    if (WangPan_Menu1.Count > 0)
                    {
                        foreach (var item in WangPan_Menu1)
                        {
                            if (item.Name == para.wenJianJiaName)
                            {
                                throw new BusinessTureException("已存在该名称的文件夹.");
                            }
                        }
                    }
                    WangPan_Menu WangPan_Menu = new WangPan_Menu();
                    WangPan_Menu.Id = Guid.NewGuid().ToString();
                    WangPan_Menu.MemberId = memberid;
                    WangPan_Menu.Name = para.wenJianJiaName;
                    WangPan_Menu.ParentId = info.Id;
                    WangPan_Menu.CreateDate = DateTime.Now;
                    _JointOfficeContext.WangPan_Menu.Add(WangPan_Menu);
                }
            }
            else
            {
                var WangPan_Menu1 = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                if (WangPan_Menu1 != null)
                {
                    var WangPan_Menulist = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == para.wenJianJiaId).ToList();
                    if (WangPan_Menulist.Count > 0)
                    {
                        foreach (var item in WangPan_Menulist)
                        {
                            if (item.Name == para.wenJianJiaName)
                            {
                                throw new BusinessTureException("已存在该名称的文件夹.");
                            }
                        }
                    }
                    WangPan_Menu WangPan_Menu = new WangPan_Menu();
                    WangPan_Menu.Id = Guid.NewGuid().ToString();
                    WangPan_Menu.MemberId = memberid;
                    WangPan_Menu.Name = para.wenJianJiaName;
                    WangPan_Menu.ParentId = para.wenJianJiaId;
                    WangPan_Menu.CreateDate = DateTime.Now;
                    _JointOfficeContext.WangPan_Menu.Add(WangPan_Menu);
                }
                var Member_Team1 = _JointOfficeContext.Member_Team.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                if (Member_Team1 != null)
                {
                    var Member_TeamList = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.TeamId == para.wenJianJiaId).ToList();
                    if (Member_TeamList.Count > 0)
                    {
                        foreach (var item in Member_TeamList)
                        {
                            if (item.Name == para.wenJianJiaName)
                            {
                                throw new BusinessTureException("已存在该名称的文件夹.");
                            }
                        }
                    }
                    WangPan_GongXiangMenu WangPan_GongXiangMenu = new WangPan_GongXiangMenu();
                    WangPan_GongXiangMenu.Id = Guid.NewGuid().ToString();
                    WangPan_GongXiangMenu.ChuanJian = memberid;
                    WangPan_GongXiangMenu.Name = para.wenJianJiaName;
                    WangPan_GongXiangMenu.ParentId = "0";
                    WangPan_GongXiangMenu.TeamId = para.wenJianJiaId;
                    WangPan_GongXiangMenu.CreateDate = DateTime.Now;
                    _JointOfficeContext.WangPan_GongXiangMenu.Add(WangPan_GongXiangMenu);
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(WangPan_GongXiangMenu.Id + "/ceshi.txt");
                    //using (var fileStream = System.IO.File.OpenRead(@"C:\ceshi.txt"))
                    //{
                    //    blockBlob.UploadFromStreamAsync(fileStream);
                    //}
                    using (var fileStream = System.IO.File.OpenRead(Directory.GetCurrentDirectory() + @"\ceshi.txt"))
                    {
                        blockBlob.UploadFromStreamAsync(fileStream);
                    }
                }
                var WangPan_GongXiangMenu1 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                if (WangPan_GongXiangMenu1 != null)
                {
                    var WangPan_GongXiangMenuList = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ParentId == para.wenJianJiaId).ToList();
                    if (WangPan_GongXiangMenuList.Count > 0)
                    {
                        foreach (var item in WangPan_GongXiangMenuList)
                        {
                            if (item.Name == para.wenJianJiaName)
                            {
                                throw new BusinessTureException("已存在该名称的文件夹.");
                            }
                        }
                    }
                    WangPan_GongXiangMenu WangPan_GongXiangMenu = new WangPan_GongXiangMenu();
                    WangPan_GongXiangMenu.Id = Guid.NewGuid().ToString();
                    WangPan_GongXiangMenu.Name = para.wenJianJiaName;
                    WangPan_GongXiangMenu.ChuanJian = memberid;
                    WangPan_GongXiangMenu.ParentId = para.wenJianJiaId;
                    WangPan_GongXiangMenu.TeamId = WangPan_GongXiangMenu1.TeamId;
                    WangPan_GongXiangMenu.CreateDate = DateTime.Now;
                    if (WangPan_GongXiangMenu1.ParentId != "0")
                    {
                        WangPan_GongXiangMenu.Uid = WangPan_GongXiangMenu1.Uid;
                    }
                    else
                    {
                        WangPan_GongXiangMenu.Uid = WangPan_GongXiangMenu1.Id;
                    }

                    _JointOfficeContext.WangPan_GongXiangMenu.Add(WangPan_GongXiangMenu);
                }
                var WangPan_QiYeMenu1 = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                if (WangPan_QiYeMenu1 != null)
                {
                    var WangPan_QiYeMenuList = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.ParentId == para.wenJianJiaId).ToList();
                    if (WangPan_QiYeMenuList.Count > 0)
                    {
                        foreach (var item in WangPan_QiYeMenuList)
                        {
                            if (item.Name == para.wenJianJiaName)
                            {
                                throw new BusinessTureException("已存在该名称的文件夹.");
                            }
                        }
                    }
                    WangPan_QiYeMenu WangPan_QiYeMenu = new WangPan_QiYeMenu();
                    WangPan_QiYeMenu.Id = Guid.NewGuid().ToString();
                    WangPan_QiYeMenu.Name = para.wenJianJiaName;
                    WangPan_QiYeMenu.MemberId = memberid;
                    WangPan_QiYeMenu.ParentId = para.wenJianJiaId;
                    WangPan_QiYeMenu.CreateDate = DateTime.Now;
                    WangPan_QiYeMenu.TeamId = WangPan_QiYeMenu1.TeamId;
                    _JointOfficeContext.WangPan_QiYeMenu.Add(WangPan_QiYeMenu);
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("创建成功");
        }
        //public Showapi_Res_Meaasge XinJianGongXiangWenJianJia(XinJianGongXiangWenJianJiaPara para)
        //{
        //    var memberid = _PrincipalBase.GetMemberId();
        //    if (para.type == 1)
        //    {
        //    }
        //    else
        //    {
        //    }
        //    _JointOfficeContext.SaveChanges();
        //    Message Message = new Message();
        //    return Message.SuccessMeaasge("创建成功");
        //}
        //public Showapi_Res_Meaasge XinJianQiYeWenJianJia(XinJianQiYeWenJianJiaPara para)
        //{
        //    var memberid = _PrincipalBase.GetMemberId();

        //    _JointOfficeContext.SaveChanges();
        //    Message Message = new Message();
        //    return Message.SuccessMeaasge("创建成功");
        //}
        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="文件夹ID，文件夹名，文件类型"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge RenameWenJianJia(RenameWenJianJiaPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            if (para.type == 1)
            {
                var WangPan_Menu = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                if (WangPan_Menu != null)
                {
                    var WangPan_MenuList = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == WangPan_Menu.ParentId).ToList();
                    if (WangPan_MenuList.Count > 0)
                    {
                        foreach (var item in WangPan_MenuList)
                        {
                            if (item.Name == para.wenJianJiaName)
                            {
                                throw new BusinessTureException("已存在该名称的文件夹.");
                            }
                        }
                    }
                    WangPan_Menu.Name = para.wenJianJiaName;
                }
                var WangPan_QiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                if (WangPan_QiYeMenu != null)
                {
                    var WangPan_QiYeMenuList = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.ParentId == WangPan_QiYeMenu.ParentId).ToList();
                    if (WangPan_QiYeMenuList.Count > 0)
                    {
                        foreach (var item in WangPan_QiYeMenuList)
                        {
                            if (item.Name == para.wenJianJiaName)
                            {
                                throw new BusinessTureException("已存在该名称的文件夹.");
                            }
                        }
                    }
                    WangPan_QiYeMenu.Name = para.wenJianJiaName;
                }
                var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                if (WangPan_GongXiangMenu != null)
                {

                    var WangPan_GongXiangMenuList = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ParentId == WangPan_GongXiangMenu.ParentId && t.ChuanJian==memberid).ToList();
                    if (WangPan_GongXiangMenuList.Count > 0)
                    {
                        foreach (var item in WangPan_GongXiangMenuList)
                        {
                            if (item.Name == para.wenJianJiaName && item.Id!=para.wenJianJiaId)
                            {
                                throw new BusinessTureException("已存在该名称的文件夹.");
                            }
                        }
                    }
                    WangPan_GongXiangMenu.Name = para.wenJianJiaName;
                }
            }
            else
            {
                var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                if (WangPan_WenJian != null)
                {
                    var WangPan_WenJianList = _JointOfficeContext.WangPan_WenJian.Where(t => t.MenuId == WangPan_WenJian.MenuId).ToList();
                    if (WangPan_WenJianList.Count > 0)
                    {
                        foreach (var item in WangPan_WenJianList)
                        {
                            if (item.FileName == para.wenJianJiaName)
                            {
                                throw new BusinessTureException("已存在该名称的文件夹.");
                            }
                        }
                    }
                    WangPan_WenJian.FileName = para.wenJianJiaName;
                }
                var WangPan_QiYeWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                if (WangPan_QiYeWenJian != null)
                {
                    var WangPan_QiYeWenJianList = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.MenuId == WangPan_QiYeWenJian.MenuId).ToList();
                    if (WangPan_QiYeWenJianList.Count > 0)
                    {
                        foreach (var item in WangPan_QiYeWenJianList)
                        {
                            if (item.FileName == para.wenJianJiaName)
                            {
                                throw new BusinessTureException("已存在该名称的文件夹.");
                            }
                        }
                    }
                    WangPan_QiYeWenJian.FileName = para.wenJianJiaName;
                }
                var WangPan_GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                if (WangPan_GongXiangWenJian != null)
                {
                    var WWangPan_GongXiangWenJianList = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.MenuId == WangPan_GongXiangWenJian.MenuId).ToList();
                    if (WWangPan_GongXiangWenJianList.Count > 0)
                    {
                        foreach (var item in WWangPan_GongXiangWenJianList)
                        {
                            if (item.FileName == para.wenJianJiaName)
                            {
                                throw new BusinessTureException("已存在该名称的文件夹.");
                            }
                        }
                    }
                    WangPan_GongXiangWenJian.FileName = para.wenJianJiaName;
                }
            }
            _JointOfficeContext.SaveChanges();
            //Message Message = new Message();
            return Message.SuccessMeaasge("重命名成功");
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="文件夹ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge DeleteWenJianJia(DeleteWenJianJiaPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            CloudBlobContainer container = blobClient.GetContainerReference("spjointoffice");
            var WangPan_Menu1 = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
            var WangPan_QiYeMenu1 = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
            var WangPan_GongXiangMenu1 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
            if (WangPan_Menu1 != null)
            {
                var WangPan_Menu = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == para.wenJianJiaId).ToList();
                var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.MenuId == para.wenJianJiaId).ToList();
                foreach (var item in WangPan_WenJian)
                {
                    string[] b = item.url.Split(new Char[] { '/' });
                    var url = b[4];
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                    blockBlob.DeleteAsync();
                }
                _JointOfficeContext.WangPan_Menu.RemoveRange(WangPan_Menu);
                _JointOfficeContext.WangPan_Menu.Remove(WangPan_Menu1);
                _JointOfficeContext.WangPan_WenJian.RemoveRange(WangPan_WenJian);
            }
            if (WangPan_QiYeMenu1 != null)
            {
                var WangPan_QiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.ParentId == para.wenJianJiaId).ToList();
                var WangPan_QiYeWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.MenuId == para.wenJianJiaId).ToList();
                foreach (var item in WangPan_QiYeWenJian)
                {
                    string[] b = item.url.Split(new Char[] { '/' });
                    var url = b[4];
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                    blockBlob.DeleteAsync();
                }
                _JointOfficeContext.WangPan_QiYeMenu.RemoveRange(WangPan_QiYeMenu);
                _JointOfficeContext.WangPan_QiYeMenu.Remove(WangPan_QiYeMenu1);
                _JointOfficeContext.WangPan_QiYeWenJian.RemoveRange(WangPan_QiYeWenJian);

            }
            if (WangPan_GongXiangMenu1 != null)
            {
                var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ParentId == para.wenJianJiaId).ToList();
                var WangPan_GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.MenuId == para.wenJianJiaId).ToList();
                foreach (var item in WangPan_GongXiangWenJian)
                {
                    string[] b = item.url.Split(new Char[] { '/' });
                    var url = b[4];
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                    blockBlob.DeleteAsync();
                }
                _JointOfficeContext.WangPan_GongXiangMenu.RemoveRange(WangPan_GongXiangMenu);
                _JointOfficeContext.WangPan_GongXiangMenu.Remove(WangPan_GongXiangMenu1);
                _JointOfficeContext.WangPan_GongXiangWenJian.RemoveRange(WangPan_GongXiangWenJian);
            }
            _JointOfficeContext.SaveChanges();
            //Message Message = new Message();
            return Message.SuccessMeaasge("删除成功");
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="文件夹ID，文件类型"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge DeleteBlob(List<DeletePara> para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            //var memberid = "412f8982-140b-4157-a287-d45a8fb74e44";
            CloudBlobContainer container = blobClient.GetContainerReference("spjointoffice");
            var list1 = para.Where(t => t.type == 1).ToList();
            var list2 = para.Where(t => t.type == 2).ToList();

            var OneMenu = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == para.FirstOrDefault().wenJianJiaId).FirstOrDefault();
            var WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.Id == para.FirstOrDefault().wenJianJiaId).FirstOrDefault();
            if (OneMenu != null || WenJian != null)
            {
                var OneMenu1 = _JointOfficeContext.WangPan_Menu.Where(t => t.MemberId == memberid && t.ParentId == "0").FirstOrDefault();
                foreach (var item in list1)
                {
                    var WangPan_Menu1 = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                    var WangPan_Menu = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == item.wenJianJiaId).ToList();
                    var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.MenuId == item.wenJianJiaId).ToList();
                    foreach (var One in WangPan_WenJian)
                    {
                        string[] c = One.url.Split(new Char[] { '/' });
                        var url = c[4] + '/' + c[5];
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                        blockBlob.DeleteAsync();
                    }
                    _JointOfficeContext.WangPan_Menu.RemoveRange(WangPan_Menu);
                    _JointOfficeContext.WangPan_Menu.Remove(WangPan_Menu1);
                    _JointOfficeContext.WangPan_WenJian.RemoveRange(WangPan_WenJian);
                }
                //var oldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Type == 2).OrderByDescending(t => t.CreateDate).FirstOrDefault();
                var tid = Guid.NewGuid().ToString();
                //if (oldWangPan_FileJiLu == null)
                //{
                foreach (var item in list2)
                {
                    var OneWenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                    string[] c = OneWenJian.url.Split(new Char[] { '/' });
                    var url = c[4] + '/' + c[5];
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                    var WangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.WenJianId == item.wenJianJiaId).FirstOrDefault();
                    if (WangPan_FileJiLu != null)
                    {
                        WangPan_FileJiLu.IsDelete = 1;
                    }
                    //WangPan_FileJiLu WangPan_FileJiLu = new WangPan_FileJiLu();
                    //WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                    //WangPan_FileJiLu.Tid = tid;
                    //WangPan_FileJiLu.MemberId = memberid;
                    //WangPan_FileJiLu.Name = "我的文件";
                    //WangPan_FileJiLu.CreateDate = DateTime.Now;
                    //WangPan_FileJiLu.IsDelete = 1;
                    //WangPan_FileJiLu.Type = 2;
                    //_JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);
                    //var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.MenuId == item.wenJianJiaId).FirstOrDefault();
                    _JointOfficeContext.WangPan_WenJian.Remove(OneWenJian);
                    //var OldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Url == OneWenJian.url && t.Type == 1).FirstOrDefault();
                    //OldWangPan_FileJiLu.IsDelete = 1;
                    blockBlob.DeleteAsync();
                }
                //}
                //else
                //{
                //    if (oldWangPan_FileJiLu.MemberId == memberid && oldWangPan_FileJiLu.Uid == OneMenu1.Id)
                //    {
                //        foreach (var item in list2)
                //        {
                //            var OneWenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                //            string[] c = OneWenJian.url.Split(new Char[] { '/' });
                //            var url = c[4] + '/' + c[5];
                //            CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                //            WangPan_FileJiLu WangPan_FileJiLu = new WangPan_FileJiLu();
                //            WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                //            WangPan_FileJiLu.Tid = oldWangPan_FileJiLu.Tid;
                //            WangPan_FileJiLu.MemberId = memberid;
                //            WangPan_FileJiLu.Name = "我的文件";
                //            WangPan_FileJiLu.CreateDate = DateTime.Now;
                //            WangPan_FileJiLu.IsDelete = 1;
                //            WangPan_FileJiLu.Type = 2;
                //            _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);
                //            _JointOfficeContext.WangPan_WenJian.Remove(OneWenJian);
                //            var OldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Url == OneWenJian.url && t.Type == 1).FirstOrDefault();
                //            OldWangPan_FileJiLu.IsDelete = 1;
                //            blockBlob.DeleteAsync();
                //        }
                //    }
                //    else
                //    {
                //        foreach (var item in list2)
                //        {
                //            var OneWenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                //            string[] c = OneWenJian.url.Split(new Char[] { '/' });
                //            var url = c[4] + '/' + c[5];
                //            CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                //            WangPan_FileJiLu WangPan_FileJiLu = new WangPan_FileJiLu();
                //            WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                //            WangPan_FileJiLu.Tid = tid;
                //            WangPan_FileJiLu.MemberId = memberid;
                //            WangPan_FileJiLu.Name = "我的文件";
                //            WangPan_FileJiLu.CreateDate = DateTime.Now;
                //            WangPan_FileJiLu.IsDelete = 1;
                //            WangPan_FileJiLu.Type = 2;
                //            _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);
                //            _JointOfficeContext.WangPan_WenJian.Remove(OneWenJian);
                //            var OldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Url == OneWenJian.url && t.Type == 1).FirstOrDefault();
                //            OldWangPan_FileJiLu.IsDelete = 1;
                //            blockBlob.DeleteAsync();
                //        }
                //    }
                //}
            }
            var OneGongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == para.FirstOrDefault().wenJianJiaId).FirstOrDefault();
            var GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.Id == para.FirstOrDefault().wenJianJiaId).FirstOrDefault();
            if (OneGongXiangMenu != null || GongXiangWenJian != null)
            {
                var OneGongXiangMenu1 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ChuanJian == memberid && t.ParentId == "0").FirstOrDefault();
                foreach (var item in list1)
                {
                    var WangPan_GongXiangMenu1 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                    var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ParentId == item.wenJianJiaId).ToList();
                    var WangPan_GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.MenuId == item.wenJianJiaId).ToList();
                    foreach (var One in WangPan_GongXiangWenJian)
                    {
                        string[] c = One.url.Split(new Char[] { '/' });
                        var url = c[4] + '/' + c[5];
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                        blockBlob.DeleteAsync();
                    }
                    _JointOfficeContext.WangPan_GongXiangMenu.RemoveRange(WangPan_GongXiangMenu);
                    _JointOfficeContext.WangPan_GongXiangMenu.Remove(WangPan_GongXiangMenu1);
                    _JointOfficeContext.WangPan_GongXiangWenJian.RemoveRange(WangPan_GongXiangWenJian);
                }
                //var oldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Type == 2).OrderByDescending(t => t.CreateDate).FirstOrDefault();
                var tid = Guid.NewGuid().ToString();
                //if (oldWangPan_FileJiLu == null)
                //{
                foreach (var item in list2)
                {
                    var OneWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                    string[] c = OneWenJian.url.Split(new Char[] { '/' });
                    var url = c[4] + '/' + c[5];
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                    var WangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.WenJianId == item.wenJianJiaId).FirstOrDefault();
                    if (WangPan_FileJiLu != null)
                    {
                        WangPan_FileJiLu.IsDelete = 1;
                    }
                    //WangPan_FileJiLu WangPan_FileJiLu = new WangPan_FileJiLu();
                    //WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                    //WangPan_FileJiLu.Tid = tid;
                    //WangPan_FileJiLu.MemberId = memberid;
                    //WangPan_FileJiLu.Name = OneGongXiangMenu1.Name;
                    //WangPan_FileJiLu.CreateDate = DateTime.Now;
                    //WangPan_FileJiLu.IsDelete = 1;
                    //WangPan_FileJiLu.Type = 2;
                    //_JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);
                    //var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.MenuId == item.wenJianJiaId).FirstOrDefault();
                    _JointOfficeContext.WangPan_GongXiangWenJian.Remove(OneWenJian);
                    //var OldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Url == OneWenJian.url && t.Type == 1).FirstOrDefault();
                    //OldWangPan_FileJiLu.IsDelete = 1;
                    blockBlob.DeleteAsync();
                }
                //}
                //else
                //{
                //    if (oldWangPan_FileJiLu.MemberId == memberid && oldWangPan_FileJiLu.Uid == OneGongXiangMenu1.Id)
                //    {
                //        foreach (var item in list2)
                //        {
                //            var OneWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                //            string[] c = OneWenJian.url.Split(new Char[] { '/' });
                //            var url = c[4] + '/' + c[5];
                //            CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                //            WangPan_FileJiLu WangPan_FileJiLu = new WangPan_FileJiLu();
                //            WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                //            WangPan_FileJiLu.Tid = oldWangPan_FileJiLu.Tid;
                //            WangPan_FileJiLu.MemberId = memberid;
                //            WangPan_FileJiLu.Name = OneGongXiangMenu1.Name;
                //            WangPan_FileJiLu.CreateDate = DateTime.Now;
                //            WangPan_FileJiLu.IsDelete = 1;
                //            WangPan_FileJiLu.Type = 2;
                //            _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);
                //            _JointOfficeContext.WangPan_GongXiangWenJian.Remove(OneWenJian);
                //            var OldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Url == OneWenJian.url && t.Type == 1).FirstOrDefault();
                //            OldWangPan_FileJiLu.IsDelete = 1;
                //            blockBlob.DeleteAsync();
                //        }
                //    }
                //    else
                //    {
                //        foreach (var item in list2)
                //        {
                //            var OneWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                //            string[] c = OneWenJian.url.Split(new Char[] { '/' });
                //            var url = c[4] + '/' + c[5];
                //            CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                //            WangPan_FileJiLu WangPan_FileJiLu = new WangPan_FileJiLu();
                //            WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                //            WangPan_FileJiLu.Tid = tid;
                //            WangPan_FileJiLu.MemberId = memberid;
                //            WangPan_FileJiLu.Name = OneGongXiangMenu1.Name;
                //            WangPan_FileJiLu.CreateDate = DateTime.Now;
                //            WangPan_FileJiLu.IsDelete = 1;
                //            WangPan_FileJiLu.Type = 2;
                //            _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);
                //            //var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.MenuId == item.wenJianJiaId).FirstOrDefault();
                //            _JointOfficeContext.WangPan_GongXiangWenJian.Remove(OneWenJian);
                //            var OldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Url == OneWenJian.url && t.Type == 1).FirstOrDefault();
                //            OldWangPan_FileJiLu.IsDelete = 1;
                //            blockBlob.DeleteAsync();
                //        }
                //    }
                //}

            }
            var OneQiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == para.FirstOrDefault().wenJianJiaId).FirstOrDefault();
            var QiYeWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.Id == para.FirstOrDefault().wenJianJiaId).FirstOrDefault();
            if (OneQiYeMenu != null || QiYeWenJian != null)
            {
                var name = "";
                if (OneQiYeMenu != null)
                {
                    name = _JointOfficeContext.Member_Team.Where(t => t.Id == OneQiYeMenu.TeamId).FirstOrDefault().Name;
                }
                if (QiYeWenJian != null)
                {
                    name = _JointOfficeContext.Member_Team.Where(t => t.Id == QiYeWenJian.UId).FirstOrDefault().Name;

                }
                var OneQiYeMenu1 = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.MemberId == memberid && t.ParentId == "0").FirstOrDefault();
                foreach (var item in list1)
                {
                    var WangPan_QiYeMenu1 = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                    var WangPan_QiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.ParentId == item.wenJianJiaId).ToList();
                    var WangPan_QiYeWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.MenuId == item.wenJianJiaId).ToList();
                    foreach (var One in WangPan_QiYeWenJian)
                    {
                        string[] c = One.url.Split(new Char[] { '/' });
                        var url = c[4] + '/' + c[5];
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                        blockBlob.DeleteAsync();
                    }
                    _JointOfficeContext.WangPan_QiYeMenu.RemoveRange(WangPan_QiYeMenu);
                    _JointOfficeContext.WangPan_QiYeMenu.Remove(WangPan_QiYeMenu1);
                    _JointOfficeContext.WangPan_QiYeWenJian.RemoveRange(WangPan_QiYeWenJian);
                }
                //var oldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Type == 2).OrderByDescending(t => t.CreateDate).FirstOrDefault();
                var tid = Guid.NewGuid().ToString();
                //if (oldWangPan_FileJiLu == null)
                //{
                foreach (var item in list2)
                {
                    var OneWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                    string[] c = OneWenJian.url.Split(new Char[] { '/' });
                    var url = c[4] + '/' + c[5];
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                    //WangPan_FileJiLu WangPan_FileJiLu = new WangPan_FileJiLu();
                    //WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                    //WangPan_FileJiLu.Tid = tid;
                    //WangPan_FileJiLu.MemberId = memberid;
                    //WangPan_FileJiLu.Name = name;
                    //WangPan_FileJiLu.CreateDate = DateTime.Now;
                    //WangPan_FileJiLu.IsDelete = 1;
                    //WangPan_FileJiLu.Type = 2;
                    //_JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);
                    var WangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.WenJianId == item.wenJianJiaId).FirstOrDefault();
                    if (WangPan_FileJiLu != null)
                    {
                        WangPan_FileJiLu.IsDelete = 1;
                    }
                    //var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.MenuId == item.wenJianJiaId).FirstOrDefault();
                    _JointOfficeContext.WangPan_QiYeWenJian.Remove(OneWenJian);
                    //var OldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Url == OneWenJian.url && t.Type == 1).FirstOrDefault();
                    //OldWangPan_FileJiLu.IsDelete = 1;
                    blockBlob.DeleteAsync();
                }
                //}
                //else
                //{
                //if (oldWangPan_FileJiLu.MemberId == memberid && oldWangPan_FileJiLu.Uid == OneQiYeMenu1.Id)
                //{
                //    foreach (var item in list2)
                //    {
                //        var OneWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                //        string[] c = OneWenJian.url.Split(new Char[] { '/' });
                //        var url = c[4] + '/' + c[5];
                //        CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                //        WangPan_FileJiLu WangPan_FileJiLu = new WangPan_FileJiLu();
                //        WangPan_FileJiLu.Id = Guid.NewGuid().ToString();  
                //        WangPan_FileJiLu.Tid = oldWangPan_FileJiLu.Tid;
                //        WangPan_FileJiLu.MemberId = memberid;
                //        WangPan_FileJiLu.Name = name;
                //        WangPan_FileJiLu.CreateDate = DateTime.Now;
                //        WangPan_FileJiLu.IsDelete = 1;
                //        WangPan_FileJiLu.Type = 2;
                //        _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);
                //        _JointOfficeContext.WangPan_QiYeWenJian.Remove(OneWenJian);
                //        var OldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Url == OneWenJian.url && t.Type == 1).FirstOrDefault();
                //        OldWangPan_FileJiLu.IsDelete = 1;
                //        blockBlob.DeleteAsync();
                //    }
                //}
                //else
                //{
                //foreach (var item in list2)
                //{
                //    var OneWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                //    string[] c = OneWenJian.url.Split(new Char[] { '/' });
                //    var url = c[4] + '/' + c[5];
                //    CloudBlockBlob blockBlob = container.GetBlockBlobReference(url);
                //    WangPan_FileJiLu WangPan_FileJiLu = new WangPan_FileJiLu();
                //    WangPan_FileJiLu.Id = Guid.NewGuid().ToString();
                //    WangPan_FileJiLu.Tid = tid;
                //    WangPan_FileJiLu.MemberId = memberid;
                //    WangPan_FileJiLu.Name = name;
                //    WangPan_FileJiLu.CreateDate = DateTime.Now;
                //    WangPan_FileJiLu.IsDelete = 1;
                //    WangPan_FileJiLu.Type = 2;
                //    _JointOfficeContext.WangPan_FileJiLu.Add(WangPan_FileJiLu);
                //    //var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.MenuId == item.wenJianJiaId).FirstOrDefault();
                //    _JointOfficeContext.WangPan_QiYeWenJian.Remove(OneWenJian);
                //    var OldWangPan_FileJiLu = _JointOfficeContext.WangPan_FileJiLu.Where(t => t.MemberId == memberid && t.Url == OneWenJian.url && t.Type == 1).FirstOrDefault();
                //    OldWangPan_FileJiLu.IsDelete = 1;
                //    blockBlob.DeleteAsync();
                //}
                //}
                //}
            }
            var wenjianidList = para.Select(t => t.wenJianJiaId).ToList();
            var ZuiJinShiYongList = _JointOfficeContext.WangPan_ZuiJin.Where(t => wenjianidList.Contains(t.WenJianId)).ToList();
            _JointOfficeContext.WangPan_ZuiJin.RemoveRange(ZuiJinShiYongList);
            _JointOfficeContext.SaveChanges();
            // Delete the blob.
            //Message Message = new Message();
            return Message.SuccessMeaasge("删除成功");
        }
        /// <summary>
        /// 获取我的网盘页面信息
        /// </summary>
        /// <param name="页数，总数，文件夹ID，顺序，文件类型"></param>
        /// <returns></returns>
        public Showapi_Res_List<ListPara> WangPanList(WangPanListPara para)
        {
            Showapi_Res_List<ListPara> res = new Showapi_Res_List<ListPara>();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<ListPara>();
                return Return.Return();
            }
            //var memberid = "412f8982-140b-4157-a287-d45a8fb74e44";
            List<ListPara> list = new List<ListPara>();
            List<ListPara> Reslist = new List<ListPara>();
            if (para.wenJianJiaId == "0")
            {
                var info = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == "0" && t.MemberId == memberid).FirstOrDefault();
                if (info != null)
                {
                    var WangPan_Menu = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == info.Id).ToList();
                    if (WangPan_Menu.Count() > 0)
                    {
                        foreach (var item in WangPan_Menu)
                        {
                            if (para.type == 0)
                            {
                                ListPara ListPara = new ListPara();
                                ListPara.blobtype = "1";
                                ListPara.length = "-";
                                ListPara.wenJianJiaId = item.Id;
                                ListPara.name = item.Name;
                                ListPara.url = "";
                                ListPara.qxtype = 1;
                                ListPara.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                                list.Add(ListPara);
                            }
                        }
                    }
                    var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.MenuId == info.Id).ToList();
                    if (WangPan_WenJian.Count() > 0)
                    {
                        foreach (var item in WangPan_WenJian)
                        {
                            if (para.type == 0 || para.type == item.type)
                            {
                                //string[] b = item.url.Split(new Char[] { '/' });
                                //var name = b.Last();
                                ListPara ListPara = new ListPara();
                                ListPara.blobtype = "2";
                                ListPara.length = BusinessHelper.ConvertBytes(item.length);
                                ListPara.wenJianJiaId = item.Id;
                                ListPara.name = item.FileName;
                                ListPara.type = item.type;
                                ListPara.qxtype = 1;
                                ListPara.url = item.url + SasKey;
                                ListPara.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                                list.Add(ListPara);
                            }
                        }
                    }
                }
            }
            else if (para.wenJianJiaId == "gongxiang" && para.type == 0)
            {
                var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ParentId == "0" && (t.ChuanJian == memberid || t.GuanLi.Contains(memberid))).ToList();
                foreach (var item in WangPan_GongXiangMenu)
                {
                    var WangPan_GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.UId == item.Id).ToList();
                    var Member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.ChuanJian).FirstOrDefault();
                    var Member_Team = _JointOfficeContext.Member_Team.Where(t => t.Id == item.TeamId).FirstOrDefault();
                    var length = WangPan_GongXiangWenJian.Sum(t => t.length);
                    ListPara GongXiangWenJianList = new ListPara();
                    GongXiangWenJianList.length = "-";
                    GongXiangWenJianList.name = item.Name;
                    GongXiangWenJianList.person = Member_Info.Name;

                    GongXiangWenJianList.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                    GongXiangWenJianList.wenJianJiaId = item.Id;
                    GongXiangWenJianList.qxtype = 1;
                    GongXiangWenJianList.blobtype = "1";
                    list.Add(GongXiangWenJianList);
                }
                var WangPan_GongXiangMenu2 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ShangChuan.Contains(memberid)).ToList();
                var strlist = list.Select(t => t.wenJianJiaId).ToList();
                foreach (var item in WangPan_GongXiangMenu2)
                {
                    if (strlist.Contains(item.Id))
                    {
                        continue;
                    }
                    else
                    {
                        var WangPan_GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.UId == item.Id).ToList();
                        var Member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.ChuanJian).FirstOrDefault();
                        var Member_Team = _JointOfficeContext.Member_Team.Where(t => t.Id == item.TeamId).FirstOrDefault();
                        var length = WangPan_GongXiangWenJian.Sum(t => t.length);
                        ListPara GongXiangWenJianList = new ListPara();
                        GongXiangWenJianList.length = "-";
                        GongXiangWenJianList.name = item.Name;
                        GongXiangWenJianList.person = Member_Info.Name;
                        //GongXiangWenJianList.qunzuname = Member_Team.Name;
                        GongXiangWenJianList.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                        GongXiangWenJianList.wenJianJiaId = item.Id;
                        GongXiangWenJianList.qxtype = 2;
                        GongXiangWenJianList.blobtype = "1";
                        list.Add(GongXiangWenJianList);
                    }
                }
                var WangPan_GongXiangMenu3 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ChaKan.Contains(memberid)).ToList();
                strlist = list.Select(t => t.wenJianJiaId).ToList();
                foreach (var item in WangPan_GongXiangMenu3)
                {
                    if (strlist.Contains(item.Id))
                    {
                        continue;
                    }
                    else
                    {
                        var WangPan_GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.UId == item.Id).ToList();
                        var Member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.ChuanJian).FirstOrDefault();
                        var Member_Team = _JointOfficeContext.Member_Team.Where(t => t.Id == item.TeamId).FirstOrDefault();
                        var length = WangPan_GongXiangWenJian.Sum(t => t.length);
                        ListPara GongXiangWenJianList = new ListPara();
                        GongXiangWenJianList.length = "-";
                        GongXiangWenJianList.name = item.Name;
                        GongXiangWenJianList.person = Member_Info.Name;
                        //GongXiangWenJianList.qunzuname = Member_Team.Name;
                        GongXiangWenJianList.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                        GongXiangWenJianList.wenJianJiaId = item.Id;
                        GongXiangWenJianList.qxtype = 3;
                        GongXiangWenJianList.blobtype = "1";
                        list.Add(GongXiangWenJianList);
                    }
                }

            }
            else if (para.wenJianJiaId == "qiye")
            {
                var Member_Team = _JointOfficeContext.Member_Team.Where(t => t.TeamPerson.Contains(memberid) || t.MemberId == memberid).ToList();
                foreach (var item in Member_Team)
                {
                    var list2 = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == item.Id || t.TeamId == item.Id).ToList();
                    if (list2.Count() > 0)
                    {
                        var Member_Team1 = _JointOfficeContext.Member_Team.Where(t => t.Id == item.Id).FirstOrDefault();
                        var WangPan_QiYeMenu = new List<WangPan_QiYeMenu>();
                        if (Member_Team1 != null)
                        {
                            WangPan_QiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.TeamId == item.Id && (t.ParentId == "0" || t.ParentId == "1")).ToList();

                        }
                        else
                        {
                            WangPan_QiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.ParentId == item.Id).ToList();
                        }
                        if (WangPan_QiYeMenu.Count() > 0)
                        {
                            foreach (var one in WangPan_QiYeMenu)
                            {
                                if (para.type == 0)
                                {
                                    ListPara ListPara = new ListPara();
                                    ListPara.blobtype = "1";
                                    ListPara.length = "-";
                                    ListPara.wenJianJiaId = one.Id;
                                    ListPara.name = one.Name;
                                    ListPara.person = _JointOfficeContext.Member_Info.Where(t => t.MemberId == one.MemberId).FirstOrDefault().Name;
                                    ListPara.url = "";
                                    ListPara.date = one.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                                    if (one.Name == "公共区")
                                    {
                                        ListPara.display = "shang";
                                    }
                                    else
                                    {
                                        ListPara.display = "";
                                    }
                                    if (one.MemberId == memberid)
                                    {
                                        ListPara.qxtype = 1;
                                    }
                                    else
                                    {
                                        ListPara.qxtype = 2;
                                    }
                                    list.Add(ListPara);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var list1 = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == para.wenJianJiaId).ToList();
                if (list1.Count() > 0)
                {
                    var WangPan_Menu = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == para.wenJianJiaId).ToList();
                    if (WangPan_Menu.Count() > 0)
                    {
                        foreach (var item in WangPan_Menu)
                        {
                            if (para.type == 0)
                            {
                                ListPara ListPara = new ListPara();
                                ListPara.blobtype = "1";
                                ListPara.length = "-";
                                ListPara.wenJianJiaId = item.Id;
                                ListPara.name = item.Name;
                                ListPara.url = "";
                                ListPara.qxtype = 1;
                                ListPara.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                                list.Add(ListPara);
                            }
                        }
                    }
                    var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.MenuId == para.wenJianJiaId).ToList();
                    if (WangPan_WenJian.Count() > 0)
                    {
                        foreach (var item in WangPan_WenJian)
                        {
                            if (para.type == 0 || para.type == item.type)
                            {
                                //string[] b = item.url.Split(new Char[] { '/' });
                                //var name = b.Last();
                                ListPara ListPara = new ListPara();
                                ListPara.blobtype = "2";
                                ListPara.length = BusinessHelper.ConvertBytes(item.length);
                                ListPara.wenJianJiaId = item.Id;
                                ListPara.name = item.FileName;
                                ListPara.type = item.type;
                                ListPara.qxtype = 1;
                                ListPara.url = item.url + SasKey;
                                ListPara.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                                list.Add(ListPara);
                            }
                        }
                    }
                }
                var list2 = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == para.wenJianJiaId || t.TeamId == para.wenJianJiaId).ToList();
                if (list2.Count() > 0)
                {
                    var Member_Team = _JointOfficeContext.Member_Team.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
                    var WangPan_QiYeMenu = new List<WangPan_QiYeMenu>();
                    if (Member_Team != null)
                    {
                        WangPan_QiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.TeamId == para.wenJianJiaId && (t.ParentId == "0" || t.ParentId == "1")).ToList();

                    }
                    else
                    {
                        WangPan_QiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.ParentId == para.wenJianJiaId).ToList();
                    }
                    if (WangPan_QiYeMenu.Count() > 0)
                    {
                        foreach (var item in WangPan_QiYeMenu)
                        {
                            if (para.type == 0)
                            {
                                ListPara ListPara = new ListPara();
                                ListPara.blobtype = "1";
                                ListPara.length = "-";
                                ListPara.wenJianJiaId = item.Id;
                                ListPara.name = item.Name;
                                ListPara.person = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault().Name;
                                ListPara.url = "";
                                ListPara.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                                if (item.Name == "公共区")
                                {
                                    ListPara.display = "shang";
                                }
                                else
                                {
                                    ListPara.display = "";
                                }
                                if (item.MemberId == memberid)
                                {
                                    ListPara.qxtype = 1;
                                }
                                else
                                {
                                    ListPara.qxtype = 2;
                                }
                                list.Add(ListPara);
                            }
                        }
                    }
                    var WangPan_QiYeWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.MenuId == para.wenJianJiaId).ToList();
                    if (WangPan_QiYeWenJian.Count() > 0)
                    {
                        foreach (var item in WangPan_QiYeWenJian)
                        {
                            if (para.type == 0 || para.type == item.type)
                            {
                                //string[] b = item.url.Split(new Char[] { '/' });
                                //var name = b.Last();
                                ListPara ListPara = new ListPara();
                                ListPara.blobtype = "2";
                                ListPara.length = BusinessHelper.ConvertBytes(item.length);
                                ListPara.wenJianJiaId = item.Id;
                                ListPara.name = item.FileName;
                                ListPara.type = item.type;
                                ListPara.person = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault().Name;
                                ListPara.url = item.url + SasKey;
                                ListPara.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                                if (item.MemberId == memberid)
                                {
                                    ListPara.qxtype = 1;
                                }
                                else
                                {
                                    ListPara.qxtype = 2;
                                }
                                list.Add(ListPara);
                            }
                        }
                    }

                }
                var list3 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == para.wenJianJiaId).ToList();
                if (list3.Count() > 0)
                {
                    var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ParentId == para.wenJianJiaId).ToList();
                    if (WangPan_GongXiangMenu.Count() > 0)
                    {
                        foreach (var item in WangPan_GongXiangMenu)
                        {
                            if (para.type == 0)
                            {
                                ListPara ListPara = new ListPara();
                                ListPara.blobtype = "1";
                                ListPara.length = "-";
                                ListPara.wenJianJiaId = item.Id;
                                ListPara.name = item.Name;
                                ListPara.qxtype = 1;
                                ListPara.uid = item.Uid;
                                ListPara.person = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.ChuanJian).FirstOrDefault().Name;
                                ListPara.url = "";
                                ListPara.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                                list.Add(ListPara);
                            }
                        }
                    }
                    var WangPan_GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.MenuId == para.wenJianJiaId).ToList();
                    if (WangPan_GongXiangWenJian.Count() > 0)
                    {
                        foreach (var item in WangPan_GongXiangWenJian)
                        {
                            if (para.type == 0 || para.type == item.type)
                            {
                                //string[] b = item.url.Split(new Char[] { '/' });
                                //var name = b.Last();
                                ListPara ListPara = new ListPara();
                                ListPara.blobtype = "2";
                                ListPara.length = BusinessHelper.ConvertBytes(item.length);
                                ListPara.wenJianJiaId = item.Id;
                                ListPara.name = item.FileName;
                                ListPara.type = item.type;
                                ListPara.qxtype = 1;
                                ListPara.uid = item.UId;
                                ListPara.person = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault().Name;
                                ListPara.url = item.url + SasKey;
                                ListPara.date = item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                                list.Add(ListPara);
                            }
                        }
                    }
                }
            }
            if (list.Count() > 0)
            {
                if (para.shunxu == "name")
                {
                    Reslist = list.OrderBy(t => t.name).Skip(para.page * para.count).Take(para.count).ToList();
                }
                else
                {
                    Reslist = list.OrderByDescending(t => t.date).Skip(para.page * para.count).Take(para.count).ToList();
                }
            }
            var allPages = list.Count() / para.count;
            if (list.Count() % para.count != 0)
            {
                allPages += 1;
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<ListPara>();
            res.showapi_res_body.contentlist = Reslist;
            res.showapi_res_body.allPages = allPages;
            res.showapi_res_body.allNum = list.Count();
            return res;
        }
        /// <summary>
        /// 移动文件或文件夹
        /// </summary>
        /// <param name="之前文件夹，现在文件夹，文件或文件夹List"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge MoveWenJian(MoveWenJianJiaPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            if (para.oldwenJianJiaId == "0")
            {
                var zhuwenjian = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == "0" && t.MemberId == memberid).FirstOrDefault();
                para.oldwenJianJiaId = zhuwenjian.Id;
            }
            if (para.wenJianJiaId == "0")
            {
                var zhuwenjian = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == "0" && t.MemberId == memberid).FirstOrDefault();
                para.wenJianJiaId = zhuwenjian.Id;
            }
            var list1 = para.MovePara.Where(t => t.type == 1).ToList();
            var list2 = para.MovePara.Where(t => t.type == 2).ToList();
            var str = "";

            List<string> list = new List<string>();

            if (list1.Count!=0)
            {
                foreach (var item in list1)
                {
                    var wenjianmulu = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                    if (wenjianmulu != null)
                    {
                        var filelist = new filelist();
                        list.Add(item.wenJianJiaId);
                        var WangPanWay = new WangPanWay(config, _JointOfficeContext, _PrincipalBase);
                        list = WangPanWay.getfileIdList(item.wenJianJiaId, list);
                        if (list.Contains(para.wenJianJiaId))
                        {
                            throw new BusinessTureException("不能将文件移动到自身目录下");
                        }
                    }
                    var wenjianmulu1 = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                    if (wenjianmulu1 != null)
                    {
                        var filelist = new filelist();
                        list.Add(item.wenJianJiaId);
                        var WangPanWay = new WangPanWay(config, _JointOfficeContext, _PrincipalBase);
                        list = WangPanWay.getgongxiangfileIdList(item.wenJianJiaId, list);
                        if (list.Contains(para.wenJianJiaId))
                        {
                            throw new BusinessTureException("不能将文件移动到自身目录下");
                        }
                    }
                }
            }


            if (list2.Count() > 0)
            {
                if (para.oldwenJianJiaId == "0")
                {
                    var WangPan_Menu = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == "0" && t.MemberId == memberid).FirstOrDefault();
                    if (WangPan_Menu != null)
                    {
                        para.oldwenJianJiaId = WangPan_Menu.Id;
                    }
                }

                var WangPan_WenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.MenuId == para.oldwenJianJiaId).FirstOrDefault();
                if (WangPan_WenJian != null)
                {
                    var WangPan_WenJianList = _JointOfficeContext.WangPan_WenJian.Where(t => t.MenuId == para.wenJianJiaId).ToList();

                    foreach (var item in list2)
                    {
                        if (WangPan_WenJianList.Count() == 0)
                        {
                            var OneWenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                            OneWenJian.MenuId = para.wenJianJiaId;
                        }
                        else
                        {
                            var OneWenJian1 = WangPan_WenJianList.Where(t => t.FileName == item.wenJianJiaName).FirstOrDefault();
                            if (OneWenJian1 != null)
                            {
                                str = item.wenJianJiaName + " ";
                            }
                            else
                            {
                                var OneWenJian = _JointOfficeContext.WangPan_WenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                                OneWenJian.MenuId = para.wenJianJiaId;
                            }
                        }
                    }
                }
                var WangPan_QiYeWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.MenuId == para.oldwenJianJiaId).FirstOrDefault();
                if (WangPan_QiYeWenJian != null)
                {
                    var WangPan_QiYeWenJianList = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.MenuId == para.wenJianJiaId).ToList();

                    foreach (var item in list2)
                    {
                        if (WangPan_QiYeWenJianList.Count() == 0)
                        {
                            var OneWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                            OneWenJian.MenuId = para.wenJianJiaId;
                        }
                        else
                        {
                            var OneWenJian1 = WangPan_QiYeWenJianList.Where(t => t.FileName == item.wenJianJiaName).FirstOrDefault();
                            if (OneWenJian1 != null)
                            {
                                str = item.wenJianJiaName + " ";
                            }
                            else
                            {
                                var OneWenJian = _JointOfficeContext.WangPan_QiYeWenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                                OneWenJian.MenuId = para.wenJianJiaId;
                            }
                        }
                    }
                }
                var WangPan_GongXiangWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.MenuId == para.oldwenJianJiaId).FirstOrDefault();
                if (WangPan_GongXiangWenJian != null)
                {
                    var WangPan_GongXiangWenJianList = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.MenuId == para.wenJianJiaId).ToList();

                    foreach (var item in list2)
                    {
                        if (WangPan_GongXiangWenJianList.Count() == 0)
                        {
                            var OneWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                            OneWenJian.MenuId = para.wenJianJiaId;
                        }
                        else
                        {
                            var OneWenJian1 = WangPan_GongXiangWenJianList.Where(t => t.FileName == item.wenJianJiaName).FirstOrDefault();
                            if (OneWenJian1 != null)
                            {
                                str = item.wenJianJiaName + " ";
                            }
                            else
                            {
                                var OneWenJian = _JointOfficeContext.WangPan_GongXiangWenJian.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                                OneWenJian.MenuId = para.wenJianJiaId;
                            }
                        }
                    }
                }
            }
            if (list1.Count() > 0)
            {
                var WangPan_Menu = new WangPan_Menu();
                if (para.oldwenJianJiaId == "0")
                {
                    WangPan_Menu = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == "0" && t.MemberId == memberid).FirstOrDefault();
                }
                else
                {
                    WangPan_Menu = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == para.oldwenJianJiaId).FirstOrDefault();
                }
                if (WangPan_Menu != null)
                {
                    var WangPan_MenuList = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == para.wenJianJiaId).ToList();
                    foreach (var item in list1)
                    {
                        if (WangPan_MenuList.Count == 0)
                        {
                            var OneMenu = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                            OneMenu.ParentId = para.wenJianJiaId;

                        }
                        else
                        {
                            var OneMenu1 = WangPan_MenuList.Where(t => t.Name == item.wenJianJiaName).FirstOrDefault();
                            if (OneMenu1 != null)
                            {
                                str = item.wenJianJiaName + " ";
                            }
                            else
                            {
                                var OneMenu = _JointOfficeContext.WangPan_Menu.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                                OneMenu.ParentId = para.wenJianJiaId;
                            }
                        }
                    }
                }
                var WangPan_QiYeMenu = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == para.oldwenJianJiaId).FirstOrDefault();
                if (WangPan_QiYeMenu != null)
                {
                    var WangPan_QiYeMenuList = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.ParentId == para.wenJianJiaId).ToList();
                    foreach (var item in list1)
                    {
                        if (WangPan_QiYeMenuList.Count == 0)
                        {
                            var OneWenJian = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                            OneWenJian.ParentId = para.wenJianJiaId;
                        }
                        else
                        {
                            var OneWenJian1 = WangPan_QiYeMenuList.Where(t => t.Name == item.wenJianJiaName).FirstOrDefault();
                            if (OneWenJian1 != null)
                            {
                                str = item.wenJianJiaName + " ";
                            }
                            else
                            {
                                var OneWenJian = _JointOfficeContext.WangPan_QiYeMenu.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                                OneWenJian.ParentId = para.wenJianJiaId;
                            }
                        }
                    }
                }
                var WangPan_GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == para.oldwenJianJiaId).FirstOrDefault();
                if (WangPan_GongXiangMenu != null)
                {
                    var WangPan_GongXiangMenuList = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.ParentId == para.wenJianJiaId).ToList();
                    foreach (var item in list1)
                    {
                        if (WangPan_GongXiangMenuList.Count == 0)
                        {
                            var OneWenJian = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                            OneWenJian.ParentId = para.wenJianJiaId;
                        }
                        else
                        {
                            var OneWenJian1 = WangPan_GongXiangMenuList.Where(t => t.Name == item.wenJianJiaName).FirstOrDefault();
                            if (OneWenJian1 != null)
                            {
                                str = item.wenJianJiaName + " ";
                            }
                            else
                            {
                                var OneWenJian = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == item.wenJianJiaId).FirstOrDefault();
                                OneWenJian.ParentId = para.wenJianJiaId;
                            }
                        }
                    }
                }
            }
            _JointOfficeContext.SaveChanges();
            if (str != "")
            {
                throw new BusinessTureException("存在名称为" + str + "的文件或文件夹.");
            }

            return Message.SuccessMeaasge("移动成功");
        }
        /// <summary>
        /// 获取我的网盘所有文件夹
        /// </summary>
        /// <param name="页数，总数，文件夹ID，顺序，文件类型"></param>
        /// <returns></returns>
        public Showapi_Res_List<filelist> GetAllWangPanList(WangPanListPara para)
        {
            Showapi_Res_List<filelist> res = new Showapi_Res_List<filelist>();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<filelist>();
                return Return.Return();
            }
            //var memberid = "412f8982-140b-4157-a287-d45a8fb74e44";
            List<filelist> list = new List<filelist>();
            List<filelist> Reslist = new List<filelist>();
            var filelist = new filelist();
            filelist.label = "我的文件";
            filelist.wenJianJiaId = "0";
            list.Add(filelist);
            var WangPanWay = new WangPanWay(config, _JointOfficeContext, _PrincipalBase);
            list = WangPanWay.getfileList(list, memberid);
            //if (para.wenJianJiaId == "0")
            //{
            //    var info = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == "0" && t.MemberId == memberid).FirstOrDefault();
            //    if (info != null)
            //    {
            //        var WangPan_Menu = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == info.Id).ToList();
            //        if (WangPan_Menu.Count() > 0)
            //        {
            //            foreach (var item in WangPan_Menu)
            //            {
            //                filelist ListPara = new filelist();
            //                ListPara.wenJianJiaId = item.Id;
            //                ListPara.label = item.Name;
            //                list.Add(ListPara);
            //            }
            //            var WangPanWay = new WangPanWay(config, _JointOfficeContext,_PrincipalBase);
            //            list = WangPanWay.getfileList(list);
            //        }
            //    }
            //}
            var allPages = list.Count() / para.count;
            if (list.Count() % para.count != 0)
            {
                allPages += 1;
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<filelist>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allPages = allPages;
            return res;
        }
        /// <summary>
        /// 获取共享文件所有文件夹
        /// </summary>
        /// <param name="页数，总数，文件夹ID，顺序，文件类型"></param>
        /// <returns></returns>
        public Showapi_Res_List<filelist> GetGongXiangWangPanList(WangPanListPara para)
        {
            Showapi_Res_List<filelist> res = new Showapi_Res_List<filelist>();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<filelist>();
                return Return.Return();
            }
            //var memberid = "412f8982-140b-4157-a287-d45a8fb74e44";
            List<filelist> list = new List<filelist>();
            List<filelist> Reslist = new List<filelist>();
            var GongXiangMenu = _JointOfficeContext.WangPan_GongXiangMenu.Where(t => t.Id == para.wenJianJiaId).FirstOrDefault();
            var filelist = new filelist();
            filelist.label = GongXiangMenu.Name;
            filelist.wenJianJiaId = GongXiangMenu.Id;
            list.Add(filelist);
            var WangPanWay = new WangPanWay(config, _JointOfficeContext, _PrincipalBase);
            list = WangPanWay.getgongxiangfileList(list, memberid);
            //if (para.wenJianJiaId == "0")
            //{
            //    var info = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == "0" && t.MemberId == memberid).FirstOrDefault();
            //    if (info != null)
            //    {
            //        var WangPan_Menu = _JointOfficeContext.WangPan_Menu.Where(t => t.ParentId == info.Id).ToList();
            //        if (WangPan_Menu.Count() > 0)
            //        {
            //            foreach (var item in WangPan_Menu)
            //            {
            //                filelist ListPara = new filelist();
            //                ListPara.wenJianJiaId = item.Id;
            //                ListPara.label = item.Name;
            //                list.Add(ListPara);
            //            }
            //            var WangPanWay = new WangPanWay(config, _JointOfficeContext,_PrincipalBase);
            //            list = WangPanWay.getfileList(list);
            //        }
            //    }
            //}
            var allPages = list.Count() / para.count;
            if (list.Count() % para.count != 0)
            {
                allPages += 1;
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<filelist>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allPages = allPages;
            return res;
        }
    }
}
