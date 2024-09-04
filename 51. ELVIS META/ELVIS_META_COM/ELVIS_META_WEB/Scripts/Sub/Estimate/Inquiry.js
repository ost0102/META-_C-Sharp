//#region ★★★★★★전역 변수★★★★★★
var _vSelectDate = new Date();
var obj = new Object();
var mymap;
var vHtml = "";
//#endregion



//#region ★★★★★★시작영역 ★★★★★★
$(function () {
    
    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    $(".sub_esti").addClass("on");
    $(".sub_esti .sub_depth").addClass("on");
    $(".sub_esti .sub_depth li:nth-child(2) a").addClass("on");


    //$("#ETD").val(_fnMinusDate(7));
    $("#ETD").val("2022-11-01");
    $("#ETA").val(_fnPlusDate(7));

    //$("#ProgressBar_Loading").show(); //프로그래스 바
    //setTimeout(function () {
    //    $("#ProgressBar_Loading").hide(); //프로그래스 바
    //}, 1000);

    if (_fnToNull($("#Session_USR_TYPE").val()) == "S") {
        fnSearchData();
    }
    if (_fnToNull($("#Session_USR_TYPE").val()) == "F") {
        fnFwdSearchData();
    }
    
});

//#endregion



//#region ★★★★★★조회 영역 이벤트★★★★★★

//해운 / 항공 Select 박스 체인지 이벤트
$("#REQ_SVC").change(function (e) {
    if (_fnToNull($("#REQ_SVC option:selected").val()) != "") {
        $("#input_POL").attr("disabled", false);
        $("#input_POD").attr("disabled", false);
    } else {
        $("#input_POL").attr("disabled", true);
        $("#input_POD").attr("disabled", true);
    }

    $("#input_POL").siblings(".delete").hide();
    $("#input_POD").siblings(".delete").hide();
    $("#input_POL").val("");
    $("#input_POLCD").val("");
    $("#input_POD").val("");
    $("#input_PODCD").val("");

});


