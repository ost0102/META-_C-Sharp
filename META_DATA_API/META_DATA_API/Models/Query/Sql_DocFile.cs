using System.Data;

namespace META_DATA_API.Models.Query
{
    public static class Sql_DocFile
    {
        private static string sSql = "";
        private static bool rtnBool = false;
        private static DataTable rtnDt = null;

        // 회원 정보를 등록한다
        public static bool SetDocFilePath(string MngtNo, string DocType, string FileName, string FilePath, string FileExtension)
        {
            if (!string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(DocType) && !string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(FilePath) && !string.IsNullOrEmpty(FileExtension))
            {
                sSql = "";
                sSql += "MERGE INTO META_DOC_MST USING DUAL ON ( ";
                sSql += "    MNGT_NO = '" + MngtNo + "' ";
                sSql += "AND DOC_TYPE = '" + DocType + "') ";

                sSql += "WHEN MATCHED THEN ";
                sSql += "UPDATE SET ";
                sSql += "  FILE_NM = '" + FileName + "' ";
                sSql += ", FILE_PATH = '" + FilePath + "' ";
                sSql += ", FILE_EXTENSION = '" + FileExtension + "' ";
                sSql += ", ENCRY_FILE_NM = '" + _Sec_Encrypt.encryptAES256(FileName) + "' ";
                sSql += ", ENCRY_FILE_PATH = '" + _Sec_Encrypt.encryptAES256(FilePath) + "' ";
                sSql += ", ENCRY_FILE_EXTENSION = '" + _Sec_Encrypt.encryptAES256(FileExtension) + "' ";
                sSql += ", UPD_USR = 'SYSTEM' ";
                sSql += ", UPD_YMD = UFN_DATE_FORMAT('DATE') ";
                sSql += ", UPD_HM = UFN_DATE_FORMAT('TIME') ";

                sSql += "WHEN NOT MATCHED THEN ";
                sSql += "INSERT ( ";
                sSql += "  MNGT_NO ";
                sSql += ", DOC_TYPE ";
                sSql += ", FILE_NM ";
                sSql += ", FILE_PATH ";
                sSql += ", FILE_EXTENSION ";
                sSql += ", ENCRY_FILE_NM ";
                sSql += ", ENCRY_FILE_PATH ";
                sSql += ", ENCRY_FILE_EXTENSION ";
                sSql += ", INS_USR ";
                sSql += ", INS_YMD ";
                sSql += ", INS_HM ";
                sSql += ", UPD_USR ";
                sSql += ", UPD_YMD ";
                sSql += ", UPD_HM) ";

                sSql += "VALUES( ";
                sSql += "  '" + MngtNo + "' ";
                sSql += ", '" + DocType + "' ";
                sSql += ", '" + FileName + "' ";
                sSql += ", '" + FilePath + "' ";
                sSql += ", '" + FileExtension + "' ";
                sSql += ", '" + _Sec_Encrypt.encryptAES256(FileName) + "' ";
                sSql += ", '" + _Sec_Encrypt.encryptAES256(FilePath) + "' ";
                sSql += ", '" + _Sec_Encrypt.encryptAES256(FileExtension) + "' ";
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

        // Saas 서비스 신청 정보를 등록한다
        public static bool SetSaasSvcRegist(DataRow svcInfo)
        {
            if (!string.IsNullOrEmpty(svcInfo["ELVIS_USR_ID"].ToString()) && !string.IsNullOrEmpty(svcInfo["ELVIS_PSWD"].ToString())
                && !string.IsNullOrEmpty(svcInfo["CRN"].ToString()) && !string.IsNullOrEmpty(svcInfo["OFFICE_NM"].ToString())
                && !string.IsNullOrEmpty(svcInfo["LOC_NM"].ToString()) && !string.IsNullOrEmpty(svcInfo["OFFICE_ADDR"].ToString())
                && !string.IsNullOrEmpty(svcInfo["EMAIL"].ToString()) && !string.IsNullOrEmpty(svcInfo["HP_NO"].ToString())
                && !string.IsNullOrEmpty(svcInfo["REG_DATE"].ToString()) && !string.IsNullOrEmpty(svcInfo["STAT_DATE"].ToString()))
            {
                sSql = "";
                sSql += "INSERT INTO PRM_ELVIS_REG ( ";
                sSql += "  ELVIS_USR_ID ";
                sSql += ", ELVIS_PSWD ";
                sSql += ", CRN ";
                sSql += ", OFFICE_NM ";
                sSql += ", LOC_NM ";
                sSql += ", OFFICE_ADDR ";
                sSql += ", EMAIL ";
                sSql += ", HP_NO ";
                sSql += ", REG_DATE ";
                sSql += ", STAT_DATE ";

                if (svcInfo.Table.Columns.Contains("ELVIS_FMS")) sSql += " , ELVIS_FMS ";
                if (svcInfo.Table.Columns.Contains("ELVIS_WMS")) sSql += " , ELVIS_WMS ";
                if (svcInfo.Table.Columns.Contains("ELVIS_TMS")) sSql += " , ELVIS_TMS ";
                if (svcInfo.Table.Columns.Contains("ELVIS_EXPRESS")) sSql += " , ELVIS_EXPRESS ";
                if (svcInfo.Table.Columns.Contains("ELVIS_PRIME")) sSql += " , ELVIS_PRIME ";
                if (svcInfo.Table.Columns.Contains("ELVIS_BIG")) sSql += " , ELVIS_BIG ";
                if (svcInfo.Table.Columns.Contains("ELVIS_BIG_UNI")) sSql += " , ELVIS_BIG_UNI ";
                if (svcInfo.Table.Columns.Contains("ELVIS_BIG_SFI")) sSql += " , ELVIS_BIG_SFI ";
                if (svcInfo.Table.Columns.Contains("ELVIS_BIG_TER")) sSql += " , ELVIS_BIG_TER ";
                if (svcInfo.Table.Columns.Contains("ELVIS_BIG_TRK")) sSql += " , ELVIS_BIG_TRK ";
                if (svcInfo.Table.Columns.Contains("ELVIS_BIG_LAT")) sSql += " , ELVIS_BIG_LAT ";
                if (svcInfo.Table.Columns.Contains("ELVIS_FRIEND")) sSql += " , ELVIS_FRIEND ";
                if (svcInfo.Table.Columns.Contains("ELVIS_FRIEND_DOCU")) sSql += " , ELVIS_FRIEND_DOCU ";
                if (svcInfo.Table.Columns.Contains("ELVIS_FRIEND_EXIM")) sSql += " , ELVIS_FRIEND_EXIM ";
                if (svcInfo.Table.Columns.Contains("ELVIS_FRIEND_DATA")) sSql += " , ELVIS_FRIEND_DATA ";
                if (svcInfo.Table.Columns.Contains("ELVIS_CUST_INFO")) sSql += " , ELVIS_CUST_INFO ";
                if (svcInfo.Table.Columns.Contains("ELVIS_ACCOUNT")) sSql += " , ELVIS_ACCOUNT ";
                if (svcInfo.Table.Columns.Contains("SERVICE_FAX")) sSql += " , SERVICE_FAX ";
                if (svcInfo.Table.Columns.Contains("SERVICE_SMS")) sSql += " , SERVICE_SMS ";

                sSql += ", INS_USR ";
                sSql += ", INS_YMD ";
                sSql += ", INS_HM ";
                sSql += ", UPD_USR ";
                sSql += ", UPD_YMD ";
                sSql += ", UPD_HM) ";


                sSql += "VALUES( ";
                sSql += "  '" + svcInfo["ELVIS_USR_ID"].ToString() + "' ";
                sSql += ", UNF_MD5_ENCRYPTION('" + svcInfo["ELVIS_PSWD"].ToString() + "') ";
                sSql += ", '" + svcInfo["CRN"].ToString() + "' ";
                sSql += ", '" + svcInfo["OFFICE_NM"].ToString() + "' ";
                sSql += ", '" + svcInfo["LOC_NM"].ToString() + "' ";
                sSql += ", '" + svcInfo["OFFICE_ADDR"].ToString() + "' ";
                sSql += ", '" + svcInfo["EMAIL"].ToString() + "' ";
                sSql += ", '" + svcInfo["HP_NO"].ToString() + "' ";
                sSql += ", '" + svcInfo["REG_DATE"].ToString() + "' ";
                sSql += ", '" + svcInfo["STAT_DATE"].ToString() + "' ";

                if (svcInfo.Table.Columns.Contains("ELVIS_FMS")) sSql += ", '" + svcInfo["ELVIS_FMS"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_WMS")) sSql += ", '" + svcInfo["ELVIS_WMS"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_TMS")) sSql += ", '" + svcInfo["HP_ELVIS_TMSNO"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_EXPRESS")) sSql += ", '" + svcInfo["ELVIS_EXPRESS"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_PRIME")) sSql += ", '" + svcInfo["ELVIS_PRIME"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_BIG")) sSql += ", '" + svcInfo["ELVIS_BIG"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_BIG_UNI")) sSql += ", '" + svcInfo["ELVIS_BIG_UNI"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_BIG_SFI")) sSql += ", '" + svcInfo["ELVIS_BIG_SFI"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_BIG_TER")) sSql += ", '" + svcInfo["ELVIS_BIG_TER"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_BIG_TRK")) sSql += ", '" + svcInfo["ELVIS_BIG_TRK"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_BIG_LAT")) sSql += ", '" + svcInfo["ELVIS_BIG_LAT"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_FRIEND")) sSql += ", '" + svcInfo["ELVIS_FRIEND"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_FRIEND_DOCU")) sSql += ", '" + svcInfo["ELVIS_FRIEND_DOCU"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_FRIEND_EXIM")) sSql += ", '" + svcInfo["ELVIS_FRIEND_EXIM"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_FRIEND_DATA")) sSql += ", '" + svcInfo["ELVIS_FRIEND_DATA"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_CUST_INFO")) sSql += ", '" + svcInfo["ELVIS_CUST_INFO"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("ELVIS_ACCOUNT")) sSql += ", '" + svcInfo["ELVIS_ACCOUNT"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("SERVICE_FAX")) sSql += ", '" + svcInfo["SERVICE_FAX"].ToString() + "' ";
                if (svcInfo.Table.Columns.Contains("SERVICE_SMS")) sSql += ", '" + svcInfo["SERVICE_SMS"].ToString() + "' ";

