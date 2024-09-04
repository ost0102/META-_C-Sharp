////////////////////전역 변수//////////////////////////
//var _vPage = 0;
var _vSchDTLName = "";
var _sch_chart;
var _LineChart = new Object();
var mymap;
//var sort = false;
var _isSearch = false;
var vResult = "";
////////////////////jquery event///////////////////////
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    $(".sub_sch").addClass("on");
    $(".sub_sch .sub_depth").addClass("on");
    $(".sub_sch .sub_depth li:nth-child(1) a").addClass("on");

    $("#input_ETD").val(_fnPlusDate(0));
    $("#no_search").show();

    $("#ProgressBar_Loading").show(); //프로그래스 바
    setTimeout(function () {
        $("#ProgressBar_Loading").hide(); //프로그래스 바
    }, 1000);

});

//숫자만 입력 되게 수정
$(document).on("keyup", "#ETD", function () {
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
    if (!_fnisDate($(this).val())) {
        $(this).val($("#ETA").val());
        $(this).focus();
    }
});

//엔터키 입력시 마다 다음 input으로 가기
$(document).keyup(function (e) {
    if (e.keyCode == 13) {
        //alert($(e.target).attr('data-index'));
        if ($(e.target).attr('data-index') != undefined) {
            var vIndex = $(e.target).attr('data-index');
            if (parseFloat(vIndex) == 3) {                
                //_vPage = 0;
                fnSearchSchMst();
            } else {
                $('[data-index="' + (parseFloat(vIndex) + 1).toString() + '"]').focus();
            }
        }
    }
});

//해운 / 항공 클릭 시 데이터 초기화 시키기
$(document).on("click", "input[name='transfer']", function () {
    $("#Sch_ResultArea").hide();
    $("#no_data").hide();
    $("#no_search").show();
    $("#input_ETD").val(_fnPlusDate(0));
    $("#input_POL").val("");
    $("#input_POLCD").val("");
    $("#input_POD").val("");
    $("#input_PODCD").val("");
    $("#select_Service").val("");
    $(".delete").hide();
    $("#Paging_List_Area").empty();
});

//날짜 포스커 아웃 할때 벨리데이션
$(document).on("focusout", "#input_ETD", function () {
    if (!_fnisDate($(this).val())) {
        $(this).val("");
        $(this).focus();
    } else {
        var vValue = $(this).val();
        var vValue_Num = vValue.replace(/[^0-9]/g, "");
        if (vValue != "") {
            vValue = vValue_Num.substring("0", "4") + "-" + vValue_Num.substring("4", "6") + "-" + vValue_Num.substring("6", "8");
            $(this).val(vValue);
        }
    }
});

//조회 버튼 클릭 이벤트
$(document).on("click", "#btn_search", function () {
    //$('.btn_order').removeClass('on').removeClass('desc');
    //sort = false;
    $("#SEATable_List th button").removeClass();
    _isSearch = false;
    fnSearchSchMst();
});

//정렬 Sort 이벤트
//$('.btn_order').click(function () {
//    if (_isSearch) {
//        $('.btn_order').removeClass('on').removeClass('desc');
//        $('#' + this.id).addClass('on');
//        if (sort) {
//            //내림차순
//            $('#' + this.id).removeClass('desc');
//            sort = false;
//            fnSchDesc(this.id, "ASC");
//            //MakeBooking();            
//        } else {
//            //오름차순
//            $('#' + this.id).addClass('desc');
//            sort = true;            
//            fnSchDesc(this.id, "DESC");
//        }
//    }
//});

//sort 기능
$(document).on("click", "#SEATable_List th", function () {
    if (_isSearch) {
        if ($(this).find("button").length > 0) {

            var vValue = "";

            if ($(this).find("button").hasClass("asc")) {
                vValue = "desc";
            }
            else if ($(this).find("button").hasClass("desc")) {
                vValue = "asc";
            } else {
                vValue = "desc";
            }

            //초기화
            $("#SEATable_List th button").removeClass();
            $(this).find("button").addClass(vValue);

            fnSchDesc($(this).find("button").attr("id"), vValue.toUpperCase());
        }
    }
});

