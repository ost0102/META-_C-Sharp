
////////////////////전역 변수//////////////////////////
var _vSelectDate = new Date();
var obj = new Object();
var mymap;
var objDs = new Object();
var vOCRResultALL;
var vOCRResultALL2;
var vOCRResult;
////////////////////jquery event///////////////////////
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    $(".sub_esti").addClass("on");
    $(".sub_esti .sub_depth").addClass("on");
    $(".sub_esti .sub_depth li:nth-child(1) a").addClass("on");

    

    fnGetTemplate();

    if (_fnToNull($("#LK_POL_CD").val()) != "") {
        $("#input_POL").val(_fnToNull($("#LK_POL_NM").val()));
        $("#input_POD").val(_fnToNull($("#LK_POD_NM").val()));
        $("#input_POLCD").val(_fnToNull($("#LK_POL_CD").val()));
        $("#input_PODCD").val(_fnToNull($("#LK_POD_CD").val()));
        $("#MAIN_ITEM").val(_fnToNull($("#LK_ITEM_NM").val()));
        if ($("#LK_REQ_SVC").val() == "AIR") {
            $("#AIR").addClass("on");
            $("#SEA").removeClass("on");
        }
    }

    $("#ProgressBar_Loading").show(); //프로그래스 바
    setTimeout(function () {
        $("#ProgressBar_Loading").hide(); //프로그래스 바
    }, 1000);
});
//=====================MAIN============================
$(document).on("keyup", "#ETD", function (e) {
    if (e.keyCode == 13) {
        _fnFormatDate($(this).val());
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

function fnGetPortData(vValue) {
    try {
        var rtnJson;
        var objJsonData = new Object();

        //선택에 따라서 Sea S / air A 체크
        if ($("#SEA").hasClass("on")) {
            objJsonData.REQ_SVC = "SEA";
        } else {
            objJsonData.REQ_SVC = "AIR";
        }
        
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



$(document).ready(function () {
    var fileTarget = $('.int_box #doc_file1');
    var fileTarget2 = $('.int_box #doc_file2');

    fileTarget.on('change', function () {  // 값이 변경되면
        if (window.FileReader) {  // modern browser
            var filename = $(this)[0].files[0].name;
        }
        else {  // old IE
            var filename = $(this).val().split('/').pop().split('\\').pop();  // 파일명만 추출
        }

        // 추출한 파일명 삽입
        //$(this).siblings('#doc_file1').val(filename);
        //$('#doc_file1').html(filename);
        $(this).next('label').text(filename);
    });

    fileTarget2.on('change', function () {  // 값이 변경되면
        if (window.FileReader) {  // modern browser
            var filename = $(this)[0].files[0].name;
        }
        else {  // old IE
            var filename = $(this).val().split('/').pop().split('\\').pop();  // 파일명만 추출
        }

        // 추출한 파일명 삽입
        $(this).next('label').text(filename);
    });
})


$(document).on("focusout", ".cargo", function () {
    _fnGetNumber(this, "");
});

$(document).on("keyup", ".cargo", function () {
    _fnGetNumber(this, "");
});


//날짜 포스커 아웃 할때 벨리데이션
$(document).on("focusout", "#ETD", function () {
    var vValue = _fnToNull($("#ETD").val());
    var vValue_Num = vValue.replace(/[^0-9]/g, "");
    if (vValue != "") {
        vValue = vValue_Num.substring("0", "4") + "-" + vValue_Num.substring("4", "6") + "-" + vValue_Num.substring("6", "8");
        $(this).val(vValue);
    }

    //값 벨리데이션 체크
    if (!_fnisDate($(this).val())) {
        $(this).val($("#ETA").val());
        $(this).focus();
    }

    //날짜 벨리데이션 체크$
    //var vETD = _fnToNull($("#ETD").val().replace(/[^0-9]/g, ""));
    //var vETA = _fnToNull($("#ETA").val().replace(/[^0-9]/g, ""));
    //if (vETA != "") {
    //    if (vETA < vETD) {
    //        _fnAlertMsg("ETD가 ETA 보다 빠를 수 없습니다. ");
    //        $("#ETD").val(vETA.substring("0", "4") + "-" + vETA.substring("4", "6") + "-" + vETA.substring("6", "8"));
    //    }
    //}
    $(".xdsoft_datetimepicker").hide();
});

//날짜 포스커 아웃 할때 벨리데이션
$(document).on("focusout", "#ETA", function () {
    var vValue = _fnToNull($("#ETA").val());
    var vValue_Num = vValue.replace(/[^0-9]/g, "");
    if (vValue != "") {
        vValue = vValue_Num.substring("0", "4") + "-" + vValue_Num.substring("4", "6") + "-" + vValue_Num.substring("6", "8");
        $(this).val(vValue);
    }

    //값 벨리데이션 체크
    if (!_fnisDate($(this).val())) {
        $(this).val($("#ETD").val());
        $(this).focus();
    }

    //날짜 벨리데이션 체크
    //var vETD = $("#ETD").val().replace(/[^0-9]/g, "");
    //var vETA = $("#ETA").val().replace(/[^0-9]/g, "");
    //if (vETD != "") {
    //    if (vETA < vETD) {
    //        _fnAlertMsg("ETA가 ETD 보다 빠를 수 없습니다. ");
    //        $("#ETA").val(vETD.substring("0", "4") + "-" + vETD.substring("4", "6") + "-" + vETD.substring("6", "8"));
    //    }
    //}
    $(".xdsoft_datetimepicker").hide();
});

function ChkValidation() {
    if (_fnToNull($("#input_POL").val()) == "") {
        _fnAlertMsg("출발지를 입력하세요", "input_POL");
        return false;
    }
    if (_fnToNull($("#input_POLCD").val()) == "") {
        _fnAlertMsg("출발지를 입력하세요", "input_POL");
        return false;
    }

    if (_fnToNull($("#input_POD").val()) == "") {
        _fnAlertMsg("도착지를 입력하세요", "input_POD");
        return false;
    }

    if (_fnToNull($("#input_PODCD").val()) == "") {
        _fnAlertMsg("도착지를 입력하세요", "input_POD");
        return false;
    }

    return true;
}


function fnGetTemplate() {
    var vHTML = "";

    $("#TEMP_LIST1").empty();
    $("#TEMP_LIST2").empty();

    var objJsonData = new Object();
    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.APV_YN = "Y";
    objJsonData.TMPLT_TYPE = "CI";
    
    var rtnVal = _fnGetAjaxData("POST", "Format", "fnGetTemplateList", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;
        vHTML = "";
        vHTML += "<option value='' selected>등록서식 선택하기</option>"
        $.each(vResult, function (i) {
            vHTML += "<option value=" + vResult[i].NC_OCR_ID + ">" + vResult[i].TMPLT_NM + "</option>"
        });

        $("#TEMP_LIST1").append(vHTML);
        $("#TEMP_LIST1").attr("disabled", false);
        $("#TMPLT_ID1").val(vResult[0].TMPLT_ID);

    } else {
        vHTML += "<option value='' selected>등록된 서식이 없습니다.</option>";
        $("#TEMP_LIST1").append(vHTML);
        $("#TEMP_LIST1").attr("disabled", true);
    }

    objJsonData.TMPLT_TYPE = "PL";

    var rtnVal = _fnGetAjaxData("POST", "Format", "fnGetTemplateList", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;
        vHTML = "";
        vHTML += "<option value='' selected>등록서식 선택하기</option>"
        $.each(vResult, function (i) {
            vHTML += "<option value=" + vResult[i].NC_OCR_ID + ">" + vResult[i].TMPLT_NM + "</option>"
        });

        $("#TEMP_LIST2").append(vHTML);
        $("#TEMP_LIST2").attr("disabled", false);
        $("#TMPLT_ID2").val(vResult[0].TMPLT_ID);

    } else {
        vHTML += "<option value='' selected>등록된 서식이 없습니다.</option>";
        $("#TEMP_LIST2").append(vHTML);
        $("#TEMP_LIST2").attr("disabled", true);
    }
}
var chkFile = false
//===================COMMON===============================
//다음버튼 클릭
$(".btns.next").on("click", function () {
    if (ChkValidation()) {
        var $seq = $(this).closest(".req-info__seq");
        if ($("#doc_file1")[0].files.length == 0 && $("#doc_file2")[0].files.length == 0) {
            if ($(this).closest(".req-info__seq").index() == "1") {
                chkFile = true;
                var statNum = $(this).closest(".req-info__seq").index() + 1;
                var statNum2 = $(this).closest(".req-info__seq").index() + 2;

                if (!$(this).hasClass("request")) {
                    $seq.hide();
                    $seq.next().next(".req-info__seq").show();
                    $(".req-status__info").removeClass("on");
                    $(".req-status__info:nth-child(" + statNum + ")").addClass("passed");
                    $(".req-status__info:nth-child(" + statNum2 + ")").addClass("on");
                }

            } else {
                var statNum = $(this).closest(".req-info__seq").index();
                var statNum2 = $(this).closest(".req-info__seq").index() + 1;

                if (!$(this).hasClass("request")) {
                    $seq.hide();
                    $seq.next(".req-info__seq").show();
                    $(".req-status__info").removeClass("on");
                    $(".req-status__info:nth-child(" + statNum + ")").addClass("passed");
                    $(".req-status__info:nth-child(" + statNum2 + ")").addClass("on");
                }
            }
        } else {

            var statNum = $(this).closest(".req-info__seq").index();
            var statNum2 = $(this).closest(".req-info__seq").index() + 1;

            if (!$(this).hasClass("request")) {
                $seq.hide();
                $seq.next(".req-info__seq").show();
                $(".req-status__info").removeClass("on");
                $(".req-status__info:nth-child(" + statNum + ")").addClass("passed");
                $(".req-status__info:nth-child(" + statNum2 + ")").addClass("on");
            }
        }
   }
});

$("#btn_part1").click(function (e) {
    if (ChkValidation()) {
        if (!chkFile) {
            //$("#ProgressBar_Loading").show(); //프로그래스 바
            fnOcrSend();
            //$("#ProgressBar_Loading").hide(); //프로그래스 바
        } else {
            fnConfirmData();
            chkFile = false;
        }
    }
});

$("#btn_part2").click(function (e) {
    fnConfirmData();
});

$("#btn_part3").click(function (e) {
    fnSearchForwarder();
    fnSearchReccomendForwarderList();
});

//이전버튼 클릭
$(".btns.prev").on("click", function () {

    if ($("#doc_file1")[0].files.length == 0 && $("#doc_file2")[0].files.length == 0) {
        var $seq = $(this).closest(".req-info__seq");
        if ($(this).closest(".req-info__seq").index() == "3") {
            var statNum = $(this).closest(".req-info__seq").index() - 1;
            var statNum2 = $(this).closest(".req-info__seq").index() - 2;
            $seq.hide();
            $seq.prev().prev(".req-info__seq").show();
            $(".req-status__info").removeClass("on");
            $(".req-status__info:nth-child(" + statNum + ")").removeClass("passed");
            $(".req-status__info:nth-child(" + statNum2 + ")").removeClass("passed");
            $(".req-status__info:nth-child(" + statNum2 + ")").addClass("on");
        } else {
            var $seq = $(this).closest(".req-info__seq");
            var statNum = $(this).closest(".req-info__seq").index();
            var statNum2 = $(this).closest(".req-info__seq").index() - 1;
            $seq.hide();
            $seq.prev(".req-info__seq").show();
            $(".req-status__info").removeClass("on");
            $(".req-status__info:nth-child(" + statNum + ")").removeClass("passed");
            $(".req-status__info:nth-child(" + statNum2 + ")").removeClass("passed");
            $(".req-status__info:nth-child(" + statNum2 + ")").addClass("on");
        }
    } else {
        var $seq = $(this).closest(".req-info__seq");
        var statNum = $(this).closest(".req-info__seq").index();
        var statNum2 = $(this).closest(".req-info__seq").index() - 1;
        $seq.hide();
        $seq.prev(".req-info__seq").show();
        $(".req-status__info").removeClass("on");
        $(".req-status__info:nth-child(" + statNum + ")").removeClass("passed");
        $(".req-status__info:nth-child(" + statNum2 + ")").removeClass("passed");
        $(".req-status__info:nth-child(" + statNum2 + ")").addClass("on");
    }
})

$(document).on("click", ".btns.cargo_del", function () {
    $(this).closest(".req-info__cargo2").remove();
    cargo_seq -= 1;
})

$(".doc_select .req-info__desc").on("click", function () {
    //if (!$(".doc_select .req-info__desc").hasClass('wd100')) {
        $(".doc_select .req-info__desc").removeClass("on");
        $(this).addClass("on");
   // }
})

$(document).on("click", ".req-forward", function () {
    if (!$(this).hasClass("on")) {
        $(this).addClass("on");
    }
    else {
        $(this).removeClass("on");
    }
})

var cargo_seq = 1;
$("#addCargo").on("click", function () {
    if (_fnToNull($("#CARGO_PKG").val()) == "" && _fnToNull($("#CARGO_GRS_WGT").val()) == "" && _fnToNull($("#CARGO_VOL_WGT").val()) == "" && _fnToNull($("#CARGO_WIDTH").val()) == "" && _fnToNull($("#CARGO_HEIGHT").val()) == "" && _fnToNull($("#CARGO_LENGTH").val()) == "") {
        return false;
    }
    if (_fnToZero($("#CARGO_PKG").val()) == 0) {
        _fnAlertMsg("수량을 입력해주세요.", "CARGO_PKG");
        $("#CARGO_PKG").val("")
        return false;
    }
    if (_fnToZero($("#CARGO_GRS_WGT").val()) == 0) {
        _fnAlertMsg("G/WT를 입력해주세요.", "CARGO_GRS_WGT");
        $("#CARGO_GRS_WGT").val("")
        return false;
    }
    if (_fnToZero($("#CARGO_VOL_WGT").val()) == 0) {
        _fnAlertMsg("V/WT를 입력해주세요." , "CARGO_VOL_WGT");
        $("#CARGO_VOL_WGT").val("")
        return false;
    }

    if (_fnToZero($("#CARGO_WIDTH").val()) > 300) {
        _fnAlertMsg("최대 300cm 까지 입력가능합니다.", "CARGO_WIDTH");
        $("#CARGO_WIDTH").val("")
        return false;
    }
    if (_fnToZero($("#CARGO_LENGTH").val()) > 230) {
        _fnAlertMsg("최대 230cm 까지 입력가능합니다.", "CARGO_LENGTH");
        $("#CARGO_LENGTH").val("")
        return false;
    }
    if (_fnToZero($("#CARGO_HEIGHT").val()) > 230) {
        _fnAlertMsg("최대 230cm 까지 입력가능합니다.", "CARGO_HEIGHT");
        $("#CARGO_HEIGHT").val("")
        return false;
    }

    var vHTML = "";
    vHTML += "<div class=\"req-info__cargo2\">";
    vHTML += "    <div class=\"req-info__num\"><p><span>" + cargo_seq + "</span></p><span style='display:none'>|</span></div>";
    vHTML += "    <div class=\"req-info__cont\">";
    vHTML += "        <p>";
    vHTML += "           수량 <span class='TOT_CARGO_PKG'>" + _fnToZero($("#CARGO_PKG").val()) + " | </span>   G/WT <span class='TOT_CARGO_GRS_WGT'>" + _fnToZero($("#CARGO_GRS_WGT").val()) + " | </span>  V/WT <span class='TOT_CARGO_VOL_WGT'>" + _fnToZero($("#CARGO_VOL_WGT").val()) + " | </span>  가로 <span>" + _fnToZero($("#CARGO_WIDTH").val()) + " CM | </span>  세로 <span>" + _fnToZero($("#CARGO_LENGTH").val()) + " CM | </span> 높이 <span>" + _fnToZero($("#CARGO_HEIGHT").val()) + " CM </span>";
    vHTML += "        </p>";
    vHTML += "    </div>";
    vHTML += "    <button type=\"button\" class=\"btns cargo_del\"></button>";
    vHTML += "</div>";
    cargo_seq += 1;
    $("#cargoList").append(vHTML);
    $(".cargo").val("");
    $(".cargodel").hide();
})


//엔터키 입력시 마다 다음 input으로 가기
$(document).keyup(function (e) {
    if (e.keyCode == 13) {
        //alert($(e.target).attr('data-index'));
        if ($(e.target).attr('data-index') != undefined) {
            var vIndex = $(e.target).attr('data-index');
            $('[data-index="' + (parseFloat(vIndex) + 1).toString() + '"]').focus();
        }
    }
});


//==========================PART2==================================
var ocrReturnDt1;
var ocrReturnDt2;
var checkNoFile = false;
function fnOcrSend(rtnStr) {

    formData = new FormData(); //Form 초기화
    $(".OcrList").empty();
    var files;
    var tmp_type;
    if ($("#doc_file1")[0].files.length > 0) {
        files = $("#doc_file1")[0].files;
        tmp_type = $("#TEMP_LIST1 option:selected").val();
        $("#DOC_TEMP_NM1").text("C/I");
        $("#DOC_FILE_NM1").text(files[0].name);

        formData.append("files", files[0]);
        formData.append("TMP_ID", tmp_type);

        $.ajax({
            type: "POST",
            url: "/Main/OcrFiles",
            dataType: "json",
            async: true,
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: formData,
            success: function (result) {
                if (_fnToNull(result) != "") {
                    vOCRResultALL = JSON.parse(result.replace(/\\n/gi, '<br/>'));
                    if (_fnToNull(vOCRResultALL) != "") {
                        makeOcrList(vOCRResultALL);
                    } else {
                        $("#FILE_NO_DATA").show();
                    }

                    $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + files[0].name);
                } else {
                    if ($("#doc_file1")[0].files.length > 0) {
                        $("#DOC_TEMP_TYPE1").text("C/I")
                        $(".DOC_TEMP_NM1").text($("#TEMP_LIST1 option:selected").text());
                        $(".DOC_FILE_NM1").text($("#doc_file1")[0].files[0].name);

                        vHtml = "<div class='req-field__cont no_data' style='padding:0' id='PART2_NO_DATA'>";
                        vHtml += "<p><img src='/Images/no_data.png'/></p>";
                        vHtml += "</div>";

                        $(".OcrList").append(vHtml);
                    }
                    $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + files[0].name);

                }
            },
            error: function (xhr, status, error) {
                _fnAlertMsg("[Error]관리자에게 문의 해 주세요. " + status);
                vReturn = false;
            }, beforeSend: function () {
                $("#ProgressBar_Loading").show(); //프로그래스 바 
            },
            complete: function () {
                $("#ProgressBar_Loading").hide(); //프로그래스 바 
            }
        });
    } else {
        checkNoFile = true;
    }
   

    if ($("#doc_file2")[0].files.length > 0) {
        formData = new FormData();
        const files2 = $("#doc_file2")[0].files;
        tmp_type = $("#TEMP_LIST2 option:selected").val();

        var vHtml = "";
        formData.append("files", files2[0]);
        formData.append("TMP_ID", tmp_type);

        $.ajax({
            type: "POST",
            url: "/Main/OcrFiles",
            dataType: "json",
            async: true,
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: formData,
            success: function (result) {
                if (_fnToNull(result) != "") { 
                    vOCRResultALL2 = JSON.parse(result.replace(/\\n/gi, '<br/>'));
                var vOCR2 = vOCRResultALL2.images[0];
                    if (vOCR2.inferResult != "FAILURE") {
                        vOCRResult = vOCR2.fields;
                        $.each(vOCRResult, function (i) {
                            vHtml += "	<div class='req-field__cont ocrRow2'> ";
                            vHtml += "        <p><span class='s_mo'>판독영역</span>" + "필드" + (i + 1) + "</p>";
                            vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + i + "</span></p>";
                            vHtml += "        <p><span class='s_mo'>이름</span><span class='s_dt'>" + vOCRResult[i].name + "</span></p>";
                            vHtml += "        <p><span class='s_mo'>내용</span><span class='s_dt'>" + vOCRResult[i].inferText + "</span></p>";
                            vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.top + "</span></p>";
                            vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.left + "</span></p>";
                            vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.width + "</span></p>";
                            vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.height + "</span></p>";
                            vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].valueType + "</span></p>";
                            vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].inferConfidence + "</span></p>";
                            vHtml += "    </div>";
                        });
                        $(".OcrList2").append(vHtml);
                        $("#DOC_TEMP_TYPE2").text("P/L");
                        $(".DOC_TEMP_NM2").text($("#TEMP_LIST2 option:selected").text());
                        $(".DOC_FILE_NM2").text($("#doc_file2")[0].files[0].name);

                        $("#DOC_TEMP2").show();
                        
                    }
                } else {
                    if ($("#doc_file2")[0].files.length > 0) {
                        $("#DOC_TEMP_TYPE2").text("P/L")
                        $(".DOC_TEMP_NM2").text($("#TEMP_LIST2 option:selected").text());
                        $(".DOC_FILE_NM2").text($("#doc_file2")[0].files[0].name);

                        $("#DOC_TEMP2").show();
                    }
                }
                if (checkNoFile) {
                    $("#DOC_TEMP1").hide();
                    $("#DOC_TEMP2").click();
                }
            },
            error: function (xhr, status, error) {
                _fnAlertMsg("[Error]관리자에게 문의 해 주세요. " + status);
                vReturn = false;
            }, beforeSend: function () {
                $("#ProgressBar_Loading").show(); //프로그래스 바 
            },
            complete: function () {
                $("#ProgressBar_Loading").hide(); //프로그래스 바 
            }
        });
    }

}

function makeOcrList(rtnStr) {

    if (_fnToNull(rtnStr) != "") {
        var vHtml = ""
        $(".OcrList").empty();
        var OCRdt = rtnStr.images[0];
        const files = $("#doc_file1")[0].files
        if (OCRdt.inferResult == "FAILURE") {

            if ($("#doc_file1")[0].files.length > 0) {
                $("#DOC_TEMP_TYPE1").text("C/I")
                $(".DOC_TEMP_NM1").text($("#TEMP_LIST1 option:selected").text());
                $(".DOC_FILE_NM1").text($("#doc_file1")[0].files[0].name);

                vHtml = "<div class='req-field__cont no_data' style='padding:0' id='PART2_NO_DATA'>";
                vHtml += "<p><img src='/Images/no_data.png'/></p>";
                vHtml += "</div>";

                $(".OcrList").append(vHtml);
                $("#TEMP_TLT").texts
                $("#TEMP_CONT").text("");
                $("#OCR_FILED1").show();
                $("#OCR_FILED2").hide();
            }
            $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + files[0].name);

            $("#TEMP_TLT").text("");
            $("#TEMP_CONT").text("");
            if ($("#doc_file2")[0].files.length > 0) {
                $("#DOC_TEMP_TYPE2").text("P/L")
                $(".DOC_TEMP_NM2").text($("#TEMP_LIST2 option:selected").text());
                $(".DOC_FILE_NM2").text($("#doc_file2")[0].files[0].name);

                $("#DOC_TEMP2").show();
            }
        } else {
            if (OCRdt != "") {
                var vTitle = OCRdt.title;

                if ($("#doc_file1")[0].files.length > 0) {
                    $("#DOC_TEMP_TYPE1").text("C/I")
                    $(".DOC_TEMP_NM1").text($("#TEMP_LIST1 option:selected").text());
                    $(".DOC_FILE_NM1").text($("#doc_file1")[0].files[0].name);
                }

                if ($("#doc_file2")[0].files.length > 0) {
                    $("#DOC_TEMP_TYPE2").text("P/L");
                    $(".DOC_TEMP_NM2").text($("#TEMP_LIST2 option:selected").text());
                    $(".DOC_FILE_NM2").text($("#doc_file2")[0].files[0].name);

                    $("#DOC_TEMP2").show();
                }

                $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + files[0].name);


                $("#TEMP_TLT").text(vTitle.name);
                $("#TEMP_CONT").text(vTitle.inferText);

                vOCRResult = OCRdt.fields;
                $.each(vOCRResult, function (i) {
                    vHtml += "	<div class='req-field__cont ocrRow'> ";
                    vHtml += "        <p><span class='s_mo'>판독영역</span>" + "필드" + (i + 1) + "</p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + i + "</span></p>";
                    vHtml += "        <p><span class='s_mo'>이름</span><span class='s_dt'>" + vOCRResult[i].name + "</span></p>";
                    vHtml += "        <p><span class='s_mo'>내용</span><span class='s_dt'>" + vOCRResult[i].inferText + "</span></p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.top + "</span></p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.left + "</span></p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.width + "</span></p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.height + "</span></p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].valueType + "</span></p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].inferConfidence + "</span></p>";
                    vHtml += "    </div>";
                });

                $(".OcrList").append(vHtml);
            }

        }
        $("#OCR_FILED1").show();
        $("#OCR_FILED2").hide();
    } else {

        const files = $("#doc_file1")[0].files;
        $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + files[0].name);
        $(".OcrList").empty();

        vHtml = "<div class='req-field__cont no_data' style='padding:0' id='PART2_NO_DATA'>";
        vHtml += "<p><img src='/Images/no_data.png'/></p>";
        vHtml += "</div>";

        $("#TEMP_TLT").text("");
        $("#TEMP_CONT").text("");
        $(".OcrList").append(vHtml);
        $("#OCR_FILED1").show();
        $("#OCR_FILED2").hide();
        
    }
}


