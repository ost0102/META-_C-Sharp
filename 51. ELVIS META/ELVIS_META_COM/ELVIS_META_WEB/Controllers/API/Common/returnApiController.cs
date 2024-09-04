using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ELVIS_META_WEB.Controllers.API.Common
{
    public class returnApiController : Controller
    {
        // GET: returnApi
        public ActionResult Index()
        {
            return View();
        }

        public class paramData
        {
            public string LOCATION { get; set; }
            public string CONTROLLER { get; set; }
            public string TMP_DTL { get; set; }
            public string QUOT_NO { get; set; }
            public string POL_CD { get; set; }
            public string POD_CD { get; set; }
            public string POL_NM { get; set; }
            public string POD_NM { get; set; }
            public string ITEM_NM { get; set; }
            public string REQ_SVC { get; set; }
            public string MNGT_NO { get; set; }
        }

        [HttpPost]
        public string CallPage(paramData paramData)
        {
            string rtnPage = "";
            string view = paramData.LOCATION;
            string controller = paramData.CONTROLLER;
            string TMP_DTL = paramData.TMP_DTL;
            string QUOT_NO = paramData.QUOT_NO;
            string POL_CD = paramData.POL_CD;
            string POD_CD = paramData.POD_CD;
            string POL_NM = paramData.POL_NM;
            string POD_NM = paramData.POD_NM;
            string ITEM_NM = paramData.ITEM_NM;
            string REQ_SVC = paramData.REQ_SVC;
            string MNGT_NO = paramData.MNGT_NO;

            try
            {
                if (paramData != null)
                {
                    if (MNGT_NO != null)
                    {
                        TempData["MNGT_NO"] = MNGT_NO;
                    }
                    if (TMP_DTL != null)
                    {
                        TempData["TMP_DTL"] = TMP_DTL;
                    }

                    if (QUOT_NO != null)
                    {
                        TempData["QUOT_NO"] = QUOT_NO;
                    }

                    if (POL_CD != null)
                    {
                        TempData["POL_CD"] = POL_CD;
                    }

                    if (POD_CD != null)
                    {
                        TempData["POD_CD"] = POD_CD;
                    }

                    if (POL_NM != null)
                    {
                        TempData["POL_NM"] = POL_NM;
                    }

                    if (POD_NM != null)
                    {
                        TempData["POD_NM"] = POD_NM;
                    }

                    if (ITEM_NM != null)
                    {
                        TempData["ITEM_NM"] = ITEM_NM;
                    }
                    if (REQ_SVC != null)
                    {
                        TempData["REQ_SVC"] = REQ_SVC;
                    }
                    rtnPage = "/" + controller + "/" + view;
                }
                return rtnPage;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        public ActionResult CallMailPage()
        {
            #region // Param 정의 내용
            /*
             * Param 구조
             * domain : domain
             * key : UserID
             * no : Primary Key
             * type : 업무구분
             *
             * MSG01 USR 회원가입
                MSG01 BKG 부킹
                MSG01 QUO 견적
                MSG01 HBL 비엘
                MSG01 INV 청구서
                MSG01 TRC 화물추적
             */
            #endregion
            DataTable LoginDt = new DataTable();

            try
            {
                string param = Request["id"];
                string param2 = Request["mngt_no"].Replace("amp;","");

                if (param != null)
                {

                    //Proc1. 자동로그인
                    #region // 로그인 하기
                    Hashtable paramHt = new Hashtable();
                    paramHt.Add("USR_ID", param);
                    paramHt.Add("MNGT_NO", param2);
                    string rtnJson = JsonConvert.SerializeObject(paramHt);
                    if (rtnJson.ToString().Substring(0, 1) != "[") rtnJson = "[" + rtnJson + "]";

                    string strToken = System.Configuration.ConfigurationManager.AppSettings["RPA_MngtNo"] + "^" + System.Configuration.ConfigurationManager.AppSettings["RPA_Key"];

                    string strResult = fnGetAPIData("POST", System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + "/api/UserData/GetUserInfo", "N", strToken, rtnJson);
                    if (strResult != null)
                    {

                        DataSet ds = JsonConvert.DeserializeObject<DataSet>(strResult);
                        DataTable rst = ds.Tables["Result"];
                        LoginDt = ds.Tables["Table1"];
                        if (rst.Rows[0]["trxCode"].ToString() == "Y")
                        {
                            
                            Session["USR_ID"] = LoginDt.Rows[0]["USR_ID"].ToString();
                            Session["CRN"] = LoginDt.Rows[0]["CRN"].ToString();
                            Session["MNGT_NO"] = LoginDt.Rows[0]["MNGT_NO"].ToString();
                            Session["AUTH_KEY"] = LoginDt.Rows[0]["MNGT_NO"].ToString() + "^" + LoginDt.Rows[0]["USR_ID"].ToString() + "^" + LoginDt.Rows[0]["AUTH_KEY"].ToString();
                            Session["USR_NM"] = LoginDt.Rows[0]["USR_NM"].ToString();
                            Session["CUST_NM"] = LoginDt.Rows[0]["LOC_NM"].ToString();
                            Session["HP_NO"] = LoginDt.Rows[0]["HP_NO"].ToString();
                        }
                        else
                        {
                            //결과값이 없다!

                        return RedirectToAction("Index", "");
                        }
                    }
                    else
                    {
                       // 결과값이 없다!

                      return RedirectToAction("Index", "");
                    }
                    #endregion
                    if (LoginDt.Rows.Count > 0) //로그인이 성공했다면
                    {
                        TempData["QUOT_NO"] = param2;

                        return RedirectToAction("Inquiry", "Estimate");
                    }
                    else
                    {
                        return RedirectToAction("index", "Main");
                    }
                }
                else
                {
                    //예외상황은 무조건 로그인 화면으로 이동
                    return RedirectToAction("index", "Main");
                }
            }
            catch (Exception e)
            {
                //예외상황은 무조건 로그인 화면으로 이동
                return RedirectToAction("index", "Main");
            }
        }


        public string fnGetAPIData(string strType, string strURL, string auth_type, string strToken, string strParam)
        {
            string strJson = "";
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