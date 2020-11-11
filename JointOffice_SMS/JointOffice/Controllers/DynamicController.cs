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

    public class DynamicController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        private readonly IDynamic _IDynamic;
        ExceptionMessage em;
        IOptions<Root> config;
        public DynamicController(IOptions<Root> config, IDynamic IDynamic,JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _IDynamic = IDynamic;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
        }
        /// <summary>
        /// 获取个人动态
        /// </summary>
        /// <param name="页数，总数，类型"></param>
        /// <returns></returns>
        [HttpPost("GetPersonjobList")]
        public Personjob GetPersonjobList(Persondynamic para)
        {
            try
            {
                return _IDynamic.GetPersonjobList(para);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
    }
}
