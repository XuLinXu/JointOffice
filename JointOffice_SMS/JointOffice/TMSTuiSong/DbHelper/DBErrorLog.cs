using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.SessionState;

namespace TMSTuiSongJointOffice.DbHelper
{
    /// <summary>
    /// 系统日志相关操作
    /// </summary>
    public class DBErrorLog
    {
        string connstr = "";
        public DBErrorLog(string conString)
        {
            this.connstr = conString;
        }


        ////////////////////////////////////////////////////////////////////////
        ///                     
        ///                 "系统日志相关操作"
        ///
        ////////////////////////////////////////////////////////////////////////
        #region 系统日志相关操作
        /// <summary>
        /// 日志获取
        /// </summary>
        /// <param name="e"></param>

        public void Log_Save(System.Exception e, string sql)
        {
            string[] i = new string[2];
            i[0] = e.Message;
            i[1] = e.Source;
            Log_Db_Save(i[0].Trim(), sql.Trim(), "无");


        }
        /// <summary>
        /// 日志写入数据库
        /// </summary>
        /// <param name="sjm">事件名称</param>
        /// <param name="k_sql">SQL语句</param>
        /// <param name="bz">备注</param>
        public void Log_Db_Save(string sjm, string k_sql, string bz)
        {
            SqlConnection conn = new SqlConnection(connstr);
            string sql = "insert into log(sjm,k_sql,rzsj,bz)values(@sjm,@k_sql,@sj,@bz)";
            SqlParameter[] para = { new SqlParameter("@sjm", SqlDbType.VarChar, 300), new SqlParameter("@k_sql", SqlDbType.VarChar, 300), new SqlParameter("@sj", SqlDbType.DateTime, 8), new SqlParameter("@bz", SqlDbType.VarChar, 50) };
            para[0].Value = sjm;
            para[1].Value = k_sql;
            para[2].Value = DateTime.Now;
            para[3].Value = bz;

            SqlCommand com = new SqlCommand(sql, conn);

            for (int i = 0; i < para.Length; i++)
            {
                com.Parameters.Add(para[i]);
            }
            conn.Open();
            try
            {
                com.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //Log_Save(e, sql);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
    }
}

