using System;
using System.IO;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using Newtonsoft.Json;
using META_DATA_API.Models;
using META_DATA_API.Models.Query;

namespace FileUpload.Controllers
{
    public class DocFileController : ApiController
    {
        private bool ErrorOccur = false;
        public string ReturnJsonVal;
        private string strProcLog = "";
        private DataTable dt = new DataTable();


        /// <summary>
        /// Customer License File Upload
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("DocUpload")]
        public HttpResponseMessage DocUpload()
        {
            #region // 변수 선언 영역
            HttpResponseMessage result = null;
            HttpPostedFile file = null;
            var httpRequest = HttpContext.Current.Request;
            
            string fileName = "";
            string filePath = "";
            #endregion
            
            try
            {                
                if (httpRequest.Files.Count > 0)
                {
                    #region // Validation Check & Save Variable                    
                    #endregion

                    for(int i = 0;i<httpRequest.Files.AllKeys.Length;i++)
                    {
                        file = httpRequest.Files[i];
                        fileName = file.FileName;
                        filePath = HttpContext.Current.Server.MapPath(httpRequest.QueryString.Get("SavePath"));

                        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                        if (File.Exists(filePath + fileName)) File.Delete(filePath + fileName);

                        file.SaveAs(filePath + fileName);

                        strProcLog = "[DocUpload ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + httpRequest.Params.Get(0) + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                        result = Request.CreateResponse(HttpStatusCode.OK);
                    }                    
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                strProcLog = "[DocUpload-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return result;
        }


        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("UpdateFileStamp")]
        public string UpdateFileStamp(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Mngt_No = "";
            string Param_CustCd = "";
            string Param_UsrId = "";
            string Param_File_Size = "";
            string Param_File_Cnt = "";
            string Param_Svc_Type = "";
            string Param_Inv_Amt = "";
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
                        Param_Mngt_No = String.Format("USE{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), (99 * (new Random().Next(8) + 2))).ToString();

                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_CustCd = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_CustCd)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN is Null");
                        }

                        if (dt.Rows[0].Table.Columns.Contains("USR_ID"))
                        {
                            Param_UsrId = dt.Rows[0]["USR_ID"].ToString();
                            if (string.IsNullOrEmpty(Param_CustCd)) _common.ThrowMsg(ErrorOccur, "USR_ID is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "USR_ID is Null");
                        }
                        #endregion

                        DataTable rtnDt = Sql_DocFile.GetFileList(dt.Rows[0]);
                        if (rtnDt.Rows.Count == 0)
                        {
                            rtnStatus = Sql_DocFile.SetFileList(dt.Rows[0]);
                            if (!rtnStatus) sqlResult = false;
                        }
                        ReturnJsonVal = _common.MakeJson("Y", "Success");
                        //Log 정보 저장
                        strProcLog = "[GetExImDocPath ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetExImDocPath-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }





        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("fileTest")]
        public void fileTest()
        {
            try
            {
                File.Copy("http://175.45.195.63:8091/EDMS/FRND/AN/1148183347/YJITSOSK2022100006/YJITSOSK2022100006_2022102509536971", "http://atest.elvisprime.com/EDMS/test.pdf", true);
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
    }


}