using System.Data;

namespace META_DATA_API.Models.Query
{
    public static class Sql_ExImData
    {
        private static string sSql = "";
        private static bool rtnBool = false;
        private static DataTable rtnDt = null;

        // ELVIS 업무 정보를 저장한다
        public static bool SetBLInfo(string Crn, string HblNo, string ExImType, DataRow ParamInfo)
        {
            if (!string.IsNullOrEmpty(Crn) && !string.IsNullOrEmpty(HblNo) && !string.IsNullOrEmpty(ExImType) && ParamInfo != null)
            {
                sSql = "";
                sSql += "MERGE INTO META_HBL_STATUS USING DUAL ON ( ";
                sSql += "      CRN = '" + Crn + "' ";
                sSql += "  AND HBL_NO = '" + HblNo + "' ";
                sSql += "  AND EX_IM_TYPE = '" + ExImType + "') ";

                sSql += "WHEN MATCHED THEN ";
                sSql += "UPDATE SET UPD_USR = '" + ParamInfo["INS_USR"].ToString() + "' ";
                sSql += "         , UPD_YMD = UFN_DATE_FORMAT('DATE') ";
                sSql += "         , UPD_HM = UFN_DATE_FORMAT('TIME') ";
                if (ParamInfo.Table.Columns.Contains("MBL_NO")) sSql += ", MBL_NO = '" + ParamInfo["MBL_NO"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("CUST_CRN")) sSql += ", CUST_CRN = '" + ParamInfo["CUST_CRN"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("OFFICE_CD")) sSql += ", OFFICE_CD = '" + ParamInfo["OFFICE_CD"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("REQ_SVC")) sSql += ", REQ_SVC = '" + ParamInfo["REQ_SVC"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("POL_CD")) sSql += ", POL_CD = '" + ParamInfo["POL_CD"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("POL_NM")) sSql += ", POL_NM = '" + ParamInfo["POL_NM"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("POD_CD")) sSql += ", POD_CD = '" + ParamInfo["POD_CD"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("POD_NM")) sSql += ", POD_NM = '" + ParamInfo["POD_NM"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ETD")) sSql += ", ETD = '" + ParamInfo["ETD"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ETD_HM")) sSql += ", ETD_HM = '" + ParamInfo["ETD_HM"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ETA")) sSql += ", ETA = '" + ParamInfo["ETA"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ETA_HM")) sSql += ", ETA_HM = '" + ParamInfo["ETA_HM"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ETDA")) sSql += ", ETDA = '" + ParamInfo["ETDA"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ETDA_HM")) sSql += ", ETDA_HM = '" + ParamInfo["ETDA_HM"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("PKG")) sSql += ", PKG = " + ParamInfo["PKG"].ToString() + " ";
                if (ParamInfo.Table.Columns.Contains("PKG_UNIT_CD")) sSql += ", PKG_UNIT_CD = '" + ParamInfo["PKG_UNIT_CD"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("GRS_WGT")) sSql += ", GRS_WGT = " + ParamInfo["GRS_WGT"].ToString() + " ";
                if (ParamInfo.Table.Columns.Contains("CHB_WGT")) sSql += ", CHB_WGT = " + ParamInfo["CHB_WGT"].ToString() + " ";
                if (ParamInfo.Table.Columns.Contains("VOL_WGT")) sSql += ", VOL_WGT = " + ParamInfo["VOL_WGT"].ToString() + " ";
                if (ParamInfo.Table.Columns.Contains("MSRMT")) sSql += ", MSRMT = " + ParamInfo["MSRMT"].ToString() + " ";
                if (ParamInfo.Table.Columns.Contains("CNTR_CNT_TEU")) sSql += ", CNTR_CNT_TEU = " + ParamInfo["CNTR_CNT_TEU"].ToString() + " ";
                if (ParamInfo.Table.Columns.Contains("ED_CNT")) sSql += ", ED_CNT = " + ParamInfo["ED_CNT"].ToString() + " ";
                if (ParamInfo.Table.Columns.Contains("DOC_CNT")) sSql += ", DOC_CNT = " + ParamInfo["DOC_CNT"].ToString() + " ";
                if (ParamInfo.Table.Columns.Contains("BL_SEND_DT")) sSql += ", BL_SEND_DT = '" + ParamInfo["BL_SEND_DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("INV_SEND_DT")) sSql += ", INV_SEND_DT = '" + ParamInfo["INV_SEND_DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("MFCS_SEND_DT")) sSql += ", MFCS_SEND_DT = '" + ParamInfo["MFCS_SEND_DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("FWB_SEND_DT")) sSql += ", FWB_SEND_DT = '" + ParamInfo["FWB_SEND_DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("AMS_SEND_DT")) sSql += ", AMS_SEND_DT = '" + ParamInfo["AMS_SEND_DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("AFR_SEND__DT")) sSql += ", AFR_SEND__DT = '" + ParamInfo["AFR_SEND__DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("DCD_SEND__DT")) sSql += ", DCD_SEND__DT = '" + ParamInfo["DCD_SEND__DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("DO_SEND_DT")) sSql += ", DO_SEND_DT = '" + ParamInfo["DO_SEND_DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("TAX_SEND_DT")) sSql += ", TAX_SEND_DT = '" + ParamInfo["TAX_SEND_DT"].ToString() + "' ";

                sSql += "WHEN NOT MATCHED THEN ";
                sSql += "INSERT ( ";
                sSql += "       CRN ";
                sSql += "     , HBL_NO ";
                sSql += "     , EX_IM_TYPE ";
                if (ParamInfo.Table.Columns.Contains("MBL_NO")) sSql += ", MBL_NO ";
                if (ParamInfo.Table.Columns.Contains("CUST_CRN")) sSql += ", CUST_CRN ";
                if (ParamInfo.Table.Columns.Contains("OFFICE_CD")) sSql += ", OFFICE_CD ";
                if (ParamInfo.Table.Columns.Contains("REQ_SVC")) sSql += ", REQ_SVC ";
                if (ParamInfo.Table.Columns.Contains("POL_CD")) sSql += ", POL_CD ";
                if (ParamInfo.Table.Columns.Contains("POL_NM")) sSql += ", POL_NM ";
                if (ParamInfo.Table.Columns.Contains("POD_CD")) sSql += ", POD_CD ";
                if (ParamInfo.Table.Columns.Contains("POD_NM")) sSql += ", POD_NM ";
                if (ParamInfo.Table.Columns.Contains("ETD")) sSql += ", ETD ";
                if (ParamInfo.Table.Columns.Contains("ETD_HM")) sSql += ", ETD_HM ";
                if (ParamInfo.Table.Columns.Contains("ETA")) sSql += ", ETA ";
                if (ParamInfo.Table.Columns.Contains("ETA_HM")) sSql += ", ETA_HM ";
                if (ParamInfo.Table.Columns.Contains("ETDA")) sSql += ", ETDA ";
                if (ParamInfo.Table.Columns.Contains("ETDA_HM")) sSql += ", ETDA_HM ";
                if (ParamInfo.Table.Columns.Contains("PKG")) sSql += ", PKG ";
                if (ParamInfo.Table.Columns.Contains("PKG_UNIT_CD")) sSql += ", PKG_UNIT_CD ";
                if (ParamInfo.Table.Columns.Contains("GRS_WGT")) sSql += ", GRS_WGT ";
                if (ParamInfo.Table.Columns.Contains("CHB_WGT")) sSql += ", CHB_WGT ";
                if (ParamInfo.Table.Columns.Contains("VOL_WGT")) sSql += ", VOL_WGT ";
                if (ParamInfo.Table.Columns.Contains("MSRMT")) sSql += ", MSRMT ";
                if (ParamInfo.Table.Columns.Contains("CNTR_CNT_TEU")) sSql += ", CNTR_CNT_TEU ";
                if (ParamInfo.Table.Columns.Contains("ED_CNT")) sSql += ", ED_CNT ";
                if (ParamInfo.Table.Columns.Contains("DOC_CNT")) sSql += ", DOC_CNT ";
                if (ParamInfo.Table.Columns.Contains("BL_SEND_DT")) sSql += ", BL_SEND_DT ";
                if (ParamInfo.Table.Columns.Contains("INV_SEND_DT")) sSql += ", INV_SEND_DT ";
                if (ParamInfo.Table.Columns.Contains("MFCS_SEND_DT")) sSql += ", MFCS_SEND_DT ";
                if (ParamInfo.Table.Columns.Contains("FWB_SEND_DT")) sSql += ", FWB_SEND_DT ";
                if (ParamInfo.Table.Columns.Contains("AMS_SEND_DT")) sSql += ", AMS_SEND_DT ";
                if (ParamInfo.Table.Columns.Contains("AFR_SEND__DT")) sSql += ", AFR_SEND__DT ";
                if (ParamInfo.Table.Columns.Contains("DCD_SEND__DT")) sSql += ", DCD_SEND__DT ";
                if (ParamInfo.Table.Columns.Contains("DO_SEND_DT")) sSql += ", DO_SEND_DT ";
                if (ParamInfo.Table.Columns.Contains("TAX_SEND_DT")) sSql += ", TAX_SEND_DT ";
                sSql += ", INS_USR ";
                sSql += ", INS_YMD ";
                sSql += ", INS_HM) ";

                sSql += "VALUES( ";
                sSql += "  '" + Crn + "' ";
                sSql += ", '" + HblNo + "' ";
                sSql += ", '" + ExImType + "' ";
                if (ParamInfo.Table.Columns.Contains("MBL_NO")) sSql += ", '" + ParamInfo["MBL_NO"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("CUST_CRN")) sSql += ", '" + ParamInfo["CUST_CRN"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("OFFICE_CD")) sSql += ", '" + ParamInfo["OFFICE_CD"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("REQ_SVC")) sSql += ", '" + ParamInfo["REQ_SVC"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("POL_CD")) sSql += ", '" + ParamInfo["POL_CD"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("POL_NM")) sSql += ", '" + ParamInfo["POL_NM"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("POD_CD")) sSql += ", '" + ParamInfo["POD_CD"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("POD_NM")) sSql += ", '" + ParamInfo["POD_NM"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ETD")) sSql += ", '" + ParamInfo["ETD"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ETD_HM")) sSql += ", '" + ParamInfo["ETD_HM"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ETA")) sSql += ", '" + ParamInfo["ETA"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ETA_HM")) sSql += ", '" + ParamInfo["ETA_HM"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ETDA")) sSql += ", '" + ParamInfo["ETDA"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ETDA_HM")) sSql += ", '" + ParamInfo["ETDA_HM"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("PKG")) sSql += ", '" + ParamInfo["PKG"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("PKG_UNIT_CD")) sSql += ", '" + ParamInfo["PKG_UNIT_CD"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("GRS_WGT")) sSql += ", '" + ParamInfo["GRS_WGT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("CHB_WGT")) sSql += ", '" + ParamInfo["CHB_WGT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("VOL_WGT")) sSql += ", '" + ParamInfo["VOL_WGT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("MSRMT")) sSql += ", '" + ParamInfo["MSRMT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("CNTR_CNT_TEU")) sSql += ", '" + ParamInfo["CNTR_CNT_TEU"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ED_CNT")) sSql += ", '" + ParamInfo["ED_CNT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("DOC_CNT")) sSql += ", '" + ParamInfo["DOC_CNT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("BL_SEND_DT")) sSql += ", '" + ParamInfo["BL_SEND_DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("INV_SEND_DT")) sSql += ", '" + ParamInfo["INV_SEND_DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("MFCS_SEND_DT")) sSql += ", '" + ParamInfo["MFCS_SEND_DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("FWB_SEND_DT")) sSql += ", '" + ParamInfo["FWB_SEND_DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("AMS_SEND_DT")) sSql += ", '" + ParamInfo["AMS_SEND_DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("AFR_SEND__DT")) sSql += ", '" + ParamInfo["AFR_SEND__DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("DCD_SEND__DT")) sSql += ", '" + ParamInfo["DCD_SEND__DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("DO_SEND_DT")) sSql += ", '" + ParamInfo["DO_SEND_DT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("TAX_SEND_DT")) sSql += ", '" + ParamInfo["TAX_SEND_DT"].ToString() + "' ";
                sSql += ", '" + ParamInfo["INS_USR"].ToString() + "' ";
                sSql += ", UFN_DATE_FORMAT('DATE') ";
                sSql += ", UFN_DATE_FORMAT('TIME')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }

