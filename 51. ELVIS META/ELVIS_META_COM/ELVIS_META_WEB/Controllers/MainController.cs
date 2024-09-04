using ELVIS_META_COMMON.Controller;
using ELVIS_META_COMMON.YJIT_Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Data;
using System.Net.Mail;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
namespace ELVIS_META_WEB.Scripts
{
    public class MainController : Controller
    {
		Encryption ec = new Encryption(); //암호화 - Encryption 
		string strJson = "";
		string strResult = "";		
		DataTable dt = new DataTable();
		Con_Main Con_Main = new Con_Main();

		// GET: Main
		public ActionResult Index()
        {
            return View();
        }
        public ActionResult Join()
        {
            return View();
        }
        public ActionResult Find()
        {
            return View();
        }
		
        public ActionResult Mypage()
        {
            return View();
        }
		public ActionResult Use()
		{
			return View();
		}
		public class JsonData
		{
			public string vJsonData { get; set; }
		}
		

		public class LoginCls
		{
			public string paramObj { get; set; }
		}


		/// <summary>
		/// 로그인 후 데이터 세션에 Save
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public ActionResult SaveLogin(JsonData value)
		{
			DataSet ds = JsonConvert.DeserializeObject<DataSet>(value.vJsonData);
			DataTable rst = ds.Tables["Result"];
			DataTable dt = ds.Tables["Table1"];

			try
			{
				if (rst.Rows[0]["trxCode"].ToString() == "N") return Content("N");

				if (rst.Rows[0]["trxCode"].ToString() == "Y")
				{
					Session["USR_ID"] = dt.Rows[0]["USR_ID"].ToString();
					Session["USR_NM"] = dt.Rows[0]["USR_NM"].ToString();
					Session["CUST_NM"] = dt.Rows[0]["LOC_NM"].ToString();
					Session["CRN"] = dt.Rows[0]["CRN"].ToString();
					Session["HP_NO"] = dt.Rows[0]["HP_NO"].ToString();
					Session["AUTH_KEY"] = dt.Rows[0]["MNGT_NO"].ToString() + "^" + dt.Rows[0]["USR_ID"].ToString() + "^" + dt.Rows[0]["AUTH_KEY"].ToString();
					Session["MNGT_NO"] = dt.Rows[0]["MNGT_NO"].ToString();
					Session["DOMAIN"] = System.Configuration.ConfigurationManager.AppSettings["Domain"];
					Session["USR_TYPE"] = dt.Rows[0]["USR_TYPE"].ToString();
					Session["DOCU"] = dt.Rows[0]["DOCU"].ToString();
					Session["EXPORT"] = dt.Rows[0]["EXPORT"].ToString();
					Session["IMPORT"] = dt.Rows[0]["IMPORT"].ToString();
					Session["QUOT"] = dt.Rows[0]["QUOT"].ToString();
					Session["SEA_EX"] = dt.Rows[0]["SEA_EX"].ToString();
					Session["SEA_IM"] = dt.Rows[0]["SEA_IM"].ToString();
					Session["AIR_EX"] = dt.Rows[0]["AIR_EX"].ToString();
					Session["AIR_IM"] = dt.Rows[0]["AIR_IM"].ToString();

					return Content("Y");
				}

				return Content("N");
			}
			catch (Exception e)
			{
				return Content(e.Message);
			}
		}

		/// <summary>
		/// 로그아웃 페이지
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public string fnLogOut()
		{
			Session.Clear();
			Session.RemoveAll();
			Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Response.Cache.SetNoStore();

			return "Y";
		}

		[HttpPost]
		public ActionResult fnTest(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();
				

				string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];

				string	strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/Schedule/GetSchedule_Port","N" , strToken, vJsonData);


				return Json(strJson);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnLoginApi(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();


				string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];

			string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/GetUserInfo", "N" , strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}


		[HttpPost]
		public ActionResult ChkEmailApi(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();


				string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/GetUserInfo", "N", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult UserInfoApi(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();


				string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/SetUserInfo", "N", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}

		}