$("#PART3_FILE1").click(function (e) {
    if (_fnToNull(vOCRResultALL) != "") {
        $("#PART3_OCR_LIST").empty();
        var vHtml = "";
        var vOCR2 = vOCRResultALL.images[0];
        if (vOCR2.inferResult != "FAILURE") {

            var vTitle = vOCR2.title;
            $("#PART3_TEMP_TLT").text(vTitle.name);
            $("#PART3_TEMP_CONT").text(vTitle.inferText);

            vOCRResult = vOCR2.fields;
            $.each(vOCRResult, function (i) {
                vHtml += "	<div class='req-field__cont'> ";
                vHtml += "        <p><span class='s_mo'>판독영역</span>" + "필드" + (i + 1) + "</p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + i + "</span></p>";
                vHtml += "        <p><span class='s_mo'>이름</span><span class='s_dt'>" + vOCRResult[i].name + "</span></p>";
                vHtml += "        <p><span class='s_mo'>내용</span><span class='s_dt'>" + vOCRResult[i].inferText + "</span></p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.top + "</span></p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.left + "</span></p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.width + "</span></p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.height + "</span></p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].valueType + "</span></p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].inferConfidence + "</span></p>";
                vHtml += "    </div>";
            });
            $("#PART3_OCR_LIST").append(vHtml);
            $("#PART3_LIST2").hide();
            $("#PART3_LIST1").show();
        } else {

            vHtml = "<div class='req-field__cont no_data' style='padding:0' id='PART3_NO_DATA'>";
            vHtml += "<p><img src='/Images/no_data.png'/></p>";
            vHtml += "</div>";

            $("#PART3_TEMP_TLT").text("");
            $("#PART3_TEMP_CONT").text("");
            $("#PART3_OCR_LIST").append(vHtml);
            $("#PART3_LIST2").hide();
            $("#PART3_LIST1").show();
        }
    } else {
        $("#PART3_OCR_LIST").empty();
        vHtml = "<div class='req-field__cont no_data' style='padding:0' id='PART3_NO_DATA'>";
        vHtml += "<p><img src='/Images/no_data.png'/></p>";
        vHtml += "</div>";

        $("#PART3_TEMP_TLT").text("");
        $("#PART3_TEMP_CONT").text("");
        $("#PART3_OCR_LIST").append(vHtml);
        $("#PART3_LIST2").hide();
        $("#PART3_LIST1").show();

    }
});

