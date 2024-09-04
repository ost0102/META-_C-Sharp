////////////////////전역 변수//////////////////////////

////////////////////jquery event///////////////////////
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    //var swiper = new Swiper(".dash-swiper", {
    //    slidesPerView: 1,
    //    spaceBetween: 20,
    //    navigation: {
    //        nextEl: ".swiper-button-next",
    //        prevEl: ".swiper-button-prev"
    //    },
    //    breakpoints: {
    //        1024: {
    //            slidesPerView: 4,
    //        },
    //        768: {
    //            slidesPerView: 2
    //        }
    //    }
    //});

    $("#cal_date").text(fnSetNowDate()); //현재 날짜 세팅
    //$("#cal_date").text('2024.01'); //현재 날짜 세팅
    $("#ProgressBar_Loading").show(); //프로그래스 바
    if (_fnToNull($("#Session_USR_TYPE").val()) == "S") {
        $("#USE_ITEM").text("Elvis-meta(화주)");
    } else {
        $("#USE_ITEM").text("Elvis-meta(실행사)");
    }
    $("#USE_YMD").text(fnGetNowDate());

    setTimeout(function () {
        $("#ProgressBar_Loading").hide(); //프로그래스 바
    }, 1000);
    var vHtml = "<option value='" + _vSelectDate.getFullYear() + "'>" + _vSelectDate.getFullYear() + "</option><option value='" + (_vSelectDate.getFullYear() - 1) + "'>" + (_vSelectDate.getFullYear()- 1) + "</option>";
    console.log(vHtml);
    $("#SELECT_YYYY").append(vHtml);

    fnSearchData();
    fnSearchRecommend();

    
    //if (_fnToNull($("#Session_USR_TYPE").val()) == "S") { //화주
    //    myChart = new Chart(document.getElementById('UseGraph'));
    //    fnSearchData();
    //    fnSearchRecommend();
    //}
    //else { // 실행사 'F'

    //    myChart = new Chart(document.getElementById('UseGraph'));
    //    fnSearchData();
    //    fnSearchRecommend();
    //}
});


