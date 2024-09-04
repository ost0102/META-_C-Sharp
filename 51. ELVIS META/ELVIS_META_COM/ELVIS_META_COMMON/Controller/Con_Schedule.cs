using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ELVIS_META_COMMON.Query;
using ELVIS_META_COMMON.YJIT_Utils;
using ELVIS_META_DATA;
using System.Reflection;

namespace ELVIS_META_COMMON.Controller
{
    public class Con_Schedule
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        Schedule_Query SQ = new Schedule_Query();

        //전역 변수
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();
        string rtnJson = "";

        /// <summary>
        /// 스케줄 - 포트 정보 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetPort(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_GetPortData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Table";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "Fail", Resultdt);
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "Success", Resultdt);
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 스케줄 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        public string Con_fnGetSchData(string strValue,string strData)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                //API에서 가져온 데이터 테이블 세팅
                DataSet ds = JsonConvert.DeserializeObject<DataSet>(strData);

                //예외처리
                if (ds.Tables["Table1"] == null)
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                    return rtnJson;
                }

                DataTable APIdt = ds.Tables["Result"];
                if(APIdt.Rows[0]["trxCode"].ToString() != "Y")
                {
                    rtnJson = comm.MakeJson("N", APIdt.Rows[0]["trxMsg"].ToString());
                    return rtnJson;
                }
                else
                {
                    APIdt = ds.Tables["Table1"];
                    APIdt.TableName = "API_Data";
                }

                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_fnGetSchData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "DB_Data";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "Fail", Resultdt);
                }
                else
                {
                    
                    //비교 - REQ_SVC , LINE_CD , POL_CD , POD_CD , ETD , ETA , VSL , VOY
                    var query = (from APIData in APIdt.AsEnumerable()
                                 join dbData in Resultdt.AsEnumerable()
                                 on new
                                 {
                                     REQ_SVC = APIData.Field<string>("REQ_SVC").ToString(),
                                     LINE_CD = APIData.Field<string>("LINE_CD").ToString(),                                                                          
                                     POL_CD = APIData.Field<string>("POL_CD").ToString(),
                                     POD_CD = APIData.Field<string>("POD_CD").ToString(),
                                     ETD = APIData.Field<string>("ETD").ToString(),
                                     ETA = APIData.Field<string>("ETA").ToString(),
                                     VSL = APIData.Field<string>("VSL").ToString(),
                                     VOY = APIData.Field<string>("VOY").ToString()
                                   
                                 }
                                 equals new
                                 {                                     
                                     REQ_SVC = dbData.Field<string>("REQ_SVC").ToString(),
                                     LINE_CD = dbData.Field<string>("LINE_CD").ToString(),
                                     POL_CD = dbData.Field<string>("POL_CD").ToString(),
                                     POD_CD = dbData.Field<string>("POD_CD").ToString(),
                                     ETD = dbData.Field<string>("ETD").ToString(),
                                     ETA = dbData.Field<string>("ETA").ToString(),
                                     VSL = dbData.Field<string>("VSL").ToString(),
                                     VOY = dbData.Field<string>("VOY").ToString()
                    
                                 } into SchData
                                 from SchDB in SchData.DefaultIfEmpty()
                                 select new
                                 {
                                   
                                     REQ_SVC = APIData.Field<string>("REQ_SVC") == null ? string.Empty : APIData.Field<string>("REQ_SVC"),
                                     LINE_CD = APIData.Field<string>("LINE_CD") == null ? string.Empty : APIData.Field<string>("LINE_CD"),
                                     LINE_PATH = ((SchDB == null) ? string.Empty : SchDB.Field<string>("LINE_PATH")),
                                     POL_CD = APIData.Field<string>("POL_CD") == null ? string.Empty : APIData.Field<string>("POL_CD"),
                                     POD_CD = APIData.Field<string>("POD_CD") == null ? string.Empty : APIData.Field<string>("POD_CD"),
                                     ETD = APIData.Field<string>("ETD") == null ? string.Empty : APIData.Field<string>("ETD"),
                                     ETD_HM = APIData.Field<string>("ETD_HM") == null ? string.Empty : APIData.Field<string>("ETD_HM"),
                                     ETA = APIData.Field<string>("ETA") == null ? string.Empty : APIData.Field<string>("ETA"),
                                     ETA_HM = APIData.Field<string>("ETA_HM") == null ? string.Empty : APIData.Field<string>("ETA_HM"),
                                     VSL = APIData.Field<string>("VSL") == null ? string.Empty : APIData.Field<string>("VSL"),
                                     VOY = APIData.Field<string>("VOY") == null ? string.Empty : APIData.Field<string>("VOY"),
                                     DOC_CLOS_YMD = APIData.Field<string>("DOC_CLOS_YMD") == null ? string.Empty : APIData.Field<string>("DOC_CLOS_YMD"),
                                     DOC_CLOS_HM = APIData.Field<string>("DOC_CLOS_HM") == null ? string.Empty : APIData.Field<string>("DOC_CLOS_HM"),
                                     CARGO_CLOS_YMD = APIData.Field<string>("CARGO_CLOS_YMD") == null ? string.Empty : APIData.Field<string>("CARGO_CLOS_YMD"),
                                     CARGO_CLOS_HM = APIData.Field<string>("CARGO_CLOS_HM") == null ? string.Empty : APIData.Field<string>("CARGO_CLOS_HM"),
                                     SCH_PIC = APIData.Field<string>("SCH_PIC") == null ? string.Empty : APIData.Field<string>("SCH_PIC"),
                                     RMK = APIData.Field<string>("RMK") == null ? string.Empty : APIData.Field<string>("RMK"),
                                     POL_NM = APIData.Field<string>("POL_NM") == null ? string.Empty : APIData.Field<string>("POL_NM"),
                                     POD_NM = APIData.Field<string>("POD_NM") == null ? string.Empty : APIData.Field<string>("POD_NM"),
                                     //POL_NM = ((SchDB == null) ? string.Empty : SchDB.Field<string>("POL_NM")),
                                     //POD_NM = ((SchDB == null) ? string.Empty : SchDB.Field<string>("POD_NM")),
                                     FCL_CNT = ((SchDB == null) ? 0 : SchDB.Field<int>("FCL_CNT")),
                                     LCL_CNT = ((SchDB == null) ? 0 : SchDB.Field<int>("LCL_CNT")),
                                     CONSOL_CNT = ((SchDB == null) ? 0 : SchDB.Field<int>("CONSOL_CNT"))
                                 });

                    //var query = (from APIData in APIdt.AsEnumerable()
                    //             join dbData in Resultdt.AsEnumerable()
                    //             on new
                    //             {
                    //                 REQ_SVC = APIData.Field<string>("REQ_SVC"),
                    //                 LINE_CD = APIData.Field<string>("LINE_CD"),
                    //                 POL_CD = APIData.Field<string>("POL_CD"),
                    //                 POD_CD = APIData.Field<string>("POD_CD"),
                    //                 ETD = APIData.Field<string>("ETD"),
                    //                 ETA = APIData.Field<string>("ETA"),
                    //                 VSL = APIData.Field<string>("VSL"),
                    //                 VOY = APIData.Field<string>("VOY")
                    //
                    //             }
                    //             equals new
                    //             {
                    //                 REQ_SVC = dbData.Field<string>("REQ_SVC"),
                    //                 LINE_CD = dbData.Field<string>("LINE_CD"),
                    //                 POL_CD = dbData.Field<string>("POL_CD"),
                    //                 POD_CD = dbData.Field<string>("POD_CD"),
                    //                 ETD = dbData.Field<string>("ETD"),
                    //                 ETA = dbData.Field<string>("ETA"),
                    //                 VSL = dbData.Field<string>("VSL"),
                    //                 VOY = dbData.Field<string>("VOY")
                    //
                    //             }
                    //             select new
                    //             {                                     
                    //                 REQ_SVC = APIData.Field<string>("REQ_SVC"),
                    //                 LINE_CD = APIData.Field<string>("LINE_CD"),
                    //                 POL_CD = APIData.Field<string>("POL_CD"),
                    //                 POD_CD = APIData.Field<string>("POD_CD"),
                    //                 ETD = APIData.Field<string>("ETD"),
                    //                 ETD_HM = APIData.Field<string>("ETD_HM"),
                    //                 ETA = APIData.Field<string>("ETA"),
                    //                 ETA_HM = APIData.Field<string>("ETA_HM"),
                    //                 VSL = APIData.Field<string>("VSL"),
                    //                 VOY = APIData.Field<string>("VOY"),
                    //                 DOC_CLOS_YMD = APIData.Field<string>("DOC_CLOS_YMD"),
                    //                 DOC_CLOS_HM = APIData.Field<string>("DOC_CLOS_HM"),
                    //                 CARGO_CLOS_YMD = APIData.Field<string>("CARGO_CLOS_YMD"),
                    //                 CARGO_CLOS_HM = APIData.Field<string>("CARGO_CLOS_HM"),
                    //                 SCH_PIC = APIData.Field<string>("SCH_PIC"),
                    //                 RMK = APIData.Field<string>("RMK"),
                    //                 POL_NM = dbData.Field<string>("POL_NM"),
                    //                 POD_NM = dbData.Field<string>("POD_NM"),
                    //                 FCL_CNT = dbData.Field<int>("FCL_CNT"),
                    //                 LCL_CNT = dbData.Field<int>("LCL_CNT"),
                    //                 CONSOL_CNT = dbData.Field<int>("CONSOL_CNT")
                    //
                    //             });

                    DataTable Final_DT = new DataTable();
                    Final_DT.Columns.Add("RNUM");
                    Final_DT.Columns.Add("PAGE");
                    Final_DT.Columns.Add("TOTCNT");
                    Final_DT.Columns.Add("REQ_SVC");
                    Final_DT.Columns.Add("LINE_CD");
                    Final_DT.Columns.Add("LINE_PATH");
                    Final_DT.Columns.Add("POL_CD");
                    Final_DT.Columns.Add("POL_NM");
                    Final_DT.Columns.Add("POD_CD");
                    Final_DT.Columns.Add("POD_NM");
                    Final_DT.Columns.Add("ETD");
                    Final_DT.Columns.Add("ETD_HM");
                    Final_DT.Columns.Add("ETA");
                    Final_DT.Columns.Add("ETA_HM");
                    Final_DT.Columns.Add("VSL");
                    Final_DT.Columns.Add("VOY");
                    Final_DT.Columns.Add("DOC_CLOS_YMD");
                    Final_DT.Columns.Add("DOC_CLOS_HM");
                    Final_DT.Columns.Add("CARGO_CLOS_YMD");
                    Final_DT.Columns.Add("CARGO_CLOS_HM");
                    Final_DT.Columns.Add("SCH_PIC");
                    Final_DT.Columns.Add("RMK");
                    Final_DT.Columns.Add("FCL_CNT");
                    Final_DT.Columns.Add("LCL_CNT");
                    Final_DT.Columns.Add("CONSOL_CNT");
                    
                    //Total을 위해                    
                    int nCount = 0;
                    int nPage = Convert.ToInt32(dt.Rows[0]["PAGE"]);
                    int nTotalPage = 0;
                    
                    foreach (var item in query)
                    {
                        nCount++;
                    }
                    
                    nTotalPage = nCount;
                    nCount = 0;

                    foreach (var item in query)
                    {
                        if ((10 * nPage) <= nCount && (10 * (nPage + 1)) > nCount)
                        {
                            DataRow Final_Dr = Final_DT.NewRow();
                            Final_Dr["RNUM"] = (nCount+1);
                            Final_Dr["PAGE"] = (nPage+1);
                            Final_Dr["TOTCNT"] = nTotalPage;
                            Final_Dr["REQ_SVC"] = item.REQ_SVC;
                            Final_Dr["LINE_CD"] = item.LINE_CD;
                            Final_Dr["LINE_PATH"] = item.LINE_PATH;
                            Final_Dr["POL_CD"] = item.POL_CD;
                            Final_Dr["POL_NM"] = item.POL_NM;
                            Final_Dr["POD_CD"] = item.POD_CD;
                            Final_Dr["POD_NM"] = item.POD_NM;
                            Final_Dr["ETD"] = item.ETD;
                            Final_Dr["ETD_HM"] = item.ETD_HM;
                            Final_Dr["ETA"] = item.ETA;
                            Final_Dr["ETA_HM"] = item.ETA_HM;
                            Final_Dr["VSL"] = item.VSL;
                            Final_Dr["VOY"] = item.VOY;
                            Final_Dr["DOC_CLOS_YMD"] = item.DOC_CLOS_YMD;
                            Final_Dr["DOC_CLOS_HM"] = item.DOC_CLOS_HM;
                            Final_Dr["CARGO_CLOS_YMD"] = item.CARGO_CLOS_YMD;
                            Final_Dr["CARGO_CLOS_HM"] = item.CARGO_CLOS_HM;
                            Final_Dr["SCH_PIC"] = item.SCH_PIC;
                            Final_Dr["RMK"] = item.RMK;
                            Final_Dr["FCL_CNT"] = item.FCL_CNT;
                            Final_Dr["LCL_CNT"] = item.LCL_CNT;
                            Final_Dr["CONSOL_CNT"] = item.CONSOL_CNT;
                            Final_DT.Rows.Add(Final_Dr);
                        }
                    
                        nCount++;
                    }

                    if (Final_DT.Rows.Count == 0)
                    {
                        rtnJson = comm.MakeJson("N", "Fail");
                    }
                    else
                    {
                        rtnJson = comm.MakeJson("Y", "Success", Final_DT);
                    }
                    
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                string test = e.Message.ToString();

                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 스케줄 디테일 데이터 가져오기 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetSchDtlData(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_fnGetSchDtlData(dt.Rows[0]), CommandType.Text);                

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "Fail", Resultdt);
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "Success", Resultdt);
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 스케줄 거래처 데이터 가져오기 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetCustDtlData(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);
            DataSet ds = new DataSet();

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_fnGetCustData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Cust";
                ds.Tables.Add(Resultdt);

                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_fnGetCustCircleChart(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Circle";
                ds.Tables.Add(Resultdt);

                Resultdt = new DataTable();
                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_fnGetCustLineChart_Before(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Line_Before";
                ds.Tables.Add(Resultdt);

                Resultdt = new DataTable();
                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_fnGetCustLineChart_Now(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Line_Now";
                ds.Tables.Add(Resultdt);

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }
                else
                {
                    rtnJson = comm.DS_MakeJson("Y", "Success", ds);
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 스케줄 거래처 디테일 데이터 가져오기 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetLineChartData(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);
            DataSet ds = new DataSet();

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();                

                Resultdt = new DataTable();
                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_fnGetCustLineChart_Before(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Line_Before";
                ds.Tables.Add(Resultdt);

                Resultdt = new DataTable();
                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_fnGetCustLineChart_Now(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Line_Now";
                ds.Tables.Add(Resultdt);

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }
                else
                {
                    rtnJson = comm.DS_MakeJson("Y", "Success", ds);
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 스케줄 거래처 디테일 데이터 가져오기 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetVslData(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);
            DataSet ds = new DataSet();

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = new DataTable();
                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_fnGetCarrData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "CARR";
                ds.Tables.Add(Resultdt);

                Resultdt = new DataTable();
                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_fnGetVSLData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "VSL";
                ds.Tables.Add(Resultdt);

                rtnJson = comm.DS_MakeJson("Y", "Success", ds);

                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 라인 이미지가 있는지 체크하는 로직
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetLineImgPath(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_fnGetLineImgPath(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "LinePath";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "Fail", Resultdt);
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "Success", Resultdt);
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }
    }
}

