////////////////////////전역 변수//////////////////////////
////var _vSelectDate = new Date();
////var obj = new Object();
////var mymap;
////var chkfirst = true;
////////////////////////jquery event///////////////////////
////$(function () {

////    //로그인 하지 않고 들어왔을때
////    if (_fnToNull($("#Session_USR_ID").val()) == "") {
////        location.href = window.location.origin;
////    }

////    $(".sub_esti").addClass("on");
////    $(".sub_esti .sub_depth").addClass("on");
////    $(".sub_esti .sub_depth li:nth-child(2) a").addClass("on");

////    if (_fnToNull($("#QUOT_NO").val()) != "") {
////        fnSearchQuotation();
////    } else {
////        location.href = window.location.origin + "/Estimate/Inquiry";
////    }


////});

//////등록자료내역 C/I P/L 클릭
////$(document).on("click", "#FILE_LIST .req-info__desc", function () {
////    $("#FILE_LIST .req-info__desc").removeClass("on");
////    $(this).closest(".req-info__desc").addClass("on");
////})

////$(".switch_label").click(function (e) {
////    fnSearchQuotation();
////});

//////function fnSearchQuotation() {
//////    var vHtml = "";
//////    var objJsonData = new Object();

//////    objJsonData.QUOT_NO = _fnToNull($("#QUOT_NO").val());
//////    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
//////    objJsonData.CRN = _fnToNull($("#Session_CRN").val());
//////    objJsonData.ORDER_TYPE = $("input[name='bookingvalue']:checked").val();
    
//////    var mngt_no = "";
//////    var v_crn = "";
//////    var v_pol = "";
//////    var v_pod = "";
//////    var v_vsl = "";
//////    var v_etd = "";
//////    var v_eta = "";
//////    var v_req_svc= "";
//////    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetQuotationDetail", objJsonData);
//////    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
//////        $(".bk-list").empty();
//////        var vResult = JSON.parse(rtnVal).LIST;
//////        if (vResult.length > 0) {
//////            $.each(vResult, function (i) {
//////                mngt_no = _fnToNull(vResult[i].MNGT_NO);
//////                v_pol = _fnToNull(vResult[i].POL_NM);
//////                v_pod = _fnToNull(vResult[i].POD_NM);
//////                v_vsl = _fnToNull(vResult[i].VSL);
//////                v_etd = _fnToNull(vResult[i].ETD);
//////                v_eta = _fnToNull(vResult[i].ETA);
//////                v_crn = _fnToNull(vResult[i].CRN);
//////                v_req_svc = _fnToNull(vResult[i].REQ_SVC);
//////                var curr = _fnToNull(vResult[i].CURR_CD).split(",");
//////                var fare_loc_amt = _fnToNull(vResult[i].FARE_LOC_AMT).split(",");
//////                var fare_amt = _fnToNull(vResult[i].FARE_AMT).split(",");
                
