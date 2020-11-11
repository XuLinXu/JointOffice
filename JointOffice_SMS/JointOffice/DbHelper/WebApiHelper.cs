using JointOffice.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace JointOffice.DbHelper
{
    public class WebApiHelper
    {
        //static string appkey = "k51hidwq1i1ub";
        //static string appsecret = "v7UN0Oy3u1O";
        //IOptions<Root> config;
        //string appkey;
        //string appsecret;
        //public WebApiHelper(IOptions<Root> config)
        //{
        //    this.config = config;
        //    appkey = this.config.Value.ConnectionStrings.appkey;
        //    appsecret = this.config.Value.ConnectionStrings.appsecret;
        //}
        public static  T PostAsynctMethod<T>(string actionUrl, string param, string appkey, string appsecret)
        {
            HttpClient _client = new HttpClient();
            string nonce = BusinessHelper.CreateRandomString(8);
            string timestamp = BusinessHelper.GetTimeStamp();
            string signStr = appsecret + nonce + timestamp;
            byte[] bytes = Encoding.UTF8.GetBytes(signStr);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(bytes);
            var sign = Encoding.UTF8.GetString(hash);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            _client.DefaultRequestHeaders.Add("App-Key", appkey);
            _client.DefaultRequestHeaders.Add("Nonce", nonce);
            _client.DefaultRequestHeaders.Add("Timestamp", timestamp);
            _client.DefaultRequestHeaders.Add("Signature", sb.ToString());
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/x-www-form-urlencoded");
            var res = _client.PostAsync(actionUrl, contentPost).Result;
            var resList = res.Content.ReadAsStringAsync().Result;
            var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resList);
            return objectList;
        }
        public static T PostAsynctMethod222<T>(string actionUrl, string param, string text)
        {
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("authtoken", text);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            var res = _client.PostAsync(actionUrl, contentPost).Result;
            var resList = res.Content.ReadAsStringAsync().Result;
            var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resList);
            return objectList;
        }
        public static T PostAsynctMethod<T>(string actionUrl, string param)
        {
            HttpClient _client = new HttpClient();
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            var res = _client.PostAsync(actionUrl, contentPost).Result;
            var resList = res.Content.ReadAsStringAsync().Result;
            var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resList);
            return objectList;
        }

        /// <summary>
        /// 同步get请求
        /// </summary>
        /// <param name="url">链接地址</param>    
        /// <param name="formData">写在header中的键值对</param>
        /// <returns></returns>

        public static string HttpGet(string url, List<KeyValuePair<string, string>> formData = null)
        {
            HttpClient httpClient = new HttpClient();
            if (formData != null)
            {
                HttpContent content = new FormUrlEncodedContent(formData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                content.Headers.ContentType.CharSet = "UTF-8";
                for (int i = 0; i < formData.Count; i++)
                {
                    content.Headers.Add(formData[i].Key, formData[i].Value);
                }
            }
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            if (formData != null)
            {
                for (int i = 0; i < formData.Count; i++)
                {
                    request.Headers.Add(formData[i].Key, formData[i].Value);
                }
            }
            var res = httpClient.SendAsync(request);
            res.Wait();
            var resp = res.Result;
            Task<string> temp = resp.Content.ReadAsStringAsync();
            temp.Wait();
            return temp.Result;

        }
    }
}
