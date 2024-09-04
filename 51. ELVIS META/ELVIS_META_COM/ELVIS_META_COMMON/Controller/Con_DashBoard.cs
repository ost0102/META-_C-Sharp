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
    public class Con_DashBoard
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        DashBoard_Query DQ = new DashBoard_Query();

        //전역 변수
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();
        string rtnJson = "";

        /// <summary>
        /// 선적자동화 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetDashBoardData(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);
        
            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";
        
            try
            {
                DataTable Resultdt = new DataTable();
        
                Resultdt = DataHelper.ExecuteDataTable(DQ.Query_fnGetDashBoardData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "DashBoard";
        
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
