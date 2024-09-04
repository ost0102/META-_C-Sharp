using System;
using System.Data;
using System.Text;
using System.Net.Mail;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR.Client.Hubs;


namespace META_DATA_API.Models
{
    public static class _common
    {
        public static string jSon = "";
        public static string sec_jSon = "";
        private static string strPageCount = "100";
        public static void ThrowMsg(bool ErrorOccur, string Msg)
        {
            ErrorOccur = true;
            throw new Exception(Msg);
        }

        public static int DataPageCount        
        {
            get
            {
                int rtnCount = 0;

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["DataPageCount"] != null)
                {
                    strPageCount = System.Web.Configuration.WebConfigurationManager.AppSettings["DataPageCount"].ToString();
                }

                rtnCount = int.Parse(strPageCount);
                return rtnCount;
            }
        }

        #region Make Json - Data Convert to jSon
        
        public static string MakeJson(string status, string Msg)
        {
            sec_jSon = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {                                
                dt.Columns.Add("trxCode");
                dt.Columns.Add("trxMsg");
                DataRow row1 = dt.NewRow();
                row1["trxCode"] = status;
                row1["trxMsg"] = Msg;
                dt.Rows.Add(row1);
                dt.TableName = "Result";
                ds.Tables.Add(dt);
                //sec_jSon = _Sec_Encrypt.encryptAES256(JsonConvert.SerializeObject(ds, Formatting.Indented));
                sec_jSon = JsonConvert.SerializeObject(ds);
                return sec_jSon;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string MakeJson(string status, string Msg, DataTable args)
        {
            sec_jSon = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {                                
                dt.Columns.Add("trxCode");
                dt.Columns.Add("trxMsg");
                DataRow row1 = dt.NewRow();
                row1["trxCode"] = status;
                row1["trxMsg"] = Msg;
                dt.Rows.Add(row1);
                dt.TableName = "Result";
                ds.Tables.Add(dt);
                if (status != "E" && args.Rows.Count > 0) ds.Tables.Add(args);
                //jSon = JsonConvert.SerializeObject(ds);
                //sec_jSon = _Sec_Encrypt.encryptAES256(JsonConvert.SerializeObject(ds));
                sec_jSon = JsonConvert.SerializeObject(ds);
                return sec_jSon;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string MakeJson(string status, string Msg, DataSet args)
        {
            sec_jSon = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("trxCode");
                dt.Columns.Add("trxMsg");
                DataRow row1 = dt.NewRow();
                row1["trxCode"] = status;
                row1["trxMsg"] = Msg;
                dt.Rows.Add(row1);
                dt.TableName = "Result";
                ds.Tables.Add(dt);
                if (status != "E" && args.Tables.Count > 0)
                {
                    for(int i = 0;i<args.Tables.Count;i++) ds.Tables.Add(args.Tables[i].Copy());
                }
                //jSon = JsonConvert.SerializeObject(ds);
                //sec_jSon = _Sec_Encrypt.encryptAES256(JsonConvert.SerializeObject(ds));
                sec_jSon = JsonConvert.SerializeObject(ds);
                return sec_jSon;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string MakeJson(DataTable Result, DataTable DT1)
        {
            jSon = "";
            sec_jSon = "";
            try
            {
                DataSet ds = new DataSet();
                Result.TableName = "Master";
                ds.Tables.Add(Result);
                DT1.TableName = "Detail";
                ds.Tables.Add(DT1);
                jSon = JsonConvert.SerializeObject(ds, Formatting.Indented);
                //sec_jSon = _Sec_Encrypt.encryptAES256(jSon);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return jSon;
        }

        /// <summary>
        /// return DS
        /// </summary>
        /// <param name="Result">MakeResultDT를 써서 가져온 DataTable을 넣어주면 됨.</param>
        /// <param name="DT1">병합할 DataTable 첫번째</param>
        /// <param name="DT2">병합할 DataTable 두번째</param>
        /// <returns></returns>
        public static string MakeJson(DataTable Result, DataTable DT1, DataTable DT2)
        {
            jSon = "";
            sec_jSon = "";
            try
            {
                DataSet ds = new DataSet();
                if (DT1.TableName == DT2.TableName)
                {
                    DT1.TableName = DT1.TableName + "1";
                    DT2.TableName = DT2.TableName + "2";
                }

                ds.Tables.Add(Result);
                ds.Tables.Add(DT1);
                ds.Tables.Add(DT2);
                jSon = JsonConvert.SerializeObject(ds, Formatting.Indented);
                sec_jSon = _Sec_Encrypt.encryptAES256(jSon);
                return sec_jSon;
            }
            catch (Exception e)
            {
                return e.Message;
            }            
        }

        public static string MakeNonJson(string status, string Msg, DataTable args)
        {
            jSon = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {                                
                dt.Columns.Add("trxCode");
                dt.Columns.Add("trxMsg");
                DataRow row1 = dt.NewRow();
                row1["trxCode"] = status;
                row1["trxMsg"] = Msg;
                dt.Rows.Add(row1);
                dt.TableName = "Result";
                ds.Tables.Add(dt);
                if (status != "E" && args.Rows.Count > 0) ds.Tables.Add(args);
                //string strValue = JsonConvert.SerializeObject(ds);
                jSon = JsonConvert.SerializeObject(ds);
                return jSon;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string MakeNonJson(DataTable Result, DataTable DT1)
        {

            jSon = "";            
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(Result);
                ds.Tables.Add(DT1);
                jSon = JsonConvert.SerializeObject(ds, Formatting.Indented);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return jSon;
        }
        #endregion

        /// <summary>
        /// 결과값 DT로 만들어 주는 함수
        /// </summary>
        /// <param name="status"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static DataTable MakeResultDT(string status, string Msg)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("trxCode");
            dt.Columns.Add("trxMsg");
            DataRow row1 = dt.NewRow();
            row1["trxCode"] = status;
            row1["trxMsg"] = Msg;
            dt.Rows.Add(row1);
            dt.TableName = "Result";
            return dt;
        }

        /// <summary>
        /// 랜덤 텍스트 가지고 오기 + 숫자
        /// </summary>
        /// <param name="numLength">텍스트 길이</param>
        /// <returns></returns>
        public static string fnGetRandomString(int numLength)
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

        public static string fnGetConfirmKey()
        {
            string strResult = "";
            Random rand = new Random();
            string strRandomChar = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
            string strRandomNum = "1234567890";
            string strRandomSC = "!@$%^()";
            StringBuilder rs = new StringBuilder();
            //특수문자 1 + 문자 3 + 숫자 1 + 문자 3 + 특수문자 1
            rs.Append(strRandomSC[(int)(rand.NextDouble() * strRandomSC.Length)]);
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            rs.Append(strRandomNum[(int)(rand.NextDouble() * strRandomNum.Length)]);
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            rs.Append(strRandomSC[(int)(rand.NextDouble() * strRandomSC.Length)]);
            strResult = rs.ToString();
            return strResult;
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

        public static bool fnSendMail(string toAddr, string Html)
        {
            try 
            {
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("mail.yjit.co.kr", 587)
                {
                    UseDefaultCredentials = false, // 시스템에 설정된 인증 정보를 사용하지 않는다. 
                    EnableSsl = false,  // SSL을 사용한다. 
                    DeliveryMethod = SmtpDeliveryMethod.Network, // 이걸 하지 않으면 Gmail에 인증을 받지 못한다. 
                    Credentials = new System.Net.NetworkCredential("mailmaster@yjit.co.kr", "Yjit0921)#$%"),
                    Timeout = 100000
                };

                MailAddress from = new MailAddress("help@yjit.co.kr");
                MailAddress to = new MailAddress(toAddr);

                MailMessage message = new MailMessage(from, to);
                message.Body = Html;
                message.IsBodyHtml = true;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = "[ELVIS-FRIEND] 임시 비밀번호 입니다.";
                message.SubjectEncoding = System.Text.Encoding.UTF8;

                System.Net.ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                client.Send(message);
                message.Dispose();
            }
            catch 
            {

                return false;
            }

            return true;
        }

        #region // ======Message ChatHub====== 
        public static IHubProxy messageHub = null;
        public static HubConnection HubConnection = null;
        public static IHubProxy chatHub = null;

        public static bool connectToCallChatHub(Dictionary<string, string> dicChat)
        {
            string MessageHubURL = "http://chat.elvisprime.com/signalr";
            bool rtnResult = true;

            try
            {
                if (!string.IsNullOrEmpty(MessageHubURL))
                {
                    HubConnection = new HubConnection(MessageHubURL);
                    messageHub = HubConnection.CreateHubProxy("chatHub");
                    HubConnection.Start().Wait();
                    messageHub.Invoke("Connect", dicChat);

                    _LogWriter.Writer("[connectToCallChatHub ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    ChatHub Connected" + System.Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                _LogWriter.Writer("[connectToCallChatHub ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    failed : " + ex.Message + System.Environment.NewLine);
                rtnResult = false;
            }

            return rtnResult;
        }

        public static void SendMessageHub(string domain, string sender, string receiver, string reqsvc, string msgType, string jobType, string msg, string msgId,
                                   string ref1 = "", string ref2 = "", string ref3 = "", string ref4 = "", string ref5 = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(domain) && !string.IsNullOrEmpty(sender) && !string.IsNullOrEmpty(msg))
                {
                    _LogWriter.Writer("[SendMessageHub ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Try ChatHub Connect" + System.Environment.NewLine);

                    //도메인|보내는사람|받는사람|요청서비스|구분|형태|메세지|key아이디|
                    var FullMsg = domain + "|" + sender + "|" + receiver + "|" + reqsvc + "|" + msgType + "|" + jobType + "|" + msg + "|" + msgId + "|" + ref1 + "|" + ref2 + "|" + ref3 + "|" + ref4 + "|" + ref5;
                    messageHub.Invoke("prime_Message", sender, FullMsg);
                }
            }
            catch (Exception ex)
            {
                _LogWriter.Writer("[SendMessageHub ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    failed : " + ex.Message + System.Environment.NewLine);
            }
        }

        public static void DiscConnectChatHub(Dictionary<string, string> dicChat)
        {
            try
            {
                chatHub.Invoke("DisConnect", dicChat);
            }
            catch (Exception ex)
            {
                _LogWriter.Writer("[DisConnectChatHub ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    failed : " + ex.Message + System.Environment.NewLine);
            }
        }
        #endregion
    }
}