using System;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using JointOffice.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace JointOffice.DbHelper
{
    public class BusinessHelper
    {
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AESEncrypt(string input, string key)
        {
            var encryptKey = Encoding.UTF8.GetBytes(key);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(encryptKey, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor,
                            CryptoStreamMode.Write))

                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result,
                            iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AESDecrypt(string input, string key)
        {
            var fullCipher = Convert.FromBase64String(input);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var decryptKey = Encoding.UTF8.GetBytes(key);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(decryptKey, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt,
                            decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        public static string GetMD5(string value)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("x2"));
            }
            return result.ToString();
        }
        public static string CreateRandomString(int strLength, params int[] Seed)
        {
            string strSep = ",";
            char[] chrSep = strSep.ToCharArray();
            string strChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] aryChar = strChar.Split(chrSep, strChar.Length);
            string strRandom = string.Empty;
            Random Rnd;
            if (Seed != null && Seed.Length > 0)
            {
                Rnd = new Random(Seed[0]);
            }
            else
            {
                Rnd = new Random();
            }
            //生成随机字符串
            for (int i = 0; i < strLength; i++)
            {
                strRandom += aryChar[Rnd.Next(aryChar.Length)];
            }
            return strRandom;
        }
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
        public static string ConvertBytes(long len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            decimal dml = Convert.ToDecimal(len);
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                dml = dml / 1024;
                len = len / 1024;
            }
            var str = dml.ToString("0.0") + sizes[order];
            return str;
        }
        //public static string GetNumber()
        //{
        //}
        public static string GetToken(string userid, string name, string appkey, string appsecret, string url)
        {
            string res = "";
            try
            {
                string postUrl = "http://api.cn.ronghub.com/user/getToken.json";
                string postStr = "";
                postStr += "userId=" + userid;
                postStr += "&name=" + name;
                postStr += "&portraitUri=" + url + "?sv=2016-05-31&ss=b&srt=sco&sp=r&se=2020-03-03T01:28:32Z&st=2017-01-01T17:28:32Z&spr=https,http&sig=xTsaDe8VLJQhRblFu6lTHdZCsAOGYBQ9wAhiGIS3NVY%3D";
                var postRes = WebApiHelper.PostAsynctMethod<RongYun>(postUrl, postStr, appkey, appsecret);
                if (postRes.code == "200")
                {
                    res = postRes.token;
                }
            }
            catch
            {
            }
            return res;
        }
        public static string GetGongShanInfo(string name, string appcode)
        {
            String host = "http://qianzhan1.market.alicloudapi.com";
            String path = "/CommerceAccurate";
            String querys = "comName=" + name + "&page=1";
            String url = host + path + "?" + querys;
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "APPCODE " + appcode);
            //HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            var res1 = _client.GetAsync(url).Result;
            var resList = res1.Content.ReadAsStringAsync().Result;
            return resList;
        }

        public static async Task<AttachmentUpload> FileSave(IFormFileCollection files, string host)
        {

            AttachmentUpload attachmentUpload = new AttachmentUpload();

            if (files.Count > 0)
            {
                #region 保存
                // 确定一个目录来保存内容
                var directory = Path.Combine(Directory.GetCurrentDirectory() + "\\AttachmentFile\\", "Upload");

                bool exists = Directory.Exists(directory);

                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(directory);
                }
                else
                {
                    //删除目录下文件
                    //foreach (var file in Directory.GetFiles(directory))
                    //{
                    //    try
                    //    {
                    //        FileInfo fileInfo = new FileInfo(file);

                    //        fileInfo.Delete();
                    //    }
                    //    catch (Exception e)
                    //    {

                    //    }
                    //}
                }
                #endregion
                AttachmentFiles attachmentFiles = new AttachmentFiles();
                foreach (IFormFile file in files)
                {
                    #region 扩展名
                    string extension = System.IO.Path.GetExtension(file.FileName);

                    if (!string.IsNullOrEmpty(extension) && ".exe".Equals(extension))
                    {
                        throw new Exception("出于安全性考虑，不允许添加以下可执行文件文件。");
                    }
                    #endregion

                    if (file.Length > 0)
                    {
                        string name = Guid.NewGuid().ToString("D");
                        string newFileName = name + extension;
                        string filePath = directory + "\\" + newFileName;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            stream.Flush();
                            stream.Dispose();
                        }
                        attachmentUpload.key = name;
                        attachmentUpload.path = host + "\\AttachmentFile\\Upload\\" + newFileName;
                        attachmentFiles.newfilename = newFileName;
                        attachmentFiles.filename = file.FileName;
                        attachmentFiles.file = "data:image/jpeg;base64," + FileToBase64(filePath);
                        attachmentUpload.files = attachmentFiles;
                        attachmentUpload.status = true;
                    }
                }
            }

            return attachmentUpload;
        }

        /// <summary>
        /// 文件转换成Base64字符串
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <returns></returns>
        public static String FileToBase64(string fileName)
        {
            string strRet = null;

            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    byte[] bt = new byte[fs.Length];
                    fs.Read(bt, 0, bt.Length);
                    strRet = Convert.ToBase64String(bt);
                    fs.Flush();
                    fs.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strRet;
        }
    }
    public class RongYun
    {
        public string code { get; set; }
        public string token { get; set; }
        public string userId { get; set; }
    }
}