$("#SELECT_YYYY").change(function () {
    var vHtml = "";
    var objJsonData = new Object();
    $("#tbd_inv").empty();

    objJsonData.CRN = _fnToNull($("#Session_CRN").val());
    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
    objJsonData.DATE_YYYY = $("#SELECT_YYYY option:selected").val();

    //연간사용통계 조회
    $.ajax({
        type: "POST",
        url: "/Info/fnGetYearList",
        async: true,
        dataType: "json",
        //data: callObj,
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                var vResult = JSON.parse(result).Table1;
                $.each(vResult, function (i) {
                    vHtml += "	<tr>";
                    vHtml += "    <td>" + _fnToNull(vResult[i].USE_YMD).substring(0, 4) + "." + _fnToNull(vResult[i].USE_YMD).substring(4, 7) + "</td>";
                    vHtml += "    <td>" + _fnGetNumber(Math.ceil(parseFloat(vResult[i].FILE_CNE)), "sum") + "건</td>";
                    vHtml += "    <td>" + _fnGetNumber(Math.ceil(parseFloat(vResult[i].FILE_CNE)) * 100 , "sum") + "원</td>";
                    vHtml += "    <td>X</td>";
                    vHtml += "    <td><button type='button' class='btnPrint'>다운로드</button></td>";
                    vHtml += "	</tr>";
                });
                $("#tbd_inv").append(vHtml);

            }
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

});

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

function fnGetNowDate() {
    try {
        var weekDate = _vSelectDate.getTime() + (24 * 60 * 60 * 1000);
        _vSelectDate.setTime(weekDate);

        var weekYear = _vSelectDate.getFullYear();
        var weekMonth = _vSelectDate.getMonth() + 1;

        var lastDay = new Date(_vSelectDate.getFullYear(), _vSelectDate.getMonth() + 1, 0).getDate();

        var result = weekYear + "-" + _pad(weekMonth, "2") + "-" + "01 ~ " + weekYear + "-" + _pad(weekMonth, "2") + "-" + lastDay;
        return result;
    }
    catch (err) {
        console.log("[Error - fnSetNowDate]" + err.message);
    }
}

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

function fnSearchData() {

        var objJsonData = new Object();

        objJsonData.CRN = _fnToNull($("#Session_CRN").val());
        objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
        objJsonData.DATE_YYYY = $("#cal_date").text().split(".")[0];
        objJsonData.DATE_MM = $("#cal_date").text().split(".")[1];
        objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
        if (_fnToNull($("#Session_USR_TYPE").val()) == "S") {//화주
            if (_fnToNull($("#Session_DOCU").val()) == "Y" && _fnToNull($("#Session_QUOT").val()) == "Y") {
                objJsonData.DOC_TYPE = "'DOCU','QUOT'";
            } else if (_fnToNull($("#Session_DOCU").val()) == "Y") {
                objJsonData.DOC_TYPE = "'DOCU'";
            } else if (_fnToNull($("#Session_QUOT").val()) == "Y") {
                objJsonData.DOC_TYPE = "'QUOT'";
            }

            if (_fnToNull($("#Session_EXPORT").val()) == "Y" && _fnToNull($("#Session_IMPORT").val()) == "Y") {
                objJsonData.SVC_TYPE = "'EXPORT','IMPORT'";
            } else if (_fnToNull($("#Session_EXPORT").val()) == "Y") {
                objJsonData.SVC_TYPE = "'EXPORT'";
            } else if (_fnToNull($("#Session_IMPORT").val()) == "Y") {
                objJsonData.SVC_TYPE = "'IMPORT'";
            }

        } else if (_fnToNull($("#Session_USR_TYPE").val()) == "F") {//화주
            if (_fnToNull($("#Session_QUOT").val()) == "Y") {
                objJsonData.DOC_TYPE = "'QUOT'";
            }

            if (_fnToNull($("#Session_SEA_EX").val()) == "Y") {
                objJsonData.SEA_EX = "Y";
            }
            if (_fnToNull($("#Session_SEA_IM").val()) == "Y") {
                objJsonData.SEA_IM = "Y";
            }
            if (_fnToNull($("#Session_AIR_EX").val()) == "Y") {
                objJsonData.AIR_EX = "Y";
            }
            if (_fnToNull($("#Session_AIR_IM").val()) == "Y") {
                objJsonData.AIR_IM = "Y";
            }
        }
        $.ajax({
            type: "POST",
            url: "/Info/fnGetUsedInfo",
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
                //$("#ProgressBar_Loading").show(); //프로그래스 바
            },
            complete: function () {
                //$("#ProgressBar_Loading").hide(); //프로그래스 바
            }
        });
}


$(document).on("click", ".btn_esti", function () {
    var tr = $(this).closest('div');
    var td = tr.children();

    var objParam = new Object();
    objParam.POL_CD = td.eq(1).text().trim();
    objParam.POD_CD = td.eq(2).text().trim();
    objParam.POL_NM = td.eq(3).text().trim();
    objParam.POD_NM = td.eq(4).text().trim();
    objParam.ITEM_NM = td.eq(5).text().trim();
    objParam.REQ_SVC = td.eq(6).text().trim();

    controllerToLink("Request", "Estimate", objParam, false);
});
$(document).on("click", ".btnPrint", function () {
    _fnAlertMsg("청구서 발행 전입니다.")
});