$("#PART3_FILE2").click(function (e) {
    if (_fnToNull(vOCRResultALL2) != "") {
        $("#PART3_OCR_LIST2").empty();
        var vHtml = "";
        var vOCR2 = vOCRResultALL2.images[0];
        if (vOCR2.inferResult != "FAILURE") {

            var vTitle = vOCR2.title;
            $("#PART3_TEMP_TLT").text(vTitle.name);
            $("#PART3_TEMP_CONT").text(vTitle.inferText);

            vOCRResult = vOCR2.fields;
            $.each(vOCRResult, function (i) {
                vHtml += "	<div class='req-field__cont'> ";
                vHtml += "        <p><span class='s_mo'>판독영역</span>" + "필드" + (i + 1) + "</p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + i + "</span></p>";
                vHtml += "        <p><span class='s_mo'>이름</span><span class='s_dt'>" + vOCRResult[i].name + "</span></p>";
                vHtml += "        <p><span class='s_mo'>내용</span><span class='s_dt'>" + vOCRResult[i].inferText + "</span></p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.top + "</span></p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.left + "</span></p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.width + "</span></p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.height + "</span></p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].valueType + "</span></p>";
                vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].inferConfidence + "</span></p>";
                vHtml += "    </div>";
            });
            $("#PART3_OCR_LIST2").append(vHtml);
            $("#PART3_LIST2").show();
            $("#PART3_LIST1").hide();
        } else {

            vHtml = "<div class='req-field__cont no_data' style='padding:0' id='PART3_NO_DATA'>";
            vHtml += "<p><img src='/Images/no_data.png'/></p>";
            vHtml += "</div>";

            $("#PART3_TEMP_TLT").text("");
            $("#PART3_TEMP_CONT").text("");
            $("#PART3_OCR_LIST2").append(vHtml);
            $("#PART3_LIST2").show();
            $("#PART3_LIST1").hide();
        }
    } else {
        $("#PART3_OCR_LIST2").empty();
        vHtml = "<div class='req-field__cont no_data' style='padding:0' id='PART3_NO_DATA'>";
        vHtml += "<p><img src='/Images/no_data.png'/></p>";
        vHtml += "</div>";

        $("#PART3_TEMP_TLT").text("");
        $("#PART3_TEMP_CONT").text("");
        $("#PART3_OCR_LIST2").append(vHtml);
        $("#PART3_LIST2").show();
        $("#PART3_LIST1").hide();
    }
});