//////                vHtml += " <div class='bk-list__cont'>	";
//////                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].MNGT_NO) + "</p>";
//////                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].CRN) + "</p>";
//////                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].POL_NM) + "</p>";
//////                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].POD_NM) + "</p>";
//////                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].VSL) + "</p>";
//////                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].ETD) + "</p>";
//////                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].ETA) + "</p>";
//////                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].REQ_SVC) + "</p>";
//////                if (_fnToNull(vResult[i].REQ_SVC) == "SEA") {
//////                    if (i == 0) {
//////                        vHtml += "        <div class='bk-cont ship on'> ";
//////                    } else {
//////                        vHtml += "        <div class='bk-cont ship'> ";
//////                    }
//////                } else {
//////                    if (i == 0) {
//////                        vHtml += "        <div class='bk-cont air on'> ";
//////                    } else {
//////                        vHtml += "        <div class='bk-cont air'> ";
//////                    }
//////                }
//////                vHtml += "            <div class='bk-cont__img'>";
//////                vHtml += "            </div>";
//////                vHtml += "            <div class='bk-cont__desc'>";
//////                vHtml += "                <div class='bk-desc'>";
//////                vHtml += "                    <div class='bk-desc__inner'>";
//////                vHtml += "                        <p class='bk-desc__title'>상호명</p>";
//////                vHtml += "                        <p>" + _fnToNull(vResult[i].FWD_OFFICE_NM) + "</p>";
//////                vHtml += "                    </div>";
//////                vHtml += "                    <div class='bk-desc__inner'>";
//////                vHtml += "                        <p class='bk-desc__title'>담당자</p>";
//////                vHtml += "                        <p>" + _fnToNull(vResult[i].FWD_PIC_NM) + "</p>";
//////                vHtml += "                    </div>";
//////                vHtml += "                </div>";
//////                vHtml += "                <div class='bk-desc'>";
//////                for (var j = 0; j < curr.length; j++) {
//////                    vHtml += "                    <div class='bk-desc__inner'>";
//////                    vHtml += "                        <p class='bk-desc__title'>" + _fnToNull(curr[j]) + "</p>";
//////                    if (_fnToNull(curr[j]) == "KRW") {
//////                        vHtml += "                        <p>" + _fnToNull(_fnGetNumber(parseInt(fare_loc_amt[j]), "sum")) + "</p>";
//////                    } else {
//////                        vHtml += "                        <p>" + _fnToNull(_fnGetNumber(parseInt(fare_amt[j]), "sum")) + "</p>";
//////                    }
//////                    vHtml += "                    </div>";
//////                }
//////                vHtml += "                </div>";
//////                vHtml += "            </div>";
//////                vHtml += "            <div class='bk-cont__esti'>";
//////                vHtml += "                <button class='btns esti_btn'>견적서 보기</button>";
//////                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].MNGT_NO) + "</p>";
//////                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].CRN) + "</p>";
//////                vHtml += "            </div>";
//////                vHtml += "        </div>";
//////                vHtml += "     </div>";
//////                if (i == 0) {
//////                    if (chkfirst) {
//////                        QuotationRowClick(mngt_no, v_crn, v_pol, v_pod, v_vsl, v_etd, v_eta , v_req_svc);
//////                        chkfirst = false;
//////                    }
//////                }
//////            });


//////        } else {
//////            vHtml += "   <div class=\"exim-box no_data\"> ";
//////            vHtml += "   	<p><img src=\"/Images/no_data.png\" /></p> ";
//////            vHtml += "   </div> ";
//////        }
//////        $(".bk-list").append(vHtml);
       
//////        vResult = JSON.parse(rtnVal.replace(/\\n/gi,'<br/>')).MAIN[0];


//////        $("#DT_POL_NM").text(_fnToNull(vResult.POL_NM));
//////        $("#DT_POD_NM").text(_fnToNull(vResult.POD_NM));
//////        $("#DT_ETA").text(String(_fnToNull(vResult.ETA)).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3'));
//////        $("#DT_ETD").text(String(_fnToNull(vResult.ETD)).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3'));
//////        $("#DT_ITEM_NM").text(_fnToNull(vResult.ITEM_NM));
//////        $("#DT_TOT_PKG").text(_fnToNull(_fnGetNumber(parseInt(_fnToZero(vResult.TOT_PKG)), "sum")));
//////        $("#DT_TOT_GRS_WGT").text(_fnToNull(_fnGetNumber(parseInt(_fnToZero(vResult.TOT_GRS_WGT)), "sum")));
//////        $("#DT_TOT_VOL_WGT").text(_fnToNull(_fnGetNumber(parseInt(_fnToZero(vResult.TOT_VOL_WGT)), "sum")));
//////        $("#DT_TOT_RMK")[0].innerHTML = _fnToNull(vResult.RMK);
//////        if (_fnToNull(vResult.STATUS) == "Y") {
//////            $("#btn_booking").hide();
//////        }
//////        vResult = JSON.parse(rtnVal).DOC;
//////        if (vResult.length > 0) {
//////            objJsonData.OCR_NO = _fnToNull(vResult[0].OCR_NO);
//////        }

//////        vHtml = "";

//////        $("#OCR_LIST").empty();
//////        var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetOcrData", objJsonData);
//////        if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
//////            vResult = JSON.parse(rtnVal.replace(/\\n/gi, '<br/>')).Table1;

