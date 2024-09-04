//#region ★★★★★★전역변수★★★★★
const reco_class = ['ship', 'air'];
const reco_exim_class = ["export", "import"];
var myChart; 
//#endregion

////////////////////jquery event///////////////////////
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }
    else { // 세션 있고 유저 타입별로 체크

        //#region 좌측 매뉴텝 event
        $(".sub_info").addClass("on");
        $(".sub_info .sub_depth").addClass("on");
        $(".sub_info .sub_depth li:nth-child(1) a").addClass("on");

        //#endregion

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

        if (_fnToNull($("#Session_USR_TYPE").val()) == "S") { //화주
            $("#ShipperNm").text(_fnToNull($("#Session_USR_NM")).val()); //로그인자 명 바인딩

            myChart = new Chart(document.getElementById('DoughnutShipper'));
            fnSearchData();
            fnSearchRecommend();
        }
        else { // 실행사 'F'

            myChart = new Chart(document.getElementById('DoughnutExcution'));
            fnSearchData();
            fnSearchFQuot();
        }

    }

    
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

//#region ★★★★★★기본 조회 기능 ★★★★★
//달력 왼쪽 클릭
$(document).on("click", "#btn_cal_left", function () {
    $("#cal_date").text(fnSetYearMonth("L"));
    //if (_fnToNull($("#Session_USR_TYPE").val()) == "S") {
        fnSearchData();
    //}
    
});

//달력 오른쪽 클릭
$(document).on("click", "#btn_cal_right", function () {
    $("#cal_date").text(fnSetYearMonth("R"));
    //if (_fnToNull($("#Session_USR_TYPE").val()) == "S") {
        fnSearchData();
    //}
    
});

//#endregion




