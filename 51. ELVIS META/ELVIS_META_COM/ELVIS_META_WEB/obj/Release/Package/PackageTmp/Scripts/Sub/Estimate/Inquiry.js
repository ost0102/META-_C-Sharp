////////////////////전역 변수//////////////////////////
var _vSelectDate = new Date();
var obj = new Object();
var mymap;
////////////////////jquery event///////////////////////
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

    $("#ProgressBar_Loading").show(); //프로그래스 바
    setTimeout(function () {
        $("#ProgressBar_Loading").hide(); //프로그래스 바
    }, 1000);

    fnSearchData();

});

//상세사항 펼치기
$(document).on("click", ".esti-cont__circle", function () {
    var $btnYN = $(this).closest(".esti-cont__transit").siblings(".esti-cont__transit").find(".esti-cont__circle");
    if ($btnYN.hasClass("on")) {
        $btnYN.removeClass("on");
        $(this).addClass("on");
    }
    else if ($(this).hasClass("on")) {
        $(this).removeClass("on");
        $(this).closest(".esti-cont__info").siblings(".esti-detail").slideToggle();
    }
    else {
        $(this).addClass("on");
        $(this).closest(".esti-cont__info").siblings(".esti-detail").slideToggle();
        $(this).closest(".esti-cont")[0].scrollIntoView({behavior: "smooth"});
    }
})

$("#btn_search").click(function (e) {
    fnSearchData();
});


$("#estDate").click(function (e) {
    fnSearchData();
});


