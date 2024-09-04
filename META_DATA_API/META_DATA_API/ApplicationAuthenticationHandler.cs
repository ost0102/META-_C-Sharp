using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Configuration;
using META_DATA_API.Models;
using META_DATA_API.Models.TBL;
using META_DATA_API.Models.Query;

using System.Windows.Forms;

namespace META_DATA_API
{
    public class ApplicationAuthenticationHandler : DelegatingHandler
    {
        // Http Response Messages  
        private const string InvalidType = "Invalid Authorization-Type";
        private const string MissingType = "Missing Authorization-Type";
        private const string InvalidToken = "Invalid Authorization-Token";
        private const string MissingToken = "Missing Authorization-Token";
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            string strLog = "";
            // Checking the Header values
            try
            {
                // Write your Authentication code here
                IEnumerable<string> AuthTypeHeaderValues = null;
                IEnumerable<string> RpaApiKeyHeaderValues = null;
                // 인증키 체크 여부 확인
                if (request.Method == HttpMethod.Post)
                {
                    if (request.Headers.TryGetValues("Authorization-Type", out AuthTypeHeaderValues))
                    {
                        if (AuthTypeHeaderValues.First().ToString() == "N")
                        {
                            var userNameClaim = new Claim(ClaimTypes.Name, "UnLogin");
                            var identity = new ClaimsIdentity(new[] { userNameClaim }, "UnloginApiKey");
                            var principal = new ClaimsPrincipal(identity);
                            Thread.CurrentPrincipal = principal;
                            if (System.Web.HttpContext.Current != null)
                            {
                                System.Web.HttpContext.Current.User = principal;
                            }
                            //Log 정보 저장
                            strLog += "[Auth-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Unlogin     /   " + request.RequestUri + System.Environment.NewLine;
                            _LogWriter.Auth_Writer(strLog);
                        }
                        else
                        {
                            //*****인증키 배열 순서 : [MNGT_NO]|[USR_ID]|[CRN]|[AUTH_STRT_YMD]|[AUTH_END_YMD]
                            //string testVal = "20220929155634206199" + "|" + "bhhyun@yjit.co.kr" + "|" + "1148183347" + "||;
                            //string test = _Sec_Encrypt.encryptAES256(testVal);
                            if (request.Headers.TryGetValues("Authorization-Token", out RpaApiKeyHeaderValues))
                            {
                                //MNGT_NO 와 USR_ID 와 Auth Key 로 구성 (ex:   X-AppApiKey: [MNGT_NO]:[Auth_Key]
                                string[] apiKeyHeaderValue = RpaApiKeyHeaderValues.First().Split('^');
                                // Validating header value must have both APP ID & APP key  
                                if (apiKeyHeaderValue.Length == 3)
                                {
                                    //인증키 체크 로직
                                    //1. 복호화
                                    var appID = apiKeyHeaderValue[0];  //MNGT_NO
                                    var usrID = apiKeyHeaderValue[1];  //USR_ID

                                    var AppKey = _Sec_Encrypt.decryptAES256(apiKeyHeaderValue[2]);  //인증키                        
                                    string[] AppKeysList = AppKey.ToString().Split('|');

                                    //Valisation Check
                                    if (AppKeysList.Length != 5) return requestCancel(request, cancellationToken, InvalidToken);    //Param Length
                                    if (appID.ToString() != AppKeysList[0].ToString()) return requestCancel(request, cancellationToken, InvalidToken); //MNGT_NO = AppKey.MNGT_NO
                                    if (usrID.ToString() != AppKeysList[1].ToString()) return requestCancel(request, cancellationToken, InvalidToken); //USR_ID = AppKey.USR_ID
                                    if (AppKeysList[3].ToString().Length == 8 && AppKeysList[4].ToString().Length == 8)   //check Start Date ~ End Date
                                    {
                                        string App_In_Days = System.Web.Configuration.WebConfigurationManager.AppSettings["CheckAppDate"].ToString();
                                        if (string.IsNullOrEmpty(App_In_Days)) App_In_Days = "0";

                                        DateTime Now_Date = DateTime.Now;
                                        DateTime Start_Date = Convert.ToDateTime(AppKeysList[3].ToString());
                                        DateTime End_Date = Convert.ToDateTime(AppKeysList[4].ToString()).AddDays(int.Parse(App_In_Days));
                                        if (Now_Date >= Start_Date && Now_Date <= End_Date) { }
                                        else
                                        {
                                            return requestCancel(request, cancellationToken, InvalidToken);
                                        }
                                    }

                                    META_USR_MST Auth_list = new META_USR_MST();
                                    //3. DB 값 체크                            
                                    Auth_list.MNGT_NO = AppKeysList[0].ToString();
                                    Auth_list.USR_ID = AppKeysList[1].ToString();
                                    Auth_list.CRN = AppKeysList[2].ToString();
                                    Auth_list.AUTH_STRT_YMD = AppKeysList[3].ToString();
                                    Auth_list.AUTH_END_YMD = AppKeysList[4].ToString();

                                    if (Sql_Comm.Check_Auth(Auth_list))
                                    {
                                        var userNameClaim = new Claim(ClaimTypes.Name, appID);
                                        var identity = new ClaimsIdentity(new[] { userNameClaim }, "AppApiKey");
                                        var principal = new ClaimsPrincipal(identity);
                                        Thread.CurrentPrincipal = principal;
                                        if (System.Web.HttpContext.Current != null)
                                        {
                                            System.Web.HttpContext.Current.User = principal;
                                        }
                                        //Log 정보 저장
                                        strLog += "[Auth-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    MNGT_NO:   " + appID + "   /   USR_ID:   " + usrID + "   /   Authorization-Token:" + AppKey + System.Environment.NewLine;
                                        _LogWriter.Auth_Writer(strLog);
                                    }
                                    else
                                    {
                                        return requestCancel(request, cancellationToken, InvalidToken);
                                    }
                                }
                                else
                                {
                                    // Web request cancel reason missing APP key or APP ID  
                                    return requestCancel(request, cancellationToken, MissingToken);
                                }
                            }
                            else
                            {
                                // Web request cancel reason APP key missing all parameters  
                                return requestCancel(request, cancellationToken, MissingToken);
                            }
                        }
                    }
                    else
                    {
                        return requestCancel(request, cancellationToken, MissingType);
                    }
                }
                
                return base.SendAsync(request, cancellationToken);
            }
            catch(Exception e)
            {
                return requestCancel(request, cancellationToken, e.Message);
            }            
        }
        private System.Threading.Tasks.Task<HttpResponseMessage> requestCancel(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken, string message)
        {
            CancellationTokenSource _tokenSource = new CancellationTokenSource();
            cancellationToken = _tokenSource.Token;
            _tokenSource.Cancel();
            HttpResponseMessage response = new HttpResponseMessage();
            response = request.CreateResponse(HttpStatusCode.BadRequest);
            response.Content = new StringContent(message);
            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
                return response;
            });
        }
    }
}