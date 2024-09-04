//#region ★★★★★전역변수★★★★★
const status_text = ["진행중", "견적완료"];
const list_text = ["진행중", "진행완료"];

var vHtml = "";

//#endregion

//#region ★★★★★시작 이벤트★★★★★
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    //메뉴탭 활성화
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

//#endregion



//#region ★★★★★이벤트 영역 ★★★★★

//달력 왼쪽 클릭
$(document).on("click", "#btn_cal_left", function () {
    $("#cal_date").text(fnSetYearMonth("L"));
    fnSearchData();
});

//달력 오른쪽 클릭
$(document).on("click", "#btn_cal_right", function () {
    $("#cal_date").text(fnSetYearMonth("R"));
    fnSearchData();
});


//상세 클릭
$(document).on("click", ".esti_dtl", function () {
    var tr = $(this).closest('div');
    var td = tr.children();
    var objParam = new Object();
    objParam.QUOT_NO = td.eq(1).text().trim();

    controllerToLink("InquiryDetail", "Estimate", objParam, false);

});

//#region 상단 상태별 View 
$(".esti-total__status.total").on("click", function () {
    $(".esti-cont.present").show();
    $(".esti-cont.finished").show();
})
$(".esti-total__status.present").on("click", function () {
    $(".esti-cont.present").show();
    $(".esti-cont.finished").hide();
})
$(".esti-total__status.finished").on("click", function () {
    $(".esti-cont.present").hide();
    $(".esti-cont.finished").show();
})
//#endregion

//#endregion





//#region ★★★★★ 함수 영역 ★★★★★

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


// 조회 함수
function fnSearchData() {
    $("#estiList").empty();
    var _vHtml = "";


    var newDt = new Date($("#cal_date").text().split(".")[0], $("#cal_date").text().split(".")[1], 0);
    var lastDt = newDt.getDate();

    var objJsonData = new Object();
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.YMD_TYPE = "REQ_YMD";

    objJsonData.FM_YMD = $("#cal_date").text().replace(/\./gi, '') + "01";
    objJsonData.TO_YMD = $("#cal_date").text().replace(/\./gi, '') + lastDt;


    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetQuotationList", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vList = JSON.parse(rtnVal).Table1;
        _vHtml = fnDrawList(vList);
        

    } else if (JSON.parse(rtnVal).Result[0]["trxCode"] == "N") {
        _vHtml += "   <div class=\"exim-box no_data\"> ";
        _vHtml += "   	<p><img src=\"/Images/no_data.png\" /></p> ";
        _vHtml += "   </div> ";


        $("#TOT_CNT").text(_fnToZero(0));
        $("#PRE_CNT").text(0);
        $("#COMP_CNT").text(0);
    }

    $("#estiList").append(_vHtml);
}

//#endregion


