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

namespace ELVIS_META_WEB.Controllers
{
    public class ScheduleController : Controller
    {

        public ActionResult Air()
        {
            return View();
        }
        public ActionResult Sea()
        {
            return View();
        }

        Encryption ec = new Encryption(); //암호화 - Encryption 
        string strJson = "";
        string strResult = "";
        DataTable dt = new DataTable();
        Con_Schedule Con_Schedule = new Con_Schedule();

        public class JsonData
        {
            public string vJsonData { get; set; }
        }


        /// <summary>
		/// 스케줄 페이지 - Port 정보 가져오기
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpPost]
        public ActionResult fnGetSchData(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";
                string strResult = "";

                string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];

                //API 보내기
                strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Schedule/GetSchedule_Port", "N" , strToken, vJsonData);


                return Json(strResult);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        [HttpPost]
        public ActionResult fnGetSvtgAuthToken(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

                string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

                string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/GetSvtgAuthToken", "Y", strToken, vJsonData);


                return Json(strResult);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }


        [HttpPost]
        public string fnGetAuthToken(JsonData value)
        {
            string vJsonData = value.vJsonData.ToString();

            DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

            HttpWebRequest request = WebRequest.Create("https://svmp.seavantage.com/api/v1/user/authToken") as HttpWebRequest;
            
            request.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dt.Rows[0]["SVTG_ID"].ToString() + ":" + dt.Rows[0]["SVTG_PWD"].ToString())));
            
            request.Method = "GET";
            request.ContentType = "application/json";

            HttpWebResponse response;
            response = request.GetResponse() as HttpWebResponse;

            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string strResult = reader.ReadToEnd();
            //DataSet ds = JsonConvert.DeserializeObject<DataSet>(strResult);
            stream.Close();
            response.Close();
            reader.Close();

            return strResult;

        }


        [HttpPost]
        public ActionResult fnSetSvtgAuthToken(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

                string strToken = dt.Rows[0]["AUTH_KEY"].ToString();

                string strJson = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/SetSvtgAuthToken", "Y", strToken, vJsonData);


                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        [HttpPost]
        public ActionResult fnGetPortApi(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();


                string strToken = "TEST220926" + "^" + "hwkim@yjit.co.kr" + "^" + "KJj6Bq181sb+WVHADj/FTZAJHOGYgbGZyDv07KljjvAXHdm3pJdnDH9TM8pKEP1g";

                string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Schedule/GetPortList", "N", strToken, vJsonData);


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

    }
}