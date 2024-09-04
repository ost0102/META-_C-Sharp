////////////////////전역 변수//////////////////////////

var _vSelectDate = new Date();
////////////////////jquery event///////////////////////
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }
    if (_fnToNull($("#Session_USR_TYPE").val()) == "S") { // 화주
        $(".sub_info").addClass("on");
        $(".sub_info .sub_depth").addClass("on");
        $(".sub_info .sub_depth li:nth-child(4) a").addClass("on");
    } else { // 실행사
        $(".sub_info").addClass("on");
        $(".sub_info .sub_depth").addClass("on");
        $(".sub_info .sub_depth li:nth-child(2) a").addClass("on");
    }

    $("#cal_date").text(fnSetNowDate()); //현재 날짜 세팅

    $("#ProgressBar_Loading").show(); //프로그래스 바
    setTimeout(function () {
        $("#ProgressBar_Loading").hide(); //프로그래스 바
    }, 1000);
    fnSearchData();
});

//년/월 세팅
function fnSetNowDate() {
    try {
        var weekDate = _vSelectDate.getTime() + (24 * 60 * 60 * 1000);
        _vSelectDate.setTime(weekDate);

        var weekYear = _vSelectDate.getFullYear();
        var weekMonth = _vSelectDate.getMonth() + 1;

        var result = weekYear + "." + _pad(weekMonth, "2");
        return result;
    }
    catch (err) {
        console.log("[Error - fnSetNowDate]" + err.message);
    }
}
function fnSearchData() {

    var newDt = new Date($("#cal_date").text().split(".")[0], $("#cal_date").text().split(".")[1], 0);
    var lastDt = newDt.getDate();

    var objJsonData = new Object();

    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.EX_IM_TYPE = "E,I";
    objJsonData.YMD_TYPE = "ETD";
    objJsonData.USR_TYPE = $("#Session_USR_TYPE").val();
    objJsonData.FM_YMD = $("#cal_date").text().replace(/\./gi, '') + "01";
    objJsonData.TO_YMD = $("#cal_date").text().replace(/\./gi, '') + lastDt ;


    $.ajax({
        type: "POST",
        url: "/Export/fnGetExImList",
        async: true,
        dataType: "json",
        //data: callObj,
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            fnMakeList(result);
        }, error: function (xhr, status, error) {
            $("#ProgressBar_Loading").hide(); //프로그래스 바
            _fnAlertMsg("담당자에게 문의 하세요.");
            console.log(error);
        },
        beforeSend: function () {
            $("#ProgressBar_Loading").show(); //프로그래스 바
        },
        complete: function () {
            $("#ProgressBar_Loading").hide(); //프로그래스 바
        }
    });
}

$(document).on("click", "#btn_cal_left", function () {
    $("#cal_date").text(fnSetYearMonth("L"));
    fnSearchData();
});

//달력 오른쪽 클릭
$(document).on("click", "#btn_cal_right", function () {
    $("#cal_date").text(fnSetYearMonth("R"));
    fnSearchData();
});


$(document).on("click", ".prog-status__stat.btnTax", function () {
    //window.open('http://bill.elvisprime.com/', 'newWindow');
    //사이트 URL 변경으로 인한 링크 변경
    window.open('http://elvisbill.com/', 'newWindow');
});