            return rtnBool;
        }

        // ELVIS 에서 생성한 문서 내역을 저장한다
        public static bool SetDocPath(string MngtNo, string Crn, string SvrFileNm, string FileSize, string FileNm, string FileExt, string FilePath, string DocType, string OrgMngtNo)
        {
            if (!string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(Crn) && !string.IsNullOrEmpty(DocType) && !string.IsNullOrEmpty(SvrFileNm) && !string.IsNullOrEmpty(FileSize)
                && !string.IsNullOrEmpty(FileNm) && !string.IsNullOrEmpty(FilePath) && !string.IsNullOrEmpty(FileExt) && !string.IsNullOrEmpty(OrgMngtNo))
            {
                sSql = "";
                sSql += "MERGE INTO META_DOC_MST USING DUAL ON ( ";
                sSql += "      MNGT_NO = '" + MngtNo + "' ";
                sSql += "  AND CRN = '" + Crn + "'";
                sSql += "  AND DOC_TYPE = '" + DocType + "') ";

                sSql += "WHEN MATCHED THEN ";
                sSql += "UPDATE SET ";
                sSql += "  FILE_NM = '" + _Sec_Encrypt.decryptAES256(FileNm) + "' ";
                sSql += ", SVR_FILE_NM = '" + _Sec_Encrypt.decryptAES256(SvrFileNm) + "' ";
                sSql += ", FILE_SIZE = '" + _Sec_Encrypt.decryptAES256(FileSize) + "' ";
                sSql += ", FILE_PATH = '" + _Sec_Encrypt.decryptAES256(FilePath) + "' ";
                sSql += ", FILE_EXT = '" + _Sec_Encrypt.decryptAES256(FileExt) + "' ";
                sSql += ", UPD_USR = 'SYSTEM' ";
                sSql += ", UPD_YMD = UFN_DATE_FORMAT('DATE') ";
                sSql += ", UPD_HM = UFN_DATE_FORMAT('TIME') ";

                sSql += "WHEN NOT MATCHED THEN ";
                sSql += "INSERT ( ";
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
                sSql += ", '" + _Sec_Encrypt.decryptAES256(FileNm) + "' ";
                sSql += ", '" + _Sec_Encrypt.decryptAES256(SvrFileNm) + "' ";
                sSql += ", '" + _Sec_Encrypt.decryptAES256(FileSize) + "' ";
                sSql += ", '" + _Sec_Encrypt.decryptAES256(FileExt) + "' ";
                sSql += ", '" + _Sec_Encrypt.decryptAES256(FilePath) + "' ";
                sSql += ", '" + DocType + "' ";
                sSql += ", '" + OrgMngtNo + "' ";
                sSql += ", 'SYSTEM' ";
                sSql += ", UFN_DATE_FORMAT('DATE') ";
                sSql += ", UFN_DATE_FORMAT('TIME') ";
                sSql += ", 'SYSTEM' ";
                sSql += ", UFN_DATE_FORMAT('DATE') ";
                sSql += ", UFN_DATE_FORMAT('TIME')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }

            return rtnBool;
        }

        // 문서 내역을 조회한다
        public static DataTable GetDocPath(string MngtNo, string Crn, string DocType)
        {
            if (!string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(Crn))
            {
                sSql = "";
                sSql += " SELECT FDM.DOC_TYPE ";
                sSql += "      , FTM.TMPLT_NM ";
                sSql += "      , FDM.FILE_NM AS FILE_NM ";
                sSql += "      , FDM.FILE_PATH || FDM.FILE_NM AS FILE_PATH ";
                sSql += "   FROM META_DOC_MST FDM ";
                sSql += "   LEFT OUTER JOIN META_OCR_MST FOM ";
                sSql += "     ON FDM.OCR_NO = FOM.MNGT_NO ";
                sSql += "   LEFT OUTER JOIN META_TMPLT_MST FTM ";
                sSql += "     ON FOM.TMPLT_ID = FTM.TMPLT_ID ";
                sSql += "  WHERE FDM.ORG_MNGT_NO = '" + MngtNo +"' ";
                sSql += "    AND FDM.CRN = '" + Crn + "' ";
                if (!string.IsNullOrEmpty(DocType)) sSql += " AND FDM.DOC_TYPE IN ('" + DocType.Replace(",", "','") + "') ";

                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);                
            }

            return rtnDt;
        }

        // 수출입 리스트를 조회한다
        public static DataTable GetExImList(string Crn, string ExImType, string YmdType, string FmYmd, string ToYmd, string ReqSvc, string PolCd, string PodCd, string UsrType)
        {
            if (!string.IsNullOrEmpty(Crn) && !string.IsNullOrEmpty(ExImType) && !string.IsNullOrEmpty(YmdType) && !string.IsNullOrEmpty(FmYmd) && !string.IsNullOrEmpty(ToYmd) && !string.IsNullOrEmpty(UsrType))
            {
                sSql = "";
                sSql += " SELECT CRN ";
                sSql += "      , REQ_SVC ";
                sSql += "      , HBL_NO ";
                sSql += "      , MBL_NO ";
                sSql += "      , EX_IM_TYPE ";
                sSql += "      , POL_CD ";
                sSql += "      , POL_NM ";
                sSql += "      , POD_CD ";
                sSql += "      , POD_NM ";
                sSql += "      , ETD ";
                sSql += "      , ETD_HM ";
                sSql += "      , ETA ";
                sSql += "      , ETA_HM ";
                sSql += "      , BL_SEND_DT ";
                sSql += "      , INV_SEND_DT ";
                sSql += "      , MFCS_SEND_DT ";
                sSql += "      , DO_SEND_DT ";
                sSql += "      , TAX_SEND_DT ";
                sSql += "   FROM META_HBL_STATUS ";
                sSql += "  WHERE 1=1 ";
                if(UsrType == "F")//실행사
                {
                    sSql += "    AND CRN = '" + Crn + "' ";
                }
                else //화주
                {
                    sSql += "    AND CUST_CRN = '" + Crn + "' ";
                }
                
                sSql += "    AND EX_IM_TYPE IN ('" + ExImType.Replace(",","','") + "') ";                
                sSql += "    AND " + YmdType + " BETWEEN " + FmYmd + " AND " + ToYmd;
                if (!string.IsNullOrEmpty(ReqSvc)) sSql += " AND REQ_SVC = '" + ReqSvc + "' ";
                if (!string.IsNullOrEmpty(PolCd)) sSql += " AND POL_CD = '" + PolCd + "' ";
                if (!string.IsNullOrEmpty(PodCd)) sSql += " AND POD_CD = '" + PodCd + "' ";
                sSql += "  ORDER BY ETD ";

                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            }

            return rtnDt;
        }

        public static DataTable GetTrackingList(string MngtNo)
        {
            if (!string.IsNullOrEmpty(MngtNo))
            {
                
                sSql = "";
                sSql += " SELECT MAX(RTM.REQ_SVC) REQ_SVC,";
                sSql += "       MAX(RTM.EX_IM_TYPE) EX_IM_TYPE,";
                sSql += "       MAX(ACT_LOC_NM) ACT_LOC_NM,";
                sSql += "       MAX(ACT_YMD) ACT_YMD,";
                sSql += "       MAX(ACT_HM) ACT_HM,";
                sSql += "       MAX(SEQ) SEQ,";
                sSql += "       EVENT_CD,";
                sSql += "       MAX(MCC.CD_NM) EVENT_NM,";
                sSql += "       MAX(LAST_EVENT_CD) LAST_EVENT_CD ,";
                sSql += "       MAX(LAST_EVENT_NM) LAST_EVENT_NM,";
                sSql += "       MAX(RTM.MBL_NO) MBL_NO,";
                sSql += "       MAX(RTM.CNTR_NO) CNTR_NO";
                sSql += "  FROM RPA_TRK_MST@DL_RPA RTM";
                sSql += "   LEFT OUTER JOIN RPA_TRK_AIS@DL_RPA RTA";
                sSql += "    ON RTM.HBL_NO = RTA.HBL_NO AND RTM.CNTR_NO = RTA.CNTR_NO";
                sSql += "   LEFT OUTER JOIN MDM_COM_CODE MCC";
                sSql += "    ON RTA.EVENT_CD = MCC.COMN_CD AND MCC.OPT_ITEM4 = RTM.REQ_SVC";
                sSql += "    WHERE (RTM.HBL_NO = '"+MngtNo+"' OR RTM.CNTR_NO = '"+MngtNo+"')";
                sSql += "    GROUP BY EVENT_CD";
                sSql += "    ORDER BY SEQ";

                rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            }

            return rtnDt;
        }
    }
}