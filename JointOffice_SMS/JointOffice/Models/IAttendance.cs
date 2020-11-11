using JointOffice.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Models
{
    public interface IAttendance
    {
        /// <summary>
        /// 考勤详情
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<CheckRecord> GetCheckRecordList(CheckRecordInPara para);
        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge CheckIn(CheckInPara para);
        /// <summary>
        /// 备注  写入
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Meaasge RemarksIn(RemarksInPara para);
        /// <summary>
        /// 考勤统计
        /// </summary>
        Showapi_Res_Single<CheckCountPara> GetCheckCountList(CheckCountInPara para);
        /// <summary>
        /// 显示考勤统计人员和次数页面  团队
        /// </summary>
        Showapi_Res_List<CheckCountTeamListPara> GetCheckCountTeamList(CheckCountTeamListParaInPara para);
        /// <summary>
        /// 显示某个人员具体签到类型的明细列表
        /// </summary>
        Showapi_Res_List<CheckCountTypeList> GetCheckCountTypeList(CheckCountTypeListInPara para);
        /// <summary>
        /// 某个人员具体签到类型的明细列表进考勤详情
        /// </summary>
        /// <returns></returns>
        Showapi_Res_Single<CheckRecord> GetCheckCountTypeRecordList(CheckCountTypeRecordInPara para);
    }
    /// <summary>
    /// 考勤详情  入参
    /// </summary>
    public class CheckRecordInPara
    {
        /// <summary>
        /// 年月日  yyyy-MM-dd
        /// </summary>
        public string time { get; set; }
    }
    /// <summary>
    /// 考勤详情
    /// </summary>
    public class CheckRecord
    {
        /// <summary>
        /// 签到详情
        /// </summary>
        public List<CheckRecordInfo> checkRecordInfo { get; set; }
        /// <summary>
        /// 备注详情
        /// </summary>
        public List<RemarksInfo> remarksInfo { get; set; }
    }
    /// <summary>
    /// 签到详情
    /// </summary>
    public class CheckRecordInfo
    {
        /// <summary>
        /// 类型 1出发 2交通中转 3到达客户 4离开客户 5上班签到 6下班签到
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 签到时间
        /// </summary>
        public string toOffTime { get; set; }
        /// <summary>
        /// 我的坐标
        /// </summary>
        public string map { get; set; }
        /// <summary>
        /// 我的位置
        /// </summary>
        public string address { get; set; }
    }
    /// <summary>
    /// 备注详情
    /// </summary>
    public class RemarksInfo
    {
        /// <summary>
        /// 备注内容
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 备注时间
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 类型 "" 出发 交通中转 到达客户 离开客户 上班签到 下班签到
        /// </summary>
        public string type { get; set; }
    }
    /// <summary>
    /// 签到
    /// </summary>
    /// <returns></returns>
    public class CheckInPara
    {
        /// <summary>
        /// 我的坐标
        /// </summary>
        public string map { get; set; }
        /// <summary>
        /// 我的位置
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 签到类型 1出发 2交通中转 3到达客户 4离开客户 5上班签到 6下班签到
        /// </summary>
        public int checkType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }
    }
    /// <summary>
    /// 备注  入参
    /// </summary>
    public class RemarksInPara
    {
        /// <summary>
        /// 用户查看日期  yyyy-MM-dd
        /// </summary>
        public string checkDate { get; set; }
        /// <summary>
        /// 备注内容
        /// </summary>
        public string remarks { get; set; }
    }
    /// <summary>
    /// 考勤统计  入参
    /// </summary>
    public class CheckCountInPara
    {
        /// <summary>
        /// 1团队  2我的
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 人id  list
        /// </summary>
        public List<CheckCountTypeInPara> memberidList { get; set; }
    }
    /// <summary>
    /// 考勤统计  获取人员   List
    /// </summary>
    public class CheckCountTypeInPara
    {
        /// <summary>
        /// 1个人  2部门
        /// </summary>
        public int type { get; set;}
        public string id { get; set; }
    }
    public class SqlList
    {
        public int type { get; set; }
        public string memberid { get; set; }
    }
    /// <summary>
    /// 考勤统计
    /// </summary>
    public class CheckCountPara
    {
        /// <summary>
        /// 考勤人数
        /// </summary>
        public int personNum { get; set; }
        /// <summary>
        /// 考勤类型列表
        /// </summary>
        public List<CheckCountListPara> checkCountListPara { get; set; }
    }
    /// <summary>
    /// 考勤类型列表
    /// </summary>
    public class CheckCountListPara
    {
        /// <summary>
        /// 1出发 2交通中转 3到达客户 4离开客户 5上班签到 6下班签到
        /// </summary>
        public string mark { get; set; }
        /// <summary>
        /// 1-6
        /// </summary>
        public string markNum { get; set; }
        /// <summary>
        /// 类别对应数量
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 总人数
        /// </summary>
        public int zongNum { get; set; }
    }
    /// <summary>
    /// 显示考勤统计人员和次数页面  团队  入参
    /// </summary>
    public class CheckCountTeamListParaInPara
    {
        /// <summary>
        /// 1出发 2交通中转 3到达客户 4离开客户 5上班签到 6下班签到
        /// </summary>
        public int mark { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 人id  list
        /// </summary>
        public List<CheckCountTypeInPara> memberidList { get; set; }
    }
    /// <summary>
    /// 显示考勤统计人员和次数页面  团队
    /// </summary>
    public class CheckCountTeamListPara
    {
        public string memberid { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string jobName { get; set; }
        /// <summary>
        /// 次数
        /// </summary>
        public int num { get; set; }
    }
    /// <summary>
    /// 显示某个人员具体签到类型的明细列表  入参
    /// </summary>
    public class CheckCountTypeListInPara
    {
        /// <summary>
        /// 1团队 2个人
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 1出发 2交通中转 3到达客户 4离开客户 5上班签到 6下班签到
        /// </summary>
        public int mark { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 人id
        /// </summary>
        public string memberid { get; set; }
    }
    /// <summary>
    /// 显示某个人员具体签到类型的明细列表
    /// </summary>
    public class CheckCountTypeList
    {
        /// <summary>
        /// 人ID
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 签到日期
        /// </summary>
        public string checkDate { get; set; }
        /// <summary>
        /// 提示内容
        /// </summary>
        public string timeBody { get; set; }
        /// <summary>
        /// 签到位置
        /// </summary>
        public string address { get; set; }
    }
    /// <summary>
    /// 某个人员具体签到类型的明细列表进考勤详情  入参
    /// </summary>
    public class CheckCountTypeRecordInPara
    {
        /// <summary>
        /// 年月日  yyyy-MM-dd
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 人ID
        /// </summary>
        public string memberid { get; set; }
    }
}
