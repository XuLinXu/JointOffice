using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public interface IPrincipalBase
    {
        string GetMemberidToken();
        string GetMemberId();
        string GetMemberType();
        ModelInfo GetModelInfo(string memberid);
        void AddSystem_Message(string memberid, string Title, string Message, string Params);
        GeTuiToken GetGeTuiToken(string tokenName);
        void UpdateGeTuiToken(string value, DateTime datetime, string tokenName);
        List<QianDaoType> GetQianDaoType();
        //alldsdd GetAlldsdd(string name);
    }
    public class UserInfo
    {
        public string memberid { get; set; }//memberid
        public string type { get; set; }//  web   phone
    }
    public class ModelInfo
    {
        public string Device { get; set; }
        public string Cid { get; set; }
        public string Token { get; set; }
    }
    public class GeTuiToken
    {
        public string Value { get; set; }
        public DateTime DateTime { get; set; }
    }
    public class alldsdd
    {
        public string uid { get; set; }
        public string name { get; set; }
    }
}
