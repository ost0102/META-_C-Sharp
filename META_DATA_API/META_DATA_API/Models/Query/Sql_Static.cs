using System;
using System.Data;

namespace META_DATA_API.Models.Query
{
    public static class Sql_Static
    {
        private static string sSql = "";
        private static DataSet rtnDs = null;
        private static DataTable rtnDt = null;

        //지역별(국가) 물동량 자료를 찾는다
        public static DataSet GetCtryVolume(string DateYyyy, string DateMm, string ExImType, string PortCd)
        {
            if (!string.IsNullOrEmpty(DateYyyy) && !string.IsNullOrEmpty(DateYyyy) && !string.IsNullOrEmpty(ExImType))
            {
                rtnDs = new DataSet();
                sSql = "";
                sSql += " SELECT MAX(POL.MAP_LAT) AS POL_LAT ";
                sSql += "      , MAX(POL.MAP_LNG) AS POL_LNG ";
                sSql += "      , MAX(POD.MAP_LAT) AS POD_LAT ";
                sSql += "      , MAX(POD.MAP_LNG) AS POD_LNG ";
                sSql += "      , MAX(MST.EX_IM_TYPE) AS EX_IM_TYPE ";
                if (ExImType == "E")
                {
                    sSql += "  , MST.POD_CD AS PORT";
                }
                else
                {
                    sSql += "  , MST.POL_CD AS PORT";
                }
                sSql += "   FROM META_VOL_STATIC MST ";
                sSql += "  INNER JOIN META_MDM_PORT_MST POL ON POL.PORT_CD = MST.POL_CD AND POL.MAP_LAT IS NOT NULL";
                sSql += "  INNER JOIN META_MDM_PORT_MST POD ON POD.PORT_CD = MST.POD_CD AND POD.MAP_LAT IS NOT NULL";
                sSql += "  WHERE 1 = 1 ";
                sSql += "    AND DATE_YYYY = '" + DateYyyy + "' ";
                sSql += "    AND DATE_MM  = '" + DateMm + "' ";
                sSql += "    AND EX_IM_TYPE = '" + ExImType + "' ";
                if (ExImType == "E")
                {
                    sSql += "GROUP BY POD_CD";
                }
                else
                {
                    sSql += "GROUP BY POL_CD";
                }

                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
                rtnDt.TableName = "PORT";
                rtnDs.Tables.Add(rtnDt);

                sSql = "";
                sSql = " SELECT REQ_SVC ";
                sSql += "     , TOT_DATA_CNT  ";
                sSql += "     , TOT_DATA_RTON  ";
                sSql += "     , PORT_NM  ";
                if (!string.IsNullOrEmpty(PortCd))
                {
                    sSql += " , PORT_DATA_CNT  ";
                    sSql += " , PORT_DATA_RTON  ";
                }
                else
                {
                    sSql += " , CASE WHEN PORT_DATA_CNT = 0 THEN TOT_DATA_CNT ELSE PORT_DATA_CNT END AS PORT_DATA_CNT";
                    sSql += " , CASE WHEN PORT_DATA_RTON = 0 THEN TOT_DATA_RTON ELSE PORT_DATA_RTON END AS PORT_DATA_RTON";
                }
                sSql += "  FROM (";
                sSql += " SELECT REQ_SVC ";
                sSql += "      , SUM(DATA_CNT) TOT_DATA_CNT ";
                sSql += "      , SUM(DATA_RTON) TOT_DATA_RTON ";
                if (ExImType == "E")
                {

                    sSql += "  , SUM(CASE WHEN POD_CD = '" + PortCd + "' THEN DATA_CNT ELSE 0 END) AS PORT_DATA_CNT ";
                    sSql += "  , SUM(CASE WHEN POD_CD = '" + PortCd + "' THEN DATA_RTON ELSE 0 END) AS PORT_DATA_RTON ";
                }
                else
                {

                    sSql += "  , SUM(CASE WHEN POL_CD = '" + PortCd + "' THEN DATA_CNT ELSE 0 END) AS PORT_DATA_CNT ";
                    sSql += "  , SUM(CASE WHEN POL_CD = '" + PortCd + "' THEN DATA_RTON ELSE 0 END) AS PORT_DATA_RTON ";
                }

                sSql += "      , (SELECT CASE WHEN MAX (PORT_NM) || ' , ' || MAX (PORT_CTRY_NM) = ' , ' ";
                sSql += "                     THEN 'ALL PORT' ";
                sSql += "                     ELSE MAX (PORT_NM) || ' , ' || MAX (PORT_CTRY_NM) END ";
                sSql += "           FROM META_MDM_PORT_MST ";
                sSql += "          WHERE PORT_CD = '" + PortCd + "') AS PORT_NM";
                sSql += "   FROM META_VOL_STATIC ";
                sSql += "  WHERE 1 = 1";
                sSql += "    AND DATE_YYYY = '" + DateYyyy + "' ";
                sSql += "    AND DATE_MM  = '" + DateMm + "' ";
                sSql += "    AND EX_IM_TYPE = '" + ExImType + "' ";
                sSql += "  GROUP BY REQ_SVC) ";
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            rtnDt.TableName = "TOTAL";
            rtnDs.Tables.Add(rtnDt);

            return rtnDs;
        }

        // 해상운임추이 자료를 찾는다
        public static DataTable GetFrtCharge(string DateYyyy, string PolCd, string PodCd, string CntrSize)
        {
            if (!string.IsNullOrEmpty(DateYyyy) && !string.IsNullOrEmpty(PolCd) && !string.IsNullOrEmpty(PodCd) && !string.IsNullOrEmpty(CntrSize))
            {                
                sSql = "";
                sSql += " SELECT DATE_YYYY ";
                sSql += "      , DATE_MM ";
                sSql += "      , POL_CD ";
                sSql += "      , POD_CD ";
                sSql += "      , AVG(UNIT_AVG) AS UNIT_AVG ";
                sSql += "   FROM RPA_FRT_CHARGE_INTGRT@DL_RPA ";
                sSql += "  WHERE 1=1 ";
                sSql += "    AND DATE_YYYY IN ('" + DateYyyy + "', '" + (Convert.ToInt32(DateYyyy)-1) + "') ";
                sSql += "    AND POL_CD = '" + PolCd + "' ";
                sSql += "    AND POD_CD = '" + PodCd + "' ";
                sSql += "    AND CNTR_SIZE IN ('" + CntrSize.Replace(",","','") + "') ";
                sSql += "  GROUP BY DATE_YYYY, DATE_MM, POL_CD, POD_CD ";
                sSql += "  ORDER BY DATE_YYYY, DATE_MM ";
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // 지역별/품목별 거래내역 자료를 찾는다
        public static DataTable GetItemAnalyze(string ItemNm)
        {
            if (!string.IsNullOrEmpty(ItemNm))
            {
                sSql = "";
                sSql += "SELECT MAX(DATE_YYYY) DATE_YYYY";
                sSql += "     , MAX(DATE_MM) DATE_MM";
                sSql += "  FROM RPA_ITEM_ANALYZE@DL_RPA";
                sSql += " WHERE ITEM_NM = '" + ItemNm + "' ";

                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);

                sSql = "";
                sSql += " SELECT FIA.DATE_YYYY ";
                sSql += "      , FIA.DATE_MM ";
                sSql += "      , FIA.ITEM_NM ";
                sSql += "      , FIA.PORT_CD ";
                sSql += "      , FIA.PORT_NM ";
                sSql += "      , MCC.SORT    ";
                sSql += "      , FIA.PERF_DATA ";
                sSql += "   FROM RPA_ITEM_ANALYZE @DL_RPA FIA ";
                sSql += "   LEFT OUTER JOIN MDM_COM_CODE MCC ";
                sSql += "     ON FIA.PORT_CD = MCC.COMN_CD ";
                sSql += "  WHERE 1 = 1 ";
                sSql += "    AND DATE_YYYY = '" + rtnDt.Rows[0]["DATE_YYYY"] + "' ";
                sSql += "    AND DATE_MM = '" + rtnDt.Rows[0]["DATE_MM"] + "' ";
                sSql += "    AND ITEM_NM = '" + ItemNm + "' ";
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }
    }
}