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

    $(".sub_exp").addClass("on");
    $(".sub_exp .sub_depth").addClass("on");
    $(".sub_exp .sub_depth li:nth-child(1) a").addClass("on");

    //$("#ETD").val(_fnMinusDate(7));
    $("#ETD").val("2022-11-01");
    $("#ETA").val(_fnPlusDate(7));

    $("#ProgressBar_Loading").show(); //프로그래스 바
    setTimeout(function () {
        $("#ProgressBar_Loading").hide(); //프로그래스 바
    }, 1000);

    fnSearchData();
});


//숫자만 입력 되게 수정
$(document).on("keyup", "#ETD", function () {
    $(this).val($(this).val().replace(/[^0-9\-]/gi, ""));
});

//숫자만 입력 되게 수정
$(document).on("keyup", "#ETA", function () {
    $(this).val($(this).val().replace(/[^0-9\-]/gi, ""));
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


//엔터키 입력시 마다 다음 input으로 가기
$(document).keyup(function (e) {
    if (e.keyCode == 13) {
        //alert($(e.target).attr('data-index'));
        if ($(e.target).attr('data-index') != undefined) {
            var vIndex = $(e.target).attr('data-index');
            if (parseFloat(vIndex) == 2) {
                if (_fnisDate($(e.target))){
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
                else{
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

//X 박스 클릭 시 안보이는 POLCD 데이터 값 지우기
$(document).on("click", "#btn_deletePOL", function () {
    $("#input_POLCD").val("");
});

//X 박스 클릭 시 안보이는 POLCD 데이터 값 지우기
$(document).on("click", "#btn_deletePOD", function () {
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


$(document).on("click", ".btnBL", function () {
    var tr = $(this).closest('div');
    var td = tr.children();

    var objJsonData = new Object();
    objJsonData.USR_ID = $("#Session_USR_ID").val();
    objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.MNGT_NO = td.eq(2).text().trim();
    objJsonData.DOC_TYPE = "HBL";
    objJsonData.SVC_TYPE = "EXPORT";

    var rtnVal = _fnGetAjaxData("POST", "Export" , "fnGetDocPath", objJsonData);
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


$(document).on("click", ".btnDo", function () {
    var tr = $(this).closest('div');
    var td = tr.children();

    var objJsonData = new Object
    objJsonData.USR_ID = $("#Session_USR_ID").val();
    objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.MNGT_NO = td.eq(2).text().trim();
    objJsonData.DOC_TYPE = "Do";
    objJsonData.SVC_TYPE = "EXPORT";

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



$(document).on("click", ".btnInv", function () {
    var tr = $(this).closest('div');
    var td = tr.children();

    var objJsonData = new Object();

    objJsonData.USR_ID = $("#Session_USR_ID").val();
    objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.MNGT_NO = td.eq(2).text().trim();
    objJsonData.DOC_TYPE = "INV";
    objJsonData.SVC_TYPE = "EXPORT";


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


$(document).on("click", ".btnEdi", function () {
    var tr = $(this).closest('div');
    var td = tr.children();

    var objJsonData = new Object();
    objJsonData.USR_ID = $("#Session_USR_ID").val();
    objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.MNGT_NO = td.eq(2).text().trim();
    objJsonData.DOC_TYPE = "MANI";
    objJsonData.SVC_TYPE = "EXPORT";

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





$(document).on("click", ".btnTax", function () {
    //window.open('http://bill.elvisprime.com/', 'newWindow');
    //사이트 URL 변경으로 인한 링크 변경
    window.open('http://elvisbill.com/', 'newWindow');
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
    objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
    objJsonData.FILE_PATH = td.eq(1).text().trim() + "/" + td.eq(4).text().trim();
    objJsonData.FILE_NM = td.eq(2).text().trim();
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

/////////////////////function///////////////////////////////////
//포트 정보 가져오기 
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
//조회 버튼 클릭 이벤트
$(document).on("click", "#btn_search", function () {
    fnSearchData();
});

function fnSearchData() {

    var objJsonData = new Object();

    objJsonData.CRN = $("#Session_CRN").val();
    objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
    objJsonData.REQ_SVC = $("#REQ_SVC").find("option:selected").val();
    objJsonData.EX_IM_TYPE = "E";
    objJsonData.YMD_TYPE = $("#YMD_TYPE option:selected").val();
    objJsonData.POL_CD = $("#input_POLCD").val();
    objJsonData.POD_CD = $("#input_PODCD").val();
    objJsonData.USR_TYPE = $("#Session_USR_TYPE").val();

    objJsonData.FM_YMD = $("#ETD").val().replace(/-/gi, "");
    objJsonData.TO_YMD = $("#ETA").val().replace(/-/gi, "");


    $.ajax({
        type: "POST",
        url: "/Export/fnGetExImList",
        async: true,
        dataType: "json",
        //data: callObj,
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            $("#no_data").hide();
            fnMakeList(result);
        }, error: function (xhr, status, error) {
            $("#ProgressBar_Loading").hide(); //프로그래스 바
            _fnAlertMsg("담당자에게 문의 하세요.");
            console.log(error);
        },
        beforeSend: function () {
          //  $("#ProgressBar_Loading").show(); //프로그래스 바
        },
        complete: function () {
          //  $("#ProgressBar_Loading").hide(); //프로그래스 바
        }
    });
}

function fnMakeList(vJsonData) {
    try {
        var vHTML = "";
        $("#eximList").empty();
        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            vResult = JSON.parse(vJsonData).Table1;

            //반복문
            $.each(vResult, function (i) {
                if (_fnToNull(vResult[i].REQ_SVC) == "SEA") {
                    vHTML += "<div class='exim-box exp ship none-border'>			";
                    vHTML += "  <div class='exim-icn'>";
                    vHTML += "      <img src='/Images/ship_export_icn.png'/>";
                } else {
                    vHTML += "<div class='exim-box exp air none-border'>			";
                    vHTML += "  <div class='exim-icn'>";
                    vHTML += "      <img src='/Images/air_export_icn.png'/>";
                }
                vHTML += "           <div class='exim-cont__prog mo'>";
                if (_fnToNull(vResult[i].BL_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished'>";
                } else{
                    vHTML += "               <div class='prog-status__stat present'>";
                }
                vHTML += "                   <div class='prog-status__bar'>";
                vHTML += "                       <div class='prog-status__percent'></div>";
                vHTML += "                   </div>";
                vHTML += "                   <button type='button' class='prog-status__nm btnBL'>B/L</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "               </div>";
                if (_fnToNull(vResult[i].INV_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished'>";
                } else{
                    vHTML += "               <div class='prog-status__stat present'>";
                }
                vHTML += "                   <div class='prog-status__bar'>";
                vHTML += "                       <div class='prog-status__percent'></div>";
                vHTML += "                   </div>";
                vHTML += "                   <button type='button' class='prog-status__nm btnInv'>INV</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "               </div>";
                if (_fnToNull(vResult[i].DO_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished'>";
                } else {
                    vHTML += "               <div class='prog-status__stat present'>";
                }
                vHTML += "                   <div class='prog-status__bar'>";
                vHTML += "                       <div class='prog-status__percent'></div>";
                vHTML += "                   </div>";
                vHTML += "                   <button type='button' class='prog-status__nm btnDo'>D/O</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "               </div>";
                if (_fnToNull(vResult[i].MFCS_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished'>";
                }else {
                    vHTML += "               <div class='prog-status__stat present'>";
                }
                vHTML += "                   <div class='prog-status__bar'>";
                vHTML += "                       <div class='prog-status__percent'></div>";
                vHTML += "                   </div>";
                vHTML += "                   <button type='button' class='prog-status__nm btnEdi'>EDI</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "               </div>";
                if (_fnToNull(vResult[i].TAX_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished'>";
                } else{
                    vHTML += "               <div class='prog-status__stat present'>";
                }
                vHTML += "                   <div class='prog-status__bar'>";
                vHTML += "                       <div class='prog-status__percent'></div>";
                vHTML += "                   </div>";
                vHTML += "                   <button type='button' class='prog-status__nm btnTax'>TAX</button>";
                vHTML += "               </div>";

                vHTML += "           </div>";
                vHTML += "       </div>";
                vHTML += "       <div class='exim-inner'>";
                vHTML += "           <div class='exim-cont text-info'>";
                vHTML += "               <div class='exim-cont__info line30'>";
                vHTML += "                   <div class='exim-cont__inner exim-title__flex'>";
                vHTML += "                       <div class='exim-cont__title'>HBL No</div>";
                vHTML += "                       <p>" + _fnToNull(vResult[i].HBL_NO) + "</p>";
                vHTML += "                   </div>";
                vHTML += "                   <div class='exim-cont__inner exim-title__flex'>";
                vHTML += "                       <div class='exim-cont__title'>MBL No</div>";
                if (_fnToNull(vResult[i].REQ_SVC) == "SEA") {
                    vHTML += "                       <p>" + _fnToNull(vResult[i].MBL_NO) + "</p>";
                } else {
                    vHTML += "                       <p>" + _fnToNull(vResult[i].MBL_NO).substring("0", "3") + "-" + _fnToNull(vResult[i].MBL_NO).substring("3") + "</p>";
                }
                vHTML += "                   </div>";
                vHTML += "               </div>";
                vHTML += "             <div class='exim-cont text-info'>";
                vHTML += "               <div class='exim-cont__info'>";
                vHTML += "                   <div class='exim-cont__inner'>";
                vHTML += "                      <div class='exim-cont__flex'>";
                vHTML += "                          <div class='exim-cont__desc'>";
                vHTML += "                              <p>" + _fnToNull(vResult[i].POL_NM) + "</p>";
                vHTML += "                          </div>";
                vHTML += "                          <div class='exim-cont__desc2'>";
                if (_fnToNull(vResult[i]["ETD"]) != "") {
                    vHTML += " " + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ") " + "</span></p>";
                }
                vHTML += "                          </div>";
                vHTML += "                      </div>";
                vHTML += "                  </div>";
                vHTML += "              </div>";
                vHTML += "              <div class='exim-cont__info progress-padding'>";
                vHTML += "                  <div class='exim-cont__inner'>";
                vHTML += "                     <p class='exim-cont__progress'>";
                vHTML += "                     <img src='/Images/icn_progress.png''/></p>";
                vHTML += "                  </div>";
                vHTML += "              </div>";
                vHTML += "              <div class='exim-cont__info'>";
                vHTML += "                   <div class='exim-cont__inner'>";
                vHTML += "                      <div class='exim-cont__flex'>";
                vHTML += "                          <div class='exim-cont__desc'>";
                vHTML += "                              <p>" + _fnToNull(vResult[i].POD_NM) + "</p>";
                vHTML += "                          </div>";
                vHTML += "                          <div class='exim-cont__desc2'>";
                if (_fnToNull(vResult[i]["ETA"]) != "") {
                    vHTML += " " + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ") " + "</span></p>";
                }
                vHTML += "                          </div>";
                vHTML += "                      </div>";
                vHTML += "                      </div>";
                vHTML += "                  </div>";
                vHTML += "               </div>";
                vHTML += "           </div>";
                vHTML += "           <div class='exim-cont prog-info pc'>";
                vHTML += "               <div class='exim-cont__prog'>";
                if (_fnToNull(vResult[i].BL_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished'>";
                }else {
                    vHTML += "               <div class='prog-status__stat present'>";
                }
                vHTML += "                       <div class='prog-status__bar'>";
                vHTML += "                           <div class='prog-status__percent'></div>";
                vHTML += "                       </div>";
                vHTML += "                       <button type='button' class='prog-status__nm btnBL'>B/L</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "                   </div>";
                if (_fnToNull(vResult[i].INV_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished'>";
                }else {
                    vHTML += "               <div class='prog-status__stat present'>";
                }
                vHTML += "                       <div class='prog-status__bar'>";
                vHTML += "                           <div class='prog-status__percent'></div>";
                vHTML += "                       </div>";
                vHTML += "                       <button type='button' class='prog-status__nm btnInv'>INV</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "                   </div>";
                if (_fnToNull(vResult[i].DO_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished'>";
                } else {
                    vHTML += "               <div class='prog-status__stat present'>";
                }
                vHTML += "                   <div class='prog-status__bar'>";
                vHTML += "                       <div class='prog-status__percent'></div>";
                vHTML += "                   </div>";
                vHTML += "                   <button type='button' class='prog-status__nm btnDo'>D/O</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "               </div>";
                if (_fnToNull(vResult[i].MFCS_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished'>";
                }else {
                    vHTML += "               <div class='prog-status__stat present'>";
                }
                vHTML += "                       <div class='prog-status__bar'>";
                vHTML += "                           <div class='prog-status__percent'></div>";
                vHTML += "                       </div>";
                vHTML += "                       <button type='button' class='prog-status__nm btnEdi'>EDI</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].EX_IM_TYPE) + "<span>";
                vHTML += "                   </div>";
                if (_fnToNull(vResult[i].TAX_SEND_DT) != "") {
                    vHTML += "               <div class='prog-status__stat finished'>";
                }else {
                    vHTML += "               <div class='prog-status__stat present'>";
                }
                vHTML += "                       <div class='prog-status__bar'>";
                vHTML += "                           <div class='prog-status__percent'></div>";
                vHTML += "                       </div>";
                vHTML += "                       <button type='button' class='prog-status__nm btnTax'>TAX</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "                   </div>";
                vHTML += "               </div>";
                vHTML += "           </div>";
                vHTML += "           <div class='exim-cont doc-info'>";
                vHTML += "               <button type='button' class='btns exim-cont__doc btnDocuList'>Document</button>";
                vHTML += "                       <p style='display:none'>" + _fnToNull(vResult[i].HBL_NO) + "<span>";
                vHTML += "           </div>";
                vHTML += "       </div>";
                vHTML += "   </div>";
                vHTML += "   </div>";
                vHTML += "   </div>";
            });
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



$(document).on("click", ".esti_btn", function () {
    var vHtml = "";

    var tr = $(this).closest('div');
    var td = tr.children();

    var objJsonData = new Object();

    objJsonData.DOC_TYPE = "QUOT";
    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
    objJsonData.CRN = _fnToNull($("#Session_CRN").val());
    objJsonData.MNGT_NO = td.eq(1).text().trim();

    var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetQuotationDoc", objJsonData);
    if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;

        objJsonData = new Object();
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
    }



});
/////////////////////function///////////////////////////////////

//////////////////////function makelist////////////////////////

/////////////////////////////API///////////////////////////////
