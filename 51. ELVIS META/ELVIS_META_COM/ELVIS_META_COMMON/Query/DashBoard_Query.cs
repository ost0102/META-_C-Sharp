using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YJIT.Utils;

namespace ELVIS_META_COMMON.Query
{
    class DashBoard_Query
    {
        string sqlstr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_fnGetDashBoardData(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT * ";
            sqlstr += "   FROM (SELECT ROWNUM AS RNUM, ";
            sqlstr += "                FLOOR ( (ROWNUM - 1) / 7 + 1) AS PAGE, ";
            sqlstr += "                COUNT (*) OVER () AS TOTCNT, ";
            sqlstr += "                A.* ";
            sqlstr += "           FROM (  SELECT MST.BK_NO, ";
            sqlstr += "                          MST.LINE_BKG_NO, ";
            sqlstr += "                          MST.HBL_NO, ";
            sqlstr += "                          MST.MBL_NO, ";
            sqlstr += "                          MST.SR_NO, ";
            sqlstr += "                          CNTR.CNTR_NO, ";
            sqlstr += "                          MST.BK_SEND_YMD, ";
            sqlstr += "                          MST.LINE_CD, ";
            sqlstr += "                          (SELECT IMG_PATH FROM BIG_MDM_LINE_IMG WHERE LINE_CD = MST.LINE_CD) AS LINE_PATH,  ";
            sqlstr += "                          MST.POL_CD, ";
            sqlstr += "                          (SELECT PORT_NM || ' , ' || PORT_CTRY_NM ";
            sqlstr += "                             FROM BIG_MDM_PORT_MST ";
            sqlstr += "                            WHERE PORT_CD = MST.POL_CD) ";
            sqlstr += "                             AS POL_NM, ";
            sqlstr += "                          MST.POD_CD, ";
            sqlstr += "                          (SELECT PORT_NM || ' , ' || PORT_CTRY_NM ";
            sqlstr += "                             FROM BIG_MDM_PORT_MST ";
            sqlstr += "                            WHERE PORT_CD = MST.POD_CD) ";
            sqlstr += "                             AS POD_NM, ";
            sqlstr += "                          MST.ETD, ";
            sqlstr += "                          MST.ETA, ";
            sqlstr += "                          MST.BK_CUST_NM, ";
            sqlstr += "                          (SELECT OFFICE_NM ";
            sqlstr += "                             FROM BIG_MDM_OFFICE_MST ";
            sqlstr += "                            WHERE OFFICE_CD = MST.LINE_CD) ";
            sqlstr += "                             AS OFFICE_NM, ";
            sqlstr += "                          MST.BK_RECV_YMD, ";
            sqlstr += "                          CASE ";
            sqlstr += "                             WHEN MST.BK_STATUS = '20' THEN 'Y' ";
            sqlstr += "                             WHEN MST.BK_STATUS = '31' THEN 'D' ";
            sqlstr += "                             ELSE 'N' ";
            sqlstr += "                          END ";
            sqlstr += "                             AS BK_STATUS, ";
            sqlstr += "                          MST.SR_SEND_YMD, ";
            sqlstr += "                          CASE ";
            sqlstr += "                             WHEN MST.SR_SEND_YMD IS NOT NULL THEN 'Y' ";
            sqlstr += "                             WHEN MST.SR_SEND_YMD IS NULL THEN 'N' ";
            sqlstr += "                             ELSE 'N' ";
            sqlstr += "                          END ";
            sqlstr += "                             AS SR_STATUS, ";
            sqlstr += "                          MST.INV_RECV_YMD, ";
            sqlstr += "                          CASE ";
            sqlstr += "                             WHEN MST.INV_RECV_YMD IS NOT NULL THEN 'Y' ";
            sqlstr += "                             WHEN MST.INV_RECV_YMD IS NULL THEN 'N' ";
            sqlstr += "                             ELSE 'N' ";
            sqlstr += "                          END ";
            sqlstr += "                             AS INV_STATUS, ";
            sqlstr += "                          CNTR.LOC_GATE_IN_YMD, ";
            sqlstr += "                          CNTR.LOC_GATE_IN_HM, ";
            sqlstr += "                          CASE ";
            sqlstr += "                             WHEN CNTR.LOC_GATE_IN_YMD IS NOT NULL THEN 'Y' ";
            sqlstr += "                             WHEN CNTR.LOC_GATE_IN_YMD IS NULL THEN 'N' ";
            sqlstr += "                             ELSE 'N' ";
            sqlstr += "                          END ";
            sqlstr += "                             AS CNTR_STATUS ";
            sqlstr += "                     FROM    BIG_NEXN_MST MST ";
            sqlstr += "                          LEFT OUTER JOIN ";
            sqlstr += "                             BIG_NEXN_CNTR CNTR ";
            sqlstr += "                          ON MST.HBL_NO = CNTR.HBL_NO ";
            sqlstr += "                    WHERE 1 = 1 ";
            sqlstr += "                          AND MST.LINE_BKG_NO = '" + dr["LINE_BKG_NO"] + "' ";
            sqlstr += "                 ORDER BY BK_NO,BK_SEND_YMD ASC) A) TOTAL ";
            sqlstr += " WHERE PAGE = '" + dr["PAGE"] + "' ";

            return sqlstr;
        }

    }
}
