using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using ELVIS_META_COMMON.Controller;
using ELVIS_META_COMMON.YJIT_Utils;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;

namespace ELVIS_META_WEB.Controllers
{
    public class EstimateController : Controller
    {
        // GET: User
        public new ActionResult Request()
        {
			if (TempData.ContainsKey("POL_CD"))
			{
				string ref1 = TempData["POL_CD"].ToString();
				if (ref1 != "")
				{
					ViewBag.POL_CD = ref1;
				}
			}

			if (TempData.ContainsKey("POD_CD"))
			{
				string ref1 = TempData["POD_CD"].ToString();
				if (ref1 != "")
				{
					ViewBag.POD_CD = ref1;
				}
			}

			if (TempData.ContainsKey("POL_NM"))
			{
				string ref1 = TempData["POL_NM"].ToString();
				if (ref1 != "")
				{
					ViewBag.POL_NM = ref1;
				}
			}

			if (TempData.ContainsKey("POD_NM"))
			{
				string ref1 = TempData["POD_NM"].ToString();
				if (ref1 != "")
				{
					ViewBag.POD_NM = ref1;
				}
			}

			if (TempData.ContainsKey("ITEM_NM"))
			{
				string ref1 = TempData["ITEM_NM"].ToString();
				if (ref1 != "")
				{
					ViewBag.ITEM_NM = ref1;
				}
			}

			if (TempData.ContainsKey("REQ_SVC"))
			{
				string ref1 = TempData["REQ_SVC"].ToString();
				if (ref1 != "")
				{
					ViewBag.REQ_SVC = ref1;
				}
			}
			return View();
        }

        public ActionResult Inquiry()
        {
			if (TempData.ContainsKey("QUOT_NO"))
			{
				string ref1 = TempData["QUOT_NO"].ToString();
				if (ref1 != "")
				{
					ViewBag.QUOT_NO = ref1;
				}
			}
			return View();
        }
        
        public ActionResult InquiryDetail()
        {
			if (TempData.ContainsKey("QUOT_NO"))
			{
				string ref1 = TempData["QUOT_NO"].ToString();
				if (ref1 != "")
				{
					ViewBag.QUOT_NO = ref1;
				}
			}

			return View();
        }
		public ActionResult Regist()
		{
			return View();
		}

		public ActionResult RegistDetail()
		{
			if (TempData.ContainsKey("QUOT_NO"))
			{
				string ref1 = TempData["QUOT_NO"].ToString();
				if (ref1 != "")
				{
					ViewBag.QUOT_NO = ref1;
				}
			}

			return View();
		}

		public ActionResult ExInquiryDetail()
		{
            if (TempData.ContainsKey("MNGT_NO"))
            {
				string ref1 = TempData["MNGT_NO"].ToString();
				if(ref1 != "")
                {
					ViewBag.MNGT_NO = ref1;
                }
            }
			return View();
		}
		
		string strJson = "";
		public class JsonData
		{
			public string vJsonData { get; set; }


		}

		[HttpPost]
		public ActionResult fnGetFwdQuotData(JsonData value)
        {
            try
            {
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/GetForwarderQuotData", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch(Exception e)
            {
				strJson = e.Message;
				return Json(strJson);
			}
        }


		[HttpPost]
		public ActionResult fnGetForwarderList(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/GetForwarderList", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}


		[HttpPost]
		public ActionResult fnGetReccomendForwarderList(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/GetReccomendFowarder", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnSendQuotationToElvis(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataSet ds = JsonConvert.DeserializeObject<DataSet>(vJsonData);
				DataTable dt = ds.Tables["MAIN"];

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				fnGetAPIDataNoResponse("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/SendQuotationToElvis", "Y", strToken, vJsonData);
				string strResult = "Y";

				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnSetQuotStatus(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				fnGetAPIDataNoResponse("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/SetQuotStatus", "Y", strToken, vJsonData);
				string strResult = "Y";

				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnGetQuotationDetail(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/GetQuotationDetail", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnGetOcrData(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/GetOcrData", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnGetTrackingList(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/ExImData/GetTrackingList", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnGetQuotationDoc(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/GetQuotationDoc", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}




		[HttpPost]
		public ActionResult fnGetQuotationFreight(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/GetQuotationFreight", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnGetFwdQuotationList(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string test = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/GetFwdQuotationList", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}


		[HttpPost]
		public ActionResult fnFwdSendQuot(JsonData value)
        {
            try
            {
				string vJsonData = value.vJsonData.ToString();

				DataSet ds = JsonConvert.DeserializeObject<DataSet>(vJsonData);

				string strToken = ds.Tables[0].Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/PostFwdQuotationData", "Y", strToken, vJsonData);

				return Json(strResult);
			}
			catch(Exception e)
            {
				strJson = e.Message;
				return Json(strJson);
            }
        }


		[HttpPost]
		public ActionResult fnGetQuotationList(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string test = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/GetQuotationList", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}


		[HttpPost]
		public ActionResult fnGetQuotationFList(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string test = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/GetQuotationFList", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnGetFowarderInfo(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/GetFwdVolInfo", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnGetQuotationDetailList(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();

				DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

				string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Quotation/GetQuotationDetailList", "Y", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}
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



		public void fnGetAPIDataNoResponse(string strType, string strURL, string auth_type, string strToken, string strParam)
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
			}
			catch (Exception e)
			{
				strJson = e.Message;
			}
		}

	}
}