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
    public class Con_Tracking
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        Tracking_Query TQ = new Tracking_Query();

        //전역 변수
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();
        DataSet ds = new DataSet();
        string rtnJson = "";
        string strValue = "";

        public string Con_fnGetTracking(string strValue)
        {

            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                string strResult = String_Encrypt.decryptAES256(strValue);

                //데이터
                dt = JsonConvert.DeserializeObject<DataTable>(strResult);
                DataTable HBLdt = new DataTable();
                DataTable Resultdt = new DataTable();

                //HBL_NO 있는지 체크
                //HBLdt = DataHelper.ExecuteDataTable(TQ.fnGetHBLNO(dt.Rows[0]), CommandType.Text);

                //HBL_NO로만 검색
                //Resultdt = DataHelper.ExecuteDataTable(TQ.fnTracking_Query(HBLdt.Rows[0]["HBL_NO"].ToString(), HBLdt.Rows[0]["EX_IM_TYPE"].ToString()), CommandType.Text);

                //기존에 하던거
                Resultdt = DataHelper.ExecuteDataTable(TQ.fnTracking_Query(dt.Rows[0]), CommandType.Text);

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

        public string Con_fnGetVslData(string strValue)
        {

            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                string strResult = String_Encrypt.decryptAES256(strValue);

                //데이터
                dt = JsonConvert.DeserializeObject<DataTable>(strResult);
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(TQ.fnGetVslData_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "VSLINFO";

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
