using JointOffice.Configuration;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IO;

namespace JointOffice.Controllers
{
    [Route("[controller]")]
    public class AttachmentFileController
    {
        JointOfficeContext _JointOfficeContext;
        ExceptionMessage em;
        IOptions<Root> config;
        public AttachmentFileController(IOptions<Root> config, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
        }

        /// <summary>
        /// 图片
        /// </summary>
        /// <returns></returns>
        [HttpGet("Upload/{id}")]
        public IActionResult Upload(string id)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory() + "\\AttachmentFile\\", "Upload");
            string filePath = directory + "\\" + id;
            //string file = "data:image/jpeg;base64," + FileToBase64(filePath);

            using (var sw = new FileStream(filePath, FileMode.Open))
            {
                var bytes = new byte[sw.Length];
                sw.Read(bytes, 0, bytes.Length);
                return new FileContentResult(bytes, "image/jpeg");
            }
        }
        /// <summary>
        /// 图片
        /// </summary>
        /// <returns></returns>
        [HttpGet("MailDetail/{id}")]
        public IActionResult MailDetail(string id)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory() + "\\AttachmentFile\\", "MailDetail");
            string filePath = directory + "\\" + id;
            //return "data:image/jpeg;base64," + FileToBase64(filePath);
            using (var sw = new FileStream(filePath, FileMode.Open))
            {
                var bytes = new byte[sw.Length];
                sw.Read(bytes, 0, bytes.Length);
                return new FileContentResult(bytes, "image/jpeg");
            }
        }
        /// <summary>
        /// 图片下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("Download/{id}/{name}")]
        public FileResult Download(string id,string name)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory() + "\\AttachmentFile\\", "Download");
            string filePath = directory + "\\" + id;
            ////return "data:image/jpeg;base64," + FileToBase64(filePath);
            //using (var sw = new FileStream(filePath, FileMode.Open))
            //{
            //    var bytes = new byte[sw.Length];
            //    sw.Read(bytes, 0, bytes.Length);
            //    return new FileContentResult(bytes, "image/jpeg");
            //}
            
            FileContentResult result = new FileContentResult(System.IO.File.ReadAllBytes(filePath), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = name
            };
            // return File("~/excels/report.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx"); // 返回 File + 路径也是可以, 这个路径是从 wwwroot 走起 
            // return File(await System.IO.File.ReadAllBytesAsync(path), same...) // 或则我们可以直接返回 byte[], 任意从哪里获取都可以. 

            return result;
        }
    }
}
