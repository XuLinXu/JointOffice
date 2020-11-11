using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Web;

/// <summary>
/// mingjiacaozuo 的摘要说明。
/// </summary>
namespace TMSTuiSongJointOffice.DbHelper
{
    public class SqlDbHelper
    {
        private string constring;
        private int cssj = 300;
        //Note Sava
        private HttpContext Context = null;
        public DBErrorLog log;
        public RightLogDo rlog;
        /// <summary>
        /// 取数据库连接字符串
        /// </summary>
        /// 
        public SqlDbHelper(string constring)
        {
            this.constring = System.Configuration.ConfigurationManager.AppSettings["DbServerConnectionString"].ToString();
            if (HttpContext.Current != null)
            {
                Context = HttpContext.Current;
            }
            log = new DBErrorLog(constring);
            rlog = new RightLogDo(constring);
        }
        /// <summary>
        /// 取数据库连接字符串
        /// </summary>
        /// 
        public SqlDbHelper()
        {
            constring = System.Configuration.ConfigurationManager.AppSettings["JoinOfficeDbServerConnectionString"].ToString();
            if (HttpContext.Current != null)
            {
                Context = HttpContext.Current;
            }
            log = new DBErrorLog(constring);
            rlog = new RightLogDo(constring);
        }

        public SqlDbHelper(HttpContext CurrentContext)
        {
            constring = System.Configuration.ConfigurationManager.AppSettings["ConnString"].ToString();
            this.Context = CurrentContext;

            log = new DBErrorLog(constring);
            rlog = new RightLogDo(constring);
        }
        /// <summary>
        /// 执行SQL并返回DataSet
        /// </summary>
        /// <param name="startnum">开始记录值</param>
        /// <param name="fillnum">结束记录值</param>
        /// <param name="tablename">表名</param>
        /// <param name="comstr">SQL语句</param>
        /// <returns></returns>
        public DataSet Manage_List(int startnum, int fillnum, string tablename, string comstr)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(comstr, con);
            sqlDataAdapter1.SelectCommand.CommandTimeout = cssj;
            DataSet ds = new DataSet();
            try
            {
                sqlDataAdapter1.Fill(ds, startnum, fillnum, tablename);
            }
            catch (Exception e)
            {
                throw e;
            }
            return ds;

        }
        /// <summary>
        /// 执行SQL语句并返回记录数
        /// </summary>
        /// <param name="comstr"></param>
        /// <returns></returns>
        public int Manage_ListCount(string comstr)
        {

            SqlConnection con = new SqlConnection(constring);
            SqlCommand com = new SqlCommand(comstr, con);
            com.CommandTimeout = cssj;
            con.Open();
            int sum = 0;
            try
            {
                sum = (int)com.ExecuteScalar();
                //日志写入，查询语句
                ////rlog.RightLogDo_S( rlog.GetTrueSql(comstr));
            }
            catch (Exception e)
            {
                throw e;

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return sum;
        }
        /// <summary>
        /// 执行带参数的SQL语句并返回Dataset
        /// </summary>
        /// <param name="startnum">开始记录值</param>
        /// <param name="fillnum">结束记录值</param>
        /// <param name="tablename">表名</param>
        /// <param name="comstr">sql语句</param>
        /// <param name="myParamArray">参数数组</param>
        /// <returns></returns>
        public DataSet Manage_GetListByParam(int startnum, int fillnum, string tablename, string comstr, SqlParameter[] myParamArray)
        {

            SqlConnection con = new SqlConnection(constring);
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(comstr, con);
            sqlDataAdapter1.SelectCommand.CommandTimeout = cssj;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                sqlDataAdapter1.SelectCommand.Parameters.Add(myParamArray[j]);
            }

            DataSet ds = new DataSet();
            try
            {
                sqlDataAdapter1.Fill(ds, startnum, fillnum, tablename);
            }
            catch (Exception e)
            {
                throw e;
            }

            return ds;

        }

        /// <summary>
        /// 执行带参数的存储过程并返回Dataset
        /// </summary>
        /// <param name="startnum">开始记录值</param>
        /// <param name="fillnum">结束记录值</param>
        /// <param name="tablename">表名</param>
        /// <param name="comstr">sql语句</param>
        /// <param name="myParamArray">参数数组</param>
        /// <returns></returns>
        public DataSet Manage_GetListByParam_Pro(int startnum, int fillnum, string tablename, string comstr, SqlParameter[] myParamArray)
        {

            SqlConnection con = new SqlConnection(constring);
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(comstr, con);
            sqlDataAdapter1.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter1.SelectCommand.CommandTimeout = cssj;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                sqlDataAdapter1.SelectCommand.Parameters.Add(myParamArray[j]);
            }

