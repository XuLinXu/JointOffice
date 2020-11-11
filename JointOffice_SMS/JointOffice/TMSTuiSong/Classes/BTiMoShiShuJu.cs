using System;
using System.Data;
using System.Data.SqlClient;
using TMSTuiSongJointOffice.DbHelper;

namespace TMSTuiSongJointOffice.Classes
{

    public class BTiMoShiShuJu
    {
        // 日志日计划
        public static DataTable GetRiQingWeiXinTuiSongjob()
        {
            SqlDbHelper helper = new SqlDbHelper();
            try
            {
                string str = @"select wl.MemberId, mi.Name, mi.JobName, wl.MoBanTime, wl.WorkPlan, wl.WorkSummary, wl.Experience, wl.CreateDate, wl.Id, wl.TMSstate 
                                from Work_Log wl 
                                inner join Member_Info as mi on mi.MemberId = wl.MemberId 
                                where CONVERT(varchar(100), wl.CreateDate, 23) = CONVERT(varchar(100),getdate(), 23) 
                                and (wl.TMSstate = 0 or wl.TMSstate is null) and MoBan = 1";

                //string str = @"select wl.MemberId, mi.Name, mi.JobName, wl.MoBanTime, wl.WorkPlan, wl.WorkSummary, wl.Experience, wl.CreateDate, wl.Id, wl.TMSstate 
                //                from Work_Log wl 
                //                inner join Member_Info as mi on mi.MemberId = wl.MemberId 
                //                where CONVERT(varchar(100), left(wl.CreateDate,charindex(' ',wl.CreateDate,1)-1), 23) = CONVERT(varchar(100),getdate(), 23) 
                //                and MoBan = 1 and (wl.TMSstate = 0 or wl.TMSstate is null)";

                DataTable novel = helper.Manage_ResultTB_Caozuo(str);
                return novel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // 日志每天推一次
        public static DataTable GetRiQingWeiXinTuiSongID()
        {
            SqlDbHelper helper = new SqlDbHelper();
            try
            {
                string str = @"select value from Base_KeyValue where VID = '8682EFFC-DE02-4303-AE23-CAF1DF146E10'";
                DataTable novel = helper.Manage_ResultTB_Caozuo(str);
                return novel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // 日志日计划添加数据
        public static int InsertRiQing(string name, byte[] by, string bs)
        {
            SqlDbHelper helper = new SqlDbHelper("");
            try
            {
                string str = @"Insert into [dbo].[TMoShiTuPian] (ID,InsertTime,name,imageBase64) VALUES (newID(),getdate(),@name,@imageBase64)";
                SqlParameter[] para = {
                            new SqlParameter("@name",SqlDbType.NVarChar),
                            new SqlParameter("@imageBase64",SqlDbType.Text)
                            };

                para[0].Value = name;
                para[1].Value = bs;
                int novel = helper.Manage_caozuoByParam(str, para);
                return novel;
          
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        // 日志修改状态
        public static int UpdateState(string Id)
        {
            SqlDbHelper helper = new SqlDbHelper();
            try
            {
                string str = @"update Work_Log set TMSstate=1 where Id=@Id";
                SqlParameter[] para = {
                            new SqlParameter("@Id", SqlDbType.NVarChar)
                            };

                para[0].Value = Id;
                int novel = helper.Manage_caozuoByParam(str, para);
                return novel;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        //每天发送
        public static DataTable GetRiQingWeiXinTuiSongDayhuibao(string Id, string datetime)
        {
            SqlDbHelper helper = new SqlDbHelper();
            try
            {
//                string str = @"select ml.Name,ml.MemberId ,b.TMSstate from Member_Info ml left join 
//(select Member_Info.Name,Member_Info.MemberId, Work_Log.TMSstate from 
//(select wl.MemberId, min(wl.CreateDate) cd from Work_Log wl where CONVERT(varchar(100), wl.CreateDate, 23) = CONVERT(varchar(100),DATEADD( day,-1,getdate()), 23) group by wl.MemberId) a 
//inner join Work_Log on Work_Log.CreateDate = a.cd 
//inner join Member_Info on Member_Info.MemberId = a.MemberId) b 
//on b.MemberId = ml.MemberId where  ml.MemberId in " + Id.ToString();

                string str = @"select * from (
                        select 
                        a.Name,
                        CONVERT(varchar(100), left(b.MoBanTime,charindex(' ',b.MoBanTime,1)-1), 23) MoBanTime
                        from Member_Info a left join Work_Log b on a.MemberId = b.MemberId and b.MoBan = 1 
                        and CONVERT(varchar(100), left(b.MoBanTime,charindex(' ',b.MoBanTime,1)-1), 23) = '"+ datetime + @"'
                        where a.MemberId in " + Id.ToString() + @"
                        ) as tab group by Name,MoBanTime";
                //SqlParameter[] para = {
                //    new SqlParameter("@id", SqlDbType.VarChar)
                //};

                //para[0].Value = Id;

                DataTable novel = helper.Manage_ResultTB_Caozuo(str);
                return novel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
