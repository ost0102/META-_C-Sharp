using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YJIT.Utils;

namespace ELVIS_META_COMMON.Query
{
    class Freight_Query
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
        /// 운임 평균 정보 가져오는 쿼리 (과거년)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_GetFreLineBeforeData(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "   SELECT A.DATE_YYYY, ";
            sqlstr += "          A.DATE_MM, ";
            sqlstr += "          SUM (A.DATA_CNT) AS DATA_CNT, ";
            sqlstr += "          SUM (A.UNIT_PRC_SUM) AS UNIT_PRC_SUM, ";
            sqlstr += "          TRUNC (SUM (A.UNIT_PRC_SUM) / SUM (A.DATA_CNT)) AS UNIT_AVG ";
            sqlstr += "     FROM BIG_CUST_FRT_MST A ";
            sqlstr += "    WHERE 1 = 1 ";
            sqlstr += "    AND A.DATE_YYYY = '"+ dr["DATE_YYYY_PAST"].ToString() + "' ";

            if (dr["POL_CD"].ToString() != "")
            {
                sqlstr += "    AND A.POL_CD = '"+ dr["POL_CD"].ToString() + "' ";
            }

            if (dr["POD_CD"].ToString() != "")
            {
                sqlstr += "    AND A.POD_CD = '" + dr["POD_CD"].ToString() + "' ";
            }

            if(dr["CNTR_TYPE"].ToString() != "")
            {
                if (dr["CNTR_TYPE"].ToString() == "20")
                {
                    sqlstr += "    AND A.PKG_UNIT LIKE '2%' ";
                }
                else if (dr["CNTR_TYPE"].ToString() == "40")
                {
                    sqlstr += "    AND A.PKG_UNIT LIKE '4%' ";
                }
                else if (dr["CNTR_TYPE"].ToString() == "기타")
                {
                    sqlstr += "    AND A.PKG_UNIT NOT LIKE '2%' AND A.PKG_UNIT NOT LIKE '4%' ";
                }
            }
            
            sqlstr += " GROUP BY A.DATE_YYYY, A.DATE_MM ";
            sqlstr += " ORDER BY A.DATE_YYYY ASC, A.DATE_MM ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 운임 평균 정보 가져오는 쿼리 (현재년)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_GetFreLineNowData(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "   SELECT A.DATE_YYYY, ";
            sqlstr += "          A.DATE_MM, ";
            sqlstr += "          SUM (A.DATA_CNT) AS DATA_CNT, ";
            sqlstr += "          SUM (A.UNIT_PRC_SUM) AS UNIT_PRC_SUM, ";
            sqlstr += "          TRUNC (SUM (A.UNIT_PRC_SUM) / SUM (A.DATA_CNT)) AS UNIT_AVG ";
            sqlstr += "     FROM BIG_CUST_FRT_MST A ";
            sqlstr += "    WHERE 1 = 1 ";
            sqlstr += "    AND A.DATE_YYYY = '" + dr["DATE_YYYY"].ToString() + "' ";

            if (dr["POL_CD"].ToString() != "")
            {
                sqlstr += "    AND A.POL_CD = '" + dr["POL_CD"].ToString() + "' ";
            }

            if (dr["POD_CD"].ToString() != "")
            {
                sqlstr += "    AND A.POD_CD = '" + dr["POD_CD"].ToString() + "' ";
            }

            if (dr["CNTR_TYPE"].ToString() != "")
            {
                if (dr["CNTR_TYPE"].ToString() == "20")
                {
                    sqlstr += "    AND A.PKG_UNIT LIKE '2%' ";
                }
                else if (dr["CNTR_TYPE"].ToString() == "40")
                {
                    sqlstr += "    AND A.PKG_UNIT LIKE '4%' ";
                }
                else if (dr["CNTR_TYPE"].ToString() == "기타")
                {
                    sqlstr += "    AND A.PKG_UNIT NOT LIKE '2%' AND A.PKG_UNIT NOT LIKE '4%' ";
                }
            }