function fnSearchRecommend() {
    var vHtml = "";
    var objJsonData = new Object();

    objJsonData.CRN = _fnToNull($("#Session_CRN").val());
    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
    objJsonData.DATE_YYYY = $("#cal_date").text().split(".")[0];
    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
    objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());

    objJsonData.PREV_YYYY = _vSelectDate.getFullYear() - 1;

    //연간사용통계 조회
    $.ajax({
        type: "POST",
        url: "/Info/fnGetYearCount",
        async: true,
        dataType: "json",
        //data: callObj,
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                var vResult = JSON.parse(result).YEARLIST;
                var vChart = JSON.parse(result).YEARCHART;
                $.each(vResult, function (i) {
                    vHtml += "	<tr>";
                    vHtml += "    <td>" + _fnToNull(vResult[i].USE_YMD).substring(0, 4) + "." + _fnToNull(vResult[i].USE_YMD).substring(4, 7) + "</td>";
                    vHtml += "    <td>" + _fnGetNumber(Math.ceil(parseFloat(vResult[i].FILE_CNT)), "sum") + "건</td>";
                    vHtml += "    <td>" + _fnGetNumber(Math.ceil(parseFloat(vResult[i].FILE_CNT)) * 100, "sum") + "원</td>";
                    vHtml += "    <td>X</td>";
                    vHtml += "    <td><button type='button' class='btnPrint'>다운로드</button></td>";
                    vHtml += "	</tr>";
                });
                $("#tbd_inv").append(vHtml);

                var now_year = new Array();
                var prev_year = new Array();
                var j = 0;
                for (var j = 0; j < vChart.length; j++) {
                    if (_fnToNull(vChart[j].USE_YMD).substring(0, 4) == (_vSelectDate.getFullYear() - 1)) {
                        prev_year.push(Math.ceil(parseFloat(vChart[j].FILE_CNT)));
                    } else if (_fnToNull(vChart[j].USE_YMD).substring(0, 4) == (_vSelectDate.getFullYear())) {
                        now_year.push(Math.ceil(parseFloat(vChart[j].FILE_CNT)));
                    }
                }

                console.log("w" + prev_year);
                console.log("wow" + now_year);



                //연간사용통계 그래프
                var options = {
                    series: [
                        {
                            name: _vSelectDate.getFullYear() - 1,
                            data: prev_year
                        },
                        {
                            name: _vSelectDate.getFullYear(),
                            data: now_year
                        }
                    ],
                    chart: {
                        width: 600,
                        height: 350,
                        type: 'line',
                        toolbar: {
                            show: false
                        }
                    },
                    colors: ['#f68e5c', '#434e9b'],
                    dataLabels: {
                        enabled: false,
                    },
                    stroke: {
                        curve: 'smooth'
                    },
                    title: {
                        align: 'left'
                    },
                    grid: {
                        borderColor: '#e7e7e7',
                        row: {
                            colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
                            opacity: 0.5
                        },
                    },
                    markers: {
                        size: 1
                    },
                    xaxis: {
                        categories: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
                    },
                    yaxis: {
                        show: false,
                    },
                    legend: {
                        position: 'bottom',
                        horizontalAlign: 'right',
                        floating: true,
                        offsetY: -30,
                        offsetX: -5,
                        markers: {
                            width: 12,
                            height: 12,
                            shape: 'square',
                        }
                    },
                    responsive: [{
                        breakpoint: 641,
                        options: {
                            chart: {
                                width: 300,
                                height: 300
                            },
                        },
                    }]
                };


                var chart = new ApexCharts(document.querySelector("#StatsGraph"), options);
                chart.render();


                //예상청구금액 그래프
                var options = {
                    chart: {
                        type: 'radialBar',
                        height: 250,
                    },
                    stroke: {
                        width: 0 // 차트의 테두리 너비를 0으로 설정하여 테두리 제거
                    },
                    series: [100], // 첫 번째 값은 실제 데이터, 두 번째 값은 더미 데이터
                    colors: ['#434e9b', '#eeeeff'], // 첫 번째 색은 실제 데이터, 두 번째 색은 차트의 배경색과 동일하게 설정
                    plotOptions: {
                        radialBar: {
                            startAngle: -90,
                            endAngle: 90,
                            track: {
                                background: '#e5e9ff', //차트 빈공간 배경 색
                            },
                            dataLabels: {
                                name: {
                                    show: false,
                                },
                                value: { //퍼센트 글자 나타내기
                                    show: false,
                                }
                            }
                        }
                    },
                    colors: ['#6565ff'], //차트 채워지는 색
                };

                var chart = new ApexCharts(document.querySelector("#CostGraph"), options);
                chart.render();


            }
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

function fnMakeList(vJsonData) {
    if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
        console.log(vJsonData);
        vResult = JSON.parse(vJsonData).Table1;
        if (_fnToNull($("#Session_USR_TYPE").val()) == "S") {
            var TOTAL = 0, TOTAL_SIZE = 0, QUOT_AVG = 0, DOCU_AVG = 0, IM_AVG = 0, EX_AVG = 0, EX_CNT = 0, IM_CNT = 0, DOCU_CNT = 0, QUOT_CNT = 0;
            for (var i = 0; i < vResult.length; i++) {
                TOTAL += parseInt(vResult[i]["CNT"]);
                TOTAL_SIZE += Math.round(parseFloat(vResult[i]["FILE_CNT"]));
            }
            var tot_dt = new Array();
            for (var i = 0; i < vResult.length; i++) {
                if (_fnToNull(vResult[i]["DOC_TYPE"]) == "DOCU") {
                    DOCU_AVG = _fnToZero(Math.round((parseInt(vResult[i]["CNT"]) / TOTAL) * 100));
                    DOCU_CNT = _fnToZero(Math.round(vResult[i]["FILE_CNT"]));
                } else if (_fnToNull(vResult[i]["DOC_TYPE"]) == "QUOT") {
                    QUOT_AVG = _fnToZero(Math.round((parseInt(vResult[i]["CNT"]) / TOTAL) * 100));
                    QUOT_CNT = _fnToZero(Math.round(vResult[i]["FILE_CNT"]));
                } else {
                    if (_fnToNull(vResult[i]["REQ_SVC"]) == "SEA") {
                        EX_AVG = _fnToZero(Math.round((parseInt(vResult[i]["CNT"]) / TOTAL) * 100));
                        EX_CNT = _fnToZero(Math.round(vResult[i]["FILE_CNT"]));
                    } else if (_fnToNull(vResult[i]["REQ_SVC"]) == "AIR") {
                        IM_CNT = _fnToZero(Math.round(vResult[i]["FILE_CNT"]));
                        IM_AVG = _fnToZero(Math.round((parseInt(vResult[i]["CNT"]) / TOTAL) * 100));
                    }
                }

            }

            $("#TOT").text(TOTAL_SIZE + "건");
            TOTAL_SIZE = TOTAL_SIZE * 100;
            $("#DEBIT_AMT").text("0원");
            $("#MON_AMT").text(_fnGetNumber(TOTAL_SIZE, "sum") + "원");
            $("#SUM_AMT").text(_fnGetNumber(TOTAL_SIZE, "sum") + "원");

            $("#DOCU_PER").addClass("dash-progress__bar");
            $("#QUOT_PER").addClass("dash-progress__bar");
            $("#EX_PER").addClass("dash-progress__bar");
            $("#IM_PER").addClass("dash-progress__bar");
            $("#DOCU_PER").addClass("per" + DOCU_AVG);
            $("#QUOT_PER").addClass("per" + QUOT_AVG);
            $("#EX_PER").addClass("per" + EX_AVG);
            $("#IM_PER").addClass("per" + IM_AVG);

            $("#DOCU_SIZE")[0].innerHTML = "<span class='use-progress_value' id='DOCU_SIZE'>" + _fnGetNumber(DOCU_CNT,"sum") + "건</span>";
            $("#QUOT_SIZE")[0].innerHTML = "<span class='use-progress_value' id='QUOT_SIZE'>" + _fnGetNumber(QUOT_CNT, "sum") + "건</span>";
            $("#EX_SIZE")[0].innerHTML = "<span class='use-progress_value' id='EX_SIZE'>" + _fnGetNumber(EX_CNT, "sum") + "건</span>";
            $("#IM_SIZE")[0].innerHTML = "<span class='use-progress_value' id='IM_SIZE'>" + _fnGetNumber(IM_CNT, "sum")+ "건</span>";
            var doc_title = new Array();
            var chk = false;
            if (_fnToNull($("#Session_DOCU").val()) == "Y") {
                $("#CHK_DOCU").show();
                doc_title.push('서식');
                tot_dt.push(DOCU_AVG);
            }

            if (_fnToNull($("#Session_QUOT").val()) == "Y") {
                $("#CHK_QUOT").show();
                doc_title.push('견적');
                tot_dt.push(QUOT_AVG);
            }

            if (_fnToNull($("#Session_EXPORT").val()) == "Y") {
                $("#CHK_EX").show();
                doc_title.push('수출');
                tot_dt.push(EX_AVG);
            }

            if (_fnToNull($("#Session_IMPORT").val()) == "Y") {
                $("#CHK_IM").show();
                doc_title.push('수입');
                tot_dt.push(IM_AVG);
            }
        } else {

            var TOTAL = 0, TOTAL_SIZE = 0, QUOT_AVG = 0, SEA_IM_AVG = 0, SEA_EX_AVG = 0, AIR_EX_AVG = 0, AIR_IM_AVG = 0, SEA_EX_CNT = 0, SEA_IM_CNT = 0, AIR_EX_CNT = 0, AIR_IM_CNT = 0 , QUOT_CNT = 0;
            for (var i = 0; i < vResult.length; i++) {
                TOTAL += parseInt(vResult[i]["CNT"]);
                TOTAL_SIZE += Math.round(parseFloat(vResult[i]["FILE_CNT"]));
            }
            var tot_dt = new Array();
            for (var i = 0; i < vResult.length; i++) {
                if (_fnToNull(vResult[i]["DOC_TYPE"]) == "QUOT") {
                    QUOT_AVG = _fnToZero(Math.round((parseInt(vResult[i]["CNT"]) / TOTAL) * 100));
                    QUOT_CNT = _fnToZero(Math.round(vResult[i]["FILE_CNT"]));
                } else {
                    if (_fnToNull(vResult[i]["REQ_SVC"]) == "SEA") {
                        if (_fnToNull(vResult[i]["SVC_TYPE"]) == "EXPORT") {
                            SEA_EX_AVG = _fnToZero(Math.round((parseInt(vResult[i]["CNT"]) / TOTAL) * 100));
                            SEA_EX_CNT = _fnToZero(Math.round(vResult[i]["FILE_CNT"]));
                        } else {
                            SEA_IM_AVG = _fnToZero(Math.round((parseInt(vResult[i]["CNT"]) / TOTAL) * 100));
                            SEA_IM_CNT = _fnToZero(Math.round(vResult[i]["FILE_CNT"]));
                        }
                    } else if (_fnToNull(vResult[i]["REQ_SVC"]) == "AIR") {
                        if (_fnToNull(vResult[i]["SVC_TYPE"]) == "EXPORT") {
                            AIR_EX_AVG = _fnToZero(Math.round((parseInt(vResult[i]["CNT"]) / TOTAL) * 100));
                            AIR_EX_CNT = _fnToZero(Math.round(vResult[i]["FILE_CNT"]));
                        } else {
                            AIR_IM_AVG = _fnToZero(Math.round((parseInt(vResult[i]["CNT"]) / TOTAL) * 100));
                            AIR_IM_CNT = _fnToZero(Math.round(vResult[i]["FILE_CNT"]));
                        }
                    }
                }

            }

            $("#TOT").text(TOTAL_SIZE + "건");
            TOTAL_SIZE = TOTAL_SIZE * 100;
            $("#DEBIT_AMT").text("0원");
            $("#MON_AMT").text(_fnGetNumber(TOTAL_SIZE, "sum") + "원");
            $("#SUM_AMT").text(_fnGetNumber(TOTAL_SIZE, "sum") + "원");
            $("#QUOT_PER").addClass("dash-progress__bar");
            $("#SEA_EX_PER").addClass("dash-progress__bar");
            $("#SEA_IM_PER").addClass("dash-progress__bar");
            $("#AIR_EX_PER").addClass("dash-progress__bar");
            $("#AIR_IM_PER").addClass("dash-progress__bar");
            $("#QUOT_PER").addClass("per" + QUOT_AVG);
            $("#SEA_EX_PER").addClass("per" + SEA_EX_AVG);
            $("#SEA_IM_PER").addClass("per" + SEA_IM_AVG);
            $("#AIR_EX_PER").addClass("per" + AIR_EX_AVG);
            $("#AIR_IM_PER").addClass("per" + AIR_IM_AVG);

            $("#QUOT_SIZE")[0].innerHTML = "<span class='use-progress_value' id='QUOT_SIZE'>" + _fnGetNumber(QUOT_CNT, "sum") + "건</span>";
            $("#SEA_EX_SIZE")[0].innerHTML = "<span class='use-progress_value' id='EX_SIZE'>" + _fnGetNumber(SEA_EX_CNT, "sum") + "건</span>";
            $("#SEA_IM_SIZE")[0].innerHTML = "<span class='use-progress_value' id='IM_SIZE'>" + _fnGetNumber(SEA_IM_CNT, "sum") + "건</span>";
            $("#AIR_EX_SIZE")[0].innerHTML = "<span class='use-progress_value' id='EX_SIZE'>" + _fnGetNumber(AIR_EX_CNT, "sum") + "건</span>";
            $("#AIR_IM_SIZE")[0].innerHTML = "<span class='use-progress_value' id='IM_SIZE'>" + _fnGetNumber(AIR_IM_CNT, "sum") + "건</span>";
            var doc_title = new Array();
            var chk = false;
            if (_fnToNull($("#Session_DOCU").val()) == "Y") {
                $("#CHK_DOCU").show();
                doc_title.push('서식');
                tot_dt.push(DOCU_AVG);
            }

            if (_fnToNull($("#Session_QUOT").val()) == "Y") {
                $("#CHK_QUOT").show();
                doc_title.push('견적');
                tot_dt.push(QUOT_AVG);
            }

            if (_fnToNull($("#Session_EXPORT").val()) == "Y") {
                $("#CHK_EX").show();
                doc_title.push('수출');
                tot_dt.push(EX_AVG);
            }

            if (_fnToNull($("#Session_IMPORT").val()) == "Y") {
                $("#CHK_IM").show();
                doc_title.push('수입');
                tot_dt.push(IM_AVG);
            }

            if (_fnToNull($("#Session_SEA_EX").val()) == "Y") {
                $("#CHK_SEA_EX").show();
                doc_title.push('해운수출');
                tot_dt.push(SEA_EX_AVG);
            }

            if (_fnToNull($("#Session_SEA_IM").val()) == "Y") {
                $("#CHK_SEA_IM").show();
                doc_title.push('해운수입');
                tot_dt.push(SEA_IM_AVG);
            }

            if (_fnToNull($("#Session_AIR_EX").val()) == "Y") {
                $("#CHK_AIR_EX").show();
                doc_title.push('항공수출');
                tot_dt.push(AIR_EX_AVG);
            }

            if (_fnToNull($("#Session_AIR_IM").val()) == "Y") {
                $("#CHK_AIR_IM").show();
                doc_title.push('항공수입');
                tot_dt.push(AIR_IM_AVG);
            }

        }
        console.log(tot_dt);
        console.log(doc_title);
        var options = {
            chart: {
                type: 'donut',
                width: 500,
                height: 500,
            },
            series: tot_dt,
            labels: doc_title,
            colors: ['#434e9b', '#6c5ea2', '#aa6685', '#c6817a'],
            legend: {
                markers: {
                    width: 10,
                    height: 10,
                    shape: 'square',
                }
            },
            responsive: [{
                breakpoint: 769,
                options: {
                    chart: {
                        width: 300,
                        height: 300
                    },
                    legend: {
                        show: false
                    }
                },
            }]
        }

        var chart = new ApexCharts(document.querySelector("#UseGraph"), options);
        chart.render();

        $('.dash-progress__bar').each(function () {
            // 클래스 이름에서 퍼센트를 추출합니다.
            var className = $(this).attr('class');
            console.log(className);
            var percentage = className.match(/per(\d+)/)[1];

            // 너비를 0으로 초기화합니다.
            $(this).css('width', '0');

            // 해당 퍼센트로 너비를 부드럽게 변경합니다.
            $(this).animate({
                width: percentage + "%"
            }, 900);
        });
    }
}



$(document).ready(function () {
   
});
$(document).on('click', '.change_appli', function () {
    location.href = '/Main/Mypage';
});