			[HttpPost]
		public ActionResult GetFindIDApi(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();


				string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/FindUserId", "N", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnRegisterApi(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();


				string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/SetUserRegist", "N", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnGetCrnFile(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();


				string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/GetLicensePath", "N", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		


		[HttpPost]
		public void sendApiFile(JsonData value)
		{
			try
			{
				string mapPath = Server.MapPath("~/Content/TempFiles/test.pdf");
				byte[] pdfFile = UTF8Encoding.UTF8.GetBytes(mapPath);

				WebRequest request = WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/SetUserRegist");
				request.Method = "POST";
				request.ContentLength = pdfFile.Length;
				request.ContentType = "application/pdf";

				Stream stream = request.GetRequestStream();
				stream.Write(pdfFile, 0, pdfFile.Length);
				stream.Close();

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				StreamReader reader = new StreamReader(response.GetResponseStream());
				Console.WriteLine(reader.ReadToEnd());
				reader.Close();

			}
			catch (Exception e)
			{
			}
		}


		[HttpPost]
		public ActionResult UserInfoUpdateApi(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();


				string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/SetUserInfo", "N", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult UserServiceInfoUpdateApi(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();


				string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/SetUserServiceInfo", "N", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}

		[HttpPost]
		public ActionResult fnGoodBye(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();


				string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];

				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/GoodBye", "N", strToken, vJsonData);


				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}


		public string fnGetAPIData(string strType, string strURL, string auth_type ,  string strToken, string strParam)
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

		[HttpPost]
		public ActionResult fnGetPassword(JsonData value)
		{
			try
			{
				string pswd = fnGetRandomString(8);
				return Json(pswd);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}


		[HttpPost]
		public ActionResult JsonDataDesc(JsonData value)
		{
			string strJson = "";
			try
			{
				string vJsonData = value.vJsonData.ToString();
				DataSet ds = new DataSet();
				DataTable dt = new DataTable();
				ds = JsonConvert.DeserializeObject<DataSet>(vJsonData);
				dt = ds.Tables["DTL"];
				string id = ds.Tables["MAIN"].Rows[0]["ID"].ToString();
				string desc = ds.Tables["MAIN"].Rows[0]["DESC"].ToString();
					
				dt.DefaultView.Sort = id + " " + desc;
				dt = dt.DefaultView.ToTable();
				strJson = JsonConvert.SerializeObject(dt, Formatting.Indented);
				return Json(strJson);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			}
		}


		[HttpPost]
		public ActionResult SetPWApi(JsonData value)
		{
			try
			{
				string vJsonData = value.vJsonData.ToString();
				string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];
				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/SetUserPswdReset", "N", strToken, vJsonData);

				return Json(strResult);
			}
			catch (Exception e)
			{
				strJson = e.Message;
				return Json(strJson);
			} 
		}


		public string fnGetRandomString(int numLength)
		{

			string strResult = "";
			Random rand = new Random();
			string strRandomChar = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789";

			StringBuilder rs = new StringBuilder();

			for (int i = 0; i < numLength; i++)
			{
				rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
			}
			strResult = rs.ToString();

			return strResult;
		}

		public string ConnectionUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];
		string json = "";

		public string FileDownload(JsonData value) {

			string vJsonData = value.vJsonData.ToString();
			DataTable dt = new DataTable();
			dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);
			
			string targetUrl = ConnectionUrl + dt.Rows[0]["FILE_PATH"].ToString();
			string mapPath = Server.MapPath("~/Content/TempFiles/" + dt.Rows[0]["FILE_NM"].ToString());
			System.IO.FileInfo existFile;
			existFile = new System.IO.FileInfo(Server.MapPath("~/Content/TempFiles/") + dt.Rows[0]["FILE_NM"].ToString());

			if (existFile.Exists)
			{
				existFile.Delete();
			}

			using (WebClient myWebClient = new WebClient())
            {
                // Download the Web resource and save it into the current filesystem folder.
                myWebClient.DownloadFile(targetUrl, mapPath);
            }

			return "Y";
		}
		
