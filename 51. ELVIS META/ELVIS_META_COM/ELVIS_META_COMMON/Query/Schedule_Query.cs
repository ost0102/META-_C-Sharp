using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YJIT.Utils;

namespace ELVIS_META_COMMON.Query
{
    class Schedule_Query
    {
        string sqlstr;

        /// <summary>
        /// Port 정보 가져오는 쿼리
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_GetPortData(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT PORT_CD AS CODE , PORT_NM || ' , ' || PORT_CTRY_NM AS NAME ";
            sqlstr += "   FROM BIG_MDM_PORT_MST ";
            sqlstr += "  WHERE     1 = 1 ";
            sqlstr += "        AND PORT_CD IS NOT NULL ";
            sqlstr += "        AND ( ('" + dr["LOC_TYPE"].ToString() + "' IS NULL AND 1 = 1) OR ('" + dr["LOC_TYPE"].ToString() + "' IS NOT NULL AND PORT_TYPE = '" + dr["LOC_TYPE"].ToString() + "')) ";

            if (dr["LOC_CD"].ToString() != "")
            {
                sqlstr += "        AND (REPLACE (PORT_CD, ' ', '') LIKE '%" + dr["LOC_CD"].ToString() + "%' ";
                sqlstr += "             OR REPLACE (PORT_NM, ' ', '') LIKE '%" + dr["LOC_CD"].ToString() + "%') ";
            }          

            return sqlstr;
        }

        /// <summary>
        /// 스케줄 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_fnGetSchData(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT  ";
            sqlstr += " A.LINE_CD, ";
            sqlstr += "(SELECT IMG_PATH FROM BIG_MDM_LINE_IMG WHERE LINE_CD = A.LINE_CD) AS LINE_PATH,  ";
            sqlstr += " A.REQ_SVC,";
            sqlstr += " A.POL_CD, ";
            sqlstr += " (SELECT PORT_NM || ' , ' || PORT_CTRY_NM FROM BIG_MDM_PORT_MST WHERE PORT_CD = A.POL_CD) AS POL_NM, ";
            sqlstr += " A.POD_CD, ";
            sqlstr += " (SELECT PORT_NM || ' , ' || PORT_CTRY_NM FROM BIG_MDM_PORT_MST WHERE PORT_CD = A.POD_CD) AS POD_NM, ";
            sqlstr += " A.ETD, ";
            sqlstr += " A.ETA, ";
            sqlstr += " A.VSL, ";
            sqlstr += " A.VOY, ";
            sqlstr += " A.FCL_CNT, ";
            sqlstr += " A.LCL_CNT, ";
            sqlstr += " A.BULK_CNT, ";
            sqlstr += " A.CONSOL_CNT ";            
            sqlstr += " FROM BIG_PRM_SCH_MST A ";

            sqlstr += " WHERE 1=1  ";
            sqlstr += " AND A.REQ_SVC = '" + dr["REQ_SVC"].ToString() + "' ";
            sqlstr += " AND A.POL_CD = '" + dr["POL_CD"].ToString() + "' ";
            sqlstr += " AND A.POD_CD = '" + dr["POD_CD"].ToString() + "' ";
            sqlstr += " AND A.ETD >= '" + dr["ST_DATE"].ToString() + "' ";
            sqlstr += " AND A.ETD <= '" + dr["END_DATE"].ToString() + "' ";

            if (dr["CNTR_TYPE"].ToString() != "")
            {
                if (dr["CNTR_TYPE"].ToString() == "FCL")
                {
                    sqlstr += " A.FCL_YN = 'Y' ";
                }
                else if (dr["CNTR_TYPE"].ToString() == "LCL")
                {
                    sqlstr += " A.LCL_YN = 'Y' ";
                }
                else if (dr["CNTR_TYPE"].ToString() == "CONSOL")
                {
                    sqlstr += " A.CONSOL_YN = 'Y'";
                }
            }

            return sqlstr;
        }

