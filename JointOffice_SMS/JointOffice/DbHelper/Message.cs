using JointOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.DbHelper
{
    public class Message
    {
        public Showapi_Res_Meaasge MemberMeaasge()
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            res.showapi_res_error = "账号未登陆";
            res.showapi_res_code = "510";
            ReturnMessage mes = new ReturnMessage();
            mes.Oprationflag = false;
            mes.Message = "账号未登陆";
            res.showapi_res_body = mes;
            return res;
        }
        public Showapi_Res_Meaasge SuccessMeaasge(string message)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            res.showapi_res_error = message;
            res.showapi_res_code = "200";
            ReturnMessage mes = new ReturnMessage();
            mes.Oprationflag = true;
            mes.Message = message;
            res.showapi_res_body = mes;
            return res;
        }
        public Showapi_Res_Meaasge SuccessMeaasgeCode(string message, string code)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            res.showapi_res_error = message;
            res.showapi_res_code = "200";
            ReturnMessage mes = new ReturnMessage();
            mes.Oprationflag = true;
            mes.Message = message;
            mes.memberid = code;
            res.showapi_res_body = mes;
            return res;
        }
    }
    public class ReturnList<T> where T : class
    {
        public Showapi_Res_List<T> Return()
        {
            Showapi_Res_List<T> res = new Showapi_Res_List<T>();
            res.showapi_res_error = "账号未登陆";
            res.showapi_res_code = "510";
            return res;
        }
    }
    public class ReturnSingle<T> where T : class, new()
    {
        public Showapi_Res_Single<T> Return()
        {
            Showapi_Res_Single<T> res = new Showapi_Res_Single<T>();
            res.showapi_res_error = "账号未登陆";
            res.showapi_res_code = "510";
            return res;
        }
    }
}
