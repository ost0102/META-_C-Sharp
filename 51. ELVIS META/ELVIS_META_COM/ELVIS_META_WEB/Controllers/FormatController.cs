using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using System.Configuration;
using System.Web;
using System.Text.RegularExpressions;

namespace ELVIS_META_WEB.Controllers
{
    public class FormatController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Regist()
        {
			if (TempData.ContainsKey("TMP_DTL"))
			{
				string TMP_DTL = TempData["TMP_DTL"].ToString();
				if (TMP_DTL != "")
				{
					ViewBag.TMP_DTL = TMP_DTL;
				}
			}

			return View();
        }


		public class JsonData
		{
			public string vJsonData { get; set; }
		}

		[HttpPost]
		public ActionResult fnGetTemplateList(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Template/GetTemplateList", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnGetTemplatePath(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Template/GetTemplatePath", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}


		string strJson = "";
		
		public string fnGetAPIData(string strType, string strURL, string auth_type, string strToken, string strParam)
		{
			try
			{
				string URL = strURL;

				if (strType == "GET")
				{
					URL = String.Format("{0}?{1}", strURL, strParam);
				}

				HttpWebRequest request = WebRequest.Create(URL) as HttpWebRequest;
				if (!string.IsNullOrEmpty(strToken))
				{
					request.Headers.Add("Authorization-Type", auth_type);
					request.Headers.Add("Authorization-Token", strToken);
				}
				request.Method = strType.ToUpper();
				request.ContentType = "application/json;charset=UTF-8";

				if (strType == "POST")
				{
					Byte[] byteDataParams = Encoding.UTF8.GetBytes(strParam);
					request.ContentLength = byteDataParams.Length;

					Stream st = request.GetRequestStream();
					st.Write(byteDataParams, 0, byteDataParams.Length);
					st.Close();
				}

				HttpWebResponse response;
				response = request.GetResponse() as HttpWebResponse;

				Stream stream = response.GetResponseStream();
				StreamReader reader = new StreamReader(stream, Encoding.UTF8);
				string strResult = reader.ReadToEnd();
				//strResult = strResult.ToString().Replace("\\r", "/&r").Replace("\\n", "/&n").Replace("\\", String.Empty).Replace("/&r", "\\r").Replace("/&n", "\\n").Remove(0, 1).Substring(0,strResult.Length-1);
				strResult = strResult.ToString().Replace("\\r", "/&r").Replace("\\n", "/&n").Replace("\\", String.Empty).Replace("/&r", "\\r").Replace("/&n", "\\n");
				strResult = strResult.Remove(0, 1);
				strResult = strResult.Substring(0, strResult.Length - 1);
				//DataSet ds = JsonConvert.DeserializeObject<DataSet>(strResult);
				stream.Close();
				response.Close();
				reader.Close();

				return strResult;
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return strJson;
			}
		}
	}
}