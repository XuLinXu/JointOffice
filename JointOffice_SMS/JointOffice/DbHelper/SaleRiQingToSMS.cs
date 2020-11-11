using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JointOffice.DbHelper
{
    public class SaleRiQingToSMS
    {
        public static string WebAPIServerUrl
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["WebApiServer"]; }
        }
        public static T PostAsynctMethod<T>(string actionUrl, string param, string job, string token)
        {
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("job", "CP001," + job);
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            var res = _client.PostAsync(actionUrl, contentPost).Result;
            var resList = res.Content.ReadAsStringAsync().Result;
            var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resList);
            return objectList;
        }
        public static T PostAsynctMethod_NoHeaders<T>(string actionUrl, string param)
        {
            HttpClient _client = new HttpClient();
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            var res = _client.PostAsync(actionUrl, contentPost).Result;
            var resList = res.Content.ReadAsStringAsync().Result;
            var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resList);
            return objectList;
        }
        public static T GetAsynctMethod<T>(string actionUrl, string job, string token)
        {
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("job", "CP001," + job);
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            //HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            var res = _client.GetAsync(actionUrl).Result;
            var resList = res.Content.ReadAsStringAsync().Result;
            var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resList);
            return objectList;
        }
    }
}