function fnSearchData() {

    var Surl = "";
    if (_fnToNull($("#Session_USR_TYPE").val()) == "S") {
        Surl = "/Info/fnGetDashBoardInfo";
    }
    else {
        Surl = "/Info/fnGetFDashBoardInfo";
    }

    var objJsonData = new Object();

    objJsonData.CRN = _fnToNull($("#Session_CRN").val());
    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
    objJsonData.DATE_YYYY = $("#cal_date").text().split(".")[0];
    objJsonData.DATE_MM = $("#cal_date").text().split(".")[1];


    $.ajax({
        type: "POST",
        //url: "/Info/fnGetDashBoardInfo",
        url: Surl,
        async: true,
        dataType: "json",
        //data: callObj,
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            $("#no_data").hide();
            if (_fnToNull($("#Session_USR_TYPE").val()) == "S") {
                fnMakeList(result);
            }
            else {
                fnMakeFList(result);
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

//#region 화주용 EVENT

//견적하기
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



//#endregion


//#region ★★★★★화주용 함수 ★★★★★

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
                var vList = JSON.parse(result).Table1;

                vHtml = fnMakeRecoList(vList);

                $("#eximList").append(vHtml);
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

//#endregion


//#region ★★★★★실행사 함수 ★★★★★

function fnSearchFQuot() {
    var vHtml = "";
    var objJsonData = new Object();

    objJsonData.CRN = _fnToNull($("#Session_CRN").val());
    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());

    $.ajax({
        type: "POST",
        url: "/Info/fnGetFQuotList",
        async: true,
        dataType: "json",
        //data: callObj,
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                var vList = JSON.parse(result).Table1;

                vHtml = fnMakeFQuotList(vList);

                $("#eximList").append(vHtml);
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

//#endregion






//// 대시보드(실행사) 도넛 그래프
//var ctx = document.getElementById('DoughnutExcution');
//var myChart = new Chart(ctx, {
//    type: 'doughnut',
//    data: {
//        labels: [
//            '해운수출',
//            '해운수입',
//            '항공수출',
//            '항공수입',
//            '견적진행'
//        ],
//        datasets: [{
//            data: [40, 20, 10, 10, 20],
//            backgroundColor: [
//                '#434e9b',
//                '#6c5ea2',
//                '#aa6685',
//                '#c6817a',
//                '#b8c1d6'
//            ]
//        }]
//    },
//    options: {
//        plugins: {
//            legend: {
//                display: false
//            },
//            tooltip: {
//                callbacks: {
//                    label: function (context) {
//                        var index = context.dataIndex;
//                        var dataset = context.dataset;
//                        var total = dataset.data.reduce(function (previousValue, currentValue, currentIndex, array) {
//                            return previousValue + currentValue;
//                        });
//                        var currentValue = dataset.data[index];
//                        var percentage = Math.floor(((currentValue / total) * 100) + 0.5);
//                        return percentage + "%";
//                    }
//                }
//            }
//        },
//        responsive: false,
//        scales: {
//            yAxes: [{
//                ticks: {
//                    beginAtZero: true
//                }
//            }]
//        },
//    }
//});


//#region ★★★★★★ 그리기 함수 ★★★★★
    //#region ★★화주용★★

//#region 그래프 그리기
//
function fnMakeList(vJsonData) {
    try {
        var vHTML = "";
        
        
        //$("#dashBar").empty();
        //$("#testdonut")[0].innerHTML = "";

        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            myChart.destroy();
            vResult = JSON.parse(vJsonData).Table1;

            var IM_TOT = (parseInt(vResult[0]["SEA_IM_TOT"]) + parseInt(vResult[0]["AIR_IM_TOT"]));
            var IM_COM = (parseInt(vResult[0]["SEA_IM_COM"]) + parseInt(vResult[0]["AIR_IM_COM"]));

            var EX_TOT = (parseInt(vResult[0]["SEA_EX_TOT"]) + parseInt(vResult[0]["AIR_EX_TOT"]));
            var EX_COM = (parseInt(vResult[0]["SEA_EX_COM"]) + parseInt(vResult[0]["AIR_EX_COM"]));

            var IM_AVG = _fnToZero(Math.round((IM_COM / IM_TOT) * 100));
            var EX_AVG = _fnToZero(Math.round((EX_COM/EX_TOT) * 100));
            

            var OCR_AVG = _fnToZero(Math.round((parseInt(vResult[0]["OCR_COM"]) / parseInt(vResult[0]["OCR_TOT"])) * 100));
            var QUOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["QUOT_COM"]) / parseInt(vResult[0]["QUOT_TOT"])) * 100));

            var ALL_TOT = _fnToZero(parseInt(vResult[0]["QUOT_COM"]) + parseInt(vResult[0]["OCR_COM"]) + IM_COM + EX_COM);
            var TOT_EX_AVG = _fnToZero(((EX_COM / ALL_TOT) * 100).toFixed(2));
            var TOT_IM_AVG = _fnToZero(((IM_COM / ALL_TOT) * 100).toFixed(2));
            var TOT_OCR_AVG = _fnToZero(((parseInt(vResult[0]["OCR_COM"]) / ALL_TOT) * 100).toFixed(2));
            var TOT_QUOT_AVG = _fnToZero(((parseInt(vResult[0]["QUOT_COM"]) / ALL_TOT) * 100).toFixed(2));

            //#region 하단 항목별 막대 그래프

            //서식
            $("#OCR_PER")[0].innerHTML = "<span class='dash-format'>" + _fnGetNumber(vResult[0]["OCR_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["OCR_TOT"], "sum") + "</span>건";
            $("#OCR_PROG")[0].innerHTML = "<div class='dash-progress__bar per" + OCR_AVG + "'></div>";
            $("#OCR_PROG_PER").text(OCR_AVG + "%");

            //견적
            $("#QUOT_PER")[0].innerHTML = "<span class='dash-estimate'>" + _fnGetNumber(vResult[0]["QUOT_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["QUOT_TOT"], "sum") + "</span>건";
            $("#QUOT_PROG")[0].innerHTML = "<div class='dash-progress__bar per" + QUOT_AVG + "'></div>";
            $("#QUOT_PROG_PER").text(QUOT_AVG + "%");

            $("#IM_PER")[0].innerHTML = "<span class='dash-estimate'>" + _fnGetNumber(IM_COM, "sum") + "</span> / <span>" + _fnGetNumber(IM_TOT, "sum") + "</span>건";
            $("#IM_PROG")[0].innerHTML = "<div class='dash-progress__bar per" + IM_AVG + "'></div>";
            $("#IM_PROG_PER").text(IM_AVG + "%");

            $("#EX_PER")[0].innerHTML = "<span class='dash-estimate'>" + _fnGetNumber(EX_COM, "sum") + "</span> / <span>" + _fnGetNumber(EX_TOT, "sum") + "</span>건";
            $("#EX_PROG")[0].innerHTML = "<div class='dash-progress__bar per" + EX_AVG + "'></div>";
            $("#EX_PROG_PER").text(EX_AVG + "%");

            //#endregion

            //#region 대시보드(화주) 도넛 그래프

            var ctx = document.getElementById('DoughnutShipper');
            myChart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: [
                        '서식',
                        '견적',
                        '수출',
                        '수입'
                    ],
                    datasets: [{
                        data: [TOT_OCR_AVG, TOT_QUOT_AVG, TOT_EX_AVG, TOT_IM_AVG], //기준값 설정 필요 ekkim
                        //data: [30,20,0.1,49.9], //기준값 설정 필요 ekkim
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
                                        var num = (parseFloat(previousValue) + parseFloat(currentValue)).toFixed(2);
                                        return num;
                                    });
                                    var currentValue = dataset.data[index];
                                    var percentage = ((currentValue / total) * 100).toFixed(2);
                                    return percentage + "%";
                                }
                            }
                        }
                    },
                    responsive: false,
                    scales: {
                        //yAxes: [{
                        //    ticks: {
                        //        beginAtZero: true
                        //    }
                        //}]
                    },
                }
            }
            );
            
             //#endregion
            
            //#region
            /*
            //전체 프로세스 퍼센트 구하기

            var TOT_PER = parseInt(vResult[0]["SEA_EX_TOT"]) + parseInt(vResult[0]["SEA_IM_COM"]) + parseInt(vResult[0]["AIR_EX_COM"]) + parseInt(vResult[0]["AIR_IM_COM"]) + parseInt(vResult[0]["QUOT_COM"]) + parseInt(vResult[0]["OCR_COM"]);

            var SEA_EX_TOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["SEA_EX_TOT"]) / TOT_PER) * 100));
            var SEA_IM_TOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["SEA_IM_TOT"]) / TOT_PER) * 100));
            var AIR_EX_TOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["AIR_EX_TOT"]) / TOT_PER) * 100));
            var AIR_IM_TOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["AIR_IM_TOT"]) / TOT_PER) * 100));
            var QUOT_TOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["QUOT_TOT"]) / TOT_PER) * 100));

            let maxSpeed = { exsea: SEA_EX_TOT_AVG, imsea: SEA_IM_TOT_AVG, exair: AIR_EX_TOT_AVG, imair: AIR_IM_TOT_AVG, estimate: QUOT_TOT_AVG };
            let sortable = [];
            for (var vehicle in maxSpeed) {
                sortable.push([vehicle, maxSpeed[vehicle]]);
            }
            sortable.sort(function (a, b) {
                return a[1] - b[1];
            });

            sortable.reverse();
            var apdStr = "";

            if (sortable.length > 0) {
                for (var i = 0; i < sortable.length; i++) {
                    if (i == 0) {
                        apdStr = "<div class='dash-bar__line bar-" + sortable[i][0] + "'>";
                        apdStr += " <div class='dash-bar__title'>";
                        if (sortable[i][0] == "estimate") {
                            apdStr += "견적 진행";
                        } else if (sortable[i][0] == "exsea") {
                            apdStr += "해운 수출";
                        } else if (sortable[i][0] == "imsea") {
                            apdStr += "해운 수입";
                        } else if (sortable[i][0] == "exair") {
                            apdStr += "항공 수출";
                        } else if (sortable[i][0] == "imair") {
                            apdStr += "항공 수입";
                        }
                        apdStr += "</div > ";
                        apdStr += "     <div class='dash-progress'>";
                        apdStr += "     <div class='dash-progress__bar per100'></div><span>100%</span>";
                        apdStr += "     </div>";
                        apdStr += "</div>";
                    } else {
                        var perCal = Math.round((100 / sortable[0][1]) * sortable[i][1]);
                        apdStr += "<div class='dash-bar__line bar-" + sortable[i][0] + "'>";
                        apdStr += " <div class='dash-bar__title'>";
                        if (sortable[i][0] == "estimate") {
                            apdStr += "견적 진행";
                        } else if (sortable[i][0] == "exsea") {
                            apdStr += "해운 수출";
                        } else if (sortable[i][0] == "imsea") {
                            apdStr += "해운 수입";
                        } else if (sortable[i][0] == "exair") {
                            apdStr += "항공 수출";
                        } else if (sortable[i][0] == "imair") {
                            apdStr += "항공 수입";
                        }
                        apdStr += "</div > ";
                        apdStr += "     <div class='dash-progress'>";
                        apdStr += "     <div class='dash-progress__bar per" + perCal + "'></div><span>" + perCal + "%</span>";
                        apdStr += "     </div>";
                        apdStr += "</div>";
                    }
                }

            } else {
                apdStr = "		<div class='dash-bar__line bar-estimate'>	";
                apdStr += "            <div class='dash-bar__title'>견적 진행</div>	";
                apdStr += "            <div class='dash-progress'>	";
                apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
                apdStr += "            </div>	";
                apdStr += "        </div>	";
                apdStr += "        <div class='dash-bar__line bar-exsea'>	";
                apdStr += "            <div class='dash-bar__title'>해운 수출</div>	";
                apdStr += "            <div class='dash-progress'>	";
                apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
                apdStr += "            </div>	";
                apdStr += "        </div>	";
                apdStr += "        <div class='dash-bar__line bar-imsea'>	";
                apdStr += "            <div class='dash-bar__title'>해운 수입</div>	";
                apdStr += "            <div class='dash-progress'>	";
                apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
                apdStr += "            </div>	";
                apdStr += "        </div>	";
                apdStr += "        <div class='dash-bar__line bar-exair'>	";
                apdStr += "            <div class='dash-bar__title'>항공 수출</div>	";
                apdStr += "            <div class='dash-progress'>	";
                apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
                apdStr += "            </div>	";
                apdStr += "        </div>	";
                apdStr += "        <div class='dash-bar__line bar-imair'>	";
                apdStr += "            <div class='dash-bar__title'>항공 수입</div>	";
                apdStr += "            <div class='dash-progress'>	";
                apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
                apdStr += "            </div>	";
                apdStr += "        </div>	";

            }
            $("#dashBar").append(apdStr);
            

            //$("#TOT_QUOT")[0].innerHTML = "<div class='dash-progress__bar per" + QUOT_TOT_AVG + "'></div><span>" + QUOT_TOT_AVG + "%</span>";
            //$("#TOT_SEA_EX")[0].innerHTML = "<div class='dash-progress__bar per" + SEA_EX_TOT_AVG + "'></div><span>" + SEA_EX_TOT_AVG + "%</span>";
            //$("#TOT_SEA_IM")[0].innerHTML = "<div class='dash-progress__bar per" + SEA_IM_TOT_AVG + "'></div><span>" + SEA_IM_TOT_AVG + "%</span>";
            //$("#TOT_AIR_EX")[0].innerHTML = "<div class='dash-progress__bar per" + AIR_EX_TOT_AVG + "'></div><span>" + AIR_EX_TOT_AVG + "%</span>";
            //$("#TOT_AIR_IM")[0].innerHTML = "<div class='dash-progress__bar per" + AIR_IM_TOT_AVG + "'></div><span>" + AIR_IM_TOT_AVG + "%</span>";
            */
            //#endregion
        } else {
            
            $("#QUOT_PER").text("0/0건");
            $("#QUOT_PROG")[0].innerHTML = "<div class='dash-progress__bar per0'></div>";
            $("#QUOT_PROG_PER").text("0%");
            $("#OCR_PER").text("0/0건");
            $("#OCR_PROG")[0].innerHTML = "<div class='dash-progress__bar per0'></div>";
            $("#OCR_PROG_PER").text("0%");


            $("#IM_PER").text("0/0건");
            $("#IM_PROG")[0].innerHTML = "<div class='dash-progress__bar per0'></div>";
            $("#IM_PROG_PER").text("0%");


            $("#EX_PER").text("0/0건");
            $("#EX_PROG")[0].innerHTML = "<div class='dash-progress__bar per0'></div>";
            $("#EX_PROG_PER").text("0%");




            /*
            apdStr = "		<div class='dash-bar__line bar-estimate'>	";
            apdStr += "            <div class='dash-bar__title'>견적 진행</div>	";
            apdStr += "            <div class='dash-progress'>	";
            apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
            apdStr += "            </div>	";
            apdStr += "        </div>	";
            apdStr += "        <div class='dash-bar__line bar-exsea'>	";
            apdStr += "            <div class='dash-bar__title'>해운 수출</div>	";
            apdStr += "            <div class='dash-progress'>	";
            apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
            apdStr += "            </div>	";
            apdStr += "        </div>	";
            apdStr += "        <div class='dash-bar__line bar-imsea'>	";
            apdStr += "            <div class='dash-bar__title'>해운 수입</div>	";
            apdStr += "            <div class='dash-progress'>	";
            apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
            apdStr += "            </div>	";
            apdStr += "        </div>	";
            apdStr += "        <div class='dash-bar__line bar-exair'>	";
            apdStr += "            <div class='dash-bar__title'>항공 수출</div>	";
            apdStr += "            <div class='dash-progress'>	";
            apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
            apdStr += "            </div>	";
            apdStr += "        </div>	";
            apdStr += "        <div class='dash-bar__line bar-imair'>	";
            apdStr += "            <div class='dash-bar__title'>항공 수입</div>	";
            apdStr += "            <div class='dash-progress'>	";
            apdStr += "            <div class='dash-progress__bar per0'></div><span>0%</span>";
            apdStr += "            </div>	";
            apdStr += "        </div>	";

            $("#dashBar").append(apdStr);
            */
        }
        
    }
    catch (e) {
        console.log(e.message);
    }
}
//#endregion