        /// <summary>
        /// 스케줄 디테일 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_fnGetSchDtlData(DataRow dr)
        {
            sqlstr = "";

            //상호명이 필요하여 들어가야됨.
            sqlstr += "   SELECT A.OFFICE_CD, ";
            sqlstr += "          (SELECT IMG_PATH FROM BIG_MDM_LINE_IMG WHERE LINE_CD = A.OFFICE_CD) AS LINE_PATH,  ";
            sqlstr += "          (SELECT OFFICE_NM FROM BIG_MDM_OFFICE_MST WHERE OFFICE_CD = A.OFFICE_CD) AS OFFICE_ENG_NM, ";
            sqlstr += "          (SELECT LOC_NM FROM BIG_MDM_OFFICE_MST WHERE OFFICE_CD = A.OFFICE_CD) AS OFFICE_KOR_NM, ";
            sqlstr += "          A.ETD, ";
            sqlstr += "          A.ETA, ";
            sqlstr += "          A.POL_CD, ";
            sqlstr += "          A.POD_CD, ";
            sqlstr += "          A.REQ_SVC, ";
            sqlstr += "          A.DOC_CLOSE_YMD, ";
            sqlstr += "          A.DOC_CLOSE_HM, ";
            sqlstr += "          CASE WHEN A.POL_CD LIKE 'KR%' THEN 'E' ELSE 'I' END AS EX_IM_TYPE, ";
            sqlstr += "          A.CARGO_CLOSE_YMD, ";
            sqlstr += "          A.CARGO_CLOSE_HM, ";
            sqlstr += "          A.SCH_PIC, ";
            sqlstr += "          A.RMK ";
            sqlstr += "     FROM BIG_PRM_SCH_DTL A ";
            sqlstr += "     WHERE 1=1  ";
            sqlstr += " AND A.REQ_SVC = '" + dr["REQ_SVC"].ToString() + "' ";
            sqlstr += " AND A.LINE_CD = '" + dr["LINE_CD"].ToString() + "' ";
            sqlstr += " AND A.POL_CD = '" + dr["POL_CD"].ToString() + "' ";
            sqlstr += " AND A.POD_CD = '" + dr["POD_CD"].ToString() + "' ";
            sqlstr += " AND A.ETD = '" + dr["ETD"].ToString() + "' ";
            sqlstr += " AND A.ETA = '" + dr["ETA"].ToString() + "' ";
            sqlstr += " AND A.VSL = '" + dr["VSL"].ToString() + "' ";
            sqlstr += " AND A.VOY = '" + dr["VOY"].ToString() + "' ";
            sqlstr += " AND A.CNTR_TYPE = '" + dr["CNTR_TYPE"].ToString() + "' ";

            sqlstr += " ORDER BY A.OFFICE_CD ASC ";            

            return sqlstr;
        }

        /// <summary>
        /// 스케줄 거래처 디테일 데이터 가져오기 - 업체 정보
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_fnGetCustData(DataRow dr)
        {
            sqlstr = "";

            //이메일 , 전화번호 추가 필요
            sqlstr += " SELECT OFFICE_CD, ";
            sqlstr += "        (SELECT IMG_PATH FROM BIG_MDM_LINE_IMG WHERE LINE_CD = OFFICE_CD) AS LINE_PATH,  ";
            sqlstr += "        OFFICE_NM AS OFFICE_ENG_NM, ";
            sqlstr += "        LOC_NM AS OFFICE_KOR_NM, ";
            sqlstr += "        LOC_ADDR, ";
            sqlstr += "        ENG_ADDR, ";
            sqlstr += "        EMAIL, ";
            sqlstr += "        TEL_NO AS HP_NO ";
            sqlstr += "   FROM BIG_MDM_OFFICE_MST ";
            sqlstr += "  WHERE OFFICE_CD = '" + dr["OFFICE_CD"] + "' ";

            return sqlstr;
        }

