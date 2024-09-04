using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YJIT.Utils;

namespace ELVIS_META_COMMON.Query
{
    class Main_Query
    {
        string sqlstr;

        /// <summary>
        /// 메인 - 로그인
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnLogin_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "   SELECT A.USR_ID, ";
            sqlstr += "          A.OFFICE_NM, ";
            sqlstr += "          A.APV_YN, ";
            sqlstr += "          A.CRN, ";
            sqlstr += "          A.FOR_CRN, ";
            sqlstr += "          A.EMAIL, ";
            sqlstr += "          A.HP_NO, ";
            sqlstr += "          A.EMAIL_MSG, ";
            sqlstr += "          LISTAGG (B.PAGE_CD, ',') WITHIN GROUP (ORDER BY B.PAGE_CD) AS PAGE_CD, ";
            sqlstr += "          LISTAGG (B.PAGE_AUTH, ',') WITHIN GROUP (ORDER BY B.PAGE_CD) AS PAGE_AUTH ";
            sqlstr += "     FROM PRM_USR_MST A INNER JOIN PRM_USR_DTL B ON A.USR_ID = B.USR_ID ";
            sqlstr += "         WHERE 1=1  ";
            sqlstr += "         AND A.USR_ID = '" + dr["ID"].ToString() + "' ";
            sqlstr += "         AND A.PSWD = '" + YJIT.Utils.StringUtils.Md5Hash((string)dr["PSWD"]) + "'   ";
            sqlstr += " GROUP BY A.USR_ID, A.APV_YN, A.CRN , A.FOR_CRN, A.EMAIL, A.HP_NO , A.EMAIL_MSG , A.OFFICE_NM";

            return sqlstr;
        }

        /// <summary>
        /// 회원가입 - 아이디 중복 체크
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnCheckID_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT USR_ID ";
            sqlstr += "   FROM PRM_USR_MST ";
            sqlstr += "  WHERE USR_ID = '" + dr["ID"].ToString() + "' ";            