function fnSchDesc(id,desc) {
    var objDs = new Object();
    var objJsonData = new Object();
    objJsonData.ID = id;
    objJsonData.DESC = desc;
    if (_fnToNull(vResult) != "") {
        objDs.MAIN = JSON.parse(_fnMakeJson(objJsonData));
        objDs.DTL = JSON.parse(_fnMakeJson(vResult));

        $.ajax({
            type: "POST",
            url: "/Main/JsonDataDesc",
            async: false,
            dataType: "json",
            //data: callObj,
            data: { "vJsonData": JSON.stringify(objDs) },
            success: function (result) {
                fnMakeSchMSTDesc(result);
            }, error: function (xhr, status, error) {
                _fnAlertMsg("담당자에게 문의 하세요.");
                console.log(error);
            }
        });
    }
};

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

    //출발 도시 바로 선택 화면 가리기
    if ($(this).val().length > 0) {
        $("#select_SEA_pop01").hide();
        $("#select_AIR_pop01").hide();
    } else if ($(this).val().length == 0) {
        $("#select_SEA_pop01").hide();
        $("#select_AIR_pop01").hide();
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

    //출발 도시 바로 선택 화면 가리기
    if ($(this).val().length > 0) {
        $("#select_SEA_pop02").hide();
        $("#select_AIR_pop02").hide();
    } else if ($(this).val().length == 0) {
        $("#select_SEA_pop02").hide();
        $("#select_AIR_pop02").hide();
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

/////////////////////function///////////////////////////////////
//포트 정보 가져오기 
function fnGetPortData(vValue) {
    try {
        var rtnJson;
        var objJsonData = new Object();

        //선택에 따라서 Sea S / air A 체크
        objJsonData.REQ_SVC = "SEA";
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


//검색 조회 함수
function fnSearchSchMst() {
    try {
        if (fnValidation()) {           

            var objJsonData = new Object();

            objJsonData.REQ_SVC = "SEA";
            
            objJsonData.POL_CD = $("#input_POLCD").val(); 
            objJsonData.POD_CD = $("#input_PODCD").val();
            
            objJsonData.ST_DATE = $("#input_ETD").val().replace(/-/gi, "");
            objJsonData.END_DATE = _fnSetDate($("#input_ETD").val(), 30).replace(/-/gi, ""); // + 30일

            
            $.ajax({
                type: "POST",
                url: "/Schedule/fnGetSchData",
                async: true,
                dataType: "json",
                //data: callObj,
                data: { "vJsonData": _fnMakeJson(objJsonData) },
                success: function (result) {
                    $("#no_data").hide();
                    fnMakeSchMST(result);
                }, error: function (xhr, status, error) {
                    $("#ProgressBar_Loading").hide(); //프로그래스 바
                    _fnAlertMsg("담당자에게 문의 하세요.");
                    console.log(error);
                },
                beforeSend: function () {
//$("#ProgressBar_Loading").show(); //프로그래스 바
                },
                complete: function () {
                    //$("#ProgressBar_Loading").hide(); //프로그래스 바
                }
            });

        }
    } catch (err) {
        console.log("[Error - fnSearchSchMst]" + err.message);
    }
}

//search 밸리데이션
function fnValidation() {
    try {               

        if (_fnToNull($("#input_ETD").val()) == "") {
            _fnAlertMsg("날짜를 입력 해 주세요.",);
            return false;
        }
        
        if (_fnToNull($("#input_POL").val() == "")) {
            _fnAlertMsg("출발지를 입력 해 주세요.");
            return false;
        }

        if (_fnToNull($("#input_POLCD").val() == "")) {
            _fnAlertMsg("출발지를 입력 해 주세요.");
            return false;
        }

        if (_fnToNull($("#input_POD").val() == "")) {
            _fnAlertMsg("도착지를 입력 해 주세요.");
            return false;
        }


        if (_fnToNull($("#input_PODCD").val() == "")) {
            _fnAlertMsg("도착지를 입력 해 주세요.");
            return false;
        }

        return true;
    } catch (err) {
        console.log("[Error - fnValidation]" + err.message);
    }
}

//////////////////////function makelist////////////////////////
//스케줄 Row 만들기
function fnMakeSchMST(vJsonData) {
    try {
        var vHTML = "";
        $("#SCH_LIST").empty();
        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            _isSearch = true;
            vResult = JSON.parse(vJsonData).Table1;

            //반복문
            $.each(vResult, function (i) {
                vHTML += "	    <tr class='row Schedule_KMTC' data-row='row_" + i +"'>	";
                vHTML += "                <td><img src='/Images/logo/" + vResult[i].LINE_CD + ".png' alt=''></td>	";
                vHTML += "                <td><a href='javascript:void(0)' style='color:#1dadc7; padding-right:5px;' class='btnTracing'>" + vResult[i].VSL + "</a>" + vResult[i].LINE_CD + "</td>	";
                vHTML += "<td>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ") " +  "</td>";
                vHTML += "<td>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ") " +  "</td>";
                if (_fnToNull(vResult[i]["DOC_CLOS_YMD"]) == "" || _fnToNull(vResult[i]["DOC_CLOS_YMD"]) == "0") {
                    vHTML += "   	<td> ";
                    vHTML += "";
                } else {
                    vHTML += "   	<td style='color:#e0a049;'> ";
                    
                    vHTML += String(_fnToNull(vResult[i]["DOC_CLOS_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["DOC_CLOS_YMD"]).replace(/\./gi, ""))) + ")";
                    vHTML += "<span style='padding-left:5px;'>";
                    if (_fnToZero(vResult[i]["DOC_CLOS_HM"]) == 0) {
                        vHTML += "00:00";
                    } else {
                        vHTML += _fnFormatTime(_fnToNull(vResult[i]["DOC_CLOS_HM"]));
                    }
                    vHTML += "</span>";
                }
                //vHTML += "                <td>" + _fnToNull(vResult[i].CNTR_TYPE) + "</td>	";

                if (_fnToNull(vResult[i].TT_DAY) != "") {
                    if (vResult[i].TT_DAY == 0) {
                        vHTML += "                                        <td></td>	";
                    }
                    else if (vResult[i].TT_DAY == 1) {
                        vHTML += "                                        <td>" + _fnToNull(vResult[i].TT_DAY) + " Day</td>	";
                    } else if (vResult[i].TT_DAY > 0) {
                        vHTML += "                                        <td>" + _fnToNull(vResult[i].TT_DAY) + " Days</td>	";
                    } else {
                        vHTML += "                                        <td>" + _fnToNull(vResult[i].TT_DAY) + "</td>	";
                    }
                }
                else {
                    vHTML += "                                        <td></td>	";
                }                

                if (vResult[i].TS_YN == "N") {
                    vHTML += "                <td>Direct</td>	";
                } else {
                    vHTML += "                <td>TS</td>	";
                }
                vHTML += "                <td class='mobile_layout' colspan='9'>	";
                vHTML += "                    <div class='layout_type2'>	";
                vHTML += "                        <div class='row s3'>	";
                vHTML += "                            <table>	";
                vHTML += "                                <tbody>	";
                vHTML += "                                    <tr>	";
                vHTML += "                                        <th>Carrier</th>	";
                vHTML += "                                        <td><img src='/Images/logo/" + vResult[i].LINE_CD + ".png' alt=''></td>	";
                vHTML += "                                    </tr>	";
                vHTML += "                                    <tr>	";
                vHTML += "                                        <th>Vessel</th>	";
                vHTML += "                <td>" + vResult[i].LINE_CD + " <a href='javascript:void(0)' style='color:#1dadc7;' class='btnTracing'>" + vResult[i].VSL + "</a></td>	";
                vHTML += "                                    </tr>	";
                vHTML += "                                    <tr>	";
                vHTML += "                                        <th>Departure</th>	";
                vHTML += " <td>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ") " + "</td>";
                vHTML += "                                    </tr>	";
                vHTML += "                                    <tr>	";
                vHTML += "                                        <th>Arrival</th>	";
                vHTML += " <td>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ") "  + "</td>";
                vHTML += "                                    </tr>	";
                vHTML += "                                    <tr>	";
                vHTML += "                                        <th>Doc Closing</th>	";
                if (_fnToNull(vResult[i]["DOC_CLOS_YMD"]) == "" || _fnToNull(vResult[i]["DOC_CLOS_YMD"]) == "0") {
                    vHTML += "   	<td> ";
                    vHTML += "";
                } else {
                    vHTML += "   	<td> ";
                    vHTML += String(_fnToNull(vResult[i]["DOC_CLOS_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["DOC_CLOS_YMD"]).replace(/\./gi, ""))) + ")<br /> ";

                    if (_fnToZero(vResult[i]["DOC_CLOS_HM"]) == 0) {
                        vHTML += "00:00";
                    } else {
                        vHTML += _fnFormatTime(_fnToNull(vResult[i]["DOC_CLOS_HM"]));
                    }
                }
                vHTML += "                                    </tr>	";
                //vHTML += "                                    <tr>	";
                //vHTML += "                                        <th>Service</th>	";
                //vHTML += "                                        <td>" + _fnToNull(vResult[i].CNTR_TYPE) + "</td>	";
                //vHTML += "                                    </tr>	";
                vHTML += "                                    <tr>	";
                vHTML += "                                        <th>T/time</th>	";

                if (_fnToNull(vResult[i].TT_DAY) != "") {
                    if (vResult[i].TT_DAY == 0) {
                        vHTML += "                                        <td></td>	";
                    }
                    else if (vResult[i].TT_DAY == 1) {
                        vHTML += "                                        <td>" + _fnToNull(vResult[i].TT_DAY) + " Day</td>	";
                    } else if (vResult[i].TT_DAY > 0) {
                        vHTML += "                                        <td>" + _fnToNull(vResult[i].TT_DAY) + " Days</td>	";
                    } else {
                        vHTML += "                                        <td>" + _fnToNull(vResult[i].TT_DAY) + "</td>	";
                    }
                }
                else {
                    vHTML += "                                        <td></td>	";
                }

                vHTML += "                                    </tr>	";
                vHTML += "                                    <tr>	";
                vHTML += "                                        <th>T/S</th>	";
                if (vResult[i].TS_YN == "N") {
                    vHTML += "                                    <td>Direct</td>	";
                } else {
                    vHTML += "                                    <td>TS</td>	";
                }
                vHTML += "                                    </tr>	";
                vHTML += "                                </tbody>	";
                vHTML += "                            </table>	";
                vHTML += "                        </div>	";
                vHTML += "                    </div>	";
                vHTML += "                </td>	";
                vHTML += "	            </tr>	";
            });

            $("#no_search").hide(); //원하시는 정보 검색 숨기기
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
            _isSearch = false;
            vHTML += "   <tr> ";
            vHTML += "   	<td class=\"no_data\" colspan=\"7\"> ";
            vHTML += "   		<p><img src=\"/Images/no_data.png\" /></p> ";
            vHTML += "   	</td> ";
            vHTML += "   </tr> ";
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
            _isSearch = false;
            _fnAlertMsg("담당자에게 문의하세요");
            console.log("[Error - fnMakeSchMST] :" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
        }

        $("#SCH_LIST")[0].innerHTML = vHTML;
    }
    catch (err) {
        console.log("[Error - fnMakeSchMST]" + err.message);
    }
}


$(document).on("click", ".btnTracing", function () {
    if (fnGetTrackingParamLayer()) {
        if (fnChkTokenExpireLayer()) {
            $("#layervss").attr("src", "https://svmp.seavantage.com/#/tracking/ship?authToken=" + layerObj.TOKEN + "&shipName=" + $(this).text().trim().replace(/ /gi, ''));
            $("#vss_pop2").show();
        }
        else {
            $("#layervss").attr("src", "");
            $("#layersvmp").empty();
            $("#vss_pop2").hide();
        }
    }
    else {
        $("#layervss").attr("src", "");
        $("#layersvmp").empty();
        $("#vss_pop2").hide();
    }
});
var layerObj = new Object();
function fnGetTrackingParamLayer() {
    try {
        var vBoolean = true;
        var objJsonData = new Object();
        objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());

        $.ajax({
            type: "POST",
            url: "/Schedule/fnGetSvtgAuthToken",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (rtnVal) {
                if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
                    layerObj.TOKEN = JSON.parse(rtnVal).Table1[0]["AUTH_TOKEN"];
                    layerObj.ID = JSON.parse(rtnVal).Table1[0]["ID"];
                    layerObj.PWD = JSON.parse(rtnVal).Table1[0]["PWD"];
                    vBoolean = true;
                } else if (rtnVal.Result[0].trxCode == "N") {
                    _fnAlertMsg("토큰 정보가 없습니다");
                    vBoolean = false;
                }
            }, error: function (xhr) {
                console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
                console.log(xhr);
                return;
            }
        });
        return vBoolean;
    }
    catch (err) {
        console.log("[Error - fnGetTrackingParam()]" + err.message);
    }
}


////씨벤티지 Token 만료 된건지 체크하는 로직
//function fnChkTokenExpireLayer() {
//    try {

//        var vBoolean = true;

//        //토큰 만료 체크 확인
//        $.ajax({
//            url: "https://svmp.seavantage.com/api/v1/user/me",
//            beforeSend: function (xhr) {
//                xhr.setRequestHeader('Authorization', layerObj.TOKEN);
//            },
//            type: "GET",
//            async: false,
//            dataType: "json",
//            success: function (result) {
//                if (result.message == "OK") {
//                    vBoolean = true;
//                } else {
//                    vBoolean = false;
//                    _fnAlertMsg("담당자에게 문의하세요");
//                    console.log("[Error - fnChkLayerTokenExpire()]" + result.message);
//                }
//            }, error: function (xhr) {
//                if (JSON.parse(xhr.responseText).message == "UNAUTHORIZED") {
//                    console.log("[Error - fnChkLayerTokenExpire()]" + xhr);
//                    vBoolean = true;
//                }
//            }
//        });

//        return vBoolean;
//    }
//    catch (err) {
//        console.log("[Error - fnChkLayerTokenExpire()]" + err.message);
//    }
//}


function fnSetSvtgAuthToken() {
    try {
        var objJsonData = new Object();
        objJsonData.SVTG_ID = layerObj.ID;
        objJsonData.SVTG_PWD = layerObj.PWD;

        $.ajax({
            url: "https://svmp.seavantage.com/api/v1/user/authToken",
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', "Basic " +  btoa(objJsonData.SVTG_ID + ":" + objJsonData.SVTG_PWD));
            },
            type: "GET",
            async: false,
            contentType: "application/json",
            success: function (result) {
                if (result.message == "OK") {
                    layerObj.TOKEN = result.response.tokenId;
                } else {
                    vBoolean = false;
                    _fnAlertMsg("담당자에게 문의하세요");
                    console.log("[Error - fnChkLayerTokenExpire()]" + result.message);
                }
            }, error: function (xhr) {
                if (JSON.parse(xhr.responseText).message == "UNAUTHORIZED") {
                    console.log("[Error - fnChkLayerTokenExpire()]" + xhr);
                    vBoolean = true;
                }
            }
        });

        //$.ajax({
        //    type: "POST",
        //    url: "/Schedule/fnGetAuthToken",
        //    async: false,
        //    dataType: "json",
        //    data: { "vJsonData": _fnMakeJson(objJsonData) },
        //    success: function (rtnVal) {
        //        var test = rtnVal.response;
        //        layerObj.TOKEN = rtnVal.response.tokenId;
        //        alert("2");
        //    }, error: function (xhr) {
        //        alert("2-2");
        //        console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
        //        console.log(xhr);
        //        return;
        //    }
        //});
        objJsonData = new Object();
        objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
        objJsonData.AUTH_TOKEN = _fnToNull(layerObj.TOKEN);

        $.ajax({
            type: "POST",
            url: "/Schedule/fnSetSvtgAuthToken ",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (rtnVal) {
                if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
                    vBoolean = true;
                } else if (JSON.parse(rtnVal).Result[0]["trxCode"] == "N") {
                    _fnAlertMsg("토큰생성에 실패했습니다. 담당자에게 문의해주세요");
                }
            }, error: function (xhr) {
                console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
                console.log(xhr);
                return;
            }
        });
        return vBoolean;
    }
    catch (err) {
        console.log("[Error - fnGetTrackingParam()]" + err.message);
    }
}