        /// <summary>
        /// 스케줄 거래처 디테일 데이터 가져오기 - 차트 (전체 년도 , 스케줄 월)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_fnGetCustCircleChart(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT  ";
            sqlstr += " DATE_YYYY,  ";
            sqlstr += " DATE_MM,  ";
            sqlstr += " OFFICE_CD,  ";
            sqlstr += " REQ_SVC,  ";
            sqlstr += " POL_CD,  ";
            sqlstr += " POD_CD,  ";
            sqlstr += " EX_IM_TYPE,  ";
            sqlstr += " SUM (DATA_CNT) TOT_DATA_CNT,  ";
            sqlstr += " SUM (DATA_RTON) TOT_DATA_RTON, ";
            sqlstr += " (SELECT SUM(DATA_CNT) AS DATA_CNT FROM BIG_DASHBOARD_OFFICE WHERE DATE_YYYY = '"+dr["DATE_YYYY"]+ "' AND OFFICE_CD = '" + dr["OFFICE_CD"] + "' AND EX_IM_TYPE = '" + dr["EX_IM_TYPE"] + "' AND POL_CD = '" + dr["POL_CD"] + "' AND POD_CD = '" + dr["POD_CD"] + "' ) AS YEAR_DATA_CNT, ";
            sqlstr += " (SELECT SUM(DATA_RTON)   FROM BIG_DASHBOARD_OFFICE WHERE DATE_YYYY = '" + dr["DATE_YYYY"] + "' AND OFFICE_CD = '" + dr["OFFICE_CD"] + "' AND EX_IM_TYPE = '" + dr["EX_IM_TYPE"] + "' AND POL_CD = '" + dr["POL_CD"] + "' AND POD_CD = '" + dr["POD_CD"] + "' ) AS YEAR_DATA_RTON ";
            sqlstr += "   FROM BIG_DASHBOARD_OFFICE ";
            sqlstr += "  WHERE     1 = 1 ";
            sqlstr += "        AND DATE_YYYY = '" + dr["DATE_YYYY"] + "' ";
            sqlstr += "        AND DATE_MM = '" + dr["DATE_MM"] + "' ";
            sqlstr += "        AND OFFICE_CD = '" + dr["OFFICE_CD"] + "' ";
            sqlstr += "        AND REQ_SVC = '" + dr["REQ_SVC"] + "' ";
            sqlstr += "        AND EX_IM_TYPE = '" + dr["EX_IM_TYPE"] + "' ";
            sqlstr += "        AND POL_CD = '" + dr["POL_CD"] + "' ";
            sqlstr += "        AND POD_CD = '" + dr["POD_CD"] + "' ";

            sqlstr += " GROUP BY OFFICE_CD , REQ_SVC , DATE_YYYY , DATE_MM , POL_CD , POD_CD , EX_IM_TYPE";

