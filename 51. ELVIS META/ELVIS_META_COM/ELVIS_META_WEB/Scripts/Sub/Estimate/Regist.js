//#region ★★★★★★전역 변수★★★★★★
var _vSelectDate = new Date();
var obj = new Object();
var mymap;
var _vHtml = "";
var doc_cnt = 0;
//#endregion



//#region ★★★★★★시작영역 ★★★★★★
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    $(".sub_esti").addClass("on");
    $(".sub_esti .sub_depth").addClass("on");
    $(".sub_esti .sub_depth li:first-child a").addClass("on");


    //$("#ETD").val(_fnMinusDate(7));
    $("#ETD").val("2022-11-01");
    $("#ETA").val(_fnPlusDate(7));

    //$("#ProgressBar_Loading").show(); //프로그래스 바
    //setTimeout(function () {
    //    $("#ProgressBar_Loading").hide(); //프로그래스 바
    //}, 1000);

    fnSearchData();

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


//조회 버튼 클릭
$(document).on("click", "#btn_search", function () {
    

    fnSearchData();

    //console.log("click the 'Search btn'");
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
                        } else {
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
                    if (parseFloat(vIndex) == 4) {
                        fnSearchData();
                    }
                    else {
                        $('[data-index="' + (parseFloat(vIndex) + 1).toString() + '"]').focus();
                    }
                    
                }

            }
        }
    }
});


//#endregion



//#region ★★★★★★리스트 영역 이벤트★★★★★★

//등록 버튼 클릭
$(document).on('click', ".esti_dtl", function () {
    //#region 파라미터 생성
    var tr = $(this).closest('div');
    var td = tr.children();
    var objParam = new Object();
    objParam.QUOT_NO = td.eq(1).text().trim();
    //#endregion

    controllerToLink("RegistDetail", "Estimate", objParam, false);

});

//#endregion



//#region ★★★★★★함수 영역★★★★★★

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



// 조회 함수 validation
function _fnSearchVali() {



    return true;
}



//리스트 조회 함수
function fnSearchData() {
    //#region 파라미터 만들기
    var objJsonData = new Object();
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.CRN = $("#Session_CRN").val();

    objJsonData.YMD_TYPE = "REQ_YMD";
    objJsonData.FM_YMD = $("#ETD").val().replace(/-/gi, '');
    objJsonData.TO_YMD = $("#ETA").val().replace(/-/gi, '');
    objJsonData.POL_CD = $("#input_POLCD").val();
    objJsonData.POD_CD = $("#input_PODCD").val();

    //#endregion

    $.ajax({
        type: "POST",
        url: "/Estimate/fnGetQuotationFList",
        async: true,
        dataType: "json",

        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (rtnVal) {

            if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") { // 데이터 있을 때
                var DataList = JSON.parse(rtnVal).Table1;

                _fnMakeList(DataList);
            }
        },
        beforeSend: function () {
            $("#ProgressBar_Loading").show(); //프로그래스 바
        },
        complete: function () {
            $("#ProgressBar_Loading").hide(); //프로그래스 바
        }
    });


}


//#endregion



//#region ★★★★★그리기 영역 ★★★★★

