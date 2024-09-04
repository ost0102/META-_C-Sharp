using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ELVIS_META_COMMON.Query;
using ELVIS_META_COMMON.YJIT_Utils;
using ELVIS_META_DATA;


namespace ELVIS_META_COMMON.Controller
{
    public class Con_Sub
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        Sub_Query SQ = new Sub_Query();

        //전역 변수
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();
        DataSet ds = new DataSet();
        string rtnJson = "";
        string strValue = "";

        public string Con_fnGetTotal(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_GetPortData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "PORT";
                ds.Tables.Add(Resultdt);

                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_GetTotalData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "TOTAL";
                ds.Tables.Add(Resultdt);

                dt = new DataTable();
                dt.Columns.Add("trxCode");
                dt.Columns.Add("trxMsg");
                DataRow row = dt.NewRow();
                row["trxCode"] = "Y";
                row["trxMsg"] = "Success";
                dt.Rows.Add(row);
                dt.TableName = "Result";
                ds.Tables.Add(dt);

                strValue = JsonConvert.SerializeObject(ds, Formatting.Indented);
                rtnJson = String_Encrypt.encryptAES256(strValue);

                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }



        public string Con_fnGetDashboard(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(SQ.Query_GetTotalData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "TOTAL";

                rtnJson = comm.MakeJson("Y", "Success", Resultdt);

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
        /// 트레킹 검색 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetTracking(string strValue)
        {

            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                string strResult = String_Encrypt.decryptAES256(strValue);

                //데이터
                dt = JsonConvert.DeserializeObject<DataTable>(strResult);
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(SQ.fnTracking_Query(dt.Rows[0]), CommandType.Text);

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "Fail");
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