            return sqlstr;
        }

        /// <summary>
        /// 회원가입 - 포워더 CRN 체크
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnGetCRN_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT * ";
            sqlstr += "   FROM PRM_ELVIS_REG ";
            sqlstr += "  WHERE 1=1  ";
            sqlstr += "  AND CRN = '"+ dr["CRN"].ToString() + "'  ";
            sqlstr += "  AND APV_YN = 'Y'  ";
            //sqlstr += "  AND ELVIS_BIG = 'Y' "; //엘비스 빅 사용자만 회원가입 가능하게.
                        
            return sqlstr;
        }

        /// <summary>
        /// CRM 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnGetDomain_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT DOMAIN ";
            sqlstr += "   FROM PRM_ELVIS_REG ";
            sqlstr += "  WHERE 1=1  ";
            sqlstr += "  AND CRN IS NOT NULL  ";
            sqlstr += "  AND CRN = '" + dr["FOR_CRN"] + "' ";

            return sqlstr;
        }


        /// <summary>
        /// 회원가입
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnRegister_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " INSERT INTO PRM_USR_MST (USR_ID, ";
            sqlstr += "                          PSWD, ";
            sqlstr += "                          CRN, ";
            sqlstr += "                          DOMAIN, ";
            sqlstr += "                          FOR_CRN, ";
            sqlstr += "                          MNGT_NO, ";
            sqlstr += "                          OFFICE_NM, ";
            sqlstr += "                          HP_NO, ";
            sqlstr += "                          OFFICE_ADDR, ";
            sqlstr += "                          OFFICE_ADDR2, ";
            sqlstr += "                          OFFICE_ADDR_CD, ";
            sqlstr += "                          EMAIL, ";
            sqlstr += "                          APV_YN, ";
            sqlstr += "                          INS_USR, ";
            sqlstr += "                          INS_YMD, ";
            sqlstr += "                          INS_HM, ";
            sqlstr += "                          UPD_USR, ";
            sqlstr += "                          UPD_YMD, ";
            sqlstr += "                          UPD_HM) ";
            sqlstr += "      VALUES ( ";
            sqlstr += "      '" + dr["ID"] + "' ";
            sqlstr += "      ,'" + YJIT.Utils.StringUtils.Md5Hash((string)dr["PSWD"]) + "' ";
            sqlstr += "      ,'" + dr["CRN"] + "' ";
            sqlstr += "      ,'" + dr["DOMAIN"] + "' ";
            sqlstr += "      ,'" + dr["FOR_CRN"] + "' ";
            sqlstr += "      ,'" + dr["MNGT_NO"] + "' ";
            sqlstr += "      ,'" + dr["OFFICE_NM"] + "' ";
            sqlstr += "      ,'" + dr["HP_NO"] + "' ";
            sqlstr += "      ,'" + dr["OFFICE_ADDR"] + "' ";
            sqlstr += "      ,'" + dr["OFFICE_ADDR2"] + "' ";
            sqlstr += "      ,'" + dr["OFFICE_ADDR_CD"] + "' ";
            sqlstr += "      ,'" + dr["EMAIL"] + "' ";
            sqlstr += "      ,'" + dr["APV_YN"] + "' ";
            sqlstr += "      ,'WEB' ";
            sqlstr += "      ,UFN_DATE_FORMAT('DATE') ";
            sqlstr += "      ,UFN_DATE_FORMAT('TIME') ";
            sqlstr += "      ,'WEB' ";
            sqlstr += "      ,UFN_DATE_FORMAT('DATE') ";
            sqlstr += "      ,UFN_DATE_FORMAT('TIME') ";
            sqlstr += "      ) ";

            return sqlstr;
        }

        /// <summary>
        /// 회원가입 - DTL
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnRegister_DTL_Query(DataRow dr,string strPAGE)
        {
            sqlstr = "";

            sqlstr += " INSERT INTO PRM_USR_DTL (USR_ID, ";            
            sqlstr += "                          PAGE_CD, ";
            sqlstr += "                          PAGE_AUTH, ";
            sqlstr += "                          INS_USR, ";
            sqlstr += "                          INS_YMD, ";
            sqlstr += "                          INS_HM, ";
            sqlstr += "                          UPD_USR, ";
            sqlstr += "                          UPD_YMD, ";
            sqlstr += "                          UPD_HM) ";
            sqlstr += "      VALUES ( ";
            sqlstr += "      '" + dr["ID"] + "' ";                        
            sqlstr += "      ,'" + strPAGE + "' ";
            sqlstr += "      ,'N' ";
            sqlstr += "      ,'WEB' ";
            sqlstr += "      ,UFN_DATE_FORMAT('DATE') ";
            sqlstr += "      ,UFN_DATE_FORMAT('TIME') ";
            sqlstr += "      ,'WEB' ";
            sqlstr += "      ,UFN_DATE_FORMAT('DATE') ";
            sqlstr += "      ,UFN_DATE_FORMAT('TIME') ";
            sqlstr += "      ) ";

            return sqlstr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetUserInfo(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT USR_ID, ";
            sqlstr += "        LOC_NM AS USER_NM, ";
            sqlstr += "        EMAIL, ";
            sqlstr += "       HP_NO, ";
            sqlstr += "        CUST_CD, ";
            sqlstr += "        CUST_NM, ";
            sqlstr += "        USR_TYPE, ";
            sqlstr += "        APV_YN, ";
            sqlstr += "        AUTH_KEY, ";
            sqlstr += "        CASE ";
            sqlstr += "           WHEN AUTH_TYPE = 'A' THEN 'A' ";
            sqlstr += "           WHEN AUTH_TYPE = 'C' THEN 'B' ";
            sqlstr += "           ELSE 'C' ";
            sqlstr += "        END ";
            sqlstr += "           AS AUTH_TYPE, ";
            sqlstr += " DEF_OFFICE_CD AS OFFICE_CD ";
            sqlstr += "   FROM MDM_EXT_USR_MST ";
            sqlstr += "   WHERE 1=1 ";
            sqlstr += " AND USE_YN = 'Y' ";
            sqlstr += " AND UPPER(USR_ID) = UPPER('" + dr["USR_ID"] + "') ";
            sqlstr += " AND PSWD = '" + YJIT.Utils.StringUtils.Md5Hash((string)dr["PSWD"]) + "' ";

            return sqlstr;
        }

        public string MailLogin(DataRow dr)
        {
            sqlstr = "";
            sqlstr += " SELECT  B.LOC_NM ";
            sqlstr += "         , B.USR_ID ";
            sqlstr += "         , B.LOC_NM AS USER_NM ";
            sqlstr += "         , B.EMAIL ";
            sqlstr += "         , B.HP_NO ";
            sqlstr += "         , B.CUST_CD ";
            sqlstr += "         , B.CUST_NM ";
            sqlstr += "         , B.USR_TYPE ";
            sqlstr += "         , B.APV_YN ";
            sqlstr += "         , A.AUTH_KEY ";
            sqlstr += "         , AUTH_TYPE ";
            sqlstr += "         , DEF_OFFICE_CD AS OFFICE_CD";
            sqlstr += "         , A.JOB_TYPE AS JOB_TYPE";
            sqlstr += "         , A.DOMAIN AS DOMAIN";
            sqlstr += "         , A.REF1 ";
            sqlstr += "         , A.REF2 ";
            sqlstr += "         , A.REF3 ";
            sqlstr += "         , A.REF4 ";
            sqlstr += "         , A.REF5 ";
            sqlstr += " FROM PRM_EMAIL_LOG A";
            sqlstr += "     INNER JOIN MDM_EXT_USR_MST B ON UPPER(A.RECEIVE_MAIL) = UPPER(B.EMAIL) ";
            sqlstr += " WHERE 1=1 ";
            sqlstr += " AND A.AUTH_KEY = '" + dr["USR_ID"] + "' ";

            return sqlstr;
        }

        

        /// <summary>
		/// 간편 온라인 접수 Query
		/// </summary>
		/// <param name="dr"></param>
		/// <returns></returns>
		public string fnCrmRegMst_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " INSERT              ";
            sqlstr += "     INTO CRM_AS_MST ";
            sqlstr += "     (MNGT_NO        ";
            sqlstr += "     , CONTENT       ";
            sqlstr += "     , TEL_NO        ";
            sqlstr += "     , SYS_ID        ";
            sqlstr += "     , ONLINE_YN        ";
            sqlstr += "     , DEPT_CD        ";
            sqlstr += "     , REQ_YMD       ";
            sqlstr += "     , REQ_HM        ";
            sqlstr += "     , INS_YMD       ";
            sqlstr += "     , INS_HM        ";
            sqlstr += "     , UPD_YMD       ";
            sqlstr += "     , UPD_HM)       ";
            sqlstr += "     VALUES          ";
            sqlstr += " 	('" + dr["MNGT_NO"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["CONTENT"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["HP_NO"].ToString() + "'   ";
            sqlstr += " 	,'ELVIS'   ";
            sqlstr += " 	,'Y'   ";
            sqlstr += " 	,'01'   ";
            sqlstr += " 	, UFN_DATE_FORMAT('DATE')   ";
            sqlstr += " 	, UFN_DATE_FORMAT('TIME')   ";
            sqlstr += " 	, UFN_DATE_FORMAT('DATE')   ";
            sqlstr += " 	, UFN_DATE_FORMAT('TIME')   ";
            sqlstr += " 	, UFN_DATE_FORMAT('DATE')   ";
            sqlstr += " 	, UFN_DATE_FORMAT('TIME'))   ";

            return sqlstr;
        }

        /// <summary>
        /// 온라인 접수 Insert
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnCrmRegCust_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " 	INSERT                                                                                     ";
            sqlstr += " 	    INTO CRM_AS_CUST                                                                       ";
            sqlstr += " 	    (MNGT_NO                                                                               ";
            sqlstr += " 	    , SUBJECT                                                                              ";
            sqlstr += " 	    , INS_YMD                                                                              ";
            sqlstr += " 	    , INS_HM                                                                               ";
            sqlstr += " 	    , UPD_YMD                                                                              ";
            sqlstr += " 	    , UPD_HM)                                                                              ";
            sqlstr += " 	    VALUES                                                                                 ";
            sqlstr += " 	    ('" + dr["MNGT_NO"].ToString() + "'                                                        ";
            sqlstr += " 		,'[양재IT] ELVISBIG 가입 신청 입니다.'	                                                       ";
            sqlstr += " 	, UFN_DATE_FORMAT('DATE')   ";
            sqlstr += " 	, UFN_DATE_FORMAT('TIME')   ";
            sqlstr += " 	, UFN_DATE_FORMAT('DATE')   ";
            sqlstr += " 	, UFN_DATE_FORMAT('TIME'))   ";

            return sqlstr;
        }
    }
}
