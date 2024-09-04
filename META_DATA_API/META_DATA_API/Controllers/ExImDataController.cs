using System;
using System.Data;
using System.Web.Http;
using System.Collections.Generic;

using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using META_DATA_API.Models;
using META_DATA_API.Models.Query;

namespace META_DATA_API.Controllers
{
    public class ExImDataController : ApiController
    {
        private bool ErrorOccur = false;
        public string ReturnJsonVal;
        private string strProcLog = "";
        private DataTable dt = new DataTable();

        /// <summary>
        /// Set B/L Info Data From ELVIS
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="HBL_NO">HBL번호*</param>
        /// <param name="EX_IM_TYPE">수출입*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SetBLInfo")]
        public string SetBLInfo(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Hbl_No = "";
            string Param_Ex_Im_Type = "";

            bool rtnStatus = false;
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
                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_Crn = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("HBL_NO"))
                        {
                            Param_Hbl_No = dt.Rows[0]["HBL_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Hbl_No)) _common.ThrowMsg(ErrorOccur, "HBL_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "HBL_NO Parameter is Null");
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
                        #endregion

                        rtnStatus = Sql_ExImData.SetBLInfo(Param_Crn, Param_Hbl_No, Param_Ex_Im_Type, dt.Rows[0]);

                        if (rtnStatus) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Failed");

                        //MongoDB Send
                        rtnStatus = SendMongoDB(dt.Rows[0]);

                        if (rtnStatus)
                        {
                            strProcLog = "[SendMongoDB-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Success :   " + System.Environment.NewLine;
                            _LogWriter.Writer(strProcLog);
                        }

                        //Log 정보 저장
                        strProcLog = "[SetBLInfo ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[SetBLInfo-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Set Document Path Data
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="MNGT_NO">문서 관리번호*</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="FILE_NM">FILE 명*</param>
        /// <param name="SVR_FILE_NM">SERVER FILE 명*</param>
        /// <param name="FILE_SIZE">FILE 크기*</param>
        /// <param name="FILE_EXT">FILE 확장자*</param>
        /// <param name="FILE_PATH">FILE PATH*</param>
        /// <param name="DOC_TYPE">문서 타입*</param>
        /// <param name="ORG_MNGT_NO">원본번호*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("SetDocPath")]
        public string SetDocPath(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Mngt_no = "";
            string Param_Crn = "";
            string Param_Doc_Type = "";
            string Param_Svr_File_Nm = "";
            string Param_File_Nm = "";
            string Param_File_Size = "";
            string Param_File_Path = "";
            string Param_File_Ext = "";
            string Param_Org_Mngt_No = "";

            bool rtnStatus = false;
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
                        if (dt.Rows[0].Table.Columns.Contains("MNGT_NO"))
                        {
                            Param_Mngt_no = dt.Rows[0]["MNGT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Mngt_no)) _common.ThrowMsg(ErrorOccur, "MNGT_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "MNGT_NO Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_Crn = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("SVR_FILE_NM"))
                        {
                            Param_Svr_File_Nm = dt.Rows[0]["SVR_FILE_NM"].ToString();
                            if (string.IsNullOrEmpty(Param_Svr_File_Nm)) _common.ThrowMsg(ErrorOccur, "SVR_FILE_NM is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "SVR_FILE_NM Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("FILE_NM"))
                        {
                            Param_File_Nm = dt.Rows[0]["FILE_NM"].ToString();
                            if (string.IsNullOrEmpty(Param_File_Nm)) _common.ThrowMsg(ErrorOccur, "FILE_NM is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "FILE_NM Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("FILE_SIZE"))
                        {
                            Param_File_Size = dt.Rows[0]["FILE_SIZE"].ToString();
                            if (string.IsNullOrEmpty(Param_File_Size)) _common.ThrowMsg(ErrorOccur, "FILE_SIZE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "FILE_SIZE Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("FILE_EXT"))
                        {
                            Param_File_Ext = dt.Rows[0]["FILE_EXT"].ToString();
                            if (string.IsNullOrEmpty(Param_File_Ext)) _common.ThrowMsg(ErrorOccur, "FILE_EXT is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "FILE_EXT Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("FILE_PATH"))
                        {
                            Param_File_Path = dt.Rows[0]["FILE_PATH"].ToString();
                            if (string.IsNullOrEmpty(Param_File_Path)) _common.ThrowMsg(ErrorOccur, "FILE_PATH is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "FILE_PATH Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("DOC_TYPE"))
                        {
                            Param_Doc_Type = dt.Rows[0]["DOC_TYPE"].ToString();
                            if (string.IsNullOrEmpty(Param_Doc_Type)) _common.ThrowMsg(ErrorOccur, "DOC_TYPE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "DOC_TYPE Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("ORG_MNGT_NO"))
                        {
                            Param_Org_Mngt_No = dt.Rows[0]["ORG_MNGT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Doc_Type)) _common.ThrowMsg(ErrorOccur, "ORG_MNGT_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "ORG_MNGT_NO Parameter is Null");
                        }
                        #endregion

                        rtnStatus = Sql_ExImData.SetDocPath(Param_Mngt_no, Param_Crn, Param_Svr_File_Nm, Param_File_Size, Param_File_Nm, Param_File_Ext, Param_File_Path, Param_Doc_Type, Param_Org_Mngt_No);

                        if (rtnStatus) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Failed");

                        //Log 정보 저장
                        strProcLog = "[SetDocPath ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[SetDocPath-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get B/L List Data
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="EX_IM_TYPE">수출입*</param>
        /// <param name="YMD_TYPE">ETD/ETA*</param>
        /// <param name="FM_YMD">조회조건 시작일자*</param>
        /// <param name="TO_YMD">조회조건 종료일자*</param>
        /// <param name="REQ_SVC">해운/항공</param>
        /// <param name="POL_CD">출발지</param>
        /// <param name="POD_CD">도착지</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetExImList")]
        public string GetExImList(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Crn = "";
            string Param_Ex_Im_Type = "";
            string Param_Ymd_Type = "";
            string Param_Fm_Ymd = "";
            string Param_To_Ymd = "";
            string Param_Req_Svc = "";
            string Param_Pol_Cd = "";
            string Param_Pod_Cd = "";
            string Param_Usr_Tpye = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_Crn = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
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
                        if (dt.Rows[0].Table.Columns.Contains("YMD_TYPE"))
                        {
                            Param_Ymd_Type = dt.Rows[0]["YMD_TYPE"].ToString();
                            if (string.IsNullOrEmpty(Param_Ymd_Type)) _common.ThrowMsg(ErrorOccur, "YMD_TYPE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "YMD_TYPE Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("FM_YMD"))
                        {
                            Param_Fm_Ymd = dt.Rows[0]["FM_YMD"].ToString();
                            if (string.IsNullOrEmpty(Param_Fm_Ymd)) _common.ThrowMsg(ErrorOccur, "FM_YMD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "FM_YMD Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("TO_YMD"))
                        {
                            Param_To_Ymd = dt.Rows[0]["TO_YMD"].ToString();
                            if (string.IsNullOrEmpty(Param_To_Ymd)) _common.ThrowMsg(ErrorOccur, "TO_YMD is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "TO_YMD Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("USR_TYPE"))
                        {
                            Param_Usr_Tpye = dt.Rows[0]["USR_TYPE"].ToString();
                            if (string.IsNullOrEmpty(Param_Usr_Tpye)) _common.ThrowMsg(ErrorOccur, "USR_TYPE is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "USR_TYPE Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("REQ_SVC")) Param_Req_Svc = dt.Rows[0]["REQ_SVC"].ToString();
                        if (dt.Rows[0].Table.Columns.Contains("POL_CD")) Param_Pol_Cd = dt.Rows[0]["POL_CD"].ToString();
                        if (dt.Rows[0].Table.Columns.Contains("POD_CD")) Param_Pod_Cd = dt.Rows[0]["POD_CD"].ToString();
                        #endregion

                        dt = Sql_ExImData.GetExImList(Param_Crn, Param_Ex_Im_Type, Param_Ymd_Type, Param_Fm_Ymd, Param_To_Ymd, Param_Req_Svc, Param_Pol_Cd, Param_Pod_Cd, Param_Usr_Tpye);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data Not Found!");
                        
                        //Log 정보 저장
                        strProcLog = "[GetExImList ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetExImList-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Document Path Data
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="MNGT_NO">관리번호*</param>
        /// <param name="CRN">사업자등록번호*</param>
        /// <param name="DOC_TYPE">문서타입</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetExImDocPath")]
        public string GetExImDocPath(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Mngt_No = "";
            string Param_Crn = "";
            string Param_Doc_Type = "";            
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
                        if (dt.Rows[0].Table.Columns.Contains("MNGT_NO"))
                        {
                            Param_Mngt_No = dt.Rows[0]["MNGT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Mngt_No)) _common.ThrowMsg(ErrorOccur, "MNGT_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "MNGT_NO Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("CRN"))
                        {
                            Param_Crn = dt.Rows[0]["CRN"].ToString();
                            if (string.IsNullOrEmpty(Param_Crn)) _common.ThrowMsg(ErrorOccur, "CRN is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "CRN Parameter is Null");
                        }
                        if (dt.Rows[0].Table.Columns.Contains("DOC_TYPE")) Param_Doc_Type = dt.Rows[0]["DOC_TYPE"].ToString();                        
                        #endregion

                        dt = Sql_ExImData.GetDocPath(Param_Mngt_No, Param_Crn, Param_Doc_Type);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not exist");

                        //Log 정보 저장
                        strProcLog = "[GetExImDocPath ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetExImDocPath-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        /// <summary>
        /// Get Tracking List Data
        /// </summary>
        /// <param name="reqVal">JSON 암호화 파라미터 -하위 파라미터 참조</param>
        /// <param name="MNGT_NO">조회번호*</param>
        /// 
        /// <returns>Result Data (Boolean)</returns>
        [Authorize]
        [AcceptVerbs("POST", "GET")]
        [ActionName("GetTrackingList")]
        public string GetTrackingList(object reqVal)
        {
            #region // 변수 선언 영역
            string Param_Mngt_No = "";
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
                        if (dt.Rows[0].Table.Columns.Contains("MNGT_NO"))
                        {
                            Param_Mngt_No = dt.Rows[0]["MNGT_NO"].ToString();
                            if (string.IsNullOrEmpty(Param_Mngt_No)) _common.ThrowMsg(ErrorOccur, "MNGT_NO is Empty");
                        }
                        else
                        {
                            _common.ThrowMsg(ErrorOccur, "MNGT_NO Parameter is Null");
                        }
                        #endregion

                        dt = Sql_ExImData.GetTrackingList(Param_Mngt_No);
                        if (dt.Rows.Count > 0) ReturnJsonVal = _common.MakeJson("Y", "Success", dt);
                        else ReturnJsonVal = _common.MakeJson("N", "Data not Found");

                        //Log 정보 저장
                        strProcLog = "[GetTrackingList ----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Parameter:   " + rtnVal + System.Environment.NewLine;
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
                strProcLog = "[GetTrackingList-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }

            return ReturnJsonVal;
        }

        private bool SendMongoDB(DataRow dr)
        {
            try
            {
                string[] DTArr = { "BL", "INV", "MFCS", "FWB", "AMS", "AFR", "DCD", "DO", "TAX" };
                MongoClient mgClient = new MongoClient("mongodb://friend:yjit1234!@bga9v.pub-vpc.mg.naverncp.com:17017/friendDB?authSource=friendDB&readPreference=primary&directConnection=true&tls=false");
                
                var mgDatabase = mgClient.GetDatabase("friendDB");
                var mgCollection = mgDatabase.GetCollection<BsonDocument>("FriendHblStatus");
                BsonDocument dic = new BsonDocument();

                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    dic.Add(dr.Table.Columns[i].ColumnName, dr[i].ToString());
                }

                // 해당 B/L 존재하면 DELETE, INSERT
                var filter = Builders<BsonDocument>.Filter.And(
                                Builders<BsonDocument>.Filter.Eq("HBL_NO", dr["HBL_NO"].ToString()),
                                Builders<BsonDocument>.Filter.Eq("CRN", dr["CRN"].ToString()));
                var docCnt = mgCollection.Find(filter).CountDocuments();

                if (docCnt > 0)
                {
                    var hblList = mgCollection.Find(filter).First();
                    var jsonStr = hblList.ToString().Replace("ObjectId(", "").Replace("\")", "\"");
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>("[" + jsonStr + "]");

                    for (int i = 0; i < DTArr.Length; i++)
                    {
                        string colNm = DTArr[i] + "_SEND_DT";
                        if (!string.IsNullOrEmpty(dt.Rows[0][colNm].ToString()) && string.IsNullOrEmpty(dr[colNm].ToString()))
                        {
                            dic[colNm] = dt.Rows[i][colNm].ToString();
                        }
                    }

                    filter = Builders<BsonDocument>.Filter.And(
                        Builders<BsonDocument>.Filter.Eq("HBL_NO", dr["HBL_NO"].ToString()),
                        Builders<BsonDocument>.Filter.Eq("CRN", dr["CRN"].ToString()));
                    var rtnrsult = mgCollection.DeleteOne(filter);
                }

                mgCollection.InsertOne(dic);
            }
            catch (Exception ex)
            {
                strProcLog = "[SendMongoDB-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]    Exception :   " + ex.Message + System.Environment.NewLine;
                _LogWriter.Writer(strProcLog);
            }            

            return true;
        }

    }
}