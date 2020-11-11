using JdSoft.Apple.Apns.Notifications;
using JointOffice.Configuration;
using JointOffice.DbHelper;
using JointOffice.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JointOffice.DbHelper
{
    public class SendHelper
    {
        IMemoryCache _memoryCache;
        IPrincipalBase _PrincipalBase;
        IOptions<Root> config;
        public SendHelper(IPrincipalBase IPrincipalBase, IMemoryCache memoryCache, IOptions<Root> config)
        {
            _PrincipalBase = IPrincipalBase;
            this.config = config;
            _memoryCache = memoryCache;
        }
        public void SendXiaoXi(string Mid, string memberid, string parameters, params object[] args)
        {
            var lnt = "";
            var lat = "";
            TokenManager TokenManager = new TokenManager(_memoryCache, _PrincipalBase, config);
            var Model_info = _PrincipalBase.GetModelInfo(memberid);
            if (Model_info != null)
            {
                MessageReposities MessageReposities = new MessageReposities();
                var Message = MessageReposities.GetMessageBody(Mid);
                //var content = string.Format(MessageReposities.GetMessageBody(Mid).Content, args);
                var content = Message.Content;
                var title = Message.Title;

                touchuanneirong touchuanneirongs = new touchuanneirong();
                touchuanneirongs.content = content;
                touchuanneirongs.title = title;
                touchuanneirongs.status = Message.PushId;
                touchuanneirongs.lnt = lnt;
                touchuanneirongs.lat = lat;
                touchuanneirongs.image = "";
                touchuanneirongs.Sign = Message.Sign;
                touchuanneirongs.Params = parameters;
                touchuanneirongs.isNotification = "true";
                touchuanneirong touchuanneirongsTouChuan = new touchuanneirong();
                touchuanneirongsTouChuan.content = content;
                touchuanneirongsTouChuan.title = title;
                touchuanneirongsTouChuan.status = Message.PushId;
                touchuanneirongsTouChuan.lnt = lnt;
                touchuanneirongsTouChuan.lat = lat;
                touchuanneirongsTouChuan.image = "";
                touchuanneirongsTouChuan.Sign = Message.Sign;
                touchuanneirongsTouChuan.Params = parameters;
                touchuanneirongsTouChuan.isNotification = "false";
                var paras = JsonConvert.SerializeObject(touchuanneirongs);
                var parasTouChuan = JsonConvert.SerializeObject(touchuanneirongsTouChuan);
                var Cid = Model_info.Cid;

                _PrincipalBase.AddSystem_Message(memberid, title, touchuanneirongs.content, touchuanneirongs.Params);
                var Token = TokenManager.GetToken("Token");
                var Retult = "";
                Retult = TokenManager.TouChuanXiaoXi(parasTouChuan, touchuanneirongs.title, touchuanneirongs.content, Cid, Token);
                if (Model_info.Device == "Android")
                {
                    Retult = TokenManager.SendXiaoXiGeTui(paras, touchuanneirongs.title, touchuanneirongs.content, Cid, Token);
                }
                else
                {
                    if (Model_info.Token != null && Model_info.Token != "" && !Model_info.Token.Contains("null"))
                    {
                        TokenManager.IosApns(content, parameters, Model_info.Token);
                    }
                }

                if (Retult == "not_auth")
                {
                    var value = TokenManager.GetToken("Token");
                    TokenManager.RefTokenCache("Token", value, 1425);
                    TokenManager.RefTokenSQL("Token", value);
                    Retult = TokenManager.TouChuanXiaoXi(parasTouChuan, touchuanneirongs.title, touchuanneirongs.content, Cid, Token);
                    if (Model_info.Device == "android")
                    {
                        Retult = TokenManager.SendXiaoXiGeTui(paras, touchuanneirongs.title, touchuanneirongs.content, Cid, Token);
                    }
                }
            }
        }

        public static string getXiaoXiParams(string id, string type)
        {
            #region 参数
            string Params = "{'id':'" + id + "'," + // tab 状态
               "'type':'" + type + "'}";  //唯一标识 
            #endregion
            return Params;
        }

    }

    public class MessageReposities
    {
        List<MessageBody> message = new List<MessageBody>();
        public MessageReposities()
        {
            message.Add(new DbHelper.MessageBody() { Mid = "待点评的日志", Title = "待点评的日志", Content = "收到一条待点评的日志", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "待审批的审批", Title = "待审批的审批", Content = "收到一条待审批的审批", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "待执行的任务", Title = "待执行的任务", Content = "收到一条待执行的任务", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "待执行的指令", Title = "待执行的指令", Content = "收到一条待执行的指令", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "待参与的日程", Title = "待参与的日程", Content = "收到一条待参与的日程", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "收到一条点赞", Title = "点赞消息", Content = "收到一条点赞", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "收到一条回复", Title = "回复消息", Content = "收到一条回复", PushId = "service_2", Sign = true });

            message.Add(new DbHelper.MessageBody() { Mid = "日志已点评", Title = "日志已点评", Content = "您的日志已点评", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "审批已审批", Title = "审批已审批", Content = "您的审批已审批", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "任务已执行", Title = "任务已执行", Content = "您的任务已执行", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "指令已执行", Title = "指令已执行", Content = "指令已执行", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "日程已参与", Title = "日程已参与", Content = "日程已参与", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "你发出的审批已通过", Title = "你发出的审批已通过", Content = "你发出的审批已通过", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "你待点评的日志已删除", Title = "你待点评的日志已删除", Content = "你待点评的日志已删除", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "你待审批的审批已取消", Title = "你待审批的审批已取消", Content = "你待审批的审批已取消", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "你待执行的任务已取消", Title = "你待执行的任务已取消", Content = "你待执行的任务已取消", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "你待执行的指令已取消", Title = "你待执行的指令已取消", Content = "你待执行的指令已取消", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "你待参与的日程已删除", Title = "你待参与的日程已删除", Content = "你待参与的日程已删除", PushId = "service_2", Sign = true });


            message.Add(new DbHelper.MessageBody() { Mid = "你发出的审批被拒绝", Title = "你发出的审批被拒绝", Content = "你发出的审批被拒绝", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "你发出的审批审批人已取消", Title = "你发出的审批审批人已取消", Content = "你发出的审批审批人已取消", PushId = "service_2", Sign = true });

            message.Add(new DbHelper.MessageBody() { Mid = "你发出的任务已取消", Title = "你发出的任务已取消", Content = "你发出的任务已取消", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "你发出的指令已取消", Title = "你发出的指令已取消", Content = "你发出的指令已取消", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "你发出的日程已取消", Title = "你发出的日程已取消", Content = "你发出的日程已取消", PushId = "service_2", Sign = true });


            message.Add(new DbHelper.MessageBody() { Mid = "你已执行的指令需继续执行", Title = "你已执行的指令需继续执行", Content = "你已执行的指令需继续执行", PushId = "service_2", Sign = true });

            message.Add(new DbHelper.MessageBody() { Mid = "你执行的任务已被点评", Title = "你执行的任务已被点评", Content = "你执行的任务已被点评", PushId = "service_2", Sign = true });
            message.Add(new DbHelper.MessageBody() { Mid = "你执行的指令已被点评", Title = "你执行的指令已被点评", Content = "你执行的指令已被点评", PushId = "service_2", Sign = true });

        }
        public MessageBody GetMessageBody(string Mid)
        {
            return this.message.Where(t => t.Mid == Mid).FirstOrDefault();
        }
    }
    public class MessageBody
    {
        internal string Mid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string PushId { get; set; }
        public string lat { get; set; }
        public string lnt { get; set; }
        public string XiaoXiCid { get; set; }
        public bool Sign { get; set; }
        public string Params { get; set; }
    }
    public class MessageBodyHouTai
    {
        internal string Mid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string PushId { get; set; }
        public string Url { get; set; }
    }
    public class TokenManager
    {
        IMemoryCache _memoryCache;
        IPrincipalBase _PrincipalBase;
        static object lockObj = new object();
        IOptions<Root> config;
        string GTAPPID;
        string GTAPPKEY;
        string GTMASTERSECRET;
        string AppleZhengShu;
        string ApplePWD;
 
        public static DateTime? Expiration { get; set; }
        public static readonly DateTime DoNotStore = DateTime.MinValue;
        private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public const int DEVICE_TOKEN_BINARY_SIZE = 32;
        public const int DEVICE_TOKEN_STRING_SIZE = 64;
        public const int MAX_PAYLOAD_SIZE = 256;
        private static X509Certificate certificate;
        private static X509CertificateCollection certificates;
        public TokenManager(IMemoryCache memoryCache, IPrincipalBase IPrincipalBase, IOptions<Root> config)
        {
            this._memoryCache = memoryCache;
            this._PrincipalBase = IPrincipalBase;
            this.config = config;
            GTAPPID = this.config.Value.ConnectionStrings.GTAPPID;
            GTAPPKEY = this.config.Value.ConnectionStrings.GTAPPKEY;
            GTMASTERSECRET = this.config.Value.ConnectionStrings.GTMASTERSECRET;
            AppleZhengShu = this.config.Value.ConnectionStrings.AppleZhengShu;
            ApplePWD = this.config.Value.ConnectionStrings.ApplePWD;
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public string GetToken(string tokenName)
        {
            var CacheToken = _memoryCache.Get(tokenName);
            var token = "";
            if (CacheToken == null)
            {
                TokenManager TokenManager = new TokenManager(_memoryCache, _PrincipalBase, config);
                var GeTuiToken = _PrincipalBase.GetGeTuiToken(tokenName);
                if (GeTuiToken != null && GeTuiToken.DateTime > DateTime.Now)
                {
                    TimeSpan ts = GeTuiToken.DateTime - DateTime.Now;
                    //token = WebApiHelper.GetGeTui(paras, Cid, touchuanneirongs.title, touchuanneirongs.content, GeTuiToken.Value);
                    //_memoryCache.Set("token", token.ToString(), GeTuiToken.DateTime);
                    TokenManager.RefTokenCache(tokenName, GeTuiToken.Value, ts.TotalMinutes);
                    return GeTuiToken.Value;

                }
                else
                {
                    lock (lockObj)
                    {
                        var CacheToken2 = _memoryCache.Get(tokenName);
                        if (CacheToken2 != null)
                        {
                            return CacheToken2.ToString();
                        }
                        else
                        {
                            try
                            {
                                var checkUrl3 = "https://restapi.getui.com/v1/" + GTAPPID + "/auth_close";
        
                                var eeee = "";
                                var messageresultx = WebApiHelper.PostAsynctMethod222<close>(checkUrl3, eeee, GeTuiToken.Value);
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                            var value = TokenManager.MarkToken();


                            TokenManager.RefTokenCache(tokenName, value, 1425);
                            TokenManager.RefTokenSQL(tokenName, value);
                            return value;
                        }
                    }
                    //token = WebApiHelper.GetGeTui(paras, Cid, touchuanneirongs.title, touchuanneirongs.content, "");
                    //_memoryCache.Set("token", token.ToString(), DateTime.Now.AddHours(23).AddMinutes(55));
                    //_PrincipalBase.UpdateGeTuiToken(token, DateTime.Now.AddHours(23).AddMinutes(55));
                }
            }
            else
            {
                return CacheToken.ToString();
            }
        }
        /// <summary>
        /// 创建token
        /// </summary>
        /// <returns></returns>
        public string MarkToken()
        {
            try
            {
                var checkUrl = "https://restapi.getui.com/v1/" + GTAPPID + "/auth_sign";
                GetTuiTokenPara GetTuiTokenPara = new GetTuiTokenPara();
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                var ShiJianChuo = Convert.ToInt64(ts.TotalMilliseconds).ToString();
                var sign2 = GTAPPKEY + ShiJianChuo + GTMASTERSECRET;
                //var sign2= khAPPKEY+"1495879766698" + khMASTERSECRET;
                byte[] bytes = Encoding.UTF8.GetBytes(sign2);
                var sha256 = System.Security.Cryptography.SHA256.Create();
                var hash = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                GetTuiTokenPara.sign = sb.ToString();
                GetTuiTokenPara.timestamp = ShiJianChuo;
                GetTuiTokenPara.appkey = GTAPPKEY;
                var paramar = JsonConvert.SerializeObject(GetTuiTokenPara);
                var result = WebApiHelper.PostAsynctMethod<token>(checkUrl, paramar);
                var token = result.auth_token;
                return token;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <returns></returns>
        public void RefTokenCache(string tokenName, string value, double Minutes)
        {
            _memoryCache.Set(tokenName, value.ToString(), DateTime.Now.AddMinutes((int)Minutes));
        }
        public void RefTokenSQL(string tokenName, string value)
        {
            _PrincipalBase.UpdateGeTuiToken(value, DateTime.Now.AddHours(23).AddMinutes(55), tokenName);
        }
        public void IosApns(string title, string para, string testDeviceToken)
        {
            bool sandbox = true;


            string p12File = AppleZhengShu;

            string p12FilePassword = ApplePWD;

            string p12Filename = System.IO.Path.Combine(Directory.GetCurrentDirectory(), p12File);

            NotificationService service = new NotificationService(sandbox, p12Filename, p12FilePassword, 1);

            service.SendRetries = 5; //5 retries before generating notificationfailed event
            service.ReconnectDelay = 5000; //5 seconds

            service.Error += new NotificationService.OnError(service_Error);
            service.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);

            service.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);
            service.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);
            service.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);
            service.Connecting += new NotificationService.OnConnecting(service_Connecting);
            service.Connected += new NotificationService.OnConnected(service_Connected);
            service.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);

            //The notifications will be sent like this:
            //		Testing: 1...
            //		Testing: 2...
            //		Testing: 3...
            // etc...
            //for (int i = 1; i <= count; i++)
            //{
            //    //Create a new notification to send
            Notification alertNotification = new Notification(testDeviceToken);

            alertNotification.Payload.applyRepairId = "0";
            alertNotification.Payload.Alert.Body = title;
            alertNotification.Payload.Sound = "default";
            alertNotification.Payload.Badge = 1;
            var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<ApnsPara>(para);
            alertNotification.Payload.state = objectList.state;
            alertNotification.Payload.name = objectList.name;
            alertNotification.Payload.type = objectList.type;
            alertNotification.Payload.id = objectList.id;
            alertNotification.Payload.code = objectList.code;

            service.QueueNotification(alertNotification);
            //Queue the notification to be sent
            //if (service.QueueNotification(alertNotification))
            //    Console.WriteLine("Notification Queued!");
            //else
            //    Console.WriteLine("Notification Failed to be Queued!");

            //Sleep in between each message
            //if (i < count)
            //{
            //    Console.WriteLine("Sleeping " + sleepBetweenNotifications + " milliseconds before next Notification...");
            //    System.Threading.Thread.Sleep(sleepBetweenNotifications);
            //}
            //}

            service.Close();

            //Clean up
            service.Dispose();




            //string hostIP = "gateway.sandbox.push.apple.com";//
            //int port = 2195;
            //string password = AppleKeHuPWD;//
            //string certificatepath = AppleKeHuZhengShu;//bin/debug
            //string p12Filename = System.IO.Path.Combine(Directory.GetCurrentDirectory(), certificatepath);
            //certificate = new X509Certificate2(System.IO.File.ReadAllBytes(p12Filename), password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            //certificates = new X509CertificateCollection();
            //certificates.Add(certificate);
            //TcpClient apnsClient = new TcpClient();
            //apnsClient.Client.Connect(hostIP, port);
            //var sss = apnsClient.GetStream();
            //SslStream apnsStream = new SslStream(sss, false, new RemoteCertificateValidationCallback(validateServerCertificate), new LocalCertificateSelectionCallback(selectLocalCertificate));
            //try
            //{
            //    //APNs已不支持SSL 3.0 
            //    apnsStream.AuthenticateAsClientAsync(hostIP, certificates, System.Security.Authentication.SslProtocols.Tls, false);
            //}
            //catch (System.Security.Authentication.AuthenticationException ex)
            //{
            //    Console.WriteLine("error+" + ex.Message);
            //}
            //if (!apnsStream.IsMutuallyAuthenticated)
            //{
            //    Console.WriteLine("error:Ssl Stream Failed to Authenticate！");
            //}
            //if (!apnsStream.CanWrite)
            //{
            //    Console.WriteLine("error:Ssl Stream is not Writable!");
            //}
            //Byte[] message = ToBytes(testDeviceToken, title);
            //apnsStream.Write(message);


        }
        private static byte[] BuildBufferFrom(IList<byte[]> bufferParts)
        {
            int bufferSize = 0;
            for (int i = 0; i < bufferParts.Count; i++)
                bufferSize += bufferParts[i].Length;
            byte[] buffer = new byte[bufferSize];
            int position = 0;
            for (int i = 0; i < bufferParts.Count; i++)
            {
                byte[] part = bufferParts[i];
                Buffer.BlockCopy(bufferParts[i], 0, buffer, position, part.Length);
                position += part.Length;
            }
            return buffer;
        }
        private static bool validateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; // Dont care about server's cert
        }
        private static X509Certificate selectLocalCertificate(object sender, string targetHost, X509CertificateCollection localCertificates,
         X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return certificate;
        }
        public string SendXiaoXiGeTui(string para, string Title, string Text, string Cid, string token)
        {
            try
            {
                var checkUrl2 = "https://restapi.getui.com/v1/" + GTAPPID + "/push_single";
                GetTuiPara GetTuiParas = new GetTuiPara();
                message messages = new message();
                notification notifications = new notification();
                style style = new style();
                style.type = 0;
                style.text = Text;
                style.title = Title;
                style.is_ring = true;
                style.is_vibrate = true;
                style.is_clearable = true;
                notifications.style = style;
                notifications.transmission_type = false;
                notifications.transmission_content = para;
                messages.appkey = GTAPPKEY;
                messages.is_offline = true;
                messages.offline_expire_time = 10000000;
                messages.msgtype = "notification";
                GetTuiParas.message = messages;
                GetTuiParas.notification = notifications;
                GetTuiParas.cid = Cid;
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                GetTuiParas.requestid = Convert.ToInt64(ts.TotalMilliseconds).ToString();
                var param = JsonConvert.SerializeObject(GetTuiParas);
                var messageresult = WebApiHelper.PostAsynctMethod222<messageresult>(checkUrl2, param, token);
                return messageresult.result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string TouChuanXiaoXi(string para, string Title, string Text, string Cid, string token)
        {
            try
            {
                var checkUrl2 = "https://restapi.getui.com/v1/" + GTAPPID + "/push_single";
                push_info push_info = new push_info();
                aps aps = new aps();
                alert alert = new alert();
                multimedia mult = new multimedia();
                List<multimedia> m = new List<multimedia>();
                mult.url = "http://ol5mrj259.bkt.clouddn.com/test2.mp4";
                mult.type = 3;
                mult.only_wifi = true;
                m.Add(mult);
                alert.title = Title;
                alert.body = Text;
                aps.autoBadge = "+1";
                aps.alert = alert;
                aps.contentAvailable = 1;
                push_info.aps = aps;
                push_info.multimedia = m;
                TouChuanPara GetTuiParas = new TouChuanPara();
                messagepara messages = new messagepara();
                messages.appkey = GTAPPKEY;
                messages.is_offline = true;
                //messages.offline_expire_time = 10000000;
                messages.msgtype = "transmission";
                GetTuiParas.message = messages;

                transmission transmission = new transmission();
                transmission.transmission_type = false;
                transmission.transmission_content = para;
                transmission.duration_begin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                transmission.duration_end = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
                GetTuiParas.push_info = push_info;
                GetTuiParas.transmission = transmission;
                GetTuiParas.cid = Cid;
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                GetTuiParas.requestid = Convert.ToInt64(ts.TotalMilliseconds).ToString();
                var param = JsonConvert.SerializeObject(GetTuiParas);
                param.Replace("contentAvailable", "content-available");
                var messageresult = WebApiHelper.PostAsynctMethod222<messageresult>(checkUrl2, param, token);
                return messageresult.result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        static void service_BadDeviceToken(object sender, BadDeviceTokenException ex)
        {
            Console.WriteLine("Bad Device Token: {0}", ex.Message);
        }

        static void service_Disconnected(object sender)
        {
            Console.WriteLine("Disconnected...");
        }

        static void service_Connected(object sender)
        {
            Console.WriteLine("Connected...");
        }

        static void service_Connecting(object sender)
        {
            Console.WriteLine("Connecting...");
        }

        static void service_NotificationTooLong(object sender, NotificationLengthException ex)
        {
            Console.WriteLine(string.Format("Notification Too Long: {0}", ex.Notification.ToString()));
        }

        static void service_NotificationSuccess(object sender, Notification notification)
        {
            Console.WriteLine(string.Format("Notification Success: {0}", notification.ToString()));
        }

        static void service_NotificationFailed(object sender, Notification notification)
        {
            Console.WriteLine(string.Format("Notification Failed: {0}", notification.ToString()));
        }

        static void service_Error(object sender, Exception ex)
        {
            Console.WriteLine(string.Format("Error: {0}", ex.Message));
        }

    }
    public class touchuanneirong
    {
        public string title { get; set; }
        public string content { get; set; }
        public string status { get; set; }
        public string lnt { get; set; }
        public string lat { get; set; }
        public string image { get; set; }
        public string isNotification { get; set; }
        public bool Sign { get; set; }
        public string Params { get; set; }
    }
    public class GetTuiTokenPara
    {
        public string sign { get; set; }
        public string timestamp { get; set; }
        public string appkey { get; set; }
    }
    public class push_info
    {
        public aps aps { get; set; }
        public List<multimedia> multimedia { get; set; }
    }
    public class aps
    {
        public alert alert { get; set; }
        public string autoBadge { get; set; }
        public int contentAvailable { get; set; }
    }
    public class multimedia
    {
        public string url { get; set; }
        public int type { get; set; }
        public bool only_wifi { get; set; }
    }
    public class transmission
    {
        public bool transmission_type { get; set; }
        public string transmission_content { get; set; }
        public string duration_begin { get; set; }

        public string duration_end { get; set; }

    }
    public class alert
    {
        public string title { get; set; }
        public string body { get; set; }
    }
    public class messagepara
    {
        public string appkey { get; set; }
        public bool is_offline { get; set; }
        public string msgtype { get; set; }
    }
    public class TouChuanPara
    {
        public messagepara message { get; set; }
        public transmission transmission { get; set; }
        public push_info push_info { get; set; }
        public string cid { get; set; }
        public string requestid { get; set; }
    }
    public class ApnsPara
    {
        public string state { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }
    public class close
    {
        public string result { get; set; }
    }
    public class token
    {
        public string result { get; set; }
        public string auth_token { get; set; }
    }
    public class GetTuiPara
    {
        public message message { get; set; }
        public notification notification { get; set; }
        public string cid { get; set; }
        public string requestid { get; set; }

    }
    public class message
    {
        public string appkey { get; set; }
        public bool is_offline { get; set; }
        public int offline_expire_time { get; set; }
        public string msgtype { get; set; }
    }
    public class notification
    {
        public style style { get; set; }
        public bool transmission_type { get; set; }
        public string transmission_content { get; set; }
    }
    public class style
    {
        public int type { get; set; }
        public string text { get; set; }
        public string title { get; set; }
        public bool is_ring { get; set; }

        public bool is_vibrate { get; set; }

        public bool is_clearable { get; set; }

    }
    public class messageresult
    {
        public string result { get; set; }
        public string taskid { get; set; }
        public string status { get; set; }
    }
}