function fnChkTokenExpireLayer() {
    try {
        var vBoolean = true;
        
        //토큰 만료 체크 확인
        $.ajax({
            url: "https://svmp.seavantage.com/api/v1/user/me",
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', layerObj.TOKEN);
            },
            type: "GET",
            async: false,
            dataType: "json",
            success: function (result) {
                if (result.message == "OK") {
                    vBoolean = true;
                } else {
                    vBoolean = false;
                    _fnAlertMsg("담당자에게 문의하세요");
                    console.log("[Error - fnChkLayerTokenExpire()]" + result.message);
                }
            }, error: function (xhr) {
                //if (JSON.parse(xhr.responseText).message == "UNAUTHORIZED") {
                    fnSetSvtgAuthToken();
                    vBoolean = true;
                //}
            }
        });

        return vBoolean;
    }
    catch (err) {
        console.log("[Error - fnLayerChkTokenExpire()]" + err.message);
    }
}
function fnMakeSchMSTDesc(vJsonData) {
    try {
        var vHTML = "";
        $("#SCH_LIST").empty();
        vResult = JSON.parse(vJsonData);
        //반복문
        $.each(vResult, function (i) {
            vHTML += "	    <tr class='row Schedule_KMTC' data-row='row_" + i + "'>	";
            vHTML += "                <td><img src='/Images/logo/" + vResult[i].LINE_CD + ".png' alt=''></td>	";
            vHTML += "                <td>" + vResult[i].LINE_CD + " <a href='javascript:void(0)' style='color:#1dadc7;' class='btnTracing'>" + vResult[i].VSL + "</a></td>	";
            vHTML += "<td>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ") " + "</td>";
            vHTML += "<td>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ") " + "</td>";
            if (_fnToNull(vResult[i]["DOC_CLOS_YMD"]) == "" || _fnToNull(vResult[i]["DOC_CLOS_YMD"]) == "0") {
                vHTML += "   	<td> ";
                vHTML += "";
            } else {
                vHTML += "   	<td> ";    
                vHTML += String(_fnToNull(vResult[i]["DOC_CLOS_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["DOC_CLOS_YMD"]).replace(/\./gi, ""))) + ")<br /> ";

                if (_fnToZero(vResult[i]["DOC_CLOS_HM"]) == 0) {
                    vHTML += "00:00";
                } else {
                    vHTML += _fnFormatTime(_fnToNull(vResult[i]["DOC_CLOS_HM"]));
                }
            }
            //vHTML += "                <td>" + _fnToNull(vResult[i].CNTR_TYPE) + "</td>	";

            if (_fnToNull(vResult[i].TT_DAY) != "") {
                if (vResult[i].TT_DAY == 0) {
                    vHTML += "                                        <td></td>	";
                }
                else if (vResult[i].TT_DAY == 1) {
                    vHTML += "                                        <td>" + _fnToNull(vResult[i].TT_DAY) + " Day</td>	";
                } else if (vResult[i].TT_DAY > 0) {
                    vHTML += "                                        <td>" + _fnToNull(vResult[i].TT_DAY) + " Days</td>	";
                } else {
                    vHTML += "                                        <td>" + _fnToNull(vResult[i].TT_DAY) + "</td>	";
                }
            }
            else {
                vHTML += "                                        <td></td>	";
            }

            if (vResult[i].TS_YN == "N") {
                vHTML += "                <td>Direct</td>	";
            } else {
                vHTML += "                <td>TS</td>	";
            }
            vHTML += "                <td class='mobile_layout' colspan='9'>	";
            vHTML += "                    <div class='layout_type2'>	";
            vHTML += "                        <div class='row s3'>	";
            vHTML += "                            <table>	";
            vHTML += "                                <tbody>	";
            vHTML += "                                    <tr>	";
            vHTML += "                                        <th>Carrier</th>	";
            vHTML += "                                        <td><img src='/Images/logo/" + vResult[i].LINE_CD + ".png' alt=''></td>	";
            vHTML += "                                    </tr>	";
            vHTML += "                                    <tr>	";
            vHTML += "                                        <th>Vessel</th>	";
            vHTML += "                                        <td><a href='javascript:void(0)' style='color:#1dadc7;' class='btnTracing'>" + vResult[i].VSL + "</a></td>	";
            vHTML += "                                    </tr>	";
            vHTML += "                                    <tr>	";
            vHTML += "                                        <th>Departure</th>	";
            vHTML += " <td>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ") " + "</td>";
            vHTML += "                                    </tr>	";
            vHTML += "                                    <tr>	";
            vHTML += "                                        <th>Arrival</th>	";
            vHTML += " <td>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ") " + "</td>";
            vHTML += "                                    </tr>	";
            vHTML += "                                    <tr>	";
            vHTML += "                                        <th>Doc Closing</th>	";
            if (_fnToNull(vResult[i]["DOC_CLOS_YMD"]) == "" || _fnToNull(vResult[i]["DOC_CLOS_YMD"]) == "0") {
                vHTML += "   	<td> ";
                vHTML += "";
            } else {
                vHTML += "   	<td> ";    
                vHTML += String(_fnToNull(vResult[i]["DOC_CLOS_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["DOC_CLOS_YMD"]).replace(/\./gi, ""))) + ")<br /> ";

                if (_fnToZero(vResult[i]["DOC_CLOS_HM"]) == 0) {
                    vHTML += "00:00";
                } else {
                    vHTML += _fnFormatTime(_fnToNull(vResult[i]["DOC_CLOS_HM"]));
                }
            }
            vHTML += "                                    </tr>	";
            //vHTML += "                                    <tr>	";
            //vHTML += "                                        <th>Service</th>	";
            //vHTML += "                                        <td>" + _fnToNull(vResult[i].CNTR_TYPE) + "</td>	";
            //vHTML += "                                    </tr>	";
            vHTML += "                                    <tr>	";
            vHTML += "                                        <th>T/time</th>	";
            if (_fnToNull(vResult[i].TT_DAY) != "") {
                if (vResult[i].TT_DAY == 0) {
                    vHTML += "                                        <td></td>	";
                }
                else if (vResult[i].TT_DAY == 1) {
                    vHTML += "                                        <td>" + _fnToNull(vResult[i].TT_DAY) + " Day</td>	";
                } else if (vResult[i].TT_DAY > 0) {
                    vHTML += "                                        <td>" + _fnToNull(vResult[i].TT_DAY) + " Days</td>	";
                } else {
                    vHTML += "                                        <td>" + _fnToNull(vResult[i].TT_DAY) + "</td>	";
                }
            }
            else {
                vHTML += "                                        <td></td>	";
            }
            vHTML += "                                    </tr>	";
            vHTML += "                                    <tr>	";
            vHTML += "                                        <th>T/S</th>	";
            if (vResult[i].TS_YN == "N") {
                vHTML += "                                    <td>Direct</td>	";
            } else {
                vHTML += "                                    <td>TS</td>	";
            }
            vHTML += "                                    </tr>	";
            vHTML += "                                </tbody>	";
            vHTML += "                            </table>	";
            vHTML += "                        </div>	";
            vHTML += "                    </div>	";
            vHTML += "                </td>	";
            vHTML += "	            </tr>	";
        });

        $("#no_search").hide(); //원하시는 정보 검색 숨기기
        $("#SCH_LIST")[0].innerHTML = vHTML;
    }
    catch (err) {
        console.log("[Error - fnMakeSchMST]" + err.message);
    }
}