//출발지 자동완성
$(document).on("keyup", "#input_POL", function () {
    //input_POL 초기화
    if (_fnToNull($(this).val()) == "") {
        $("#input_POLCD").val("");
    }

    //autocomplete
    $(this).autocomplete({
        minLength: 3,
        open: function (event, ui) {
            $(this).autocomplete("widget").css({
                "width": $("#AC_POL_Width").width()
            });
        },
        source: function (request, response) {
            var result = fnGetPortData($("#input_POL").val().toUpperCase());
            if (result != undefined) {
                result = JSON.parse(result).Table1
                response(
                    $.map(result, function (item) {
                        return {
                            label: item.NAME,
                            value: item.NAME,
                            code: item.CODE
                        }
                    })
                );
            }
        },
        delay: 150,
        select: function (event, ui) {
            if (ui.item.value.indexOf('데이터') == -1) {
                $("#input_POL").val(ui.item.value);
                $("#input_POLCD").val(ui.item.code);
            } else {
                ui.item.value = "";
            }
        },
        focus: function (event, ui) {
            return false;
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<div>" + item.value + "<br>" + item.code + "</div>")
            .appendTo(ul);
    };
});

//도착지 자동완성
$(document).on("keyup", "#input_POD", function () {
    //input_POL 초기화
    if (_fnToNull($(this).val()) == "") {
        $("#input_PODCD").val("");
    }

    //autocomplete
    $(this).autocomplete({
        minLength: 3,
        open: function (event, ui) {
            $(this).autocomplete("widget").css({
                "width": $("#AC_POD_Width").width()
            });
        },
        source: function (request, response) {
            var result = fnGetPortData($("#input_POD").val().toUpperCase());
            if (result != undefined) {
                result = JSON.parse(result).Table1
                response(
                    $.map(result, function (item) {
                        return {
                            label: item.NAME,
                            value: item.NAME,
                            code: item.CODE
                        }
                    })
                );
            }
        },
        delay: 150,
        select: function (event, ui) {
            if (ui.item.value.indexOf('데이터') == -1) {
                $("#input_POD").val(ui.item.value);
                $("#input_PODCD").val(ui.item.code);
            } else {
                ui.item.value = "";
            }
        },
        focus: function (event, ui) {
            return false;
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<div>" + item.value + "<br>" + item.code + "</div>")
            .appendTo(ul);
    };
});


//#region 조회버튼 클릭 이벤트
$("#btn_search").click(function (e) {
    if (_fnToNull($("#Session_USR_TYPE").val()) == "S") {
        fnSearchData();
    }

    if (_fnToNull($("#Session_USR_TYPE").val()) == "F") {
        fnFwdSearchData();
    }

});

$("#estDate").click(function (e) {
    if (_fnToNull($("#Session_USR_TYPE").val()) == "S") {
        fnSearchData();
    }

    if (_fnToNull($("#Session_USR_TYPE").val()) == "F") {
        fnFwdSearchData();
    }


});

$("#reqDate").click(function (e) {
    if (_fnToNull($("#Session_USR_TYPE").val()) == "S") {
        fnSearchData();
    }

    if (_fnToNull($("#Session_USR_TYPE").val()) == "F") {
        fnFwdSearchData();
    }

});

//#endregion

//#endregion



//#region ★★★★★★Common★★★★★★


//날짜 포스커 아웃 할때 벨리데이션
$(document).on("focusout", "#ETD", function () {
    var vValue = $("#ETD").val();
    var vValue_Num = vValue.replace(/[^0-9]/g, "");
    if (vValue != "") {
        vValue = vValue_Num.substring("0", "4") + "-" + vValue_Num.substring("4", "6") + "-" + vValue_Num.substring("6", "8");
        $(this).val(vValue);
    }

    //값 벨리데이션 체크
    //if (!_fnisDate($(this))) {
    //    $(this).val($("#ETA").val());
    //    //$(this).focus();
    //}

    //날짜 벨리데이션 체크$
    var vETD = $("#ETD").val().replace(/[^0-9]/g, "");
    var vETA = $("#ETA").val().replace(/[^0-9]/g, "");

    if (vETA < vETD) {
        _fnAlertMsg("ETD가 ETA 보다 빠를 수 없습니다. ");
        $("#ETD").val(vETA.substring("0", "4") + "-" + vETA.substring("4", "6") + "-" + vETA.substring("6", "8"));
    }
});

//날짜 포스커 아웃 할때 벨리데이션
$(document).on("focusout", "#ETA", function () {
    var vValue = $("#ETA").val();
    var vValue_Num = vValue.replace(/[^0-9]/g, "");
    if (vValue != "") {
        vValue = vValue_Num.substring("0", "4") + "-" + vValue_Num.substring("4", "6") + "-" + vValue_Num.substring("6", "8");
        $(this).val(vValue);
    }

    ////값 벨리데이션 체크
    //if (!_fnisDate($(this))) {
    //    //$(this).val($("#ETD").val());
    //    //$(this).focus();
    //}

    //날짜 벨리데이션 체크
    var vETD = $("#ETD").val().replace(/[^0-9]/g, "");
    var vETA = $("#ETA").val().replace(/[^0-9]/g, "");

    if (vETA < vETD) {
        _fnAlertMsg("ETA가 ETD 보다 빠를 수 없습니다. ");
        $("#ETA").val(vETD.substring("0", "4") + "-" + vETD.substring("4", "6") + "-" + vETD.substring("6", "8"));
    }
});


//엔터키 nextFocus
$(document).keyup(function (e) {
    if (e.keyCode == 13) {
        //alert($(e.target).attr('data-index'));
        if ($(e.target).attr('data-index') != undefined) {
            var vIndex = $(e.target).attr('data-index');
            if (parseFloat(vIndex) == 2) {
                if (_fnisDate($(e.target))) {
                    if (!layerChek) {
                        if ($("#REQ_SVC").find("option:selected").val() != "") {
                            $('[data-index="' + (parseFloat(vIndex) + 1).toString() + '"]').focus();
                        }
                        else {
                            _vPage = 0;
                            $(e.target).focusout();
                            fnSearchData();
                        }
                    }
                    else {
                        layerChek = false;
                    }
                }
                else {
                    $(e.target).val($("#ETD").val());
                }

            } else {
                if (parseFloat(vIndex) == 1) {
                    if (_fnisDate($(e.target))) {
                        if (!layerChek) {
                            $('[data-index="' + (parseFloat(vIndex) + 1).toString() + '"]').focus();
                        }
                        else {
                            layerChek = false;
                        }
                    }
                    else {
                        $(e.target).val($("#ETA").val());
                    }
                } else {
                    $('[data-index="' + (parseFloat(vIndex) + 1).toString() + '"]').focus();
                }

            }
        }
    }
});

//해운,항공 버튼 디테일 조회 animation
function OpenDtlList(elemnet) {
    var nPosition = elemnet;
    var $btnYN = nPosition.closest(".esti-cont__transit").siblings(".esti-cont__transit").find(".esti-cont__circle");
    if ($btnYN.hasClass("on")) {
        $btnYN.removeClass("on");
        nPosition.addClass("on");
    }
    else if (nPosition.hasClass("on")) {
        nPosition.removeClass("on");
        nPosition.closest(".esti-cont__info").siblings(".esti-detail").slideToggle();
    }
    else {
        nPosition.addClass("on");
        nPosition.closest(".esti-cont__info").siblings(".esti-detail").slideToggle();
        nPosition.closest(".esti-cont")[0].scrollIntoView({ behavior: "smooth" });
    }
}

//#endregion



//#region ★★★★★★리스트 내 이벤트★★★★★★


//#region 화주

//해상 건수 조회 버튼
$(document).on("click", ".seaQuot", function () {
    
    var tr = $(this);
    fnSearchDtlList(tr);


});

//항공 건수 조회 버튼
$(document).on("click", ".airQuot", function () {
    
    var tr = $(this);
    fnSearchDtlList(tr);
   

});


//상세버튼 동작 (페이지 이동)
$(document).on("click", ".esti_dtl", function () {
    var tr = $(this).closest('div');
    var td = tr.children();
    var objParam = new Object();
    objParam.QUOT_NO = td.eq(1).text().trim();

    controllerToLink("InquiryDetail", "Estimate", objParam, false);

});

//디테일 부킹요청 버튼
$(document).on("click", ".esti_booking", function () {
    var tr = $(this).closest('div');
    var td = tr.children();

    var objJsonData = new Object();

    objJsonData.QUOT_NO = td.eq(1).text().trim();
    objJsonData.QUOT_DTL_NO = td.eq(2).text().trim();
    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
    objJsonData.CRN = _fnToNull($("#Session_CRN").val());
    objJsonData.ELVIS_CRN = td.eq(3).text().trim();

    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnSetQuotStatus", objJsonData);
    _fnAlertMsg("부킹요청되었습니다.");
    layerPopup('#alert01', "", false);
    $("#alert_close").focus();
    $('#alert_close').click(function () {
        fnSearchData();
    });
});


$(document).on("click", ".inqu_dtl", function () {
    var objParam = new Object();
    objParam.MNGT_NO = $(this).siblings('input').val();

    controllerToLink("ExInquiryDetail", "Estimate", objParam, false);
});

//#endregion

//#region 미사용
//디테일 펼치기 펼치기
//$(document).on("click", ".esti-cont__circle", function () {
//    var $btnYN = $(this).closest(".esti-cont__transit").siblings(".esti-cont__transit").find(".esti-cont__circle");
//    if ($btnYN.hasClass("on")) {
//        $btnYN.removeClass("on");
//        $(this).addClass("on");
//    }
//    else if ($(this).hasClass("on")) {
//        $(this).removeClass("on");
//        $(this).closest(".esti-cont__info").siblings(".esti-detail").slideToggle();
//    }
//    else {
//        $(this).addClass("on");
//        $(this).closest(".esti-cont__info").siblings(".esti-detail").slideToggle();
//        $(this).closest(".esti-cont")[0].scrollIntoView({ behavior: "smooth" });
//    }
//})
//#endregion

//#endregion



//#region ★★★★★★Func★★★★★★

//자동완성 포트정보 Load Func
function fnGetPortData(vValue) {
    try {
        var rtnJson;
        var objJsonData = new Object();

        //선택에 따라서 Sea S / air A 체크
        objJsonData.REQ_SVC = $("#REQ_SVC option:selected").val();
        objJsonData.LOC_CD = vValue;

        $.ajax({
            type: "POST",
            url: "/Schedule/fnGetPortApi",
            async: false,
            dataType: "json",
            //data: callObj,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                rtnJson = result;
            }, error: function (xhr, status, error) {
                _fnAlertMsg("담당자에게 문의 하세요.");
                console.log(error);
            }
        });

        return rtnJson;
    }
    catch (err) {
        console.log("[Error - fnGetPortData]" + err.message);
    }
}


//메인리스트 조회 함수
function fnSearchData() {
    var vHtml = "";
    var objJsonData = new Object();
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    if (_fnToNull($("#QUOT_NO").val()) != "") {
        objJsonData.QUOT_NO = _fnToNull($("#QUOT_NO").val());
    } else {

        objJsonData.CRN = $("#Session_CRN").val();
        if ($("#reqDate").prop("checked")) {
            objJsonData.YMD_TYPE = "REQ_YMD";
        } else if ($("#estDate").prop("checked")) {
            objJsonData.YMD_TYPE = "QUOT_YMD";
        }
        objJsonData.FM_YMD = $("#ETD").val().replace(/-/gi, '');
        objJsonData.TO_YMD = $("#ETA").val().replace(/-/gi, '');
        objJsonData.POL_CD = $("#input_POLCD").val();
        objJsonData.POD_CD = $("#input_PODCD").val();
    }
    $("#estiList").empty();
    $.ajax({
        type: "POST",
        url: "/Estimate/fnGetQuotationList",
        async: true,
        dataType: "json",

        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (rtnVal) {

            if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") { // 데이터 있을 때
                var DataList = JSON.parse(rtnVal).Table1;

                vHtml = fnMakeQuotList(DataList);

                //#region 기존 그리기

                //$.each(vResult, function (i) {
                    //if (parseint(vresult[i].req_fwd_cnt) != parseint(vresult[i].fwd_quot_cnt)) {
                    //    vhtml += "<div class='esti-cont present'>";
                    //} else {
                    //    vhtml += "<div class='esti-cont finished'>";
                    //}
                    //vhtml += "     <div class='esti-cont__info'>";
                    //vhtml += "         <div class='esti-cont__box'>";
                    //vhtml += "             <div class='esti-cont__inner'>";
                    //vhtml += "                 <div class='esti-cont__flex'>";
                    //vhtml += "                     <div class='esti-cont__title'><p>출발</p></div>";
                    //vhtml += "                     <div class='esti-cont__desc'><p>" + _fntonull(vresult[i].pol_nm);
                    //if (_fntonull(vresult[i]["etd"]) != "") {
                    //    vhtml += " <span>" + string(_fntonull(vresult[i]["etd"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fngetwhatday_eng(_fntonull(vresult[i]["etd"]).replace(/\./gi, ""))) + ")" + "</span>";
                    //}
                    //vhtml += " </p></div> ";
                    //vhtml += "                 </div>";
                    //vhtml += "                 <div class='esti-cont__flex'>";
                    //vhtml += "                     <div class='esti-cont__title'><p>도착</p></div>";
                    //vhtml += "                     <div class='esti-cont__desc'><p>" + _fntonull(vresult[i].pod_nm);
                    //if (_fntonull(vresult[i]["eta"]) != "") {
                    //    vhtml += " <span>" + string(_fntonull(vresult[i]["eta"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fngetwhatday_eng(_fntonull(vresult[i]["eta"]).replace(/\./gi, ""))) + ")" + "</span>";
                    //}
                    //vhtml += " </p></div> ";
                    //vhtml += "                 </div>";
                    //vhtml += "             </div>";
                    //vhtml += "         </div>";
                    //vhtml += "         <div class='esti-cont__box flex'>";
                    //vhtml += "             <div class='esti-cont__inner'>";
                    //vhtml += "                 <div class='esti-cont__flex'>";
                    //vhtml += "                     <div class='esti-cont__title'><p>견적요청일</p></div>";
                    //vhtml += "                     <div class='esti-cont__desc'><p>" + string(_fntonull(vresult[i]["req_ymd"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</p></div>";
                    //vhtml += "                 </div>";
                    //vhtml += "                 <div class='esti-cont__flex'>";
                    //vhtml += "                     <div class='esti-cont__title'><p>최종견적일</p></div>";
                    //vhtml += "                     <div class='esti-cont__desc'><p>" + string(_fntonull(vresult[i]["quot_ymd"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</p></div>";
                    //vhtml += "                 </div>";
                    //vhtml += "             </div>";
                    //vhtml += "             <div class='esti-cont__inner'>";
                    //vhtml += "                 <div class='esti-cont__flex'>";
                    //vhtml += "                     <div class='esti-cont__title'><p>품목명</p></div>";
                    //vhtml += "                     <div class='esti-cont__desc'><p>" + _fntonull(vresult[i]["item_nm"]) + "</p></div>";
                    //vhtml += "                 </div>";
                    //vhtml += "                 <div class='esti-cont__flex'>";
                    //vhtml += "                     <div class='esti-cont__title'><p>문서</p></div>";
                    //vhtml += "                     <div class='esti-cont__desc'><p>";
                    //if (_fntozero(vresult[i]["ci_cnt"]) == 1) {
                    //    vhtml += "                     <span class='esti-cont__doc'>c/i</span>";
                    //} else if (_fntozero(vresult[i]["ci_cnt"]) == 2) {
                    //    vhtml += "                     <span class='esti-cont__doc'>c/i</span>";
                    //    vhtml += "                     <span class='esti-cont__doc'>c/i</span>";
                    //}
                    //if (_fntozero(vresult[i]["pl_cnt"]) == 1) {
                    //    vhtml += "                     <span class='esti-cont__doc'>p/l</span></p>";
                    //} else if (_fntozero(vresult[i]["pl_cnt"]) == 2) {
                    //    vhtml += "                     <span class='esti-cont__doc'>p/l</span></p>";
                    //    vhtml += "                     <span class='esti-cont__doc'>p/l</span></p>";
                    //}
                    //vhtml += "                 </div > ";
                    //vhtml += "                 </div>";
                    //vhtml += "             </div>";
                    //vhtml += "         </div>";
                    //vhtml += "         <div class='esti-cont__box'>";
                    //vhtml += "             <div class='esti-cont__inner'>";
                    //vhtml += "                 <div class='esti-cont__flex'>";
                    //vhtml += "                     <div class='esti-cont__transit ship'>";
                    //vhtml += "                         <button type='button' class='btns esti-cont__circle seaquot'></button>";
                    //if (parseint(vresult[i].req_fwd_cnt) == parseint(vresult[i].fwd_quot_cnt)) {
                    //    vhtml += "                         <p style='display:none'>" + _fntonull(vresult[i].quot_no) + "</p>";
                    //    vhtml += "                         <p style='display:none'>" + i + "</p>";
                    //    vhtml += "                         <p style='display:none'>" + _fntonull(vresult[i].status) + "</p>";
                    //    vhtml += "                         <p>" + parseint(vresult[i].sea_cnt) + "</p>";
                    //} else {
                    //    vhtml += "                         <p style='display:none'>" + _fntonull(vresult[i].quot_no) + "</p>";
                    //    vhtml += "                         <p style='display:none'>" + i + "</p>";
                    //    vhtml += "                         <p style='display:none'>" + _fntonull(vresult[i].status) + "</p>";
                    //    vhtml += "                         <p>진행중</p>";
                    //}
                    //vhtml += "                     </div>";
                    //vhtml += "                     <div class='esti-cont__transit air'>";
                    //vhtml += "                         <button type='button' class='btns esti-cont__circle airquot'></button>";
                    //if (parseint(vresult[i].req_fwd_cnt) == parseint(vresult[i].fwd_quot_cnt)) {
                    //    vhtml += "                         <p style='display:none'>" + _fntonull(vresult[i].quot_no) + "</p>";
                    //    vhtml += "                         <p style='display:none'>" + i + "</p>";
                    //    vhtml += "                         <p style='display:none'>" + _fntonull(vresult[i].status) + "</p>";
                    //    vhtml += "                         <p>" + parseint(vresult[i].air_cnt) + "</p>";
                    //} else {
                    //    vhtml += "                         <p style='display:none'>" + _fntonull(vresult[i].quot_no) + "</p>";
                    //    vhtml += "                         <p style='display:none'>" + i + "</p>";
                    //    vhtml += "                         <p style='display:none'>" + _fntonull(vresult[i].status) + "</p>";
                    //    vhtml += "                         <p>진행중</p>";
                    //}
                    //vhtml += "                     </div>";
                    //vhtml += "                 </div>";
                    //vhtml += "             </div>";
                    //vhtml += "         </div>";
                    //vhtml += "         <div class='esti-cont__box'>";
                    //vhtml += "             <div class='esti-cont__inner'>";
                    //vhtml += "                 <button type='button' class='btns esti_dtl'>상세</button>";
                    //vhtml += "                         <p style='display:none'>" + _fntonull(vresult[i].quot_no) + "</p>";
                    //vhtml += "             </div>";
                    //vhtml += "         </div>";
                    //vhtml += "     </div>";
                    //vhtml += "     <div class='esti-detail' id='esti" + i + "'>";
                    //vhtml += "     </div>";
                    //vhtml += " </div>";
                //});

                //#endregion

            }
            else if (JSON.parse(rtnVal).Result[0]["trxCode"] == "N") { //데이터 없을 시 
                vHtml += "   <div class=\"exim-box no_data\"> ";
                vHtml += "   	<p><img src=\"/Images/no_data.png\" /></p> ";
                vHtml += "   </div> ";
            }
            $("#estiList").append(vHtml);
            $("#QUOT_NO").val("");

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
    //var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetQuotationList", objJsonData);

}


//포워더 메인리스트 조회
function fnFwdSearchData(vList) {
    var vHtml = "";
    var objJsonData = new Object();
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    if (_fnToNull($("#QUOT_NO").val()) != "") {
        objJsonData.QUOT_NO = _fnToNull($("#QUOT_NO").val());
    } else {

        objJsonData.CRN = $("#Session_CRN").val();
        if ($("#reqDate").prop("checked")) {
            objJsonData.YMD_TYPE = "REQ_YMD";
        } else if ($("#estDate").prop("checked")) {
            objJsonData.YMD_TYPE = "QUOT_YMD";
        }
        objJsonData.FM_YMD = $("#ETD").val().replace(/-/gi, '');
        objJsonData.TO_YMD = $("#ETA").val().replace(/-/gi, '');
        objJsonData.POL_CD = $("#input_POLCD").val();
        objJsonData.POD_CD = $("#input_PODCD").val();
    }
    $("#estiList").empty();

    $.ajax({
        type: "POST",
        url: "/Estimate/fnGetFwdQuotationList",
        async: true,
        dataType: "json",

        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (rtnVal) {

            if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") { // 데이터 있을 때
                var DataList = JSON.parse(rtnVal).Table1;

                vHtml = fnMakeFwdQuotList(DataList);

            }
            else if (JSON.parse(rtnVal).Result[0]["trxCode"] == "N") { //데이터 없을 시 
                vHtml += "   <div class=\"exim-box no_data\"> ";
                vHtml += "   	<p><img src=\"/Images/no_data.png\" /></p> ";
                vHtml += "   </div> ";
            }
            $("#estiList").append(vHtml);
            $("#QUOT_NO").val("");

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


//디테일 리스트 조회 함수
function fnSearchDtlList(element) {
    var td = element.children();
    var objParam = new Object();
    objParam.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
    objParam.CRN = _fnToNull($("#Session_CRN").val());
    objParam.QUOT_NO = td.eq(1).text().trim();
    objParam.REQ_SVC = td.eq(4).text().trim();
    var cnt = td.eq(2).text().trim();
    var status = td.eq(3).text().trim();
    var req_svc = td.eq(4).text().trim();
    $("#esti" + cnt).empty();
    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetQuotationDetailList", objParam);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;
        
        var strHtml = fnMakeQuotDtlList(vResult, status, req_svc);
        $("#esti" + cnt).append(strHtml);

        OpenDtlList(element);
    }
}

//#endregion


//#region ★★★★★★Drawing List★★★★★★

//메인리스트 그리기
function fnMakeQuotList(vList) {

    //#region 변수 선언
    var vResult = vList;
    var _vHtml = "";
    //#endregion


    $.each(vResult, function (i) {
        var state_text = "";

        if (parseInt(vResult[i].REQ_FWD_CNT) != parseInt(vResult[i].FWD_QUOT_CNT) || parseInt(vResult[i].FWD_QUOT_CNT) == 0) {
            _vHtml += "<div class='esti-cont present'>";
            state_text = "진행중";
        } else {
            _vHtml += "<div class='esti-cont finished'>";
            state_text = "견적완료";
        }

        //#region 리스트 메인 영역
        _vHtml += "  <div class='esti-cont__info'>";
        ////상태표기
        _vHtml += "      <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont__inner'>";
        _vHtml += "             <p class='esti-cont__stat'>" + state_text + "</p>";
        _vHtml += "         </div>";
        _vHtml += "      </div>";
        ////POL
        _vHtml += "      <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont__flex'>";
        _vHtml += "             <div class='esti-cont__desc'>";
        _vHtml += "                 <p>" + _fnToNull(vResult[i]["POL_NM"]) + "</p>";
        _vHtml += "             </div>";
        _vHtml += "             <div class='esti-cont__desc2'>";
        if (_fnToNull(vResult[i]["ETD"]) != "") { // ETD 있을 때 
            _vHtml += "                 <p>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "(" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ")" + "</p>";
        }
        else { //없을 때
            _vHtml += "                 <p style='visibility:hidden'>-</p>";
        }
        _vHtml += "             </div>";
        _vHtml += "         </div>";
        _vHtml += "      </div>";
        ////화살표
        _vHtml += "      <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont__inner'>";
        _vHtml += "             <p class='esti-cont__progress inquiry '>";
        _vHtml += "                 <img src='/Images/icn_progress.png'>";
        _vHtml += "             </p>";
        _vHtml += "         </div>";
        _vHtml += "      </div>";
        ////POD
        _vHtml += "      <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont__flex'>";
        _vHtml += "             <div class='esti-cont__desc'>";
        _vHtml += "                 <p>" + _fnToNull(vResult[i]["POD_NM"]) + "</p>";
        _vHtml += "             </div>";
        _vHtml += "             <div class='esti-cont__desc2'>";
        if (_fnToNull(vResult[i]["ETA"]) != "") { // ETD 있을 때 
            _vHtml += "                 <p>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "(" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ")" + "</p>";
        }
        else { //없을 때
            _vHtml += "                 <p style='visibility:hidden'>-</p>";
        }
        _vHtml += "             </div>";
        _vHtml += "         </div>";
        _vHtml += "      </div>";
        ////서브데이터
        _vHtml += "      <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont__inner inner-flex'>";
        _vHtml += "             <div class='esti-cont__date flex-column'>";
        _vHtml += "                 <div class='esti-cont__date_start'>";
        _vHtml += "                     <span class='esti-cont__date_title'>견적요청일</span>";
        _vHtml += "                     <span class='esti-cont__date_cont'>" + String(_fnToNull(vResult[i]["REQ_YMD"]).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3')) + "</span>" //데이터 바인딩 필요
        _vHtml += "                 </div>";
        _vHtml += "                 <div class='esti-cont__date_end'>";
        _vHtml += "                     <span class='esti-cont__date_title'>최종견적일</span>";
        _vHtml += "                     <span class='esti-cont__date_cont'>" + String(_fnToNull(vResult[i]["QUOT_YMD"]).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3')) + "</span>" //데이터 바인딩 필요
        _vHtml += "                 </div>";
        _vHtml += "             </div>";
        _vHtml += "             <div class='esti-cont__etc'>";
        _vHtml += "                 <div class='esti-cont__etc_item'>";
        _vHtml += "                     <span class='esti-cont__etc_title'>품목명</span>";
        _vHtml += "                     <span class='esti-cont__etc_cont'>" + _fnToNull(vResult[i]["ITEM_NM"]) + "</span>" //데이터 바인딩 필요
        _vHtml += "                 </div>";
        _vHtml += "                 <div class='esti-cont__date_file'>";
        _vHtml += "                     <span class='esti-cont__etc_title'>문서</span>";
        _vHtml += "                     <span class='esti-cont__etc_cont'>"
        var doc_cnt =0
        if (_fnToZero(vResult[i]["CI_CNT"]) == 1) {
            _vHtml += "                         <span class='doc_type1'><img src='/Images/icn_doc02.png'>C/I</span>";
            doc_cnt += 1;
        }
        else if (_fnToZero(vResult[i]["CI_CNT"]) == 2) {
            _vHtml += "                         <span class='doc_type1'><img src='/Images/icn_doc02.png'>C/I</span>";
            _vHtml += "                         <span class='doc_type2'><img src='/Images/icn_doc02.png'>C/I</span>";
            doc_cnt += 2;
        }

        if (_fnToZero(vResult[i]["PL_CNT"]) == 1) {
            if (doc_cnt == 1) {
                _vHtml += "                         <span class='doc_type2'><img src='/Images/icn_doc02.png'>P/L</span>";
            }
            else {
                _vHtml += "                         <span class='doc_type1'><img src='/Images/icn_doc02.png'>P/L</span>";
            }
            
            doc_cnt += 1;
        }
        else if (_fnToZero(vResult[i]["PL_CNT"]) == 2) {
            _vHtml += "                         <span class='doc_type1'><img src='/Images/icn_doc02.png'>P/L</span>";
            _vHtml += "                         <span class='doc_type2'><img src='/Images/icn_doc02.png'>P/L</span>";
            doc_cnt += 2;
        }

        if (doc_cnt == 1) {
            _vHtml += "                         <span class='doc_type2' style='visibility:hidden'><img src='/Images/icn_doc02.png'>No</span>";
        }
        else if (doc_cnt == 0) {
            _vHtml += "                         <span class='doc_type1' style='visibility:hidden'><img src='/Images/icn_doc02.png'>No</span>";
            _vHtml += "                         <span class='doc_type2' style='visibility:hidden'><img src='/Images/icn_doc02.png'>No</span>";
        }

        _vHtml += "                     </span>"
        _vHtml += "                 </div>";
        _vHtml += "             </div>";
        _vHtml += "         </div>";
        _vHtml += "      </div>";
        ////서브(항공+ 해운) 버튼
        _vHtml += "      <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont-flex'>";
        //#region 해운버튼
        _vHtml += "             <div class='esti-cont__transit ship'>";
        _vHtml += "                 <button type='button' class='btns esti-cont__circle seaQuot'>";
        _vHtml += "                     <image src='/Images/icn_ship.png'>";
        _vHtml += "                     <p style='display:none'>" + _fnToNull(vResult[i]["QUOT_NO"]) + "</p>";
        _vHtml += "                     <p style='display:none'>" + i + "</p>";
        _vHtml += "                     <p style='display:none'>" + _fnToNull(vResult[i]["STATUS"]) + "</p>";
        _vHtml += "                     <p style='display:none'>SEA</p>";
        if (state_text == "진행중") {
            _vHtml += "                     <p>" + state_text; "</p>"
        }
        else {

            _vHtml += "                     <p>" + _fnToZero(vResult[i]["SEA_CNT"]).toString() + "건</p>"
            
        }
        
        _vHtml += "                 </button>";
        _vHtml += "             </div>";
        //#endregion

        //#region 항공버튼
        _vHtml += "                 <div class='esti-cont__transit air'>";
        _vHtml += "                     <button type='button' class='btns esti-cont__circle airQuot'>";
        _vHtml += "                         <image src='/Images/icn_air.png'>";
        _vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i]["QUOT_NO"]) + "</p>";
        _vHtml += "                         <p style='display:none'>" + i + "</p>";
        _vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i]["STATUS"]) + "</p>";
        _vHtml += "                     <p style='display:none'>AIR</p>";
        if (state_text == "진행중") {
            _vHtml += "                         <p>" + state_text; "</p>"
        }
        else {

            _vHtml += "                         <p>" + _fnToZero(vResult[i]["AIR_CNT"]).toString() + "건</p>"

        }

        _vHtml += "                 </button>";
        
        _vHtml += "             </div>";
        //#endregion
        _vHtml += "         </div>";
        _vHtml += "      </div>";
        ////상세 버튼
        _vHtml += "      <div class='esti-cont__box inquiry'>";
        _vHtml += "          <div class='esti-cont__inner'>";
        _vHtml += "              <button type='button' class='btns esti_dtl'>상세</button>";
        _vHtml += "              <p style='display:none'>" + _fnToNull(vResult[i]["QUOT_NO"]) + "</p>";
        
        _vHtml += "          </div>";
        _vHtml += "      </div>";
        _vHtml += "  </div>";
        //#endregion

        //#region 상세 영역
        _vHtml += " <div class='esti-detail' id ='esti"+i+"'>";
        _vHtml += " </div>";
        //#endregion 

        _vHtml += "</div> "


    });

    return _vHtml;
}


//항목별 디테일 그리기
function fnMakeQuotDtlList(vList, state, req_svc) {

    //#region 변수 선언
    var vResult = vList;
    var _vHtml = "";
    var Liststatus = state;
    var SeaAir = req_svc;
    //#endregion

    //$.each(vResult, function (i) {
    //    var curr = _fnToNull(vResult[i].CURR_CD).split(",");
    //    var fare_loc_amt = _fnToNull(vResult[i].FARE_LOC_AMT).split(",");
    //    var fare_amt = _fnToNull(vResult[i].FARE_AMT).split(",");

    //    _vHtml += "	<div class='esti-detail__list'>	";

    //    _vHtml += "        <div class='esti-detail__box'>";
    //    _vHtml += "            <div class='esti-detail__inner'>";
    //    _vHtml += "                <div class='esti-detail__transit'>";
    //    if (_fnToNull(SeaAir) == "SEA") {
    //        _vHtml += "                    <img src='/Images/icn_ship02.png' />";
    //    }
    //    else {
    //        _vHtml += "                    <img src='/Images/icn_air02.png' />";
    //    }
        
    //    _vHtml += "                </div>";
    //    _vHtml += "            </div>";
    //    _vHtml += "            <div class='esti-detail__inner'>";
    //    _vHtml += "                <span class='esti-detail__dot'>상호명</span>";
    //    _vHtml += "                <span>" + _fnToNull(vResult[i].FWD_OFFICE_NM) + "</span>";
    //    _vHtml += "            </div>";
    //    _vHtml += "            <div class='esti-detail__inner'>";
    //    _vHtml += "                <span class='esti-detail__dot'>담당자</span>";
    //    _vHtml += "                <span>" + _fnToNull(vResult[i].FWD_PIC_NM) + "</span>";
    //    _vHtml += "            </div>";
    //    _vHtml += "            <div class='esti-detail__inner'>";
    //    _vHtml += "                <span class='esti-detail__dot'>연락처</span>";
    //    _vHtml += "                <span>" + _fnToNull(vResult[i].FWD_PIC_TEL) + "</span>";
    //    _vHtml += "            </div>";
    //    _vHtml += "            <div class='esti-detail__inner'>";
    //    _vHtml += "                <span class='esti-detail__dot'>이메일</span>";
    //    _vHtml += "                <span>" + _fnToNull(vResult[i].FWD_PIC_EMAIL) + "</span>";
    //    _vHtml += "            </div>";
    //    _vHtml += "        </div>";

    //    _vHtml += "        <div class='esti-detail__box'>";
    //    _vHtml += "            <div class='esti-detail__inner'>";
    //    _vHtml += "                <div class='esti-detail__cost'>";
    //    _vHtml += "                    <p class='esti-detail__title'>견적금액</p>";
    //    for (var j = 0; j < curr.length; j++) {
    //        if (_fnToNull(curr[j]) == "KRW") {
    //            if (j == 0) {
    //                _vHtml += "                    <p>&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_loc_amt[j]), "sum")) + "</span>";
    //            } else {
    //                _vHtml += "                    <p>&nbsp;|&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_loc_amt[j]), "sum")) + "</span>";
    //            }
    //        } else {
    //            if (j == 0) {
    //                _vHtml += "                    <p>&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_amt[j]), "sum")) + "</span>";
    //            } else {
    //                _vHtml += "                    <p>&nbsp;|&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_amt[j]), "sum")) + "</span>";
    //            }
    //        }
    //    }
    //    _vHtml += "                    </p > ";
    //    _vHtml += "                </div>";
    //    _vHtml += "            </div>";
    //    _vHtml += "        </div>";

    //    _vHtml += "        <div class='esti-detail__box'>";
    //    if (Liststatus == "N") {
    //        _vHtml += "            <button type='button' class='btns esti_booking'>부킹요청</button>";
    //        _vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].QUOT_NO) + "</p>";
    //        _vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].MNGT_NO) + "</p>";
    //        _vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].CRN) + "</p>";

    //    }
    //    else {
    //        _vHtml += "            <button type='button' class='btns esti_booking' style='visibility:hidden'>-</button>";
    //    }
    //    _vHtml += "        </div>";
    //    _vHtml += "    </div>";
    //});

     $.each(vResult, function (i) {
        var curr = _fnToNull(vResult[i].CURR_CD).split(",");
        var fare_loc_amt = _fnToNull(vResult[i].FARE_LOC_AMT).split(",");
        var fare_amt = _fnToNull(vResult[i].FARE_AMT).split(",");

        _vHtml += "	<div class='esti-detail__list'>	";

        _vHtml += "        <div class='esti-detail__box'>";
        _vHtml += "            <div class='esti-detail__inner'>";
        _vHtml += "                <div class='esti-detail__transit'>";
        if (_fnToNull(SeaAir) == "SEA") {
            _vHtml += "                    <img src='/Images/icn_ship02.png' />";
        }
        else {
            _vHtml += "                    <img src='/Images/icn_air02.png' />";
        }
        
        _vHtml += "                </div>";
        _vHtml += "            </div>";
        _vHtml += "            <div class='esti-detail__inner'>";
        _vHtml += "                <span class='esti-detail__dot'>상호명</span>";
        _vHtml += "                <span>" + _fnToNull(vResult[i].FWD_OFFICE_NM) + "</span>";
        _vHtml += "            </div>";
        _vHtml += "            <div class='esti-detail__inner'>";
        _vHtml += "                <span class='esti-detail__dot'>담당자</span>";
        _vHtml += "                <span>" + _fnToNull(vResult[i].FWD_PIC_NM) + "</span>";
        _vHtml += "            </div>";
        _vHtml += "            <div class='esti-detail__inner'>";
        _vHtml += "                <span class='esti-detail__dot'>연락처</span>";
        _vHtml += "                <span>" + _fnToNull(vResult[i].FWD_PIC_TEL) + "</span>";
        _vHtml += "            </div>";
        _vHtml += "            <div class='esti-detail__inner'>";
        _vHtml += "                <span class='esti-detail__dot'>이메일</span>";
        _vHtml += "                <span>" + _fnToNull(vResult[i].FWD_PIC_EMAIL) + "</span>";
        _vHtml += "            </div>";
        _vHtml += "        </div>";

        _vHtml += "        <div class='esti-detail__box'>";
        _vHtml += "            <div class='esti-detail__inner'>";
        _vHtml += "                <div class='esti-detail__cost'>";
        _vHtml += "                    <p class='esti-detail__title'>견적금액</p>";
        for (var j = 0; j < curr.length; j++) {
            if (_fnToNull(curr[j]) == "KRW") {
                if (j == 0) {
                    _vHtml += "                    <p>&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_loc_amt[j]), "sum")) + "</span>";
                } else {
                    _vHtml += "                    <p>&nbsp;|&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_loc_amt[j]), "sum")) + "</span>";
                }
            } else {
                if (j == 0) {
                    _vHtml += "                    <p>&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_amt[j]), "sum")) + "</span>";
                } else {
                    _vHtml += "                    <p>&nbsp;|&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_amt[j]), "sum")) + "</span>";
                }
            }
        }
        _vHtml += "                    </p > ";
        _vHtml += "                </div>";
        _vHtml += "            </div>";
        _vHtml += "        </div>";

        _vHtml += "        <div class='esti-detail__box'>";
        if (Liststatus == "N") {
            _vHtml += "            <button type='button' class='btns esti_booking'>부킹요청</button>";
            _vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].QUOT_NO) + "</p>";
            _vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].MNGT_NO) + "</p>";
            _vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].CRN) + "</p>";

        }
        _vHtml += "        </div>";
        _vHtml += "    </div>";
    });

    return _vHtml;
    



}


