using Dapper;
using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Threading;

namespace JointOffice.Models
{
    public class BContacts : IContacts
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        CloudBlobClient blobClient = null;
        private readonly IPrincipalBase _PrincipalBase;
        public BContacts(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.config.Value.ConnectionStrings.StorageConnectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
        }
        /// <summary>
        /// 获取人员资料
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<Memberinfo> GetMemberinfo(MemberID para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<Memberinfo>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<Memberinfo> res = new Showapi_Res_Single<Memberinfo>();
            Memberinfo zhuBiao = new Memberinfo();
            string sql = @"select a.*,isnull(b.name,'') huiBaoDuiXiangName,isnull(c.name,'') zhubumenName,isnull(e.Name,'') bumenfuzerenName 
						   from Member_Info a 
                           left join Member_Info b on a.HuiBaoDuiXiang=b.MemberId
                           left join Member_Company c on a.ZhuBuMen=c.Id
                           left join Member_Info e on a.BuMenFuZeRen=e.MemberId
                           where 1=1  ";
            sql = sql + " and a.memberid =@memberid ";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                zhuBiao = conText.Query<Memberinfo>(sql, new { memberid = para.memberid }).FirstOrDefault();
            }
            zhuBiao.picture = zhuBiao.picture + SasKey;

            var isStar = _JointOfficeContext.Contacts_Star.Where(t => t.MemberId == memberid && t.OtherMemberId == para.memberid && t.IsStar == 1).FirstOrDefault();
            if (isStar != null)
            {
                zhuBiao.isStar = true;
            }
            else
            {
                zhuBiao.isStar = false;
            }

