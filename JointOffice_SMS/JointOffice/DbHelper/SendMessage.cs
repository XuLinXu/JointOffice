using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace JointOffice.DbHelper
{
    public class SendMessage
    {
        public static string Post(string url, string data, Encoding encoding)
        {
            try
            {
                HttpWebRequest req = WebRequest.CreateHttp(new Uri(url));
                req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                req.Method = "POST";
                req.Accept = "text/xml,text/javascript";
                req.ContinueTimeout = 60000;

                byte[] postData = encoding.GetBytes(data);
                System.IO.Stream reqStream = req.GetRequestStreamAsync().Result;
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Dispose();

                var rsp = (HttpWebResponse)req.GetResponseAsync().Result;
                var result = GetResponseAsString(rsp, encoding);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static T Post<T>(string url, string data, Encoding encoding)
        {
            try
            {
                var result = Post(url, data, encoding);
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return null;
            }

            StringBuilder query = new StringBuilder();
            bool hasParam = false;

            foreach (KeyValuePair<string, string> kv in parameters)
            {
                string name = kv.Key;
                string value = kv.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        query.Append("&");
                    }

                    query.Append(name);
                    query.Append("=");
                    query.Append(WebUtility.UrlEncode(value));
                    hasParam = true;
                }
            }

            return query.ToString();
        }

        public static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Dispose();
                if (stream != null) stream.Dispose();
                if (rsp != null) rsp.Dispose();
            }
        }

        public static string GetAlidayuSign(IDictionary<string, string> parameters, string secret, string signMethod)
        {
            //把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);

            //把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            if (Constants.SIGN_METHOD_MD5.Equals(signMethod))
            {
                query.Append(secret);
            }
            foreach (KeyValuePair<string, string> kv in sortedParams)
            {
                if (!string.IsNullOrEmpty(kv.Key) && !string.IsNullOrEmpty(kv.Value))
                {
                    query.Append(kv.Key).Append(kv.Value);
                }
            }

            //使用MD5/HMAC加密
            if (Constants.SIGN_METHOD_HMAC.Equals(signMethod))
            {
                return Hmac(query.ToString(), secret);
            }
            else
            {
                query.Append(secret);
                return Md5(query.ToString());
            }
        }

        public static string Hmac(string value, string key)
        {
            byte[] bytes;
            using (var hmac = new HMACMD5(Encoding.UTF8.GetBytes(key)))
            {
                bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            StringBuilder result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }

        public static string Md5(string value)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }

        public static SmsResultAli SendSms(string url, string appKey, string appSecret, DateTime timestamp, Dictionary<string, string> parsms)
        {
            var txtParams = new SortedDictionary<string, string>();
            txtParams.Add(Constants.METHOD, "alibaba.aliqin.fc.sms.num.send");
            txtParams.Add(Constants.VERSION, "2.0");
            txtParams.Add(Constants.SIGN_METHOD, Constants.SIGN_METHOD_HMAC);
            txtParams.Add(Constants.APP_KEY, appKey);
            txtParams.Add(Constants.FORMAT, "json");
            txtParams.Add(Constants.TIMESTAMP, timestamp.ToString(Constants.DATE_TIME_FORMAT));
            txtParams.Add(Constants.SMS_TYPE, "normal");
            foreach (var item in parsms)
            {
                txtParams.Add(item.Key, item.Value);
            }
            txtParams.Add(Constants.SIGN, GetAlidayuSign(txtParams, appSecret, Constants.SIGN_METHOD_HMAC));
            var result = Post<SmsResultAli>(url, BuildQuery(txtParams), Encoding.UTF8);
            return result;
        }
        public static SmsResultAli SendVoiceSms(string url, string appKey, string appSecret, DateTime timestamp, Dictionary<string, string> parsms)
        {
            var txtParams = new SortedDictionary<string, string>();
            txtParams.Add(Constants.METHOD, "alibaba.aliqin.fc.tts.num.singlecall");
            txtParams.Add(Constants.VERSION, "2.0");
            txtParams.Add(Constants.SIGN_METHOD, Constants.SIGN_METHOD_HMAC);
            txtParams.Add(Constants.APP_KEY, appKey);
            txtParams.Add(Constants.FORMAT, "json");
            txtParams.Add(Constants.TIMESTAMP, timestamp.ToString(Constants.DATE_TIME_FORMAT));
            foreach (var item in parsms)
            {
                txtParams.Add(item.Key, item.Value);
            }
            txtParams.Add(Constants.SIGN, GetAlidayuSign(txtParams, appSecret, Constants.SIGN_METHOD_HMAC));
            var result = Post<SmsResultAli>(url, BuildQuery(txtParams), Encoding.UTF8);
            return result;
        }
    }
    public sealed class Constants
    {
        public const string ACCEPT_ENCODING = "Accept-Encoding";
        public const string APP_KEY = "app_key";
        public const string CHARSET_UTF8 = "utf-8";
        public const string CONTENT_ENCODING = "Content-Encoding";
        public const string CONTENT_ENCODING_GZIP = "gzip";
        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DATE_TIME_MS_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";
        public const string ERROR_CODE = "code";
        public const string ERROR_MSG = "msg";
        public const string ERROR_RESPONSE = "error_response";
        public const string FORMAT = "format";
        public const string FORMAT_JSON = "json";
        public const string FORMAT_XML = "xml";
        public const string LOG_FILE_NAME = "topsdk.log";
        public const string LOG_SPLIT = "^_^";
        public const string METHOD = "method";
        public const string MIME_TYPE_DEFAULT = "application/octet-stream";
        public const string PARTNER_ID = "partner_id";
        public const string QM_CONTENT_TYPE = "text/xml;charset=utf-8";
        public const string QM_CUSTOMER_ID = "customerId";
        public const string QM_ROOT_TAG_REQ = "request";
        public const string QM_ROOT_TAG_RSP = "response";
        public const int READ_BUFFER_SIZE = 4096;
        public const string SDK_VERSION = "top-sdk-net-20160426";
        public const string SDK_VERSION_CLUSTER = "top-sdk-net-cluster-20160426";
        public const string SESSION = "session";
        public const string SIGN = "sign";
        public const string SIGN_METHOD = "sign_method";
        public const string SIGN_METHOD_HMAC = "hmac";
        public const string SIGN_METHOD_MD5 = "md5";
        public const string SIMPLIFY = "simplify";
        public const string TARGET_APP_KEY = "target_app_key";
        public const string TIMESTAMP = "timestamp";
        public const string VERSION = "v";
        public const string EXTEND = "extend";
        public const string REC_NUM = "rec_num";
        public const string SMS_FREE_SIGN_NAME = "sms_free_sign_name";
        public const string SMS_PARAM = "sms_param";
        public const string SMS_TEMPLATE_CODE = "sms_template_code";
        public const string SMS_TYPE = "sms_type";
        public const string CALLED_NUM = "called_num";
        public const string CALLED_SHOW_NUM = "called_show_num";
        public const string TTS_CODE = "tts_code";
    }
    public class SmsResultAli
    {
        public SmsResponseALi Alibaba_Aliqin_Fc_Sms_Num_Send_Response { get; set; }
    }
    public class VoiceSmsResultAli
    {
        public SmsResponseALi Alibaba_Aliqin_Fc_Tts_Num_Singlecall_Response { get; set; }
    }
    public class SmsResponseALi
    {
        public string Request_Id { get; set; }
        public SmsResponseResultAli Result { get; set; }
    }

    public class SmsResponseResultAli
    {
        public string Err_Code { get; set; }

        public string Model { get; set; }

        public bool Success { get; set; }
    }
}

