using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ELVIS_META_COMMON.Query;
using ELVIS_META_COMMON.YJIT_Utils;
using ELVIS_META_DATA;

namespace ELVIS_META_COMMON.Controller
{
    public class Con_Main
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        Main_Query MQ = new Main_Query();

        //전역 변수
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();
        DataSet ds = new DataSet();
        string rtnJson = "";

        /// <summary>
        /// 메인 - 로그인
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnLogin(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(MQ.fnLogin_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Table";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "아이디 혹은 비밀번호가 틀렸습니다.", Resultdt);                    
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
        /// CRN이 있는지 확인하는 함수
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnCheckID(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(MQ.fnCheckID_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Data";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("Y", "Success", Resultdt);                    
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "아이디가 중복 됩니다.", Resultdt);
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
        /// CRN이 있는지 확인하는 함수
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnCheckCRN(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataHelper.ConnectionString_Select = "CRM";

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = DataHelper.ExecuteDataTable(MQ.fnGetCRN_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Data";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "포워더 사업자가 없음.", Resultdt);                    
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "사업자 번호가 있음.", Resultdt);
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
        /// 회원가입
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnRegister(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);
            string strMNGT_NO = "";
            int nResult;
            
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            
            try
            {
                //twkim - Domain 데이터 가져오기 20211202 추가 (김은규 과장님 요청)
                DataHelper.ConnectionString_Select = "CRM";
                Resultdt = DataHelper.ExecuteDataTable(MQ.fnGetDomain_Query(dt.Rows[0]), CommandType.Text);
                dt.Columns.Add("DOMAIN");

                //예외처리
                if(Resultdt != null)
                {
                    dt.Rows[0]["DOMAIN"] = Resultdt.Rows[0]["DOMAIN"];
                }
                else
                {
                    dt.Rows[0]["DOMAIN"] = "";
                }

                DataHelper.ConnectionString_Select = "PRIME";

                //관리번호 채번
                strMNGT_NO = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
                dt.Columns.Add("MNGT_NO");
                dt.Rows[0]["MNGT_NO"] = strMNGT_NO;

                nResult = DataHelper.ExecuteNonQuery(MQ.fnRegister_Query(dt.Rows[0]), CommandType.Text);

                if(nResult == 0)
                {
                    rtnJson = comm.MakeJson("N", "회원 가입이 되지 않았습니다.");
                }
                else
                {
                    string[] stringArray = {"SCH", "UNI", "SFI", "TER", "TRK", "LAT" };

                    for(int i = 0; i<stringArray.Length; i++)
                    {
                        DataHelper.ExecuteNonQuery(MQ.fnRegister_DTL_Query(dt.Rows[0], stringArray[i]), CommandType.Text);
                    }

                    DataHelper.ConnectionString_Select = "CRM";
                    //온라인 접수 넣기
                    DataHelper.ExecuteNonQuery(MQ.fnCrmRegMst_Query(dt.Rows[0]), CommandType.Text);
                    DataHelper.ExecuteNonQuery(MQ.fnCrmRegCust_Query(dt.Rows[0]), CommandType.Text);

                    rtnJson = comm.MakeJson("Y", "회원 가입 되었습니다.");
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