            DataSet ds = new DataSet();
            try
            {
                sqlDataAdapter1.Fill(ds, startnum, fillnum, tablename);
            }
            catch (Exception e)
            {
                throw e;
            }

            return ds;

        }

        /// <summary>
        /// 执行带参数的SQL语句并返回记录总数
        /// </summary>
        /// <param name="comstr">sql语句</param>
        /// <param name="myParamArray">参数数组</param>
        /// <returns></returns>
        public int Manage_GetListCountByParam(string comstr, SqlParameter[] myParamArray)
        {

            SqlConnection con = new SqlConnection(constring);

            SqlCommand com = new SqlCommand(comstr, con);
            com.CommandTimeout = cssj;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                com.Parameters.Add(myParamArray[j]);
            }

            con.Open();
            int sum = 0;
            try
            {
                sum = (int)com.ExecuteScalar();
                //写入日志，带参数
                rlog.RightLogDo_S(rlog.GetTrueSql(comstr, myParamArray));
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return sum;
        }
        /// <summary>
        /// 执行带参数的存储过程并返回记录总数
        /// </summary>
        /// <param name="comstr">sql语句</param>
        /// <param name="myParamArray">参数数组</param>
        /// <returns></returns>
        public int Manage_GetListCountByParam_Pro(string comstr, SqlParameter[] myParamArray)
        {

            SqlConnection con = new SqlConnection(constring);
            SqlCommand com = new SqlCommand(comstr, con);
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = cssj;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                com.Parameters.Add(myParamArray[j]);
            }

            con.Open();
            int sum = 0;
            try
            {
                sum = (int)com.ExecuteScalar();
                //写入日志，带参数
                rlog.RightLogDo_S(rlog.GetTrueSql(comstr, myParamArray));
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return sum;
        }

        /// <summary>
        /// 执行SQL语句并返回记录总数
        /// </summary>
        /// <param name="comstr"></param>
        /// <returns></returns>
        public int Manage_caozuo(string comstr)
        {

            SqlConnection con = new SqlConnection(constring);
            SqlCommand com = new SqlCommand(comstr, con);
            com.CommandTimeout = cssj;
            con.Open();
            int i = 0;
            try
            {
                i = com.ExecuteNonQuery();
                //写入日志
                rlog.RightLogDo_S(comstr);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
                //con.Dispose();
            }

            return i;
        }
        /// <summary>
        /// 执行带参数的SQL语句并返回记录总数
        /// </summary>
        /// <param name="comstr"></param>
        /// <returns></returns>
        public int Manage_caozuoByParam(string comstr, SqlParameter[] myParamArray)
        {

            SqlConnection con = new SqlConnection(constring);
            SqlCommand com = new SqlCommand(comstr, con);
            com.CommandTimeout = cssj;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                com.Parameters.Add(myParamArray[j]);
            }

            con.Open();
            int i = 0;
            try
            {
                i = com.ExecuteNonQuery();
                //写入日志
                rlog.RightLogDo_S(rlog.GetTrueSql(comstr, myParamArray));
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return i;
        }
        /// <summary>
        /// 执行带参数的存储过程并返回记录总数
        /// </summary>
        /// <param name="comstr"></param>
        /// <returns></returns>
        public int Manage_caozuoByParam_Pro(string comstr, SqlParameter[] myParamArray)
        {

            SqlConnection con = new SqlConnection(constring);
            SqlCommand com = new SqlCommand(comstr, con);
            com.CommandTimeout = cssj;
            com.CommandType = CommandType.StoredProcedure;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                com.Parameters.Add(myParamArray[j]);
            }

            con.Open();
            int i = 0;
            try
            {
                i = com.ExecuteNonQuery();
                //写入日志
                rlog.RightLogDo_S(rlog.GetTrueSql(comstr, myParamArray));
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return i;
        }
        /// <summary>
        /// 执行带参数的存储过程并返回记录总数
        /// </summary>
        /// <param name="comstr"></param>
        /// <returns></returns>
        public int Manage_caozuoByParam_Pro(string comstr)
        {

            SqlConnection con = new SqlConnection(constring);
            SqlCommand com = new SqlCommand(comstr, con);
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = cssj;
            con.Open();
            int i = 0;
            try
            {
                i = com.ExecuteNonQuery();
                //写入日志
                //rlog.RightLogDo_S( rlog.GetTrueSql(comstr));
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return i;
        }
        /// <summary>
        /// 执行带参数的存储过程并返回记录总数
        /// </summary>
        /// <param name="comstr"></param>
        /// <returns></returns>
        public int Manage_caozuoByParam_ProForReturnValue(string comstr, SqlParameter[] myParamArray)
        {

            SqlConnection con = new SqlConnection(constring);
            SqlCommand com = new SqlCommand(comstr, con);
            com.CommandTimeout = cssj;
            com.CommandType = CommandType.StoredProcedure;

            for (int j = 0; j < myParamArray.Length; j++)
            {
                com.Parameters.Add(myParamArray[j]);
            }
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            int i = 0;
            try
            {
                sqlDataAdapter1.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return int.Parse(dt.Rows[0][0].ToString());
                }
                //写入日志
                rlog.RightLogDo_S(rlog.GetTrueSql(comstr, myParamArray));
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return i;
        }
        /// <summary>
        /// 执行带参数的存储过程并返回记录总数
        /// </summary>
        /// <param name="comstr"></param>
        /// <returns></returns>
        public int Manage_caozuoByParam_ProForReturnValue(string comstr)
        {

            SqlConnection con = new SqlConnection(constring);
            SqlCommand com = new SqlCommand(comstr, con);
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = cssj;
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            int i = 0;
            try
            {
                sqlDataAdapter1.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return int.Parse(dt.Rows[0][0].ToString());
                }
                //i = com.ExecuteNonQuery();
                //写入日志
                //rlog.RightLogDo_S( rlog.GetTrueSql(comstr));
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return i;
        }
        /// <summary>
        /// 执行SQL语句并返回DataTable
        /// </summary>
        /// <param name="comstr"></param>
        /// <returns>返回DataTable</returns>
        public DataTable Manage_ResultTB_Caozuo(string comstr)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(comstr, con);
            sqlDataAdapter1.SelectCommand.CommandTimeout = cssj;
            DataTable dt = new DataTable();
            try
            {
                sqlDataAdapter1.Fill(dt);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        /// <summary>
        /// 执行带参数的SQL语句返回DataTablet
        /// </summary>
        /// <param name="comstr"></param>
        /// <returns>返回DataTable</returns>
        public DataTable Manage_ResultTB_CaozuoByParam(string comstr, SqlParameter[] myParamArray)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(comstr, con);
            sqlDataAdapter1.SelectCommand.CommandTimeout = cssj;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                sqlDataAdapter1.SelectCommand.Parameters.Add(myParamArray[j]);
            }
            DataTable dt = new DataTable();
            try
            {
                sqlDataAdapter1.Fill(dt);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        /// <summary>
        /// 执行带参数的存储过程并返回DataTable
        /// </summary>
        /// <param name="comstr">sql语句</param>
        /// <param name="myParamArray">参数</param>
        /// <returns>返回DataTable</returns>
        public DataTable Manage_ResultTB_CaozuoByParam_Pro(string comstr, SqlParameter[] myParamArray)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlDataAdapter da = new SqlDataAdapter(comstr, con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = cssj;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                da.SelectCommand.Parameters.Add(myParamArray[j]);
            }
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        /// <summary>
        /// 执行带参数的存储过程并返回DataTable
        /// </summary>
        /// <param name="comstr">sql语句</param>
        /// <param name="myParamArray">参数</param>
        /// <returns>返回DataTable</returns>
        public DataTable Manage_ResultTB_CaozuoByParam_Pro(string comstr)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlDataAdapter da = new SqlDataAdapter(comstr, con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = cssj;
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        /// <summary>
        /// 执行SQL语句并返回DataReader
        /// </summary>
        /// <param name="comstr"></param>
        /// <returns></returns>
        public SqlDataReader Manage_ResultDR_Caozuo(string comstr)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlCommand com = new SqlCommand(comstr, con);
            com.CommandTimeout = cssj;
            SqlDataReader dr = null;
            try
            {
                con.Open();

                dr = com.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                throw e;
            }

            return dr;
        }
        /// <summary>
        /// 执行带参数的SQL语句并返回DataReader
        /// </summary>
        /// <param name="comstr"></param>
        /// <param name="myParamArray"></param>
        /// <returns></returns>
        public SqlDataReader Manage_ResultDR_CaozuoByParam(string comstr, SqlParameter[] myParamArray)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlCommand com = new SqlCommand(comstr, con);
            com.CommandTimeout = cssj;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                com.Parameters.Add(myParamArray[j]);
            }

            SqlDataReader dr = null;
            try
            {
                con.Open();

                dr = com.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                throw e;
            }
            return dr;
        }
        /// <summary>
        /// 执行带参数的存储过程并返回DataReader
        /// </summary>
        /// <param name="comstr"></param>
        /// <param name="myParamArray"></param>
        /// <returns></returns>
        public SqlDataReader Manage_ResultDR_CaozuoByParam_Pro(string comstr, SqlParameter[] myParamArray)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlCommand com = new SqlCommand(comstr, con);
            com.CommandTimeout = cssj;
            com.CommandType = CommandType.StoredProcedure;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                com.Parameters.Add(myParamArray[j]);
            }

            SqlDataReader dr = null;
            try
            {
                con.Open();

                dr = com.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                throw e;
                //Modified by mayue in 2006-01-13
                //con.Close();
            }
            return dr;
        }
        /// <summary>
        /// 执行Sql并返回DataSet
        /// </summary>
        /// <param name="comstr"></param>
        /// <returns></returns>
        public DataSet Manage_ResultDS_Caozuo(string comstr)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(comstr, con);
            sqlDataAdapter1.SelectCommand.CommandTimeout = cssj;
            DataSet dt = new DataSet();
            try
            {
                sqlDataAdapter1.Fill(dt);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        /// <summary>
        /// 执行带参数的sql并返回DataSet
        /// </summary>
        /// <param name="comstr"></param>
        /// <returns></returns>
        public DataSet Manage_ResultDS_CaozuoByParam(string comstr, SqlParameter[] myParamArray)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(comstr, con);
            sqlDataAdapter1.SelectCommand.CommandTimeout = cssj;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                sqlDataAdapter1.SelectCommand.Parameters.Add(myParamArray[j]);
            }
            DataSet dt = new DataSet();
            try
            {
                sqlDataAdapter1.Fill(dt);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        /// <summary>
        /// 执行带参数的存储过程并返回DataSet
        /// </summary>
        /// <param name="comstr"></param>
        /// <param name="myParamArray"></param>
        /// <returns></returns>
        public DataSet Manage_ResultDS_CaozuoByParam_Pro(string comstr, SqlParameter[] myParamArray)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(comstr, con);
            sqlDataAdapter1.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter1.SelectCommand.CommandTimeout = cssj;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                sqlDataAdapter1.SelectCommand.Parameters.Add(myParamArray[j]);
            }
            DataSet dt = new DataSet();
            try
            {
                sqlDataAdapter1.Fill(dt);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        /// <summary>
        /// 执行存储过程并返回DataSet
        /// </summary>
        /// <param name="comstr"></param>
        /// <param name="myParamArray"></param>
        /// <returns></returns>
        public DataSet Manage_ResultDS_CaozuoByParam_Pro(string comstr)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(comstr, con);
            sqlDataAdapter1.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter1.SelectCommand.CommandTimeout = cssj;
            DataSet dt = new DataSet();
            try
            {
                sqlDataAdapter1.Fill(dt);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        /// <summary>
        /// 执行带参数的Sql并返回Command
        /// </summary>
        /// <param name="comstr">sql</param>
        /// <param name="ct"></param>
        /// <param name="myParamArray">参数</param>
        /// <returns></returns>
        public SqlCommand Manage_ResultCommand(string comstr, CommandType ct, SqlParameter[] myParamArray)
        {
            SqlCommand com = new SqlCommand(comstr);
            com.CommandTimeout = cssj;
            com.CommandType = ct;
            for (int j = 0; j < myParamArray.Length; j++)
            {
                com.Parameters.Add(myParamArray[j]);
            }
            return com;
        }
        /// <summary>
        /// 执行sql并返回Commmand
        /// </summary>
        /// <param name="comstr"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public SqlCommand Manage_ResultCommand(string comstr, CommandType ct)
        {
            SqlCommand com = new SqlCommand(comstr);
            com.CommandTimeout = cssj;
            com.CommandType = ct;
            return com;
        }
        /// <summary>
        /// 执行sql并进行事务操作
        /// </summary>
        /// <param name="comds"></param>
        /// <returns></returns>
        public bool Manager_caozuoByTransForText(string comds)
        {
            bool ok = false;
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlTransaction myTrans;
            string cmdText = comds;
            myTrans = con.BeginTransaction();
            try
            {

                SqlCommand cmdEnd = new SqlCommand(cmdText);
                cmdEnd.CommandTimeout = cssj;
                cmdEnd.Connection = con;
                cmdEnd.Transaction = myTrans;
                cmdEnd.ExecuteNonQuery();
                myTrans.Commit();
                ok = true;
                //添加日志
                rlog.RightLogDo_S("事务处理");
            }
            catch (Exception e)
            {
                try
                {
                    myTrans.Rollback();
                    log.Log_Save(e, "事务回滚");
                }
                catch (SqlException ex)
                {
                    if (myTrans.Connection != null)
                    {

                        log.Log_Save(ex, "事务回滚");
                    }
                }

            }
            finally
            {
                con.Close();
            }
            return ok;
        }
        /// <summary>
        /// 执行sql并进行事务操作
        /// </summary>
        /// <param name="comds"></param>
        /// <returns></returns>
        public bool Manager_caozuoByTrans(ArrayList comds)
        {
            bool ok = false;
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlTransaction myTrans;
            string cmdText = "";
            myTrans = con.BeginTransaction();
            try
            {

                for (int i = 0; i < comds.Count; i++)
                {
                    cmdText += ((SqlCommand)comds[i]).CommandText + " ";
                }

                //SqlCommand ccd=(SqlCommand)comds[i];
                SqlCommand cmdEnd = new SqlCommand(cmdText);
                cmdEnd.CommandTimeout = cssj;
                cmdEnd.Connection = con;
                cmdEnd.Transaction = myTrans;
                cmdEnd.ExecuteNonQuery();
                //ccd.Connection=con;
                //ccd.Transaction=myTrans;
                //ccd.ExecuteNonQuery();
                myTrans.Commit();
                ok = true;
                //添加日志
                rlog.RightLogDo_S("事务处理");
            }
            catch (Exception e)
            {
                try
                {
                    myTrans.Rollback();
                    log.Log_Save(e, "事务回滚");
                }
                catch (SqlException ex)
                {
                    if (myTrans.Connection != null)
                    {

                        log.Log_Save(ex, "事务回滚");
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ok;
        }
        /// <summary>
        /// 执行sql并进行事务操作
        /// </summary>
        /// <param name="comds"></param>
        /// <returns></returns>
        public bool Manager_caozuoByTrans_NoText(ArrayList comds)
        {
            bool ok = false;
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlTransaction myTrans;
            //string cmdText = "";
            myTrans = con.BeginTransaction();
            try
            {
                for (int i = 0; i < comds.Count; i++)
                {
                    SqlCommand ccd = (SqlCommand)comds[i];
                    ccd.Connection = con;
                    ccd.Transaction = myTrans;
                    ccd.ExecuteNonQuery();
                }
                myTrans.Commit();
                ok = true;
                //添加日志
                rlog.RightLogDo_S("事务处理");
            }
            catch (Exception e)
            {
                try
                {
                    myTrans.Rollback();
                    log.Log_Save(e, "事务回滚");
                }
                catch (SqlException ex)
                {
                    if (myTrans.Connection != null)
                    {
                        log.Log_Save(ex, "事务回滚");
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ok;
        }
        public bool Manager_caozuoByTrans_NoText(List<SqlCommand> comds)
        {
            bool ok = false;
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlTransaction myTrans;
            //string cmdText = "";
            myTrans = con.BeginTransaction();
            try
            {
                for (int i = 0; i < comds.Count; i++)
                {
                    SqlCommand ccd = comds[i];
                    ccd.Connection = con;
                    ccd.Transaction = myTrans;
                    ccd.ExecuteNonQuery();
                }
                myTrans.Commit();
                ok = true;
                //添加日志
                rlog.RightLogDo_S("事务处理");
            }
            catch (Exception e)
            {
                try
                {
                    myTrans.Rollback();
                    log.Log_Save(e, "事务回滚");
                }
                catch (SqlException ex)
                {
                    if (myTrans.Connection != null)
                    {
                        log.Log_Save(ex, "事务回滚");
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ok;
        }
        /// <summary>
        /// 为短信自动发送功能所开发--执行sql并进行事务操作
        /// </summary>
        /// <param name="comds"></param>
        /// <returns></returns>
        public bool Manager_caozuoByTrans_NoTextForSms(ArrayList comds)
        {
            bool ok = false;
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlTransaction myTrans;
            //string cmdText = "";
            myTrans = con.BeginTransaction();
            try
            {

                for (int i = 0; i < comds.Count; i++)
                {
                    SqlCommand ccd = (SqlCommand)comds[i];
                    ccd.Connection = con;
                    ccd.Transaction = myTrans;
                    ccd.ExecuteNonQuery();
                }
                myTrans.Commit();
                ok = true;
            }
            catch (Exception e)
            {
                myTrans.Rollback();
            }
            finally
            {
                con.Close();
            }
            return ok;
        }
    }
}