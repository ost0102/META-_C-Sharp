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
    public class Con_Freight
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        Freight_Query FQ = new Freight_Query();

        //전역 변수
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();
        string rtnJson = "";

        /// <summary>
        /// 운임 - 포트 정보 가져오기
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

                Resultdt = DataHelper.ExecuteDataTable(FQ.Query_GetPortData(dt.Rows[0]), CommandType.Text);
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
        /// 운임 - 연도별 해상운임 추이 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_GetFreData(string strValue)
        {
            DataSet ds = new DataSet();
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(FQ.Query_GetFreLineBeforeData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Line_Before";
                ds.Tables.Add(Resultdt);

                Resultdt = DataHelper.ExecuteDataTable(FQ.Query_GetFreLineNowData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Line_Now";
                ds.Tables.Add(Resultdt);

                Resultdt = DataHelper.ExecuteDataTable(FQ.Query_GetFreCustData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Cust";
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
        /// 추천 포워더 비용 , 시간 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetRecomendCust(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(FQ.Query_GetFreCustData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Cust";

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
        /// 해상 운임 레이어 거래처 데이터 가져오기 
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

                Resultdt = DataHelper.ExecuteDataTable(FQ.Query_fnGetCustData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Cust";
                ds.Tables.Add(Resultdt);

                Resultdt = DataHelper.ExecuteDataTable(FQ.Query_fnGetCustCircleChart(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Circle";
                ds.Tables.Add(Resultdt);

                Resultdt = new DataTable();
                Resultdt = DataHelper.ExecuteDataTable(FQ.Query_fnGetCustLineChart_Before(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Line_Before";
                ds.Tables.Add(Resultdt);

                Resultdt = new DataTable();
                Resultdt = DataHelper.ExecuteDataTable(FQ.Query_fnGetCustLineChart_Now(dt.Rows[0]), CommandType.Text);
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
                Resultdt = DataHelper.ExecuteDataTable(FQ.Query_fnGetCustLineChart_Before(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Line_Before";
                ds.Tables.Add(Resultdt);

                Resultdt = new DataTable();
                Resultdt = DataHelper.ExecuteDataTable(FQ.Query_fnGetCustLineChart_Now(dt.Rows[0]), CommandType.Text);
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

    }
}