$("#reqDate").click(function (e) {
    fnSearchData();
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


//function fnSearchData() {
//    var vHtml = "";
//    var objJsonData = new Object();
//        objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
//    if (_fnToNull($("#QUOT_NO").val()) != "") {
//        objJsonData.QUOT_NO = _fnToNull($("#QUOT_NO").val());
//    } else {
        
//        objJsonData.CRN = $("#Session_CRN").val();
//        if ($("#reqDate").prop("checked")) {
//            objJsonData.YMD_TYPE = "REQ_YMD";
//        } else if ($("#estDate").prop("checked")) {
//            objJsonData.YMD_TYPE = "QUOT_YMD";
//        }
//        objJsonData.FM_YMD = $("#ETD").val().replace(/-/gi, '');
//        objJsonData.TO_YMD = $("#ETA").val().replace(/-/gi, '');
//        objJsonData.POL_CD = $("#input_POLCD").val();
//        objJsonData.POD_CD = $("#input_PODCD").val();
//    }
//    $("#estiList").empty();
//    $.ajax({
//        type: "POST",
//        url: "/Estimate/fnGetQuotationList",
//        async: true,
//        dataType: "json",
//        //data: callObj,
//        data: { "vJsonData": _fnMakeJson(objJsonData) },
//        success: function (rtnVal) {
//            if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
//                var vResult = JSON.parse(rtnVal).Table1;

//                $.each(vResult, function (i) {
//                    if (parseInt(vResult[i].REQ_FWD_CNT) != parseInt(vResult[i].FWD_QUOT_CNT)) {
//                        vHtml += "<div class='esti-cont present'>";
//                    } else {
//                        vHtml += "<div class='esti-cont finished'>";
//                    }
//                    vHtml += "     <div class='esti-cont__info'>";
//                    vHtml += "         <div class='esti-cont__box'>";
//                    vHtml += "             <div class='esti-cont__inner'>";
//                    vHtml += "                 <div class='esti-cont__flex'>";
//                    vHtml += "                     <div class='esti-cont__title'><p>출발</p></div>";
//                    vHtml += "                     <div class='esti-cont__desc'><p>" + _fnToNull(vResult[i].POL_NM);
//                    if (_fnToNull(vResult[i]["ETD"]) != "") {
//                        vHtml += " <span>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ")" + "</span>";
//                    }
//                    vHtml += " </p></div> ";
//                    vHtml += "                 </div>";
//                    vHtml += "                 <div class='esti-cont__flex'>";
//                    vHtml += "                     <div class='esti-cont__title'><p>도착</p></div>";
//                    vHtml += "                     <div class='esti-cont__desc'><p>" + _fnToNull(vResult[i].POD_NM);
//                    if (_fnToNull(vResult[i]["ETA"]) != "") {
//                        vHtml += " <span>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ")" + "</span>";
//                    }
//                    vHtml += " </p></div> ";
//                    vHtml += "                 </div>";
//                    vHtml += "             </div>";
//                    vHtml += "         </div>";
//                    vHtml += "         <div class='esti-cont__box flex'>";
//                    vHtml += "             <div class='esti-cont__inner'>";
//                    vHtml += "                 <div class='esti-cont__flex'>";
//                    vHtml += "                     <div class='esti-cont__title'><p>견적요청일</p></div>";
//                    vHtml += "                     <div class='esti-cont__desc'><p>" + String(_fnToNull(vResult[i]["REQ_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</p></div>";
//                    vHtml += "                 </div>";
//                    vHtml += "                 <div class='esti-cont__flex'>";
//                    vHtml += "                     <div class='esti-cont__title'><p>최종견적일</p></div>";
//                    vHtml += "                     <div class='esti-cont__desc'><p>" + String(_fnToNull(vResult[i]["QUOT_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</p></div>";
//                    vHtml += "                 </div>";
//                    vHtml += "             </div>";
//                    vHtml += "             <div class='esti-cont__inner'>";
//                    vHtml += "                 <div class='esti-cont__flex'>";
//                    vHtml += "                     <div class='esti-cont__title'><p>품목명</p></div>";
//                    vHtml += "                     <div class='esti-cont__desc'><p>" + _fnToNull(vResult[i]["ITEM_NM"]) + "</p></div>";
//                    vHtml += "                 </div>";
//                    vHtml += "                 <div class='esti-cont__flex'>";
//                    vHtml += "                     <div class='esti-cont__title'><p>문서</p></div>";
//                    vHtml += "                     <div class='esti-cont__desc'><p>";
//                    if (_fnToZero(vResult[i]["CI_CNT"]) == 1) {
//                        vHtml += "                     <span class='esti-cont__doc'>C/I</span>";
//                    } else if (_fnToZero(vResult[i]["CI_CNT"]) == 2) {
//                        vHtml += "                     <span class='esti-cont__doc'>C/I</span>";
//                        vHtml += "                     <span class='esti-cont__doc'>C/I</span>";
//                    }
//                    if (_fnToZero(vResult[i]["PL_CNT"]) == 1) {
//                        vHtml += "                     <span class='esti-cont__doc'>P/L</span></p>";
//                    } else if (_fnToZero(vResult[i]["PL_CNT"]) == 2) {
//                        vHtml += "                     <span class='esti-cont__doc'>P/L</span></p>";
//                        vHtml += "                     <span class='esti-cont__doc'>P/L</span></p>";
//                    }
//                    vHtml += "                 </div > ";
//                    vHtml += "                 </div>";
//                    vHtml += "             </div>";
//                    vHtml += "         </div>";
//                    vHtml += "         <div class='esti-cont__box'>";
//                    vHtml += "             <div class='esti-cont__inner'>";
//                    vHtml += "                 <div class='esti-cont__flex'>";
//                    vHtml += "                     <div class='esti-cont__transit ship'>";
//                    vHtml += "                         <button type='button' class='btns esti-cont__circle seaQuot'></button>";
//                    if (parseInt(vResult[i].REQ_FWD_CNT) == parseInt(vResult[i].FWD_QUOT_CNT)) {
//                        vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].QUOT_NO) + "</p>";
//                        vHtml += "                         <p style='display:none'>" + i + "</p>";
//                        vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].STATUS) + "</p>";
//                        vHtml += "                         <p>" + parseInt(vResult[i].SEA_CNT) + "</p>";
//                    } else {
//                        vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].QUOT_NO) + "</p>";
//                        vHtml += "                         <p style='display:none'>" + i + "</p>";
//                        vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].STATUS) + "</p>";
//                        vHtml += "                         <p>진행중</p>";
//                    }
//                    vHtml += "                     </div>";
//                    vHtml += "                     <div class='esti-cont__transit air'>";
//                    vHtml += "                         <button type='button' class='btns esti-cont__circle airQuot'></button>";
//                    if (parseInt(vResult[i].REQ_FWD_CNT) == parseInt(vResult[i].FWD_QUOT_CNT)) {
//                        vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].QUOT_NO) + "</p>";
//                        vHtml += "                         <p style='display:none'>" + i + "</p>";
//                        vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].STATUS) + "</p>";
//                        vHtml += "                         <p>" + parseInt(vResult[i].AIR_CNT) + "</p>";
//                    } else {
//                        vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].QUOT_NO) + "</p>";
//                        vHtml += "                         <p style='display:none'>" + i + "</p>";
//                        vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].STATUS) + "</p>";
//                        vHtml += "                         <p>진행중</p>";
//                    }
//                    vHtml += "                     </div>";
//                    vHtml += "                 </div>";
//                    vHtml += "             </div>";
//                    vHtml += "         </div>";
//                    vHtml += "         <div class='esti-cont__box'>";
//                    vHtml += "             <div class='esti-cont__inner'>";
//                    vHtml += "                 <button type='button' class='btns esti_dtl'>상세</button>";
//                    vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].QUOT_NO) + "</p>";
//                    vHtml += "             </div>";
//                    vHtml += "         </div>";
//                    vHtml += "     </div>";
//                    vHtml += "     <div class='esti-detail' id='esti" + i + "'>";
//                    vHtml += "     </div>";
//                    vHtml += " </div>";
//                });

//                $("#estiList").append(vHtml);
//                $("#QUOT_NO").val("");
//            } else if (JSON.parse(rtnVal).Result[0]["trxCode"] == "N") {
//                vHtml += "   <div class=\"exim-box no_data\"> ";
//                vHtml += "   	<p><img src=\"/Images/no_data.png\" /></p> ";
//                vHtml += "   </div> ";
//            }
//        }, error: function (xhr, status, error) {
//            $("#ProgressBar_Loading").hide(); //프로그래스 바
//            _fnAlertMsg("담당자에게 문의 하세요.");
//            console.log(error);
//        },
//        beforeSend: function () {
//           // $("#ProgressBar_Loading").show(); //프로그래스 바
//        },
//        complete: function () {
//           // $("#ProgressBar_Loading").hide(); //프로그래스 바
//        }
//    });
//    //var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetQuotationList", objJsonData);

//}


//$(document).on("click", ".esti_dtl", function () {
//    var tr = $(this).closest('div');
//    var td = tr.children();
//    var objParam = new Object();
//    objParam.QUOT_NO = td.eq(1).text().trim();
  
//    controllerToLink("InquiryDetail", "Estimate", objParam, false);

//});

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



$(document).on("click", ".seaQuot", function () {
    var vHtml = "";
    var tr = $(this).closest('div');
    var td = tr.children();
    var objParam = new Object();
    objParam.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
    objParam.CRN = _fnToNull($("#Session_CRN").val());
    objParam.QUOT_NO = td.eq(1).text().trim();
    objParam.REQ_SVC = "SEA";
    var cnt = td.eq(2).text().trim();
    var status = td.eq(3).text().trim();
    $("#esti" + cnt).empty();
    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetQuotationDetailList", objParam);
    //if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
    //    var vResult = JSON.parse(rtnVal).Table1;

    //    $.each(vResult, function (i) {
    //        var curr = _fnToNull(vResult[i].CURR_CD).split(",");
    //        var fare_loc_amt = _fnToNull(vResult[i].FARE_LOC_AMT).split(",");
    //        var fare_amt = _fnToNull(vResult[i].FARE_AMT).split(",");

    //        vHtml += "	<div class='esti-detail__list'>	";
    //        vHtml += "        <div class='esti-detail__box'>";
    //        vHtml += "            <div class='esti-detail__inner'>";
    //        vHtml += "                <div class='esti-detail__transit'>";
    //        vHtml += "                    <img src='/Images/icn_ship02.png' />";
    //        vHtml += "                </div>";
    //        vHtml += "            </div>";
    //        vHtml += "            <div class='esti-detail__inner'>";
    //        vHtml += "                <span class='esti-detail__dot'>상호명</span>";
    //        vHtml += "                <span>" + _fnToNull(vResult[i].FWD_OFFICE_NM) + "</span>";
    //        vHtml += "            </div>";
    //        vHtml += "            <div class='esti-detail__inner'>";
    //        vHtml += "                <span class='esti-detail__dot'>담당자</span>";
    //        vHtml += "                <span>" + _fnToNull(vResult[i].FWD_PIC_NM) + "</span>";
    //        vHtml += "            </div>";
    //        vHtml += "            <div class='esti-detail__inner'>";
    //        vHtml += "                <span class='esti-detail__dot'>연락처</span>";
    //        vHtml += "                <span>" + _fnToNull(vResult[i].FWD_PIC_TEL) + "</span>";
    //        vHtml += "            </div>";
    //        vHtml += "            <div class='esti-detail__inner'>";
    //        vHtml += "                <span class='esti-detail__dot'>이메일</span>";
    //        vHtml += "                <span>" + _fnToNull(vResult[i].FWD_PIC_EMAIL) + "</span>";
    //        vHtml += "            </div>";
    //        vHtml += "        </div>";
    //        vHtml += "        <div class='esti-detail__box'>";
    //        vHtml += "            <div class='esti-detail__inner'>";
    //        vHtml += "                <div class='esti-detail__cost'>";
    //        vHtml += "                    <p class='esti-detail__title'>견적금액</p>";
    //        for (var j = 0; j < curr.length; j++) {
    //            if (_fnToNull(curr[j]) == "KRW") {
    //                if (j == 0) {
    //                    vHtml += "                    <p>&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_loc_amt[j]), "sum")) + "</span>";
    //                } else {
    //                    vHtml += "                    <p>&nbsp;|&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_loc_amt[j]), "sum")) + "</span>";
    //                }
    //            } else {
    //                if (j == 0) {
    //                    vHtml += "                    <p>&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_amt[j]), "sum")) + "</span>";
    //                } else {
    //                    vHtml += "                    <p>&nbsp;|&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_amt[j]), "sum")) + "</span>";
    //                }
    //            }
    //        }
    //        vHtml += "                      </p > ";
    //        vHtml += "                </div>";
    //        vHtml += "            </div>";
    //        vHtml += "        </div>";
    //        vHtml += "        <div class='esti-detail__box'>";
    //        if (status == "N") {
    //            vHtml += "            <button type='button' class='btns esti_booking'>부킹요청</button>";
    //            vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].QUOT_NO) + "</p>";
    //            vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].MNGT_NO) + "</p>";
    //            vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].CRN) + "</p>";
               
    //        }
    //        vHtml += "        </div>";
    //        vHtml += "    </div>";
    //    });
    //    $("#esti" + cnt).append(vHtml);
    //}
   
});


$(document).on("click", ".airQuot", function () {
    var vHtml = "";
    var tr = $(this).closest('div');
    var td = tr.children();
    var objParam = new Object();
    objParam.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
    objParam.CRN = _fnToNull($("#Session_CRN").val());
    objParam.QUOT_NO = td.eq(1).text().trim();
    objParam.REQ_SVC = "AIR";
    var cnt = td.eq(2).text().trim();
    var status = td.eq(3).text().trim();
    $("#esti" + cnt).empty();
    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetQuotationDetailList", objParam);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;

        $.each(vResult, function (i) {
            var curr = _fnToNull(vResult[i].CURR_CD).split(",");
            var fare_loc_amt = _fnToNull(vResult[i].FARE_LOC_AMT).split(",");
            var fare_amt = _fnToNull(vResult[i].FARE_AMT).split(",");

            vHtml += "	<div class='esti-detail__list'>	";
            vHtml += "        <div class='esti-detail__box'>";
            vHtml += "            <div class='esti-detail__inner'>";
            vHtml += "                <div class='esti-detail__transit'>";
            vHtml += "                    <img src='/Images/icn_air02.png' />";
            vHtml += "                </div>";
            vHtml += "            </div>";
            vHtml += "            <div class='esti-detail__inner'>";
            vHtml += "                <span class='esti-detail__dot'>상호명</span>";
            vHtml += "                <span>" + _fnToNull(vResult[i].FWD_OFFICE_NM) + "</span>";
            vHtml += "            </div>";
            vHtml += "            <div class='esti-detail__inner'>";
            vHtml += "                <span class='esti-detail__dot'>담당자</span>";
            vHtml += "                <span>" + _fnToNull(vResult[i].FWD_PIC_NM) + "</span>";
            vHtml += "            </div>";
            vHtml += "            <div class='esti-detail__inner'>";
            vHtml += "                <span class='esti-detail__dot'>연락처</span>";
            vHtml += "                <span>" + _fnToNull(vResult[i].FWD_PIC_TEL) + "</span>";
            vHtml += "            </div>";
            vHtml += "            <div class='esti-detail__inner'>";
            vHtml += "                <span class='esti-detail__dot'>이메일</span>";
            vHtml += "                <span>" + _fnToNull(vResult[i].FWD_PIC_EMAIL) + "</span>";
            vHtml += "            </div>";
            vHtml += "        </div>";
            vHtml += "        <div class='esti-detail__box'>";
            vHtml += "            <div class='esti-detail__inner'>";
            vHtml += "                <div class='esti-detail__cost'>";
            vHtml += "                    <p class='esti-detail__title'>견적금액</p>";
            for (var j = 0; j < curr.length; j++) {
                if (_fnToNull(curr[j]) == "KRW") {
                    if (j == 0) {
                        vHtml += "                    <p>&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_loc_amt[j]), "sum")) + "</span>";
                    } else {
                        vHtml += "                    <p>&nbsp;|&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_loc_amt[j]), "sum")) + "</span>";
                    }
                } else {
                    if (j == 0) {
                        vHtml += "                    <p>&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_amt[j]), "sum")) + "</span>";
                    } else {
                        vHtml += "                    <p>&nbsp;|&nbsp;" + _fnToNull(curr[j]) + " : <span>" + _fnToNull(_fnGetNumber(parseInt(fare_amt[j]), "sum")) + "</span>";
                    }
                }
            }
            vHtml += "                      </p> ";
            vHtml += "                </div>";
            vHtml += "            </div>";
            vHtml += "        </div>";
            vHtml += "        <div class='esti-detail__box'>";
            if (status == "N") {
                vHtml += "            <button type='button' class='btns esti_booking'>부킹요청</button>";
                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].QUOT_NO) + "</p>";
                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].MNGT_NO) + "</p>";
                vHtml += "                         <p style='display:none'>" + _fnToNull(vResult[i].CRN) + "</p>";

            }
            vHtml += "        </div>";
            vHtml += "    </div>";
        });
        $("#esti" + cnt).append(vHtml);
    }

});
/////////////////////1unction///////////////////////////////////

//////////////////////function makelist////////////////////////

/////////////////////////////API///////////////////////////////