            sqlstr += " GROUP BY A.DATE_YYYY, A.DATE_MM ";
            sqlstr += " ORDER BY A.DATE_YYYY ASC, A.DATE_MM ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 현재년도 운임 거래처 정보 보여주기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_GetFreCustData(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " WITH W_BIG_CUST_FRT_MST ";
            sqlstr += " AS ";
            sqlstr += " ( ";
            sqlstr += "     SELECT A.CUST_CD AS OFFICE_CD, ";
            sqlstr += "         A.DATE_YYYY, ";
            sqlstr += "         A.REQ_SVC, ";
            sqlstr += "         A.POL_CD, ";
            sqlstr += "         A.POD_CD, ";
            sqlstr += "         TRUNC (SUM (A.UNIT_PRC_SUM) / SUM (A.DATA_CNT)) AS UNIT_AVG, ";
            sqlstr += "         TRUNC (SUM (A.TRANSIT_TIME_SUM) / SUM (A.DATA_CNT)) AS TIME_AVG ";
            sqlstr += "     FROM BIG_CUST_FRT_MST A ";
            sqlstr += "      WHERE 1 = 1 ";
            sqlstr += "         AND A.DATE_YYYY = '" + dr["DATE_YYYY"].ToString() + "' ";
            sqlstr += "         AND A.REQ_SVC = 'SEA' ";
            sqlstr += "         AND A.POL_CD = '" + dr["POL_CD"].ToString() + "' ";
            sqlstr += "         AND A.POD_CD = '" + dr["POD_CD"].ToString() + "' ";

            if (dr["CNTR_TYPE"].ToString() != "")
            {
                if (dr["CNTR_TYPE"].ToString() == "20")
                {
                    sqlstr += "    AND A.PKG_UNIT LIKE '2%' ";
                }
                else if (dr["CNTR_TYPE"].ToString() == "40")
                {
                    sqlstr += "    AND A.PKG_UNIT LIKE '4%' ";
                }
                else if (dr["CNTR_TYPE"].ToString() == "기타")
                {
                    sqlstr += "    AND A.PKG_UNIT NOT LIKE '2%' AND A.PKG_UNIT NOT LIKE '4%' ";
                }
            }

            sqlstr += "     GROUP BY A.CUST_CD, ";
            sqlstr += "         A.DATE_YYYY, ";
            sqlstr += "         A.REQ_SVC, ";
            sqlstr += "         A.POL_CD, ";
            sqlstr += "         A.POD_CD ";
            sqlstr += " ) ";
            sqlstr += " SELECT A.OFFICE_CD,  ";
            sqlstr += "     (SELECT IMG_PATH ";
            sqlstr += "       FROM BIG_MDM_LINE_IMG ";
            sqlstr += "      WHERE LINE_CD = A.OFFICE_CD) ";
            sqlstr += "       AS LINE_PATH, ";
            sqlstr += "       MAX(C.LOC_NM) AS CUST_NM, ";
            sqlstr += "      SUM(A.DATA_CNT) AS DATA_CNT, ";
            sqlstr += "      SUM(A.DATA_RTON) AS DATA_RTON, ";
            sqlstr += "      MAX(B.UNIT_AVG) AS UNIT_AVG ";
            sqlstr += " FROM BIG_DASHBOARD_OFFICE A ";
            sqlstr += "      INNER JOIN W_BIG_CUST_FRT_MST B ";
            sqlstr += "         ON A.OFFICE_CD = B.OFFICE_CD  ";
            sqlstr += "        AND A.DATE_YYYY = B.DATE_YYYY ";
            sqlstr += "        AND A.REQ_SVC = B.REQ_SVC ";
            sqlstr += "        AND A.POL_CD = B.POL_CD ";
            sqlstr += "        AND A.POD_CD = B.POD_CD ";
            sqlstr += "      INNER JOIN BIG_MDM_OFFICE_MST C ";
            sqlstr += "         ON A.OFFICE_CD = C.OFFICE_CD  ";
            sqlstr += " WHERE A.DATE_YYYY = '" + dr["DATE_YYYY"].ToString() + "' ";
            sqlstr += " AND A.REQ_SVC = 'SEA' ";
            sqlstr += " AND A.POL_CD = '" + dr["POL_CD"].ToString() + "' ";
            sqlstr += " AND A.POD_CD = '" + dr["POD_CD"].ToString() + "' ";
            sqlstr += " GROUP BY A.OFFICE_CD ";

            if (dr["RECOMEND"].ToString() == "PRICE")
            {
                sqlstr += " ORDER BY MAX(B.UNIT_AVG) DESC ";
            }
            else if (dr["RECOMEND"].ToString() == "TIME")
            {
                sqlstr += " ORDER BY MAX(B.TIME_AVG) DESC ";
            }