function makeOcrList2(rtnStr, bool) {
    if (_fnToNull(rtnStr) != "") {
        $(".OcrList2").empty();
        var vHtml = ""
        var OCRdt2 = rtnStr.images[0];
        const files = $("#doc_file2")[0].files
        if (_fnToNull(OCRdt2.inferResult) == "FAILURE" || _fnToNull(OCRdt2.inferResult) == "") {

            if ($("#doc_file1")[0].files.length > 0) {
                $("#DOC_TEMP_TYPE1").text("C/I")
                $(".DOC_TEMP_NM1").text($("#TEMP_LIST1 option:selected").text());
                $(".DOC_FILE_NM1").text($("#doc_file1")[0].files[0].name);
            }

            if ($("#doc_file2")[0].files.length > 0) {
                $("#DOC_TEMP_TYPE2").text("P/L");
                $(".DOC_TEMP_NM2").text($("#TEMP_LIST2 option:selected").text());
                $(".DOC_FILE_NM2").text($("#doc_file2")[0].files[0].name);

                $("#DOC_TEMP2").show();
            }

            $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + files[0].name);

            vHtml = "<div class='req-info__desc wd100'>";
            vHtml += "<div class='req-doc__desc'>";
            vHtml += "<p><img src='/Images/no_data.png'/></p>";
            vHtml += "</div>";
            vHtml += "</div>";

            $("#TEMP_TLT").text("");
            $("#TEMP_CONT").text("");
            $(".OcrList2").append(vHtml);
            $("#OCR_FILED1").hide();
            $("#OCR_FILED2").show();


        } else {
            if (OCRdt2 != "") {
                var vTitle = OCRdt2.title;

                if ($("#doc_file2")[0].files.length > 0) {
                    $("#DOC_TEMP_TYPE2").text("P/L")
                    $(".DOC_TEMP_NM2").text($("#TEMP_LIST2 option:selected").text());
                    $(".DOC_FILE_NM2").text($("#doc_file2")[0].files[0].name);

                    $("#DOC_TEMP2").show();
                }

                $("#TEMP_TLT").text(vTitle.name);
                $("#TEMP_CONT").text(vTitle.inferText);

                vOCRResult = OCRdt2.fields;
                $.each(vOCRResult, function (i) {
                    vHtml += "	<div class='req-field__cont ocrRow2'> ";
                    vHtml += "        <p><span class='s_mo'>판독영역</span>" + "필드" + (i + 1) + "</p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + i + "</span></p>";
                    vHtml += "        <p><span class='s_mo'>이름</span><span class='s_dt'>" + vOCRResult[i].name + "</span></p>";
                    vHtml += "        <p><span class='s_mo'>내용</span><span class='s_dt'>" + vOCRResult[i].inferText + "</span></p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.top + "</span></p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.left + "</span></p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.width + "</span></p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.height + "</span></p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].valueType + "</span></p>";
                    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].inferConfidence + "</span></p>";
                    vHtml += "    </div>";
                });
                $(".OcrList2").append(vHtml);
                $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + files[0].name);
                $("#OCR_FILED1").hide();
                $("#OCR_FILED2").show();
            }

        }
    } else {
        const files = $("#doc_file2")[0].files;
        $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + files[0].name);
        $(".OcrList2").empty();

        vHtml = "<div class='req-field__cont no_data' style='padding:0'>";
        vHtml += "<p><img src='/Images/no_data.png'/></p>";
        vHtml += "</div>";

        $("#TEMP_TLT").text("");
        $("#TEMP_CONT").text("");
        $(".OcrList2").append(vHtml);
        $("#OCR_FILED1").hide();
        $("#OCR_FILED2").show();
    }
}

$("#DOC_TEMP1").click(function (e) {
    makeOcrList(vOCRResultALL);
});


$("#DOC_TEMP2").click(function (e) {
    makeOcrList2(vOCRResultALL2);
});

