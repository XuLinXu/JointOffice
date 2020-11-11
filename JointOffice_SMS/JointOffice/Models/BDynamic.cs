using Dapper;
using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public class BDynamic : IDynamic
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        private readonly IPrincipalBase _PrincipalBase;
        public BDynamic(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
        }
        /// <summary>
        /// 获取个人动态
        /// </summary>
        /// <param name="页数，总数，类型"></param>
        /// <returns></returns>
        public Personjob GetPersonjobList(Persondynamic para)
        {
            Personjob Personjob = new Personjob();
            Personinfo Personinfo = new Personinfo();
            List<Personinfo> list = new List<Personinfo>();
            Personjob.jobname = "经理";
            Personjob.name = "张三";
            Personjob.suozaibumen = "开发";
            Personjob.picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;

            Personinfo.name = "张三";
            Personinfo.picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            Personinfo.createdate = "2017-07-26";
            Personinfo.commentperson = "李四";
            Personinfo.from = "纷享销客";
            Personinfo.dengji = 5;
            Personinfo.range = "范围";
            Personinfo.state = "点评状态";
            Personinfo.summary = "今日总结";
            Personinfo.plan = "计划";
            Personinfo.experience = "体会";
            Personinfo.commentcontent = "点评人点评";
            Personinfo.commentdate = "2017-07-27";
            Personinfo.zanNum = "20";
            Personinfo.shifouzan = 1;
            Personinfo.commentNum = "回复";
            list.Add(Personinfo);
            Personjob.zhuyaoneirong = list;
            //res.showapi_res_code = "200";
            //res.showapi_res_body.contentlist = list;
            //return res;
            return Personjob;
        }
        
    }
}
