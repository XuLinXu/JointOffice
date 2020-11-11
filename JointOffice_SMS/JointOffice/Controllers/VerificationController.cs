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
using Microsoft.WindowsAzure.Storage.Blob;
using JointOffice.DbModel;

namespace JointOffice.Controllers
{
    [Route("api/[controller]")]

    public class VerificationController : Controller
    {
        JointOfficeContext _JointOfficeContext;
        private readonly IVerification _IVerification;
        ExceptionMessage em;
        IOptions<Root> config;
        public VerificationController(IOptions<Root> config, IVerification IVerification, JointOfficeContext JointOfficeContext)
        {
            _JointOfficeContext = JointOfficeContext;
            _IVerification = IVerification;
            this.config = config;
            em = new ExceptionMessage(_JointOfficeContext);
        }
        /// <summary>
        /// 注册短信验证码
        /// </summary>
        /// <param name="手机号，验证码"></param>
        /// <returns></returns>
        [HttpPost("SendVerificationCode")]
        public Showapi_Res_Meaasge SendVerificationCode([FromBody]ZhuCePara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.mobile)||string.IsNullOrEmpty(para.DiQuCode) )
                {
                    throw new BusinessException("请填写手机号和地区.");
                }
                return _IVerification.SendVerificationCode(para);           
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 登录短信验证码
        /// </summary>
        /// <param name="手机号，验证码"></param>
        /// <returns></returns>
        [HttpPost("SendDengluCode")]
        public Showapi_Res_Meaasge SendDengluCode([FromBody]ZhuCePara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.mobile) || string.IsNullOrEmpty(para.DiQuCode))
                {
                    throw new BusinessException("请填写手机号和地区.");
                }
                return _IVerification.SendDengluCode(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 发送语音验证码
        /// </summary>
        /// <param name="手机号"></param>
        /// <returns></returns>
        [HttpPost("SendVoiceVerificationCode")]
        public Showapi_Res_Meaasge SendVoiceVerificationCode([FromBody]string mobile)
        {
            try
            {
                if (string.IsNullOrEmpty(mobile))
                {
                    throw new BusinessException("请填写手机号.");
                }
                return _IVerification.SendVoiceVerificationCode(mobile);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 账号密码登录
        /// </summary>
        /// <param name="登录名，登录密码"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public Showapi_Res_Meaasge Login([FromBody]LoginPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.loginname)|| string.IsNullOrEmpty(para.loginpwd))
                {
                    throw new BusinessException("请确认账号密码全部填写.");
                }
                return _IVerification.Login(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 验证码登录
        /// </summary>
        /// <param name="登录名，验证码"></param>
        /// <returns></returns>
        [HttpPost("YanZhengMaLogin")]
        public Showapi_Res_Meaasge YanZhengMaLogin([FromBody]YanZhengMaPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.loginname) || string.IsNullOrEmpty(para.code))
                {
                    throw new BusinessException("请确认账号和验证码全部填写.");
                }
                return _IVerification.YanZhengMaLogin(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 验证码验证
        /// </summary>
        /// <param name="登录名，验证码"></param>
        /// <returns></returns>
        [HttpPost("YanZhengMa")]
        public Showapi_Res_Meaasge YanZhengMa([FromBody]YanZhengMaPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.loginname) || string.IsNullOrEmpty(para.code) )
                {
                    throw new BusinessException("请确认账号和验证码全部填写.");
                }
                return _IVerification.YanZhengMa(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 设置登录密码
        /// </summary>
        /// <param name="登录名，登录密码"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public Showapi_Res_Meaasge Register([FromBody]LoginPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.loginname) || string.IsNullOrEmpty(para.loginpwd))
                {
                    throw new BusinessException("请确认账号和密码全部填写.");
                }
                return _IVerification.Register(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }

        }
        /// <summary>
        /// 通过原密码修改密码
        /// </summary>
        /// <param name="登录名，旧密码，新密码"></param>
        /// <returns></returns>
        [HttpPost("UpdatePassword")]
        public Showapi_Res_Meaasge UpdatePassword([FromBody]UpdatePasswordPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.loginname) || string.IsNullOrEmpty(para.loginpwd) || string.IsNullOrEmpty(para.oldloginpwd))
                {
                    throw new BusinessException("请确认账号,原密码和密码全部填写.");
                }
                if (para.loginpwd.Length<6)
                {
                    throw new BusinessException("新密码要大于等于6位.");
                }
                return _IVerification.UpdatePassword(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }

        }
        /// <summary>
        /// 忘记密码修改密码
        /// </summary>
        /// <param name="登录名，登录密码"></param>
        /// <returns></returns>
        [HttpPost("GengGaiMiMa")]
        public Showapi_Res_Meaasge GengGaiMiMa([FromBody]LoginPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.loginname) || string.IsNullOrEmpty(para.loginpwd))
                {
                    throw new BusinessException("请确认账号和密码全部填写.");
                }
                return _IVerification.GengGaiMiMa(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }

        }
        /// <summary>
        /// 挂失
        /// </summary>
        /// <param name="登录名，登录密码"></param>
        /// <returns></returns>
        [HttpPost("GuaShi")]
        public Showapi_Res_Meaasge GuaShi([FromBody]LoginPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.loginname) || string.IsNullOrEmpty(para.loginpwd))
                {
                    throw new BusinessException("请确认账号密码全部填写.");
                }
                return _IVerification.GuaShi(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        } 
        /// <summary>
        /// 更改绑定手机号
        /// </summary>
        /// <param name="原账号，现手机号，验证码"></param>
        /// <returns></returns>
        [HttpPost("GengGaiBangDingShouJiHao")]
        public Showapi_Res_Meaasge GengGaiBangDingShouJiHao([FromBody]GengGaiBangDingShouJiHaoPara para)
        {
            try
            {
                if (string.IsNullOrEmpty(para.mobile) || string.IsNullOrEmpty(para.code) || string.IsNullOrEmpty(para.loginname))
                {
                    throw new BusinessException("请填写原手机号,手机号和验证码.");
                }
                return _IVerification.GengGaiBangDingShouJiHao(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 登陆记录手机机型
        /// </summary>
        /// <param name="原账号，现手机号，验证码"></param>
        /// <returns></returns>
        [HttpPost("SengModel")]
        public Showapi_Res_Meaasge SengModel([FromBody]SengModelPara para)
        {
            try
            {
                //var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                //if (string.IsNullOrEmpty(ip))
                //{
                //    ip = HttpContext.Connection.RemoteIpAddress.ToString();
                //}
                //para.ip = ip;
                return _IVerification.SengModel(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 报错日志
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost("ErrorLogs")]
        public Showapi_Res_Meaasge ErrorLogs([FromBody]ErrorLogsPara para)
        {
            try
            {
                return _IVerification.ErrorLogs(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 获取国家代码
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost("GetCountryCode")]
        public Showapi_Res_List<CountryCodeList> GetCountryCode()
        {
            Showapi_Res_List<CountryCodeList> res = new Showapi_Res_List<CountryCodeList>();
            try
            {
                return _IVerification.GetCountryCode();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// 搜索国家代码
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost("GetCountryCodeByName")]
        public Showapi_Res_List<CountryCodeList> GetCountryCodeByName([FromBody]CountryCodePara para)
        {
            Showapi_Res_List<CountryCodeList> res = new Showapi_Res_List<CountryCodeList>();
            try
            {
                return _IVerification.GetCountryCodeByName(para);
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }


        /// <summary>
        /// 注册   Odoo
        /// </summary>
        /// <returns></returns>
        [HttpPost("Registered_Odoo")]
        public Showapi_Res_Meaasge Registered_Odoo([FromBody]RegisteredPara para)
        {
            try
            {
                return _IVerification.Registered_Odoo(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 登录   Odoo
        /// </summary>
        /// <returns></returns>
        [HttpPost("Login_Odoo")]
        public Showapi_Res_Meaasge Login_Odoo([FromBody]LoginPara para)
        {
            try
            {
                return _IVerification.Login_Odoo(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 获取验证码   Odoo
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetVerification_Odoo")]
        public Showapi_Res_Meaasge GetVerification_Odoo([FromBody]GetVerificationPara para)
        {
            try
            {
                return _IVerification.GetVerification_Odoo(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 修改密码   Odoo
        /// </summary>
        /// <returns></returns>
        [HttpPost("ModifyPwd_Odoo")]
        public Showapi_Res_Meaasge ModifyPwd_Odoo(ModifyPwdPara para)
        {
            try
            {
                return _IVerification.ModifyPwd_Odoo(para);
            }
            catch (Exception ex)
            {
                return em.ReturnMeaasge(ex);
            }
        }
        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetBanBen")]
        public Showapi_Res_Single<BanBen> GetBanBen()
        {
            Showapi_Res_Single<BanBen> res = new Showapi_Res_Single<BanBen>();
            try
            {
                return _IVerification.GetBanBen();
            }
            catch (Exception ex)
            {
                em.ReturnMeaasge(ex);
                res.showapi_res_code = "508";
                res.showapi_res_error = ex.Message;
                return res;
            }
        }
    }
}
