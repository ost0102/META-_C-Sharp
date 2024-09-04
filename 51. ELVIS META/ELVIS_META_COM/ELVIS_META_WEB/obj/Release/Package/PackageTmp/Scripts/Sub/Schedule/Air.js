////////////////////전역 변수//////////////////////////
var _vPage = 0;
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
    $(".sub_sch .sub_depth li:nth-child(2) a").addClass("on");

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
    $("#AIRTable_List th button").removeClass();
    _isSearch = false;
    //_vPage = 0;
    fnSearchSchMst();
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

/////////////////////function///////////////////////////////////
//포트 정보 가져오기 
function fnGetPortData(vValue) {
    try {
        var rtnJson;
        var objJsonData = new Object();

        objJsonData.REQ_SVC = "AIR";

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
            objJsonData.REQ_SVC = "AIR";               
            
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

//function goPage(pageIndex) {
//    _vPage = pageIndex-1;
//    fnSearchSchMst();
//}

//totalData = 총 데이터 count
//dataPerPage = 한페이지에 나타낼 데이터 수
// pageCount = 한화면에 나타낼 페이지 수
//currentPage = 선택한 페이지 
//function fnPaging(totalData, dataPerPage, pageCount, currentPage) {
//    var totalPage = Math.ceil(totalData / dataPerPage);    // 총 페이지 수
//    var pageGroup = Math.ceil(currentPage / pageCount);    // 페이지 그룹            
//    if (pageCount > totalPage) pageCount = totalPage;
//    var last = pageGroup * pageCount;    // 화면에 보여질 마지막 페이지 번호
//    if (last > totalPage) last = totalPage;
//    var first = last - (pageCount - 1);    // 화면에 보여질 첫번째 페이지 번호
//    var next = last + 1;
//    var prev = first - 1;

//    //$("#paging_list").empty();

//    var prevPage;
//    var nextPage;
//    if (currentPage - 1 < 1) { prevPage = 1; } else { prevPage = currentPage - 1; }
//    if (last < totalPage) { nextPage = currentPage + 1; } else { nextPage = last; }

//    var vHTML = "";

//    vHTML += " <a href=\"javascript:void(0)\" onclick=\"goPage(1)\" class=\"page first\"><span class=\"blind\">처음페이지로 가기</span></a> ";
//    vHTML += " <a href=\"javascript:void(0)\" onclick=\"goPage(" + prevPage + ")\" class=\"page prev\"><span class=\"blind\">이전페이지로 가기</span></a> ";
    
//    for (var i = first; i <= last; i++) {
//        if (i == currentPage) {
//            vHTML += " <span class=\"number\"><span class=\"on\">" + i + "</span></span> ";
//        } else {
//            vHTML += " <span class=\"number\" onclick='goPage(" + i + ")'><span>" + i + "</span></span> ";
//        }
//    }

//    vHTML += " <a href=\"javascript:void(0)\" onclick=\"goPage(" + nextPage + ")\" class=\"page next\"><span class=\"blind\">다음페이지로 가기</span></a> ";
//    vHTML += " <a href=\"javascript:void(0)\" onclick=\"goPage(" + totalPage + ")\" class=\"page last\"><span class=\"blind\">마지막페이지로 가기</span></a> ";
    
//    $("#Paging_List_Area")[0].innerHTML = vHTML; // 페이지 목록 생성
//}

//show hide 유무 체크
function fnShowhideDTL(vThis) {
    try {
        if (!$(vThis).closest(".flex_type1").siblings(".Sch_ResultDtlArea").hasClass("show")) {
            $(".client .cliSwiper").removeClass("show");
            $(".client .cliSwiper").addClass("show_n");
            $(".client .cliSwiper").slideUp();
            //$(".bl_count").removeClass("on");
            $(".btn_freight").removeClass("on");

            var $cliSwiper = $(vThis).closest(".client").children(".cliSwiper");

            if ($cliSwiper.hasClass("show_n")) {
                $cliSwiper.slideDown();
                $cliSwiper.removeClass("show_n");
                $cliSwiper.addClass("show");
                $(vThis).addClass("on");
            }
            else if ($(vThis).siblings(".bl_count").hasClass("on")) {
                if ($(vThis).hasClass("on")) {
                    $cliSwiper.removeClass("show");
                    $cliSwiper.addClass("show_n");
                    $cliSwiper.slideUp();
                    $(vThis).siblings(".bl_count").removeClass("on");
                    $(vThis).removeClass("on");
                }
                $(vThis).siblings(".bl_count").removeClass("on");
                $(vThis).addClass("on");
            }
            else {
                $cliSwiper.removeClass("show");
                $cliSwiper.addClass("show_n");
                $cliSwiper.slideUp();
                $(vThis).siblings(".bl_count").removeClass("on");
                $(vThis).removeClass("on");
            }
        }
    }
    catch (err) {
        console.log("Error - fnShowhideDTL" + err.message);
    }
}

//디테일 정보 가져오기
function fnSearchSchDtl(vThis,vCntrType) {

    try {
        var objJsonData = new Object();

        objJsonData.REQ_SVC = $(vThis).closest(".client").find("div[name='div_SearchInfo'] input").eq(0).val();
        objJsonData.LINE_CD = $(vThis).closest(".client").find("div[name='div_SearchInfo'] input").eq(1).val();
        objJsonData.POL_CD = $(vThis).closest(".client").find("div[name='div_SearchInfo'] input").eq(2).val();
        objJsonData.POD_CD = $(vThis).closest(".client").find("div[name='div_SearchInfo'] input").eq(3).val();
        objJsonData.ETD = $(vThis).closest(".client").find("div[name='div_SearchInfo'] input").eq(4).val();
        objJsonData.ETA = $(vThis).closest(".client").find("div[name='div_SearchInfo'] input").eq(5).val();
        objJsonData.VSL = $(vThis).closest(".client").find("div[name='div_SearchInfo'] input").eq(6).val();
        objJsonData.VOY = $(vThis).closest(".client").find("div[name='div_SearchInfo'] input").eq(7).val();
        objJsonData.CNTR_TYPE = vCntrType;

        $.ajax({
            type: "POST",
            url: "/Schedule/fnGetSchDtlData",
            async: false,
            dataType: "json",
            //data: callObj,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                fnMakeSchDTL(result, $(vThis).closest(".client").find(".Sch_ResultDtlArea").attr("name"));
            }, error: function (xhr, status, error) {                
                _fnAlertMsg("담당자에게 문의 하세요.");
                console.log(error);
            }
        });
    }
    catch (err) {
        console.log("Error - fnSearchSchMst" + err.message);
    }
}

//스케줄 상세 정보 클릭 시 데이터 뿌려주기
function fnGetCustDTL(vThis) {
    try {        

        var objJsonData = new Object();

        objJsonData.DATE_YYYY_PAST = Number($(vThis).siblings("input[type='hidden']").eq(1).val().substring("0", "4")) - 1;        
        objJsonData.DATE_YYYY = $(vThis).siblings("input[type='hidden']").eq(1).val().substring("0","4");
        objJsonData.DATE_MM = $(vThis).siblings("input[type='hidden']").eq(1).val().substring("4", "6");
        objJsonData.OFFICE_CD = $(vThis).siblings("input[type='hidden']").eq(0).val();
        objJsonData.REQ_SVC = $(vThis).siblings("input[type='hidden']").eq(2).val();
        objJsonData.EX_IM_TYPE = $(vThis).siblings("input[type='hidden']").eq(3).val();
        objJsonData.POL_CD = $(vThis).siblings("input[type='hidden']").eq(4).val();
        objJsonData.POD_CD = $(vThis).siblings("input[type='hidden']").eq(5).val();

        $.ajax({
            type: "POST",
            url: "/Schedule/fnGetCustDtlData",
            async: false,
            dataType: "json",            
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                //alert(result);
                layerPopup('#clientPop');

                //년/월 세팅
                $("#sch_year_month").text(objJsonData.DATE_YYYY + "년 " + objJsonData.DATE_MM+"월");
                $("#sch_year").text(objJsonData.DATE_YYYY+"년");

                fnLineChart(result, "BL", objJsonData.DATE_YYYY_PAST, objJsonData.DATE_YYYY, objJsonData.DATE_MM, objJsonData.REQ_SVC);   //Line 차트
                fnMakeCustDTL(result); //거래처 정보
                fnApexChart(result);   //상세정보
                $("#sch_line_graph").css("visibility", "visible");
                //fnMakeSchDTL(result, $(vThis).closest(".client").find(".Sch_ResultDtlArea").attr("name"));
            }, error: function (xhr, status, error) {
                _fnAlertMsg("담당자에게 문의 하세요.");
                console.log(error);
            }
        });

    }
    catch (err) {
        console.log("Error - fnGetCustDTL" + err.message);
    }
}