                sSql += ", '" + svcInfo["USR_ID"].ToString() + "' ";
                sSql += ", UFN_DATE_FORMAT('DATE') ";
                sSql += ", UFN_DATE_FORMAT('TIME') ";
                sSql += ", '" + svcInfo["USR_ID"].ToString() + "' ";
                sSql += ", UFN_DATE_FORMAT('DATE') ";
                sSql += ", UFN_DATE_FORMAT('TIME')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }
        //사용자 정보를 조회 한다
        public static DataTable GetUserInfo(DataRow UserInfo)
        {
            if (!string.IsNullOrEmpty(UserInfo["USR_ID"].ToString()))
            {
                sSql = "";
                sSql += " SELECT * ";
                sSql += "   FROM META_USR_MST ";
                sSql += "  WHERE 1=1 ";
                sSql += "    AND USR_ID = '" + UserInfo["USR_ID"] + "' ";
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        //사용자 아이디를 찾는다
        public static DataTable FindUserId(DataRow UserInfo)
        {
            if (!string.IsNullOrEmpty(UserInfo["USR_NM"].ToString()) && !string.IsNullOrEmpty(UserInfo["HP_NO"].ToString()))
            {
                sSql = "";
                sSql += " SELECT USR_ID ";
                sSql += "   FROM META_USR_MST ";
                sSql += "  WHERE 1=1 ";
                sSql += "    AND USR_NM = '" + UserInfo["USR_NM"] + "' ";
                sSql += "    AND HP_NO = '" + UserInfo["HP_NO"] + "' ";
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // 비밀번호를 초기화한다
        public static bool SetUserPswdReset(DataRow UserInfo)
        {
            if (!string.IsNullOrEmpty(UserInfo["USR_ID"].ToString()) && !string.IsNullOrEmpty(UserInfo["PSWD"].ToString()))
            {
                sSql = "";
                sSql += "UPDATE META_USR_MST ";
                sSql += "   SET PSWD = UNF_MD5_ENCRYPTION('" + UserInfo["PSWD"].ToString() + "') ";
                sSql += ",  UPD_USR = 'SYSTEM' ";
                sSql += ",  UPD_YMD = UFN_DATE_FORMAT('DATE') ";
                sSql += ",  UPD_HM = UFN_DATE_FORMAT('TIME') ";
                sSql += " WHERE USR_ID = '" + UserInfo["USR_ID"].ToString() + "'";
                
                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }

        // 사용자 정보를 수정한다
        public static bool SetUserInfo(DataRow UserInfo)
        {
            if (!string.IsNullOrEmpty(UserInfo["USR_ID"].ToString()))
            {
                sSql = "";
                sSql += "UPDATE META_USR_MST ";
                sSql += "   SET UPD_USR = 'SYSTEM' ";
                sSql += "     , UPD_YMD = UFN_DATE_FORMAT('DATE') ";
                sSql += "     , UPD_HM = UFN_DATE_FORMAT('TIME') ";
                if (UserInfo.Table.Columns.Contains("PSWD")) sSql += ", PSWD = UNF_MD5_ENCRYPTION('" + UserInfo["PSWD"].ToString() + "') ";
                if (UserInfo.Table.Columns.Contains("USR_NM")) sSql += ", USR_NM = '" + UserInfo["USR_NM"].ToString() + "' ";
                if (UserInfo.Table.Columns.Contains("HP_NO")) sSql += ", HP_NO = '" + UserInfo["HP_NO"].ToString() + "' ";
                if (UserInfo.Table.Columns.Contains("LOC_ADDR")) sSql += ", LOC_ADDR = '" + UserInfo["LOC_ADDR"].ToString() + "' ";
                if (UserInfo.Table.Columns.Contains("BIZCOND")) sSql += ", BIZCOND = '" + UserInfo["BIZCOND"].ToString() + "' ";
                if (UserInfo.Table.Columns.Contains("BIZTYPE")) sSql += ", BIZTYPE = '" + UserInfo["BIZTYPE"].ToString() + "' ";
                if (UserInfo.Table.Columns.Contains("CEO")) sSql += ", CEO = '" + UserInfo["CEO"].ToString() + "' ";
                if (UserInfo.Table.Columns.Contains("TEL_NO")) sSql += ", TEL_NO = '" + UserInfo["TEL_NO"].ToString() + "' ";
                if (UserInfo.Table.Columns.Contains("ENG_NM")) sSql += ", ENG_NM = '" + UserInfo["ENG_NM"].ToString() + "' ";
                if (UserInfo.Table.Columns.Contains("ENG_ADDR")) sSql += ", ENG_ADDR = '" + UserInfo["ENG_ADDR"].ToString() + "' ";
                if (UserInfo.Table.Columns.Contains("CTRY_CD")) sSql += ", CTRY_CD = '" + UserInfo["CTRY_CD"].ToString() + "' ";
                sSql += " WHERE USR_ID = '" + UserInfo["USR_ID"].ToString() + "'";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }

        // 첨부파일을 등록한다
        public static bool SetDocFile(DataRow DocInfo)
        {
            if (!string.IsNullOrEmpty(DocInfo["USR_ID"].ToString()) && !string.IsNullOrEmpty(DocInfo["PSWD"].ToString()))
            {                
                sSql = "";
                sSql += "INSERT INTO META_DOC_MST ( ";
                sSql += "  MNGT_NO ";
                sSql += ", USR_ID ";
                sSql += ", PSWD ";
                sSql += ", USR_NM ";
                sSql += ", HP_NO ";
                sSql += ", CRN ";
                sSql += ", LOC_NM ";
                sSql += ", LOC_ADDR ";
                sSql += ", BIZCOND ";
                sSql += ", BIZTYPE ";
                sSql += ", CEO ";
                sSql += ", TEL_NO ";
                sSql += ", ENG_NM ";
                sSql += ", ENG_ADDR ";
                sSql += ", CTRY_CD ";
                sSql += ", INS_USR ";
                sSql += ", INS_YMD ";
                sSql += ", INS_HM ";
                sSql += ", UPD_USR ";
                sSql += ", UPD_YMD ";
                sSql += ", UPD_HM) ";

                sSql += "VALUES( ";
                sSql += "  '" + DocInfo["MNGT_NO"].ToString() + "' ";
                sSql += ", '" + DocInfo["DOC_TYPE"].ToString() + "' ";
                sSql += ", '" + DocInfo["DOC_NO"].ToString() + "') ";
                sSql += ", '" + DocInfo["FILE_NM"].ToString() + "' ";
                sSql += ", '" + DocInfo["FILE_PATH"].ToString() + "' ";
                sSql += ", '" + DocInfo["FILE_EXTENSION"].ToString() + "' ";
                sSql += ", '" + DocInfo["ENCRY_FILE_NM"].ToString() + "' ";
                sSql += ", '" + DocInfo["ENCRY_FILE_PATH"].ToString() + "' ";
                sSql += ", '" + DocInfo["ENCRY_EXTENSION"].ToString() + "' ";
                sSql += ", '" + DocInfo["USR_ID"].ToString() + "' ";
                sSql += ", UFN_DATE_FORMAT('DATE') ";
                sSql += ", UFN_DATE_FORMAT('TIME') ";
                sSql += ", '" + DocInfo["USR_ID"].ToString() + "' ";
                sSql += ", UFN_DATE_FORMAT('DATE') ";
                sSql += ", UFN_DATE_FORMAT('TIME')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }


        //등록된 사업자등록증을 조회 한다
        public static DataTable GetDocFilePath(DataRow ParamInfo)
        {
            if (!string.IsNullOrEmpty(ParamInfo["MNGT_NO"].ToString()))
            {
                sSql = "";
                sSql += " SELECT FILE_PATH || '/' || FILE_NM || FILE_EXTENSION AS FILE_PATH ";
                sSql += "      , FILE_NM || FILE_EXTENSION AS FILE_NAME ";
                sSql += "   FROM META_DOC_MST ";
                sSql += "  WHERE 1=1 ";
                sSql += "    AND MNGT_NO = '" + ParamInfo["MNGT_NO"] +"'";
                sSql += "    AND DOC_TYPE = '" + ParamInfo["DOC_TYPE"] + "'";
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        public static DataTable GetFileList(DataRow ParamInfo)
        {
            DataTable rtnDt = new DataTable();

            sSql = "";
            sSql += "SELECT * FROM META_USE_DTL ";
            sSql += "      WHERE  MNGT_NO ='" + ParamInfo["MNGT_NO"].ToString() + "' AND SVC_TYPE = '" + ParamInfo["SVC_TYPE"].ToString()  + "'" + " AND DOC_TYPE = '" + ParamInfo["DOC_TYPE"].ToString() + "'" + " AND CRN = '" + ParamInfo["CRN"].ToString() + "'" + " AND USR_TYPE = '" + ParamInfo["USR_TYPE"].ToString() + "'";

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);

            return rtnDt;
        }
        public static bool SetFileList(DataRow ParamInfo)
        {

                sSql = "";
                sSql += "INSERT INTO META_USE_DTL ( ";
                sSql += "       MNGT_NO ";
                sSql += "     , CRN AS CUST_CD ";
                sSql += "     , USR_ID ";
                sSql += "     , USE_YMD ";
                sSql += "     , SVC_TYPE ";
                sSql += "     , DOC_TYPE ";
                sSql += "     , REQ_SVC ";
                sSql += "     , USR_TYPE ";
            if (ParamInfo.Table.Columns.Contains("INV_AMT")) sSql += " , INV_AMT ";
                if (ParamInfo.Table.Columns.Contains("FILE_SIZE")) sSql += " , FILE_SIZE ";
                if (ParamInfo.Table.Columns.Contains("FILE_CNT")) sSql += " , FILE_CNT ";
                sSql += "     , INS_USR ";
                sSql += "     , INS_YMD ";
                sSql += "     , UPD_USR ";
                sSql += "     , UPD_YMD )";

                sSql += "VALUES( ";
                sSql += "       '" + ParamInfo["MNGT_NO"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["CRN"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["USR_ID"].ToString() + "' ";
                sSql += "     , TO_CHAR(TO_DATE(SYSDATE),'yyyymmdd') ";
                sSql += "     , '" + ParamInfo["SVC_TYPE"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["DOC_TYPE"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["REQ_SVC"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["USR_TYPE"].ToString() + "' ";
            if (ParamInfo.Table.Columns.Contains("INV_AMT")) sSql += ", '" + ParamInfo["INV_AMT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("FILE_SIZE")) sSql += ", '" + ParamInfo["FILE_SIZE"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("FILE_CNT")) sSql += ", " + ParamInfo["FILE_CNT"].ToString();
                sSql += "     , '" + ParamInfo["USR_ID"].ToString() + "' ";
                sSql += "     , TO_CHAR(SYSDATE,'YYYYMMDD') ";
                sSql += "     , '" + ParamInfo["USR_ID"].ToString() + "' ";
                sSql += "     , TO_CHAR(SYSDATE,'YYYYMMDD')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            
            return rtnBool;
        }

        public static bool SetFileList(string mngt_no , string crn , string usr_id, string svc_type , string doc_type, string req_svc, string usr_type)
        {

            sSql = "";
            sSql += "INSERT INTO META_USE_DTL ( ";
            sSql += "       MNGT_NO ";
            sSql += "     , CRN AS CUST_CD ";
            sSql += "     , USR_ID ";
            sSql += "     , USE_YMD ";
            sSql += "     , REQ_SVC ";
            sSql += "     , SVC_TYPE ";
            sSql += "     , DOC_TYPE ";
            sSql += "     , FILE_SIZE ";
            sSql += "     , USR_TYPE ";
            sSql += "     , INS_USR ";
            sSql += "     , INS_YMD ";
            sSql += "     , UPD_USR ";
            sSql += "     , UPD_YMD )";

            sSql += "VALUES( ";
            sSql += "       '" + mngt_no + "' ";
            sSql += "     , '" + crn + "' ";
            sSql += "     , '" +  usr_id + "' ";
            sSql += "     , TO_CHAR(TO_DATE(SYSDATE),'yyyymmdd') ";
            sSql += "     , '" + req_svc + "' ";
            sSql += "     , '" + svc_type + "' ";
            sSql += "     , '" + doc_type + "' ";
            sSql += ", 1 ";
            sSql += "     , '" + usr_type + "' ";
            sSql += "     , '" + usr_id + "' ";
            sSql += "     , TO_CHAR(SYSDATE,'YYYYMMDD') ";
            sSql += "     , '" + usr_id + "' ";
            sSql += "     , TO_CHAR(SYSDATE,'YYYYMMDD')) ";

            int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
            if (rtnStatus >= 0) rtnBool = true;

            return rtnBool;
        }


    }
}