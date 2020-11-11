using Dapper;
using JointOffice.Configuration;
using JointOffice.Core;
using JointOffice.DbHelper;
using JointOffice.DbModel;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public class BAttendance : IAttendance
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        private readonly IPrincipalBase _PrincipalBase;
        public BAttendance(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            this.config = config;
            _PrincipalBase = IPrincipalBase;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
        }
        /// <summary>
        /// 考勤详情
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<CheckRecord> GetCheckRecordList(CheckRecordInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnSingle<CheckRecord>();
                return Return.Return();
            }
            Showapi_Res_Single<CheckRecord> res = new Showapi_Res_Single<CheckRecord>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            para.time = WorkDetails.GetYMD(para.time);
            var time1 = Convert.ToDateTime(para.time);
            var time2 = Convert.ToDateTime(para.time).AddDays(1);
            var checkInfo = _JointOfficeContext.Attendance_Check.Where(t => t.MemberId == memberid && t.CheckDate > time1 && t.CheckDate < time2).OrderBy(t => t.CheckDate).ToList();
            List<CheckRecordInfo> list1 = new List<CheckRecordInfo>();
            List<RemarksInfo> list2 = new List<RemarksInfo>();
            CheckRecord CheckRecord = new CheckRecord();
            if (checkInfo.Count != 0 && checkInfo != null)
            {
                foreach (var item in checkInfo)
                {
                    CheckRecordInfo CheckRecordInfo = new CheckRecordInfo();
                    CheckRecordInfo.type = item.Type;
                    CheckRecordInfo.toOffTime = item.CheckDate.ToString("HH:mm");
                    CheckRecordInfo.map = item.Map;
                    CheckRecordInfo.address = item.Address;
                    list1.Add(CheckRecordInfo);
                }
                var remarksInfo = _JointOfficeContext.Remark.Where(t => t.MemberId == memberid && t.CheckDate == para.time).OrderBy(t => t.RemarksTime).ToList();
                foreach (var item in remarksInfo)
                {
                    RemarksInfo RemarksInfo = new RemarksInfo();
                    RemarksInfo.remarks = item.Remarks;
                    RemarksInfo.time = item.RemarksTime.ToString("MM-dd HH:mm");
                    switch (item.Type)
                    {
                        case 0:
                            RemarksInfo.type = "";
                            break;
                        case 1:
                            RemarksInfo.type = "出发";
                            break;
                        case 2:
                            RemarksInfo.type = "交通中转";
                            break;
                        case 3:
                            RemarksInfo.type = "到达客户";
                            break;
                        case 4:
                            RemarksInfo.type = "离开客户";
                            break;
                        case 5:
                            RemarksInfo.type = "上班签到";
                            break;
                        case 6:
                            RemarksInfo.type = "下班签到";
                            break;
                    }
                    list2.Add(RemarksInfo);
                }
            }
            CheckRecord.checkRecordInfo = list1;
            CheckRecord.remarksInfo = list2;
            res.showapi_res_body = CheckRecord;
            res.showapi_res_code = "200";
            return res;
        }
        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge CheckIn(CheckInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            Attendance_Check Attendance_Check = new Attendance_Check();
            Attendance_Check.Id = Guid.NewGuid().ToString();
            Attendance_Check.MemberId = memberid;
            Attendance_Check.CheckDate = DateTime.Now;
            Attendance_Check.Type = para.checkType;
            Attendance_Check.Map = para.map;
            Attendance_Check.Address = para.address;
            Attendance_Check.Mark = 0;
            if (para.remarks != null && para.remarks != "")
            {
                Attendance_Check.Remarks = para.remarks;
                Attendance_Check.RemarksTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                Remark Remark = new Remark();
                Remark.Id = Guid.NewGuid().ToString();
                Remark.MemberId = memberid;
                Remark.CheckDate = DateTime.Now.ToString("yyyy-MM-dd");
                Remark.Remarks = para.remarks;
                Remark.RemarksTime = DateTime.Now;
                Remark.Type = para.checkType;
                _JointOfficeContext.Remark.Add(Remark);
            }
            else
            {
                Attendance_Check.Remarks = "";
                Attendance_Check.RemarksTime = "";
            }
            _JointOfficeContext.Attendance_Check.Add(Attendance_Check);

            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 备注  写入
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Meaasge RemarksIn(RemarksInPara para)
        {
            Message Message = new Message();
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                return Message.MemberMeaasge();
            }
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            para.checkDate = WorkDetails.GetYMD(para.checkDate);
            Remark Remark = new Remark();
            Remark.Id = Guid.NewGuid().ToString();
            Remark.MemberId = memberid;
            Remark.CheckDate = para.checkDate;
            Remark.Remarks = para.remarks;
            Remark.RemarksTime = DateTime.Now;
            Remark.Type = 0;
            _JointOfficeContext.Remark.Add(Remark);
            _JointOfficeContext.SaveChanges();
            return Message.SuccessMeaasge("操作成功");
        }
        /// <summary>
        /// 考勤统计
        /// </summary>
        public Showapi_Res_Single<CheckCountPara> GetCheckCountList(CheckCountInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var ReturnSingle = new ReturnSingle<CheckCountPara>();
                return ReturnSingle.Return();
            }
            Showapi_Res_Single<CheckCountPara> res = new Showapi_Res_Single<CheckCountPara>();
            CheckCountPara CheckCountPara = new CheckCountPara();
            var beginTime = Convert.ToDateTime(para.beginTime);
            var stopTime = Convert.ToDateTime(para.stopTime);
            if (beginTime == stopTime)
            {
                stopTime = Convert.ToDateTime(stopTime.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            List<CheckCountListPara> list1 = new List<CheckCountListPara>();
            int[] array1 = new int[] { 1, 2, 3, 4, 5, 6 };

            //团队
            if (para.type == 1)
            {
                //获取全部人员的memberid
                List<string> memberInfo = new List<string>();
                if (para.memberidList.Count == 0)
                {
                    var memInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == memberid).FirstOrDefault();
                    if (memInfo != null && !string.IsNullOrEmpty(memInfo.CompanyIDS))
                    {
                        memberInfo = _JointOfficeContext.Member_Info_Company.Where(t => memInfo.CompanyIDS.Contains(t.CompanyId)).Select(t => t.MemberId).ToList();
                    }
                    
                    //memberInfo = _JointOfficeContext.Member.Select(t => t.Id).ToList();
                    //memberInfo = _JointOfficeContext.Member_Info.Where(t => t.ZhuBuMen != "" && t.ZhuBuMen != null).Select(t => t.MemberId).ToList();
                }
                else
                {
                    foreach (var item1 in para.memberidList)
                    {
                        if (Convert.ToInt32(item1.type) == 2)
                        {
                            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                            memberInfo = WorkDetails.GetCompanyPersonList(memberInfo, item1.id);
                        }
                        if (Convert.ToInt32(item1.type) == 1)
                        {
                            var member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item1.id).Select(t => t.MemberId).ToList();
                            memberInfo.AddRange(member_Info);
                        }
                    }
                }
                var memberInfoset = new HashSet<string>(memberInfo);

                for (int i = 0; i < array1.Length; i++)
                {
                    List<SqlList> list = new List<SqlList>();
                    CheckCountListPara CheckCountListPara = new CheckCountListPara();
                    var type = array1[i];
                    var memberStr = "";
                    foreach (var item in memberInfoset)
                    {
                        memberStr += "'" + item + "',";

                        //var checkTeamMark = _JointOfficeContext.Attendance_Check.Where(t => t.Type == array1[i] && t.MemberId == item && t.CheckDate >= beginTime && t.CheckDate <= stopTime).ToList();
                        //if (checkTeamMark.Count != 0)
                        //{
                        //    CheckCountListPara.num += 1;
                        //}
                    }
                    memberStr = memberStr.Remove(memberStr.LastIndexOf(","));
                    var sql = @"select a.MemberId,type
                                from Attendance_Check a
                                left join Member b on a.MemberId = b.ID
                                where type = " + type + @" and
                                a.MemberId in (" + memberStr + @") and
                                a.CheckDate > '" + beginTime + @"' and
                                a.CheckDate < '" + stopTime + @"'
                                Group by type,a.MemberId";
                    using (SqlConnection conText = new SqlConnection(constr))
                    {
                        list = conText.Query<SqlList>(sql).ToList();
                    }
                    CheckCountListPara.num = list.Count();
                    switch (array1[i])
                    {
                        case 1:
                            CheckCountListPara.mark = "出发";
                            CheckCountListPara.markNum = "1";
                            break;
                        case 2:
                            CheckCountListPara.mark = "交通中转";
                            CheckCountListPara.markNum = "2";
                            break;
                        case 3:
                            CheckCountListPara.mark = "到达客户";
                            CheckCountListPara.markNum = "3";
                            break;
                        case 4:
                            CheckCountListPara.mark = "离开客户";
                            CheckCountListPara.markNum = "4";
                            break;
                        case 5:
                            CheckCountListPara.mark = "上班签到";
                            CheckCountListPara.markNum = "5";
                            break;
                        case 6:
                            CheckCountListPara.mark = "下班签到";
                            CheckCountListPara.markNum = "6";
                            break;
                    }
                    CheckCountListPara.zongNum = memberInfoset.Count();
                    list1.Add(CheckCountListPara);
                }

                CheckCountPara.checkCountListPara = list1;
                CheckCountPara.personNum = memberInfoset.Count();
            }
            //我的
            if (para.type == 2)
            {
                for (int i = 0; i < array1.Length; i++)
                {
                    List<SqlList> checkMe = new List<SqlList>();
                    var type = array1[i];
                    //var checkMe = _JointOfficeContext.Attendance_Check.Where(t => t.MemberId == memberid && t.Type == array1[i] && t.CheckDate >= beginTime && t.CheckDate <= stopTime).ToList();
                    var sql = @"select a.MemberId,type
                                from Attendance_Check a
                                left join Member b on a.MemberId = b.ID
                                where type = " + type + @" and
                                a.MemberId = '" + memberid + @"' and
                                a.CheckDate > '" + beginTime + @"' and
                                a.CheckDate < '" + stopTime + @"'
                                Group by type,a.MemberId";
                    using (SqlConnection conText = new SqlConnection(constr))
                    {
                        checkMe = conText.Query<SqlList>(sql).ToList();
                    }
                    if (checkMe.Count != 0)
                    {
                        CheckCountListPara CheckCountListPara = new CheckCountListPara();
                        switch (array1[i])
                        {
                            case 1:
                                CheckCountListPara.mark = "出发";
                                CheckCountListPara.markNum = "1";
                                break;
                            case 2:
                                CheckCountListPara.mark = "交通中转";
                                CheckCountListPara.markNum = "2";
                                break;
                            case 3:
                                CheckCountListPara.mark = "到达客户";
                                CheckCountListPara.markNum = "3";
                                break;
                            case 4:
                                CheckCountListPara.mark = "离开客户";
                                CheckCountListPara.markNum = "4";
                                break;
                            case 5:
                                CheckCountListPara.mark = "上班签到";
                                CheckCountListPara.markNum = "5";
                                break;
                            case 6:
                                CheckCountListPara.mark = "下班签到";
                                CheckCountListPara.markNum = "6";
                                break;
                        }
                        CheckCountListPara.num = checkMe.Count();
                        CheckCountListPara.zongNum = 0;
                        list1.Add(CheckCountListPara);
                    }
                }

                CheckCountPara.personNum = 0;
                CheckCountPara.checkCountListPara = list1;
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = CheckCountPara;
            return res;
        }
        /// <summary>
        /// 显示考勤统计人员和次数页面  团队
        /// </summary>
        public Showapi_Res_List<CheckCountTeamListPara> GetCheckCountTeamList(CheckCountTeamListParaInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<CheckCountTeamListPara>();
                return Return.Return();
            }
            Showapi_Res_List<CheckCountTeamListPara> res = new Showapi_Res_List<CheckCountTeamListPara>();
            List<CheckCountTeamListPara> list1 = new List<CheckCountTeamListPara>();
            var beginTime = Convert.ToDateTime(para.beginTime);
            var stopTime = Convert.ToDateTime(para.stopTime);
            if (beginTime == stopTime)
            {
                stopTime = Convert.ToDateTime(stopTime.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            List<string> memberInfo = new List<string>();
            if (para.memberidList.Count == 0)
            {
                //memberInfo = _JointOfficeContext.Member.Select(t => t.Id).ToList();
                memberInfo = _JointOfficeContext.Member_Info.Where(t => t.ZhuBuMen != "" && t.ZhuBuMen != null).Select(t => t.MemberId).ToList();
            }
            else
            {
                foreach (var item1 in para.memberidList)
                {
                    if (Convert.ToInt32(item1.type) == 2)
                    {
                        WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                        memberInfo = WorkDetails.GetCompanyPersonList(memberInfo, item1.id);
                    }
                    if (Convert.ToInt32(item1.type) == 1)
                    {
                        var member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item1.id).Select(t => t.MemberId).ToList();
                        memberInfo.AddRange(member_Info);
                    }
                }
            }
            var memberInfoset = new HashSet<string>(memberInfo);

            foreach (var item in memberInfoset)
            {
                List<SqlList> checkTeamMark = new List<SqlList>();
                var sql = @"select a.MemberId,type
                            from Attendance_Check a
                            left join Member b on a.MemberId = b.ID
                            where type = " + para.mark + @" and
                            a.MemberId = '" + item + @"' and
                            a.CheckDate > '" + beginTime + @"' and
                            a.CheckDate < '" + stopTime + @"'";
                            //Group by type,a.MemberId";
                using (SqlConnection conText = new SqlConnection(constr))
                {
                    checkTeamMark = conText.Query<SqlList>(sql).ToList();
                }
                //var checkTeamMark = _JointOfficeContext.Attendance_Check.Where(t => t.Type == para.mark && t.MemberId == item && t.CheckDate >= beginTime && t.CheckDate <= stopTime).ToList();
                if (checkTeamMark.Count != 0 && checkTeamMark != null)
                {
                    CheckCountTeamListPara CheckCountTeamListPara = new CheckCountTeamListPara();
                    CheckCountTeamListPara.memberid = item;
                    var Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item).FirstOrDefault();
                    CheckCountTeamListPara.name = Info.Name;
                    CheckCountTeamListPara.picture = Info.Picture + SasKey;
                    CheckCountTeamListPara.jobName = "";
                    var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == Info.JobID).FirstOrDefault();
                    if (memJob != null)
                    {
                        CheckCountTeamListPara.jobName = memJob.Name;
                    }
                    CheckCountTeamListPara.num = checkTeamMark.Count();
                    list1.Add(CheckCountTeamListPara);
                }
            }

            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<CheckCountTeamListPara>();
            res.showapi_res_body.contentlist = list1;
            return res;
        }
        /// <summary>
        /// 显示某个人员具体签到类型的明细列表
        /// </summary>
        public Showapi_Res_List<CheckCountTypeList> GetCheckCountTypeList(CheckCountTypeListInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnList<CheckCountTypeList>();
                return Return.Return();
            }
            Showapi_Res_List<CheckCountTypeList> res = new Showapi_Res_List<CheckCountTypeList>();
            List<CheckCountTypeList> list = new List<CheckCountTypeList>();
            List<Attendance_Check> checkCountMark = new List<Attendance_Check>();
            var beginTime = Convert.ToDateTime(para.beginTime);
            var stopTime = Convert.ToDateTime(para.stopTime);
            if (beginTime == stopTime)
            {
                stopTime = Convert.ToDateTime(stopTime.ToString("yyyy-MM-dd") + " 23:59:59");
            }

            if (para.type == 1)
            {
                checkCountMark = _JointOfficeContext.Attendance_Check.Where(t => t.Type == para.mark && t.MemberId == para.memberid && t.CheckDate >= beginTime && t.CheckDate <= stopTime).OrderByDescending(t => t.CheckDate).ToList();
            }
            if (para.type == 2)
            {
                checkCountMark = _JointOfficeContext.Attendance_Check.Where(t => t.Type == para.mark && t.MemberId == memberid && t.CheckDate >= beginTime && t.CheckDate <= stopTime).OrderByDescending(t => t.CheckDate).ToList();
            }
            foreach (var item in checkCountMark)
            {
                CheckCountTypeList CheckCountTypeList = new CheckCountTypeList();
                CheckCountTypeList.memberid = item.MemberId;
                CheckCountTypeList.checkDate = item.CheckDate.ToString("yyyy-MM-dd HH:mm");
                switch (para.mark)
                {
                    case 1:
                        CheckCountTypeList.timeBody = "出发";
                        break;
                    case 2:
                        CheckCountTypeList.timeBody = "交通中转";
                        break;
                    case 3:
                        CheckCountTypeList.timeBody = "到达客户";
                        break;
                    case 4:
                        CheckCountTypeList.timeBody = "离开客户";
                        break;
                    case 5:
                        CheckCountTypeList.timeBody = "上班签到";
                        break;
                    case 6:
                        CheckCountTypeList.timeBody = "下班签到";
                        break;
                }
                CheckCountTypeList.address = item.Address;
                list.Add(CheckCountTypeList);
            }
            res.showapi_res_code = "200";
            res.showapi_res_body = new Showapi_res_body_list<CheckCountTypeList>();
            res.showapi_res_body.contentlist = list;
            return res;
        }
        /// <summary>
        /// 某个人员具体签到类型的明细列表进考勤详情
        /// </summary>
        /// <returns></returns>
        public Showapi_Res_Single<CheckRecord> GetCheckCountTypeRecordList(CheckCountTypeRecordInPara para)
        {
            var memberid = _PrincipalBase.GetMemberId();
            if (memberid == null || memberid == "")
            {
                var Return = new ReturnSingle<CheckRecord>();
                return Return.Return();
            }
            Showapi_Res_Single<CheckRecord> res = new Showapi_Res_Single<CheckRecord>();
            WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
            para.time = WorkDetails.GetYMD(para.time);
            var time1 = Convert.ToDateTime(para.time);
            var time2 = Convert.ToDateTime(para.time).AddDays(1);
            var checkInfo = _JointOfficeContext.Attendance_Check.Where(t => t.MemberId == para.memberid && t.CheckDate > time1 && t.CheckDate < time2).OrderBy(t => t.CheckDate).ToList();
            List<CheckRecordInfo> list1 = new List<CheckRecordInfo>();
            List<RemarksInfo> list2 = new List<RemarksInfo>();
            CheckRecord CheckRecord = new CheckRecord();
            if (checkInfo.Count != 0 && checkInfo != null)
            {
                foreach (var item in checkInfo)
                {
                    CheckRecordInfo CheckRecordInfo = new CheckRecordInfo();
                    CheckRecordInfo.type = item.Type;
                    CheckRecordInfo.toOffTime = item.CheckDate.ToString("HH:mm");
                    CheckRecordInfo.map = item.Map;
                    CheckRecordInfo.address = item.Address;
                    list1.Add(CheckRecordInfo);
                }
                var remarksInfo = _JointOfficeContext.Remark.Where(t => t.MemberId == para.memberid && t.CheckDate == para.time).OrderBy(t => t.RemarksTime).ToList();
                foreach (var item in remarksInfo)
                {
                    RemarksInfo RemarksInfo = new RemarksInfo();
                    RemarksInfo.remarks = item.Remarks;
                    RemarksInfo.time = item.RemarksTime.ToString("MM-dd HH:mm");
                    switch (item.Type)
                    {
                        case 0:
                            RemarksInfo.type = "";
                            break;
                        case 1:
                            RemarksInfo.type = "出发";
                            break;
                        case 2:
                            RemarksInfo.type = "交通中转";
                            break;
                        case 3:
                            RemarksInfo.type = "到达客户";
                            break;
                        case 4:
                            RemarksInfo.type = "离开客户";
                            break;
                        case 5:
                            RemarksInfo.type = "上班签到";
                            break;
                        case 6:
                            RemarksInfo.type = "下班签到";
                            break;
                    }
                    list2.Add(RemarksInfo);
                }
            }
            CheckRecord.checkRecordInfo = list1;
            CheckRecord.remarksInfo = list2;
            res.showapi_res_body = CheckRecord;
            res.showapi_res_code = "200";
            return res;
        }
    }
}
