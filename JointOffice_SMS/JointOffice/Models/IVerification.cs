using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public interface IVerification
    {
        /// <summary>
        /// 注册短信验证码
        /// </summary>
        /// <param name="手机号，验证码"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge SendVerificationCode(ZhuCePara para);
        /// <summary>
        /// 登录短信验证码
        /// </summary>
        /// <param name="手机号，验证码"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge SendDengluCode(ZhuCePara para);
        /// <summary>
        /// 发送语音验证码
        /// </summary>
        /// <param name="手机号"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge SendVoiceVerificationCode(string mobile);
        /// <summary>
        /// 账号密码登录
        /// </summary>
        /// <param name="登录名，登录密码"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge Login(LoginPara para);
        /// <summary>
        /// 验证码登录
        /// </summary>
        /// <param name="登录名，验证码"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge YanZhengMaLogin(YanZhengMaPara para);
        /// <summary>
        /// 验证码验证
        /// </summary>
        /// <param name="登录名，验证码"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge YanZhengMa(YanZhengMaPara para);
        /// <summary>
        /// 设置登录密码
        /// </summary>
        /// <param name="登录名，登录密码"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge Register(LoginPara para);
        /// <summary>
        /// 通过原密码修改密码
        /// </summary>
        /// <param name="登录名，旧密码，新密码"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge UpdatePassword(UpdatePasswordPara para);
        /// <summary>
        /// 忘记密码修改密码
        /// </summary>
        /// <param name="登录名，登录密码"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge GengGaiMiMa(LoginPara para);
        /// <summary>
        /// 挂失
        /// </summary>
        /// <param name="登录名，登录密码"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge GuaShi(LoginPara para);
        /// <summary>
        /// 更改绑定手机号
        /// </summary>
        /// <param name="原账号，现手机号，验证码"></param>
        /// <returns></returns>
        Showapi_Res_Meaasge GengGaiBangDingShouJiHao(GengGaiBangDingShouJiHaoPara para);
        /// <summary>
        /// 登陆记录手机机型
        /// </summary>
        Showapi_Res_Meaasge SengModel(SengModelPara para);
        /// <summary>
        /// 报错日志
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Showapi_Res_Meaasge ErrorLogs(ErrorLogsPara para);
        /// <summary>
        /// 获取国家代码
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Showapi_Res_List<CountryCodeList> GetCountryCode();
        /// <summary>
        /// 搜索国家代码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Showapi_Res_List<CountryCodeList> GetCountryCodeByName(CountryCodePara para);


        /// <summary>
        /// 注册   Odoo
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge Registered_Odoo(RegisteredPara para);
        /// <summary>
        /// 登录   Odoo
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge Login_Odoo(LoginPara para);
        /// <summary>
        /// 获取验证码   Odoo
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge GetVerification_Odoo(GetVerificationPara para);
        /// <summary>
        /// 修改密码   Odoo
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge ModifyPwd_Odoo(ModifyPwdPara para);
        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<BanBen> GetBanBen();
    }
    public class Showapi_Res_List<T> where T : class
    {
        public string showapi_res_code { get; set; } = "0";
        public string showapi_res_error { get; set; } = "";
        public Showapi_res_body_list<T> showapi_res_body { get; set; }
    }
    public class Showapi_res_body_list<T> where T : class
    {
        public int allPages { get; set; } = 0;
        public int currentPage { get; set; } = 0;
        public int allNum { get; set; } = 0;
        public int unread { get; set; } = 0; // 邮件未读数量
        public int maxResult { get; set; } = 0;
        public List<T> contentlist { get; set; } = new List<T>();
    }
    public class Showapi_Res_Single<T> where T : class, new()
    {
        public string showapi_res_code { get; set; } = "0";
        public string showapi_res_error { get; set; } = "";
        public T showapi_res_body { get; set; } = new T();
    }
    public class Showapi_Res_Meaasge
    {
        public string showapi_res_code { get; set; }
        public string showapi_res_error { get; set; }
        public ReturnMessage showapi_res_body { get; set; }
    }
    public class ReturnMessage
    {
        public bool Oprationflag { get; set; }
        public string Message { get; set; }
        public bool ShiFouGuaShi { get; set; }
        public string token { get; set; }
        public string memberid { get; set; }
        public string ryToken { get; set; }
        public string smsToken { get; set; }
        public string mail { get; set; }
    }
    public class YanZhengMaPara
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string loginname { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string code { get; set; }
    }
    public class LoginPara
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string loginname { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string loginpwd { get; set; }
    }
    public class ZhuCePara
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string DiQuCode { get; set; }
    }
    public class GengGaiBangDingShouJiHaoPara
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string loginname { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string code { get; set; }
    }
    public class UpdatePasswordPara
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string loginname { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        public string oldloginpwd { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string loginpwd { get; set; }
    }
    //public class LoginMessage
    //{
    //    /// <summary>
    //    /// 是否成功
    //    /// </summary>
    //    public bool ShiFouChengGong { get; set; }
    //    /// <summary>
    //    /// 是否挂失
    //    /// </summary>
    //    public bool ShiFouGuaShi { get; set; }
    //    /// <summary>
    //    /// 用户Id的Token值
    //    /// </summary>
    //    public string token { get; set; }
    //    /// <summary>
    //    /// 错误信息
    //    /// </summary>
    //    public string Message { get; set; }
    //}
    //public class blobPara
    //{
    //    public string name { get; set; }
    //    public string dizhi { get; set; }
    //    public string wenJianJiaId { get; set; }
    //}
    public class XinJianWenJianJiaPara
    {
        /// <summary>
        /// 文件夹名
        /// </summary>
        public string wenJianJiaName { get; set; }
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
    }
    //public class XinJianGongXiangWenJianJiaPara
    //{
    //    /// <summary>
    //    /// 文件夹名
    //    /// </summary>
    //    public string wenJianJiaName { get; set; }
    //    /// <summary>
    //    /// 文件夹ID
    //    /// </summary>
    //    public string wenJianJiaId { get; set; }
    //    /// <summary>
    //    /// 分组ID
    //    /// </summary>
    //    public string teamid { get; set; }
    //    /// <summary>
    //    /// 类型
    //    /// </summary>
    //    public int type { get; set; }
    //}
    //public class XinJianQiYeWenJianJiaPara
    //{
    //    /// <summary>
    //    /// 文件夹名
    //    /// </summary>
    //    public string wenJianJiaName { get; set; }
    //    /// <summary>
    //    /// 文件夹ID
    //    /// </summary>
    //    public string wenJianJiaId { get; set; }
    //}
    public class DeleteWenJianJiaPara
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
    }
    public class ChaKanGongXiangQuanXianPara
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
    }
    public class GongXiangQuanXianRenYuanPara
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int type { get; set; }
    }
    public class GongXiangQuanXian
    {
        /// <summary>
        /// 管理
        /// </summary>
        public int guanli { get; set; }
        /// <summary>
        /// 查看
        /// </summary>
        public int chakan { get; set; }
        /// <summary>
        /// 上传
        /// </summary>
        public int shangchuan { get; set; }
    }
    public class UpdateGongXiangQuanXianPara
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 删除还是添加
        /// </summary>
        public int shiFouDelete { get; set; }
        /// <summary>
        /// 个人IDList
        /// </summary>
        public List<string> memberidlist { get; set; }
    }
    public class People
    {
        /// <summary>
        /// 个人ID
        /// </summary>
        public string memberid { get; set; }
    }
    public class WoDeWangPanList
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// 企业文件List
        /// </summary>
        public List<QiYeWenJian> qiyelist { get; set; }
    }
    public class QiYeWenJian
    {
        /// <summary>
        /// 企业文件Id
        /// </summary>
        public string wenJianJiaId { get; set; }
        /// <summary>
        /// 企业文件名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 企业文件大小
        /// </summary>
        public string length { get; set; }
    }
    public class RenameWenJianJiaPara
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
        /// <summary>
        /// 文件夹名
        /// </summary>
        public string wenJianJiaName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int type { get; set; }
    }
    public class MoveWenJianJiaPara
    {
        /// <summary>
        /// 之前文件夹
        /// </summary>
        public string oldwenJianJiaId { get; set; }
        /// <summary>
        /// 现在文件夹
        /// </summary>
        public string wenJianJiaId { get; set; }
        /// <summary>
        /// 文件或文件夹List
        /// </summary>
        public List<MovePara> MovePara { get; set; }
    }
    public class MovePara
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
        /// <summary>
        /// 文件夹名
        /// </summary>
        public string wenJianJiaName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int type { get; set; }
    }
    public class DeletePara
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int type { get; set; }
    }
    public class ListPara
    {
        public string wenJianJiaId { get; set; }
        public string name { get; set; }
        public string person { get; set; }
        public string length { get; set; }
        public string url { get; set; }
        public string date { get; set; }
        public string blobtype { get; set; }
        public int type { get; set; }
        public int qxtype { get; set; }
        public string display { get; set; }
        public string uid { get; set; }
    }
    public class filelist
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string label { get; set; }
        ///// <summary>
        ///// 子类
        ///// </summary>
        public List<filelist> children { get; set; }
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
    }
    public class filepara
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string filename { get; set; }
        ///// <summary>
        ///// 所属分类名称
        ///// </summary>
        public string name { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long length { get; set; }
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
    }
    public class fileinfo
    {
        public string url { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long length { get; set; }
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
    }
    public class SengModelPara
    {
        /// <summary>
        /// 机型
        /// </summary>
        public string model { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string ip { get; set; }
        public string device { get; set; }
        public string cid { get; set; }
        public string type { get; set; }
        public string token { get; set; }
    }
    public class ErrorLogsPara
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 所属组织
        /// </summary>
        public string origin { get; set; }
    }
    public class CountryCodeList
    {
        /// <summary>
        /// 国家名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 国家代码
        /// </summary>
        public string code { get; set; }
    }
    public class CountryCodePara
    {
        /// <summary>
        /// 国家名称
        /// </summary>
        public string name { get; set; }
    }

    /// <summary>
    /// 注册  入参  Odoo
    /// </summary>
    public class RegisteredPara
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string login { get; set; }
        /// <summary>
        /// 手机验证码
        /// </summary>
        public string phone_code { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string confirm_password { get; set; }
    }
    /// <summary>
    /// Odoo注册返回类
    /// </summary>
    public class Showapi_Res_Meaasge_Registered
    {
        public string showapi_res_code { get; set; }
        public string showapi_res_error { get; set; }
        public ReturnBody_Registered showapi_res_body { get; set; }
    }
    /// <summary>
    /// Odoo注册返回类中body的内容
    /// </summary>
    public class ReturnBody_Registered
    {
        public string message { get; set; }
        public string result { get; set; }
        public string success { get; set; }
    }
    /// <summary>
    /// 解析ReturnBody_Registered中result字段
    /// </summary>
    public class ResultJson
    {
        public bool register { get; set; }
    }
    /// <summary>
    /// 登录打包入参  Odoo
    /// </summary>
    public class LoginParaJson
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string pwd { get; set; }
        ///// <summary>
        ///// 数据库
        ///// </summary>
        //public string data { get; set; }
    }
    /// <summary>
    /// Odoo登录返回类
    /// </summary>
    public class Showapi_Res_Meaasge_Login
    {
        public string showapi_res_code { get; set; }
        public string showapi_res_error { get; set; }
        public ReturnBody_Login showapi_res_body { get; set; }
    }
    /// <summary>
    /// Odoo登录返回类中body的内容
    /// </summary>
    public class ReturnBody_Login
    {
        public string message { get; set; }
        public bool success { get; set; }
        public string token { get; set; }
        public string user { get; set; }
    }
    /// <summary>
    /// 解析ReturnBody_Login中user字段
    /// </summary>
    public class UserJson
    {
        /// <summary>
        /// token失效日期
        /// </summary>
        public string expires { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string user_id { get; set; }
    }
    /// <summary>
    /// 忘记密码and修改密码  入参  Odoo
    /// </summary>
    public class ModifyPwdPara
    {
        /// <summary>
        /// 1忘记密码  2修改密码
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string new_passwd { get; set; }
        /// <summary>
        /// 手机号(忘记密码必填)
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 验证码(忘记密码必填)
        /// </summary>
        public string phone_code { get; set; }
        /// <summary>
        /// 旧密码(修改密码必填)
        /// </summary>
        public string old_passwd { get; set; }
    }
    /// <summary>
    /// 获取验证码  入参  Odoo
    /// </summary>
    public class GetVerificationPara
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 1忘记密码找回密码 2注册
        /// </summary>
        public string type { get; set; }
    }
    /// <summary>
    /// 解析获取验证码的result字段
    /// </summary>
    public class ResultJson1
    {
        public string alibaba_aliqin_fc_sms_num_send_response { get; set; }
    }
    /// <summary>
    /// 解析ResultJson1的alibaba_aliqin_fc_sms_num_send_response字段
    /// </summary>
    public class ResultVerificationJson
    {
        public string result { get; set; }
        public string request_id { get; set; }
    }
    /// <summary>
    /// 解析ResultVerificationJson的result字段
    /// </summary>
    public class ResultVerificationResultJson
    {
        public string msg { get; set; }
        public string model { get; set; }
        public bool success { get; set; }
        public string err_code { get; set; }
    }
    /// <summary>
    /// 解析修改密码的result字段
    /// </summary>
    public class ResultJson2
    {
        public bool setPassword { get; set; }
    }
    public class SaveGongXiangQuanXianPara
    {
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string wenJianJiaId { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 个人IDList
        /// </summary>
        public List<GongXiangQuanXianpeople> memberidlist { get; set; }
    }
    public class GongXiangQuanXianpeople
    {
        /// <summary>
        /// 个人ID
        /// </summary>
        public string membrid { get; set; }
    }

    public class AttachmentUpload
    {
        public string data { get; set; }
        public string key { get; set; }
        public string path { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public AttachmentFiles files {get;set;}
    }

    public class AttachmentFiles
    {
        public string newfilename { get; set; }
        public string filename { get; set; }
        public string file { get; set; }
 
    }
    public class BanBen
    {
        public string android { get; set; }
        public string ios { get; set; }
        public bool androidGengXin { get; set; }
        public bool iosGengXin { get; set; }
        public string url { get; set; }
        public string android_lowest_version { get; set; }
        public string ios_lowest_version { get; set; }

    }
    public class hhhhhh
    {
        public string memberid { get; set; }
    }
}