//#region 화주용 추천 화물 그리기
function fnMakeRecoList(vJsonData) {
    var vResult = vJsonData;
    var _vHtml = "";
    var sa_type = ""; // 해상 해운
    var ei_type = "";


    $("#eximList").empty();

    $.each(vResult, function (i) {

        sa_type = "";
        ei_type = "";


        if (_fnToNull(vResult[i]["REQ_SVC"]) == "SEA") {
            sa_type = reco_class[0].toString();
        }
        else {
            sa_type = reco_class[1].toString();
        }

        if (_fnToNull(vResult[i]["EX_IM_TYPE"]) == "E") {
            ei_type = reco_exim_class[0].toString();
        }
        else {
            ei_type = reco_exim_class[1].toString();
        }

        _vHtml += "<div class='exim-box " + sa_type + "'>";

        _vHtml += "     <div class='exim-icn'>";
        _vHtml += "         <img src='/Images/" + sa_type + "_" + ei_type + "_icn.png'>";
        _vHtml += "     </div>";

        _vHtml += "     <div class='exim-inner'>";

        _vHtml += "         <div class='exim-cont text-info home'>";

        _vHtml += "             <div class='exim-cont__info'>";
        _vHtml += "                 <div class='exim-cont__inner exim-flex'>";
        _vHtml += "                     <div class='exim-cont__title'>POL</div>";
        _vHtml += "                     <div class='esti-cont__desc pol'>";
        _vHtml += "                         <p>" + _fnToNull(vResult[i]["POL_NM"]) + "</p>";
        _vHtml += "                     </div>";
        _vHtml += "                 </div>";
        _vHtml += "                 <div class='exim-cont__inner exim-flex'>";
        _vHtml += "                     <div class='exim-cont__title'>POD</div>";
        _vHtml += "                     <div class='esti-cont__desc'>";
        _vHtml += "                         <p>" + _fnToNull(vResult[i]["POD_NM"]) + "</p>";
        _vHtml += "                     </div>";
        _vHtml += "                 </div>";
        _vHtml += "             </div>";

        _vHtml += "             <div class='exim-cont__info'>";
        _vHtml += "                 <div class='exim-cont__inner exim-flex'>";
        _vHtml += "                     <div class='exim-cont__title'>ITEM</div>";
        _vHtml += "                     <div class='esti-cont__desc'>";
        _vHtml += "                         <p>" + _fnToNull(vResult[i]["ITEM_NM"]) + "</p>";
        _vHtml += "                     </div>";
        _vHtml += "                 </div>";
        _vHtml += "             </div>";

        _vHtml += "         </div>";


        _vHtml += "         <div class='exim-cont doc-info'>";
        _vHtml += "             <button type='button' class='do_estimate btn_esti'>견적하기<img src='/Images/icn_estimate.png'></button>";
        _vHtml += "             <p style='display:none'>" + _fnToNull(vResult[i]["POL_CD"]) + "</p>";
        _vHtml += "             <p style='display:none'>" + _fnToNull(vResult[i]["POD_CD"]) + "</p>";
        _vHtml += "             <p style='display:none'>" + _fnToNull(vResult[i]["POL_NM"]) + "</p>";
        _vHtml += "             <p style='display:none'>" + _fnToNull(vResult[i]["POD_NM"]) + "</p>";
        _vHtml += "             <p style='display:none'>" + _fnToNull(vResult[i]["ITEM_NM"]) + "</p>";
        _vHtml += "             <p style='display:none'>" + _fnToNull(vResult[i]["REQ_SVC"]) + "</p>";
        _vHtml += "         </div>";

        _vHtml += "     </div>";


        _vHtml += "</div>";





    });

    return _vHtml;

}

