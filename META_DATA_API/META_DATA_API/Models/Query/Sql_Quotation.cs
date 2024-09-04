using System;
using System.Data;

namespace META_DATA_API.Models.Query
{
    public static class Sql_Quotation
    {
        private static string sSql = "";
        private static bool rtnBool = false;
        private static DataTable rtnDt = null;


        // 추천 견적 자료를 조회한다
        public static DataTable GetRecommendQuot(string Crn)
        {
            if (!string.IsNullOrEmpty(Crn))
            {
                sSql = "";
                sSql += " SELECT FRQ.CRN ";
                sSql += "      , FRQ.POL_CD ";
                sSql += "      , POL.LOC_NM AS POL_NM ";
                sSql += "      , FRQ.POD_CD ";
                sSql += "      , POD.LOC_NM AS POD_NM ";
                sSql += "      , FRQ.EX_IM_TYPE ";
                sSql += "      , FRQ.REQ_SVC ";
                sSql += "      , FRQ.HBL_NO ";
                sSql += "      , FHS.MAIN_ITEM_NM AS ITEM_NM ";
                sSql += "   FROM META_RECOMMEND_QUOT FRQ ";
                sSql += "   LEFT OUTER JOIN META_HBL_STATUS FHS ";
                sSql += "     ON FRQ.HBL_NO = FHS.HBL_NO ";
                sSql += "     AND FHS.CRN = FRQ.CRN ";
                sSql += "   LEFT OUTER JOIN MDM_PORT_MST POL ";
                sSql += "     ON FRQ.POL_CD = POL.LOC_CD ";
                sSql += "   LEFT OUTER JOIN MDM_PORT_MST POD ";
                sSql += "     ON FRQ.POD_CD = POD.LOC_CD ";
                sSql += "  WHERE FRQ.CRN = '" + Crn + "' ";
                sSql += "  ORDER BY PRE_DATE ";
            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // 견적요청 기본정보를 저장한다
        public static bool SetQuotationMain(string QuotNo, string Crn, string UsrId, DataRow ParamInfo)
        {
            if (!string.IsNullOrEmpty(QuotNo) && !string.IsNullOrEmpty(ParamInfo["CRN"].ToString()) && !string.IsNullOrEmpty(ParamInfo["REQ_SVC"].ToString()) 
                && !string.IsNullOrEmpty(ParamInfo["POL_CD"].ToString()) && !string.IsNullOrEmpty(ParamInfo["POD_CD"].ToString()))
            {
                sSql = "";
                sSql += "INSERT INTO META_QUOT_MST ( ";
                sSql += "       QUOT_NO ";
                sSql += "     , CRN ";
                sSql += "     , REQ_SVC ";
                sSql += "     , POL_CD ";
                sSql += "     , POL_NM ";
                sSql += "     , POD_CD ";
                sSql += "     , POD_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["ITEM_NM"].ToString())) sSql += ", ITEM_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["ETD"].ToString())) sSql += ", ETD ";
                if (!string.IsNullOrEmpty(ParamInfo["ETA"].ToString())) sSql += ", ETA ";
                if (!string.IsNullOrEmpty(ParamInfo["OFFICE_NM"].ToString())) sSql += ", OFFICE_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["PIC_CD"].ToString())) sSql += ", PIC_CD ";
                if (!string.IsNullOrEmpty(ParamInfo["PIC_NM"].ToString())) sSql += ", PIC_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["PIC_TEL"].ToString())) sSql += ", PIC_TEL ";
                if (!string.IsNullOrEmpty(ParamInfo["PIC_MAIL"].ToString())) sSql += ", PIC_MAIL ";
                if (!string.IsNullOrEmpty(ParamInfo["RMK"].ToString())) sSql += ", RMK ";
                //if (!string.IsNullOrEmpty(ParamInfo["REQ_FWD_CNT"].ToString())) sSql += ", REQ_FWD_CNT ";
                sSql += "     , INS_USR ";
                sSql += "     , INS_YMD ";
                sSql += "     , INS_HM) ";

                sSql += "VALUES( ";
                sSql += "       '" + QuotNo + "' ";
                sSql += "     , '" + Crn + "' ";
                sSql += "     , '" + ParamInfo["REQ_SVC"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["POL_CD"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["POL_NM"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["POD_CD"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["POD_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["ITEM_NM"].ToString())) sSql += ", '" + ParamInfo["ITEM_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["ETD"].ToString())) sSql += ", '" + ParamInfo["ETD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["ETA"].ToString())) sSql += ", '" + ParamInfo["ETA"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["OFFICE_NM"].ToString())) sSql += ", '" + ParamInfo["OFFICE_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["PIC_CD"].ToString())) sSql += ", '" + ParamInfo["PIC_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["PIC_NM"].ToString())) sSql += ", '" + ParamInfo["PIC_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["PIC_TEL"].ToString())) sSql += ", '" + ParamInfo["PIC_TEL"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["PIC_MAIL"].ToString())) sSql += ", '" + ParamInfo["PIC_MAIL"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["RMK"].ToString())) sSql += ", '" + ParamInfo["RMK"].ToString() + "' ";
                //if (!string.IsNullOrEmpty(ParamInfo["REQ_FWD_CNT"].ToString())) sSql += ", '" + ParamInfo["REQ_FWD_CNT"].ToString() + "' ";
                sSql += "     , '" + UsrId + "' ";
                sSql += "     , UFN_DATE_FORMAT('DATE') ";
                sSql += "     , UFN_DATE_FORMAT('TIME')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }

        // 견적요청 화물정보를 저장한다
        public static bool SetQuotationDim(string QuotNo, string UsrId, DataTable ParamInfo)
        {
            for (int i = 0; i < ParamInfo.Rows.Count; i++)
            {
                DataRow dr = ParamInfo.Rows[i];

                if (!string.IsNullOrEmpty(QuotNo) && !string.IsNullOrEmpty(UsrId) && 
                    (!string.IsNullOrEmpty(dr["PKG"].ToString()) || !string.IsNullOrEmpty(dr["GRS_WGT"].ToString()) || !string.IsNullOrEmpty(dr["VOL_WGT"].ToString())
                    || !string.IsNullOrEmpty(dr["WDT"].ToString()) || !string.IsNullOrEmpty(dr["VERT"].ToString()) || !string.IsNullOrEmpty(dr["HGT"].ToString())))
                {
                    sSql = "";
                    sSql += "INSERT INTO META_QUOT_DIM ( ";
                    sSql += "       QUOT_NO ";
                    sSql += "     , SEQ ";
                    if (!string.IsNullOrEmpty(dr["PKG"].ToString())) sSql += ", PKG ";
                    if (!string.IsNullOrEmpty(dr["GRS_WGT"].ToString())) sSql += ", GRS_WGT ";
                    if (!string.IsNullOrEmpty(dr["VOL_WGT"].ToString())) sSql += ", VOL_WGT ";
                    if (!string.IsNullOrEmpty(dr["WDT"].ToString())) sSql += ", WDT ";
                    if (!string.IsNullOrEmpty(dr["VERT"].ToString())) sSql += ", VERT ";
                    if (!string.IsNullOrEmpty(dr["HGT"].ToString())) sSql += ", HGT ";
                    sSql += "     , INS_USR ";
                    sSql += "     , INS_YMD ";
                    sSql += "     , INS_HM ";
                    sSql += "     , UPD_USR ";
                    sSql += "     , UPD_YMD ";
                    sSql += "     , UPD_HM) ";

                    sSql += "VALUES( ";
                    sSql += "      '" + QuotNo + "' ";
                    sSql += "     , " + dr["SEQ"].ToString() + " ";
                    if (!string.IsNullOrEmpty(dr["PKG"].ToString())) sSql += ", " + dr["PKG"].ToString() + " ";
                    if (!string.IsNullOrEmpty(dr["GRS_WGT"].ToString())) sSql += ", " + dr["GRS_WGT"].ToString() + " ";
                    if (!string.IsNullOrEmpty(dr["VOL_WGT"].ToString())) sSql += ", " + dr["VOL_WGT"].ToString() + " ";
                    if (!string.IsNullOrEmpty(dr["WDT"].ToString())) sSql += ", " + dr["WDT"].ToString() + " ";
                    if (!string.IsNullOrEmpty(dr["VERT"].ToString())) sSql += ", " + dr["VERT"].ToString() + " ";
                    if (!string.IsNullOrEmpty(dr["HGT"].ToString())) sSql += ", " + dr["HGT"].ToString() + " ";
                    sSql += "     , '" + UsrId + "' ";
                    sSql += "     , UFN_DATE_FORMAT('DATE') ";
                    sSql += "     , UFN_DATE_FORMAT('TIME') ";
                    sSql += "     , '" + UsrId + "' ";
                    sSql += "     , UFN_DATE_FORMAT('DATE') ";
                    sSql += "     , UFN_DATE_FORMAT('TIME')) ";

                    int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                    if (rtnStatus >= 0) rtnBool = true;
                }
            }
            return rtnBool;
        }

        // 견적요청 문서정보를 저장한다
        public static bool SetQuotationDoc(string MngtNo, string Crn, string UsrId, string fileName, int fileSize, string filePath, string fileExt, string QuotNo, string DocType)
        {
            if (!string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(fileExt) && 
                !string.IsNullOrEmpty(UsrId) && !string.IsNullOrEmpty(DocType))
            {
                sSql = "";
                sSql += "INSERT INTO META_DOC_MST ( ";
                sSql += "  MNGT_NO ";
                sSql += ", CRN ";
                sSql += ", FILE_NM ";
                sSql += ", SVR_FILE_NM ";
                sSql += ", FILE_SIZE ";
                sSql += ", FILE_EXT ";
                sSql += ", FILE_PATH ";
                sSql += ", DOC_TYPE ";
                sSql += ", ORG_MNGT_NO ";
                sSql += ", INS_USR ";
                sSql += ", INS_YMD ";
                sSql += ", INS_HM ";
                sSql += ", UPD_USR ";
                sSql += ", UPD_YMD ";
                sSql += ", UPD_HM) ";

                sSql += "VALUES( ";
                sSql += "  '" + MngtNo + "' ";
                sSql += ", '" + Crn + "' ";
                sSql += ", '" + fileName + "' ";
                sSql += ", '" + fileName + "' ";
                sSql += ", '" + fileSize + "' ";
                sSql += ", '" + fileExt + "' ";
                sSql += ", '" + filePath + "' ";
                sSql += ", '" + DocType + "' ";
                sSql += ", '" + QuotNo + "' ";
                sSql += ", '" + UsrId + "' ";
                sSql += ", UFN_DATE_FORMAT('DATE') ";
                sSql += ", UFN_DATE_FORMAT('TIME') ";
                sSql += ", '" + UsrId + "' ";
                sSql += ", UFN_DATE_FORMAT('DATE') ";
                sSql += ", UFN_DATE_FORMAT('TIME')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }

            return rtnBool;
        }

        // OCR 판독 요청 내역을 저장한다
        public static bool SetQuotationOcrMst(string OcrNo, string DocMngtNo, string QuotNo, string UsrId, DataRow ParamInfo)
        {
            if (!string.IsNullOrEmpty(OcrNo) && !string.IsNullOrEmpty(DocMngtNo) && !string.IsNullOrEmpty(QuotNo) && !string.IsNullOrEmpty(UsrId) && ParamInfo != null)
            {                
                sSql = "";
                sSql += "INSERT INTO META_OCR_MST ( ";
                sSql += "  MNGT_NO ";
                sSql += ", CRN ";
                sSql += ", QUOT_NO ";
                sSql += ", TMPLT_ID ";
                sSql += ", TMPLT_TYPE ";
                sSql += ", NC_OCR_ID ";
                sSql += ", OCR_RESULT ";
                sSql += ", OCR_MESSAGE ";
                sSql += ", INS_USR ";
                sSql += ", INS_YMD ";
                sSql += ", INS_HM) ";

                sSql += "VALUES( ";
                sSql += "  '" + OcrNo + "' ";
                sSql += ", '" + ParamInfo["CRN"].ToString() + "' ";
                sSql += ", '" + QuotNo + "' ";
                sSql += ", '" + ParamInfo["TMPLT_ID"].ToString() + "' ";
                sSql += ", '" + ParamInfo["TMPLT_TYPE"].ToString() + "' ";
                sSql += ", '" + ParamInfo["NC_OCR_ID"].ToString() + "' ";
                sSql += ", '" + ParamInfo["OCR_RESULT"].ToString() + "' ";
                sSql += ", '" + ParamInfo["OCR_MESSAGE"].ToString() + "' ";
                sSql += ", '" + UsrId + "' ";
                sSql += ", UFN_DATE_FORMAT('DATE') ";
                sSql += ", UFN_DATE_FORMAT('TIME')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0)
                {
                    sSql = "";
                    sSql += "UPDATE META_DOC_MST ";
                    sSql += "   SET OCR_NO = '" + OcrNo + "' ";
                    sSql += " WHERE 1=1 ";
                    sSql += "   AND CRN = '" + ParamInfo["CRN"].ToString() + "' ";
                    sSql += "   AND MNGT_NO = '" + DocMngtNo + "' ";

                    rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                    if (rtnStatus >= 0) rtnBool = true;
                }
            }

            return rtnBool;
        }

        // OCR 판독 결과 자료를 저장한다
        public static bool SetQuotationOcrDtl(string MngtNo, string UsrId, DataRow[] ParamInfo)
        {
            if (!string.IsNullOrEmpty(MngtNo) && ParamInfo != null)
            {
                for (int i = 0; i < ParamInfo.Length; i++)
                {
                    sSql = "";
                    sSql += "INSERT INTO META_OCR_DTL ( ";
                    sSql += "  MNGT_NO ";
                    sSql += ", SEQ ";
                    sSql += ", OCR_NAME ";
                    sSql += ", OCR_TEXT ";
                    sSql += ", OCR_TOP ";
                    sSql += ", OCR_LEFT ";
                    sSql += ", OCR_WIDTH ";
                    sSql += ", OCR_HEIGHT ";
                    sSql += ", OCR_TYPE ";
                    sSql += ", OCR_CONFIDENCE ";
                    sSql += ", INS_USR ";
                    sSql += ", INS_YMD ";
                    sSql += ", INS_HM) ";

                    sSql += "VALUES( ";
                    sSql += "  '" + MngtNo + "' ";
                    sSql += ", " + ParamInfo[i]["FIELD_SEQ"].ToString() + " ";
                    sSql += ", '" + ParamInfo[i]["OCR_NAME"].ToString() + "' ";
                    sSql += ", '" + ParamInfo[i]["OCR_TEXT"].ToString() + "' ";
                    sSql += ", '" + ParamInfo[i]["OCR_TOP"].ToString() + "' ";
                    sSql += ", '" + ParamInfo[i]["OCR_LEFT"].ToString() + "' ";
                    sSql += ", '" + ParamInfo[i]["OCR_WIDTH"].ToString() + "' ";
                    sSql += ", '" + ParamInfo[i]["OCR_HEIGHT"].ToString() + "' ";
                    sSql += ", '" + ParamInfo[i]["OCR_TYPE"].ToString() + "' ";
                    sSql += ", '" + ParamInfo[i]["OCR_CONFIDENCE"].ToString() + "' ";
                    sSql += ", '" + UsrId + "' ";
                    sSql += ", UFN_DATE_FORMAT('DATE') ";
                    sSql += ", UFN_DATE_FORMAT('TIME')) ";

                    int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                    if (rtnStatus >= 0) rtnBool = true;
                }                
            }

            return rtnBool;
        }

        // OCR 판독 결과 자료를 조회한다
        public static DataTable GetOcrData(string OcrNo)
        {
            if (!string.IsNullOrEmpty(OcrNo))
            {
                sSql = "";
                sSql += " SELECT * ";
                sSql += "   FROM META_OCR_MST FOM ";
                sSql += "   LEFT OUTER JOIN META_OCR_DTL FOD ";
                sSql += "     ON FOM.MNGT_NO = FOD.MNGT_NO ";
                sSql += "  WHERE FOM.MNGT_NO = '" + OcrNo + "' ";
            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // 견적요청 기본 정보를 ELVIS 로 전송한다.
        public static bool SendQuotMainToElvis(string QuotNo, string UsrId, string FwdCrn)
        {
            if (!string.IsNullOrEmpty(QuotNo) && !string.IsNullOrEmpty(UsrId) && !string.IsNullOrEmpty(FwdCrn))
            {
                sSql = "";
                sSql += "SELECT QUOT_NO";
                sSql += "     , CRN ";
                sSql += "     , STATUS ";
                sSql += "     , REQ_SVC ";
                sSql += "     , POL_CD ";
                sSql += "     , POL_NM ";
                sSql += "     , POD_CD ";
                sSql += "     , POD_NM ";
                sSql += "     , ETD ";
                sSql += "     , ETA ";
                sSql += "     , ITEM_NM ";
                sSql += "     , OFFICE_NM ";
                sSql += "     , PIC_CD ";
                sSql += "     , PIC_NM ";
                sSql += "     , PIC_TEL ";
                sSql += "     , PIC_MAIL ";
                sSql += "     , RMK ";
                sSql += "  FROM META_QUOT_MST ";
                sSql += " WHERE QUOT_NO = '" + QuotNo + "' ";

                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);

                if (rtnDt.Rows.Count > 0)
                {
                        sSql = "";
                        sSql += "UPDATE META_QUOT_MST ";
                        sSql += "   SET REQ_FWD_CNT = REQ_FWD_CNT + 1 ";
                        sSql += " WHERE QUOT_NO = '" + QuotNo + "' ";

                        int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                        if (rtnStatus >= 0) rtnBool = true;


                        sSql = "";

                        sSql += "INSERT INTO META_QUOT_REQ ( ";
                        sSql += "       MNGT_NO";
                        sSql += "     , QUOT_NO";
                        sSql += "     , CRN ";
                        sSql += "     , FWD_CRN ";
                        sSql += "     , INS_USR ";
                        sSql += "     , INS_YMD ";
                        sSql += "     , INS_HM) ";

                        sSql += "VALUES ( ";
                        sSql += "       '" + String.Format("REQ{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), (99 * (new Random().Next(8) + 2))).ToString() + "' ";
                        sSql += "     , '" + rtnDt.Rows[0]["QUOT_NO"].ToString() + "' ";
                        sSql += "     , '" + rtnDt.Rows[0]["CRN"].ToString() + "' ";
                        sSql += "     , '" + FwdCrn + "' ";
                        sSql += "     , '" + UsrId + "' ";
                        sSql += "     , UFN_DATE_FORMAT('DATE') ";
                        sSql += "     , UFN_DATE_FORMAT('TIME')) ";

                        rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                        if (rtnStatus >= 0) rtnBool = true;

                        sSql = "";
                        sSql += "INSERT INTO META_QUOT_CRN ( ";
                        sSql += "      QUOT_NO";
                        sSql += "     , CRN )";

                        sSql += "VALUES ( ";
                        sSql += "      '" + QuotNo + "' ";
                        sSql += "     , '" + FwdCrn + "')";

                    rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                        if (rtnStatus >= 0) rtnBool = true;
                }                
            }

            return rtnBool;
        }

        // 견적요청 화물 정보를 ELVIS 로 전송한다.
        public static bool SendQuotDimToElvis(string ConnStr, string QuotNo, string UsrId)
        {
            if (!string.IsNullOrEmpty(ConnStr) && !string.IsNullOrEmpty(QuotNo))
            {
                sSql = "";
                sSql += "SELECT QUOT_NO";
                sSql += "     , SEQ ";
                sSql += "     , PKG ";
                sSql += "     , GRS_WGT ";
                sSql += "     , VOL_WGT ";
                sSql += "     , WDT ";
                sSql += "     , VERT ";
                sSql += "     , HGT ";
                sSql += "  FROM META_QUOT_DIM ";
                sSql += " WHERE QUOT_NO = '" + QuotNo + "' ";

                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);

                if (rtnDt.Rows.Count > 0)
                {
                    for (int i = 0; i < rtnDt.Rows.Count; i++)
                    {
                        DataRow dr = rtnDt.Rows[i];

                        if (!string.IsNullOrEmpty(dr["PKG"].ToString()) || !string.IsNullOrEmpty(dr["GRS_WGT"].ToString()) || !string.IsNullOrEmpty(dr["VOL_WGT"].ToString()) 
                            || !string.IsNullOrEmpty(dr["WDT"].ToString()) || !string.IsNullOrEmpty(dr["VERT"].ToString()) || !string.IsNullOrEmpty(dr["HGT"].ToString()))
                        {
                            sSql = "";
                            sSql += "INSERT INTO META_QUOT_DIM ( ";
                            sSql += "  QUOT_NO ";
                            sSql += ", SEQ ";
                            if (!string.IsNullOrEmpty(dr["PKG"].ToString())) sSql += ", PKG ";
                            if (!string.IsNullOrEmpty(dr["GRS_WGT"].ToString())) sSql += ", GRS_WGT ";
                            if (!string.IsNullOrEmpty(dr["VOL_WGT"].ToString())) sSql += ", VOL_WGT ";
                            if (!string.IsNullOrEmpty(dr["WDT"].ToString())) sSql += ", WDT ";
                            if (!string.IsNullOrEmpty(dr["VERT"].ToString())) sSql += ", VERT ";
                            if (!string.IsNullOrEmpty(dr["HGT"].ToString())) sSql += ", HGT ";
                            sSql += ", INS_USR ";
                            sSql += ", INS_YMD ";
                            sSql += ", INS_HM ";
                            sSql += ", UPD_USR ";
                            sSql += ", UPD_YMD ";
                            sSql += ", UPD_HM) ";

                            sSql += "VALUES( ";
                            sSql += "  '" + QuotNo + "' ";
                            sSql += ", " + dr["SEQ"].ToString() + " ";
                            if (!string.IsNullOrEmpty(dr["PKG"].ToString())) sSql += ", " + dr["PKG"].ToString() + " ";
                            if (!string.IsNullOrEmpty(dr["GRS_WGT"].ToString())) sSql += ", " + dr["GRS_WGT"].ToString() + " ";
                            if (!string.IsNullOrEmpty(dr["VOL_WGT"].ToString())) sSql += ", " + dr["VOL_WGT"].ToString() + " ";
                            if (!string.IsNullOrEmpty(dr["WDT"].ToString())) sSql += ", " + dr["WDT"].ToString() + " ";
                            if (!string.IsNullOrEmpty(dr["VERT"].ToString())) sSql += ", " + dr["VERT"].ToString() + " ";
                            if (!string.IsNullOrEmpty(dr["HGT"].ToString())) sSql += ", " + dr["HGT"].ToString() + " ";
                            sSql += ", '" + UsrId + "' ";
                            sSql += ", UFN_DATE_FORMAT('DATE') ";
                            sSql += ", UFN_DATE_FORMAT('TIME') ";
                            sSql += ", '" + UsrId + "' ";
                            sSql += ", UFN_DATE_FORMAT('DATE') ";
                            sSql += ", UFN_DATE_FORMAT('TIME')) ";

                            int rtnStatus = _DataHelper.ExecuteNonQuery(ConnStr, sSql, CommandType.Text);
                            if (rtnStatus >= 0) rtnBool = true;
                        }
                    }
                }
            }
                                    
            return rtnBool;
        }

        // 견적요청 기본 정보를 ELVIS 로 전송한다.
        public static bool SendQuotationOcrMst(string ConnStr, string QuotNo)
        {
            if (!string.IsNullOrEmpty(ConnStr) && !string.IsNullOrEmpty(QuotNo))
            {
                sSql = "";
                sSql += "SELECT MNGT_NO";
                sSql += "     , CRN ";
                sSql += "     , QUOT_NO ";
                sSql += "     , TMPLT_ID ";
                sSql += "     , TMPLT_TYPE ";
                sSql += "     , NC_OCR_ID ";
                sSql += "     , OCR_RESULT ";
                sSql += "     , OCR_MESSAGE ";
                sSql += "     , FILE_NM ";
                sSql += "     , INS_USR ";
                sSql += "  FROM META_OCR_MST ";
                sSql += " WHERE QUOT_NO = '" + QuotNo + "' ";

                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);

                if (rtnDt.Rows.Count > 0)
                {
                    for(int i = 0; i< rtnDt.Rows.Count;i++)
                    {
                        sSql = "";
                        sSql += "INSERT INTO META_OCR_MST ( ";
                        sSql += "       MNGT_NO";
                        sSql += "     , CRN ";
                        sSql += "     , QUOT_NO ";
                        sSql += "     , TMPLT_ID ";
                        sSql += "     , TMPLT_TYPE ";
                        sSql += "     , NC_OCR_ID ";
                        sSql += "     , OCR_RESULT ";
                        sSql += "     , OCR_MESSAGE ";
                        sSql += "     , INS_USR ";
                        sSql += "     , INS_YMD ";
                        sSql += "     , INS_HM) ";

                        sSql += "VALUES ( ";
                        sSql += "  '" + rtnDt.Rows[i]["MNGT_NO"].ToString() + "' ";
                        sSql += ", '" + rtnDt.Rows[i]["CRN"].ToString() + "' ";
                        sSql += ", '" + rtnDt.Rows[i]["QUOT_NO"].ToString() + "' ";
                        sSql += ", '" + rtnDt.Rows[i]["TMPLT_ID"].ToString() + "' ";
                        sSql += ", '" + rtnDt.Rows[i]["TMPLT_TYPE"].ToString() + "' ";
                        sSql += ", '" + rtnDt.Rows[i]["NC_OCR_ID"].ToString() + "' ";
                        sSql += ", '" + rtnDt.Rows[i]["OCR_RESULT"].ToString() + "' ";
                        sSql += ", '" + rtnDt.Rows[i]["OCR_MESSAGE"].ToString() + "' ";
                        sSql += ", '" + rtnDt.Rows[i]["INS_USR"].ToString() + "' ";
                        sSql += ", UFN_DATE_FORMAT('DATE') ";
                        sSql += ", UFN_DATE_FORMAT('TIME')) ";

                        int rtnStatus = _DataHelper.ExecuteNonQuery(ConnStr, sSql, CommandType.Text);
                        if (rtnStatus >= 0) rtnBool = true;
                    }
                }
            }

            return rtnBool;
        }

        // 견적요청 문서 정보를 ELVIS 로 전송한다.
        public static bool SendQuotDocToElvis(string ConnStr, DataRow ParamInfo)
        {
            if (ParamInfo != null)
            {
                sSql = "";
                sSql += "INSERT INTO META_DOC_MST ( ";
                sSql += "  MNGT_NO ";
                sSql += ", CRN ";
                sSql += ", FILE_NM ";
                sSql += ", SVR_FILE_NM ";
                sSql += ", FILE_SIZE ";
                sSql += ", FILE_EXT ";
                sSql += ", FILE_PATH ";
                sSql += ", DOC_TYPE ";
                sSql += ", ORG_MNGT_NO ";
                sSql += ", OCR_NO ";
                sSql += ", INS_USR ";
                sSql += ", INS_YMD ";
                sSql += ", INS_HM) ";

                sSql += "VALUES( ";
                sSql += "  '" + ParamInfo["MNGT_NO"].ToString() + "' ";
                sSql += ", '" + ParamInfo["CRN"].ToString() + "' ";
                sSql += ", '" + ParamInfo["FILE_NM"].ToString() + "' ";
                sSql += ", '" + ParamInfo["SVR_FILE_NM"].ToString() + "' ";
                sSql += ", '" + ParamInfo["FILE_SIZE"].ToString() + "' ";
                sSql += ", '" + ParamInfo["FILE_EXT"].ToString() + "' ";
                sSql += ", '" + ParamInfo["FILE_PATH"].ToString() + "' ";
                sSql += ", '" + ParamInfo["DOC_TYPE"].ToString() + "' ";
                sSql += ", '" + ParamInfo["ORG_MNGT_NO"].ToString() + "' ";
                sSql += ", '" + ParamInfo["OCR_NO"].ToString() + "' ";
                sSql += ", '" + ParamInfo["INS_USR"].ToString() + "' ";
                sSql += ", UFN_DATE_FORMAT('DATE') ";
                sSql += ", UFN_DATE_FORMAT('TIME')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(ConnStr, sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }

            return rtnBool;
        }

        // ELVIS 에서 전송한 견적 정보를 저장한다.
        public static bool SendQuotToElvisFriend(DataRow ParamInfo)
        {
            if (ParamInfo != null)
            {
                sSql = "";
                sSql += "INSERT INTO META_QUOT_DTL ( ";
                sSql += "  QUOT_NO ";
                sSql += ", MNGT_NO ";
                sSql += ", CRN ";
                sSql += ", VLDT_YMD ";
                sSql += ", REQ_SVC ";
                if (!string.IsNullOrEmpty(ParamInfo["CUST_CD"].ToString())) sSql += ", CUST_CD ";
                if (!string.IsNullOrEmpty(ParamInfo["CUST_NM"].ToString())) sSql += ", CUST_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["CUST_PIC_CD"].ToString())) sSql += ", CUST_PIC_CD ";
                if (!string.IsNullOrEmpty(ParamInfo["CUST_PIC_NM"].ToString())) sSql += ", CUST_PIC_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["CUST_PIC_TEL"].ToString())) sSql += ", CUST_PIC_TEL ";
                if (!string.IsNullOrEmpty(ParamInfo["CUST_PIC_EMAIL"].ToString())) sSql += ", CUST_PIC_EMAIL ";
                if (!string.IsNullOrEmpty(ParamInfo["FWD_OFFICE_NM"].ToString())) sSql += ", FWD_OFFICE_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["FWD_PIC_NM"].ToString())) sSql += ", FWD_PIC_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["FWD_PIC_TEL"].ToString())) sSql += ", FWD_PIC_TEL ";
                if (!string.IsNullOrEmpty(ParamInfo["FWD_PIC_EMAIL"].ToString())) sSql += ", FWD_PIC_EMAIL ";
                sSql += ", POL_CD ";
                sSql += ", POL_NM ";
                sSql += ", POD_CD ";
                sSql += ", POD_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["ETD"].ToString())) sSql += ", ETD ";
                if (!string.IsNullOrEmpty(ParamInfo["ETA"].ToString())) sSql += ", ETA ";
                if (!string.IsNullOrEmpty(ParamInfo["ETD_HM"].ToString())) sSql += ", ETD_HM ";
                if (!string.IsNullOrEmpty(ParamInfo["ETA_HM"].ToString())) sSql += ", ETA_HM ";
                if (!string.IsNullOrEmpty(ParamInfo["LINE_CD"].ToString())) sSql += ", LINE_CD ";
                if (!string.IsNullOrEmpty(ParamInfo["LINE_NM"].ToString())) sSql += ", LINE_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["VSL_CD"].ToString())) sSql += ", VSL_CD ";
                if (!string.IsNullOrEmpty(ParamInfo["VSL"].ToString())) sSql += ", VSL ";
                if (!string.IsNullOrEmpty(ParamInfo["VOY"].ToString())) sSql += ", VOY ";
                if (!string.IsNullOrEmpty(ParamInfo["ITEM_CD"].ToString())) sSql += ", ITEM_CD ";
                if (!string.IsNullOrEmpty(ParamInfo["ITEM_NM"].ToString())) sSql += ", ITEM_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["RMK"].ToString())) sSql += ", RMK ";
                if (!string.IsNullOrEmpty(ParamInfo["SHP_CD"].ToString())) sSql += ", SHP_CD ";
                if (!string.IsNullOrEmpty(ParamInfo["SHP_ADDR"].ToString())) sSql += ", SHP_ADDR ";
                if (!string.IsNullOrEmpty(ParamInfo["CNE_CD"].ToString())) sSql += ", CNE_CD ";
                if (!string.IsNullOrEmpty(ParamInfo["CNE_ADDR"].ToString())) sSql += ", CNE_ADDR ";
                if (!string.IsNullOrEmpty(ParamInfo["NFY_CD"].ToString())) sSql += ", NFY_CD ";
                if (!string.IsNullOrEmpty(ParamInfo["NFY_ADDR"].ToString())) sSql += ", NFY_ADDR ";
                if (!string.IsNullOrEmpty(ParamInfo["SALES_CD"].ToString())) sSql += ", SALES_CD ";
                if (!string.IsNullOrEmpty(ParamInfo["SALES_NM"].ToString())) sSql += ", SALES_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["OP_CD"].ToString())) sSql += ", OP_CD ";
                if (!string.IsNullOrEmpty(ParamInfo["OP_NM"].ToString())) sSql += ", OP_NM ";
                if (!string.IsNullOrEmpty(ParamInfo["REPORT_TITLE"].ToString())) sSql += ", REPORT_TITLE ";
                if (!string.IsNullOrEmpty(ParamInfo["ANN1"].ToString())) sSql += ", ANN1 ";
                if (!string.IsNullOrEmpty(ParamInfo["ANN2"].ToString())) sSql += ", ANN2 ";
                if (!string.IsNullOrEmpty(ParamInfo["CNTR_TYPE"].ToString())) sSql += ", CNTR_TYPE ";
                if (!string.IsNullOrEmpty(ParamInfo["PKG"].ToString())) sSql += ", PKG ";
                if (!string.IsNullOrEmpty(ParamInfo["GRS_WGT"].ToString())) sSql += ", GRS_WGT ";
                if (!string.IsNullOrEmpty(ParamInfo["MSRMT"].ToString())) sSql += ", MSRMT ";
                sSql += ", INS_USR ";
                sSql += ", INS_YMD ";
                sSql += ", INS_HM) ";

                sSql += "VALUES( ";
                sSql += "  '" + ParamInfo["QUOT_NO"].ToString() + "' ";
                sSql += ", '" + ParamInfo["MNGT_NO"].ToString() + "' ";
                sSql += ", '" + ParamInfo["CRN"].ToString() + "' ";
                sSql += ", '" + ParamInfo["VLDT_YMD"].ToString() + "' ";
                sSql += ", '" + ParamInfo["REQ_SVC"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["CUST_CD"].ToString())) sSql += ", '" + ParamInfo["CUST_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["CUST_NM"].ToString())) sSql += ", '" + ParamInfo["CUST_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["CUST_PIC_CD"].ToString())) sSql += ", '" + ParamInfo["CUST_PIC_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["CUST_PIC_NM"].ToString())) sSql += ", '" + ParamInfo["CUST_PIC_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["CUST_PIC_TEL"].ToString())) sSql += ", '" + ParamInfo["CUST_PIC_TEL"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["CUST_PIC_EMAIL"].ToString())) sSql += ", '" + ParamInfo["CUST_PIC_EMAIL"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["FWD_OFFICE_NM"].ToString())) sSql += ", '" + ParamInfo["FWD_OFFICE_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["FWD_PIC_NM"].ToString())) sSql += ", '" + ParamInfo["FWD_PIC_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["FWD_PIC_TEL"].ToString())) sSql += ", '" + ParamInfo["FWD_PIC_TEL"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["FWD_PIC_EMAIL"].ToString())) sSql += ", '" + ParamInfo["FWD_PIC_EMAIL"].ToString() + "' ";
                sSql += ", '" + ParamInfo["POL_CD"].ToString() + "' ";
                sSql += ", '" + ParamInfo["POL_NM"].ToString() + "' ";
                sSql += ", '" + ParamInfo["POD_CD"].ToString() + "' ";
                sSql += ", '" + ParamInfo["POD_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["ETD"].ToString())) sSql += ", '" + ParamInfo["ETD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["ETA"].ToString())) sSql += ", '" + ParamInfo["ETA"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["ETD_HM"].ToString())) sSql += ", '" + ParamInfo["ETD_HM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["ETA_HM"].ToString())) sSql += ", '" + ParamInfo["ETA_HM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["LINE_CD"].ToString())) sSql += ", '" + ParamInfo["LINE_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["LINE_NM"].ToString())) sSql += ", '" + ParamInfo["LINE_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["VSL_CD"].ToString())) sSql += ", '" + ParamInfo["VSL_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["VSL"].ToString())) sSql += ", '" + ParamInfo["VSL"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["VOY"].ToString())) sSql += ", '" + ParamInfo["VOY"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["ITEM_CD"].ToString())) sSql += ", '" + ParamInfo["ITEM_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["ITEM_NM"].ToString())) sSql += ", '" + ParamInfo["ITEM_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["RMK"].ToString())) sSql += ", '" + ParamInfo["RMK"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["SHP_CD"].ToString())) sSql += ", '" + ParamInfo["SHP_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["SHP_ADDR"].ToString())) sSql += ", '" + ParamInfo["SHP_ADDR"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["CNE_CD"].ToString())) sSql += ", '" + ParamInfo["CNE_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["CNE_ADDR"].ToString())) sSql += ", '" + ParamInfo["CNE_ADDR"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["NFY_CD"].ToString())) sSql += ", '" + ParamInfo["NFY_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["NFY_ADDR"].ToString())) sSql += ", '" + ParamInfo["NFY_ADDR"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["SALES_CD"].ToString())) sSql += ", '" + ParamInfo["SALES_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["SALES_NM"].ToString())) sSql += ", '" + ParamInfo["SALES_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["OP_CD"].ToString())) sSql += ", '" + ParamInfo["OP_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["OP_NM"].ToString())) sSql += ", '" + ParamInfo["OP_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["REPORT_TITLE"].ToString())) sSql += ", '" + ParamInfo["REPORT_TITLE"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["ANN1"].ToString())) sSql += ", '" + ParamInfo["ANN1"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["ANN2"].ToString())) sSql += ", '" + ParamInfo["ANN2"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["CNTR_TYPE"].ToString())) sSql += ", '" + ParamInfo["CNTR_TYPE"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["PKG"].ToString())) sSql += ", '" + ParamInfo["PKG"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["GRS_WGT"].ToString())) sSql += ", '" + ParamInfo["GRS_WGT"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ParamInfo["MSRMT"].ToString())) sSql += ", '" + ParamInfo["MSRMT"].ToString() + "' ";
                sSql += ", '" + ParamInfo["INS_USR"].ToString() + "' ";
                sSql += ", UFN_DATE_FORMAT('DATE') ";
                sSql += ", UFN_DATE_FORMAT('TIME')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }

            return rtnBool;
        }

        // ELVIS 에서 전송한 견적의 운임 정보을 저장한다.
        public static bool SendQuotFrtToElvisFriend(DataTable ParamInfo)
        {
            if (ParamInfo != null)
            {
                for (int i = 0;i<ParamInfo.Rows.Count;i++)
                {
                    sSql = "";
                    sSql += "INSERT INTO META_QUOT_FRT ( ";
                    sSql += "  QUOT_NO ";
                    sSql += ", MNGT_NO ";
                    sSql += ", CRN ";
                    sSql += ", FARE_SEQ ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["ISSUED_AT"].ToString())) sSql += ", ISSUED_AT ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["SELL_BUY_TYPE"].ToString())) sSql += ", SELL_BUY_TYPE ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["FARE_CD"].ToString())) sSql += ", FARE_CD ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["FARE_NM"].ToString())) sSql += ", FARE_NM ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["PKG"].ToString())) sSql += ", PKG ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["PKG_UNIT"].ToString())) sSql += ", PKG_UNIT ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["UNIT_PRC"].ToString())) sSql += ", UNIT_PRC ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["CURR_CD"].ToString())) sSql += ", CURR_CD ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["EXRT"].ToString())) sSql += ", EXRT ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["FARE_AMT"].ToString())) sSql += ", FARE_AMT ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["FARE_LOC_AMT"].ToString())) sSql += ", FARE_LOC_AMT ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["FARE_VAT_AMT"].ToString())) sSql += ", FARE_VAT_AMT ";
                    sSql += ", INS_USR ";
                    sSql += ", INS_YMD ";
                    sSql += ", INS_HM) ";

                    sSql += "VALUES( ";
                    sSql += "  '" + ParamInfo.Rows[i]["QUOT_NO"].ToString() + "' ";

                    sSql += ", '" + ParamInfo.Rows[i]["MNGT_NO"].ToString() + "' ";
                    sSql += ", '" + ParamInfo.Rows[i]["CRN"].ToString() + "' ";
                    sSql += ", " + ParamInfo.Rows[i]["FARE_SEQ"].ToString();
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["ISSUED_AT"].ToString())) sSql += ", '" + ParamInfo.Rows[i]["ISSUED_AT"].ToString() + "' ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["SELL_BUY_TYPE"].ToString())) sSql += ", '" + ParamInfo.Rows[i]["SELL_BUY_TYPE"].ToString() + "' ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["FARE_CD"].ToString())) sSql += ", '" + ParamInfo.Rows[i]["FARE_CD"].ToString() + "' ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["FARE_NM"].ToString())) sSql += ", '" + ParamInfo.Rows[i]["FARE_NM"].ToString() + "' ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["PKG"].ToString())) sSql += ", " + ParamInfo.Rows[i]["PKG"].ToString();
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["PKG_UNIT"].ToString())) sSql += ", '" + ParamInfo.Rows[i]["PKG_UNIT"].ToString() + "' ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["UNIT_PRC"].ToString())) sSql += ", " + ParamInfo.Rows[i]["UNIT_PRC"].ToString();
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["CURR_CD"].ToString())) sSql += ", '" + ParamInfo.Rows[i]["CURR_CD"].ToString() + "' ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["EXRT"].ToString())) sSql += ", " + ParamInfo.Rows[i]["EXRT"].ToString();
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["FARE_AMT"].ToString())) sSql += ", " + ParamInfo.Rows[i]["FARE_AMT"].ToString();
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["FARE_LOC_AMT"].ToString())) sSql += ", " + ParamInfo.Rows[i]["FARE_LOC_AMT"].ToString();
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["FARE_VAT_AMT"].ToString())) sSql += ", " + ParamInfo.Rows[i]["FARE_VAT_AMT"].ToString();
                    sSql += ", '" + ParamInfo.Rows[i]["INS_USR"].ToString() + "' ";
                    sSql += ", UFN_DATE_FORMAT('DATE') ";
                    sSql += ", UFN_DATE_FORMAT('TIME')) ";

                    int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                    if (rtnStatus >= 0) rtnBool = true;
                }
            }

            return rtnBool;
        }

        // ELVIS 에서 전송한 견적의 컨테이너 정보을 저장한다.
        public static bool SendQuotCntrToElvisFriend(DataTable ParamInfo)
        {
            if (ParamInfo != null)
            {
                for (int i = 0; i < ParamInfo.Rows.Count; i++)
                {
                    sSql = "";
                    sSql += "INSERT INTO META_QUOT_CNTR ( ";
                    sSql += "  QUOT_NO ";
                    sSql += ", MNGT_NO ";
                    sSql += ", CRN ";
                    sSql += ", CNTR_SEQ ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["CNTR_TYPE"].ToString())) sSql += ", CNTR_TYPE ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["CNTR_PKG"].ToString())) sSql += ", CNTR_PKG ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["TEMP"].ToString())) sSql += ", TEMP ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["VENT"].ToString())) sSql += ", VENT ";
                    sSql += ", INS_USR ";
                    sSql += ", INS_YMD ";
                    sSql += ", INS_HM) ";

                    sSql += "VALUES( ";
                    sSql += "  '" + ParamInfo.Rows[i]["QUOT_NO"].ToString() + "' ";
                    sSql += ", '" + ParamInfo.Rows[i]["MNGT_NO"].ToString() + "' ";
                    sSql += ", '" + ParamInfo.Rows[i]["CRN"].ToString() + "' ";
                    sSql += ", " + ParamInfo.Rows[i]["CNTR_SEQ"].ToString();
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["CNTR_TYPE"].ToString())) sSql += ", '" + ParamInfo.Rows[i]["CNTR_TYPE"].ToString() + "' ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["CNTR_PKG"].ToString())) sSql += ", " + ParamInfo.Rows[i]["CNTR_PKG"].ToString() + " ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["TEMP"].ToString())) sSql += ", " + ParamInfo.Rows[i]["TEMP"].ToString() + " ";
                    if (!string.IsNullOrEmpty(ParamInfo.Rows[i]["VENT"].ToString())) sSql += ", '" + ParamInfo.Rows[i]["VENT"].ToString() + "' ";
                    sSql += ", '" + ParamInfo.Rows[i]["INS_USR"].ToString() + "' ";
                    sSql += ", UFN_DATE_FORMAT('DATE') ";
                    sSql += ", UFN_DATE_FORMAT('TIME')) ";

                    int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                    if (rtnStatus >= 0) rtnBool = true;
                }

            }

            return rtnBool;
        }

        // 견적 메인 정보를 조회 한다
        public static DataTable GetQuotationMain(string QuotNo, string Crn)
        {
            if (!string.IsNullOrEmpty(QuotNo) && !string.IsNullOrEmpty(Crn))
            {
                sSql = "";
                sSql += " SELECT FQM.QUOT_NO ";
                sSql += "      , MAX(FQM.CRN) AS CRN ";
                sSql += "      , MAX(FQM.STATUS) AS STATUS ";
                sSql += "      , MAX(FQM.POL_NM) AS POL_NM ";
                sSql += "      , MAX(FQM.POD_NM) AS POD_NM ";
                sSql += "      , MAX(FQM.ETD) AS ETD ";
                sSql += "      , MAX(FQM.ETA) AS ETA ";
                sSql += "      , MAX(FQM.ITEM_NM) AS ITEM_NM ";
                sSql += "      , SUM(FQD.PKG) AS TOT_PKG ";
                sSql += "      , SUM(FQD.GRS_WGT) AS TOT_GRS_WGT ";
                sSql += "      , SUM(FQD.VOL_WGT) AS TOT_VOL_WGT ";
                sSql += "      , MAX(FQM.RMK) AS RMK ";
                sSql += " FROM META_QUOT_MST FQM ";
                sSql += " LEFT OUTER JOIN META_QUOT_DIM FQD ";
                sSql += " ON FQM.QUOT_NO = FQD.QUOT_NO ";
                sSql += " WHERE FQM.QUOT_NO = '" + QuotNo + "' ";
                sSql += " AND FQM.CRN = '" + Crn + "' ";
                sSql += " GROUP BY FQM.QUOT_NO ";
            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            rtnDt.TableName = "MAIN";
            return rtnDt;
        }

        // ELVIS-FRIEND 가입 된 포워더 정보를 조회 한다
        public static DataTable GetForwarderInfo(string Crn)
        {
            if (!string.IsNullOrEmpty(Crn))
            {
                sSql = "";
                sSql += " SELECT PER.DOMAIN ";
                sSql += "      , WEB_IP_EXT AS WEB_IP_EXT ";
                sSql += "      , WEB_PORT AS WEB_PORT ";
                sSql += "      , DB_IP_EXT AS DB_IP ";
                sSql += "      , USR_ID AS DB_ID ";
                sSql += "      , USR_PWD AS DB_PWD ";
                sSql += "      , DB_PORT AS DB_PORT ";
                sSql += "      , DB_SID AS DB_SID ";
                sSql += "   FROM PRM_ELVIS_REG PER ";
                sSql += "   LEFT OUTER JOIN META_CUST_MST FCM ";
                sSql += "     ON PER.DOMAIN = FCM.DOMAIN ";
                sSql += "  WHERE PER.ELVIS_FRIEND = 'Y' ";
                sSql += "    AND PER.CRN = '" + Crn + "' ";
            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // ELVIS-FRIEND 가입 된 포워더 리스트를 조회 한다
        public static DataTable GetForwarderList(string OfficeNm)
        {
            sSql = "";
            sSql += " SELECT CRN ";
            sSql += "      , DOMAIN ";
            sSql += "      , OFFICE_NM ";
            sSql += "      , LOC_NM ";
            sSql += "      , TEL_NO ";
            sSql += "      , EMAIL ";
            sSql += "   FROM PRM_ELVIS_REG ";
            sSql += "  WHERE ELVIS_FRIEND = 'Y' ";
            if (!string.IsNullOrEmpty(OfficeNm)) sSql += "    AND OFFICE_NM LIKE '%" + OfficeNm + "%' ";
            sSql += "  ORDER BY OFFICE_NM ";

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        //ELVIS-FRIEND 가입 된 포워더 정보를 조회 한다
        public static DataTable GetFwdVolInfo(string Crn, string PolCd, string PodCd)
        {
            sSql = "";
            sSql += " SELECT FVS.CRN ";
            sSql += "      , SUM(CASE WHEN POL_CD = '" + PolCd + "' AND POD_CD = '" + PodCd + "' THEN DATA_CNT END) DATA_CNT ";
            sSql += "      , SUM(DATA_CNT) AS TOT_DATA_CNT ";
            sSql += "      , SUM(CASE WHEN POL_CD = '" + PolCd + "' AND POD_CD = '" + PodCd + "' THEN DATA_RTON END) DATA_RTON ";
            sSql += "      , SUM(DATA_RTON) AS TOT_DATA_RTON ";
            sSql += "      , SUM(CASE WHEN POL_CD = '" + PolCd + "' AND POD_CD = '" + PodCd + "' THEN DATA_TEU END) DATA_TEU ";
            sSql += "      , SUM(DATA_TEU) AS TOT_DATA_TEU ";
            sSql += "      , MAX(PER.OFFICE_NM) OFFICE_NM ";
            sSql += "      , MAX(PER.OFFICE_ADDR) OFFICE_ADDR ";
            sSql += "      , MAX(PER.TEL_NO) TEL_NO ";
            sSql += "      , MAX(PER.EMAIL) EMAIL ";
            sSql += "      , MAX(PER.INFO_FILE_PATH) INFO_FILE_PATH ";
            sSql += "      , MAX(PER.INFO_FILE_NM) INFO_FILE_NM ";
            sSql += "      , MAX(PER.INFO_FILE_EXT) INFO_FILE_EXT ";
            sSql += "   FROM META_VOL_STATIC FVS ";
            sSql += "   LEFT OUTER JOIN PRM_ELVIS_REG PER ";
            sSql += "     ON FVS.CRN = PER.CRN ";
            sSql += "  WHERE PER.CRN = '" + Crn + "'";
            sSql += "  GROUP BY FVS.CRN ";

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // 견적 추천 포워더 리스트를 조회 한다
        public static DataTable GetReccomendFowarder(string PolCd, string PodCd, string OrderType)
        {
            sSql = "";
            sSql += " SELECT PER.CRN ";
            sSql += "      , DOMAIN ";
            sSql += "      , OFFICE_NM ";
            sSql += "      , LOC_NM ";
            sSql += "      , EMAIL ";
            sSql += "      , TEL_NO ";
            sSql += "   FROM META_RECOMMEND_FWD FRF ";
            sSql += "   LEFT OUTER JOIN PRM_ELVIS_REG PER ";
            sSql += "     ON FRF.CRN = PER.CRN ";
            sSql += "  WHERE ELVIS_FRIEND = 'Y' ";
            sSql += "    AND POL_CD = '" + PolCd + "' ";
            sSql += "    AND POD_CD = '" + PodCd + "' ";
            sSql += "  ORDER BY " + OrderType;

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        //포워더 견적내역을 조회한다
        public static DataTable GetFwdQuotationList(string Crn, string YmdType, string FmYmd, string ToYmd, string PolCd, string PodCd, string MngtNo)
        {
            if (!string.IsNullOrEmpty(MngtNo))
            {
                //단건 상세 조회
                sSql = "";
                sSql += " SELECT FQM.QUOT_NO ";
                sSql += "      , MAX(FQM.STATUS) AS STATUS ";
                sSql += "      , MAX(FQM.REQ_SVC) AS REQ_SVC ";
                sSql += "      , MAX(FQM.POL_CD) AS POL_CD ";
                sSql += "      , MAX(FQM.POL_NM) AS POL_NM ";
                sSql += "      , MAX(FQM.POD_CD) AS POD_CD ";
                sSql += "      , MAX(FQM.POD_NM) AS POD_NM ";
                sSql += "      , MAX(FQM.ETD) AS ETD ";
                sSql += "      , MAX(FQM.ETA) AS ETA ";
                sSql += "      , MAX(FQM.PIC_NM) AS PIC_NM ";
                sSql += "      , MAX(FQM.ITEM_NM) AS ITEM_NM ";
                sSql += "      , SUM(CASE WHEN FQD.REQ_SVC = 'SEA' THEN 1 ELSE 0 END) SEA_CNT ";
                sSql += "      , SUM(CASE WHEN FQD.REQ_SVC = 'AIR' THEN 1 ELSE 0 END) AIR_CNT ";
                sSql += "      , MAX(REQ_FWD_CNT) AS REQ_FWD_CNT ";
                sSql += "      , NVL(REGEXP_COUNT(WM_CONCAT(DISTINCT(FQD.CRN)),',') + 1,0) AS FWD_QUOT_CNT ";
                sSql += "      , MAX(FQM.INS_YMD) AS REQ_YMD ";
                sSql += "      , MAX(FQD.INS_YMD) AS QUOT_YMD ";
                sSql += "      , (SELECT COUNT(*) FROM META_DOC_MST WHERE DOC_TYPE = 'CI' AND ORG_MNGT_NO = FQM.QUOT_NO) AS CI_CNT ";
                sSql += "      , (SELECT COUNT(*) FROM META_DOC_MST WHERE DOC_TYPE = 'PL' AND ORG_MNGT_NO = FQM.QUOT_NO) AS PL_CNT ";
                sSql += "   FROM META_QUOT_MST FQM ";
                sSql += "   LEFT OUTER JOIN META_QUOT_DTL FQD ";
                sSql += "     ON FQM.QUOT_NO = FQD.QUOT_NO ";
                sSql += "  WHERE 1=1 ";
                sSql += "    AND FQM.QUOT_NO = '" + MngtNo + "' ";
                sSql += "  GROUP BY FQM.QUOT_NO ";
            }
            else if (!string.IsNullOrEmpty(Crn) && !string.IsNullOrEmpty(YmdType) && !string.IsNullOrEmpty(FmYmd) && !string.IsNullOrEmpty(ToYmd))
            {
                //메인 리스트 조회
                sSql = "";
                sSql = " SELECT A.QUOT_NO ";
                sSql += "     , A.MNGT_NO ";
                sSql += "     , MAX(FWD_CRN) AS FWD_CRN ";
                sSql += "     , MAX(FQM.POL_CD) AS POL_CD ";
                sSql += "     , MAX(FQM.POL_NM) AS POL_NM ";
                sSql += "     , MAX(A.ETD) AS ETD ";
                sSql += "     , MAX(FQM.POD_CD) AS POD_CD ";
                sSql += "     , MAX(FQM.POD_NM) AS POD_NM ";
                sSql += "     , MAX(A.ETA) AS ETA ";
                sSql += "     , MAX(FQM.INS_YMD) AS REQ_YMD ";
                sSql += "     , MAX(A.QUOT_YMD) AS QUOT_YMD ";
                sSql += "     , MAX(FQM.ITEM_NM) AS ITEM_NM ";
                sSql += "     , MAX(CUST_PIC_NM) AS CUST_PIC_NM ";
                sSql += "     , WM_CONCAT(CURR_CD) AS CURR_CD ";
                sSql += "     , WM_CONCAT(FARE_AMT) AS FARE_AMT ";
                sSql += "     , WM_CONCAT(FARE_LOC_AMT) AS FARE_LOC_AMT ";
                sSql += "FROM (SELECT MAX(FQD.QUOT_NO) AS QUOT_NO ";
                sSql += "           , FQD.MNGT_NO ";
                sSql += "           , MAX(FQD.CRN) AS FWD_CRN ";
                sSql += "           , MAX(FQD.POL_CD) AS POL_CD ";
                sSql += "           , MAX(FQD.POL_NM) AS POL_NM ";
                sSql += "           , MAX(FQD.ETD) AS ETD ";
                sSql += "           , MAX(FQD.POD_CD) AS POD_CD ";
                sSql += "           , MAX(FQD.POD_NM) AS POD_NM ";
                sSql += "           , MAX(FQD.ETA) AS ETA ";
                sSql += "           , MAX(FQD.INS_YMD) AS QUOT_YMD ";
                sSql += "           , MAX(FQD.ITEM_CD) AS ITEM_NM ";
                sSql += "           , MAX(FQD.CUST_PIC_NM) AS CUST_PIC_NM ";
                sSql += "           , FQF.CURR_CD ";
                sSql += "           , SUM(FQF.FARE_AMT) AS FARE_AMT  ";
                sSql += "           , SUM(FQF.FARE_LOC_AMT) AS FARE_LOC_AMT  ";
                sSql += "       FROM META_QUOT_DTL FQD ";
                sSql += "       LEFT OUTER JOIN META_QUOT_FRT FQF ";
                sSql += "           ON FQD.MNGT_NO = FQF.MNGT_NO ";
                sSql += "       WHERE 1=1 ";
                sSql += "       GROUP BY FQD.MNGT_NO ,FQF.CURR_CD) A ";
                sSql += "LEFT JOIN META_QUOT_MST FQM ";
                sSql += "       ON A.QUOT_NO = FQM.QUOT_NO ";
                sSql += "WHERE 1=1 ";
                sSql += "  AND FWD_CRN = '"+ Crn + "' ";
                if (YmdType == "REQ_YMD")
                {
                    sSql += "AND FQM.INS_YMD >= '" + FmYmd + "' ";
                    sSql += "AND FQM.INS_YMD <= '" + ToYmd + "' ";
                }
                else
                {
                    sSql += "AND QUOT_YMD >= '" + FmYmd + "' ";
                    sSql += "AND QUOT_YMD <= '" + ToYmd + "' ";
                }
                if (!string.IsNullOrEmpty(PolCd)) sSql += " AND POL_CD = '" + PolCd + "' ";
                if (!string.IsNullOrEmpty(PodCd)) sSql += " AND POD_CD = '" + PodCd + "' ";
                sSql += "GROUP BY A.QUOT_NO, A.MNGT_NO ";


            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }


        // 견적 요청 정보를 조회 한다
        public static DataTable GetQuotationList(string Crn, string YmdType, string FmYmd, string ToYmd, string PolCd, string PodCd, string QuotNo)
        {
            if (!string.IsNullOrEmpty(QuotNo))
            {
                sSql = "";
                sSql += " SELECT FQM.QUOT_NO ";
                sSql += "      , MAX(FQM.STATUS) AS STATUS ";
                sSql += "      , MAX(FQM.REQ_SVC) AS REQ_SVC ";
                sSql += "      , MAX(FQM.POL_CD) AS POL_CD ";
                sSql += "      , MAX(FQM.POL_NM) AS POL_NM ";
                sSql += "      , MAX(FQM.POD_CD) AS POD_CD ";
                sSql += "      , MAX(FQM.POD_NM) AS POD_NM ";
                sSql += "      , MAX(FQM.ETD) AS ETD ";
                sSql += "      , MAX(FQM.ETA) AS ETA ";
                sSql += "      , MAX(FQM.PIC_NM) AS PIC_NM ";
                sSql += "      , MAX(FQM.PIC_TEL) AS PIC_TEL ";
                sSql += "      , MAX(FQM.PIC_MAIL) AS PIC_MAIL ";
                sSql += "      , MAX(FQM.OFFICE_NM) AS OFFICE_NM ";
                sSql += "      , MAX(FQM.ITEM_NM) AS ITEM_NM ";
                sSql += "      , SUM(CASE WHEN FQD.REQ_SVC = 'SEA' THEN 1 ELSE 0 END) SEA_CNT ";
                sSql += "      , SUM(CASE WHEN FQD.REQ_SVC = 'AIR' THEN 1 ELSE 0 END) AIR_CNT ";
                sSql += "      , MAX(REQ_FWD_CNT) AS REQ_FWD_CNT ";
                sSql += "      , NVL(REGEXP_COUNT(WM_CONCAT(DISTINCT(FQD.CRN)),',') + 1,0) AS FWD_QUOT_CNT ";
                sSql += "      , MAX(FQM.INS_YMD) AS REQ_YMD ";
                sSql += "      , MAX(FQD.INS_YMD) AS QUOT_YMD ";
                sSql += "      , (SELECT COUNT(*) FROM META_DOC_MST WHERE DOC_TYPE = 'CI' AND ORG_MNGT_NO = FQM.QUOT_NO) AS CI_CNT ";
                sSql += "      , (SELECT COUNT(*) FROM META_DOC_MST WHERE DOC_TYPE = 'PL' AND ORG_MNGT_NO = FQM.QUOT_NO) AS PL_CNT ";
                sSql += "   FROM META_QUOT_MST FQM ";
                sSql += "   LEFT OUTER JOIN META_QUOT_DTL FQD ";
                sSql += "     ON FQM.QUOT_NO = FQD.QUOT_NO ";
                sSql += "  WHERE 1=1 ";
                sSql += "    AND FQM.QUOT_NO = '" + QuotNo + "' ";
                sSql += "  GROUP BY FQM.QUOT_NO ";
            }
            else if (!string.IsNullOrEmpty(Crn) && !string.IsNullOrEmpty(YmdType) && !string.IsNullOrEmpty(FmYmd) && !string.IsNullOrEmpty(ToYmd))
            {
                sSql = "";
                sSql += " SELECT FQM.QUOT_NO ";
                sSql += "      , MAX(FQM.STATUS) AS STATUS ";
                sSql += "      , MAX(FQM.REQ_SVC) AS REQ_SVC ";
                sSql += "      , MAX(FQM.POL_CD) AS POL_CD ";
                sSql += "      , MAX(FQM.POL_NM) AS POL_NM ";
                sSql += "      , MAX(FQM.POD_CD) AS POD_CD ";
                sSql += "      , MAX(FQM.POD_NM) AS POD_NM ";
                sSql += "      , MAX(FQM.ETD) AS ETD ";
                sSql += "      , MAX(FQM.ETA) AS ETA ";
                sSql += "      , MAX(FQM.PIC_NM) AS PIC_NM ";
                sSql += "      , MAX(FQM.ITEM_NM) AS ITEM_NM ";
                sSql += "      , SUM(CASE WHEN FQD.REQ_SVC = 'SEA' THEN 1 ELSE 0 END) SEA_CNT ";
                sSql += "      , SUM(CASE WHEN FQD.REQ_SVC = 'AIR' THEN 1 ELSE 0 END) AIR_CNT ";
                sSql += "      , MAX(REQ_FWD_CNT) AS REQ_FWD_CNT ";
                sSql += "      , NVL(REGEXP_COUNT(WM_CONCAT(DISTINCT(FQD.CRN)),',') + 1,0) AS FWD_QUOT_CNT ";
                sSql += "      , MAX(FQM.INS_YMD) AS REQ_YMD ";
                sSql += "      , MAX(FQD.INS_YMD) AS QUOT_YMD ";
                sSql += "      , (SELECT COUNT(*) FROM META_DOC_MST WHERE DOC_TYPE = 'CI' AND ORG_MNGT_NO = FQM.QUOT_NO) AS CI_CNT ";
                sSql += "      , (SELECT COUNT(*) FROM META_DOC_MST WHERE DOC_TYPE = 'PL' AND ORG_MNGT_NO = FQM.QUOT_NO) AS PL_CNT ";
                sSql += "   FROM META_QUOT_MST FQM ";
                sSql += "   LEFT OUTER JOIN META_QUOT_DTL FQD ";
                sSql += "     ON FQM.QUOT_NO = FQD.QUOT_NO ";
                sSql += "  WHERE 1=1 ";
                sSql += "    AND FQM.CRN = '" + Crn + "' ";
                if (YmdType == "REQ_YMD")
                {
                    sSql += "AND FQM.INS_YMD >= '" + FmYmd + "' ";
                    sSql += "AND FQM.INS_YMD <= '" + ToYmd + "' ";
                }
                else
                {
                    sSql += "AND FQD.INS_YMD >= '" + FmYmd + "' ";
                    sSql += "AND FQD.INS_YMD <= '" + ToYmd + "' ";
                }
                if (!string.IsNullOrEmpty(PolCd)) sSql += " AND FQM.POL_CD = '" + PolCd + "' ";
                if (!string.IsNullOrEmpty(PodCd)) sSql += " AND FQM.POD_CD = '" + PodCd + "' ";
                sSql += " GROUP BY FQM.QUOT_NO ";
            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        //실행사 견적요청자료 조회 한다
        public static DataTable GetQuotationFList(string fwdCrn, string FmYmd, string ToYmd ,  string PolCd , string PodCd)
        {
            sSql = "";

            sSql += "SELECT FQC.CRN AS FWD_CRN ";
            sSql += "       , FQC.QUOT_NO AS QUOT_NO  ";
            sSql += "       , FQM.POL_NM              ";
            sSql += "       , FQM.POL_CD              ";
            sSql += "       , FQM.ETD                 ";
            sSql += "       , FQM.POD_NM              ";
            sSql += "       , FQM.POD_CD              ";
            sSql += "       , FQM.ETA                 ";
            sSql += "       , FQM.INS_YMD AS REQ_YMD  ";
            sSql += "       , FQM.ITEM_NM             ";
            sSql += "       , FQM.PIC_NM              ";
            sSql += "       , FQM.PIC_MAIL            ";
            sSql += "       , FQM.PIC_TEL             ";
            sSql += "       , FQM.REQ_SVC             ";
            sSql += "      , (SELECT COUNT(*) FROM META_DOC_MST WHERE DOC_TYPE = 'CI' AND ORG_MNGT_NO = FQM.QUOT_NO) AS CI_CNT ";
            sSql += "      , (SELECT COUNT(*) FROM META_DOC_MST WHERE DOC_TYPE = 'PL' AND ORG_MNGT_NO = FQM.QUOT_NO) AS PL_CNT ";
            sSql += "FROM  META_QUOT_CRN FQC ";
            sSql += " LEFT JOIN META_QUOT_MST FQM ";
            sSql += "   ON FQC.QUOT_NO = FQM.QUOT_NO ";
            sSql += " WHERE 1=1 ";
            sSql += "   AND FQC.CRN = '"+fwdCrn+"'";
            sSql += "   AND FQM.INS_YMD >= '" + FmYmd + "' ";
            sSql += "   AND FQM.INS_YMD <= '" + ToYmd + "' ";
            if (!string.IsNullOrEmpty(PolCd)) sSql += " AND FQM.POL_CD = '" + PolCd + "' ";
            if (!string.IsNullOrEmpty(PodCd)) sSql += " AND FQM.POD_CD = '" + PodCd + "' ";

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // ELVIS 견적 리스트를 조회 한다
        public static DataTable GetQuotationDetailList(string QuotNo, string ReqSvc)
        {
            if (!string.IsNullOrEmpty(QuotNo) && !string.IsNullOrEmpty(ReqSvc))
            {
                sSql = "";
                sSql += " SELECT QUOT_NO ";
                sSql += "      , MNGT_NO ";
                sSql += "      , MAX(CRN) AS CRN ";
                sSql += "      , MAX(CUST_CD) AS CUST_CD ";
                sSql += "      , MAX(CUST_NM) AS CUST_NM ";
                sSql += "      , MAX(CUST_PIC_NM) AS CUST_PIC_NM ";
                sSql += "      , MAX(CUST_PIC_TEL) AS CUST_PIC_TEL ";
                sSql += "      , MAX(CUST_PIC_EMAIL) AS CUST_PIC_EMAIL ";
                sSql += "      , MAX(FWD_OFFICE_NM) AS FWD_OFFICE_NM ";
                sSql += "      , MAX(FWD_PIC_NM) AS FWD_PIC_NM ";
                sSql += "      , MAX(FWD_PIC_TEL) AS FWD_PIC_TEL ";
                sSql += "      , MAX(FWD_PIC_EMAIL) AS FWD_PIC_EMAIL ";
                sSql += "      , WM_CONCAT(CURR_CD) AS CURR_CD ";
                sSql += "      , WM_CONCAT(FARE_AMT) AS FARE_AMT ";
                sSql += "      , WM_CONCAT(FARE_LOC_AMT) AS FARE_LOC_AMT ";
                sSql += "   FROM ";
                sSql += "(SELECT MAX(FQD.QUOT_NO) AS QUOT_NO ";
                sSql += "      , FQD.MNGT_NO ";
                sSql += "      , MAX(FQD.CRN) AS CRN ";
                sSql += "      , MAX(FQD.CUST_CD) AS CUST_CD ";
                sSql += "      , MAX(FQD.CUST_NM) AS CUST_NM ";
                sSql += "      , MAX(FQD.CUST_PIC_NM) AS CUST_PIC_NM ";
                sSql += "      , MAX(FQD.CUST_PIC_TEL) AS CUST_PIC_TEL ";
                sSql += "      , MAX(FQD.CUST_PIC_EMAIL) AS CUST_PIC_EMAIL ";
                sSql += "      , MAX(FQD.FWD_OFFICE_NM) AS FWD_OFFICE_NM ";
                sSql += "      , MAX(FQD.FWD_PIC_NM) AS FWD_PIC_NM ";
                sSql += "      , MAX(FQD.FWD_PIC_TEL) AS FWD_PIC_TEL ";
                sSql += "      , MAX(FQD.FWD_PIC_EMAIL) AS FWD_PIC_EMAIL ";
                sSql += "      , FQF.CURR_CD ";
                sSql += "      , SUM(FQF.FARE_AMT) AS FARE_AMT ";
                sSql += "      , SUM(FQF.FARE_LOC_AMT) AS FARE_LOC_AMT ";
                sSql += "   FROM META_QUOT_DTL FQD ";
                sSql += "   LEFT OUTER JOIN META_QUOT_FRT FQF ";
                sSql += "     ON FQD.MNGT_NO = FQF.MNGT_NO ";
                sSql += "  WHERE FQD.QUOT_NO = '" + QuotNo + "' ";
                sSql += "    AND FQD.REQ_SVC = '" + ReqSvc + "' ";
                sSql += "  GROUP BY FQD.MNGT_NO, FQF.CURR_CD) ";
                sSql += "  GROUP BY QUOT_NO, MNGT_NO ";
            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // 견적 요청 정보 상세를 조회 한다
        public static DataTable GetQuotationDetail(string QuotNo, string Crn, string OrderType)
        {
            if (!string.IsNullOrEmpty(QuotNo))
            {
                sSql = "";
                sSql += " SELECT MNGT_NO ";
                sSql += "      , MAX(CRN) AS CRN ";
                sSql += "      , MAX (REQ_SVC) AS REQ_SVC ";
                sSql += "      , MAX (FWD_OFFICE_NM) AS FWD_OFFICE_NM ";
                sSql += "      , MAX (FWD_PIC_NM) AS FWD_PIC_NM ";
                sSql += "      , MAX (VSL) || ' ' || MAX (VOY) AS VSL";
                sSql += "      , WM_CONCAT(CURR_CD) AS CURR_CD ";
                sSql += "      , WM_CONCAT(FARE_AMT) AS FARE_AMT ";
                sSql += "      , WM_CONCAT(FARE_LOC_AMT) AS FARE_LOC_AMT ";
                sSql += "      , SUM(FARE_LOC_AMT) AS TOT_AMT ";
                sSql += "      , MAX (FILE_PATH) AS FILE_PATH ";
                sSql += "      , MAX (FILE_NM) AS FILE_NM ";
                sSql += "      , MAX (FILE_EXT) AS FILE_EXT ";
                sSql += "      , MAX (POL_NM) AS POL_NM ";
                sSql += "      , MAX (POD_NM) AS POD_NM ";
                sSql += "      , MAX (ETD) AS ETD ";
                sSql += "      , MAX (ETA) AS ETA ";
                sSql += "      , MAX(INFO_FILE_PATH) AS INFO_FILE_PATH ";
                sSql += "      , MAX(INFO_FILE_NM) AS INFO_FILE_NM ";
                sSql += "      , MAX(INFO_FILE_EXT) AS INFO_FILE_EXT ";
                sSql += "      , TO_NUMBER(REGEXP_SUBSTR(WM_CONCAT(FARE_LOC_AMT), '^\\d+')) AS FIRST_FARE_LOC_AMT ";
                sSql += "   FROM ";
                sSql += "(SELECT FQD.MNGT_NO ";
                sSql += "      , MAX(FQD.CRN) AS CRN ";
                sSql += "      , MAX(FQD.REQ_SVC) AS REQ_SVC ";
                sSql += "      , MAX (FQD.FWD_OFFICE_NM) AS FWD_OFFICE_NM ";
                sSql += "      , MAX (FQD.FWD_PIC_NM) AS FWD_PIC_NM ";
                sSql += "      , MAX(FQD.VSL) AS VSL ";
                sSql += "      , MAX(FQD.VOY) AS VOY ";
                sSql += "      , FQF.CURR_CD ";
                sSql += "      , SUM(FQF.FARE_AMT) AS FARE_AMT ";
                sSql += "      , SUM(FQF.FARE_LOC_AMT) AS FARE_LOC_AMT ";
                sSql += "      , MAX(FDM.FILE_PATH) AS FILE_PATH ";
                sSql += "      , MAX(FDM.FILE_NM) AS FILE_NM ";
                sSql += "      , MAX(FDM.FILE_EXT) AS FILE_EXT ";
                sSql += "      , MAX(FQD.POL_NM) AS POL_NM ";
                sSql += "      , MAX(FQD.POD_NM) AS POD_NM ";
                sSql += "      , MAX(FQD.ETD) AS ETD ";
                sSql += "      , MAX(FQD.ETA) AS ETA ";
                sSql += "      , MAX(PER.INFO_FILE_PATH) AS INFO_FILE_PATH ";
                sSql += "      , MAX(PER.INFO_FILE_NM) AS INFO_FILE_NM ";
                sSql += "      , MAX(PER.INFO_FILE_EXT) AS INFO_FILE_EXT ";
                sSql += "   FROM META_QUOT_DTL FQD ";
                sSql += "   LEFT OUTER JOIN META_QUOT_MST FQM ";
                sSql += "     ON FQM.QUOT_NO = FQD.QUOT_NO ";
                sSql += "   LEFT OUTER JOIN META_QUOT_FRT FQF ";
                sSql += "     ON FQD.MNGT_NO = FQF.MNGT_NO ";
                sSql += "   LEFT OUTER JOIN META_DOC_MST FDM ";
                sSql += "     ON FQD.MNGT_NO = FDM.ORG_MNGT_NO ";
                sSql += "   LEFT OUTER JOIN PRM_ELVIS_REG PER ";
                sSql += "     ON PER.CRN = FQD.CRN ";
                sSql += "  WHERE FQM.QUOT_NO = '" + QuotNo + "' ";
                sSql += "    AND FQD.CRN = '" + Crn + "' ";
                sSql += "  GROUP BY FQD.MNGT_NO, FQF.CURR_CD ";
                sSql += "  ORDER BY FQF.CURR_CD) ";
                sSql += "  GROUP BY MNGT_NO ";
                if (OrderType == "AMT") sSql += "  ORDER BY FIRST_FARE_LOC_AMT ";
                else if (OrderType == "PERIOD") sSql += "  ORDER BY  ABS(ETA - ETD) ";
            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            rtnDt.TableName = "LIST";
            return rtnDt;
        }

        // ELVIS 견적의 운임 정보를 조회 한다
        public static DataTable GetQuotationFreight(string QuotNo, string MngtNo, string Crn)
        {
            if (!string.IsNullOrEmpty(QuotNo) && !string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(Crn))
            {
                sSql = "";
                sSql += " SELECT * ";
                sSql += "   FROM META_QUOT_FRT ";
                sSql += "  WHERE QUOT_NO = '" + QuotNo + "' ";
                sSql += "    AND MNGT_NO = '" + MngtNo + "' ";
                sSql += "    AND CRN = '" + Crn + "' ";
                sSql += "  ORDER BY FARE_SEQ ";
            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // 견적 요청 문서 정보를 조회 한다
        public static DataTable GetQuotationDoc(string MngtNo, string Crn, string DocType)
        {
            if (!string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(Crn))
            {
                sSql = "";
                sSql += " SELECT MNGT_NO ";
                sSql += "      , CRN ";
                sSql += "      , FILE_NM ";
                sSql += "      , SVR_FILE_NM ";
                sSql += "      , FILE_SIZE ";
                sSql += "      , FILE_EXT ";
                sSql += "      , FILE_PATH ";
                sSql += "      , DOC_TYPE ";
                sSql += "      , ORG_MNGT_NO ";
                sSql += "      , OCR_NO ";
                sSql += "      , INS_USR ";
                sSql += "   FROM META_DOC_MST ";
                sSql += "  WHERE ORG_MNGT_NO = '" + MngtNo + "' ";
                sSql += "    AND CRN = '" + Crn + "' ";
                sSql += "    AND DOC_TYPE IN ('" + DocType.Replace(",", "','") + "') ";
            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            rtnDt.TableName = "DOC";
            return rtnDt;
        }

        // 견적 관련 문서 리스트를 조회한다
        public static DataTable GetDocList(string MngtNo, string Crn)
        {
            if (!string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(Crn))
            {
                sSql = "";
                sSql += " SELECT MCC.CD_NM AS DOC_TYPE ";
                sSql += "      , FDM.FILE_PATH ";
                sSql += "      , FDM.FILE_NM ";
                sSql += "      , FDM.SVR_FILE_NM ";
                sSql += "      , FDM.FILE_EXT ";
                sSql += "   FROM META_DOC_MST FDM ";
                sSql += "   LEFT OUTER JOIN MDM_COM_CODE MCC ";
                sSql += "     ON FDM.DOC_TYPE = MCC.COMN_CD ";
                sSql += "    AND MCC.GRP_CD = 'M33' ";
                sSql += "  WHERE ORG_MNGT_NO = '" + MngtNo + "' ";
                sSql += "    AND CRN = '" + Crn + "' ";

                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            }

            return rtnDt;
        }

        // 견적요청 기본정보를 저장한다
        public static bool SetQuotStatus(string Crn, string QuotNo, string QuotDtlNo, string ElvisCrn, string ConnStr)
        {
            if (!string.IsNullOrEmpty(Crn) && !string.IsNullOrEmpty(QuotNo) && !string.IsNullOrEmpty(QuotDtlNo) && !string.IsNullOrEmpty(ConnStr) && !string.IsNullOrEmpty(ElvisCrn))
            {
                sSql = "";

                sSql += " UPDATE META_QUOT_MST ";
                sSql += "    SET STATUS = 'Y' ";
                sSql += "  WHERE QUOT_NO = '" + QuotNo + "' ";
                sSql += "    AND CRN = '" + Crn + "'";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;

                sSql = " UPDATE META_QUOT_DTL ";
                sSql += "    SET STATUS = 'Y' ";
                sSql += "  WHERE QUOT_NO = '" + QuotNo + "' ";
                sSql += "    AND MNGT_NO = '" + QuotDtlNo + "' ";
                sSql += "    AND CRN = '" + ElvisCrn + "'";

                rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;

                sSql = " UPDATE META_QUOT_DTL ";
                sSql += "    SET STATUS = 'Y' ";
                sSql += "  WHERE QUOT_NO = '" + QuotNo + "' ";
                sSql += "    AND MNGT_NO = '" + QuotDtlNo + "' ";
                sSql += "    AND CRN = '" + ElvisCrn + "'";

                rtnStatus = _DataHelper.ExecuteNonQuery(ConnStr, sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }


        public static bool SetWwdQuotationMain(string QuotNo,string MngtNo,string Param_Crn,string userId, DataRow Quot_Dr)
        {
            if(!string.IsNullOrEmpty(QuotNo) &&!string.IsNullOrEmpty(MngtNo)&& !string.IsNullOrEmpty(Param_Crn) && !string.IsNullOrEmpty(userId) && Quot_Dr != null)
            {
                sSql = "";
                sSql += "INSERT INTO META_QUOT_DTL (";
                sSql += "                     QUOT_NO ";
                sSql += "                   , MNGT_NO ";
                sSql += "                   , CRN ";
                sSql += "                   , POL_CD ";
                sSql += "                   , POL_NM ";
                sSql += "                   , POD_CD ";
                sSql += "                   , POD_NM ";
                sSql += "                   , ETD ";
                sSql += "                   , ETA ";
                if (!string.IsNullOrEmpty(Quot_Dr["VLDT_YMD"].ToString())) sSql += "    , VLDT_YMD ";
                if (!string.IsNullOrEmpty(Quot_Dr["REQ_SVC"].ToString())) sSql += "    , REQ_SVC ";
                //if (!string.IsNullOrEmpty(Quot_Dr["CUST_CD"].ToString())) sSql += "    , CUST_CD ";
                if (!string.IsNullOrEmpty(Quot_Dr["CUST_NM"].ToString())) sSql += "    , CUST_NM ";
                //if (!string.IsNullOrEmpty(Quot_Dr["CUST_PIC_CD"].ToString())) sSql += "    , CUST_PIC_CD ";
                if (!string.IsNullOrEmpty(Quot_Dr["CUST_PIC_NM"].ToString())) sSql += "    , CUST_PIC_NM ";
                if (!string.IsNullOrEmpty(Quot_Dr["CUST_PIC_EMAIL"].ToString())) sSql += "    , CUST_PIC_EMAIL ";
                if (!string.IsNullOrEmpty(Quot_Dr["VSL"].ToString())) sSql += "    , VSL ";
                if (!string.IsNullOrEmpty(Quot_Dr["ITEM_NM"].ToString())) sSql += "    , ITEM_NM ";
                if (!string.IsNullOrEmpty(Quot_Dr["RMK"].ToString())) sSql += "    , RMK ";
                if (!string.IsNullOrEmpty(Quot_Dr["CNTR_TYPE"].ToString())) sSql += "    , CNTR_TYPE ";
                if (!string.IsNullOrEmpty(Quot_Dr["PKG"].ToString())) sSql += "    , PKG ";
                if (!string.IsNullOrEmpty(Quot_Dr["GRS_WGT"].ToString())) sSql += "    , GRS_WGT ";
                if (!string.IsNullOrEmpty(Quot_Dr["FWD_OFFICE_NM"].ToString())) sSql += "    , FWD_OFFICE_NM ";
                if (!string.IsNullOrEmpty(Quot_Dr["FWD_PIC_NM"].ToString())) sSql += "    , FWD_PIC_NM ";
                if (!string.IsNullOrEmpty(Quot_Dr["FWD_PIC_TEL"].ToString())) sSql += "    , FWD_PIC_TEL ";
                if (!string.IsNullOrEmpty(Quot_Dr["FWD_PIC_EMAIL"].ToString())) sSql += "    , FWD_PIC_EMAIL ";
                if (!string.IsNullOrEmpty(Quot_Dr["CBM"].ToString())) sSql += "    , CBM ";
                if (!string.IsNullOrEmpty(Quot_Dr["CNTR_NO"].ToString())) sSql += "    , CNTR_NO ";

                sSql += "                   , INS_USR ";
                sSql += "                   , INS_YMD ";
                sSql += "                   , INS_HM) ";
                sSql += "VALUES( ";
                sSql += "                     '"+QuotNo+"' ";
                sSql += "                   , '"+MngtNo+"' ";
                sSql += "                   , '" + Param_Crn + "' ";
                sSql += "                   , '" + Quot_Dr["POL_CD"].ToString()+ "' ";
                sSql += "                   , '" + Quot_Dr["POL_NM"].ToString() + "' ";
                sSql += "                   , '" + Quot_Dr["POD_CD"].ToString() + "' ";
                sSql += "                   , '" + Quot_Dr["POD_NM"].ToString() + "' ";
                sSql += "                   , '" + Quot_Dr["ETD"].ToString() + "' ";
                sSql += "                   , '" + Quot_Dr["ETA"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["VLDT_YMD"].ToString())) sSql += "    , '"+Quot_Dr["VLDT_YMD"].ToString()+"' ";
                if (!string.IsNullOrEmpty(Quot_Dr["REQ_SVC"].ToString())) sSql += "    , '" + Quot_Dr["REQ_SVC"].ToString() + "' ";
                //if (!string.IsNullOrEmpty(Quot_Dr["CUST_CD"].ToString())) sSql += "    , '" + Quot_Dr["CUST_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["CUST_NM"].ToString())) sSql += "    , '" + Quot_Dr["CUST_NM"].ToString() + "' ";
                //if (!string.IsNullOrEmpty(Quot_Dr["CUST_PIC_CD"].ToString())) sSql += "    , '" + Quot_Dr["CUST_PIC_CD"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["CUST_PIC_NM"].ToString())) sSql += "    , '" + Quot_Dr["CUST_PIC_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["CUST_PIC_EMAIL"].ToString())) sSql += "    , '" + Quot_Dr["CUST_PIC_EMAIL"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["VSL"].ToString())) sSql += "    , '" + Quot_Dr["VSL"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["ITEM_NM"].ToString())) sSql += "    , '" + Quot_Dr["ITEM_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["RMK"].ToString())) sSql += "    , '" + Quot_Dr["RMK"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["CNTR_TYPE"].ToString())) sSql += "    , '" + Quot_Dr["CNTR_TYPE"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["PKG"].ToString())) sSql += "    , '" + Quot_Dr["PKG"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["GRS_WGT"].ToString())) sSql += "    , '" + Quot_Dr["GRS_WGT"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["FWD_OFFICE_NM"].ToString())) sSql += "    , '" + Quot_Dr["FWD_OFFICE_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["FWD_PIC_NM"].ToString())) sSql += "    , '" + Quot_Dr["FWD_PIC_NM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["FWD_PIC_TEL"].ToString())) sSql += "    , '" + Quot_Dr["FWD_PIC_TEL"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["FWD_PIC_EMAIL"].ToString())) sSql += "    , '" + Quot_Dr["FWD_PIC_EMAIL"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["CBM"].ToString())) sSql += "    , '" + Quot_Dr["CBM"].ToString() + "' ";
                if (!string.IsNullOrEmpty(Quot_Dr["CNTR_NO"].ToString())) sSql += "    , '" + Quot_Dr["CNTR_NO"].ToString() + "' ";

                sSql += "                   , '"+ userId + "' ";
                sSql += "                   , UFN_DATE_FORMAT('DATE') ";
                sSql += "                   , UFN_DATE_FORMAT('TIME') )";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }

            return rtnBool;
        }


        public static bool SetFwdQuotationFrt(string QuotNo , string MngtNo, string Param_Crn, string userId, DataTable dt)
        {
            if(dt.Rows.Count > 0)
            {
                if (1==1)
                {
                    for(int i = 0;i< dt.Rows.Count; i++)
                    {
                        sSql = "";
                        sSql += "INSERT INTO META_QUOT_FRT (";

                        sSql += "                             QUOT_NO ";
                        sSql += "                           , MNGT_NO ";
                        sSql += "                           , CRN ";
                        sSql += "                           , FARE_SEQ ";

                        if (!string.IsNullOrEmpty(dt.Rows[i]["FARE_CD"].ToString())) sSql += "    , FARE_CD ";
                        if (!string.IsNullOrEmpty(dt.Rows[i]["FARE_NM"].ToString())) sSql += "    , FARE_NM ";
                        if (!string.IsNullOrEmpty(dt.Rows[i]["CURR_CD"].ToString())) sSql += "    , CURR_CD ";

                        if (!string.IsNullOrEmpty(dt.Rows[i]["PKG_UNIT"].ToString())) sSql += "    , PKG_UNIT ";
                        if (!string.IsNullOrEmpty(dt.Rows[i]["PKG"].ToString())) sSql += "    , PKG ";
                        if (!string.IsNullOrEmpty(dt.Rows[i]["UNIT_PRC"].ToString())) sSql += "    , UNIT_PRC ";

                        if (!string.IsNullOrEmpty(dt.Rows[i]["FARE_AMT"].ToString())) sSql += "    , FARE_AMT ";
                        if (!string.IsNullOrEmpty(dt.Rows[i]["FARE_LOC_AMT"].ToString())) sSql += "    , FARE_LOC_AMT ";
                        if (!string.IsNullOrEmpty(dt.Rows[i]["FARE_VAT_AMT"].ToString())) sSql += "    , FARE_VAT_AMT ";
                        

                        sSql += "                           , INS_USR ";
                        sSql += "                           , INS_YMD ";
                        sSql += "                           , INS_HM) ";

                        sSql += " VALUES( ";
                        
                        sSql += "             '"+QuotNo+"' ";
                        sSql += "           , '" + MngtNo + "' ";
                        sSql += "           , '" + Param_Crn + "' ";
                        sSql += "           , '"+ (i + 1) + "' ";

                        if (!string.IsNullOrEmpty(dt.Rows[i]["FARE_CD"].ToString())) sSql += "    , '" + dt.Rows[i]["FARE_CD"].ToString() + "' ";
                        if (!string.IsNullOrEmpty(dt.Rows[i]["FARE_NM"].ToString())) sSql += "    , '" + dt.Rows[i]["FARE_NM"].ToString() + "' ";
                        if (!string.IsNullOrEmpty(dt.Rows[i]["CURR_CD"].ToString())) sSql += "    , '" + dt.Rows[i]["CURR_CD"].ToString() + "' ";


                        if (!string.IsNullOrEmpty(dt.Rows[i]["PKG_UNIT"].ToString())) sSql += "    , '" + dt.Rows[i]["PKG_UNIT"].ToString() + "' ";
                        if (!string.IsNullOrEmpty(dt.Rows[i]["PKG"].ToString())) sSql += "    , '" + dt.Rows[i]["PKG"].ToString() + "' ";
                        if (!string.IsNullOrEmpty(dt.Rows[i]["UNIT_PRC"].ToString())) sSql += "    , '" + dt.Rows[i]["UNIT_PRC"].ToString() + "' ";

                        if (!string.IsNullOrEmpty(dt.Rows[i]["FARE_AMT"].ToString())) sSql += "    , '" + dt.Rows[i]["FARE_AMT"].ToString() + "' ";
                        if (!string.IsNullOrEmpty(dt.Rows[i]["FARE_LOC_AMT"].ToString())) sSql += "    , '" + dt.Rows[i]["FARE_LOC_AMT"].ToString() + "' ";
                        if (!string.IsNullOrEmpty(dt.Rows[i]["FARE_VAT_AMT"].ToString())) sSql += "    , '" + dt.Rows[i]["FARE_VAT_AMT"].ToString() + "' ";


                        sSql += "           , '"+ userId + "' ";
                        sSql += "           , UFN_DATE_FORMAT('DATE') ";
                        sSql += "           , UFN_DATE_FORMAT('TIME') ) ";


                        int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                        if (rtnStatus >= 0) rtnBool = true;
                    }
                    
                }
            }

            return rtnBool;
        }

        /// <summary>
        /// 수정중
        /// </summary>
        /// <param name="QuotNo"></param>
        /// <param name="MngtNo"></param>
        /// <param name="Param_Crn"></param>
        /// <param name="userId"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool SetFwdQuotationCnt(string QuotNo, string MngtNo, string Param_Crn, string userId, DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                if (1 == 1)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sSql = "";
                        sSql += "INSERT INTO META_QUOT_CNTR (";

                        sSql += "                             QUOT_NO ";
                        sSql += "                           , MNGT_NO ";
                        sSql += "                           , CRN ";
                        sSql += "                           , CNTR_SEQ ";




                        sSql += "                           , INS_USR ";
                        sSql += "                           , INS_YMD ";
                        sSql += "                           , INS_HM) ";

                        sSql += " VALUES( ";

                        sSql += "             '" + QuotNo + "' ";
                        sSql += "           , '" + MngtNo + "' ";
                        sSql += "           , '" + Param_Crn + "' ";
                        sSql += "           , '" + (i + 1) + "' ";

                       


                        sSql += "           , '" + userId + "' ";
                        sSql += "           , UFN_DATE_FORMAT('DATE') ";
                        sSql += "           , UFN_DATE_FORMAT('TIME') ) ";


                        int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                        if (rtnStatus >= 0) rtnBool = true;
                    }

                }
            }

            return rtnBool;
        }



        public static DataTable GetFwdQuotHeader(string MngtNo , string Param_Crn)
        {
            if (!string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(Param_Crn))
            {
                sSql = "";

                sSql += "SELECT A.QUOT_NO ";
                sSql += " , A.MNGT_NO ";
                sSql += " , MAX(FWD_CRN) AS FWD_CRN ";
                sSql += " , MAX(FQM.POL_CD) AS POL_CD ";
                sSql += " , MAX(FQM.POL_NM) AS POL_NM ";
                sSql += " , MAX(A.ETD) AS ETD ";
                sSql += " , MAX(FQM.POD_CD) AS POD_CD ";
                sSql += " , MAX(FQM.POD_NM) AS POD_NM ";
                sSql += " , MAX(A.ETA) AS ETA ";
                sSql += " , MAX(FQM.INS_YMD) AS REQ_YMD ";
                sSql += " , MAX(A.QUOT_YMD) AS QUOT_YMD ";
                sSql += " , MAX(FQM.ITEM_NM) AS ITEM_NM ";
                sSql += " , MAX(CUST_PIC_NM) AS CUST_PIC_NM ";
                sSql += " , WM_CONCAT(CURR_CD) AS CURR_CD ";
                sSql += " , WM_CONCAT(FARE_AMT) AS FARE_AMT ";
                sSql += " , WM_CONCAT(FARE_LOC_AMT) AS FARE_LOC_AMT ";
                sSql += " FROM (SELECT MAX(FQD.QUOT_NO) AS QUOT_NO ";
                sSql += "               , FQD.MNGT_NO ";
                sSql += "               , MAX(FQD.CRN) AS FWD_CRN ";
                sSql += "               , MAX(FQD.POL_CD) AS POL_CD ";
                sSql += "               , MAX(FQD.POL_NM) AS POL_NM ";
                sSql += "               , MAX(FQD.ETD) AS ETD ";
                sSql += "               , MAX(FQD.POD_CD) AS POD_CD ";
                sSql += "               , MAX(FQD.POD_NM) AS POD_NM ";
                sSql += "               , MAX(FQD.ETA) AS ETA ";
                sSql += "               , MAX(FQD.INS_YMD) AS QUOT_YMD ";
                sSql += "               , MAX(FQD.ITEM_CD) AS ITEM_NM ";
                sSql += "               , MAX(FQD.CUST_PIC_NM) AS CUST_PIC_NM ";
                sSql += "               , FQF.CURR_CD ";
                sSql += "               , SUM(FQF.FARE_AMT) AS FARE_AMT ";
                sSql += "               , SUM(FQF.FARE_LOC_AMT) AS FARE_LOC_AMT  ";
                sSql += "         FROM META_QUOT_DTL FQD ";
                sSql += "         LEFT OUTER JOIN META_QUOT_FRT FQF ";
                sSql += "           ON FQD.MNGT_NO = FQF.MNGT_NO ";
                sSql += "         WHERE 1=1 ";
                sSql += "           GROUP BY FQD.MNGT_NO ,FQF.CURR_CD) A ";
                sSql += "   LEFT JOIN META_QUOT_MST FQM ";
                sSql += "       ON A.QUOT_NO = FQM.QUOT_NO ";
                sSql += "   WHERE 1=1 ";
                sSql += "   AND FWD_CRN = '"+Param_Crn+"' ";
                sSql += "   AND A.MNGT_NO ='"+MngtNo+"' ";
                sSql += "   GROUP BY A.QUOT_NO , A.MNGT_NO ";

                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
                rtnDt.TableName = "HEADER";
            }


            return rtnDt;
        }

        public static DataTable GetFQuotList(string Crn) {
            sSql = "";
            sSql += "       SELECT MAX(A.QUOT_NO) QUOT_NO                                                                  ";
            sSql += "   ,   MAX(A.POL_NM) POL_NM                                                                           ";
            sSql += "   ,   MAX(A.ETD) ETD                                                                                 ";
            sSql += "   ,   MAX(A.POD_NM) POD_NM                                                                           ";
            sSql += "   ,   MAX(A.ETA) ETA                                                                                 ";
            sSql += "   ,   MAX(A.REQ_YMD) REQ_YMD                                                                         ";
            sSql += "   ,   MAX(A.ITEM_NM) ITEM_NM                                                                         ";
            sSql += "   ,   MAX(FQD.INS_YMD) QUOT_YMD                                                                      ";
            sSql += "   , (SELECT COUNT(QUOT_NO) FROM META_QUOT_DTL WHERE QUOT_NO = A.QUOT_NO AND REQ_SVC = 'SEA') SEA_CNT ";
            sSql += "   , (SELECT COUNT(QUOT_NO) FROM META_QUOT_DTL WHERE QUOT_NO = A.QUOT_NO AND REQ_SVC = 'AIR') AIR_CNT ";
            sSql += "   , (SELECT COUNT(*) FROM META_DOC_MST WHERE DOC_TYPE = 'CI' AND ORG_MNGT_NO = A.QUOT_NO) AS CI_CNT  ";
            sSql += "   , (SELECT COUNT(*) FROM META_DOC_MST WHERE DOC_TYPE = 'PL' AND ORG_MNGT_NO = A.QUOT_NO) AS PL_CNT  ";
            sSql += " FROM ( ";

            sSql += "       SELECT FQM.QUOT_NO            ";
            sSql += "       , FQM.POL_NM                  ";
            sSql += "       , FQM.ETD                     ";
            sSql += "       , FQM.POD_NM                  ";
            sSql += "       , FQM.ETA                     ";
            sSql += "       , (FQM.INS_YMD)AS REQ_YMD     ";
            sSql += "       , FQM.ITEM_NM                 ";
            sSql += "       FROM META_QUOT_CRN FQC        ";
            sSql += "       INNER JOIN  META_QUOT_MST FQM  ";
            sSql += "       ON FQM.QUOT_NO = FQC.QUOT_NO  ";
            sSql += "       AND FQC.CRN = '"+Crn+"') A ";
            sSql += " LEFT JOIN META_QUOT_DTL FQD ";
            sSql += "       ON A.QUOT_NO = FQD.QUOT_NO ";
            sSql += "   GROUP BY A.QUOT_NO ";
            sSql += "   ORDER BY REQ_YMD DESC ";

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);

            return rtnDt;
        }


        public static DataTable GetFwdQuotDtl(string MngtNo, string Param_Crn)
        {
            if (!string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(Param_Crn))
            {
                sSql = "";
                sSql += "SELECT FQD.MNGT_NO ";
                sSql += "     , FQD.ETD ";
                sSql += "     , FQD.ETA ";
                sSql += "     , FQD.VLDT_YMD ";
                sSql += "     , FQD.REQ_SVC ";
                sSql += "     , FQD.VSL ";
                sSql += "     , FQD.CNTR_TYPE ";
                sSql += "     , FQD.PKG ";
                sSql += "     , FQD.GRS_WGT ";
                sSql += "     , FQD.CBM ";
                sSql += "     , FQD.CNTR_NO";
                sSql += " FROM META_QUOT_DTL FQD ";
                sSql += " WHERE 1=1 ";
                sSql += " AND CRN = '"+ Param_Crn + "'";
                sSql += " AND MNGT_NO = '" + MngtNo + "'";




                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
                rtnDt.TableName = "DETAIL";
            }


            return rtnDt;
        }
        public static DataTable GetFwdQuotFrt(string MngtNo, string Param_Crn)
        {
            if (!string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(Param_Crn))
            {
                sSql = "";
                sSql += "SELECT FARE_CD ";
                sSql += "       , FARE_NM ";
                sSql += "       , CURR_CD ";
                sSql += "       , PKG_UNIT ";
                sSql += "       , PKG ";
                sSql += "       , UNIT_PRC ";
                sSql += "       , FARE_AMT ";
                sSql += "       , FARE_LOC_AMT ";
                sSql += "       , FARE_VAT_AMT ";
                sSql += "       , (FARE_LOC_AMT)AS TOT_AMT ";
                sSql += "FROM META_QUOT_FRT FQD";
                sSql += "   WHERE MNGT_NO ='"+ MngtNo + "' ";
                sSql += "       AND CRN = '"+ Param_Crn + "' ";
                sSql += "   ORDER BY FARE_SEQ ";


                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
                rtnDt.TableName = "FRT";
            }


            return rtnDt;
        }

        public static DataTable GetFwdQuotCntr(string MngtNo, string Param_Crn)
        {
            if (!string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(Param_Crn))
            {
                sSql = "";



                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
                rtnDt.TableName = "CNTR";
            }


            return rtnDt;
        }


    }
}