//LineChart만 데이터 가져오기
function fnSetLineChart(vOFFICE_CD, vDATE_YYYY, vDATE_MM, vREQ_SVC, vPOL_CD, vPOD_CD, vEX_IM_TYPE, vDATA_TYPE) {
    try {
        var objJsonData = new Object();

        objJsonData.DATE_YYYY_PAST = Number(vDATE_YYYY) - 1;
        objJsonData.DATE_YYYY = vDATE_YYYY;
        objJsonData.DATE_MM = vDATE_MM;
        objJsonData.OFFICE_CD = vOFFICE_CD;
        objJsonData.REQ_SVC = vREQ_SVC;
        objJsonData.POL_CD = vPOL_CD;
        objJsonData.POD_CD = vPOD_CD;
        objJsonData.EX_IM_TYPE = vEX_IM_TYPE;

        $.ajax({
            type: "POST",
            url: "/Schedule/fnGetLineChartData",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {                
                fnLineChart(result, vDATA_TYPE, objJsonData.DATE_YYYY_PAST, objJsonData.DATE_YYYY, objJsonData.DATE_MM, objJsonData.REQ_SVC);   //Line 차트
                //fnMakeSchDTL(result, $(vThis).closest(".client").find(".Sch_ResultDtlArea").attr("name"));
            }, error: function (xhr, status, error) {
                _fnAlertMsg("담당자에게 문의 하세요.");
                console.log(error);
            }
        });
    }
    catch (err) {
        console.log("Error - fnSetLineChart" + err.message);
    }
}

