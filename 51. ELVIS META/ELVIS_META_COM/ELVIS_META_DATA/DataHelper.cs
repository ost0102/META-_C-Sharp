using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using System.Collections;

namespace ELVIS_META_DATA
{
    public static class DataHelper
    {
        private static string _connectionString;
        private static string _connectionString_Select;
        private static object _connectionStringLockObject = new object();
        private static string _ConnectionEtcString;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    lock (_connectionStringLockObject)
                    {
                        _connectionString = ConfigurationManager.ConnectionStrings["PRIME"].ConnectionString;
                    }
                }

                return _connectionString;
            }
        }

        public static string ConnectionString_Select
        {
            set
            {
                if (value == "PRIME")
                {
                    lock (_connectionStringLockObject)
                    {
                        _connectionString = ConfigurationManager.ConnectionStrings["PRIME"].ConnectionString;
                    }
                }
                else if (value == "CRM")
                {
                    lock (_connectionStringLockObject)
                    {
                        _connectionString = ConfigurationManager.ConnectionStrings["CRM"].ConnectionString;
                    }
                }
                else
                {
                    lock (_connectionStringLockObject)
                    {
                        _connectionString = ConfigurationManager.ConnectionStrings["PRIME"].ConnectionString;
                    }
                }
            }
        }

        #region "ODP.NET 이용 관련"

        /// <summary>
        /// Insert Query 전용
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string Sql, CommandType cmdType)
        {
            int result;

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand(Sql, conn);
                    cmd.CommandType = cmdType;

                    result = cmd.ExecuteNonQuery();

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

        #endregion // ADO.NET 끝
    }
}