function fnMakeList(vJsonData) {
    try {
        var vHTML = "";
        $("#eximList").empty();
        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            vResult = JSON.parse(vJsonData).Table1;

            //#region 과장님 시안 
            //반복문
            $.each(vResult, function (i) {
                if (_fnToNull(vResult[i].EX_IM_TYPE) == "E") {
                    if (_fnToNull(vResult[i].REQ_SVC) == "SEA") {
                        vHTML += "<div class='exim-box exp ship'>			";
                        vHTML += "  <div class='exim-icn'>";
                        vHTML += "      <img src='/Images/ship_export_icn.png'/>";
                    } else {
                        vHTML += "<div class='exim-box exp air'>			";
                        vHTML += "  <div class='exim-icn'>";
                        vHTML += "      <img src='/Images/air_export_icn.png'/>";
                    }
                } else {
                    if (_fnToNull(vResult[i].REQ_SVC) == "SEA") {
                        vHTML += "<div class='exim-box imp ship'>			";
                        vHTML += "  <div class='exim-icn'>";
                        vHTML += "      <img src='/Images/ship_import_icn.png'/>";
                    } else {
                        vHTML += "<div class='exim-box imp air'>			";
                        vHTML += "  <div class='exim-icn'>";
                        vHTML += "      <img src='/Images/air_import_icn.png'/>";
                    }
                }
                vHTML += "           <div class='exim-cont__prog mo'>";
                if (_fnToNull(vResult[i].BL_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished btnBL'>";
                } else {
                    vHTML += "               <div class='prog-status__stat present btnBL'>";
                }
                vHTML += "                   <div class='prog-status__bar'>";
                vHTML += "                       <div class='prog-status__percent'></div>";
                vHTML += "                   </div>";
                vHTML += "                   <button type='button' class='prog-status__nm'>B/L</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "               </div>";
                if (_fnToNull(vResult[i].INV_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished btnInv'>";
                } else {
                    vHTML += "               <div class='prog-status__stat present btnInv'>";
                }
                vHTML += "                   <div class='prog-status__bar'>";
                vHTML += "                       <div class='prog-status__percent'></div>";
                vHTML += "                   </div>";
                vHTML += "                   <button type='button' class='prog-status__nm'>INV</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "               </div>";
                    if (_fnToNull(vResult[i].DO_SEND_DT) != "") {
                        vHTML += "               <div class='prog-status__stat finished btnDo'>";
                    } else {
                        vHTML += "               <div class='prog-status__stat present btnDo'>";
                    }
                    vHTML += "                   <div class='prog-status__bar'>";
                    vHTML += "                       <div class='prog-status__percent'></div>";
                    vHTML += "                   </div>";
                    vHTML += "                   <button type='button' class='prog-status__nm'>D/O</button>";
                    vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                    vHTML += "               </div>";
                if (_fnToNull(vResult[i].MFCS_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished btnEdi'>";
                } else {
                    vHTML += "               <div class='prog-status__stat present btnEdi'>";
                }
                vHTML += "                   <div class='prog-status__bar'>";
                vHTML += "                       <div class='prog-status__percent'></div>";
                vHTML += "                   </div>";
                vHTML += "                   <button type='button' class='prog-status__nm'>EDI</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "               </div>";
                if (_fnToNull(vResult[i].TAX_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished btnTax'>";
                } else {
                    vHTML += "               <div class='prog-status__stat present btnTax'>";
                }
                vHTML += "                   <div class='prog-status__bar'>";
                vHTML += "                       <div class='prog-status__percent'></div>";
                vHTML += "                   </div>";
                vHTML += "                   <button type='button' class='prog-status__nm'>TAX</button>";
                vHTML += "               </div>";
                vHTML += "           </div>";
                vHTML += "       </div>";
                vHTML += "       <div class='exim-inner'>";
                vHTML += "           <div class='exim-cont text-info'>";
                vHTML += "               <div class='exim-cont__info'>";
                vHTML += "                   <div class='exim-cont__inner'>";
                vHTML += "                       <div class='exim-cont__flex'>";
                vHTML += "                          <div class='exim-cont__desc'>";
                vHTML += "                              <p>" + _fnToNull(vResult[i].POL_NM) + "</p>";
                vHTML += "                          </div>";
                vHTML += "                          <div class='exim-cont__desc2'>";
                if (_fnToNull(vResult[i].REQ_SVC) == "SEA") {
                    if (_fnToNull(vResult[i]["ETD"]) != "") {
                        vHTML += "<p> " + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " " + _fnFormatTime(_fnToNull(vResult[i]["ETD_HM"])) + " (" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ") " + "</p>";
                    }
                } else {
                    if (_fnToNull(vResult[i]["ETD"]) != "") {
                        vHTML += "<p> " + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " " + _fnFormatTime(_fnToNull(vResult[i]["ETD_HM"])) + " (" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ") " + "</p>";
                    }
                }
                vHTML += "                          </div>";
                vHTML += "                       </div > ";
                vHTML += "                   </div>";
                vHTML += "               </div>";
                vHTML += "               <div class='exim-cont__info'>";
                vHTML += "                   <div class='exim-cont__inner'>";
                vHTML += "                      <p class='exim-cont__progress'>";
                vHTML += "                      <img src='/Images/icn_progress.png''/>20개 업체 견적 진행중</p>";
                vHTML += "                   </div>";
                vHTML += "               </div>";
                vHTML += "               <div class='exim-cont__info'>";
                vHTML += "                   <div class='exim-cont__inner'>";
                vHTML += "                       <div class='exim-cont__flex'>";
                vHTML += "                          <div class='exim-cont__desc'>";
                vHTML += "                              <p>" + _fnToNull(vResult[i].POD_NM) + "</p>";
                vHTML += "                          </div>";
                vHTML += "                          <div class='exim-cont__desc2'>";
                if (_fnToNull(vResult[i].REQ_SVC) == "SEA") {
                    if (_fnToNull(vResult[i]["ETA"]) != "") {
                        vHTML += "<p> " + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " " + _fnFormatTime(_fnToNull(vResult[i]["ETA_HM"])) + " (" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ") " + "</p>";
                    }
                } else {
                    if (_fnToNull(vResult[i]["ETA"]) != "") {
                        vHTML += "<p> " + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " " + _fnFormatTime(_fnToNull(vResult[i]["ETA_HM"])) + " (" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ") " + "</p>";
                    }
                }
                vHTML += "                          </div>";
                vHTML += "                       </div > ";
                vHTML += "                   </div>";
                vHTML += "               </div>";
                vHTML += "           </div>";
                vHTML += "           <div class='exim-cont prog-info pc'>";
                vHTML += "               <div class='exim-cont__prog'>";
                if (_fnToNull(vResult[i].BL_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished btnBL'>";
                } else {
                    vHTML += "               <div class='prog-status__stat present btnBL'>";
                }
                vHTML += "                       <div class='prog-status__bar'>";
                vHTML += "                           <div class='prog-status__percent'></div>";
                vHTML += "                       </div>";
                vHTML += "                       <button type='button' class='prog-status__nm'>B/L</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "                   </div>";
                if (_fnToNull(vResult[i].INV_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished btnInv'>";
                } else {
                    vHTML += "               <div class='prog-status__stat present btnInv'>";
                }
                vHTML += "                       <div class='prog-status__bar'>";
                vHTML += "                           <div class='prog-status__percent'></div>";
                vHTML += "                       </div>";
                vHTML += "                       <button type='button' class='prog-status__nm'>INV</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "                   </div>";
                    if (_fnToNull(vResult[i].DO_SEND_DT) != "") {
                        vHTML += "               <div class='prog-status__stat finished btnDo'>";
                    } else {
                        vHTML += "               <div class='prog-status__stat present btnDo'>";
                    }
                    vHTML += "                       <div class='prog-status__bar'>";
                    vHTML += "                           <div class='prog-status__percent'></div>";
                    vHTML += "                       </div>";
                    vHTML += "                       <button type='button' class='prog-status__nm'>D/O</button>";
                    vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                    vHTML += "                   </div>";
                if (_fnToNull(vResult[i].MFCS_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished btnEdi'>";
                } else {
                    vHTML += "               <div class='prog-status__stat present btnEdi'>";
                }
                vHTML += "                       <div class='prog-status__bar'>";
                vHTML += "                           <div class='prog-status__percent'></div>";
                vHTML += "                       </div>";
                vHTML += "                       <button type='button' class='prog-status__nm'>EDI</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "                   </div>";
                if (_fnToNull(vResult[i].TAX_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished btnTax'>";
                } else {
                    vHTML += "               <div class='prog-status__stat present btnTax'>";
                }
                vHTML += "                       <div class='prog-status__bar'>";
                vHTML += "                           <div class='prog-status__percent'></div>";
                vHTML += "                       </div>";
                vHTML += "                       <button type='button' class='prog-status__nm'>TAX</button>";
                vHTML += "                   </div>";
                vHTML += "               </div>";
                vHTML += "           </div>";
                vHTML += "           <div class='exim-cont doc-info'>";
                vHTML += "               <button type='button' class='btns exim-cont__doc btnDocuList'>Document</button>";
                vHTML += "               <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "           </div>";
                vHTML += "       </div>";
                vHTML += "   </div>";
            });
            //#endregion

        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
            vHTML += "   <div class=\"exim-box no_data\"> ";
            vHTML += "   	<p><img src=\"/Images/no_data.png\" /></p> ";
            vHTML += "   </div> ";
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
            _fnAlertMsg("담당자에게 문의하세요");
            console.log("[Error - fnMakeSchMST] :" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
        }

        $("#eximList")[0].innerHTML = vHTML;
    }
    catch (err) {
        console.log("[Error - fnMakeSchMST]" + err.message);
    }
}


$(document).on("click", ".prog-status__stat.btnEdi", function () {
    var tr = $(this).closest('div');
    var td = tr.children();

    var objJsonData = new Object
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.MNGT_NO = td.eq(2).text().trim();
    objJsonData.DOC_TYPE = "MANI";

    var rtnVal = _fnGetAjaxData("POST", "Export", "fnGetDocPath", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;

        objJsonData = new Object();
        objJsonData.FILE_PATH = vResult[0].FILE_PATH;
        objJsonData.FILE_NM = vResult[0].FILE_NM;
        $("#DOC_TYPE").text();
        //layerPopup('#pdfLayer');
        //$("#iframe_test").attr("src", _ApiUrl +  objJsonData.FILE_PATH);
        $.ajax({
            type: "POST",
            url: "/Main/FileApiToServerDownload",
            async: true,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                layerPopup('#pdfLayer');
                console.log(result);
                $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + objJsonData.FILE_NM + ".pdf");

            }, error: function (xhr) {
                console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
                console.log(xhr);
                return;
            }
        });

        layerPopup('#pdfLayer');
        $("#iframe_test").attr("src", _ApiUrl + vResult[0].FILE_PATH);

    } else {
        _fnAlertMsg("파일이 존재하지 않습니다.");
    }
});


$(document).on("click", ".btnDocuList", function () {
    var vHtml = "";
    var tr = $(this).closest('div');
    var td = tr.children();

    var objJsonData = new Object
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.MNGT_NO = td.eq(1).text().trim();

    var rtnVal = _fnGetAjaxData("POST", "Export", "fnGetDocList", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;
        $("#doc_list").empty();

        $.each(vResult, function (i) {
            vHtml += "	<div class='doc-layer__file'>	";
            vHtml += "        <div class='doc-layer__sort'>";
            vHtml += "            <p>" + _fnToNull(vResult[i]["DOC_TYPE"]) + "</p>";
            vHtml += "        </div>";
            vHtml += "        <div class='doc-layer__nm'>";
            vHtml += "            <p><a href='javascript:void(0)' class='fileDown'>" + _fnToNull(vResult[i]["FILE_NM"]) + _fnToNull(vResult[i]["FILE_EXT"]) + "</a></p>";
            vHtml += "            <p style='display:none'>" + _fnToNull(vResult[i]["FILE_PATH"]) + "</p>";
            vHtml += "            <p style='display:none'>" + _fnToNull(vResult[i]["FILE_NM"]) + "</p>";
            vHtml += "            <p style='display:none'>" + _fnToNull(vResult[i]["FILE_EXT"]) + "</p>";
            vHtml += "            <p style='display:none'>" + _fnToNull(vResult[i]["SVR_FILE_NM"]) + "</p>";
            vHtml += "        </div>";
            vHtml += "    </div>";
        });

        $("#doc_list").append(vHtml);
        layerPopup("#docLayer");

    } else {
        _fnAlertMsg("파일이 존재하지않습니다");
    }
});


$(document).on("click", ".fileDown", function () {
    var tr = $(this).closest('div');
    var td = tr.children();

    var objJsonData = new Object();
    objJsonData.FILE_PATH = td.eq(1).text().trim() + "/" + td.eq(4).text().trim();
    objJsonData.FILE_NM = td.eq(2).text().trim();
    objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
    objJsonData.FILE_EXT = td.eq(3).text().trim();
    $("#DOC_TYPE").text();
    //layerPopup('#pdfLayer');
    //$("#iframe_test").attr("src", _ApiUrl +  objJsonData.FILE_PATH);
    $.ajax({
        type: "POST",
        url: "/Main/FileApiToServerDownload",
        async: true,
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            layerPopup('#pdfLayer');
            console.log(result);
            $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + objJsonData.FILE_NM + objJsonData.FILE_EXT);

        }, error: function (xhr) {
            console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
            console.log(xhr);
            return;
        }
    });

});

$(document).on("click", ".prog-status__stat.btnBL", function () {
    var tr = $(this).closest('div');
    var td = tr.children();

    var objJsonData = new Object();

    objJsonData.USR_ID = $("#Session_USR_ID").val();
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.MNGT_NO = td.eq(2).text().trim();
    objJsonData.DOC_TYPE = "HBL";
    objJsonData.REQ_SVC = "SEA";
    objJsonData.SVC_TYPE = "SEA_IM";

    var rtnVal = _fnGetAjaxData("POST", "Export", "fnGetDocPath", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;

        objJsonData.FILE_PATH = vResult[0].FILE_PATH;
        objJsonData.FILE_NM = vResult[0].FILE_NM;
        $("#DOC_TYPE").text();
        //layerPopup('#pdfLayer');
        //$("#iframe_test").attr("src", _ApiUrl +  objJsonData.FILE_PATH);
        $.ajax({
            type: "POST",
            url: "/Main/FileApiToServerDownload",
            async: true,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                layerPopup('#pdfLayer');
                console.log(result);
                $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + objJsonData.FILE_NM + ".pdf");

            }, error: function (xhr) {
                console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
                console.log(xhr);
                return;
            }
        });

        layerPopup('#pdfLayer');
        $("#iframe_test").attr("src", _ApiUrl + vResult[0].FILE_PATH);

    } else {
        _fnAlertMsg("파일이 존재하지 않습니다.");
    }
});

$(document).on("click", ".prog-status__stat.btnInv", function () {
    var tr = $(this).closest('div');
    var td = tr.children();

    var objJsonData = new Object();
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.MNGT_NO = td.eq(2).text().trim();
    objJsonData.DOC_TYPE = "INV";

    var rtnVal = _fnGetAjaxData("POST", "Export", "fnGetDocPath", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;

        objJsonData = new Object();
        objJsonData.FILE_PATH = vResult[0].FILE_PATH;
        objJsonData.FILE_NM = vResult[0].FILE_NM;
        $("#DOC_TYPE").text();
        //layerPopup('#pdfLayer');
        //$("#iframe_test").attr("src", _ApiUrl +  objJsonData.FILE_PATH);
        $.ajax({
            type: "POST",
            url: "/Main/FileApiToServerDownload",
            async: true,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                layerPopup('#pdfLayer');
                console.log(result);
                $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + objJsonData.FILE_NM + ".pdf");

            }, error: function (xhr) {
                console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
                console.log(xhr);
                return;
            }
        });

        layerPopup('#pdfLayer');
        $("#iframe_test").attr("src", _ApiUrl + vResult[0].FILE_PATH);

    } else {
        _fnAlertMsg("파일이 존재하지 않습니다.");
    }
});