//==========================PART3==================================
var checkNofile2 = false;
function fnConfirmData() {
    var vHtml = "";
    var tot_pkg = 0;
    var tot_grs_wgt = 0;
    var tot_vol_wgt = 0;
    $('.TOT_CARGO_PKG').each(function () {
        var test = $(this).text();
        tot_pkg += parseInt($(this).text().replace(/,/gi, ''));
    });
    $('.TOT_CARGO_GRS_WGT').each(function () {
        tot_grs_wgt += parseInt($(this).text().replace(/,/gi,''));
    });
    $('.TOT_CARGO_VOL_WGT').each(function () {
        tot_vol_wgt += parseInt($(this).text().replace(/,/gi, ''));
    });
    if ($("#SEA").hasClass("on")) {
        $("#CFM_REQ_SVC").text("해운");
    } else {
        $("#CFM_REQ_SVC").text("항공");
    }
    $("#CFM_POL").text($("#input_POL").val());
    $("#CFM_ETD").text($("#ETD").val());
    $("#CFM_POD").text($("#input_POD").val());
    $("#CFM_ETA").text($("#ETA").val());
    $("#CFM_PKG").text(_fnToNull(_fnGetNumber(tot_pkg, "sum")));
    $("#CFM_MAIN_ITEM").text($("#MAIN_ITEM").val());
    $("#CFM_GRS_WGT").text(_fnToNull(_fnGetNumber(tot_grs_wgt, "sum")));
    $("#CFM_VOL_WGT").text(_fnToNull(_fnGetNumber(tot_vol_wgt, "sum")));
    $("#CFM_RMK").text($("#RMK").val());


    if ($("#doc_file1")[0].files.length == 0 && $("#doc_file2")[0].files.length == 0) {
        $("#PART3_FILE1").hide();
        $("#PART3_FILE2").hide();
        $("#FILE_NO_DATA").show();
    } else {
        if ($("#doc_file1")[0].files.length > 0) {
            $("#PART3_TYPE_NM1").text("C/I");
            $("#PART3_FILE_NM1").text($("#doc_file1")[0].files[0].name);
            $("#PART3_TEMP_NM1").text($("#TEMP_LIST1 option:selected").text());
            $("#PART3_FILE1").show();
            $("#FILE_NO_DATA").hide();
        } else {
            $("#PART3_FILE1").hide();
        }

        if ($("#doc_file2")[0].files.length > 0) {
            $("#PART3_TYPE_NM2").text("P/L");
            $("#PART3_FILE_NM2").text($("#doc_file2")[0].files[0].name);
            $("#PART3_TEMP_NM2").text($("#TEMP_LIST2 option:selected").text());
            $("#PART3_FILE2").show();
            $("#FILE_NO_DATA").hide();
        }

    }

    if (_fnToNull(vOCRResult) != "") {
        $("#PART3_FILE1").click();

        $("#PART3_TEMP_TLT").text($("#TEMP_TLT").text());
        $("#PART3_TEMP_CONT").text($("#TEMP_CONT").text());
        //$("#PART3_OCR_LIST").empty();


        //$.each(vOCRResult, function (i) {
        //    vHtml += "	<div class='req-field__cont'> ";
        //    vHtml += "        <p><span class='s_mo'>판독영역</span>" + "필드" + (i + 1) + "</p>";
        //    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + i + "</span></p>";
        //    vHtml += "        <p><span class='s_mo'>이름</span><span class='s_dt'>" + vOCRResult[i].name + "</span></p>";
        //    vHtml += "        <p><span class='s_mo'>내용</span><span class='s_dt'>" + vOCRResult[i].inferText + "</span></p>";
        //    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.top + "</span></p>";
        //    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.left + "</span></p>";
        //    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.width + "</span></p>";
        //    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].bounding.height + "</span></p>";
        //    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].valueType + "</span></p>";
        //    vHtml += "        <p style='display:none'><span class='s_mo'></span><span class='s_dt'>" + vOCRResult[i].inferConfidence + "</span></p>";
        //    vHtml += "    </div>";
        //});
        //$("#OCR_NO_DATA").hide();
        //$("#PART3_OCR_LIST").append(vHtml);
    } else if (_fnToNull(vOCRResultALL2) != "") {
        $("#PART3_FILE2").click();
    }else {
        $("#OCR_NO_DATA").show();
    }
}

//==========================PART4==================================


$("#btn_Quotation").click(function (e) {
  
   fnRequestData();
   //fnSendForwarderList();
});

function fnSearchForwarder() {
    var vHtml = ""
    var objJsonData = new Object();
    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
    if (_fnToNull($("#FRD_CUST_NM").val()) != "") {
        objJsonData.OFFICE_NM = _fnToNull($("#FRD_CUST_NM").val());
    }
    
    var rtnVal = _fnGetAjaxData("POST", "Estimate" , "fnGetForwarderList", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;

        $("#FRD_LIST").empty();

        $.each(vResult, function (i) {
            vHtml += "     <div class='req-field__cont type2 part1_add_cargo'>	";
            vHtml += "          <p class='chk type2'>";
            vHtml += "              <span class='check'>";
            vHtml += "                  <input type='checkbox' id='forward-chk" + i + "' />";
            vHtml += "                  <label for='forward-chk" + i + "'></label>";
            vHtml += "              </span>";
            vHtml += "          </p>";
            vHtml += "          <p><span class='s_mo'>상호명</span><label>" + _fnToNull(vResult[i].OFFICE_NM) + "</label></p>";
            vHtml += "          <p><span class='s_mo'>담당자</span><label>" + _fnToNull(vResult[i].LOC_NM) + "</label></p>";
            vHtml += "          <p><span class='s_mo'>이메일</span><label>" + _fnToNull(vResult[i].EMAIL) + "</label></p>";
            vHtml += "          <p><span class='s_mo'>연락처</span><label>" + _fnToNull(vResult[i].TEL_NO) + "</label></p>";
            vHtml += "          <p style='display:none'><span class='s_mo'>연락처</span><label>" + _fnToNull(vResult[i].CRN) + "</label></p>";
            vHtml += "      </div>";
        });

        $("#FRD_LIST").append(vHtml);
    } else {
        $("#FRD_LIST").empty();
    }
}

function fnSearchReccomendForwarderList() {

    var vHtml = "";
    var objJsonData = new Object();
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();

    objJsonData.POL_CD = $("#input_POLCD").val();
    objJsonData.POD_CD = $("#input_PODCD").val();
    objJsonData.ORDER_TYPE = $("#ORDER_TYPE option:selected").val();

    rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetReccomendForwarderList", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;

        $("#RECCOMEND_FRD_LIST").empty();

        $.each(vResult, function (i) {
            vHtml += "	<div class='req-forward'>	";
            vHtml += "        <div class='req-forward__nm'>";
            vHtml += "            <p><img src='/Images/fwd_logo/" + _fnToNull(vResult[i].CRN) + ".png' /></p>";
            vHtml += "            <button type='button' class='btns detail btnDetail'>상세</button>";
            vHtml += "        </div>";
            vHtml += "        <div class='req-forward__desc'>";
            vHtml += "            <div class='req-forward__cont'>";
            vHtml += "                <p class='req-forward__title'>상호명</p>";
            vHtml += "                <p class='frd_data'>" + _fnToNull(vResult[i].OFFICE_NM) + "</p>";
            vHtml += "            </div>";
            vHtml += "            <div class='req-forward__cont'>";
            vHtml += "                <p class='req-forward__title'>담당자</p>";
            vHtml += "                <p class='frd_data'>" + _fnToNull(vResult[i].LOC_NM) + "</p>";
            vHtml += "            </div>";
            vHtml += "            <div class='req-forward__cont'>";
            vHtml += "                <p class='req-forward__title'>이메일</p>";
            vHtml += "                <p class='frd_data'>" + _fnToNull(vResult[i].EMAIL) + "</p>";
            vHtml += "            </div>";
            vHtml += "            <div class='req-forward__cont'>";
            vHtml += "                <p class='req-forward__title'>연락처</p>";
            vHtml += "                <p class='frd_data'>" + _fnToNull(vResult[i].TEL_NO) + "</p>";
            vHtml += "            </div>";
            vHtml += "            <div class='req-forward__cont' style='display:none'>";
            vHtml += "                <p class='req-forward__title'>담당자</p>";
            vHtml += "                <p class='frd_data'>" + _fnToNull(vResult[i].CRN) + "</p>";
            vHtml += "            </div>";
            vHtml += "        </div>";
            vHtml += "    </div>";
        });
    } else {
        vHtml = "   <div class=\"req-field__cont no_data\"> ";
        vHtml += "   	<p><img src=\"/Images/no_data.png\" /></p> ";
        vHtml += "   </div> ";
    }

    $("#RECCOMEND_FRD_LIST").append(vHtml);
}


