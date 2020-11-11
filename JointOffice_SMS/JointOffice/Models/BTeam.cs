using Dapper;
using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public class BTeam: ITeam
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        private readonly IPrincipalBase _PrincipalBase;
        public BTeam(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
        }
        public Showapi_Res_Meaasge CreateTeam(CreateTeamPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var PeopleList = new List<People>();
            foreach (var item in para.memberidlist)
            {
                var People = new People();
                People.memberid = item;
                PeopleList.Add(People);
            }
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(PeopleList);
            Member_Team Member_Team = new Member_Team();
            Member_Team.Id = Guid.NewGuid().ToString();
            Member_Team.MemberId = memberid;
            Member_Team.TeamPerson = str;
            Member_Team.Name = para.name;

            _JointOfficeContext.Member_Team.Add(Member_Team);
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("创建成功");
        }
        public Showapi_Res_Meaasge UpdateTeam(UpdateTeamPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var Member_Team = _JointOfficeContext.Member_Team.Where(t => t.Id == para.teamid).FirstOrDefault();
            var renyuanlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<People>>(Member_Team.TeamPerson);
            if (para.type==1)
            {
                foreach (var item in para.memberidlist)
                {
                    var People = new People();
                    People.memberid = item;
                    renyuanlist.Add(People);
                }
            }
            else
            {
                foreach (var item in para.memberidlist)
                {
                    var one = renyuanlist.Where(t => t.memberid == item).FirstOrDefault();
                    renyuanlist.Remove(one);
                }
            }
            Member_Team.TeamPerson = Newtonsoft.Json.JsonConvert.SerializeObject(renyuanlist);
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("创建成功");
        }
    }
}
