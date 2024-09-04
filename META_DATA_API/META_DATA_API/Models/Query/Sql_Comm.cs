using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using META_DATA_API.Models;
using META_DATA_API.Models.TBL;

namespace META_DATA_API.Models.Query
{
    public static class Sql_Comm
    {
        private static string sSql = "";        
        private static bool rtnBool = false;
        public static bool Check_Auth(META_USR_MST Auth_Usr)
        {
            rtnBool = false;
            sSql = "";
            sSql += " SELECT * ";
            sSql += "   FROM META_USR_MST ";
            sSql += "  WHERE APV_YN = 'Y'";
            sSql += "    AND MNGT_NO = '"+ Auth_Usr.MNGT_NO.ToString() + "' ";            
            sSql += "    AND USR_ID = '" + Auth_Usr.USR_ID.ToString() + "' ";
            sSql += "    AND CRN = '" + Auth_Usr.CRN.ToString() + "' ";
            if (!string.IsNullOrEmpty(Auth_Usr.AUTH_STRT_YMD.ToString())) sSql += "AND AUTH_STRT_YMD = '" + Auth_Usr.AUTH_STRT_YMD.ToString() + "' ";
            if (!string.IsNullOrEmpty(Auth_Usr.AUTH_END_YMD.ToString())) sSql += "AND AUTH_END_YMD = '" + Auth_Usr.AUTH_END_YMD.ToString() + "' ";

            DataTable dt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            if (dt.Rows.Count > 0) rtnBool = true;            
            return rtnBool;
        }
    }
}