		public string FileApiToServerDownload(JsonData value)
		{
			var test = "";
			try
			{
				string vJsonData = value.vJsonData.ToString();
				DataTable dt = new DataTable();
				dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);
				var dir = Path.GetDirectoryName(Server.MapPath("~/APIFILES/" + dt.Rows[0]["FILE_PATH"].ToString()));

				Directory.CreateDirectory(dir);
				 test = Server.MapPath("~/Files/TEMP/" + dt.Rows[0]["FILE_NM"].ToString()) + ".pdf";
				System.IO.File.Copy(Server.MapPath("~/APIFILES/" + dt.Rows[0]["FILE_PATH"].ToString()), test, true);

				FileInfo info = new FileInfo(test);
				double fileSize = ConvertBytesToKilobytes(info.Length);

				dt.Columns.Add("FILE_SIZE");
				dt.Rows[0]["FILE_SIZE"] = Math.Round(fileSize,2);

				vJsonData = JsonConvert.SerializeObject(dt);

				string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];
				string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/DocFile/UpdateFileStamp", "N", strToken, vJsonData);


			}
			catch (Exception e) {
				test = e.Message;
			}
            return test;
		}
		//MeraBytes 로 변환
		static double ConvertBytesToKilobytes(double bytes)
		{
			const double bytesInKb = 1024.0;
			return bytes / bytesInKb;
		}

		public string FileCustApiToServerDownload(JsonData value)
		{
			var test = "";
			try
			{
				string vJsonData = value.vJsonData.ToString();
				DataTable dt = new DataTable();
				dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);
				var dir = Path.GetDirectoryName(Server.MapPath("~/APIFILES/" + dt.Rows[0]["FILE_PATH"].ToString()));

				Directory.CreateDirectory(dir);
				test = Server.MapPath("~/Files/TEMP/" + dt.Rows[0]["FILE_NM"].ToString());
				System.IO.File.Copy(Server.MapPath("~/APIFILES/" + dt.Rows[0]["FILE_PATH"].ToString()), test, true);
			}
			catch (Exception e)
			{
				test = e.Message;
			}
			return test;
		}


		public class RtnFilesInfo
		{
			public string FILE_NAME { get; set; }
			public string FILE_NM { get; set; }
			public string FILE_PATH { get; set; }
			public string REPLACE_FILE_NM { get; set; }
			public string MNGT_NO { get; set; }
			public string INS_USR { get; set; }
			public string SEQ { get; set; }
		}

		[HttpGet]
		public ActionResult DownloadFile(RtnFilesInfo value)
		{
			string strFILE_NM = value.FILE_NM;
			string strFILE_PATH = value.FILE_PATH;
			string strREPLACE_FILE_NM = value.REPLACE_FILE_NM;

			try
			{
				System.IO.FileInfo fi = new System.IO.FileInfo(Server.MapPath("~/Content/TempFiles/") + strFILE_NM);

				if (fi.Exists)
				{
					return File(fi.FullName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strFILE_NM);
				}
				else
				{
					return Content("<script>alert('파일이 존재하지 않습니다.'); window.history.back();</script>");
				}
			}
			catch (Exception ex)
			{
				return Content("<script>alert('" + ex.Message + "')</script>");
			}
		}



		/// <summary>
		/// 부킹 - 파일 업로드 로직
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Upload_Files()
		{
			UTF8Encoding UTF8_Encodeing = new UTF8Encoding();
			string SavePath = "/EDMS/WEB/" + ConfigurationManager.AppSettings["Domain"].ToString() + "/" + ConfigurationManager.AppSettings["OfficeCode"].ToString() + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
			HttpFileCollectionBase files = Request.Files;
			string strFilePath = "";
			try
			{
				string ChkFileNM = Path.GetFileName(files[0].FileName);

				if (ChkFileNM != "")
				{
					for (int i = 0; i < files.Count; i++)
					{
						string InputFileName = "";
						string strFileSize = "";
						string strNowTime = "_" + DateTime.Now.ToString("yyyyMMddHHmmssffffff");
						string strRealFileNM = "";

						HttpPostedFileBase file = files[i];

						if (file.FileName != "" && file.ContentLength != 0)
						{
							if (file != null)
							{
								InputFileName = Path.GetFileName(file.FileName);
								InputFileName = Regex.Replace(InputFileName, @"[/\+:*?<>|""#]", "");
								InputFileName = InputFileName.Replace(" ", "");
								strRealFileNM = InputFileName;


								strFilePath = Path.Combine(Server.MapPath("~/Files/TEMP/") + InputFileName);

								file.SaveAs(strFilePath);
							}
						}
					}
				}


				return Json(strJson);
			}
			catch (Exception e)
			{
				return Json("[Error]" + e.Message);
            }
        }

        [HttpPost]
        public ActionResult OcrFiles()
        {
            UTF8Encoding UTF8_Encodeing = new UTF8Encoding();
            HttpFileCollectionBase files = Request.Files;
			string TMP_ID = Request.Form["TMP_ID"];
			string strFilePath = "";
            try
            {
                string ChkFileNM = Path.GetFileName(files[0].FileName);

                if (ChkFileNM != "")
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        string InputFileName = "";
                        string strFileSize = "";
                        string strNowTime = "_" + DateTime.Now.ToString("yyyyMMddHHmmssffffff");
                        string strRealFileNM = "";

                        HttpPostedFileBase file = files[i];

                        if (file.FileName != "" && file.ContentLength != 0)
                        {
                            if (file != null)
                            {
                                InputFileName = Path.GetFileName(file.FileName);
                                InputFileName = Regex.Replace(InputFileName, @"[/\+:*?<>|""#]", "");
                                //InputFileName = InputFileName.Replace(" ", "");
                                strRealFileNM = InputFileName;


                                strFilePath = Path.Combine(Server.MapPath("~/Files/TEMP/") + InputFileName);

                                file.SaveAs(strFilePath);
								if (TMP_ID != "")
								{
									strJson = FnOCRSend(InputFileName, TMP_ID);
								}
							}
                        }
                    }
                }


				return Json(strJson);
            }
            catch (Exception e)
            {
                return Json("[Error]" + e.Message);
            }
        }

		public string FnOCRSend(string file_nm, string tmp_id) {
			try
			{
				string api_url = "https://wm8czll3av.apigw.ntruss.com/custom/v1/18918/1bc86b9532859c433888273837b55327430d350f4f83e6ea0fbe94cd2f3b1bab/infer";

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(api_url));
				request.ContentType = "application/json";
				request.Headers.Add("X-OCR-SECRET", "S0phdml6V01XZ3dRVFZSdExzdmljd29CWnhIdHZxZmQ=");
				request.Method = "POST";

				JObject ClovaOcr = new JObject();

				ClovaOcr.Add("version", "V1");
				ClovaOcr.Add("requestId", "string");
				ClovaOcr.Add("timestamp", 0);
				ClovaOcr.Add("lang", "ko");
				string realfilePath = Server.MapPath("~/Files/TEMP/" + file_nm);
				string filePath = System.IO.Path.GetDirectoryName("~/Files/TEMP/" + file_nm);
				string tempExt = System.IO.Path.GetExtension(realfilePath);
				string tempFileName = DateTime.Now.ToString("yyMMddHHmmss") + tempExt;
				string tempFullPath = filePath;

				//System.IO.File.Copy(realfilePath, tempFullPath);

				System.IO.Stream fs = System.IO.File.Open(realfilePath, FileMode.Open);
				System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
				Byte[] bytes = br.ReadBytes(Convert.ToInt32(fs.Length));
				string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
				fs.Close();
				//if (System.IO.File.Exists(realfilePath)) {
				//    System.IO.File.Delete(realfilePath);
				//}

				JArray imageList = new JArray();
				JObject image = new JObject();
				image.Add("format", "pdf");
				image.Add("name", "CI");
				image.Add("data", base64String);
				//image.Add("templateIds", "[19491]");
				imageList.Add(image);
				ClovaOcr.Add("images", imageList);

				string temp = ClovaOcr.ToString();//.Replace("\\[19491]\\", "[19491]");
				Byte[] byteDataParams = Encoding.UTF8.GetBytes(temp);
				Stream st = request.GetRequestStream();
				st.Write(byteDataParams, 0, byteDataParams.Length);
				st.Close();

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();

				Stream stream = response.GetResponseStream();
				StreamReader reader = new StreamReader(stream, Encoding.UTF8);
				string RcvStr = reader.ReadToEnd();
				stream.Close();
				response.Close();
				reader.Close();


				return RcvStr;
			}
			catch (Exception e)
			{
				return e.Message;
			}

		}

    }
}