//선뱍 위치 찾기 함수
function fnGetVslLocation(vThis) {
    try {

        var objJsonData = new Object();

        //1 LINE_CD , 2 POL_CD , 3 POD_CD , 6 VSL
        if (_fnToNull($(vThis).closest(".client").find("div[name='div_SearchInfo'] input[type='hidden']").eq(1).val()) == "") {
            _fnAlertMsg("위치를 확인 할 수 없습니다.");
            return false;
        }
        
        if (_fnToNull($(vThis).closest(".client").find("div[name='div_SearchInfo'] input[type='hidden']").eq(2).val()) == "") {
            _fnAlertMsg("위치를 확인 할 수 없습니다.");
            return false;
        }

        if (_fnToNull($(vThis).closest(".client").find("div[name='div_SearchInfo'] input[type='hidden']").eq(3).val()) == "") {
            _fnAlertMsg("위치를 확인 할 수 없습니다.");
            return false;
        }

        if (_fnToNull($(vThis).closest(".client").find("div[name='div_SearchInfo'] input[type='hidden']").eq(6).val()) == "") {
            _fnAlertMsg("위치를 확인 할 수 없습니다.");
            return false;
        }        

        objJsonData.reqVal1 = _fnToNull($(vThis).closest(".client").find("div[name='div_SearchInfo'] input[type='hidden']").eq(6).val());
        objJsonData.reqVal2 = _fnToNull($(vThis).closest(".client").find("div[name='div_SearchInfo'] input[type='hidden']").eq(2).val());
        objJsonData.reqVal3 = _fnToNull($(vThis).closest(".client").find("div[name='div_SearchInfo'] input[type='hidden']").eq(3).val());
        objJsonData.LINE_CD = _fnToNull($(vThis).closest(".client").find("div[name='div_SearchInfo'] input[type='hidden']").eq(1).val());
        objJsonData.VSL = _fnToNull($(vThis).closest(".client").find("div[name='div_SearchInfo'] input[type='hidden']").eq(6).val());
        
        var strurl = _ApiUrl + "api/Trk/GetTrackingVessel"; //스케줄        
        $("#ProgressBar_Loading").show(); //프로그래스 바

        $.ajax({
            url: strurl,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization-Token', _ApiKey);
            },
            type: "POST",
            async: true,
            dataType: "json",
            data: { "": _fnMakeJson(objJsonData) },
            success: function (result) {
                $("#ProgressBar_Loading").hide(); //프로그래스 바
                if (_fnToNull(result) != "") {
                    var rtnData = JSON.parse(result);
                    if (_fnToNull(rtnData.Result) != "") {
                        if (rtnData.Result[0].trxCode == "N" || rtnData.Result[0].trxCode == "E") {
                            //drawingLayerNodata();
                            _fnAlertMsg("위치를 확인 할 수 없습니다.");
                            return false;
                        }
                    } else {
                        //선박 위치의 경도 위도가 없을 경우 예외처리
                        if (_fnToNull(rtnData.Master[0].MAP_LAT) != "" && _fnToNull(rtnData.Master[0].MAP_LNG) != "") {
                            layerPopup('#trackingPop');
                            fnMakeRealLocation(result, objJsonData); //데이터 넣기
                            drawingLayer(rtnData);
                        } else {
                            _fnAlertMsg("위치를 확인 할 수 없습니다.");
                            return false;
                        }
                    }
                } else {
                    _fnAlertMsg("위치를 확인 할 수 없습니다.");
                }
                //createMap()
            }, error: function (xhr) {
                $("#ProgressBar_Loading").hide(); //프로그래스 바
                layerClose("#tracking_layer");
                _fnAlertMsg("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
                console.log(xhr);
                return;
            }
        });

    }
    catch (err) {
        console.log("Error - fnSetLineChart" + err.message);
    }
}

