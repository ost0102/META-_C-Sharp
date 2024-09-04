////////////////////전역 변수//////////////////////////
var _vSelectDate = new Date();
var obj = new Object();
var mymap;
var url = "Format";
////////////////////jquery event///////////////////////
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    $(".sub_info").addClass("on");
    $(".sub_info .sub_depth").addClass("on");
    $(".sub_info .sub_depth li:nth-child(2) a").addClass("on");
    fnSearchData();
    $("#ProgressBar_Loading").show(); //프로그래스 바
    setTimeout(function () {
        $("#ProgressBar_Loading").hide(); //프로그래스 바
    }, 1000);
});

function fnSearchData() {

    var objJsonData = new Object();
    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();

    var rtnVal = _fnGetAjaxData("POST", "Format", "fnGetTemplateList", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        makeSearchList(rtnVal);
    }
}
function TmpFileDown(rtnval) {
    var objJsonData = new Object();
    objJsonData.FILE_PATH = rtnval.FILE_PATH + "/" + rtnval.FILE_NAME;
    objJsonData.FILE_NM = rtnval.FILE_NAME;

    $.ajax({
        type: "POST",
        url: "/Main/FileDownload",
        async: true,
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {

            layerPopup('#pdfLayer');
            $("#iframe_test").attr("src", "/web/viewer.html?file=/Content/TempFiles/" + rtnval.FILE_NAME);

        }, error: function (xhr) {
            console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
            console.log(xhr);
            return;
        }
    });

};

function fniframeOpen(tmplt_id) {

    var objJsonData = new Object();
    objJsonData.TMPLT_ID = tmplt_id;
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();

    var rtnVal = _fnGetAjaxData("POST", url, "fnGetTemplatePath", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        makeSearchList(rtnVal);
    }

}


function fniframeClose() {
    layerClose('#pdfLayer');
    $("#iframe_test").attr("src", "");
}


$(document).on("click", "button[name='layer_Frame_btn']", function () {
    var vTmpId = $(this).siblings("input").val();

    var objJsonData = new Object();
    objJsonData.TMPLT_ID = vTmpId;
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();

    var rtnVal = _fnGetAjaxData("POST", url, "fnGetTemplatePath", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        TmpFileDown(JSON.parse(rtnVal).Table1[0]);
    }

});


$(document).on("click", "button[name='layer_Frame_Detail']", function () {
    var TmpPath = $(this).siblings("input").val();


    var objJsonData = new Object();
    objJsonData.TMP_DTL = TmpPath;
    controllerToLink("Regist", "Format", objJsonData);

});

function makeSearchList(rtnVal) {

    var vHTML = "";
    try {
        if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
            var vResult = JSON.parse(rtnVal).Table1;

            $("#format_list").empty();

            $.each(vResult, function (i) {
                vHTML += " 	<div class='format-list__item'>	";
                vHTML += "         <div class='format-list__cont'>	";
                if (_fnToNull(vResult[i]["APV_YN"]) == "Y") {
                    vHTML += "             <div class='format-list__stat approve'>	";
                } else if (_fnToNull(vResult[i]["APV_YN"]) == "P") {
                    vHTML += "             <div class='format-list__stat present'>	";
                } else if (_fnToNull(vResult[i]["APV_YN"]) == "C") {
                    vHTML += "             <div class='format-list__stat cancel'>	";
                } else {
                    vHTML += "             <div class='format-list__stat notapprove'>	";
                }
                vHTML += "                 <p class='mo'>상태</p>	";
                if (_fnToNull(vResult[i]["APV_YN"]) == "Y") {
                    vHTML += "                 <span class='stat_img'><img src='/Images/approve.png'></span><span class='stat_title'>승인</span>	";
                } else if (_fnToNull(vResult[i]["APV_YN"]) == "P") {
                    vHTML += "                 <span class='stat_img'><img src='/Images/present.png'></span><span class='stat_title'>진행중</span>	";
                } else if (_fnToNull(vResult[i]["APV_YN"]) == "C") {
                    vHTML += "                 <span class='stat_img'><img src='/Images/cancel.png'></span><span class='stat_title'>취소</span>	";
                } else {
                    vHTML += "                 <span class='stat_img'><img src='/Images/notapprove.png'></span><span class='stat_title'>미승인</span>	";
                }
                vHTML += "             </div>	";
                vHTML += "         </div>	";
                vHTML += "         <div class='format-list__cont'>	";
                vHTML += "             <div class='format-list__original'>	";
                vHTML += "                 <p class='mo'>원본</p>	";
                vHTML += "                 <input type=\"hidden\" value=\"" + _fnToNull(vResult[i]["TMPLT_ID"]) + "\" /> ";
                vHTML += "                 <button type='button' class='btns origin' name=\"layer_Frame_btn\"></button>	";
                vHTML += "             </div>	";
                vHTML += "         </div>	";
                vHTML += "         <div class='format-list__cont'>	";
                vHTML += "             <div class='format-list__kind'>	";
                vHTML += "                 <p class='mo'>종류</p>	";
                if (_fnToNull(vResult[i]["TMPLT_TYPE"]) == "CI") {
                    vHTML += "                 <p>C/I</p>	";
                } else {
                    vHTML += "                 <p>P/L</p>	";
                }
                vHTML += "             </div>	";
                vHTML += "         </div>	";
                vHTML += "         <div class='format-list__cont'>	";
                vHTML += "             <div class='format-list__nm'>	";
                vHTML += "                 <p class='mo'>서식명</p>	";
                vHTML += "                 <p>" + _fnToNull(vResult[i]["TMPLT_NM"]) + "</p>	";
                vHTML += "             </div>	";
                vHTML += "         </div>	";
                vHTML += "         <div class='format-list__cont'>	";
                vHTML += "             <div class='format-list__date'>	";
                vHTML += "                 <p class='mo'>등록일자</p>	";
                vHTML += "                 <p>" + String(_fnToNull(vResult[i]["INS_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</p>	";
                vHTML += "             </div>	";
                vHTML += "         </div>	";
                vHTML += "         <div class='format-list__cont'>	";
                vHTML += "             <div class='format-list__dtl'>	";
                vHTML += "                <button type='button' class='btns format info' name=\"layer_Frame_Detail\">확인</button>";
                vHTML += "                 <input type=\"hidden\" value=\"" + _fnToNull(vResult[i]["APV_YN"]) + "_^_" + _fnToNull(vResult[i]["TMPLT_TYPE"]) + "_^_" + _fnToNull(vResult[i]["TMPLT_NM"]) + "_^_" + _fnToNull(vResult[i]["FILE_NAME"]) + "_^_" + _fnToNull(vResult[i]["RMK"]) + "_^_" + _fnToNull(vResult[i]["FILE_PATH"]) + "\" /> ";
                vHTML += "             </div>	";
                vHTML += "         </div>	";
                vHTML += "     </div>	";
            });

            $("#format_list").append(vHTML);
        }
    } catch (err) {
        console.log("[Error - makeSearchList]" + err.message);
    }
}