            var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == para.memberid).FirstOrDefault();
            zhuBiao.jobName = "";
            var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == memInfo.JobID).FirstOrDefault();
            if (memJob != null)
            {
                zhuBiao.jobName = memJob.Name;
            }

            List<MemberCompanyInfo> list = new List<MemberCompanyInfo>();
            var memberCompanyInfo = _JointOfficeContext.Member_Info_Company.Where(t => t.MemberId == para.memberid).OrderBy(t => t.Num).ToList();
            foreach (var item in memberCompanyInfo)
            {
                MemberCompanyInfo MemberCompanyInfo = new MemberCompanyInfo();
                MemberCompanyInfo.id = item.Id;
                var com = _JointOfficeContext.Member_Company.Where(t => t.Id == item.CompanyId).FirstOrDefault();
                if (com != null)
                {
                    MemberCompanyInfo.company = com.Name;
                    MemberCompanyInfo.companyId = com.Id;
                }
                else
                {
                    MemberCompanyInfo.company = "";
                    MemberCompanyInfo.companyId = "";
                }
                var job = _JointOfficeContext.Member_Job.Where(t => t.Id == item.JobId).FirstOrDefault();
                if (job != null)
                {
                    MemberCompanyInfo.job = job.Name;
                }
                else
                {
                    MemberCompanyInfo.job = "";
                }
                var dept = _JointOfficeContext.Member_Company.Where(t => t.Id == item.DeptId).FirstOrDefault();
                if (dept != null)
                {
                    MemberCompanyInfo.dept = dept.Name;
                }
                else
                {
                    MemberCompanyInfo.dept = "";
                }
                var deptMember = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.DeptMemberId).FirstOrDefault();
                if (deptMember != null)
                {
                    MemberCompanyInfo.deptMember = deptMember.Name;
                }
                else
                {
                    MemberCompanyInfo.deptMember = "";
                }
                MemberCompanyInfo.huiBaoDuiXiangID = item.HuiBaoDuiXiangId;
                if (!string.IsNullOrEmpty(item.HuiBaoDuiXiangId))
                {
                    var huibao = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.HuiBaoDuiXiangId).FirstOrDefault();
                    MemberCompanyInfo.huiBaoDuiXiang = huibao.Name;
                }
                else
                {
                    MemberCompanyInfo.huiBaoDuiXiang = "";
                }
                if (!string.IsNullOrEmpty(item.GongZuoJieShao))
                {
                    MemberCompanyInfo.gongZuoJieShao = item.GongZuoJieShao;
                }
                else
                {
                    MemberCompanyInfo.gongZuoJieShao = "";
                }
                list.Add(MemberCompanyInfo);
            }
            zhuBiao.memberCompanyInfo = list;
            res.showapi_res_body = zhuBiao;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<LianXiRenInfo> GetLianXiRenList(GetLianXiRenListInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<LianXiRenInfo>();
                return Return.Return();
            }
            Showapi_Res_List<LianXiRenInfo> res = new Showapi_Res_List<LianXiRenInfo>();
            List<LianXiRenInfo> list = new List<LianXiRenInfo>();
            List<Member_Info_Company> memcom = new List<Member_Info_Company>();
            if (string.IsNullOrEmpty(para.companyId))
            {
                var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                if (memInfo != null && !string.IsNullOrEmpty(memInfo.CompanyIDS))
                {
                    memcom = _JointOfficeContext.Member_Info_Company.Where(t => memInfo.CompanyIDS.Contains(t.CompanyId)).ToList();
                }
            }
            else
            {
                memcom = _JointOfficeContext.Member_Info_Company.Where(t => t.CompanyId == para.companyId).ToList();
            }
            var memStr = "";
            foreach (var item in memcom)
            {
                if (!memStr.Contains(item.MemberId))
                {
                    memStr += "'" + item.MemberId + "',";
                }
            }
            memStr = memStr.Remove(memStr.LastIndexOf(","));

            string sql = @"SELECT a.memberid,a.name,isnull(a.jobName,'') jobName,a.mobile,a.picture,isnull(b.name,'') zhubumenName,isnull(c.isStar,'') isStar,isnull(a.zhubumen,'') zhubumen,isnull(e.Name,'') bumenfuzerenName,
                                    a.gender,isnull(a.mail,'') mail,isnull(a.weChat,'') weChat,isnull(a.qq,'') qq,isnull(a.gongZuoJieShao,'') gongZuoJieShao,
                                    isnull(a.bumenfuzeren,'') bumenfuzeren,isnull(a.phone,'') phone,isnull(a.huiBaoDuiXiang,'') huiBaoDuiXiang,isnull(d.name,'') huiBaoDuiXiangName,
                                    a.jobid,a.membercode
                                from Member_Info a 
                                left join Member_Info d on a.HuiBaoDuiXiang=d.MemberId
                                left join Member_Company b on a.ZhuBuMen=b.Id
                                left join Member_Info e on a.BuMenFuZeRen=e.MemberId
                                inner join Member m on m.Id=a.MemberId and m.IsUse=1 and m.IsDel=1
                                left join Contacts_Star c on a.MemberId=c.OtherMemberId and c.IsStar=1  and  c.MemberId='" + memberid + "'  where a.zhubumen != '' and a.MemberId in (" + memStr + ")";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                list = conText.Query<LianXiRenInfo>(sql).ToList();
            }
            foreach (var one in list)
            {
                one.picture = one.picture + SasKey;
                one.jobName = "";
                var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == one.jobid).FirstOrDefault();
                if (memJob != null)
                {
                    one.jobName = memJob.Name;
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<LianXiRenInfo>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 按组织架构查看联系人列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<ContactsList> GetLianXiRenListByBuMen(UpdateMember_info para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<ContactsList>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<ContactsList> res = new Showapi_Res_Single<ContactsList>();
            List<LianXiRenInfo> lianXiRenList = new List<LianXiRenInfo>();
            List<BuMenInfo> buMenList = new List<BuMenInfo>();
            ContactsList ContactsList = new ContactsList();
            if (string.IsNullOrEmpty(para.id))
            {
                ContactsList.nowCompanyName = "全公司";
                var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                if (memInfo != null && !string.IsNullOrEmpty(memInfo.CompanyIDS))
                {
                    var memComList = memInfo.CompanyIDS.Split(",");
                    foreach (var item1 in memComList)
                    {
                        //string sql = @"SELECT a.memberid,a.name,a.jobName,a.mobile,a.picture,b.name zhubumenName,a.zhubumen,c.isStar,e.Name bumenfuzerenName,
                        //                a.gender,a.mail,a.weChat,a.qq,a.gongZuoJieShao,a.bumenfuzeren,a.phone,a.huiBaoDuiXiang,d.name huiBaoDuiXiangName,
                        //                a.jobid,a.membercode
                        //                from Member_Info a 
                        //                left join Member_Info e on a.BuMenFuZeRen=e.MemberId
                        //                left join Member_Info d on a.HuiBaoDuiXiang=d.MemberId
                        //                inner join Member_Info_Company b1 on b1.MemberId=a.MemberId and b1.DeptId='" + item1 + @"'
                        //                inner join Member_Company b on b.Id=b1.DeptId
                        //                inner join Member m on m.Id=a.MemberId and m.IsUse=1 and m.IsDel=1
                        //                left join Contacts_Star c on a.MemberId=c.OtherMemberId and c.IsStar=1 and  c.MemberId='" + memberid + @"'  
                        //                where a.zhubumen != '' ";
                        //using (SqlConnection conText = new SqlConnection(constr))
                        //{
                        //    lianXiRenList.AddRange(conText.Query<LianXiRenInfo>(sql).ToList());
                        //    //lianXiRenList = conText.Query<LianXiRenInfo>(sql).ToList();
                        //}

                        //var list = _JointOfficeContext.Member_Company.Where(t => t.ParentId == item1).ToList();
                        //if (list.Count != 0)
                        //{
                        //    foreach (var item in list)
                        //    {
                        //        BuMenInfo BuMenInfo = new BuMenInfo();
                        //        //var list2 = _JointOfficeContext.Member_Info.Where(t => t.ZhuBuMen == item.Id || t.FuBuMen.Contains(item.Id)).ToList();
                        //        var list2 = _JointOfficeContext.Member_Info_Company.Where(t => t.DeptId == item.Id).ToList();
                        //        BuMenInfo.id = item.Id;
                        //        BuMenInfo.name = item.Name;
                        //        int number = list2.Count();
                        //        number += GetLianXiRenListByBuMenNumDiGui(item.Id);
                        //        BuMenInfo.count = number;
                        //        buMenList.Add(BuMenInfo);
                        //    }
                        //}

                        var com = _JointOfficeContext.Member_Company.Where(t => t.Id == item1).FirstOrDefault();
                        if (com != null)
                        {
                            BuMenInfo BuMenInfo = new BuMenInfo();
                            var list2 = _JointOfficeContext.Member_Info_Company.Where(t => t.DeptId == item1).ToList();
                            BuMenInfo.id = com.Id;
                            BuMenInfo.name = com.Name;
                            int number = list2.Count();
                            number += GetLianXiRenListByBuMenNumDiGui(com.Id);
                            BuMenInfo.count = number;
                            buMenList.Add(BuMenInfo);
                        }
                    }
                }
            }
            else
            {
                ContactsList.nowCompanyName = "";
                var nowCom = _JointOfficeContext.Member_Company.Where(t => t.Id == para.id).FirstOrDefault();
                if (nowCom != null)
                {
                    ContactsList.nowCompanyName = nowCom.Name;
                }

                string sql = @"SELECT a.memberid,a.name,a.jobName,a.mobile,a.picture,b.name zhubumenName,a.zhubumen,c.isStar,e.Name bumenfuzerenName,
                                a.gender,a.mail,a.weChat,a.qq,a.gongZuoJieShao,a.bumenfuzeren,a.phone,a.huiBaoDuiXiang,d.name huiBaoDuiXiangName,
                                a.jobid,a.membercode
                                from Member_Info a 
                                left join Member_Info e on a.BuMenFuZeRen=e.MemberId
                                left join Member_Info d on a.HuiBaoDuiXiang=d.MemberId
                                inner join Member_Info_Company b1 on b1.MemberId=a.MemberId and b1.DeptId='" + para.id + @"'
                                inner join Member_Company b on b.Id=b1.DeptId
                                inner join Member m on m.Id=a.MemberId and m.IsUse=1 and m.IsDel=1
                                left join Contacts_Star c on a.MemberId=c.OtherMemberId and c.IsStar=1 and  c.MemberId='" + memberid + @"'  
                                where a.zhubumen != '' ";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    lianXiRenList = conText.Query<LianXiRenInfo>(sql).ToList();
                }

                var list = _JointOfficeContext.Member_Company.Where(t => t.ParentId == para.id).ToList();
                if (list.Count != 0)
                {
                    foreach (var item in list)
                    {
                        BuMenInfo BuMenInfo = new BuMenInfo();
                        //var list2 = _JointOfficeContext.Member_Info.Where(t => t.ZhuBuMen == item.Id || t.FuBuMen.Contains(item.Id)).ToList();
                        var list2 = _JointOfficeContext.Member_Info_Company.Where(t => t.DeptId == item.Id).ToList();
                        BuMenInfo.id = item.Id;
                        BuMenInfo.name = item.Name;
                        int number = list2.Count();
                        number += GetLianXiRenListByBuMenNumDiGui(item.Id);
                        //var Strlist = new List<string>();
                        //Strlist.Add(item.Id);
                        //var id= item.Id
                        //var list3 = _JointOfficeContext.Member_Company.Where(t => t.ParentId == item.Id).ToList();
                        //while (true)
                        //{
                        //    var list3 = _JointOfficeContext.Member_Company.Where(t => Strlist.Contains(t.ParentId)).ToList();
                        //    Strlist.Clear();
                        //    if (list3.Count != 0)
                        //    {
                        //        foreach (var one in list3)
                        //        {
                        //            //var list4 = _JointOfficeContext.Member_Info.Where(t => t.ZhuBuMen == one.Id || t.FuBuMen.Contains(one.Id)).ToList();
                        //            var list4 = _JointOfficeContext.Member_Info_Company.Where(t => t.DeptId == one.Id).ToList();
                        //            foreach (var OneDate in list4)
                        //            {
                        //                Strlist.Add(OneDate.Id);
                        //            }
                        //            number = number + list4.Count;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        break;
                        //    }
                        //}
                        BuMenInfo.count = number;
                        buMenList.Add(BuMenInfo);
                    }
                }
            }
            foreach (var item in lianXiRenList)
            {
                item.picture = item.picture + SasKey;
                item.jobName = "";
                var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == item.jobid).FirstOrDefault();
                if (memJob != null)
                {
                    item.jobName = memJob.Name;
                }
            }
            ContactsList.buMenList = buMenList;
            ContactsList.lianXiRenList = lianXiRenList;
            res.showapi_res_code = "200";
            res.showapi_res_body = ContactsList;
            return res;
        }
        /// <summary>
        /// 获取部门(包含子部门下)全部人员数量  递归
        /// </summary>
        public int GetLianXiRenListByBuMenNumDiGui(string id)
        {
            var num = 0;
            var list = _JointOfficeContext.Member_Company.Where(t => t.ParentId == id).ToList();
            foreach (var item in list)
            {
                var list2 = _JointOfficeContext.Member_Info_Company.Where(t => t.DeptId == item.Id).ToList();
                num += list2.Count();
                var num1 = GetLianXiRenListByBuMenNumDiGui(item.Id);
                num += num1;
            }
            return num;
        }
        /// <summary>
        /// 查看部门列表
        /// </summary>
        public Showapi_Res_List<BuMenInfo> GetBuMenList(CompanyPersonListInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<BuMenInfo>();
                return Return.Return();
            }
            Showapi_Res_List<BuMenInfo> res = new Showapi_Res_List<BuMenInfo>();
            List<BuMenInfo> list = new List<BuMenInfo>();
            List<CompanyList> companyList = new List<CompanyList>();
            var bumen1 = _JointOfficeContext.Member_Company.Where(t => t.ParentId == para.id && t.Name.Contains(para.body)).ToList();
            foreach (var item in bumen1)
            {
                CompanyList CompanyList = new CompanyList();
                CompanyList.id = item.Id;
                CompanyList.name = item.Name;
                companyList.Add(CompanyList);
                GetCompanyListDiGui(companyList, item.Id, para.body);
            }
            //var bumenlist = _JointOfficeContext.Member_Company.Where(t => t.ParentId != "0").ToList();
            foreach (var item in companyList)
            {
                List<ZanRenYuanList> personList = new List<ZanRenYuanList>();
                BuMenInfo BuMenInfo = new BuMenInfo();
                BuMenInfo.id = item.id;
                BuMenInfo.name = item.name;
                var bumen = _JointOfficeContext.Member_Info_Company.Where(t => t.DeptId == item.id).ToList();
                int number = bumen.Count();
                foreach (var item1 in bumen)
                {
                    ZanRenYuanList ZanRenYuanList = new ZanRenYuanList();
                    ZanRenYuanList.memberid = item1.MemberId;
                    var mem = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item1.MemberId).FirstOrDefault();
                    if (mem != null)
                    {
                        ZanRenYuanList.name = mem.Name;
                        ZanRenYuanList.picture = mem.Picture + SasKey;
                    }
                    else
                    {
                        ZanRenYuanList.name = "";
                        ZanRenYuanList.picture = "";
                    }
                    personList.Add(ZanRenYuanList);
                }
                var Strlist = new List<string>();
                Strlist.Add(item.id);
                //var id= item.Id
                //var list3 = _JointOfficeContext.Member_Company.Where(t => t.ParentId == item.Id).ToList();
                //while (true)
                //{
                //    var list3 = _JointOfficeContext.Member_Company.Where(t => Strlist.Contains(t.ParentId)).ToList();
                //    Strlist.Clear();
                //    if (list3.Count != 0)
                //    {
                //        foreach (var one in list3)
                //        {
                //            var list4 = _JointOfficeContext.Member_Info.Where(t => t.ZhuBuMen == one.Id || t.FuBuMen.Contains(one.Id)).ToList();
                //            foreach (var OneDate in list4)
                //            {
                //                Strlist.Add(OneDate.Id);
                //                ZanRenYuanList ZanRenYuanList = new ZanRenYuanList();
                //                ZanRenYuanList.memberid = OneDate.MemberId;
                //                ZanRenYuanList.name = OneDate.Name;
                //                ZanRenYuanList.picture = OneDate.Picture + SasKey;
                //                personList.Add(ZanRenYuanList);
                //            }
                //            number = number + list4.Count;
                //        }
                //    }
                //    else
                //    {
                //        break;
                //    }
                //}
                BuMenInfo.count = number;
                BuMenInfo.personList = personList;
                list.Add(BuMenInfo);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<BuMenInfo>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 查看部门列表  递归
        /// </summary>
        public List<CompanyList> GetCompanyListDiGui(List<CompanyList> list, string id, string body)
        {
            var bumen1 = _JointOfficeContext.Member_Company.Where(t => t.ParentId == id && t.Name.Contains(body)).ToList();
            foreach (var item in bumen1)
            {
                CompanyList CompanyList = new CompanyList();
                CompanyList.id = item.Id;
                CompanyList.name = item.Name;
                list.Add(CompanyList);
                GetCompanyListDiGui(list, item.Id, body);
            }
            return list;
        }
        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="邮箱"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateMail(UpdateMemberinfo_Mail para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            //var memberid = "0b6f0588-36ca-45cc-ac36-6f482975827d";
            var Onemail = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            Onemail.Mail = para.mail;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 修改QQ
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateQQ(UpdateMemberinfo_QQ para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            //var memberid = "0b6f0588-36ca-45cc-ac36-6f482975827d";
            var Oneqq = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            Oneqq.QQ = para.qq;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 修改微信
        /// </summary>
        /// <param name="微信"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateWeChat(UpdateMemberinfo_WeChat para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            //var memberid = "0b6f0588-36ca-45cc-ac36-6f482975827d";
            var Onewechat = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            Onewechat.WeChat = para.wechat;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 修改职务
        /// </summary>
        /// <param name="职务"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateJobName(UpdateMemberinfo_JobName para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            //var memberid = "0b6f0588-36ca-45cc-ac36-6f482975827d";
            var Onejobname = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            //Onejobname.JobName = para.jobname;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 修改性别
        /// </summary>
        /// <param name="性别"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateGender(UpdateMemberinfo_Gender para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            //var memberid = "0b6f0588-36ca-45cc-ac36-6f482975827d";
            var Onegender = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            Onegender.Gender = para.gender;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 修改汇报对象
        /// </summary>
        /// <param name="汇报对象"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateHuiBaoDuiXiang(UpdateMemberinfo_HuiBaoDuiXiang para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            //var memberid = "0b6f0588-36ca-45cc-ac36-6f482975827d";
            var Onehuibaoduixiang = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            Onehuibaoduixiang.HuiBaoDuiXiang = para.huibaoduixiang;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 修改电话
        /// </summary>
        /// <param name="电话"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdatePhone(UpdateMemberinfo_Phone para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            //var memberid = "0b6f0588-36ca-45cc-ac36-6f482975827d";
            var Onephone = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            Onephone.Phone = para.phone;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 修改工作介绍
        /// </summary>
        /// <param name="工作介绍"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateGongZuoJieShao(UpdateMemberinfo_GongZuoJieShao para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            //var memberid = "0b6f0588-36ca-45cc-ac36-6f482975827d";
            var Onegongzuojieshao = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            Onegongzuojieshao.GongZuoJieShao = para.gongzuojieshao;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 修改头像
        /// </summary>
        public Showapi_Res_Meaasge UpdateInfoUrl(BlobFilePara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            //var memberid = "0b6f0588-36ca-45cc-ac36-6f482975827d";
            var Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            Info.Picture = para.fileurl;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 修改个人信息
        /// </summary>
        public Showapi_Res_Meaasge UpdatePersonInfo(UpdatePersonInfoInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            memberInfo.Gender = para.gender;
            //memberInfo.GongZuoJieShao = para.gongzuojieshao;
            memberInfo.Phone = para.phone;
            memberInfo.WeChat = para.wechat;
            memberInfo.Mail = para.mail;
            memberInfo.QQ = para.qq;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 设为星标联系人
        /// </summary>
        /// <param name="对方ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge SetStar(UpdateMemberinfo_SetStar para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var islist = _JointOfficeContext.Contacts_Star.Where(t => t.MemberId == memberid && t.OtherMemberId == para.othermemberid).FirstOrDefault();
            if (islist == null)
            {
                Contacts_Star newstar = new Contacts_Star();
                newstar.Id = Guid.NewGuid().ToString();
                newstar.CreateDate = DateTime.Now;
                newstar.IsStar = 1;
                newstar.MemberId = memberid;
                newstar.OtherMemberId = para.othermemberid;
                _JointOfficeContext.Contacts_Star.Add(newstar);
            }
            else
            {
                islist.IsStar = 1;
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("设置星标成功");
        }
        /// <summary>
        /// 取消星标联系人
        /// </summary>
        /// <param name="对方ID"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge NoSetStar(UpdateMemberinfo_NoSetStar para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var islist = _JointOfficeContext.Contacts_Star.Where(t => t.MemberId == memberid && t.OtherMemberId == para.othermemberid).FirstOrDefault();
            if (islist == null)
            {
                return Message.SuccessMeaasge("未设置星标");
            }
            else
            {
                islist.IsStar = 0;
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("取消星标成功");
        }
        /// <summary>
        /// 点赞  取消点赞
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge SetAgree(SetAgreePara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            if (para.isDian == 1)
            {
                Agree Agree = new Agree();
                Agree.Id = Guid.NewGuid().ToString();
                Agree.MemberId = memberid;
                Agree.OtherMemberId = para.otherMemberId;
                Agree.Type = para.type;
                if (para.type.Contains("+"))
                {
                    Agree.UId = "";
                    Agree.P_UId = para.uid;

                    var OneDate = _JointOfficeContext.TotalNum.Where(t => t.P_UId == para.uid && t.PId == para.pid).FirstOrDefault();
                    if (OneDate != null)
                    {
                        OneDate.DianZanNum += 1;
                    }
                }
                else
                {
                    Agree.UId = para.uid;
                    Agree.P_UId = "";

                    var OneDate = _JointOfficeContext.TotalNum.Where(t => t.UId == para.uid).FirstOrDefault();
                    if (OneDate != null)
                    {
                        OneDate.DianZanNum += 1;
                    }
                }
                Agree.PId = para.pid;
                Agree.Body = para.body;
                Agree.CreateTime = DateTime.Now;
                Agree.PhoneModel = para.phoneModel;
                _JointOfficeContext.Agree.Add(Agree);
                _JointOfficeContext.SaveChanges();
                return Message.SuccessMeaasge("点赞成功");
            }
            else if (para.isDian == 2)
            {
                if (para.type.Contains("+"))
                {
                    var OneAgree = _JointOfficeContext.Agree.Where(t => t.PId == para.pid && t.MemberId == memberid).FirstOrDefault();
                    if (OneAgree != null)
                    {
                        _JointOfficeContext.Agree.Remove(OneAgree);

                        var OneDate = _JointOfficeContext.TotalNum.Where(t => t.P_UId == para.uid && t.PId == para.pid).FirstOrDefault();
                        if (OneDate != null)
                        {
                            OneDate.DianZanNum -= 1;
                        }
                    }

                    //var OneDate = _JointOfficeContext.TotalNum.Where(t => t.P_UId == para.uid && t.PId == para.pid).FirstOrDefault();
                    //if (OneDate != null)
                    //{
                    //    OneDate.DianZanNum -= 1;
                    //}
                }
                else
                {
                    var OneAgree = _JointOfficeContext.Agree.Where(t => t.UId == para.uid && t.MemberId == memberid).FirstOrDefault();
                    if (OneAgree != null)
                    {
                        _JointOfficeContext.Agree.Remove(OneAgree);

                        var OneDate = _JointOfficeContext.TotalNum.Where(t => t.UId == para.uid).FirstOrDefault();
                        if (OneDate != null)
                        {
                            OneDate.DianZanNum -= 1;
                        }
                    }

                    //var OneDate = _JointOfficeContext.TotalNum.Where(t => t.UId == para.uid).FirstOrDefault();
                    //if (OneDate != null)
                    //{
                    //    OneDate.DianZanNum -= 1;
                    //}
                }
                _JointOfficeContext.SaveChanges();
                return Message.SuccessMeaasge("取消点赞成功");
            }
            else
            {
                return Message.SuccessMeaasge("");
            }
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id,类型"></param>
        /// <returns></returns>
        public Showapi_Res_Meaasge NoSetComment(NoSetCommentPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var PingLun = _JointOfficeContext.Comment_Body.Where(t => t.Id == para.id).FirstOrDefault();
            if (PingLun != null)
            {
                var OneDate = _JointOfficeContext.TotalNum.Where(t => t.UId == PingLun.UId).FirstOrDefault();
                if (OneDate != null)
                {
                    OneDate.PingLunNum -= 1;
                }
                var OneDate1 = _JointOfficeContext.TotalNum.Where(t => t.PId == para.id).FirstOrDefault();
                if (OneDate1 != null)
                {
                    _JointOfficeContext.TotalNum.Remove(OneDate1);
                }
                _JointOfficeContext.Comment_Body.Remove(PingLun);
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("删除成功");
        }
        /// <summary>
        /// 查看赞人员
        /// </summary>
        public Showapi_Res_List<ZanRenYuanList> GetZanRenYuanList(GetZanRenYuanListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<ZanRenYuanList>();
                return Return.Return();
            }
            Showapi_Res_List<ZanRenYuanList> res = new Showapi_Res_List<ZanRenYuanList>();
            List<ZanRenYuanList> list = new List<ZanRenYuanList>();
            var Onelist = _JointOfficeContext.Agree.Where(t => t.UId == para.id).OrderByDescending(t => t.CreateTime).ToList();
            foreach (var item in Onelist)
            {
                ZanRenYuanList ZanRenYuanList = new ZanRenYuanList();
                var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
                ZanRenYuanList.memberid = item.MemberId;
                ZanRenYuanList.name = info.Name;
                ZanRenYuanList.picture = info.Picture + SasKey;
                list.Add(ZanRenYuanList);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<ZanRenYuanList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 查看评论列表
        /// </summary>
        public Showapi_Res_List<PingLun> GetPingLunList(GetZanRenYuanListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<PingLun>();
                return Return.Return();
            }
            Showapi_Res_List<PingLun> res = new Showapi_Res_List<PingLun>();
            List<PingLun> list = new List<PingLun>();
            List<string> idList = new List<string>();
            var allPage = 0;
            var allNum = 0;
            var sql = @"exec GetPingLun '" + para.id + "'";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                idList = conText.Query<string>(sql).ToList();
                allNum = idList.Count();
                var allPage1 = idList.Count() / para.count;
                if (idList.Count % para.count != 0)
                {
                    allPage1 += 1;
                }
                allPage = allPage1;
                idList = idList.Skip(para.page * para.count).Take(para.count).ToList();
            }
            foreach (var item in idList)
            {
                PingLun PingLun = new PingLun();
                var comment = _JointOfficeContext.Comment_Body.Where(t => t.Id == item).FirstOrDefault();
                var dianping = _JointOfficeContext.DianPing_Body.Where(t => t.Id == item).FirstOrDefault();
                var totalNum = _JointOfficeContext.TotalNum.Where(t => t.P_UId == para.id && t.PId == item).FirstOrDefault();
                var agree = _JointOfficeContext.Agree.Where(t => t.PId == item && t.MemberId == memberid).FirstOrDefault();
                if (comment != null)
                {
                    PingLun.id = item;
                    PingLun.reviewPersonID = comment.PingLunMemberId;
                    PingLun.reviewPersonName = comment.Name;
                    PingLun.picture = comment.Picture + SasKey;
                    PingLun.body = comment.Body;
                    PingLun.previousMemberId = comment.PersonId;
                    PingLun.previousName = comment.PersonName;
                    TimeSpan ts = DateTime.Now - comment.PingLunTime;
                    if (ts.Days > 0 || ts.Hours > 0)
                    {
                        PingLun.pingLunTime = comment.PingLunTime.ToString("yyyy-MM-dd HH:mm");
                    }
                    else
                    {
                        if (ts.Minutes > 0)
                        {
                            PingLun.pingLunTime = ts.Minutes + "分钟前";
                        }
                        else
                        {
                            //PingLun.pingLunTime = ts.Seconds + "秒前";
                            PingLun.pingLunTime = "刚刚";
                        }
                    }
                    PingLun.phoneModel = comment.PhoneModel;
                    if (comment.PictureList != null && comment.PictureList != "")
                    {
                        var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(comment.PictureList);
                        foreach (var itemPictureList in listPicture)
                        {
                            itemPictureList.url = itemPictureList.url + SasKey;
                        }
                        PingLun.appendPicture = listPicture;
                    }
                    if (comment.Annex != null && comment.Annex != "")
                    {
                        var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Work_File>>(comment.Annex);
                        long length = 0;
                        foreach (var itemAnnex in listAnnex)
                        {
                            itemAnnex.url = itemAnnex.url + SasKey;
                            length += itemAnnex.length;
                        }
                        PingLun.annex = listAnnex;
                        PingLun.annexLength = BusinessHelper.ConvertBytes(length);
                    }
                    if (comment.Voice != null && comment.Voice != "")
                    {
                        PingLun.voice = comment.Voice + SasKey;
                    }
                    if (comment.VoiceLength != null && comment.VoiceLength != "" && comment.VoiceLength.Substring(0, 1) == "0")
                    {
                        PingLun.voiceLength = comment.VoiceLength.Substring(1, 1);
                    }
                    else
                    {
                        PingLun.voiceLength = comment.VoiceLength;
                    }
                    if (comment.PingLunMemberId == memberid && comment.IsExeComment == 0)
                    {
                        PingLun.isDelete = "1";
                    }
                    else
                    {
                        PingLun.isDelete = "0";
                    }
                    if (agree != null)
                    {
                        PingLun.isZan = "1";
                    }
                    else
                    {
                        PingLun.isZan = "0";
                    }
                    if (totalNum != null)
                    {
                        PingLun.pingLunNum = totalNum.PingLunNum;
                        PingLun.dianZanNum = totalNum.DianZanNum;
                    }
                    else
                    {
                        PingLun.pingLunNum = 0;
                        PingLun.dianZanNum = 0;
                    }
                }
                if (dianping != null)
                {
                    PingLun.id = item;
                    PingLun.reviewPersonID = dianping.DianPingMemberId;
                    PingLun.reviewPersonName = dianping.Name;
                    PingLun.picture = dianping.Picture + SasKey;
                    PingLun.body = dianping.Body;
                    PingLun.previousMemberId = "";
                    PingLun.previousName = "";
                    TimeSpan ts = DateTime.Now - dianping.DianPingTime;
                    if (ts.Days > 0 || ts.Hours > 0)
                    {
                        PingLun.pingLunTime = dianping.DianPingTime.ToString("yyyy-MM-dd HH:mm");
                    }
                    else
                    {
                        if (ts.Minutes > 0)
                        {
                            PingLun.pingLunTime = ts.Minutes + "分钟前";
                        }
                        else
                        {
                            //PingLun.pingLunTime = ts.Seconds + "秒前";
                            PingLun.pingLunTime = "刚刚";
                        }
                    }
                    PingLun.phoneModel = dianping.PhoneModel;
                    if (dianping.PictureList != null && dianping.PictureList != "")
                    {
                        var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(dianping.PictureList);
                        foreach (var itemPictureList in listPicture)
                        {
                            itemPictureList.url = itemPictureList.url + SasKey;
                        }
                        PingLun.appendPicture = listPicture;
                    }
                    if (dianping.Annex != null && dianping.Annex != "")
                    {
                        var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Work_File>>(dianping.Annex);
                        long length = 0;
                        foreach (var itemAnnex in listAnnex)
                        {
                            itemAnnex.url = itemAnnex.url + SasKey;
                            length += itemAnnex.length;
                        }
                        PingLun.annex = listAnnex;
                        PingLun.annexLength = BusinessHelper.ConvertBytes(length);
                    }
                    if (dianping.Voice != null && dianping.Voice != "")
                    {
                        PingLun.voice = dianping.Voice + SasKey;
                    }
                    if (dianping.VoiceLength != null && dianping.VoiceLength != "" && dianping.VoiceLength.Substring(0, 1) == "0")
                    {
                        PingLun.voiceLength = dianping.VoiceLength.Substring(1, 1);
                    }
                    else
                    {
                        PingLun.voiceLength = dianping.VoiceLength;
                    }
                    PingLun.isDelete = "0";
                    if (agree != null)
                    {
                        PingLun.isZan = "1";
                    }
                    else
                    {
                        PingLun.isZan = "0";
                    }
                    if (totalNum != null)
                    {
                        PingLun.pingLunNum = totalNum.PingLunNum;
                        PingLun.dianZanNum = totalNum.DianZanNum;
                    }
                    else
                    {
                        PingLun.pingLunNum = 0;
                        PingLun.dianZanNum = 0;
                    }
                }
                list.Add(PingLun);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<PingLun>();
            res.showapi_res_body.allPages = allPage;
            res.showapi_res_body.allNum = allNum;
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 搜索联系人
        /// </summary>
        public Showapi_Res_List<LianXiRenInfo> GetLianXiRenListByName(GetLianXiRenListByNamePara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<LianXiRenInfo>();
                return Return.Return();
            }
            Showapi_Res_List<LianXiRenInfo> res = new Showapi_Res_List<LianXiRenInfo>();
            List<LianXiRenInfo> list = new List<LianXiRenInfo>();
            string sql = @"SELECT a.memberid,a.name,isnull(a.jobName,'') jobName,a.mobile,a.picture,isnull(b.name,'') zhubumenName,isnull(c.isStar,'') isStar,isnull(a.zhubumen,'') zhubumen,isnull(e.Name,'') bumenfuzerenName,
                                a.gender,isnull(a.mail,'') mail,isnull(a.weChat,'') weChat,isnull(a.qq,'') qq,isnull(a.gongZuoJieShao,'') gongZuoJieShao,
								isnull(a.bumenfuzeren,'') bumenfuzeren,isnull(a.phone,'') phone,isnull(a.huiBaoDuiXiang,'') huiBaoDuiXiang,isnull(d.name,'') huiBaoDuiXiangName
                            from Member_Info a 
                            left join Member_Info d on a.HuiBaoDuiXiang=d.MemberId
                            left join Member_Company b on a.ZhuBuMen=b.Id
                            left join Member_Info e on a.BuMenFuZeRen=e.MemberId
                            left join Contacts_Star c on a.MemberId=c.OtherMemberId and c.IsStar=1  and  c.MemberId='" + memberid + "' where a.name like '%" + para.name + "%' ";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                list = conText.Query<LianXiRenInfo>(sql).ToList();
            }
            foreach (var one in list)
            {
                one.picture = one.picture + SasKey;
                one.jobName = "";
                var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == one.jobid).FirstOrDefault();
                if (memJob != null)
                {
                    one.jobName = memJob.Name;
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<LianXiRenInfo>();
            res.showapi_res_body.contentlist = list;
            return res;

        }
        /// <summary>
        /// 获取当前用户通讯录首页  所属公司信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<PersonAddressBook> GetPersonAddressBook()
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<PersonAddressBook>();
                return Return.Return();
            }
            Showapi_Res_List<PersonAddressBook> res = new Showapi_Res_List<PersonAddressBook>();
            List<PersonAddressBook> list = new List<PersonAddressBook>();
            //Thread.Sleep(6000);
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            if (memberInfo != null && !string.IsNullOrEmpty(memberInfo.CompanyIDS))
            {
                //var memberCompanys = "";
                var memberCompanyList = memberInfo.CompanyIDS.Split(",");
                foreach (var item in memberCompanyList)
                {
                    var memCom = _JointOfficeContext.Member_Company.Where(t => t.Id == item).FirstOrDefault();
                    if (memCom != null)
                    {
                        PersonAddressBook PersonAddressBook = new PersonAddressBook();
                        PersonAddressBook.id = memCom.Id;
                        PersonAddressBook.name = memCom.Name;
                        list.Add(PersonAddressBook);
                    }
                    //memberCompanys += "'" + item + "',";
                }
                //memberCompanys = memberCompanys.Remove(memberCompanys.LastIndexOf(","));
                //var sql = @"select id,name
                //            from Member_Company
                //            where id in (" + memberCompanys + @")";
                //using (SqlConnection conText = new SqlConnection(constr))
                //{
                //    list = conText.Query<PersonAddressBook>(sql).ToList();
                //}
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<PersonAddressBook>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 获取某公司下所有人员
        /// </summary>
        public Showapi_Res_List<SearchPersonList> GetCompanyPersonList(CompanyPersonListInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<SearchPersonList>();
                return Return.Return();
            }
            Showapi_Res_List<SearchPersonList> res = new Showapi_Res_List<SearchPersonList>();
            List<SearchPersonList> list = new List<SearchPersonList>();
            List<Member_Info> memberList = new List<Member_Info>();
            if (string.IsNullOrEmpty(para.body))
            {
                memberList = _JointOfficeContext.Member_Info.Where(t => t.CompanyIDS.Contains(para.id)).ToList();
            }
            else
            {
                memberList = _JointOfficeContext.Member_Info.Where(t => t.CompanyIDS.Contains(para.id) && (t.Name == para.body || t.Mobile == para.body)).ToList();
            }
            foreach (var item in memberList)
            {
                var memIsUse = _JointOfficeContext.Member.Where(t => t.Id == item.MemberId).FirstOrDefault();
                if (memIsUse != null && memIsUse.IsDel == 1 && memIsUse.IsUse == 1)
                {
                    SearchPersonList SearchPersonList = new SearchPersonList();
                    var star = _JointOfficeContext.Contacts_Star.Where(t => t.MemberId == memberid && t.OtherMemberId == item.MemberId && t.IsStar == 1).FirstOrDefault();
                    if (star != null)
                    {
                        SearchPersonList.isStar = true;
                    }
                    else
                    {
                        SearchPersonList.isStar = false;
                    }
                    SearchPersonList.id = item.MemberId;
                    SearchPersonList.name = item.Name;
                    var memInfoCom = _JointOfficeContext.Member_Info_Company.Where(t => t.MemberId == item.MemberId && t.CompanyId == para.id).OrderBy(t => t.Num).FirstOrDefault();
                    if (memInfoCom != null)
                    {
                        SearchPersonList.job = "";
                        var job = _JointOfficeContext.Member_Job.Where(t => t.Id == memInfoCom.JobId).FirstOrDefault();
                        if (job != null)
                        {
                            SearchPersonList.job = job.Name;
                        }
                    }
                    else
                    {
                        SearchPersonList.job = "";
                    }
                    SearchPersonList.picture = item.Picture + SasKey;
                    SearchPersonList.mobile = item.Mobile;
                    list.Add(SearchPersonList);
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<SearchPersonList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 搜索联系人
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<SearchPersonList> GetSearchPersonList(SearchPersonInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<SearchPersonList>();
                return Return.Return();
            }
            Showapi_Res_List<SearchPersonList> res = new Showapi_Res_List<SearchPersonList>();
            List<SearchPersonList> list = new List<SearchPersonList>();
            var memberContact = _JointOfficeContext.Member_Contact.Where(t => t.MemberId == memberid).FirstOrDefault();
            var member = _JointOfficeContext.Member_Info.Where(t => t.Name == para.body || t.Mobile == para.body).ToList();
            //if (para.type == "1")
            //{
            //    if (memberContact != null && !string.IsNullOrEmpty(memberContact.Contacts))
            //    {
            //        foreach (var item in member)
            //        {
            //            if (memberContact.Contacts.Contains(item.MemberId))
            //            {
            //                SearchPersonList SearchPersonList = new SearchPersonList();
            //                SearchPersonList.id = item.MemberId;
            //                SearchPersonList.name = item.Name;
            //                SearchPersonList.job = item.JobName;
            //                if (!string.IsNullOrEmpty(item.CompanyIDS))
            //                {
            //                    var memberCompanyList = item.CompanyIDS.Split(",");
            //                    var company = _JointOfficeContext.Member_Company.Where(t => t.Id == memberCompanyList[0]).FirstOrDefault();
            //                    SearchPersonList.company = company.Name;
            //                }
            //                else
            //                {
            //                    SearchPersonList.company = "";
            //                }
            //                SearchPersonList.picture = item.Picture + SasKey;
            //                SearchPersonList.mobile = item.Mobile;
            //                SearchPersonList.isAdd = true;
            //                list.Add(SearchPersonList);
            //            }
            //        }
            //    }
            //}
            //if (para.type == "2")
            //{
            foreach (var item in member)
            {
                var memIsUse = _JointOfficeContext.Member.Where(t => t.Id == item.MemberId).FirstOrDefault();
                if (memIsUse != null && memIsUse.IsDel == 1 && memIsUse.IsUse == 1)
                {
                    SearchPersonList SearchPersonList = new SearchPersonList();
                    SearchPersonList.id = item.MemberId;
                    SearchPersonList.name = item.Name;
                    SearchPersonList.job = "";
                    var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == item.JobID).FirstOrDefault();
                    if (memJob != null)
                    {
                        SearchPersonList.job = memJob.Name;
                    }
                    if (!string.IsNullOrEmpty(item.CompanyIDS))
                    {
                        var memberCompanyList = item.CompanyIDS.Split(",");
                        SearchPersonList.company = "";
                        var company = _JointOfficeContext.Member_Company.Where(t => t.Id == memberCompanyList[0]).FirstOrDefault();
                        if (company != null)
                        {
                            SearchPersonList.company = company.Name;
                        }
                    }
                    else
                    {
                        SearchPersonList.company = "";
                    }
                    SearchPersonList.picture = item.Picture + SasKey;
                    SearchPersonList.mobile = item.Mobile;
                    if (memberContact != null && !string.IsNullOrEmpty(memberContact.Contacts))
                    {
                        if (memberContact.Contacts.Contains(item.MemberId))
                        {
                            SearchPersonList.isAdd = true;
                        }
                        else
                        {
                            SearchPersonList.isAdd = false;
                        }
                    }
                    else
                    {
                        SearchPersonList.isAdd = false;
                    }
                    list.Add(SearchPersonList);
                }
            }
            //}
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<SearchPersonList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge AddContact(UpdateMember_info para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var memone = _JointOfficeContext.Member_Info.Where(t => t.MemberId == para.id).FirstOrDefault();
            if (memone != null)
            {
                var memberContact = _JointOfficeContext.Member_Contact.Where(t => t.MemberId == memberid).FirstOrDefault();
                if (memberContact == null)
                {
                    Member_Contact Member_Contact = new Member_Contact();
                    Member_Contact.Id = Guid.NewGuid().ToString();
                    Member_Contact.MemberId = memberid;
                    Member_Contact.Contacts = para.id;
                    _JointOfficeContext.Member_Contact.Add(Member_Contact);
                    _JointOfficeContext.SaveChanges();
                    return Message.SuccessMeaasgeCode("添加成功", para.id);
                }
                else
                {
                    if (!memberContact.Contacts.Contains(para.id))
                    {
                        memberContact.Contacts += "," + para.id;
                        _JointOfficeContext.SaveChanges();
                        return Message.SuccessMeaasgeCode("添加成功", para.id);
                    }
                    else
                    {
                        return Message.SuccessMeaasgeCode("已添加此联系人", para.id);
                    }
                }
            }
            else
            {
                return Message.SuccessMeaasgeCode("此人员不存在", "");
            }
        }
        /// <summary>
        /// 我的联系人列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<SearchPersonList> GetMyContactList(MyContactListInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<SearchPersonList>();
                return Return.Return();
            }
            Showapi_Res_List<SearchPersonList> res = new Showapi_Res_List<SearchPersonList>();
            List<SearchPersonList> list = new List<SearchPersonList>();
            var memberContact = _JointOfficeContext.Member_Contact.Where(t => t.MemberId == memberid).FirstOrDefault();
            if (memberContact != null && !string.IsNullOrEmpty(memberContact.Contacts))
            {
                var contacts = memberContact.Contacts.Split(",");
                foreach (var item in contacts)
                {
                    var memIsUse = _JointOfficeContext.Member.Where(t => t.Id == item).FirstOrDefault();
                    if (memIsUse != null && memIsUse.IsDel == 1 && memIsUse.IsUse == 1)
                    {
                        var memberOne = new Member_Info();
                        if (string.IsNullOrEmpty(para.body))
                        {
                            memberOne = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item).FirstOrDefault();
                        }
                        else
                        {
                            memberOne = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item && (t.Name == para.body || t.Mobile == para.body)).FirstOrDefault();
                        }
                        if (memberOne != null)
                        {
                            SearchPersonList SearchPersonList = new SearchPersonList();
                            var star = _JointOfficeContext.Contacts_Star.Where(t => t.MemberId == memberid && t.OtherMemberId == memberOne.MemberId && t.IsStar == 1).FirstOrDefault();
                            if (star != null)
                            {
                                SearchPersonList.isStar = true;
                            }
                            else
                            {
                                SearchPersonList.isStar = false;
                            }
                            SearchPersonList.id = memberOne.MemberId;
                            SearchPersonList.memberid = memberOne.MemberId;
                            SearchPersonList.name = memberOne.Name;
                            SearchPersonList.job = "";
                            var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == memberOne.JobID).FirstOrDefault();
                            if (memJob != null)
                            {
                                SearchPersonList.job = memJob.Name;
                            }
                            if (!string.IsNullOrEmpty(memberOne.CompanyIDS))
                            {
                                var memberCompanyList = memberOne.CompanyIDS.Split(",");
                                SearchPersonList.company = "";
                                var company = _JointOfficeContext.Member_Company.Where(t => t.Id == memberCompanyList[0]).FirstOrDefault();
                                if (company != null)
                                {
                                    SearchPersonList.company = company.Name;
                                }
                            }
                            else
                            {
                                SearchPersonList.company = "";
                            }
                            SearchPersonList.picture = memberOne.Picture + SasKey;
                            SearchPersonList.mobile = memberOne.Mobile;
                            SearchPersonList.isAdd = true;
                            list.Add(SearchPersonList);
                        }
                    }
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<SearchPersonList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 获取Member_Info表中全部信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<GetAllPerson> GetAllPerson()
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<GetAllPerson>();
                return Return.Return();
            }
            Showapi_Res_List<GetAllPerson> res = new Showapi_Res_List<GetAllPerson>();
            List<GetAllPerson> list = new List<GetAllPerson>();
            var memberList = _JointOfficeContext.Member_Info.ToList();
            foreach (var item in memberList)
            {
                GetAllPerson GetAllPerson = new GetAllPerson();
                GetAllPerson.memberid = item.MemberId;
                GetAllPerson.isUse = 1;
                var memOne = _JointOfficeContext.Member.Where(t => t.Id == item.MemberId).FirstOrDefault();
                if (memOne != null)
                {
                    if (memOne.IsDel == 0)
                    {
                        GetAllPerson.isUse = 2;
                    }
                    else
                    {
                        GetAllPerson.isUse = memOne.IsUse;
                    }
                }
                GetAllPerson.name = item.Name;
                GetAllPerson.phone = item.Phone;
                GetAllPerson.mail = item.Mail;
                GetAllPerson.wechat = item.WeChat;
                GetAllPerson.qq = item.QQ;
                GetAllPerson.company = "";
                GetAllPerson.job = "";
                GetAllPerson.dept = "";
                GetAllPerson.deptMember = "";
                var memInfoCom = _JointOfficeContext.Member_Info_Company.Where(t => t.MemberId == item.MemberId).OrderBy(t => t.Num).FirstOrDefault();
                if (memInfoCom != null)
                {
                    var memCom = _JointOfficeContext.Member_Company.Where(t => t.Id == memInfoCom.CompanyId).FirstOrDefault();
                    if (memCom != null)
                    {
                        GetAllPerson.company = memCom.Name;
                    }
                    var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == memInfoCom.JobId).FirstOrDefault();
                    if (memJob != null)
                    {
                        GetAllPerson.job = memJob.Name;
                    }
                    var memDept = _JointOfficeContext.Member_Company.Where(t => t.Id == memInfoCom.DeptId).FirstOrDefault();
                    if (memDept != null)
                    {
                        GetAllPerson.dept = memDept.Name;
                    }
                    var memDeptMember = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memInfoCom.DeptMemberId).FirstOrDefault();
                    if (memDeptMember != null)
                    {
                        GetAllPerson.deptMember = memDeptMember.Name;
                    }
                }
                GetAllPerson.role = item.Roles;
                list.Add(GetAllPerson);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<GetAllPerson>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<CompanyList> GetCompanyList()
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<CompanyList>();
                return Return.Return();
            }
            Showapi_Res_List<CompanyList> res = new Showapi_Res_List<CompanyList>();
            List<CompanyList> list = new List<CompanyList>();
            var com = _JointOfficeContext.Member_Company.Where(t => t.ParentId == "jituan").ToList();
            foreach (var item in com)
            {
                CompanyList CompanyList = new CompanyList();
                CompanyList.id = item.Id;
                CompanyList.name = item.Name;
                list.Add(CompanyList);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<CompanyList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<DeptList> GetDeptList(UpdateMember_info para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<DeptList>();
                return Return.Return();
            }
            Showapi_Res_List<DeptList> res = new Showapi_Res_List<DeptList>();
            List<DeptList> list = new List<DeptList>();
            var comSelf = _JointOfficeContext.Member_Company.Where(t => t.Id == para.id).FirstOrDefault();
            if (comSelf != null)
            {
                DeptList DeptList = new DeptList();
                DeptList.id = comSelf.Id;
                DeptList.name = comSelf.Name;
                list.Add(DeptList);
            }
            var com = _JointOfficeContext.Member_Company.Where(t => t.ParentId == para.id).ToList();
            foreach (var item in com)
            {
                DeptList DeptList = new DeptList();
                list.Add(DeptList);
                DeptList.id = item.Id;
                DeptList.name = item.Name;
                DeptList.children = new List<DeptList>();
                DeptList.children = GetAllDeptList(DeptList.children, item.Id);
                if (DeptList.children.Count == 0)
                {
                    DeptList.children = null;
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<DeptList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 获取部门信息  递归
        /// </summary>
        /// <returns></returns>
        public List<DeptList> GetAllDeptList(List<DeptList> obj, string id)
        {
            var com = _JointOfficeContext.Member_Company.Where(t => t.ParentId == id).ToList();
            foreach (var item in com)
            {
                DeptList DeptList = new DeptList();
                obj.Add(DeptList);
                DeptList.id = item.Id;
                DeptList.name = item.Name;
                DeptList.children = new List<DeptList>();
                DeptList.children = GetAllDeptList(DeptList.children, item.Id);
                if (DeptList.children.Count == 0)
                {
                    DeptList.children = null;
                }
            }
            return obj;
        }
        /// <summary>
        /// 获取job信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<CompanyJobList> GetJobList(GetJobListInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<CompanyJobList>();
                return Return.Return();
            }
            Showapi_Res_List<CompanyJobList> res = new Showapi_Res_List<CompanyJobList>();
            List<CompanyJobList> list = new List<CompanyJobList>();
            var job = _JointOfficeContext.Member_Job.Where(t => t.MemberCompanyId == para.comId && t.MemberDeptId == para.deptId).ToList();
            foreach (var item in job)
            {
                CompanyJobList CompanyJobList = new CompanyJobList();
                CompanyJobList.id = item.Id;
                var name = "";
                var comObj = GetParentCompany(item.MemberDeptId, name);
                CompanyJobList.name = item.Name + "（" + comObj.name + "）";
                CompanyJobList.nameEx = comObj.name;
                list.Add(CompanyJobList);
            }
            list = list.OrderBy(t => t.nameEx).ToList();
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<CompanyJobList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 获取部门负责人
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<CompanyList> GetDeptBossList(UpdateMember_info para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<CompanyList>();
                return Return.Return();
            }
            Showapi_Res_List<CompanyList> res = new Showapi_Res_List<CompanyList>();
            List<CompanyList> list = new List<CompanyList>();
            var all = _JointOfficeContext.Member_Info_Company.Where(t => t.DeptId == para.id).Select(t => t.MemberId).ToList();
            if (all != null && all.Count != 0)
            {
                var allMember = "";
                foreach (var item in all)
                {
                    allMember += "'" + item + "',";
                }
                allMember = allMember.Remove(allMember.LastIndexOf(","));
                var sql = @"select MemberId id,name from Member_Info where MemberId in (" + allMember + @")";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list = conText.Query<CompanyList>(sql).ToList();
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<CompanyList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 编辑人员公司信息  后台
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge AddPersonCompany(AddPersonCompanyInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var member = _JointOfficeContext.Member_Info.Where(t => t.MemberId == para.memberid).FirstOrDefault();
            var memCom = _JointOfficeContext.Member_Info_Company.Where(t => t.MemberId == para.memberid).OrderBy(t => t.Num).ToList();
            if (memCom.Count == 0)
            {
                var num = 0;
                var comStr = "";
                foreach (var item in para.list)
                {
                    if (!string.IsNullOrEmpty(item.comId) || !string.IsNullOrEmpty(item.deptId) || !string.IsNullOrEmpty(item.jobId))
                    {
                        if (num == 0)
                        {
                            member.ZhuBuMen = item.deptId;
                            //var job = _JointOfficeContext.Member_Job.Where(t => t.Id == item.jobId).FirstOrDefault();
                            member.JobID = item.jobId;
                            //member.JobName = job.Name;
                        }
                        Member_Info_Company Member_Info_Company = new Member_Info_Company();
                        Member_Info_Company.Id = Guid.NewGuid().ToString();
                        Member_Info_Company.MemberId = para.memberid;
                        Member_Info_Company.CompanyId = item.comId;
                        Member_Info_Company.DeptId = item.deptId;
                        Member_Info_Company.JobId = item.jobId;
                        Member_Info_Company.DeptMemberId = item.deptBossId;
                        Member_Info_Company.HuiBaoDuiXiangId = "";
                        Member_Info_Company.GongZuoJieShao = "";
                        Member_Info_Company.Num = num;
                        _JointOfficeContext.Member_Info_Company.Add(Member_Info_Company);

                        if (!comStr.Contains(item.comId))
                        {
                            comStr += item.comId + ",";
                        }
                        num++;
                    }
                }
                if (comStr != "")
                {
                    comStr = comStr.Remove(comStr.LastIndexOf(","));
                }
                member.CompanyIDS = comStr;
            }
            else
            {
                var num = memCom.Count;
                var comStr = member.CompanyIDS + ",";
                foreach (var item in para.list)
                {
                    if (!string.IsNullOrEmpty(item.comId) || !string.IsNullOrEmpty(item.deptId) || !string.IsNullOrEmpty(item.jobId))
                    {
                        Member_Info_Company Member_Info_Company = new Member_Info_Company();
                        Member_Info_Company.Id = Guid.NewGuid().ToString();
                        Member_Info_Company.MemberId = para.memberid;
                        Member_Info_Company.CompanyId = item.comId;
                        Member_Info_Company.DeptId = item.deptId;
                        Member_Info_Company.JobId = item.jobId;
                        Member_Info_Company.DeptMemberId = item.deptBossId;
                        Member_Info_Company.HuiBaoDuiXiangId = "";
                        Member_Info_Company.GongZuoJieShao = "";
                        Member_Info_Company.Num = num;
                        _JointOfficeContext.Member_Info_Company.Add(Member_Info_Company);

                        if (!comStr.Contains(item.comId))
                        {
                            comStr += item.comId + ",";
                        }
                        num++;
                    }
                }
                if (comStr != "")
                {
                    comStr = comStr.Remove(comStr.LastIndexOf(","));
                }
                member.CompanyIDS = comStr;
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("添加成功");
        }
        /// <summary>
        /// 编辑人员公司信息  两端
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdatePersonComInfo(UpdatePersonComInfoInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var memberCom = _JointOfficeContext.Member_Info_Company.Where(t => t.Id == para.id).FirstOrDefault();
            if (memberCom != null)
            {
                memberCom.HuiBaoDuiXiangId = para.huiBaoDuiXiang;
                memberCom.GongZuoJieShao = para.gongZuoJieShao;
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 获取我的群组列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<MyGroupList> GetMyGroupList(MyGroupListInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<MyGroupList>();
                return Return.Return();
            }
            Showapi_Res_List<MyGroupList> res = new Showapi_Res_List<MyGroupList>();
            List<MyGroupList> list = new List<MyGroupList>();
            List<Member_Group> group = new List<Member_Group>();
            if (para.type == "1")
            {
                group = _JointOfficeContext.Member_Group.Where(t => t.MemberId == memberid && t.State != 0).OrderByDescending(t => t.CreateDate).ToList();
            }
            if (para.type == "2")
            {
                group = _JointOfficeContext.Member_Group.Where(t => t.GroupPersonId.Contains(memberid) && t.State != 0).OrderByDescending(t => t.CreateDate).ToList();
            }
            if (!string.IsNullOrEmpty(para.body))
            {
                group = group.Where(t => t.Name.Contains(para.body)).OrderByDescending(t => t.CreateDate).ToList();
            }
            foreach (var item in group)
            {
                if (item.Id.Length <= 36)
                {
                    MyGroupList MyGroupList = new MyGroupList();
                    MyGroupList.id = item.Id;
                    MyGroupList.name = item.Name;
                    MyGroupList.picture = item.Picture + SasKey;
                    var strList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MemberID>>(item.GroupPersonId);
                    MyGroupList.personNum = strList.Count() + "人";
                    list.Add(MyGroupList);
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<MyGroupList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 后台修改人员公司信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdatePersonComInfoBack(UpdatePersonComInfoBackInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            if (!string.IsNullOrEmpty(para.comId) || !string.IsNullOrEmpty(para.deptId) || !string.IsNullOrEmpty(para.jobId))
            {
                var memCom = _JointOfficeContext.Member_Info_Company.Where(t => t.Id == para.id).FirstOrDefault();
                if (memCom != null)
                {
                    memCom.CompanyId = para.comId;
                    memCom.DeptId = para.deptId;
                    memCom.JobId = para.jobId;
                    memCom.DeptMemberId = para.deptBossId;

                    if (memCom.Num == 0)
                    {
                        var member = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memCom.MemberId).FirstOrDefault();
                        if (member != null)
                        {
                            member.ZhuBuMen = para.deptId;
                            //var job = _JointOfficeContext.Member_Job.Where(t => t.Id == para.jobId).FirstOrDefault();
                            member.JobID = para.jobId;
                            //member.JobName = job.Name;
                        }
                    }
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 生成二维码并上传
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge CreateQRCode(UpdateMember_info para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            //批量生成
            //var memInfo = _JointOfficeContext.Member_Info.ToList();
            //foreach (var item in memInfo)
            //{
            //    //生成二维码的内容
            //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
            //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(item.MemberId, QRCodeGenerator.ECCLevel.Q);
            //    QRCode qrcode = new QRCode(qrCodeData);

            //    Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 15, 6, false);
            //    //MemoryStream ms = new MemoryStream();
            //    //qrCodeImage.Save(ms, ImageFormat.Jpeg);
            //    var filePath = @"QRCodeImage\" + item.MemberId + ".jpg";
            //    qrCodeImage.Save(filePath);

            //    //C#文件流读文件
            //    FileStream fsRead = new FileStream(filePath, FileMode.Open);
            //    List<BlobFilePara> blobFiles = new List<BlobFilePara>();
            //    var oneFile = new BlobFilePara();
            //    oneFile.fileYName = ".jpg";
            //    oneFile.filelength = fsRead.Length;
            //    oneFile.fileurl = "https://ygsrs.blob.core.chinacloudapi.cn/spjointoffice/images/" + item.MemberId + ".jpg";
            //    oneFile.fileName = "images/" + item.MemberId + ".jpg";
            //    oneFile.fileContent = fsRead;
            //    oneFile.filetype = 1;
            //    oneFile.annexfiletype = 3;
            //    blobFiles.Add(oneFile);
            //    CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);

            //    //var member = _JointOfficeContext.Member_Info.Where(t => t.MemberId == para.id).FirstOrDefault();
            //    item.QRCodeURL = oneFile.fileurl;
            //}

            //单个生成
            //生成二维码的内容
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(para.id, QRCodeGenerator.ECCLevel.Q);
            QRCode qrcode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 15, 6, false);
            //MemoryStream ms = new MemoryStream();
            //qrCodeImage.Save(ms, ImageFormat.Jpeg);
            var filePath = @"QRCodeImage\" + para.id + ".jpg";
            qrCodeImage.Save(filePath);

            //C#文件流读文件
            FileStream fsRead = new FileStream(filePath, FileMode.Open);
            List<BlobFilePara> blobFiles = new List<BlobFilePara>();
            var oneFile = new BlobFilePara();
            oneFile.fileYName = ".jpg";
            oneFile.filelength = fsRead.Length;
            oneFile.fileurl = "https://ygsrs.blob.core.chinacloudapi.cn/spjointoffice/images/" + para.id + ".jpg";
            oneFile.fileName = "images/" + para.id + ".jpg";
            oneFile.fileContent = fsRead;
            oneFile.filetype = 1;
            oneFile.annexfiletype = 3;
            blobFiles.Add(oneFile);
            CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);

            var member = _JointOfficeContext.Member_Info.Where(t => t.MemberId == para.id).FirstOrDefault();
            member.QRCodeURL = oneFile.fileurl;
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("生成成功");
        }
        /// <summary>
        /// 获取某人的二维码
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<PersonDynamic_info_url> GetPersonQRCode(UpdateMember_info para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnSingle<PersonDynamic_info_url>();
                return Return.Return();
            }
            Showapi_Res_Single<PersonDynamic_info_url> res = new Showapi_Res_Single<PersonDynamic_info_url>();
            PersonDynamic_info_url PersonDynamic_info_url = new PersonDynamic_info_url();
            var member = _JointOfficeContext.Member_Info.Where(t => t.MemberId == para.id).FirstOrDefault();
            if (member != null && !string.IsNullOrEmpty(member.QRCodeURL))
            {
                PersonDynamic_info_url.url = member.QRCodeURL + SasKey;
            }
            else
            {
                PersonDynamic_info_url.url = "";
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = PersonDynamic_info_url;
            return res;
        }
        /// <summary>
        /// 常用联系人
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<SearchPersonList> GetOftenContactList()
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<SearchPersonList>();
                return Return.Return();
            }
            Showapi_Res_List<SearchPersonList> res = new Showapi_Res_List<SearchPersonList>();
            List<SearchPersonList> list = new List<SearchPersonList>();
            var time = DateTime.Now.AddDays(-30);
            var memoften = _JointOfficeContext.Member_Often.Where(t => t.MemberId == memberid && t.WriteDate > time).OrderByDescending(t => t.WriteDate).ToList();
            foreach (var item in memoften)
            {
                var memIsUse = _JointOfficeContext.Member.Where(t => t.Id == item.MemberId).FirstOrDefault();
                if (memIsUse != null && memIsUse.IsDel == 1 && memIsUse.IsUse == 1)
                {
                    var memone = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.OtherMemberId).FirstOrDefault();
                    if (memone != null)
                    {
                        SearchPersonList SearchPersonList = new SearchPersonList();
                        SearchPersonList.id = item.OtherMemberId;
                        SearchPersonList.name = memone.Name;
                        SearchPersonList.job = "";
                        var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == memone.JobID).FirstOrDefault();
                        if (memJob != null)
                        {
                            SearchPersonList.job = memJob.Name;
                        }
                        if (!string.IsNullOrEmpty(memone.CompanyIDS))
                        {
                            var memberCompanyList = memone.CompanyIDS.Split(",");
                            var com = _JointOfficeContext.Member_Company.Where(t => t.Id == memberCompanyList[0]).FirstOrDefault();
                            if (com != null)
                            {
                                SearchPersonList.company = com.Name;
                            }
                            else
                            {
                                SearchPersonList.company = "";
                            }
                        }
                        else
                        {
                            SearchPersonList.company = "";
                        }
                        SearchPersonList.picture = memone.Picture + SasKey;
                        SearchPersonList.mobile = memone.Mobile;
                        var memberContact = _JointOfficeContext.Member_Contact.Where(t => t.MemberId == memberid).FirstOrDefault();
                        if (memberContact != null && !string.IsNullOrEmpty(memberContact.Contacts))
                        {
                            if (memberContact.Contacts.Contains(item.OtherMemberId))
                            {
                                SearchPersonList.isAdd = true;
                            }
                            else
                            {
                                SearchPersonList.isAdd = false;
                            }
                        }
                        else
                        {
                            SearchPersonList.isAdd = false;
                        }
                        var star = _JointOfficeContext.Contacts_Star.Where(t => t.MemberId == memberid && t.OtherMemberId == item.OtherMemberId && t.IsStar == 1).FirstOrDefault();
                        if (star != null)
                        {
                            SearchPersonList.isStar = true;
                        }
                        else
                        {
                            SearchPersonList.isStar = false;
                        }
                        list.Add(SearchPersonList);
                    }
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<SearchPersonList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// web左上角切换公司时人员job随之变化
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<WebMemberInfo> GetWebMemberInfo(UpdateMember_info para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnSingle<WebMemberInfo>();
                return Return.Return();
            }
            Showapi_Res_Single<WebMemberInfo> res = new Showapi_Res_Single<WebMemberInfo>();
            WebMemberInfo WebMemberInfo = new WebMemberInfo();
            var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            WebMemberInfo.memberid = memInfo.MemberId;
            WebMemberInfo.name = memInfo.Name;
            WebMemberInfo.mobile = memInfo.Mobile;
            WebMemberInfo.picture = memInfo.Picture + SasKey;
            var memInfoCom = _JointOfficeContext.Member_Info_Company.Where(t => t.MemberId == memberid && t.CompanyId == para.id).OrderBy(t => t.Num).FirstOrDefault();
            if (memInfoCom != null)
            {
                WebMemberInfo.jobName = "";
                var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == memInfoCom.JobId).FirstOrDefault();
                if (memJob != null)
                {
                    WebMemberInfo.jobName = memJob.Name;
                }
            }
            else
            {
                WebMemberInfo.jobName = "";
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = WebMemberInfo;
            return res;
        }
        /// <summary>
        /// 后台新建+修改个人基本信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdatePersonInfoBack(GetAllPerson para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            if (string.IsNullOrEmpty(para.memberid))
            {
                var phone = _JointOfficeContext.Member_Info.Where(t => t.Mobile == para.phone || t.Phone == para.phone).FirstOrDefault();
                if (phone != null)
                {
                    throw new BusinessTureException("该手机号已被使用.");
                }

                Member membr = new Member();
                membr.Id = Guid.NewGuid().ToString();
                membr.LoginName = para.phone;
                membr.LoginPwd = BusinessHelper.GetMD5("123456");
                membr.CreateDate = DateTime.Now;
                membr.IsDel = 1;
                membr.IsUse = 1;
                _JointOfficeContext.Member.Add(membr);

                WangPan_Member WangPan_Member = new WangPan_Member();
                WangPan_Member.Id = Guid.NewGuid().ToString();
                WangPan_Member.MemberId = membr.Id;
                WangPan_Member.Name = Guid.NewGuid().ToString("N");
                WangPan_Member.CreateDate = DateTime.Now;
                _JointOfficeContext.WangPan_Member.Add(WangPan_Member);

                WangPan_Menu WangPan_Menu = new WangPan_Menu();
                WangPan_Menu.Id = Guid.NewGuid().ToString();
                WangPan_Menu.MemberId = membr.Id;
                WangPan_Menu.Name = WangPan_Member.Name;
                WangPan_Menu.ParentId = "0";
                WangPan_Menu.CreateDate = DateTime.Now;
                _JointOfficeContext.WangPan_Menu.Add(WangPan_Menu);

                Member_Info Member_Info = new Member_Info();
                Member_Info.Id = Guid.NewGuid().ToString();
                Member_Info.MemberId = membr.Id;
                Member_Info.Mobile = membr.LoginName;
                Member_Info.Gender = 0;
                Member_Info.Mail = "";
                Member_Info.ZhuBuMen = "";
                Member_Info.FuBuMen = "";
                Member_Info.HuiBaoDuiXiang = "";
                Member_Info.BuMenFuZeRen = "";
                Member_Info.WeChat = "";
                Member_Info.QQ = "";
                Member_Info.GongZuoJieShao = "";
                Member_Info.Name = para.name;
                Member_Info.JobName = "";
                Member_Info.Phone = membr.LoginName;
                Member_Info.Picture = "https://ygsrs.blob.core.chinacloudapi.cn/spjointoffice/face.png";
                Member_Info.CreateDate = DateTime.Now;
                Member_Info.JobID = "";
                Member_Info.JobGrade = 0;
                Member_Info.MemberCode = "";

                //生成二维码的内容
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(membr.Id, QRCodeGenerator.ECCLevel.Q);
                QRCode qrcode = new QRCode(qrCodeData);

                Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 15, 6, false);
                //MemoryStream ms = new MemoryStream();
                //qrCodeImage.Save(ms, ImageFormat.Jpeg);
                var filePath = @"QRCodeImage\" + membr.Id + ".jpg";
                qrCodeImage.Save(filePath);

                //C#文件流读文件
                FileStream fsRead = new FileStream(filePath, FileMode.Open);
                List<BlobFilePara> blobFiles = new List<BlobFilePara>();
                var oneFile = new BlobFilePara();
                oneFile.fileYName = ".jpg";
                oneFile.filelength = fsRead.Length;
                oneFile.fileurl = "https://ygsrs.blob.core.chinacloudapi.cn/spjointoffice/images/" + membr.Id + ".jpg";
                oneFile.fileName = "images/" + membr.Id + ".jpg";
                oneFile.fileContent = fsRead;
                oneFile.filetype = 1;
                oneFile.annexfiletype = 3;
                blobFiles.Add(oneFile);
                CloudBlobHelper.CreateAsyncTask(this.config.Value.ConnectionStrings.StorageConnectionString, blobFiles);

                Member_Info.QRCodeURL = oneFile.fileurl;

                var num = 0;
                var comStr = "";
                foreach (var item in para.list)
                {
                    if (!string.IsNullOrEmpty(item.comId) || !string.IsNullOrEmpty(item.deptId) || !string.IsNullOrEmpty(item.jobId))
                    {
                        if (num == 0)
                        {
                            Member_Info.ZhuBuMen = item.deptId;
                            //var job = _JointOfficeContext.Member_Job.Where(t => t.Id == item.jobId).FirstOrDefault();
                            Member_Info.JobID = item.jobId;
                            //member.JobName = job.Name;
                        }
                        Member_Info_Company Member_Info_Company = new Member_Info_Company();
                        Member_Info_Company.Id = Guid.NewGuid().ToString();
                        Member_Info_Company.MemberId = membr.Id;
                        Member_Info_Company.CompanyId = item.comId;
                        Member_Info_Company.DeptId = item.deptId;
                        Member_Info_Company.JobId = item.jobId;
                        Member_Info_Company.DeptMemberId = item.deptBossId;
                        Member_Info_Company.HuiBaoDuiXiangId = "";
                        Member_Info_Company.GongZuoJieShao = "";
                        Member_Info_Company.Num = num;
                        _JointOfficeContext.Member_Info_Company.Add(Member_Info_Company);

                        if (!comStr.Contains(item.comId))
                        {
                            comStr += item.comId + ",";
                        }
                        num++;
                    }
                }
                if (comStr != "")
                {
                    comStr = comStr.Remove(comStr.LastIndexOf(","));
                }
                Member_Info.CompanyIDS = comStr;

                _JointOfficeContext.Member_Info.Add(Member_Info);

                CloudBlobContainer container = blobClient.GetContainerReference("jointoffice");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(WangPan_Member.Name + "/ceshi.txt");
                //本机测试D盘  133用C盘
                //using (var fileStream = System.IO.File.OpenRead(@"C:\ceshi.txt"))
                //{
                //    blockBlob.UploadFromStreamAsync(fileStream);
                //}
                //正式库
                using (var fileStream = System.IO.File.OpenRead(Directory.GetCurrentDirectory() + @"\ceshi.txt"))
                {
                    blockBlob.UploadFromStreamAsync(fileStream);
                }
            }
            else
            {
                var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == para.memberid).FirstOrDefault();
                if (memInfo != null)
                {
                    if (memInfo.Phone != para.phone)
                    {
                        var phone = _JointOfficeContext.Member_Info.Where(t => t.MemberId != para.memberid && (t.Mobile == para.phone || t.Phone == para.phone)).FirstOrDefault();
                        if (phone != null)
                        {
                            throw new BusinessTureException("该手机号已被使用.");
                        }
                        memInfo.Phone = para.phone;
                    }
                    memInfo.Name = para.name;
                    memInfo.Mail = para.mail;
                    memInfo.WeChat = para.wechat;
                    memInfo.QQ = para.qq;
                    //if (string.IsNullOrEmpty(memInfo.Roles))
                    //{
                    //    memInfo.Roles = para.role;
                    //}
                    //else
                    //{
                    //    if (!memInfo.Roles.Contains(para.role))
                    //    {
                    //        memInfo.Roles += "," + para.role;
                    //    }
                    //}
                    memInfo.Roles = para.role;
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 后台组织架构管理  获取全部公司部门信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<AllDeptListBack> GetAllDeptListBack()
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<AllDeptListBack>();
                return Return.Return();
            }
            Showapi_Res_List<AllDeptListBack> res = new Showapi_Res_List<AllDeptListBack>();
            List<AllDeptListBack> list = new List<AllDeptListBack>();
            var dept1 = _JointOfficeContext.Member_Company.Where(t => t.ParentId == "jituan").ToList();
            foreach (var item in dept1)
            {
                AllDeptListBack AllDeptListBack = new AllDeptListBack();
                AllDeptListBack.id = item.Id;
                List<string> memberInfo = new List<string>();
                memberInfo = GetCompanyPersonList(memberInfo, item.Id);
                AllDeptListBack.name = item.Name + "（" + memberInfo.Count + "人）";
                AllDeptListBack.children = new List<AllDeptListBack>();
                AllDeptListBack.children = GetAllDeptListBackDiGui(AllDeptListBack.children, item.Id);
                list.Add(AllDeptListBack);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<AllDeptListBack>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 后台组织架构管理  获取全部公司部门信息  递归
        /// </summary>
        /// <returns></returns>
        public List<AllDeptListBack> GetAllDeptListBackDiGui(List<AllDeptListBack> list, string deptId)
        {
            var dept1 = _JointOfficeContext.Member_Company.Where(t => t.ParentId == deptId).ToList();
            foreach (var item in dept1)
            {
                AllDeptListBack AllDeptListBack = new AllDeptListBack();
                AllDeptListBack.id = item.Id;
                List<string> memberInfo = new List<string>();
                memberInfo = GetCompanyPersonList(memberInfo, item.Id);
                AllDeptListBack.name = item.Name + "（" + memberInfo.Count + "人）";
                AllDeptListBack.children = new List<AllDeptListBack>();
                AllDeptListBack.children = GetAllDeptListBackDiGui(AllDeptListBack.children, item.Id);
                list.Add(AllDeptListBack);
            }
            return list;
        }
        public List<string> GetCompanyPersonList(List<string> strlist, string comid)
        {
            var member_Info = _JointOfficeContext.Member_Info_Company.Where(t => t.DeptId == comid).Select(t => t.MemberId).ToList();
            strlist.AddRange(member_Info);
            var childCompany = _JointOfficeContext.Member_Company.Where(t => t.ParentId == comid).ToList();
            if (childCompany.Count != 0)
            {
                foreach (var item in childCompany)
                {
                    strlist = GetCompanyPersonList(strlist, item.Id);
                }
            }
            return strlist;
        }
        /// <summary>
        /// 后台新建+编辑组织
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge AddDeptBack(AddDeptBackInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            if (string.IsNullOrEmpty(para.id))
            {
                Member_Company Member_Company = new Member_Company();
                Member_Company.Id = Guid.NewGuid().ToString();
                Member_Company.ParentId = para.parentId;
                Member_Company.Name = para.name;
                Member_Company.FuZeRen = "";
                Member_Company.CreateDate = DateTime.Now;
                _JointOfficeContext.Member_Company.Add(Member_Company);
            }
            else
            {
                var dept = _JointOfficeContext.Member_Company.Where(t => t.Id == para.id).FirstOrDefault();
                if (dept != null)
                {
                    dept.Name = para.name;
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 后台禁用启用删除账户
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge OpenCloseUser(OpenCloseUserInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(para.memberList);
            var memList = _JointOfficeContext.Member.Where(t => str.Contains(t.Id)).ToList();
            if (para.type != 2)
            {
                foreach (var item in memList)
                {
                    item.IsUse = para.type;
                }
            }
            else
            {
                foreach (var item in memList)
                {
                    item.IsDel = 0;
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 后台删除某人公司信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge DeletePersonCompanyInfo(UpdateMember_info para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var memCom = _JointOfficeContext.Member_Info_Company.Where(t => t.Id == para.id).FirstOrDefault();
            if (memCom != null)
            {
                var otherMemCom = _JointOfficeContext.Member_Info_Company.Where(t => t.MemberId == memCom.MemberId && t.Id != para.id).OrderBy(t => t.Num).ToList();
                if (otherMemCom.Count == 0)
                {
                    throw new BusinessTureException("当前用户只有一条公司信息，不允许删除！");
                }
                var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memCom.MemberId).FirstOrDefault();
                //主公司
                if (memCom.Num == 0)
                {
                    var memCom1 = otherMemCom.Where(t => t.Num == 1).FirstOrDefault();
                    memInfo.ZhuBuMen = memCom1.DeptId;
                    //var job = _JointOfficeContext.Member_Job.Where(t => t.Id == memCom1.JobId).FirstOrDefault();
                    memInfo.JobID = memCom1.JobId;
                    //memInfo.JobName = job.Name;
                }
                var num = 0;
                foreach (var item in otherMemCom)
                {
                    item.Num = num;
                    num++;
                }
                var haveCom = _JointOfficeContext.Member_Info_Company.Where(t => t.MemberId == memCom.MemberId && t.CompanyId == memCom.CompanyId).ToList();
                //如果Member_Info_Company表里只有一条数据，代表这个人在这个公司里只有一个职位，就要对Member_Info表的CompanyIDS字段进行修改
                //删除CompanyIDS字段里此公司id
                if (haveCom.Count == 1)
                {
                    memInfo.CompanyIDS = memInfo.CompanyIDS.Replace(memCom.CompanyId, "");
                    if (memInfo.CompanyIDS.Substring(0, 1) == ",")
                    {
                        memInfo.CompanyIDS = memInfo.CompanyIDS.Remove(0, 1);
                    }
                    else if (memInfo.CompanyIDS.Substring(memInfo.CompanyIDS.Length - 1, 1) == ",")
                    {
                        memInfo.CompanyIDS = memInfo.CompanyIDS.Remove(memInfo.CompanyIDS.Length - 1, 1);
                    }
                    else
                    {
                        memInfo.CompanyIDS = memInfo.CompanyIDS.Replace(",,", ",");
                    }
                }
                _JointOfficeContext.Member_Info_Company.Remove(memCom);
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("删除成功");
        }
        /// <summary>
        /// 后台新建+编辑职务
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge AddJobBack(AddDeptBackInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            if (string.IsNullOrEmpty(para.id))
            {
                Member_Job Member_Job = new Member_Job();
                Member_Job.Id = Guid.NewGuid().ToString();
                var name = "";
                var comObj = GetParentCompany(para.parentId, name);
                if (string.IsNullOrEmpty(comObj.id))
                {
                    throw new BusinessTureException("id为空，新建失败");
                }
                Member_Job.Name = para.name;
                Member_Job.Code = "";
                Member_Job.MemberCompanyId = comObj.id;
                Member_Job.MemberDeptId = para.parentId;
                _JointOfficeContext.Member_Job.Add(Member_Job);
            }
            else
            {
                var job = _JointOfficeContext.Member_Job.Where(t => t.Id == para.id).FirstOrDefault();
                if (job != null)
                {
                    job.Name = para.name;
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
        /// <summary>
        /// 后台新建+编辑职务  递归
        /// </summary>
        /// <returns></returns>
        public CompanyList GetParentCompany(string id, string name)
        {
            CompanyList CompanyList = new CompanyList();
            var com = _JointOfficeContext.Member_Company.Where(t => t.Id == id).FirstOrDefault();
            if (com == null)
            {
                CompanyList.id = "";
                CompanyList.name = "此信息不可用";
                return CompanyList;
            }
            if (name == "")
            {
                name = com.Name;
            }
            else
            {
                name = com.Name + "/" + name;
            }
            if (com.ParentId != "jituan")
            {
                var comObj = GetParentCompany(com.ParentId, name);
                return comObj;
            }
            else
            {
                CompanyList.id = com.Id;
                CompanyList.name = name;
                return CompanyList;
            }
        }
        /// <summary>
        /// 后台根据部门id获取job信息
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<CompanyJobList> GetJobListByDeptId(UpdateMember_info para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<CompanyJobList>();
                return Return.Return();
            }
            Showapi_Res_List<CompanyJobList> res = new Showapi_Res_List<CompanyJobList>();
            List<CompanyJobList> list = new List<CompanyJobList>();
            var jobList = _JointOfficeContext.Member_Job.Where(t => t.MemberDeptId == para.id).ToList();
            foreach (var item in jobList)
            {
                CompanyJobList CompanyJobList = new CompanyJobList();
                CompanyJobList.id = item.Id;
                CompanyJobList.name = item.Name;
                CompanyJobList.nameEx = "";
                var memberids = "";
                var memComInfo = _JointOfficeContext.Member_Info_Company.Where(t => t.JobId == item.Id).ToList();
                foreach (var item1 in memComInfo)
                {
                    if (!memberids.Contains(item1.MemberId))
                    {
                        memberids += item1.MemberId;
                    }
                }
                List<CompanyList> list1 = new List<CompanyList>();
                var sql = @"select MemberId id,name from Member_Info where '" + memberids + @"' like '%'+MemberId+'%'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list1 = conText.Query<CompanyList>(sql).ToList();
                }
                foreach (var item2 in list1)
                {
                    CompanyJobList.nameEx += item2.name + "、";
                }
                if (CompanyJobList.nameEx != "")
                {
                    CompanyJobList.nameEx = CompanyJobList.nameEx.Remove(CompanyJobList.nameEx.LastIndexOf("、"));
                }
                list.Add(CompanyJobList);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<CompanyJobList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 后台删除组织
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge DeleteDeptBack(UpdateMember_info para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var dept = _JointOfficeContext.Member_Company.Where(t => t.ParentId == para.id).FirstOrDefault();
            if (dept != null)
            {
                throw new BusinessTureException("该组织包含子组织，不允许删除.");
            }
            var memComInfo = _JointOfficeContext.Member_Info_Company.Where(t => t.CompanyId == para.id || t.DeptId == para.id).FirstOrDefault();
            if (memComInfo != null)
            {
                throw new BusinessTureException("该组织已关联人员，不允许删除.");
            }
            var memJob = _JointOfficeContext.Member_Job.Where(t => t.MemberCompanyId == para.id || t.MemberDeptId == para.id).FirstOrDefault();
            if (memJob != null)
            {
                throw new BusinessTureException("该组织已关联职务，不允许删除.");
            }
            var deptOne = _JointOfficeContext.Member_Company.Where(t => t.Id == para.id).FirstOrDefault();
            if (deptOne != null)
            {
                _JointOfficeContext.Member_Company.Remove(deptOne);
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("删除成功");
        }
        /// <summary>
        /// 后台删除职务
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge DeleteJobBack(UpdateMember_info para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var memComInfo = _JointOfficeContext.Member_Info_Company.Where(t => t.JobId == para.id).FirstOrDefault();
            if (memComInfo != null)
            {
                throw new BusinessTureException("该职务已关联人员，不允许删除.");
            }
            var job = _JointOfficeContext.Member_Job.Where(t => t.Id == para.id).FirstOrDefault();
            if (job != null)
            {
                _JointOfficeContext.Member_Job.Remove(job);
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("删除成功");
        }
        /// <summary>
        /// 后台新建角色
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge AddRoleBack(GetAllRoleBack para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            if (string.IsNullOrEmpty(para.id))
            {
                Member_Role Member_Role = new Member_Role();
                Member_Role.Id = Guid.NewGuid().ToString();
                Member_Role.Name = para.name;
                Member_Role.Code = para.code;
                Member_Role.Description = para.description;
                Member_Role.IsDel = 0;
                _JointOfficeContext.Member_Role.Add(Member_Role);
            }
            else
            {
                var memRole = _JointOfficeContext.Member_Role.Where(t => t.Id == para.id).FirstOrDefault();
                if (memRole != null)
                {
                    memRole.Name = para.name;
                    memRole.Code = para.code;
                    memRole.Description = para.description;
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("添加成功");
        }
        /// <summary>
        /// 后台获取所有角色
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<GetAllRoleBack> GetAllRoleBack()
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<GetAllRoleBack>();
                return Return.Return();
            }
            Showapi_Res_List<GetAllRoleBack> res = new Showapi_Res_List<GetAllRoleBack>();
            List<GetAllRoleBack> list = new List<GetAllRoleBack>();
            var sql = @"select id,name,code,description from Member_Role";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                list = conText.Query<GetAllRoleBack>(sql).ToList();
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<GetAllRoleBack>();
            res.showapi_res_body.contentlist = list;
            return res;
        }


        ///// <summary>
        ///// 根据部门获取父公司  并写入
        ///// </summary>
        ///// <returns></returns>
        //public Showapi_Res_Meaasge DeptToCompany()
        //{
        //    Message Message = new Message();
        //    var memList = _JointOfficeContext.Member_Info.ToList();
        //    foreach (var item in memList)
        //    {
        //        if (!string.IsNullOrEmpty(item.ZhuBuMen) && item.ZhuBuMen != "zonggongsi")
        //        {
        //            var comId = GetParentCompany(item.ZhuBuMen);
        //            item.CompanyIDS = comId;
        //        }
        //    }
        //    _JointOfficeContext.SaveChanges();
        //    return Message.SuccessMeaasge("修改成功");
        //}
        ///// <summary>
        ///// 同步job
        ///// </summary>
        ///// <returns></returns>
        //public Showapi_Res_Meaasge AddMemberJob()
        //{
        //    Message Message = new Message();
        //    List<AddMemberJobPara> list = new List<AddMemberJobPara>();
        //    var sql = @"select jobname,companyids from Member_Info group by jobname,companyids";
        //    using (SqlConnection conText = new SqlConnection(constr))
        //    {
        //        list = conText.Query<AddMemberJobPara>(sql).ToList();
        //    }
        //    foreach (var item in list)
        //    {
        //        if (!string.IsNullOrEmpty(item.jobname) && !string.IsNullOrEmpty(item.companyids))
        //        {
        //            Member_Job Member_Job = new Member_Job();
        //            Member_Job.Id = Guid.NewGuid().ToString();
        //            Member_Job.Name = item.jobname;
        //            Member_Job.Code = "";
        //            Member_Job.MemberCompanyId = item.companyids;
        //            _JointOfficeContext.Member_Job.Add(Member_Job);
        //        }
        //    }
        //    _JointOfficeContext.SaveChanges();
        //    return Message.SuccessMeaasge("修改成功");
        //}
        ///// <summary>
        ///// 同步jobid
        ///// </summary>
        ///// <returns></returns>
        //public Showapi_Res_Meaasge UpdateMemberJobId()
        //{
        //    Message Message = new Message();
        //    var memList = _JointOfficeContext.Member_Info.ToList();
        //    foreach (var item in memList)
        //    {
        //        var memJob = _JointOfficeContext.Member_Job.Where(t => t.Name == item.JobName && t.MemberCompanyId == item.CompanyIDS).FirstOrDefault();
        //        if (memJob != null)
        //        {
        //            item.JobID = memJob.Id;
        //        }
        //    }
        //    _JointOfficeContext.SaveChanges();
        //    return Message.SuccessMeaasge("修改成功");
        //}
        ///// <summary>
        ///// 修改工作模块表的CompanyId字段
        ///// </summary>
        ///// <returns></returns>
        //public Showapi_Res_Meaasge UpdateWorkCompanyId()
        //{
        //    Message Message = new Message();
        //    var shenpi = _JointOfficeContext.Work_Approval.ToList();
        //    foreach (var item in shenpi)
        //    {
        //        var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
        //        if (memInfo != null && !string.IsNullOrEmpty(memInfo.CompanyIDS))
        //        {
        //            item.CompanyId = memInfo.CompanyIDS;
        //        }
        //    }
        //    var rizhi = _JointOfficeContext.Work_Log.ToList();
        //    foreach (var item in rizhi)
        //    {
        //        var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
        //        if (memInfo != null && !string.IsNullOrEmpty(memInfo.CompanyIDS))
        //        {
        //            item.CompanyId = memInfo.CompanyIDS;
        //        }
        //    }
        //    var renwu = _JointOfficeContext.Work_Task.ToList();
        //    foreach (var item in renwu)
        //    {
        //        var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
        //        if (memInfo != null && !string.IsNullOrEmpty(memInfo.CompanyIDS))
        //        {
        //            item.CompanyId = memInfo.CompanyIDS;
        //        }
        //    }
        //    var richeng = _JointOfficeContext.Work_Program.ToList();
        //    foreach (var item in richeng)
        //    {
        //        var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
        //        if (memInfo != null && !string.IsNullOrEmpty(memInfo.CompanyIDS))
        //        {
        //            item.CompanyId = memInfo.CompanyIDS;
        //        }
        //    }
        //    var zhiling = _JointOfficeContext.Work_Order.ToList();
        //    foreach (var item in zhiling)
        //    {
        //        var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
        //        if (memInfo != null && !string.IsNullOrEmpty(memInfo.CompanyIDS))
        //        {
        //            item.CompanyId = memInfo.CompanyIDS;
        //        }
        //    }
        //    var gonggao = _JointOfficeContext.Work_Announcement.ToList();
        //    foreach (var item in gonggao)
        //    {
        //        var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
        //        if (memInfo != null && !string.IsNullOrEmpty(memInfo.CompanyIDS))
        //        {
        //            item.CompanyId = memInfo.CompanyIDS;
        //        }
        //    }
        //    var fenxiang = _JointOfficeContext.Work_Share.ToList();
        //    foreach (var item in fenxiang)
        //    {
        //        var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
        //        if (memInfo != null && !string.IsNullOrEmpty(memInfo.CompanyIDS))
        //        {
        //            item.CompanyId = memInfo.CompanyIDS;
        //        }
        //    }
        //    _JointOfficeContext.SaveChanges();
        //    return Message.SuccessMeaasge("修改成功");
        //}
        ///// <summary>
        ///// 同步Member_Info_Company表数据
        ///// </summary>
        ///// <returns></returns>
        //public Showapi_Res_Meaasge UpdateMemberInfoCompany()
        //{
        //    Message Message = new Message();
        //    var memInfo = _JointOfficeContext.Member_Info.ToList();
        //    foreach (var item in memInfo)
        //    {
        //        if (!string.IsNullOrEmpty(item.CompanyIDS))
        //        {
        //            Member_Info_Company Member_Info_Company = new Member_Info_Company();
        //            Member_Info_Company.Id = Guid.NewGuid().ToString();
        //            Member_Info_Company.MemberId = item.MemberId;
        //            Member_Info_Company.CompanyId = item.CompanyIDS;
        //            Member_Info_Company.DeptId = item.ZhuBuMen;
        //            Member_Info_Company.JobId = item.JobID;
        //            Member_Info_Company.DeptMemberId = "";
        //            Member_Info_Company.HuiBaoDuiXiangId = "";
        //            Member_Info_Company.GongZuoJieShao = "";
        //            Member_Info_Company.Num = 0;
        //            _JointOfficeContext.Member_Info_Company.Add(Member_Info_Company);
        //        }
        //    }
        //    _JointOfficeContext.SaveChanges();
        //    return Message.SuccessMeaasge("修改成功");
        //}
        ///// <summary>
        ///// 同步正式库job表的MemberDeptId字段
        ///// </summary>
        ///// <returns></returns>
        //public Showapi_Res_Meaasge UpdateJobDeptId()
        //{
        //    Message Message = new Message();
        //    var job = _JointOfficeContext.Member_Job.ToList();
        //    foreach (var item in job)
        //    {
        //        var mem = _JointOfficeContext.Member_Info.Where(t => t.JobID == item.Id).FirstOrDefault();
        //        if (mem != null)
        //        {
        //            item.MemberDeptId = mem.ZhuBuMen;
        //        }
        //    }
        //    _JointOfficeContext.SaveChanges();
        //    return Message.SuccessMeaasge("修改成功");
        //}
    }
}
