using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Diagnostics;


namespace TMSTuiSongJointOffice.DbHelper
{
    /// <summary>
    /// RightLogDo 的摘要说明
    /// </summary>
    public class RightLogDo
    {
        string connectionstring = "";
        protected DBErrorLog log;

        public RightLogDo(string conString)
        {
            connectionstring = conString;
            log = new DBErrorLog(connectionstring);
            if (HttpContext.Current != null)
            {
                myContext = HttpContext.Current;
            }
        }
        //日志保存
        private HttpContext myContext = null;
     
        #region 系统日志相关操作
        public void RightLogDo_S(string sql)
        {
            ////
            //// TODO: 在此处添加构造函数逻辑
            ////
            //string PersonID = "";
            //string PersonName = "";
            //string Node = "";
            //string Operation = "";
            //string IP = "";
            //myContext = System.Web.HttpContext.Current;
            //if (myContext == null || myContext.Session["lc"] == null || myContext.Session["lc"].ToString() == "")
            //{
            //    //用户登陆时获取用户名
            //    //UserInfo = myContext.Request["username"].ToString().Trim();
            //}
            //else
            //{
            //    myinfo = (LoginCertificate)myContext.Session["lc"];
            //}
            //if (myinfo != null)
            //{
            //    PersonID = myinfo.UserId;
            //    PersonName = myinfo.LoginName;
            //    if (myinfo.CurrentNodeId != null && myinfo.CurrentNodeId != "")
            //    {
            //        Node = myinfo.CurrentNodeId;
            //    }
            //    else
            //    {
            //        Node = "空";
            //    } Operation = sql.ToString();
            //    IP = myContext.Request.UserHostAddress.ToString();
            //    this.RightLogDo_Sava(PersonID, PersonName, Node, Operation, IP);
            //}

        }
        /// <summary>
        /// ////对SQL语句进行拆解
        ///
        /// </summary>
        /// <param name="PersonID"></param>
        /// <param name="PersonName"></param>
        /// <param name="Node"></param>
        /// <param name="Operation"></param>
        /// Sql查询语句
        public string GetTrueSql(string EnterSqlString)
        {
            string ResultString = "";
            ResultString = EnterSqlString;
            return ResultString;
        }
        /// 是否存储过程/Sql语句带有一个或多个参数
        public string GetTrueSql(string EnterSqlString, SqlParameter[] myParamArray)
        {
            string ResultString = "";
            ResultString += EnterSqlString;
            ResultString += "(";
            for (int i = 0; i < myParamArray.Length; i++)
            {
                ResultString += (myParamArray[i].ParameterName + "=" + myParamArray[i].Value.ToString() + ",");
            }
            ResultString = ResultString.Substring(0, ResultString.Length - 1);
            ResultString += ")";
            return ResultString;
        }
        public void RightLogDo_Sava(string PersonID, string PersonName, string Node, string Operation, string IP)
        {
            //写入数据库
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = this.connectionstring;
            SqlCommand rs = new SqlCommand();
            rs.Connection = conn;
            try
            {
                conn.Open();

                rs.Parameters.Add("@PersonID", SqlDbType.VarChar).Value = PersonID.ToString();
                rs.Parameters.Add("@PersonName", SqlDbType.VarChar).Value = PersonName.ToString();
                rs.Parameters.Add("@Node", SqlDbType.VarChar).Value = Node.ToString();
                rs.Parameters.Add("@Operation", SqlDbType.VarChar).Value = Operation.ToString();
                rs.Parameters.Add("@NoteDateTime", SqlDbType.DateTime).Value = System.DateTime.Now.ToString();
                rs.Parameters.Add("@IP", SqlDbType.Char).Value = IP.ToString();
                rs.CommandText = "insert into sys_OperationNote(PersonID,PersonName,Node,Operation,NoteDateTime,IP) values(@PersonID,@PersonName,@Node,@Operation,@NoteDateTime,@IP)";

                rs.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                log.Log_Save(ex, Operation.ToString());
            }
            finally
            {
                rs.Dispose();
                rs.Connection.Close();
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// 日志操作
        /// </summary>
        /// <param name="xiaoXi"></param>
        private void RiZhi(string xiaoXi)
        {
            //创建日志
            if (!EventLog.SourceExists("SMS订单状态更新"))
            {
                EventLog.CreateEventSource("SMS订单状态更新", "DingDanZhuangTaiGeiBian");
            }

            EventLog myLog = new EventLog();
            myLog.Source = "SMS订单状态更新";

            myLog.WriteEntry(xiaoXi);
        }

        #endregion
    }
}
