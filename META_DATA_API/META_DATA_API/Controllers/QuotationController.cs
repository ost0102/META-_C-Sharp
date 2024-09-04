using System;
using System.IO;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Collections.Specialized;
using System.Collections.Generic;

using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using META_DATA_API.Models;
using META_DATA_API.Models.Query;

namespace FileUpload.Controllers
{
    public class QuotationController : ApiController
    {
        private bool ErrorOccur = false;
        public string ReturnJsonVal;
        private string strProcLog = "";
        private DataSet ds = new DataSet();   
        private DataTable dt = new DataTable();

        /// <summary>
        /// Set Quotation Request Data
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="MAIN">견적 기본정보*</param>
        /// <param name="DIM">견적 화물정보*</param>
        /// <param name="DOC">견적 문서정보*</param>
        /// <param name="OCR">문서 판독정보*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SetQuotationRequest")]
        public HttpResponseMessage SetQuotationRequest()
        {
            #region // 변수 선언 영역
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            var DocPath = "/EDMS/FRND/QUOT/";

            DataTable Param_Main_Tbl = null;
            DataTable Param_Dim_Tbl = null;
            DataTable Param_Doc_Tbl = null;
            DataTable Param_Ocr_Mst = null;
            DataTable Param_Ocr_Dtl = null;

            string Param_Crn = "";
            string Param_Usr_Id = "";
            string Param_Svc_Type = "";
            string Param_req_Svc = "";
            string Param_Usr_Type = "";

            string fileName = "";
            string filePath = "";
            string fileExt = "";
            int fileSize = 0;

            bool rtnStatus = false;
            bool sqlResult = true;
            #endregion
            try
            {
                if (httpRequest.Form.Count > 0)
                {
                    if (httpRequest.Form.GetValues("REQVAL") != null)
                    {
                        ds = JsonConvert.DeserializeObject<DataSet>(httpRequest.Form.GetValues("REQVAL")[0]);
                        string QuotNo = String.Format("QUOT{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), (99 * (new Random().Next(8) + 2))).ToString();

                        #region // Validation Check & Save Variable
                        if (ds.Tables.Contains("MAIN"))
                        {
                            Param_Main_Tbl = ds.Tables["MAIN"];
                            Param_req_Svc = Param_Main_Tbl.Rows[0]["REQ_SVC"].ToString();
                            Param_Svc_Type = Param_Main_Tbl.Rows[0]["SVC_TYPE"].ToString();
                            Param_Usr_Type = Param_Main_Tbl.Rows[0]["USR_TYPE"].ToString();
                            if (ds.Tables["MAIN"].Rows.Count == 0) _common.ThrowMsg(ErrorOccur, "MAIN Info is Empty");
                            else
                            {
                                if (ds.Tables["MAIN"].Columns.Contains("CRN"))
                                {
                                    Param_Crn = ds.Tables["MAIN"].Rows[0]["CRN"].ToString();
                                    if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "MAIN CRN is Empty");
                                } 
                                else _common.ThrowMsg(ErrorOccur, "MAIN CRN is Null");
                                if (ds.Tables["MAIN"].Columns.Contains("REQ_SVC"))
                                {
                                    if (string.IsNullOrEmpty(ds.Tables["MAIN"].Rows[0]["REQ_SVC"].ToString())) _common.ThrowMsg(ErrorOccur, "MAIN REQ_SVC is Empty");
                                }
                                else _common.ThrowMsg(ErrorOccur, "MAIN REQ_SVC is Null");
                                if (ds.Tables["MAIN"].Columns.Contains("USR_ID"))
                                {
                                    Param_Usr_Id = ds.Tables["MAIN"].Rows[0]["USR_ID"].ToString();
                                    if (string.IsNullOrEmpty(Param_Usr_Id)) _common.ThrowMsg(ErrorOccur, "MAIN USR_ID is Empty");
                                }
                                else _common.ThrowMsg(ErrorOccur, "MAIN USR_ID is Null");
                                if (ds.Tables["MAIN"].Columns.Contains("POL_CD"))
                                {
                                    if (string.IsNullOrEmpty(ds.Tables["MAIN"].Rows[0]["POL_CD"].ToString())) _common.ThrowMsg(ErrorOccur, "MAIN POL_CD is Empty");
                                }
                                else _common.ThrowMsg(ErrorOccur, "MAIN POL_CD is Null");
                                if (ds.Tables["MAIN"].Columns.Contains("POD_CD"))
                                {
                                    if (string.IsNullOrEmpty(ds.Tables["MAIN"].Rows[0]["POD_CD"].ToString())) _common.ThrowMsg(ErrorOccur, "MAIN POD_CD is Empty");
                                }
                                else _common.ThrowMsg(ErrorOccur, "MAIN POD_CD is Null");
                                if (string.IsNullOrEmpty(ds.Tables["MAIN"].Rows[0]["REQ_FWD_CNT"].ToString())) _common.ThrowMsg(ErrorOccur, "REQ_FWD_CNT is Null");
                                else
                                {
                                    if (Convert.ToInt32(ds.Tables["MAIN"].Rows[0]["REQ_FWD_CNT"]) == 0) _common.ThrowMsg(ErrorOccur, "REQ_FWD is Empty");
                                }                                
                            }
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "MAIN Info Parameter is Null");
                        }
                        if (ds.Tables.Contains("DIM")) Param_Dim_Tbl = ds.Tables["DIM"];
                        if (ds.Tables.Contains("DOC")) Param_Doc_Tbl = ds.Tables["DOC"];
                        if (ds.Tables.Contains("OCR_MST")) Param_Ocr_Mst = ds.Tables["OCR_MST"];
                        if (ds.Tables.Contains("OCR_DTL")) Param_Ocr_Dtl = ds.Tables["OCR_DTL"];                        
                        #endregion

                        //견적 기본정보 저장
                        rtnStatus = Sql_Quotation.SetQuotationMain(QuotNo, Param_Crn, Param_Usr_Id, Param_Main_Tbl.Rows[0]);
                        if (!rtnStatus) sqlResult = false;
                        //견적 화물정보 저장
                        rtnStatus = Sql_Quotation.SetQuotationDim(QuotNo, Param_Usr_Id, Param_Dim_Tbl);
                        if (!rtnStatus) sqlResult = false;

                        rtnStatus = Sql_DocFile.SetFileList(QuotNo, Param_Crn, Param_Usr_Id, Param_Svc_Type, "QUOT", Param_req_Svc , Param_Usr_Type);

                        //견적 문서정보 저장
                        for (int i = 0; i < httpRequest.Files.Count; i++)
                        {
                            #region // FILE UPLOAD
                            var uploadFile = httpRequest.Files[i];
                            var fullPath = HttpContext.Current.Server.MapPath("~" + DocPath + QuotNo + "/" + uploadFile.FileName);
                            
                            fileName = uploadFile.FileName.Substring(0, uploadFile.FileName.IndexOf("."));
                            filePath = fullPath.Substring(0, fullPath.LastIndexOf("\\")).Replace("\\", "/");
                            fileExt = uploadFile.FileName.Substring(uploadFile.FileName.IndexOf("."));
                            fileSize = uploadFile.ContentLength;

                            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                            uploadFile.SaveAs(fullPath);
                            #endregion

                            string MngtNo = String.Format("DOC{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfff"), (9 * (new Random().Next(8) + 2))).ToString();

                            rtnStatus = Sql_Quotation.SetQuotationDoc(MngtNo, Param_Crn, Param_Usr_Id, fileName, fileSize, DocPath + QuotNo + "/", fileExt,
                            QuotNo, Param_Doc_Tbl.Rows[i]["DOC_TYPE"].ToString());



                            //OCR 판독 이력정보 저장
                            if (Param_Ocr_Mst != null)
                            {
                                if (i < Param_Ocr_Mst.Rows.Count)
                                {
                                    string OcrNo = String.Format("OCR{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), (99 * (new Random().Next(8) + 2))).ToString();

                                    rtnStatus = Sql_Quotation.SetQuotationOcrMst(OcrNo, MngtNo, QuotNo, Param_Usr_Id, Param_Ocr_Mst.Rows[i]);
                                    if (!rtnStatus) sqlResult = false;
                                    rtnStatus = Sql_Quotation.SetQuotationOcrDtl(OcrNo, Param_Usr_Id, Param_Ocr_Dtl.Select("ID = " + i + 1, "FIELD_SEQ ASC"));
                                    if (!rtnStatus) sqlResult = false;
                                }
                            }
                        }

                        if (sqlResult) 
                        {
                            strProcLog = "[SetQuotationRequest ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + httpRequest.Form.GetValues("REQVAL")[0] + System.Environment.NewLine;
                            _LogWriter.Writer(strProcLog);
                            result = Request.CreateResponse(HttpStatusCode.OK, QuotNo);
                        } 
                        else result = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    }
                    else
                    {
                        result = Request.CreateResponse(HttpStatusCode.NoContent);
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {                
                strProcLog = "[SetQuotationRequest-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
                result = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return result;
        }

        /// <summary>
        /// Send Quotation To ELVIS From ELVIS-FRIEND
        /// </summary>
        /// <param name = "reqVal" > JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name = "MAIN">기본 정보*</param>
        /// <param name = "FWD_LIST" > 포워더 리스트 *</ param >
        ///
        /// < returns > Result Data(Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SendQuotationToElvis")]
        public void SendQuotationToElvis(object reqVal)
        {
            #region // 변수 선언 영역
            DataTable Param_Main_Tbl = null;
            DataTable Param_Fwd_Tbl = null;
            string Param_Crn = "";
            string Param_Quot_No = "";
            string Param_Usr_Id = "";

            Dictionary<string, string> dicChat = null;

            bool rtnStatus = false;
            bool sqlResult = true;
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        ds = JsonConvert.DeserializeObject<DataSet>(rtnVal);

                        #region // Validation Check & Save Variable                        
                        if (ds.Tables.Contains("MAIN"))
                        {
                            Param_Main_Tbl = ds.Tables["MAIN"];
                            if (Param_Main_Tbl.Rows.Count == 0) _common.ThrowMsg(ErrorOccur, "MAIN Info is Empty");
                            else
                            {
                                if (string.IsNullOrEmpty(Param_Main_Tbl.Rows[0]["CRN"].ToString())) _common.ThrowMsg(ErrorOccur, "MAIN CRN Info is Empty");
                                else Param_Crn = Param_Main_Tbl.Rows[0]["CRN"].ToString();
                                if (string.IsNullOrEmpty(Param_Main_Tbl.Rows[0]["QUOT_NO"].ToString())) _common.ThrowMsg(ErrorOccur, "MAIN QUOT_NO Info is Empty");
                                else Param_Quot_No = Param_Main_Tbl.Rows[0]["QUOT_NO"].ToString();
                                if (string.IsNullOrEmpty(Param_Main_Tbl.Rows[0]["USR_ID"].ToString())) _common.ThrowMsg(ErrorOccur, "MAIN USR_ID Info is Empty");
                                else Param_Usr_Id = Param_Main_Tbl.Rows[0]["USR_ID"].ToString();
                            }
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "MAIN Info Parameter is Null");
                        }
                        if (ds.Tables.Contains("FWD_LIST"))
                        {
                            Param_Fwd_Tbl = ds.Tables["FWD_LIST"];
                            if (Param_Fwd_Tbl.Rows.Count == 0) _common.ThrowMsg(ErrorOccur, "FWD_LIST Info is Empty");
                            else
                            {
                                if (string.IsNullOrEmpty(Param_Fwd_Tbl.Rows[0]["CRN"].ToString())) _common.ThrowMsg(ErrorOccur, "FWD_LIST CRN Info is Empty");                                
                            }
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "FWD_LIST Info Parameter is Null");
                        }
                        #endregion                                              

                        for (int i = 0; i < Param_Fwd_Tbl.Rows.Count; i++)
                        {
                            DataTable fwdDt = Sql_Quotation.GetForwarderInfo(Param_Fwd_Tbl.Rows[i]["CRN"].ToString());

                            if (fwdDt.Rows.Count == 1)
                            {
                                DataRow svrInfo = fwdDt.Rows[0];
                                //string ConnStr = String.Format("User ID={0};Password={1};pooling=false;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={4})))",
                                //    svrInfo["DB_ID"].ToString(), svrInfo["DB_PWD"].ToString(), svrInfo["DB_IP"].ToString(), svrInfo["DB_PORT"].ToString(), svrInfo["DB_SID"].ToString());
                                //    "ELVIS_FRIEND", "FRIEND#!&%", "175.45.195.63", "1521", "friendDB");

                                //견적 기본정보 저장
                                rtnStatus = Sql_Quotation.SendQuotMainToElvis(Param_Quot_No, Param_Usr_Id, Param_Fwd_Tbl.Rows[i]["CRN"].ToString());
                                if (!rtnStatus) sqlResult = false;
                                //견적 화물정보 저장
                                //rtnStatus = Sql_Quotation.SendQuotDimToElvis(ConnStr, Param_Quot_No, Param_Usr_Id);
                                //if (!rtnStatus) sqlResult = false;
                                //OCR 판독 이력정보 저장
                                //string MngtNo = String.Format("OCR{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), (99 * (new Random().Next(8) + 2))).ToString();
                                //rtnStatus = Sql_Quotation.SendQuotationOcrMst(Param_Quot_No);
                                //if (!rtnStatus) sqlResult = false;
                                //견적 문서정보 저장
                                //DataTable docDt = Sql_Quotation.GetQuotationDoc(Param_Quot_No, Param_Crn, "CI,PL");

                                //for (int j = 0; j < docDt.Rows.Count; j++)
                                //{
                                //    DataRow docDr = docDt.Rows[j];

                                //    WebClient wc = new WebClient();
                                //    string sUploadHandler = "http://" + svrInfo["WEB_IP_EXT"] + ":" + svrInfo["WEB_PORT"] + "/wcf/UploadHandler.aspx";
                                //    //string sUploadHandler = "http://175.45.195.63:8091/wcf/UploadHandler.aspx";
                                //    string SavePath = docDr["FILE_PATH"].ToString() + docDr["FILE_NM"].ToString() + docDr["FILE_EXT"].ToString();
                                //    NameValueCollection myQueryStringCollection = new NameValueCollection();                                                                      
                                //    string strFilePath = HttpContext.Current.Server.MapPath("~" + docDr["FILE_PATH"] + docDr["FILE_NM"] + docDr["FILE_EXT"]);

                                //    myQueryStringCollection.Add("SavePath", SavePath);
                                //    wc.QueryString = myQueryStringCollection;
                                //    wc.UploadFile(sUploadHandler, "POST", strFilePath);

                                //    rtnStatus = Sql_Quotation.SendQuotDocToElvis(ConnStr, docDr);
                                //    if (!rtnStatus) sqlResult = false;
                                //}

                                //if (sqlResult)
                                //{
                                //    strProcLog = "[SendQuotationToElvis ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Success" + System.Environment.NewLine;
                                //    _LogWriter.Writer(strProcLog);

                                    // ELVIS PUSH MESSAGE 전송
                                    //try
                                    //{
                                    //    dicChat = new Dictionary<string, string>();
                                    //    dicChat.Add("NAME", Param_Usr_Id);
                                    //    dicChat.Add("DOMAIN", fwdDt.Rows[0]["DOMAIN"].ToString());

                                    //    if (_common.connectToCallChatHub(dicChat))
                                    //    {
                                    //        //도메인|보내는사람|받는사람|요청서비스|구분|형태|메세지|메시지아이디|
                                    //        string msg = "요청 견적을 확인해주세요.";
                                    //        _common.SendMessageHub(fwdDt.Rows[0]["DOMAIN"].ToString(), Param_Usr_Id, "", "WE", "P", "QUOT", msg, "", Param_Quot_No);
                                    //        _common.DiscConnectChatHub(dicChat);
                                    //    }                                                                             
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    strProcLog = "[SendMessageHub ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Push Message Failed :   " + ex.Message + System.Environment.NewLine;
                                    //    _LogWriter.Writer(strProcLog);
                                    //}
                                //}
                                //else
                                //{
                                //    strProcLog = "[SendQuotationToElvis ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Failed" + System.Environment.NewLine;
                                //    _LogWriter.Writer(strProcLog);
                                //}
                            }
                        }
                        
                        //Log 정보 저장
                        strProcLog = "[SendQuotationToElvis ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        strProcLog = "[SendQuotationToElvis ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   Json Parameter is Null" + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                }
                else
                {
                    strProcLog = "[SendQuotationToElvis ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   Json Parameter is Null" + System.Environment.NewLine;
                    _LogWriter.Writer(strProcLog);
                }
            }
            catch (Exception ex)
            {
                strProcLog = "[SendQuotationToElvis ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }
        }

        /// <summary>
        /// Send Quotation To ELVIS-FRIEND From ELVIS
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="MAIN">견적번호*</param>
        /// <param name="FRT">운임 리스트*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SendQuotationToElvisFriend")]
        public void SendQuotationToElvisFriend(object reqVal)
        {
            #region // 변수 선언 영역
            DataTable Param_Quot_Tbl = null;
            DataTable Param_Frt_Tbl = null;
            DataTable Param_Cntr_Tbl = null;

            bool rtnStatus = false;
            bool sqlResult = true;
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        ds = JsonConvert.DeserializeObject<DataSet>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (ds.Tables.Contains("MAIN"))
                        {
                            Param_Quot_Tbl = ds.Tables["MAIN"];
                            if (ds.Tables["MAIN"].Rows.Count == 0) _common.ThrowMsg(ErrorOccur, "MAIN Info is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "MAIN Info Parameter is Null");
                        }
                        if (ds.Tables.Contains("FRT"))
                        {
                            Param_Frt_Tbl = ds.Tables["FRT"];
                            if (ds.Tables["FRT"].Rows.Count == 0) _common.ThrowMsg(ErrorOccur, "FRT Info is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "FRT Info Parameter is Null");
                        }
                        if (ds.Tables.Contains("CNTR")) Param_Cntr_Tbl = ds.Tables["CNTR"];
                        #endregion

                        //견적 정보 저장
                        rtnStatus = Sql_Quotation.SendQuotToElvisFriend(Param_Quot_Tbl.Rows[0]);
                        if (!rtnStatus) sqlResult = false;
                        //견적 운임 정보 저장
                        rtnStatus = Sql_Quotation.SendQuotFrtToElvisFriend(Param_Frt_Tbl);
                        if (!rtnStatus) sqlResult = false;
                        //견적 컨테이너 정보 저장
                        rtnStatus = Sql_Quotation.SendQuotCntrToElvisFriend(Param_Cntr_Tbl);
                        if (!rtnStatus) sqlResult = false;

                        if (sqlResult)
                        {
                            strProcLog = "[SendQuotationToElvisFriend ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Success" + System.Environment.NewLine;
                            _LogWriter.Writer(strProcLog);

                            try
                            {
                                // 견적 내역 메일 전송
                                string subject = "[ELVIS-FRIEND] 견적 안내드립니다.";
                                string body = "";
                                string etd = Param_Quot_Tbl.Rows[0]["ETD"].ToString();
                                string eta = Param_Quot_Tbl.Rows[0]["ETA"].ToString();
                                string filePath = "";
                                string fileExt = "";

                                dt = Sql_Quotation.GetQuotationDoc(Param_Quot_Tbl.Rows[0]["MNGT_NO"].ToString(), Param_Quot_Tbl.Rows[0]["CUST_CRN"].ToString(), "QUOT");
                                if (dt.Rows.Count > 0)
                                {
                                    filePath = dt.Rows[0]["FILE_PATH"].ToString() + dt.Rows[0]["SVR_FILE_NM"].ToString();
                                    fileExt = dt.Rows[0]["FILE_EXT"].ToString();
                                }

                                body += System.Environment.NewLine;
                                body += "POL : " + Param_Quot_Tbl.Rows[0]["POL_NM"].ToString() + System.Environment.NewLine;
                                body += "POD : " + Param_Quot_Tbl.Rows[0]["POD_NM"].ToString() + System.Environment.NewLine;
                                body += "ETD : " + etd.Substring(0, 4) + "-" + etd.Substring(4, 2) + "-" + etd.Substring(6, 2) + System.Environment.NewLine;
                                body += "ETA : " + eta.Substring(0, 4) + "-" + eta.Substring(4, 2) + "-" + eta.Substring(6, 2) + System.Environment.NewLine;
                                body += "업체 명 : " + Param_Quot_Tbl.Rows[0]["FWD_OFFICE_NM"].ToString() + System.Environment.NewLine;
                                body += "담당자 명 : " + Param_Quot_Tbl.Rows[0]["FWD_PIC_NM"].ToString() + System.Environment.NewLine;
                                body += "담당자 연락처 : " + Param_Quot_Tbl.Rows[0]["FWD_PIC_TEL"].ToString() + System.Environment.NewLine;
                                body += "담당자 E-mail : " + Param_Quot_Tbl.Rows[0]["FWD_PIC_EMAIL"].ToString();
                                body += System.Environment.NewLine;

                                if (!string.IsNullOrEmpty(Param_Quot_Tbl.Rows[0]["CUST_PIC_EMAIL"].ToString()))
                                {
                                    SendEmail(subject, Param_Quot_Tbl.Rows[0]["CUST_PIC_EMAIL"].ToString(), 
                                        MakeEmailForm(body, Param_Quot_Tbl.Rows[0]["CUST_PIC_EMAIL"].ToString(), Param_Quot_Tbl.Rows[0]["QUOT_NO"].ToString()), filePath, fileExt);
                                }

                                strProcLog = "[SendQuotationToElvisFriend(Send Mail) ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Success" + System.Environment.NewLine;
                                _LogWriter.Writer(strProcLog);
                            }
                            catch (Exception ex)
                            {
                                strProcLog = "[SendQuotationToElvisFriend(Send Mail) ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                                _LogWriter.Writer(strProcLog);
                            }
                        }
                        else
                        {
                            strProcLog = "[SendQuotationToElvisFriend(Send Mail) ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Failed" + System.Environment.NewLine;
                            _LogWriter.Writer(strProcLog);
                        }
                        //Log 정보 저장
                        strProcLog = "[SendQuotationToElvisFriend ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        strProcLog = "[SendQuotationToElvisFriend ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   Json Parameter is Null" + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                }
                else
                {
                    strProcLog = "[SendQuotationToElvisFriend ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   Json Parameter is Null" + System.Environment.NewLine;
                    _LogWriter.Writer(strProcLog);
                }
            }
            catch (Exception ex)
            {
                strProcLog = "[SendQuotationToElvisFriend ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }
        }


        /// <summary>
        /// Get Quotation List Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="YMD_TYPE">요청일/견적일*</param>
        /// <param name="FM_YMD">조회조건 시작일자*</param>
        /// <param name="TO_YMD">조회조건 종료일자*</param>
        /// <param name="POL_CD">조회조건 출발지</param>
        /// <param name="POD_CD">조회조건 도착지</param>
        /// <param name="QUOT_NO">조회조건 도착지</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetFwdQuotationList")]
        public string GetFwdQuotationList(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Ymd_Type = "";
            string Param_Fm_Ymd = "";
            string Param_To_Ymd = "";
            string Param_Pol_Cd = "";
            string Param_Pod_Cd = "";
            string Param_Quot_No = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("QUOT_NO"))
                        {
                            Param_Quot_No = dt.Rows[0]["QUOT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Quot_No)) _common.ThrowMsg(ErrorOccur, "QUOT_NO is Empty");
                        }
                        else
                        {
                            if (dt.Rows[0].Table.Columns.Contains("CRN"))
                            {
                                Param_Crn = dt.Rows[0]["CRN"].ToString();
                                if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                            }
                            else
                            {
                                _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                            }
                            if (dt.Rows[0].Table.Columns.Contains("YMD_TYPE"))
                            {
                                Param_Ymd_Type = dt.Rows[0]["YMD_TYPE"].ToString();
                                if (string.IsNullOrEmpty(Param_Ymd_Type)) _common.ThrowMsg(ErrorOccur, "YMD_TYPE is Empty");
                            }
                            else
                            {
                                _common.ThrowMsg(ErrorOccur, "YMD_TYPE Parameter is Null");
                            }
                            if (dt.Rows[0].Table.Columns.Contains("FM_YMD"))
                            {
                                Param_Fm_Ymd = dt.Rows[0]["FM_YMD"].ToString();
                                if (string.IsNullOrEmpty(Param_Fm_Ymd)) _common.ThrowMsg(ErrorOccur, "FM_YMD is Empty");
                            }
                            else
                            {
                                _common.ThrowMsg(ErrorOccur, "FM_YMD Parameter is Null");
                            }
                            if (dt.Rows[0].Table.Columns.Contains("TO_YMD"))
                            {
                                Param_To_Ymd = dt.Rows[0]["TO_YMD"].ToString();
                                if (string.IsNullOrEmpty(Param_To_Ymd)) _common.ThrowMsg(ErrorOccur, "TO_YMD is Empty");
                            }
                            else
                            {
                                _common.ThrowMsg(ErrorOccur, "TO_YMD Parameter is Null");
                            }
                            if (dt.Rows[0].Table.Columns.Contains("POL_CD")) Param_Pol_Cd = dt.Rows[0]["POL_CD"].ToString();
                            if (dt.Rows[0].Table.Columns.Contains("POD_CD")) Param_Pod_Cd = dt.Rows[0]["POD_CD"].ToString();
                        }
                        #endregion

                        dt = Sql_Quotation.GetFwdQuotationList(Param_Crn, Param_Ymd_Type, Param_Fm_Ymd, Param_To_Ymd, Param_Pol_Cd, Param_Pod_Cd, Param_Quot_No);

                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");

                        //Log 정보 저장
                        strProcLog = "[GetFwdQuotationList ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetFwdQuotationList-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Fowarder Quot Information
        /// </summary>
        /// <param name="reqVal"></param>
        /// /// <param name="MNGT_NO">견적 관리번호</param>
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST","GET")]
        [ActionName("GetForwarderQuotData")]
        public string GetForwarderQuotData(object reqVal)
        {
            #region 변수 선언
            string Param_Crn = "";
            string Param_Mngt_No = "";

            #endregion

            try
            {
                if(reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("MNGT_NO"))
                        {
                            Param_Mngt_No = dt.Rows[0]["MNGT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Mngt_No)) _common.ThrowMsg(ErrorOccur, "MNGT_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "MNGT_NO Parameter is Null");
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

                        #endregion
                        ds = new DataSet();

                        dt = Sql_Quotation.GetFwdQuotHeader(Param_Mngt_No, Param_Crn);
                        
                        ds.Tables.Add(dt);


                        dt = Sql_Quotation.GetFwdQuotDtl(Param_Mngt_No, Param_Crn);
                        
                        ds.Tables.Add(dt);

                        dt = Sql_Quotation.GetFwdQuotFrt(Param_Mngt_No, Param_Crn);
                        
                        ds.Tables.Add(dt);


                        //dt = Sql_Quotation.GetFwdQuotCntr(Param_Mngt_No, Param_Crn);

                        //ds.Tables.Add(dt);

                        ReturnJsonVal = _common.MakeJson("Y", "Success", ds);

                        //Log 정보 저장
                        strProcLog = "[GetForwarderQuotData ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetForwarderQuotData-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }


            return ReturnJsonVal;
        }




        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SetFwdMakeQuotation")]
        public HttpResponseMessage SetFwdMakeQuotation()
        {
            #region // 변수 선언 영역
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            var DocPath = "/EDMS/FRND/QUOT/";

            DataTable Param_Main_Tbl = null;
            DataTable Param_Frt_Tbl = null;
            DataTable Param_Cnt_Tbl = null;
            //DataTable Param_Ocr_Mst = null;
            //DataTable Param_Ocr_Dtl = null;

            string Param_Crn = "";
            string Param_Usr_Id = "";
            string Param_Svc_Type = "";
            string Param_req_Svc = "";
            string Param_Usr_Type = "";

            bool rtnStatus = false;
            bool sqlResult = true;
            #endregion
            try
            {
                if (httpRequest.Form.Count > 0)
                {
                    if (httpRequest.Form.GetValues("REQVAL") != null)
                    {
                        ds = JsonConvert.DeserializeObject<DataSet>(httpRequest.Form.GetValues("REQVAL")[0]);
                        //string QuotNo = String.Format("QUOT{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), (99 * (new Random().Next(8) + 2))).ToString();
                        
                        string MngtNo = String.Format("QUOTGOV{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                        string QuotNo = "";
                        #region // Validation Check & Save Variable
                        if (ds.Tables.Contains("MAIN"))
                        {
                            Param_Main_Tbl = ds.Tables["MAIN"];
                            QuotNo = Param_Main_Tbl.Rows[0]["QUOT_NO"].ToString();
                            Param_req_Svc = Param_Main_Tbl.Rows[0]["REQ_SVC"].ToString();
                            Param_Svc_Type = Param_Main_Tbl.Rows[0]["SVC_TYPE"].ToString();
                            Param_Usr_Type = Param_Main_Tbl.Rows[0]["USR_TYPE"].ToString();
                            if (ds.Tables["MAIN"].Rows.Count == 0) _common.ThrowMsg(ErrorOccur, "MAIN Info is Empty");
                            else
                            {
                                if (ds.Tables["MAIN"].Columns.Contains("CRN"))
                                {
                                    Param_Crn = ds.Tables["MAIN"].Rows[0]["CRN"].ToString();
                                    if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "MAIN CRN is Empty");
                                }
                                else _common.ThrowMsg(ErrorOccur, "MAIN CRN is Null");
                                if (ds.Tables["MAIN"].Columns.Contains("REQ_SVC"))
                                {
                                    if (string.IsNullOrEmpty(ds.Tables["MAIN"].Rows[0]["REQ_SVC"].ToString())) _common.ThrowMsg(ErrorOccur, "MAIN REQ_SVC is Empty");
                                }
                                else _common.ThrowMsg(ErrorOccur, "MAIN REQ_SVC is Null");
                                if (ds.Tables["MAIN"].Columns.Contains("USR_ID"))
                                {
                                    Param_Usr_Id = ds.Tables["MAIN"].Rows[0]["USR_ID"].ToString();
                                    if (string.IsNullOrEmpty(Param_Usr_Id)) _common.ThrowMsg(ErrorOccur, "MAIN USR_ID is Empty");
                                }
                                else _common.ThrowMsg(ErrorOccur, "MAIN USR_ID is Null");
                                if (ds.Tables["MAIN"].Columns.Contains("POL_CD"))
                                {
                                    if (string.IsNullOrEmpty(ds.Tables["MAIN"].Rows[0]["POL_CD"].ToString())) _common.ThrowMsg(ErrorOccur, "MAIN POL_CD is Empty");
                                }
                                else _common.ThrowMsg(ErrorOccur, "MAIN POL_CD is Null");
                                if (ds.Tables["MAIN"].Columns.Contains("POD_CD"))
                                {
                                    if (string.IsNullOrEmpty(ds.Tables["MAIN"].Rows[0]["POD_CD"].ToString())) _common.ThrowMsg(ErrorOccur, "MAIN POD_CD is Empty");
                                }
                                else _common.ThrowMsg(ErrorOccur, "MAIN POD_CD is Null");

                            }
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "MAIN Info Parameter is Null");
                        }
                        if (ds.Tables.Contains("FRT")) Param_Frt_Tbl = ds.Tables["FRT"];
                        if (ds.Tables.Contains("CNTR")) Param_Cnt_Tbl = ds.Tables["CNTR"];


                        #endregion

                        //견적 기본정보 저장
                        //rtnStatus = Sql_Quotation.SetFwdQuotationMain(QuotNo, Param_Crn, Param_Usr_Id, Param_Main_Tbl.Rows[0]);
                        rtnStatus = Sql_Quotation.SetWwdQuotationMain(QuotNo, MngtNo, Param_Crn, Param_Usr_Id, Param_Main_Tbl.Rows[0]);
                        if (!rtnStatus) sqlResult = false;
                        
                        //견적 운임 저장
                        rtnStatus = Sql_Quotation.SetFwdQuotationFrt(QuotNo, MngtNo, Param_Crn, Param_Usr_Id, Param_Frt_Tbl);
                        
                        if (!rtnStatus) sqlResult = false;
                        //견적 문서정보 저장


                        rtnStatus = Sql_DocFile.SetFileList(QuotNo , Param_Crn , Param_Usr_Id , Param_Svc_Type, "QUOT" , Param_req_Svc , Param_Usr_Type);

                        //견적 컨테이너 저장
                        //rtnStatus = Sql_Quotation.SetQuotationCntr(QuotNo, MngtNo, Param_Crn, Param_Usr_Id, Param_Cnt_Tbl);
                        if (!rtnStatus) sqlResult = false;

                        if (sqlResult)
                        {
                            strProcLog = "[SetFwdMakeQuotation ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + httpRequest.Form.GetValues("REQVAL")[0] + System.Environment.NewLine;
                            _LogWriter.Writer(strProcLog);
                            result = Request.CreateResponse(HttpStatusCode.OK, QuotNo);
                        }
                        else result = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    }
                    else
                    {
                        result = Request.CreateResponse(HttpStatusCode.NoContent);
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                strProcLog = "[SetFwdMakeQuotation-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
                result = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return result;
        }



        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetQuotationFList")]
        public string GetQuotationFList(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Ymd_Type = "";
            string Param_Fm_Ymd = "";
            string Param_To_Ymd = "";
            string Param_Pol_Cd = "";
            string Param_Pod_Cd = "";
            string Param_Quot_No = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("QUOT_NO"))
                        {
                            Param_Quot_No = dt.Rows[0]["QUOT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Quot_No)) _common.ThrowMsg(ErrorOccur, "QUOT_NO is Empty");
                        }
                        else
                        {
                            if (dt.Rows[0].Table.Columns.Contains("CRN"))
                            {
                                Param_Crn = dt.Rows[0]["CRN"].ToString();
                                if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                            }
                            else
                            {
                                _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                            }
                            if (dt.Rows[0].Table.Columns.Contains("FM_YMD"))
                            {
                                Param_Fm_Ymd = dt.Rows[0]["FM_YMD"].ToString();
                                if (string.IsNullOrEmpty(Param_Fm_Ymd)) _common.ThrowMsg(ErrorOccur, "FM_YMD is Empty");
                            }
                            else
                            {
                                _common.ThrowMsg(ErrorOccur, "FM_YMD Parameter is Null");
                            }
                            if (dt.Rows[0].Table.Columns.Contains("TO_YMD"))
                            {
                                Param_To_Ymd = dt.Rows[0]["TO_YMD"].ToString();
                                if (string.IsNullOrEmpty(Param_To_Ymd)) _common.ThrowMsg(ErrorOccur, "TO_YMD is Empty");
                            }
                            else
                            {
                                _common.ThrowMsg(ErrorOccur, "TO_YMD Parameter is Null");
                            }
                            if (dt.Rows[0].Table.Columns.Contains("POL_CD")) Param_Pol_Cd = dt.Rows[0]["POL_CD"].ToString();
                            if (dt.Rows[0].Table.Columns.Contains("POD_CD")) Param_Pod_Cd = dt.Rows[0]["POD_CD"].ToString();
                        }
                        #endregion

                        dt = Sql_Quotation.GetQuotationFList(Param_Crn, Param_Fm_Ymd, Param_To_Ymd, Param_Pol_Cd, Param_Pod_Cd);


                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");

                        //Log 정보 저장
                        strProcLog = "[GetQuotationFList ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetQuotationFList-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }





        /// <summary>
        /// Get Quotation List Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="YMD_TYPE">요청일/견적일*</param>
        /// <param name="FM_YMD">조회조건 시작일자*</param>
        /// <param name="TO_YMD">조회조건 종료일자*</param>
        /// <param name="POL_CD">조회조건 출발지</param>
        /// <param name="POD_CD">조회조건 도착지</param>
        /// <param name="QUOT_NO">조회조건 도착지</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetQuotationList")]
        public string GetQuotationList(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Ymd_Type = "";
            string Param_Fm_Ymd = "";
            string Param_To_Ymd = "";
            string Param_Pol_Cd = "";
            string Param_Pod_Cd = "";
            string Param_Quot_No = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("QUOT_NO"))
                        {
                            Param_Quot_No = dt.Rows[0]["QUOT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Quot_No)) _common.ThrowMsg(ErrorOccur, "QUOT_NO is Empty");
                        }
                        else
                        {
                            if (dt.Rows[0].Table.Columns.Contains("CRN"))
                            {
                                Param_Crn = dt.Rows[0]["CRN"].ToString();
                                if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                            }
                            else
                            {
                                _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                            }
                            if (dt.Rows[0].Table.Columns.Contains("YMD_TYPE"))
                            {
                                Param_Ymd_Type = dt.Rows[0]["YMD_TYPE"].ToString();
                                if (string.IsNullOrEmpty(Param_Ymd_Type)) _common.ThrowMsg(ErrorOccur, "YMD_TYPE is Empty");
                            }
                            else
                            {
                                _common.ThrowMsg(ErrorOccur, "YMD_TYPE Parameter is Null");
                            }
                            if (dt.Rows[0].Table.Columns.Contains("FM_YMD"))
                            {
                                Param_Fm_Ymd = dt.Rows[0]["FM_YMD"].ToString();
                                if (string.IsNullOrEmpty(Param_Fm_Ymd)) _common.ThrowMsg(ErrorOccur, "FM_YMD is Empty");
                            }
                            else
                            {
                                _common.ThrowMsg(ErrorOccur, "FM_YMD Parameter is Null");
                            }
                            if (dt.Rows[0].Table.Columns.Contains("TO_YMD"))
                            {
                                Param_To_Ymd = dt.Rows[0]["TO_YMD"].ToString();
                                if (string.IsNullOrEmpty(Param_To_Ymd)) _common.ThrowMsg(ErrorOccur, "TO_YMD is Empty");
                            }
                            else
                            {
                                _common.ThrowMsg(ErrorOccur, "TO_YMD Parameter is Null");
                            }
                            if (dt.Rows[0].Table.Columns.Contains("POL_CD")) Param_Pol_Cd = dt.Rows[0]["POL_CD"].ToString();
                            if (dt.Rows[0].Table.Columns.Contains("POD_CD")) Param_Pod_Cd = dt.Rows[0]["POD_CD"].ToString();
                        }                        
                        #endregion

                        dt = Sql_Quotation.GetQuotationList(Param_Crn, Param_Ymd_Type, Param_Fm_Ymd, Param_To_Ymd, Param_Pol_Cd, Param_Pod_Cd, Param_Quot_No);


                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");

                        //Log 정보 저장
                        strProcLog = "[GetQuotationList ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetQuotationList-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Quotation Detail List Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="QUOT_NO">사업자등록번호*</param>
        /// <param name="REQ_SVC">해운/항공*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetQuotationDetailList")]
        public string GetQuotationDetailList(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Quot_No = "";
            string Param_Req_Svc = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("QUOT_NO"))
                        {
                            Param_Quot_No = dt.Rows[0]["QUOT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Quot_No)) _common.ThrowMsg(ErrorOccur, "QUOT_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "QUOT_NO Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("REQ_SVC"))
                        {
                            Param_Req_Svc = dt.Rows[0]["REQ_SVC"].ToString();
                            if (string.IsNullOrEmpty(Param_Req_Svc)) _common.ThrowMsg(ErrorOccur, "REQ_SVC is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "REQ_SVC Parameter is Null");
                        }
                        #endregion

                        dt = Sql_Quotation.GetQuotationDetailList(Param_Quot_No, Param_Req_Svc);

                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");

                        //Log 정보 저장
                        strProcLog = "[GetQuotationDetailList ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetQuotationDetailList-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Quotation Detail Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="QUOT_NO">견적번호*</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetQuotationDetail")]
        public string GetQuotationDetail(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Quot_No = "";
            string Param_Crn = "";
            string Param_Order_Type = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("QUOT_NO"))
                        {
                            Param_Quot_No = dt.Rows[0]["QUOT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Quot_No)) _common.ThrowMsg(ErrorOccur, "QUOT_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "QUOT_NO Parameter is Null");
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
                        if (dt.Rows[0].Table.Columns.Contains("ORDER_TYPE"))
                        {
                            Param_Order_Type = dt.Rows[0]["ORDER_TYPE"].ToString();
                            if (string.IsNullOrEmpty(Param_Order_Type)) _common.ThrowMsg(ErrorOccur, "ORDER_TYPE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "ORDER_TYPE Parameter is Null");
                        }
                        #endregion

                        ds = new DataSet();
                        // ELVIS 견적 내역
                        dt = Sql_Quotation.GetQuotationDetail(Param_Quot_No, Param_Crn, Param_Order_Type);
                        ds.Tables.Add(dt);
                        // 등록자료 내역
                        dt = Sql_Quotation.GetQuotationMain(Param_Quot_No, Param_Crn);
                        ds.Tables.Add(dt);
                        // 등록문서 내역
                        dt = Sql_Quotation.GetQuotationDoc(Param_Quot_No, Param_Crn, "CI,PL");
                        ds.Tables.Add(dt);

                        ReturnJsonVal = _common.MakeJson("Y", "Success", ds);

                        //Log 정보 저장
                        strProcLog = "[GetQuotationDetail ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetQuotationDetail-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Quotation Freight Data Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="QUOT_NO">견적번호*</param>
        /// <param name="MNGT_NO">ELVIS 견적번호*</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetQuotationFreight")]
        public string GetQuotationFreight(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Quot_No = "";
            string Param_Mngt_No = "";
            string Param_Crn = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("QUOT_NO"))
                        {
                            Param_Quot_No = dt.Rows[0]["QUOT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Quot_No)) _common.ThrowMsg(ErrorOccur, "QUOT_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "QUOT_NO Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("MNGT_NO"))
                        {
                            Param_Mngt_No = dt.Rows[0]["MNGT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Mngt_No)) _common.ThrowMsg(ErrorOccur, "MNGT_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "MNGT_NO Parameter is Null");
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
                        #endregion

                        dt = Sql_Quotation.GetQuotationFreight(Param_Quot_No, Param_Mngt_No, Param_Crn);

                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else if (dt.Rows.Count == 0) ReturnJsonVal = _common.MakeJson("N", "Data not found");
                        else _common.ThrowMsg(ErrorOccur, "");

                        //Log 정보 저장
                        strProcLog = "[GetQuotationFreight ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetQuotationFreight-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Quotation Document Data Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="QUOT_NO">견적번호*</param>
        /// <param name="MNGT_NO">ELVIS 견적번호*</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetQuotationDoc")]
        public string GetQuotationDoc(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Mngt_No = "";
            string Param_Crn = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_Crn = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                        }
                        #endregion

                        dt = Sql_Quotation.GetQuotationDoc(Param_Mngt_No, Param_Crn, "QUOT");

                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else if (dt.Rows.Count == 0) ReturnJsonVal = _common.MakeJson("N", "Data not found");
                        else _common.ThrowMsg(ErrorOccur, "");

                        //Log 정보 저장
                        strProcLog = "[GetQuotationFreight ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetQuotationFreight-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Fowarder List
        /// /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="OFFICE_NM">사업장명*</param>
        /// 
        /// <returns>DataTable</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetForwarderList")]
        public string GetForwarderList(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Office_Nm = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("OFFICE_NM")) Param_Office_Nm = dt.Rows[0]["OFFICE_NM"].ToString();
                        #endregion

                        dt = Sql_Quotation.GetForwarderList(Param_Office_Nm);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");
                    }

                    //Log 정보 저장
                    strProcLog = "[GetFowarderList ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    " + System.Environment.NewLine;
                    _LogWriter.Writer(strProcLog);
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetFowarderList-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Forwarder Volume Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="POL_CD">출발지</param>
        /// <param name="POD_CD">도착지</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetFwdVolInfo")]
        public string GetFwdVolInfo(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Pol_Cd = "";
            string Param_Pod_Cd = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("POL_CD"))
                        {
                            Param_Pol_Cd = dt.Rows[0]["POL_CD"].ToString();
                            if (string.IsNullOrEmpty(Param_Pol_Cd)) _common.ThrowMsg(ErrorOccur, "POL_CD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "POL_CD Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("POD_CD"))
                        {
                            Param_Pod_Cd = dt.Rows[0]["POD_CD"].ToString();
                            if (string.IsNullOrEmpty(Param_Pod_Cd)) _common.ThrowMsg(ErrorOccur, "POD_CD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "POD_CD Parameter is Null");
                        }
                        #endregion

                        dt = Sql_Quotation.GetFwdVolInfo(Param_Crn, Param_Pol_Cd, Param_Pod_Cd);

                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");
                        

                        //Log 정보 저장
                        strProcLog = "[GetFwdVolInfo ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetFwdVolInfo-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Reccomend Forwarder Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="POL_CD">출발지*</param>
        /// <param name="POD_CD">도착지*</param>
        /// <param name="ORDER_TYPE">정렬기준*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetReccomendFowarder")]
        public string GetReccomendFowarder(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Pol_Cd = "";
            string Param_Pod_Cd = "";
            string Param_Order_Type = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("POL_CD"))
                        {
                            Param_Pol_Cd = dt.Rows[0]["POL_CD"].ToString();
                            if (string.IsNullOrEmpty(Param_Pol_Cd)) _common.ThrowMsg(ErrorOccur, "POL_CD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "POL_CD Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("POD_CD"))
                        {
                            Param_Pod_Cd = dt.Rows[0]["POD_CD"].ToString();
                            if (string.IsNullOrEmpty(Param_Pod_Cd)) _common.ThrowMsg(ErrorOccur, "POD_CD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "POD_CD Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("ORDER_TYPE"))
                        {
                            Param_Order_Type = dt.Rows[0]["ORDER_TYPE"].ToString();
                            if (string.IsNullOrEmpty(Param_Order_Type)) _common.ThrowMsg(ErrorOccur, "ORDER_TYPE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "ORDER_TYPE Parameter is Null");
                        }
                        #endregion

                        dt = Sql_Quotation.GetReccomendFowarder(Param_Pol_Cd, Param_Pod_Cd, Param_Order_Type);

                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");
                        

                        //Log 정보 저장
                        strProcLog = "[GetReccomendFowarder ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetReccomendFowarder-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get OCR Data Information
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="OCR_NO">OCR 관리번호*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetOcrData")]
        public string GetOcrData(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Ocr_No = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("OCR_NO"))
                        {
                            Param_Ocr_No = dt.Rows[0]["OCR_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Ocr_No)) _common.ThrowMsg(ErrorOccur, "OCR_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "OCR_NO Parameter is Null");
                        }
                        #endregion

                        dt = Sql_Quotation.GetOcrData(Param_Ocr_No);

                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");
                        

                        //Log 정보 저장
                        strProcLog = "[GetOcrData ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetOcrData-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Document List Data
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="MNGT_NO">관리번호*</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetDocList")]
        public string GetDocList(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Mngt_No = "";
            string Param_Crn = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_Crn = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                        }
                        #endregion

                        dt = Sql_Quotation.GetDocList(Param_Mngt_No, Param_Crn);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not Found");

                        //Log 정보 저장
                        strProcLog = "[GetDocList ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetDocList-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }


        /// <summary>
        /// Get Document List Data
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetRecommendQuot")]
        public string GetRecommendQuot(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
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
                        #endregion

                        dt = Sql_Quotation.GetRecommendQuot(Param_Crn);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not Found");

                        //Log 정보 저장
                        strProcLog = "[GetRecommendQuot ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetRecommendQuot-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }


        /// <summary>
        /// 실행사 견적 현황 
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// 
        /// <returns>Result Data (DataTable)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetFQuotList")]
        public string GetFQuotList(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
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
                        #endregion

                        dt = Sql_Quotation.GetFQuotList(Param_Crn);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not Found");

                        //Log 정보 저장
                        strProcLog = "[GetFQuotList ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetFQuotList-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }


        /// <summary>
        /// Send Quotation To ELVIS From ELVIS-FRIEND
        /// </summary>
        /// <param name = "reqVal" > JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name = "CRN">사업자등록번호*</param>
        /// <param name = "QUOT_NO">견적 번호*</param>
        /// <param name = "QUOT_DTL_NO">견적 상세 번호*</param>
        /// <param name = "ELVIS_CRN">견적 번호*</param>
        ///
        /// < returns > Result Data(Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SetQuotStatus")]
        public void SetQuotStatus(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Quot_No = "";
            string Param_Quot_Dtl_No = "";
            string Param_Elvis_Crn = "";

            string ConnStr = "";
            bool rtnStatus = false;
            bool sqlResult = true;
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
                        if (dt.Rows[0].Table.Columns.Contains("QUOT_NO"))
                        {
                            Param_Quot_No = dt.Rows[0]["QUOT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Quot_No)) _common.ThrowMsg(ErrorOccur, "QUOT_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "QUOT_NO Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("QUOT_DTL_NO"))
                        {
                            Param_Quot_Dtl_No = dt.Rows[0]["QUOT_DTL_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Quot_Dtl_No)) _common.ThrowMsg(ErrorOccur, "QUOT_DTL_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "QUOT_DTL_NO Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("ELVIS_CRN"))
                        {
                            Param_Elvis_Crn = dt.Rows[0]["ELVIS_CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Elvis_Crn)) _common.ThrowMsg(ErrorOccur, "ELVIS_CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "ELVIS_CRN Parameter is Null");
                        }
                        #endregion

                        DataTable fwdDt = Sql_Quotation.GetForwarderInfo(Param_Elvis_Crn);

                        if (fwdDt.Rows.Count == 1)
                        {
                            DataRow svrInfo = fwdDt.Rows[0];
                            ConnStr = String.Format("User ID={0};Password={1};pooling=false;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={4})))",
                                svrInfo["DB_ID"].ToString(), svrInfo["DB_PWD"].ToString(), svrInfo["DB_IP"].ToString(), svrInfo["DB_PORT"].ToString(), svrInfo["DB_SID"].ToString());
                        }

                        rtnStatus = Sql_Quotation.SetQuotStatus(Param_Crn, Param_Quot_No, Param_Quot_Dtl_No, Param_Elvis_Crn, ConnStr);

                        if (rtnStatus) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Failed");


                        //Log 정보 저장
                        strProcLog = "[SetQuotStatus ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        strProcLog = "[SetQuotStatus ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   Json Parameter is Null" + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                }
                else
                {
                    strProcLog = "[SetQuotStatus ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   Json Parameter is Null" + System.Environment.NewLine;
                    _LogWriter.Writer(strProcLog);
                }
            }
            catch (Exception ex)
            {
                strProcLog = "[SetQuotStatus ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }
        }

        public void SendEmail(string subject, string strTo, string strBody, string filePath, string fileExt)
        {            
            string tmpPath = HttpContext.Current.Server.MapPath("~/EDMS/TEMP/");
            
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

                MailAddress from = new MailAddress("friend@yjit.co.kr");
                MailAddress to = new MailAddress(strTo);

                MailMessage message = new MailMessage(from, to);
                message.Body = strBody;
                message.IsBodyHtml = true;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = subject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;

                //견적서 파일 첨부
                if (!string.IsNullOrEmpty(filePath))
                {
                    string fileName = filePath.Substring(filePath.LastIndexOf("/") + 1) + fileExt;
                    string atchPath = tmpPath + fileName;
                    if (!Directory.Exists(tmpPath)) Directory.CreateDirectory(tmpPath);

                    File.Copy(HttpContext.Current.Server.MapPath("~" + filePath), atchPath);

                    if (!string.IsNullOrEmpty(filePath)) message.Attachments.Add(new System.Net.Mail.Attachment(atchPath));
                }                   

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

        public string MakeEmailForm(string body, string usrId, string quotNo)
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
            strHTML += "            <td valign=\"top\" align=\"center\" style=\"padding:40px 0 50px;background-color: #ffffff;color: #666666;font-size: 20px;letter-spacing:-1px;font-family:'Noto Sans KR', 'Noto Sans', 'Malgun Gothic', 'Apple SD Gothic Neo', sans-serif;font-weight: 300;\">";
            strHTML += "               <span style=\"font-weight: 600;color: #111111;\">[ELVIS-FRIEND] 견적 안내 드립니다. ";
            strHTML += "            </td>";
            strHTML += "         </tr>";

            strHTML += "         <tr>";
            strHTML += "            <td valign=\"top\" style=\"padding:0 13px;\">";
            strHTML += "               <div style=\"max-width: 610px;margin: 0 auto\">";
            strHTML += "                  <table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" style=\"border-top: 2px solid #222222;width: 100%;\">";
            strHTML += "                <tr>";
            strHTML += "                        <td valign=\"middle\" align=\"center\" width=\"28%\" style=\"width:28%;padding:0;height:43px;font-size: 14px;color: #333333;background-color: #f8f8f8;border-bottom: 1px solid #eaeaea;font-family:'Noto Sans KR', 'Noto Sans', 'Malgun Gothic', 'Apple SD Gothic Neo', sans-serif;font-weight: 600;\">";
            strHTML += "                              <strong>견적 내역</strong>";
            strHTML += "                        </td>  ";
            strHTML += "                        <td valign=\"middle\" align=\"left\" style=\"padding:0 0 0 20px;height:43px;font-size: 14px;color: #888888;border-bottom: 1px solid #eaeaea;font-family:'Noto Sans KR', 'Noto Sans', 'Malgun Gothic', 'Apple SD Gothic Neo', sans-serif;font-weight: 300;\">";
            strHTML += "                           <Pre>" + body + "</Pre> ";
            strHTML += "                        </td> ";
            strHTML += "               </tr>";
            strHTML += "                   </table> ";
            strHTML += "                 </div> ";
            strHTML += "             </td>";
            strHTML += "         </tr>";

            strHTML += "         <tr>";
            strHTML += "            <td valign=\"middle\"align=\"center\" style=\"padding: 40px 0 88px\"";
            strHTML += "               ";
            strHTML += "               <div style=\"display:inline-block;width:200px;max-width:100%;vertical-align:top\">";
            strHTML += "                       <table style=\"table-layout:fixed;width:200px;\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">";
            strHTML += "                           <tbody>";
            strHTML += "                              <tr>";
            strHTML += "                                 <td>";
            strHTML += "                                       <table style=\"table-layout:fixed;width:100%;background:#f49f17\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">";
            strHTML += "                                           <tbody>";
            strHTML += "                                              <tr>";
            strHTML += "                                                 <td height=\"56\" style=\"text-align:center;\">";
            strHTML += "                                                       <a href='http://friend.yjit.co.kr/returnApi/CallMailPage?id=" + usrId + "&mngt_no=" + quotNo + "' style=\"text-decoration:none !important; font-size:18px;color:#fff;text-decoration:none;font-family:'Noto Sans KR', 'Noto Sans', 'Malgun Gothic', 'Apple SD Gothic Neo', sans-serif;font-weight: 600;text-decoration: none;\" rel=\"noreferrer noopener\" target=\"_blank\">견적 보기</a>";
            strHTML += "                                                    </td>";
            strHTML += "                                                </tr>";
            strHTML += "                                            </tbody>";
            strHTML += "                                        </table>";
            strHTML += "                                    </td>";
            strHTML += "                                </tr>";
            strHTML += "                            </tbody>";
            strHTML += "                        </table>";
            strHTML += "                   </div>";
            strHTML += "               ";
            strHTML += "            </td>";
            strHTML += "         </tr>";

            strHTML += "      </table>";
            strHTML += "   </div>";
            strHTML += "</body>";
            strHTML += "</html>";
            return strHTML;
        }

        private bool SendMongoDB(DataRow dr)
        {
            try
            {
                string[] DTArr = { "BL", "INV", "MFCS", "FWB", "AMS", "AFR", "DCD", "DO", "TAX" };
                MongoClient mgClient = new MongoClient("mongodb://friend:yjit1234!@bga9v.pub-vpc.mg.naverncp.com:17017/friendDB?authSource=friendDB&readPreference=primary&directConnection=true&tls=false");
            
                var mgDatabase = mgClient.GetDatabase("friendDB");
                var mgCollection = mgDatabase.GetCollection<BsonDocument>("FriendQuotMst");
                BsonDocument dic = new BsonDocument();

                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    dic.Add(dr.Table.Columns[i].ColumnName, dr[i].ToString());
                }

                // 해당 자료 존재하면 DELETE, INSERT
                var filter = Builders<BsonDocument>.Filter.And(
                                Builders<BsonDocument>.Filter.Eq("HBL_NO", dr["HBL_NO"].ToString()),
                                Builders<BsonDocument>.Filter.Eq("CRN", dr["CRN"].ToString()));
                var docCnt = mgCollection.Find(filter).CountDocuments();

                if (docCnt > 0)
                {
                    var hblList = mgCollection.Find(filter).First();
                    var jsonStr = hblList.ToString().Replace("ObjectId(", "").Replace("\")", "\"");
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>("[" + jsonStr + "]");

                    for (int i = 0; i < DTArr.Length; i++)
                    {
                        string colNm = DTArr[i] + "_SEND_DT";
                        if (!string.IsNullOrEmpty(dt.Rows[0][colNm].ToString()) && string.IsNullOrEmpty(dr[colNm].ToString()))
                        {
                            dic[colNm] = dt.Rows[i][colNm].ToString();
                        }
                    }

                    filter = Builders<BsonDocument>.Filter.And(
                        Builders<BsonDocument>.Filter.Eq("HBL_NO", dr["HBL_NO"].ToString()),
                        Builders<BsonDocument>.Filter.Eq("CRN", dr["CRN"].ToString()));
                    var rtnrsult = mgCollection.DeleteOne(filter);
                }

                mgCollection.InsertOne(dic);
            }
            catch (Exception ex)
            {
                strProcLog = "[SendMongoDB-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return true;
        }

    }
}