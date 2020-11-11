using JointOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JointOffice.DbModel;
using JointOffice.Configuration;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using Dapper;

namespace JointOffice.DbHelper
{
    public class ExceptionMessage
    {
        JointOfficeContext _JointOfficeContext;
        public ExceptionMessage(JointOfficeContext JointOfficeContext)   
        {   
            _JointOfficeContext = JointOfficeContext;
        }
        public Showapi_Res_Meaasge ReturnMeaasge(Exception ex)
        {
            Logs log = new Logs();
            log.CreateDate = DateTime.Now;
            log.Createperson = "admin"; 
            log.Origin = "后台";
            log.Exception = ex.Message;
            log.Track = ex.StackTrace;
            _JointOfficeContext.Logs.Add(log);
            _JointOfficeContext.SaveChanges();
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            res.showapi_res_error = ex.Message;
            res.showapi_res_code = "508";
            ReturnMessage mes = new ReturnMessage();
            mes.Oprationflag = false;
            mes.Message = ex.Message;
            res.showapi_res_body = mes;
            return res;
        }
        public void XieLogs(Exception ex)
        {
            Logs log = new Logs();
            log.CreateDate = DateTime.Now;
            log.Createperson = "admin";
            log.Origin = "后台";
            log.Exception = ex.Message;
            log.Track = ex.StackTrace;
            _JointOfficeContext.Logs.Add(log);
            _JointOfficeContext.SaveChanges();
        }

        internal Showapi_Res_Meaasge MemberMeaasge()
        {
            throw new NotImplementedException();
        }
    }
}
