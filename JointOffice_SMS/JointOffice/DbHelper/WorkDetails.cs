using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using JointOffice.Configuration;
using JointOffice.DbModel;
using JointOffice.Models;
using Microsoft.Extensions.Options;

namespace JointOffice.DbHelper
{
    public class WorkDetails
    {
        JointOfficeContext _JointOfficeContext;
        IOptions<Root> config;
        string constr;
        string SasKey;
        private readonly IPrincipalBase _PrincipalBase;
        public WorkDetails(IOptions<Root> config, JointOfficeContext JointOfficeContext, IPrincipalBase IPrincipalBase)
        {
            _JointOfficeContext = JointOfficeContext;
            _PrincipalBase = IPrincipalBase;
            this.config = config;
            constr = this.config.Value.ConnectionStrings.JointOfficeConnection;
            SasKey = this.config.Value.ConnectionStrings.SasKey;
        }
        /// <summary>
        /// 抄送范围
        /// </summary>
        /// <returns></returns>
        public static String GetRange(string rangeStr)
        {
            var list1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(rangeStr);
            var list11 = list1.Where(t => t.type == "1").ToList();
            var list12 = list1.Where(t => t.type == "2").ToList();
            var list13 = list1.Where(t => t.type == "3").ToList();
            var range = "";
            var range1 = "";
            var range2 = "";
            var range3 = "";
            if (list11.Count != 0)
            {
                range1 = list11.Count() + "个同事";
            }
            if ((list11.Count != 0 && list12.Count != 0) || (list11.Count != 0 && list13.Count != 0))
            {
                range1 = range1 + "、";
            }
            if (list12.Count != 0)
            {
                range2 = list12.Count() + "个部门";
            }
            if (list12.Count != 0 && list13.Count != 0)
            {
                range2 = range2 + "、";
            }
            if (list13.Count != 0)
            {
                range3 = list13.Count() + "个群组";
            }
            range = range1 + range2 + range3;
            return range;
        }
        /// <summary>
        /// 大类  list  处理
        /// </summary>
        /// <returns></returns>
        public List<PersonDynamic_info> GetPersonDynamic_info(List<PersonDynamic_info> list)
        {
            var memberid = _PrincipalBase.GetMemberId();
            //此条动态是否已关注 已收藏
            //图片 录音 附件 录音时长
            //头像  创建时间  grade
            //抄送范围
            //执行列表
            foreach (var item in list)
            {
                //此条动态的分类名称
                item.dyTypeName = item.type;
                //此条动态是否已关注 已收藏
                var focus = _JointOfficeContext.News_Focus.Where(t => t.UId == item.id && t.MemberId == memberid).FirstOrDefault();
                if (focus != null)
                {
                    item.isFocus = "1";
                }
                else
                {
                    item.isFocus = "0";
                }
                var collection = _JointOfficeContext.News_Collection.Where(t => t.UId == item.id && t.MemberId == memberid).FirstOrDefault();
                if (collection != null)
                {
                    item.isCollection = "1";
                }
                else
                {
                    item.isCollection = "0";
                }
                //图片 录音 附件 录音时长
                if (item.picturelist != null && item.picturelist != "")
                {
                    var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item.picturelist);
                    foreach (var itemPicture in listPicture)
                    {
                        itemPicture.url = itemPicture.url + SasKey;
                    }
                    item.appendPicture = listPicture;
                }
                if (item.annexlist != null && item.annexlist != "")
                {
                    var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Work_File>>(item.annexlist);
                    long length = 0;
                    foreach (var itemAnnex in listAnnex)
                    {
                        itemAnnex.url = itemAnnex.url + SasKey;
                        itemAnnex.lengthStr = BusinessHelper.ConvertBytes(itemAnnex.length);
                        length += itemAnnex.length;
                    }
                    item.annex = listAnnex;
                    item.annexLength = BusinessHelper.ConvertBytes(length);
                }
                if (item.voice != null && item.voice != "")
                {
                    item.voice = item.voice + SasKey;
                }
                if (item.voiceLength != null && item.voiceLength != "" && item.voiceLength.Substring(0, 1) == "0")
                {
                    item.voiceLength = item.voiceLength.Substring(1, 1);
                }
                else
                {
                    item.voiceLength = item.voiceLength;
                }
                //头像  创建时间  grade
                item.picture = item.picture + SasKey;
                item.createDate = string.Format("{0:f}", Convert.ToDateTime(item.createDate));
                if (item.grade == null)
                {
                    item.grade = "0";
                }
                if (item.grade != null && item.grade != "")
                {
                    item.gradeNum = Convert.ToInt32(item.grade);
                }
                else
                {
                    item.gradeNum = 0;
                }
                //抄送范围
                if (item.range != null && item.range != "" && item.range != "[]")
                {
                    if (string.IsNullOrEmpty(item.rangeNew))
                    {
                        item.range = GetRange(item.range);
                    }
                    else
                    {
                        item.range = GetRange(item.rangeNew);
                    }
                }
                else
                {
                    item.range = "公开";
                }
                //执行列表
                var dianpingList = _JointOfficeContext.DianPing_Body.Where(t => t.UId == item.id).FirstOrDefault();
                var comment = _JointOfficeContext.Comment_Body.Where(t => t.UId == item.id && t.IsExeComment == 1).OrderByDescending(t => t.PingLunTime).ToList();
                List<ExeCommentList> list2 = new List<ExeCommentList>();
                switch (item.typeNum)
                {
                    case "1":
                        foreach (var itemcom in comment)
                        {
                            ExeCommentList ExeCommentList = new ExeCommentList();
                            switch (itemcom.Body.Substring(0, 1))
                            {
                                case "已":
                                    ExeCommentList.state = "已批准";
                                    ExeCommentList.stateNum = "2";
                                    break;
                                case "取":
                                    ExeCommentList.state = "取消审批";
                                    ExeCommentList.stateNum = "3";
                                    break;
                                case "未":
                                    ExeCommentList.state = "未批准";
                                    ExeCommentList.stateNum = "3";
                                    break;
                            }
                            ExeCommentList.name = itemcom.Name;
                            ExeCommentList.exeDate = itemcom.PingLunTime.ToString("yyyy-MM-dd HH:mm");
                            ExeCommentList.exeBody = itemcom.Body;
                            ExeCommentList.phoneModel = itemcom.PhoneModel;
                            list2.Add(ExeCommentList);
                        }
                        break;
                    case "2":
                        if (dianpingList != null)
                        {
                            ExeCommentList ExeCommentList = new ExeCommentList();
                            ExeCommentList.name = dianpingList.Name;
                            ExeCommentList.exeDate = dianpingList.DianPingTime.ToString("yyyy-MM-dd HH:mm");
                            ExeCommentList.exeBody = dianpingList.Body;
                            ExeCommentList.state = "点评日志";
                            ExeCommentList.stateNum = "1";
                            ExeCommentList.phoneModel = dianpingList.PhoneModel;
                            list2.Add(ExeCommentList);
                        }
                        break;
                    case "3":
                        if (dianpingList != null)
                        {
                            ExeCommentList ExeCommentList = new ExeCommentList();
                            ExeCommentList.name = dianpingList.Name;
                            ExeCommentList.exeDate = dianpingList.DianPingTime.ToString("yyyy-MM-dd HH:mm");
                            ExeCommentList.exeBody = dianpingList.Body;
                            ExeCommentList.state = "点评任务";
                            ExeCommentList.stateNum = "1";
                            ExeCommentList.phoneModel = dianpingList.PhoneModel;
                            list2.Add(ExeCommentList);
                        }
                        foreach (var itemcom in comment)
                        {
                            ExeCommentList ExeCommentList = new ExeCommentList();
                            switch (itemcom.Body.Substring(0, 2))
                            {
                                case "完成":
                                    ExeCommentList.state = "完成任务";
                                    ExeCommentList.stateNum = "2";
                                    break;
                                case "取消":
                                    ExeCommentList.state = "取消任务";
                                    ExeCommentList.stateNum = "3";
                                    break;
                            }
                            ExeCommentList.name = itemcom.Name;
                            ExeCommentList.exeDate = itemcom.PingLunTime.ToString("yyyy-MM-dd HH:mm");
                            ExeCommentList.exeBody = itemcom.Body;
                            ExeCommentList.phoneModel = itemcom.PhoneModel;
                            list2.Add(ExeCommentList);
                        }
                        break;
                    case "4":
                        foreach (var itemcom in comment)
                        {
                            ExeCommentList ExeCommentList = new ExeCommentList();
                            ExeCommentList.name = itemcom.Name;
                            ExeCommentList.exeDate = itemcom.PingLunTime.ToString("yyyy-MM-dd HH:mm");
                            ExeCommentList.exeBody = itemcom.Body;
                            ExeCommentList.state = "取消日程";
                            ExeCommentList.stateNum = "3";
                            ExeCommentList.phoneModel = itemcom.PhoneModel;
                            list2.Add(ExeCommentList);
                        }
                        break;
                    case "5":
                        if (dianpingList != null)
                        {
                            ExeCommentList ExeCommentList = new ExeCommentList();
                            ExeCommentList.name = dianpingList.Name;
                            ExeCommentList.exeDate = dianpingList.DianPingTime.ToString("yyyy-MM-dd HH:mm");
                            ExeCommentList.exeBody = dianpingList.Body;
                            ExeCommentList.state = "点评指令";
                            ExeCommentList.stateNum = "1";
                            ExeCommentList.phoneModel = dianpingList.PhoneModel;
                            list2.Add(ExeCommentList);
                        }
                        foreach (var itemcom in comment)
                        {
                            ExeCommentList ExeCommentList = new ExeCommentList();
                            switch (itemcom.Body.Substring(0, 2))
                            {
                                case "完成":
                                    ExeCommentList.state = "完成指令";
                                    ExeCommentList.stateNum = "2";
                                    break;
                                case "取消":
                                    ExeCommentList.state = "取消指令";
                                    ExeCommentList.stateNum = "3";
                                    break;
                                case "继续":
                                    ExeCommentList.state = "继续执行";
                                    ExeCommentList.stateNum = "4";
                                    break;
                            }
                            ExeCommentList.name = itemcom.Name;
                            ExeCommentList.exeDate = itemcom.PingLunTime.ToString("yyyy-MM-dd HH:mm");
                            ExeCommentList.exeBody = itemcom.Body;
                            ExeCommentList.phoneModel = itemcom.PhoneModel;
                            list2.Add(ExeCommentList);
                        }
                        break;
                }
                item.exeCommentList = list2;
            }
            //任务执行人
            //任务完成人数
            //此任务谁完成了
            //执行人执行完没 1执行完 0没执行完
            //执行进度
            var list1 = list.Where(t => t.type == "任务").ToList();
            foreach (var item in list1)
            {
                //执行人
                List<Executor> list2 = new List<Executor>();
                var Execute_Content = _JointOfficeContext.Execute_Content.Where(t => t.UId == item.id).ToList();
                foreach (var one in Execute_Content)
                {
                    Executor Executor = new Executor();
                    Executor.id = one.OtherMemberId;
                    Executor.name = one.OtherMemberName;
                    Executor.picture = one.OtherMemberPicture + SasKey;
                    Executor.executorCompleteRemarks = one.Content;
                    Executor.executorCompleteTime = one.ExecuteDate;
                    switch (one.State)
                    {
                        case 0:
                            Executor.type = "执行中";
                            break;
                        case 1:
                            Executor.type = "已执行";
                            break;
                        case 2:
                            Executor.type = "已取消";
                            break;
                        case 3:
                            Executor.type = "已失效";
                            break;
                    }
                    list2.Add(Executor);
                }
                item.executor = list2;
                //完成人数
                var Execute_Content1 = Execute_Content.Where(t => t.State == 1).ToList();
                item.executorNum = Execute_Content1.Count().ToString() + "/" + Execute_Content.Count().ToString();
                item.executorNumYes = Execute_Content1.Count().ToString();
                item.executorNumAll = Execute_Content.Count().ToString();
                //此任务谁完成了
                if (Execute_Content1.Count > 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var exePersonStr = Execute_Content1[i].OtherMemberName;
                        if ((i + 1) != 2)
                        {
                            item.exePerson += exePersonStr + "、";
                        }
                        else
                        {
                            item.exePerson += exePersonStr + "等" + Execute_Content1.Count() + "人";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Execute_Content1.Count; i++)
                    {
                        var exePersonStr = Execute_Content1[i].OtherMemberName;
                        if ((i + 1) != Execute_Content1.Count)
                        {
                            item.exePerson += exePersonStr + "、";
                        }
                        else
                        {
                            item.exePerson += exePersonStr;
                        }
                    }
                }
                //执行人执行完没 1执行完 0没执行完
                var execute4 = Execute_Content.Where(t => t.OtherMemberId == memberid).FirstOrDefault();
                if (execute4 != null)
                {
                    switch (execute4.State)
                    {
                        case 0:
                            item.isExe = "0";
                            break;
                        case 1:
                            item.isExe = "1";
                            break;
                        case 2:
                            item.isExe = "2";
                            break;
                    }
                }
                else
                {
                    item.isExe = "";
                }
                //执行进度
                var num1 = Execute_Content1.Count() + ".0";
                double num2 = Convert.ToDouble(num1) / Convert.ToDouble(Execute_Content.Count());
                var percentage = Math.Round(num2, 2);
                item.percentage = percentage.ToString();
            }
            //提醒时间
            var list3 = list.Where(t => t.type == "任务" || t.type == "日程").ToList();
            foreach (var item in list3)
            {
                if (item.remindTime != null && item.remindTime != "")
                {
                    var remindTimelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RemindTime>>(item.remindTime);
                    var remindTimeStr = "";
                    for (int i = 0; i < remindTimelist.Count; i++)
                    {
                        remindTimeStr = remindTimeStr + remindTimelist[i].timeWord;
                        if ((i + 1) != remindTimelist.Count)
                        {
                            remindTimeStr = remindTimeStr + ",";
                        }
                    }
                    item.remindTime = remindTimeStr;
                }
            }
            //审批人
            //审批类型
            //审批进度
            var list10 = list.Where(t => t.type == "审批").ToList();
            foreach (var item in list10)
            {
                //审批人
                List<Executor> list8 = new List<Executor>();
                var Approval_Content = _JointOfficeContext.Approval_Content.Where(t => t.UId == item.id).OrderBy(t => t.OtherMemberOrder).ToList();
                if (Approval_Content.Count != 0 && Approval_Content != null)
                {
                    foreach (var one in Approval_Content)
                    {
                        Executor Executor = new Executor();
                        Executor.id = one.OtherMemberId;
                        Executor.name = one.OtherMemberName;
                        Executor.picture = one.OtherMemberPicture + SasKey;
                        Executor.executorCompleteRemarks = one.Content;
                        Executor.executorCompleteTime = one.ApprovalTime;
                        Executor.num = one.State.ToString();
                        switch (one.State)
                        {
                            case 0:
                                Executor.type = "审批中";
                                break;
                            case 1:
                                Executor.type = "已批准";
                                break;
                            case 2:
                                Executor.type = "已取消";
                                break;
                            case 3:
                                Executor.type = "未批准";
                                break;
                            case 5:
                                Executor.type = "待审批";
                                break;
                            case 6:
                                Executor.type = "已失效";
                                break;
                        }
                        list8.Add(Executor);
                    }
                }
                item.executor = list8;
                //审批类型
                switch (item.approvalType)
                {
                    case "0":
                        item.approvalType = "";
                        item.approvalTypeNum = "0";
                        break;
                    case "1":
                        item.approvalType = "加班";
                        item.approvalTypeNum = "1";
                        break;
                    case "2":
                        item.approvalType = "出差";
                        item.approvalTypeNum = "2";
                        break;
                    case "3":
                        item.approvalType = "请假";
                        item.approvalTypeNum = "3";
                        break;
                    case "4":
                        item.approvalType = "差旅报销";
                        item.approvalTypeNum = "4";
                        break;
                    case "5":
                        item.approvalType = "普通报销";
                        item.approvalTypeNum = "5";
                        break;
                }
                //加班
                if (item.overTimeStr != null && item.overTimeStr != "")
                {
                    var OverTimelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OverTime>>(item.overTimeStr);
                    List<OverTime> overTimelist = new List<OverTime>();
                    foreach (var item1 in OverTimelist)
                    {
                        OverTime OverTime = new OverTime();
                        OverTime.beginTime = item1.beginTime;
                        OverTime.stopTime = item1.stopTime;
                        OverTime.hour = item1.hour;
                        overTimelist.Add(OverTime);
                    }
                    item.overTime = overTimelist;
                }
                //出差总详情
                item.travelAll = item.travelAllStr;
                //出差分详情
                if (item.travelStr != null && item.travelStr != "")
                {
                    var Travellist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Travel>>(item.travelStr);
                    List<Travel> travellist = new List<Travel>();
                    foreach (var item1 in Travellist)
                    {
                        Travel Travel = new Travel();
                        Travel.goArea = item1.goArea;
                        Travel.toArea = item1.toArea;
                        Travel.goDate = item1.goDate;
                        Travel.toDate = item1.toDate;
                        Travel.tran = item1.tran;
                        Travel.stay = item1.stay;
                        travellist.Add(Travel);
                    }
                    item.travel = travellist;
                }
                //请假
                if (item.leaveStr != null && item.leaveStr != "")
                {
                    var Leavelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Leave>>(item.leaveStr);
                    List<Leave> leavelist = new List<Leave>();
                    foreach (var item1 in Leavelist)
                    {
                        Leave Leave = new Leave();
                        Leave.leaveItems = item1.leaveItems;
                        Leave.beginTime = item1.beginTime;
                        Leave.stopTime = item1.stopTime;
                        Leave.hour = item1.hour;
                        leavelist.Add(Leave);
                    }
                    item.leave = leavelist;
                }
                //出差报销
                if (item.travelRebStr != null && item.travelRebStr != "")
                {
                    var TravelReblist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TravelReb>>(item.travelRebStr);
                    List<TravelReb> travelReblist = new List<TravelReb>();
                    foreach (var item1 in TravelReblist)
                    {
                        TravelReb TravelReb = new TravelReb();
                        TravelReb.travelRebType = item1.travelRebType;
                        TravelReb.goArea = item1.goArea;
                        TravelReb.toArea = item1.toArea;
                        TravelReb.goDate = item1.goDate;
                        TravelReb.toDate = item1.toDate;
                        TravelReb.money = item1.money;
                        TravelReb.remarks = item1.remarks;
                        if (item1.proPicture != null && item1.proPicture != "")
                        {
                            var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item1.proPicture);
                            foreach (var itemPicture in listPicture)
                            {
                                itemPicture.url = itemPicture.url + SasKey;
                            }
                            var str = Newtonsoft.Json.JsonConvert.SerializeObject(listPicture);
                            item1.proPicture = str;
                        }
                        TravelReb.proPicture = item1.proPicture;
                        travelReblist.Add(TravelReb);
                    }
                    item.travelReb = travelReblist;
                }
                //普通报销
                if (item.rebStr != null && item.rebStr != "")
                {
                    var Reblist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Reb>>(item.rebStr);
                    List<Reb> reblist = new List<Reb>();
                    foreach (var item1 in Reblist)
                    {
                        Reb Reb = new Reb();
                        Reb.reimbursementMatters = item1.reimbursementMatters;
                        Reb.money = item1.money;
                        Reb.remarks = item1.remarks;
                        Reb.associatedCustomer = item1.associatedCustomer;
                        if (item1.proPicture != null && item1.proPicture != "")
                        {
                            var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item1.proPicture);
                            foreach (var itemPicture in listPicture)
                            {
                                itemPicture.url = itemPicture.url + SasKey;
                            }
                            var str = Newtonsoft.Json.JsonConvert.SerializeObject(listPicture);
                            item1.proPicture = str;
                        }
                        Reb.proPicture = item1.proPicture;
                        reblist.Add(Reb);
                    }
                    item.reb = reblist;
                }
                //审批进度
                var percentageStr = "";
                switch (item.state)
                {
                    case "审批中":
                        if (Approval_Content.Count != 0 && Approval_Content != null)
                        {
                            var Approval_ContentOne = Approval_Content.Where(t => t.State == 0).FirstOrDefault();
                            var Approval_ContentOneBefore = Approval_Content.Where(t => t.OtherMemberOrder == (Approval_ContentOne.OtherMemberOrder - 1)).FirstOrDefault();
                            if (Approval_ContentOneBefore != null)
                            {
                                percentageStr += Approval_ContentOneBefore.OtherMemberName + "已提交 ";
                            }
                            if (Approval_ContentOne != null)
                            {
                                percentageStr += "待" + Approval_ContentOne.OtherMemberName + "审批";
                            }
                        }
                        break;
                    case "已批准":
                        percentageStr = "此审批已被批准";
                        break;
                    case "已取消":
                        percentageStr = "此审批已被取消";
                        break;
                    case "未批准":
                        percentageStr = "此审批未被批准";
                        break;
                }
                item.percentage = percentageStr;
            }
            //指令  由谁执行   信息
            //超时标志
            var list5 = list.Where(t => t.type == "指令").ToList();
            foreach (var item in list5)
            {
                //指令  由谁执行   信息
                var execute = _JointOfficeContext.Execute_Content.Where(t => t.UId == item.id).FirstOrDefault();
                var workOrder = _JointOfficeContext.Work_Order.Where(t => t.Id == item.id).FirstOrDefault();
                switch (item.state)
                {
                    case "执行中":
                        item.exeBody = "该指令由" + execute.OtherMemberName + "执行,应于" + string.Format("{0:f}", workOrder.StopTime) + "前完成。";
                        break;
                    case "已执行":
                        item.exeBody = "该指令由" + execute.OtherMemberName + "执行,已于" + string.Format("{0:f}", execute.ExecuteDate) + "完成。";
                        break;
                    case "已取消":
                        item.exeBody = "该指令由" + execute.OtherMemberName + "执行,已于" + string.Format("{0:f}", execute.ExecuteDate) + "取消。";
                        break;
                }
                //超时标志
                if (DateTime.Now > Convert.ToDateTime(workOrder.StopTime))
                {
                    item.overTimeMark = "1";
                }
                else
                {
                    item.overTimeMark = "0";
                }
            }
            //当前用户是否为执行人/审批人/点评人
            var list6 = list.Where(t => t.type != "日程").ToList();
            foreach (var item in list6)
            {
                if (item.type == "审批")
                {
                    var approvalContent = _JointOfficeContext.Approval_Content.Where(t => t.UId == item.id && t.OtherMemberId == memberid && t.IsMeApproval == "1").FirstOrDefault();
                    if (approvalContent != null)
                    {
                        item.isExecutor = "1";
                    }
                    else
                    {
                        item.isExecutor = "0";
                    }
                }
                if (item.type == "日志")
                {
                    if (item.reviewPersonId == memberid)
                    {
                        item.isExecutor = "1";
                    }
                    else
                    {
                        item.isExecutor = "0";
                    }
                }
                if (item.type == "任务")
                {
                    var executorList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(item.typeExecutor);
                    if (executorList != null && executorList.Count != 0)
                    {
                        foreach (var item1 in executorList)
                        {
                            if (item1.id == memberid)
                            {
                                item.isExecutor = "1";
                                break;
                            }
                            else
                            {
                                item.isExecutor = "0";
                            }
                        }
                    }
                }
                if (item.type == "指令")
                {
                    var executorList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(item.typeExecutor);
                    if (executorList != null && executorList.Count != 0)
                    {
                        if (executorList[0].id == memberid)
                        {
                            item.isExecutor = "1";
                        }
                        else
                        {
                            item.isExecutor = "0";
                        }
                    }
                }
                if (item.type == "公告")
                {
                    //公告标题
                    item.title = item.taskTitle;
                }
            }
            //日程参与人
            //日程状态
            var list7 = list.Where(t => t.type == "日程").ToList();
            foreach (var item in list7)
            {
                //日程参与人
                var joinPersonList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(item.joinPerson);
                if (joinPersonList != null && joinPersonList.Count != 0)
                {
                    item.joinPerson = "";
                    if (joinPersonList.Count > 2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            var joinPersonInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == joinPersonList[i].id).FirstOrDefault();
                            var joinPersonStr = joinPersonInfo.Name;
                            if ((i + 1) != 2)
                            {
                                item.joinPerson += joinPersonStr + "、";
                            }
                            else
                            {
                                item.joinPerson += joinPersonStr + "等" + joinPersonList.Count() + "人";
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < joinPersonList.Count; i++)
                        {
                            var joinPersonInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == joinPersonList[i].id).FirstOrDefault();
                            var joinPersonStr = joinPersonInfo.Name;
                            if ((i + 1) != joinPersonList.Count)
                            {
                                item.joinPerson += joinPersonStr + "、";
                            }
                            else
                            {
                                item.joinPerson += joinPersonStr;
                            }
                        }
                    }
                }
                else
                {
                    item.joinPerson = "(参与人已取消日程)";
                }
                //日程状态
                switch (item.state)
                {
                    case "未开始":
                        if (DateTime.Now < Convert.ToDateTime(item.beginTime))
                        {
                            item.state = "未开始";
                        }
                        else if (DateTime.Now >= Convert.ToDateTime(item.beginTime))
                        {
                            item.state = "已开始";
                            var workProgram = _JointOfficeContext.Work_Program.Where(t => t.Id == item.id).FirstOrDefault();
                            if (workProgram != null)
                            {
                                workProgram.State = 1;
                                _JointOfficeContext.SaveChanges();
                            }
                        }
                        break;
                    case "已开始":
                        item.state = "已开始";
                        break;
                    case "已删除":
                        item.state = "已取消";
                        break;
                }
            }
            var list11 = list.Where(t => t.type == "日志" || t.type == "日程" || t.type == "公告" || t.type == "分享").ToList();
            foreach (var item in list11)
            {
                //是否需要当前用户回执
                if (item.receipt != null)
                {
                    if (item.receipt.Contains(memberid))
                    {
                        item.isReceipt = "1";
                    }
                    else
                    {
                        item.isReceipt = "0";
                    }
                }
                else
                {
                    item.isReceipt = "0";
                }
                //回执
                var receipts = _JointOfficeContext.Receipts.Where(t => t.UId == item.id).ToList();
                if (receipts != null && receipts.Count != 0)
                {
                    var receiptsNum = 0;
                    foreach (var receiptsOne in receipts)
                    {
                        if (receiptsOne.Body != null && receiptsOne.Body != "")
                        {
                            receiptsNum++;
                        }
                    }
                    item.remarks = "需要" + receipts.Count() + "人回执，已有" + receiptsNum + "人回执。";
                }
                else
                {
                    item.remarks = "需要0人回执，已有0人回执";
                }
                var receipts1 = receipts.Where(t => t.MemberId == memberid).FirstOrDefault();
                if (receipts1 != null)
                {
                    if (receipts1.Body != null && receipts1.Body != "")
                    {
                        item.isReceiptOk = "1";
                    }
                    else
                    {
                        item.isReceiptOk = "0";
                    }
                }
                else
                {
                    item.isReceiptOk = "1";
                }
            }
            return list;
        }
        /// <summary>
        /// 审批详情页  one
        /// </summary>
        /// <returns></returns>
        public DaiShenPi GetDaiShenPiOne(Work_Approval item, string memberid)
        {
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
            var TotalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == item.Id).FirstOrDefault();
            var Agree = _JointOfficeContext.Agree.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            var Approval_Content = _JointOfficeContext.Approval_Content.Where(t => t.UId == item.Id).OrderBy(t => t.OtherMemberOrder).ToList();
            DaiShenPi DaiShenPi = new DaiShenPi();
            DaiShenPi.dyTypeName = "审批";
            switch (item.Type)
            {
                case "0":
                    DaiShenPi.approvalType = "";
                    DaiShenPi.approvalTypeNum = "0";
                    break;
                case "1":
                    DaiShenPi.approvalType = "加班";
                    DaiShenPi.approvalTypeNum = "1";
                    break;
                case "2":
                    DaiShenPi.approvalType = "出差";
                    DaiShenPi.approvalTypeNum = "2";
                    break;
                case "3":
                    DaiShenPi.approvalType = "请假";
                    DaiShenPi.approvalTypeNum = "3";
                    break;
                case "4":
                    DaiShenPi.approvalType = "差旅报销";
                    DaiShenPi.approvalTypeNum = "4";
                    break;
                case "5":
                    DaiShenPi.approvalType = "普通报销";
                    DaiShenPi.approvalTypeNum = "5";
                    break;
            }
            DaiShenPi.shenpiid = item.Id;
            DaiShenPi.code = item.Code;
            DaiShenPi.body = item.Body;
            DaiShenPi.shenpiMemberid = item.MemberId;
            DaiShenPi.picture = memberInfo.Picture + SasKey;
            DaiShenPi.name = memberInfo.Name;
            DaiShenPi.createDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm");
            var percentageStr = "";
            switch (item.State)
            {
                case 0:
                    DaiShenPi.state = "审批中";
                    if (Approval_Content.Count != 0 && Approval_Content != null)
                    {
                        var Approval_ContentOne = Approval_Content.Where(t => t.State == 0).FirstOrDefault();
                        var Approval_ContentOneBefore = Approval_Content.Where(t => t.OtherMemberOrder == (Approval_ContentOne.OtherMemberOrder - 1)).FirstOrDefault();
                        if (Approval_ContentOneBefore != null)
                        {
                            percentageStr += Approval_ContentOneBefore.OtherMemberName + "已提交 ";
                        }
                        if (Approval_ContentOne != null)
                        {
                            percentageStr += "待" + Approval_ContentOne.OtherMemberName + "审批";
                        }
                    }
                    break;
                case 1:
                    DaiShenPi.state = "已批准";
                    percentageStr = "此审批已被批准";
                    break;
                case 2:
                    DaiShenPi.state = "已取消";
                    percentageStr = "此审批已被取消";
                    break;
                case 3:
                    DaiShenPi.state = "未批准";
                    percentageStr = "此审批未被批准";
                    break;
                case 4:
                    DaiShenPi.state = "已取消";
                    percentageStr = "此审批已被取消";
                    break;
            }
            DaiShenPi.percentage = percentageStr;
            if (item.Range != null && item.Range != "" && item.Range != "[]")
            {
                if (string.IsNullOrEmpty(item.RangeNew))
                {
                    DaiShenPi.range = GetRange(item.Range);
                }
                else
                {
                    DaiShenPi.range = GetRange(item.RangeNew);
                }
            }
            else
            {
                DaiShenPi.range = "公开";
            }
            //加班总时长
            DaiShenPi.workDuration = item.WorkDuration;
            //加班
            if (item.OverTime != "" && item.OverTime != null)
            {
                var OverTimelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OverTime>>(item.OverTime);
                List<OverTime> overTimelist = new List<OverTime>();
                foreach (var item1 in OverTimelist)
                {
                    OverTime OverTime = new OverTime();
                    OverTime.beginTime = item1.beginTime;
                    OverTime.stopTime = item1.stopTime;
                    OverTime.hour = item1.hour;
                    overTimelist.Add(OverTime);
                }
                DaiShenPi.overTime = overTimelist;
            }
            //出差总详情
            DaiShenPi.travelAll = item.TravelAll;
            //出差分详情
            if (item.Travel != "" && item.Travel != null)
            {
                var Travellist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Travel>>(item.Travel);
                List<Travel> travellist = new List<Travel>();
                foreach (var item1 in Travellist)
                {
                    Travel Travel = new Travel();
                    Travel.goArea = item1.goArea;
                    Travel.toArea = item1.toArea;
                    Travel.goDate = item1.goDate;
                    Travel.toDate = item1.toDate;
                    Travel.tran = item1.tran;
                    Travel.stay = item1.stay;
                    travellist.Add(Travel);
                }
                DaiShenPi.travel = travellist;
            }
            //请假
            if (item.Leave != "" && item.Leave != null)
            {
                var Leavelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Leave>>(item.Leave);
                List<Leave> leavelist = new List<Leave>();
                foreach (var item1 in Leavelist)
                {
                    Leave Leave = new Leave();
                    Leave.leaveItems = item1.leaveItems;
                    Leave.beginTime = item1.beginTime;
                    Leave.stopTime = item1.stopTime;
                    Leave.hour = item1.hour;
                    leavelist.Add(Leave);
                }
                DaiShenPi.leave = leavelist;
            }
            //出差报销
            if (item.TravelReb != "" && item.TravelReb != null)
            {
                var TravelReblist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TravelReb>>(item.TravelReb);
                List<TravelReb> travelReblist = new List<TravelReb>();
                foreach (var item1 in TravelReblist)
                {
                    TravelReb TravelReb = new TravelReb();
                    TravelReb.travelRebType = item1.travelRebType;
                    TravelReb.goArea = item1.goArea;
                    TravelReb.toArea = item1.toArea;
                    TravelReb.goDate = item1.goDate;
                    TravelReb.toDate = item1.toDate;
                    TravelReb.money = item1.money;
                    TravelReb.remarks = item1.remarks;
                    if (item1.proPicture != null && item1.proPicture != "")
                    {
                        var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item1.proPicture);
                        foreach (var itemPicture in listPicture)
                        {
                            itemPicture.url = itemPicture.url + SasKey;
                        }
                        var str = Newtonsoft.Json.JsonConvert.SerializeObject(listPicture);
                        item1.proPicture = str;
                    }
                    TravelReb.proPicture = item1.proPicture;
                    travelReblist.Add(TravelReb);
                }
                DaiShenPi.travelReb = travelReblist;
            }
            //普通报销
            if (item.Reb != "" && item.Reb != null)
            {
                var Reblist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Reb>>(item.Reb);
                List<Reb> reblist = new List<Reb>();
                foreach (var item1 in Reblist)
                {
                    Reb Reb = new Reb();
                    Reb.reimbursementMatters = item1.reimbursementMatters;
                    Reb.money = item1.money;
                    Reb.remarks = item1.remarks;
                    Reb.associatedCustomer = item1.associatedCustomer;
                    if (item1.proPicture != null && item1.proPicture != "")
                    {
                        var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item1.proPicture);
                        foreach (var itemPicture in listPicture)
                        {
                            itemPicture.url = itemPicture.url + SasKey;
                        }
                        var str = Newtonsoft.Json.JsonConvert.SerializeObject(listPicture);
                        item1.proPicture = str;
                    }
                    Reb.proPicture = item1.proPicture;
                    reblist.Add(Reb);
                }
                DaiShenPi.reb = reblist;
            }
            DaiShenPi.leaveDuration = item.LeaveDuration;
            DaiShenPi.travelMoney = item.TravelMoney;
            DaiShenPi.rebMoney = item.RebMoney;
            if (TotalNum != null)
            {
                if (TotalNum.PingLunNum != 0)
                {
                    DaiShenPi.pingLunNum = TotalNum.PingLunNum;
                }
                else
                {
                    DaiShenPi.pingLunNum = 0;
                }
                if (TotalNum.DianZanNum != 0)
                {
                    DaiShenPi.dianZanNum = TotalNum.DianZanNum;
                }
                else
                {
                    DaiShenPi.dianZanNum = 0;
                }
            }
            else
            {
                DaiShenPi.pingLunNum = 0;
                DaiShenPi.dianZanNum = 0;
            }
            if (Agree == null)
            {
                DaiShenPi.isZan = 0;
            }
            else
            {
                DaiShenPi.isZan = 1;
            }
            DaiShenPi.phoneModel = item.PhoneModel;
            DaiShenPi.map = item.Map;
            DaiShenPi.address = item.Address;
            if (item.Picture != null && item.Picture != "")
            {
                var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item.Picture);
                foreach (var itemPicture in listPicture)
                {
                    itemPicture.url = itemPicture.url + SasKey;
                }
                DaiShenPi.appendPicture = listPicture;
            }
            if (item.Annex != null && item.Annex != "")
            {
                var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Work_File>>(item.Annex);
                long length = 0;
                foreach (var itemAnnex in listAnnex)
                {
                    itemAnnex.url = itemAnnex.url + SasKey;
                    itemAnnex.lengthStr = BusinessHelper.ConvertBytes(itemAnnex.length);
                    length += itemAnnex.length;
                }
                DaiShenPi.annex = listAnnex;
                DaiShenPi.annexLength = BusinessHelper.ConvertBytes(length);
            }
            if (item.Voice != null && item.Voice != "")
            {
                DaiShenPi.voice = item.Voice + SasKey;
            }
            if (item.VoiceLength != null && item.VoiceLength != "" && item.VoiceLength.Substring(0, 1) == "0")
            {
                DaiShenPi.voiceLength = item.VoiceLength.Substring(1, 1);
            }
            else
            {
                DaiShenPi.voiceLength = item.VoiceLength;
            }
            //审批人
            List<Executor> list1 = new List<Executor>();
            if (Approval_Content.Count != 0 && Approval_Content != null)
            {
                foreach (var one in Approval_Content)
                {
                    Executor Executor = new Executor();
                    Executor.id = one.OtherMemberId;
                    Executor.name = one.OtherMemberName;
                    Executor.picture = one.OtherMemberPicture + SasKey;
                    Executor.executorCompleteRemarks = one.Content;
                    Executor.executorCompleteTime = one.ApprovalTime;
                    Executor.num = one.State.ToString();
                    switch (one.State)
                    {
                        case 0:
                            Executor.type = "审批中";
                            break;
                        case 1:
                            Executor.type = "已批准";
                            break;
                        case 2:
                            Executor.type = "已取消";
                            break;
                        case 3:
                            Executor.type = "未批准";
                            break;
                        case 5:
                            Executor.type = "待审批";
                            break;
                        case 6:
                            Executor.type = "已失效";
                            break;
                    }
                    list1.Add(Executor);
                }
            }
            DaiShenPi.executor = list1;
            //审批列表
            var comment = _JointOfficeContext.Comment_Body.Where(t => t.UId == item.Id && t.IsExeComment == 1).OrderByDescending(t => t.PingLunTime).ToList();
            List<ExeCommentList> list2 = new List<ExeCommentList>();
            foreach (var itemcom in comment)
            {
                ExeCommentList ExeCommentList = new ExeCommentList();
                switch (itemcom.Body.Substring(0, 1))
                {
                    case "已":
                        ExeCommentList.state = "已批准";
                        ExeCommentList.stateNum = "2";
                        break;
                    case "取":
                        ExeCommentList.state = "取消审批";
                        ExeCommentList.stateNum = "3";
                        break;
                    case "未":
                        ExeCommentList.state = "未批准";
                        ExeCommentList.stateNum = "3";
                        break;
                }
                ExeCommentList.name = itemcom.Name;
                ExeCommentList.exeDate = itemcom.PingLunTime.ToString("yyyy-MM-dd HH:mm");
                ExeCommentList.exeBody = itemcom.Body;
                ExeCommentList.phoneModel = itemcom.PhoneModel;
                list2.Add(ExeCommentList);
            }
            DaiShenPi.exeCommentList = list2;
            //
            var approvalPerson = Approval_Content.Where(t => t.OtherMemberId == memberid && t.IsMeApproval == "1").FirstOrDefault();
            if (approvalPerson != null)
            {
                DaiShenPi.isExecutor = "1";
            }
            else
            {
                DaiShenPi.isExecutor = "0";
            }
            return DaiShenPi;
        }
        /// <summary>
        /// 日志详情页  one
        /// </summary>
        /// <returns></returns>
        public DaiDianPingDeRiZhi GetDaiDianPingDeRiZhiOne(Work_Log item, string memberid)
        {
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
            var dianPing_Body = _JointOfficeContext.DianPing_Body.Where(t => t.UId == item.Id).FirstOrDefault();
            var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == item.Id).FirstOrDefault();
            var Agree = _JointOfficeContext.Agree.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            DaiDianPingDeRiZhi DaiDianPingDeRiZhi = new DaiDianPingDeRiZhi();
            DaiDianPingDeRiZhi.dyTypeName = "日志";
            DaiDianPingDeRiZhi.moneyInfo = item.MoneyInfo;
            DaiDianPingDeRiZhi.money = item.Money;
            DaiDianPingDeRiZhi.rizhiid = item.Id;
            DaiDianPingDeRiZhi.rizhimemberid = item.MemberId;
            DaiDianPingDeRiZhi.picture = memberInfo.Picture + SasKey;
            DaiDianPingDeRiZhi.name = memberInfo.Name;
            DaiDianPingDeRiZhi.createDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm");
            DaiDianPingDeRiZhi.reviewPersonName = item.ReviewPersonName;
            DaiDianPingDeRiZhi.moban = item.MoBan;
            DaiDianPingDeRiZhi.mobanTime = item.MoBanTime;
            switch (item.State)
            {
                case 0:
                    DaiDianPingDeRiZhi.state = "未点评";
                    break;
                case 1:
                    DaiDianPingDeRiZhi.state = "已点评";
                    break;
                case 2:
                    DaiDianPingDeRiZhi.state = "已删除";
                    break;
            }
            if (dianPing_Body != null)
            {
                DaiDianPingDeRiZhi.grade = dianPing_Body.Grade;
                DaiDianPingDeRiZhi.gradeNum = Convert.ToInt32(dianPing_Body.Grade);
            }
            else
            {
                DaiDianPingDeRiZhi.grade = "0";
                DaiDianPingDeRiZhi.gradeNum = 0;
            }
            //执行列表
            var dianpingList = _JointOfficeContext.DianPing_Body.Where(t => t.UId == item.Id).FirstOrDefault();
            List<ExeCommentList> list1 = new List<ExeCommentList>();
            if (dianpingList != null)
            {
                ExeCommentList ExeCommentList = new ExeCommentList();
                ExeCommentList.name = dianpingList.Name;
                ExeCommentList.exeDate = dianpingList.DianPingTime.ToString("yyyy-MM-dd HH:mm");
                ExeCommentList.exeBody = dianpingList.Body;
                ExeCommentList.state = "点评日志";
                ExeCommentList.stateNum = "1";
                ExeCommentList.phoneModel = dianpingList.PhoneModel;
                list1.Add(ExeCommentList);
            }
            DaiDianPingDeRiZhi.exeCommentList = list1;
            DaiDianPingDeRiZhi.workSummary = item.WorkSummary;
            DaiDianPingDeRiZhi.workPlan = item.WorkPlan;
            DaiDianPingDeRiZhi.experience = item.Experience;
            if (item.Range != null && item.Range != "" && item.Range != "[]")
            {
                if (string.IsNullOrEmpty(item.RangeNew))
                {
                    DaiDianPingDeRiZhi.range = GetRange(item.Range);
                }
                else
                {
                    DaiDianPingDeRiZhi.range = GetRange(item.RangeNew);
                }
            }
            else
            {
                DaiDianPingDeRiZhi.range = "公开";
            }
            //回执
            var receipts = _JointOfficeContext.Receipts.Where(t => t.UId == item.Id).ToList();
            if (receipts != null && receipts.Count != 0)
            {
                var receiptsNum = 0;
                foreach (var receiptsOne in receipts)
                {
                    if (receiptsOne.Body != null && receiptsOne.Body != "")
                    {
                        receiptsNum++;
                    }
                }
                DaiDianPingDeRiZhi.remarks = "需要" + receipts.Count() + "人回执，已有" + receiptsNum + "人回执。";
            }
            else
            {
                DaiDianPingDeRiZhi.remarks = "需要0人回执，已有0人回执";
            }
            var receipts1 = receipts.Where(t => t.MemberId == memberid).FirstOrDefault();
            if (receipts1 != null)
            {
                if (receipts1.Body != null && receipts1.Body != "")
                {
                    DaiDianPingDeRiZhi.isReceiptOk = "1";
                }
                else
                {
                    DaiDianPingDeRiZhi.isReceiptOk = "0";
                }
            }
            else
            {
                DaiDianPingDeRiZhi.isReceiptOk = "1";
            }
            if (totalNum != null)
            {
                if (totalNum.PingLunNum != 0)
                {
                    DaiDianPingDeRiZhi.pingLunNum = totalNum.PingLunNum;
                }
                else
                {
                    DaiDianPingDeRiZhi.pingLunNum = 0;
                }
                if (totalNum.DianZanNum != 0)
                {
                    DaiDianPingDeRiZhi.dianZanNum = totalNum.DianZanNum;
                }
                else
                {
                    DaiDianPingDeRiZhi.dianZanNum = 0;
                }
            }
            else
            {
                DaiDianPingDeRiZhi.pingLunNum = 0;
                DaiDianPingDeRiZhi.dianZanNum = 0;
            }
            if (Agree == null)
            {
                DaiDianPingDeRiZhi.isZan = 0;
            }
            else
            {
                DaiDianPingDeRiZhi.isZan = 1;
            }
            DaiDianPingDeRiZhi.map = item.Map;
            DaiDianPingDeRiZhi.address = item.Address;
            if (item.Picture != null && item.Picture != "")
            {
                var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item.Picture);
                foreach (var itemPicture in listPicture)
                {
                    itemPicture.url = itemPicture.url + SasKey;
                }
                DaiDianPingDeRiZhi.appendPicture = listPicture;
            }
            if (item.Annex != null && item.Annex != "")
            {
                var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Work_File>>(item.Annex);
                long length = 0;
                foreach (var itemAnnex in listAnnex)
                {
                    itemAnnex.url = itemAnnex.url + SasKey;
                    itemAnnex.lengthStr = BusinessHelper.ConvertBytes(itemAnnex.length);
                    length += itemAnnex.length;
                }
                DaiDianPingDeRiZhi.annex = listAnnex;
                DaiDianPingDeRiZhi.annexLength = BusinessHelper.ConvertBytes(length);
            }
            if (item.Voice != null && item.Voice != "")
            {
                DaiDianPingDeRiZhi.voice = item.Voice + SasKey;
            }
            if (item.VoiceLength != null && item.VoiceLength != "" && item.VoiceLength.Substring(0, 1) == "0")
            {
                DaiDianPingDeRiZhi.voiceLength = item.VoiceLength.Substring(1, 1);
            }
            else
            {
                DaiDianPingDeRiZhi.voiceLength = item.VoiceLength;
            }
            DaiDianPingDeRiZhi.phoneModel = item.PhoneModel;
            if (item.ReviewPersonId == memberid)
            {
                DaiDianPingDeRiZhi.isExecutor = "1";
            }
            else
            {
                DaiDianPingDeRiZhi.isExecutor = "0";
            }
            var focus = _JointOfficeContext.News_Focus.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            if (focus != null)
            {
                DaiDianPingDeRiZhi.isFocus = "1";
            }
            else
            {
                DaiDianPingDeRiZhi.isFocus = "0";
            }
            var collection = _JointOfficeContext.News_Collection.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            if (collection != null)
            {
                DaiDianPingDeRiZhi.isCollection = "1";
            }
            else
            {
                DaiDianPingDeRiZhi.isCollection = "0";
            }
            //是否需要当前用户回执
            if (item.Receipt != null)
            {
                if (item.Receipt.Contains(memberid))
                {
                    DaiDianPingDeRiZhi.isReceipt = "1";
                }
                else
                {
                    DaiDianPingDeRiZhi.isReceipt = "0";
                }
            }
            else
            {
                DaiDianPingDeRiZhi.isReceipt = "0";
            }
            return DaiDianPingDeRiZhi;
        }
        /// <summary>
        /// 任务详情页  one
        /// </summary>
        /// <returns></returns>
        public DaiZhiXingDeRenWu GetDaiZhiXingDeRenWuOne(Work_Task item, string memberid)
        {
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
            var execute1 = _JointOfficeContext.Execute_Content.Where(t => t.UId == item.Id).ToList();
            var execute2 = execute1.Where(t => t.State == 1).ToList();
            var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == item.Id).FirstOrDefault();
            var Agree = _JointOfficeContext.Agree.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            DaiZhiXingDeRenWu DaiZhiXingDeRenWu = new DaiZhiXingDeRenWu();
            DaiZhiXingDeRenWu.dyTypeName = "任务";
            DaiZhiXingDeRenWu.renwuid = item.Id;
            DaiZhiXingDeRenWu.taskTitle = item.TaskTitle;
            DaiZhiXingDeRenWu.remarks = item.Remarks;
            DaiZhiXingDeRenWu.renwuMemberid = item.MemberId;
            DaiZhiXingDeRenWu.picture = memberInfo.Picture + SasKey;
            DaiZhiXingDeRenWu.name = memberInfo.Name;
            DaiZhiXingDeRenWu.createDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm");
            switch (item.State)
            {
                case 0:
                    DaiZhiXingDeRenWu.state = "执行中";
                    DaiZhiXingDeRenWu.stateNum = "0";
                    break;
                case 1:
                    DaiZhiXingDeRenWu.state = "已执行";
                    DaiZhiXingDeRenWu.stateNum = "1";
                    break;
                case 2:
                    DaiZhiXingDeRenWu.state = "已取消";
                    DaiZhiXingDeRenWu.stateNum = "2";
                    break;
            }
            var dianping = _JointOfficeContext.DianPing_Body.Where(t => t.UId == item.Id).FirstOrDefault();
            if (dianping != null)
            {
                DaiZhiXingDeRenWu.grade = dianping.Grade;
                DaiZhiXingDeRenWu.gradeNum = Convert.ToInt32(dianping.Grade);
            }
            else
            {
                DaiZhiXingDeRenWu.grade = "0";
                DaiZhiXingDeRenWu.gradeNum = 0;
            }
            if (item.Range != null && item.Range != "" && item.Range != "[]")
            {
                if (string.IsNullOrEmpty(item.RangeNew))
                {
                    DaiZhiXingDeRenWu.range = GetRange(item.Range);
                }
                else
                {
                    DaiZhiXingDeRenWu.range = GetRange(item.RangeNew);
                }
            }
            else
            {
                DaiZhiXingDeRenWu.range = "公开";
            }
            DaiZhiXingDeRenWu.executorNum = execute2.Count().ToString() + "/" + execute1.Count().ToString();
            DaiZhiXingDeRenWu.executorNumYes = execute2.Count().ToString();
            DaiZhiXingDeRenWu.executorNumAll = execute1.Count().ToString();
            //执行进度
            var num1 = execute2.Count() + ".0";
            double num2 = Convert.ToDouble(num1) / Convert.ToDouble(execute1.Count());
            var percentage = Math.Round(num2, 2);
            DaiZhiXingDeRenWu.percentage = percentage.ToString();
            //
            DaiZhiXingDeRenWu.stopTime = item.StopTime;
            //提醒时间
            var remindTimelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RemindTime>>(item.RemindTime);
            var remindTimeStr = "";
            for (int i = 0; i < remindTimelist.Count; i++)
            {
                remindTimeStr = remindTimeStr + remindTimelist[i].timeWord;
                if ((i + 1) != remindTimelist.Count)
                {
                    remindTimeStr = remindTimeStr + ",";
                }
            }
            DaiZhiXingDeRenWu.remindTime = remindTimeStr;
            //
            if (totalNum != null)
            {
                if (totalNum.PingLunNum != 0)
                {
                    DaiZhiXingDeRenWu.pingLunNum = totalNum.PingLunNum;
                }
                else
                {
                    DaiZhiXingDeRenWu.pingLunNum = 0;
                }
                if (totalNum.DianZanNum != 0)
                {
                    DaiZhiXingDeRenWu.dianZanNum = totalNum.DianZanNum;
                }
                else
                {
                    DaiZhiXingDeRenWu.dianZanNum = 0;
                }
            }
            else
            {
                DaiZhiXingDeRenWu.pingLunNum = 0;
                DaiZhiXingDeRenWu.dianZanNum = 0;
            }
            if (Agree == null)
            {
                DaiZhiXingDeRenWu.isZan = 0;
            }
            else
            {
                DaiZhiXingDeRenWu.isZan = 1;
            }
            DaiZhiXingDeRenWu.phoneModel = item.PhoneModel;
            DaiZhiXingDeRenWu.map = item.Map;
            DaiZhiXingDeRenWu.address = item.Address;
            if (item.Picture != null && item.Picture != "")
            {
                var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item.Picture);
                foreach (var itemPicture in listPicture)
                {
                    itemPicture.url = itemPicture.url + SasKey;
                }
                DaiZhiXingDeRenWu.appendPicture = listPicture;
            }
            if (item.Annex != null && item.Annex != "")
            {
                var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Work_File>>(item.Annex);
                long length = 0;
                foreach (var itemAnnex in listAnnex)
                {
                    itemAnnex.url = itemAnnex.url + SasKey;
                    itemAnnex.lengthStr = BusinessHelper.ConvertBytes(itemAnnex.length);
                    length += itemAnnex.length;
                }
                DaiZhiXingDeRenWu.annex = listAnnex;
                DaiZhiXingDeRenWu.annexLength = BusinessHelper.ConvertBytes(length);
            }
            if (item.Voice != null && item.Voice != "")
            {
                DaiZhiXingDeRenWu.voice = item.Voice + SasKey;
            }
            if (item.VoiceLength != null && item.VoiceLength != "" && item.VoiceLength.Substring(0, 1) == "0")
            {
                DaiZhiXingDeRenWu.voiceLength = item.VoiceLength.Substring(1, 1);
            }
            else
            {
                DaiZhiXingDeRenWu.voiceLength = item.VoiceLength;
            }
            //执行人List
            List<Executor> list1 = new List<Executor>();
            if (execute1.Count != 0 && execute1 != null)
            {
                foreach (var item1 in execute1)
                {
                    Executor Executor = new Executor();
                    Executor.id = item1.OtherMemberId;
                    Executor.name = item1.OtherMemberName;
                    Executor.picture = item1.OtherMemberPicture + SasKey;
                    Executor.executorCompleteTime = item1.ExecuteDate;
                    Executor.executorCompleteRemarks = item1.Content;
                    switch (item1.State)
                    {
                        case 0:
                            Executor.type = "执行中";
                            break;
                        case 1:
                            Executor.type = "已执行";
                            break;
                        case 2:
                            Executor.type = "已取消";
                            break;
                        case 3:
                            Executor.type = "已失效";
                            break;
                    }
                    list1.Add(Executor);
                }
            }
            DaiZhiXingDeRenWu.executor = list1;
            //执行列表
            var dianpingList = _JointOfficeContext.DianPing_Body.Where(t => t.UId == item.Id).FirstOrDefault();
            var comment = _JointOfficeContext.Comment_Body.Where(t => t.UId == item.Id && t.IsExeComment == 1).OrderByDescending(t => t.PingLunTime).ToList();
            List<ExeCommentList> list2 = new List<ExeCommentList>();
            if (dianpingList != null)
            {
                ExeCommentList ExeCommentList = new ExeCommentList();
                ExeCommentList.name = dianpingList.Name;
                ExeCommentList.exeDate = dianpingList.DianPingTime.ToString("yyyy-MM-dd HH:mm");
                ExeCommentList.exeBody = dianpingList.Body;
                ExeCommentList.state = "点评任务";
                ExeCommentList.stateNum = "1";
                ExeCommentList.phoneModel = dianpingList.PhoneModel;
                list2.Add(ExeCommentList);
            }
            foreach (var itemcom in comment)
            {
                ExeCommentList ExeCommentList = new ExeCommentList();
                switch (itemcom.Body.Substring(0, 2))
                {
                    case "完成":
                        ExeCommentList.state = "完成任务";
                        ExeCommentList.stateNum = "2";
                        break;
                    case "取消":
                        ExeCommentList.state = "取消任务";
                        ExeCommentList.stateNum = "3";
                        break;
                }
                ExeCommentList.name = itemcom.Name;
                ExeCommentList.exeDate = itemcom.PingLunTime.ToString("yyyy-MM-dd HH:mm");
                ExeCommentList.exeBody = itemcom.Body;
                ExeCommentList.phoneModel = itemcom.PhoneModel;
                list2.Add(ExeCommentList);
            }
            DaiZhiXingDeRenWu.exeCommentList = list2;
            //当前用户是否为执行人   执行人执行完没
            var executor = execute1.Select(t => t.OtherMemberId).ToList();
            if (executor.Contains(memberid))
            {
                DaiZhiXingDeRenWu.isExecutor = "1";
                var execute3 = execute1.Where(t => t.OtherMemberId == memberid).FirstOrDefault();
                if (execute3 != null)
                {
                    switch (execute3.State)
                    {
                        case 0:
                            DaiZhiXingDeRenWu.isExe = "0";
                            break;
                        case 1:
                            DaiZhiXingDeRenWu.isExe = "1";
                            break;
                        case 2:
                            DaiZhiXingDeRenWu.isExe = "2";
                            break;
                    }
                }
            }
            else
            {
                DaiZhiXingDeRenWu.isExecutor = "0";
                DaiZhiXingDeRenWu.isExe = "";
            }
            //此任务谁完成了
            if (execute2.Count > 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    var exePersonStr = execute2[i].OtherMemberName;
                    if ((i + 1) != 2)
                    {
                        DaiZhiXingDeRenWu.exePerson += exePersonStr + "、";
                    }
                    else
                    {
                        DaiZhiXingDeRenWu.exePerson += exePersonStr + "等" + execute2.Count() + "人";
                    }
                }
            }
            else
            {
                for (int i = 0; i < execute2.Count; i++)
                {
                    var exePersonStr = execute2[i].OtherMemberName;
                    if ((i + 1) != execute2.Count)
                    {
                        DaiZhiXingDeRenWu.exePerson += exePersonStr + "、";
                    }
                    else
                    {
                        DaiZhiXingDeRenWu.exePerson += exePersonStr;
                    }
                }
            }
            //
            var focus = _JointOfficeContext.News_Focus.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            if (focus != null)
            {
                DaiZhiXingDeRenWu.isFocus = "1";
            }
            else
            {
                DaiZhiXingDeRenWu.isFocus = "0";
            }
            var collection = _JointOfficeContext.News_Collection.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            if (collection != null)
            {
                DaiZhiXingDeRenWu.isCollection = "1";
            }
            else
            {
                DaiZhiXingDeRenWu.isCollection = "0";
            }
            return DaiZhiXingDeRenWu;
        }
        /// <summary>
        /// 日程详情页  one
        /// </summary>
        /// <returns></returns>
        public RiChengDetail GetRiChengDetailOne(Work_Program item, string memberid)
        {
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
            var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == item.Id).FirstOrDefault();
            var Agree = _JointOfficeContext.Agree.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            RiChengDetail RiChengDetail = new RiChengDetail();
            RiChengDetail.dyTypeName = "日程";
            RiChengDetail.richengid = item.Id;
            RiChengDetail.body = item.Body;
            RiChengDetail.richengMemberid = item.MemberId;
            RiChengDetail.picture = memberInfo.Picture + SasKey;
            RiChengDetail.name = memberInfo.Name;
            RiChengDetail.createDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm");
            //开始时间
            RiChengDetail.beginTime = item.Year + " " + item.Hour;
            //状态
            switch (item.State)
            {
                case 0:
                    if (DateTime.Now < Convert.ToDateTime(RiChengDetail.beginTime))
                    {
                        RiChengDetail.state = "未开始";
                    }
                    else if (DateTime.Now >= Convert.ToDateTime(RiChengDetail.beginTime))
                    {
                        RiChengDetail.state = "已开始";
                        item.State = 1;
                        _JointOfficeContext.SaveChanges();
                    }
                    break;
                case 1:
                    RiChengDetail.state = "已开始";
                    break;
                case 2:
                    RiChengDetail.state = "已取消";
                    break;
            }
            //抄送范围
            if (item.JoinPerson != null && item.JoinPerson != "" && item.JoinPerson != "[]")
            {
                RiChengDetail.range = GetRange(item.JoinPerson);
            }
            else
            {
                RiChengDetail.range = "公开";
            }
            //参与人
            var joinPersonList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(item.JoinPerson);
            if (joinPersonList != null && joinPersonList.Count != 0)
            {
                if (joinPersonList.Count > 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var joinPersonInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == joinPersonList[i].id).FirstOrDefault();
                        var joinPersonStr = joinPersonInfo.Name;
                        if ((i + 1) != 2)
                        {
                            RiChengDetail.joinPerson += joinPersonStr + "、";
                        }
                        else
                        {
                            RiChengDetail.joinPerson += joinPersonStr + "等" + joinPersonList.Count() + "人";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < joinPersonList.Count; i++)
                    {
                        var joinPersonInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == joinPersonList[i].id).FirstOrDefault();
                        var joinPersonStr = joinPersonInfo.Name;
                        if ((i + 1) != joinPersonList.Count)
                        {
                            RiChengDetail.joinPerson += joinPersonStr + "、";
                        }
                        else
                        {
                            RiChengDetail.joinPerson += joinPersonStr;
                        }
                    }
                }
            }
            else
            {
                RiChengDetail.joinPerson = "(参与人已取消日程)";
            }
            //回执
            var receipts = _JointOfficeContext.Receipts.Where(t => t.UId == item.Id).ToList();
            if (receipts != null && receipts.Count != 0)
            {
                var receiptsNum = 0;
                foreach (var receiptsOne in receipts)
                {
                    if (receiptsOne.Body != null && receiptsOne.Body != "")
                    {
                        receiptsNum++;
                    }
                }
                RiChengDetail.remarks = "需要" + receipts.Count() + "人回执，已有" + receiptsNum + "人回执。";
            }
            else
            {
                RiChengDetail.remarks = "需要0人回执，已有0人回执";
            }
            var receipts1 = receipts.Where(t => t.MemberId == memberid).FirstOrDefault();
            if (receipts1 != null)
            {
                if (receipts1.Body != null && receipts1.Body != "")
                {
                    RiChengDetail.isReceiptOk = "1";
                }
                else
                {
                    RiChengDetail.isReceiptOk = "0";
                }
            }
            else
            {
                RiChengDetail.isReceiptOk = "1";
            }
            //执行列表
            var comment = _JointOfficeContext.Comment_Body.Where(t => t.UId == item.Id && t.IsExeComment == 1).OrderByDescending(t => t.PingLunTime).ToList();
            List<ExeCommentList> list1 = new List<ExeCommentList>();
            foreach (var itemcom in comment)
            {
                ExeCommentList ExeCommentList = new ExeCommentList();
                ExeCommentList.name = itemcom.Name;
                ExeCommentList.exeDate = itemcom.PingLunTime.ToString("yyyy-MM-dd HH:mm");
                ExeCommentList.exeBody = itemcom.Body;
                ExeCommentList.state = "取消日程";
                ExeCommentList.stateNum = "3";
                ExeCommentList.phoneModel = itemcom.PhoneModel;
                list1.Add(ExeCommentList);
            }
            RiChengDetail.exeCommentList = list1;
            //提醒时间
            var remindTimelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RemindTime>>(item.RemindTime);
            var remindTimeStr = "";
            for (int i = 0; i < remindTimelist.Count; i++)
            {
                remindTimeStr = remindTimeStr + remindTimelist[i].timeWord;
                if ((i + 1) != remindTimelist.Count)
                {
                    remindTimeStr = remindTimeStr + ",";
                }
            }
            RiChengDetail.remindTime = remindTimeStr;
            //
            if (totalNum != null)
            {
                if (totalNum.PingLunNum != 0)
                {
                    RiChengDetail.pingLunNum = totalNum.PingLunNum;
                }
                else
                {
                    RiChengDetail.pingLunNum = 0;
                }
                if (totalNum.DianZanNum != 0)
                {
                    RiChengDetail.dianZanNum = totalNum.DianZanNum;
                }
                else
                {
                    RiChengDetail.dianZanNum = 0;
                }
            }
            else
            {
                RiChengDetail.pingLunNum = 0;
                RiChengDetail.dianZanNum = 0;
            }
            if (Agree == null)
            {
                RiChengDetail.isZan = 0;
            }
            else
            {
                RiChengDetail.isZan = 1;
            }
            RiChengDetail.phoneModel = item.PhoneModel;
            RiChengDetail.map = item.Map;
            RiChengDetail.address = item.Address;
            if (item.Picture != null && item.Picture != "")
            {
                var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item.Picture);
                foreach (var itemPicture in listPicture)
                {
                    itemPicture.url = itemPicture.url + SasKey;
                }
                RiChengDetail.appendPicture = listPicture;
            }
            if (item.Annex != null && item.Annex != "")
            {
                var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Work_File>>(item.Annex);
                long length = 0;
                foreach (var itemAnnex in listAnnex)
                {
                    itemAnnex.url = itemAnnex.url + SasKey;
                    itemAnnex.lengthStr = BusinessHelper.ConvertBytes(itemAnnex.length);
                    length += itemAnnex.length;
                }
                RiChengDetail.annex = listAnnex;
                RiChengDetail.annexLength = BusinessHelper.ConvertBytes(length);
            }
            if (item.Voice != null && item.Voice != "")
            {
                RiChengDetail.voice = item.Voice + SasKey;
            }
            if (item.VoiceLength != null && item.VoiceLength != "" && item.VoiceLength.Substring(0, 1) == "0")
            {
                RiChengDetail.voiceLength = item.VoiceLength.Substring(1, 1);
            }
            else
            {
                RiChengDetail.voiceLength = item.VoiceLength;
            }
            var focus = _JointOfficeContext.News_Focus.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            if (focus != null)
            {
                RiChengDetail.isFocus = "1";
            }
            else
            {
                RiChengDetail.isFocus = "0";
            }
            var collection = _JointOfficeContext.News_Collection.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            if (collection != null)
            {
                RiChengDetail.isCollection = "1";
            }
            else
            {
                RiChengDetail.isCollection = "0";
            }
            //是否需要当前用户回执
            if (item.Receipt != null)
            {
                if (item.Receipt.Contains(memberid))
                {
                    RiChengDetail.isReceipt = "1";
                }
                else
                {
                    RiChengDetail.isReceipt = "0";
                }
            }
            else
            {
                RiChengDetail.isReceipt = "0";
            }
            return RiChengDetail;
        }
        /// <summary>
        /// 指令详情页  one
        /// </summary>
        /// <returns></returns>
        public ZhiLingDetail GetZhiLingDetailOne(Work_Order item, string memberid)
        {
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
            var execute = _JointOfficeContext.Execute_Content.Where(t => t.UId == item.Id).FirstOrDefault();
            var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == item.Id).FirstOrDefault();
            var Agree = _JointOfficeContext.Agree.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            ZhiLingDetail ZhiLingDetail = new ZhiLingDetail();
            ZhiLingDetail.dyTypeName = "指令";
            ZhiLingDetail.zhilingid = item.Id;
            ZhiLingDetail.body = item.Body;
            ZhiLingDetail.zhilingMemberid = item.MemberId;
            ZhiLingDetail.picture = memberInfo.Picture + SasKey;
            ZhiLingDetail.name = memberInfo.Name;
            ZhiLingDetail.createDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm");
            ZhiLingDetail.stopTime = item.StopTime;
            if (DateTime.Now > Convert.ToDateTime(item.StopTime))
            {
                ZhiLingDetail.overTimeMark = "1";
            }
            else
            {
                ZhiLingDetail.overTimeMark = "0";
            }
            switch (item.State)
            {
                case 0:
                    ZhiLingDetail.state = "执行中";
                    ZhiLingDetail.stateNum = "0";
                    ZhiLingDetail.exeBody = "该指令由" + execute.OtherMemberName + "执行,应于" + string.Format("{0:f}", item.StopTime) + "前完成。";
                    break;
                case 1:
                    ZhiLingDetail.state = "已执行";
                    ZhiLingDetail.stateNum = "1";
                    ZhiLingDetail.exeBody = "该指令由" + execute.OtherMemberName + "执行,已于" + string.Format("{0:f}", execute.ExecuteDate) + "完成。";
                    break;
                case 2:
                    ZhiLingDetail.state = "已取消";
                    ZhiLingDetail.stateNum = "2";
                    ZhiLingDetail.exeBody = "该指令由" + execute.OtherMemberName + "执行,已于" + string.Format("{0:f}", execute.ExecuteDate) + "取消。";
                    break;
            }
            var dianping = _JointOfficeContext.DianPing_Body.Where(t => t.UId == item.Id).FirstOrDefault();
            if (dianping != null)
            {
                ZhiLingDetail.grade = dianping.Grade;
                ZhiLingDetail.gradeNum = Convert.ToInt32(dianping.Grade);
            }
            else
            {
                ZhiLingDetail.grade = "0";
                ZhiLingDetail.gradeNum = 0;
            }
            if (item.Range != null && item.Range != "" && item.Range != "[]")
            {
                if (string.IsNullOrEmpty(item.RangeNew))
                {
                    ZhiLingDetail.range = GetRange(item.Range);
                }
                else
                {
                    ZhiLingDetail.range = GetRange(item.RangeNew);
                }
            }
            else
            {
                ZhiLingDetail.range = "公开";
            }
            if (totalNum != null)
            {
                if (totalNum.PingLunNum != 0)
                {
                    ZhiLingDetail.pingLunNum = totalNum.PingLunNum;
                }
                else
                {
                    ZhiLingDetail.pingLunNum = 0;
                }
                if (totalNum.DianZanNum != 0)
                {
                    ZhiLingDetail.dianZanNum = totalNum.DianZanNum;
                }
                else
                {
                    ZhiLingDetail.dianZanNum = 0;
                }
            }
            else
            {
                ZhiLingDetail.pingLunNum = 0;
                ZhiLingDetail.dianZanNum = 0;
            }
            if (Agree == null)
            {
                ZhiLingDetail.isZan = 0;
            }
            else
            {
                ZhiLingDetail.isZan = 1;
            }
            ZhiLingDetail.phoneModel = item.PhoneModel;
            ZhiLingDetail.map = item.Map;
            ZhiLingDetail.address = item.Address;
            if (item.Picture != null && item.Picture != "")
            {
                var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item.Picture);
                foreach (var itemPicture in listPicture)
                {
                    itemPicture.url = itemPicture.url + SasKey;
                }
                ZhiLingDetail.appendPicture = listPicture;
            }
            if (item.Annex != null && item.Annex != "")
            {
                var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Work_File>>(item.Annex);
                long length = 0;
                foreach (var itemAnnex in listAnnex)
                {
                    itemAnnex.url = itemAnnex.url + SasKey;
                    itemAnnex.lengthStr = BusinessHelper.ConvertBytes(itemAnnex.length);
                    length += itemAnnex.length;
                }
                ZhiLingDetail.annex = listAnnex;
                ZhiLingDetail.annexLength = BusinessHelper.ConvertBytes(length);
            }
            if (item.Voice != null && item.Voice != "")
            {
                ZhiLingDetail.voice = item.Voice + SasKey;
            }
            if (item.VoiceLength != null && item.VoiceLength != "" && item.VoiceLength.Substring(0, 1) == "0")
            {
                ZhiLingDetail.voiceLength = item.VoiceLength.Substring(1, 1);
            }
            else
            {
                ZhiLingDetail.voiceLength = item.VoiceLength;
            }
            //执行列表
            var dianpingList = _JointOfficeContext.DianPing_Body.Where(t => t.UId == item.Id).FirstOrDefault();
            var comment = _JointOfficeContext.Comment_Body.Where(t => t.UId == item.Id && t.IsExeComment == 1).OrderByDescending(t => t.PingLunTime).ToList();
            List<ExeCommentList> list1 = new List<ExeCommentList>();
            if (dianpingList != null)
            {
                ExeCommentList ExeCommentList = new ExeCommentList();
                ExeCommentList.name = dianpingList.Name;
                ExeCommentList.exeDate = dianpingList.DianPingTime.ToString("yyyy-MM-dd HH:mm");
                ExeCommentList.exeBody = dianpingList.Body;
                ExeCommentList.state = "点评指令";
                ExeCommentList.stateNum = "1";
                ExeCommentList.phoneModel = dianpingList.PhoneModel;
                list1.Add(ExeCommentList);
            }
            foreach (var itemcom in comment)
            {
                ExeCommentList ExeCommentList = new ExeCommentList();
                switch (itemcom.Body.Substring(0, 2))
                {
                    case "完成":
                        ExeCommentList.state = "完成指令";
                        ExeCommentList.stateNum = "2";
                        break;
                    case "取消":
                        ExeCommentList.state = "取消指令";
                        ExeCommentList.stateNum = "3";
                        break;
                    case "继续":
                        ExeCommentList.state = "继续执行";
                        ExeCommentList.stateNum = "4";
                        break;
                }
                ExeCommentList.name = itemcom.Name;
                ExeCommentList.exeDate = itemcom.PingLunTime.ToString("yyyy-MM-dd HH:mm");
                ExeCommentList.exeBody = itemcom.Body;
                ExeCommentList.phoneModel = itemcom.PhoneModel;
                list1.Add(ExeCommentList);
            }
            ZhiLingDetail.exeCommentList = list1;
            if (execute.OtherMemberId == memberid)
            {
                ZhiLingDetail.isExecutor = "1";
            }
            else
            {
                ZhiLingDetail.isExecutor = "0";
            }
            var focus = _JointOfficeContext.News_Focus.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            if (focus != null)
            {
                ZhiLingDetail.isFocus = "1";
            }
            else
            {
                ZhiLingDetail.isFocus = "0";
            }
            var collection = _JointOfficeContext.News_Collection.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            if (collection != null)
            {
                ZhiLingDetail.isCollection = "1";
            }
            else
            {
                ZhiLingDetail.isCollection = "0";
            }
            return ZhiLingDetail;
        }
        /// <summary>
        /// 公告详情页  one
        /// </summary>
        /// <returns></returns>
        public GongGaoDetail GetGongGaoDetailOne(Work_Announcement item, string memberid)
        {
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
            var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == item.Id).FirstOrDefault();
            var Agree = _JointOfficeContext.Agree.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            GongGaoDetail GongGaoDetail = new GongGaoDetail();
            GongGaoDetail.dyTypeName = "公告";
            GongGaoDetail.gonggaoid = item.Id;
            GongGaoDetail.title = item.Title;
            GongGaoDetail.body = item.Body;
            GongGaoDetail.gonggaoMemberid = item.MemberId;
            GongGaoDetail.picture = memberInfo.Picture + SasKey;
            GongGaoDetail.name = memberInfo.Name;
            GongGaoDetail.createDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm");
            GongGaoDetail.beginTime = item.BeginTime.ToString();
            GongGaoDetail.stopTime = item.StopTime.ToString();
            var dianping = _JointOfficeContext.DianPing_Body.Where(t => t.UId == item.Id).FirstOrDefault();
            if (dianping != null)
            {
                GongGaoDetail.grade = dianping.Grade;
                GongGaoDetail.gradeNum = Convert.ToInt32(dianping.Grade);
            }
            else
            {
                GongGaoDetail.grade = "0";
                GongGaoDetail.gradeNum = 0;
            }
            //抄送范围
            if (item.Range != null && item.Range != "" && item.Range != "[]")
            {
                if (string.IsNullOrEmpty(item.RangeNew))
                {
                    GongGaoDetail.range = GetRange(item.Range);
                }
                else
                {
                    GongGaoDetail.range = GetRange(item.RangeNew);
                }
            }
            else
            {
                GongGaoDetail.range = "公开";
            }
            //回执
            var receipts = _JointOfficeContext.Receipts.Where(t => t.UId == item.Id).ToList();
            if (receipts != null && receipts.Count != 0)
            {
                var receiptsNum = 0;
                foreach (var receiptsOne in receipts)
                {
                    if (receiptsOne.Body != null && receiptsOne.Body != "")
                    {
                        receiptsNum++;
                    }
                }
                GongGaoDetail.remarks = "需要" + receipts.Count() + "人回执，已有" + receiptsNum + "人回执。";
            }
            else
            {
                GongGaoDetail.remarks = "需要0人回执，已有0人回执";
            }
            var receipts1 = receipts.Where(t => t.MemberId == memberid).FirstOrDefault();
            if (receipts1 != null)
            {
                if (receipts1.Body != null && receipts1.Body != "")
                {
                    GongGaoDetail.isReceiptOk = "1";
                }
                else
                {
                    GongGaoDetail.isReceiptOk = "0";
                }
            }
            else
            {
                GongGaoDetail.isReceiptOk = "1";
            }
            if (totalNum != null)
            {
                if (totalNum.PingLunNum != 0)
                {
                    GongGaoDetail.pingLunNum = totalNum.PingLunNum;
                }
                else
                {
                    GongGaoDetail.pingLunNum = 0;
                }
                if (totalNum.DianZanNum != 0)
                {
                    GongGaoDetail.dianZanNum = totalNum.DianZanNum;
                }
                else
                {
                    GongGaoDetail.dianZanNum = 0;
                }
            }
            else
            {
                GongGaoDetail.pingLunNum = 0;
                GongGaoDetail.dianZanNum = 0;
            }
            if (Agree == null)
            {
                GongGaoDetail.isZan = 0;
            }
            else
            {
                GongGaoDetail.isZan = 1;
            }
            GongGaoDetail.phoneModel = item.PhoneModel;
            GongGaoDetail.map = item.Map;
            GongGaoDetail.address = item.Address;
            if (item.Picture != null && item.Picture != "")
            {
                var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item.Picture);
                foreach (var itemPicture in listPicture)
                {
                    itemPicture.url = itemPicture.url + SasKey;
                }
                GongGaoDetail.appendPicture = listPicture;
            }
            if (item.Annex != null && item.Annex != "")
            {
                var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Work_File>>(item.Annex);
                long length = 0;
                foreach (var itemAnnex in listAnnex)
                {
                    itemAnnex.url = itemAnnex.url + SasKey;
                    itemAnnex.lengthStr = BusinessHelper.ConvertBytes(itemAnnex.length);
                    length += itemAnnex.length;
                }
                GongGaoDetail.annex = listAnnex;
                GongGaoDetail.annexLength = BusinessHelper.ConvertBytes(length);
            }
            if (item.Voice != null && item.Voice != "")
            {
                GongGaoDetail.voice = item.Voice + SasKey;
            }
            if (item.VoiceLength != null && item.VoiceLength != "" && item.VoiceLength.Substring(0, 1) == "0")
            {
                GongGaoDetail.voiceLength = item.VoiceLength.Substring(1, 1);
            }
            else
            {
                GongGaoDetail.voiceLength = item.VoiceLength;
            }
            var focus = _JointOfficeContext.News_Focus.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            if (focus != null)
            {
                GongGaoDetail.isFocus = "1";
            }
            else
            {
                GongGaoDetail.isFocus = "0";
            }
            var collection = _JointOfficeContext.News_Collection.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            if (collection != null)
            {
                GongGaoDetail.isCollection = "1";
            }
            else
            {
                GongGaoDetail.isCollection = "0";
            }
            //是否需要当前用户回执
            if (item.Receipt.Contains(memberid))
            {
                GongGaoDetail.isReceipt = "1";
            }
            else
            {
                GongGaoDetail.isReceipt = "0";
            }
            return GongGaoDetail;
        }
        /// <summary>
        /// 分享详情页  one
        /// </summary>
        /// <returns></returns>
        public FenXiangDetail GetFenXiangDetailOne(Work_Share item, string memberid)
        {
            var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.MemberId).FirstOrDefault();
            var totalNum = _JointOfficeContext.TotalNum.Where(t => t.UId == item.Id).FirstOrDefault();
            var Agree = _JointOfficeContext.Agree.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            FenXiangDetail FenXiangDetail = new FenXiangDetail();
            FenXiangDetail.dyTypeName = "分享";
            FenXiangDetail.fenxiangid = item.Id;
            FenXiangDetail.body = item.Body;
            FenXiangDetail.fenxiangMemberid = item.MemberId;
            FenXiangDetail.picture = memberInfo.Picture + SasKey;
            FenXiangDetail.name = memberInfo.Name;
            FenXiangDetail.createDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm");
            var dianping = _JointOfficeContext.DianPing_Body.Where(t => t.UId == item.Id).FirstOrDefault();
            if (dianping != null)
            {
                FenXiangDetail.grade = dianping.Grade;
                FenXiangDetail.gradeNum = Convert.ToInt32(dianping.Grade);
            }
            else
            {
                FenXiangDetail.grade = "0";
                FenXiangDetail.gradeNum = 0;
            }
            //抄送范围
            if (item.Range != null && item.Range != "" && item.Range != "[]")
            {
                if (string.IsNullOrEmpty(item.RangeNew))
                {
                    FenXiangDetail.range = GetRange(item.Range);
                }
                else
                {
                    FenXiangDetail.range = GetRange(item.RangeNew);
                }
            }
            else
            {
                FenXiangDetail.range = "公开";
            }
            //回执
            var receipts = _JointOfficeContext.Receipts.Where(t => t.UId == item.Id).ToList();
            if (receipts != null && receipts.Count != 0)
            {
                var receiptsNum = 0;
                foreach (var receiptsOne in receipts)
                {
                    if (receiptsOne.Body != null && receiptsOne.Body != "")
                    {
                        receiptsNum++;
                    }
                }
                FenXiangDetail.remarks = "需要" + receipts.Count() + "人回执，已有" + receiptsNum + "人回执。";
            }
            else
            {
                FenXiangDetail.remarks = "需要0人回执，已有0人回执";
            }
            var receipts1 = receipts.Where(t => t.MemberId == memberid).FirstOrDefault();
            if (receipts1 != null)
            {
                if (receipts1.Body != null && receipts1.Body != "")
                {
                    FenXiangDetail.isReceiptOk = "1";
                }
                else
                {
                    FenXiangDetail.isReceiptOk = "0";
                }
            }
            else
            {
                FenXiangDetail.isReceiptOk = "1";
            }
            if (totalNum != null)
            {
                if (totalNum.PingLunNum != 0)
                {
                    FenXiangDetail.pingLunNum = totalNum.PingLunNum;
                }
                else
                {
                    FenXiangDetail.pingLunNum = 0;
                }
                if (totalNum.DianZanNum != 0)
                {
                    FenXiangDetail.dianZanNum = totalNum.DianZanNum;
                }
                else
                {
                    FenXiangDetail.dianZanNum = 0;
                }
            }
            else
            {
                FenXiangDetail.pingLunNum = 0;
                FenXiangDetail.dianZanNum = 0;
            }
            if (Agree == null)
            {
                FenXiangDetail.isZan = 0;
            }
            else
            {
                FenXiangDetail.isZan = 1;
            }
            FenXiangDetail.phoneModel = item.PhoneModel;
            FenXiangDetail.map = item.Map;
            FenXiangDetail.address = item.Address;
            if (item.Picture != null && item.Picture != "")
            {
                var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(item.Picture);
                foreach (var itemPicture in listPicture)
                {
                    itemPicture.url = itemPicture.url + SasKey;
                }
                FenXiangDetail.appendPicture = listPicture;
            }
            if (item.Annex != null && item.Annex != "")
            {
                var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Work_File>>(item.Annex);
                long length = 0;
                foreach (var itemAnnex in listAnnex)
                {
                    itemAnnex.url = itemAnnex.url + SasKey;
                    itemAnnex.lengthStr = BusinessHelper.ConvertBytes(itemAnnex.length);
                    length += itemAnnex.length;
                }
                FenXiangDetail.annex = listAnnex;
                FenXiangDetail.annexLength = BusinessHelper.ConvertBytes(length);
            }
            if (item.Voice != null && item.Voice != "")
            {
                FenXiangDetail.voice = item.Voice + SasKey;
            }
            if (item.VoiceLength != null && item.VoiceLength != "" && item.VoiceLength.Substring(0, 1) == "0")
            {
                FenXiangDetail.voiceLength = item.VoiceLength.Substring(1, 1);
            }
            else
            {
                FenXiangDetail.voiceLength = item.VoiceLength;
            }
            var focus = _JointOfficeContext.News_Focus.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            if (focus != null)
            {
                FenXiangDetail.isFocus = "1";
            }
            else
            {
                FenXiangDetail.isFocus = "0";
            }
            var collection = _JointOfficeContext.News_Collection.Where(t => t.UId == item.Id && t.MemberId == memberid).FirstOrDefault();
            if (collection != null)
            {
                FenXiangDetail.isCollection = "1";
            }
            else
            {
                FenXiangDetail.isCollection = "0";
            }
            //是否需要当前用户回执
            if (item.Receipt.Contains(memberid))
            {
                FenXiangDetail.isReceipt = "1";
            }
            else
            {
                FenXiangDetail.isReceipt = "0";
            }
            return FenXiangDetail;
        }
        /// <summary>
        /// 工作回复  回复我的  @我的回复
        /// </summary>
        /// <returns></returns>
        public List<WorkReply> GetWorkReply(string item, string memberid)
        {
            List<WorkReply> list = new List<WorkReply>();
            WorkReply WorkReply = new WorkReply();
            var comment_Body = _JointOfficeContext.Comment_Body.Where(t => t.Id == item).FirstOrDefault();
            var dianping_Body = _JointOfficeContext.DianPing_Body.Where(t => t.Id == item).FirstOrDefault();
            var TotalNum = _JointOfficeContext.TotalNum.Where(t => t.PId == item).FirstOrDefault();
            var Agree = _JointOfficeContext.Agree.Where(t => t.PId == item && t.MemberId == memberid).FirstOrDefault();
            if (comment_Body != null)
            {
                var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == comment_Body.MemberId).FirstOrDefault();
                WorkReply.replyWorkID = comment_Body.UId;
                WorkReply.replyWorkNameID = comment_Body.MemberId;
                WorkReply.replyWorkName = memberInfo.Name;
                WorkReply.replyTypeNum = comment_Body.Type;
                switch (comment_Body.Type)
                {
                    case "1":
                        WorkReply.replyType = "审批";
                        var work_Approval = _JointOfficeContext.Work_Approval.Where(t => t.Id == comment_Body.UId).FirstOrDefault();
                        if (work_Approval != null)
                        {
                            WorkReply.replyWorkBody = work_Approval.Body;
                        }
                        break;
                    case "2":
                        WorkReply.replyType = "日志";
                        var work_Log = _JointOfficeContext.Work_Log.Where(t => t.Id == comment_Body.UId).FirstOrDefault();
                        if (work_Log != null)
                        {
                            WorkReply.replyWorkBody = work_Log.WorkSummary;
                        }
                        break;
                    case "3":
                        WorkReply.replyType = "任务";
                        var work_Task = _JointOfficeContext.Work_Task.Where(t => t.Id == comment_Body.UId).FirstOrDefault();
                        if (work_Task != null)
                        {
                            WorkReply.replyWorkBody = work_Task.TaskTitle;
                        }
                        break;
                    case "4":
                        WorkReply.replyType = "日程";
                        var work_Program = _JointOfficeContext.Work_Program.Where(t => t.Id == comment_Body.UId).FirstOrDefault();
                        if (work_Program != null)
                        {
                            WorkReply.replyWorkBody = work_Program.Body;
                        }
                        break;
                    case "5":
                        WorkReply.replyType = "指令";
                        var work_Order = _JointOfficeContext.Work_Order.Where(t => t.Id == comment_Body.UId).FirstOrDefault();
                        if (work_Order != null)
                        {
                            WorkReply.replyWorkBody = work_Order.Body;
                        }
                        break;
                    case "6":
                        WorkReply.replyType = "群通知";
                        var news_GroupNotice = _JointOfficeContext.News_GroupNotice.Where(t => t.Id == comment_Body.UId).FirstOrDefault();
                        if (news_GroupNotice != null)
                        {
                            WorkReply.replyWorkBody = news_GroupNotice.Body;
                        }
                        break;
                    case "8":
                        WorkReply.replyType = "公告";
                        var work_Announcement = _JointOfficeContext.Work_Announcement.Where(t => t.Id == comment_Body.UId).FirstOrDefault();
                        if (work_Announcement != null)
                        {
                            WorkReply.replyWorkBody = work_Announcement.Body;
                        }
                        break;
                    case "9":
                        WorkReply.replyType = "分享";
                        var work_Share = _JointOfficeContext.Work_Share.Where(t => t.Id == comment_Body.UId).FirstOrDefault();
                        if (work_Share != null)
                        {
                            WorkReply.replyWorkBody = work_Share.Body;
                        }
                        break;
                }
                if (comment_Body.Type.Contains("+"))
                {
                    WorkReply.replyWorkBody = comment_Body.OtherBody;
                    switch (comment_Body.Type.Substring(2, 1))
                    {
                        case "1":
                            WorkReply.replyType = "审批的回复";
                            break;
                        case "2":
                            WorkReply.replyType = "日志的回复";
                            break;
                        case "3":
                            WorkReply.replyType = "任务的回复";
                            break;
                        case "4":
                            WorkReply.replyType = "日程的回复";
                            break;
                        case "5":
                            WorkReply.replyType = "指令的回复";
                            break;
                        case "8":
                            WorkReply.replyType = "公告的回复";
                            break;
                        case "9":
                            WorkReply.replyType = "分享的回复";
                            break;
                    }
                }
                WorkReply.replyTime = comment_Body.PingLunTime.ToString("yyyy-MM-dd HH:mm");
                var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == comment_Body.PingLunMemberId).FirstOrDefault();
                WorkReply.memberId = comment_Body.PingLunMemberId;
                WorkReply.memberName = info.Name;
                WorkReply.memberPicture = info.Picture + SasKey;
                WorkReply.id = comment_Body.Id;
                WorkReply.body = comment_Body.Body;
                if (comment_Body.PictureList != null && comment_Body.PictureList != "")
                {
                    var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(comment_Body.PictureList);
                    foreach (var itemPicture in listPicture)
                    {
                        itemPicture.url = itemPicture.url + SasKey;
                    }
                    WorkReply.appendPicture = listPicture;
                }
                if (comment_Body.Annex != null && comment_Body.Annex != "")
                {
                    var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Work_File>>(comment_Body.Annex);
                    long length = 0;
                    foreach (var itemAnnex in listAnnex)
                    {
                        itemAnnex.url = itemAnnex.url + SasKey;
                        itemAnnex.lengthStr = BusinessHelper.ConvertBytes(itemAnnex.length);
                        length += itemAnnex.length;
                    }
                    WorkReply.annex = listAnnex;
                    WorkReply.annexLength = BusinessHelper.ConvertBytes(length);
                }
                if (comment_Body.Voice != null && comment_Body.Voice != "")
                {
                    WorkReply.voice = comment_Body.Voice + SasKey;
                }
                if (comment_Body.VoiceLength != null && comment_Body.VoiceLength != "" && comment_Body.VoiceLength.Substring(0, 1) == "0")
                {
                    WorkReply.voiceLength = comment_Body.VoiceLength.Substring(1, 1);
                }
                else
                {
                    WorkReply.voiceLength = comment_Body.VoiceLength;
                }
                if (TotalNum != null)
                {
                    if (TotalNum.PingLunNum != 0)
                    {
                        WorkReply.pingLunNum = TotalNum.PingLunNum;
                    }
                    else
                    {
                        WorkReply.pingLunNum = 0;
                    }
                    if (TotalNum.DianZanNum != 0)
                    {
                        WorkReply.dianZanNum = TotalNum.DianZanNum;
                    }
                    else
                    {
                        WorkReply.dianZanNum = 0;
                    }
                }
                else
                {
                    WorkReply.pingLunNum = 0;
                    WorkReply.dianZanNum = 0;
                }
                if (Agree == null)
                {
                    WorkReply.isZan = 0;
                }
                else
                {
                    WorkReply.isZan = 1;
                }
                WorkReply.phoneModel = comment_Body.PhoneModel;
            }
            if (dianping_Body != null)
            {
                var memberInfo = _JointOfficeContext.Member_Info.Where(t => t.MemberId == dianping_Body.MemberId).FirstOrDefault();
                WorkReply.replyWorkID = dianping_Body.UId;
                WorkReply.replyWorkNameID = dianping_Body.MemberId;
                WorkReply.replyWorkName = memberInfo.Name;
                WorkReply.replyTypeNum = dianping_Body.Type;
                switch (dianping_Body.Type)
                {
                    case "2":
                        WorkReply.replyType = "日志";
                        var work_Log = _JointOfficeContext.Work_Log.Where(t => t.Id == dianping_Body.UId).FirstOrDefault();
                        if (work_Log != null)
                        {
                            WorkReply.replyWorkBody = work_Log.WorkSummary;
                        }
                        break;
                    case "3":
                        WorkReply.replyType = "任务";
                        var work_Task = _JointOfficeContext.Work_Task.Where(t => t.Id == dianping_Body.UId).FirstOrDefault();
                        if (work_Task != null)
                        {
                            WorkReply.replyWorkBody = work_Task.TaskTitle;
                        }
                        break;
                    case "5":
                        WorkReply.replyType = "指令";
                        var work_Order = _JointOfficeContext.Work_Order.Where(t => t.Id == dianping_Body.UId).FirstOrDefault();
                        if (work_Order != null)
                        {
                            WorkReply.replyWorkBody = work_Order.Body;
                        }
                        break;
                }
                WorkReply.replyTime = dianping_Body.DianPingTime.ToString("yyyy-MM-dd HH:mm");
                var info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == dianping_Body.DianPingMemberId).FirstOrDefault();
                WorkReply.memberId = dianping_Body.DianPingMemberId;
                WorkReply.memberName = info.Name;
                WorkReply.memberPicture = info.Picture + SasKey;
                WorkReply.id = dianping_Body.Id;
                WorkReply.body = dianping_Body.Body;
                if (dianping_Body.PictureList != null && dianping_Body.PictureList != "")
                {
                    var listPicture = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonDynamic_info_url>>(dianping_Body.PictureList);
                    foreach (var itemPicture in listPicture)
                    {
                        itemPicture.url = itemPicture.url + SasKey;
                    }
                    WorkReply.appendPicture = listPicture;
                }
                if (dianping_Body.Annex != null && dianping_Body.Annex != "")
                {
                    var listAnnex = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Work_File>>(dianping_Body.Annex);
                    long length = 0;
                    foreach (var itemAnnex in listAnnex)
                    {
                        itemAnnex.url = itemAnnex.url + SasKey;
                        itemAnnex.lengthStr = BusinessHelper.ConvertBytes(itemAnnex.length);
                        length += itemAnnex.length;
                    }
                    WorkReply.annex = listAnnex;
                    WorkReply.annexLength = BusinessHelper.ConvertBytes(length);
                }
                if (dianping_Body.Voice != null && dianping_Body.Voice != "")
                {
                    WorkReply.voice = dianping_Body.Voice + SasKey;
                }
                if (dianping_Body.VoiceLength != null && dianping_Body.VoiceLength != "" && dianping_Body.VoiceLength.Substring(0, 1) == "0")
                {
                    WorkReply.voiceLength = dianping_Body.VoiceLength.Substring(1, 1);
                }
                else
                {
                    WorkReply.voiceLength = dianping_Body.VoiceLength;
                }
                if (TotalNum != null)
                {
                    if (TotalNum.PingLunNum != 0)
                    {
                        WorkReply.pingLunNum = TotalNum.PingLunNum;
                    }
                    else
                    {
                        WorkReply.pingLunNum = 0;
                    }
                    if (TotalNum.DianZanNum != 0)
                    {
                        WorkReply.dianZanNum = TotalNum.DianZanNum;
                    }
                    else
                    {
                        WorkReply.dianZanNum = 0;
                    }
                }
                else
                {
                    WorkReply.pingLunNum = 0;
                    WorkReply.dianZanNum = 0;
                }
                if (Agree == null)
                {
                    WorkReply.isZan = 0;
                }
                else
                {
                    WorkReply.isZan = 1;
                }
                WorkReply.phoneModel = dianping_Body.PhoneModel;
            }
            list.Add(WorkReply);
            return list;
        }
        /// <summary>
        /// 处理日期入参 规范年月日格式
        /// </summary>
        /// <returns></returns>
        public string GetYMD(string time)
        {
            if (time.Length == 8)
            {
                var timeY = time.Substring(0, 5);
                var timeM = "0" + time.Substring(5, 1) + "-";
                var timeD = "0" + time.Substring(7, 1);
                time = timeY + timeM + timeD;
            }
            else if (time.Length == 9)
            {
                if (time.Substring(6, 1) == "-")
                {
                    var timeY = time.Substring(0, 5);
                    var timeM = "0" + time.Substring(5, 1) + "-";
                    var timeD = time.Substring(7, 2);
                    time = timeY + timeM + timeD;
                }
                else if (time.Substring(7, 1) == "-")
                {
                    var timeY = time.Substring(0, 5);
                    var timeM = time.Substring(5, 2) + "-";
                    var timeD = "0" + time.Substring(8, 1);
                    time = timeY + timeM + timeD;
                }
            }
            return time;
        }
        /// <summary>
        /// 获取抄送范围/参与人详情
        /// </summary>
        /// <returns></returns>
        public List<RangeInfo> GetRangeInfo(string memberid, string range)
        {
            List<RangeInfo> list = new List<RangeInfo>();
            var rangeStr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PeoPleInfo>>(range);
            if (rangeStr != null && rangeStr.Count != 0)
            {
                //List<string> memberInfo = new List<string>();
                //foreach (var item in rangeStr)
                //{
                //    if (item.type == "2")
                //    {
                //        WorkDetails WorkDetails = new WorkDetails(config, _JointOfficeContext, _PrincipalBase);
                //        memberInfo = WorkDetails.GetCompanyPersonList(memberInfo, item.id);
                //    }
                //    if (item.type == "1")
                //    {
                //        var member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.id).Select(t => t.MemberId).ToList();
                //        memberInfo.AddRange(member_Info);
                //    }
                //}
                //var memberInfoset = new HashSet<string>(memberInfo);

                //foreach (var item in memberInfoset)
                //{
                //    var member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item).FirstOrDefault();
                //    if (member_Info != null)
                //    {
                //        RangeInfo RangeInfo = new RangeInfo();
                //        RangeInfo.memberid = member_Info.MemberId;
                //        RangeInfo.name = member_Info.Name;
                //        RangeInfo.picture = member_Info.Picture + SasKey;
                //        RangeInfo.jobName = "";
                //        var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == member_Info.JobID).FirstOrDefault();
                //        if (memJob != null)
                //        {
                //            RangeInfo.jobName = memJob.Name;
                //        }
                //        list.Add(RangeInfo);
                //    }
                //}

                foreach (var item in rangeStr)
                {
                    RangeInfo RangeInfo = new RangeInfo();
                    if (item.type == "2")
                    {
                        var company = _JointOfficeContext.Member_Company.Where(t => t.Id == item.id).FirstOrDefault();
                        if (company != null)
                        {
                            RangeInfo.memberid = item.id;
                            RangeInfo.name = company.Name;
                            RangeInfo.picture = "";
                            RangeInfo.jobName = "";
                            RangeInfo.type = "2";
                            list.Add(RangeInfo);
                        }
                    }
                    if (item.type == "1")
                    {
                        var member_Info = _JointOfficeContext.Member_Info.Where(t => t.MemberId == item.id).FirstOrDefault();
                        if (member_Info != null)
                        {
                            RangeInfo.memberid = member_Info.MemberId;
                            RangeInfo.name = member_Info.Name;
                            RangeInfo.picture = member_Info.Picture + SasKey;
                            RangeInfo.jobName = "";
                            var memJob = _JointOfficeContext.Member_Job.Where(t => t.Id == member_Info.JobID).FirstOrDefault();
                            if (memJob != null)
                            {
                                RangeInfo.jobName = memJob.Name;
                            }
                            RangeInfo.type = "1";
                            list.Add(RangeInfo);
                        }
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取某部门下所有人员，包括子部门  （递归）
        /// </summary>
        public List<string> GetCompanyPersonList(List<string> strlist, string comid)
        {
            var member_Info = _JointOfficeContext.Member_Info.Where(t => t.ZhuBuMen == comid || t.FuBuMen.Contains(comid)).Select(t => t.MemberId).ToList();
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
        /// 抄送范围选择部门时递归
        /// </summary>
        /// <returns></returns>
        public List<PeoPleInfo> CreateWorkRange(List<PeoPleInfo> list1, string id)
        {
            var person1 = _JointOfficeContext.Member_Info_Company.Where(t => t.DeptId == id).ToList();
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
            foreach (var item in person1)
            {
                if (!str.Contains(item.MemberId))
                {
                    PeoPleInfo PeoPleInfo = new PeoPleInfo();
                    PeoPleInfo.type = "1";
                    PeoPleInfo.id = item.MemberId;
                    list1.Add(PeoPleInfo);
                }
            }

            var list = _JointOfficeContext.Member_Company.Where(t => t.ParentId == id).ToList();
            foreach (var item in list)
            {
                list1 = CreateWorkRange(list1, item.Id);
            }
            return list1;
        }
    }
}
