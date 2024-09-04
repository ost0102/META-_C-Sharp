using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Specialized;

using Newtonsoft.Json;
using META_DATA_API.Models;
using META_DATA_API.Models.Query;

namespace META_DATA_API.Controllers
{
    public class TestController : ApiController
    {
        // GET: Test
        [AcceptVerbs("POST", "GET")]
        [ActionName("LicenseDownload_test")]
        public HttpResponseMessage LicenseDownload_test(string rtnVal)
        {
            #region // 변수 선언 영역
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            #endregion

            try
            {
                var filepath = System.Web.HttpContext.Current.Server.MapPath("~/EDMS/DOC/CRN/20221007171552522397/yjCRN.pdf");
                var stream = new FileStream(filepath, FileMode.Open);
                result.Content = new StreamContent(stream);
                //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(filepath);
                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //result.Content.Headers.ContentLength = stream.Length;
                                
                result = Request.CreateResponse(HttpStatusCode.OK, new StreamContent(stream));


            }
            catch (Exception ex)
            {                
                result = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return result;
        }

        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("UploadTest")]
        public void UploadTest()
        {
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                string sUploadHandler = "http://110.45.209.36:9634/wcf/UploadHandler.aspx";
                string SavePath = "/EDMS/FRND/QCTT.ELVIS.COM/QUOT/" + DateTime.Now.ToString("yyyyMMdd") + "/QUOT221015165645792/";
                NameValueCollection myQueryStringCollection = new NameValueCollection();
                string strFilePath = HttpContext.Current.Server.MapPath("~/EDMS/FRND/QUOT/QUOT221015165645792/CI_.pdf");

                myQueryStringCollection.Add("SavePath", SavePath);
                wc.QueryString = myQueryStringCollection;
                wc.UploadFile(sUploadHandler, "POST", strFilePath);
            }
            catch (Exception ex)
            {
                string strProcLog = "[UploadTest ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }
            
        }
    }
}