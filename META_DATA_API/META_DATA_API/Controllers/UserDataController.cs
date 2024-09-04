using System;
using System.Text;
using System.IO;
using System.Data;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Configuration;
using System.Collections.Specialized;

using Newtonsoft.Json;
using META_DATA_API.Models;
using META_DATA_API.Models.Query;


namespace META_DATA_API.Controllers
{
    public class UserDataController : ApiController
    {
        private bool ErrorOccur = false;
        public string ReturnJsonVal;
        private string strProcLog = "";
        private DataTable dt = new DataTable();

        /// <summary>
        /// Set User Regist Data
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="USR_ID">아이디(이메일)*</param>
        /// <param name="PSWD">비밀번호*</param>
        /// <param name="USR_NM">담당자명*</param>
        /// <param name="HP_NO">핸드폰*</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="LOC_NM">사업자명(한글)*</param>
        /// <param name="LOC_ADDR">사업장주소(한글)*</param>
        /// <param name="BIZCOND">업태</param>
        /// <param name="BIZTYPE">종목</param>
        /// <param name="CEO">대표자명</param>
        /// <param name="TEL_NO">대표전화번호</param>
        /// <param name="ENG_NM">사업자명(영문)</param>
        /// <param name="ENG_ADDR">사업장주소(영문)</param>
        /// <param name="CTRY_CD">국가코드</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SetUserRegist")]
        public string SetUserRegist(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Usr_Id = "";
            string Param_Pswd = "";
            string Param_Usr_Nm = "";
            string Param_Hp_No = "";
            string Param_Loc_Nm = "";
            string Param_Domain = "";

            bool rtnStatus = false;
            string mngtNo = "";
            #endregion
            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        mngtNo = String.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssFFF"), (99 * (new Random().Next(8) + 2))).ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("USR_ID"))
                        {
                            Param_Usr_Id = dt.Rows[0]["USR_ID"].ToString();
                            if (string.IsNullOrEmpty(Param_Usr_Id)) _common.ThrowMsg(ErrorOccur, "USR_ID is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "USR_ID Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("PSWD"))
                        {
                            Param_Pswd = dt.Rows[0]["PSWD"].ToString();
                            if (string.IsNullOrEmpty(Param_Pswd)) _common.ThrowMsg(ErrorOccur, "PSWD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "PSWD Parameter is Null");
                        }

                        if (dt.Rows[0].Table.Columns.Contains("USR_NM"))
                        {
                            Param_Usr_Nm = dt.Rows[0]["USR_NM"].ToString();
                            if (string.IsNullOrEmpty(Param_Usr_Nm)) _common.ThrowMsg(ErrorOccur, "USR_NM is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "USR_NM Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("HP_NO"))
                        {
                            Param_Hp_No = dt.Rows[0]["HP_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Hp_No)) _common.ThrowMsg(ErrorOccur, "HP_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "HP_NO Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("LOC_NM"))
                        {
                            Param_Loc_Nm = dt.Rows[0]["LOC_NM"].ToString();
                            if (string.IsNullOrEmpty(Param_Loc_Nm)) _common.ThrowMsg(ErrorOccur, "LOC_NM is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "LOC_NM Parameter is Null");
                        }

                        if (dt.Rows[0].Table.Columns.Contains("DOMAIN")) Param_Domain = dt.Rows[0]["DOMAIN"].ToString();
                        #endregion

                        rtnStatus = Sql_UserData.SetUserRegist(mngtNo , dt.Rows[0]);

                        if (rtnStatus)
                        {
                            dt.Columns.Add("MNGT_NO");
                            dt.Rows[0]["MNGT_NO"] = mngtNo;
                            ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        }
                        else
                        {
                            ReturnJsonVal = _common.MakeJson("N", "Failed");
                        }

                        //Log 정보 저장
                        strProcLog = "[SetUserRegist ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[SetUserRegist-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            //CRM 온라인 접수 자동 등록
            try
            {
                //string sUploadHandler = "http://crm.yjit.co.kr:9634/wcf/UpdateCrm.aspx?";
                //WebClient wc = new System.Net.WebClient();
                //string content = "[ELVIS-FRIEND] 회원가입 신청";
                //content += System.Environment.NewLine + "아이디(이메일) : " + Param_Usr_Id;
                //content += System.Environment.NewLine + "담당자 명 : " + Param_Usr_Nm;
                //content += System.Environment.NewLine + "담당자(휴대전화) : " + Param_Hp_No;
                //content += System.Environment.NewLine + "사업자 명(한글) : " + Param_Loc_Nm;
                //content += System.Environment.NewLine + "사업장 주소(한글) : " + Param_Loc_Addr;

                //StringBuilder sb = new StringBuilder();
                //sb.Append(sUploadHandler);
                //sb.AppendFormat("CUST_CD={0}", "011906");//ELVIS-FRIEND 전용 화주 거래처코드로 고정
                //sb.AppendFormat("&CONTENT={0}", content);
                //sb.AppendFormat("&USR_NM={0}", Param_Usr_Nm);
                //sb.AppendFormat("&TEL_NO={0}", Param_Hp_No);
                //sb.AppendFormat("&OFFICE_CD={0}", "");

                //string call_upload_cmd = sb.ToString();
                //wc.DownloadString(call_upload_cmd);
            }
            catch (Exception ex)
            {
                strProcLog = "[SetUserRegist(CRM)-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Set Saas Service Regist Data
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="USR_ID">아이디(이메일)*</param>
        /// <param name="PSWD">비밀번호*</param>
        /// <param name="USR_NM">담당자명*</param>
        /// <param name="HP_NO">핸드폰*</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="LOC_NM">사업자명(한글)*</param>
        /// <param name="LOC_ADDR">사업장주소(한글)*</param>
        /// <param name="BIZCOND">업태</param>
        /// <param name="BIZTYPE">종목</param>
        /// <param name="CEO">대표자명</param>
        /// <param name="TEL_NO">대표전화번호</param>
        /// <param name="ENG_NM">사업자명(영문)</param>
        /// <param name="ENG_ADDR">사업장주소(영문)</param>
        /// <param name="CTRY_CD">국가코드</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SetSaasSvcRegist")]
        public string SetSaasSvcRegist(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Elvis_Usr_Id = "";
            string Param_Elvis_Pswd = "";
            string Param_Crn = "";
            string Param_Office_Nm = "";
            string Param_Loc_Nm = "";
            string Param_Office_Addr = "";
            string Param_Email = "";
            string Param_Hp_No = "";
            string Param_Reg_Date = "";
            string Param_Stat_Date = "";

            bool rtnStatus = false;
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("ELVIS_USR_ID"))
                        {
                            Param_Elvis_Usr_Id = dt.Rows[0]["ELVIS_USR_ID"].ToString();
                            if (string.IsNullOrEmpty(Param_Elvis_Usr_Id)) _common.ThrowMsg(ErrorOccur, "ELVIS_USR_ID is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "ELVIS_USR_ID Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("ELVIS_PSWD"))
                        {
                            Param_Elvis_Pswd = dt.Rows[0]["ELVIS_PSWD"].ToString();
                            if (string.IsNullOrEmpty(Param_Elvis_Pswd)) _common.ThrowMsg(ErrorOccur, "ELVIS_PSWD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "ELVIS_PSWD Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_Crn = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("HP_NO"))
                        {
                            Param_Office_Nm = dt.Rows[0]["OFFICE_NM"].ToString();
                            if (string.IsNullOrEmpty(Param_Office_Nm)) _common.ThrowMsg(ErrorOccur, "OFFICE_NM is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "OFFICE_NM Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("LOC_NM"))
                        {
                            Param_Loc_Nm = dt.Rows[0]["LOC_NM"].ToString();
                            if (string.IsNullOrEmpty(Param_Loc_Nm)) _common.ThrowMsg(ErrorOccur, "LOC_NM is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "LOC_NM Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("OFFICE_ADDR"))
                        {
                            Param_Office_Addr = dt.Rows[0]["OFFICE_ADDR"].ToString();
                            if (string.IsNullOrEmpty(Param_Office_Addr)) _common.ThrowMsg(ErrorOccur, "OFFICE_ADDR is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "OFFICE_ADDR Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("EMAIL"))
                        {
                            Param_Email = dt.Rows[0]["EMAIL"].ToString();
                            if (string.IsNullOrEmpty(Param_Email)) _common.ThrowMsg(ErrorOccur, "EMAIL is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "EMAIL Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("HP_NO"))
                        {
                            Param_Hp_No = dt.Rows[0]["HP_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Hp_No)) _common.ThrowMsg(ErrorOccur, "HP_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "HP_NO Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("REG_DATE"))
                        {
                            Param_Reg_Date = dt.Rows[0]["REG_DATE"].ToString();
                            if (string.IsNullOrEmpty(Param_Reg_Date)) _common.ThrowMsg(ErrorOccur, "REG_DATE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "REG_DATE Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("STAT_DATE"))
                        {
                            Param_Stat_Date = dt.Rows[0]["STAT_DATE"].ToString();
                            if (string.IsNullOrEmpty(Param_Stat_Date)) _common.ThrowMsg(ErrorOccur, "STAT_DATE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "STAT_DATE Parameter is Null");
                        }
                        #endregion

                        rtnStatus = Sql_UserData.SetSaasSvcRegist(dt.Rows[0]);

                        if (rtnStatus)
                        {
                            ReturnJsonVal = _common.MakeJson("Y", "Success");
                        }
                        else
                        {
                            ReturnJsonVal = _common.MakeJson("N", "Failed");
                        }

                        //Log 정보 저장
                        strProcLog = "[SetSaasSvcRegist ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[SetSaasSvcRegist-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get User Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="USR_ID">아이디(이메일)*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetUserInfo")]
        public string GetUserInfo(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Usr_Id = "";
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("USR_ID"))
                        {
                            //Param_Usr_Id = ConvertBase64ToOrign(dt.Rows[0]["USR_ID"].ToString());
                            Param_Usr_Id = dt.Rows[0]["USR_ID"].ToString();
                            if (string.IsNullOrEmpty(Param_Usr_Id)) _common.ThrowMsg(ErrorOccur, "USR_ID is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "USR_ID Parameter is Null");
                        }
                        #endregion

                        dt = Sql_UserData.GetUserInfo(Param_Usr_Id);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "User not found");                        

                        //Log 정보 저장
                        strProcLog = "[GetUserInfo ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetUserInfo-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }


        /// <summary>
        /// Get User Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="USR_ID">아이디(이메일)*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GoodBye")]
        public string GoodBye(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Usr_Id = "";
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("USR_ID"))
                        {
                            Param_Usr_Id = dt.Rows[0]["USR_ID"].ToString();
                            if (string.IsNullOrEmpty(Param_Usr_Id)) _common.ThrowMsg(ErrorOccur, "USR_ID is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "USR_ID Parameter is Null");
                        }
                        #endregion

                        bool rtnStatus = false;
                        rtnStatus = Sql_UserData.SetUserStatus(Param_Usr_Id);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "User not found");

                        //Log 정보 저장
                        strProcLog = "[GetUserInfo ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetUserInfo-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }
        /// <summary>
        /// Find User Id
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="USR_NM">사용자 명(한글)*</param>
        /// <param name="HP_NO">핸드폰*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("FindUserId")]
        public string FindUserId(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Usr_Nm = "";
            string Param_Hp_No = "";
            #endregion
            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("USR_NM"))
                        {
                            Param_Usr_Nm = dt.Rows[0]["USR_NM"].ToString();
                            if (string.IsNullOrEmpty(Param_Usr_Nm)) _common.ThrowMsg(ErrorOccur, "USR_NM is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "USR_NM Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("HP_NO"))
                        {
                            Param_Hp_No = dt.Rows[0]["HP_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Hp_No)) _common.ThrowMsg(ErrorOccur, "HP_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "HP_NO Parameter is Null");
                        }
                        #endregion

                        dt = Sql_UserData.FindUserId(Param_Usr_Nm, Param_Hp_No);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "User not exist");
                        

                        //Log 정보 저장
                        strProcLog = "[FindUserId ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[FindUserId-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Set User Password Reset
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="USR_ID">아이디(이메일)*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SetUserPswdReset")]
        public string SetUserPswdReset(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Usr_Id = "";
            string Param_Pswd = "";

            bool rtnStatus = false;
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("USR_ID"))
                        {
                            Param_Usr_Id = dt.Rows[0]["USR_ID"].ToString();
                            if (string.IsNullOrEmpty(Param_Usr_Id)) _common.ThrowMsg(ErrorOccur, "USR_ID is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "USR_ID Parameter is Null");
                        }

                        if (dt.Rows[0].Table.Columns.Contains("PSWD"))
                        {
                            Param_Pswd = dt.Rows[0]["PSWD"].ToString();
                            if (string.IsNullOrEmpty(Param_Pswd)) _common.ThrowMsg(ErrorOccur, "PSWD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "PSWD Parameter is Null");
                        }
                        #endregion

                        rtnStatus = Sql_UserData.SetUserPswdReset(Param_Usr_Id, Param_Pswd);

                        if (rtnStatus)
                        {
                            //임시 비밀번호 메일 전송
                            string subject = "[ELVIS-FRIEND] 임시 비밀번호 입니다.";
                            SendEmail(subject, Param_Usr_Id, MakeEmailForm(Param_Pswd));
                            ReturnJsonVal = _common.MakeJson("Y", "Success");
                        }
                        else
                        {
                            ReturnJsonVal = _common.MakeJson("N", "Failed");
                        }

                        //Log 정보 저장
                        strProcLog = "[SetUserPswdReset ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[SetUserPswdReset-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Set User Infomations Update
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="USR_ID">아이디(이메일)*</param>
        /// <param name="PSWD">비밀번호</param>
        /// <param name="USR_NM">담당자명</param>
        /// <param name="HP_NO">핸드폰</param>
        /// <param name="LOC_ADDR">사업장주소(한글)</param>
        /// <param name="BIZCOND">업태</param>
        /// <param name="BIZTYPE">종목</param>
        /// <param name="CEO">대표자명</param>
        /// <param name="TEL_NO">대표전화번호</param>
        /// <param name="ENG_NM">사업자명(영문)<>/param>
        /// <param name="ENG_ADDR"">사업장주소(영문)</param>
        /// <param name="CTRY_CD">국가코드</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SetUserInfo")]
        public string SetUserInfo(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Usr_Id = "";
            string Param_Pswd = "";
            string Param_Usr_Nm = "";
            string Param_Hp_No = "";

            bool rtnStatus = false;
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("USR_ID"))
                        {
                            Param_Usr_Id = dt.Rows[0]["USR_ID"].ToString();
                            if (string.IsNullOrEmpty(Param_Usr_Id)) _common.ThrowMsg(ErrorOccur, "USR_ID is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "USR_ID Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("PSWD")) Param_Pswd = dt.Rows[0]["PSWD"].ToString();
                        if (dt.Rows[0].Table.Columns.Contains("USR_NM")) Param_Usr_Nm = dt.Rows[0]["USR_NM"].ToString();
                        if (dt.Rows[0].Table.Columns.Contains("HP_NO")) Param_Hp_No = dt.Rows[0]["HP_NO"].ToString();
                        #endregion

                        rtnStatus = Sql_UserData.SetUserInfo(Param_Usr_Id, Param_Pswd, Param_Usr_Nm, Param_Hp_No);

                        if (rtnStatus)
                        {
                            ReturnJsonVal = _common.MakeJson("Y", "Success");
                        }
                        else
                        {
                            ReturnJsonVal = _common.MakeJson("N", "Failed");
                        }

                        //Log 정보 저장
                        strProcLog = "[SetUserInfo ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[SetUserInfo-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Customer License File Upload
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="MNGT_NO">관리번호*</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SetUserServiceInfo")]
        public string SetUserServiceInfo(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Usr_Id = "";
            string Param_Usr_Docu = "";
            string Param_Usr_Quot = "";
            string Param_Usr_Ex = "";
            string Param_Usr_Im = "";

            bool rtnStatus = false;
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("USR_ID"))
                        {
                            //Param_Usr_Id = ConvertBase64ToOrign(dt.Rows[0]["USR_ID"].ToString());
                            Param_Usr_Id = dt.Rows[0]["USR_ID"].ToString();
                            if (string.IsNullOrEmpty(Param_Usr_Id)) _common.ThrowMsg(ErrorOccur, "USR_ID is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "USR_ID Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("DOCU_YN")) Param_Usr_Docu = dt.Rows[0]["DOCU_YN"].ToString();
                        if (dt.Rows[0].Table.Columns.Contains("QUOT_YN")) Param_Usr_Quot = dt.Rows[0]["QUOT_YN"].ToString();
                        if (dt.Rows[0].Table.Columns.Contains("EX_YN")) Param_Usr_Ex = dt.Rows[0]["EX_YN"].ToString();
                        if (dt.Rows[0].Table.Columns.Contains("IM_YN")) Param_Usr_Im = dt.Rows[0]["IM_YN"].ToString();
                        #endregion

                        rtnStatus = Sql_UserData.SetUserServiceInfo(Param_Usr_Id, Param_Usr_Docu, Param_Usr_Quot, Param_Usr_Ex, Param_Usr_Im);

                        if (rtnStatus)
                        {
                            ReturnJsonVal = _common.MakeJson("Y", "Success");
                        }
                        else
                        {
                            ReturnJsonVal = _common.MakeJson("N", "Failed");
                        }

                        //Log 정보 저장
                        strProcLog = "[SetUserServiceInfo ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[SetUserServiceInfo-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Customer License File Upload
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="MNGT_NO">관리번호*</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("LicenseUpload")]
        public HttpResponseMessage LicenseUpload()
        {
            #region // 변수 선언 영역
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            var DocPath = "/EDMS/FRND/CRN/";

            string Param_Mngt_No = "";

            string svrfileName = "";
            string fileName = "";
            string filePath = "";
            string fileExt = "";
            int fileSize = 0;
            #endregion

            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    #region // Validation Check & Save Variable
                    if (!string.IsNullOrEmpty(httpRequest.Form.GetValues("MNGT_NO")[0].ToString()))
                    {
                        Param_Mngt_No = httpRequest.Form.GetValues("MNGT_NO")[0].ToString();
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "MNGT_NO Parameter is Null");
                    }
                    //if (!string.IsNullOrEmpty(httpRequest.Form.GetValues("DOC_TYPE")[0].ToString()))
                    //{
                    //    Param_Type = httpRequest.Form.GetValues("DOC_TYPE")[0].ToString();
                    //}
                    //else
                    //{
                    //    _common.ThrowMsg(ErrorOccur, "DOC_TYPE Parameter is Null");
                    //}
                    #endregion

                    var uploadFile = httpRequest.Files[0];
                    var fullPath = HttpContext.Current.Server.MapPath("~" + DocPath + "/" + Param_Mngt_No + "/" + uploadFile.FileName);
                    FileInfo fileInfo = new FileInfo(fullPath);

                    svrfileName = string.Format("{0}_{1}", Param_Mngt_No, DateTime.Now.ToString("yyyyMMddHHmmssFFF"));
                    fileName = uploadFile.FileName.Substring(0, uploadFile.FileName.IndexOf("."));
                    filePath = fullPath.Substring(0, fullPath.LastIndexOf("\\")).Replace("\\", "/");
                    fileExt = uploadFile.FileName.Substring(uploadFile.FileName.IndexOf("."));
                    fileSize = uploadFile.ContentLength;

                    if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                    uploadFile.SaveAs(fullPath);

                    //CRM 문서 UPLOAD
                    //System.Net.WebClient wc = new System.Net.WebClient();
                    //string sUploadHandler = "http://crm.yjit.co.kr:9634/wcf/UploadHandler.aspx";
                    //string SavePath = DocPath + Param_Crn + "/" + Param_Mngt_No + "/";
                    //NameValueCollection myQueryStringCollection = new NameValueCollection();
                    //string strFilePath = HttpContext.Current.Server.MapPath("~" + SavePath + fileName + fileExt);

                    //myQueryStringCollection.Add("SavePath", SavePath);
                    //wc.QueryString = myQueryStringCollection;
                    //wc.UploadFile(sUploadHandler, "POST", strFilePath);

                    Sql_UserData.LicenseUpload(Param_Mngt_No, fileName, DocPath + "/" + Param_Mngt_No + "/", fileExt, fileSize);

                    strProcLog = "[LicenseUpload ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + httpRequest.Params.Get(0) + System.Environment.NewLine;
                    _LogWriter.Writer(strProcLog);
                    result = Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                strProcLog = "[LicenseUpload-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Get Customer License File Path
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="MNGT_NO">관리번호*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetLicensePath")]
        public string GetLicensePath(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Mngt_No = "";
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("MNGT_NO"))
                        {
                            Param_Mngt_No = dt.Rows[0]["MNGT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Mngt_No)) _common.ThrowMsg(ErrorOccur, "MNGT_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "MNGT_NO Parameter is Null");
                        }
                        #endregion

                        dt = Sql_UserData.GetLicensePath(Param_Mngt_No);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not Found");

                        //Log 정보 저장
                        strProcLog = "[GetLicensePath ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetLicensePath-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get SeaVantage Auth Token
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetSvtgAuthToken")]
        public string GetSvtgAuthToken()
        {
            #region // 변수 선언 영역
            string connStr = "";
            #endregion

            try
            {
                #region // Validation Check & Save Variable                        
                #endregion

                connStr = ConfigurationManager.ConnectionStrings["CRM_ORACLE"].ConnectionString;
                dt = Sql_UserData.GetSvtgAuthToken(connStr);

                if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                else ReturnJsonVal = _common.MakeJson("N", "Data not exist");
                
                //Log 정보 저장
                strProcLog = "[GetSvtgAuthToken ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Success!   " + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetSvtgAuthToken-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Set SeaVantage Auth Token
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="AUTH_TOKEN">아이디(이메일)*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SetSvtgAuthToken")]
        public string SetSvtgAuthToken(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Auth_Token = "";
            string connStr = "";

            bool rtnStatus = false;
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("AUTH_TOKEN"))
                        {
                            Param_Auth_Token = dt.Rows[0]["AUTH_TOKEN"].ToString();
                            if (string.IsNullOrEmpty(Param_Auth_Token)) _common.ThrowMsg(ErrorOccur, "AUTH_TOKEN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "AUTH_TOKEN Parameter is Null");
                        }
                        #endregion

                        connStr = ConfigurationManager.ConnectionStrings["CRM_ORACLE"].ConnectionString;
                        rtnStatus = Sql_UserData.SetSvtgAuthToken(connStr, Param_Auth_Token);

                        if (rtnStatus) ReturnJsonVal = _common.MakeJson("Y", "Success");
                        else ReturnJsonVal = _common.MakeJson("N", "Failed");

                        //Log 정보 저장
                        strProcLog = "[SetSvtgAuthToken ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[SetSvtgAuthToken-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get DashBoard Data Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="DATE_YYYY">년도*</param>
        /// <param name="DATE_MM">월*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetDashBoardInfo")]
        public string GetDashBoardInfo(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Date_Yyyy = "";
            string Param_Date_Mm = "";
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_Crn = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("DATE_YYYY"))
                        {
                            Param_Date_Yyyy = dt.Rows[0]["DATE_YYYY"].ToString();
                            if (string.IsNullOrEmpty(Param_Date_Yyyy)) _common.ThrowMsg(ErrorOccur, "DATE_YYYY is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "DATE_YYYY Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("DATE_MM"))
                        {
                            Param_Date_Mm = dt.Rows[0]["DATE_MM"].ToString();
                            if (string.IsNullOrEmpty(Param_Date_Mm)) _common.ThrowMsg(ErrorOccur, "DATE_MM is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "DATE_MM Parameter is Null");
                        }
                        #endregion

                        dt = Sql_UserData.GetDashBoardInfo(Param_Crn, Param_Date_Yyyy, Param_Date_Mm);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");
                        
                        //Log 정보 저장
                        strProcLog = "[GetDashBoardInfo ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetDashBoardInfo-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get DashBoard Data Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="DATE_YYYY">년도*</param>
        /// <param name="DATE_MM">월*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetFDashBoardInfo")]
        public string GetDashFBoardInfo(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Date_Yyyy = "";
            string Param_Date_Mm = "";
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_Crn = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("DATE_YYYY"))
                        {
                            Param_Date_Yyyy = dt.Rows[0]["DATE_YYYY"].ToString();
                            if (string.IsNullOrEmpty(Param_Date_Yyyy)) _common.ThrowMsg(ErrorOccur, "DATE_YYYY is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "DATE_YYYY Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("DATE_MM"))
                        {
                            Param_Date_Mm = dt.Rows[0]["DATE_MM"].ToString();
                            if (string.IsNullOrEmpty(Param_Date_Mm)) _common.ThrowMsg(ErrorOccur, "DATE_MM is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "DATE_MM Parameter is Null");
                        }
                        #endregion

                        dt = Sql_UserData.GetDashFBoardInfo(Param_Crn, Param_Date_Yyyy, Param_Date_Mm);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");

                        //Log 정보 저장
                        strProcLog = "[GetFDashBoardInfo ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetFDashBoardInfo-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get DashBoard Data Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="DATE_YYYY">년도*</param>
        /// <param name="DATE_MM">월*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetUsedInfo")]
        public string GetUsedInfo(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Date_Yyyy = "";
            string Param_Date_Mm = "";
            string Param_Doc_Type = "";
            string Param_Svc_Type = "";
            string Param_Usr_Type = "";
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_Crn = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("DATE_YYYY"))
                        {
                            Param_Date_Yyyy = dt.Rows[0]["DATE_YYYY"].ToString();
                            if (string.IsNullOrEmpty(Param_Date_Yyyy)) _common.ThrowMsg(ErrorOccur, "DATE_YYYY is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "DATE_YYYY Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("DATE_MM"))
                        {
                            Param_Date_Mm = dt.Rows[0]["DATE_MM"].ToString();
                            if (string.IsNullOrEmpty(Param_Date_Mm)) _common.ThrowMsg(ErrorOccur, "DATE_MM is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "DATE_MM Parameter is Null");
                        }
                        #endregion
                        if (dt.Rows[0]["USR_TYPE"].ToString() == "S")
                        {

                            Param_Doc_Type = dt.Rows[0]["DOC_TYPE"].ToString();
                            Param_Svc_Type = dt.Rows[0]["SVC_TYPE"].ToString();
                            Param_Usr_Type = dt.Rows[0]["USR_TYPE"].ToString();

                            dt = Sql_UserData.GetUsedInfo(Param_Crn, Param_Date_Yyyy, Param_Date_Mm, Param_Doc_Type, Param_Svc_Type, Param_Usr_Type);
                        }
                        else {
                            dt = Sql_UserData.GetPartnerUsedInfo(dt.Rows[0]);
                        }
                        
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");

                        //Log 정보 저장
                        strProcLog = "[GetDashBoardInfo ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetDashBoardInfo-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get DashBoard Data Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="DATE_YYYY">년도*</param>
        /// <param name="DATE_MM">월*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetYearList")]
        public string GetYearList(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Date_Yyyy = "";
            string Param_Date_Mm = "";
            string Param_Doc_Type = "";
            string Param_Svc_Type = "";
            string Param_Use_type = "";
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_Crn = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("DATE_YYYY"))
                        {
                            Param_Date_Yyyy = dt.Rows[0]["DATE_YYYY"].ToString();
                            if (string.IsNullOrEmpty(Param_Date_Yyyy)) _common.ThrowMsg(ErrorOccur, "DATE_YYYY is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "DATE_YYYY Parameter is Null");
                        }
                        #endregion
                        dt = Sql_UserData.GetYearCount(Param_Crn, Param_Date_Yyyy, Param_Use_type);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");

                        //Log 정보 저장
                        strProcLog = "[GetDashBoardInfo ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetDashBoardInfo-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get DashBoard Data Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="DATE_YYYY">년도*</param>
        /// <param name="DATE_MM">월*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetYearCount")]
        public string GetYearCount(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Date_Yyyy = "";
            string Param_Date_Mm = "";
            string Param_Doc_Type = "";
            string Param_Use_type = "";
            string Param_Prev_Yyyy = "";
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        Param_Prev_Yyyy = dt.Rows[0]["PREV_YYYY"].ToString();
                        Param_Use_type = dt.Rows[0]["USR_TYPE"].ToString();
                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_Crn = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("DATE_YYYY"))
                        {
                            Param_Date_Yyyy = dt.Rows[0]["DATE_YYYY"].ToString();
                            if (string.IsNullOrEmpty(Param_Date_Yyyy)) _common.ThrowMsg(ErrorOccur, "DATE_YYYY is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "DATE_YYYY Parameter is Null");
                        }
                        //if (dt.Rows[0].Table.Columns.Contains("DATE_MM"))
                        //{
                        //    Param_Date_Mm = dt.Rows[0]["DATE_MM"].ToString();
                        //    if (string.IsNullOrEmpty(Param_Date_Mm)) _common.ThrowMsg(ErrorOccur, "DATE_MM is Empty");
                        //}
                        //else
                        //{
                        //    _common.ThrowMsg(ErrorOccur, "DATE_MM Parameter is Null");
                        //}
                        #endregion
                        DataSet ds = new DataSet();
                        dt = Sql_UserData.GetYearCount(Param_Crn, Param_Date_Yyyy, Param_Use_type);
                        dt.TableName = "YEARLIST";
                        ds.Tables.Add(dt);

                        dt = Sql_UserData.GetYearChart(Param_Crn, Param_Date_Yyyy, Param_Date_Mm, Param_Prev_Yyyy);
                        dt.TableName = "YEARCHART";
                        ds.Tables.Add(dt);

                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", ds);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");

                        //Log 정보 저장
                        strProcLog = "[GetDashBoardInfo ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetDashBoardInfo-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }
        public void SendEmail(string subject, string strTo, string strBody)
        {
            try
            {
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("mail.yjit.co.kr", 587)
                {
                    UseDefaultCredentials = false, // 시스템에 설정된 인증 정보를 사용하지 않는다. 
                    EnableSsl = true,  // SSL을 사용한다. 
                    DeliveryMethod = SmtpDeliveryMethod.Network, // 이걸 하지 않으면 Gmail에 인증을 받지 못한다. 
                    Credentials = new System.Net.NetworkCredential("mailmaster@yjit.co.kr", "Yjit0921)#$%"),
                    Timeout = 100000
                };

                MailAddress from = new MailAddress("help@yjit.co.kr");
                MailAddress to = new MailAddress(strTo);

                MailMessage message = new MailMessage(from, to);
                message.Body = strBody;
                message.IsBodyHtml = true;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = subject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;

                //서버 인증서의 유효성 검사하는 부분을 무조건 true 
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                client.Send(message);
                message.Dispose();
            }
            catch (Exception ex)
            {
                strProcLog = "[SendEmail-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
                _common.ThrowMsg(ErrorOccur, ex.Message);
            }
        }

        public string MakeEmailForm(string strPswd)
        {

            string strHTML = "";
            strHTML += "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
            strHTML += "<html xmlns=\"http://www.w3.org/1999/xhtml\">";
            strHTML += "<head>";
            strHTML += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />";
            strHTML += "    <!--[if !mso]><!-->";
            strHTML += "        <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />";
            strHTML += "    <!--<![endif]-->";
            strHTML += "    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">";
            strHTML += "    <title></title>";
            strHTML += "</head>";
            strHTML += "<body style=\"margin: 0; padding: 0;\">";
            strHTML += "   ";
            strHTML += "   <div style=\"max-width: 700px;margin: 0 auto;width: 100%;\" align=\"center\">";
            strHTML += "      <table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" align=\"center\" style=\"margin: 0 auto;\">";
            strHTML += "         <tr>";
            strHTML += "            <td valign=\"middle\" style=\"padding:0 13px;background-color: #f49f17;color: #ffffff;font-size: 20px;height:70px;letter-spacing:-1px;font-family:'Noto Sans KR', 'Noto Sans', 'Malgun Gothic', 'Apple SD Gothic Neo', sans-serif;\">";
            strHTML += "                    <div style='max-width: 610px;margin: 0 auto'> 안녕하세요. ELVIS-FRIEND 입니다.</div>";
            strHTML += "            </td>";
            strHTML += "         </tr>";
            strHTML += "         <tr>";
            strHTML += "            <td valign=\"top\" align=\"center\" style=\"padding:80px 0 50px;background-color: #ffffff;color: #666666;font-size: 20px;letter-spacing:-1px;font-family:'Noto Sans KR', 'Noto Sans', 'Malgun Gothic', 'Apple SD Gothic Neo', sans-serif;font-weight: 300;\">";
            strHTML += "               <span style=\"font-weight: 600;color: #111111;\">[ELVIS-FRIEND] 임시 비밀번호 입니다. ";
            strHTML += "            </td>";
            strHTML += "         </tr>";

            strHTML += "         <tr>";
            strHTML += "            <td valign=\"top\" style=\"padding:0 13px;\">";
            strHTML += "               <div style=\"max-width: 610px;margin: 0 auto\">";
            strHTML += "                  <table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" style=\"border-top: 2px solid #222222;width: 100%;\">";
            strHTML += "                <tr>";
            strHTML += "                        <td valign=\"middle\" align=\"center\" width=\"28%\" style=\"width:28%;padding:0;height:43px;font-size: 14px;color: #333333;background-color: #f8f8f8;border-bottom: 1px solid #eaeaea;font-family:'Noto Sans KR', 'Noto Sans', 'Malgun Gothic', 'Apple SD Gothic Neo', sans-serif;font-weight: 600;\">";
            strHTML += "                              <strong>임시 비밀번호</strong>";
            strHTML += "                        </td>  ";
            strHTML += "                        <td valign=\"middle\" align=\"left\" style=\"padding:0 0 0 20px;height:43px;font-size: 14px;color: #888888;border-bottom: 1px solid #eaeaea;font-family:'Noto Sans KR', 'Noto Sans', 'Malgun Gothic', 'Apple SD Gothic Neo', sans-serif;font-weight: 300;\">";
            strHTML += "                           <Pre>" + strPswd + "</Pre> ";
            strHTML += "                        </td> ";
            strHTML += "               </tr>";
            strHTML += "                   </table> ";
            strHTML += "                 </div> ";
            strHTML += "             </td>";
            strHTML += "         </tr>";
            strHTML += "         <tr>";
            strHTML += "            <td valign=\"middle\" align=\"center\" style=\"padding:0 13px;background-color: #f2f2f2;height: 89px;\">";
            strHTML += "               <div style=\"max-width: 610px;margin: 0 auto;\">";
            strHTML += "                  <p style=\"font-size: 13px;color: #999999;font-family:'Noto Sans KR', 'Noto Sans', 'Malgun Gothic', 'Apple SD Gothic Neo', sans-serif;font-weight: 300;margin: 6px 0 0;\">본 메일은 발신 전용이므로 회신할 수 없습니다.</p>";
            strHTML += "               </div>";
            strHTML += "            </td>";
            strHTML += "         </tr>   ";
            strHTML += "      </table>";
            strHTML += "   </div>";
            strHTML += "</body>";
            strHTML += "</html>";
            return strHTML;
        }

        /// <summary>
        /// base64 Convert String 
        /// </summary>
        /// <param name="encryTxt"></param>
        /// <returns></returns>
        private string ConvertBase64ToOrign(string encryTxt)
        {
            string resultTxt = "";


            byte[] byteEncrypt = Convert.FromBase64String(encryTxt);

            resultTxt = Encoding.UTF8.GetString(byteEncrypt);

            return resultTxt;
        }
    }
}