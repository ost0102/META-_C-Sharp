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
    public static class Sql_Schedule
    {
        private static string sSql = "";
        private static DataTable rtnDt = null;

        /// <summary>
        /// Search Schedule Func
        /// </summary>
        /// <param name="dr">REQ_SVC / LINE_CD / ST_DATE / END_DATE / </param>
        /// <returns></returns>
        public static DataTable Get_Schedule(string Req_Svc, string Line_Cd, string Start_Date, string End_date, string PageCount)
        {
            int pageCount = 1;
            if (!string.IsNullOrEmpty(PageCount)) pageCount = int.Parse(PageCount);

            if (!string.IsNullOrEmpty(Req_Svc) && !string.IsNullOrEmpty(Line_Cd) && !string.IsNullOrEmpty(Start_Date) && !string.IsNullOrEmpty(End_date))                
            {
                sSql = "";
                sSql += " SELECT * ";
                sSql += "   FROM (SELECT COUNT(*) OVER() AS TOTCNT ";
                sSql += "              , FLOOR((COUNT(*) OVER() - 1) / " + _common.DataPageCount + " + 1) AS PAGECNT ";
                sSql += "              , FLOOR((ROWNUM - 1) / "+ _common.DataPageCount +" + 1) AS PAGE ";
                sSql += "              , A.* ";
                sSql += "           FROM RPA_SCH_MST@DL_RPA A ";
                sSql += "     INNER JOIN RPA_CARR_MST@DL_RPA B ";
                sSql += "             ON A.LINE_CD = B.CARR_CD ";
                sSql += "          WHERE LENGTH(A.SCH_NO) > 0 ";
                sSql += "            AND A.REQ_SVC = '" + Req_Svc + "' ";
                sSql += "            AND A.LINE_CD = '" + Line_Cd + "' ";
                sSql += "            AND A.ETD >= '" + Start_Date + "' ";
                sSql += "            AND A.ETD <= '" + End_date + "' ";
                sSql += "          ORDER BY A.ETD, A.ETD_HM, A.SCH_NO) X ";
                sSql += "  WHERE X.PAGE = "+ PageCount + " ";
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);            
            return rtnDt;
        }

        public static DataTable Get_Schedule_Port(string Req_Svc, string Pol_Cd, string Pod_Cd, string Start_Date, string End_date, string Line_Cd)
        {                        
            if (!string.IsNullOrEmpty(Req_Svc) && !string.IsNullOrEmpty(Pol_Cd) && !string.IsNullOrEmpty(Pod_Cd) && !string.IsNullOrEmpty(Start_Date) && !string.IsNullOrEmpty(End_date))
            {
                sSql = "";
                sSql += " SELECT A.* ";
                sSql += "   FROM RPA_SCH_MST@DL_RPA A ";
                sSql += "  INNER JOIN RPA_CARR_MST@DL_RPA B ";
                sSql += "     ON A.LINE_CD = B.CARR_CD ";
                sSql += "  WHERE LENGTH(A.SCH_NO) > 0 ";
                sSql += "    AND A.REQ_SVC = '" + Req_Svc + "' ";
                sSql += "    AND A.ETD >= '" + Start_Date + "' ";
                sSql += "    AND A.ETD <= '" + End_date + "' ";
                sSql += "    AND A.POL_CD = '" + Pol_Cd + "' ";
                sSql += "    AND A.POD_CD = '" + Pod_Cd + "' ";
                if (!string.IsNullOrEmpty(Line_Cd)) sSql += " AND A.LINE_CD = '" + Line_Cd + "' ";
                sSql += "  ORDER BY A.ETD, A.ETD_HM, A.SCH_NO ";
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        public static DataTable Get_Port_List(string Req_Svc, string Loc_Cd)
        {
            if (!string.IsNullOrEmpty(Req_Svc) && !string.IsNullOrEmpty(Loc_Cd))            
            {
                sSql = "";
                sSql += " SELECT LOC_CD AS CODE ";
                sSql += "      , LOC_NM AS NAME ";
                sSql += "   FROM MDM_PORT_MST ";
                sSql += "  WHERE 1 = 1 ";
                sSql += "    AND LOC_CD IS NOT NULL ";
                sSql += "    AND (('" + Req_Svc + "' IS NULL AND 1 = 1) ";
                sSql += "        OR ('" + Req_Svc + "' IS NOT NULL AND LOC_TYPE = '" + Req_Svc.Substring(0,1) + "')) ";

                if (Loc_Cd != "")
                {
                    sSql += " AND (REPLACE (LOC_CD, ' ', '') LIKE '%" + Loc_Cd + "%' ";
                    sSql += " OR REPLACE (LOC_NM, ' ', '') LIKE '%" + Loc_Cd + "%') ";
                }
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }
    }
}