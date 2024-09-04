using System.Data;

namespace META_DATA_API.Models.Query
{
    public static class Sql_UserData
    {
        private static string sSql = "";
        private static bool rtnBool = false;
        private static DataTable rtnDt = null;

        // ELVIS-FRIEND 회원 정보를 등록한다
        public static bool SetUserRegist(string mngt_no , DataRow ParamInfo)
        {
            if (!string.IsNullOrEmpty(mngt_no) && !string.IsNullOrEmpty(ParamInfo["USR_ID"].ToString()) && !string.IsNullOrEmpty(ParamInfo["PSWD"].ToString()) && !string.IsNullOrEmpty(ParamInfo["USR_NM"].ToString())
                && !string.IsNullOrEmpty(ParamInfo["HP_NO"].ToString()) && !string.IsNullOrEmpty(ParamInfo["LOC_NM"].ToString()))
            {
                sSql = "";
                sSql += "INSERT INTO META_USR_MST ( ";
                sSql += "       MNGT_NO ";
                sSql += "     , USR_ID ";
                sSql += "     , PSWD ";
                sSql += "     , USR_NM ";
                sSql += "     , HP_NO ";
                sSql += "     , MAIN_YN ";
                sSql += "     , LOC_NM ";
                sSql += "     , DOMAIN ";
                sSql += "     , CRN ";
                if (ParamInfo.Table.Columns.Contains("USR_TYPE")) sSql += " , USR_TYPE ";
                if (ParamInfo.Table.Columns.Contains("DOCU")) sSql += " , DOCU ";
                if (ParamInfo.Table.Columns.Contains("QUOT")) sSql += " , QUOT ";
                if (ParamInfo.Table.Columns.Contains("EXPORT")) sSql += " , EXPORT ";
                if (ParamInfo.Table.Columns.Contains("IMPORT")) sSql += " , IMPORT ";
                if (ParamInfo.Table.Columns.Contains("SEA_EX")) sSql += " , SEA_EX ";
                if (ParamInfo.Table.Columns.Contains("SEA_IM")) sSql += " , SEA_IM ";
                if (ParamInfo.Table.Columns.Contains("AIR_EX")) sSql += " , AIR_EX ";
                if (ParamInfo.Table.Columns.Contains("AIR_IM")) sSql += " , AIR_IM ";
                sSql += "     , INS_USR ";
                sSql += "     , INS_YMD ";
                sSql += "     , INS_HM ";
                sSql += "     , UPD_USR ";
                sSql += "     , UPD_YMD ";
                sSql += "     , UPD_HM) ";

                sSql += "VALUES( ";
                sSql += "       '" + mngt_no + "' ";
                sSql += "     , '" + ParamInfo["USR_ID"].ToString() + "' ";
                sSql += "     , UNF_MD5_ENCRYPTION('" + ParamInfo["PSWD"].ToString() + "') ";
                sSql += "     , '" + ParamInfo["USR_NM"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["HP_NO"].ToString() + "' ";
                sSql += "     , 'N' ";
                sSql += "     , '" + ParamInfo["LOC_NM"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["DOMAIN"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["CRN"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("USR_TYPE")) sSql += ", '" + ParamInfo["USR_TYPE"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("DOCU")) sSql += ", '" + ParamInfo["DOCU"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("QUOT")) sSql += ", '" + ParamInfo["QUOT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("EXPORT")) sSql += ", '" + ParamInfo["EXPORT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("IMPORT")) sSql += ", '" + ParamInfo["IMPORT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("SEA_EX")) sSql += ", '" + ParamInfo["SEA_EX"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("SEA_IM")) sSql += ", '" + ParamInfo["SEA_IM"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("AIR_EX")) sSql += ", '" + ParamInfo["AIR_EX"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("AIR_IM")) sSql += ", '" + ParamInfo["AIR_IM"].ToString() + "' ";

                sSql += "     , '" + ParamInfo["USR_ID"].ToString() + "' ";
                sSql += "     , UFN_DATE_FORMAT('DATE') ";
                sSql += "     , UFN_DATE_FORMAT('TIME') ";
                sSql += "     , '" + ParamInfo["USR_ID"].ToString() + "' ";
                sSql += "     , UFN_DATE_FORMAT('DATE') ";
                sSql += "     , UFN_DATE_FORMAT('TIME')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }

        // Saas 서비스 신청 정보를 등록한다
        public static bool SetSaasSvcRegist(DataRow ParamInfo)
        {
            if (!string.IsNullOrEmpty(ParamInfo["ELVIS_USR_ID"].ToString()) && !string.IsNullOrEmpty(ParamInfo["ELVIS_PSWD"].ToString())
                && !string.IsNullOrEmpty(ParamInfo["CRN"].ToString()) && !string.IsNullOrEmpty(ParamInfo["OFFICE_NM"].ToString())
                && !string.IsNullOrEmpty(ParamInfo["LOC_NM"].ToString()) && !string.IsNullOrEmpty(ParamInfo["OFFICE_ADDR"].ToString())
                && !string.IsNullOrEmpty(ParamInfo["EMAIL"].ToString()) && !string.IsNullOrEmpty(ParamInfo["HP_NO"].ToString())
                && !string.IsNullOrEmpty(ParamInfo["REG_DATE"].ToString()) && !string.IsNullOrEmpty(ParamInfo["STAT_DATE"].ToString()))
            {
                sSql = "";
                sSql += "INSERT INTO PRM_ELVIS_REG ( ";
                sSql += "       ELVIS_USR_ID ";
                sSql += "     , ELVIS_PSWD ";
                sSql += "     , CRN ";
                sSql += "     , OFFICE_NM ";
                sSql += "     , LOC_NM ";
                sSql += "     , OFFICE_ADDR ";
                sSql += "     , EMAIL ";
                sSql += "     , HP_NO ";
                sSql += "     , REG_DATE ";
                sSql += "     , STAT_DATE ";

                if (ParamInfo.Table.Columns.Contains("ELVIS_FMS")) sSql += " , ELVIS_FMS ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_WMS")) sSql += " , ELVIS_WMS ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_TMS")) sSql += " , ELVIS_TMS ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_EXPRESS")) sSql += " , ELVIS_EXPRESS ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_PRIME")) sSql += " , ELVIS_PRIME ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_BIG")) sSql += " , ELVIS_BIG ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_BIG_UNI")) sSql += " , ELVIS_BIG_UNI ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_BIG_SFI")) sSql += " , ELVIS_BIG_SFI ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_BIG_TER")) sSql += " , ELVIS_BIG_TER ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_BIG_TRK")) sSql += " , ELVIS_BIG_TRK ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_BIG_LAT")) sSql += " , ELVIS_BIG_LAT ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_FRIEND")) sSql += " , ELVIS_FRIEND ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_FRIEND_DOCU")) sSql += " , ELVIS_FRIEND_DOCU ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_FRIEND_EXIM")) sSql += " , ELVIS_FRIEND_EXIM ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_FRIEND_DATA")) sSql += " , ELVIS_FRIEND_DATA ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_CUST_INFO")) sSql += " , ELVIS_CUST_INFO ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_ACCOUNT")) sSql += " , ELVIS_ACCOUNT ";
                if (ParamInfo.Table.Columns.Contains("SERVICE_FAX")) sSql += " , SERVICE_FAX ";
                if (ParamInfo.Table.Columns.Contains("SERVICE_SMS")) sSql += " , SERVICE_SMS ";

                sSql += "     , INS_USR ";
                sSql += "     , INS_YMD ";
                sSql += "     , INS_HM ";
                sSql += "     , UPD_USR ";
                sSql += "     , UPD_YMD ";
                sSql += "     , UPD_HM) ";


                sSql += "VALUES( ";
                sSql += "        '" + ParamInfo["ELVIS_USR_ID"].ToString() + "' ";
                sSql += "     , UNF_MD5_ENCRYPTION('" + ParamInfo["ELVIS_PSWD"].ToString() + "') ";
                sSql += "     , '" + ParamInfo["CRN"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["OFFICE_NM"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["LOC_NM"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["OFFICE_ADDR"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["EMAIL"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["HP_NO"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["REG_DATE"].ToString() + "' ";
                sSql += "     , '" + ParamInfo["STAT_DATE"].ToString() + "' ";

                if (ParamInfo.Table.Columns.Contains("ELVIS_FMS")) sSql += ", '" + ParamInfo["ELVIS_FMS"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_WMS")) sSql += ", '" + ParamInfo["ELVIS_WMS"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_TMS")) sSql += ", '" + ParamInfo["HP_ELVIS_TMSNO"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_EXPRESS")) sSql += ", '" + ParamInfo["ELVIS_EXPRESS"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_PRIME")) sSql += ", '" + ParamInfo["ELVIS_PRIME"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_BIG")) sSql += ", '" + ParamInfo["ELVIS_BIG"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_BIG_UNI")) sSql += ", '" + ParamInfo["ELVIS_BIG_UNI"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_BIG_SFI")) sSql += ", '" + ParamInfo["ELVIS_BIG_SFI"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_BIG_TER")) sSql += ", '" + ParamInfo["ELVIS_BIG_TER"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_BIG_TRK")) sSql += ", '" + ParamInfo["ELVIS_BIG_TRK"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_BIG_LAT")) sSql += ", '" + ParamInfo["ELVIS_BIG_LAT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_FRIEND")) sSql += ", '" + ParamInfo["ELVIS_FRIEND"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_FRIEND_DOCU")) sSql += ", '" + ParamInfo["ELVIS_FRIEND_DOCU"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_FRIEND_EXIM")) sSql += ", '" + ParamInfo["ELVIS_FRIEND_EXIM"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_FRIEND_DATA")) sSql += ", '" + ParamInfo["ELVIS_FRIEND_DATA"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_CUST_INFO")) sSql += ", '" + ParamInfo["ELVIS_CUST_INFO"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("ELVIS_ACCOUNT")) sSql += ", '" + ParamInfo["ELVIS_ACCOUNT"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("SERVICE_FAX")) sSql += ", '" + ParamInfo["SERVICE_FAX"].ToString() + "' ";
                if (ParamInfo.Table.Columns.Contains("SERVICE_SMS")) sSql += ", '" + ParamInfo["SERVICE_SMS"].ToString() + "' ";

                sSql += "     , '" + ParamInfo["USR_ID"].ToString() + "' ";
                sSql += "     , UFN_DATE_FORMAT('DATE') ";
                sSql += "     , UFN_DATE_FORMAT('TIME') ";
                sSql += "     , '" + ParamInfo["USR_ID"].ToString() + "' ";
                sSql += "     , UFN_DATE_FORMAT('DATE') ";
                sSql += "    , UFN_DATE_FORMAT('TIME')) ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }

        // 사용자 정보를 조회 한다
        public static DataTable GetUserInfo(string UsrId)
        {
            if (!string.IsNullOrEmpty(UsrId))
            {
                sSql = "";
                sSql += " SELECT * ";
                sSql += "   FROM META_USR_MST ";
                sSql += "  WHERE 1=1 ";
                sSql += "    AND USR_ID = '" + UsrId + "' ";
                sSql += "    AND APV_YN = 'Y' ";
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // 사용자 정보를 조회 한다
        public static bool SetUserStatus(string UsrId)
        {
            if (!string.IsNullOrEmpty(UsrId))
            {
                sSql = "";
                sSql += " UPDATE META_USR_MST SET APV_YN = 'N' ";
                sSql += "  WHERE 1=1 ";
                sSql += "    AND USR_ID = '" + UsrId + "' ";
            }

            int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
            if (rtnStatus >= 0) rtnBool = true;

            return rtnBool;
        }


        // 사용자 아이디를 찾는다
        public static DataTable FindUserId(string UsrNm, string HpNo)
        {
            if (!string.IsNullOrEmpty(UsrNm) && !string.IsNullOrEmpty(HpNo))
            {
                sSql = "";
                sSql += " SELECT USR_ID ";
                sSql += "   FROM META_USR_MST ";
                sSql += "  WHERE 1=1 ";
                sSql += "    AND USR_NM = '" + UsrNm + "' ";
                sSql += "    AND HP_NO = '" + HpNo + "' ";
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // 비밀번호를 초기화한다
        public static bool SetUserPswdReset(string UsrId, string Pswd)
        {
            if (!string.IsNullOrEmpty(UsrId) && !string.IsNullOrEmpty(Pswd))
            {
                sSql = "";
                sSql += "UPDATE META_USR_MST ";
                sSql += "   SET PSWD = UNF_MD5_ENCRYPTION('" + Pswd + "') ";
                sSql += ",  UPD_USR = 'SYSTEM' ";
                sSql += ",  UPD_YMD = UFN_DATE_FORMAT('DATE') ";
                sSql += ",  UPD_HM = UFN_DATE_FORMAT('TIME') ";
                sSql += " WHERE USR_ID = '" + UsrId + "'";
                
                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }

        // 사용자 정보를 수정한다
        public static bool SetUserInfo(string UsrId, string Pswd, string UsrNm, string HpNo)
        {
            if (!string.IsNullOrEmpty(UsrId))
            {
                sSql = "";
                sSql += "UPDATE META_USR_MST ";
                sSql += "   SET UPD_USR = 'SYSTEM' ";
                sSql += "     , UPD_YMD = UFN_DATE_FORMAT('DATE') ";
                sSql += "     , UPD_HM = UFN_DATE_FORMAT('TIME') ";
                if (!string.IsNullOrEmpty(Pswd)) sSql += ", PSWD = UNF_MD5_ENCRYPTION('" + Pswd + "') ";
                if (!string.IsNullOrEmpty(UsrNm)) sSql += ", USR_NM = '" + UsrNm + "' ";
                if (!string.IsNullOrEmpty(HpNo)) sSql += ", HP_NO = '" + HpNo + "' ";
                sSql += " WHERE USR_ID = '" + UsrId + "'";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }

        // 사용자 서비스를 수정한다
        public static bool SetUserServiceInfo(string UsrId, string UsrDocu, string UsrQuot, string UsrEx, string UsrIm)
        {
            if(!string.IsNullOrEmpty(UsrId))
            {
                sSql = "";
                sSql += "UPDATE META_USR_MST";
                sSql += "    SET UPD_USR = 'SYSTEM'";
                sSql += "         , UPD_YMD = UFN_DATE_FORMAT('DATE')";
                sSql += "         , UPD_HM = UFN_DATE_FORMAT('TIME')";
                sSql += "         , DOCU = '" + UsrDocu + "'";
                sSql += "         , QUOT = '" + UsrQuot + "'";
                sSql += "         , EXPORT = '" + UsrEx + "'";
                sSql += "         , IMPORT = '" + UsrIm + "'";
                sSql += "    WHERE USR_ID = '" + UsrId + "'";

                int rtnStatus = _DataHelper.ExecuteNonQuery(sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }

        // 사업자등록증을 등록한다
        public static bool LicenseUpload(string MngtNo, string FileName, string FilePath, string FileExt, long FileSize)
        {
            if (!string.IsNullOrEmpty(MngtNo) && !string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(FilePath) && !string.IsNullOrEmpty(FileExt))
            {
                sSql = "";
                sSql += "MERGE INTO META_DOC_MST USING DUAL ON ( ";
                sSql += "    MNGT_NO = '" + MngtNo + "' ";
                //sSql += "AND CRN = '" + Crn + "'";
                sSql += "AND DOC_TYPE = 'CRN') ";

                sSql += "WHEN MATCHED THEN ";
                sSql += "UPDATE SET ";
                sSql += "  FILE_NM = '" + FileName + "' ";
                sSql += ", FILE_PATH = '" + FilePath + "' ";
                sSql += ", FILE_EXT = '" + FileExt + "' ";
                sSql += ", FILE_SIZE = '" + FileSize + "' ";
                sSql += ", UPD_USR = 'SYSTEM' ";
                sSql += ", UPD_YMD = UFN_DATE_FORMAT('DATE') ";
                sSql += ", UPD_HM = UFN_DATE_FORMAT('TIME') ";

                sSql += "WHEN NOT MATCHED THEN ";
                sSql += "INSERT ( ";
                sSql += "  MNGT_NO ";
                //sSql += ", CRN ";
                sSql += ", DOC_TYPE ";
                sSql += ", FILE_NM ";
                sSql += ", FILE_PATH ";
                sSql += ", FILE_EXT ";
                sSql += ", FILE_SIZE ";
                sSql += ", INS_USR ";
                sSql += ", INS_YMD ";
                sSql += ", INS_HM ";
                sSql += ", UPD_USR ";
                sSql += ", UPD_YMD ";
                sSql += ", UPD_HM) ";

                sSql += "VALUES( ";
                sSql += "  '" + MngtNo + "' ";
                //sSql += ", '" + Crn + "' ";
                sSql += ", 'CRN' ";
                sSql += ", '" + FileName + "' ";
                sSql += ", '" + FilePath + "' ";
                sSql += ", '" + FileExt + "' ";
                sSql += ", '" + FileSize + "' ";
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

        // 사업자등록증 파일경로를 조회 한다
        public static DataTable GetLicensePath(string MngtNo)
        {
            if (!string.IsNullOrEmpty(MngtNo))
            {
                sSql = "";
                sSql += " SELECT FILE_PATH || '/' || FILE_NM || FILE_EXT AS FILE_PATH ";
                sSql += "      , FILE_NM || FILE_EXT AS FILE_NAME ";
                sSql += "   FROM META_DOC_MST ";
                sSql += "  WHERE 1=1 ";
                sSql += "    AND MNGT_NO = '" + MngtNo + "' ";
                sSql += "    AND DOC_TYPE = 'CRN' ";
            }
            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // Seavantage 인증 토큰을 조회 한다
        public static DataTable GetSvtgAuthToken(string ConnStr)
        {
            if (!string.IsNullOrEmpty(ConnStr))
            {
                sSql = "";
                sSql += "SELECT ID ";
                sSql += "     , PWD ";
                sSql += "     , SENDER_ID AS AUTH_TOKEN";
                sSql += "  FROM CRM_CUST_EDI ";
                sSql += " WHERE 1 = 1 ";
                sSql += "   AND CUST_CD = 'YJIT' ";
                sSql += "   AND EDI_SYS_CD = 'SVTG' ";
            }

            rtnDt = _DataHelper.ExecuteDataTable(ConnStr, sSql, CommandType.Text);
            return rtnDt;
        }

        // Seavantage 인증 토큰을 저장 한다
        public static bool SetSvtgAuthToken(string ConnStr, string AuthToken)
        {
            if (!string.IsNullOrEmpty(ConnStr))
            {
                sSql = "";
                sSql += "UPDATE CRM_CUST_EDI ";
                sSql += "   SET SENDER_ID = '" + AuthToken + "'";
                sSql += " WHERE CUST_CD = 'YJIT' ";
                sSql += "   AND EDI_SYS_CD = 'SVTG' ";

                int rtnStatus = _DataHelper.ExecuteNonQuery(ConnStr, sSql, CommandType.Text);
                if (rtnStatus >= 0) rtnBool = true;
            }
            return rtnBool;
        }

        // DashBoard 자료를 조회 한다
        public static DataTable GetDashBoardInfo(string Crn, string DateYyyy, string DateMm)
        {
            if (!string.IsNullOrEmpty(Crn) && !string.IsNullOrEmpty(DateYyyy) && !string.IsNullOrEmpty(DateMm))
            {
                sSql = "";
                sSql += "SELECT SEA_AIR_TOT ";
                sSql += "     , SEA_EX_TOT ";
                sSql += "     , SEA_EX_COM ";
                sSql += "     , SEA_IM_TOT ";
                sSql += "     , SEA_IM_COM ";
                sSql += "     , AIR_EX_TOT ";
                sSql += "     , AIR_EX_COM ";
                sSql += "     , AIR_IM_TOT ";
                sSql += "     , AIR_IM_COM ";
                sSql += "     , (SELECT SUM(OCR_TOT) FROM META_DASHBOARD_MST WHERE CRN = '" + Crn + "') OCR_TOT ";
                sSql += "     , (SELECT SUM(OCR_COM) FROM META_DASHBOARD_MST WHERE CRN = '" + Crn + "') OCR_COM ";
                sSql += "     , QUOT_TOT ";
                sSql += "     , QUOT_COM ";
                sSql += "  FROM META_DASHBOARD_MST ";
                sSql += " WHERE CRN = '" + Crn + "' ";
                sSql += "   AND DATE_YYYY = '" + DateYyyy + "' ";
                sSql += "   AND DATE_MM = '" + DateMm + "' ";
            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }
        /// <summary>
        /// 실행사 대쉬보드 조회 쿼리
        /// </summary>
        /// <param name="Crn"></param>
        /// <param name="DateYyyy"></param>
        /// <param name="DateMm"></param>
        /// <returns></returns>
        public static DataTable GetDashFBoardInfo(string Crn, string DateYyyy, string DateMm)
        {
            if (!string.IsNullOrEmpty(Crn) && !string.IsNullOrEmpty(DateYyyy) && !string.IsNullOrEmpty(DateMm))
            {
                sSql = "";
                sSql += "SELECT  ";
                sSql += "      SEA_EX_TOT ";
                sSql += "     , SEA_EX_COM ";
                sSql += "     , SEA_IM_TOT ";
                sSql += "     , SEA_IM_COM ";
                sSql += "     , AIR_EX_TOT ";
                sSql += "     , AIR_EX_COM ";
                sSql += "     , AIR_IM_TOT ";
                sSql += "     , AIR_IM_COM ";
                sSql += "     , QUOT_TOT ";
                sSql += "     , QUOT_COM ";
                sSql += "  FROM META_FWD_DASHBOARD_MST ";
                sSql += " WHERE CRN = '" + Crn + "' ";
                sSql += "   AND DATE_YYYY = '" + DateYyyy + "' ";
                sSql += "   AND DATE_MM = '" + DateMm + "' ";
            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }


        // 월별 사용량 자료를 조회 한다
        public static DataTable GetUsedInfo(string Crn, string DateYyyy, string DateMm, string Doc_Type, string Svc_Type, string usr_type)
        {
            if (!string.IsNullOrEmpty(Crn) && !string.IsNullOrEmpty(DateYyyy) && !string.IsNullOrEmpty(DateMm))
            {
                sSql = "";
                if (Doc_Type != null)
                {
                    sSql += "	SELECT DOC_TYPE,	";
                    sSql += "			COUNT (DOC_TYPE) AS CNT,";
                    sSql += "			MAX(REQ_SVC) AS REQ_SVC ,";
                    sSql += "			SUM (FILE_CNT) AS FILE_CNT,";
                    sSql += "			SUM (FILE_SIZE) AS FILE_SIZE";
                    sSql += "		FROM META_USE_DTL";
                    sSql += "	WHERE CRN = '" + Crn + "' AND TO_CHAR(TO_DATE(USE_YMD),'YYYYMM') LIKE '" + DateYyyy + DateMm + "%'";
                    sSql += "		AND DOC_TYPE IN (" + Doc_Type + ")";
                    sSql += "		AND USR_TYPE =  '" + usr_type + "'";
                    sSql += "	GROUP BY DOC_TYPE";
                    sSql += "	UNION";
                }
                sSql += "	SELECT MAX(DOC_TYPE) AS DOC_TYPE,	";
                sSql += "			COUNT (DOC_TYPE) AS CNT,";
                sSql += "			REQ_SVC,";
                sSql += "			SUM (FILE_CNT) AS FILE_CNT,";
                sSql += "			SUM (FILE_SIZE) AS FILE_SIZE";
                sSql += "		FROM META_USE_DTL";
                //sSql += "	WHERE CUST_CD = '" + Crn + "' AND TO_CHAR(USE_YMD,'YYYYMM') LIKE '" + DateYyyy + DateMm +  "%'";
                sSql += "	WHERE CRN = '" + Crn + "' AND TO_CHAR(TO_DATE(USE_YMD),'YYYYMM')  LIKE '" + DateYyyy + DateMm + "%'";
                sSql += "		AND DOC_TYPE <> 'QUOT' AND DOC_TYPE <> 'DOCU'";
                sSql += "		AND USR_TYPE =  '" + usr_type + "'";
                //sSql += "		AND SVC_TYPE IN (" + Svc_Type + ")";
                sSql += "	GROUP BY REQ_SVC";

            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // 월별 사용량 자료를 조회 한다
        public static DataTable GetPartnerUsedInfo(DataRow dr)
        {
            
            sSql = "";
            if (dr["DOC_TYPE"].ToString() != "")
            {
                sSql += "	SELECT DOC_TYPE,	";
                sSql += "			COUNT (DOC_TYPE) AS CNT,";
                sSql += "			MAX(REQ_SVC) AS REQ_SVC ,";
                sSql += "			MAX(SVC_TYPE) AS SVC_TYPE ,";
                sSql += "			SUM (FILE_CNT) AS FILE_CNT,";
                sSql += "			SUM (FILE_SIZE) AS FILE_SIZE";
                sSql += "		FROM META_USE_DTL";
                sSql += "	WHERE CRN = '" + dr["CRN"].ToString() + "' AND TO_CHAR(TO_DATE(USE_YMD),'YYYYMM') LIKE '" + dr["DATE_YYYY"].ToString() + dr["DATE_MM"].ToString() + "%'";
                sSql += "		AND DOC_TYPE IN (" + dr["DOC_TYPE"].ToString() + ")";
                sSql += "		AND USR_TYPE =  '" + dr["USR_TYPE"].ToString() + "'";
                sSql += "	GROUP BY DOC_TYPE";
                sSql += "	UNION";
            }
            sSql += "	SELECT MAX(DOC_TYPE) AS DOC_TYPE,	";
            sSql += "			COUNT (DOC_TYPE) AS CNT,";
            sSql += "			REQ_SVC,";
            sSql += "			MAX(SVC_TYPE) AS SVC_TYPE,";
            sSql += "			SUM (FILE_CNT) AS FILE_CNT,";
            sSql += "			SUM (FILE_SIZE) AS FILE_SIZE";
            sSql += "		FROM META_USE_DTL";
            sSql += "	WHERE CRN = '" + dr["CRN"].ToString() + "' AND TO_CHAR(TO_DATE(USE_YMD),'YYYYMM') LIKE '" + dr["DATE_YYYY"].ToString() + dr["DATE_MM"].ToString() + "%'";
            sSql += "		AND DOC_TYPE <> 'QUOT' AND DOC_TYPE <> 'DOCU'";
            sSql += "		AND USR_TYPE =  '" + dr["USR_TYPE"].ToString() + "'";
            //if (dr.Table.Columns.Contains("SEA_EX")) {
            //    if (dr["SEA_EX"].ToString() == "Y")
            //    {
            //        sSql += "		OR REQ_SVC = 'SEA' OR SVC_TYPE = 'EXPORT' ";
            //    }
            //}
            //if (dr.Table.Columns.Contains("SEA_IM"))
            //{
            //    if (dr["SEA_IM"].ToString() == "Y")
            //    {
            //        sSql += "		OR REQ_SVC = 'SEA' OR SVC_TYPE = 'IMPORT' ";
            //    }
            //}
            //if (dr.Table.Columns.Contains("AIR_EX"))
            //{
            //    if (dr["AIR_EX"].ToString() == "Y")
            //    {
            //        sSql += "		OR REQ_SVC = 'AIR' OR SVC_TYPE = 'EXPORT' ";
            //    }
            //}
            //if (dr.Table.Columns.Contains("AIR_IM"))
            //{
            //    if (dr["AIR_IM"].ToString() == "Y")
            //    {
            //        sSql += "		OR REQ_SVC = 'AIR' OR SVC_TYPE = 'IMPORT' ";
            //    }
            //}
            sSql += "	GROUP BY REQ_SVC ";

            

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }



        // 월별 사용량 자료를 조회 한다
        public static DataTable GetYearCount(string Crn, string DateYyyy, string usr_type)
        {
            if (!string.IsNullOrEmpty(Crn) && !string.IsNullOrEmpty(DateYyyy))
            {
                sSql = "";
                //sSql += "	SELECT USE_YMD,	";
                //sSql += "			SUM (FILE_SIZE) AS FILE_SIZE";
                //sSql += "		FROM META_USE_DTL";
                //sSql += "	WHERE CUST_CD = '" + Crn + "' AND TO_DATE(USE_YMD,'YYYYMM') LIKE '" + prev_yyyy + "%'";
                //sSql += "	GROUP BY USE_YMD";
                //sSql += "	UNION";
                sSql += "	SELECT TO_CHAR(TO_DATE(USE_YMD),'YYYYMM') AS USE_YMD,	";
                sSql += "			SUM (FILE_SIZE) AS FILE_SIZE,";
                sSql += "			SUM (FILE_CNT) AS FILE_CNT";
                sSql += "		FROM META_USE_DTL";
                sSql += "	WHERE CRN = '" + Crn + "' AND USE_YMD BETWEEN TO_DATE('" + DateYyyy + "0101', 'YYYYMMDD') AND TO_DATE('" + DateYyyy + "1231', 'YYYYMMDD')";
                sSql += "	AND USR_TYPE = '" + usr_type + "'";
                sSql += "	GROUP BY TO_CHAR (TO_DATE(USE_YMD), 'YYYYMM')";
                sSql += "	ORDER BY USE_YMD";

            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }

        // 월별 사용량 자료를 조회 한다
        public static DataTable GetYearChart(string Crn, string DateYyyy, string DateMm, string prev_yyyy)
        {
            if (!string.IsNullOrEmpty(Crn) && !string.IsNullOrEmpty(DateYyyy) )
            {
                sSql = "";
                sSql += " SELECT TO_CHAR(TO_DATE(b.dt), 'YYYYMM') AS USE_YMD, NVL(SUM(a.FILE_CNT), 0) AS FILE_CNT	";
                sSql += " FROM (";
                sSql += "   SELECT USE_YMD AS USE_YMD, SUM(FILE_CNT) AS FILE_CNT";
                sSql += "   FROM META_USE_DTL";
                sSql += "   WHERE USE_YMD BETWEEN TO_DATE('" + prev_yyyy + "0101', 'YYYYMMDD') AND TO_DATE('" + prev_yyyy + "1201', 'YYYYMMDD')";
                sSql += "   AND CRN = '" + Crn + "'";
                sSql += "   GROUP BY USE_YMD";
                sSql += " ) a";
                sSql += " RIGHT JOIN (";
                sSql += "   SELECT TO_DATE('20230101', 'YYYYMMDD') + LEVEL - 1 AS dt";
                sSql += "   FROM dual ";
                sSql += "   CONNECT BY LEVEL <= (TO_DATE('" + prev_yyyy + "1231', 'YYYYMMDD') - TO_DATE('" + prev_yyyy + "0101', 'YYYYMMDD') + 1)";
                sSql += " ) b ON a.USE_YMD = b.dt";
                sSql += " GROUP BY TO_CHAR(TO_DATE(b.dt), 'YYYYMM')";
                sSql += " UNION";
                sSql += " SELECT TO_CHAR(TO_DATE(b.dt), 'YYYYMM') AS USE_YMD, NVL(SUM(a.FILE_CNT), 0) AS FILE_CNT";
                sSql += " FROM (";
                sSql += "   SELECT USE_YMD AS USE_YMD, SUM(FILE_CNT) AS FILE_CNT";
                sSql += "   FROM META_USE_DTL";
                sSql += "   WHERE USE_YMD BETWEEN TO_DATE('" + DateYyyy + "0101', 'YYYYMMDD') AND TO_DATE('" + DateYyyy + "1231', 'YYYYMMDD')";
                sSql += "   AND CRN = '" + Crn + "'";
                sSql += "   GROUP BY USE_YMD";
                sSql += " ) a";
                sSql += " RIGHT JOIN (";
                sSql += "   SELECT TO_DATE('20240101', 'YYYYMMDD') + LEVEL - 1 AS dt";
                sSql += "   FROM dual ";
                sSql += "   CONNECT BY LEVEL <= (TO_DATE('" + DateYyyy + "1231', 'YYYYMMDD') - TO_DATE('" + DateYyyy + "0101', 'YYYYMMDD') + 1)";
                sSql += " ) b ON a.USE_YMD = b.dt";
                sSql += " GROUP BY TO_CHAR(TO_DATE(b.dt), 'YYYYMM')";

            }

            rtnDt = _DataHelper.ExecuteDataTable(sSql, CommandType.Text);
            return rtnDt;
        }
    }
}