function _fnMakeList(vJson) {
    var vResult = vJson;

    _vHtml = "";

    $("#estiList").empty();

    if (vResult.length > 0) {
        $.each(vResult, function (i) {
            _vHtml += "<div class='esti-cont regist'>";
            _vHtml += " <div class='esti-cont__info'>";
            //pol data
            _vHtml += "     <div class='esti-cont__box inquiry'>";
            _vHtml += "         <div class='esti-cont__inner'>";
            _vHtml += "             <div class='esti-cont__flex'>";
            _vHtml += "                 <div class='esti-cont__desc'><p>" + _fnToNull(vResult[i]["POL_NM"]) + "</p></div>";
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

            //이미지
            _vHtml += "     <div class='esti-cont__box inquiry'>";
            _vHtml += "         <div class='esti-cont__inner'>";
            _vHtml += "             <p class='esti-cont__progress inquiry'>";
            _vHtml += "                 <img src='/Images/icn_progress.png'>";
            _vHtml += "             </p>"
            _vHtml += "         </div>";
            _vHtml += "     </div>";

            //pod data
            _vHtml += "     <div class='esti-cont__box inquiry'>";
            _vHtml += "         <div class='esti-cont__inner'>";
            _vHtml += "             <div class='esti-cont__flex'>";
            _vHtml += "                 <div class='esti-cont__desc'><p>" + _fnToNull(vResult[i]["POD_NM"]) + "</p></div>";
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

            //견적 요청일
            _vHtml += "     <div class='esti-cont__box inquiry'>";
            _vHtml += "         <div class='esti-cont__inner inner-flex'>";
            _vHtml += "             <div class='esti-cont__date flex-column'>";
            _vHtml += "                 <div class='esti-cont__date_start'>";
            _vHtml += "                     <span class='esti-cont__date_title'>견적요청일</span>";
            _vHtml += "                     <span class='esti-cont__date_cont'>" + String(_fnToNull(vResult[i]["REQ_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3')+"</span>";
            _vHtml += "                 </div>";
            //품목명
            _vHtml += "                 <div class='esti-cont__etc'>";
            _vHtml += "                     <div class='esti-cont__etc_item'>";
            _vHtml += "                         <span class='esti-cont__etc_title'>품목명</span>";
            _vHtml += "                         <span class='est-cont__etc_cont'>" + _fnToNull(vResult[i]["ITEM_NM"]) + "</span>";
            _vHtml += "                     </div>";
            _vHtml += "                 </div>";

            _vHtml += "             </div>";
            _vHtml += "         </div>";
            _vHtml += "     </div>";

            ////품목 명
            //_vHtml += "     <div class='esti-cont__box inquiry'>";
            //_vHtml += "         <div class='esti-cont__etc'>";
            //_vHtml += "             <div class='esti-cont__etc_item'>";
            //_vHtml += "                 <span class='esti-cont__etc_title'>품목명</span>";
            //_vHtml += "                 <span class='est-cont__etc_cont'>" + _fnToNull(vResult[i]["ITEM_NM"])+"</span>";
            //_vHtml += "             </div>";
            //_vHtml += "         </div>";
            //_vHtml += "     </div>";

            //문서, 담당자

            _vHtml += "     <div class='esti-cont__box inquiry'>";
            _vHtml += "         <div class='esti-cont__inner inner-flex' >";
            _vHtml += "             <div class='flex-column'>";

            _vHtml += "                 <div class='esti-cont__date_file'>";
            _vHtml += "                     <span class='esti-cont__etc_title' >문서</span>";
            _vHtml += "                     <span class='esti-cont__etc_cont'>";
                        doc_cnt = 0;
            if (_fnToZero(vResult[i].CI_CNT) != 0) { //CI 있을 때 
                _vHtml += "                 <span class='doc_type1'>C/I</span>";
                doc_cnt += 1
            }

            if (_fnToZero(vResult[i].PL_CNT) != 0) { //PI 있을 때 
                if (doc_cnt != 0) {
                    _vHtml += ",<span class='doc_type2'>P/L</span>";
                }
                else {
                    _vHtml += "                 <span class='doc_type1'>P/L</span>";
                }
                doc_cnt += 1
            }
            _vHtml += "                     </span>";
            _vHtml += "                 </div>";

            _vHtml += "                 <div class='esti-cont__etc'>";
            _vHtml += "                     <div class='esti-cont__etc_item'>";
            _vHtml += "                         <span class='esti-cont__etc_title'>담당자</span>";
            _vHtml += "                         <span class='esti-cont__etc_cont'>" + _fnToNull(vResult[i]["PIC_NM"])+"</span>";
            _vHtml += "                     </div>";
            _vHtml += "                 </div>";

            _vHtml += "             </div>";
            _vHtml += "         </div>";
            _vHtml += "     </div>";


            ////문서
            //_vHtml += "     <div class='esti-cont__box inquiry'>";
            //_vHtml += "         <div class='esti-cont__inner'>";
            //_vHtml += "             <div class='esti-cont__date_file'>";
            //_vHtml += "                 <span class='esti-cont__etc_title'>문서</span>";
            //_vHtml += "                 <span class='esti-cont__etc_cont'>";

            //doc_cnt = 0;
            //if (_fnToZero(vResult[i].CI_CNT) != 0) { //CI 있을 때 
            //    _vHtml += "                 <span class='doc_type1'>C/I</span>";
            //    doc_cnt += 1
            //}

            //if (_fnToZero(vResult[i].PL_CNT) != 0) { //PI 있을 때 
            //    if (doc_cnt != 0) {
            //        _vHtml += ",<span class='doc_type2'>P/L</span>";
            //    }
            //    else {
            //        _vHtml += "                 <span class='doc_type1'>P/L</span>";
            //    }
            //    doc_cnt += 1
            //}

            //_vHtml += "                 </span>";
            //_vHtml += "             </div>";
            //_vHtml += "         </div>";
            //_vHtml += "     </div>";

            ////담당자
            //_vHtml += "     <div class='esti-cont__box inquiry'>";
            //_vHtml += "         <div class='esti-cont__inner'>";
            //_vHtml += "             <div class='esti-cont__etc'>";
            //_vHtml += "                 <div class='esti-cont__etc_item'>";
            //_vHtml += "                     <span class='esti-cont__etc_title'>담당자</span>";
            //_vHtml += "                     <span class='esti-cont__etc_cont'>" + _fnToNull(vResult[i]["PIC_NM"])+"</span>";
            //_vHtml += "                 </div>";
            //_vHtml += "             </div>";
            //_vHtml += "         </div>";
            //_vHtml += "     </div>";

            //버튼
            _vHtml += "     <div class='esti-cont__box inquiry'>";
            _vHtml += "         <div class='esti-cont__inner'>";
            _vHtml += "             <button type='button' class='btns esti_dtl'>등록</button>";
            _vHtml += "              <p style='display:none'>" + _fnToNull(vResult[i]["QUOT_NO"]) + "</p>";
            _vHtml += "         </div>";
            _vHtml += "     </div>";

            _vHtml += " </div>";
            _vHtml += "</div>";


        });


        $("#estiList").append(_vHtml);
    }
    else {

    }
    
}

//#endregion