$(document).on("click", ".prog-status__stat.btnDo", function () {
    var tr = $(this).closest('div');
    var td = tr.children();

    var objJsonData = new Object
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
    objJsonData.MNGT_NO = td.eq(2).text().trim();
    objJsonData.DOC_TYPE = "DO";

    var rtnVal = _fnGetAjaxData("POST", "Export", "fnGetDocPath", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;

        objJsonData = new Object();
        objJsonData.FILE_PATH = vResult[0].FILE_PATH;
        objJsonData.FILE_NM = vResult[0].FILE_NM;
        $("#DOC_TYPE").text();
        //layerPopup('#pdfLayer');
        //$("#iframe_test").attr("src", _ApiUrl +  objJsonData.FILE_PATH);
        $.ajax({
            type: "POST",
            url: "/Main/FileApiToServerDownload",
            async: true,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                layerPopup('#pdfLayer');
                console.log(result);
                $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + objJsonData.FILE_NM + ".pdf");

            }, error: function (xhr) {
                console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
                console.log(xhr);
                return;
            }
        });

        layerPopup('#pdfLayer');
        $("#iframe_test").attr("src", _ApiUrl + vResult[0].FILE_PATH);

    } else {
        _fnAlertMsg("파일이 존재하지 않습니다.");
    }
});




/////////////API///////////////////////////////////////