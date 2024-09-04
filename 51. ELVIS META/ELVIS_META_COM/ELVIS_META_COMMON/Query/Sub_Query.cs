using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ELVIS_META_COMMON.Query
{
    public class Sub_Query
    {

        string sqlstr;


        public string Query_GetPortData(DataRow dr)
        {
            sqlstr = " SELECT MAX(POL.MAP_LAT) AS POL_LAT ";
            sqlstr += "           , MAX(POL.MAP_LNG) AS POL_LNG ";
            sqlstr += "           , MAX(POD.MAP_LAT) AS POD_LAT ";
            sqlstr += "           , MAX(POD.MAP_LNG) AS POD_LNG ";
            sqlstr += "           , MAX(MST.EX_IM_TYPE) AS EX_IM_TYPE ";
            if (dr["EX_IM_TYPE"].ToString() == "E")
            {
                sqlstr += "     , MST.POD_CD AS PORT";
            }
            else
            {
                sqlstr += "     , MST.POL_CD AS PORT";
            }
            sqlstr += "    FROM BIG_DASHBOARD_MST MST ";
            sqlstr += "    INNER JOIN BIG_MDM_PORT_MST POL ON POL.PORT_CD = MST.POL_CD AND POL.MAP_LAT IS NOT NULL";
            sqlstr += "    INNER JOIN BIG_MDM_PORT_MST POD ON POD.PORT_CD = MST.POD_CD AND POD.MAP_LAT IS NOT NULL";
            sqlstr += "  WHERE 1 = 1 ";
            sqlstr += "     AND DATE_YYYY = '" + dr["DATE_YYYY"].ToString() +"' ";
            sqlstr += "     AND DATE_MM  = '" + dr["DATE_MM"].ToString() +"' ";
            sqlstr += "     AND EX_IM_TYPE = '" + dr["EX_IM_TYPE"].ToString() + "' ";
            if (dr["EX_IM_TYPE"].ToString() == "E")
            {
                sqlstr += "     GROUP BY POD_CD";
            }
            else
            {
                sqlstr += "     GROUP BY POL_CD";
            }

            return sqlstr;
        }

        public string Query_GetTotalData(DataRow dr)
        {
            sqlstr = " SELECT REQ_SVC ";
            sqlstr += "      , TOT_DATA_CNT  ";
            sqlstr += "      , TOT_DATA_RTON  ";
            sqlstr += "      , PORT_NM  ";
            if (dr["PORT_CD"].ToString() != "")
            {
                sqlstr += "      , PORT_DATA_CNT  ";
                sqlstr += "      , PORT_DATA_RTON  ";
            }
            else
            {
                sqlstr += "      , CASE WHEN PORT_DATA_CNT = 0 THEN TOT_DATA_CNT ELSE PORT_DATA_CNT END AS PORT_DATA_CNT";
                sqlstr += "      , CASE WHEN PORT_DATA_RTON = 0 THEN TOT_DATA_RTON ELSE PORT_DATA_RTON END AS PORT_DATA_RTON";
            }
            sqlstr += "  FROM (";
            sqlstr += " SELECT REQ_SVC , SUM(DATA_CNT) TOT_DATA_CNT, ";
            sqlstr += " SUM(DATA_RTON) TOT_DATA_RTON, ";
            if (dr["EX_IM_TYPE"].ToString() == "E")
            {

                sqlstr += " SUM(CASE WHEN POD_CD = '" + dr["PORT_CD"].ToString() + "' THEN DATA_CNT ELSE 0 END) AS PORT_DATA_CNT, ";
                sqlstr += " SUM(CASE WHEN POD_CD = '" + dr["PORT_CD"].ToString() + "' THEN DATA_RTON ELSE 0 END) AS PORT_DATA_RTON, ";
            }
            else
            {

                sqlstr += " SUM(CASE WHEN POL_CD = '" + dr["PORT_CD"].ToString() + "' THEN DATA_CNT ELSE 0 END) AS PORT_DATA_CNT, ";
                sqlstr += " SUM(CASE WHEN POL_CD = '" + dr["PORT_CD"].ToString() + "' THEN DATA_RTON ELSE 0 END) AS PORT_DATA_RTON, ";
            }

            sqlstr += " (SELECT CASE WHEN MAX (PORT_NM) || ' , ' || MAX (PORT_CTRY_NM) = ' , ' THEN 'ALL PORT' ELSE MAX (PORT_NM) || ' , ' || MAX (PORT_CTRY_NM) END FROM BIG_MDM_PORT_MST WHERE PORT_CD = '" + dr["PORT_CD"].ToString() + "') AS PORT_NM";
            sqlstr += " FROM BIG_DASHBOARD_MST MST";
            sqlstr += "    INNER JOIN BIG_MDM_PORT_MST POL ON POL.PORT_CD = MST.POL_CD AND POL.MAP_LAT IS NOT NULL";
            sqlstr += "    INNER JOIN BIG_MDM_PORT_MST POD ON POD.PORT_CD = MST.POD_CD AND POD.MAP_LAT IS NOT NULL";
            sqlstr += " WHERE 1 = 1";
            sqlstr += "     AND DATE_YYYY = '" + dr["DATE_YYYY"].ToString() + "' ";
            sqlstr += "     AND DATE_MM  = '" + dr["DATE_MM"].ToString() + "' ";
            sqlstr += "     AND EX_IM_TYPE = '" + dr["EX_IM_TYPE"].ToString() + "' ";
            sqlstr += " GROUP BY REQ_SVC ";
            sqlstr += " )";

            return sqlstr;
        }

        public string fnTracking_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "WITH FINALDS AS (	";
            sqlstr += "       SELECT HBL_NO";
            sqlstr += "                , CNTR_NO";
            sqlstr += "                ,  MAX(CASE WHEN ACT_YMD IS NOT NULL AND ACT_HM IS NOT NULL THEN SEQ ELSE 1 END) AS SEQ";
            sqlstr += "                ,  MAX(A.SEQ) AS MAX_SEQ";
            sqlstr += "          FROM BIG_FMS_TRK_DTL A";
            sqlstr += "          WHERE (HBL_NO = '" + dr["BL_NO"].ToString() + "' OR CNTR_NO = '" + dr["BL_NO"].ToString() + "' OR HBL_NO = (SELECT MAX(HBL_NO) FROM BIG_FMS_HBL_MST WHERE MBL_NO = '" + dr["BL_NO"].ToString() + "'))";
            sqlstr += "          GROUP BY HBL_NO , CNTR_NO)";
            sqlstr += "SELECT A.HBL_NO";
            sqlstr += "         ,  A.SEQ AS ROW_COUNT";
            sqlstr += "         ,  MAX_SEQ";
            sqlstr += "         ,  A.CNTR_NO";
            sqlstr += "         ,  A.EVENT_CD";
            sqlstr += "         ,  A.EST_LOC_NM";
            sqlstr += "         ,  A.EST_YMD";
            sqlstr += "         ,  A.EST_HM";
            sqlstr += "         ,  A.ACT_LOC_NM";
            sqlstr += "         ,  A.ACT_YMD";
            sqlstr += "         ,  A.ACT_HM";
            sqlstr += "         ,  ROWNUM";
            sqlstr += "         , (SELECT CD_NM FROM MDM_COM_CODE WHERE GRP_CD = 'B30' AND COMN_CD = A.EVENT_CD) AS EVENT_NM";
            sqlstr += "         , C.SEQ";
            sqlstr += "         , (SELECT VSL FROM BIG_FMS_HBL_MST WHERE HBL_NO =  B.HBL_NO) AS VSL";
            sqlstr += "         , (SELECT POL_CD FROM BIG_FMS_HBL_MST WHERE HBL_NO =  B.HBL_NO) AS POL_CD";
            sqlstr += "         , (SELECT POD_CD FROM BIG_FMS_HBL_MST WHERE HBL_NO =  B.HBL_NO) AS POD_CD";
            sqlstr += "         , SUBSTR((SELECT ETD FROM BIG_FMS_HBL_MST WHERE HBL_NO =  B.HBL_NO),0,4) AS ETD";
            sqlstr += "         , (SELECT CASE WHEN POL_CD LIKE 'KR%' THEN 'E' WHEN POL_CD NOT LIKE 'KR%' THEN 'I' END FROM BIG_FMS_HBL_MST WHERE HBL_NO = A.HBL_NO) AS EX_IM_TYPE";
            sqlstr += " FROM BIG_FMS_TRK_DTL A ";
            sqlstr += " LEFT OUTER JOIN BIG_FMS_TRK_MST B ON A.HBL_NO = B.HBL_NO ";
            sqlstr += " LEFT OUTER JOIN FINALDS C ON A.HBL_NO = C.HBL_NO ";
            sqlstr += " WHERE (A.HBL_NO = '" + dr["BL_NO"].ToString() + "' OR A.CNTR_NO = '" + dr["BL_NO"].ToString() + "' OR A.HBL_NO = (SELECT MAX(HBL_NO) FROM BIG_FMS_HBL_MST WHERE MBL_NO = '" + dr["BL_NO"].ToString() + "' )) AND A.EVENT_CD in ('EMT','CIG','LOD','POL','ARR','ICC','CYA','CYD','EMR')";
            sqlstr += " ORDER BY A.SEQ";

            return sqlstr;
        }
    }
}