//#region ★★★★★ 그리기 영역 ★★★★★
function fnDrawList(vJson) {
    var vResult = vJson;
    var presentCnt = 0;
    var CompleteCnt = 0;
    var status = "";
    var list_txt = "";
    $("#estiList").empty();

    vHtml = "";
    $.each(vResult, function (i) {
        status = "";
        list_txt = "";
        if (parseInt(vResult[i].REQ_FWD_CNT) != parseInt(vResult[i].FWD_QUOT_CNT)) { // 예정
            presentCnt += 1
            status = status_text[0]; // 리스트 앞쪽 상태값
            list_txt = list_text[0]; // 리스트 내 상태값
            vHtml += "	<div class='esti-cont present'>	";
        }
        else { // 완료
            CompleteCnt += 1
            status = status_text[1]; //리스트 앞쪽 상태값
            list_txt = list_text[1]; // 리스트 내 상태값
            vHtml += "	<div class='esti-cont finished'>	";
        }

        vHtml += "      <div class='esti-cont__info'>";

        //상태 
        vHtml += "          <div class='esti-cont__box'>";
        vHtml += "              <div class='esti-cont__inner'>";
        vHtml += "                  <p class='esti-cont__stat'>" + status+"</p>";
        vHtml += "              </div>";
        vHtml += "          </div>";

        //POL
        vHtml += "          <div class='esti-cont__box'>";
        vHtml += "              <div class='esti-cont__flex'>";
        vHtml += "                  <div class='esti-cont__desc'>";
        vHtml += "                      <p>" +_fnToNull(vResult[i]["POL_NM"])+ "</p>";
        vHtml += "                  </div>";
        vHtml += "                  <div class='esti-cont__desc2'>";
        if (_fnToNull(vResult[i]["ETD"]) != "") { // ETD 있을 때 
            vHtml += "                 <p>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "(" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ")" + "</p>";
        }
        else { //없을 때
            vHtml += "                 <p style='visibility:hidden'>-</p>";
        }
        vHtml += "                  </div>";
        vHtml += "              </div>";
        vHtml += "          </div>";

        //건수
        vHtml += "          <div class='esti-cont__box'>";
        vHtml += "              <div class='esti-cont__inner'>";
        vHtml += "                  <p class='esti-cont__progress'>";
        vHtml += "                      <img src='/Images/icn_progress.png'>" +_fnToZero(vResult[i]["REQ_FWD_CNT"])+ "개 업체 견적 " + list_txt ;
        vHtml += "                  </p>";
        vHtml += "              </div>";
        vHtml += "          </div>";

        //POD
        vHtml += "          <div class='esti-cont__box'>";
        vHtml += "              <div class='esti-cont__flex'>";
        vHtml += "                  <div class='esti-cont__desc'>";
        vHtml += "                      <p>" + _fnToNull(vResult[i]["POD_NM"]) + "</p>";
        vHtml += "                  </div>";
        vHtml += "                  <div class='esti-cont__desc2'>";
        if (_fnToNull(vResult[i]["ETA"]) != "") { // ETA 있을 때 
            vHtml += "                 <p>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "(" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ")" + "</p>";
        }
        else { //없을 때
            vHtml += "                 <p style='visibility:hidden'>-</p>";
        }
        vHtml += "                  </div>";
        vHtml += "              </div>";
        vHtml += "          </div>";

        //견적 요청 및 확정일
        vHtml += "          <div class='esti-cont__box'>";
        vHtml += "              <div class='esti-cont__inner'>";
        vHtml += "                  <div class='esti-cont__date'>";
        vHtml += "                      <div class='esti-cont__date_start'>";
        vHtml += "                          <span class='esti-cont__date_title'>견적요청일</span>";
        if (_fnToNull(vResult[i]["REQ_YMD"]) != "") {
            vHtml += "                          <span class='esti-cont__date_cont'>" + String(_fnToNull(vResult[i]["REQ_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</span>";
        }
        else {
            vHtml += "                          <span class='esti-cont__date_cont' style='visibility:hidden'>0000.00.00</span>";
        }
        vHtml += "                      </div>";
        vHtml += "                      <div class='esti-cont__date_end'>";
        vHtml += "                          <span class='esti-cont__date_title'>최종견적일</span>";
        if (_fnToNull(vResult[i]["QUOT_YMD"]) != "") {
            vHtml += "                          <span class='esti-cont__date_cont'>" + String(_fnToNull(vResult[i]["QUOT_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</span>";
        }
        else {
            vHtml += "                          <span class='esti-cont__date_cont' style='visibility:hidden'>0000.00.00</span>";
        }
        vHtml += "                      </div>";
        vHtml += "                  </div>";
        vHtml += "              </div>";
        vHtml += "          </div>";

        //상세 버튼
        vHtml += "          <div class='esti-cont__box'>";
        vHtml += "              <div class='esti-cont__inner'>";
        vHtml += "                  <button type='button' class='btns esti_dtl'>상세</button>";
        vHtml += "                  <p style='display:none'>"+_fnToNull(vResult[i]["QUOT_NO"])+"</p>";
        vHtml += "              </div>";

        vHtml += "          </div>";


        vHtml += "      </div>";


        vHtml += "  </div>";

    });

    //#region 상단 값 바인딩
    $("#TOT_CNT").text(_fnToZero(vResult.length));
    $("#PRE_CNT").text(presentCnt);
    $("#COMP_CNT").text(CompleteCnt);
    //#endregion


    return vHtml;
}


//#endregion