//VSL 데이터 가져오기
function fnGetVslData(vObj) {
    try {
        var objJsonData = new Object();
        objJsonData = vObj;

        var vResult;

        $.ajax({
            type: "POST",
            url: "/Schedule/fnGetVslData",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                vResult = result;
            }, error: function (xhr, status, error) {
                _fnAlertMsg("담당자에게 문의 하세요.");
                console.log(error);
            }
        });

        return vResult;
    }
    catch (err) {
        console.log("Error - fnGetVslData" + err.message);
    }
}

//선사 로고 있는지 체크
function fnGetLineImgPath(vLine) {
    try {
        var objJsonData = new Object();
        objJsonData.LINE_CD = vLine;

        var vResult;

        $.ajax({
            type: "POST",
            url: "/Schedule/fnGetLineImgPath",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                vResult = result;
            }, error: function (xhr, status, error) {
                _fnAlertMsg("담당자에게 문의 하세요.");
                console.log(error);
            }
        });

        return vResult;
    }
    catch (err) {
        console.log("Error - fnGetVslData" + err.message);
    }
}

//Sort
//$('.btn_order').click(function () {
//
//    if (_isSearch) {
//        $('.btn_order').removeClass('on');
//        $('#' + this.id).addClass('on');
//        if (sort) {
//            //내림차순
//            $('#' + this.id).removeClass('desc');
//            sort = false;
//            fnSchDesc(this.id, "ASC");
//        } else {
//            //오름차순
//            $('#' + this.id).addClass('desc');
//            sort = true;
//            fnSchDesc(this.id, "DESC");
//            //MakeBooking();
//        }
//    }    
//});

//sort 기능
$(document).on("click", "#AIRTable_List th", function () {
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
            $("#AIRTable_List th button").removeClass();
            $(this).find("button").addClass(vValue);

            fnSchDesc($(this).find("button").attr("id"), vValue.toUpperCase());
        }
    }
});