            return sqlstr;

            //sqlstr += "   SELECT A.OFFICE_CD, ";
            //sqlstr += "          (SELECT IMG_PATH FROM BIG_MDM_LINE_IMG WHERE LINE_CD = A.OFFICE_CD) AS LINE_PATH, ";
            //sqlstr += "          C.LOC_NM AS CUST_NM, ";
            //sqlstr += "          SUM (A.DATA_CNT) AS DATA_CNT, ";
            //sqlstr += "          SUM (A.DATA_RTON) AS DATA_RTON, ";
            //sqlstr += "          TRUNC (SUM (B.UNIT_PRC_SUM) / SUM (B.DATA_CNT)) AS UNIT_AVG, ";
            //sqlstr += "          TRUNC (SUM (B.TRANSIT_TIME_SUM) / SUM (B.DATA_CNT)) AS TIME_AVG ";
            //sqlstr += "     FROM BIG_DASHBOARD_OFFICE A ";
            //sqlstr += "          INNER JOIN BIG_CUST_FRT_MST B ";
            //sqlstr += "             ON A.OFFICE_CD = B.CUST_CD ";
            //sqlstr += "          INNER JOIN BIG_MDM_OFFICE_MST C ";
            //sqlstr += "             ON A.OFFICE_CD = C.OFFICE_CD ";
            //sqlstr += "    WHERE 1 = 1 ";
            //sqlstr += "    AND A.REQ_SVC = 'SEA' ";
            //sqlstr += "    AND B.DATE_YYYY = '" + dr["DATE_YYYY"].ToString() + "' ";
            //
            //if (dr["CNTR_TYPE"].ToString() != "")
            //{
            //    if (dr["CNTR_TYPE"].ToString() == "20")
            //    {
            //        sqlstr += "    AND B.PKG_UNIT LIKE '2%' ";
            //    }
            //    else if (dr["CNTR_TYPE"].ToString() == "40")
            //    {
            //        sqlstr += "    AND B.PKG_UNIT LIKE '4%' ";
            //    }
            //    else if (dr["CNTR_TYPE"].ToString() == "기타")
            //    {
            //        sqlstr += "    AND B.PKG_UNIT NOT LIKE '2%' AND B.PKG_UNIT NOT LIKE '4%' ";
            //    }
            //}
            //
            //if (dr["POL_CD"].ToString() != "")
            //{
            //    sqlstr += "    AND A.POL_CD = '" + dr["POL_CD"].ToString() + "' ";
            //}
            //
            //if (dr["POD_CD"].ToString() != "")
            //{
            //    sqlstr += "    AND A.POD_CD = '" + dr["POD_CD"].ToString() + "' ";
            //}
            //
            //sqlstr += " GROUP BY A.OFFICE_CD, C.LOC_NM ";
            //
            //if (dr["RECOMEND"].ToString() == "PRICE")
            //{
            //    sqlstr += " ORDER BY TRUNC (SUM (B.UNIT_PRC_SUM) / SUM (B.DATA_CNT)) DESC ";
            //}
            //else if (dr["RECOMEND"].ToString() == "TIME")
            //{
            //    sqlstr += " ORDER BY TRUNC (SUM (B.TRANSIT_TIME_SUM) / SUM (B.DATA_CNT)) DESC ";
            //}
            //
            //return sqlstr;
        }

        /// <summary>
        /// 해상운임 정보 - 레이어 오피스 정보 데이터 가져오기
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
        /// 해상운임 정보 거래처 디테일 데이터 가져오기 - 차트 (전체 년도 , 스케줄 월)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_fnGetCustCircleChart(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT  ";
            sqlstr += " DATE_YYYY,  ";
            sqlstr += " OFFICE_CD,  ";
            sqlstr += " REQ_SVC,  ";
            sqlstr += " SUM (DATA_CNT) TOT_DATA_CNT,  ";
            sqlstr += " SUM (DATA_RTON) TOT_DATA_RTON, ";
            sqlstr += " (SELECT SUM(DATA_CNT) AS DATA_CNT FROM BIG_DASHBOARD_OFFICE WHERE DATE_YYYY = '" + dr["DATE_YYYY"] + "' ";

            if (dr["POL_CD"].ToString() != "")
            {
                sqlstr += "        AND POL_CD = '" + dr["POL_CD"] + "' ";
            }
            if (dr["POD_CD"].ToString() != "")
            {
                sqlstr += "        AND POD_CD = '" + dr["POD_CD"] + "' ";
            }

            sqlstr += " ) AS YEAR_DATA_CNT, ";

            sqlstr += " (SELECT SUM(DATA_RTON)   FROM BIG_DASHBOARD_OFFICE WHERE DATE_YYYY = '" + dr["DATE_YYYY"] + "' ";

            if (dr["POL_CD"].ToString() != "")
            {
                sqlstr += "        AND POL_CD = '" + dr["POL_CD"] + "' ";
            }
            if (dr["POD_CD"].ToString() != "")
            {
                sqlstr += "        AND POD_CD = '" + dr["POD_CD"] + "' ";
            }

            sqlstr += " ) AS YEAR_DATA_RTON ";

            sqlstr += "   FROM BIG_DASHBOARD_OFFICE ";
            sqlstr += "  WHERE     1 = 1 ";
            sqlstr += "        AND DATE_YYYY = '" + dr["DATE_YYYY"] + "' ";            
            sqlstr += "        AND OFFICE_CD = '" + dr["OFFICE_CD"] + "' ";
            sqlstr += "        AND REQ_SVC = '" + dr["REQ_SVC"] + "' ";

            if (dr["POL_CD"].ToString() != "")
            {
                sqlstr += "        AND POL_CD = '" + dr["POL_CD"] + "' ";
            }

            if (dr["POD_CD"].ToString() != "")
            {
                sqlstr += "        AND POD_CD = '" + dr["POD_CD"] + "' ";
            }

            sqlstr += " GROUP BY OFFICE_CD , REQ_SVC , DATE_YYYY";

            return sqlstr;
        }

        /// <summary>
        /// 해상운임 정보 거래처 디테일 데이터 가져오기 - 차트 (작년)
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
            //sqlstr += "        AND DATE_YYYY || DATE_MM BETWEEN TO_CHAR(ADD_MONTHS('" + dr["DATE_YYYY_PAST"] + "" + dr["DATE_MM"] + "01',-11),'YYYYMM') AND '" + dr["DATE_YYYY_PAST"] + "" + dr["DATE_MM"] + "' ";
            sqlstr += "        AND REQ_SVC = '" + dr["REQ_SVC"] + "' ";
            sqlstr += "        AND OFFICE_CD = '" + dr["OFFICE_CD"] + "'  ";
                        
            if (dr["POL_CD"].ToString() != "")
            {
                sqlstr += "        AND POL_CD = '" + dr["POL_CD"] + "' ";
            }

            if (dr["POD_CD"].ToString() != "")
            {
                sqlstr += "        AND POD_CD = '" + dr["POD_CD"] + "' ";
            }

            sqlstr += " GROUP BY DATE_YYYY, DATE_MM, OFFICE_CD ";
            sqlstr += " ORDER BY DATE_YYYY ASC, DATE_MM ASC, OFFICE_CD ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 해상운임 정보 거래처 디테일 데이터 가져오기 - 차트 (올해 데이터)
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

            if (dr["POL_CD"].ToString() != "")
            {

                sqlstr += "        AND POL_CD = '" + dr["POL_CD"] + "' ";
            }
            if (dr["POD_CD"].ToString() != "")
            {
                sqlstr += "        AND POD_CD = '" + dr["POD_CD"] + "' ";
            }

            //sqlstr += "    WHERE 1 = 1 AND DATE_YYYY = '" + dr["DATE_YYYY"] + "' AND REQ_SVC = '" + dr["REQ_SVC"] + "'AND OFFICE_CD = '" + dr["OFFICE_CD"] + "' ";
            //sqlstr += "        AND EX_IM_TYPE = '" + dr["EX_IM_TYPE"] + "' ";
            //sqlstr += "        AND POL_CD = '" + dr["POL_CD"] + "' ";
            //sqlstr += "        AND POD_CD = '" + dr["POD_CD"] + "' ";
            sqlstr += " GROUP BY DATE_YYYY, DATE_MM, OFFICE_CD ";
            sqlstr += " ORDER BY DATE_YYYY ASC, DATE_MM ASC, OFFICE_CD ASC ";

            return sqlstr;
        }

    }
}
