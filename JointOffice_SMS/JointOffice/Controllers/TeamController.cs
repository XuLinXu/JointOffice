using JointOffice.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.Configuration;
using Microsoft.Extensions.Options;
using JointOffice.DbModel;

namespace JointOffice.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        ITeam _ITeam;
        ExceptionMessage em;
        IOptions<Root> config;
        public TeamController(IOptions<Root> config, ITeam ITeam, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _ITeam = ITeam;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
        }
        //创建群组
        [HttpPost("CreateTeam")]
        public Showapi_Res_Meaasge CreateTeam([FromBody]CreateTeamPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.name) || string.IsNullOrEmpty(para.memberidlist.Count().ToString()))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _ITeam.CreateTeam(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        //修改群组人员
        [HttpPost("UpdateTeam")]
        public Showapi_Res_Meaasge UpdateTeam([FromBody]UpdateTeamPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.teamid) || string.IsNullOrEmpty(para.memberidlist.Count().ToString()) || string.IsNullOrEmpty(para.type.ToString()))
                {
                    throw new BusinessException("参数不正确.");
                }
                return _ITeam.UpdateTeam(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
    }
}
