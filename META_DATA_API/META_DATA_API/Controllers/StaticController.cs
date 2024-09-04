using System;
using System.Data;
using System.Web.Http;
using System.Net.Mail;
using System.Collections.Generic;

using Newtonsoft.Json;
using META_DATA_API.Models;
using META_DATA_API.Models.Query;

namespace META_DATA_API.Controllers
{
    public class StaticController : ApiController
    {
        private bool ErrorOccur = false;
        public string ReturnJsonVal;
        private string strProcLog = "";
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();

        /// <summary>
        /// Get Volume Data By Country
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="DATE_YYYY">년도*</param>
        /// <param name="DATE_MM">월*</param>
        /// <param name="EX_IM_TYPE">수출입*</param>
        /// <param name="PORT_CD">Port Code</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetCtryVolume")]
        public string GetCtryVolume(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Date_Yyyy = "";
            string Param_Date_Mm = "";
            string Param_Ex_Im_Type = "";
            string Param_Port_Cd = "";
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("DATE_YYYY"))
                        {
                            Param_Date_Yyyy = dt.Rows[0]["DATE_YYYY"].ToString();
                            if (string.IsNullOrEmpty(Param_Date_Yyyy)) _common.ThrowMsg(ErrorOccur, "DATE_YYYY is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "DATE_YYYY Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("DATE_MM"))
                        {
                            Param_Date_Mm = dt.Rows[0]["DATE_MM"].ToString();
                            if (string.IsNullOrEmpty(Param_Date_Mm)) _common.ThrowMsg(ErrorOccur, "DATE_MM is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "DATE_MM Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("EX_IM_TYPE"))
                        {
                            Param_Ex_Im_Type = dt.Rows[0]["EX_IM_TYPE"].ToString();
                            if (string.IsNullOrEmpty(Param_Ex_Im_Type)) _common.ThrowMsg(ErrorOccur, "EX_IM_TYPE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "EX_IM_TYPE Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("PORT_CD")) Param_Port_Cd = dt.Rows[0]["PORT_CD"].ToString();
                        #endregion

                        ds = Sql_Static.GetCtryVolume(Param_Date_Yyyy, Param_Date_Mm, Param_Ex_Im_Type, Param_Port_Cd);

                        if (ds.Tables["TOTAL"].Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", ds);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");
                        

                        //Log 정보 저장
                        strProcLog = "[GetCtryVolume ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetCtryVolume-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Frt Charge Data
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="DATE_YYYY">년도*</param>
        /// <param name="POL_CD">출발지*</param>
        /// <param name="POD_CD">도착지*</param>
        /// <param name="CNTR_SIZE">컨테이너타입*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetFrtCharge")]
        public string GetFrtCharge(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Date_Yyyy = "";
            string Param_Pol_Cd = "";
            string Param_Pod_Cd = "";
            string Param_Cntr_Size = "";             
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);
                        
                        #region // Validation Check & Save Variable
                        if (dt.Rows[0].Table.Columns.Contains("DATE_YYYY"))
                        {
                            Param_Date_Yyyy = dt.Rows[0]["DATE_YYYY"].ToString();
                            if (string.IsNullOrEmpty(Param_Date_Yyyy)) _common.ThrowMsg(ErrorOccur, "DATE_YYYY is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "DATE_YYYY Parameter is Null");
                        }
                                                
                        if (dt.Rows[0].Table.Columns.Contains("POL_CD"))
                        {
                            Param_Pol_Cd = dt.Rows[0]["POL_CD"].ToString();
                            if (string.IsNullOrEmpty(Param_Pol_Cd)) _common.ThrowMsg(ErrorOccur, "POL_CD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "POL_CD Parameter is Null");
                        }
                        

                        if (dt.Rows[0].Table.Columns.Contains("POD_CD"))
                        {
                            Param_Pod_Cd = dt.Rows[0]["POD_CD"].ToString();
                            if (string.IsNullOrEmpty(Param_Pod_Cd)) _common.ThrowMsg(ErrorOccur, "POD_CD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "POD_CD Parameter is Null");
                        }

                        if (dt.Rows[0].Table.Columns.Contains("CNTR_SIZE"))
                        {
                            Param_Cntr_Size = dt.Rows[0]["CNTR_SIZE"].ToString();
                            if (string.IsNullOrEmpty(Param_Cntr_Size)) _common.ThrowMsg(ErrorOccur, "CNTR_SIZE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CNTR_SIZE Parameter is Null");
                        }
                        #endregion

                        dt = Sql_Static.GetFrtCharge(Param_Date_Yyyy, Param_Pol_Cd, Param_Pod_Cd, Param_Cntr_Size);

                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");

                        //Log 정보 저장
                        strProcLog = "[GetFrtCharge ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetFrtCharge-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Item Analyze Data
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="ITEM_NM">품목명*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetItemAnalyze")]
        public string GetItemAnalyze(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Item_Nm = "";
            #endregion

            try
            {
                if (reqVal != null)
                {
                    string rtnVal = reqVal.ToString();
                    if (!string.IsNullOrEmpty(rtnVal))
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(rtnVal);

                        #region // Validation Check & Save Variable
                        if (dt.Columns.Contains("ITEM_NM"))
                        {
                            Param_Item_Nm = dt.Rows[0]["ITEM_NM"].ToString();
                            if (string.IsNullOrEmpty(Param_Item_Nm)) _common.ThrowMsg(ErrorOccur, "ITEM_NM is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "ITEM_NM Parameter is Null");
                        }
                        #endregion

                        dt = Sql_Static.GetItemAnalyze(Param_Item_Nm);

                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not found");

                        //Log 정보 저장
                        strProcLog = "[GetItemAnalyze ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                    else
                    {
                        _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                    }
                }
                else
                {
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message);
                strProcLog = "[GetItemAnalyze-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }
    }
}