function fnSchDesc(id, desc) {
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

//////////////////////function makelist////////////////////////
//스케줄 Row 만들기
function fnMakeSchMST(vJsonData) {
    try {
        var vHTML = "";

        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            _isSearch = true;
            vResult = JSON.parse(vJsonData).Table1;

            //반복문
                $.each(vResult, function (i) {
                    vHTML += "	    <tr class='row Schedule_KMTC' data-row='row_" + i + "'>	";
                    vHTML += "                <td><img src='/Images/logo/" + vResult[i].LINE_CD + ".png' alt=''></td>	";
                    vHTML += "                <td>" +  vResult[i].VSL + "</td>	";
                    vHTML += "<td>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " " + _fnFormatTime(_fnToNull(vResult[i]["ETD_HM"])) + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ") " + "</td>";
                    if (_fnToNull(vResult[i]["ETA"]) != "") {
                        vHTML += "<td>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " " + _fnFormatTime(_fnToNull(vResult[i]["ETA_HM"])) + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ") " + "</td>";
                    } else {
                        vHTML += "<td>-</td>"
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
                    vHTML += "                                        <th>Flight No</th>	";
                    vHTML += "                                        <td>" + vResult[i].VSL + "</td>	";
                    vHTML += "                                    </tr>	";
                    vHTML += "                                    <tr>	";
                    vHTML += "                                        <th>Departure</th>	";
                    if (_fnToNull(vResult[i]["ETD"]) != "") {
                        vHTML += "<td>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " " + _fnFormatTime(_fnToNull(vResult[i]["ETD_HM"])) + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ") " + "</td>";
                    } else {
                        vHTML += "<td>-</td>"
                    }
                    vHTML += "                                    </tr>	";
                    vHTML += "                                    <tr>	";
                    vHTML += "                                        <th>Arrival</th>	";
                    if (_fnToNull(vResult[i]["ETA"]) != "") {
                        vHTML += "<td>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " " + _fnFormatTime(_fnToNull(vResult[i]["ETD_HM"])) + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ") " + "</td>";
                    } else {
                        vHTML += "<td>-</td>"
                    }
                    vHTML += "                                    </tr>	";
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
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
            _isSearch = false;
            vHTML += "   <tr> ";
            vHTML += "   	<td class=\"no_data\" colspan=\"8\"> ";
            vHTML += "   		<p><img src=\"/Images/no_data.png\" /></p> ";
            vHTML += "   	</td> ";
            vHTML += "   </tr> ";
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
            _isSearch = false;
            _fnAlertMsg("담당자에게 문의하세요");
            console.log("[Error - fnMakeSchMST] :" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
        }

        $("#Sch_ResultArea")[0].innerHTML = vHTML;
    }
    catch (err) {
        console.log("[Error - fnMakeSchMST]" + err.message);
    }
}

function fnMakeSchMSTDesc(vJsonData) {
    try {
        var vHTML = "";
        var vResult = "";

            vResult = JSON.parse(vJsonData);

            //반복문
            $.each(vResult, function (i) {
                vHTML += "	    <tr class='row Schedule_KMTC' data-row='row_" + i + "'>	";
                vHTML += "                <td><img src='/Images/logo/" + vResult[i].LINE_CD + ".png' alt=''></td>	";
                vHTML += "                <td>" + vResult[i].VSL + "</td>	";
                vHTML += "<td>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ") " + "</td>";
                if (_fnToNull(vResult[i]["ETA"]) != "") {
                    vHTML += "<td>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ") " + "</td>";
                } else {
                    vHTML += "<td>-</td>"
                }
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
                vHTML += "                                        <th>Flight No</th>	";
                vHTML += "                                        <td>"+ vResult[i].VSL + "</td>	";
                vHTML += "                                    </tr>	";
                vHTML += "                                    <tr>	";
                vHTML += "                                        <th>Departure</th>	";
                vHTML += " <td>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ") " + "</td>";
                vHTML += "                                    </tr>	";
                vHTML += "                                    <tr>	";
                vHTML += "                                        <th>Arrival</th>	";
                if (_fnToNull(vResult[i]["ETA"]) != "") {
                    vHTML += "<td>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay_Kor(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ") " + "</td>";
                } else {
                    vHTML += "<td>-</td>"
                }
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

            $("#Sch_ResultArea")[0].innerHTML = vHTML;

    }
    catch (err) {
        console.log("[Error - fnMakeSchMST]" + err.message);
    }
}