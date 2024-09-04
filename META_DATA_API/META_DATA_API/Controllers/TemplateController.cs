using System;
using System.IO;
using System.Data;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Collections.Specialized;

using Newtonsoft.Json;
using META_DATA_API.Models;
using META_DATA_API.Models.Query;

namespace FileUpload.Controllers
{
    public class TemplateController : ApiController
    {
        private bool ErrorOccur = false;
        public string ReturnJsonVal;
        private string strProcLog = "";
        private DataTable dt = new DataTable();
        private WebClient wc = new WebClient();

        /// <summary>
        /// Templat File Upload
        /// </summary>        
        /// <param name="FILE">사업자등록증 파일*</param>
        /// <param name="MNGT_NO">관리번호*</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="TMPLT_NM">서식 명*</param>
        /// <param name="TMPLT_TYPE">서식 종류*</param>
        /// <param name="RMK">비고</param>
        /// <param name="USR_ID">아이디(이메일)*</param>
        /// <param name="USR_NM">담당자명</param>
        /// <param name="HP_NO">핸드폰</param>
        /// <param name="LOC_NM">사업자명(한글)</param>
        /// 
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("TemplateUpload")]
        public HttpResponseMessage TemplateUpload()
        {
            #region // 변수 선언 영역
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            var TemplatePath = "/EDMS/FRND/TEMPLATE/";

            string Param_Mngt_No = "";
            string Param_Crn = "";
            string Param_Tmplt_Name = "";
            string Param_Tmplt_Type = "";
            string Param_Rmk = "";
            string Param_Usr_Id = "";
            string Param_Usr_Nm = "";
            string Param_Hp_No = "";
            string Param_Loc_Nm = "";

            string fileName = "";
            string filePath = "";
            string fileExt = "";
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
                    if (!string.IsNullOrEmpty(httpRequest.Form.GetValues("CRN")[0].ToString()))
                    {
                        Param_Crn = httpRequest.Form.GetValues("CRN")[0].ToString();
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                    }
                    if (!string.IsNullOrEmpty(httpRequest.Form.GetValues("TMPLT_NAME")[0].ToString()))
                    {
                        Param_Tmplt_Name = httpRequest.Form.GetValues("TMPLT_NAME")[0].ToString();
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "TMPLT_NAME Parameter is Null");
                    }
                    if (!string.IsNullOrEmpty(httpRequest.Form.GetValues("TMPLT_TYPE")[0].ToString()))
                    {
                        Param_Tmplt_Type = httpRequest.Form.GetValues("TMPLT_TYPE")[0].ToString();
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "TMPLT_TYPE Parameter is Null");
                    }
                    if (!string.IsNullOrEmpty(httpRequest.Form.GetValues("RMK")[0].ToString())) Param_Rmk = httpRequest.Form.GetValues("RMK")[0].ToString();
                    if (!string.IsNullOrEmpty(httpRequest.Form.GetValues("USR_ID")[0].ToString()))
                    {
                        Param_Usr_Id = httpRequest.Form.GetValues("USR_ID")[0].ToString();
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "USR_ID Parameter is Null");
                    }
                    if (!string.IsNullOrEmpty(httpRequest.Form.GetValues("USR_NM")[0].ToString()))
                    {
                        Param_Usr_Nm = httpRequest.Form.GetValues("USR_NM")[0].ToString();
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "USR_NM Parameter is Null");
                    }
                    if (!string.IsNullOrEmpty(httpRequest.Form.GetValues("HP_NO")[0].ToString()))
                    {
                        Param_Hp_No = httpRequest.Form.GetValues("HP_NO")[0].ToString();
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "HP_NO Parameter is Null");
                    }
                    if (!string.IsNullOrEmpty(httpRequest.Form.GetValues("LOC_NM")[0].ToString()))
                    {
                        Param_Loc_Nm = httpRequest.Form.GetValues("LOC_NM")[0].ToString();
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "LOC_NM Parameter is Null");
                    }
                    #endregion

                    var uploadFile = httpRequest.Files[0];
                    var fullPath = HttpContext.Current.Server.MapPath("~" + TemplatePath + Param_Crn + "/" + uploadFile.FileName);

                    fileName = uploadFile.FileName.Substring(0, uploadFile.FileName.IndexOf("."));
                    filePath = fullPath.Substring(0, fullPath.LastIndexOf("\\")).Replace("\\", "/");
                    fileExt = uploadFile.FileName.Substring(uploadFile.FileName.IndexOf("."));

                    if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                    uploadFile.SaveAs(fullPath);

                    //CRM 서식 UPLOAD
                    wc = new WebClient();
                    string sUploadHandler = "http://crm.yjit.co.kr:9634/wcf/UploadHandler.aspx";
                    string SavePath = TemplatePath + Param_Crn + "/";
                    NameValueCollection myQueryStringCollection = new NameValueCollection();
                    string strFilePath = HttpContext.Current.Server.MapPath("~" + SavePath + fileName + fileExt);

                    myQueryStringCollection.Add("SavePath", SavePath);
                    wc.QueryString = myQueryStringCollection;
                    wc.UploadFile(sUploadHandler, "POST", strFilePath);

                    Sql_Template.TemplateUpload(Param_Mngt_No, Param_Crn, Param_Tmplt_Name, Param_Tmplt_Type, Param_Rmk, fileName, TemplatePath + Param_Crn, fileExt, Param_Usr_Id);

                    strProcLog = "[TemplateUpload ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + httpRequest.Params.Get(0) + System.Environment.NewLine;
                    _LogWriter.Writer(strProcLog);

                    //CRM 온라인 접수 자동 등록
                    try
                    {
                        sUploadHandler = "http://crm.yjit.co.kr:9634/wcf/UpdateCrm.aspx?";
                        wc = new WebClient();
                        string content = "[ELVIS-FRIEND] 신규 서식등록 요청";
                        content += System.Environment.NewLine + "아이디(이메일) : " + Param_Usr_Id;
                        content += System.Environment.NewLine + "담당자 명 : " + Param_Usr_Nm;
                        if (Param_Hp_No.StartsWith("02")) content += System.Environment.NewLine + "담당자 연락처 : " + Param_Hp_No.Substring(0, 2) + "-" + Param_Hp_No.Substring(2, 4) + "-" + Param_Hp_No.Substring(6);
                        else content += System.Environment.NewLine + "담당자 연락처 : " + Param_Hp_No.Substring(0, 3) + "-" + Param_Hp_No.Substring(3, 4) + "-" + Param_Hp_No.Substring(7);                        
                        content += System.Environment.NewLine + "사업자 명(한글) : " + Param_Loc_Nm;
                        content += System.Environment.NewLine + "사업자 등록번호 : " + Param_Crn.Substring(0, 3) + "-" + Param_Crn.Substring(3, 2) + "-" + Param_Crn.Substring(5);
                        content += System.Environment.NewLine + "서식 명 : " + Param_Tmplt_Name;
                        content += System.Environment.NewLine + "서식 타입 : " + Param_Tmplt_Type;
                        
                        StringBuilder sb = new StringBuilder();
                        sb.Append(sUploadHandler);
                        sb.AppendFormat("CUST_CD={0}", "011906");//ELVIS-FRIEND 전용 화주 거래처코드로 고정
                        sb.AppendFormat("&CONTENT={0}", content);
                        sb.AppendFormat("&USR_NM={0}", Param_Usr_Nm);
                        sb.AppendFormat("&TEL_NO={0}", Param_Hp_No);
                        sb.AppendFormat("&OFFICE_CD={0}", "");

                        string call_upload_cmd = sb.ToString();
                        wc.DownloadString(call_upload_cmd);
                    }
                    catch (Exception ex)
                    {
                        strProcLog = "[TemplateUpload(CRM)-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }

                    result = Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {                
                strProcLog = "[TemplateUpload-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
                result = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }          

            return result;
        }

        /// <summary>
        /// Get Template List
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호</param>
        /// <param name="APV_YN">승인여부</param>
        /// <param name="FM_YMD">조회조건 시작일자</param>
        /// <param name="TO_YMD">조회조건 종료일자</param>
        /// <param name="TMPLY_TYPE">서식 종류</param>
        /// <param name="TMPLT_NM">서식 명</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetTemplateList")]
        public string GetTemplateList(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Apv_Yn = "";
            string Param_Fm_Ymd = "";
            string Param_To_Ymd = "";
            string Param_Tmplt_Type = "";
            string param_Tmplt_Nm = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("CRN")) Param_Crn = dt.Rows[0]["CRN"].ToString();
                        if (dt.Rows[0].Table.Columns.Contains("APV_YN")) Param_Apv_Yn = dt.Rows[0]["APV_YN"].ToString();
                        if (dt.Rows[0].Table.Columns.Contains("FM_YMD")) Param_Fm_Ymd = dt.Rows[0]["FM_YMD"].ToString();
                        if (dt.Rows[0].Table.Columns.Contains("TO_YMD")) Param_To_Ymd = dt.Rows[0]["TO_YMD"].ToString();
                        if (dt.Rows[0].Table.Columns.Contains("TMPLT_TYPE")) Param_Tmplt_Type = dt.Rows[0]["TMPLT_TYPE"].ToString();
                        if (dt.Rows[0].Table.Columns.Contains("TMPLT_NM")) param_Tmplt_Nm = dt.Rows[0]["TMPLT_NM"].ToString();
                        #endregion

                        dt = Sql_Template.GetTemplateList(Param_Crn, Param_Apv_Yn, Param_Fm_Ymd, Param_To_Ymd, Param_Tmplt_Type, param_Tmplt_Nm);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else if (dt.Rows.Count == 0) ReturnJsonVal = _common.MakeJson("N", "Template is not exist");
                        else _common.ThrowMsg(ErrorOccur, "Error Occured");

                        //Log 정보 저장
                        strProcLog = "[GetTemplateList ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetTemplateList-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Template Path
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="TMPLT_ID">서식 관리번호*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetTemplatePath")]
        public string GetTemplatePath(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Tmplt_Id = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("TMPLT_ID"))
                        {
                            Param_Tmplt_Id = dt.Rows[0]["TMPLT_ID"].ToString();
                            if (string.IsNullOrEmpty(Param_Tmplt_Id)) _common.ThrowMsg(ErrorOccur, "TMPLT_ID is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "TMPLT_ID Parameter is Null");
                        }
                        #endregion

                        dt = Sql_Template.GetTemplatePath(Param_Tmplt_Id);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else if (dt.Rows.Count == 0) ReturnJsonVal = _common.MakeJson("N", "File not exist");
                        else _common.ThrowMsg(ErrorOccur, "Error Occured");

                        //Log 정보 저장
                        strProcLog = "[GetTemplatePath ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetTemplatePath-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Set Template Approval Status Update
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="TMPLT_ID">서식ID*</param>
        /// <param name="APV_YN">서식ID*</param>
        /// <param name="NC_OCR_ID">네이버 OCR 템플릿 ID</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SetTmpltApvStatus")]
        public string SetTmpltApvStatus(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Tmplt_Id = "";
            string Param_Apv_Yn = "";
            string Param_Nc_Ocr_Id = "";

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
                        if (dt.Rows[0].Table.Columns.Contains("TMPLT_ID"))
                        {
                            Param_Tmplt_Id = dt.Rows[0]["TMPLT_ID"].ToString();
                            if (string.IsNullOrEmpty(Param_Tmplt_Id)) _common.ThrowMsg(ErrorOccur, "TMPLT_ID is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "TMPLT_ID Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("APV_YN"))
                        {
                            Param_Apv_Yn = dt.Rows[0]["APV_YN"].ToString();
                            if (string.IsNullOrEmpty(Param_Apv_Yn)) _common.ThrowMsg(ErrorOccur, "APV_YN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "APV_YN Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("NC_OCR_ID"))
                        {
                            if (Param_Apv_Yn == "Y")
                            {
                                Param_Nc_Ocr_Id = dt.Rows[0]["NC_OCR_ID"].ToString();
                                if (string.IsNullOrEmpty(Param_Nc_Ocr_Id)) _common.ThrowMsg(ErrorOccur, "NC_OCR_ID is Empty");
                            }
                        }
                        #endregion

                        rtnStatus = Sql_Template.SetTmpltApvStatus(Param_Tmplt_Id, Param_Apv_Yn, Param_Nc_Ocr_Id);

                        if (rtnStatus)
                        {
                            ReturnJsonVal = _common.MakeJson("Y", "Success");
                        }
                        else
                        {
                            ReturnJsonVal = _common.MakeJson("N", "Failed");
                        }

                        //Log 정보 저장
                        strProcLog = "[SetTmpltApvStatus ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[SetTmpltApvStatus-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }
    }

}