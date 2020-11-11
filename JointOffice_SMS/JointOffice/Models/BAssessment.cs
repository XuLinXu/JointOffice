using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
namespace JointOffice.Models
{
    public class BAssessment : IAssessment
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        string aliappcode;
        private readonly IPrincipalBase _PrincipalBase;
        public BAssessment(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
            aliappcode = this.config.Value.ConnectionStrings.aliappcode;
        }
        /// <summary>
        /// 获取客户授信列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_List<GetKeHuCreditList> GetKeHuCreditList(GetKeHuCreditListPara para)
        {
            Showapi_Res_List<GetKeHuCreditList> res = new Showapi_Res_List<GetKeHuCreditList>();
            List<GetKeHuCreditList> list = new List<GetKeHuCreditList>();
            if (para.page == 0)
            {
                GetKeHuCreditList GetKeHuCreditList = new GetKeHuCreditList();
                GetKeHuCreditList.kehuId = "11111";
                GetKeHuCreditList.code = "SMD0001";
                GetKeHuCreditList.state = "1";
                GetKeHuCreditList.name = "深圳市腾讯计算机系统有限公司";
                GetKeHuCreditList.info = "青岛 山东 中国";
                GetKeHuCreditList.id = Guid.NewGuid().ToString();
                GetKeHuCreditList.email = "zhangxianzhong@163.com";
                GetKeHuCreditList.url = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
                GetKeHuCreditList.person = "张三";
                GetKeHuCreditList.memberid = "";
                GetKeHuCreditList.money = "";
                GetKeHuCreditList.contact = "马化腾";


                GetKeHuCreditList GetKeHuCreditList1 = new GetKeHuCreditList();
                GetKeHuCreditList1.kehuId ="22222";
                GetKeHuCreditList1.code = "SMD0002";
                GetKeHuCreditList1.state = "2";
                GetKeHuCreditList1.name = "阿里巴巴网络技术有限公司";
                GetKeHuCreditList1.info = "青岛 山东 中国";
                GetKeHuCreditList1.id = Guid.NewGuid().ToString();
                GetKeHuCreditList1.email = "zhangxianzhong@163.com";
                GetKeHuCreditList1.url = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
                GetKeHuCreditList1.person = "张三";
                GetKeHuCreditList1.memberid = Guid.NewGuid().ToString();
                GetKeHuCreditList1.money = "50000";
                GetKeHuCreditList1.contact = "马云";


                GetKeHuCreditList GetKeHuCreditList2 = new GetKeHuCreditList();
                GetKeHuCreditList2.kehuId ="33333";
                GetKeHuCreditList2.code = "SMD0003";
                GetKeHuCreditList2.state = "3";
                GetKeHuCreditList2.name = "北京百度网讯科技有限公司 ";
                GetKeHuCreditList2.info = "青岛 山东 中国";
                GetKeHuCreditList2.id = Guid.NewGuid().ToString();
                GetKeHuCreditList2.email= "zhangxianzhong@163.com";
                GetKeHuCreditList2.url = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
                GetKeHuCreditList2.person = "张三";
                GetKeHuCreditList2.memberid = Guid.NewGuid().ToString();
                GetKeHuCreditList2.money = "50000";
                GetKeHuCreditList2.contact = "李彦宏";


                var memberid = _PrincipalBase.GetMemberId();
                var jobid = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault().JobName;
                if (jobid=="销售总监"|| jobid == "片区经理")
                {
                    GetKeHuCreditList.binDingSale = "0";
                    list.Add(GetKeHuCreditList);

                    GetKeHuCreditList1.binDingSale = "1";
                    list.Add(GetKeHuCreditList1);

                    GetKeHuCreditList2.binDingSale = "1";
                    list.Add(GetKeHuCreditList2);
                }
                else 
                {
                    GetKeHuCreditList.binDingSale = "0";
                    list.Add(GetKeHuCreditList);

                    GetKeHuCreditList1.binDingSale = "0";
                    list.Add(GetKeHuCreditList1);

                    GetKeHuCreditList2.binDingSale = "0";
                    list.Add(GetKeHuCreditList2);
                }
                if (para.state != "0")
                {
                    list = list.Where(t => t.state == para.state).ToList();
                }
            }
            res.showapi_res_body = new Showapi_res_body_list<GetKeHuCreditList>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 获取客户授信列表  Odoo
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<GetKeHuCreditListOdoo> GetKeHuCreditListOdoo(GetInfoOdooAPI para)
        {
            Showapi_Res_List<GetKeHuCreditListOdoo> res = new Showapi_Res_List<Models.GetKeHuCreditListOdoo>();
            GetInfoOdooAPI GetInfoOdooAPI = new GetInfoOdooAPI();
            GetInfoOdooAPI.id = para.id;
            GetInfoOdooAPI.token = para.token;
            GetInfoOdooAPI.mark = para.mark;
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(GetInfoOdooAPI);
            OdooClientCreditInfoAPIResult result = new OdooClientCreditInfoAPIResult();
            result = UseOdooAPI.GetAnyInfoOdoo<OdooClientCreditInfoAPIResult>("http://localhost:8088/info/clientCredit", str);
            if (result.success)
            {
                List<GetKeHuCreditListOdoo> list = new List<GetKeHuCreditListOdoo>();
                var body = result.result;
                if (body != null && body.Count != 0)
                {
                    foreach (var item in body)
                    {
                        GetKeHuCreditListOdoo GetKeHuCreditListOdoo = new GetKeHuCreditListOdoo();
                        GetKeHuCreditListOdoo.clientID = item.clientID;
                        GetKeHuCreditListOdoo.code = item.code;
                        GetKeHuCreditListOdoo.name = item.name;
                        GetKeHuCreditListOdoo.image = item.image;
                        GetKeHuCreditListOdoo.address = item.address;
                        GetKeHuCreditListOdoo.email = item.email;
                        GetKeHuCreditListOdoo.controller = item.controller;
                        GetKeHuCreditListOdoo.credit_limit = item.credit_limit;
                        GetKeHuCreditListOdoo.state = item.state;
                        GetKeHuCreditListOdoo.isBindSales = item.isBindSales;
                        list.Add(GetKeHuCreditListOdoo);
                    }
                    res.showapi_res_code = "200";
                    res.showapi_res_body = new Showapi_res_body_list<GetKeHuCreditListOdoo>();
                    res.showapi_res_body.contentlist = list;
                }
            }
            else
            {
                res.showapi_res_code = "507";
                res.showapi_res_error = "数据异常";
            }
            return res;
        }
        /// <summary>
        /// 获取客户信息
        /// </summary>
        public Showapi_Res_Single<GetCompanyInfo> GetCompanyInfo(GetCompanyInfoPara para)
        {
            Showapi_Res_Single<GetCompanyInfo> res = new Showapi_Res_Single<GetCompanyInfo>();
            GetCompanyInfo GetCompanyInfo = new GetCompanyInfo();
            GetCompanyInfo.kehuId = Guid.NewGuid().ToString();
            GetCompanyInfo.code = "SMD0001";
            GetCompanyInfo.url = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            GetCompanyInfo.address = "山东省青岛市城阳区";
            GetCompanyInfo.telephone = "13866886688";
            GetCompanyInfo.mobile = "88888888";
            GetCompanyInfo.fax = "88888888";
            GetCompanyInfo.website = "www.baidu.com";
            if (para.kehuId=="11111")
            {
                GetCompanyInfo.name = "深圳市腾讯计算机系统有限公司";
            }
            else if (para.kehuId == "22222")
            {
                GetCompanyInfo.name = "阿里巴巴网络技术有限公司";
            }
            else if (para.kehuId == "33333")
            {
                GetCompanyInfo.name = "北京百度网讯科技有限公司";
            }
            GetCompanyInfo.email = "dmkwqmd@163.com";
            GetCompanyInfo.label = "国企";
            List<ContactsInfo> contactsInfolist = new List<ContactsInfo>();
            ContactsInfo ContactsInfo = new ContactsInfo();
            ContactsInfo.realname = "张三";
            ContactsInfo.name = "先生";
            ContactsInfo.jobname = "经理";
            ContactsInfo.email = "111@163.com";
            ContactsInfo.telephone = "13866886688";
            ContactsInfo.mobile = "88888888";
            ContactsInfo.remarks = "备注"; 
            ContactsInfo.picture= "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            contactsInfolist.Add(ContactsInfo);
            ContactsInfo ContactsInfo1 = new ContactsInfo();
            ContactsInfo1.realname = "李四";
            ContactsInfo1.name = "女士";
            ContactsInfo1.jobname = "副经理";
            ContactsInfo1.email = "222@163.com";
            ContactsInfo1.telephone = "13866886688";
            ContactsInfo1.mobile = "88888888";
            ContactsInfo1.remarks = "备注";
            ContactsInfo1.picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            contactsInfolist.Add(ContactsInfo1);
            GetCompanyInfo.contactsInfolist = contactsInfolist;
            res.showapi_res_body = GetCompanyInfo;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 获取客户信息  Odoo
        /// </summary>
        public Showapi_Res_Single<CompanyInfoOdoo> GetCompanyInfoOdoo(GetInfoOdooAPI para)
        {
            Showapi_Res_Single<CompanyInfoOdoo> res = new Showapi_Res_Single<CompanyInfoOdoo>();

            GetInfoOdooAPI GetInfoOdooAPI = new GetInfoOdooAPI();
            GetInfoOdooAPI.id = para.id;
            GetInfoOdooAPI.token = para.token;
            GetInfoOdooAPI.mark = para.mark;
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(GetInfoOdooAPI);
            OdooCompanyInfoAPIResult result = new OdooCompanyInfoAPIResult();
            result = UseOdooAPI.GetAnyInfoOdoo<OdooCompanyInfoAPIResult>("http://localhost:8088/info/company", str);
            if (result.success)
            {
                var body = result.result;
                if (body != null && body.Count != 0)
                {
                    CompanyInfoOdoo CompanyInfoOdoo = new CompanyInfoOdoo();
                    CompanyInfoOdoo.id = body[0].id;
                    CompanyInfoOdoo.name = body[0].name;
                    CompanyInfoOdoo.image = body[0].image;
                    CompanyInfoOdoo.code = body[0].code;
                    CompanyInfoOdoo.contact_address = body[0].contact_address;
                    CompanyInfoOdoo.mobile = body[0].mobile;
                    CompanyInfoOdoo.phone = body[0].phone;
                    CompanyInfoOdoo.fax = body[0].fax;
                    CompanyInfoOdoo.web = body[0].web;
                    CompanyInfoOdoo.email = body[0].email;
                    CompanyInfoOdoo.category_id = body[0].category_id;
                    CompanyInfoOdoo.credit_limit = body[0].credit_limit;
                    CompanyInfoOdoo.client_grade = body[0].client_grade;

                    res.showapi_res_code = "200";
                    res.showapi_res_body = CompanyInfoOdoo;
                }
            }
            else
            {
                res.showapi_res_code = "507";
                res.showapi_res_error = "数据异常";
            }
            return res;
        }
        /// <summary>
        /// 获取公司联系人信息  Odoo
        /// </summary>
        public Showapi_Res_List<CompanyPersonInfoOdoo> GetCompanyPersonInfoOdoo(GetInfoOdooAPI para)
        {
            Showapi_Res_List<CompanyPersonInfoOdoo> res = new Showapi_Res_List<CompanyPersonInfoOdoo>();
            GetInfoOdooAPI GetInfoOdooAPI = new GetInfoOdooAPI();
            GetInfoOdooAPI.id = para.id;
            GetInfoOdooAPI.token = para.token;
            GetInfoOdooAPI.mark = para.mark;
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(GetInfoOdooAPI);
            OdooCompanyPersonInfoAPIResult result = new OdooCompanyPersonInfoAPIResult();
            result = UseOdooAPI.GetAnyInfoOdoo<OdooCompanyPersonInfoAPIResult>("http://localhost:8088/info/companyperson", str);
            if (result.success)
            {
                List<CompanyPersonInfoOdoo> list = new List<CompanyPersonInfoOdoo>();
                var body = result.result;
                if (body != null && body.Count != 0)
                {
                    foreach (var item in body)
                    {
                        CompanyPersonInfoOdoo CompanyPersonInfoOdoo = new CompanyPersonInfoOdoo();
                        CompanyPersonInfoOdoo.id = item.id;
                        CompanyPersonInfoOdoo.name = item.name;
                        CompanyPersonInfoOdoo.image = item.image;
                        CompanyPersonInfoOdoo.appellation = item.appellation;
                        CompanyPersonInfoOdoo.function = item.function;
                        CompanyPersonInfoOdoo.email = item.email;
                        CompanyPersonInfoOdoo.mobile = item.mobile;
                        CompanyPersonInfoOdoo.phone = item.phone;
                        CompanyPersonInfoOdoo.comment = item.comment;
                        list.Add(CompanyPersonInfoOdoo);
                    }
                    res.showapi_res_code = "200";
                    res.showapi_res_body = new Showapi_res_body_list<CompanyPersonInfoOdoo>();
                    res.showapi_res_body.contentlist = list;
                }
            }
            else
            {
                res.showapi_res_code = "507";
                res.showapi_res_error = "数据异常";
            }
            return res;
        }
        /// <summary>
        /// 获取客户金钱信息
        /// </summary>
        public Showapi_Res_Single<GetCompanyMoneyInfo> GetCompanyMoneyInfo(GetCompanyInfoPara para)
        {
            Showapi_Res_Single<GetCompanyMoneyInfo> res = new Showapi_Res_Single<GetCompanyMoneyInfo>();
            GetCompanyMoneyInfo GetCompanyMoneyInfo = new GetCompanyMoneyInfo();
            GetCompanyMoneyInfo.kehuId = Guid.NewGuid().ToString();
            GetCompanyMoneyInfo.name = "阿里巴巴网络技术有限公司";
            GetCompanyMoneyInfo.url = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
            GetCompanyMoneyInfo.orderNum = "10";
            GetCompanyMoneyInfo.money = "800.00";
            GetCompanyMoneyInfo.arrears = "100.00";
            //GetCompanyMoneyInfo.quotaMoney = "100000.00";
            GetCompanyMoneyInfo.grade = "S级";
            GetCompanyMoneyInfo.creditMoney = "10000.00";
            List<KeHuGrade> keHuGrade = new List<KeHuGrade>();
            KeHuGrade KeHuGrade = new KeHuGrade();
            KeHuGrade.id = Guid.NewGuid().ToString();
            KeHuGrade.name = "S级";
            keHuGrade.Add(KeHuGrade);

            KeHuGrade KeHuGrade1 = new KeHuGrade();
            KeHuGrade1.id = Guid.NewGuid().ToString();
            KeHuGrade1.name = "A级";
            keHuGrade.Add(KeHuGrade1);

            KeHuGrade KeHuGrade2 = new KeHuGrade();
            KeHuGrade2.id = Guid.NewGuid().ToString();
            KeHuGrade2.name = "B级";
            keHuGrade.Add(KeHuGrade2);

            KeHuGrade KeHuGrade3 = new KeHuGrade();
            KeHuGrade3.id = Guid.NewGuid().ToString();
            KeHuGrade3.name = "C级";
            keHuGrade.Add(KeHuGrade3);

            GetCompanyMoneyInfo.keHuGrade = keHuGrade;
            res.showapi_res_body = GetCompanyMoneyInfo;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 提交授信 修改授信
        /// </summary>
        public Showapi_Res_Meaasge UpdateCredit(UpdateCreditPara para)
        {
            Message Message = new Message();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 审核授信
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge AuditCredit(AuditCreditPara para)
        {
            Message Message = new Message();
            return Message.SuccessMeaasge("审核成功");
        }
        /// <summary>
        /// 销售人员
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_List<GetSalePersonList> GetSalePersonList(GetSalePersonListPara para)
        {
            Showapi_Res_List<GetSalePersonList> res = new Showapi_Res_List<GetSalePersonList>();
            List<GetSalePersonList> list = new List<GetSalePersonList>();
            if (para.page == 0)
            {
                GetSalePersonList GetSalePersonList = new GetSalePersonList();
                GetSalePersonList.memberid = Guid.NewGuid().ToString();
                GetSalePersonList.name = "张三";
                GetSalePersonList.department = "市场部";
                GetSalePersonList.picture= "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;
                list.Add(GetSalePersonList);

                GetSalePersonList GetSalePersonList1 = new GetSalePersonList();
                GetSalePersonList1.memberid = Guid.NewGuid().ToString();
                GetSalePersonList1.name = "李四";
                GetSalePersonList1.department = "后勤部";
                GetSalePersonList1.picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;

                list.Add(GetSalePersonList1);

                GetSalePersonList GetSalePersonList2 = new GetSalePersonList();
                GetSalePersonList2.memberid = Guid.NewGuid().ToString();
                GetSalePersonList2.name = "王五";
                GetSalePersonList2.department = "销售部";
                GetSalePersonList2.picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;

                list.Add(GetSalePersonList2);

                GetSalePersonList GetSalePersonList3 = new GetSalePersonList();
                GetSalePersonList3.memberid = Guid.NewGuid().ToString();
                GetSalePersonList3.name = "刘六";
                GetSalePersonList3.department = "财务部";
                GetSalePersonList3.picture = "https://ygsrs.blob.core.chinacloudapi.cn/ygs/imagesource/face.png" + SasKey;

                list.Add(GetSalePersonList3);
            }

            res.showapi_res_body = new Showapi_res_body_list<GetSalePersonList>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 绑定销售人员
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge BinDingSalePerson(BinDingSalePerson para)
        {
            Message Message = new Message();
            return Message.SuccessMeaasge("审核成功");
        }
        /// <summary>
        /// 获取应收账款统计信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_Single<GetAccountsReceivableInfo> GetAccountsReceivableInfo(GetAccountsReceivableInfoPara para)
        {
            Showapi_Res_Single<GetAccountsReceivableInfo> res = new Showapi_Res_Single<GetAccountsReceivableInfo>();
            GetAccountsReceivableInfo GetAccountsReceivableInfo = new GetAccountsReceivableInfo();
            List<AreaList> arealist = new List<AreaList>();
            AreaList AreaList = new AreaList();
            AreaList.id = Guid.NewGuid().ToString();
            AreaList.name = "华东区";
            arealist.Add(AreaList);

            AreaList AreaList1 = new AreaList();
            AreaList1.id = Guid.NewGuid().ToString();
            AreaList1.name = "华南区";
            arealist.Add(AreaList1);

            AreaList AreaList2 = new AreaList();
            AreaList2.id = Guid.NewGuid().ToString();
            AreaList2.name = "华北区";
            arealist.Add(AreaList2);

            AreaList AreaList3 = new AreaList();
            AreaList3.id = Guid.NewGuid().ToString();
            AreaList3.name = "华中区";
            arealist.Add(AreaList3);

            List<AccountsReceivableInfo> AccountsReceivableList = new List<AccountsReceivableInfo>();
            AccountsReceivableInfo AccountsReceivableInfo = new AccountsReceivableInfo();
            AccountsReceivableInfo.area = "华东区";
            AccountsReceivableInfo.nopayment = "1000";
            AccountsReceivableInfo.payment = "2000";
            AccountsReceivableInfo.order = "100";
            AccountsReceivableList.Add(AccountsReceivableInfo);

            AccountsReceivableInfo AccountsReceivableInfo1 = new AccountsReceivableInfo();
            AccountsReceivableInfo1.area = "华南区";
            AccountsReceivableInfo1.nopayment = "1100";
            AccountsReceivableInfo1.payment = "2100";
            AccountsReceivableInfo1.order = "110";
            AccountsReceivableList.Add(AccountsReceivableInfo1);

            AccountsReceivableInfo AccountsReceivableInfo2 = new AccountsReceivableInfo();
            AccountsReceivableInfo2.area = "华北区";
            AccountsReceivableInfo2.nopayment = "1200";
            AccountsReceivableInfo2.payment = "2200";
            AccountsReceivableInfo2.order = "120";
            AccountsReceivableList.Add(AccountsReceivableInfo2);

            AccountsReceivableInfo AccountsReceivableInfo3 = new AccountsReceivableInfo();
            AccountsReceivableInfo3.area = "华中区";
            AccountsReceivableInfo3.nopayment = "1300";
            AccountsReceivableInfo3.payment = "2300";
            AccountsReceivableInfo3.order = "130";
            AccountsReceivableList.Add(AccountsReceivableInfo3);

            GetAccountsReceivableInfo.accountsReceivableList = AccountsReceivableList;
            GetAccountsReceivableInfo.arealist = arealist;
            res.showapi_res_body = GetAccountsReceivableInfo;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        ///我的钱包
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_Single<GetMyPurse> GetMyPurse(GetMyPursePara para)
        {
            Showapi_Res_Single<GetMyPurse> res = new Showapi_Res_Single<Models.GetMyPurse>();
            GetMyPurse GetMyPurse = new GetMyPurse();
            GetMyPurse.sumMoney = "200000.00";
            GetMyPurse.payMoney = "150000.00";
            GetMyPurse.nopayMoney = "50000.00";

            List<YingShouBaoBiao> list = new List<YingShouBaoBiao>();
            YingShouBaoBiao YingShouBaoBiao = new YingShouBaoBiao();
            YingShouBaoBiao.month = "9月";
            YingShouBaoBiao.sumMoney = "90000.00";
            YingShouBaoBiao.payMoney = "80000.00";
            YingShouBaoBiao.nopayMoney = "10000.00";
            list.Add(YingShouBaoBiao);

            YingShouBaoBiao YingShouBaoBiao8 = new YingShouBaoBiao();
            YingShouBaoBiao8.month = "8月";
            YingShouBaoBiao8.sumMoney = "80000.00";
            YingShouBaoBiao8.payMoney = "70000.00";
            YingShouBaoBiao8.nopayMoney = "10000.00";
            list.Add(YingShouBaoBiao8);

            YingShouBaoBiao YingShouBaoBiao7 = new YingShouBaoBiao();
            YingShouBaoBiao7.month = "7月";
            YingShouBaoBiao7.sumMoney = "70000.00";
            YingShouBaoBiao7.payMoney = "60000.00";
            YingShouBaoBiao7.nopayMoney = "10000.00";
            list.Add(YingShouBaoBiao7);

            YingShouBaoBiao YingShouBaoBiao6 = new YingShouBaoBiao();
            YingShouBaoBiao6.month = "6月";
            YingShouBaoBiao6.sumMoney = "60000.00";
            YingShouBaoBiao6.payMoney = "50000.00";
            YingShouBaoBiao6.nopayMoney = "10000.00";
            list.Add(YingShouBaoBiao6);

            YingShouBaoBiao YingShouBaoBiao5 = new YingShouBaoBiao();
            YingShouBaoBiao5.month = "5月";
            YingShouBaoBiao5.sumMoney = "50000.00";
            YingShouBaoBiao5.payMoney = "40000.00";
            YingShouBaoBiao5.nopayMoney = "10000.00";
            list.Add(YingShouBaoBiao5);

            YingShouBaoBiao YingShouBaoBiao4 = new YingShouBaoBiao();
            YingShouBaoBiao4.month = "4月";
            YingShouBaoBiao4.sumMoney = "40000.00";
            YingShouBaoBiao4.payMoney = "30000.00";
            YingShouBaoBiao4.nopayMoney = "10000.00";
            list.Add(YingShouBaoBiao4);

            YingShouBaoBiao YingShouBaoBiao3 = new YingShouBaoBiao();
            YingShouBaoBiao3.month = "3月";
            YingShouBaoBiao3.sumMoney = "30000.00";
            YingShouBaoBiao3.payMoney = "20000.00";
            YingShouBaoBiao3.nopayMoney = "10000.00";
            list.Add(YingShouBaoBiao3);

            YingShouBaoBiao YingShouBaoBiao2 = new YingShouBaoBiao();
            YingShouBaoBiao2.month = "2月";
            YingShouBaoBiao2.sumMoney = "20000.00";
            YingShouBaoBiao2.payMoney = "10000.00";
            YingShouBaoBiao2.nopayMoney = "10000.00";
            list.Add(YingShouBaoBiao2);

            YingShouBaoBiao YingShouBaoBiao1 = new YingShouBaoBiao();
            YingShouBaoBiao1.month = "1月";
            YingShouBaoBiao1.sumMoney = "10000.00";
            YingShouBaoBiao1.payMoney = "10000.00";
            YingShouBaoBiao1.nopayMoney = "0.00";
            list.Add(YingShouBaoBiao1);

            GetMyPurse.monthlist = list;
            res.showapi_res_body = GetMyPurse;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        ///我的钱包月份明细
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_Single<GetMyPurse> GetMyPurseByMonth(GetMyPurseByMonthPara para)
        {
            Showapi_Res_Single<GetMyPurse> res = new Showapi_Res_Single<Models.GetMyPurse>();
            GetMyPurse GetMyPurse = new GetMyPurse();
            GetMyPurse.sumMoney = "20000.00";
            GetMyPurse.payMoney = "15000.00";
            GetMyPurse.nopayMoney = "5000.00";

            List<YingShouBaoBiao> list = new List<YingShouBaoBiao>();
            YingShouBaoBiao YingShouBaoBiao = new YingShouBaoBiao();
            YingShouBaoBiao.month = "DDH0001";
            YingShouBaoBiao.sumMoney = "9000.00";
            YingShouBaoBiao.payMoney = "8000.00";
            YingShouBaoBiao.nopayMoney = "1000.00";
            list.Add(YingShouBaoBiao);

            YingShouBaoBiao YingShouBaoBiao8 = new YingShouBaoBiao();
            YingShouBaoBiao8.month = "DDH0002";
            YingShouBaoBiao8.sumMoney = "8000.00";
            YingShouBaoBiao8.payMoney = "7000.00";
            YingShouBaoBiao8.nopayMoney = "1000.00";
            list.Add(YingShouBaoBiao8);

            YingShouBaoBiao YingShouBaoBiao7 = new YingShouBaoBiao();
            YingShouBaoBiao7.month = "DDH0003";
            YingShouBaoBiao7.sumMoney = "7000.00";
            YingShouBaoBiao7.payMoney = "6000.00";
            YingShouBaoBiao7.nopayMoney = "1000.00";
            list.Add(YingShouBaoBiao7);

            YingShouBaoBiao YingShouBaoBiao6 = new YingShouBaoBiao();
            YingShouBaoBiao6.month = "DDH0004";
            YingShouBaoBiao6.sumMoney = "6000.00";
            YingShouBaoBiao6.payMoney = "5000.00";
            YingShouBaoBiao6.nopayMoney = "1000.00";
            list.Add(YingShouBaoBiao6);

            YingShouBaoBiao YingShouBaoBiao5 = new YingShouBaoBiao();
            YingShouBaoBiao5.month = "DDH0005";
            YingShouBaoBiao5.sumMoney = "5000.00";
            YingShouBaoBiao5.payMoney = "4000.00";
            YingShouBaoBiao5.nopayMoney = "1000.00";
            list.Add(YingShouBaoBiao5);

            YingShouBaoBiao YingShouBaoBiao4 = new YingShouBaoBiao();
            YingShouBaoBiao4.month = "DDH0006";
            YingShouBaoBiao4.sumMoney = "4000.00";
            YingShouBaoBiao4.payMoney = "3000.00";
            YingShouBaoBiao4.nopayMoney = "1000.00";
            list.Add(YingShouBaoBiao4);

            YingShouBaoBiao YingShouBaoBiao3 = new YingShouBaoBiao();
            YingShouBaoBiao3.month = "DDH0007";
            YingShouBaoBiao3.sumMoney = "3000.00";
            YingShouBaoBiao3.payMoney = "2000.00";
            YingShouBaoBiao3.nopayMoney = "1000.00";
            list.Add(YingShouBaoBiao3);

            YingShouBaoBiao YingShouBaoBiao2 = new YingShouBaoBiao();
            YingShouBaoBiao2.month = "DDH0008";
            YingShouBaoBiao2.sumMoney = "20000.00";
            YingShouBaoBiao2.payMoney = "10000.00";
            YingShouBaoBiao2.nopayMoney = "10000.00";
            list.Add(YingShouBaoBiao2);

            YingShouBaoBiao YingShouBaoBiao1 = new YingShouBaoBiao();
            YingShouBaoBiao1.month = "DDH0009";
            YingShouBaoBiao1.sumMoney = "1000.00";
            YingShouBaoBiao1.payMoney = "1000.00";
            YingShouBaoBiao1.nopayMoney = "0.00";
            list.Add(YingShouBaoBiao1);

            GetMyPurse.monthlist = list;
            res.showapi_res_body = GetMyPurse;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        ///客户工商信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Showapi_Res_Single<GetGongShangInfo> GetGongShangInfo(GetGongShangInfoPara para)
        {
            Showapi_Res_Single<GetGongShangInfo> res = new Showapi_Res_Single<Models.GetGongShangInfo>();
            var result = BusinessHelper.GetGongShanInfo("深圳市腾讯计算机系统有限公司", aliappcode);
            var objectList = Newtonsoft.Json.JsonConvert.DeserializeObject<GongShang>(result);
            if (objectList.status == "200")
            {
                var Info = objectList.result;
                GetGongShangInfo GetGongShangInfo = new GetGongShangInfo();
                GetGongShangInfo.address = Info.address;
                //GetGongShangInfo.businessStatus = Info.businessStatus;
                GetGongShangInfo.cerValidityPeriod = Info.cerValidityPeriod;
                GetGongShangInfo.companyName = Info.companyName;
                GetGongShangInfo.creditCode = Info.creditCode;
                GetGongShangInfo.faRen = Info.faRen;
                GetGongShangInfo.issueTime = Info.issueTime;
                GetGongShangInfo.regOrgName = Info.regOrgName;
                GetGongShangInfo.regType = Info.regType;
                GetGongShangInfo.regMoney = Info.regMoney;
                GetGongShangInfo.bussinessDes = Info.bussinessDes;
                res.showapi_res_body = GetGongShangInfo;
                res.showapi_res_code = "200";
                return res;
            }
            else
            {
                throw new BusinessTureException(objectList.message);

            }

        }
        /// <summary>
        ///客户工商信息  Odoo
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<CompanyBusInfoOdoo> GetGongShangInfoOdoo(GetInfoOdooAPI para)
        {
            Showapi_Res_Single<CompanyBusInfoOdoo> res = new Showapi_Res_Single<CompanyBusInfoOdoo>();

            GetInfoOdooAPI GetInfoOdooAPI = new GetInfoOdooAPI();
            GetInfoOdooAPI.id = para.id;
            GetInfoOdooAPI.token = para.token;
            GetInfoOdooAPI.mark = para.mark;
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(GetInfoOdooAPI);
            OdooCompanyBusInfoAPIResult result = new OdooCompanyBusInfoAPIResult();
            result = UseOdooAPI.GetAnyInfoOdoo<OdooCompanyBusInfoAPIResult>("http://localhost:8088/info/companybus", str);
            if (result.success)
            {
                var body = result.result;
                if (body != null && body.Count != 0)
                {
                    CompanyBusInfoOdoo CompanyBusInfoOdoo = new CompanyBusInfoOdoo();
                    CompanyBusInfoOdoo.id = body[0].id;
                    CompanyBusInfoOdoo.name = body[0].name;
                    CompanyBusInfoOdoo.credit_code = body[0].credit_code;
                    CompanyBusInfoOdoo.legalperson = body[0].legalperson;
                    CompanyBusInfoOdoo.contact_address = body[0].contact_address;
                    CompanyBusInfoOdoo.operateproject = body[0].operateproject;
                    CompanyBusInfoOdoo.registertype = body[0].registertype;
                    CompanyBusInfoOdoo.registermoney = body[0].registermoney;
                    CompanyBusInfoOdoo.setdate = body[0].setdate;
                    CompanyBusInfoOdoo.registerarea = body[0].registerarea;
                    CompanyBusInfoOdoo.businessdate = body[0].businessdate;

                    res.showapi_res_code = "200";
                    res.showapi_res_body = CompanyBusInfoOdoo;
                }
            }
            else
            {
                res.showapi_res_code = "507";
                res.showapi_res_error = "数据异常";
            }
            return res;
        }
    }
}