//포워더 메인리스트 그리기
function fnMakeFwdQuotList(vList) {
    //#region 변수 선언
    var vResult = vList;
    var _vHtml = "";
    //#endregion

    $.each(vResult, function (i) {
        var curr = _fnToNull(vResult[i].CURR_CD).split(",");
        var fare_loc_amt = _fnToNull(vResult[i].FARE_LOC_AMT).split(",");
        var fare_amt = _fnToNull(vResult[i].FARE_AMT).split(",");
        var curr_txt = "";
        _vHtml += "<div class='esti-cont excution_inquiry'>";
        _vHtml += " <div class='esti-cont__info'>";
        //pol
        _vHtml += "     <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont__inner'>";
        _vHtml += "             <div class='esti-cont__flex'>";
        _vHtml += "                 <div class='esti-cont__desc'>";
        _vHtml += "                     <p>"+_fnToNull(vResult[i]["POL_NM"])+"</p>";
        _vHtml += "                 </div>";
        _vHtml += "                 <div class='esti-cont__desc2'>";
        if (_fnToNull(vResult[i]["ETD"]) != "") { // ETD 있을 때 
            _vHtml += "                 <p>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "(" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ")" + "</p>";
        }
        else { //없을 때
            _vHtml += "                 <p style='visibility:hidden'>-</p>";
        }
        _vHtml += "                 </div>";
        _vHtml += "             </div>";
        _vHtml += "         </div>";
        _vHtml += "     </div>";
        //화살표
        _vHtml += "     <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont__inner'>";
        _vHtml += "             <p class='esti-cont__progress inquiry'><img src='/Images/icn_progress.png'></p>";
        _vHtml += "         </div>";
        _vHtml += "     </div>";
        //pod
        _vHtml += "     <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont__inner'>";
        _vHtml += "             <div class='esti-cont__flex'>";
        _vHtml += "                 <div class='esti-cont__desc'>";
        _vHtml += "                     <p>" + _fnToNull(vResult[i]["POD_NM"]) + "</p>";
        _vHtml += "                 </div>";
        _vHtml += "                 <div class='esti-cont__desc2'>";
        if (_fnToNull(vResult[i]["ETA"]) != "") { // ETD 있을 때 
            _vHtml += "                 <p>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "(" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ")" + "</p>";
        }
        else { //없을 때
            _vHtml += "                 <p style='visibility:hidden'>-</p>";
        }
        _vHtml += "                 </div>";
        _vHtml += "             </div>";
        _vHtml += "         </div>";
        _vHtml += "     </div>";

        //기타 정보
        _vHtml += "     <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont__inner inner-flex'>";
        _vHtml += "             <div class='esti-cont__date flex-column'>";
        _vHtml += "                 <div class='esti-cont__date_start'>";
        _vHtml += "                     <span class='esti-cont__date_title'>견적요청일</span>";
        _vHtml += "                     <span class='esti-cont__date_cont'>" + String(_fnToNull(vResult[i]["REQ_YMD"]).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3'))+"</span>";
        _vHtml += "                 </div>";
        _vHtml += "                 <div class='esti-cont__date_end'>";
        _vHtml += "                     <span class='esti-cont__date_title'>최종견적일</span>";
        _vHtml += "                     <span class='esti-cont__date_cont'>" + String(_fnToNull(vResult[i]["QUOT_YMD"]).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3'))+"</span>";
        _vHtml += "                 </div>";
        _vHtml += "             </div>";
        _vHtml += "             <div class='esti-cont__etc'>";
        _vHtml += "                 <div class='esti-cont__etc_item'>";
        _vHtml += "                     <span class='esti-cont__etc_title'>품목명</span>";
        _vHtml += "                     <span class='esti-cont__etc_cont'>"+_fnToNull(vResult[i]["ITEM_NM"])+"</span>";
        _vHtml += "                 </div>";
        _vHtml += "                 <div class='esti-cont__etc_item'>";
        _vHtml += "                     <span class='esti-cont__etc_title'>담당자</span>";
        _vHtml += "                     <span class='esti-cont__etc_cont'>" +_fnToNull(vResult[i]["CUST_PIC_NM"])+"</span>" ;
        _vHtml += "                 </div>";
        _vHtml += "             </div>";
        _vHtml += "         </div>";
        _vHtml += "     </div>";

        //견적액 
        _vHtml += "     <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont__inner'>";
        _vHtml += "             <div class='esti-cont__etc'>";
        _vHtml += "                 <div class='esti-cont__etc_item'>";
        _vHtml += "                     <span class='esti-cont__etc_title'>견적금액</span>";
        _vHtml += "                     <span class='esti-cont__etc_cont amount'>";
        if (curr[0] != "") {
            for (j = 0; j < curr.length; j++) {
                if (_fnToNull(curr[j]) == "KRW") {
                    if (j == 0) {
                        _vHtml += "                    " + _fnToNull(curr[j]) + ":" + _fnToNull(_fnGetNumber(parseInt(fare_loc_amt[j]), "sum"));
                    } else {
                        _vHtml += "                    | " + _fnToNull(curr[j]) + ":" + _fnToNull(_fnGetNumber(parseInt(fare_loc_amt[j]), "sum"));
                    }
                } else {
                    if (j == 0) {
                        _vHtml += "                    " + _fnToNull(curr[j]) + ":" + _fnToNull(_fnGetNumber(parseInt(fare_amt[j]), "sum"));
                    } else {
                        _vHtml += "                    | " + _fnToNull(curr[j]) + ":" + _fnToNull(_fnGetNumber(parseInt(fare_amt[j]), "sum"));
                    }
                }
            }
        }
       
        _vHtml += "                     </span>";

        _vHtml += "                 </div>";
        _vHtml += "             </div>";
        _vHtml += "         </div>";
        _vHtml += "     </div>";

        //버튼
        _vHtml += "     <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont__inner'>";
        _vHtml += "             <button type='button' class='btns inqu_dtl'>상세</button>";
        _vHtml += "             <input type='hidden' value= '"+_fnToNull(vResult[i]["MNGT_NO"])+"'>";
        _vHtml += "         </div>";
        _vHtml += "     </div>";

        _vHtml += " </div>";
        _vHtml += "</div>";
    });

    return _vHtml;
}


//#endregion
