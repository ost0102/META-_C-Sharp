using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Data;
using Newtonsoft.Json;

namespace ELVIS_META_WEB.Controllers
{
	public class UnipassController : Controller
	{
		DataTable dt = new DataTable();

		public class JsonData
		{
			public string vJsonData { get; set; }
		}

		/// <summary>
		/// 화물진행정보 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult GetCargoInfo(JsonData value)
		{
			string vJsonData = value.vJsonData.ToString();
			dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

			string str_crkyCn = dt.Rows[0]["crkyCn"].ToString();
			string str_cargMtNo = dt.Rows[0]["cargMtNo"].ToString();
			string str_mblNo = dt.Rows[0]["mblNo"].ToString();
			string str_hblNo = dt.Rows[0]["hblNo"].ToString();
			string str_blYy = dt.Rows[0]["blYy"].ToString();
			string sURL = "";
			string ReadingText = "";
			//
			////화물관리번호 , BL 사용해서 했는지 확인 여부 필요.
			if (string.IsNullOrEmpty(str_cargMtNo) == false)
			{
				sURL = "https://unipass.customs.go.kr:38010/ext/rest/cargCsclPrgsInfoQry/retrieveCargCsclPrgsInfo?crkyCn=" + str_crkyCn + "&cargMtNo=" + str_cargMtNo;
			}
			else if (string.IsNullOrEmpty(str_blYy) == false)
			{
				sURL = "https://unipass.customs.go.kr:38010/ext/rest/cargCsclPrgsInfoQry/retrieveCargCsclPrgsInfo?crkyCn=" + str_crkyCn + "&mblNo=" + str_mblNo + "&hblNo=" + str_hblNo + "&blYy=" + str_blYy;
			}
			else
			{
				sURL = "https://unipass.customs.go.kr:38010/ext/rest/cargCsclPrgsInfoQry/retrieveCargCsclPrgsInfo?crkyCn=" + str_crkyCn;
			}

			if (sURL != "")
			{
				WebRequest wrGETURL;
				wrGETURL = WebRequest.Create(sURL);
				WebProxy myProxy = new WebProxy("myproxy", 80);

				myProxy.BypassProxyOnLocal = true;

				//wrGETURL.Proxy = WebProxy.GetDefaultProxy();

				Stream objStream;
				objStream = wrGETURL.GetResponse().GetResponseStream();

				StreamReader objReader = new StreamReader(objStream);

				string sLine = "";
				int i = 0;
				while (sLine != null)
				{
					i++;
					sLine = objReader.ReadLine();
					ReadingText += sLine;
				}
			}

			return this.Content(ReadingText, "text/xml");
		}

		/// <summary>
		/// 수출이행내역 XML 가져오기 - 수출신고번호
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult GetOBNumInfo(JsonData value)
		{
			string vJsonData = value.vJsonData.ToString();
			dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

			string str_crkyCn = dt.Rows[0]["crkyCn"].ToString();
			string str_expDclrNo = dt.Rows[0]["expDclrNo"].ToString();
			string str_Url = "";
			string ReadingText = "";
			WebRequest wrGETURL;

			if (string.IsNullOrEmpty(str_expDclrNo) == false)
			{
				str_Url = "https://unipass.customs.go.kr:38010/ext/rest/expDclrNoPrExpFfmnBrkdQry/retrieveExpDclrNoPrExpFfmnBrkd?crkyCn=" + str_crkyCn + "&expDclrNo=" + str_expDclrNo;
			}
			else
			{
				str_Url = "https://unipass.customs.go.kr:38010/ext/rest/expDclrNoPrExpFfmnBrkdQry/retrieveExpDclrNoPrExpFfmnBrkd?crkyCn=" + str_crkyCn;
			}

			if (str_Url != "")
			{
				wrGETURL = WebRequest.Create(str_Url);
				WebProxy myProxy = new WebProxy("myproxy", 80);

				myProxy.BypassProxyOnLocal = true;

				//wrGETURL.Proxy = WebProxy.GetDefaultProxy();

				Stream objStream;
				objStream = wrGETURL.GetResponse().GetResponseStream();

				StreamReader objReader = new StreamReader(objStream);

				string sLine = "";
				int i = 0;
				while (sLine != null)
				{
					i++;
					sLine = objReader.ReadLine();
					ReadingText += sLine;
				}
			}

			return this.Content(ReadingText, "text/xml");
		}


		/// <summary>
		/// 수출이행내역 XML 가져오기 - B/L
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult GetOBBLInfo(JsonData value)
		{
			string vJsonData = value.vJsonData.ToString();
			dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);

			string str_crkyCn = dt.Rows[0]["crkyCn"].ToString();
			string str_blNo = dt.Rows[0]["blNo"].ToString();
			string str_Url = "";
			string ReadingText = "";
			WebRequest wrGETURL;

			if (string.IsNullOrEmpty(str_blNo) == false)
			{
				str_Url = "https://unipass.customs.go.kr:38010/ext/rest/expDclrNoPrExpFfmnBrkdQry/retrieveExpDclrNoPrExpFfmnBrkd?crkyCn=" + str_crkyCn + "&blNo=" + str_blNo;
			}
			else
			{
				str_Url = "https://unipass.customs.go.kr:38010/ext/rest/expDclrNoPrExpFfmnBrkdQry/retrieveExpDclrNoPrExpFfmnBrkd?crkyCn=" + str_crkyCn;
			}


			if (str_Url != "")
			{
				wrGETURL = WebRequest.Create(str_Url);
				WebProxy myProxy = new WebProxy("myproxy", 80);

				myProxy.BypassProxyOnLocal = true;

				//wrGETURL.Proxy = WebProxy.GetDefaultProxy();

				Stream objStream;
				objStream = wrGETURL.GetResponse().GetResponseStream();

				StreamReader objReader = new StreamReader(objStream);

				string sLine = "";
				int i = 0;
				while (sLine != null)
				{
					i++;
					sLine = objReader.ReadLine();
					ReadingText += sLine;
				}
			}

			return this.Content(ReadingText, "text/xml");
		}

	}
}
