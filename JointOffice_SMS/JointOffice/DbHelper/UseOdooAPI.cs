using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using JointOffice.Configuration;
using JointOffice.DbModel;
using JointOffice.Models;
using Microsoft.Extensions.Options;

namespace JointOffice.DbHelper
{
    public class UseOdooAPI
    {

        public static T PostAsynctMethod<T>(string actionUrl, string param, string mark, string token)
        {
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("mark", mark);
            _client.DefaultRequestHeaders.Add("token", token);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/soap+xml");
            var res = _client.PostAsync(actionUrl, contentPost).Result;
            var resList = res.Content.ReadAsStringAsync().Result;
            var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resList);
            return objectList;
        }
        public static T PostAsynctMethodLogin<T>(string actionUrl, string param, string mark)
        {
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("mark", mark);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/soap+xml");
            var res = _client.PostAsync(actionUrl, contentPost).Result;
            var resList = res.Content.ReadAsStringAsync().Result;
            var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resList);
            return objectList;
        }
        public static T GetAnyInfoOdoo<T>(string actionUrl, string param)
        {
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("mark", "Shopping");
            _client.DefaultRequestHeaders.Add("token", "");
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/soap+xml");
            var res = _client.PostAsync(actionUrl, contentPost).Result;
            var resList = res.Content.ReadAsStringAsync().Result;
            var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resList);
            return objectList;
        }
    }
}
