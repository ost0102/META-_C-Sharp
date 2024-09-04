////////////////////전역 변수//////////////////////////

////////////////////jquery event///////////////////////
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    $(".sub_info").addClass("on");
    $(".sub_info .sub_depth").addClass("on");
    $(".sub_info .sub_depth li:nth-child(3) a").addClass("on");

    $("#cal_date").text(fnSetNowDate()); //현재 날짜 세팅
    fnSearchData();
    $("#ProgressBar_Loading").show(); //프로그래스 바
    setTimeout(function () {
        $("#ProgressBar_Loading").hide(); //프로그래스 바
    }, 1000);
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


$(document).on("click", "#btn_cal_left", function () {
    $("#cal_date").text(fnSetYearMonth("L"));
    fnSearchData();
});

//달력 오른쪽 클릭
$(document).on("click", "#btn_cal_right", function () {
    $("#cal_date").text(fnSetYearMonth("R"));
    fnSearchData();
});

//function fnSearchData() {
//    $("#estiList").empty();
//    var vHtml = "";
//    var presentCnt = 0;
//    var CompleteCnt = 0;

//    var newDt = new Date($("#cal_date").text().split(".")[0], $("#cal_date").text().split(".")[1], 0);
//    var lastDt = newDt.getDate();

//    var objJsonData = new Object();
//    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
//    objJsonData.CRN = $("#Session_CRN").val();
//    objJsonData.YMD_TYPE = "REQ_YMD";
 
//    objJsonData.FM_YMD = $("#cal_date").text().replace(/\./gi, '') + "01";
//    objJsonData.TO_YMD = $("#cal_date").text().replace(/\./gi, '') + lastDt;


//    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetQuotationList", objJsonData);
//    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
//        var vResult = JSON.parse(rtnVal).Table1;
//        $("#estiList").empty();

//        $.each(vResult, function (i) {
//            if (parseInt(vResult[i].REQ_FWD_CNT) != parseInt(vResult[i].FWD_QUOT_CNT)) {
//                presentCnt += 1;
//                vHtml += "	<div class='esti-cont present'>	";
//            } else {
//                CompleteCnt += 1;
//                vHtml += "	<div class='esti-cont finished'>	";
//            }
//            vHtml += "        <div class='esti-cont__info'>";
//            vHtml += "            <div class='esti-cont__box'>";
//            vHtml += "                <div class='esti-cont__inner'>";

//            if (parseInt(vResult[i].REQ_FWD_CNT) != parseInt(vResult[i].FWD_QUOT_CNT)) {
//                vHtml += "                    <p class='esti-cont__progress'><img src='/Images/icn_progress.png' />" + parseInt(vResult[i].REQ_FWD_CNT) + "개 업체 견적 진행중</p>";
//            } else {
//                vHtml += "                    <p class='esti-cont__progress'>" + parseInt(vResult[i].REQ_FWD_CNT) + "개 업체 견적 진행완료</p>";
//            }
//            vHtml += "                </div>";
//            vHtml += "            </div>";
//            vHtml += "            <div class='esti-cont__box'>";
//            vHtml += "                <div class='esti-cont__inner'>";
//            vHtml += "                    <div class='esti-cont__flex'>";
//            vHtml += "                        <div class='esti-cont__title'><p>출발</p></div>";
//            vHtml += "                     <div class='esti-cont__desc'><p>" + _fnToNull(vResult[i].POL_NM);
//            if (_fnToNull(vResult[i]["ETD"]) != "") {
//                vHtml += " <span>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ")" + "</span>";
//            }
//            vHtml += " </p></div> ";
//            vHtml += "                    </div>";
//            vHtml += "                    <div class='esti-cont__flex'>";
//            vHtml += "                        <div class='esti-cont__title'><p>도착</p></div>";
//            vHtml += "                     <div class='esti-cont__desc'><p>" + _fnToNull(vResult[i].POD_NM);
//            if (_fnToNull(vResult[i]["ETA"]) != "") {
//                vHtml += " <span>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ")" + "</span>";
//            }
//            vHtml += " </p></div> ";
//            vHtml += "                    </div>";
//            vHtml += "                </div>";
//            vHtml += "            </div>";
//            vHtml += "            <div class='esti-cont__box'>";
//            vHtml += "                <div class='esti-cont__inner'>";
//            vHtml += "                    <div class='esti-cont__flex'>";
//            vHtml += "                        <div class='esti-cont__title'><p>견적요청일</p></div>";
//            vHtml += "                        <div class='esti-cont__desc'><p>" + String(_fnToNull(vResult[i]["REQ_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</p></div>";
//            vHtml += "                    </div>";
//            vHtml += "                    <div class='esti-cont__flex'>";
//            vHtml += "                        <div class='esti-cont__title'><p>최종견적일</p></div>";
//            vHtml += "                        <div class='esti-cont__desc'><p>" + String(_fnToNull(vResult[i]["QUOT_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</p></div>";
//            vHtml += "                    </div>";
//            vHtml += "                </div>";
//            vHtml += "            </div>";
//            vHtml += "            <div class='esti-cont__box'>";
//            vHtml += "                <div class='esti-cont__inner'>";
//            vHtml += "                    <button type='button' class='btns esti_dtl'>상세</button>";
//            vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].QUOT_NO) + "</p>";
//            vHtml += "                </div>";
//            vHtml += "            </div>";
//            vHtml += "        </div>";
//            vHtml += "    </div>";
//        });

//        $("#TOT_CNT").text(_fnToZero(vResult.length));
//        $("#PRE_CNT").text(presentCnt);
//        $("#COMP_CNT").text(CompleteCnt);
//    } else if (JSON.parse(rtnVal).Result[0]["trxCode"] == "N") {
//        vHtml += "   <div class=\"exim-box no_data\"> ";
//        vHtml += "   	<p><img src=\"/Images/no_data.png\" /></p> ";
//        vHtml += "   </div> ";


//        $("#TOT_CNT").text(_fnToZero(0));
//        $("#PRE_CNT").text(0);
//        $("#COMP_CNT").text(0);
//    }
//    $("#estiList").append(vHtml);
//}

$(document).on("click", ".esti_dtl", function () {
    var tr = $(this).closest('div');
    var td = tr.children();
    var objParam = new Object();
    objParam.QUOT_NO = td.eq(1).text().trim();

    controllerToLink("InquiryDetail", "Estimate", objParam, false);

});


$(".esti-total__desc.total").on("click", function () {
    $(".esti-cont.present").show();
    $(".esti-cont.finished").show();
})
$(".esti-total__desc.present").on("click", function () {
    $(".esti-cont.present").show();
    $(".esti-cont.finished").hide();
})
$(".esti-total__desc.finished").on("click", function () {
    $(".esti-cont.present").hide();
    $(".esti-cont.finished").show();
})

/////////////////////function///////////////////////////////////

//////////////////////function makelist////////////////////////

/////////////////////API///////////////////////////////////////