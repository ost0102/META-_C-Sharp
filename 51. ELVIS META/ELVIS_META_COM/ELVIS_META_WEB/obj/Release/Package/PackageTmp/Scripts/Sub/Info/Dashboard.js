////////////////////전역 변수//////////////////////////

////////////////////jquery event///////////////////////
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    $(".sub_info").addClass("on");
    $(".sub_info .sub_depth").addClass("on");
    $(".sub_info .sub_depth li:nth-child(1) a").addClass("on");



    var swiper = new Swiper(".dash-swiper", {
        slidesPerView: 1,
        spaceBetween: 20,
        navigation: {
            nextEl: ".swiper-button-next",
            prevEl: ".swiper-button-prev"
        },
        breakpoints: {
            1024: {
                slidesPerView: 4,
            },
            768: {
                slidesPerView: 2
            }
        }
    });

    $("#cal_date").text(fnSetNowDate()); //현재 날짜 세팅
    $("#ProgressBar_Loading").show(); //프로그래스 바
    setTimeout(function () {
        $("#ProgressBar_Loading").hide(); //프로그래스 바
    }, 1000);
    fnSearchData();
    fnSearchRecommend();
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


    $.ajax({
        type: "POST",
        url: "/Info/fnGetDashBoardInfo",
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


function fnSearchRecommend() {
    var vHtml = "";
    var objJsonData = new Object();

    objJsonData.CRN = _fnToNull($("#Session_CRN").val());
    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());


    $.ajax({
        type: "POST",
        url: "/Info/fnGetRecommendQuot",
        async: true,
        dataType: "json",
        //data: callObj,
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                var vResult = JSON.parse(result).Table1;
                $.each(vResult, function (i) {
                    vHtml += "	<div class='swiper-slide'>	";
                    if (_fnToNull(vResult[i]["REQ_SVC"]) == "SEA") {
                        if (_fnToNull(vResult[i]["EX_IM_TYPE"]) == "E") {
                            vHtml += "        <div class='dash-swiper__card sea export'>";
                        } else {
                            vHtml += "        <div class='dash-swiper__card sea import'>";
                        }
                    } else {
                        if (_fnToNull(vResult[i]["EX_IM_TYPE"]) == "E") {
                            vHtml += "        <div class='dash-swiper__card air export'>";
                        } else {
                            vHtml += "        <div class='dash-swiper__card air import'>";
                        }
                    }
                    vHtml += "            <button type='button' class='btns swiper__btn btn_esti'>견적하기</button>";
                    vHtml += "            <p style='display:none'>" + _fnToNull(vResult[i]["POL_CD"]) + "</p>";
                    vHtml += "            <p style='display:none'>" + _fnToNull(vResult[i]["POD_CD"]) + "</p>";
                    vHtml += "            <p style='display:none'>" + _fnToNull(vResult[i]["POL_NM"]) + "</p>";
                    vHtml += "            <p style='display:none'>" + _fnToNull(vResult[i]["POD_NM"]) + "</p>";
                    vHtml += "            <p style='display:none'>" + _fnToNull(vResult[i]["ITEM_NM"]) + "</p>";
                    vHtml += "            <p style='display:none'>" + _fnToNull(vResult[i]["REQ_SVC"]) + "</p>";
                    vHtml += "            <div class='dash-swiper__inner'>";
                    vHtml += "                <div class='dash-swiper__cont'>";
                    vHtml += "                    <p class='dash-swiper__head2'>POL</p>";
                    vHtml += "                    <p class='dash-swiper__desc'>" + _fnToNull(vResult[i]["POL_NM"]) + "</p>";
                    vHtml += "                </div>";
                    vHtml += "                <div class='dash-swiper__cont'>";
                    vHtml += "                    <p class='dash-swiper__head2'>POD</p>";
                    vHtml += "                    <p class='dash-swiper__desc'>" + _fnToNull(vResult[i]["POD_NM"]) + "</p>";
                    vHtml += "                </div>";
                    vHtml += "                <div class='dash-swiper__cont'>";
                    vHtml += "                    <p class='dash-swiper__head2'>ITEM</p>";
                    vHtml += "                    <p class='dash-swiper__desc'>" + _fnToNull(vResult[i]["ITEM_NM"]) + "</p>";
                    vHtml += "                </div>";
                    vHtml += "            </div>";
                    vHtml += "        </div>";
                    vHtml += "    </div>";
                });

                $("#recommendQuot").append(vHtml);
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
//function fnMakeList(vJsonData) {
//    try {
//        var vHTML = "";

//        $("#dashBar").empty();
//        //$("#testdonut")[0].innerHTML = "";

//        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
//            vResult = JSON.parse(vJsonData).Table1;
//            var SEA_EX_AVG = _fnToZero(Math.round((parseInt(vResult[0]["SEA_EX_COM"]) / parseInt(vResult[0]["SEA_EX_TOT"])) * 100));
//            var SEA_IM_AVG = _fnToZero(Math.round((parseInt(vResult[0]["SEA_IM_COM"]) / parseInt(vResult[0]["SEA_IM_TOT"])) * 100));
//            var AIR_EX_AVG = _fnToZero(Math.round((parseInt(vResult[0]["AIR_EX_COM"]) / parseInt(vResult[0]["AIR_EX_TOT"])) * 100));
//            var AIR_IM_AVG = _fnToZero(Math.round((parseInt(vResult[0]["AIR_IM_COM"]) / parseInt(vResult[0]["AIR_IM_TOT"])) * 100));
//            var OCR_AVG = _fnToZero(Math.round((parseInt(vResult[0]["OCR_COM"]) / parseInt(vResult[0]["OCR_TOT"])) * 100));
//            var QUOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["QUOT_COM"]) / parseInt(vResult[0]["QUOT_TOT"])) * 100));

//            $("#QUOT_PER")[0].innerHTML = "<span class='dash-estimate'>" + _fnGetNumber(vResult[0]["QUOT_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["QUOT_TOT"], "sum") + "</span>건";
//            $("#QUOT_PROG")[0].innerHTML = "<div class='dash-progress__percent per" + QUOT_AVG + "'></div>";
//            $("#QUOT_PROG_PER").text(QUOT_AVG + "%");
//            $("#OCR_PER")[0].innerHTML = "<span class='dash-format'>" + _fnGetNumber(vResult[0]["OCR_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["OCR_TOT"], "sum") + "</span>건";
//            $("#OCR_PROG")[0].innerHTML = "<div class='dash-progress__percent per" + OCR_AVG + "'></div>";
//            $("#OCR_PROG_PER").text(OCR_AVG + "%");
//            $("#AIR_EX_PER")[0].innerHTML = "<span class='dash-exair'>" + _fnGetNumber(vResult[0]["AIR_EX_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["AIR_EX_TOT"], "sum") + "</span>건";
//            $("#AIR_EX_PROG")[0].innerHTML = "<div class='dash-progress__percent per" + AIR_EX_AVG + "'></div>";
//            $("#AIR_EX_PROG_PER").text(AIR_EX_AVG + "%");
//            $("#SEA_EX_PER")[0].innerHTML = "<span class='dash-exsea'>" + _fnGetNumber(vResult[0]["SEA_EX_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["SEA_EX_TOT"], "sum") + "</span>건";
//            $("#SEA_EX_PROG")[0].innerHTML = "<div class='dash-progress__percent per" + SEA_EX_AVG + "'></div>";
//            $("#SEA_EX_PROG_PER").text(SEA_EX_AVG + "%");
//            $("#AIR_IM_PER")[0].innerHTML = "<span class='dash-imair'>" + _fnGetNumber(vResult[0]["AIR_IM_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["AIR_IM_TOT"], "sum") + "</span>건";
//            $("#AIR_IM_PROG")[0].innerHTML = "<div class='dash-progress__percent per" + AIR_IM_AVG + "'></div>";
//            $("#AIR_IM_PROG_PER").text(AIR_IM_AVG + "%");
//            $("#SEA_IM_PER")[0].innerHTML = "<span class='dash-imsea'>" + _fnGetNumber(vResult[0]["SEA_IM_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["SEA_IM_TOT"], "sum") + "</span>건";
//            $("#SEA_IM_PROG")[0].innerHTML = "<div class='dash-progress__percent per" + SEA_IM_AVG + "'></div>";
//            $("#SEA_IM_PROG_PER").text(SEA_IM_AVG + "%");


//            //전체 프로세스 퍼센트 구하기

//            var TOT_PER = parseInt(vResult[0]["SEA_EX_TOT"]) + parseInt(vResult[0]["SEA_IM_COM"]) + parseInt(vResult[0]["AIR_EX_COM"]) + parseInt(vResult[0]["AIR_IM_COM"]) + parseInt(vResult[0]["QUOT_COM"]) + parseInt(vResult[0]["OCR_COM"]);

//            var SEA_EX_TOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["SEA_EX_TOT"]) / TOT_PER) * 100));
//            var SEA_IM_TOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["SEA_IM_TOT"]) / TOT_PER) * 100));
//            var AIR_EX_TOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["AIR_EX_TOT"]) / TOT_PER) * 100));
//            var AIR_IM_TOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["AIR_IM_TOT"]) / TOT_PER) * 100));
//            var QUOT_TOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["QUOT_TOT"]) / TOT_PER) * 100));

//            let maxSpeed = { exsea: SEA_EX_TOT_AVG, imsea: SEA_IM_TOT_AVG, exair: AIR_EX_TOT_AVG, imair: AIR_IM_TOT_AVG, estimate: QUOT_TOT_AVG };
//            let sortable = [];
//            for (var vehicle in maxSpeed) {
//                sortable.push([vehicle, maxSpeed[vehicle]]);
//            }
//            sortable.sort(function (a, b) {
//                return a[1] - b[1];
//            });

//            sortable.reverse();
//            var apdStr = "";

//            if (sortable.length > 0) {
//                for (var i = 0; i < sortable.length; i++) {
//                    if (i == 0) {
//                        apdStr = "<div class='dash-bar__line bar-" + sortable[i][0] + "'>";
//                        apdStr += " <div class='dash-bar__title'>";
//                        if (sortable[i][0] == "estimate") {
//                            apdStr += "견적 진행";
//                        } else if (sortable[i][0] == "exsea") {
//                            apdStr += "해운 수출";
//                        } else if (sortable[i][0] == "imsea") {
//                            apdStr += "해운 수입";
//                        } else if (sortable[i][0] == "exair") {
//                            apdStr += "항공 수출";
//                        } else if (sortable[i][0] == "imair") {
//                            apdStr += "항공 수입";
//                        }
//                        apdStr += "</div > ";
//                        apdStr += "     <div class='dash-progress'>";
//                        apdStr += "     <div class='dash-progress__bar per100'></div><span>100%</span>";
//                        apdStr += "     </div>";
//                        apdStr += "</div>";
//                    } else {
//                        var perCal = Math.round((100 / sortable[0][1]) * sortable[i][1]);
//                        apdStr += "<div class='dash-bar__line bar-" + sortable[i][0] + "'>";
//                        apdStr += " <div class='dash-bar__title'>";
//                        if (sortable[i][0] == "estimate") {
//                            apdStr += "견적 진행";
//                        } else if (sortable[i][0] == "exsea") {
//                            apdStr += "해운 수출";
//                        } else if (sortable[i][0] == "imsea") {
//                            apdStr += "해운 수입";
//                        } else if (sortable[i][0] == "exair") {
//                            apdStr += "항공 수출";
//                        } else if (sortable[i][0] == "imair") {
//                            apdStr += "항공 수입";
//                        }
//                        apdStr += "</div > ";
//                        apdStr += "     <div class='dash-progress'>";
//                        apdStr += "     <div class='dash-progress__bar per" + perCal + "'></div><span>" + perCal + "%</span>";
//                        apdStr += "     </div>";
//                        apdStr += "</div>";
//                    }
//                }

//            } else {
//                apdStr = "		<div class='dash-bar__line bar-estimate'>	";
//                apdStr += "            <div class='dash-bar__title'>견적 진행</div>	";
//                apdStr += "            <div class='dash-progress'>	";
//                apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
//                apdStr += "            </div>	";
//                apdStr += "        </div>	";
//                apdStr += "        <div class='dash-bar__line bar-exsea'>	";
//                apdStr += "            <div class='dash-bar__title'>해운 수출</div>	";
//                apdStr += "            <div class='dash-progress'>	";
//                apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
//                apdStr += "            </div>	";
//                apdStr += "        </div>	";
//                apdStr += "        <div class='dash-bar__line bar-imsea'>	";
//                apdStr += "            <div class='dash-bar__title'>해운 수입</div>	";
//                apdStr += "            <div class='dash-progress'>	";
//                apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
//                apdStr += "            </div>	";
//                apdStr += "        </div>	";
//                apdStr += "        <div class='dash-bar__line bar-exair'>	";
//                apdStr += "            <div class='dash-bar__title'>항공 수출</div>	";
//                apdStr += "            <div class='dash-progress'>	";
//                apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
//                apdStr += "            </div>	";
//                apdStr += "        </div>	";
//                apdStr += "        <div class='dash-bar__line bar-imair'>	";
//                apdStr += "            <div class='dash-bar__title'>항공 수입</div>	";
//                apdStr += "            <div class='dash-progress'>	";
//                apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
//                apdStr += "            </div>	";
//                apdStr += "        </div>	";

//            }
//            $("#dashBar").append(apdStr);


//            //$("#TOT_QUOT")[0].innerHTML = "<div class='dash-progress__bar per" + QUOT_TOT_AVG + "'></div><span>" + QUOT_TOT_AVG + "%</span>";
//            //$("#TOT_SEA_EX")[0].innerHTML = "<div class='dash-progress__bar per" + SEA_EX_TOT_AVG + "'></div><span>" + SEA_EX_TOT_AVG + "%</span>";
//            //$("#TOT_SEA_IM")[0].innerHTML = "<div class='dash-progress__bar per" + SEA_IM_TOT_AVG + "'></div><span>" + SEA_IM_TOT_AVG + "%</span>";
//            //$("#TOT_AIR_EX")[0].innerHTML = "<div class='dash-progress__bar per" + AIR_EX_TOT_AVG + "'></div><span>" + AIR_EX_TOT_AVG + "%</span>";
//            //$("#TOT_AIR_IM")[0].innerHTML = "<div class='dash-progress__bar per" + AIR_IM_TOT_AVG + "'></div><span>" + AIR_IM_TOT_AVG + "%</span>";

//        } else {
//            $("#QUOT_PER").text("0%");
//            $("#QUOT_PROG")[0].innerHTML = "<div class='dash-progress__percent per0'></div>";
//            $("#QUOT_PROG_PER").text("0%");
//            $("#OCR_PER").text("0%");
//            $("#OCR_PROG")[0].innerHTML = "<div class='dash-progress__percent per0'></div>";
//            $("#OCR_PROG_PER").text("0%");
//            $("#AIR_EX_PER").text("0%");
//            $("#AIR_EX_PROG")[0].innerHTML = "<div class='dash-progress__percent per0'></div>";
//            $("#AIR_EX_PROG_PER").text("0%");
//            $("#SEA_EX_PER").text("0%");
//            $("#SEA_EX_PROG")[0].innerHTML = "<div class='dash-progress__percent per0'></div>";
//            $("#SEA_EX_PROG_PER").text("0%");
//            $("#AIR_IM_PER").text("0%");
//            $("#AIR_IM_PROG")[0].innerHTML = "<div class='dash-progress__percent per0'></div>";
//            $("#AIR_IM_PROG_PER").text("0%");
//            $("#SEA_IM_PER").text("0%");
//            $("#SEA_IM_PROG")[0].innerHTML = "<div class='dash-progress__percent per0'></div>";
//            $("#SEA_IM_PROG_PER").text("0%");

//            apdStr = "		<div class='dash-bar__line bar-estimate'>	";
//            apdStr += "            <div class='dash-bar__title'>견적 진행</div>	";
//            apdStr += "            <div class='dash-progress'>	";
//            apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
//            apdStr += "            </div>	";
//            apdStr += "        </div>	";
//            apdStr += "        <div class='dash-bar__line bar-exsea'>	";
//            apdStr += "            <div class='dash-bar__title'>해운 수출</div>	";
//            apdStr += "            <div class='dash-progress'>	";
//            apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
//            apdStr += "            </div>	";
//            apdStr += "        </div>	";
//            apdStr += "        <div class='dash-bar__line bar-imsea'>	";
//            apdStr += "            <div class='dash-bar__title'>해운 수입</div>	";
//            apdStr += "            <div class='dash-progress'>	";
//            apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
//            apdStr += "            </div>	";
//            apdStr += "        </div>	";
//            apdStr += "        <div class='dash-bar__line bar-exair'>	";
//            apdStr += "            <div class='dash-bar__title'>항공 수출</div>	";
//            apdStr += "            <div class='dash-progress'>	";
//            apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
//            apdStr += "            </div>	";
//            apdStr += "        </div>	";
//            apdStr += "        <div class='dash-bar__line bar-imair'>	";
//            apdStr += "            <div class='dash-bar__title'>항공 수입</div>	";
//            apdStr += "            <div class='dash-progress'>	";
//            apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
//            apdStr += "            </div>	";
//            apdStr += "        </div>	";

//            $("#dashBar").append(apdStr);
           

//        }

//    } catch (e) {


//    }
//}


function fnMakeRecommendList(vJsonData) {

}

// 대시보드 도넛 그래프
var ctx = document.getElementById('Doughnut');
var myChart = new Chart(ctx, {
    type: 'doughnut',
    data: {
        labels: [
            '서식',
            '견적',
            '수출',
            '수입'
        ],
        datasets: [{
            data: [20, 34, 27, 19],
            backgroundColor: [
                '#434e9b',
                '#6c5ea2',
                '#aa6685',
                '#c6817a'
            ]
        }]
    },
    options: {
        plugins: {
            legend: {
                display: false
            },
            tooltip: {
                callbacks: {
                    label: function (context) {
                        var index = context.dataIndex;
                        var dataset = context.dataset;
                        var total = dataset.data.reduce(function (previousValue, currentValue, currentIndex, array) {
                            return previousValue + currentValue;
                        });
                        var currentValue = dataset.data[index];
                        var percentage = Math.floor(((currentValue / total) * 100) + 0.5);
                        return percentage + "%";
                    }
                }
            }
        },
        responsive: false,
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        },
    }
});

/////////////////////API///////////////////////////////////////