//////            $.each(vResult, function (i) {
//////                vHtml += "	      <div class='req-field__cont ocrRow'> ";
//////                vHtml += "        <p><span class='s_mo'>판독영역</span>" + "필드" + (i + 1) + "</p>";
//////                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + i + "</span></p>";
//////                vHtml += "        <p><span class='s_mo'>이름</span><span class='s_dt'>" + vResult[i].OCR_NAME + "</span></p>";
//////                vHtml += "        <p class='content'><span class='s_mo'>내용</span><span class='s_dt'>" + vResult[i].OCR_TEXT + "</span></p>";
//////                vHtml += "        </div>";
//////            });

//////            $("#OCR_LIST").append(vHtml);
//////        }

//////        vHtml = "";

//////        objJsonData.MNGT_NO = _fnToNull($("#QUOT_NO").val());
//////        objJsonData.DOC_TYPE = "CI,PL";

//////        $("#FILE_LIST").empty();

//////        var rtnVal = _fnGetAjaxData("POST", "Export", "fnGetExImDocPath", objJsonData);
//////        if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
//////            vResult = JSON.parse(rtnVal).Table1;
//////            $.each(vResult, function (i) {
//////                vHtml += "	<div class='req-info__desc' id='FILE_TEMP" + i + "'>	";
//////                if (_fnToNull(vResult[i].DOC_TYPE) == "CI") {
//////                    vHtml += "        <p class='req-doc__nm'>C/I</p>";
//////                } else {
//////                    vHtml += "        <p class='req-doc__nm'>P/L</p>";
//////                }
//////                vHtml += "        <div class='req-doc__desc'>";
//////                vHtml += "            <p class='req-doc__title'>서식명</p>";
//////                vHtml += "            <p>" + _fnToNull(vResult[i].TMPLT_NM) + "</p>";
//////                vHtml += "        </div>";
//////                vHtml += "        <div class='req-doc__desc'>";
//////                vHtml += "            <p class='req-doc__title'>파일명</p>";
//////                vHtml += "            <p>" + _fnToNull(vResult[i].FILE_NM) + "</p>";
//////                vHtml += "        </div>";
//////                vHtml += "    </div>";
//////            });
//////        } else {
//////            vHtml += "<div class='req-info__desc wd100'>";
//////            vHtml += "<div class='req-doc__desc'>";
//////            vHtml += "<p><img src='/Images/no_data.png'/></p>";
//////            vHtml += "</div>";
//////            vHtml += "</div>";
//////        }

//////        $("#FILE_LIST").append(vHtml);

//////    } else {
//////        location.href = window.location.origin + "/Estimate/Inquiry";
//////    }
//////}


////$(document).on("click", "#FILE_TEMP0", function () {

////});


////$(document).on("click", "#FILE_TEMP1", function () {

////});

////$(document).on("click", ".esti_btn", function () {
////    var vHtml = "";

////    var tr = $(this).closest('div');
////    var td = tr.children();

////    var objJsonData = new Object();

////    objJsonData.DOC_TYPE = "QUOT";
////    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
////    objJsonData.CRN = _fnToNull($("#Session_CRN").val());
////    objJsonData.MNGT_NO = td.eq(1).text().trim();

////    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetQuotationDoc", objJsonData);
////    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
////        var vResult = JSON.parse(rtnVal).DOC;

////        objJsonData = new Object();
////        objJsonData.FILE_PATH = vResult[0].FILE_PATH + vResult[0].SVR_FILE_NM;
////        objJsonData.FILE_NM = vResult[0].SVR_FILE_NM;
////        objJsonData.FILE_EXT = vResult[0].FILE_EXT;

////        $.ajax({
////            type: "POST",
////            url: "/Main/FileApiToServerDownload",
////            async: true,
////            data: { "vJsonData": _fnMakeJson(objJsonData) },
////            success: function (result) {

////                layerPopup('#pdfLayer');
////                $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + objJsonData.FILE_NM + ".pdf");

////            }, error: function (xhr) {
////                console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
////                console.log(xhr);
////                return;
////            }
////        });
////    } else {
////        _fnAlertMsg("견적서가 없습니다.");
////    }
////});

////$("#btn_booking").click(function (e) {
////    var objJsonData = new Object();