function fnRequestData() {

    var forwardCnt = checkForwarder();
    if (forwardCnt == 0) {
        _fnAlertMsg("포워더를 선택해주세요");
        return false;
    }
    //Main 데이터 넘기기
    var objJsonData = new Object();
    objJsonData.USR_ID = $("#Session_USR_ID").val();
    objJsonData.CRN = $("#Session_CRN").val();
    if ($("#SEA").hasClass("on")) {
        objJsonData.REQ_SVC = "SEA";
    } else {
        objJsonData.REQ_SVC = "AIR";
    }
    objJsonData.POL_CD = _fnToNull($("#input_POLCD").val());
    objJsonData.POD_CD = _fnToNull($("#input_PODCD").val());
    objJsonData.POL_NM = _fnToNull($("#input_POL").val());
    objJsonData.POD_NM = _fnToNull($("#input_POD").val());
    objJsonData.ETD = _fnToNull($("#ETD").val().replace(/-/gi,''));
    objJsonData.ETA = _fnToNull($("#ETA").val().replace(/-/gi, ''));
    objJsonData.RMK =_fnToNull($("#RMK").val());
    objJsonData.ITEM_NM = _fnToNull($("#MAIN_ITEM").val());
    objJsonData.PIC_CD = _fnToNull($("#Session_USR_ID").val());
    objJsonData.PIC_NM = _fnToNull($("#Session_USR_NM").val());
    objJsonData.PIC_TEL = _fnToNull($("#Session_HP_NO").val().replace(/-/gi, ''));
    objJsonData.PIC_MAIL = _fnToNull($("#Session_USR_ID").val());
    objJsonData.OFFICE_NM = _fnToNull($("#Session_CUST_NM").val());
    objJsonData.REQ_FWD_CNT = forwardCnt;

    objDs.MAIN = JSON.parse(_fnMakeJson(objJsonData));


    // 화물정보 넘기기
    var obj1 = "";
    var arry = new Array();
    //FCL Grid
    var tblhdr = $('.head_col .fcl').map(function () {
        return $(this).attr('data-value');
    }).get();

    var tbl = $('.req-info__cargo2').map(function (idx, el) {
        const td = $(el).find('span').text().replace(/CM/gi, '').replace(/,/gi, '').split("|");
        obj1 = { id: idx + 1 };
        for (var i = 0; i < tblhdr.length; i++) {
            obj1[tblhdr[i]] = td[i].trim();
        }
        arry.push(obj1);

        return obj1;
    }).get();


    var jsonArray = JSON.parse(JSON.stringify(arry));
    objDs.DIM = jsonArray;

    // 파일정보 넘기기
    objJsonData = new Object();
    arry = new Array();
    obj1 = "";

    if ($("#doc_file1")[0].files.length > 0) {
        obj1 = { id: 1 };
        obj1["DOC_TYPE"] = "CI";
        obj1["NC_OCR_ID"] = $("#TEMP_LIST1 option:selected").val();
        arry.push(obj1);
    }

    if ($("#doc_file2")[0].files.length > 0) {
        obj1 = { id: 2 };
        obj1["DOC_TYPE"] = "PL";
        obj1["NC_OCR_ID"] = $("#TEMP_LIST2 option:selected").val();
        arry.push(obj1);
    }

    var jsonArray = JSON.parse(JSON.stringify(arry));
    objDs.DOC = jsonArray;


    // OCR 정보 넘기기 
    if (_fnToNull(vOCRResultALL) != "") {
        objJsonData = new Object();
        arry = new Array();
        obj1 = "";
        obj1 = { id: 1 };
        obj1["TMPLT_ID"] = _fnToNull($("#TMPLT_ID1").val());
        obj1["TMPLT_TYPE"] = _fnToNull("CI");
        obj1["NC_OCR_ID"] = _fnToNull($("#TEMP_LIST1 option:selected").val());
        obj1["OCR_RESULT"] = _fnToNull(vOCRResultALL.images[0].inferResult);
        obj1["OCR_MESSAGE"] = _fnToNull(vOCRResultALL.images[0].message);
        obj1["CRN"] = _fnToNull($("#Session_CRN").val());
        arry.push(obj1);

        if (_fnToNull(vOCRResultALL2) != "") {
            obj1 = { id: 2 };
            obj1["TMPLT_ID"] = _fnToNull($("#TMPLT_ID2").val());
            obj1["TMPLT_TYPE"] = _fnToNull("PL");
            obj1["NC_OCR_ID"] = _fnToNull($("#TEMP_LIST2 option:selected").val());
            obj1["OCR_RESULT"] = _fnToNull(vOCRResultALL2.images[0].inferResult);
            obj1["OCR_MESSAGE"] = _fnToNull(vOCRResultALL2.images[0].message);
            obj1["CRN"] = _fnToNull($("#Session_CRN").val());
            arry.push(obj1);
        }

        var jsonArray = JSON.parse(JSON.stringify(arry));
        objDs.OCR_MST = jsonArray;
        var map_id = 1;
        // 화물정보 넘기기
        obj1 = "";
        arry = new Array();
        //FCL Grid
        tblhdr = $('.ocrTb p').map(function () {
            return $(this).attr('data-value');
        }).get();

        tbl = $('.ocrRow').map(function (idx, el) {
            const td = $(el).find('.s_dt');
            obj1 = { id: map_id };
            for (var i = 0; i < tblhdr.length; i++) {
                obj1[tblhdr[i]] = td.eq(i).text().trim();
            }
            arry.push(obj1);
            return obj1;
        }).get();

        map_id += 1;

        tbl = $('.ocrRow2').map(function (idx, el) {
            const td = $(el).find('.s_dt');
            obj1 = { id: map_id };
            for (var i = 0; i < tblhdr.length; i++) {
                obj1[tblhdr[i]] = td.eq(i).text().trim();
            }
            arry.push(obj1);
            return obj1;
        }).get();

        var jsonArray = JSON.parse(JSON.stringify(arry));
        objDs.OCR_DTL = jsonArray;
    }

    // 전체 데이터 API 전송
    var formData = new FormData();
    if ($("#doc_file1")[0].files.length > 0) {
        const files = $("#doc_file1")[0].files;
        formData.append("DOC", files[0]);
        if ($("#doc_file2")[0].files.length > 0) {
            const files2 = $("#doc_file2")[0].files;
            formData.append("DOC2", files2[0]);
        }
        
        formData.append("REQVAL", JSON.stringify(objDs));

        var request = new XMLHttpRequest();
        request.open("POST", _ApiUrl + "api/Quotation/SetQuotationRequest")
        request.setRequestHeader("Authorization-Type", "Y");
        request.setRequestHeader("Authorization-Token", _fnToNull($("#Session_AUTH_KEY").val()));
        request.send(formData);
        request.onload = function (e) {
            if (this.status == 200) {
                console.log('response', this.statusText);
                $("#QUOT_NO").val(this.responseText.replace(/\"/gi,''));
                fnSendForwarderList(forwardCnt); // 포워더 정보 전송
            }
        };
    } else {
        //파일데이터 없을시 
        formData.append("DOC", "");
        formData.append("REQVAL", JSON.stringify(objDs));

        var request = new XMLHttpRequest();
        request.open("POST", _ApiUrl + "api/Quotation/SetQuotationRequest")
        request.setRequestHeader("Authorization-Type", "Y");
        request.setRequestHeader("Authorization-Token", _fnToNull($("#Session_AUTH_KEY").val()));
        request.send(formData);
        request.onload = function (e) {
            if (this.status == 200) {
                console.log('response', this.statusText);
                $("#QUOT_NO").val(this.responseText.replace(/\"/gi,''));
                fnSendForwarderList(forwardCnt); // 포워더 정보 전송
            }
        };
    }
}

function checkForwarder() {
    var obj1 = new Object();
    var arry = new Array();

    var tblhdr = $('.Head .chkForward p').map(function () {
        return $(this).attr('data-value');
    }).get();

    var tbl = $('.part1_add_cargo').map(function (idx, el) {
        const td = $(el).find('span');
        obj1 = { id: idx + 1 };
        if (td.eq(0).find('input').prop("checked")) {
            for (var i = 0; i < tblhdr.length; i++) {
                obj1[tblhdr[i]] = td.next().eq(4).text();
            }
            arry.push(obj1);
        }
        return obj1;
    }).get();

    var tbl1 = $('.req-forward.on').map(function (idx, el) {
        const td = $(el).find('.req-forward__title').next();
        obj1 = { id: idx + 1 };
        for (var i = 0; i < tblhdr.length; i++) {
            obj1[tblhdr[i]] = td.eq(4).text();
        }
        arry.push(obj1);

        return obj1;
    }).get();

    return arry.length;
}

function checkForwarderAll() {
    var obj1 = new Object();
    var arry = new Array();

    var tblhdr = $('.Head .chkForward p').map(function () {
        return $(this).attr('data-value');
    }).get();

    var tbl1 = $('.req-forward').map(function (idx, el) {
        const td = $(el).find('.req-forward__title').next();
        obj1 = { id: idx + 1 };
        for (var i = 0; i < tblhdr.length; i++) {
            obj1[tblhdr[i]] = td.eq(4).text();
        }
        arry.push(obj1);

        return obj1;
    }).get();

    return arry.length;
}


$("#btn_all_esti").click(function (e) {

    var forwardCnt = checkForwarderAll();
    if (forwardCnt == 0) {
        _fnAlertMsg("추천 포워더가 없습니다. ");
        return false;
    }
    var objJsonData = new Object();
    objJsonData.USR_ID = $("#Session_USR_ID").val();
    objJsonData.CRN = $("#Session_CRN").val();
    if ($("#SEA").hasClass("on")) {
        objJsonData.REQ_SVC = "SEA";
    } else {
        objJsonData.REQ_SVC = "AIR";
    }
    objJsonData.POL_CD = _fnToNull($("#input_POLCD").val());
    objJsonData.POD_CD = _fnToNull($("#input_PODCD").val());
    objJsonData.POL_NM = _fnToNull($("#input_POL").val());
    objJsonData.POD_NM = _fnToNull($("#input_POD").val());
    objJsonData.ETD = _fnToNull($("#ETD").val().replace(/-/gi, ''));
    objJsonData.ETA = _fnToNull($("#ETA").val().replace(/-/gi, ''));
    objJsonData.RMK = _fnToNull($("#RMK").val());
    objJsonData.ITEM_NM = _fnToNull($("#MAIN_ITEM").val());
    objJsonData.PIC_CD = _fnToNull($("#Session_USR_ID").val());
    objJsonData.PIC_NM = _fnToNull($("#Session_USR_NM").val());
    objJsonData.PIC_TEL = _fnToNull($("#Session_HP_NO").val().replace(/-/gi, ''));
    objJsonData.PIC_MAIL = _fnToNull($("#Session_USR_ID").val());
    objJsonData.OFFICE_NM = _fnToNull($("#Session_CUST_NM").val());
    objJsonData.REQ_FWD_CNT = forwardCnt;

    objDs.MAIN = JSON.parse(_fnMakeJson(objJsonData));


    // 화물정보 넘기기
    var obj1 = "";
    var arry = new Array();
    //FCL Grid
    var tblhdr = $('.head_col .fcl').map(function () {
        return $(this).attr('data-value');
    }).get();

    var tbl = $('.req-info__cargo2').map(function (idx, el) {
        const td = $(el).find('span').text().replace(/CM/gi, '').replace(/,/gi, '').split("|");
        obj1 = { id: idx + 1 };
        for (var i = 0; i < tblhdr.length; i++) {
            obj1[tblhdr[i]] = td[i].trim();
        }
        arry.push(obj1);

        return obj1;
    }).get();


    var jsonArray = JSON.parse(JSON.stringify(arry));
    objDs.DIM = jsonArray;

    // 파일정보 넘기기
    objJsonData = new Object();
    arry = new Array();
    obj1 = "";

    if ($("#doc_file1")[0].files.length > 0) {
        obj1 = { id: 1 };
        obj1["DOC_TYPE"] = "CI";
        obj1["NC_OCR_ID"] = $("#TEMP_LIST1 option:selected").val();
        arry.push(obj1);
    }

    if ($("#doc_file2")[0].files.length > 0) {
        obj1 = { id: 2 };
        obj1["DOC_TYPE"] = "PL";
        obj1["NC_OCR_ID"] = $("#TEMP_LIST2 option:selected").val();
        arry.push(obj1);
    }

    var jsonArray = JSON.parse(JSON.stringify(arry));
    objDs.DOC = jsonArray;


    // OCR 정보 넘기기 
    if (_fnToNull(vOCRResultALL) != "") {
        objJsonData = new Object();
        arry = new Array();
        obj1 = "";
        obj1 = { id: 1 };
        obj1["TMPLT_ID"] = _fnToNull($("#TMPLT_ID1").val());
        obj1["TMPLT_TYPE"] = _fnToNull("CI");
        obj1["NC_OCR_ID"] = _fnToNull($("#TEMP_LIST1 option:selected").val());
        obj1["OCR_RESULT"] = _fnToNull(vOCRResultALL.images[0].inferResult);
        obj1["OCR_MESSAGE"] = _fnToNull(vOCRResultALL.images[0].message);
        obj1["CRN"] = _fnToNull($("#Session_CRN").val());
        arry.push(obj1);

        if (_fnToNull(vOCRResultALL2) != "") {
            obj1 = { id: 2 };
            obj1["TMPLT_ID"] = _fnToNull($("#TMPLT_ID2").val());
            obj1["TMPLT_TYPE"] = _fnToNull("PL");
            obj1["NC_OCR_ID"] = _fnToNull($("#TEMP_LIST2 option:selected").val());
            obj1["OCR_RESULT"] = _fnToNull(vOCRResultALL2.images[0].inferResult);
            obj1["OCR_MESSAGE"] = _fnToNull(vOCRResultALL2.images[0].message);
            obj1["CRN"] = _fnToNull($("#Session_CRN").val());
            arry.push(obj1);
        }

        var jsonArray = JSON.parse(JSON.stringify(arry));
        objDs.OCR_MST = jsonArray;
        var map_id = 1;
        // 화물정보 넘기기
        obj1 = "";
        arry = new Array();
        //FCL Grid
        tblhdr = $('.ocrTb p').map(function () {
            return $(this).attr('data-value');
        }).get();

        tbl = $('.ocrRow').map(function (idx, el) {
            const td = $(el).find('.s_dt');
            obj1 = { id: map_id };
            for (var i = 0; i < tblhdr.length; i++) {
                obj1[tblhdr[i]] = td.eq(i).text().trim();
            }
            arry.push(obj1);
            return obj1;
        }).get();

        map_id += 1;

        tbl = $('.ocrRow2').map(function (idx, el) {
            const td = $(el).find('.s_dt');
            obj1 = { id: map_id };
            for (var i = 0; i < tblhdr.length; i++) {
                obj1[tblhdr[i]] = td.eq(i).text().trim();
            }
            arry.push(obj1);
            return obj1;
        }).get();

        var jsonArray = JSON.parse(JSON.stringify(arry));
        objDs.OCR_DTL = jsonArray;
    }

    // 전체 데이터 API 전송
    var formData = new FormData();
    if ($("#doc_file1")[0].files.length > 0) {
        const files = $("#doc_file1")[0].files;
        formData.append("DOC", files[0]);
        if ($("#doc_file2")[0].files.length > 0) {
            const files2 = $("#doc_file2")[0].files;
            formData.append("DOC2", files2[0]);
        }

        formData.append("REQVAL", JSON.stringify(objDs));

        var request = new XMLHttpRequest();
        request.open("POST", _ApiUrl + "api/Quotation/SetQuotationRequest")
        request.setRequestHeader("Authorization-Type", "Y");
        request.setRequestHeader("Authorization-Token", _fnToNull($("#Session_AUTH_KEY").val()));
        request.send(formData);
        request.onload = function (e) {
            if (this.status == 200) {
                console.log('response', this.statusText);
                $("#QUOT_NO").val(this.responseText.replace(/\"/gi, ''));
            }
        };
    } else {
        //파일데이터 없을시 
        formData.append("DOC", "");
        formData.append("REQVAL", JSON.stringify(objDs));

        var request = new XMLHttpRequest();
        request.open("POST", _ApiUrl + "api/Quotation/SetQuotationRequest")
        request.setRequestHeader("Authorization-Type", "Y");
        request.setRequestHeader("Authorization-Token", _fnToNull($("#Session_AUTH_KEY").val()));
        request.send(formData);
        request.onload = function (e) {
            if (this.status == 200) {
                console.log('response', this.statusText);
                $("#QUOT_NO").val(this.responseText.replace(/\"/gi, ''));
            }
        };
    }

    var arry = new Array();
    var obj1 = new Object();
    var objForwardDs = new Object();

    var tblhdr = $('.Head .chkForward p').map(function () {
        return $(this).attr('data-value');
    }).get();


    var tbl1 = $('.req-forward').map(function (idx, el) {
        const td = $(el).find('.req-forward__title').next();
        obj1 = { id: idx + 1 };
        for (var i = 0; i < tblhdr.length; i++) {
            obj1[tblhdr[i]] = td.eq(4).text();
        }
        arry.push(obj1);

        return obj1;
    }).get();


    var jsonArray = JSON.parse(JSON.stringify(arry))
    objForwardDs.FWD_LIST = jsonArray;


    var objJson = new Object();
    objJson.CRN = _fnToNull($("#Session_CRN").val());
    objJson.QUOT_NO = _fnToNull($("#QUOT_NO").val());
    objJson.USR_ID = _fnToNull($("#Session_USR_ID").val());
    objJson.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());

    objForwardDs.MAIN = JSON.parse(_fnMakeJson(objJson));

    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnSendQuotationToElvis", objForwardDs);
    if (rtnVal == "Y") {
        $(".alert_cont .inner").html(forwardCnt + "개의 포워더로 견적요청 되었습니다.");
        layerPopup('#alert01', "", false);
        $("#alert_close").focus();
        $('#alert_close').click(function () {
            location.href = window.location.origin + "/Estimate/Inquiry";
        });
    }

});


function fnSendForwarderList(forwardCnt) {
    var arry = new Array();
    var obj1 = new Object();
    var objForwardDs = new Object();

    var tblhdr = $('.Head .chkForward p').map(function () {
        return $(this).attr('data-value');
    }).get();

    var tbl = $('.part1_add_cargo').map(function (idx, el) {
        const td = $(el).find('span');
        obj1 = { id: idx + 1 };
        if (td.eq(0).find('input').prop("checked")) {
            for (var i = 0; i < tblhdr.length; i++) {
                obj1[tblhdr[i]] = td.next().eq(4).text();
            }
        arry.push(obj1);
        }
        return obj1;
    }).get();

    var tbl1 = $('.req-forward.on').map(function (idx, el) {
        const td = $(el).find('.req-forward__title').next();
        obj1 = { id: idx + 1 };
        for (var i = 0; i < tblhdr.length; i++) {     
            obj1[tblhdr[i]] = td.eq(4).text();
        }
        arry.push(obj1);

        return obj1;
    }).get();


    var jsonArray = JSON.parse(JSON.stringify(arry))
    objForwardDs.FWD_LIST = jsonArray;


    var objJson = new Object();
    objJson.CRN = _fnToNull($("#Session_CRN").val());
    objJson.QUOT_NO = _fnToNull($("#QUOT_NO").val());
    objJson.USR_ID = _fnToNull($("#Session_USR_ID").val());
    objJson.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());

    objForwardDs.MAIN = JSON.parse(_fnMakeJson(objJson));

    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnSendQuotationToElvis", objForwardDs);
    if (rtnVal == "Y") {
        $(".alert_cont .inner").html(forwardCnt + "개의 포워더로 견적요청 되었습니다.");
        layerPopup('#alert01', "", false);
        $("#alert_close").focus();
        $('#alert_close').click(function () {
            location.href = window.location.origin + "/Estimate/Inquiry";
        });
    }

}

$("#ORDER_TYPE").change(function (e) {

    fnSearchReccomendForwarderList();
});

$(document).on("click", ".btnDetail", function () {
    var tr = $(this).closest('div');
    var td = tr.next().children();
    var objParam = new Object();
    objParam.CRN = td.eq(4).text().replace(/담당자/, '').trim();
    objParam.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
    objParam.POL_CD = _fnToNull($("#input_POLCD").val());
    objParam.POD_CD = _fnToNull($("#input_PODCD").val());


    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetFowarderInfo", objParam);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1[0];
        fnMakeChart(vResult);
    } else {
        _fnAlertMsg("업체 정보가 없습니다.");
    }
    $(this).closest(".req-forward").toggleClass("on");
});

$("#btnFrdSearch").click(function (e) {
    fnSearchForwarder();
});

$("#FRD_CUST_NM").keyup(function (e) {
    if (e.keyCode == 13) {
        fnSearchForwarder();
    }
});

function fnMakeChart(rtnResult) {
    $("#CUST_POL_NM").text(_fnToNull($("#input_POL").val()));
    $("#CUST_POD_NM").text(_fnToNull($("#input_POD").val()));

    $("#CUST_LOGO")[0].innerHTML = "<img src='/Images/fwd_logo/" + rtnResult.CRN + ".png'/>";
    $("#CUST_LOC_NM").text(_fnToNull(rtnResult.OFFICE_NM));
    $("#CUST_ADDR_NM").text(_fnToNull(rtnResult.OFFICE_ADDR));
    $("#CUST_TEL_NO").text(_fnToNull(rtnResult.TEL_NO));
    $("#CUST_EMAIL").text(_fnToNull(rtnResult.EMAIL));

    var AvgBLCnt = Math.ceil(_fnToZero(parseInt(rtnResult.TOT_DATA_CNT) / (parseInt(_fnToZero(rtnResult.DATA_CNT)) * 100)));
    var AvgRtonCnt = Math.ceil(_fnToZero(rtnResult.TOT_DATA_RTON) / (parseInt(_fnToZero(rtnResult.DATA_RTON)) * 100));
    var AvgTeuCnt = Math.ceil(_fnToZero(rtnResult.TOT_DATA_TEU) / (parseInt(_fnToZero(rtnResult.DATA_TEU)) * 100));

    $("#DATA_CNT").text(_fnToZero(_fnGetNumber(Math.ceil(rtnResult.DATA_CNT), "sum")));
    $("#DATA_RTON").text(_fnToZero(_fnGetNumber(Math.ceil(rtnResult.DATA_RTON), "sum")));
    $("#DATA_TEU").text(_fnToZero(_fnGetNumber(Math.ceil(rtnResult.DATA_TEU), "sum")));

    $("#chart1")[0].innerHTML = "<div class='fwd-chart' id='chart1'></div>";
    $("#chart2")[0].innerHTML = "<div class='fwd-chart' id='chart2'></div>";
    $("#chart3")[0].innerHTML = "<div class='fwd-chart' id='chart3'></div>";

    //포워더 레이어 차트

    var options1 = {
        series: [_fnToZero(AvgBLCnt)], //100% 중 퍼센트
        chart: {
            width: '100%',
            height: 175,
            type: 'radialBar',
        },
        plotOptions: {
            radialBar: {
                track: {
                    background: '#e5d1b5', //차트 빈공간 배경 색
                },
                dataLabels: {
                    name: {
                        show: false,
                    },
                    value: { //퍼센트 글자 나타내기
                        color: '#e1ab40',
                        fontSize: '14px',
                        offsetY: 5,
                    }
                }
            }
        },
        colors: ['#e1ab40'], //차트 채워지는 색
    };
    var chart1 = new ApexCharts(document.querySelector("#chart1"), options1);
    chart1.render();


    var options2 = {
        series: [_fnToZero(AvgRtonCnt)],
        chart: {
            width: '100%',
            height: 175,
            type: 'radialBar',
        },
        plotOptions: {
            radialBar: {
                track: {
                    background: '#e5d1b5',
                },
                dataLabels: {
                    name: {
                        show: false,
                    },
                    value: {
                        color: '#e68234',
                        fontSize: '14px',
                        offsetY: 5,
                    }
                }
            }
        },
        colors: ['#e68234'],
    };
    var chart2 = new ApexCharts(document.querySelector("#chart2"), options2);
    chart2.render();

    var options3 = {
        series: [_fnToZero(AvgTeuCnt)],
        chart: {
            height: 175,
            type: 'radialBar',
        },
        width: '100%',
        plotOptions: {
            radialBar: {
                track: {
                    background: '#e8efb1',
                },
                dataLabels: {
                    name: {
                        show: false,
                    },
                    value: {
                        color: '#b1c454',
                        fontSize: '14px',
                        offsetY: 5,
                    }
                }
            }
        },
        colors: ['#b1c454'],
    };
    var chart3 = new ApexCharts(document.querySelector("#chart3"), options3);
    chart3.render();
    if (_fnToNull(rtnResult.INFO_FILE_PATH) != "") {
        var objJsonData = new Object();
        objJsonData.FILE_PATH = rtnResult.INFO_FILE_PATH + rtnResult.INFO_FILE_NM + rtnResult.INFO_FILE_EXT;
        objJsonData.FILE_NM = rtnResult.INFO_FILE_NM + rtnResult.INFO_FILE_EXT;
        $("#DOC_TYPE").text();
        //layerPopup('#pdfLayer');
        //$("#iframe_test").attr("src", _ApiUrl +  objJsonData.FILE_PATH);
        $.ajax({
            type: "POST",
            url: "/Main/FileCustApiToServerDownload",
            async: true,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                console.log(result);
                $("#iframe_test2").attr("src", "/web/viewer.html?file=/Files/TEMP/" + objJsonData.FILE_NM);

            }, error: function (xhr) {
                console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
                console.log(xhr);
                return;
            }
        });

    } else {
        $("#iframe_test2").text("업체소개자료가 없습니다.");
    }
    layerPopup("#forwarderLayer");
}
///////////////////////////////API///////////////////////////////
//var objDs = new Object();
//$(".btns.next").on("click", function () {
//    try {
//        var obj1 = "";
//        var arry = new Array();
//        var exit = false;
//        //FCL Grid
//        var tblhdr = $('.head_col .fcl').map(function () {
//            return $(this).attr('data-value');
//        }).get();

//        var tbl = $('.req-info__cargo2').map(function (idx, el) {
//            const td = $(el).find('p').text().split("|");
//            obj1 = { id: idx + 1 };
//            for (var i = 0; i < tblhdr.length; i++) {
//                obj1[tblhdr[i]] = td[i+1].trim();
//            }

//            arry.push(obj1);

//            return obj1;
//        }).get();


//        var jsonArray = JSON.parse(JSON.stringify(arry))
//        objDs.DIM = jsonArray;
        
//        var obj2 = new Object();

//        obj2.POL_CD = $("#input_POL").val();
//        obj2.POD_CD = $("#input_POD").val();
//        obj2.ETD = $("#ETD").val();
//        obj2.ETA = $("#ETA").val();
//        obj2.RMK = $("#RMK").val();

//        objDs.MAIN = JSON.parse(_fnMakeJson(obj2));
//        var test = JSON.stringify(objDs);
//        var formData = new FormData();
//        if ($("#doc_file1")[0].files.length > 0) {
//            const files = $("#doc_file1")[0].files;
//            var i = 0;

//            formData.append("DOC", files[0]);
//            formData.append("MAIN", JSON.stringify(objDs));


//            var request = new XMLHttpRequest();
//            request.open("POST", _ApiUrl + "api/UserData/LicenseUpload")
//            request.setRequestHeader("Authorization-Type", "N");
//            request.send(formData);
//            request.onload = function (e) {
//                if (this.status == 200) {
//                    console.log('response', this.statusText);
//                }
//            };
//        }

//    } catch (err) {
//        console.log(err.message);
//    }
//});