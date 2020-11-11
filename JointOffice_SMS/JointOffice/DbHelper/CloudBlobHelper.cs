using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.Models;

namespace JointOffice.DbHelper
{
    public class CloudBlobHelper
    {
        //删除一天前的临时文件
        public static void DeleteDicFiles()
        {
            //删除一天前的临时文件
            //进程正在被使用
            try
            {
                string path = Directory.GetCurrentDirectory() + @"\wwwroot";
                if (Directory.Exists(path) == false)
                {
                    throw new Exception("路径无效");
                }
                DirectoryInfo dir = new DirectoryInfo(path);
                FileInfo[] deletefiles = dir.GetFiles();
                foreach (var item in deletefiles)
                {
                    if (item.CreationTime < DateTime.Now.AddDays(-1))
                        System.IO.File.Delete(item.FullName);
                    //File.Delete(item.FullName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("删除文件失败." + ex.Message);
            }
        }
        //创建所有的blobFiles
        //循环找出stream和fileType和fileName
        public static List<BlobFilePara> SelectBlobFiles(IFormFileCollection files, string name)
        {
            List<BlobFilePara> blobFiles = new List<BlobFilePara>();
            //var dirFilename = "";
            //var mp3SavePth = "";
            foreach (var file in files)
            {
                var oneFile = new BlobFilePara();
                //var fileNameType = Guid.NewGuid().ToString("N") + "." + file.FileName.Split('.').Last();
                oneFile.fileYName = file.FileName;
                oneFile.filelength = file.Length;
                oneFile.fileurl = "https://ygsrs.blob.core.chinacloudapi.cn/spjointoffice/" + name + "/" + file.FileName;
                oneFile.fileName =  name + "/" + file.FileName;
                oneFile.fileContent = file.OpenReadStream();
                var HouZhui =file.FileName.Split('.').Last();
                if (HouZhui == "doc"|| HouZhui == "docx" || HouZhui == "xls" || HouZhui == "xlsx" || HouZhui == "txt" || HouZhui == "pptx" || HouZhui == "pdf" || HouZhui == "ppt")
                {
                    oneFile.filetype = 1;
                }
                else if (HouZhui == "rar" || HouZhui == "zip" || HouZhui == "cab" || HouZhui == "arj" || HouZhui == "tar")
                {
                    oneFile.filetype = 2;
                }
                else if (HouZhui == "bmp" || HouZhui == "gif" || HouZhui == "jpeg" || HouZhui == "png" || HouZhui == "jpg")
                {
                    oneFile.filetype = 3;
                }
                else if (HouZhui == "mp4" || HouZhui == "avi" || HouZhui == "3gp" || HouZhui == "rmvb" || HouZhui == "mov")
                {
                    oneFile.filetype = 4;
                }
                else if (HouZhui == "mp3" || HouZhui == "wav" || HouZhui == "aac" || HouZhui == "wma" || HouZhui == "cda" || HouZhui == "amr")
                {
                    oneFile.filetype = 5;
                }
                else if (HouZhui == "exe" || HouZhui == "app" || HouZhui == "vbp" || HouZhui == "frm" || HouZhui == "jar")
                {
                    oneFile.filetype = 6;
                }
                else
                {
                    oneFile.filetype = 7;
                }
                blobFiles.Add(oneFile);
            }
            return blobFiles;
        }
        public static List<BlobFilePara> SelectBlobFiles(IFormFileCollection files)
        {
            List<BlobFilePara> blobFiles = new List<BlobFilePara>();
            //var dirFilename = "";
            //var mp3SavePth = "";
            foreach (var file in files)
            {
                //var oneFile = new BlobFilePara();
                var fileNameType = Guid.NewGuid().ToString("N") + "." + file.FileName.Split('.').Last();
                //oneFile.fileYName = file.FileName;
                //oneFile.filelength = file.Length;
                //图片
                if (file.Name == "image")
                {
                    var oneFile = new BlobFilePara();
                    //var fileNameType = Guid.NewGuid().ToString("N") + "." + file.FileName.Split('.').Last();
                    oneFile.fileYName = file.FileName;
                    oneFile.filelength = file.Length;
                    oneFile.fileurl = "https://ygsrs.blob.core.chinacloudapi.cn/spjointoffice/images/" + fileNameType;
                    oneFile.fileName = "images/" + fileNameType;
                    oneFile.fileContent = file.OpenReadStream();
                    oneFile.filetype = 1;
                    oneFile.annexfiletype = 3;
                    blobFiles.Add(oneFile);
                }
                //录音
                else if (file.Name == "audio")
                {
                    var oneFile = new BlobFilePara();
                    //var fileNameType = Guid.NewGuid().ToString("N") + "." + file.FileName.Split('.').Last();
                    oneFile.fileYName = file.FileName;
                    oneFile.filelength = file.Length;
                    oneFile.fileurl = "https://ygsrs.blob.core.chinacloudapi.cn/spjointoffice/audios/" + fileNameType;
                    oneFile.fileName = "audios/" + fileNameType;
                    oneFile.fileContent = file.OpenReadStream();
                    oneFile.filetype = 2;
                    oneFile.annexfiletype = 5;
                    blobFiles.Add(oneFile);
                }
                //附件
                else if (file.Name == "attach")
                {
                    var oneFile = new BlobFilePara();
                    //var fileNameType = Guid.NewGuid().ToString("N") + "." + file.FileName.Split('.').Last();
                    oneFile.fileYName = file.FileName;
                    oneFile.filelength = file.Length;
                    oneFile.fileurl = "https://ygsrs.blob.core.chinacloudapi.cn/spjointoffice/attachs/" + fileNameType;
                    oneFile.fileName = "attachs/" + fileNameType;
                    oneFile.fileContent = file.OpenReadStream();
                    oneFile.filetype = 3;
                    var HouZhui = file.FileName.Split('.').Last();
                    if (HouZhui == "doc" || HouZhui == "docx" || HouZhui == "xls" || HouZhui == "xlsx" || HouZhui == "txt" | HouZhui == "pptx" || HouZhui == "pdf" || HouZhui == "ppt")
                    {
                        oneFile.annexfiletype = 1;
                    }
                    else if (HouZhui == "rar" || HouZhui == "zip" || HouZhui == "cab" || HouZhui == "arj" || HouZhui == "tar")
                    {
                        oneFile.annexfiletype = 2;
                    }
                    else if (HouZhui == "bmp" || HouZhui == "gif" || HouZhui == "jpeg" || HouZhui == "png" || HouZhui == "jpg")
                    {
                        oneFile.annexfiletype = 3;
                    }
                    else if (HouZhui == "mp4" || HouZhui == "avi" || HouZhui == "3gp" || HouZhui == "rmvb" || HouZhui == "mov")
                    {
                        oneFile.annexfiletype = 4;
                    }
                    else if (HouZhui == "mp3" || HouZhui == "wav" || HouZhui == "aac" || HouZhui == "wma" || HouZhui == "cda" || HouZhui == "amr")
                    {
                        oneFile.annexfiletype = 5;
                    }
                    else if (HouZhui == "exe" || HouZhui == "app" || HouZhui == "vbp" || HouZhui == "frm" || HouZhui == "jar")
                    {
                        oneFile.annexfiletype = 6;
                    }
                    else
                    {
                        oneFile.annexfiletype = 7;
                    }
                    blobFiles.Add(oneFile);
                }
                else
                {
                    var oneFile = new BlobFilePara();
                    //var fileNameType = Guid.NewGuid().ToString("N") + "." + file.FileName.Split('.').Last();
                    oneFile.fileYName = file.FileName;
                    oneFile.filelength = file.Length;
                    oneFile.fileurl = "https://ygsrs.blob.core.chinacloudapi.cn/spjointoffice/images/" + fileNameType;
                    oneFile.fileName = "images/" + fileNameType;
                    oneFile.fileContent = file.OpenReadStream();
                    oneFile.filetype = 0;
                    oneFile.annexfiletype = 3;
                    oneFile.fileMName = file.Name;
                    blobFiles.Add(oneFile);
                }
            }
            return blobFiles;
        }

        //创建异步多线程服务完成
        public static void CreateAsyncTask(string StorageConnec, List<BlobFilePara> blobFiles)
        {
            Func<object, bool> action = (object obj) =>
            {
                BlobFilePara item = (BlobFilePara)obj;
                if (!CloudBlobHelper.SaveFileToCloudBlob(StorageConnec, item.fileContent, item.fileName, item.filePath).Result)
                    throw new Exception(item.fileName + "文件上传失败.");
                //return false;
                return true;
            };
            var tasks = new List<Task<bool>>();
            //循环上传
            foreach (var item in blobFiles)
            {
                tasks.Add(Task<bool>.Factory.StartNew(action, item));
            }
            try
            {
                //等待所有任务完成。
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException e)
            {
                throw new Exception("文件上传失败." + e.Message);
            }
        }
        public async static Task<bool> SaveFileToCloudBlob(string StorageConnec, System.IO.Stream liu, string leiXingFileName, string dirFilename)
        {
            try
            {
                //Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnec);
                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                // Retrieve a reference to a container.
                CloudBlobContainer container = blobClient.GetContainerReference("spjointoffice");

                //// Create the container if it doesn't already exist.
                await container.CreateIfNotExistsAsync();
                ////将容器设置为公共容器：
                //await container.SetPermissionsAsync(
                //    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Unknown });
                ////将容器设置为带令牌的
                //await container.SetPermissionsAsync(
                //    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Off });
                //容器中文件夹中文件：aaa/123.pug
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(leiXingFileName);
                // Create or overwrite the "myblob" blob with contents from a local file.
                //using (var fileStream = System.IO.File.OpenRead(fileUrl))
                //{
                await blockBlob.UploadFromStreamAsync(liu);
                liu.Dispose();
                //}
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private static string GetBlobSasUri(CloudBlobContainer container, string blobName, string policyName = null)
        {
            string sasBlobToken;

            // Get a reference to a blob within the container.
            // Note that the blob may not exist yet, but a SAS can still be created for it.
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (policyName == null)
            {
                // Create a new access policy and define its constraints.
                // Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad-hoc SAS, and
                // to construct a shared access policy that is saved to the container's shared access policies.
                SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
                {
                    // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                    // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(5),
                    Permissions = SharedAccessBlobPermissions.Read
                };

                // Generate the shared access signature on the blob, setting the constraints directly on the signature.
                sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);
            }
            else
            {
                // Generate the shared access signature on the blob. In this case, all of the constraints for the
                // shared access signature are specified on the container's stored access policy.
                sasBlobToken = blob.GetSharedAccessSignature(null, policyName);
            }
            // Return the URI string for the container, including the SAS token.
            return blob.Uri + sasBlobToken;
        }
    }
    public class BlobFilePara
    {
        public string filePath { get; set; }
        public string fileName { get; set; }
        public string fileYName { get; set; }
        public string fileurl { get; set; }
        public int filetype { get; set; }
        public int annexfiletype { get; set; }
        public long filelength { get; set; }
        public System.IO.Stream fileContent { get; set; }
        public bool fileYuanShuJu { get; set; }
        public string fileMName { get; set; }
    }
}