////    objJsonData.QUOT_NO = _fnToNull($("#QUOT_NO").val());
////    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
////    objJsonData.CRN = _fnToNull($("#Session_CRN").val());
////    objJsonData.QUOT_DTL_NO = _fnToNull($("#MNGT_NO").val());
////    objJsonData.ELVIS_CRN = _fnToNull($("#ELVIS_CRN").val());;

////    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnSetQuotStatus", objJsonData);
////    _fnAlertMsg("부킹요청되었습니다.");
////    layerPopup('#alert01', "", false);
////    $("#alert_close").focus();
////    $('#alert_close').click(function () {
////        controllerToLink("Inquiry", "Estimate", "");
////    });
////});


////$(document).on("click", ".bk-list__cont", function () {

////    var tr = $(this).closest('div');
////    var td = tr.children();
////    $("#ELVIS_CRN").val(td.eq(1).text().trim());
////    var MNGT_NO = td.eq(0).text().trim();
////    var crn = td.eq(1).text().trim();
////    var pol = td.eq(2).text().trim();
////    var pod = td.eq(3).text().trim();
////    var vsl = td.eq(4).text().trim();
////    var etd = td.eq(5).text().trim();
////    var eta = td.eq(6).text().trim();
////    var req_svc = td.eq(7).text().trim();

////    QuotationRowClick(MNGT_NO, crn ,pol,pod,vsl,etd,eta,req_svc );
////});

////function QuotationRowClick(mngt_no , elvis_crn,pol,pod,vsl,etd,eta,req_svc) {
////    var vHtml = "";
////    var objJsonData = new Object();
////    $("#Frt_List").empty();
////    objJsonData.QUOT_NO = _fnToNull($("#QUOT_NO").val());
////    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
////    objJsonData.CRN = _fnToNull($("#Session_CRN").val());
////    objJsonData.MNGT_NO = mngt_no;
////    $("#ELVIS_CRN").val(elvis_crn);
////    $("#MNGT_NO").val(objJsonData.MNGT_NO);


////    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetQuotationFreight", objJsonData);
////    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
////        var vResult = JSON.parse(rtnVal).Table1;

////        $.each(vResult, function (i) {
////            vHtml += "	<div class='bk-esti__cont'>	";
////            vHtml += "        <p><span class='s_mo'>운임코드</span>" + _fnToNull(vResult[i].FARE_CD) + "</p>";
////            vHtml += "        <p><span class='s_mo'>운임명</span>" + _fnToNull(vResult[i].FARE_NM) + "</p>";
////            vHtml += "        <p><span class='s_mo'>화폐</span>" + _fnToNull(vResult[i].CURR_CD) + "</p>";
////            if (_fnToNull(vResult[i].CURR_CD) == "KRW") {
////                vHtml += "        <p><span class='s_mo'>금액</span>" + _fnToNull(_fnGetNumber(parseInt(vResult[i].FARE_LOC_AMT), "sum")) + "</p>";
////            } else {
////                vHtml += "        <p><span class='s_mo'>금액</span>" + _fnToNull(_fnGetNumber(parseInt(vResult[i].FARE_AMT), "sum")) + "</p>";
////            }
////            vHtml += "    </div>";
////        });
////        $("#Frt_List").append(vHtml);
////        $("#FRT_IMG")[0].innerHTML = "<img src='/Images/fwd_logo/" + elvis_crn + ".png'>";
////        $("#FRT_POL")[0].innerHTML = pol + " <span>" + String(etd).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Eng(_fnToNull(etd).replace(/\./gi, ""))) + ")</span>";
////        $("#FRT_POD")[0].innerHTML = pod + " <span>" + String(eta).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Eng(eta.replace(/\./gi, ""))) + ")</span>";
////        $("#VSL")[0].innerHTML = _fnToNull(vsl);
////        if (req_svc == "AIR") {
////            $("#VSL_TXT").text("Flight");
////        } else {
////            $("#VSL_TXT").text("Vsl");
////        }
////    }
////}


////$(document).on("click", ".bk-cont", function () {
////    $(".bk-cont").removeClass("on");
////    $(this).closest(".bk-cont").addClass("on");
////})

/////////////////////////function///////////////////////////////////

//////////////////////////function makelist////////////////////////

/////////////////////////////////API///////////////////////////////
