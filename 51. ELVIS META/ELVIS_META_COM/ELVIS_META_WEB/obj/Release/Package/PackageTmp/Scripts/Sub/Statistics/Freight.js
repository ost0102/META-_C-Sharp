////////////////////전역 변수//////////////////////////

var date = new Date();
var today_yyyy = date.getFullYear();
var now_yyyy = date.getFullYear();
////////////////////jquery event///////////////////////
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    $(".sub_stat").addClass("on");
    $(".sub_stat .sub_depth").addClass("on");
    $(".sub_stat .sub_depth li:nth-child(2) a").addClass("on");

    $("#cal_date").text(now_yyyy);

    $("#ProgressBar_Loading").show(); //프로그래스 바
    setTimeout(function () {
        $("#ProgressBar_Loading").hide(); //프로그래스 바
    }, 1000);
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

$("#btn_search").click(function (e) {
    if (fnValidation()) {

        var vData_Before = [];
        var vData_Now = [];
        $("#trend_graph")[0].innerHTML = "<div id=\"trend_graph\"></div>";

        var objJsonData = new Object();
        objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
        objJsonData.DATE_YYYY = _fnToNull($("#cal_date").text());
        objJsonData.POL_CD = _fnToNull($("#input_POLCD").val());
        objJsonData.POD_CD = _fnToNull($("#input_PODCD").val());
        objJsonData.CNTR_SIZE = _fnToNull($("#CNTR_SIZE option:selected").val());

        var rtnVal = _fnGetAjaxData("POST", "Statistic", "fnGetFrtCharge", objJsonData);

        if (JSON.parse(rtnVal).Result[0].trxCode == "Y") {
            var vResult = JSON.parse(rtnVal).Table1;


            var date_check = "1";
            var date_compare;
            var j = 0;
            if (vResult.length == 0) {
                for (var i = 0; i < vResult.length; i++) {
                    vData_Before.push(0);
                }
            } else {
                //데이터 만들기 1월~12월 Before
                for (var i = 0; i < vResult.length; i++) {

                    if (vResult[i]["DATE_YYYY"] == objJsonData.DATE_YYYY) {
                        vData_Now.push(Math.ceil(Number(vResult[j]["UNIT_AVG"])));
                        if ((j + 1) != vResult.length) {
                            j++;
                        }
                    } else {
                        if (vResult.length > 12) {
                            for (var k = 0; k < 12; k++) {
                                if (date_check.toString().length == 1) {
                                    date_compare = "0" + date_check;
                                } else {
                                    date_compare = date_check;
                                }
                                if (vResult[i]["DATE_MM"] == date_compare) {

                                    vData_Before.push(Math.ceil(Number(vResult[j]["UNIT_AVG"])));
                                    if ((k + 1) != 12) {
                                        i++;
                                    }
                                    if ((j + 1) != vResult.length) {
                                        j++;
                                    }
                                } else {
                                    vData_Before.push(0);
                                    i = 0;
                                }
                                date_check = parseInt(date_check) + 1;
                            }
                        } else {
                            for (var k = 0; k < vResult.length; k++) {
                                if (date_check.toString().length == 1) {
                                    date_compare = "0" + date_check;
                                } else {
                                    date_compare = date_check;
                                }
                                if (vResult[i]["DATE_MM"] == date_compare) {

                                    vData_Before.push(Math.ceil(Number(vResult[j]["UNIT_AVG"])));
                                    if ((k + 1) != 12) {
                                        i++;
                                    }
                                    if ((j + 1) != vResult.length) {
                                        j++;
                                    }
                                } else {
                                    vData_Before.push(0);
                                    i = 0;
                                }
                                date_check = parseInt(date_check) + 1;
                            }
                        }
                    }
                }
            }



            var options = {
                series: [{
                    name: parseInt(objJsonData.DATE_YYYY) - 1,
                    data: vData_Before
                }, {
                    name: objJsonData.DATE_YYYY,
                    data: vData_Now
                }],
                chart: {
                    height: '100%',
                    type: 'area',
                    toolbar: {
                        show: false,
                    },
                },
                colors: ['#fdbe3d', '#d2e187'],
                dataLabels: {
                    enabled: false
                },
                legend: {
                    position: 'top',
                    horizontalAlign: 'right',
                    fontSize: '16px',
                    fontWeight: '700'
                },
                fill: {
                    type: "gradient",
                    gradient: {
                        opacityFrom: 0.9,
                        opacityTo: 1,
                        stops: [100]
                    }
                },
                stroke: {
                    curve: 'smooth',
                    width: 0,
                },
                xaxis: {
                    categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                    axisBorder: {
                        show: false
                    },
                    axisTicks: {
                        show: false
                    }
                },
                yaxis: {
                    axisBorder: {
                        show: false
                    },
                    axisTicks: {
                        show: false
                    }
                },
                tooltip: {
                    x: {
                        format: 'MM'
                    },
                },
            };

            var chart = new ApexCharts(document.querySelector("#trend_graph"), options);
            chart.render();
        } else {
            for (var i = 0; i < 12; i++) {
                vData_Before.push(0);
            }
            for (var i = 0; i < 12; i++) {
                vData_Now.push(0);
            }


            var options = {
                series: [{
                    name: parseInt(objJsonData.DATE_YYYY) - 1,
                    data: vData_Before
                }, {
                    name: objJsonData.DATE_YYYY,
                    data: vData_Now
                }],
                chart: {
                    height: '100%',
                    type: 'area',
                    toolbar: {
                        show: false,
                    },
                },
                colors: ['#fdbe3d', '#d2e187'],
                dataLabels: {
                    enabled: false
                },
                legend: {
                    position: 'top',
                    horizontalAlign: 'right',
                    fontSize: '16px',
                    fontWeight: '700'
                },
                fill: {
                    type: "gradient",
                    gradient: {
                        opacityFrom: 0.9,
                        opacityTo: 1,
                        stops: [100]
                    }
                },
                stroke: {
                    curve: 'smooth',
                    width: 0,
                },
                xaxis: {
                    categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                    axisBorder: {
                        show: false
                    },
                    axisTicks: {
                        show: false
                    }
                },
                yaxis: {
                    axisBorder: {
                        show: false
                    },
                    axisTicks: {
                        show: false
                    }
                },
                tooltip: {
                    x: {
                        format: 'MM'
                    },
                },
            };

            var chart = new ApexCharts(document.querySelector("#trend_graph"), options);
            chart.render();
        }
    }

});


//search 밸리데이션
function fnValidation() {
    try {

        if (_fnToNull($("#input_POL").val() == "")) {
            _fnAlertMsg("출발지를 입력 해 주세요.");
            return false;
        }

        if (_fnToNull($("#input_POD").val() == "")) {
            _fnAlertMsg("도착지를 입력 해 주세요.");
            return false;
        }

        return true;
    } catch (err) {
        console.log("[Error - fnValidation]" + err.message);
    }
}


$("#btn_cal_right").click(function (e) {
    if (parseInt(now_yyyy) + 1 <= today_yyyy) {
        now_yyyy += 1;
        $("#cal_date").text(now_yyyy);

        $("#btn_search").click();
    }
});

$("#btn_cal_left").click(function (e) {
    now_yyyy -= 1;
    $("#cal_date").text(now_yyyy);
    $("#btn_search").click();
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