using Dapper;
using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public class BWork : IWork
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        private readonly IPrincipalBase _PrincipalBase;
        IMemoryCache _memoryCache;
        public BWork(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase, IMemoryCache memoryCache)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// 待审批
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<DaiShenPi> GetDaiShenPiList(GetDaiDianPingDeRiZhiListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<DaiShenPi>();
                return Return.Return();
            }
            Showapi_Res_List<DaiShenPi> res = new Showapi_Res_List<DaiShenPi>();
            List<DaiShenPi> list = new List<DaiShenPi>();
            var approval_Content = _JointOfficeContext.Approval_Content.Where(t => t.MemberId == para.memberid && t.OtherMemberId == memberid && t.IsMeApproval == "1").OrderByDescending(t => t.CreateDate).ToList();
            if (approval_Content != null && approval_Content.Count != 0)
            {
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                foreach (var item in approval_Content)
                {
                    DaiShenPi DaiShenPi = new DaiShenPi();
                    var workApproval = _JointOfficeContext.Work_Approval.Where(t => t.Id == item.UId).FirstOrDefault();
                    DaiShenPi = WorkDetails.GetDaiShenPiOne(workApproval, memberid);
                    list.Add(DaiShenPi);
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<DaiShenPi>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 待点评的日志
        /// </summary>
        /// <param name="页数，页大小"></param>
        /// <returns></returns>
        public Showapi_Res_List<DaiDianPingDeRiZhi> GetDaiDianPingDeRiZhiList(GetDaiDianPingDeRiZhiListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<DaiDianPingDeRiZhi>();
                return Return.Return();
            }
            Showapi_Res_List<DaiDianPingDeRiZhi> res = new Showapi_Res_List<DaiDianPingDeRiZhi>();
            List<DaiDianPingDeRiZhi> list = new List<DaiDianPingDeRiZhi>();
            var Work_LogList = _JointOfficeContext.Work_Log.Where(t => t.MemberId == para.memberid && t.ReviewPersonId == memberid && t.State == 0).OrderByDescending(t => t.CreateDate).ToList();
            if (Work_LogList != null && Work_LogList.Count != 0)
            {
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                foreach (var item in Work_LogList)
                {
                    DaiDianPingDeRiZhi DaiDianPingDeRiZhi = new DaiDianPingDeRiZhi();
                    DaiDianPingDeRiZhi = WorkDetails.GetDaiDianPingDeRiZhiOne(item, memberid);
                    list.Add(DaiDianPingDeRiZhi);
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<DaiDianPingDeRiZhi>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 待执行的任务
        /// </summary>
        /// <param name="页数，页大小"></param>
        /// <returns></returns>
        public Showapi_Res_List<DaiZhiXingDeRenWu> GetDaiZhiXingDeRenWuList(GetDaiDianPingDeRiZhiListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<DaiZhiXingDeRenWu>();
                return Return.Return();
            }
            Showapi_Res_List<DaiZhiXingDeRenWu> res = new Showapi_Res_List<DaiZhiXingDeRenWu>();
            List<DaiZhiXingDeRenWu> list = new List<DaiZhiXingDeRenWu>();
            var execute_Content = _JointOfficeContext.Execute_Content.Where(t => t.MemberId == para.memberid && t.OtherMemberId == memberid && t.Type == 3 && t.State == 0).OrderByDescending(t => t.CreateDate).ToList();
            if (execute_Content != null && execute_Content.Count != 0)
            {
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                foreach (var item in execute_Content)
                {
                    DaiZhiXingDeRenWu DaiZhiXingDeRenWu = new DaiZhiXingDeRenWu();
                    var workTask = _JointOfficeContext.Work_Task.Where(t => t.Id == item.UId).FirstOrDefault();
                    DaiZhiXingDeRenWu = WorkDetails.GetDaiZhiXingDeRenWuOne(workTask, memberid);
                    list.Add(DaiZhiXingDeRenWu);
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<DaiZhiXingDeRenWu>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 待执行的指令
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<ZhiLingDetail> GetZhiLingDetailList(GetDaiDianPingDeRiZhiListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<ZhiLingDetail>();
                return Return.Return();
            }
            Showapi_Res_List<ZhiLingDetail> res = new Showapi_Res_List<ZhiLingDetail>();
            List<ZhiLingDetail> list = new List<ZhiLingDetail>();
            var execute_Content = _JointOfficeContext.Execute_Content.Where(t => t.MemberId == para.memberid && t.OtherMemberId == memberid && t.Type == 5 && t.State == 0).OrderByDescending(t => t.CreateDate).ToList();
            if (execute_Content != null && execute_Content.Count != 0)
            {
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                foreach (var item in execute_Content)
                {
                    ZhiLingDetail ZhiLingDetail = new ZhiLingDetail();
                    var workOrder = _JointOfficeContext.Work_Order.Where(t => t.Id == item.UId).FirstOrDefault();
                    ZhiLingDetail = WorkDetails.GetZhiLingDetailOne(workOrder, memberid);
                    list.Add(ZhiLingDetail);
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<ZhiLingDetail>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 待回执的公告
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<GongGaoDetail> GetGongGaoDetailList(GetDaiDianPingDeRiZhiListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<GongGaoDetail>();
                return Return.Return();
            }
            Showapi_Res_List<GongGaoDetail> res = new Showapi_Res_List<GongGaoDetail>();
            List<GongGaoDetail> list = new List<GongGaoDetail>();
            var receipts = _JointOfficeContext.Receipts.Where(t => t.MemberId == memberid && (t.Body == null || t.Body == "") && t.Type == 8).OrderByDescending(t => t.CreateTime).Select(t => t.UId).ToList();
            if (receipts != null && receipts.Count != 0)
            {
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                foreach (var item in receipts)
                {
                    GongGaoDetail GongGaoDetail = new GongGaoDetail();
                    var workAnnouncement = _JointOfficeContext.Work_Announcement.Where(t => t.Id == item).FirstOrDefault();
                    GongGaoDetail = WorkDetails.GetGongGaoDetailOne(workAnnouncement, memberid);
                    list.Add(GongGaoDetail);
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<GongGaoDetail>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 待回执的分享
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<FenXiangDetail> GetFenXiangDetailList(GetDaiDianPingDeRiZhiListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<FenXiangDetail>();
                return Return.Return();
            }
            Showapi_Res_List<FenXiangDetail> res = new Showapi_Res_List<FenXiangDetail>();
            List<FenXiangDetail> list = new List<FenXiangDetail>();
            var receipts = _JointOfficeContext.Receipts.Where(t => t.MemberId == memberid && (t.Body == null || t.Body == "") && t.Type == 9).OrderByDescending(t => t.CreateTime).Select(t => t.UId).ToList();
            if (receipts != null && receipts.Count != 0)
            {
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                foreach (var item in receipts)
                {
                    FenXiangDetail FenXiangDetail = new FenXiangDetail();
                    var workShare = _JointOfficeContext.Work_Share.Where(t => t.Id == item).FirstOrDefault();
                    FenXiangDetail = WorkDetails.GetFenXiangDetailOne(workShare, memberid);
                    list.Add(FenXiangDetail);
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<FenXiangDetail>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 个人动态主页
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<PersonDynamic_info> GetPersonDynamic_infoList(GetPersonDynamic_infoListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<PersonDynamic_info>();
                return Return.Return();
            }
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            List<PersonDynamic_info> list = new List<PersonDynamic_info>();
            var sql = "";
            var sql1 = "";
            int allPages = 0;
            int allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            if (memberid == para.memberid)
            {
                if (string.IsNullOrEmpty(para.companyId))
                {
                    sql = @"exec MeZhuYe '" + memberid + "'," + begin + "," + end + ",'" + para.type + "'";
                    sql1 = @"exec MeZhuYeCount '" + memberid + "','" + para.type + "'";
                }
                else
                {
                    sql = @"exec MeZhuYeCompany '" + memberid + "'," + begin + "," + end + ",'" + para.type + "','" + para.companyId + "'";
                    sql1 = @"exec MeZhuYeCompanyCount '" + memberid + "','" + para.type + "','" + para.companyId + "'";
                }
            }
            else
            {
                var Companyid = _JointOfficeContext.Member_Company.Where(t => t.ParentId == "0").FirstOrDefault().Id;
                var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == para.memberid).FirstOrDefault();
                sql = @"exec GeRenZhuYe '" + para.memberid + "','" + memberid + "','" + info.ZhuBuMen + "','" + Companyid + "'," + begin + "," + end + ",'" + para.type + "'";
                sql1 = @"exec GeRenZhuYeCount '" + para.memberid + "','" + memberid + "','" + info.ZhuBuMen + "','" + Companyid + "','" + para.type + "'";
            }
            using (SqlConnection conText = new SqlConnection(constr))
            {
                list = conText.Query<PersonDynamic_info>(sql).ToList();
                allNum = conText.Query<int>(sql1).FirstOrDefault();
                allPages = allNum / para.count;
                if (allNum % para.count != 0)
                {
                    allPages += 1;
                }
            }
            //大类  list  处理
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            list = WorkDetails.GetPersonDynamic_info(list);
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<PersonDynamic_info>();
            res.showapi_res_body.allPages = allPages;
            res.showapi_res_body.allNum = allNum;
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 部门信息中个人动态主页
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<PersonDynamic_info> GetDept_PersonDynamic_infoList(GetPersonDynamic_infoListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<PersonDynamic_info>();
                return Return.Return();
            }
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            List<PersonDynamic_info> list = new List<PersonDynamic_info>();
            var sql = "";
            var sql1 = "";
            int allPages = 0;
            int allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            if (para.type == "1")
            {
                var memList = _JointOfficeContext.Member_Info_Company.Where(t => t.DeptId == para.memberid).ToList();
                var memListStr = "";
                foreach (var item in memList)
                {
                    memListStr += item.MemberId;
                }
                //sql = @"exec BuMenZhuYeFaChu '" + para.memberid + "','" + memberid + "'," + begin + "," + end;
                //sql1 = @"exec BuMenZhuYeFaChuCount '" + para.memberid + "','" + memberid + "'";
                sql = @"exec BuMenZhuYeFaChu '" + memListStr + "','" + memberid + "'," + begin + "," + end;
                sql1 = @"exec BuMenZhuYeFaChuCount '" + memListStr + "','" + memberid + "'";
            }
            if (para.type == "2")
            {
                sql = @"exec BuMenZhuYeShouDao '" + para.memberid + "','" + memberid + "'," + begin + "," + end;
                sql1 = @"exec BuMenZhuYeShouDaoCount '" + para.memberid + "','" + memberid + "'";
            }
            using (SqlConnection conText = new SqlConnection(constr))
            {
                list = conText.Query<PersonDynamic_info>(sql).ToList();
                allNum = conText.Query<int>(sql1).FirstOrDefault();
                allPages = allNum / para.count;
                if (allNum % para.count != 0)
                {
                    allPages += 1;
                }
            }
            //大类  list  处理
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            list = WorkDetails.GetPersonDynamic_info(list);
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<PersonDynamic_info>();
            res.showapi_res_body.allPages = allPages;
            res.showapi_res_body.allNum = allNum;
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 工作回复  回复我的
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<WorkReply> GetWorkReplyList(GetReplyListPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<WorkReply>();
                return Return.Return();
            }
            Showapi_Res_List<WorkReply> res = new Showapi_Res_List<WorkReply>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            List<WorkReply> list = new List<WorkReply>();
            List<string> idList = new List<string>();
            var allPage = 0;
            var sql = "";
            if (para.memberid != null && para.memberid != "")
            {
                if (para.memberid == memberid)
                {
                    sql = @"exec GongZuoHuiFuMeToOther '" + memberid + "'";
                }
                else
                {
                    sql = @"exec GongZuoHuiFuOtherToMe '" + memberid + "','" + para.memberid + "'";
                }
            }
            else
            {
                var sql1 = "";
                if (string.IsNullOrEmpty(para.companyId))
                {
                    sql = @"exec ReplyMe '" + memberid + "'";
                    sql1 = @"exec ReplyMeIsRead '" + memberid + "'";
                }
                else
                {
                    sql = @"exec ReplyMeCompany '" + memberid + "','" + para.companyId + "'";
                    sql1 = @"exec ReplyMeIsReadCompany '" + memberid + "','" + para.companyId + "'";
                }
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    var isReadList = conText.Query<ReplyMeIsRead>(sql1).Where(t => t.isRead == false).ToList();
                    foreach (var item in isReadList)
                    {
                        if (item.pingtype == "ping")
                        {
                            var com = _JointOfficeContext.Comment_Body.Where(t => t.Id == item.id).FirstOrDefault();
                            com.IsRead = true;
                        }
                        if (item.pingtype == "dian")
                        {
                            var com = _JointOfficeContext.DianPing_Body.Where(t => t.Id == item.id).FirstOrDefault();
                            com.IsRead = true;
                        }
                    }
                    _JointOfficeContext.SaveChanges();
                }
            }
            using (SqlConnection conText = new SqlConnection(constr))
            {
                idList = conText.Query<string>(sql).ToList();
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
                List<WorkReply> list11 = new List<WorkReply>();
                list11 = WorkDetails.GetWorkReply(item, memberid);
                list.AddRange(list11);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<WorkReply>();
            res.showapi_res_body.allPages = allPage;
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 创建审批
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge CreateWorkApproval(Work_Approval para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            para.Id = Guid.NewGuid().ToString();
            para.MemberId = memberid;
            para.CreateDate = DateTime.Now;
            para.ApprovalPersonNum = 1;

            string[] s1 = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            Random num = new Random();
            int codeNum = num.Next(1, 99999);
            var codeABC = s1[num.Next(0, s1.Length)];
            var code = "";
            switch (codeNum.ToString().Length)
            {
                case 1:
                    code = codeABC + "0000" + codeNum;
                    break;
                case 2:
                    code = codeABC + "000" + codeNum;
                    break;
                case 3:
                    code = codeABC + "00" + codeNum;
                    break;
                case 4:
                    code = codeABC + "0" + codeNum;
                    break;
                case 5:
                    code = codeABC + codeNum;
                    break;
            }
            para.Code = "NO." + code;

            if (para.ApprovalPerson != null && para.ApprovalPerson != "")
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ApprovalPeoPleInfo>>(para.ApprovalPerson);
                foreach (var item in list)
                {
                    Approval_Content Approval_Content = new Approval_Content();
                    Approval_Content.Id = Guid.NewGuid().ToString();
                    Approval_Content.MemberId = memberid;
                    Approval_Content.UId = para.Id;
                    Approval_Content.OtherMemberId = item.id;
                    var OtherMember = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.id).FirstOrDefault();
                    Approval_Content.OtherMemberName = OtherMember.Name;
                    Approval_Content.OtherMemberPicture = OtherMember.Picture;
                    Approval_Content.OtherMemberOrder = Convert.ToInt32(item.order);
                    if (item.order == "1")
                    {
                        Approval_Content.IsMeApproval = "1";
                        Approval_Content.State = 0;
                    }
                    else
                    {
                        Approval_Content.IsMeApproval = "0";
                        Approval_Content.State = 5;
                    }
                    Approval_Content.Type = para.Type;
                    Approval_Content.CreateDate = DateTime.Now;
                    Approval_Content.Content = "";
                    Approval_Content.Picture = "";
                    Approval_Content.ApprovalTime = "";
                    Approval_Content.PhoneModel = "";
                    _JointOfficeContext.Approval_Content.Add(Approval_Content);
                }
            }
            _JointOfficeContext.Work_Approval.Add(para);
            TotalNum TotalNum = new TotalNum();
            TotalNum.Id = Guid.NewGuid().ToString();
            TotalNum.Type = "1";
            TotalNum.UId = para.Id;
            TotalNum.PId = "";
            TotalNum.P_UId = "";
            TotalNum.DianZanNum = 0;
            TotalNum.ZhuanFaNum = 0;
            TotalNum.PingLunNum = 0;
            TotalNum.CreateTime = DateTime.Now;
            _JointOfficeContext.TotalNum.Add(TotalNum);
            _JointOfficeContext.SaveChanges();
            SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
            var parems = SendHelper.getXiaoXiParams(para.Id, "1");
            var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ApprovalPeoPleInfo>>(para.ApprovalPerson);
            var item1 = list1.Where(t => t.order == "1").FirstOrDefault();
            if (item1 != null)
            {
                SendHelper.SendXiaoXi("待审批的审批", item1.id, parems);
            }
            return Message.SuccessMeaasge("创建成功");
        }
        /// <summary>
        /// 创建日志
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge CreateWorkLog(Work_Log para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            para.Id = Guid.NewGuid().ToString();
            para.MemberId = memberid;
            para.CreateDate = DateTime.Now;
            if (para.Receipt != null && para.Receipt != "")
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(para.Receipt);
                List<string> memberInfo = new List<string>();
                foreach (var item1 in list)
                {
                    if (item1.type == "1")
                    {
                        var member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item1.id).Select(t => t.MemberId).ToList();
                        memberInfo.AddRange(member_Info);
                    }
                    if (item1.type == "2")
                    {
                        WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                        memberInfo = WorkDetails.GetCompanyPersonList(memberInfo, item1.id);
                    }
                    if (item1.type == "3")
                    {
                        var member_Info1 = _JointOfficeContext.Member_Group.Where(t => t.Id == item1.id).FirstOrDefault();
                        var list2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo2>>(member_Info1.GroupPersonId);
                        List<string> member_Info = new List<string>();
                        foreach (var item in list2)
                        {
                            var exeMember = item.memberid;
                            member_Info.Add(exeMember);
                        }
                        memberInfo.AddRange(member_Info);
                    }
                }
                var memberInfoset = new HashSet<string>(memberInfo);
                foreach (var item in memberInfoset)
                {
                    Receipts Receipts = new Receipts();
                    Receipts.Id = Guid.NewGuid().ToString();
                    Receipts.MemberId = item;
                    Receipts.OtherMemberId = memberid;
                    Receipts.Type = 2;
                    Receipts.UId = para.Id;
                    Receipts.CreateTime = DateTime.Now;
                    _JointOfficeContext.Receipts.Add(Receipts);
                }
            }
            _JointOfficeContext.Work_Log.Add(para);
            //为此条日志  添加 统计
            TotalNum TotalNum = new TotalNum();
            TotalNum.Id = Guid.NewGuid().ToString();
            TotalNum.Type = "2";
            TotalNum.UId = para.Id;
            TotalNum.PId = "";
            TotalNum.P_UId = "";
            TotalNum.DianZanNum = 0;
            TotalNum.ZhuanFaNum = 0;
            TotalNum.PingLunNum = 0;
            TotalNum.CreateTime = DateTime.Now;
            _JointOfficeContext.TotalNum.Add(TotalNum);
            _JointOfficeContext.SaveChanges();
            SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
            var parems = SendHelper.getXiaoXiParams(para.Id, "2");
            SendHelper.SendXiaoXi("待点评的日志", para.ReviewPersonId, parems);
            return Message.SuccessMeaasge("创建成功");
        }
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge CreateWorkTask(Work_Task para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            para.Id = Guid.NewGuid().ToString();
            para.MemberId = memberid;
            para.CreateDate = DateTime.Now;
            if (para.Executor != null && para.Executor != "")
            {
                //获取执行人memberid
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(para.Executor);
                List<string> memberInfo = new List<string>();
                foreach (var item1 in list)
                {
                    if (item1.type == "1")
                    {
                        var member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item1.id).Select(t => t.MemberId).ToList();
                        memberInfo.AddRange(member_Info);
                    }
                    if (item1.type == "2")
                    {
                        WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                        memberInfo = WorkDetails.GetCompanyPersonList(memberInfo, item1.id);
                    }
                    if (item1.type == "3")
                    {
                        var member_Info1 = _JointOfficeContext.Member_Group.Where(t => t.Id == item1.id).FirstOrDefault();
                        var list2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo2>>(member_Info1.GroupPersonId);
                        List<string> member_Info = new List<string>();
                        foreach (var item in list2)
                        {
                            var exeMember = item.memberid;
                            member_Info.Add(exeMember);
                        }
                        memberInfo.AddRange(member_Info);
                    }
                }
                var memberInfoset = new HashSet<string>(memberInfo);
                foreach (var item in memberInfoset)
                {
                    Execute_Content Execute_Content = new Execute_Content();
                    Execute_Content.Id = Guid.NewGuid().ToString();
                    Execute_Content.MemberId = memberid;
                    Execute_Content.Type = 3;
                    Execute_Content.UId = para.Id;
                    Execute_Content.OtherMemberId = item;
                    var OtherMember = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item).FirstOrDefault();
                    Execute_Content.OtherMemberName = OtherMember.Name;
                    Execute_Content.OtherMemberPicture = OtherMember.Picture;
                    Execute_Content.State = 0;
                    Execute_Content.CreateDate = DateTime.Now;
                    Execute_Content.Content = "";
                    Execute_Content.ExecuteDate = "";
                    Execute_Content.PhoneModel = "";
                    _JointOfficeContext.Execute_Content.Add(Execute_Content);
                }
            }
            _JointOfficeContext.Work_Task.Add(para);
            TotalNum TotalNum = new TotalNum();
            TotalNum.Id = Guid.NewGuid().ToString();
            TotalNum.Type = "3";
            TotalNum.UId = para.Id;
            TotalNum.PId = "";
            TotalNum.P_UId = "";
            TotalNum.DianZanNum = 0;
            TotalNum.ZhuanFaNum = 0;
            TotalNum.PingLunNum = 0;
            TotalNum.CreateTime = DateTime.Now;
            _JointOfficeContext.TotalNum.Add(TotalNum);
            _JointOfficeContext.SaveChanges();
            SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
            var parems = SendHelper.getXiaoXiParams(para.Id, "3");
            var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(para.Executor);
            foreach (var item in list1)
            {
                SendHelper.SendXiaoXi("待执行的任务", item.id, parems);
            }
            return Message.SuccessMeaasge("创建成功");
        }
        /// <summary>
        /// 创建日程
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge CreateWorkProgram(Work_Program para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            para.Id = Guid.NewGuid().ToString();
            para.MemberId = memberid;
            para.CreateDate = DateTime.Now;
            if (para.Receipt != null && para.Receipt != "")
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(para.Receipt);
                List<string> memberInfo = new List<string>();
                foreach (var item1 in list)
                {
                    if (item1.type == "1")
                    {
                        var member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item1.id).Select(t => t.MemberId).ToList();
                        memberInfo.AddRange(member_Info);
                    }
                    if (item1.type == "2")
                    {
                        WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                        memberInfo = WorkDetails.GetCompanyPersonList(memberInfo, item1.id);
                    }
                    if (item1.type == "3")
                    {
                        var member_Info1 = _JointOfficeContext.Member_Group.Where(t => t.Id == item1.id).FirstOrDefault();
                        var list2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo2>>(member_Info1.GroupPersonId);
                        List<string> member_Info = new List<string>();
                        foreach (var item in list2)
                        {
                            var exeMember = item.memberid;
                            member_Info.Add(exeMember);
                        }
                        memberInfo.AddRange(member_Info);
                    }
                }
                var memberInfoset = new HashSet<string>(memberInfo);
                foreach (var item in memberInfoset)
                {
                    Receipts Receipts = new Receipts();
                    Receipts.Id = Guid.NewGuid().ToString();
                    Receipts.MemberId = item;
                    Receipts.OtherMemberId = memberid;
                    Receipts.Type = 4;
                    Receipts.UId = para.Id;
                    Receipts.CreateTime = DateTime.Now;
                    _JointOfficeContext.Receipts.Add(Receipts);
                }
            }
            _JointOfficeContext.Work_Program.Add(para);
            TotalNum TotalNum = new TotalNum();
            TotalNum.Id = Guid.NewGuid().ToString();
            TotalNum.Type = "4";
            TotalNum.UId = para.Id;
            TotalNum.PId = "";
            TotalNum.P_UId = "";
            TotalNum.DianZanNum = 0;
            TotalNum.ZhuanFaNum = 0;
            TotalNum.PingLunNum = 0;
            TotalNum.CreateTime = DateTime.Now;
            _JointOfficeContext.TotalNum.Add(TotalNum);
            _JointOfficeContext.SaveChanges();
            SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
            var parems = SendHelper.getXiaoXiParams(para.Id, "4");
            var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(para.JoinPerson);
            foreach (var item in list1)
            {
                SendHelper.SendXiaoXi("待参与的日程", item.id, parems);
            }
            return Message.SuccessMeaasge("创建成功");
        }
        /// <summary>
        /// 创建指令
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge CreateWorkOrder(Work_Order para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
            if (para.Executor != null && para.Executor != "")
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(para.Executor);
                foreach (var item in list)
                {
                    var str = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                    Work_Order Work_Order = new Work_Order();
                    Work_Order.Id = Guid.NewGuid().ToString();
                    Work_Order.MemberId = memberid;
                    Work_Order.Body = para.Body;
                    Work_Order.StopTime = para.StopTime;
                    Work_Order.Range = para.Range;
                    Work_Order.Map = para.Map;
                    Work_Order.Address = para.Address;
                    Work_Order.IsDraft = para.IsDraft;
                    Work_Order.CreateDate = DateTime.Now;
                    Work_Order.VoiceLength = para.VoiceLength;
                    Work_Order.Voice = para.Voice;
                    Work_Order.Annex = para.Annex;
                    Work_Order.Picture = para.Picture;
                    Work_Order.WangPanJson = para.WangPanJson;
                    Work_Order.Executor = "[" + str + "]";
                    Work_Order.ATPerson = para.ATPerson;
                    Work_Order.State = para.State;
                    Work_Order.PhoneModel = para.PhoneModel;
                    Work_Order.CompanyId = para.CompanyId;
                    _JointOfficeContext.Work_Order.Add(Work_Order);

                    Execute_Content Execute_Content = new Execute_Content();
                    Execute_Content.Id = Guid.NewGuid().ToString();
                    Execute_Content.MemberId = memberid;
                    Execute_Content.Type = 5;
                    Execute_Content.UId = Work_Order.Id;
                    Execute_Content.OtherMemberId = item.id;
                    var OtherMember = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.id).FirstOrDefault();
                    Execute_Content.OtherMemberName = OtherMember.Name;
                    Execute_Content.OtherMemberPicture = OtherMember.Picture;
                    Execute_Content.State = 0;
                    Execute_Content.CreateDate = DateTime.Now;
                    Execute_Content.Content = "";
                    Execute_Content.ExecuteDate = "";
                    Execute_Content.PhoneModel = "";
                    _JointOfficeContext.Execute_Content.Add(Execute_Content);

                    TotalNum TotalNum = new TotalNum();
                    TotalNum.Id = Guid.NewGuid().ToString();
                    TotalNum.Type = "5";
                    TotalNum.UId = Work_Order.Id;
                    TotalNum.PId = "";
                    TotalNum.P_UId = "";
                    TotalNum.DianZanNum = 0;
                    TotalNum.ZhuanFaNum = 0;
                    TotalNum.PingLunNum = 0;
                    TotalNum.CreateTime = DateTime.Now;
                    _JointOfficeContext.TotalNum.Add(TotalNum);
                    var parems = SendHelper.getXiaoXiParams(Work_Order.Id, "5");

                    SendHelper.SendXiaoXi("待执行的指令", item.id, parems);
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("创建成功");
        }
        /// <summary>
        /// 创建公告
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge CreateWorkAnnouncement(Work_Announcement para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            para.Id = Guid.NewGuid().ToString();
            para.MemberId = memberid;
            para.CreateDate = DateTime.Now;
            if (para.Receipt != null && para.Receipt != "")
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(para.Receipt);
                List<string> memberInfo = new List<string>();
                foreach (var item1 in list)
                {
                    if (item1.type == "1")
                    {
                        var member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item1.id).Select(t => t.MemberId).ToList();
                        memberInfo.AddRange(member_Info);
                    }
                    if (item1.type == "2")
                    {
                        WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                        memberInfo = WorkDetails.GetCompanyPersonList(memberInfo, item1.id);
                    }
                    if (item1.type == "3")
                    {
                        var member_Info1 = _JointOfficeContext.Member_Group.Where(t => t.Id == item1.id).FirstOrDefault();
                        var list2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo2>>(member_Info1.GroupPersonId);
                        List<string> member_Info = new List<string>();
                        foreach (var item in list2)
                        {
                            var exeMember = item.memberid;
                            member_Info.Add(exeMember);
                        }
                        memberInfo.AddRange(member_Info);
                    }
                }
                var memberInfoset = new HashSet<string>(memberInfo);
                foreach (var item in memberInfoset)
                {
                    Receipts Receipts = new Receipts();
                    Receipts.Id = Guid.NewGuid().ToString();
                    Receipts.MemberId = item;
                    Receipts.OtherMemberId = memberid;
                    Receipts.Type = 8;
                    Receipts.UId = para.Id;
                    Receipts.CreateTime = DateTime.Now;
                    _JointOfficeContext.Receipts.Add(Receipts);
                }
            }
            _JointOfficeContext.Work_Announcement.Add(para);
            TotalNum TotalNum = new TotalNum();
            TotalNum.Id = Guid.NewGuid().ToString();
            TotalNum.Type = "8";
            TotalNum.UId = para.Id;
            TotalNum.PId = "";
            TotalNum.P_UId = "";
            TotalNum.DianZanNum = 0;
            TotalNum.ZhuanFaNum = 0;
            TotalNum.PingLunNum = 0;
            TotalNum.CreateTime = DateTime.Now;
            _JointOfficeContext.TotalNum.Add(TotalNum);
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("创建成功");
        }
        /// <summary>
        /// 创建分享
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge CreateWorkShare(Work_Share para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            para.Id = Guid.NewGuid().ToString();
            para.MemberId = memberid;
            para.CreateDate = DateTime.Now;
            if (para.Receipt != null && para.Receipt != "")
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(para.Receipt);
                List<string> memberInfo = new List<string>();
                foreach (var item1 in list)
                {
                    if (item1.type == "1")
                    {
                        var member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item1.id).Select(t => t.MemberId).ToList();
                        memberInfo.AddRange(member_Info);
                    }
                    if (item1.type == "2")
                    {
                        WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                        memberInfo = WorkDetails.GetCompanyPersonList(memberInfo, item1.id);
                    }
                    if (item1.type == "3")
                    {
                        var member_Info1 = _JointOfficeContext.Member_Group.Where(t => t.Id == item1.id).FirstOrDefault();
                        var list2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo2>>(member_Info1.GroupPersonId);
                        List<string> member_Info = new List<string>();
                        foreach (var item in list2)
                        {
                            var exeMember = item.memberid;
                            member_Info.Add(exeMember);
                        }
                        memberInfo.AddRange(member_Info);
                    }
                }
                var memberInfoset = new HashSet<string>(memberInfo);
                foreach (var item in memberInfoset)
                {
                    Receipts Receipts = new Receipts();
                    Receipts.Id = Guid.NewGuid().ToString();
                    Receipts.MemberId = item;
                    Receipts.OtherMemberId = memberid;
                    Receipts.Type = 9;
                    Receipts.UId = para.Id;
                    Receipts.CreateTime = DateTime.Now;
                    _JointOfficeContext.Receipts.Add(Receipts);
                }
            }
            _JointOfficeContext.Work_Share.Add(para);
            TotalNum TotalNum = new TotalNum();
            TotalNum.Id = Guid.NewGuid().ToString();
            TotalNum.Type = "9";
            TotalNum.UId = para.Id;
            TotalNum.PId = "";
            TotalNum.P_UId = "";
            TotalNum.DianZanNum = 0;
            TotalNum.ZhuanFaNum = 0;
            TotalNum.PingLunNum = 0;
            TotalNum.CreateTime = DateTime.Now;
            _JointOfficeContext.TotalNum.Add(TotalNum);
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("创建成功");
        }
        /// <summary>
        /// 审批详情
        /// </summary>
        /// <param name="审批ID"></param>
        /// <returns></returns>
        public Showapi_Res_Single<DaiShenPi> GetShenPiDetail(DetailID para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<DaiShenPi>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<DaiShenPi> res = new Showapi_Res_Single<DaiShenPi>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var workApproval = _JointOfficeContext.Work_Approval.Where(t => t.Id == para.id).FirstOrDefault();
            var DaiShenPi = WorkDetails.GetDaiShenPiOne(workApproval, memberid);
            res.showapi_res_body = DaiShenPi;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 日志详情
        /// </summary>
        /// <param name="日志ID"></param>
        /// <returns></returns>
        public Showapi_Res_Single<DaiDianPingDeRiZhi> GetRiZhiDetail(DetailID para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<DaiDianPingDeRiZhi>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<DaiDianPingDeRiZhi> res = new Showapi_Res_Single<DaiDianPingDeRiZhi>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var workLog = _JointOfficeContext.Work_Log.Where(t => t.Id == para.id).FirstOrDefault();
            var DaiDianPingDeRiZhi = WorkDetails.GetDaiDianPingDeRiZhiOne(workLog, memberid);
            res.showapi_res_body = DaiDianPingDeRiZhi;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 任务详情
        /// </summary>
        /// <param name="任务ID"></param>
        /// <returns></returns>
        public Showapi_Res_Single<DaiZhiXingDeRenWu> GetRenWuDetail(DetailID para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<DaiZhiXingDeRenWu>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<DaiZhiXingDeRenWu> res = new Showapi_Res_Single<DaiZhiXingDeRenWu>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var workTask = _JointOfficeContext.Work_Task.Where(t => t.Id == para.id).FirstOrDefault();
            var DaiZhiXingDeRenWu = WorkDetails.GetDaiZhiXingDeRenWuOne(workTask, memberid);
            res.showapi_res_body = DaiZhiXingDeRenWu;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 日程详情
        /// </summary>
        /// <param name="日程ID"></param>
        /// <returns></returns>
        public Showapi_Res_Single<RiChengDetail> GetRiChengDetail(DetailID para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<RiChengDetail>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<RiChengDetail> res = new Showapi_Res_Single<RiChengDetail>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var workProgram = _JointOfficeContext.Work_Program.Where(t => t.Id == para.id).FirstOrDefault();
            var RiChengDetail = WorkDetails.GetRiChengDetailOne(workProgram, memberid);
            res.showapi_res_body = RiChengDetail;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 指令详情
        /// </summary>
        /// <param name="指令ID"></param>
        /// <returns></returns>
        public Showapi_Res_Single<ZhiLingDetail> GetZhiLingDetail(DetailID para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<ZhiLingDetail>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<ZhiLingDetail> res = new Showapi_Res_Single<ZhiLingDetail>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var workOrder = _JointOfficeContext.Work_Order.Where(t => t.Id == para.id).FirstOrDefault();
            var ZhiLingDetail = WorkDetails.GetZhiLingDetailOne(workOrder, memberid);
            res.showapi_res_body = ZhiLingDetail;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 公告详情
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<GongGaoDetail> GetGongGaoDetail(DetailID para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<GongGaoDetail>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<GongGaoDetail> res = new Showapi_Res_Single<GongGaoDetail>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var workAnnouncement = _JointOfficeContext.Work_Announcement.Where(t => t.Id == para.id).FirstOrDefault();
            var GongGaoDetail = WorkDetails.GetGongGaoDetailOne(workAnnouncement, memberid);
            res.showapi_res_body = GongGaoDetail;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 分享详情
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<FenXiangDetail> GetFenXiangDetail(DetailID para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<FenXiangDetail>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<FenXiangDetail> res = new Showapi_Res_Single<FenXiangDetail>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var workShare = _JointOfficeContext.Work_Share.Where(t => t.Id == para.id).FirstOrDefault();
            var FenXiangDetail = WorkDetails.GetFenXiangDetailOne(workShare, memberid);
            res.showapi_res_body = FenXiangDetail;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 工作列表中每个工作的详情页
        /// </summary>
        public Showapi_Res_Single<AllDetails> GetAllDetails(FocusInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<AllDetails>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<AllDetails> res = new Showapi_Res_Single<AllDetails>();
            AllDetails AllDetails = new AllDetails();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            AllDetails.type = para.type;
            switch (para.type)
            {
                case "1":
                    var workApproval = _JointOfficeContext.Work_Approval.Where(t => t.Id == para.id).FirstOrDefault();
                    AllDetails.daiShenPi = WorkDetails.GetDaiShenPiOne(workApproval, memberid);
                    break;
                case "2":
                    var workLog = _JointOfficeContext.Work_Log.Where(t => t.Id == para.id).FirstOrDefault();
                    AllDetails.daiDianPingDeRiZhi = WorkDetails.GetDaiDianPingDeRiZhiOne(workLog, memberid);
                    break;
                case "3":
                    var workTask = _JointOfficeContext.Work_Task.Where(t => t.Id == para.id).FirstOrDefault();
                    AllDetails.daiZhiXingDeRenWu = WorkDetails.GetDaiZhiXingDeRenWuOne(workTask, memberid);
                    break;
                case "4":
                    var workProgram = _JointOfficeContext.Work_Program.Where(t => t.Id == para.id).FirstOrDefault();
                    AllDetails.riChengDetail = WorkDetails.GetRiChengDetailOne(workProgram, memberid);
                    break;
                case "5":
                    var workOrder = _JointOfficeContext.Work_Order.Where(t => t.Id == para.id).FirstOrDefault();
                    AllDetails.zhiLingDetail = WorkDetails.GetZhiLingDetailOne(workOrder, memberid);
                    break;
                case "8":
                    var workAnnouncement = _JointOfficeContext.Work_Announcement.Where(t => t.Id == para.id).FirstOrDefault();
                    AllDetails.gongGaoDetail = WorkDetails.GetGongGaoDetailOne(workAnnouncement, memberid);
                    break;
                case "9":
                    var workShare = _JointOfficeContext.Work_Share.Where(t => t.Id == para.id).FirstOrDefault();
                    AllDetails.fenXiangDetail = WorkDetails.GetFenXiangDetailOne(workShare, memberid);
                    break;
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = AllDetails;
            return res;
        }
        /// <summary>
        /// 工作附件
        /// </summary>
        /// <param name="人id,工作文档类型"></param>
        /// <returns></returns>
        public Showapi_Res_List<WorkDoc_Annex> GetWorkDoc_AnnexList(WorkDoc_Para para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<WorkDoc_Annex>();
                return Return.Return();
            }
            Showapi_Res_List<WorkDoc_Annex> res = new Showapi_Res_List<WorkDoc_Annex>();
            List<WorkDoc_Annex> list = new List<WorkDoc_Annex>();
            List<PersonDynamic_info> list1 = new List<PersonDynamic_info>();
            int allPages = 0;
            int allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            var sql = "";
            var sql1 = "";
            if (para.memberid == memberid)
            {
                if (string.IsNullOrEmpty(para.companyId))
                {
                    sql = @"exec MeZhuYe '" + memberid + "'," + begin + "," + end + ",'0'";
                    sql1 = @"exec MeZhuYeCount '" + memberid + "','0'";
                }
                else
                {
                    sql = @"exec MeZhuYeCompany '" + memberid + "'," + begin + "," + end + ",'0','" + para.companyId + "'";
                    sql1 = @"exec MeZhuYeCompanyCount '" + memberid + "','0','" + para.companyId + "'";
                }
            }
            else
            {
                var Companyid = _JointOfficeContext.Member_Company.Where(t => t.ParentId == "0").FirstOrDefault().Id;
                var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == para.memberid).FirstOrDefault();
                sql = @"exec GeRenZhuYe '" + para.memberid + "','" + memberid + "','" + info.ZhuBuMen + "','" + Companyid + "'," + begin + "," + end + ",'0 '";
                sql1 = @"exec GeRenZhuYeCount '" + para.memberid + "','" + memberid + "','" + info.ZhuBuMen + "','" + Companyid + "','0'";
            }
            using (SqlConnection conText = new SqlConnection(constr))
            {
                list1 = conText.Query<PersonDynamic_info>(sql).ToList();
                allNum = conText.Query<int>(sql1).FirstOrDefault();
                allPages = allNum / para.count;
                if (allNum % para.count != 0)
                {
                    allPages += 1;
                }
            }
            foreach (var item in list1)
            {
                if (item.annexlist != null && item.annexlist != "")
                {
                    var url = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item.annexlist);
                    foreach (var one in url)
                    {
                        WorkDoc_Annex WorkDoc_Annex = new WorkDoc_Annex();
                        WorkDoc_Annex.annexurl = one.url + SasKey;
                        WorkDoc_Annex.annexName = one.url.Split('/').Last();
                        WorkDoc_Annex.annexTime = item.createDate;
                        WorkDoc_Annex.annexSize = "";
                        list.Add(WorkDoc_Annex);
                    }
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<WorkDoc_Annex>();
            res.showapi_res_body.allPages = allPages;
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 工作图片
        /// </summary>
        /// <param name="人id,工作文档类型"></param>
        /// <returns></returns>
        public Showapi_Res_List<WorkDoc_Picture> GetWorkDoc_PictureList(WorkDoc_Para para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<WorkDoc_Picture>();
                return Return.Return();
            }
            Showapi_Res_List<WorkDoc_Picture> res = new Showapi_Res_List<WorkDoc_Picture>();
            List<WorkDoc_Picture> list = new List<WorkDoc_Picture>();
            List<PersonDynamic_info> list1 = new List<PersonDynamic_info>();
            int allPages = 0;
            int allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            var sql = "";
            var sql1 = "";
            if (para.memberid == memberid)
            {
                if (string.IsNullOrEmpty(para.companyId))
                {
                    sql = @"exec MeZhuYe '" + memberid + "'," + begin + "," + end + ",'0'";
                    sql1 = @"exec MeZhuYeCount '" + memberid + "','0'";
                }
                else
                {
                    sql = @"exec MeZhuYeCompany '" + memberid + "'," + begin + "," + end + ",'0','" + para.companyId + "'";
                    sql1 = @"exec MeZhuYeCompanyCount '" + memberid + "','0','" + para.companyId + "'";
                }
            }
            else
            {
                var Companyid = _JointOfficeContext.Member_Company.Where(t => t.ParentId == "0").FirstOrDefault().Id;
                var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == para.memberid).FirstOrDefault();
                sql = @"exec GeRenZhuYe '" + para.memberid + "','" + memberid + "','" + info.ZhuBuMen + "','" + Companyid + "'," + begin + "," + end + ",'0'";
                sql1 = @"exec GeRenZhuYeCount '" + para.memberid + "','" + memberid + "','" + info.ZhuBuMen + "','" + Companyid + "','0'";
            }
            using (SqlConnection conText = new SqlConnection(constr))
            {
                list1 = conText.Query<PersonDynamic_info>(sql).ToList();
                allNum = conText.Query<int>(sql1).FirstOrDefault();
                allPages = allNum / para.count;
                if (allNum % para.count != 0)
                {
                    allPages += 1;
                }
            }
            foreach (var item in list1)
            {
                if (item.picturelist != null && item.picturelist != "")
                {
                    WorkDoc_Picture WorkDoc_Picture = new WorkDoc_Picture();
                    List<WorkDoc_PictureList> list2 = new List<WorkDoc_PictureList>();
                    var url = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item.picturelist);
                    foreach (var one in url)
                    {
                        WorkDoc_PictureList WorkDoc_PictureList = new WorkDoc_PictureList();
                        WorkDoc_PictureList.url = one.url + SasKey;
                        list2.Add(WorkDoc_PictureList);
                    }
                    WorkDoc_Picture.pictureList = list2;
                    list.Add(WorkDoc_Picture);
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<WorkDoc_Picture>();
            res.showapi_res_body.allPages = allPages;
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 工作录音
        /// </summary>
        /// <param name="人id,工作文档类型"></param>
        /// <returns></returns>
        public Showapi_Res_List<WorkDoc_Voice> GetWorkDoc_VoiceList(WorkDoc_Para para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<WorkDoc_Voice>();
                return Return.Return();
            }
            Showapi_Res_List<WorkDoc_Voice> res = new Showapi_Res_List<WorkDoc_Voice>();
            List<WorkDoc_Voice> list = new List<WorkDoc_Voice>();
            List<PersonDynamic_info> list1 = new List<PersonDynamic_info>();
            int allPages = 0;
            int allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            var sql = "";
            var sql1 = "";
            if (para.memberid == memberid)
            {
                if (string.IsNullOrEmpty(para.companyId))
                {
                    sql = @"exec MeZhuYeVoice '" + memberid + "'," + begin + "," + end + ",'0'";
                    sql1 = @"exec MeZhuYeCountVoice '" + memberid + "','0'";
                }
                else
                {
                    sql = @"exec MeZhuYeVoiceCompany '" + memberid + "'," + begin + "," + end + ",'0','" + para.companyId + "'";
                    sql1 = @"exec MeZhuYeVoiceCompanyCount '" + memberid + "','0','" + para.companyId + "'";
                }
            }
            else
            {
                var Companyid = _JointOfficeContext.Member_Company.Where(t => t.ParentId == "0").FirstOrDefault().Id;
                var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == para.memberid).FirstOrDefault();
                sql = @"exec GeRenZhuYeVoice '" + para.memberid + "','" + memberid + "','" + info.ZhuBuMen + "','" + Companyid + "'," + begin + "," + end + ",'0'";
                sql1 = @"exec GeRenZhuYeCountVoice '" + para.memberid + "','" + memberid + "','" + info.ZhuBuMen + "','" + Companyid + "','0'";
            }
            using (SqlConnection conText = new SqlConnection(constr))
            {
                list1 = conText.Query<PersonDynamic_info>(sql).ToList();
                allNum = conText.Query<int>(sql1).FirstOrDefault();
                allPages = allNum / para.count;
                if (allNum % para.count != 0)
                {
                    allPages += 1;
                }
            }
            foreach (var item in list1)
            {
                if (item.voice != null && item.voice != "")
                {
                    WorkDoc_Voice WorkDoc_Voice = new WorkDoc_Voice();
                    WorkDoc_Voice.annexurl = item.voice + SasKey;
                    WorkDoc_Voice.annexTime = item.createDate;
                    if (item.voiceLength != null && item.voiceLength != "" && item.voiceLength.Substring(0, 1) == "0")
                    {
                        WorkDoc_Voice.annexSize = item.voiceLength.Substring(1, 1);
                    }
                    else
                    {
                        WorkDoc_Voice.annexSize = item.voiceLength;
                    }
                    list.Add(WorkDoc_Voice);
                }
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<WorkDoc_Voice>();
            res.showapi_res_body.allPages = allPages;
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 工作列表
        /// </summary>
        public Showapi_Res_Single<WorkListAll> GetWorkListAll(WorkListAllInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<WorkListAll>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<WorkListAll> res = new Showapi_Res_Single<WorkListAll>();
            WorkListAll WorkListAll = new WorkListAll();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            var allPage = 0;
            int allNum = 0;
            //var Companyid = _JointOfficeContext.Member_Company.Where(t => t.ParentId == "0").FirstOrDefault().Id;
            //if (Companyid == null)
            //{
            //    Companyid = "";
            //}
            var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            var memList = _JointOfficeContext.Member_Info.Where(t => t.CompanyIDS.Contains(para.companyId)).ToList();
            var memListStr = "";
            foreach (var item in memList)
            {
                memListStr += item.MemberId;
            }
            if (string.IsNullOrEmpty(para.companyId))
            {
                para.companyId = "";
            }
            //全部
            if (para.type == 0)
            {
                List<WorkAll> list0 = new List<WorkAll>();
                List<WorkAllSQL> list00 = new List<WorkAllSQL>();
                var begin = para.page * para.count + 1;
                var end = (para.page + 1) * para.count;
                //memListStr = memListStr.Remove(memListStr.LastIndexOf(","));
                //var sql = @"exec WorkListAll1 '" + memberid + "'";
                //memListStr = "(" + memListStr + ")";
                var sql = @"exec WorkListAll1 '" + memListStr + "'," + begin + "," + end + ",'" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','0'";
                var sql1 = @"exec WorkListAll1Count '" + memListStr + "','" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','0'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list00 = conText.Query<WorkAllSQL>(sql).ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                    var allPage1 = allNum / para.count;
                    if (allNum % para.count != 0)
                    {
                        allPage1 += 1;
                    }
                    allPage = allPage1;
                    //list00 = list00.Skip(para.page * para.count).Take(para.count).ToList();
                }
                foreach (var item00 in list00)
                {
                    WorkAll WorkAll = new WorkAll();
                    if (item00.type == "1")
                    {
                        List<DaiShenPi> list = new List<DaiShenPi>();
                        DaiShenPi DaiShenPi = new DaiShenPi();
                        var workApproval = _JointOfficeContext.Work_Approval.Where(t => t.Id == item00.id).FirstOrDefault();
                        DaiShenPi = WorkDetails.GetDaiShenPiOne(workApproval, memberid);
                        list.Add(DaiShenPi);
                        WorkAll.WorkGetApprovalRemindList = list;
                        WorkAll.type = "1";
                        WorkAll.id = item00.id;
                    }
                    if (item00.type == "2")
                    {
                        List<DaiDianPingDeRiZhi> list = new List<DaiDianPingDeRiZhi>();
                        DaiDianPingDeRiZhi DaiDianPingDeRiZhi = new DaiDianPingDeRiZhi();
                        var workLog = _JointOfficeContext.Work_Log.Where(t => t.Id == item00.id).FirstOrDefault();
                        DaiDianPingDeRiZhi = WorkDetails.GetDaiDianPingDeRiZhiOne(workLog, memberid);
                        list.Add(DaiDianPingDeRiZhi);
                        WorkAll.WorkGetLogRemindList = list;
                        WorkAll.type = "2";
                        WorkAll.id = item00.id;
                    }
                    if (item00.type == "3")
                    {
                        List<DaiZhiXingDeRenWu> list = new List<DaiZhiXingDeRenWu>();
                        DaiZhiXingDeRenWu DaiZhiXingDeRenWu = new DaiZhiXingDeRenWu();
                        var workTask = _JointOfficeContext.Work_Task.Where(t => t.Id == item00.id).FirstOrDefault();
                        DaiZhiXingDeRenWu = WorkDetails.GetDaiZhiXingDeRenWuOne(workTask, memberid);
                        list.Add(DaiZhiXingDeRenWu);
                        WorkAll.WorkGetIPublishTaskList = list;
                        WorkAll.type = "3";
                        WorkAll.id = item00.id;
                    }
                    if (item00.type == "4")
                    {
                        List<RiChengDetail> list = new List<RiChengDetail>();
                        RiChengDetail RiChengDetail = new RiChengDetail();
                        var workProgram = _JointOfficeContext.Work_Program.Where(t => t.Id == item00.id).FirstOrDefault();
                        RiChengDetail = WorkDetails.GetRiChengDetailOne(workProgram, memberid);
                        list.Add(RiChengDetail);
                        WorkAll.WorkGetProgramNoticeList = list;
                        WorkAll.type = "4";
                        WorkAll.id = item00.id;
                    }
                    if (item00.type == "5")
                    {
                        List<ZhiLingDetail> list = new List<ZhiLingDetail>();
                        ZhiLingDetail ZhiLingDetail = new ZhiLingDetail();
                        var workOrder = _JointOfficeContext.Work_Order.Where(t => t.Id == item00.id).FirstOrDefault();
                        ZhiLingDetail = WorkDetails.GetZhiLingDetailOne(workOrder, memberid);
                        list.Add(ZhiLingDetail);
                        WorkAll.WorkGetOrderRemindList = list;
                        WorkAll.type = "5";
                        WorkAll.id = item00.id;
                    }
                    if (item00.type == "8")
                    {
                        List<GongGaoDetail> list = new List<GongGaoDetail>();
                        GongGaoDetail GongGaoDetail = new GongGaoDetail();
                        var workAnnouncement = _JointOfficeContext.Work_Announcement.Where(t => t.Id == item00.id).FirstOrDefault();
                        GongGaoDetail = WorkDetails.GetGongGaoDetailOne(workAnnouncement, memberid);
                        list.Add(GongGaoDetail);
                        WorkAll.WorkGetAnnouncementRemindList = list;
                        WorkAll.type = "8";
                        WorkAll.id = item00.id;
                    }
                    if (item00.type == "9")
                    {
                        List<FenXiangDetail> list = new List<FenXiangDetail>();
                        FenXiangDetail FenXiangDetail = new FenXiangDetail();
                        var workShare = _JointOfficeContext.Work_Share.Where(t => t.Id == item00.id).FirstOrDefault();
                        FenXiangDetail = WorkDetails.GetFenXiangDetailOne(workShare, memberid);
                        list.Add(FenXiangDetail);
                        WorkAll.WorkGetShareRemindList = list;
                        WorkAll.type = "9";
                        WorkAll.id = item00.id;
                    }
                    list0.Add(WorkAll);
                }
                WorkListAll.type = para.type;
                WorkListAll.allPage = allPage;
                WorkListAll.WorkGetAllList = list0;
            }
            //审批
            if (para.type == 1)
            {
                List<WorkAllSQL> list00 = new List<WorkAllSQL>();
                List<DaiShenPi> list = new List<DaiShenPi>();
                var begin = para.page * para.count + 1;
                var end = (para.page + 1) * para.count;
                var sql = @"exec WorkListAll1 '" + memListStr + "'," + begin + "," + end + ",'" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','1'";
                var sql1 = @"exec WorkListAll1Count '" + memListStr + "','" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','1'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list00 = conText.Query<WorkAllSQL>(sql).Where(t => t.type == "1").ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                    var allPage1 = allNum / para.count;
                    if (allNum % para.count != 0)
                    {
                        allPage1 += 1;
                    }
                    allPage = allPage1;
                    //list00 = list00.Skip(para.page * para.count).Take(para.count).ToList();
                }
                foreach (var item00 in list00)
                {
                    //WorkAll WorkAll = new WorkAll();
                    DaiShenPi DaiShenPi = new DaiShenPi();
                    var workApproval = _JointOfficeContext.Work_Approval.Where(t => t.Id == item00.id).FirstOrDefault();
                    DaiShenPi = WorkDetails.GetDaiShenPiOne(workApproval, memberid);
                    list.Add(DaiShenPi);
                    //WorkAll.WorkGetApprovalRemindList = list;
                    //WorkAll.type = "1";
                    //WorkAll.id = item00.id;
                    //list0.Add(WorkAll);
                }
                //var list1 = _JointOfficeContext.Work_Approval.Where(t => t.ATPerson.Contains(memberid) || t.ATPerson.Contains(info.ZhuBuMen) || t.MemberId == memberid || t.ApprovalPerson.Contains(memberid) || t.Range.Contains(memberid) || t.Range.Contains(info.ZhuBuMen) || t.Range.Contains(Companyid)).OrderByDescending(t => t.CreateDate).ToList();
                //var list1 = _JointOfficeContext.Work_Approval.Where(t => memListStr.Contains(t.MemberId) && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
                //var allPage1 = list1.Count() / para.count;
                //if (list1.Count % para.count != 0)
                //{
                //    allPage1 += 1;
                //}
                //allPage = allPage1;
                //list1 = list1.Skip(para.page * para.count).Take(para.count).ToList();
                //foreach (var item in list1)
                //{
                //    DaiShenPi DaiShenPi = new DaiShenPi();
                //    DaiShenPi = WorkDetails.GetDaiShenPiOne(item, memberid);
                //    list.Add(DaiShenPi);
                //}
                WorkListAll.type = para.type;
                WorkListAll.allPage = allPage;
                WorkListAll.WorkGetApprovalRemindList = list;
            }
            //日志
            if (para.type == 2)
            {
                //List<WorkAll> list0 = new List<WorkAll>();
                List<WorkAllSQL> list00 = new List<WorkAllSQL>();
                List<DaiDianPingDeRiZhi> list = new List<DaiDianPingDeRiZhi>();
                var begin = para.page * para.count + 1;
                var end = (para.page + 1) * para.count;
                var sql = @"exec WorkListAll1 '" + memListStr + "'," + begin + "," + end + ",'" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','2'";
                var sql1 = @"exec WorkListAll1Count '" + memListStr + "','" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','2'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list00 = conText.Query<WorkAllSQL>(sql).Where(t => t.type == "2").ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                    var allPage1 = allNum / para.count;
                    if (allNum % para.count != 0)
                    {
                        allPage1 += 1;
                    }
                    allPage = allPage1;
                    //list00 = list00.Skip(para.page * para.count).Take(para.count).ToList();
                }
                foreach (var item00 in list00)
                {
                    //WorkAll WorkAll = new WorkAll();
                    DaiDianPingDeRiZhi DaiDianPingDeRiZhi = new DaiDianPingDeRiZhi();
                    var workLog = _JointOfficeContext.Work_Log.Where(t => t.Id == item00.id).FirstOrDefault();
                    DaiDianPingDeRiZhi = WorkDetails.GetDaiDianPingDeRiZhiOne(workLog, memberid);
                    list.Add(DaiDianPingDeRiZhi);
                    //WorkAll.WorkGetLogRemindList = list;
                    //WorkAll.type = "2";
                    //WorkAll.id = item00.id;
                    //list0.Add(WorkAll);
                }
                //List<DaiDianPingDeRiZhi> list = new List<DaiDianPingDeRiZhi>();
                ////var list1 = _JointOfficeContext.Work_Log.Where(t => t.ATPerson.Contains(memberid) || t.ATPerson.Contains(info.ZhuBuMen) || t.MemberId == memberid || t.ReviewPersonId == memberid || t.Range.Contains(memberid) || t.Range.Contains(info.ZhuBuMen) || t.Range.Contains(Companyid)).OrderByDescending(t => t.CreateDate).ToList();
                //var list1 = _JointOfficeContext.Work_Log.Where(t => memListStr.Contains(t.MemberId) && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
                //var allPage1 = list1.Count() / para.count;
                //if (list1.Count % para.count != 0)
                //{
                //    allPage1 += 1;
                //}
                //allPage = allPage1;
                //list1 = list1.Skip(para.page * para.count).Take(para.count).ToList();
                //foreach (var item in list1)
                //{
                //    DaiDianPingDeRiZhi DaiDianPingDeRiZhi = new DaiDianPingDeRiZhi();
                //    DaiDianPingDeRiZhi = WorkDetails.GetDaiDianPingDeRiZhiOne(item, memberid);
                //    list.Add(DaiDianPingDeRiZhi);
                //}
                WorkListAll.type = para.type;
                WorkListAll.allPage = allPage;
                WorkListAll.WorkGetLogRemindList = list;
            }
            //任务
            if (para.type == 3)
            {
                List<WorkAllSQL> list00 = new List<WorkAllSQL>();
                List<DaiZhiXingDeRenWu> list = new List<DaiZhiXingDeRenWu>();
                var begin = para.page * para.count + 1;
                var end = (para.page + 1) * para.count;
                var sql = @"exec WorkListAll1 '" + memListStr + "'," + begin + "," + end + ",'" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','3'";
                var sql1 = @"exec WorkListAll1Count '" + memListStr + "','" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','3'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list00 = conText.Query<WorkAllSQL>(sql).Where(t => t.type == "3").ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                    var allPage1 = allNum / para.count;
                    if (allNum % para.count != 0)
                    {
                        allPage1 += 1;
                    }
                    allPage = allPage1;
                    //list00 = list00.Skip(para.page * para.count).Take(para.count).ToList();
                }
                foreach (var item00 in list00)
                {
                    //WorkAll WorkAll = new WorkAll();
                    DaiZhiXingDeRenWu DaiZhiXingDeRenWu = new DaiZhiXingDeRenWu();
                    var workTask = _JointOfficeContext.Work_Task.Where(t => t.Id == item00.id).FirstOrDefault();
                    DaiZhiXingDeRenWu = WorkDetails.GetDaiZhiXingDeRenWuOne(workTask, memberid);
                    list.Add(DaiZhiXingDeRenWu);
                    //WorkAll.WorkGetIPublishTaskList = list;
                    //WorkAll.type = "3";
                    //WorkAll.id = item00.id;
                    //list0.Add(WorkAll);
                }
                //var list1 = _JointOfficeContext.Work_Task.Where(t => t.ATPerson.Contains(memberid) || t.ATPerson.Contains(info.ZhuBuMen) || t.MemberId == memberid || t.Executor.Contains(memberid) || t.Executor.Contains(info.ZhuBuMen) || t.Range.Contains(memberid) || t.Range.Contains(info.ZhuBuMen) || t.Range.Contains(Companyid)).OrderByDescending(t => t.CreateDate).ToList();
                //var list1 = _JointOfficeContext.Work_Task.Where(t => memListStr.Contains(t.MemberId) && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
                //var allPage1 = list1.Count() / para.count;
                //if (list1.Count % para.count != 0)
                //{
                //    allPage1 += 1;
                //}
                //allPage = allPage1;
                //list1 = list1.Skip(para.page * para.count).Take(para.count).ToList();
                //foreach (var item in list1)
                //{
                //    DaiZhiXingDeRenWu DaiZhiXingDeRenWu = new DaiZhiXingDeRenWu();
                //    DaiZhiXingDeRenWu = WorkDetails.GetDaiZhiXingDeRenWuOne(item, memberid);
                //    list.Add(DaiZhiXingDeRenWu);
                //}
                WorkListAll.type = para.type;
                WorkListAll.allPage = allPage;
                WorkListAll.WorkGetIPublishTaskList = list;
            }
            //日程
            if (para.type == 4)
            {
                List<WorkAllSQL> list00 = new List<WorkAllSQL>();
                List<RiChengDetail> list = new List<RiChengDetail>();
                var begin = para.page * para.count + 1;
                var end = (para.page + 1) * para.count;
                var sql = @"exec WorkListAll1 '" + memListStr + "'," + begin + "," + end + ",'" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','4'";
                var sql1 = @"exec WorkListAll1Count '" + memListStr + "','" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','4'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list00 = conText.Query<WorkAllSQL>(sql).Where(t => t.type == "4").ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                    var allPage1 = allNum / para.count;
                    if (allNum % para.count != 0)
                    {
                        allPage1 += 1;
                    }
                    allPage = allPage1;
                    //list00 = list00.Skip(para.page * para.count).Take(para.count).ToList();
                }
                foreach (var item00 in list00)
                {
                    //WorkAll WorkAll = new WorkAll();
                    RiChengDetail RiChengDetail = new RiChengDetail();
                    var workProgram = _JointOfficeContext.Work_Program.Where(t => t.Id == item00.id).FirstOrDefault();
                    RiChengDetail = WorkDetails.GetRiChengDetailOne(workProgram, memberid);
                    list.Add(RiChengDetail);
                    //WorkAll.WorkGetProgramNoticeList = list;
                    //WorkAll.type = "4";
                    //WorkAll.id = item00.id;
                    //list0.Add(WorkAll);
                }
                //var list1 = _JointOfficeContext.Work_Program.Where(t => t.ATPerson.Contains(memberid) || t.ATPerson.Contains(info.ZhuBuMen) || t.MemberId == memberid || t.JoinPerson.Contains(memberid) || t.JoinPerson.Contains(info.ZhuBuMen) || t.Range.Contains(memberid) || t.Range.Contains(info.ZhuBuMen) || t.Range.Contains(Companyid)).OrderByDescending(t => t.CreateDate).ToList();
                //var list1 = _JointOfficeContext.Work_Program.Where(t => memListStr.Contains(t.MemberId) && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
                //var allPage1 = list1.Count() / para.count;
                //if (list1.Count % para.count != 0)
                //{
                //    allPage1 += 1;
                //}
                //allPage = allPage1;
                //list1 = list1.Skip(para.page * para.count).Take(para.count).ToList();
                //foreach (var item in list1)
                //{
                //    RiChengDetail RiChengDetail = new RiChengDetail();
                //    RiChengDetail = WorkDetails.GetRiChengDetailOne(item, memberid);
                //    list.Add(RiChengDetail);
                //}
                WorkListAll.type = para.type;
                WorkListAll.allPage = allPage;
                WorkListAll.WorkGetProgramNoticeList = list;
            }
            //指令
            if (para.type == 5)
            {
                List<WorkAllSQL> list00 = new List<WorkAllSQL>();
                List<ZhiLingDetail> list = new List<ZhiLingDetail>();
                var begin = para.page * para.count + 1;
                var end = (para.page + 1) * para.count;
                var sql = @"exec WorkListAll1 '" + memListStr + "'," + begin + "," + end + ",'" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','5'";
                var sql1 = @"exec WorkListAll1Count '" + memListStr + "','" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','5'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list00 = conText.Query<WorkAllSQL>(sql).Where(t => t.type == "5").ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                    var allPage1 = allNum / para.count;
                    if (allNum % para.count != 0)
                    {
                        allPage1 += 1;
                    }
                    allPage = allPage1;
                    //list00 = list00.Skip(para.page * para.count).Take(para.count).ToList();
                }
                foreach (var item00 in list00)
                {
                    //WorkAll WorkAll = new WorkAll();
                    ZhiLingDetail ZhiLingDetail = new ZhiLingDetail();
                    var workOrder = _JointOfficeContext.Work_Order.Where(t => t.Id == item00.id).FirstOrDefault();
                    ZhiLingDetail = WorkDetails.GetZhiLingDetailOne(workOrder, memberid);
                    list.Add(ZhiLingDetail);
                    //WorkAll.WorkGetOrderRemindList = list;
                    //WorkAll.type = "5";
                    //WorkAll.id = item00.id;
                    //list0.Add(WorkAll);
                }
                //var list1 = _JointOfficeContext.Work_Order.Where(t => t.ATPerson.Contains(memberid) || t.ATPerson.Contains(info.ZhuBuMen) || t.MemberId == memberid || t.Executor.Contains(memberid) || t.Executor.Contains(info.ZhuBuMen) || t.Range.Contains(memberid) || t.Range.Contains(info.ZhuBuMen) || t.Range.Contains(Companyid)).OrderByDescending(t => t.CreateDate).ToList();
                //var list1 = _JointOfficeContext.Work_Order.Where(t => memListStr.Contains(t.MemberId) && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
                //var allPage1 = list1.Count() / para.count;
                //if (list1.Count % para.count != 0)
                //{
                //    allPage1 += 1;
                //}
                //allPage = allPage1;
                //list1 = list1.Skip(para.page * para.count).Take(para.count).ToList();
                //foreach (var item in list1)
                //{
                //    ZhiLingDetail ZhiLingDetail = new ZhiLingDetail();
                //    ZhiLingDetail = WorkDetails.GetZhiLingDetailOne(item, memberid);
                //    list.Add(ZhiLingDetail);
                //}
                WorkListAll.type = para.type;
                WorkListAll.allPage = allPage;
                WorkListAll.WorkGetOrderRemindList = list;
            }
            //公告
            if (para.type == 8)
            {
                List<WorkAllSQL> list00 = new List<WorkAllSQL>();
                List<GongGaoDetail> list = new List<GongGaoDetail>();
                var begin = para.page * para.count + 1;
                var end = (para.page + 1) * para.count;
                var sql = @"exec WorkListAll1 '" + memListStr + "'," + begin + "," + end + ",'" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','8'";
                var sql1 = @"exec WorkListAll1Count '" + memListStr + "','" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','8'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list00 = conText.Query<WorkAllSQL>(sql).Where(t => t.type == "8").ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                    var allPage1 = allNum / para.count;
                    if (allNum % para.count != 0)
                    {
                        allPage1 += 1;
                    }
                    allPage = allPage1;
                    //list00 = list00.Skip(para.page * para.count).Take(para.count).ToList();
                }
                foreach (var item00 in list00)
                {
                    //WorkAll WorkAll = new WorkAll();
                    GongGaoDetail GongGaoDetail = new GongGaoDetail();
                    var workAnnouncement = _JointOfficeContext.Work_Announcement.Where(t => t.Id == item00.id).FirstOrDefault();
                    GongGaoDetail = WorkDetails.GetGongGaoDetailOne(workAnnouncement, memberid);
                    list.Add(GongGaoDetail);
                    //WorkAll.WorkGetAnnouncementRemindList = list;
                    //WorkAll.type = "8";
                    //WorkAll.id = item00.id;
                    //list0.Add(WorkAll);
                }
                //var list1 = _JointOfficeContext.Work_Announcement.Where(t => t.MemberId == memberid || t.Range.Contains(memberid) || t.Range.Contains(info.ZhuBuMen) || t.Range.Contains(Companyid) || t.Receipt.Contains(memberid) || t.Receipt.Contains(info.ZhuBuMen) || t.Receipt.Contains(Companyid) || t.ATPerson.Contains(memberid) || t.ATPerson.Contains(info.ZhuBuMen)).OrderByDescending(t => t.CreateDate).ToList();
                //var list1 = _JointOfficeContext.Work_Announcement.Where(t => memListStr.Contains(t.MemberId) && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
                //var allPage1 = list1.Count() / para.count;
                //if (list1.Count % para.count != 0)
                //{
                //    allPage1 += 1;
                //}
                //allPage = allPage1;
                //list1 = list1.Skip(para.page * para.count).Take(para.count).ToList();
                //foreach (var item in list1)
                //{
                //    GongGaoDetail GongGaoDetail = new GongGaoDetail();
                //    GongGaoDetail = WorkDetails.GetGongGaoDetailOne(item, memberid);
                //    list.Add(GongGaoDetail);
                //}
                WorkListAll.type = para.type;
                WorkListAll.allPage = allPage;
                WorkListAll.WorkGetAnnouncementRemindList = list;
            }
            //分享
            if (para.type == 9)
            {
                List<WorkAllSQL> list00 = new List<WorkAllSQL>();
                List<FenXiangDetail> list = new List<FenXiangDetail>();
                var begin = para.page * para.count + 1;
                var end = (para.page + 1) * para.count;
                var sql = @"exec WorkListAll1 '" + memListStr + "'," + begin + "," + end + ",'" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','9'";
                var sql1 = @"exec WorkListAll1Count '" + memListStr + "','" + memberid + "','" + para.companyId + "','" + info.ZhuBuMen + "','9'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list00 = conText.Query<WorkAllSQL>(sql).Where(t => t.type == "9").ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                    var allPage1 = allNum / para.count;
                    if (allNum % para.count != 0)
                    {
                        allPage1 += 1;
                    }
                    allPage = allPage1;
                    //list00 = list00.Skip(para.page * para.count).Take(para.count).ToList();
                }
                foreach (var item00 in list00)
                {
                    //WorkAll WorkAll = new WorkAll();
                    FenXiangDetail FenXiangDetail = new FenXiangDetail();
                    var workShare = _JointOfficeContext.Work_Share.Where(t => t.Id == item00.id).FirstOrDefault();
                    FenXiangDetail = WorkDetails.GetFenXiangDetailOne(workShare, memberid);
                    list.Add(FenXiangDetail);
                    //WorkAll.WorkGetShareRemindList = list;
                    //WorkAll.type = "9";
                    //WorkAll.id = item00.id;
                    //list0.Add(WorkAll);
                }
                //var list1 = _JointOfficeContext.Work_Share.Where(t => t.MemberId == memberid || t.Range.Contains(memberid) || t.Range.Contains(info.ZhuBuMen) || t.Range.Contains(Companyid) || t.Receipt.Contains(memberid) || t.Receipt.Contains(info.ZhuBuMen) || t.Receipt.Contains(Companyid) || t.ATPerson.Contains(memberid) || t.ATPerson.Contains(info.ZhuBuMen)).OrderByDescending(t => t.CreateDate).ToList();
                //var list1 = _JointOfficeContext.Work_Share.Where(t => memListStr.Contains(t.MemberId) && (t.CompanyId == para.companyId || t.CompanyId == null || t.CompanyId == "")).OrderByDescending(t => t.CreateDate).ToList();
                //var allPage1 = list1.Count() / para.count;
                //if (list1.Count % para.count != 0)
                //{
                //    allPage1 += 1;
                //}
                //allPage = allPage1;
                //list1 = list1.Skip(para.page * para.count).Take(para.count).ToList();
                //foreach (var item in list1)
                //{
                //    FenXiangDetail FenXiangDetail = new FenXiangDetail();
                //    FenXiangDetail = WorkDetails.GetFenXiangDetailOne(item, memberid);
                //    list.Add(FenXiangDetail);
                //}
                WorkListAll.type = para.type;
                WorkListAll.allPage = allPage;
                WorkListAll.WorkGetShareRemindList = list;
            }
            res.showapi_res_body = WorkListAll;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 工作列表分类
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<WorkList> GetWorkList()
        {
            Showapi_Res_List<WorkList> res = new Showapi_Res_List<WorkList>();
            List<WorkList> list = new List<WorkList>();
            string[] array = new string[] { "全部", "审批", "日志", "任务", "日程", "指令", "公告", "分享" };
            for (int i = 0; i < array.Length; i++)
            {
                WorkList WorkList = new WorkList();
                WorkList.type = i;
                WorkList.name = array[i];
                list.Add(WorkList);
            }
            //string[] array = new string[] { "全部", "日志", "任务", "日程", "指令" };
            //for (int i = 0; i < array.Length; i++)
            //{
            //    WorkList WorkList = new WorkList();
            //    switch (i)
            //    {
            //        case 0:
            //            WorkList.type = i;
            //            WorkList.name = array[i];
            //            break;
            //        case 1:
            //            WorkList.type = i + 1;
            //            WorkList.name = array[i];
            //            break;
            //        case 2:
            //            WorkList.type = i + 1;
            //            WorkList.name = array[i];
            //            break;
            //        case 3:
            //            WorkList.type = i + 1;
            //            WorkList.name = array[i];
            //            break;
            //        case 4:
            //            WorkList.type = i + 1;
            //            WorkList.name = array[i];
            //            break;
            //    }
            //    list.Add(WorkList);
            //}
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<WorkList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 工作列表分类   新
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<WorkList> GetWorkListNew()
        {
            Showapi_Res_List<WorkList> res = new Showapi_Res_List<WorkList>();
            List<WorkList> list = new List<WorkList>();
            //string[] array = new string[] { "全部" , "审批", "日志", "任务", "日程", "指令" };
            //for (int i=0 ; i<array.Length ; i++)
            //{
            //    WorkList WorkList = new WorkList();
            //    WorkList.type = i;
            //    WorkList.name = array[i];
            //    list.Add(WorkList);
            //}
            string[] array = new string[] { "全部", "日志", "任务", "日程", "指令" };
            for (int i = 0; i < array.Length; i++)
            {
                WorkList WorkList = new WorkList();
                switch (i)
                {
                    case 0:
                        WorkList.type = i;
                        WorkList.name = array[i];
                        break;
                    case 1:
                        WorkList.type = i + 1;
                        WorkList.name = array[i];
                        break;
                    case 2:
                        WorkList.type = i + 1;
                        WorkList.name = array[i];
                        break;
                    case 3:
                        WorkList.type = i + 1;
                        WorkList.name = array[i];
                        break;
                    case 4:
                        WorkList.type = i + 1;
                        WorkList.name = array[i];
                        break;
                }
                list.Add(WorkList);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<WorkList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 审批  同意/不同意
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge Approval(ApprovalInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var workApproval = _JointOfficeContext.Work_Approval.Where(t => t.Id == para.uid).FirstOrDefault();
            if (workApproval == null)
            {
                throw new BusinessTureException("此审批已删除");
            }
            var approvalMe = _JointOfficeContext.Approval_Content.Where(t => t.UId == para.uid && t.OtherMemberId == memberid).FirstOrDefault();
            if (approvalMe != null)
            {
                if (approvalMe.State == 6)
                {
                    throw new BusinessTureException("此审批已失效");
                }
                if (approvalMe.State == 5)
                {
                    throw new BusinessTureException("此审批的实时审批人非当前用户");
                }
                if (approvalMe.State == 1 || approvalMe.State == 2 || approvalMe.State == 3)
                {
                    throw new BusinessTureException("审批已结束，不能再次审批");
                }

                Comment_Body Comment_Body = new Comment_Body();
                switch (para.isAgree)
                {
                    case 1:
                        approvalMe.State = 1;
                        Comment_Body.Body = "已批准。 " + para.body;
                        var approvalContent = _JointOfficeContext.Approval_Content.Where(t => t.UId == para.uid).ToList();
                        var approvalContentNext = approvalContent.Where(t => t.OtherMemberOrder == approvalMe.OtherMemberOrder + 1).FirstOrDefault();
                        if (approvalContentNext != null)
                        {
                            approvalContentNext.IsMeApproval = "1";
                            approvalContentNext.State = 0;

                            SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                            var parems = SendHelper.getXiaoXiParams(para.uid, "1");
                            SendHelper.SendXiaoXi("待审批的审批", approvalContentNext.OtherMemberId, parems);
                        }
                        if (workApproval.ApprovalPersonNum == approvalContent.Count)
                        {
                            workApproval.State = 1;
                            workApproval.ApprovalPersonNum = 0;
                        }
                        else
                        {
                            workApproval.ApprovalPersonNum += 1;
                        }
                        break;
                    case 0:
                        approvalMe.State = 3;
                        workApproval.State = 3;
                        workApproval.ApprovalPersonNum = 0;
                        Comment_Body.Body = "未批准。 " + para.body;
                        break;
                }
                approvalMe.IsMeApproval = "0";
                approvalMe.Content = para.body;
                approvalMe.Picture = para.picture;
                approvalMe.ApprovalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                approvalMe.PhoneModel = para.phoneModel;

                Comment_Body.Id = Guid.NewGuid().ToString();
                Comment_Body.PingLunMemberId = memberid;
                var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                Comment_Body.Name = memberInfo.Name;
                Comment_Body.Picture = memberInfo.Picture;
                Comment_Body.PingLunTime = DateTime.Now;
                Comment_Body.UId = para.uid;
                Comment_Body.MemberId = para.memberid;
                Comment_Body.PId = "";
                Comment_Body.OtherBody = "";
                Comment_Body.PersonId = "";
                Comment_Body.PersonName = "";
                Comment_Body.Type = "1";
                Comment_Body.IsExeComment = 1;
                Comment_Body.PictureList = para.appendPicture;
                Comment_Body.Annex = para.annex;
                Comment_Body.Voice = para.voice;
                Comment_Body.VoiceLength = para.voiceLength;
                Comment_Body.PhoneModel = para.phoneModel;
                Comment_Body.ATPerson = para._person;
                _JointOfficeContext.Comment_Body.Add(Comment_Body);

                TotalNum TotalNum = new TotalNum();
                TotalNum.Id = Guid.NewGuid().ToString();
                TotalNum.Type = "7+1";
                TotalNum.UId = "";
                TotalNum.PId = Comment_Body.Id;
                TotalNum.P_UId = para.uid;
                TotalNum.DianZanNum = 0;
                TotalNum.ZhuanFaNum = 0;
                TotalNum.PingLunNum = 0;
                TotalNum.CreateTime = DateTime.Now;
                _JointOfficeContext.TotalNum.Add(TotalNum);

                var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.uid).FirstOrDefault();
                if (totalNum != null)
                {
                    totalNum.PingLunNum += 1;
                }

                _JointOfficeContext.SaveChanges();
                
                if (workApproval != null)
                {
                    if (para.isAgree == 0)
                    {
                        SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                        var parems = SendHelper.getXiaoXiParams(para.uid, "1");
                        SendHelper.SendXiaoXi("你发出的审批被拒绝", workApproval.MemberId, parems);
                    }
                    else
                    {
                        SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                        var parems = SendHelper.getXiaoXiParams(para.uid, "1");
                        SendHelper.SendXiaoXi("你发出的审批已通过", workApproval.MemberId, parems);
                    }
                }
            }
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 审批人  取消审批
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge EscApproval(EscApprovalInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var workApproval = _JointOfficeContext.Work_Approval.Where(t => t.Id == para.uid).FirstOrDefault();
            if (workApproval == null)
            {
                throw new BusinessTureException("此审批已删除");
            }
            var approvalMe = _JointOfficeContext.Approval_Content.Where(t => t.UId == para.uid && t.OtherMemberId == memberid).FirstOrDefault();
            if (approvalMe != null)
            {
                if (approvalMe.State == 6)
                {
                    throw new BusinessTureException("此审批已失效");
                }
                if (approvalMe.State == 5)
                {
                    throw new BusinessTureException("此审批的实时审批人非当前用户");
                }
                if (approvalMe.State == 1 || approvalMe.State == 2 || approvalMe.State == 3)
                {
                    throw new BusinessTureException("审批已结束，不能再次审批");
                }
                approvalMe.State = 2;
                approvalMe.IsMeApproval = "0";
                approvalMe.Content = "";
                approvalMe.Picture = "";
                approvalMe.ApprovalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                approvalMe.PhoneModel = para.phoneModel;

                var approvalContent = _JointOfficeContext.Approval_Content.Where(t => t.UId == para.uid).ToList();
                var approvalContentNext = approvalContent.Where(t => t.OtherMemberOrder == approvalMe.OtherMemberOrder + 1).FirstOrDefault();
                if (approvalContentNext != null)
                {
                    approvalContentNext.IsMeApproval = "1";
                    approvalContentNext.State = 0;

                    SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                    var parems = SendHelper.getXiaoXiParams(para.uid, "1");
                    SendHelper.SendXiaoXi("待审批的审批", approvalContentNext.OtherMemberId, parems);
                }
                var approvalContentTo1 = approvalContent.Where(t => t.State == 1).ToList();
                var approvalContentTo2 = approvalContent.Where(t => t.State == 2).ToList();
                if (approvalContent.Count == approvalContentTo2.Count)
                {
                    workApproval.State = 4;
                    workApproval.ApprovalPersonNum = 0;
                }
                else if (approvalContent.Count == (approvalContentTo1.Count + approvalContentTo2.Count))
                {
                    workApproval.State = 1;
                    workApproval.ApprovalPersonNum = 0;
                }
                else
                {
                    workApproval.ApprovalPersonNum += 1;
                }

                Comment_Body Comment_Body = new Comment_Body();
                Comment_Body.Id = Guid.NewGuid().ToString();
                Comment_Body.PingLunMemberId = memberid;
                var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                Comment_Body.Name = memberInfo.Name;
                Comment_Body.Picture = memberInfo.Picture;
                Comment_Body.Body = "取消审批。";
                Comment_Body.PingLunTime = DateTime.Now;
                Comment_Body.UId = para.uid;
                Comment_Body.MemberId = para.memberid;
                Comment_Body.PId = "";
                Comment_Body.OtherBody = "";
                Comment_Body.PersonId = "";
                Comment_Body.PersonName = "";
                Comment_Body.Type = "1";
                Comment_Body.IsExeComment = 1;
                Comment_Body.PictureList = "";
                Comment_Body.Voice = "";
                Comment_Body.VoiceLength = "";
                Comment_Body.Annex = "";
                Comment_Body.PhoneModel = para.phoneModel;
                Comment_Body.ATPerson = "";
                _JointOfficeContext.Comment_Body.Add(Comment_Body);

                TotalNum TotalNum = new TotalNum();
                TotalNum.Id = Guid.NewGuid().ToString();
                TotalNum.Type = "7+1";
                TotalNum.UId = "";
                TotalNum.PId = Comment_Body.Id;
                TotalNum.P_UId = para.uid;
                TotalNum.DianZanNum = 0;
                TotalNum.ZhuanFaNum = 0;
                TotalNum.PingLunNum = 0;
                TotalNum.CreateTime = DateTime.Now;
                _JointOfficeContext.TotalNum.Add(TotalNum);

                var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.uid).FirstOrDefault();
                if (totalNum != null)
                {
                    totalNum.PingLunNum += 1;
                }

                _JointOfficeContext.SaveChanges();
            }
            if (workApproval != null)
            {
                SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                var parems = SendHelper.getXiaoXiParams(para.uid, "1");
                SendHelper.SendXiaoXi("你发出的审批审批人已取消", workApproval.MemberId, parems);
            }
            return Message.SuccessMeaasge("取消审批成功");
        }
        /// <summary>
        /// 重新选择审批人
        /// </summary>
        public Showapi_Res_Meaasge AgainChooseApprovalPerson(AgainChooseApprovalPersonInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var workApproval = _JointOfficeContext.Work_Approval.Where(t => t.Id == para.uid).FirstOrDefault();
            if (workApproval != null)
            {
                workApproval.ApprovalPerson = para.newApprovalPerson;
                workApproval.ApprovalPersonNum = 1;
                workApproval.State = 0;
            }
            else
            {
                throw new BusinessTureException("此审批已删除");
            }
            var approvalContent = _JointOfficeContext.Approval_Content.Where(t => t.UId == para.uid).ToList();
            if (approvalContent != null && approvalContent.Count != 0)
            {
                foreach (var item in approvalContent)
                {
                    _JointOfficeContext.Approval_Content.Remove(item);
                }
            }
            var comment = _JointOfficeContext.Comment_Body.Where(t => t.UId == para.uid && t.IsExeComment == 1).ToList();
            if (comment != null && comment.Count != 0)
            {
                foreach (var item in comment)
                {
                    var oneData = _JointOfficeContext.TotalNum.Where(t => t.PId == item.Id).FirstOrDefault();
                    if (oneData != null)
                    {
                        _JointOfficeContext.TotalNum.Remove(oneData);
                    }
                    _JointOfficeContext.Comment_Body.Remove(item);
                }
            }
            var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.uid).FirstOrDefault();
            if (totalNum != null)
            {
                totalNum.PingLunNum -= comment.Count();
            }
            if (para.newApprovalPerson != null && para.newApprovalPerson != "")
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ApprovalPeoPleInfo>>(para.newApprovalPerson);
                foreach (var item in list)
                {
                    Approval_Content Approval_Content = new Approval_Content();
                    Approval_Content.Id = Guid.NewGuid().ToString();
                    Approval_Content.MemberId = workApproval.MemberId;
                    Approval_Content.UId = workApproval.Id;
                    Approval_Content.OtherMemberId = item.id;
                    var OtherMember = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.id).FirstOrDefault();
                    Approval_Content.OtherMemberName = OtherMember.Name;
                    Approval_Content.OtherMemberPicture = OtherMember.Picture;
                    Approval_Content.OtherMemberOrder = Convert.ToInt32(item.order);
                    if (item.order == "1")
                    {
                        Approval_Content.IsMeApproval = "1";
                        Approval_Content.State = 0;
                    }
                    else
                    {
                        Approval_Content.IsMeApproval = "0";
                        Approval_Content.State = 5;
                    }
                    Approval_Content.Type = workApproval.Type;
                    Approval_Content.CreateDate = DateTime.Now;
                    Approval_Content.Content = "";
                    Approval_Content.Picture = "";
                    Approval_Content.ApprovalTime = "";
                    Approval_Content.PhoneModel = "";
                    _JointOfficeContext.Approval_Content.Add(Approval_Content);
                }
            }

            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 执行人  同意执行
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge Exe(ExeInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            Work_Task work_Task = null;
            Work_Order work_Order = null;
            if (para.type == "3")
            {
                work_Task = _JointOfficeContext.Work_Task.Where(t => t.Id == para.uid).FirstOrDefault();
                if (work_Task == null)
                {
                    throw new BusinessTureException("此任务已删除");
                }
            }
            if (para.type == "5")
            {
                work_Order = _JointOfficeContext.Work_Order.Where(t => t.Id == para.uid).FirstOrDefault();
                if (work_Order == null)
                {
                    throw new BusinessTureException("此指令已删除");
                }
            }
            var exeContent = _JointOfficeContext.Execute_Content.Where(t => t.UId == para.uid && t.OtherMemberId == memberid).FirstOrDefault();
            if (exeContent != null)
            {
                if (exeContent.State == 3)
                {
                    throw new BusinessTureException("此工作已失效");
                }
                if (exeContent.State == 1 || exeContent.State == 2)
                {
                    throw new BusinessTureException("执行已结束，不能再次执行");
                }
                exeContent.Content = para.body;
                exeContent.State = 1;
                exeContent.ExecuteDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                exeContent.PhoneModel = para.phoneModel;
                Comment_Body Comment_Body = new Comment_Body();
                TotalNum TotalNum = new TotalNum();
                if (para.type == "3")
                {
                    var exeContentList = _JointOfficeContext.Execute_Content.Where(t => t.UId == para.uid).ToList();
                    var exeContentList1 = _JointOfficeContext.Execute_Content.Where(t => t.UId == para.uid && t.State == 1).ToList();
                    if (exeContentList.Count == (exeContentList1.Count + 1))
                    {
                        work_Task.State = 1;
                    }
                    Comment_Body.Body = "完成任务。 " + para.body;
                    TotalNum.Type = "7+3";
                }
                if (para.type == "5")
                {
                    work_Order.State = 1;

                    Comment_Body.Body = "完成指令。 " + para.body;
                    TotalNum.Type = "7+5";
                }

                Comment_Body.Id = Guid.NewGuid().ToString();
                Comment_Body.PingLunMemberId = memberid;
                var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                Comment_Body.Name = memberInfo.Name;
                Comment_Body.Picture = memberInfo.Picture;
                Comment_Body.PingLunTime = DateTime.Now;
                Comment_Body.UId = para.uid;
                Comment_Body.MemberId = para.memberid;
                Comment_Body.PId = "";
                Comment_Body.OtherBody = "";
                Comment_Body.PersonId = "";
                Comment_Body.PersonName = "";
                Comment_Body.Type = para.type;
                Comment_Body.IsExeComment = 1;
                Comment_Body.PictureList = para.appendPicture;
                Comment_Body.Annex = para.annex;
                Comment_Body.Voice = para.voice;
                Comment_Body.VoiceLength = para.voiceLength;
                Comment_Body.PhoneModel = para.phoneModel;
                Comment_Body.ATPerson = para._person;
                _JointOfficeContext.Comment_Body.Add(Comment_Body);

                TotalNum.Id = Guid.NewGuid().ToString();
                TotalNum.UId = "";
                TotalNum.PId = Comment_Body.Id;
                TotalNum.P_UId = para.uid;
                TotalNum.DianZanNum = 0;
                TotalNum.ZhuanFaNum = 0;
                TotalNum.PingLunNum = 0;
                TotalNum.CreateTime = DateTime.Now;
                _JointOfficeContext.TotalNum.Add(TotalNum);

                var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.uid).FirstOrDefault();
                if (totalNum != null)
                {
                    totalNum.PingLunNum += 1;
                }

                _JointOfficeContext.SaveChanges();
            }
            if (para.type == "3")
            {
                if (work_Task != null)
                {
                    SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                    var parems = SendHelper.getXiaoXiParams(para.uid, "3");
                    SendHelper.SendXiaoXi("任务已执行", work_Task.MemberId, parems);
                }
            }
            else
            {
                if (work_Order != null)
                {
                    SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                    var parems = SendHelper.getXiaoXiParams(para.uid, "5");
                    SendHelper.SendXiaoXi("指令已执行", work_Order.MemberId, parems);
                }
            }
            return Message.SuccessMeaasge("回复成功");
        }
        /// <summary>
        /// 执行人  取消任务/取消指令    参与人  取消日程
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge NoExe(NoExeInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            Work_Task work_Task = null;
            Work_Program workProgram = null;
            Work_Order work_Order = null;
            if (para.type == "3")
            {
                work_Task = _JointOfficeContext.Work_Task.Where(t => t.Id == para.uid).FirstOrDefault();
                if (work_Task == null)
                {
                    throw new BusinessTureException("此任务已删除");
                }
            }
            if (para.type == "4")
            {
                workProgram = _JointOfficeContext.Work_Program.Where(t => t.Id == para.uid).FirstOrDefault();
                if (workProgram == null)
                {
                    throw new BusinessTureException("此日程已删除");
                }
            }
            if (para.type == "5")
            {
                work_Order = _JointOfficeContext.Work_Order.Where(t => t.Id == para.uid).FirstOrDefault();
                if (work_Order == null)
                {
                    throw new BusinessTureException("此指令已删除");
                }
            }
            if (para.type != "4")
            {
                var exeContent = _JointOfficeContext.Execute_Content.Where(t => t.UId == para.uid && t.OtherMemberId == memberid).FirstOrDefault();
                if (exeContent != null)
                {
                    if (exeContent.State == 3)
                    {
                        throw new BusinessTureException("此工作已失效");
                    }
                    if (exeContent.State == 1 || exeContent.State == 2)
                    {
                        throw new BusinessTureException("执行已结束，不能再次执行");
                    }
                    Comment_Body Comment_Body = new Comment_Body();
                    Comment_Body.Body = "取消任务。";
                    TotalNum TotalNum = new TotalNum();
                    TotalNum.Type = "7+3";

                    exeContent.Content = "";
                    exeContent.State = 2;
                    exeContent.ExecuteDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    exeContent.PhoneModel = para.phoneModel;
                    if (para.type == "5")
                    {
                        work_Order.State = 2;

                        Comment_Body.Body = "取消指令。";
                        TotalNum.Type = "7+5";
                    }
                    
                    Comment_Body.Id = Guid.NewGuid().ToString();
                    Comment_Body.PingLunMemberId = memberid;
                    var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                    Comment_Body.Name = memberInfo.Name;
                    Comment_Body.Picture = memberInfo.Picture;
                    Comment_Body.PingLunTime = DateTime.Now;
                    Comment_Body.UId = para.uid;
                    Comment_Body.MemberId = para.memberid;
                    Comment_Body.PId = "";
                    Comment_Body.OtherBody = "";
                    Comment_Body.PersonId = "";
                    Comment_Body.PersonName = "";
                    Comment_Body.Type = para.type;
                    Comment_Body.IsExeComment = 1;
                    Comment_Body.PictureList = "";
                    Comment_Body.Voice = "";
                    Comment_Body.VoiceLength = "";
                    Comment_Body.Annex = "";
                    Comment_Body.PhoneModel = para.phoneModel;
                    Comment_Body.ATPerson = "";
                    _JointOfficeContext.Comment_Body.Add(Comment_Body);

                    TotalNum.Id = Guid.NewGuid().ToString();
                    TotalNum.UId = "";
                    TotalNum.PId = Comment_Body.Id;
                    TotalNum.P_UId = para.uid;
                    TotalNum.DianZanNum = 0;
                    TotalNum.ZhuanFaNum = 0;
                    TotalNum.PingLunNum = 0;
                    TotalNum.CreateTime = DateTime.Now;
                    _JointOfficeContext.TotalNum.Add(TotalNum);

                    var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.uid).FirstOrDefault();
                    if (totalNum != null)
                    {
                        totalNum.PingLunNum += 1;
                    }
                }
            }
            if (para.type == "4")
            {
                if (workProgram != null)
                {
                    EscProgram EscProgram = new EscProgram();
                    EscProgram.BeforeEscJoinPerson = workProgram.JoinPerson;
                    var joinPersonList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(workProgram.JoinPerson);
                    if (joinPersonList != null && joinPersonList.Count != 0)
                    {
                        var joinPersonOne = joinPersonList.Where(t => t.id == memberid).FirstOrDefault();
                        if (joinPersonOne != null)
                        {
                            joinPersonList.Remove(joinPersonOne);
                        }
                        else
                        {
                            throw new BusinessTureException("日程已取消，不能再次取消");
                        }
                        var str = Newtonsoft.Json.JsonConvert.SerializeObject(joinPersonList);
                        workProgram.JoinPerson = str;
                        if (joinPersonList.Count == 0)
                        {
                            workProgram.State = 2;
                        }

                        EscProgram.Id = Guid.NewGuid().ToString();
                        EscProgram.JoinPersonMemberId = memberid;
                        EscProgram.UId = para.uid;
                        EscProgram.MemberId = workProgram.MemberId;
                        EscProgram.EscTime = DateTime.Now;
                        EscProgram.AfterEscJoinPerson = str;
                        _JointOfficeContext.EscProgram.Add(EscProgram);
                    }

                    Comment_Body Comment_Body = new Comment_Body();
                    Comment_Body.Id = Guid.NewGuid().ToString();
                    Comment_Body.PingLunMemberId = memberid;
                    var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                    Comment_Body.Name = memberInfo.Name;
                    Comment_Body.Picture = memberInfo.Picture;
                    Comment_Body.Body = "取消日程。";
                    Comment_Body.PingLunTime = DateTime.Now;
                    Comment_Body.UId = para.uid;
                    Comment_Body.MemberId = para.memberid;
                    Comment_Body.PId = "";
                    Comment_Body.OtherBody = "";
                    Comment_Body.PersonId = "";
                    Comment_Body.PersonName = "";
                    Comment_Body.Type = para.type;
                    Comment_Body.IsExeComment = 1;
                    Comment_Body.PictureList = "";
                    Comment_Body.Voice = "";
                    Comment_Body.VoiceLength = "";
                    Comment_Body.Annex = "";
                    Comment_Body.PhoneModel = para.phoneModel;
                    Comment_Body.ATPerson = "";
                    _JointOfficeContext.Comment_Body.Add(Comment_Body);

                    TotalNum TotalNum = new TotalNum();
                    TotalNum.Id = Guid.NewGuid().ToString();
                    TotalNum.Type = "7+4";
                    TotalNum.UId = "";
                    TotalNum.PId = Comment_Body.Id;
                    TotalNum.P_UId = para.uid;
                    TotalNum.DianZanNum = 0;
                    TotalNum.ZhuanFaNum = 0;
                    TotalNum.PingLunNum = 0;
                    TotalNum.CreateTime = DateTime.Now;
                    _JointOfficeContext.TotalNum.Add(TotalNum);

                    var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.uid).FirstOrDefault();
                    if (totalNum != null)
                    {
                        totalNum.PingLunNum += 1;
                    }
                }
            }
            if (para.type == "3")
            {
                if (work_Task != null)
                {
                    SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                    var parems = SendHelper.getXiaoXiParams(para.uid, "3");
                    SendHelper.SendXiaoXi("你发出的任务已取消", work_Task.MemberId, parems);
                }
            }
            else if (para.type == "4")
            {
                if (workProgram != null)
                {
                    SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                    var parems = SendHelper.getXiaoXiParams(para.uid, "4");
                    SendHelper.SendXiaoXi("你发出的日程已取消", workProgram.MemberId, parems);
                }
            }
            else if (para.type == "5")
            {
                if (work_Order != null)
                {
                    SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                    var parems = SendHelper.getXiaoXiParams(para.uid, "5");
                    SendHelper.SendXiaoXi("你发出的指令已取消", work_Order.MemberId, parems);
                }
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 指令  继续执行
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge ContinueExe(ExeInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var workOrder = _JointOfficeContext.Work_Order.Where(t => t.Id == para.uid).FirstOrDefault();
            if (workOrder == null)
            {
                throw new BusinessTureException("此指令已删除");
            }
            var execute = _JointOfficeContext.Execute_Content.Where(t => t.UId == para.uid).FirstOrDefault();
            if (workOrder != null)
            {
                workOrder.State = 0;

                if (execute != null)
                {
                    execute.State = 0;
                    execute.Content = "";
                    execute.ExecuteDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    execute.PhoneModel = para.phoneModel;

                    Comment_Body Comment_Body = new Comment_Body();
                    Comment_Body.Id = Guid.NewGuid().ToString();
                    Comment_Body.PingLunMemberId = memberid;
                    var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                    Comment_Body.Name = memberInfo.Name;
                    Comment_Body.Picture = memberInfo.Picture;
                    Comment_Body.Body = "继续执行。 " + para.body;
                    Comment_Body.PingLunTime = DateTime.Now;
                    Comment_Body.UId = para.uid;
                    Comment_Body.MemberId = para.memberid;
                    Comment_Body.PId = "";
                    Comment_Body.OtherBody = "";
                    Comment_Body.PersonId = "";
                    Comment_Body.PersonName = "";
                    Comment_Body.Type = para.type;
                    Comment_Body.IsExeComment = 1;
                    Comment_Body.PictureList = para.appendPicture;
                    Comment_Body.Voice = para.voice;
                    Comment_Body.VoiceLength = para.voiceLength;
                    Comment_Body.Annex = para.annex;
                    Comment_Body.PhoneModel = para.phoneModel;
                    Comment_Body.ATPerson = para._person;
                    _JointOfficeContext.Comment_Body.Add(Comment_Body);

                    TotalNum TotalNum = new TotalNum();
                    TotalNum.Id = Guid.NewGuid().ToString();
                    TotalNum.Type = "7+5";
                    TotalNum.UId = "";
                    TotalNum.PId = Comment_Body.Id;
                    TotalNum.P_UId = para.uid;
                    TotalNum.DianZanNum = 0;
                    TotalNum.ZhuanFaNum = 0;
                    TotalNum.PingLunNum = 0;
                    TotalNum.CreateTime = DateTime.Now;
                    _JointOfficeContext.TotalNum.Add(TotalNum);

                    var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.uid).FirstOrDefault();
                    if (totalNum != null)
                    {
                        totalNum.PingLunNum += 1;
                    }
                }
            }
            _JointOfficeContext.SaveChanges();
            SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
            var parems = SendHelper.getXiaoXiParams(para.uid, "5");
            SendHelper.SendXiaoXi("你已执行的指令需继续执行", memberid, parems);

            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 评论
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge PingLun(Comment_Body para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            Comment_Body Comment_Body = new Comment_Body();
            Comment_Body.Id = Guid.NewGuid().ToString();
            Comment_Body.PingLunMemberId = memberid;
            Comment_Body.Name = memberInfo.Name;
            Comment_Body.Picture = memberInfo.Picture;
            Comment_Body.Body = para.Body;
            Comment_Body.PingLunTime = DateTime.Now;
            Comment_Body.UId = para.UId;
            Comment_Body.MemberId = para.MemberId;
            Comment_Body.PId = para.PId;
            if (!para.Type.Contains("+"))
            {
                Comment_Body.OtherBody = "";
                Comment_Body.PersonId = "";
                Comment_Body.PersonName = "";
            }
            else
            {
                var comment_Body = _JointOfficeContext.Comment_Body.Where(t => t.Id == para.PId).FirstOrDefault();
                var dianping_Body = _JointOfficeContext.DianPing_Body.Where(t => t.Id == para.PId).FirstOrDefault();
                if (comment_Body != null)
                {
                    Comment_Body.OtherBody = comment_Body.Body;
                    Comment_Body.PersonId = comment_Body.PingLunMemberId;
                    Comment_Body.PersonName = comment_Body.Name;
                }
                if (dianping_Body != null)
                {
                    Comment_Body.OtherBody = dianping_Body.Body;
                    Comment_Body.PersonId = dianping_Body.DianPingMemberId;
                    Comment_Body.PersonName = dianping_Body.Name;
                }
            }
            Comment_Body.Type = para.Type;
            Comment_Body.IsExeComment = 0;
            Comment_Body.PictureList = para.PictureList;
            Comment_Body.Voice = para.Voice;
            Comment_Body.VoiceLength = para.VoiceLength;
            Comment_Body.Annex = para.Annex;
            Comment_Body.PhoneModel = para.PhoneModel;
            Comment_Body.ATPerson = para.ATPerson;
            _JointOfficeContext.Comment_Body.Add(Comment_Body);

            var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.UId).FirstOrDefault();
            if (totalNum != null)
            {
                totalNum.PingLunNum += 1;
            }

            TotalNum TotalNum = new TotalNum();
            TotalNum.Id = Guid.NewGuid().ToString();
            if (para.Type.Contains("+"))
            {
                TotalNum.Type = "7+7";
            }
            else
            {
                TotalNum.Type = "7+" + para.Type;
            }
            TotalNum.UId = "";
            TotalNum.PId = Comment_Body.Id;
            TotalNum.P_UId = para.UId;
            TotalNum.DianZanNum = 0;
            TotalNum.ZhuanFaNum = 0;
            TotalNum.PingLunNum = 0;
            TotalNum.CreateTime = DateTime.Now;
            _JointOfficeContext.TotalNum.Add(TotalNum);

            _JointOfficeContext.SaveChanges();
            var oldmemberid = Comment_Body.PersonId;
            if (Comment_Body.PersonId == "")
            {
                oldmemberid = Comment_Body.MemberId;
            }

            if (Comment_Body.PingLunMemberId != oldmemberid)
            {
                var id = Comment_Body.PId;
                if (Comment_Body.PId == "")
                {
                    id = Comment_Body.UId;
                }
                var type = Comment_Body.Type;
                if (Comment_Body.Type.Contains("+"))
                {
                    type = Comment_Body.Type.Replace("7+", "");
                }
                if (type == "1" || type == "2" || type == "3" || type == "4" || type == "5")
                {
                    SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                    var parems = SendHelper.getXiaoXiParams(Comment_Body.UId, type);

                    SendHelper.SendXiaoXi("收到一条回复", oldmemberid, parems);
                }
            }
            return Message.SuccessMeaasge("回复成功");
        }
        /// <summary>
        /// 点评
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge DianPing(DianPing_Body para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();

            if (para.Type == "2")
            {
                var workLog = _JointOfficeContext.Work_Log.Where(t => t.Id == para.UId).FirstOrDefault();
                if (workLog != null)
                {
                    if (workLog.State == 2)
                    {
                        throw new BusinessTureException("此日志已删除");
                    }
                    if (workLog.State == 1)
                    {
                        throw new BusinessTureException("此日志已点评，不能再次点评");
                    }
                    workLog.State = 1;
                }
            }

            var isDianPing = _JointOfficeContext.DianPing_Body.Where(t => t.UId == para.UId).FirstOrDefault();
            if (isDianPing != null)
            {
                throw new BusinessTureException("此工作已点评，不能再次点评");
            }

            DianPing_Body DianPing_Body = new DianPing_Body();
            DianPing_Body.Id = Guid.NewGuid().ToString();
            DianPing_Body.DianPingMemberId = memberid;
            DianPing_Body.Name = memberInfo.Name;
            DianPing_Body.Picture = memberInfo.Picture;
            DianPing_Body.Body = "点评完成。 " + para.Body;
            DianPing_Body.DianPingTime = DateTime.Now;
            DianPing_Body.UId = para.UId;
            DianPing_Body.MemberId = para.MemberId;
            DianPing_Body.Type = para.Type;
            DianPing_Body.Grade = para.Grade;
            DianPing_Body.State = 1;
            DianPing_Body.PictureList = para.PictureList;
            DianPing_Body.Voice = para.Voice;
            DianPing_Body.VoiceLength = para.VoiceLength;
            DianPing_Body.Annex = para.Annex;
            DianPing_Body.PhoneModel = para.PhoneModel;
            DianPing_Body.ATPerson = para.ATPerson;
            _JointOfficeContext.DianPing_Body.Add(DianPing_Body);

            var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.UId).FirstOrDefault();
            if (totalNum != null)
            {
                totalNum.PingLunNum += 1;
            }

            TotalNum TotalNum = new TotalNum();
            TotalNum.Id = Guid.NewGuid().ToString();
            TotalNum.Type = "7+" + para.Type;
            TotalNum.UId = "";
            TotalNum.PId = DianPing_Body.Id;
            TotalNum.P_UId = para.UId;
            TotalNum.DianZanNum = 0;
            TotalNum.ZhuanFaNum = 0;
            TotalNum.PingLunNum = 0;
            TotalNum.CreateTime = DateTime.Now;
            _JointOfficeContext.TotalNum.Add(TotalNum);

            _JointOfficeContext.SaveChanges();
            if (para.Type == "2")
            {
                var work_log = _JointOfficeContext.Work_Log.Where(t => t.Id == para.UId).FirstOrDefault();
                if (work_log != null)
                {
                    SendHelper SendHelper = new SendHelper(_PrincipalBase, _memoryCache, config);
                    var parems = SendHelper.getXiaoXiParams(para.UId, "2");
                    SendHelper.SendXiaoXi("日志已点评", work_log.MemberId, parems);
                }
            }
            return Message.SuccessMeaasge("点评成功");
        }
        /// <summary>
        /// 回执
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge Rece(ReceInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            var receipts = _JointOfficeContext.Receipts.Where(t => t.UId == para.uid && t.MemberId == memberid).FirstOrDefault();
            if (receipts != null)
            {
                if (!string.IsNullOrEmpty(receipts.Body))
                {
                    throw new BusinessTureException("此工作已回执，不能再次回执");
                }
                receipts.Body = para.body;
                receipts.PhoneModel = para.phoneModel;

                Comment_Body Comment_Body = new Comment_Body();
                Comment_Body.Id = Guid.NewGuid().ToString();
                Comment_Body.PingLunMemberId = memberid;
                var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                Comment_Body.Name = memberInfo.Name;
                Comment_Body.Picture = memberInfo.Picture;
                Comment_Body.Body = para.body;
                Comment_Body.PingLunTime = DateTime.Now;
                Comment_Body.UId = para.uid;
                Comment_Body.MemberId = para.memberid;
                Comment_Body.PId = "";
                Comment_Body.OtherBody = "";
                Comment_Body.PersonId = "";
                Comment_Body.PersonName = "";
                Comment_Body.Type = para.type;
                Comment_Body.IsExeComment = 1;
                Comment_Body.PictureList = para.appendPicture;
                Comment_Body.Annex = para.annex;
                Comment_Body.Voice = para.voice;
                Comment_Body.VoiceLength = para.voiceLength;
                Comment_Body.PhoneModel = para.phoneModel;
                Comment_Body.ATPerson = para._person;
                _JointOfficeContext.Comment_Body.Add(Comment_Body);

                var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == para.uid).FirstOrDefault();
                if (totalNum != null)
                {
                    totalNum.PingLunNum += 1;
                }

                TotalNum TotalNum = new TotalNum();
                TotalNum.Id = Guid.NewGuid().ToString();
                TotalNum.Type = "7+" + para.type;
                TotalNum.UId = "";
                TotalNum.PId = Comment_Body.Id;
                TotalNum.P_UId = para.uid;
                TotalNum.DianZanNum = 0;
                TotalNum.ZhuanFaNum = 0;
                TotalNum.PingLunNum = 0;
                TotalNum.CreateTime = DateTime.Now;
                _JointOfficeContext.TotalNum.Add(TotalNum);

                _JointOfficeContext.SaveChanges();
            }
            return Message.SuccessMeaasge("回复成功");
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<PersonDynamic_info> GetAllSearchList(AllSearch para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<PersonDynamic_info>();
                return Return.Return();
            }
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            //var Companyid = _JointOfficeContext.Member_Company.Where(t => t.ParentId == "0").FirstOrDefault().Id;
            //if (Companyid == null)
            //{
            //    Companyid = "";
            //}
            var Companyid = "";
            if (!string.IsNullOrEmpty(para.companyId))
            {
                Companyid = para.companyId;
            }
            var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            var memList = _JointOfficeContext.Member_Info.Where(t => t.CompanyIDS.Contains(para.companyId)).ToList();
            var memListStr = "";
            foreach (var item in memList)
            {
                memListStr += item.MemberId;
            }
            List<PersonDynamic_info> lists = new List<PersonDynamic_info>();
            List<PersonDynamic_info> list = new List<PersonDynamic_info>();
            //处理日期入参 规范年月日格式
            if (para.beginTime == "" || para.beginTime == null)
            {
                para.beginTime = "1901-01-01";
            }
            if (para.stopTime == "" || para.stopTime == null)
            {
                para.stopTime = "2200-12-31";
            }
            //if (para.beginTime.Contains("T"))
            //{
            //    para.beginTime = string.Format("{0:d}", Convert.ToDateTime(para.beginTime));
            //}
            //if (para.stopTime.Contains("T"))
            //{
            //    para.stopTime = string.Format("{0:d}", Convert.ToDateTime(para.stopTime));
            //}
            para.beginTime = WorkDetails.GetYMD(para.beginTime) + " 00:00:00";
            para.stopTime = WorkDetails.GetYMD(para.stopTime) + " 23:59:59";
            //if (para.beginTime == para.stopTime)
            //{
            //    para.stopTime = string.Format("{0:d}", Convert.ToDateTime(para.stopTime).AddDays(1));
            //}
            var allPage = 0;
            int allNum = 0;
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            para.body = para.body.Replace("'", "");
            var sql = @"exec AllSearch '" + memListStr + "','" + para.body + "','" + para.type + "','" + para.beginTime + "','" + para.stopTime + "','" + memberid + "','" + Companyid + "','" + info.ZhuBuMen + "'," + begin + "," + end;
            var sql1 = @"exec AllSearchCount '" + memListStr + "','" + para.body + "','" + para.type + "','" + para.beginTime + "','" + para.stopTime + "','" + memberid + "','" + Companyid + "','" + info.ZhuBuMen + "'";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                lists = conText.Query<PersonDynamic_info>(sql).ToList();
                allNum = conText.Query<int>(sql1).FirstOrDefault();
                switch (para.type)
                {
                    case "":
                        list = lists;
                        break;
                    case "0":
                        list = lists;
                        break;
                    case "1":
                        switch (para.state)
                        {
                            case "":
                                list = lists;
                                break;
                            case "0":
                                list = lists.Where(t => t.state == "审批中").ToList();
                                break;
                            case "1":
                                list = lists.Where(t => t.state == "已批准").ToList();
                                break;
                            case "2":
                                list = lists.Where(t => t.state == "已取消").ToList();
                                break;
                            case "3":
                                list = lists.Where(t => t.state == "未批准").ToList();
                                break;
                        }
                        break;
                    case "2":
                        switch (para.state)
                        {
                            case "":
                                list = lists;
                                break;
                            case "1":
                                list = lists.Where(t => t.moban == "1").ToList();
                                break;
                            case "2":
                                list = lists.Where(t => t.moban == "2").ToList();
                                break;
                            case "3":
                                list = lists.Where(t => t.moban == "3").ToList();
                                break;
                        }
                        break;
                    case "3":
                        switch (para.state)
                        {
                            case "":
                                list = lists;
                                break;
                            case "0":
                                list = lists.Where(t => t.state == "执行中").ToList();
                                break;
                            case "1":
                                list = lists.Where(t => t.state == "已执行").ToList();
                                break;
                            case "2":
                                list = lists.Where(t => t.state == "已取消").ToList();
                                break;
                        }
                        break;
                    case "4":
                        switch (para.state)
                        {
                            case "":
                                list = lists;
                                break;
                            case "0":
                                list = lists.Where(t => t.state == "未开始").ToList();
                                break;
                            case "1":
                                list = lists.Where(t => t.state == "已开始").ToList();
                                break;
                        }
                        break;
                    case "5":
                        switch (para.state)
                        {
                            case "":
                                list = lists;
                                break;
                            case "0":
                                list = lists.Where(t => t.state == "执行中").ToList();
                                break;
                            case "1":
                                list = lists.Where(t => t.state == "已执行").ToList();
                                break;
                            case "2":
                                list = lists.Where(t => t.state == "已取消").ToList();
                                break;
                        }
                        break;
                    case "8":
                        list = lists;
                        break;
                    case "9":
                        list = lists;
                        break;
                }
                var allPage1 = allNum / para.count;
                if (allNum % para.count != 0)
                {
                    allPage1 += 1;
                }
                allPage = allPage1;
            }
            //大类  list  处理
            list = WorkDetails.GetPersonDynamic_info(list);
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<PersonDynamic_info>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allPages = allPage;
            res.showapi_res_body.allNum = allNum;
            return res;
        }
        /// <summary>
        /// 普通搜索
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<PersonDynamic_info> GetSearchList(Search para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<PersonDynamic_info>();
                return Return.Return();
            }
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            List<PersonDynamic_info> list = new List<PersonDynamic_info>();
            int allPages = 0;
            int allNum = 0;
            //var Companyid = _JointOfficeContext.Member_Company.Where(t => t.ParentId == "0").FirstOrDefault().Id;
            //if (Companyid == null)
            //{
            //    Companyid = "";
            //}
            var Companyid = "";
            if (!string.IsNullOrEmpty(para.companyId))
            {
                Companyid = para.companyId;
            }
            var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            if (para.body != null && para.body != "")
            {
                var memList = _JointOfficeContext.Member_Info.Where(t => t.CompanyIDS.Contains(para.companyId)).ToList();
                var memListStr = "";
                foreach (var item in memList)
                {
                    memListStr += item.MemberId;
                }
                var begin = para.page * para.count + 1;
                var end = (para.page + 1) * para.count;
                var sql = @"exec Search '" + para.body + "','" + memberid + "','" + memListStr + "','" + Companyid + "','" + info.ZhuBuMen + "'," + begin + "," + end;
                var sql1 = @"exec SearchCount '" + para.body + "','" + memberid + "','" + memListStr + "','" + Companyid + "','" + info.ZhuBuMen + "'";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    list = conText.Query<PersonDynamic_info>(sql).ToList();
                    allNum = conText.Query<int>(sql1).FirstOrDefault();
                    allPages = allNum / para.count;
                    if (allNum % para.count != 0)
                    {
                        allPages += 1;
                    }
                }
                //大类  list  处理
                WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                list = WorkDetails.GetPersonDynamic_info(list);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<PersonDynamic_info>();
            res.showapi_res_body.contentlist = list;
            res.showapi_res_body.allPages = allPages;
            return res;
        }
        /// <summary>
        /// 高级搜索
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<PersonDynamic_info> GetHighSearchList(HighSearch para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<PersonDynamic_info>();
                return Return.Return();
            }
            Showapi_Res_List<PersonDynamic_info> res = new Showapi_Res_List<PersonDynamic_info>();
            List<PersonDynamic_info> lists = new List<PersonDynamic_info>();
            List<PersonDynamic_info> list = new List<PersonDynamic_info>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            //处理日期入参 规范年月日格式
            if (para.beginTime == "" || para.beginTime == null)
            {
                para.beginTime = "1901-01-01";
            }
            if (para.stopTime == "" || para.stopTime == null)
            {
                para.stopTime = "2200-12-31";
            }
            //if (para.beginTime.Contains("T"))
            //{
            //    para.beginTime = string.Format("{0:d}", Convert.ToDateTime(para.beginTime));
            //}
            //if (para.stopTime.Contains("T"))
            //{
            //    para.stopTime = string.Format("{0:d}", Convert.ToDateTime(para.stopTime));
            //}
            para.beginTime = WorkDetails.GetYMD(para.beginTime) + " 00:00:00";
            para.stopTime = WorkDetails.GetYMD(para.stopTime) + " 23:59:59";
            //if (para.beginTime == para.stopTime)
            //{
            //    para.stopTime = string.Format("{0:d}", Convert.ToDateTime(para.stopTime).AddDays(1));
            //}
            var allPage = 0;
            int allNum = 0;
            //var Companyid = _JointOfficeContext.Member_Company.Where(t => t.ParentId == "0").FirstOrDefault().Id;
            //if (Companyid == null)
            //{
            //    Companyid = "";
            //}
            var Companyid = "";
            if (!string.IsNullOrEmpty(para.companyId))
            {
                Companyid = para.companyId;
            }
            var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            var memList = _JointOfficeContext.Member_Info.Where(t => t.CompanyIDS.Contains(para.companyId)).ToList();
            var memListStr = "";
            foreach (var item in memList)
            {
                memListStr += item.MemberId;
            }
            var begin = para.page * para.count + 1;
            var end = (para.page + 1) * para.count;
            var sql = @"exec HighSearch '" + memListStr + "','" + para.type + "','" + para.beginTime + "','" + para.stopTime + "','" + memberid + "','" + Companyid + "','" + info.ZhuBuMen + "'," + begin + "," + end;
            var sql1 = @"exec HighSearchCount '" + memListStr + "','" + para.type + "','" + para.beginTime + "','" + para.stopTime + "','" + memberid + "','" + Companyid + "','" + info.ZhuBuMen + "'";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                lists = conText.Query<PersonDynamic_info>(sql).ToList();
                allNum = conText.Query<int>(sql1).FirstOrDefault();
                switch (para.type)
                {
                    case "":
                        list = lists;
                        break;
                    case "0":
                        list = lists;
                        break;
                    case "1":
                        switch (para.state)
                        {
                            case "":
                                list = lists;
                                break;
                            case "0":
                                list = lists.Where(t => t.state == "审批中").ToList();
                                break;
                            case "1":
                                list = lists.Where(t => t.state == "已批准").ToList();
                                break;
                            case "2":
                                list = lists.Where(t => t.state == "已取消").ToList();
                                break;
                            case "3":
                                list = lists.Where(t => t.state == "未批准").ToList();
                                break;
                        }
                        break;
                    case "2":
                        switch (para.state)
                        {
                            case "":
                                list = lists;
                                break;
                            case "1":
                                list = lists.Where(t => t.moban == "1").ToList();
                                break;
                            case "2":
                                list = lists.Where(t => t.moban == "2").ToList();
                                break;
                            case "3":
                                list = lists.Where(t => t.moban == "3").ToList();
                                break;
                        }
                        break;
                    case "3":
                        switch (para.state)
                        {
                            case "":
                                list = lists;
                                break;
                            case "0":
                                list = lists.Where(t => t.state == "执行中").ToList();
                                break;
                            case "1":
                                list = lists.Where(t => t.state == "已执行").ToList();
                                break;
                            case "2":
                                list = lists.Where(t => t.state == "已取消").ToList();
                                break;
                        }
                        break;
                    case "4":
                        switch (para.state)
                        {
                            case "":
                                list = lists;
                                break;
                            case "0":
                                list = lists.Where(t => t.state == "未开始").ToList();
                                break;
                            case "1":
                                list = lists.Where(t => t.state == "已开始").ToList();
                                break;
                        }
                        break;
                    case "5":
                        switch (para.state)
                        {
                            case "":
                                list = lists;
                                break;
                            case "0":
                                list = lists.Where(t => t.state == "执行中").ToList();
                                break;
                            case "1":
                                list = lists.Where(t => t.state == "已执行").ToList();
                                break;
                            case "2":
                                list = lists.Where(t => t.state == "已取消").ToList();
                                break;
                        }
                        break;
                    case "8":
                        list = lists;
                        break;
                    case "9":
                        list = lists;
                        break;
                }
                var allPage1 = allNum / para.count;
                if (allNum % para.count != 0)
                {
                    allPage1 += 1;
                }
                allPage = allPage1;
            }
            //大类  list  处理
            list = WorkDetails.GetPersonDynamic_info(list);
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<PersonDynamic_info>();
            res.showapi_res_body.allPages = allPage;
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 高级搜索 列表
        /// </summary>
        public Showapi_Res_List<HighSearchType> GetHighSearchTypeList()
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<HighSearchType>();
                return Return.Return();
            }
            Showapi_Res_List<HighSearchType> res = new Showapi_Res_List<HighSearchType>();
            List<HighSearchType> list1 = new List<HighSearchType>();
            string[] array1 = new string[] { "0", "1", "2", "3", "4", "5", "8", "9" };
            string[] array2 = new string[] { "全部类型", "审批", "日志", "任务", "日程", "指令", "公告", "分享" };
            for (int i = 0; i < array1.Length; i++)
            {
                HighSearchType HighSearchType = new HighSearchType();
                HighSearchType.typeNum = array1[i];
                HighSearchType.typeName = array2[i];
                List<HighSearchTypeState> list2 = new List<HighSearchTypeState>();
                if (HighSearchType.typeNum == "0")
                {
                    HighSearchTypeState HighSearchTypeState = new HighSearchTypeState();
                    HighSearchTypeState.typeNum = "0";
                    HighSearchTypeState.stateNum = "0";
                    HighSearchTypeState.stateName = "全部状态";
                    list2.Add(HighSearchTypeState);
                }
                if (HighSearchType.typeNum == "1")
                {
                    string[] array11 = new string[] { "", "0", "1", "2", "3" };
                    string[] array21 = new string[] { "全部状态", "审批中", "已批准", "已取消", "未批准" };
                    for (int i1 = 0; i1 < array11.Length; i1++)
                    {
                        HighSearchTypeState HighSearchTypeState = new HighSearchTypeState();
                        HighSearchTypeState.typeNum = "1";
                        HighSearchTypeState.stateNum = array11[i1];
                        HighSearchTypeState.stateName = array21[i1];
                        list2.Add(HighSearchTypeState);
                    }
                }
                if (HighSearchType.typeNum == "2")
                {
                    string[] array11 = new string[] { "", "1", "2", "3" };
                    string[] array21 = new string[] { "全部状态", "日计划", "周计划", "月计划" };
                    for (int i1 = 0; i1 < array11.Length; i1++)
                    {
                        HighSearchTypeState HighSearchTypeState = new HighSearchTypeState();
                        HighSearchTypeState.typeNum = "2";
                        HighSearchTypeState.stateNum = array11[i1];
                        HighSearchTypeState.stateName = array21[i1];
                        list2.Add(HighSearchTypeState);
                    }
                }
                if (HighSearchType.typeNum == "3")
                {
                    string[] array11 = new string[] { "", "0", "1", "2" };
                    string[] array21 = new string[] { "全部状态", "执行中", "已完成", "已取消" };
                    for (int i1 = 0; i1 < array11.Length; i1++)
                    {
                        HighSearchTypeState HighSearchTypeState = new HighSearchTypeState();
                        HighSearchTypeState.typeNum = "3";
                        HighSearchTypeState.stateNum = array11[i1];
                        HighSearchTypeState.stateName = array21[i1];
                        list2.Add(HighSearchTypeState);
                    }
                }
                if (HighSearchType.typeNum == "4")
                {
                    string[] array11 = new string[] { "", "0", "1" };
                    string[] array21 = new string[] { "全部状态", "未开始", "已开始" };
                    for (int i1 = 0; i1 < array11.Length; i1++)
                    {
                        HighSearchTypeState HighSearchTypeState = new HighSearchTypeState();
                        HighSearchTypeState.typeNum = "4";
                        HighSearchTypeState.stateNum = array11[i1];
                        HighSearchTypeState.stateName = array21[i1];
                        list2.Add(HighSearchTypeState);
                    }
                }
                if (HighSearchType.typeNum == "5")
                {
                    string[] array11 = new string[] { "", "0", "1", "2" };
                    string[] array21 = new string[] { "全部状态", "执行中", "已执行", "已取消" };
                    for (int i1 = 0; i1 < array11.Length; i1++)
                    {
                        HighSearchTypeState HighSearchTypeState = new HighSearchTypeState();
                        HighSearchTypeState.typeNum = "5";
                        HighSearchTypeState.stateNum = array11[i1];
                        HighSearchTypeState.stateName = array21[i1];
                        list2.Add(HighSearchTypeState);
                    }
                }
                if (HighSearchType.typeNum == "8")
                {
                    string[] array11 = new string[] { "" };
                    string[] array21 = new string[] { "全部状态" };
                    for (int i1 = 0; i1 < array11.Length; i1++)
                    {
                        HighSearchTypeState HighSearchTypeState = new HighSearchTypeState();
                        HighSearchTypeState.typeNum = "8";
                        HighSearchTypeState.stateNum = array11[i1];
                        HighSearchTypeState.stateName = array21[i1];
                        list2.Add(HighSearchTypeState);
                    }
                }
                if (HighSearchType.typeNum == "9")
                {
                    string[] array11 = new string[] { "" };
                    string[] array21 = new string[] { "全部状态" };
                    for (int i1 = 0; i1 < array11.Length; i1++)
                    {
                        HighSearchTypeState HighSearchTypeState = new HighSearchTypeState();
                        HighSearchTypeState.typeNum = "9";
                        HighSearchTypeState.stateNum = array11[i1];
                        HighSearchTypeState.stateName = array21[i1];
                        list2.Add(HighSearchTypeState);
                    }
                }
                HighSearchType.highSearchTypeState = list2;
                var str = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
                HighSearchType.highSearchTypeStateStr = str;
                list1.Add(HighSearchType);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<HighSearchType>();
            res.showapi_res_body.contentlist = list1;

            return res;
        }
        /// <summary>
        /// 日志周计划  周列表
        /// </summary>
        public Showapi_Res_List<GetOneDay> GetOneDay(GetOneDayInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<GetOneDay>();
                return Return.Return();
            }
            Showapi_Res_List<GetOneDay> res = new Showapi_Res_List<GetOneDay>();
            List<GetOneDay> list = new List<GetOneDay>();
            //GetWorkDays GetWorkDays = new GetWorkDays();
            var monday = GetWorkDays.CalculateFirstDateOfWeek(Convert.ToDateTime(para.oneDay));
            var sunday = GetWorkDays.CalculateLastDateOfWeek(Convert.ToDateTime(para.oneDay));

            var newDay = Convert.ToDateTime(para.oneDay).AddYears(-1);
            var mondayBefore = GetWorkDays.CalculateFirstDateOfWeek(Convert.ToDateTime(newDay));

            if (true)
            {
                GetOneDay GetOneDay = new GetOneDay();
                GetOneDay.mondayStr = monday.ToString("yyyy-MM-dd");
                GetOneDay.sundayStr = sunday.ToString("yyyy-MM-dd");
                list.Add(GetOneDay);
            }
            while (monday != mondayBefore)
            {
                GetOneDay GetOneDay = new GetOneDay();
                monday = monday.AddDays(-7);
                sunday = sunday.AddDays(-7);
                GetOneDay.mondayStr = monday.ToString("yyyy-MM-dd");
                GetOneDay.sundayStr = sunday.ToString("yyyy-MM-dd");
                list.Add(GetOneDay);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<GetOneDay>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 点击更多返回的状态值
        /// </summary>
        public Showapi_Res_Single<FocusCollectionDeleteState> GetFocusCollectionDeleteState(FocusCollectionDeleteStateInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<FocusCollectionDeleteState>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<FocusCollectionDeleteState> res = new Showapi_Res_Single<FocusCollectionDeleteState>();
            FocusCollectionDeleteState FocusCollectionDeleteState = new FocusCollectionDeleteState();
            var focus = _JointOfficeContext.News_Focus.Where(t => t.UId == para.uid && t.MemberId == memberid).FirstOrDefault();
            if (focus != null)
            {
                FocusCollectionDeleteState.isFocus = "1";
            }
            else
            {
                FocusCollectionDeleteState.isFocus = "0";
            }

            var collection = _JointOfficeContext.News_Collection.Where(t => t.UId == para.uid && t.MemberId == memberid).FirstOrDefault();
            if (collection != null)
            {
                FocusCollectionDeleteState.isCollection = "1";
            }
            else
            {
                FocusCollectionDeleteState.isCollection = "0";
            }

            switch (para.type)
            {
                case "1":
                    var workApproval = _JointOfficeContext.Work_Approval.Where(t => t.Id == para.uid).FirstOrDefault();
                    var approvalContent = _JointOfficeContext.Approval_Content.Where(t => t.UId == para.uid && t.OtherMemberId == memberid && t.IsMeApproval == "1").FirstOrDefault();
                    if (workApproval.MemberId == memberid)
                    {
                        if (workApproval.State == 0)
                        {
                            FocusCollectionDeleteState.isDelete = "3";
                        }
                        if (workApproval.State == 1 || workApproval.State == 3)
                        {
                            FocusCollectionDeleteState.isDelete = "";
                        }
                        if (workApproval.State == 2)
                        {
                            FocusCollectionDeleteState.isDelete = "4";
                        }
                        if (workApproval.State == 4)
                        {
                            FocusCollectionDeleteState.isDelete = "4";
                        }
                    }
                    else if (approvalContent != null)
                    {
                        FocusCollectionDeleteState.isDelete = "5";
                    }
                    else
                    {
                        FocusCollectionDeleteState.isDelete = "";
                    }
                    break;
                case "2":
                    var workLog = _JointOfficeContext.Work_Log.Where(t => t.Id == para.uid).FirstOrDefault();
                    if (workLog.MemberId == memberid)
                    {
                        if (workLog.State == 0)
                        {
                            FocusCollectionDeleteState.isDelete = "1";
                        }
                        else
                        {
                            FocusCollectionDeleteState.isDelete = "";
                        }
                    }
                    else
                    {
                        FocusCollectionDeleteState.isDelete = "";
                    }
                    break;
                case "3":
                    var workTask = _JointOfficeContext.Work_Task.Where(t => t.Id == para.uid).FirstOrDefault();
                    var dianping1 = _JointOfficeContext.DianPing_Body.Where(t => t.UId == para.uid).FirstOrDefault();
                    if (workTask.MemberId == memberid)
                    {
                        if (dianping1 == null)
                        {
                            FocusCollectionDeleteState.isDelete = "3";
                            if (workTask.State == 2)
                            {
                                FocusCollectionDeleteState.isDelete = "1";
                            }
                        }
                        else
                        {
                            FocusCollectionDeleteState.isDelete = "";
                        }
                    }
                    else if (workTask.Executor.Contains(memberid))
                    {
                        var executor1 = _JointOfficeContext.Execute_Content.Where(t => t.UId == para.uid && t.OtherMemberId == memberid).FirstOrDefault();
                        if (executor1.State == 0)
                        {
                            FocusCollectionDeleteState.isDelete = "2";
                        }
                        else
                        {
                            FocusCollectionDeleteState.isDelete = "";
                        }
                    }
                    else
                    {
                        FocusCollectionDeleteState.isDelete = "";
                    }
                    break;
                case "4":
                    var workProgram = _JointOfficeContext.Work_Program.Where(t => t.Id == para.uid).FirstOrDefault();
                    if (workProgram.MemberId == memberid)
                    {
                        FocusCollectionDeleteState.isDelete = "1";
                    }
                    else if (workProgram.JoinPerson.Contains(memberid))
                    {
                        FocusCollectionDeleteState.isDelete = "2";
                    }
                    else
                    {
                        FocusCollectionDeleteState.isDelete = "";
                    }
                    break;
                case "5":
                    var workOrder = _JointOfficeContext.Work_Order.Where(t => t.Id == para.uid).FirstOrDefault();
                    var dianping2 = _JointOfficeContext.DianPing_Body.Where(t => t.UId == para.uid).FirstOrDefault();
                    if (workOrder.MemberId == memberid)
                    {
                        if (dianping2 == null)
                        {
                            FocusCollectionDeleteState.isDelete = "3";
                            if (workOrder.State == 2)
                            {
                                FocusCollectionDeleteState.isDelete = "1";
                            }
                        }
                        else
                        {
                            FocusCollectionDeleteState.isDelete = "";
                        }
                    }
                    else if (workOrder.Executor.Contains(memberid))
                    {
                        if (workOrder.State == 0)
                        {
                            FocusCollectionDeleteState.isDelete = "2";
                        }
                        if (workOrder.State == 1 || workOrder.State == 2)
                        {
                            FocusCollectionDeleteState.isDelete = "";
                        }
                    }
                    else
                    {
                        FocusCollectionDeleteState.isDelete = "";
                    }
                    break;
                case "8":
                    var workAnnouncement = _JointOfficeContext.Work_Announcement.Where(t => t.Id == para.uid).FirstOrDefault();
                    if (workAnnouncement.MemberId == memberid)
                    {
                        FocusCollectionDeleteState.isDelete = "1";
                    }
                    else
                    {
                        FocusCollectionDeleteState.isDelete = "";
                    }
                    break;
                case "9":
                    var workShare = _JointOfficeContext.Work_Share.Where(t => t.Id == para.uid).FirstOrDefault();
                    if (workShare.MemberId == memberid)
                    {
                        FocusCollectionDeleteState.isDelete = "1";
                    }
                    else
                    {
                        FocusCollectionDeleteState.isDelete = "";
                    }
                    break;
            }

            res.showapi_res_code = "200";
            res.showapi_res_body = FocusCollectionDeleteState;
            return res;
        }
        /// <summary>
        /// 获取中国城市列表
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<ChinaCity> GetChinaCity()
        {
            Showapi_Res_List<ChinaCity> res = new Showapi_Res_List<ChinaCity>();
            List<ChinaCity> list = new List<ChinaCity>();
            List<ChinaCity_Province> provinceList = new List<ChinaCity_Province>();
            var sql1 = "select id,name from System_Place where FatherId=0";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                provinceList = conText.Query<ChinaCity_Province>(sql1).ToList();
            }
            foreach (var item in provinceList)
            {
                List<string> cityList = new List<string>();
                ChinaCity ChinaCity = new ChinaCity();
                ChinaCity.province = item.name;
                var sql2 = "select Name from System_Place where FatherId=" + item.id;
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    cityList = conText.Query<string>(sql2).ToList();
                }
                ChinaCity.city = cityList;
                list.Add(ChinaCity);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<ChinaCity>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 获取中国城市列表
        /// </summary>
        public Showapi_Res_List<ChinaCity_Province> GetChinaCityNew()
        {
            Showapi_Res_List<ChinaCity_Province> res = new Showapi_Res_List<ChinaCity_Province>();
            List<ChinaCity_Province> list = new List<ChinaCity_Province>();
            List<ChinaCity_Province> list1 = new List<ChinaCity_Province>();
            var sql1 = "select id,name from System_Place where CurLevel=2";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                list = conText.Query<ChinaCity_Province>(sql1).ToList();
            }
            foreach (var item in list)
            {
                ChinaCity_Province ChinaCity_Province = new ChinaCity_Province();
                ChinaCity_Province.id = item.id;
                ChinaCity_Province.name = item.name;
                list1.Add(ChinaCity_Province);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<ChinaCity_Province>();
            res.showapi_res_body.contentlist = list1;
            return res;
        }
        /// <summary>
        /// 搜索城市
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<ChinaCitySearch> GetChinaCitySearch(ChinaCitySearchInPara para)
        {
            Showapi_Res_List<ChinaCitySearch> res = new Showapi_Res_List<ChinaCitySearch>();
            List<ChinaCitySearch> list = new List<ChinaCitySearch>();
            List<string> nameList = new List<string>();
            var sql = "select Name from System_Place where CurLevel=2 and Name like '%" + para.body + "%'";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                nameList = conText.Query<string>(sql).ToList();
            }
            foreach (var item in nameList)
            {
                ChinaCitySearch ChinaCitySearch = new ChinaCitySearch();
                ChinaCitySearch.cityName = item;
                list.Add(ChinaCitySearch);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<ChinaCitySearch>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 获取抄送范围/参与人详情
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<RangeInfo> GetRangeInfo(FocusInPara para)
        {
            Showapi_Res_List<RangeInfo> res = new Showapi_Res_List<RangeInfo>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            List<RangeInfo> list1 = new List<RangeInfo>();
            switch (para.type)
            {
                case "1":
                    var workApproval = _JointOfficeContext.Work_Approval.Where(t => t.Id == para.id).FirstOrDefault();
                    if (workApproval != null)
                    {
                        if (workApproval.Range != null && workApproval.Range != "")
                        {
                            if (string.IsNullOrEmpty(workApproval.RangeNew))
                            {
                                list1 = WorkDetails.GetRangeInfo(workApproval.MemberId, workApproval.Range);
                            }
                            else
                            {
                                list1 = WorkDetails.GetRangeInfo(workApproval.MemberId, workApproval.RangeNew);
                            }
                        }
                    }
                    break;
                case "2":
                    var workLog = _JointOfficeContext.Work_Log.Where(t => t.Id == para.id).FirstOrDefault();
                    if (workLog != null)
                    {
                        if (workLog.Range != null && workLog.Range != "")
                        {
                            if (string.IsNullOrEmpty(workLog.RangeNew))
                            {
                                list1 = WorkDetails.GetRangeInfo(workLog.MemberId, workLog.Range);
                            }
                            else
                            {
                                list1 = WorkDetails.GetRangeInfo(workLog.MemberId, workLog.RangeNew);
                            }
                        }
                    }
                    break;
                case "3":
                    var workTask = _JointOfficeContext.Work_Task.Where(t => t.Id == para.id).FirstOrDefault();
                    if (workTask != null)
                    {
                        if (workTask.Range != null && workTask.Range != "")
                        {
                            if (string.IsNullOrEmpty(workTask.RangeNew))
                            {
                                list1 = WorkDetails.GetRangeInfo(workTask.MemberId, workTask.Range);
                            }
                            else
                            {
                                list1 = WorkDetails.GetRangeInfo(workTask.MemberId, workTask.RangeNew);
                            }
                        }
                    }
                    break;
                case "4":
                    var workProgram = _JointOfficeContext.Work_Program.Where(t => t.Id == para.id).FirstOrDefault();
                    if (workProgram != null)
                    {
                        if (workProgram.Range != null && workProgram.Range != "")
                        {
                            list1 = WorkDetails.GetRangeInfo(workProgram.MemberId, workProgram.Range);
                        }
                    }
                    break;
                case "4+":
                    var workProgram1 = _JointOfficeContext.Work_Program.Where(t => t.Id == para.id).FirstOrDefault();
                    if (workProgram1 != null)
                    {
                        if (workProgram1.JoinPerson != null && workProgram1.JoinPerson != "")
                        {
                            list1 = WorkDetails.GetRangeInfo(workProgram1.MemberId, workProgram1.JoinPerson);
                        }
                    }
                    break;
                case "5":
                    var workOrder = _JointOfficeContext.Work_Order.Where(t => t.Id == para.id).FirstOrDefault();
                    if (workOrder != null)
                    {
                        if (workOrder.Range != null && workOrder.Range != "")
                        {
                            if (string.IsNullOrEmpty(workOrder.RangeNew))
                            {
                                list1 = WorkDetails.GetRangeInfo(workOrder.MemberId, workOrder.Range);
                            }
                            else
                            {
                                list1 = WorkDetails.GetRangeInfo(workOrder.MemberId, workOrder.RangeNew);
                            }
                        }
                    }
                    break;
                case "8":
                    var workAnnouncement = _JointOfficeContext.Work_Announcement.Where(t => t.Id == para.id).FirstOrDefault();
                    if (workAnnouncement != null)
                    {
                        if (workAnnouncement.Range != null && workAnnouncement.Range != "")
                        {
                            if (string.IsNullOrEmpty(workAnnouncement.RangeNew))
                            {
                                list1 = WorkDetails.GetRangeInfo(workAnnouncement.MemberId, workAnnouncement.Range);
                            }
                            else
                            {
                                list1 = WorkDetails.GetRangeInfo(workAnnouncement.MemberId, workAnnouncement.RangeNew);
                            }
                        }
                    }
                    break;
                case "9":
                    var workShare = _JointOfficeContext.Work_Share.Where(t => t.Id == para.id).FirstOrDefault();
                    if (workShare != null)
                    {
                        if (workShare.Range != null && workShare.Range != "")
                        {
                            if (string.IsNullOrEmpty(workShare.RangeNew))
                            {
                                list1 = WorkDetails.GetRangeInfo(workShare.MemberId, workShare.Range);
                            }
                            else
                            {
                                list1 = WorkDetails.GetRangeInfo(workShare.MemberId, workShare.RangeNew);
                            }
                        }
                    }
                    break;
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<RangeInfo>();
            res.showapi_res_body.contentlist = list1;
            return res;
        }
        /// <summary>
        /// WorkTest
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge WorkTest(WorkTestInPara para)
        {
            Message Message = new Message();
            var www = DateTime.Now;
            var qqq = www.AddDays(Convert.ToInt32(para.date)).ToString();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 工作列表标签顺序  写入
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge WorkListTagDES(WorkListTagDESInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            List<WorkListTagDES> list = new List<WorkListTagDES>();
            var strList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WorkListTagDES>>(para.workListTagDES);
            var strList1 = strList.OrderBy(t => Convert.ToInt32(t.des)).ToList();
            foreach (var item in strList1)
            {
                WorkListTagDES WorkListTagDES = new WorkListTagDES();
                WorkListTagDES.des = item.des;
                WorkListTagDES.name = item.name;
                WorkListTagDES.isUse = item.isUse;
                switch (item.name)
                {
                    case "全部":
                        WorkListTagDES.type = 0;
                        break;
                    case "审批":
                        WorkListTagDES.type = 1;
                        break;
                    case "日志":
                        WorkListTagDES.type = 2;
                        break;
                    case "任务":
                        WorkListTagDES.type = 3;
                        break;
                    case "日程":
                        WorkListTagDES.type = 4;
                        break;
                    case "指令":
                        WorkListTagDES.type = 5;
                        break;
                    case "公告":
                        WorkListTagDES.type = 8;
                        break;
                    case "分享":
                        WorkListTagDES.type = 9;
                        break;
                }
                list.Add(WorkListTagDES);
            }
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            var workListTagOne = _JointOfficeContext.WorkListTag.Where(t => t.MemberId == memberid).FirstOrDefault();
            if (workListTagOne == null)
            {
                WorkListTag WorkListTag = new WorkListTag();
                WorkListTag.Id = Guid.NewGuid().ToString();
                WorkListTag.MemberId = memberid;
                WorkListTag.WorkListTagDES = str;
                _JointOfficeContext.WorkListTag.Add(WorkListTag);
            }
            else
            {
                workListTagOne.WorkListTagDES = str;
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 工作列表标签顺序  获取
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<WorkListTagDES> GetWorkListTagDES()
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<WorkListTagDES>();
                return Return.Return();
            }
            Showapi_Res_List<WorkListTagDES> res = new Showapi_Res_List<WorkListTagDES>();
            List<WorkListTagDES> list = new List<WorkListTagDES>();
            //休眠6秒
            //Thread.Sleep(6000);
            var workListTagOne = _JointOfficeContext.WorkListTag.Where(t => t.MemberId == memberid).FirstOrDefault();
            if (workListTagOne != null)
            {
                var strList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WorkListTagDES>>(workListTagOne.WorkListTagDES);
                foreach (var item in strList)
                {
                    WorkListTagDES WorkListTagDES = new WorkListTagDES();
                    WorkListTagDES.des = item.des;
                    WorkListTagDES.name = item.name;
                    WorkListTagDES.type = item.type;
                    WorkListTagDES.isUse = item.isUse;
                    list.Add(WorkListTagDES);
                }
            }
            else
            {
                string[] array = new string[] { "全部", "审批", "日志", "任务", "日程", "指令", "公告", "分享" };
                foreach (var item in array)
                {
                    WorkListTagDES WorkListTagDES = new WorkListTagDES();
                    switch (item)
                    {
                        case "全部":
                            WorkListTagDES.des = "0";
                            WorkListTagDES.type = 0;
                            WorkListTagDES.isUse = true;
                            break;
                        case "审批":
                            WorkListTagDES.des = "1";
                            WorkListTagDES.type = 1;
                            WorkListTagDES.isUse = true;
                            break;
                        case "日志":
                            WorkListTagDES.des = "2";
                            WorkListTagDES.type = 2;
                            WorkListTagDES.isUse = true;
                            break;
                        case "任务":
                            WorkListTagDES.des = "3";
                            WorkListTagDES.type = 3;
                            WorkListTagDES.isUse = true;
                            break;
                        case "日程":
                            WorkListTagDES.des = "4";
                            WorkListTagDES.type = 4;
                            WorkListTagDES.isUse = true;
                            break;
                        case "指令":
                            WorkListTagDES.des = "5";
                            WorkListTagDES.type = 5;
                            WorkListTagDES.isUse = true;
                            break;
                        case "公告":
                            WorkListTagDES.des = "6";
                            WorkListTagDES.type = 8;
                            WorkListTagDES.isUse = true;
                            break;
                        case "分享":
                            WorkListTagDES.des = "7";
                            WorkListTagDES.type = 9;
                            WorkListTagDES.isUse = true;
                            break;
                    }
                    WorkListTagDES.name = item;
                    list.Add(WorkListTagDES);
                }
                var str = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                WorkListTag WorkListTag = new WorkListTag();
                WorkListTag.Id = Guid.NewGuid().ToString();
                WorkListTag.MemberId = memberid;
                WorkListTag.WorkListTagDES = str;
                _JointOfficeContext.WorkListTag.Add(WorkListTag);
                _JointOfficeContext.SaveChanges();
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<WorkListTagDES>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 单个工作刷新
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<PersonDynamic_info> GetOneWorkInfo(GetOneWorkInfopara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnSingle<PersonDynamic_info>();
                return Return.Return();
            }
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            Showapi_Res_Single<PersonDynamic_info> res = new Showapi_Res_Single<PersonDynamic_info>();
            var Companyid = _JointOfficeContext.Member_Company.Where(t => t.ParentId == "0").FirstOrDefault().Id;
            if (Companyid == null)
            {
                Companyid = "";
            }
            var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
            List<PersonDynamic_info> lists = new List<PersonDynamic_info>();
            List<PersonDynamic_info> list = new List<PersonDynamic_info>();
            var sql = @"exec GetOneWorkInfo '" + para.id + "','" + memberid+"'";
            using (SqlConnection conText = new SqlConnection(constr))
            {
                lists = conText.Query<PersonDynamic_info>(sql).ToList();
                list = lists;
            }
            //大类  list  处理
            list = WorkDetails.GetPersonDynamic_info(list);
            res.showapi_res_code = "200";
            res.showapi_res_body = list[0];
            return res;
        }
        /// <summary>
        /// 获取签到类型
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_List<QianDaoType> GetQianDaoType()
        {
            //var memberid = _PrincipalBase.GetMemberId();
            //if (memberid == null || memberid == "")
            //{
            //    var Return = new ReturnList<QianDaoType>();
            //    return Return.Return();
            //}
            Showapi_Res_List<QianDaoType> res = new Showapi_Res_List<QianDaoType>();
            var list = _PrincipalBase.GetQianDaoType();
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<QianDaoType>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 修改抄送范围
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge UpdateWorkRange(UpdateWorkRangeInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            List<PeoPleInfo> list1 = new List<PeoPleInfo>();
            List<PeoPleInfo> strList = new List<PeoPleInfo>();
            List<PeoPleInfo> rangelistnew = new List<PeoPleInfo>();
            var list2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(para.range);
            switch (para.type)
            {
                case "1":
                    var work_Approval = _JointOfficeContext.Work_Approval.Where(t => t.Id == para.id).FirstOrDefault();
                    if (work_Approval != null)
                    {
                        strList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(work_Approval.RangeNew);
                        list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(work_Approval.Range);
                        foreach (var item in list2)
                        {
                            strList.Add(item);
                            if (item.type == "2")
                            {
                                rangelistnew = WorkDetails.CreateWorkRange(rangelistnew, item.id);
                            }
                            if (item.type == "1")
                            {
                                rangelistnew.Add(item);
                            }
                        }
                        foreach (var item in rangelistnew)
                        {
                            if (!work_Approval.Range.Contains(item.id))
                            {
                                list1.Add(item);
                            }
                        }
                        var str = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                        var str1 = Newtonsoft.Json.JsonConvert.SerializeObject(strList);
                        work_Approval.Range = str;
                        work_Approval.RangeNew = str1;
                    }
                    else
                    {
                        throw new BusinessTureException("此审批已删除");
                    }
                    break;
                case "2":
                    var work_Log = _JointOfficeContext.Work_Log.Where(t => t.Id == para.id).FirstOrDefault();
                    if (work_Log != null)
                    {
                        strList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(work_Log.RangeNew);
                        list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(work_Log.Range);
                        foreach (var item in list2)
                        {
                            strList.Add(item);
                            if (item.type == "2")
                            {
                                rangelistnew = WorkDetails.CreateWorkRange(rangelistnew, item.id);
                            }
                            if (item.type == "1")
                            {
                                rangelistnew.Add(item);
                            }
                        }
                        foreach (var item in rangelistnew)
                        {
                            if (!work_Log.Range.Contains(item.id))
                            {
                                list1.Add(item);
                            }
                        }
                        var str = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                        var str1 = Newtonsoft.Json.JsonConvert.SerializeObject(strList);
                        work_Log.Range = str;
                        work_Log.RangeNew = str1;
                    }
                    else
                    {
                        throw new BusinessTureException("此日志已删除");
                    }
                    break;
                case "3":
                    var work_Task = _JointOfficeContext.Work_Task.Where(t => t.Id == para.id).FirstOrDefault();
                    if (work_Task != null)
                    {
                        strList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(work_Task.RangeNew);
                        list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(work_Task.Range);
                        foreach (var item in list2)
                        {
                            strList.Add(item);
                            if (item.type == "2")
                            {
                                rangelistnew = WorkDetails.CreateWorkRange(rangelistnew, item.id);
                            }
                            if (item.type == "1")
                            {
                                rangelistnew.Add(item);
                            }
                        }
                        foreach (var item in rangelistnew)
                        {
                            if (!work_Task.Range.Contains(item.id))
                            {
                                list1.Add(item);
                            }
                        }
                        var str = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                        var str1 = Newtonsoft.Json.JsonConvert.SerializeObject(strList);
                        work_Task.Range = str;
                        work_Task.RangeNew = str1;
                    }
                    else
                    {
                        throw new BusinessTureException("此任务已删除");
                    }
                    break;
                case "4":
                    break;
                case "5":
                    var work_Order = _JointOfficeContext.Work_Order.Where(t => t.Id == para.id).FirstOrDefault();
                    if (work_Order != null)
                    {
                        strList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(work_Order.RangeNew);
                        list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(work_Order.Range);
                        foreach (var item in list2)
                        {
                            strList.Add(item);
                            if (item.type == "2")
                            {
                                rangelistnew = WorkDetails.CreateWorkRange(rangelistnew, item.id);
                            }
                            if (item.type == "1")
                            {
                                rangelistnew.Add(item);
                            }
                        }
                        foreach (var item in rangelistnew)
                        {
                            if (!work_Order.Range.Contains(item.id))
                            {
                                list1.Add(item);
                            }
                        }
                        var str = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                        var str1 = Newtonsoft.Json.JsonConvert.SerializeObject(strList);
                        work_Order.Range = str;
                        work_Order.RangeNew = str1;
                    }
                    else
                    {
                        throw new BusinessTureException("此指令已删除");
                    }
                    break;
                case "8":
                    var work_Announcement = _JointOfficeContext.Work_Announcement.Where(t => t.Id == para.id).FirstOrDefault();
                    if (work_Announcement != null)
                    {
                        strList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(work_Announcement.RangeNew);
                        list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(work_Announcement.Range);
                        foreach (var item in list2)
                        {
                            strList.Add(item);
                            if (item.type == "2")
                            {
                                rangelistnew = WorkDetails.CreateWorkRange(rangelistnew, item.id);
                            }
                            if (item.type == "1")
                            {
                                rangelistnew.Add(item);
                            }
                        }
                        foreach (var item in rangelistnew)
                        {
                            if (!work_Announcement.Range.Contains(item.id))
                            {
                                list1.Add(item);
                            }
                        }
                        var str = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                        var str1 = Newtonsoft.Json.JsonConvert.SerializeObject(strList);
                        work_Announcement.Range = str;
                        work_Announcement.RangeNew = str1;
                    }
                    else
                    {
                        throw new BusinessTureException("此公告已删除");
                    }
                    break;
                case "9":
                    var work_Share = _JointOfficeContext.Work_Share.Where(t => t.Id == para.id).FirstOrDefault();
                    if (work_Share != null)
                    {
                        strList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(work_Share.RangeNew);
                        list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(work_Share.Range);
                        foreach (var item in list2)
                        {
                            strList.Add(item);
                            if (item.type == "2")
                            {
                                rangelistnew = WorkDetails.CreateWorkRange(rangelistnew, item.id);
                            }
                            if (item.type == "1")
                            {
                                rangelistnew.Add(item);
                            }
                        }
                        foreach (var item in rangelistnew)
                        {
                            if (!work_Share.Range.Contains(item.id))
                            {
                                list1.Add(item);
                            }
                        }
                        var str = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
                        var str1 = Newtonsoft.Json.JsonConvert.SerializeObject(strList);
                        work_Share.Range = str;
                        work_Share.RangeNew = str1;
                    }
                    else
                    {
                        throw new BusinessTureException("此分享已删除");
                    }
                    break;
            }
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("修改成功");
        }
    }
}