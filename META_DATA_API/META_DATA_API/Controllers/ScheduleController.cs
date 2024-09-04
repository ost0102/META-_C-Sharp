using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json;
using META_DATA_API.Models;
using META_DATA_API.Models.Query;


namespace META_DATA_API.Controllers
{
    public class ScheduleController : ApiController
    {
        private bool ErrorOccur = false;
        public string ReturnJsonVal;
        private string strProcLog = "";
        private DataTable dt = new DataTable();

        /// <summary>
        /// Get Shcedule List by Json
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="REQ_SVC">해운/항공 구분</param>
        /// <param name="LINE_CD">선사 코드</param>
        /// <param name="ST_DATE">ETD 시작일</param>
        /// <param name="END_DATE">ETD 종료일</param>
        /// <param name="PAGE">PAGE</param>
        /// <returns>DataTable by Schedule</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetSchedule")]        
        public string GetSchedule(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Req_Svc = "";
            string Param_Line_Cd = "";
            string Param_Start_Date = "";
            string Param_End_date = "";
            string Param_PageCount = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("REQ_SVC"))
                        {
                            Param_Req_Svc = dt.Rows[0]["REQ_SVC"].ToString();
                            if (string.IsNullOrEmpty(Param_Req_Svc)) _common.ThrowMsg(ErrorOccur, "REQ_SVC is Empty");
                        }
                        else 
                        {
                            _common.ThrowMsg(ErrorOccur, "REQ_SVC Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("LINE_CD"))
                        { 
                            Param_Line_Cd = dt.Rows[0]["LINE_CD"].ToString();
                            if (string.IsNullOrEmpty(Param_Line_Cd)) _common.ThrowMsg(ErrorOccur, "LINE_CD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "LINE_CD Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("ST_DATE"))
                        {
                            Param_Start_Date = dt.Rows[0]["ST_DATE"].ToString();
                            if (string.IsNullOrEmpty(Param_Start_Date)) _common.ThrowMsg(ErrorOccur, "ST_DATE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "ST_DATE Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("END_DATE"))
                        {
                            Param_End_date = dt.Rows[0]["END_DATE"].ToString();
                            if (string.IsNullOrEmpty(Param_End_date)) _common.ThrowMsg(ErrorOccur, "END_DATE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "END_DATE Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("PAGE"))
                        {
                            Param_PageCount = dt.Rows[0]["PAGE"].ToString();
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "PAGE Parameter is Null");
                        }
                        #endregion

                        dt = Sql_Schedule.Get_Schedule(Param_Req_Svc, Param_Line_Cd, Param_Start_Date, Param_End_date, Param_PageCount);
                        ReturnJsonVal = _common.MakeJson("Y", "Success", dt);

                        //Log 정보 저장
                        strProcLog = "[GetSchedule-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                }
                else
                {
                    //Json Decrypt Error 
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }                
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message, dt);
                strProcLog = "[GetSchedule-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }
            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Shcedule List by Json
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="REQ_SVC">해운/항공 구분</param>        
        /// <param name="POL_CD">출발지 포트 코드</param>        
        /// <param name="POD_CD">도착지 포트 코드</param>        
        /// <param name="ST_DATE">ETD 시작일</param>
        /// <param name="END_DATE">ETD 종료일</param>
        /// <param name="LINE_CD">선사 코드</param>        
        /// <returns>DataTable by Schedule</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetSchedule_Port")]
        public string GetSchedule_Port(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Req_Svc = "";            
            string Param_Pol_Cd = "";
            string Param_Pod_Cd = "";
            string Param_Start_Date = "";
            string Param_End_date = "";
            string Param_Line_Cd = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("REQ_SVC"))
                        {
                            Param_Req_Svc = dt.Rows[0]["REQ_SVC"].ToString();
                            if (string.IsNullOrEmpty(Param_Req_Svc)) _common.ThrowMsg(ErrorOccur, "REQ_SVC is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "REQ_SVC Parameter is Null");
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
                        if (dt.Rows[0].Table.Columns.Contains("ST_DATE"))
                        {
                            Param_Start_Date = dt.Rows[0]["ST_DATE"].ToString();
                            if (string.IsNullOrEmpty(Param_Start_Date)) _common.ThrowMsg(ErrorOccur, "ST_DATE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "ST_DATE Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("END_DATE"))
                        {
                            Param_End_date = dt.Rows[0]["END_DATE"].ToString();
                            if (string.IsNullOrEmpty(Param_End_date)) _common.ThrowMsg(ErrorOccur, "END_DATE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "END_DATE Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("LINE_CD")) Param_Line_Cd = dt.Rows[0]["LINE_CD"].ToString();                                                    
                        #endregion

                        dt = Sql_Schedule.Get_Schedule_Port(Param_Req_Svc, Param_Pol_Cd, Param_Pod_Cd, Param_Start_Date, Param_End_date, Param_Line_Cd);
                        ReturnJsonVal = _common.MakeJson("Y", "Success", dt);

                        //Log 정보 저장
                        strProcLog = "[GetScheduleByPort-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                }
                else
                {
                    //Json Decrypt Error 
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message, dt);
                strProcLog = "[GetScheduleByPort-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }
            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Port List by Json
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="REQ_SVC">해운/항공 구분</param>        
        /// <param name="LOC_CD">출발지 포트 코드</param>        
        /// <returns>DataTable by Schedule</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetPortList")]
        public string GetPortList(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Req_Svc = "";
            string Param_Loc_Cd = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("REQ_SVC"))
                        {
                            Param_Req_Svc = dt.Rows[0]["REQ_SVC"].ToString();
                            if (string.IsNullOrEmpty(Param_Req_Svc)) _common.ThrowMsg(ErrorOccur, "REQ_SVC is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "REQ_SVC Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("LOC_CD"))
                        {
                            Param_Loc_Cd = dt.Rows[0]["LOC_CD"].ToString();
                            if (string.IsNullOrEmpty(Param_Loc_Cd)) _common.ThrowMsg(ErrorOccur, "LOC_CD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "LOC_CD Parameter is Null");
                        }
                        #endregion

                        dt = Sql_Schedule.Get_Port_List(Param_Req_Svc, Param_Loc_Cd);
                        ReturnJsonVal = _common.MakeJson("Y", "Success", dt);

                        //Log 정보 저장
                        strProcLog = "[GetPortList-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
                        _LogWriter.Writer(strProcLog);
                    }
                }
                else
                {
                    //Json Decrypt Error 
                    _common.ThrowMsg(ErrorOccur, "Json Parameter is Null");
                }
            }
            catch (Exception ex)
            {
                ReturnJsonVal = _common.MakeJson("E", ex.Message, dt);
                strProcLog = "[GetPortList-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }
            return ReturnJsonVal;
        }
    }
}