            return sqlstr;
        }

        /// <summary>
        /// 스케줄 거래처 디테일 데이터 가져오기 - 차트 (작년)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_fnGetCustLineChart_Before(DataRow dr)
        {
            sqlstr = "";

            //테스트
            sqlstr += "   SELECT OFFICE_CD, ";
            sqlstr += "          (SELECT OFFICE_NM FROM BIG_MDM_OFFICE_MST WHERE OFFICE_CD = A.OFFICE_CD) AS OFFICE_ENG_NM, ";
            sqlstr += "          (SELECT LOC_NM FROM BIG_MDM_OFFICE_MST WHERE OFFICE_CD = A.OFFICE_CD) AS OFFICE_KOR_NM, ";
            sqlstr += "          DATE_YYYY, ";
            sqlstr += "          DATE_MM, ";
            sqlstr += "          DATE_YYYY || DATE_MM AS DATE_YYYYMM , ";
            sqlstr += "          SUM (DATA_CNT) TOT_DATA_CNT, ";
            sqlstr += "          SUM (DATA_RTON) TOT_DATA_RTON ";
            sqlstr += "     FROM BIG_DASHBOARD_OFFICE A ";
            //sqlstr += "    WHERE 1 = 1 AND DATE_YYYY = '" + dr["DATE_YYYY_PAST"] + "' AND REQ_SVC = '" + dr["REQ_SVC"] + "' AND OFFICE_CD = 'SHTC' ";
            sqlstr += "    WHERE 1 = 1   ";
            sqlstr += "        AND DATE_YYYY = '" + dr["DATE_YYYY_PAST"] + "' ";
            //sqlstr += "        AND DATE_YYYY || DATE_MM   BETWEEN TO_CHAR(ADD_MONTHS('"+ dr["DATE_YYYY_PAST"] + ""+ dr["DATE_MM"] + "01',-11),'YYYYMM') AND '"+ dr["DATE_YYYY_PAST"] + ""+ dr["DATE_MM"] + "' ";
            sqlstr += "        AND REQ_SVC = '" + dr["REQ_SVC"] + "' ";
            sqlstr += "        AND OFFICE_CD = '" + dr["OFFICE_CD"] + "'  ";
            sqlstr += "        AND EX_IM_TYPE = '" + dr["EX_IM_TYPE"] + "' ";
            sqlstr += "        AND POL_CD = '" + dr["POL_CD"] + "' ";
            sqlstr += "        AND POD_CD = '" + dr["POD_CD"] + "' ";
            sqlstr += " GROUP BY DATE_YYYY, DATE_MM, OFFICE_CD ";
            sqlstr += " ORDER BY DATE_YYYY ASC, DATE_MM ASC, OFFICE_CD ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 스케줄 거래처 디테일 데이터 가져오기 - 차트 (올해 데이터)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_fnGetCustLineChart_Now(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "   SELECT OFFICE_CD, ";
            sqlstr += "          (SELECT OFFICE_NM FROM BIG_MDM_OFFICE_MST WHERE OFFICE_CD = B.OFFICE_CD) AS OFFICE_ENG_NM, ";
            sqlstr += "          (SELECT LOC_NM FROM BIG_MDM_OFFICE_MST WHERE OFFICE_CD = B.OFFICE_CD) AS OFFICE_KOR_NM, ";
            sqlstr += "          DATE_YYYY, ";
            sqlstr += "          DATE_MM, ";
            sqlstr += "          DATE_YYYY || DATE_MM AS DATE_YYYYMM , ";
            sqlstr += "          SUM (DATA_CNT) TOT_DATA_CNT, ";
            sqlstr += "          SUM (DATA_RTON) TOT_DATA_RTON ";
            sqlstr += "     FROM BIG_DASHBOARD_OFFICE B ";
            sqlstr += "    WHERE 1 = 1   ";
            sqlstr += "        AND DATE_YYYY = '" + dr["DATE_YYYY"] + "' ";
            //sqlstr += "        AND DATE_YYYY || DATE_MM   BETWEEN TO_CHAR(ADD_MONTHS('" + dr["DATE_YYYY"] + "" + dr["DATE_MM"] + "01',-11),'YYYYMM') AND '" + dr["DATE_YYYY"] + "" + dr["DATE_MM"] + "' ";
            sqlstr += "        AND REQ_SVC = '" + dr["REQ_SVC"] + "' ";
            sqlstr += "        AND OFFICE_CD = '" + dr["OFFICE_CD"] + "'  ";
            sqlstr += "        AND EX_IM_TYPE = '" + dr["EX_IM_TYPE"] + "' ";
            sqlstr += "        AND POL_CD = '" + dr["POL_CD"] + "' ";
            sqlstr += "        AND POD_CD = '" + dr["POD_CD"] + "' ";
            //sqlstr += "    WHERE 1 = 1 AND DATE_YYYY = '" + dr["DATE_YYYY"] + "' AND REQ_SVC = '" + dr["REQ_SVC"] + "'AND OFFICE_CD = '" + dr["OFFICE_CD"] + "' ";
            //sqlstr += "        AND EX_IM_TYPE = '" + dr["EX_IM_TYPE"] + "' ";
            //sqlstr += "        AND POL_CD = '" + dr["POL_CD"] + "' ";
            //sqlstr += "        AND POD_CD = '" + dr["POD_CD"] + "' ";
            sqlstr += " GROUP BY DATE_YYYY, DATE_MM, OFFICE_CD ";
            sqlstr += " ORDER BY DATE_YYYY ASC, DATE_MM ASC, OFFICE_CD ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 케리어 정보 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_fnGetCarrData(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT CARR.CARR_CD, ";
            sqlstr += "        CARR.CARR_NM ";
            sqlstr += "   FROM BIG_MDM_CARR_MST CARR ";
            sqlstr += "  WHERE CARR.CARR_CD = '" + dr["LINE_CD"] + "' ";

            return sqlstr;
        }

        /// <summary>
        /// 뱃셀 데이터 정보 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_fnGetVSLData(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT VSL.VSL_MMSI ";
            sqlstr += "            ,VSL.VSL_IMO ";
            sqlstr += "            ,VSL.VSL_ORG_NM ";
            sqlstr += "   FROM BIG_MDM_VSL_MST VSL ";
            sqlstr += "   WHERE VSL.VSL_NM = REPLACE('" + dr["VSL"] + "',' ','') ";

            return sqlstr;
        }

        /// <summary>
        /// 라인 이미지가 있는지 체크하는 로직
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_fnGetLineImgPath(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT IMG_PATH AS LINE_PATH ";
            sqlstr += "   FROM BIG_MDM_LINE_IMG ";
            sqlstr += "    WHERE LINE_CD = '" + dr["LINE_CD"] + "' ";

            return sqlstr;
        }
    }
}