//#endregion

    //#endregion

    //#region ★★실행사★★
//#region 상단 상태별 View 
$(".esti-total__status.total").on("click", function () {
    $(".exim-box.ing").show();
    $(".exim-box.done").show();
    $('.exim-box:last-child').show();
})
$(".esti-total__status.present").on("click", function () {
    $(".exim-box.ing").show();
    $(".exim-box.done").hide();
})
$(".esti-total__status.finished").on("click", function () {
    $(".exim-box.ing").hide();
    $(".exim-box.done").show();
})
        //실행사 견적 리스트 그리기
function fnMakeFQuotList(vJsonData) {
    var vResult = vJsonData;
    var _vHtml = "";



    $("#eximList").empty();
    var tot_cnt = 0;
    var ing_cnt = 0;
    var comp_cnt = 0;
    $("#TOT_CNT").text('');
    $("#PRE_CNT").text('');
    $("#COMP_CNT").text('');
    $.each(vResult, function (i) {
        var com_cnt = parseInt(_fnToZero(vResult[i]["SEA_CNT"])) + parseInt(_fnToZero(vResult[i]["AIR_CNT"]));
        

        _vHtml += "";
        if (com_cnt != 0) { // 견적 완료 건
            comp_cnt += 1;
            _vHtml += " <div class='exim-box done'>";
            _vHtml += "     <div class='exim-icn'>진행</br>완료</div>";
        }
        else { // 진행 건
            ing_cnt += 1;
            _vHtml += " <div class='exim-box ing'>";
            _vHtml += "     <div class='exim-icn'>진행중</div>";
        }

        _vHtml += "     <div class='exim-inner'>";


        //POL
        _vHtml += "         <div class='esti-cont__box inquiry'>";
        _vHtml += "             <div class='esti-cont__flex'>";
        _vHtml += "                 <div class='esti-cont__desc'><p>" + _fnToNull(vResult[i]["POL_NM"]) + "</p></div>";
        _vHtml += "                 <div class='esti-cont__desc2'>";
        if (_fnToNull(vResult[i]["ETD"]) != "") {
            _vHtml += "                     <p>" + String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/gi, '$1.$2.$3') + "(" + _fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETD"]))+")</p>";
        }
        else {
            _vHtml += "                     <p style='visibility: hidden'>-</p>";
        }
        
        _vHtml += "                 </div>";
        _vHtml += "             </div>";
        _vHtml += "         </div>";

        //화살표
        _vHtml += "         <div class='esti-cont__box inquiry'>";
        _vHtml += "             <div class='esti-cont__inner'><p class='esti-cont__progress inquiry '><img src='/Images/icn_progress2.png'></p></div>";
        _vHtml += "         </div>";

        //POD
        _vHtml += "         <div class='esti-cont__box inquiry'>";
        _vHtml += "             <div class='esti-cont__flex'>";
        _vHtml += "                 <div class='esti-cont__desc'><p>" + _fnToNull(vResult[i]["POD_NM"]) + "</p></div>";
        _vHtml += "                 <div class='esti-cont__desc2'>";
        if (_fnToNull(vResult[i]["ETA"]) != "") {
            _vHtml += "                     <p>" + String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/gi, '$1.$2.$3') + "(" + _fnGetWhatDay_Eng(_fnToNull(vResult[i]["ETA"])) + ")</p>";
        }
        else {
            _vHtml += "                     <p style='visibility: hidden'>-</p>";
        }

        _vHtml += "                 </div>";
        _vHtml += "             </div>";
        _vHtml += "         </div>";

        //세부정보
        _vHtml += "         <div class='esti-cont__box inquiry'>";
        _vHtml += "         <div class='esti-cont__inner inner-flex'>";

        _vHtml += "             <div class='esti-cont__date flex-column'>";
        _vHtml += "                 <div class='esti-cont__date_start'>";
        _vHtml += "                     <div class='esti-cont__date_title'>견적요청일</div>";
        _vHtml += "                     <div class='esti-cont__date_cont'>" + String(_fnToNull(vResult[i]["REQ_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/gi,'$1.$2.$3')+"</div>";
        _vHtml += "                 </div>";

        _vHtml += "                 <div class='esti-cont__date_end'>";
        _vHtml += "                     <div class='esti-cont__date_title'>최종견적일</div>";
        _vHtml += "                     <div class='esti-cont__date_cont'>" + String(_fnToNull(vResult[i]["QUOT_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/gi, '$1.$2.$3') + "</div>";
        _vHtml += "                 </div>";
        _vHtml += "             </div>";

        _vHtml += "             <div class='esti-cont__etc'>";

        _vHtml += "                 <div class='esti-cont__etc_item'>";
        _vHtml += "                     <span class='esti-cont__etc_title'>품목명</span>";
        _vHtml += "                     <span class='esti-cont__etc_cont'>"+_fnToNull(vResult[i]["ITEM_NM"])+"</span>";
        _vHtml += "                 </div>";

        _vHtml += "                 <div class='esti-cont__date_file'>";
        _vHtml += "                     <span class='esti-cont__etc_title'>문서</span>";
        var ci_class = "";
        var pl_class = "";
        _vHtml += "                     <span class='esti-cont__etc_cont'>";
        if (_fnToNull(vResult[i]["CI_CNT"]) != "0") { //ci 있으면
            ci_class = "doc_type1";
            pl_class = "doc_type2";
            _vHtml += "                     <span class='" + ci_class + "'><img src='/Images/icn_doc02.png'>C/I</span>";
            if (_fnToNull(vResult[i]["PL_CNT"]) != "0") { //pl 있으면
                _vHtml += "                     <span class='" + pl_class + "'><img src='/Images/icn_doc02.png'>P/L</span>";
            }
        }
        else {
            pl_class = "doc_type1";
            if (_fnToNull(vResult[i]["PL_CNT"]) != "0") { //pl 있으면
                _vHtml += "                     <span class='" + pl_class + "'><img src='/Images/icn_doc02.png'>P/L</span>";
            }
            else {
                _vHtml += "                     <span class='" + pl_class + "' style='visibility:hidden;'><img src='/Images/icn_doc02.png'>P/L</span>";
            }

        }
        _vHtml += "                     </span>";
        _vHtml += "                 </div>";
        _vHtml += "             </div>";

        _vHtml += "         </div>";
        _vHtml += "         </div>";


        //#region 현황 표기 아이콘
        _vHtml += "         <div class='esti-cont__box inquiry'>";
        _vHtml += "             <div class='esti-cont-flex'>";
        if (com_cnt != 0) { // 견적 완료 건
            _vHtml += "             <div class='esti-cont__transit ship'>";
            _vHtml += "                 <button type='button' class='btns esti-cont__circle seaQuot'><img src='/Images/icn_ship.png'><p>"+_fnToNull(vResult[i]["SEA_CNT"])+"건</p></button>";
            _vHtml += "             </div>";
            _vHtml += "             <div class='esti-cont__transit air'>";
            _vHtml += "                 <button type='button' class='btns esti-cont__circle airQuot'><img src='/Images/icn_air.png'><p>" + _fnToNull(vResult[i]["AIR_CNT"])+"건</p></button>";
            _vHtml += "             </div>";
        }
        else { // 진행건 
            _vHtml += "             <div class='esti-cont__transit ship'>";
            _vHtml += "                 <button type='button' class='btns esti-cont__circle seaQuot'><img src='/Images/icn_ship.png'><p>진행중</p></button>";
            _vHtml += "             </div>";
            _vHtml += "             <div class='esti-cont__transit air'>";
            _vHtml += "                 <button type='button' class='btns esti-cont__circle airQuot'><img src='/Images/icn_air.png'><p>진행중</p></button>";
            _vHtml += "             </div>";
        }
        _vHtml += "             </div>";
        _vHtml += "         </div>";
        //#endregion

        _vHtml += "     </div>";
        
        _vHtml += " </div>";

    });
    tot_cnt = comp_cnt + ing_cnt;
    $("#TOT_CNT").text(tot_cnt);
    $("#PRE_CNT").text(ing_cnt);
    $("#COMP_CNT").text(comp_cnt);

    return _vHtml;
}

        //실행사 그래프 그리기
function fnMakeFList(vJsonData) {
    try {
        var vHtml = "";
        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") { // 자료 있을 때 
            //#region 하단 막대그래프
            myChart.destroy();
            var vResult = JSON.parse(vJsonData).Table1;

            var SEA_EX_AVG = _fnToZero(Math.round((parseInt(vResult[0]["SEA_EX_COM"]) / parseInt(vResult[0]["SEA_EX_TOT"])) * 100));
            var SEA_IM_AVG = _fnToZero(Math.round((parseInt(vResult[0]["SEA_IM_COM"]) / parseInt(vResult[0]["SEA_IM_TOT"])) * 100));

            var AIR_EX_AVG = _fnToZero(Math.round((parseInt(vResult[0]["AIR_EX_COM"]) / parseInt(vResult[0]["AIR_EX_TOT"])) * 100));
            var AIR_IM_AVG = _fnToZero(Math.round((parseInt(vResult[0]["AIR_IM_COM"]) / parseInt(vResult[0]["AIR_IM_TOT"])) * 100));

            var QUOT_AVG = _fnToZero(Math.round((parseInt(vResult[0]["QUOT_COM"]) / parseInt(vResult[0]["QUOT_TOT"])) * 100));

            //그래프용 퍼센티지
            var ALL_TOT = _fnToZero(parseInt(vResult[0]["QUOT_COM"]) + parseInt(vResult[0]["SEA_EX_COM"]) + parseInt(vResult[0]["SEA_IM_COM"]) + parseInt(vResult[0]["AIR_EX_COM"]) + parseInt(vResult[0]["AIR_IM_COM"]));

            var TOT_SEA_EX_AVG = _fnToZero((parseInt(vResult[0]["SEA_EX_COM"]) / ALL_TOT * 100).toFixed(2));
            var TOT_SEA_IM_AVG = _fnToZero((parseInt(vResult[0]["SEA_IM_COM"]) / ALL_TOT * 100).toFixed(2));
            var TOT_AIR_EX_AVG = _fnToZero((parseInt(vResult[0]["AIR_EX_COM"]) / ALL_TOT * 100).toFixed(2));
            var TOT__AIR_IM_AVG = _fnToZero((parseInt(vResult[0]["AIR_IM_COM"]) / ALL_TOT * 100).toFixed(2));
            var TOT_QUOT_AVG = _fnToZero((parseInt(vResult[0]["QUOT_COM"]) / ALL_TOT * 100).toFixed(2));



            $("#SEA_EX_PER")[0].innerHTML = "<span class='dash-format'>" + _fnGetNumber(vResult[0]["SEA_EX_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["SEA_EX_TOT"], "sum") + "</span>건";
            $("#SEA_EX_PROG")[0].innerHTML = "<div class='dash-progress__bar per" + SEA_EX_AVG + "'></div>";
            $("#SEA_EX_PROG_PER").text(SEA_EX_AVG + "%");

            $("#SEA_IM_PER")[0].innerHTML = "<span class='dash-format'>" + _fnGetNumber(vResult[0]["SEA_IM_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["SEA_IM_TOT"], "sum") + "</span>건";
            $("#SEA_IM_PROG")[0].innerHTML = "<div class='dash-progress__bar per" + SEA_IM_AVG + "'></div>";
            $("#SEA_IM_PROG_PER").text(SEA_IM_AVG + "%");

            $("#AIR_EX_PER")[0].innerHTML = "<span class='dash-format'>" + _fnGetNumber(vResult[0]["AIR_EX_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["AIR_EX_TOT"], "sum") + "</span>건";
            $("#AIR_EX_PROG")[0].innerHTML = "<div class='dash-progress__bar per" + AIR_EX_AVG + "'></div>";
            $("#AIR_EX_PROG_PER").text(AIR_EX_AVG + "%");
                
            $("#AIR_IM_PER")[0].innerHTML = "<span class='dash-format'>" + _fnGetNumber(vResult[0]["AIR_IM_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["AIR_IM_TOT"], "sum") + "</span>건";
            $("#AIR_IM_PROG")[0].innerHTML = "<div class='dash-progress__bar per" + AIR_IM_AVG + "'></div>";
            $("#AIR_IM_PROG_PER").text(AIR_IM_AVG + "%");

            $("#QUOT_PER")[0].innerHTML = "<span class='dash-format'>" + _fnGetNumber(vResult[0]["QUOT_COM"], "sum") + "</span> / <span>" + _fnGetNumber(vResult[0]["QUOT_TOT"], "sum") + "</span>건";
            $("#QUOT_PROG")[0].innerHTML = "<div class='dash-progress__bar per" + QUOT_AVG + "'></div>";
            $("#QUOT_PROG_PER").text(QUOT_AVG + "%");


            //#endregion

            //#region 실행사 도넛 그래프

            var ctx = document.getElementById('DoughnutExcution');
            myChart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: [
                        '해운수출',
                        '해운수입',
                        '항공수출',
                        '항공수입',
                        '견적진행'
                    ],
                    datasets: [{
                        data: [parseFloat(TOT_SEA_EX_AVG), parseFloat(TOT_SEA_IM_AVG), parseFloat(TOT_AIR_EX_AVG), parseFloat(TOT__AIR_IM_AVG), parseFloat(TOT_QUOT_AVG)], //기준값 설정 필요 ekkim
                        //data: [40, 20, 10, 0, 0],
                        backgroundColor: [
                            '#434e9b',
                            '#6c5ea2',
                            '#aa6685',
                            '#c6817a',
                            '#b8c1d6'
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
                                    //                                var percentage = Math.floor(((currentValue / total) * 100) + 0.5);
                                    var percentage = ((currentValue / total) * 100).toFixed(2);
                                    return percentage + "%";
                                }
                            }
                        }
                    },
                    responsive: false,
                    scales: {
                        //yAxes: [{
                        //    ticks: {
                        //        beginAtZero: true
                        //    }
                        //}]
                    },
                }
            });
        //#endregion

        }
        else { //자료 없을 때 
            myChart.destroy();
            $("#SEA_EX_PER").text("0/0건");
            $("#SEA_EX_PROG")[0].innerHTML = "<div class='dash-progress__bar per0'></div>";
            $("#SEA_EX_PROG_PER").text("0%");
            $("#SEA_IM_PER").text("0/0건");
            $("#SEA_IM_PROG")[0].innerHTML = "<div class='dash-progress__bar per0'></div>";
            $("#SEA_IM_PROG_PER").text("0%");


            $("#AIR_EX_PER").text("0/0건");
            $("#AIR_EX_PROG")[0].innerHTML = "<div class='dash-progress__bar per0'></div>";
            $("#AIR_EX_PROG_PER").text("0%");
            $("#AIR_IM_PER").text("0/0건");
            $("#AIR_IM_PROG")[0].innerHTML = "<div class='dash-progress__bar per0'></div>";
            $("#AIR_IM_PROG_PER").text("0%");


            $("#QUOT_PER").text("0/0건");
            $("#QUOT_PROG")[0].innerHTML = "<div class='dash-progress__bar per0'></div>";
            $("#QUOT_PROG_PER").text("0%");




        }


    }
    catch (e) {
        console.log(e.message);
    }
}

    //#endregion

//#endregion