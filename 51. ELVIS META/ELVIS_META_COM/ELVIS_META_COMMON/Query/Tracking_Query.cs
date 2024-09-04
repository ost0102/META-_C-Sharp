using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ELVIS_META_COMMON.Query
{
    class Tracking_Query
    {
        string sqlstr = "";

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
            sqlstr += "         , (SELECT ETD FROM BIG_FMS_HBL_MST WHERE HBL_NO =  B.HBL_NO) AS ETD_YMD";
            sqlstr += "         , (SELECT ETA FROM BIG_FMS_HBL_MST WHERE HBL_NO =  B.HBL_NO) AS ETA_YMD";
            sqlstr += "         , (SELECT CASE WHEN POL_CD LIKE 'KR%' THEN 'E' WHEN POL_CD NOT LIKE 'KR%' THEN 'I' END FROM BIG_FMS_HBL_MST WHERE HBL_NO = A.HBL_NO) AS EX_IM_TYPE";
            sqlstr += " FROM BIG_FMS_TRK_DTL A ";
            sqlstr += " LEFT OUTER JOIN BIG_FMS_TRK_MST B ON A.HBL_NO = B.HBL_NO ";
            sqlstr += " LEFT OUTER JOIN FINALDS C ON A.HBL_NO = C.HBL_NO ";
            sqlstr += " WHERE (A.HBL_NO = '" + dr["BL_NO"].ToString() + "' OR A.CNTR_NO = '" + dr["BL_NO"].ToString() + "' OR A.HBL_NO = (SELECT MAX(HBL_NO) FROM BIG_FMS_HBL_MST WHERE MBL_NO = '" + dr["BL_NO"].ToString() + "' )) AND A.EVENT_CD in ('EMT','CNI','CIG','LOD','POL','ARR','ICC','CYA','CYD','EMR')";
            sqlstr += " ORDER BY A.SEQ";

            return sqlstr;
        }

        public string fnGetVslData_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT HBL.HBL_NO, ";
            sqlstr += "        HBL.MBL_NO, ";
            sqlstr += "        HBL.VSL, ";
            sqlstr += "        HBL.VOY, ";
            sqlstr += "        HBL.LINE_CD, ";
            sqlstr += "        (SELECT IMG_PATH FROM BIG_MDM_LINE_IMG WHERE LINE_CD = HBL.LINE_CD) AS LINE_PATH, ";
            sqlstr += "        HBL.LINE_NM, ";
            sqlstr += "        CARR.CARR_NM, ";
            sqlstr += "        VSL.VSL_MMSI, ";
            sqlstr += "        VSL.VSL_IMO ";
            sqlstr += "   FROM BIG_FMS_HBL_MST HBL ";
            sqlstr += "        LEFT OUTER JOIN BIG_MDM_CARR_MST CARR ";
            sqlstr += "           ON HBL.LINE_CD = CARR.CARR_CD ";
            sqlstr += "        LEFT OUTER JOIN BIG_MDM_VSL_MST VSL ";
            sqlstr += "           ON REPLACE (HBL.VSL, ' ', '') = VSL.VSL_NM ";
            sqlstr += "  WHERE 1 = 1  ";
            sqlstr += "  AND HBL_NO = '" + dr["BL_NO"].ToString() + "' ";

            return sqlstr;
        }

        public string fnGetHBLNO(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT TOTAL.*, ";
            sqlstr += "        (SELECT CASE WHEN POL_CD LIKE 'KR%' THEN 'E' ELSE 'I' END ";
            sqlstr += "           FROM BIG_FMS_HBL_MST ";
            sqlstr += "          WHERE HBL_NO = TOTAL.HBL_NO) ";
            sqlstr += "           AS EX_IM_TYPE ";
            sqlstr += "   FROM (  SELECT MAX(A.HBL_NO) AS HBL_NO ";
            sqlstr += "             FROM BIG_FMS_TRK_DTL A ";
            sqlstr += "            WHERE (   HBL_NO = '" + dr["BL_NO"].ToString() + "' ";
            sqlstr += "                   OR CNTR_NO = '" + dr["BL_NO"].ToString() + "' ";
            sqlstr += "                   OR HBL_NO = (SELECT MAX (HBL_NO) ";
            sqlstr += "                                  FROM BIG_FMS_HBL_MST ";
            sqlstr += "                                 WHERE MBL_NO = '" + dr["BL_NO"].ToString() + "')) ";
            sqlstr += "         GROUP BY A.HBL_NO) TOTAL ";

            return sqlstr;
        }

        public string fnTracking_Query(string strHBL_NO , string strEX_IM_TYPE)
        {
            sqlstr = "";

            sqlstr += "WITH FINALDS AS (	";
            sqlstr += "       SELECT HBL_NO";
            sqlstr += "                , CNTR_NO";
            sqlstr += "                ,  MAX(CASE WHEN ACT_YMD IS NOT NULL AND ACT_HM IS NOT NULL THEN SEQ ELSE 1 END) AS SEQ";
            sqlstr += "                ,  MAX(A.SEQ) AS MAX_SEQ";
            sqlstr += "          FROM BIG_FMS_TRK_DTL A";
            sqlstr += "          WHERE (HBL_NO = '" + strHBL_NO + "' OR CNTR_NO = '" + strHBL_NO + "' OR HBL_NO = (SELECT MAX(HBL_NO) FROM BIG_FMS_HBL_MST WHERE MBL_NO = '" + strHBL_NO + "'))";
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
            sqlstr += "         , (SELECT ETD FROM BIG_FMS_HBL_MST WHERE HBL_NO =  B.HBL_NO) AS ETD_YMD";
            sqlstr += "         , (SELECT ETA FROM BIG_FMS_HBL_MST WHERE HBL_NO =  B.HBL_NO) AS ETA_YMD";
            sqlstr += "         , (SELECT CASE WHEN POL_CD LIKE 'KR%' THEN 'E' WHEN POL_CD NOT LIKE 'KR%' THEN 'I' END FROM BIG_FMS_HBL_MST WHERE HBL_NO = A.HBL_NO) AS EX_IM_TYPE";
            sqlstr += " FROM BIG_FMS_TRK_DTL A ";
            sqlstr += " LEFT OUTER JOIN BIG_FMS_TRK_MST B ON A.HBL_NO = B.HBL_NO ";
            sqlstr += " LEFT OUTER JOIN FINALDS C ON A.HBL_NO = C.HBL_NO ";
            sqlstr += " WHERE (A.HBL_NO = '" + strHBL_NO + "' OR A.CNTR_NO = '" + strHBL_NO + "' OR A.HBL_NO = (SELECT MAX(HBL_NO) FROM BIG_FMS_HBL_MST WHERE MBL_NO = '" + strHBL_NO + "' )) ";

            if(strEX_IM_TYPE == "E")
            {
                sqlstr += " AND A.EVENT_CD in ('EMT','CIG','LOD','POL','ARR')";
            }
            else if(strEX_IM_TYPE == "I")
            {
                sqlstr += " AND A.EVENT_CD in ('CNI','POL','ARR','ICC','CYA','CYD')";                
            }else
            {
                sqlstr += " AND A.EVENT_CD in ('EMT','CNI','CIG','LOD','POL','ARR','ICC','CYA','CYD','EMR')";
            }

            sqlstr += " ORDER BY A.SEQ";

            return sqlstr;
        }

    }
}
