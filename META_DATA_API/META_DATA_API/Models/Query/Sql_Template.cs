using System;
using System.Data;

namespace META_DATA_API.Models.Query
{
    public static class Sql_Template
    {
        private static string sSql = "";
        private static bool rtnBool = false;
        private static DataTable rtnDt = null;

        // 템플릿을 등록한다
        public static bool TemplateUpload(string MngtNo, string Crn, string TmpltName, string TmpltType, string Rmk, string FileName, string FilePath, string FileExt, string UsrId)
        {
            if (!string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(Crn) && !string.IsNullOrEmpty(TmpltName) && !string.IsNullOrEmpty(TmpltType)
                && !string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(FilePath) && !string.IsNullOrEmpty(FileExt))
            {
                string docId = String.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), Crn, (99 * (new Random().Next(8) + 2))).ToString();

                sSql = "";
                sSql += "INSERT INTO META_TMPLT_MST ( ";
                sSql += "       TMPLT_ID ";
                sSql += "     , MNGT_NO ";
                sSql += "     , CRN ";
                sSql += "     , TMPLT_NM ";
                sSql += "     , TMPLT_TYPE ";
                sSql += "     , FILE_NM ";
                sSql += "     , FILE_PATH ";
                sSql += "     , FILE_EXT ";
                if (!string.IsNullOrEmpty(Rmk)) sSql += ", RMK ";
                sSql += "     , INS_USR ";
                sSql += "     , INS_YMD ";
                sSql += "     , INS_HM ";
                sSql += "     , UPD_USR ";
                sSql += "     , UPD_YMD ";
                sSql += "     , UPD_HM) ";

                sSql += "VALUES( ";
                sSql += "       '" + docId + "' ";
                sSql += "     , '" + MngtNo + "' ";
                sSql += "     , '" + Crn + "' ";
                sSql += "     , '" + TmpltName + "' ";
                sSql += "     , '" + TmpltType + "' ";
                sSql += "     , '" + FileName + "' ";
                sSql += "     , '" + FilePath + "' ";
                sSql += "     , '" + FileExt + "' ";
                if (!string.IsNullOrEmpty(Rmk)) sSql += ", '" + Rmk + "' ";
                sSql += "     , '" + UsrId + "' ";
                sSql += "     , UFN_DATE_FORMAT('DATE') ";
                sSql += "     , UFN_DATE_FORMAT('TIME') ";
                sSql += "     , '" + UsrId + "' ";
                sSql += "     , UFN_DATE_FORMAT('DATE') ";
                sSql += "     , UFN_DATE_FORMAT('TIME')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }

        //템플릿 리스트를 조회 한다
        public static DataTable GetTemplateList(string Crn, string ApvYn, string FmYmd, string ToYmd, string TmpltType, string TmpltNm)
        {
            if (!string.IsNullOrEmpty(Crn) || !string.IsNullOrEmpty(ApvYn) || !string.IsNullOrEmpty(FmYmd) || !string.IsNullOrEmpty(ToYmd)
                || !string.IsNullOrEmpty(TmpltType) || !string.IsNullOrEmpty(TmpltNm))
            {
                sSql = "";
                sSql += " SELECT FTM.TMPLT_ID ";
                sSql += "      , FTM.TMPLT_NM ";
                sSql += "      , FTM.TMPLT_TYPE ";
                sSql += "      , FTM.FILE_PATH || '/' || FTM.FILE_NM || FTM.FILE_EXT AS FILE_PATH ";
                sSql += "      , FTM.FILE_NM || FTM.FILE_EXT AS FILE_NAME ";
                sSql += "      , FTM.APV_YN ";
                sSql += "      , FUM.USR_NM ";
                sSql += "      , FTM.INS_YMD ";
                sSql += "      , FTM.RMK ";
                sSql += "      , FTM.NC_OCR_ID ";
                sSql += "      , FUM.USR_ID ";
                sSql += "      , FTM.CRN ";
                sSql += "      , FUM.LOC_NM ";
                sSql += "   FROM META_TMPLT_MST FTM ";
                sSql += "   LEFT OUTER JOIN META_USR_MST FUM ";
                sSql += "     ON FTM.MNGT_NO = FUM.MNGT_NO ";
                sSql += "  WHERE 1=1 ";
                if (!string.IsNullOrEmpty(Crn)) sSql += " AND FTM.CRN = '" + Crn + "' ";
                if (!string.IsNullOrEmpty(ApvYn)) sSql += " AND FTM.APV_YN = '" + ApvYn + "' ";
                if (!string.IsNullOrEmpty(FmYmd)) sSql += " AND FTM.INS_YMD >= '" + FmYmd + "' ";
                if (!string.IsNullOrEmpty(ToYmd)) sSql += " AND FTM.INS_YMD <= '" + ToYmd + "' ";
                if (!string.IsNullOrEmpty(TmpltType)) sSql += " AND FTM.TMPLT_TYPE = '" + TmpltType + "' ";
                if (!string.IsNullOrEmpty(TmpltNm)) sSql += " AND FTM.TMPLT_NM LIKE '%" + TmpltNm + "%' ";
                sSql += "  ORDER BY FTM.INS_YMD||FTM.INS_HM DESC";

            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        //템플릿 파일경로를 조회 한다
        public static DataTable GetTemplatePath(string TmpltId)
        {
            if (!string.IsNullOrEmpty(TmpltId))
            {
                sSql = "";
                sSql += " SELECT TMPLT_NM ";
                sSql += "      , FILE_PATH ";
                sSql += "      , FILE_NM || FILE_EXT AS FILE_NAME ";
                sSql += "   FROM META_TMPLT_MST ";
                sSql += "  WHERE 1=1 ";
                sSql += "    AND TMPLT_ID = '" + TmpltId + "' ";
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        //템플릿 승인여부 상태 값을 수정한다
        public static bool SetTmpltApvStatus(string TmpltId, string ApvYn, string NcOcrId)
        {
            if (!string.IsNullOrEmpty(TmpltId) && !string.IsNullOrEmpty(ApvYn))
            {
                sSql = "";
                sSql += "UPDATE META_TMPLT_MST ";
                sSql += "   SET UPD_USR = 'SYSTEM' ";
                sSql += "     , UPD_YMD = UFN_DATE_FORMAT('DATE') ";
                sSql += "     , UPD_HM = UFN_DATE_FORMAT('TIME') ";
                sSql += "     , APV_YN = '" + ApvYn + "' ";
                if (ApvYn == "Y" && !string.IsNullOrEmpty(NcOcrId)) sSql += " , NC_OCR_ID = '" + NcOcrId + "' ";
                else sSql += " , NC_OCR_ID = '' ";
                sSql += " WHERE TMPLT_ID = '" + TmpltId + "'";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }
    }
}