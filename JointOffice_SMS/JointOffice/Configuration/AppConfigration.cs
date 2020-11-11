using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Configuration
{
    public class ConnectionStrings
    {
        public string aldyurl { get; set; }
        public string aldyappkey { get; set; }
        public string aldysecret { get; set; }
        public string aldyExtend { get; set; }
        public string aldySmsType { get; set; }
        public string aldySmsFreeSignName { get; set; }
        public string JointOfficeConnection { get; set; }
        public string StorageConnectionString { get; set; }
        public string SasKey { get; set; }
        public string appkey { get; set; }
        public string appsecret { get; set; }
        public string appcode { get; set; }
        public string aliappcode { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpHost { get; set; }
        public int ImapPort { get; set; }
        public string ImapHost { get; set; }
        public string GTAPPID { get; set; }
        public string GTAPPKEY { get; set; }
        public string GTMASTERSECRET { get; set; }
        public string AppleZhengShu { get; set; }
        public string ApplePWD { get; set; }
        public string corpid { get; set; }
        public string corpsecret { get; set; }
        public string Imurl { get; set; }
        public string ImConnection { get; set; }
        public string SMSUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
    }
    public class Root
    {
        /// <summary>
        /// ConnectionStrings
        /// </summary>
        public ConnectionStrings ConnectionStrings { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string MongoConnet { get; set; }
        ///// <summary>
        ///// Logging
        ///// </summary>
        //public Logging Logging { get; set; }
    }
}
