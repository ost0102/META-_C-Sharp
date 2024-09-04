using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ELVIS_META_COMMON.Query;
using ELVIS_META_COMMON.YJIT_Utils;
using ELVIS_META_DATA;
using Newtonsoft.Json;


namespace ELVIS_META_COMMON.Controller
{
    public class Con_Terminal
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        Container_Query CQ = new Container_Query();

        //전역 변수
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();
        DataSet ds = new DataSet();
        string rtnJson = "";
        string strValue = "";

        public string Con_fnGetPort()
        {

            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(CQ.fnPort_Query(), CommandType.Text);
                Resultdt.TableName = "PORT";

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

        public string Con_fnGetTerminal(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(CQ.Query_GetTerminalData(dt.Rows[0]), CommandType.Text);

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
    }
}
