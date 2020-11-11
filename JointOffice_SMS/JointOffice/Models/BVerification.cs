using Dapper;
using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace JointOffice.Models
{
    public class BVerification : IVerification
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string aldyurl;
        string aldyappkey;
        string aldysecret;
        string aldyExtend;
        string aldySmsType;
        string aldySmsFreeSignName;
        CloudBlobClient blobClient = null;
        IPrincipalBase _IPrincipalBase;
        string appkey;
        string appsecret;
        string JointOfficeconstr;
        string SMSUrl;
        public BVerification(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            aldyurl = this.config.Value.ConnectionStrings.aldyurl;
            aldyappkey = this.config.Value.ConnectionStrings.aldyappkey;
            aldysecret = this.config.Value.ConnectionStrings.aldysecret;
            aldyExtend = this.config.Value.ConnectionStrings.aldyExtend;
            aldySmsType = this.config.Value.ConnectionStrings.aldySmsType;
            aldySmsFreeSignName = this.config.Value.ConnectionStrings.aldySmsFreeSignName;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.config.Value.ConnectionStrings.StorageConnectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            _IPrincipalBase = IPrincipalBase;
            appkey = this.config.Value.ConnectionStrings.appkey;
            appsecret = this.config.Value.ConnectionStrings.appsecret;
            JointOfficeconstr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SMSUrl = this.config.Value.ConnectionStrings.SMSUrl;
        }
        /// <summary>
        /// 注册短信验证码
        /// </summary>
        /// <param name="手机号，验证码"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge SendVerificationCode(ZhuCePara para)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            var member = _JointOfficeContext.Member.Where(t => t.LoginName == para.mobile).FirstOrDefault();
            if (member != null)
            {
                if (member.IsDel == 1)
                {
                    throw new BusinessTureException("该账号已经注册,请勿重复注册.");
                }
                else
                {
                    var MobileCode = _JointOfficeContext.Member_Code.Where(t => t.Mobile == para.mobile && t.Type == "ZhuCe").FirstOrDefault();
                    if (MobileCode != null)
                    {
                        if (MobileCode.CreateDate.AddMinutes(1) > DateTime.Now)
                        {
                            throw new BusinessTureException("验证码发送频繁,请稍后再试.");
                        }
                    }
                    var parms = new Dictionary<string, string>();
                    Random r = new Random();
                    var code = r.Next(100000, 1000000);
                    parms.Add(Constants.EXTEND, aldyExtend);
                    parms.Add(Constants.REC_NUM, para.mobile);
                    parms.Add(Constants.SMS_FREE_SIGN_NAME, "身份验证");
                    parms.Add(Constants.SMS_PARAM, "{\"code\":\"" + code + "\",\"product\":\"协同办公\"}");
                    parms.Add(Constants.SMS_TEMPLATE_CODE, "SMS_33475604");
                    var req = SendMessage.SendSms(aldyurl, aldyappkey, aldysecret, DateTime.Now, parms);
                    //var req = SendMessage.SendSms(aldyurl, "LTAId9PveU7vRCx7", "buzgB7ciUIFPos00QRT4T5JOAedqzI", DateTime.Now, parms);
                    if (req.Alibaba_Aliqin_Fc_Sms_Num_Send_Response == null)
                    {
                        throw new BusinessTureException("验证码发送次数已达上限.");
                    }
                    if (req.Alibaba_Aliqin_Fc_Sms_Num_Send_Response.Result.Success)
                    {
                        var MemberCode = _JointOfficeContext.Member_Code.Where(t => t.Mobile == para.mobile && t.Type == "ZhuCe").FirstOrDefault();
                        if (MemberCode == null)
                        {
                            throw new BusinessTureException("无此账号.");
                        }
                        else
                        {
                            MemberCode.Code = code.ToString();
                            MemberCode.CreateDate = DateTime.Now;
                        }
                        _JointOfficeContext.SaveChanges();
                        Message Message = new Message();
                        return Message.SuccessMeaasge("发送成功");
                    }
                    else
                    {
                        res.showapi_res_code = "508";
                        res.showapi_res_error = "发送失败";
                        ReturnMessage mes = new ReturnMessage();
                        mes.Oprationflag = false;
                        mes.Message = "发送失败";
                        res.showapi_res_body = mes;
                        return res;
                    }
                }
            }
            else
            {
                var MobileCode = _JointOfficeContext.Member_Code.Where(t => t.Mobile == para.mobile && t.Type == "ZhuCe").FirstOrDefault();
                if (MobileCode != null)
                {
                    if (MobileCode.CreateDate.AddMinutes(1) > DateTime.Now)
                    {
                        throw new BusinessTureException("验证码发送频繁,请稍后再试.");
                    }
                }
                var parms = new Dictionary<string, string>();
                Random r = new Random();
                var code = r.Next(100000, 1000000);
                parms.Add(Constants.EXTEND, aldyExtend);
                parms.Add(Constants.REC_NUM, para.mobile);
                parms.Add(Constants.SMS_FREE_SIGN_NAME, "身份验证");
                parms.Add(Constants.SMS_PARAM, "{\"code\":\"" + code + "\"}");
                parms.Add(Constants.SMS_TEMPLATE_CODE, "SMS_158493166");
                var req = SendMessage.SendSms(aldyurl, aldyappkey, aldysecret, DateTime.Now, parms);
                if (req.Alibaba_Aliqin_Fc_Sms_Num_Send_Response == null)
                {
                    throw new BusinessTureException("验证码每小时发送次数已达上限.");
                }
                if (req.Alibaba_Aliqin_Fc_Sms_Num_Send_Response.Result.Success)
                {
                    var MemberCode = _JointOfficeContext.Member_Code.Where(t => t.Mobile == para.mobile && t.Type == "ZhuCe").FirstOrDefault();
                    if (MemberCode != null)
                    {
                        MemberCode.Code = code.ToString();
                        MemberCode.CreateDate = DateTime.Now;
                    }
                    else
                    {
                        var Member_Code = new Member_Code();
                        Member_Code.Id = Guid.NewGuid().ToString();
                        Member_Code.Code = code.ToString();
                        Member_Code.Mobile = para.mobile;
                        Member_Code.CreateDate = DateTime.Now;
                        Member_Code.DiQuCode = para.DiQuCode;
                        Member_Code.Type = "ZhuCe";
                        _JointOfficeContext.Member_Code.Add(Member_Code);
                    }
                    _JointOfficeContext.SaveChanges();
                    Message Message = new Message();
                    return Message.SuccessMeaasge("发送成功");
                }
                else
                {
                    res.showapi_res_code = "508";
                    res.showapi_res_error = "发送失败";
                    ReturnMessage mes = new ReturnMessage();
                    mes.Oprationflag = false;
                    mes.Message = "发送失败";
                    res.showapi_res_body = mes;
                    return res;
                }
            }
        }
        /// <summary>
        /// 登录短信验证码
        /// </summary>
        /// <param name="手机号，验证码"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge SendDengluCode(ZhuCePara para)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            var member = _JointOfficeContext.Member.Where(t => t.LoginName == para.mobile).FirstOrDefault();
            if (member == null)
            {
                throw new BusinessTureException("无此账号.");
            }
            else
            {
                var MemberCode = _JointOfficeContext.Member_Code.Where(t => t.Mobile == para.mobile && t.Type == "DengLu").FirstOrDefault();
                if (MemberCode == null)
                {
                    var parms = new Dictionary<string, string>();
                    Random r = new Random();
                    var code = r.Next(100000, 1000000);
                    parms.Add(Constants.EXTEND, aldyExtend);
                    parms.Add(Constants.REC_NUM, para.mobile);
                    parms.Add(Constants.SMS_FREE_SIGN_NAME, "舜浦");
                    parms.Add(Constants.SMS_PARAM, "{\"code\":\"" + code + "\",\"product\":\"协同办公\"}");
                    parms.Add(Constants.SMS_TEMPLATE_CODE, "SMS_33475608");
                    var req = SendMessage.SendSms(aldyurl, aldyappkey, aldysecret, DateTime.Now, parms);
                    if (req.Alibaba_Aliqin_Fc_Sms_Num_Send_Response == null)
                    {
                        throw new BusinessTureException("验证码每小时发送次数已达上限.");
                    }
                    if (req.Alibaba_Aliqin_Fc_Sms_Num_Send_Response.Result.Success)
                    {
                        //if (member.IsDel == 0)
                        //{
                        //    member.IsDel = 1;
                        //}
                        var Member_Code = new Member_Code();
                        Member_Code.Id = Guid.NewGuid().ToString();
                        Member_Code.Code = code.ToString();
                        Member_Code.Mobile = para.mobile;
                        Member_Code.CreateDate = DateTime.Now;
                        Member_Code.DiQuCode = para.DiQuCode;
                        Member_Code.Type = "DengLu";
                        _JointOfficeContext.Member_Code.Add(Member_Code);
                        _JointOfficeContext.SaveChanges();
                        Message Message = new Message();
                        return Message.SuccessMeaasge("发送成功");
                    }
                    else
                    {
                        res.showapi_res_code = "508";
                        res.showapi_res_error = "发送失败";
                        ReturnMessage mes = new ReturnMessage();
                        mes.Oprationflag = false;
                        mes.Message = "发送失败";
                        res.showapi_res_body = mes;
                        return res;
                    }
                }
                else
                {
                    if (MemberCode.DiQuCode != para.DiQuCode)
                    {
                        throw new BusinessTureException("该手机号的地区错误.");
                    }
                    else
                    {
                        if (MemberCode.CreateDate.AddMinutes(1) > DateTime.Now)
                        {
                            throw new BusinessTureException("验证码发送频繁,请稍后再试.");
                        }
                        var parms = new Dictionary<string, string>();
                        Random r = new Random();
                        var code = r.Next(100000, 1000000);
                        parms.Add(Constants.EXTEND, aldyExtend);
                        parms.Add(Constants.REC_NUM, para.mobile);
                        parms.Add(Constants.SMS_FREE_SIGN_NAME, "舜浦");
                        parms.Add(Constants.SMS_PARAM, "{\"code\":\"" + code + "\",\"product\":\"协同办公\"}");
                        parms.Add(Constants.SMS_TEMPLATE_CODE, "SMS_33475608");
                        var req = SendMessage.SendSms(aldyurl, aldyappkey, aldysecret, DateTime.Now, parms);
                        if (req.Alibaba_Aliqin_Fc_Sms_Num_Send_Response == null)
                        {
                            throw new BusinessTureException("验证码每小时发送次数已达上限.");
                        }
                        if (req.Alibaba_Aliqin_Fc_Sms_Num_Send_Response.Result.Success)
                        {
                            //if (member.IsDel == 0)
                            //{
                            //    member.IsDel = 1;
                            //}
                            //else
                            //{
                            MemberCode.Code = code.ToString();
                            MemberCode.CreateDate = DateTime.Now;
                            //}
                            _JointOfficeContext.SaveChanges();
                            Message Message = new Message();
                            return Message.SuccessMeaasge("发送成功");
                        }
                        else
                        {
                            res.showapi_res_code = "508";
                            res.showapi_res_error = "发送失败";
                            ReturnMessage mes = new ReturnMessage();
                            mes.Oprationflag = false;
                            mes.Message = "发送失败";
                            res.showapi_res_body = mes;
                            return res;
                        }
                    }

                }
            }
        }
        /// <summary>
        /// 发送语音验证码
        /// </summary>
        /// <param name="手机号"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge SendVoiceVerificationCode(string mobile)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            var parms = new Dictionary<string, string>();
            Random r = new Random();
            var code = r.Next(100000, 1000000);
            parms.Add(Constants.EXTEND, code.ToString());
            parms.Add(Constants.CALLED_NUM, mobile);
            parms.Add(Constants.SMS_FREE_SIGN_NAME, "舜浦");
            parms.Add(Constants.SMS_PARAM, "{\"code\":\"" + code + "\",\"product\":\"协同办公\"}");
            parms.Add(Constants.SMS_TEMPLATE_CODE, "SMS_33475608");
            parms.Add(Constants.CALLED_SHOW_NUM, "88888888");
            parms.Add(Constants.TTS_CODE, "11111111");
            var req = SendMessage.SendVoiceSms(aldyurl, aldyappkey, aldysecret, DateTime.Now, parms);
            if (req.Alibaba_Aliqin_Fc_Sms_Num_Send_Response == null)
            {
                throw new BusinessTureException("验证码每小时发送次数已达上限.");
            }
            if (req.Alibaba_Aliqin_Fc_Sms_Num_Send_Response.Result.Success)
            {
                var MemberCode = _JointOfficeContext.Member_Code.Where(t => t.Mobile == mobile).FirstOrDefault();
                if (MemberCode != null)
                {
                    MemberCode.Code = code.ToString();
                }
                else
                {
                    var Member_Code = new Member_Code();
                    Member_Code.Id = Guid.NewGuid().ToString();
                    Member_Code.Code = code.ToString();
                    Member_Code.Mobile = mobile;
                    Member_Code.CreateDate = DateTime.Now;
                    _JointOfficeContext.Member_Code.Add(Member_Code);
                }
                _JointOfficeContext.SaveChanges();
                Message Message = new Message();
                return Message.SuccessMeaasge("发送成功");
            }
            else
            {
                res.showapi_res_code = "508";
                res.showapi_res_error = "发送失败";
                ReturnMessage mes = new ReturnMessage();
                mes.Oprationflag = false;
                mes.Message = "发送失败";
                res.showapi_res_body = mes;
                return res;
            }
        }
        /// <summary>
        /// 账号密码登录
        /// </summary>
        /// <param name="登录名，登录密码"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge Login(LoginPara para)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            var member = _JointOfficeContext.Member.Where(t => t.LoginName == para.loginname).FirstOrDefault();
            if (member == null)
            {
                throw new BusinessTureException("无此账号.");
            }
            else
            {
                if (member.IsDel == 0)
                {
                    throw new BusinessTureException("该账号已删除.");
                }
                if (member.IsUse == 0)
                {
                    throw new BusinessTureException("该账号已被禁用.");
                }
                if (member.LoginPwd.ToLower() != BusinessHelper.GetMD5(para.loginpwd))
                {
                    throw new BusinessTureException("密码错误.");
                }
                else
                {
                    var membertype = _IPrincipalBase.GetMemberType();
                    var Membertoken = _JointOfficeContext.Member_Token.Where(t => t.MemberId == member.Id && t.Effective == 1 && t.Type == membertype).FirstOrDefault();
                    if (Membertoken != null)
                    {
                        Membertoken.Effective = 0;
                    }
                    var Member_Token = new Member_Token();
                    Member_Token.Id = Guid.NewGuid().ToString();
                    Member_Token.MemberId = member.Id;
                    Member_Token.Effective = 1;
                    var userid = member.Id;
                    var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == member.Id).FirstOrDefault();
                    var smsToken = "";
                    if (!string.IsNullOrEmpty(info.SMSLoginCode))
                    {
                        var str = "username=" + info.SMSLoginCode + "&password=sms123.&grant_type=password";
                        //var str = "username=100035&password=sms123.&grant_type=password";
                        var res_login = SaleRiQingToSMS.PostAsynctMethod_NoHeaders<SMSLogin>(SMSUrl + "api/Token", str);
                        if (res_login != null)
                        {
                            smsToken = res_login.access_token;
                            var isMember_Other_System_Token = _JointOfficeContext.Member_Other_System_Token.Where(t => t.MemberId == member.Id && t.System == "SMS").FirstOrDefault();
                            if (isMember_Other_System_Token == null)
                            {
                                Member_Other_System_Token Member_Other_System_Token = new Member_Other_System_Token();
                                Member_Other_System_Token.Id = Guid.NewGuid().ToString();
                                Member_Other_System_Token.MemberId = member.Id;
                                Member_Other_System_Token.Token = res_login.access_token;
                                Member_Other_System_Token.System = "SMS";
                                Member_Other_System_Token.CreateDate = DateTime.Now;
                                Member_Other_System_Token.WriteDate = DateTime.Now;
                                _JointOfficeContext.Member_Other_System_Token.Add(Member_Other_System_Token);
                            }
                            else
                            {
                                isMember_Other_System_Token.Token = res_login.access_token;
                                isMember_Other_System_Token.WriteDate = DateTime.Now;
                            }
                        }
                    }
                    var isryToken = _JointOfficeContext.Member_RYToken.Where(t => t.MemberId == member.Id && t.Type == membertype).FirstOrDefault();
                    var ryToken = "";
                    if (isryToken == null)
                    {
                        var token = BusinessHelper.GetToken(userid, member.LoginName, appkey, appsecret, info.Picture);
                        Member_RYToken Member_RYToken = new Member_RYToken();
                        Member_RYToken.Id = Guid.NewGuid().ToString();
                        Member_RYToken.Effective = 1;
                        Member_RYToken.Token = token;
                        Member_RYToken.MemberId = member.Id;
                        Member_RYToken.Type = membertype;
                        ryToken = token;
                        _JointOfficeContext.Member_RYToken.Add(Member_RYToken);
                    }
                    else
                    {
                        ryToken = isryToken.Token;
                    }
                    string[] strList = {
                        "0","1","2","3","4","5","6","7","8","9",
                        "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                        "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
                    };
                    var tokenStr = "";
                    Random r = new Random();
                    for (int i = 0; i < 64; i++)
                    {
                        tokenStr += strList[r.Next(0, strList.Count() - 1)];
                    }
                    Member_Token.Type = membertype;
                    Member_Token.Token = tokenStr;
                    Member_Token.CreateDate = DateTime.Now;
                    _JointOfficeContext.Member_Token.Add(Member_Token);
                    _JointOfficeContext.SaveChanges();
                    res.showapi_res_code = "200";
                    ReturnMessage mes = new ReturnMessage();
                    var mailInfo = _JointOfficeContext.Mail_Info.Where(t => t.Mid == member.Id && t.State == 1).FirstOrDefault();
                    if (mailInfo != null)
                    {
                        mes.mail = mailInfo.Mail;
                    }
                    else
                    {
                        mes.mail = "";
                    }
                    mes.Message = "登录成功";
                    mes.Oprationflag = true;
                    mes.token = tokenStr;
                    mes.memberid = member.Id;
                    mes.ryToken = ryToken;
                    mes.smsToken = smsToken;
                    if (member.IsDel == 0)
                    {
                        mes.ShiFouGuaShi = true;
                    }
                    else
                    {
                        mes.ShiFouGuaShi = false;
                    }
                    res.showapi_res_body = mes;
                    return res;
                }
            }
        }
        /// <summary>
        /// 验证码登录
        /// </summary>
        /// <param name="登录名，验证码"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge YanZhengMaLogin(YanZhengMaPara para)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            var member = _JointOfficeContext.Member.Where(t => t.LoginName == para.loginname).FirstOrDefault();
            if (member == null)
            {
                throw new BusinessTureException("无此账号.");
            }
            else
            {
                if (member.IsDel == 0)
                {
                    throw new BusinessTureException("该账号已删除.");
                }
                if (member.IsUse == 0)
                {
                    throw new BusinessTureException("该账号已被禁用.");
                }
                var Member_Code = _JointOfficeContext.Member_Code.Where(t => t.Mobile == para.loginname && t.Type == "DengLu").FirstOrDefault();
                if (Member_Code == null)
                {
                    throw new BusinessTureException("无此账号的验证码.");
                }
                else
                {
                    if (Member_Code.Code != para.code)
                    {
                        throw new BusinessTureException("验证码错误.");
                    }
                    if (Member_Code.CreateDate.AddMinutes(15) < DateTime.Now)
                    {
                        throw new BusinessTureException("验证码已过期.");
                    }
                    else
                    {
                        if (member.IsDel == 0)
                        {
                            member.IsDel = 1;
                        }
                        var Membertoken = _JointOfficeContext.Member_Token.Where(t => t.MemberId == member.Id && t.Effective == 1).FirstOrDefault();
                        if (Membertoken != null)
                        {
                            Membertoken.Effective = 0;
                        }
                        var Member_Token = new Member_Token();
                        Member_Token.Id = Guid.NewGuid().ToString();
                        Member_Token.MemberId = member.Id;
                        Member_Token.Effective = 1;
                        var userid = member.Id;
                        var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == member.Id).FirstOrDefault();
                        var membertype = _IPrincipalBase.GetMemberType();
                        var isryToken = _JointOfficeContext.Member_RYToken.Where(t => t.MemberId == member.Id && t.Type == membertype).FirstOrDefault();
                        var ryToken = "";
                        if (isryToken == null)
                        {
                            var token = BusinessHelper.GetToken(userid, member.LoginName, appkey, appsecret, info.Picture);
                            Member_RYToken Member_RYToken = new Member_RYToken();
                            Member_RYToken.Id = Guid.NewGuid().ToString();
                            Member_RYToken.Effective = 1;
                            Member_RYToken.Token = token;
                            Member_RYToken.MemberId = member.Id;
                            Member_RYToken.Type = membertype;
                            ryToken = token;
                            _JointOfficeContext.Member_RYToken.Add(Member_RYToken);
                        }
                        else
                        {
                            ryToken = isryToken.Token;
                        }
                        string[] strList = {
                            "0","1","2","3","4","5","6","7","8","9",
                            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
                        };
                        var tokenStr = "";
                        Random r = new Random();
                        for (int i = 0; i < 64; i++)
                        {
                            tokenStr += strList[r.Next(0, strList.Count() - 1)];
                        }
                        Member_Token.Token = tokenStr;
                        _JointOfficeContext.Member_Token.Add(Member_Token);
                        _JointOfficeContext.SaveChanges();
                        ReturnMessage mes = new ReturnMessage();
                        var mailInfo = _JointOfficeContext.Mail_Info.Where(t => t.Mid == member.Id && t.State == 1).FirstOrDefault();
                        if (mailInfo != null)
                        {
                            mes.mail = mailInfo.Mail;
                        }
                        else
                        {
                            mes.mail = "";
                        }
                        res.showapi_res_code = "200";
                        mes.Message = "登录成功";
                        mes.Oprationflag = true;
                        mes.token = tokenStr;
                        mes.memberid = member.Id;
                        mes.ryToken = ryToken;
                        if (member.IsDel == 0)
                        {
                            mes.ShiFouGuaShi = true;
                        }
                        else
                        {
                            mes.ShiFouGuaShi = false;
                        }
                        res.showapi_res_body = mes;
                        return res;
                    }
                }
            }
        }
        /// <summary>
        /// 验证码验证
        /// </summary>
        /// <param name="登录名，验证码"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge YanZhengMa(YanZhengMaPara para)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            var Member_code = _JointOfficeContext.Member_Code.Where(t => t.Mobile == para.loginname && t.Type == "ZhuCe").FirstOrDefault();
            if (Member_code == null)
            {
                throw new BusinessTureException("无此账号.");
            }
            else
            {
                if (Member_code.Code != para.code)
                {
                    throw new BusinessTureException("验证码错误.");
                }
                if (Member_code.CreateDate.AddMinutes(15) < DateTime.Now)
                {
                    throw new BusinessTureException("验证码已过期.");
                }
                else
                {
                    Message Message = new Message();
                    return Message.SuccessMeaasge("验证成功");
                }
            }
        }
        /// <summary>
        /// 设置登录密码
        /// </summary>
        /// <param name="登录名，登录密码"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge Register(LoginPara para)
        {
            //var str = "0x86B3000000000000789CEC58793C94DBFF7FC60CC30C6397A8CC6D1211327629D997EC7B9634180C33463363BF35525AA45C0AED2A65170AA150119254F625217B52B63085F9CEE8D7BDEA56AFDBBDDF7EDFFB47E799F3BCE63CE739EFF37E7F9ECFE77CCEF3C8A822E5910A5C301915E426A48C3CE3F4E15052505655D9A4262FCFE85243A297AE31CF8CB6959D3525048F65FCAB01030002C4B1D434C310B06C7033228980C12329D8600ABB3ED18FC2BCCAC70200C051276B7388352E14CBC201B5F5C3B9133DB01CC0F2A150432CC603E7E7F5FB3036562D120E83675DBA05CEE649D626E23D00561D229E4882B2B9E3B5F101D82F43C1AC03DCBCBF13CD0C1318F26534F6EDD8902022C983FC152C764FB2110583C7B97F446377C79B624844A2DF97F1D88C0308FE48F9AFA0C13DC9B67E1E58121EE787FD080875C71B90B0583F0873E457CCF70115FD5F468531E1904B6D766B0AC6CF0343F26087EA786348642C058A30D0462BC8A35D750CB5ACACF56CBEF6C0853E33FA0F9C8AF3EF60736A99591BFD09998DDB0647C0929166D820A4159180F1FB0733FC55F6FF05437D9718C41F33009F44397A79942B7E1EE5DC161812C68B84F1F7FE40036EC2702B6B7F8C3B23E0C00900C406E34646009F06910ED68F8225613D38B4F0382F3F02A305E52005623E5CFE0B009FCFF987F465883006A271009982F30CF94790707D1C894C3162448C1F8565186682F5FCD850FC01B32DC717FED16A787EB080553F5A00E4FFD55C03CB6753F9AED93E8D28C5E511D5C2F95944C14C70E43FC5349B0936108B272340ECCC5E9B107F2C94931488275374B1EE384696658CC79028FA24228105F109CD4F14009C8C64E48B257D7C3E9CCC3541F68BCB057CA9CB108BF3F2A6B08C732CB5BEB87AF0E833D33CC59A426248D661AC1F6C6C682515052D05601957EE25AE26C4202C490BEFEF8D594ED1E65B14557F0C45185A49415E414B4501ADF6159A4B0397D3F4FF164D1BEE0FCD4F163402C68A49EDC729402F29D003BEE614CBE883A3816FF1F7FF712415FE4CF21BDE004EFB164F868A1F4754F1BBFC015CF94DA269C0FFC42394BEC3237ABE29A0F2075A5AF9FB5C62FE9B447B7E205195EF730961D0B788CEFF6F5C42F5FF8C0DB09A3098E942129D7A8C807F9C6F3ED907FDCC313F73CCCF1CF333C7FCCC313F73CC528E79E7BB4BEBB3771E052E1817EC17E04359C9A8B0CFEAEF651392F9E5519539920BB60129A38054607E8C94FF00C684B131B6D033302260BCB05CB0FA0B00406FA3F7003CC6FA46FA0008C478B5631C00BD13D001C02C2CCC1FA340183F567656560884150685B2B173C23839E130389C0BC1C7C385E045C0E13C423CBCFC028282829CDC2B84850484F904040598202030630C849583959543800BCE25F0DD857E07E06507998225C1A0B5000B2F08CC0BA25702620C9EACA0A5F251388885C1910DCACE0183336E28E001584060300B04CC64CDE8DDC3E80720BCAC7CBFC86BB1F15B62A06B770BA0F7C55E624769E7DD15B47A3ABE4EC18D14C101135A21BC52447CBD84E40629452565155535751D5D3D7D034323636B1B5B3B7B871D8EEE1E584F2F6F9C0F99121018141C12BAFF40E4C143878F44C59D38199F9078EAF499CBC957AEA6A4A6A5675CBF915F5078B3A8B8E45E45E5FDAAEA9A07B50D8D4DCD2DAD6DED1DBD2FFAFA07068786475E4E4C4E4DBF9D999DA3BD63EA020160D0C7F2455DBC0C5D2C1008180265EA02B104316FE085B0FE22CFC6A76509C5ECE65F8BDEC72EA01D7B29EF2E074AC16A5CD08DF41426B44EB1577C82296D49D95F1316F1B794FD2EEC0F5D1D002718C47878605E401358E02BA203BF5D297F5BA5B9C0318293D5ECDF20B7B03B860E68873A3638C5F905DB2F6E95A00314AAFA88F4D9F19CF95FCD178717031A6CE4930F4D882FA626CF3D34F46879CD5A3056ABAD38B2F129BED2C96CACF195C42399DA9C530A77DA4294627AC5F847675B3B56DF6F49BBC93317D730184CF0283497BDFBE460F4FAAEACD1BCDCBB6D7115C834D56BA956E3EB1C0B8B125BD5CBD2C5CF381B042DC4355C2A984AAEAA1AD0B81B21589819B9CE23FD419DA35CC55919B619A7A3BEEF27853660AE1FB43F76C27C8768180D991282173B57C612BEE9B6BD0D9615BE654123A92F6C4FE7A87B84CDCB929BF60325A34365EF9DB60785E148AD156376EF9DC6E3CF0F5EDD48EEDE5C77FD01E75E47BEEB25C7324FEC8B5CC5675E3CEA228809DE7CAF7CFCD08858FB68C6CE97B11C6B74E3E6B457ABF055724B290A3F33EAE77FECBC8BABE3DED5739DC7AB5D64895E82E2D92591020722211780ACF3A2F73B340F7B0553BAB4451F58065DAF32DB56AFF0AE05B1FBA825D13E77C08A372EFA8CE9D8D0B056081D403D67987B5767F782A8AF635A8612479BC98E3ACBFD8860CBC48ABD1A31735CD89DD49AD9643AD0A63C4B07F43B5AB2F0872CCFEBEAAA46A1B1AF2AE422E73799CFC3B29EDCD013B060F5BD6ABDE5342EDAC3D8384D73145145A36E4C9AE3DA91D16A7274CB29EFE803B5DAB97420C1CB7DBE3BC180390664FBE6A4228CB30A345815B43F7A7E74920EDC297BD367275578B126F79738149A960724E74DAD808EAB6A46023B2E4749B25C4BC92B3B1B78BD0673F85E6FFFCE34E57349F7B26DF7E815E2664DFBF764F2CA4FCA2729EC9F9C1D6DC8B200311911D4B4713319EBDF1BF24AF28BD1707768E56B0A3C3371D9A2232DD902FAC2D18676A866363668AF9D3790D1115A95AB7ADC5A8A763DAE3A6F4A1435AE2616B9238D609365C2B996FBB95A1BBB1A2DE651F56CD96CDF367E4B3D01E05F564143AD629302898B45D574E0D21619F1EAF948713A704D9E0E0C9B6F77DC4E327D0CA303D59574E075CC908B3879AF201D88394207BAC4EA4B8C0F2AFAAF918D3045DDEF504EB9197A5B9A1C2280362372E41E316C432AF78E3BEAA310163DE71285E1CED70E6DCD0921263BEBBD907E1856CE8FBB79BDEB1159DA942B39DDCE08159DCBFB8AD39A237F0D1C7BD0B741621C7188FC0294F5D057FA392D53CDB5607BBF62C62D3BFDFD4109CE9C98DAFE5DE43B198FC6CB2BF49474171F07981AAB4451B6A7C76B2B1003A2A1955A6707316B9E5D30330DB8978512481AE89FEE2E2B97C9491FAA8F34B221D915BC2F2F278CA53E1AF55DCB7D48F9A06481F451A7CCF7E71183B6BEDC63D2A7FDD649F1406377DA794DB645A749E4612A60AF7CC52A160573C76B8ADEF3DE8E1D921E34DA43DDD279BEF84E974231E0318E5053CA73B0893B79D6C056589DD2E4B8BD9118BB8A0EBC5CC7587BE880D438B57D74519FB07DF1361D9809C8094E5AD04D7A9D34B093FBFD166ACDA3746A6BD2627DC194031D987D269755AE3374438E26DCB380A5A15A7334FBC39ED28157A261742099E08BD1EDF6FDED865C7BADA4DE6B5199A245272ACA79B0344C29E698A2FDC17B7D96EE98D094CD9ED1E81874EBE056F7B4EAD5C743469C26B2503645584F1536E18A9C5DB79270F30FF76A9785BF8B6816B0DCC653BF1EDD79EAB43674BD45687CC733EE39A1EE96948C9B47E1CE0EA777C457565642F58932739AE3ABABC7057DC26107874DDC5C91697B46CE7729BF162D9F78D6D46C6FE9E0D805A97737575FCB7090B03A97E4093C7755B80CD3A5B85ED6CE90B3F8D853E4738D442F17AA4A0CFBF86466D034DB8392BCBB66A8E2F38F4A03C7D4AA7BEDD20066748ACA6FCA0F771654115D5127FDA450ADBACFDA4639F0A1178EB5E406BAF2A8B0E7CB6DC20FD2D340EC9CEC489E60642B7C3CB0A3DB78BCE5D21587E16ABD86FC52C777F55592CD7C5DBDFCC5AEBF080CA05CAAB66465EE1725B02A2B04E97AF070409B02B3A8EC73F8E0ED5AFB4D179FDC6EBE6210B7839853B7774F659C61C3E6F284C3E109EEF732D2860101D9F2D6B77400B2265D4E93617E57946794E445967F59E5DEFA77935FF804850FB746F66981D2ADE65EA7B33597E2C8C1A6078CF5EF826E724734C80B930E41FA8BEE380EFD2AB1FE529A692FCDA3A540FFC47DA11531A1AC4FD742ADAE8A08E2B3C4C102B46DD0082DCC99698E79F0A56773E1BEE88EDAD9329F7AFB312B1A97D4B5E6D88B5D0389606152B1F0AA7129E14591603F54FA5823A9249AC839913F8986046E1C1549A8D43AF57AAB837321473FD8C532531061BFE03DFDFE2D4D462E7DA836D2C0FAB463EBEBF76F0A671A07DF16C41AF4DEBACDE75B12F620CD60C6A960F25580534A49F1CD63C95670639759E9DFEAB606DE51AD4166CC149CE87D03C73D5F1145C93D4E6845AC3B139679DACFF36438D0B52EABDE1F00F8C3D5ED0A3056C6562593F3C73336F94744DF9E794BDBAB4BA6036927161AE733EFD201AF1CDACA02B9775374A0BDA5F405F52D9A5A428D0F767DB520927A246095711CF532C5825A5676D1BEDEBDBCDBBFECFA0277525D79BFFBFEB789FD318E8C308F7D63400772096F168F1564533C551AE5E20475BD75BB9FABE1737AC3A93DB6F5A3AD371CE61AF582F1B24A42CA2B32DC0BBD2B24A764F6BA0FAA47EF6E1BBA10FF746D6E697CF4C54BC780A325E70A9D266AC4C645661FA74A3003057E107BA6895DF7628D169136A8ACF268FED03C2C354D56F08CAFD5F5C3777E4BB0A603326C8CFCA92136C765BE8DD5BED42D379DE7C8E3B3C26DFABBA762CE2E8267CB6179163BA5F38D5127B59FBBF155173D476BF2C6CE3D814C5BEA015AA10DE2E7C604DC70BA782BE5E1A155DC55A95939BCA3EA8131A93163AB1D5BF557C45D8D2FBDDF34CAE37BC4AC2616CE5BDC7E1876B1FF1CC8311FB0231EA16CD95CF95CAC8992BDB3B6D3AA510E19EC47905BCB5721C51B71ED0A8FD6F47D9CDDDEF2AB0FB387135B787CEC43355A37E56FA7BAA114EE18564D4F5936FA8F153B8489441DA9E592BDAFD79EE2FEAB4C939F12F1E0E2CA5BB76E14E515259A85DF56C1674B3C03EF96AA43BCF25BA956C175D76CDB4B9F6B89A93AC923090942BD91A5591B2098E366CD16C912E52CD976CD22273B5DEA5E640B444449A6494A5E8E02FE6D756A348606DB4E079CDDE9C0930B09717D8BDA7174A055980E4C4B5CCD162F5E3F1449ED7B419D3598BA16575CBE3C0DEB88683CAEB253D74B8DA61CF391765A7FCBC50CEE546AC631EBAA879234EC811F47ED7AE9E0B78A2F902F08615E613F3147505C1D649A9F4007769437E6A4343734AFC15DC3EEB0004B5DF4748BCBDBE66F7C6DC062DBFD30D49CAB7EA2B3296EFA28B4C53270FDE5961A3F417F7DC8A9676C1CAB43F9ADDABD453CCF5243F6E0521D1D9F3FFFADD57ED3B6F475FD14D93BD8BC13AD4ACD22B59B069AE9408512D19438E918A041EBBD2A75EEE543D796D9B723B3B73A7522E656EE9939B6B9D2C2889CB6B32B257BDD0DE7EE36531F97F33CE733A5640164F8ED78517213FF48FCE9CB0C1EE39DCEE7258CF21A1E3D849E6B775AB7D597FC9000DEDD3FB9C714D762EB99BAB99AA0894AE5E6B04E7DB8E5351D10EAB652A3034D468B238BB63D74E046EB7C7447E202639FFA6A74E7DBB589C74E0620A3A6E4225DB9945C21A32B82EC273B6E93EE0E48EE6B3F1E04AEB709B57FDC27D974418E5B3533ED26552646544C3BDB2450E80A2A60857459E7EA6343F331A19A0BFB89D46942B68F3E66B1A908963DB38F13DB99E7F2387F6F58B9BC83C874E2D3E0463DF53DA5A575DDBB4A391355B860915332242D76DA16D5EA3E1791FCAAD77A584B3F5E6315523BA162124E3B331373693D33AAB37B0F383B1CB13D7AFAF0E1E3FB6EDFEC676C6783F6C6FB5C3A9FB28350699C155973F242C62DA7583AC0D2A1C96F6D01B20D40B1DF34D7903757DF4818BD55E7523E3723D5AC4369DC688CD142D10EB3135FBF168DE92966EC7D4D197B59E88609FB7A3A10199073D6DBBDA8C4C9ED899EFC3D8D8B1151D5B91BD9CC10EA3D7D828D8EF98F09FCCEFAB1CE026C862F48C3D9C7B7B981505B6A564E0B26BDD0E41C5994CE8B7A6C65D45F78FC857492AC68FA2EC4446E00F7C0F53726FB4EE23311C2BCF77D9E30B7CDFFE60A4D72A6D658B9BEEB135B3C309D9FB87CCB8B9379A0A2214D07C4F8E94089E6794AFCA3B93F32A894F0D3C1BDE8BC06190DF72BBE968EED4A1D2FFD1DBB5EFA9B97BCDBB052DB0070DE2D0AB5AAF0EF39917BA2C8BF25A2BE5A27B769579CF05B2E27832627E70B7A388D2144E6E9661D0542EC19F9C9389094EA8BD288FF50F39E51517CEDB760A3204814949C932251A264442489802034195462834489922423994640B2E4D8C4A69BDC923348EA26872648862636D0C0F89BF987F79D753FCCCC9ABBEEBA1FCE97B3AA6A55D5A9E779F67ECEDE658763BE05DC6FB9A7FFFB16B0F2ECA88ED295E22C22151A9FAAD03207F7D2124E4F7A13DF91647F89060DE6CB974C9B1939BE52B14C7011BC835B103DBFDEA91597FA6093BD145EBE8F3A7CBDA02DE2EEB32DF5380511777E0AD7DCF3F2B5FB88139D40FF1612DFA3175BA01AD150FFE095A338D1D649DABC3A7F57B5F1705B665F8AB485787FE76EBBE34AE06EAAF67D66CE2B11A93423770BBE7EA367CFEF705E28B98F0624230CF593BECFD99AB3F03EFA6EE9E0630F27E6C9AB917EC3387AC1721278756FF12F4398339E8AD5B31785671AC5E884509CAB589061D7FE1F72BFAFFC637F91A2F1967E71DD634B4A2A30EDCD17E6F8BF544EF41680572E6EBC95685B47469990A606643D1ADA0DCC38EABB0504E9A70378EB2FC3831433765787630E4E2F50EB7F67FD977A2AA61C1513DF2B71A9581F7FC453B5DC10A45876092406FCC32E09EF63EA7A5315C86E58CD768049B3B3F3F7F7F6C0E8FD7193AED04191574C26AF060506C96E01FCC2D53EE4D43E1C2D0381D1AC94B2DC2BA3C4F68B3C93EEE2D15D548BA0B37E8A5067888D038AD4BDDB8EABD53654ED41AA3AE0FD50C91B2BB05C085AF52BE486DB5B6B47B16952F0A4AF98577338D3F3A8BA7D8D6E9F2385FE514C70ADCA6AE6F203A2C2C3C1159690C3AA5F978161D069DD1DEDDAB44A4B51A3D95FB213ECFE3B642D3A4AF4A46EF8AE3F0933ADC07A40DC026A013501C93E3F181BCCB798D8DA19DF813BB53117E318CC02CF82264F05DEADFC09AAF79D16840EB6BEEBF1C45CA7EA415BE27EABB1203949603D299241026E5A485EFD1EF59ADC9BDF0A0FEBF738BBBFCC07D0D74C9CB8162E6E7FC1BFCC9428656404DDF951F0CCFEB3A6C4B29AED951CE2FB2F27E013C72FFE397137A4DE06E880F2A2596CC4AF74852EB38FDEEF27F9575A6B430C5A7FF01C81AD48B913CFAA67E91E060B239BEEB79163EF6D683FE8170A9695E2CFE60038685D6A364A6793ABCEAA6B2FBD3F3EFE934EBD9B3E3B3ABE3FE73EA1A1D01EAD3173F165C622A0E916A0C922B32A44CEFACD9CEBB0382280D76EF12126E2CC2BFA35472CE335756E925AE30F3E659763EBE4068104C68FB02959F2C783ABF15138C2655F198B7BFDD6959BB362A8D43770099BBD5B400D02EAC40E0D84B62E40DFAAAE77D64ADC431B0108DE7FA017D80DA46EA1B892C7BAC75AA02D226E1840113AC90F2CB09583DAD17BBAAA2C932C56DBCF6B7FFE5A4B6871AF1CD4C1DA1E5EF596E6DBC36F1E1DA2225390130313424EEE3BBEB964D06FB1D552EF60124A314FDB09C733CDB290FD37590107E988B7392DE01DC71BA7BF002C405348090CC7C0E339B1F1AE0453BD1114A12B18BCB1504AC311A102D55A93D08401DBE9307A18F5D1B4246350FCFE248219D2228A115D6EF6A901C9F31D9EEEE716891C1856093CB180FF12116DA4F360BE42BDA368AF029810BD92BF7738FF9271F354AD0344B36ABD664F2513D6E1CA5B42CCC5FA0C0070D50B60B7B320815284FFD1C6207356C8647E4954D149B4B1072305046996AE6A60EF39DE35607ADFF58A0B0EDC974FC1A5EE78EA621B1190A52DCB4E8B9E2F391DE55574D8772BA6D22ED00833F771314368154CF1538D9149BF2F59CBCB8D11A28FEADCF3C302437671BB51CCF36C079FFC1C56E31FEEDC02483D0603A9B09F5F1C2ED6AC20A7EA05440E5E83577EC4D7B6340B02319401466A2A3C2BCFBA2E32C2177B550F593BE223B3EF1E9E481B44F831A29D09A611B36286BBBB8E82273B5EBA9D4330E22271B67DFA37EFFF2CC2ADB11F966776A1D152B2DCB9EAA0FC856B8426DC51E22173EC087AD18F0F4ED9DFF8E3C094A9466A036ABE0F9EDA4280EB2646EBDAE69AE7A31740F539E8836BB9D3808898F8981B1ED40D977D3AD3B98467007E5B08B00E86DC3D390B365187E8A7A8A8AF351F0BD24B80BBF9C7B9F9A96D138A87BF2D0FD205106954986CB6511D5A44C8EA43BA0B663BD1AC144F52F3ED1307DD0EBB7DE5CA3E79AF5388D644AADDD860134CC1EAF1B3A78161085277A3D3786AFB6CCAC3E4D0A15E9DC9D3E66111A4047FE4F2A8D86E7EB26A02AD6547DD71B003E18A47C42F8843070BC7440093094E1671085D2EAA07AD5855830734FB14699FDE7BBB8E47E7DA47586FCBEA6530C677516586AA72DA1F36AD1FF3F8E2E33851B914D0C4573CBA95432B6BC98228DD64282A5D096D9A80EE590C4FA1276CE2901AE5FA7560A960F1313EAAC219230E414359CD6F2F96CDC75A2C31A2DD2C12F513EED74A1D295FE446228A1DC2D1AB6A8D2976AF0A534CFCF3F65E850D88F2372AD2FD7A2AE126C3A71DAE408A3358612553745851207660A19DE830DFD4E92C9AD3B6FE44E7A40E79A1FB2DC18D41D6A34F9C00D1A9FD175EE1143FA0E9649F2E1F90382489199B6597B364816CDDD6135499A16E6140AEB250C3F6351EC07B60F40258F7C8BCCA3C4B7076A21052E6C9DEDCA9BDA38F1A785A7EB1A414578F6D45EFC974B671637ACA6E189B0E21F5D9E572712156B54A659C3332252A59EBC243A6CAEC2E9CD1035CA47D78F67358B2D51C66EFA6396048EF17AE2FFA7B0DC168A45EF8F8F4307E2EE62A529C0F07B0944C2C30BB8A2492A0F23A7047E079DFEDBA8C8844548B0D2B74357551C99D15166FE11D54B187297E16EDA64F1C7BA1982AD9AFCC944C68F2EBE6DE61CE57A827F4D02BE2C4997CCB40841116A2541B85BC6FF6F59E6E0D0F908BE0A22981806733EB4A68F816607054BD5EB930773A71087796C90EC434772168A072965AF12B8B61D790F787344D495734E35756190D8C45C893A3164FA2D5EBDCF156A5CEE3A5D1953D6947E043F2A2B3B1D6C997DCF19F0A153DA66B07680965EF33918F2C682553F3A935844CCD5F780A45CED667B7B27631038F3B1216EDCDA9B2FDA565E424D0261992052D5043817DBF522EF04905F8411E77F4E754657D81067328361EF3AE3B87F8D4C80A3126CB8E1E66EDA649D79F8737F124B3D0FAEAE2C1E153DC8619D76CB6D7DCFD6F55386D2FE4BE6807DD02C8AD1B27F7597FFD3D24F6CD3CE385890A4CC8D316A0BD919516286A7FB090354D3FC23B9275F8B746A84404E64D63F93A61C6F2B94E30AF748FB6647CCFF3673CEC1FC1CF82CC0036786A6C4EB29F5FB907C75BDF02E05281BF8E5B9732BE0CDC785D182F1DFA41D58E9CA35919FDAC97633534EA7E178FF9BD9875AC2FB8E170F07F1A63A52EF372843BE5926E8FD6CFA92CA8EA317F97CF2B158DABE9B8AB2BBD3E2781BF78DF2F216F6977EFCF7D2F26440682D173D97126F3B8B9DEED32A7DD22A26966823C7F598174294B70680BC96A3FB6B776C55DFA329C08CE3E43FF522F6FC545AD87CF73A2A34939BCBAA9109643D07A69F43833E955B3A709278DE177F23BC1EABF5807CA51CDD781CBA009204C2D757D2620F5DA63276F7FD89E272FA1E37FAB26D0909F2D9A8C073BAA36E158F9C966F29D5EB0461633FB537FD1E40E533E5B01263BB75E103E3B630F91BC47407C09F6E59CF658044EDB1DC2FA36DA288AEBB1A1465F18589F4E2A31795E88205C6D79A32D28906A8B7207942F5F6ED5D459FC23DDA741D16530D88BE8B821E6EE888C5EE0618A9DEF29D7CDCCC4F95688C7D175A9A8DD332DEB3F6BCBACAD71C928E2CA4C5D9D631CC31E0C016E8BC8D28D2A9598473923E70E8A37F63C670670FC93EC882614595C0D45E4C9D05CCB17A8CE8B2275522B59E55C7DE0348DAC15CDDB69AED5F8171510C2EE2F4FA2F25334690DAB919CA16189BB826097D013FC5C499C03B0BA6C2EC3D1E4F282D9D03D5F9DFB6F36C57A5D843D4239460EA3EE8AB77BD3A28758445B5AC235F08ABA1DF01A7C0A1A487315017FD6E998FEC84F75EBB64ABF8B956A9672FCF3D1FE2348A794F85D72A1CC9CFF27F7E73947729BB78033B2D65BC018ACBE3ACFB9BECF287577FD11C73959CA7CA00022B15262A76791399001F44EB0B548ECE9072F4635214E5EFA0218EFFD85C9BD4FE458F964940AFBCD4403EFAEEDB31E668A17CD392096B57D5C983BB372194FDE57F6CC37D670619150F2373F4FF9EA1B580F0510104D7D67FCC7E8AEBEDDCAC78FD9A4457D9CDBCFE61A6EEE40DEE2BD5DB81FA3B1464DD12ADA5CB4D0E97523939E5EB6CDF118AE2DAD122BBB19C987F2FAD1AC8A76516000D4FF838B336E7E670BB7FEADC6C31A8635CD2DD5E0A79055F01FAD287AC1BC6F1BAF3C23F600BF2AA9E358FC5E21BEBC7C44251BAF8C4144BF5BB90504FB6917636F015D2CD275A87DA793E059BBDFFBDD2DE7A483D6FC13C6263F025DFAA8941E8AF8C4276DF02E5FDF5FAD22B56BE33B5C182E6AE53F9B75376CF46DD654719477F0CF79FE03FEACC54DF92BB9915980EEAF4335A9D9577F61A4FEE18DCC1472EFE480B82B6FBD3ECBD6D03734E175E7A75A7353B5E1C7E881BBCD99EB2F247F9517BFFE3CA6DC0C4DAE5ACF3BFE323B5B6C13ACBDB2BE6481A5189935F3983E722632DA36E7C4CEA121F75819B0815DD2E6DF0453FC948A168DB8D2A1C785451F33FB49CB991B8F814F25A25D07982B5A888F87F956E17195D0A56FE9D9A5AB7FF3DE66E04C93FEC21CE816308B3CF792676A8A7B5BAD2A570E4A60F4084C3372127C0C923CFAF1BED53FDEDAD7B9D4A8DD0B246B00ACEBA9ED941564E652A10702991E28E253224BED03B827C47C1BF6AA9CC6670D824D321BD8A9787E89A8EDC596E4736E7C2464033A4A227CEB109A1616A9CBC4C37C3EFE83B8AB53791B649BA8B76DBBBCEC98583C3DCE7CCCE3F9580B30A97E1FEE5F3C33E8483A2BDE49F0E141EC2BDF252223AF07DB24526B870CB780EEEBCF2BF0082844A173F1A93C45A4442E9A59467DCABABE26EE4A33E96D8377DEAB0658434366A4BEA1C9AF04A255BF7E599F188A5ABF8718C68E40AA93AD031C3B3A96B11BF84C634DC323CF9ED25C99E907A1A5D8A75F124492B2CB519B65F59AF24BEBA8E39B4AB263954520CEE2A2554E8655D21067C47A5893A3730B086BB7F45F4162BED8D134F1C2B2CE7C80BEF8691C22653E553C89B700AFFAC9E495D0888DCA9C55C26E0B227B053C08EE7D1B5B0DF47078FD26838B213BF7C90C8735EDE15A8E0CC8A2A93741121F556288ED559CC429976003464D262A4FF7C4A9341604491A16B7A74CAD190C83E68322D8AAE9F78027D7CE5A5395423F6B10F52DD91DCE750BDDCE227DC8A33D83A335C03AA343F9FBE595A04DE5D7E6DB515BCE8D460AAD3B8132147B9F15B6B66F018FDB56570F18FC8C6E7A93BAD28B83711F58C3C5B469BD63FAF3778ACE4646B510298C8C1E69F9CD4DC1DB7260357A3D4547B1247546EAD9DCB5CAF1B95ED639E3D4F62AFA1D56EA037712010B353BEE895DF9F63366D577644EE693F71DCDDF89C53C56A212E993CD7BC049383AA1F72E2C0FB275CEED1068ECF8336B69FE1640EB7B0B30DEF7F3280918D3EDC698749DCDC2BBE424E63A5E3755A67AE46F5F8685D54D6B3F72FB113F333E3F08F0088B6AEDDED872E51468E22178B742D1D5AAF0EE70BF31D015A3D1694066B9DC4C90D1D34629813C6BE2E839589F9D8F8E688ADE23C88ADB620ABE8BF748752369FB1E366B790D538B156F31CAAD8A9E91ED3EBC98CCA55DF73C981738F7F9314B62EBACF5C540D3A8A44DB5721B75A26B70B53E510F6BD4F54AC61D381BD03AE3523108CB0DA91B01441CEEF9E1FE1F35EC28E86765F98ACC82D6E5C11B4711CF86EC198174A307C34C91614C435C78AFC38EEF4DBDAFFC71F3043B638926892737B75F7AB82BE60BE23EBCDE4B09219CD16CAB71E6478E8A6F79AA7CCF533353EAE6E8C93638C01BF607293BADB5164B1B9BC97C91D8C3560948CCCEF0D45333EE380458ACDDDC0210E1D52DF15AD81AB470C95035F452B3E2CC59D4F19DF3C381C7B36AACAE0B24472EDCF192E8323F8534C7800772C3C89259F24CDDB30D948E25934D4F7751F2DEDAF5F62745A67B22774BF589F1F0EF9A644B220EEBA1408C77CA38C9BAF028BFCBE07B5498A5615FDA5999B03ED19C51E4DD3F2499D5843FD319CD8C3BD48AE21807F97E0C344952A9F27F7AA64818AE4BF13BC4686E017E8ABABCBA31E67BCA770BB0405D7832DBEC706792A6B23AA8C4BE9EB7D64E173EAFFD49FEE55078945A5CBF79F2131451C73D3549936397ECB86C54A077CCC596109D5DF142F26E36997199D31EF4F518574794556D01A52C58BA97E1CED0BECB23F282EC8EC63D12B47698DF3BCB420BBB00B689D36119BD1435FEAA9346E43612F440E2E4A16AE3733248FBA77535A610355BF5FF4D9B6EF2A2987BDFFD6C0B40309484A8FDD33245EE91B884EF6D69032B82D4752971625296D0E007B1ABF84C0A11DFAC4BECDB68AAC3250B26DD33F8A64F1B5E931D36BF2EFEF4F3448837ED11CB4F1AA6CFE619CF63A1BE810F8ABE717795078C70B26ED65EB00FEC1B0ECDF955AFFB1099B8E2BBF695D5F8691C4E8EDBF334A5C757AB1CBF3978141513396796EF966B4DBCF4F468F043350517F14E4C41EE205D2B42A50A367B633CC3F269E5BEDEC6679342E7ACBDB6CB0027946344E7B0B2C7017925EC51F9B65BBF8B9BDBCB7DDE9043659D0B94B6913C0B462B0B1D59EC4B9829DE1CB91EBE8F3401E573DD79AECA5B0E50EC936CDF339E1A55F8B6E5CCD994A475BF38507EEDBF76A636FEA6D71CCC0EFA167075671457BE75FF1630D9FF4FAF7B25B00E13894DD5391A9BBC4ECA1AA2C054555765F606B6ADF85B5FB2CAA405F6F512EE08F5331346B6E8F8C81535716196429DDDBF3956A6084823FB20AB7C2A68AE87FE35DFABB9DD5E594890DF59B39CD4F9B1F4A64D4D9F32F761A0F77A2D221CEF161004345BE82A5F95E3C5E38CB4D06EF18C119E1DAA3521E0F262137D7F0B80FA3BE850779F0C59DF1365C0A3F590FA045FFD750BF88A6AF39792FB67ABD954941FF9F325F7BD972481A3C2356DCE6B0ACBFA728A2E9513C6C9DF55F25E1A69E3A08C8CEBB8C3C065713900B5F969544781D259FCA28AC417BFD5BFCFFDF5FAA0A3428F366A8F2A72FD01FC4768CDFEF398F82EAB0031BC7FAECEE4DC23E02444EE2782212F168A525821A581F5BC42C2E79E242CC7863CE7C1F8ED85763E1DC1913251AE2D51C9BAAE2C3DDC44109DA6C90B9B608DE7749B17A8A7C2F6BF7F68493051ABE2A81F1209B74A883CFCF1D6BA9D1196786266E9ADD195DFDDC4FF251D844AFF44D5877ED476D43948F791E62DE51CD1F2BDB75D1244F3656E84F0A0DEF10E15D9C9C682CCACEB854C1FC70AEE3A94096437A999C3D50DA8352B7E7E0B4846303230D86480F24148180DCDEF9919E3AEEA766767865E26B57CE2B232C0DB7F645AD46B0A0F84C4CCC4DB78E269B6E1D2DC215B2928A577B059EEFA6A65DEF1DE467AED64E1E4E907CBCE66034A61F2A3FA1BD5F50D4DF0C694D9A74E8EE73EA2D277304E748EC5257DCBAB10EFA49639C77D3BA8FCFB660C277670C520AE5E4E01AD92EE58210811DC0F60F3269751BC97EB1404546E3C4ED9A0E4122E3E306264048A464EFEE809AFC7702C01B14987B7808E268ADE537884C5D813AE94B1B5D636BB45058EFD29E33910D86F309CE8B049A54732E8A277FC4407792D864BBF411894248F1FA32E6F10F8DE6BAC942DDC147B4F59C372BA2F656F1E6B5390784E704DB2D18E0AA573B787A9396E282CBEE7B9F0E9E1CB3CECED8148DF02DA11B43FEBB0F5050EA96D0C6D691C48A7835DBC890FDE1C3C360EDE6F5C7BC0D03E2E306D6F8AD73789670A4A3ABAC9F983EBDCC76E1B8841E7ECD5898C8C61852659D8C5C18DED00A537ECAAC7A2EE68E79C95CEC3814F53A80B38E88BCE2089A5DA58C87DF88B4BC121EEF79C8633BD8DAB50120959B38D62FDE435E18A36EB26C3D4352CC8B2B2ADAEEC719273197D4168B9A971DF50E23C8D9EE1C859ECC8038AB7B96A95B551E3D70A10C8E593F3CB12F8D22DE0DEDFD5452DCC8E6E8CE27B777636C9879563DD951C213883F15388AD8105C5C15209777BCA7A6E53A2682B3B982696E65D98753265C0A1716F3D76CCA3700BCED00AEF1EC5BEC689162F6E89EE30ACFD0661E6B69C05A6C3B77B6CE3080CF30DE7A50CAE71328AE0379255659BA5F14D137B8ED2A0F539475988AF9D1E84AFF2FC746A29E0EA7AEFA0F4A80367ECCC686E62B4896032D0A8076B7E51F2B903D296AD36917DF76AECA29A9A38B1EFD915BD39CDBC9B9135A3278B4808101B7258D7FD1C41EDCCEF7E0BA0B263DC3D71102D86E59499267D32E75187FB64C2D05DAA82CB9C7DBCB9C727D68F56F7A1572647E0407FD4B0C7A3D69D8B250CA8AA0979B2EB0283D78072A85B5E7A3C1DA7144C7494CB186A37E8FBB67E7C06065C18AD0AF5AFAC8E4896B31251E9637D8B680DD4B840EB75B193A57163459E30B3823B0DA13D12893DC2B34D72162FC6718A4CAEDBE98E45854C6F8CA8F609BEB3F3EA25D0315155746EAA1D186C0920AA14760E0EF277AE032DC677DDB2E542D4B25A243C1CAE1829848CE0BE4AFE9291C3AE11F7998C38DD6393C298B2BDAB24912D4F304581D181CE1338E5756FD6BD31F533D6BD2BAE5B802AA35755C22C3EF873F845C04C5236D71B5CDBBA32E5B919EE607D02176C7EBEE03C8A1B95B9CEE5B79D0C20C5162D32829CF521410DF5C1A29CC34ECC6F30A180D48507098E779F481E985B83AAEC6E0460D8F84A6FF9CB519DB1FA3D550DD8EF533261A49AE0A9E46AACB1A1911221E55EA868E5CBEBEC17AEEB30F9C4B26343E3793393F941C7477089AB96B42C3C6F38A9C4C4A4958B2B120CF4BA81DFB47D0999A2E1C98B7AFF484719F0BF62F89CFE9B224CDDF516C0AE1EB8131378F9701C099E4FFADC81C06014AEF22F50EAA64BFFBD5F9D1BCBE283BE7EBDF316E3146FE960593E285E88E3471E4D3F2C1EA6A7770871A1C5CB27809526300FC8AB0CA4A33394A7AAFC148A413A9BF01D8F9F5B284F54918697F2F7BECD9FBF70F70DCBA0F9964C0F7E26F452EF7CF69E53856388EDC0A6AFE1BB68E19B68213053E57ACC307C90DC20210CFE4095290A42D8554536AF59A2D2945E35CDE963E3C3E11E9BC8E800751AA0790B390287554F1382093C3ADC420DF8FDFE066FDB5EE0F29CD24826BB54E3EBBCB60539C79DC0CEC2391B8B4EF1E422649165D9E28BA35735DBE90B89D0A6868EF7A531BDF4B35919836FBD580A68E4A6ACBD03BB2E03BF2DA79B10E69C53F9E8767B3808406B607701630365AB6CC1F79942BAED91D4266ECD40E94D6E0690F282D74D06AEEFCF2D40E7C955F18541C8CDA8C591A8BDC5E9DF993A886FD72D60DBEA3F9463CE5FDF753AA3C98046744A8F3C9517D59908F4C90BC0BE7816CFE36E01FE1D978327F2BE22B780C4380AA405582EE0F846B595F5A61D8EB89440D6733EBB05D89B1200AF5E848BEC4D57FE9955905BFA51095CEDBE614DED6E72443B5C6A98745B7B9020E656561635DCF2196BDD380307E70D4C8B9C61AFE91FBDE07C19279D32E844384D8648ABF2BB0CB6E7C9FBCABB48A7DC1D996CD8F8233272937130401B47E8B5052CA31FEA307165E0634FF8A6BE5D5EDFC872058BEF9A9C04E89BA975084C8C92BE1B8966FCB27F4CEB7C231755FF76210DC87D6469C43AF442C97DAEC149E16B3F8EE7CE3F17676B0DDAAF280EE7FBF6690D2CA1DC10FAD4FB1B1D69EE542A67C93CD16A822AF5008E7DAAC22330C2F8C9B4B3C00141C19D9EE5B97EC44043C4B95BCF0C577979391F1F5330912CCAC0CB2D059864AA9E23C805627436C54B7AF71F50D4ED861C9BBC72C06ABA2D71C93D8F12997E247ABFA2184E1EDF3C98C2C9CB93FE64FACA3ADE4D62225C7C1238653799FB5CABC2D4AA49DC76C9768D6B62FA6FF8757D50B6B1F08EEAD54242877957364C86D55E3BC2E6FAC6E029B93FC4825EB3B7F791FE8231DAC968896DAFDF577EC2EDB0DD829CF59BB06023E67BA28FE1AE4C2ADD3AA3E202CF9BFBC4479EA1E912DD8ED50C63D8B869399F08B678B0CB3E47B78A7623DDCC409F35C4841B54408E6BA16B25EF5F24EA25F30E28911349E14F16CC364DA7F214E52D56A0F2CA972C548B4D2EC62B4632925DF12944AD120807BE665A1DB97E9592BC933B559FA1494567DD02A2922AD67017EE38F2DBF72C9151FCC5159414D4B7112DF19CABCAD33CAFF15AA8D758BE566C15C43D11ABB81137DF1ABA951299F878AF4FE27DB008B67FD906C6EF9C3FC90EFA6894A83F2BF22468E55369FBCA2F2ED7F64D55B644CD1E9D8587F43D16248D1FA3F8D292D01261E488C7B15C2857F35E691319F91C976AFE383C65D11FB816988ABC933AC0CD2EC65A80A3F26126A87C786E72AB242F0AFCDF4DD362AF32255B5F6B070E5331784C3FA3B2235762A93EBF5FAF3E1FDD2F4941BD875F45D4A8DD4ABB0B324CBEEA2853B33D32C8DCCE61C531E77B8F3B9398B535B394124CB57EAFAD0AF7785E1B673410A95F45333F57E2B67D975E3829E68FF8E6A4803EB21DD9AEEC16F82080C74E81D4F164C6A3BD0C7E96DC0F8546AB8D397BA85B7FE7543318AC0F07B272C8587D7D563C0E6B798E967BDE7D491D5797BF507896E1FB20E5FE6FCA46ED12366AB6356AB6078AF87788F25DBE88A5152A5387ACC6336E2990B7887A55EDC47BB1A4D9BF7D8D043E5C1C782EA9DCF58CB36F4994A2A99859FB51066CB5FEEC7949E5E7BFD0F881BB76649363F77EB4C08C0968B87E18965F3C6F686632406D61053BA7ECE5B7CACF8F1D10A9AC10217A925F560F7FA0F37651EB0C9934CCDC92EAA612B383CFADD4F7A24F7FFEE5DA093DDBE77E6A0B2524C44FA960CE5EE2D3B0B3F4FEDB9DFAC93FA415C5C9D66CA5EFFAD842FA0A08B6FFC618F1FF84217BF96F92A5827F6D493BA83BA937AE5860B139D763B89D824AE7FFAE136F7BE2653101851E25DE70D59AFA9AA9F5F4F11B1A079F4FF727366262EADB1B48BE8E05CFE9F591AFB3725A6BBA061EC62F6D7FB99755D85FB48DFC8CFDACD9AC516A59CB6F9DEA4617CA589DCA18E3DB07E4A4D355623835B905303CCE4013B9CF9C40D4050AD30452F5847FD61F9BB9950E1E482D3B11193C21A8CB24EC6D7A6DBC8312111F7A601703DCFB0303CBBF0CBFB80B307D1FF3E0D9E77671D4FD014AD39AE36B050B33030DCED12C49BD92F9E2F5A3A5AAA21D8D2785FB23812B8E95551EA5D878F329ABC4BFE48785E8A7C8D6E87BA38F4082502A51B5AF299C7FB8E3C6BD091C7E979374DCF04774A1F7452D7D0A1767A52BCE9E6F071B760AAE3927A6A97DF81D2E4724C916B550610FFB53065B4A1C8BD08915CB3B40045C1E542DAA2A5C7E535850186873C36A92A60A632BE92C41D649AA595462F4364F0000C277162AA33779DAE7DA4755C4D73966165D4D719C7AB78057374C7F8B83BFA61F52E6FA9C6447E8F7A1E87F2A8A0B50341B1B72367638CD5546ECFEFB4643A8D1099786693F63996AEB27E0DB32C3F7699F8F62BB8E73DEFCBF1074A871C4A4F93E1B5C0924D7C611B71E6F9388FD050CC0267DD06EFF8FAE570561B780EC084B1C62E816F0CB7E18D864F570263CB2AF47973A025727C3BA81ABFD4B046DBE124302727FBEB5B9923CB694B90EF0B8085CB6F0C2948B9BBCB5431288BCB2B690607BE42983FA17313F634F1AFF1F07772A2D4E4B86E72A9F5C2856DFE9FB2B2B6D9DBF9E60B4167E267A375E28E43880D9BE1B401DBF4A54F1E71056961E34A91C3F93397C9D80E2F3AA26CDE1990BB80A2C4E3E7E38913E9D5749B5969E6A3BA17AE40FB8E8EC4C7767B0FD154FD8EE481BBF8793AFEB6A638FB879A2641423D068565D7E0B28AA78E94AAE5C7DD769D72F6CBBB03516598C0EBCB325DDFAA72B43C3B7D96407948EBAB2D6D5E074D3CA847B51922568AF7F3507BA3730BE48EE4153B4793B9BB7A1844EBB73B483C73B060E6767B55255D49D5E961C33AF4EE37BBB0D006CFB00598DB3CFCBB790BE7C037E95476E60BFD9ABFE198417B0113E76553DD3D53963E4F6179DC79F1ED7C9BB9B50D02B5D290E8C96EDB8181A6B307932B881A40BBB2123D9B01D302AFCC4F03E69B6D39BC38C983811529B86833B4BD8598D402AF5E69EBFC4EE93A1C9A05CFD704AF58491B4297B9B9A6DB30CE9CDABDA4712D5262ABD24C47D9617F22D79D08807E2E4EAE3DA0BA15FFC2302D7A79EDB56C211ADB6EFCF5E9F6BCD2E5DE25AC590FD390A819620E3BDF8909366A91BAEA220F92A1FDED192C7E5D447AF04B806E486A2049723FBA6DF89E6D70CE4D30D846C07129DF2C59AACC4873B09F61409C9B73FFAFCF7664C349D3DADF48937F4D2802FBA4AA53E8C9A2B0E7B4C1A24470FC9D7ABBF036D1E781E2035E179CD5E06FE8EB07EBD0F03FB3ECE7115951C184F38CDC63BB436C52DC006349E146A5212DBF903FEF0EB387F7B0453A405D732D0A6B1D4E14B6A90ACBA27323D36FCE0A958F2B87BF39CE30BB5D25B80C04B612FE225C1D7650F1AD728CD07957DBE6FFB8A3B58DCB0AC01F3F70A877D41CE1D4BE3B2BD5D87269B010FC79D766D3066F347F63428C5892B15D0B4CFB78D8B1A1EEE073F9D1744CAEEB83C6222CE5D352A6EC070CC9B9A60535C528DFDBB433D25509AF6029DF96FAC5F316A04D325277AC5088E215B3DA31A90EBA039C8F9C9D1D15586700A639A58BA01700E6586DB6F9691D362ECFCA28CCCA9F4CE96E973E21C5F4E51ADF30EB11E77E14EB559CA70F10B82A3E21CC0CF6787FF7452D4812055262DD20589F3C381962572B333696B3C0324AAAE820F7A93180FEE729670B4A6472503914DB34B1181BA7F94326C11CF57E7EAE3F150500BFAD9CD4BE68D78413864BEC1658BFF1CC6C31967B6D5F6386B8DE0F7F77B2F4D71858DF72AB35875C6652D6408310467B1B68136A9400848ACDC05B8F0CA8B031833F84C3DD98BC0936E2D86DCBF270AB52388849FFADE300BC0365875E2B6CD5AC580A728C85C13D036B228A3725BE2F305053A7972A5C6E7543F1BBD4D57D0A2AAAAB6F7ED7D83636D25DB8544F5C2C8EC761BD9DF4FA932D1EB339AAB35F8BAF0489731E3629FF0BBBCF527B5DDBB7EEBEACC9EC427AF79B9CD060425962E6F01416B152052DED0C983B21BC4327C6A2147F0E6374D7AE88D0CB2A0BE011B570389112E3067941E987592483D7AC83AA3D2B1A156D1CEEB463EB5DCD7BE8B7B0C77A6C3FEE9A655F7C17030462BDECB2D9CF10E97F3D59A689B749CB2956A301CD713C19CB91973131B71D2F307A4E5354A79995DB92F3967B795CA8DBED4C2B559C4CDDD98C7E670AAE679C7EBFE161282EA61733C7C0B673A4B1CD203CA4BED3D7597ECC75E87EDED0E8139C49CE99B8026EFCE74FDB2B2EEC9F99ED91CF646B5281C62AC300D2DF1DD54FD670E25FBE219C32A1C02BD33E376C383358ACFE247D85D93044D5CD72E17A8F39293D49D6FBC2606F3CCA8348686CB67AF15AAB042CD8E172C26AB107BE687ABACC16696183DB029170CC5A95537B9EFDEFEB09C68084DED601528CA2AB332F91E4CD39843D0FB38751522A473A51D57F9188112F79757F3109BE2AA0D9F32568D16EE6BD303350F1EFD71B94FAA63FA73B60DA3ECFB76289F9837950FEB63D18AE876AE9BC87038E178BBE17BAABD0F941D3C8B9EF490935931C38CA1114D938F1B6161FCD0C1464727A784A1EC4FEA620A6DD1C21183BFAA784B3E46EEDEC9613DFC1371DA28CF8255323E2CC8164089DFAF2759E034983855A072188365A686250D5BB7098FDC02ACE4D50CC05C1FC3D6DEC628B2BEF1CEE9687A120B95C9A2ED32960B47E42F103CDCBACC2A339D1BAE0F11C4CE537F7A866FE501382B3C1C0D835A106CA6343D8F80469311AAD6E32B166FA5F43B3B1371D5DC67009483D5973D1EB7F014829E7A0486A79AC75BB46DCF2E81DC613B979A3EC5170E01D902CB88BB0B9A98E62E4A13EF881E9A2F8FC358B2F0D39D8ADF8F1B3ABC8639B39CB736C245E6FB2A068CF21B3E34DE3BE5DA6869D68461C3610A0F0F3C20BE3D9CFA8E1CB780D109F71C0A879AFAD63F3FE4FB9006143E722D05966C862FCA06B43369F75133933A9003E42866E9F22A9D26C37D62DF0275317F61AB8BDABB61C12E69D64AE4509E0E3FD1C0C84AA73536FDF6180681A5DCA3433DE32C385EBE1BF6B827266369CBC64B4E48B01AFBBC37DD2909742FADC8BBB7F3792A45A9778656C46BD8942027D5BCC5BC16FF5A84169847D75968C0D361C6FC99C547453EB1E473E64748B0AE99C2206EDC617C8E5C3B5DB6AA1E415B359AAE7E793E79D134B2958A966D7B12F507ADD4FB247AA5AEBEA6018A548EF14CDAC25C84CE423FC8E648368AF4157EF8F0752C8E0F826D46835A2CAFA4B6D3216DF1E82C06F40D4FA8E7D37C7579C8A780DFD1CDBB35BDC2FC0D4BC171CFF0D86250B53AAF0713958A71F8088DCB1AFF4ADB6BFB855B00A35FFCF765D814142272AC81F98EA014E793F1FFB2A1FD60A7893B0B477B59D6A2719D21A6AA1925259B48DCCB546A976F554321FE121A56E3A7BCEC1C75B4C34229F1165B7929002409DCF3A9F4D3F6B787B9B04A740769F87414F6456DCF6EC17874CF922496BBC4FD639575170F1C059B673C4E8FE0567C76B883A3E6ADBFECFFFA5CDE65AC2847F53ADFD9D67794563D7F4E9527EB54686F5053463B98D1AC39DB243C49B325B6CF804A796D2406CF59125AE8FF79E58DE8F308670F7AF61424B3896ED3A423A4F8653C1395DA1BAB5229DD552A6A49B63E4540305F5D1E781CB588C4395F0AC99D7A31A20E102BD6E78F6F01FF789EFE678CBBA395FFAA02D1F837730134652D23FB5FBD7B1BB87FDD7D21B55B6ED174A8B4CF161A0FAB8CF6209E3157E5B02FBA60189A4BBC5A3EA45DD8486C7C14392242E4729F3AAB6C531AF58D305EEC16B0EB9025E7C12AB05B25B8FEB5535425F2E7072E5BB36ED18138E9658C99F52D20F6A126065FFAD325B020657C2865B08CEEB7F14565A3DEFA79E6E1737CBEA4E09911FEAD36BE9A29EBA9D520C14E909B8FC11D0D75CA6847B2A3A76623CF548883293EA15E66AE3E1ADF420C07C08E508E5A4FB432370E9E789E8D59A816CD6EF97B8DA37E0F1C9AC515CAC32B776653B21EC73FFE29B012D7E3C3097E6598DC66633793BA61FB09A00DB0C55FF698CA585908407533C83264257C371E31799FCDC988ABD05BD7E61BC826229C241EAF289358DB87FC5DA0AD0270F0BA97C952EBE9E67F22F116D6B32FB780C40CDD5B4095C27596C3E293FF6CE284058C5656A1CD5429CB02F302C903DBDA8ADA3E33DF5C859DA66EFA6DFC7D099C4CAC1508AE2F0BFFEA21F8C493B1E746637D25A54CD4DA690D671D745E7360F9F71ECE835B00689EDA53F4872579C44FB35168BC9B57C0CB7F30B6DE5F8CAD24F963D0E9A33511539AE2B681838BC2B249A0717AD9229166F7B7C419D3376F5EB8DB1C2D85ECC15D7104C63FEFE4363DA44BEDE365FD62EBA5D91C7E80F31217D79F7CA5E994ED1ECBE033F6C0A0F240D833BE6B39400CCFFE2F21279ECDE000C11DB954BCBBB48CD9A77AC8555781F68CEDC2D04C93645A46C64181DF1292A3ABFF57678590FE70E19D160BDB58CAD2DBD069FBCF94DC7B2D3FDF806F4ECD9EB87EEB61250811C6A7FE48D4EDE9419659E56A8B5FDBF32179156837AAC7E5A6409C1EF7CD53DDBF4F1ABE003F6F0E7BA4F382E09F416DCCAC328B3EDB187F9ABCEF184C9FCFFCCE85FBF329203DFE0D22FB16B0671E90E151ECF39C82D8BF304FFE0BE947BDA385EAD86909E46AF2233CCFA8AEC3AAA887DD12A87148D862FA99D7C4743FD99FD416A823DCFB6801E5402A729CF96CFED11A5993F493258B16E9FAB993FF22B37C19D8D8552BECE76588411718241DDE489E459BC51D97F281963C0D2FEA625C25F38EE17A2F60F509601DEB80D6CA4793B19A34E4FC32E71EF3707C168F4DB417FA70565EFDCE0D2AA2EC7EF860BED258BF1D4848976B4FE8AACC26054850ADD93364E1C164558556B91B3A3B3A91C10A65ABC52FC55E92BC4C509702454B57EBD648125057BB66928324C0CD0495BD8ACA3E4B9D88981ADCAB293052E1BE77FCCBDAA65AE994B4327B8C09775BB163E30B303BD3010715572CEC4DA23C8A5550F1EE186FBA43E0CECFCAEC206560926E7DB44E70FB867E4BF69374C9C06F89C4E1CE3F05436EF07987D9C3A84356ABA8158265B94B77FAF3ED19B0D59123670C262F04B914CFA32E8EBA59071A2D53999EF668E105E4A2FF26C28EC1CA16B7162A25A684B912964F558FF15D79469ED78994D0F7689B9F54215277E06B328E7F2E8D6F01E9129B95523803B44ED556FA476FEDA138B5D5E7B35FDBF81C3FAD3925EAC4BC1F78F4FE6ED8FCC857D2FB6FDE2EF8BD0BEED5C078EC2143CA156F014493614EEF9DEBE89FC4DBB98A99D8F0BECD5D88E0C5E23B8E3ECA042F5DC24F8F2562E907E65B1DA73CE62EB726F7362BB5D14FBA786E58EC54F5FC1FBE43E9E86CA490F1F51D73843DA81B5874AFA91CFBFA8CAE86FF19C5A75825F4C919CD25C92DA08302ADBE315599DD67D273E9ED6B84F29911DCF92965B810F472FBC50E18CF74E0ECA69EC9E5F409B5B883CCA986B517AA66D3DD206706F7B13EA35BD70E95CD828496DCCB9B7D63DCBA34933EEEF0330329BC3E722478BAAE9117EC5210CEFE06D9F61C233E6FDC3A8A63776B0759DDFDF5A4654E6ED6F98F558051E785914BC1AAAB1E17F386B096887A25E22F0513603EDDA4EA9C8CE84250A3E081290B5ADB5190A26D89D99E2C87405B9179A38557754A22ACC9635F793BA783C98C200CA099CCABB04740D56F389536BA0974B32F5F39867B08D821A32DE40262E2486A9DB4FC4C7A9CFDE56D9223F7649ABA8AA0B695C398E94D167AA2ACA8CCF1C45FB160A24A33E9D51BBC424E90604E6BCF6F3AD61F2131986CDE9B66C8E31010F550CAC47CCF39E58B578C22D996A1A45ECF2E143F645A5DDED54E5E105C3BEE193E43AC20D6ECC68CE0C668630798B4C99C96F1FE532E8DB099F5A79A8DEF8E9DFAC912C33843FA20BC9AE236C620583301F52036B5E7C83BD0103EF1FE701F02D19683A6BE763F76FC1AA9F7EC7AB7CFED791580E94FCC71AC2FF3FB6DBE0FFC12068F91FBF3A624E1D3E5342FD899359CE615770B5CDC54E92B55AD05735017947B1C6BAB533E0A42C7E6E1B1D464F3078E0E6235EDDC9391612944AF231282CE0D5C4992F65F9C53AB7636E1CCB97F0BD49BF88256D47F57C373DE27AB7151CB1856BA51A8702E168DA0F8D23EBB9D8AF3541C50ECE860A355E1931CC10E563F58E7E71D8F1F84F32ACE1ADAB29D700163C9C4767BD2C14DC4FF3F883CFFBF0B368331E2958D65DE88D7344D02937CD63E4CDFCA32D61C449D540F427E5978A5ABACE9724AF6B1FFE2D24790F79799608BBB3F9B7A07DC2CCEFA503980164D3EC21939F466736DF849A8B99C88852743BA4DA66435CBB6CC39A0B024E558A86F74285463AA656843E5E55C281ED134513B773BDD13A77B1A812BCFBFB7A36D19B8CFB3F2DABA6166B51AEEAF0975A3E99DC855642E83C69B1EA672834D02CA475BA1C7D368A85CB16852EB958F8200C4DF0FDC161A48CA3EE928FB94B9B2B8607E8B56585BF8A8D22A6C904F6D848EAEC03546428A6D8D4538F8CBEC9F163E34B32AB07172B7DE51824CE2B27B955440307D83D640734009EA9A794AF01AD3DE3FEC2E9E65720C9B8ADD3499A904CE1E8FDD681C71FE0FADF4070FFF532FF12B80C83EBAD8FECBFAEECE0F08F44F5A975A6E24107FB558D66078F53F7CBBD9FFE5DBB5FABFF976AD9EEC4D5070F146FEFED65C3388434410DE1098161543168CED851D52E559A672E1D5B50DEAF776542E0CBBD6B10DA37758BE32FD99CBAEF91F8B2014FF912950EFAB3F340E22FA4E4F2418B8FE637AE9F9CDE02DE04E39354147B3A63D5087E1DB7D6B85A3DF9F9C538FDBFF9E357902D0F745F21127021AAE772D3FD0F78EC83B63291671BCDD8ADE95BF4BE274487C12C1FF6EDED59FA2A19D10A2591972A13355DB993200F5B38619975D863CAA6EB40E35ADFE16191A7391C99A8AD902FE9F58F10E09B57B2DDC88FB611FE7DBF1B838D77E9CFD97F2423AAD541BC74E92406ED9EA85977B74A32BC012BC5A952495B1BE324C03EF40275BC824A58594298E7CF284E29E77D1F8607B1BF3647AFB48F2CA209F866462C1D85AA3C6B7556B9197315A67311D475C6E3E80460EE0E8D8699750F728B9C9260B1786FA20D24F329B61AC327DE654C3CC46CA52E56D10D8935D97FE556CDDE0E2C2DB359A59B9A13AEA984E8BED9A0D83685873F13E84C1CBCB7E1098F1584717247F0B405D5C16F85D15E33826C5E289ECA4496F01F7C5F7CCE7B43094938ED64662243E10FEB558D29A20FC334C97691D57E2AF2013F2FB2E5E882969C2881304F1E6591330BAC8D86ED6AC6C37B8A7033F98CE806B4507A04FCF752883E23C74469BF5763B936942EA8B12A94386A3F9200F4D34751737047E82ADF434C1E534AAF1F87C42A49DE90B98D44827313B7E7EE801DB8E31A4777A57B94F80AAADC72B71F28191AE98172F910054707D217122CB12F3230A01ECEE56D171DA16A81F3A47B4F214559ED557EE5BBE7E80455F06406E10897F1659B1D685DE1AAB8B884AD012C9499F151202F6F1B306395B3E0C670FFD2D856F2A16879650605721E45FF9FE148D935A76D46F23C7E674BEF5E3421C2E26B120AE7369C358FC05E9EA35EE351959D7C73AEE5B853ABD18181BB0545DAAC9701025BF057422E83C1094DE03978CFE84CA49ED40DAF374EB9B469A36CDFD2CB926D1E7B315C30D86C951B8E251CB5AB5CCAFAC77353A03A1161DB7009ACD770ED2165DACA4F35B094B64E2FBA986706148A70C976B0AC5D7F147DC8406746C4CA8A16EE3614F8BCF60076D3DF07EAB56AAC0C44160BAA69C3F632BCC5F3F6AA9EE26E4F34A3CB1FD17F736E2A45F408DD251F1F0049B7AEF55B84B6510414B5C242DF76B5202EBF719097C70F1D5F819DA8E0039ECCF45E9B696A353956C9278A11B4A04C5BCDF183EC94DA26B51FF67DCDD1C19971BAA9F00E9D1077F4BB4998CD6942CA760D82AF3932BCF25A5292701F1DCA6EEF56C49221D26B70A30E740739E674C50F6E7AF5FF87218A78BE32B5A9EDE2BCC532F93CB4C4549BBA7E715153E620B53D79D7C6A1B035B1734BA9320DC90496C7CD4EF3EE69797B36EEBF645DDABF5C6F1A6C8B667E18EC5231C2FE671D2BD6F4F228B9B6047AB131FBC13371C7FC542F8F29EE23BE137AD3A298EF0AD677D5ED63F0D355EC8CE38A4D0F17528CF3082588FDB8C09AC0541EA4D52E30C17AA74F23CEADCE31E9B8AF0853202D8FCDF073CF32675305E8CF3735C692FDFF43CF02A9B0E74503C8B95C707B4D4F522DCD572BFA7B800FA468D033BE3A302A8B0CD3F991DDA17D927FC548BF9C39F3E9E705A6DA05403C8B05A324382C927428225370AF9DAAC77AE03B2AD170A07FF5C38DC02C8BDCFBBB4838D5FCE6C222F6DDBE7841E78BB56586B3893CD42C2CA369BBE173C9FCB32578E51D077887C93C4AE6B49D11AFB39DD79B7BC62CECF7ABC9E0FC2DA91DA460D543FF5F933C19EB2A3DBE81EE55334C8C333E86B8DFFF5B8794F84E9CE4F14D44F119B84967BD255BA52858F351EEC0E608BD77604EE0B68031D456649FCD7397578E7468C8D984D793F9144CED7553E92984AED16226D2B2FB4338E6B2CDC621198E43778AB4DB668D1951EDCF3B68E95ADDA8DD621B8DDF5E8796E31E1C0C5EAF82DE0CA0BC7DA6861F56AEA7A26D0C47F786BCE13B9BB76F39882DA7BF5CC1B43D6B248DA1ADDCD501883CC398CDB9A2D6CD62A9CEF219E8726AAC8BF514FD87ED147FF1EAF1D5B7C988E1C5DA6295C59606CFF321AB7BF6F7D65969D516EFFCC9E1696AA79CC3FFCB9C4F4E8A353EC46B3D0A1155EFCCB7AECDCE1FEE528B019863908777274CF303CA8A7CF450A2CF0EFAD998D0D3EA5881CFB1E5C318DCF146C0F4BC51CAFF4941F9E5130F5AEE69077AE3C35E294EEBA84341977F3E15D157F6FEB4E7002E44A11C6E8CE1E4C1C4F602C023AED2F15140671390B8DEB4B34018C882E395B030C756F87C69E86110B4DD009BC29555354B1947F5CCC35D809061992B759CF90794B8FEF86EB92508D95A7DC5EE4C10C0F1D1C8EAC9F6AAA5CAE69AF8678BDB78ED66AAC4E5F10B8F8F1064898E6A514C2BA3F1469E546E0DB7D0BE88AA5178F2706CD0634AECC95573D48C91E5F5C35542B6E861A4643CF5CCB1A1B7BFBD4180D1C68967E91C9E76094DA9BE0DF0257986B304E61BE68B25AF899665B2169AA77944ADCBBF57B4BA1EE564EF2405D7222314933B7FAE216C8F63E0E57B5EEB4BA013A2E9EB348E5BD05281804F4A6DA4B2CCA627A3B397FDA07E06568456B06230554DF344D0A987A077D0473CAD2409886DA5ED51494DE24ADA51E539AE96824F61BFA11D54C09BAF3F91B2CCF3E09D5DC28F4705849BB978EBC14689EED2080FE86F47E1F14EFD27C162524C19DD0B3ABFF5DE41A6F45FB5B3605660159EAB05245BD99CDD1385E4069FF60CB38E6B9B6CD5A3589484F578F5BDCB3E9ABE2DFA8A7FE98F38E6C8AC39E1CB4050336E7E5E1486C9519EF666C89B9AABD35FC7D2E73BA2ADE37FC01977B5D1EF8D6EA82DA196988FACBD9A540AD8C392F0FEA81EF063338B15B8060F3B9A724B3D0CAEB58ED15A1A87A8B90B359C437E81EB37B7973F9E9841562DC4689304936095A61F470F04778D89AC7B1A4455A8D45BB0281650B86B65B4045285C7BD9ECCBF611FA4F27C37E5ED3E7C28224632E114D7BD8C40072FFA800DC7745B9469E114D59EF0F5B45DF4B5914FC0D2511B078D1D836E9B8889DE92FE735AFFFF90EE6EBEB5261F3729AA0B60CEF09576CB1C4317A042CFF14B4787FE2948C567732CDFA66021AADAF5402EA52DEE8309FD2CB02F338A7A5091F2D4BACC232DC928E3770664B6884AAABEA827A650A4EE2EAE046158C0DFCD5541C8A7BBA3A174F8A636F39AC908E4B64C96CA39DA8844E65E8B6C9DBF7FB1FCD470AC35EDD33E2322A20F2FAC913D4CA6242E6DC6541047D6AD1993AABD55CA46EEB7B1D56A2693FD13C3B1063B9F4A8B633D570649CEB177F23EFE7B808B4508CB810C1ECA69704837E51A1B34FF92C0855E5E187445847F9BC7A6935F728DB92D09D91FCD7E7F36E9B7E8CAFDA61650EF74117994967C0BDE055BD868913380265A9DFA6365F2661A3F6351196B42E8823F51B7EC702F1B2F0F3B0884128E7D532185B8CEEDB8FDC02786EAA3C0296B45B355F77CCEE73A54BFE89B5D66B08CB4D54A4C74BFC652D7B5481C74C54C58A14CB79E85D4611CDF0F630AEB8A8126E85019DC35353A2C93A495C89009D327D19F143F715C376C22F1CE00D98A2625B8C6E6FA72808A8A4DC36EE244E24AE374937BAD1C9C583916B0CE531D52A90FC9E144C4C7E7F7CBC8DCDDBC2DC817B4256BDC05EC0D7E632AC04DA16F5CED5BE82BA38DE81792CDC9AA0478FEA9E4F49E6A3D3ADAA6D048942D315B31E9F34451AD762FACE827CAC4F6067B696DFC3D2693F596C104BC8B24CEB7D8F686A68FDE1A4A8117FED7CB27BCA5E9AED91CA2166DFF2F1D983B5E0AEBB28DCAB469455DBE1AB26AF9053FF0F02CEC89B83DDBA992733A97D2922B11BF38F37867EE232C2A91B37C94A030437A57BC30257F80F8B23A1420FB11F3B962BCB7D0A17553D13BB080F8807199381F16E7D036EAFCB01786CC88D52345F2C6DB7B166FDD8D72D0698A363813E87E1FE94B966C5E770D558133A40B92ACEA4A7138C4495E1C42D0271F8F61827ED741D6DF4C1144F5B2B15C27FAE71036317C08321FC56E5DE88E3F32DB0DB438722730C5B9BA1C2FDF87784043EAA15815F1E706974E60142C14A1F1CA6701A15EA3F37BDBCAA0D4DB753294B1ECC18B3831E2746EAE9CC287E14D90BD349F9F8884A6CB390CEAD137C0B9802E3C022103324D8E9A9491E627B266899825C7DF9167077539A6B9C6B62DA48D9992B318653FA27A9DEFBF7B692CBAEE68C572FFC9E639EC4C873FDAD19F1214EE205E379752D13E2327845F45476C53E366185AD09FD78D67A246786AB82EBB2FD53AAFB378979AD99CDC2A52968F99AD74F9BC56D7D1D36511BAE03C4AEAEA9EF3AC472008B0A7B8C42D10A2B040DD063A3D89ED652C5C26A3D9A455D12871F8F6973245E783B51A91DFDCC8DFF787052F5EDD26CB17EA56417C22F787A0F7C3955A0C11716FC2AF6B1AA27B9D5CB02DE4C6AC7194E967C485E316D7C694CD6D361EEDD55BA720BBBBD66ABD7E0EFB634377C4B1B15DEDDA890835DA8C395218EB5647B5C112D79B0CBDDD80C6BFCF10A42097A5537EF8C1FB9A062E2358407B0A56D7158C952D5C5D04FF686DA202BE92B6CEB9A9ED37468CAAAB2F9E431AE11187D557049F65BF90120FB324C3D5443A529E3B8332378A32ADAAE58CAB7792D039F1BD54E44972EC7BAC9019767B0AEBBE23EB9931F32BA10A010D196E61872B5CF9CBC5F69374C8BF1144D132D54B066FCD2AF2426BC6D757C411E1AEA36B52DD256F6415E3EE3A1F594B9BA15D4DFF88F28BE1711CAAF2B8B7F4BDF6C0AF80B5951CB1ABF26929A9DAB9CA11597018132D9A2C57E0A8D53E272429A2D87FB90BC4DD8B7D257E099A54E7C57FEBA8A84CA69DBC1C7539C1F747F2E53F76AC1CEB5B229A269A40BE1E18FBD8D3B4C3420369C9F72A3C53E05D5B125277B3EBB2A660C174D8ADB881A4626D55F5A204C96AAD20A5C12A24072B3FED2C6838E50DC281B36701962D291CD3659EF30FC5CAF7152B6F4664C4422B3C69ED48A40B6360ADCF86B795E7A77044F6A587EA9F312721004E5D334E8A03548D83A07330E13A31D54AF8C45FA6BBE1EBFD12A1B2976A83D6ABCCBFE79E7BB7ECBABC2263305925BC08FFCD09877AFB54DCE76B65B10237FB2B00EDA0C5A41CB3BCC161D0F877DEFE5F29A2432D1F2152252A5681F6BDE0238BE2FD0795C1C3FD3CD77EDCDD351FE2025D5DF7F3AB913EAC391D7C447F048E7ADE1FFDA1F02DEB01DF63442B070ED46137F16120D991F1B1106AA0916BD44EDE0259B0E7A2B4EFC8BAF8ECE0D6AF46A5004EDAC3D8B60637DFD58EEDF7075D83A6A3DA98A30448A1F1BF431AAD07A11547BB7EF181B177FAB37E3EB02A68CDBD5C0E25D5D52BE7D81A765F6F599B87C6567C4A3E36AE0C4DFFBD50F1EFD3FDA7BEFA8A8966DD1BB11449124398392A5691024890415C9028224899243DB646890D082923348934172CE3439832020925337D06424E7DC34CFBDF7DDE79C7DBF7BEE77DF78E38E77C71BA7C7E8BF56ADAA39D7AA9A55B56ACEDFFC0A150F750FA115D2F7F5C8515C91B7171A7ED06046EBCA2373B7D0E9B159FA2B719DE74E2BD37AE8C2C3A36B00E84D4F3FA98D6723BA36F600E5EDA333CCCA8FB3AE57136922419F93135E4443C23C71A25C6FE57CBA02970FE14F48797BB30D17370C674C9BD45FF872D6DAF6AEAD9E979EF755789B9F664DBDE8DD1A7503CAA84C8A4E22BF2F1FCFE8E78A0B0DFBB6E6C23F9387DA4FC27AC9FEF4D449F5B9FB66205F34781B32FA3DCD9BD53AEBA56F7D3280284D3CE98207764CFE3B684C67EB12A8B868F8E26ED615C59926E992B1D2F977E8E50ECD8471C6617D3C26CCC1A712133D623BC710E159A2A40F61F80A2E3646CDD27E69DC072A821329B358AA8437E52B5E66F9325300AB6F5037F33D223D496D7BE533BCDCD37402AC3FC136B62AF5FFBBCF1DFE8FBC2B5F7DCBC4A1A7741CABD177906A8360063CD7C67E8355E268EF75F0F17A0D2927467E96B73C4097616EB9EAE55B3D2280745917073CE7E02A2C9D58F6DCF9CFB8446E16BAF1BDE1E630011CE251BB853A2C2ED7DE1782FCCC1080DFB9E15629F5E317E4B453BC79998C0EB03B69851BB96439730C1FBAD1BDB087FC1A8DAA8E675A85BFA3B20206723DC5E817F09C13C6BE850EEB33FA9E8D8DEBABBE4AF22E0C6E19C153C2EBC1EA3948D9F794BCF972A3294F7EA8E59E1E4BB0A081751AFD1BE1A2F4EA7545EA6687F6C1E9CC5AD4631199BE7313D2CFE44E2B8EDD4B1BE33FF43690A9E82E6279B537C5F1B295A3F3D11977AB0C9ABBCAB8A42CAF0A7DD92E1B0938AAEA703E4E07763589C7A8AB25A1D0591A3F27B639B0FA01038A7608F63DAA962A8BF96DCE49E3EA8A8AF192795DCE409526CF3A7C749CD2B7FD173D3C38F7BEB4B33B8725F9480C1F13966D41CC143F6E2745847A9C202A0FF12C3744C217E";
            //byte[] byteArray = System.Text.Encoding.Default.GetBytes(str);
            //System.IO.File.WriteAllBytes(@"E:\新建文件夹 (2)\aaa.jpg", byteArray);


            //var backpath = para.host + filePathString + "/" + newFileName;
            //foreach (var item in _JointOfficeContext.Member_Info)
            //{
            //    var info = _IPrincipalBase.GetAlldsdd(item.Name);
            //    if (info!=null)
            //    {
            //        item.MemberCode = info.uid;
            //    }
            //}
            //_JointOfficeContext.SaveChanges();
            //var list1 = new List<hhhhhh>();
            //foreach (var item in list)
            //{
            //    hhhhhh hhhhhh = new hhhhhh();
            //    hhhhhh.memberid = item;
            //    list1.Add(hhhhhh);

            //}
            //var paramar = JsonConvert.SerializeObject(list1);


            //foreach (var item in list)
            //{
            //    var membr = new Member();
            //    membr.Id = Guid.NewGuid().ToString();
            //    membr.LoginName = item.call_m.Replace(" ","");
            //    membr.LoginPwd = BusinessHelper.GetMD5("123456");
            //    membr.CreateDate = DateTime.Now;
            //    membr.IsDel = 1;
            //    _JointOfficeContext.Member.Add(membr);

            //    WangPan_Member WangPan_Member = new WangPan_Member();
            //    WangPan_Member.Id = Guid.NewGuid().ToString();
            //    WangPan_Member.MemberId = membr.Id;
            //    WangPan_Member.Name = Guid.NewGuid().ToString("N");
            //    WangPan_Member.CreateDate = DateTime.Now;
            //    _JointOfficeContext.WangPan_Member.Add(WangPan_Member);

            //    WangPan_Menu WangPan_Menu = new WangPan_Menu();
            //    WangPan_Menu.Id = Guid.NewGuid().ToString();
            //    WangPan_Menu.MemberId = membr.Id.ToString();
            //    WangPan_Menu.Name = WangPan_Member.Name;
            //    WangPan_Menu.ParentId = "0";
            //    WangPan_Menu.CreateDate = DateTime.Now;
            //    _JointOfficeContext.WangPan_Menu.Add(WangPan_Menu);

            //    Member_Info Member_Info = new Member_Info();
            //    Member_Info.Id = Guid.NewGuid().ToString();

            //    Member_Info.Name = item.name1;
            //    Member_Info.MemberId = membr.Id.ToString();
            //    Member_Info.Mobile = membr.LoginName;
            //    Member_Info.Gender = 0;
            //    Member_Info.FuBuMen = "";
            //    Member_Info.BuMenFuZeRen = "";
            //    Member_Info.GongZuoJieShao = "";
            //    Member_Info.HuiBaoDuiXiang = "";
            //    Member_Info.JobName = item.name;
            //    Member_Info.Mail = "";
            //    Member_Info.Phone = "";
            //    Member_Info.QQ = "";
            //    Member_Info.WeChat = "";
            //    Member_Info.ZhuBuMen = "";

            //    Member_Info.CreateDate = DateTime.Now;
            //    Member_Info.Picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png";
            //    _JointOfficeContext.Member_Info.Add(Member_Info);


            //var WangPan_Member = _JointOfficeContext.WangPan_Member.ToList();
            //foreach (var item in WangPan_Member)
            //{
            //    CloudBlobContainer container = blobClient.GetContainerReference("spjointoffice");
            //    CloudBlockBlob blockBlob = container.GetBlockBlobReference(item.Name + "/ceshi.txt");
            //    using (var fileStream = System.IO.File.OpenRead(@"D:\ceshi.txt"))
            //    {
            //        blockBlob.UploadFromStreamAsync(fileStream);
            //    }
            //}

            //    _JointOfficeContext.SaveChanges();

            //}











            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            res.showapi_res_code = "508";
            res.showapi_res_error = "不能注册";

            ReturnMessage mes = new ReturnMessage();
            mes.Message = "不能注册";
            mes.Oprationflag = false;
            res.showapi_res_body = mes;
            return res;
            //var member = _JointOfficeContext.Member.Where(t => t.LoginName == para.loginname).FirstOrDefault();
            //if (member != null)
            //{
            //    member.LoginPwd = BusinessHelper.GetMD5(para.loginpwd);
            //    if (member.IsDel == 0)
            //    {
            //        member.IsDel = 1;
            //    }
            //}
            //else
            //{
            //    var membr = new Member();
            //    membr.Id = Guid.NewGuid().ToString();
            //    membr.LoginName = para.loginname;
            //    membr.LoginPwd = BusinessHelper.GetMD5(para.loginpwd);
            //    membr.CreateDate = DateTime.Now;
            //    membr.IsDel = 1;
            //    _JointOfficeContext.Member.Add(membr);

            //    WangPan_Member WangPan_Member = new WangPan_Member();
            //    WangPan_Member.Id = Guid.NewGuid().ToString();
            //    WangPan_Member.MemberId = membr.Id;
            //    WangPan_Member.Name = Guid.NewGuid().ToString("N");
            //    WangPan_Member.CreateDate = DateTime.Now;
            //    _JointOfficeContext.WangPan_Member.Add(WangPan_Member);

            //    WangPan_Menu WangPan_Menu = new WangPan_Menu();
            //    WangPan_Menu.Id = Guid.NewGuid().ToString();
            //    WangPan_Menu.MemberId = membr.Id.ToString();
            //    WangPan_Menu.Name = WangPan_Member.Name;
            //    WangPan_Menu.ParentId = "0";
            //    WangPan_Menu.CreateDate = DateTime.Now;
            //    _JointOfficeContext.WangPan_Menu.Add(WangPan_Menu);

            //    Member_Info Member_Info = new Member_Info();
            //    Member_Info.Id = Guid.NewGuid().ToString();

            //    Member_Info.Name = membr.LoginName.Substring(0, 3) + "****" + membr.LoginName.Substring(7);
            //    Member_Info.MemberId = membr.Id.ToString();
            //    Member_Info.Mobile = membr.LoginName;
            //    Member_Info.Gender = 0;
            //    Member_Info.FuBuMen = "";
            //    Member_Info.BuMenFuZeRen = "";
            //    Member_Info.GongZuoJieShao = "";
            //    Member_Info.HuiBaoDuiXiang = "";
            //    Member_Info.JobName = "";
            //    Member_Info.Mail = "";
            //    Member_Info.Phone = "";
            //    Member_Info.QQ = "";
            //    Member_Info.WeChat = "";
            //    Member_Info.ZhuBuMen = "";

            //    Member_Info.CreateDate = DateTime.Now;
            //    Member_Info.Picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png";
            //    _JointOfficeContext.Member_Info.Add(Member_Info);



            //    CloudBlobContainer container = blobClient.GetContainerReference("jointoffice");
            //    CloudBlockBlob blockBlob = container.GetBlockBlobReference(WangPan_Member.Name + "/ceshi.txt");
            //    //using (var fileStream = System.IO.File.OpenRead(@"D:\ceshi.txt"))
            //    //{
            //    //    blockBlob.UploadFromStreamAsync(fileStream);
            //    //}
            //    using (var fileStream = System.IO.File.OpenRead(Directory.GetCurrentDirectory() + @"\ceshi.txt"))
            //    {
            //        blockBlob.UploadFromStreamAsync(fileStream);
            //    }
            //}
            //_JointOfficeContext.SaveChanges();
            //Message Message = new Message();
            //return Message.SuccessMeaasge("设置成功");
        }
        /// <summary>
        /// 通过原密码修改密码
        /// </summary>
        /// <param name="登录名，旧密码，新密码"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdatePassword(UpdatePasswordPara para)
        {
            var member = _JointOfficeContext.Member.Where(t => t.LoginName == para.loginname).FirstOrDefault();
            var pwd = BusinessHelper.GetMD5(para.oldloginpwd);
            if (pwd != member.LoginPwd)
            {
                throw new BusinessTureException("原密码不正确.");
            }
            else
            {
                member.LoginPwd = BusinessHelper.GetMD5(para.loginpwd);
                _JointOfficeContext.SaveChanges();
                Message Message = new Message();
                return Message.SuccessMeaasge("修改密码成功");
            }
        }
        /// <summary>
        /// 忘记密码修改密码
        /// </summary>
        /// <param name="登录名，登录密码"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge GengGaiMiMa(LoginPara para)
        {
            var member = _JointOfficeContext.Member.Where(t => t.LoginName == para.loginname).FirstOrDefault();
            if (member == null)
            {
                throw new BusinessTureException("此账号不存在.");
            }
            else
            {
                if (member.IsDel == 0)
                {
                    member.IsDel = 1;
                }
                member.LoginPwd = BusinessHelper.GetMD5(para.loginpwd);
                _JointOfficeContext.SaveChanges();
                Message Message = new Message();
                return Message.SuccessMeaasge("更改密码成功");
            }
        }
        /// <summary>
        /// 挂失
        /// </summary>
        /// <param name="登录名，登录密码"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge GuaShi(LoginPara para)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            var member = _JointOfficeContext.Member.Where(t => t.LoginName == para.loginname).FirstOrDefault();
            if (member == null)
            {
                throw new BusinessTureException("无此账号.");
            }
            else
            {
                if (member.LoginPwd.ToLower() != BusinessHelper.GetMD5(para.loginpwd))
                {
                    throw new BusinessTureException("密码错误.");
                }
                else
                {
                    if (member.IsDel == 0)
                    {
                        throw new BusinessTureException("该账号已经挂失.");
                    }
                    else
                    {
                        member.IsDel = 0;
                        _JointOfficeContext.SaveChanges();
                        Message Message = new Message();
                        return Message.SuccessMeaasge("挂失成功");
                    }
                }
            }
        }
        /// <summary>
        /// 更改绑定手机号
        /// </summary>
        /// <param name="原账号，现手机号，验证码"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge GengGaiBangDingShouJiHao(GengGaiBangDingShouJiHaoPara para)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            var member = _JointOfficeContext.Member.Where(t => t.LoginName == para.loginname).FirstOrDefault();
            if (member == null)
            {
                throw new BusinessTureException("无此账号.");
            }
            else
            {
                var newmember = _JointOfficeContext.Member.Where(t => t.LoginName == para.mobile).FirstOrDefault();
                if (newmember != null)
                {
                    throw new BusinessTureException("此手机号已存在账号.");
                }
                var Member_Code = _JointOfficeContext.Member_Code.Where(t => t.Mobile == para.mobile && t.Type == "ZhuCe").FirstOrDefault();
                if (Member_Code == null)
                {
                    throw new BusinessTureException("无此手机的验证码.");
                }
                else
                {
                    if (Member_Code.Code != para.code)
                    {
                        throw new BusinessTureException("验证码错误.");
                    }
                    else
                    {
                        member.LoginName = para.mobile;
                        var Member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == member.Id.ToString()).FirstOrDefault();
                        Member_Info.Mobile = para.mobile;
                        _JointOfficeContext.SaveChanges();
                        Message Message = new Message();
                        return Message.SuccessMeaasge("更改成功");
                    }
                }
            }
        }
        /// <summary>
        /// 登录记录
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge SengModel(SengModelPara para)
        {
            var memberid = _IPrincipalBase.GetMemberId();
            var modelList = _JointOfficeContext.Member_Model.Where(t => t.MemberId == memberid && t.ShiFouKeYong == 1).ToList();
            foreach (var item in modelList)
            {
                item.ShiFouKeYong = 0;
            }
            Member_Model Member_Model = new Member_Model();
            if (para.type == "2")
            {
                var member_token = _JointOfficeContext.Member_Token.Where(t => t.MemberId == memberid && t.Effective == 1).FirstOrDefault();
                if (member_token != null)
                {
                    member_token.Effective = 0;
                }
                Member_Model.ShiFouKeYong = 0;
            }
            else
            {
                Member_Model.ShiFouKeYong = 1;
            }
            Member_Model.Id = Guid.NewGuid().ToString();
            Member_Model.MemberId = memberid;
            Member_Model.IP = para.ip;
            Member_Model.Model = para.model;
            Member_Model.Device = para.device;
            Member_Model.Type = para.type;
            Member_Model.Token = para.token;
            Member_Model.CreateDate = DateTime.Now;
            Member_Model.Cid = para.cid;
            _JointOfficeContext.Member_Model.Add(Member_Model);
            _JointOfficeContext.SaveChanges();
            Message Message = new Message();
            return Message.SuccessMeaasge("成功");
        }
        /// <summary>
        /// 报错日志
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge ErrorLogs(ErrorLogsPara para)
        {
            var memberid = _IPrincipalBase.GetMemberId();
            Logs log = new Logs();
            log.CreateDate = DateTime.Now;
            log.Createperson = memberid;
            log.Origin = para.message;
            log.Exception = para.message;
            //log.Track = ex.StackTrace;
            _JointOfficeContext.Logs.Add(log);
            _JointOfficeContext.SaveChanges();
            Message Message = new Message();
            return Message.SuccessMeaasge("成功");
        }
        /// <summary>
        /// 获取国家代码
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<CountryCodeList> GetCountryCode()
        {
            Showapi_Res_List<CountryCodeList> res = new Showapi_Res_List<CountryCodeList>();
            List<CountryCodeList> list = new List<CountryCodeList>();
            foreach (var item in _JointOfficeContext.CountryCode)
            {
                CountryCodeList CountryCodeList = new CountryCodeList();
                CountryCodeList.code = "+" + item.Code;
                CountryCodeList.name = item.Name;
                list.Add(CountryCodeList);
            }

            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<CountryCodeList>();
            res.showapi_res_body.contentlist = list;
            return res;

        }
        /// <summary>
        /// 搜索国家代码
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<CountryCodeList> GetCountryCodeByName(CountryCodePara para)
        {
            Showapi_Res_List<CountryCodeList> res = new Showapi_Res_List<CountryCodeList>();
            List<CountryCodeList> list = new List<CountryCodeList>();
            var CountryList = _JointOfficeContext.CountryCode.Where(t => t.Name.Contains(para.name) || t.Code.Contains(para.name)).ToList();
            foreach (var item in CountryList)
            {
                CountryCodeList CountryCodeList = new CountryCodeList();
                CountryCodeList.code = "+" + item.Code;
                CountryCodeList.name = item.Name;
                list.Add(CountryCodeList);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<CountryCodeList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 注册   Odoo
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge Registered_Odoo(RegisteredPara para)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            RegisteredPara RegisteredPara = new RegisteredPara();
            RegisteredPara.login = para.login;
            RegisteredPara.phone_code = para.phone_code;
            RegisteredPara.email = para.email;
            RegisteredPara.password = para.password;
            RegisteredPara.confirm_password = para.confirm_password;
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(RegisteredPara);
            Showapi_Res_Meaasge_Registered res_registered = new Showapi_Res_Meaasge_Registered();
            res_registered = UseOdooAPI.PostAsynctMethodLogin<Showapi_Res_Meaasge_Registered>("http://localhost:8088/api/v1.0/website/call/register", str, "JointOffice");
            if (res_registered.showapi_res_code == "200")
            {
                var body = res_registered.showapi_res_body;
                var resultJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultJson>(body.result);
                if (resultJson.register)
                {
                    ReturnMessage mes = new ReturnMessage();
                    mes.Oprationflag = true;
                    mes.Message = "注册成功";
                    mes.token = "";
                    mes.memberid = "";
                    mes.ShiFouGuaShi = false;
                    res.showapi_res_body = mes;
                    res.showapi_res_code = "200";
                }
                return res;
            }
            else
            {
                throw new BusinessTureException("注册失败.");
            }
        }
        /// <summary>
        /// 登录   Odoo
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge Login_Odoo(LoginPara para)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            LoginParaJson LoginParaJson = new LoginParaJson();
            LoginParaJson.name = para.loginname;
            LoginParaJson.pwd = para.loginpwd;
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(LoginParaJson);
            Showapi_Res_Meaasge_Login res_login = new Showapi_Res_Meaasge_Login();
            res_login = UseOdooAPI.PostAsynctMethodLogin<Showapi_Res_Meaasge_Login>("http://localhost:8088/api/v1.0/get_token", str, "JointOffice");
            if (res_login.showapi_res_code == "200")
            {
                var body = res_login.showapi_res_body;
                if (body.success)
                {
                    var userJson = Newtonsoft.Json.JsonConvert.DeserializeObject<UserJson>(body.user);

                    Member_Token_New Member_Token_New = new Member_Token_New();
                    Member_Token_New.Id = Guid.NewGuid().ToString();
                    Member_Token_New.MemberId = userJson.user_id;
                    Member_Token_New.Token = body.token;
                    Member_Token_New.FailDate = userJson.expires;
                    _JointOfficeContext.Member_Token_New.Add(Member_Token_New);
                    _JointOfficeContext.SaveChanges();

                    ReturnMessage mes = new ReturnMessage();
                    mes.Oprationflag = true;
                    mes.Message = "登录成功";
                    mes.token = body.token;
                    mes.memberid = userJson.user_id;
                    mes.ShiFouGuaShi = false;
                    res.showapi_res_body = mes;
                    res.showapi_res_code = "200";
                }
                return res;
            }
            else
            {
                throw new BusinessTureException("无此账号.");
            }
        }
        /// <summary>
        /// 获取验证码   Odoo
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge GetVerification_Odoo(GetVerificationPara para)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            GetVerificationPara GetVerificationPara = new GetVerificationPara();
            if (para.type == "1")
            {
                GetVerificationPara.type = "pwd";
            }
            if (para.type == "2")
            {
                GetVerificationPara.type = "register";
            }
            GetVerificationPara.phone = para.phone;
            var verificationStr = Newtonsoft.Json.JsonConvert.SerializeObject(GetVerificationPara);
            Showapi_Res_Meaasge_Registered res1 = new Showapi_Res_Meaasge_Registered();
            res1 = UseOdooAPI.PostAsynctMethodLogin<Showapi_Res_Meaasge_Registered>("http://localhost:8088/api/v1.0/website/call/getVerification", verificationStr, "JointOffice");
            if (res1.showapi_res_code == "200")
            {
                var body = res1.showapi_res_body;
                var resultJson1 = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultJson1>(body.result);
                var resultVerificationJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultVerificationJson>(resultJson1.alibaba_aliqin_fc_sms_num_send_response);
                var resultVerificationResultJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultVerificationResultJson>(resultVerificationJson.result);
                if (resultVerificationResultJson.success)
                {
                    ReturnMessage mes = new ReturnMessage();
                    mes.Oprationflag = true;
                    mes.Message = "验证码获取成功";
                    mes.token = "";
                    mes.memberid = "";
                    mes.ShiFouGuaShi = false;
                    res.showapi_res_body = mes;
                    res.showapi_res_code = "200";
                }
                return res;
            }
            else
            {
                throw new BusinessTureException("验证码获取失败.");
            }
        }
        /// <summary>
        /// 修改密码   Odoo
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge ModifyPwd_Odoo(ModifyPwdPara para)
        {
            Showapi_Res_Meaasge res = new Showapi_Res_Meaasge();
            ModifyPwdPara ModifyPwdPara = new ModifyPwdPara();
            if (para.type == "1")
            {
                ModifyPwdPara.type = "find";
            }
            if (para.type == "2")
            {
                ModifyPwdPara.type = "update";
            }
            ModifyPwdPara.new_passwd = para.new_passwd;
            ModifyPwdPara.phone = para.phone;
            ModifyPwdPara.phone_code = para.phone_code;
            ModifyPwdPara.old_passwd = para.old_passwd;
            var ModifyPwdStr = Newtonsoft.Json.JsonConvert.SerializeObject(ModifyPwdPara);
            Showapi_Res_Meaasge_Registered res1 = new Showapi_Res_Meaasge_Registered();
            res1 = UseOdooAPI.PostAsynctMethodLogin<Showapi_Res_Meaasge_Registered>("http://localhost:8088/api/v1.0/website/call/change_password", ModifyPwdStr, "JointOffice");
            if (res1.showapi_res_code == "200")
            {
                var body = res1.showapi_res_body;
                var resultJson2 = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultJson2>(body.result);
                if (resultJson2.setPassword)
                {
                    ReturnMessage mes = new ReturnMessage();
                    mes.Oprationflag = true;
                    mes.Message = "修改密码成功";
                    mes.token = "";
                    mes.memberid = "";
                    mes.ShiFouGuaShi = false;
                    res.showapi_res_body = mes;
                    res.showapi_res_code = "200";
                }
                return res;
            }
            else
            {
                throw new BusinessTureException("修改密码失败.");
            }
        }
        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<BanBen> GetBanBen()
        {
            Showapi_Res_Single<BanBen> res = new Showapi_Res_Single<BanBen>();
            //List<BanBen> list = new List<BanBen>();
            var sql = " select value from base_keyvalue where vid='02CA9E8E-5650-40F6-9624-1C34E4CE5CD7'";
            var banben = "";
            using (SqlConnection conText = new SqlConnection(JointOfficeconstr))
            {
                banben = conText.Query<string>(sql, "").FirstOrDefault();
            }
            var info = Newtonsoft.Json.JsonConvert.DeserializeObject<BanBen>(banben);
            res.showapi_res_body = info;
            res.showapi_res_code = "200";
            return res;
        }
    }
}