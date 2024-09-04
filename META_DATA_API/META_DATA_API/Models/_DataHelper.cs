using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
namespace META_DATA_API.Models
{
    public class _DataHelper
    {
        private static string _connectionString;
        private static object _connectionStringLockObject = new object();
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    lock (_connectionStringLockObject)
                    {
                        _connectionString = ConfigurationManager.ConnectionStrings["META_ORACLE"].ConnectionString;
                    }
                }

                return _connectionString;
            }
        }

        public static bool CheckDataBaseConnecting()
        {
            try
            {
                OracleConnection conn = new OracleConnection(ConnectionString);
                conn.Open();
                conn.Clone();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Insert Query 전용
        /// </summary>
        /// <param name="Sql">Oracle Query</param>
        /// <param name="cmdType">Query Type</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string Sql, CommandType cmdType)
        {
            int result;

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {

                conn.Open();
                OracleTransaction tran = conn.BeginTransaction();
                OracleCommand cmd = new OracleCommand(Sql, conn);
                cmd.CommandType = cmdType;
                cmd.Connection = conn;
                cmd.Transaction = tran;
                try
                {
                    result = cmd.ExecuteNonQuery();
                    tran.Commit();
                    conn.Close();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }
            return result;
        }

        public static int ExecuteNonQuery(string _ConnStr, string Sql, CommandType cmdType)
        {
            int result;

            using (OracleConnection conn = new OracleConnection(_ConnStr))
            {

                conn.Open();
                OracleTransaction tran = conn.BeginTransaction();
                OracleCommand cmd = new OracleCommand(Sql, conn);
                cmd.CommandType = cmdType;
                cmd.Connection = conn;
                cmd.Transaction = tran;
                try
                {
                    result = cmd.ExecuteNonQuery();

                    tran.Commit();

                    conn.Close();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }

            return result;
        }

        public static object ExecuteScalar(string Sql, CommandType cmdType)
        {
            object result = null;

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    OracleCommand cmd = new OracleCommand(Sql, conn);
                    cmd.CommandType = cmdType;

                    result = cmd.ExecuteScalar();

                    conn.Close();
                }
                catch (Exception)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();

                    throw;
                }
            }

            return result;
        }

        public static object ExecuteScalar(string _ConnStr, string Sql, CommandType cmdType)
        {
            object result = null;

            using (OracleConnection conn = new OracleConnection(_ConnStr))
            {
                try
                {
                    conn.Open();

                    OracleCommand cmd = new OracleCommand(Sql, conn);
                    cmd.CommandType = cmdType;

                    result = cmd.ExecuteScalar();

                    conn.Close();
                }
                catch (Exception)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();

                    throw;
                }
            }

            return result;
        }

        public static DataSet ExecuteDataSet(string Sql, CommandType cmdType)
        {
            DataSet dsResult = null;

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand(Sql, conn);

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    dsResult = new DataSet();
                    da.Fill(dsResult);
                    da.Dispose();
                    conn.Close();

                }
                catch (Exception)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }

            return dsResult;
        }

        public static DataSet ExecuteDataSet(string _ConnStr, string Sql, CommandType cmdType)
        {
            DataSet dsResult = null;

            using (OracleConnection conn = new OracleConnection(_ConnStr))
            {
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand(Sql, conn);

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    dsResult = new DataSet();
                    da.Fill(dsResult);
                    da.Dispose();
                    conn.Close();

                }
                catch (Exception)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }

            return dsResult;
        }

        public static DataTable ExecuteDataTable(string Sql, CommandType cmdType)
        {
            DataTable dtResult = null;

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    OracleCommand cmd = new OracleCommand(Sql, conn);
                    cmd.CommandType = cmdType;

                    dtResult = new DataTable();

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(dtResult);
                    da.Dispose();
                    conn.Close();
                }
                catch (Exception)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }

            return dtResult;
        }

        public static DataTable ExecuteDataTable(string _ConnStr, string Sql, CommandType cmdType)
        {
            DataTable dtResult = null;

            using (OracleConnection conn = new OracleConnection(_ConnStr))
            {
                try
                {
                    conn.Open();

                    OracleCommand cmd = new OracleCommand(Sql, conn);
                    cmd.CommandType = cmdType;

                    dtResult = new DataTable();

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(dtResult);
                    da.Dispose();
                    conn.Close();
                }
                catch (Exception)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }

            return dtResult;
        }

        public static OracleDataReader ExecuteDataReader(string Sql, CommandType cmdType)
        {
            OracleDataReader rs = null;
            OracleConnection conn = null;

            try
            {
                conn = new OracleConnection(ConnectionString);
                conn.Open();

                OracleCommand cmd = new OracleCommand(Sql, conn);
                cmd.CommandType = cmdType;
                rs = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception)
            {
                if (rs != null)
                {
                    rs.Close();
                }

                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }

                throw;
            }

            
            return rs;
        }

        /*
        public static Hashtable CallPRoc(string procName, Dictionary<string, object> ValidateParams)
        {
            int result = 0;
            Hashtable ht = new Hashtable();
            if (string.IsNullOrEmpty(procName)) procName = "USP_CREATE_AUTO_KEY";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {

                try
                {
                    conn.Open();

                    OracleCommand cmd = new OracleCommand(procName, conn);
                    if (ValidateParams.Count > 0)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_JOB_TYPE", OracleDbType.Varchar2, 32767).Value = ValidateParams["P_JOB_TYPE"].ToString();
                        cmd.Parameters.Add("P_PRX1", OracleDbType.Varchar2, 32767).Value = ValidateParams["P_PRX1"].ToString();
                        cmd.Parameters.Add("P_PRX2", OracleDbType.Varchar2, 32767).Value = ValidateParams["P_PRX2"].ToString();
                        cmd.Parameters.Add("P_PRX3", OracleDbType.Varchar2, 32767).Value = ValidateParams["P_PRX3"].ToString();
                        cmd.Parameters.Add("P_CNT_LENGTH", OracleDbType.Varchar2, 32767).Value = "3";
                        cmd.Parameters.Add("R_RTNCD", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("R_RTNMSG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                        //OracleDataAdapter da = new OracleDataAdapter(cmd);
                        //da.Fill(dtResult);
                        //da.Dispose();

                        result = cmd.ExecuteNonQuery();
                        conn.Close();

                        ht.Add("R_RTNCD", cmd.Parameters["R_RTNCD"].Value.ToString());
                        ht.Add("R_RTNMSG", cmd.Parameters["R_RTNMSG"].Value.ToString());

                    }
                }
                catch (Exception e)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }
            return ht;
        }
        */



    }
}