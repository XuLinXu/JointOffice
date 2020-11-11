using JointOffice.Configuration;
using JointOffice.DbModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace JointOffice.Models
{
    public class PrincipalBase : IPrincipalBase
    {
        IOptions<Root> config;
        //IHttpContextAccessor _httpContextAccessor = null;
        //private ISession _session => _httpContextAccessor.HttpContext.Session;
        UserInfo userInfo;
        private IMemoryCache _memoryCache;
        public static IServiceProvider ServiceProvider;
        //private IHttpContextAccessor _context;
        JointOfficeContext _JointOfficeContext;
        string JointOfficeconstr;
        public PrincipalBase(IOptions<Root> config, IMemoryCache memoryCache, JointOfficeContext JointOfficeContext)
        {
            _memoryCache = memoryCache;
            this.config = config;
            UserInfoCatch();
            _JointOfficeContext = JointOfficeContext;
            JointOfficeconstr = this.config.Value.ConnectionStrings.JointOfficeConnection;
        }
        public void UserInfoCatch()
        {
            try
            {
                object factory = ServiceProvider.GetService(typeof(IHttpContextAccessor));
                HttpContext context = ((HttpContextAccessor)factory).HttpContext;
                var memberid = context.Request.Headers.Where(t => t.Key == "memberid").FirstOrDefault().Value;
                var type = context.Request.Headers.Where(t => t.Key == "type").FirstOrDefault().Value;
                //var memberid = "yPIWuXVPQmShdTvz3OEkSeQVPNVp1JHFo/ucdKfnYqLEQ11o8NYsAxESSoH0QZIP7+ZW5n5TGLQCtPMlpktTpIbL7qOj+kTI9kCM1zNkeZhstWQNbHkABcJgjFEL8sNEyco81QspZW4=";
                //var LanguageID = "zh-cn";
                //var DriverID = "000D62D61C694514C2DB9EC026D207B1A52";
                userInfo = new UserInfo();
                userInfo.memberid = memberid;
                userInfo.type = type;
                var id = _memoryCache.Get("memberid");
                var type2 = _memoryCache.Get("type");
                if (id == null)
                {
                    _memoryCache.Set("memberid", memberid, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
                }
                if (type2 == null)
                {
                    _memoryCache.Set("type", type, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetMemberidToken()
        {
            if (userInfo != null)
            {
                return userInfo.memberid;
            }
            else
            {
                var memberid = _memoryCache.Get("memberid");
                if (memberid != null)
                {
                    return memberid.ToString();
                }

            }
            return null;
        }
        public string GetMemberType()
        {
            if (userInfo != null)
            {
                return userInfo.type;
            }
            else
            {
                var type = _memoryCache.Get("type");
                if (type != null)
                {
                    return type.ToString();
                }

            }
            return null;
        }
        public string GetMemberId()
        {
            //var memberid = _JointOfficeContext.Member_Token.Where(t => t.Token == GetMemberidToken() && t.Effective == 1).FirstOrDefault();
            var memberid = _JointOfficeContext.Member_Token.Where(t => t.Token == GetMemberidToken() && t.Type == GetMemberType() && t.Effective == 1).FirstOrDefault();
            if (memberid == null)
            {
                return null;
            }
            else
            {
                return memberid.MemberId.ToString();
            }
            //return "9bf1d5cd-4ffc-4a8d-8a2a-35855da2d18f";
        }
        public ModelInfo GetModelInfo(string memberid)
        {
            var sql = " select * from Member_Model where type = 1 and ShiFouKeYong = 1 and MemberId='" + memberid + "'";
            using (SqlConnection conText = new SqlConnection(JointOfficeconstr))
            {
                return conText.Query<ModelInfo>(sql, "").FirstOrDefault();
            }
        }
        public void AddSystem_Message(string memberid, string Title, string Message, string Params)
        {
            System_Message system_Message = new System_Message();
            system_Message.Id = Guid.NewGuid().ToString();
            system_Message.MemberId = memberid;
            system_Message.Type = 1;
            system_Message.Title = Title;
            system_Message.Message = Message;
            system_Message.SendDate = DateTime.Now;
            system_Message.ShiFouYiDu = 0;
            system_Message.Params = Params;

            _JointOfficeContext.System_Message.Add(system_Message);
            _JointOfficeContext.SaveChanges();
            //var sql = " insert into System_Message(memberid,type,title,message,Senddate,Sender,ShiFouYiDu,Params)  values(" + memberid + ",1,'" + Title + "','" + Message + "',dbo.getlocaldate(8),'系统消息',0,'"+Params+"')";
            //using (SqlConnection conText = new SqlConnection(EXXconstr))
            //{
            //    conText.Query(sql, "").FirstOrDefault();
            //}
        }
        public GeTuiToken GetGeTuiToken(string tokenName)
        {
            var sql = " select Value,DateTime from GeTuiToken where token='" + tokenName + "'";
            using (SqlConnection conText = new SqlConnection(JointOfficeconstr))
            {
                return conText.Query<GeTuiToken>(sql, "").FirstOrDefault();
            }
        }
        public void UpdateGeTuiToken(string value, DateTime datetime, string tokenName)
        {
            var sql = " update GeTuiToken set Value='" + value + "',DateTime = '" + datetime.ToString() + "' where token='" + tokenName + "'";
            using (SqlConnection conText = new SqlConnection(JointOfficeconstr))
            {
                conText.Query(sql, "").FirstOrDefault();
            }
        }
        public List<QianDaoType> GetQianDaoType()
        {
            var sql = " select valuemeaning type from base_keyvalue where valuesetcode='RQFYLB'";
            using (SqlConnection conText = new SqlConnection(JointOfficeconstr))
            {
                return conText.Query<QianDaoType>(sql, "").ToList();
            }
        }
        //public alldsdd GetAlldsdd(string name)
        //{
        //    var sql = "select * from oa2 where name=N'"+ name + "' ";
        //    using (SqlConnection conText = new SqlConnection(JointOfficeconstr))
        //    {
        //        return conText.Query<alldsdd>(sql, "").FirstOrDefault();
        //    }
        //}
        //public string GetMemberId_New()
        //{
        //    var memberid = _JointOfficeContext.Member_Token_New.Where(t => t.Token == GetMemberidToken() && DateTime.Now < Convert.ToDateTime(t.FailDate)).FirstOrDefault();
        //    if (memberid == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return memberid.MemberId.ToString();
        //    }
        //    //return "aa9f0ccb-3a28-4ca5-9f0e-28ec37b23aaa";
        //}
    }

}
