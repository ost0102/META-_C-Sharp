////////////////////전역 변수//////////////////////////

////////////////////jquery event///////////////////////
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    $(".sub_stat").addClass("on");
    $(".sub_stat .sub_depth").addClass("on");
    $(".sub_stat .sub_depth li:nth-child(3) a").addClass("on");

    searchChart1();
    searchChart2();
    searchChart3();
    searchChart4();
    searchChart5();
    searchChart6();

    $("#ProgressBar_Loading").show(); //프로그래스 바
    setTimeout(function () {
        $("#ProgressBar_Loading").hide(); //프로그래스 바
    }, 1000);

});

$("#CHART_TYPE1").change(function (e) {
    searchChart1();
});

$("#CHART_TYPE2").change(function (e) {
    searchChart2();
});

$("#CHART_TYPE3").change(function (e) {
    searchChart3();
});

$("#CHART_TYPE4").change(function (e) {
    searchChart4();
});

$("#CHART_TYPE5").change(function (e) {
    searchChart5();
});

$("#CHART_TYPE6").change(function (e) {
    searchChart6();
});


var objJsonData = new Object();
objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());

function searchChart1() {
    $("#analysisChart1")[0].innerHTML = "<div class='analysis-chart' id=\"analysisChart1\"></div>";
    objJsonData.ITEM_NM = _fnToNull($("#CHART_TYPE1").val().replace(/\//gi, '.'));

    $.ajax({
        type: "POST",
        url: "/Statistic/fnGetItemAnalyze",
        async: false,
        dataType: "json",
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (rtnVal) {
            if (JSON.parse(rtnVal).Result[0].trxCode == "Y") {
                var vResult = JSON.parse(rtnVal).Table1;

                var vTotal = 0;
                for (var i = 0; i < vResult.length; i++) {
                    vTotal += _fnToZero(vResult[i]["PERF_DATA"]);
                }

                var vChartData = [];
                var vPortData = [];
                var j = 0;
                if (vResult.length == 0) {
                    vChartData.push(0);
                } else {

                    for (var i = 0; i < vResult.length; i++) {
                        if (_fnToZero(vResult[j]["PERF_DATA"]) != 0 && (_fnToZero(vResult[j]["PERF_DATA"]) / vTotal) * 100 > 1) {
                            vChartData.push(Math.ceil(Number(vResult[j]["PERF_DATA"])));
                            vPortData.push(_fnToNull(vResult[j]["PORT_NM"]));
                            if ((j + 1) != vResult.length) {
                                j++;
                            }
                        } else {
                            if ((j + 1) != vResult.length) {
                                j++;
                            }
                        }
                    }
                }

                if (matchMedia("screen and (max-width: 1600px)").matches) {
                    var options1 = {
                        series: vChartData,
                        labels: vPortData,
                        chart: {
                            type: 'donut',
                            height: 300,
                            width: '100%',
                        },
                        colors: ['#3c4d9f', '#5153a2', '#715ea6', '#97649b', '#bf6687', '#d4777d', '#b6c2d8', '#838e9f', '#4f5d77', '#4f5d77'],
                        dataLabels: {
                            enabled: false,
                            dropShadow: {
                                enabled: false
                            },
                        },
                        plotOptions: {
                            pie: {
                                dataLabels: {
                                    offset: -10,
                                }
                            }
                        },
                        legend: {
                            position: 'bottom',
                            width: '100%',
                            height: 100
                        },
                        tooltip: { //호버 시 퍼센트 나타내기
                            enabled: true,
                            custom: function ({ series, seriesIndex, dataPointIndex, w }) {
                                let total = 0; for (let x of series) { total += x; } let selected = series[seriesIndex]
                                return w.config.labels[seriesIndex] + ":  " + (selected / total * 100).toFixed(2) + "%";
                            }
                        },
                        responsive: [
                            {
                                breakpoint: 1600,
                                options: {
                                    chart: {
                                        height: 300,
                                        width: '100%',
                                    },
                                    legend: {
                                        position: 'bottom',
                                        width: '100%',
                                        height: 100
                                    }
                                },
                            },
                            {
                                breakpoint: 1024,
                                options: {
                                    chart: {
                                        height: 350,
                                        width: '100%',
                                    },
                                    legend: {
                                        position: 'bottom',
                                        width: '100%',
                                        height: 100
                                    }
                                }
                            }
                        ]
                    };

                }
                else {

                    var options1 = {
                        series: vChartData,
                        labels: vPortData,
                        chart: {
                            type: 'donut',
                            height: '100%',
                        },
                        colors: ['#3c4d9f', '#5153a2', '#715ea6', '#97649b', '#bf6687', '#d4777d', '#b6c2d8', '#838e9f', '#4f5d77', '#4f5d77'],
                        dataLabels: {
                            enabled: false,
                            dropShadow: {
                                enabled: false
                            },
                        },
                        plotOptions: {
                            pie: {
                                dataLabels: {
                                    offset: -10,
                                }
                            }
                        },
                        legend: {
                            width: 160,
                            height: '100%'
                        },
                        tooltip: { //호버 시 퍼센트 나타내기
                            enabled: true,
                            custom: function ({ series, seriesIndex, dataPointIndex, w }) {
                                let total = 0; for (let x of series) { total += x; } let selected = series[seriesIndex]
                                return w.config.labels[seriesIndex] + ":  " + (selected / total * 100).toFixed(2) + "%";
                            }
                        },
                        responsive: [
                            {
                                breakpoint: 1600,
                                options: {
                                    chart: {
                                        height: 300,
                                        width: '100%',
                                    },
                                    legend: {
                                        position: 'bottom',
                                        width: '100%',
                                        height: 100
                                    }
                                },
                            },
                            {
                                breakpoint: 1024,
                                options: {
                                    chart: {
                                        height: 350,
                                        width: '100%',
                                    },
                                    legend: {
                                        position: 'bottom',
                                        width: '100%',
                                        height: 100
                                    }
                                }
                            }
                        ]
                    };
                }

                var chart1 = new ApexCharts(document.querySelector("#analysisChart1"), options1);
                chart1.render();

            }
        }, error: function (xhr) {
            console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
            console.log(xhr);
            return;
        }, beforeSend: function () {
            $("#ProgressBar_Loading").show(); //프로그래스 바 

        },
        complete: function () {
            $("#ProgressBar_Loading").hide(); //프로그래스 바 
        }
    });

    
}


function searchChart2() {
    $("#analysisChart2")[0].innerHTML = "<div class='analysis-chart' id=\"analysisChart2\"></div>";
    objJsonData.ITEM_NM = _fnToNull($("#CHART_TYPE2").val().replace(/\//gi, '.'));

    var rtnVal = _fnGetAjaxData("POST", "Statistic", "fnGetItemAnalyze", objJsonData);

    if (JSON.parse(rtnVal).Result[0].trxCode == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;

        var vTotal = 0;
        for (var i = 0; i < vResult.length; i++) {
            vTotal += _fnToZero(vResult[i]["PERF_DATA"]);
        }

        var vChartData = [];
        var vPortData = [];
        var j = 0;
        if (vResult.length == 0) {
            vChartData.push(0);
        } else {

            for (var i = 0; i < vResult.length; i++) {
                if (_fnToZero(vResult[j]["PERF_DATA"]) != 0 && (_fnToZero(vResult[j]["PERF_DATA"])/vTotal)*100>1) {
                    vChartData.push(Math.ceil(Number(vResult[j]["PERF_DATA"])));
                    vPortData.push(_fnToNull(vResult[j]["PORT_NM"]));
                    if ((j + 1) != vResult.length) {
                        j++;
                    }
                } else {
                    if ((j + 1) != vResult.length) {
                        j++;
                    }
                }
            }
        }

        if (matchMedia("screen and (max-width: 1600px)").matches) {
            var options1 = {
                series: vChartData,
                labels: vPortData,
                chart: {
                    type: 'donut',
                    height: 300,
                    width: '100%',
                },
                colors: ['#3c4d9f', '#5153a2', '#715ea6', '#97649b', '#bf6687', '#d4777d', '#b6c2d8', '#838e9f', '#4f5d77', '#4f5d77'],
                dataLabels: {
                    enabled:false,
                    dropShadow: {
                        enabled: false
                    },
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            offset: -5,
                        }
                    }
                },
                legend: {
                    position: 'bottom',
                    width: '100%',
                    height: 100
                },
                tooltip: { //호버 시 퍼센트 나타내기
                    enabled: true,
                    custom: function ({ series, seriesIndex, dataPointIndex, w }) {
                        let total = 0; for (let x of series) { total += x; } let selected = series[seriesIndex]
                        return w.config.labels[seriesIndex] + ":  " + (selected / total * 100).toFixed(2) + "%";
                    }
                },
                responsive: [
                    {
                        breakpoint: 1600,
                        options: {
                            chart: {
                                height: 300,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        },
                    },
                    {
                        breakpoint: 1024,
                        options: {
                            chart: {
                                height: 350,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        }
                    }
                ]
            };

        }
        else {

            var options1 = {
                series: vChartData,
                labels: vPortData,
                chart: {
                    type: 'donut',
                    height: '100%',
                },
                colors: ['#3c4d9f', '#5153a2', '#715ea6', '#97649b', '#bf6687', '#d4777d', '#b6c2d8', '#838e9f', '#4f5d77', '#4f5d77'],
                dataLabels: {
                    enabled:false,
                    dropShadow: {
                        enabled: false
                    },
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            offset: -5,
                        }
                    }
                },
                legend: {
                    width: 160,
                    height: '100%'
                },
                tooltip: { //호버 시 퍼센트 나타내기
                    enabled: true,
                    custom: function ({ series, seriesIndex, dataPointIndex, w }) {
                        let total = 0; for (let x of series) { total += x; } let selected = series[seriesIndex]
                        return w.config.labels[seriesIndex] + ":  " + (selected / total * 100).toFixed(2) + "%";
                    }
                },
                responsive: [
                    {
                        breakpoint: 1600,
                        options: {
                            chart: {
                                height: 300,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        },
                    },
                    {
                        breakpoint: 1024,
                        options: {
                            chart: {
                                height: 350,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        }
                    }
                ]
            };
        }

        var chart1 = new ApexCharts(document.querySelector("#analysisChart2"), options1);
        chart1.render();

    }
}


function searchChart3() {
    $("#analysisChart3")[0].innerHTML = "<div class='analysis-chart' id=\"analysisChart3\"></div>";
    objJsonData.ITEM_NM = _fnToNull($("#CHART_TYPE3").val().replace(/\//gi, '.'));

    var rtnVal = _fnGetAjaxData("POST", "Statistic", "fnGetItemAnalyze", objJsonData);

    if (JSON.parse(rtnVal).Result[0].trxCode == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;


        var vTotal = 0;
        for (var i = 0; i < vResult.length; i++) {
            vTotal += _fnToZero(vResult[i]["PERF_DATA"]);
        }
        var vChartData = [];
        var vPortData = [];
        var j = 0;
        if (vResult.length == 0) {
            vChartData.push(0);
        } else {

            for (var i = 0; i < vResult.length; i++) {
                if (_fnToZero(vResult[j]["PERF_DATA"]) != 0 && (_fnToZero(vResult[j]["PERF_DATA"]) / vTotal) * 100 > 1) {
                    vChartData.push(Math.ceil(Number(vResult[j]["PERF_DATA"])));
                    vPortData.push(_fnToNull(vResult[j]["PORT_NM"]));
                    if ((j + 1) != vResult.length) {
                        j++;
                    }
                } else {
                    if ((j + 1) != vResult.length) {
                        j++;
                    }
                }
            }
        }

        if (matchMedia("screen and (max-width: 1600px)").matches) {
            var options1 = {
                series: vChartData,
                labels: vPortData,
                chart: {
                    type: 'donut',
                    height: 300,
                    width: '100%',
                },
                colors: ['#3c4d9f', '#5153a2', '#715ea6', '#97649b', '#bf6687', '#d4777d', '#b6c2d8', '#838e9f', '#4f5d77', '#4f5d77'],
                dataLabels: {
                    enabled:false,
                    dropShadow: {
                        enabled: false
                    },
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            offset: -25,
                        }
                    }
                },
                legend: {
                    position: 'bottom',
                    width: '100%',
                    height: 100
                },
                tooltip: { //호버 시 퍼센트 나타내기
                    enabled: true,
                    custom: function ({ series, seriesIndex, dataPointIndex, w }) {
                        let total = 0; for (let x of series) { total += x; } let selected = series[seriesIndex]
                        return w.config.labels[seriesIndex] + ":  " + (selected / total * 100).toFixed(2) + "%";
                    }
                },
                responsive: [
                    {
                        breakpoint: 1600,
                        options: {
                            chart: {
                                height: 300,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        },
                    },
                    {
                        breakpoint: 1024,
                        options: {
                            chart: {
                                height: 350,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        }
                    }
                ]
            };

        }
        else {

            var options1 = {
                series: vChartData,
                labels: vPortData,
                chart: {
                    type: 'donut',
                    height: '100%',
                },
                colors: ['#3c4d9f', '#5153a2', '#715ea6', '#97649b', '#bf6687', '#d4777d', '#b6c2d8', '#838e9f', '#4f5d77', '#4f5d77'],
                dataLabels: {
                    enabled: false,
                    dropShadow: {
                        enabled: false
                    },
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            offset: -25,
                        }
                    }
                },
                legend: {
                    width: 160,
                    height: '100%'
                },
                tooltip: { //호버 시 퍼센트 나타내기
                    enabled: true,
                    custom: function ({ series, seriesIndex, dataPointIndex, w }) {
                        let total = 0; for (let x of series) { total += x; } let selected = series[seriesIndex]
                        return w.config.labels[seriesIndex] + ":  " + (selected / total * 100).toFixed(2) + "%";
                    }
                },
                responsive: [
                    {
                        breakpoint: 1600,
                        options: {
                            chart: {
                                height: 300,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        },
                    },
                    {
                        breakpoint: 1024,
                        options: {
                            chart: {
                                height: 350,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        }
                    }
                ]
            };
        }

        var chart1 = new ApexCharts(document.querySelector("#analysisChart3"), options1);
        chart1.render();

    }
}


function searchChart4() {
    $("#analysisChart4")[0].innerHTML = "<div class='analysis-chart' id=\"analysisChart4\"></div>";
    objJsonData.ITEM_NM = _fnToNull($("#CHART_TYPE4").val().replace(/\//gi, '.'));

    var rtnVal = _fnGetAjaxData("POST", "Statistic", "fnGetItemAnalyze", objJsonData);

    if (JSON.parse(rtnVal).Result[0].trxCode == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;
        var vTotal = 0;
        for (var i = 0; i < vResult.length; i++) {
            vTotal += _fnToZero(vResult[i]["PERF_DATA"]);
        }
        var vChartData = [];
        var vPortData = [];
        var j = 0;
        if (vResult.length == 0) {
            vChartData.push(0);
        } else {

            for (var i = 0; i < vResult.length; i++) {
                if (_fnToZero(vResult[j]["PERF_DATA"]) != 0 && (_fnToZero(vResult[j]["PERF_DATA"]) / vTotal) * 100 > 1) {
                    vChartData.push(Math.ceil(Number(vResult[j]["PERF_DATA"])));
                    vPortData.push(_fnToNull(vResult[j]["PORT_NM"]));
                    if ((j + 1) != vResult.length) {
                        j++;
                    }
                } else {
                    if ((j + 1) != vResult.length) {
                        j++;
                    }
                }
            }
        }

        if (matchMedia("screen and (max-width: 1600px)").matches) {
            var options1 = {
                series: vChartData,
                labels: vPortData,
                chart: {
                    type: 'donut',
                    height: 300,
                    width: '100%',
                },
                colors: ['#3c4d9f', '#5153a2', '#715ea6', '#97649b', '#bf6687', '#d4777d', '#b6c2d8', '#838e9f', '#4f5d77', '#4f5d77'],
                dataLabels: {
                    enabled:false,
                    dropShadow: {
                        enabled: false
                    },
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            offset: -25,
                        }
                    }
                },
                legend: {
                    position: 'bottom',
                    width: '100%',
                    height: 100
                },
                tooltip: { //호버 시 퍼센트 나타내기
                    enabled: true,
                    custom: function ({ series, seriesIndex, dataPointIndex, w }) {
                        let total = 0; for (let x of series) { total += x; } let selected = series[seriesIndex]
                        return w.config.labels[seriesIndex] + ":  " + (selected / total * 100).toFixed(2) + "%";
                    }
                },
                responsive: [
                    {
                        breakpoint: 1600,
                        options: {
                            chart: {
                                height: 300,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        },
                    },
                    {
                        breakpoint: 1024,
                        options: {
                            chart: {
                                height: 350,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        }
                    }
                ]
            };

        }
        else {

            var options1 = {
                series: vChartData,
                labels: vPortData,
                chart: {
                    type: 'donut',
                    height: '100%',
                },
                colors: ['#3c4d9f', '#5153a2', '#715ea6', '#97649b', '#bf6687', '#d4777d', '#b6c2d8', '#838e9f', '#4f5d77', '#4f5d77'],
                dataLabels: {
                    enabled:false,
                    dropShadow: {
                        enabled: false
                    },
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            offset: -25,
                        }
                    }
                },
                legend: {
                    width: 160,
                    height: '100%'
                },
                tooltip: { //호버 시 퍼센트 나타내기
                    enabled: true,
                    custom: function ({ series, seriesIndex, dataPointIndex, w }) {
                        let total = 0; for (let x of series) { total += x; } let selected = series[seriesIndex]
                        return w.config.labels[seriesIndex] + ":  " + (selected / total * 100).toFixed(2) + "%";
                    }
                },
                responsive: [
                    {
                        breakpoint: 1600,
                        options: {
                            chart: {
                                height: 300,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        },
                    },
                    {
                        breakpoint: 1024,
                        options: {
                            chart: {
                                height: 350,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        }
                    }
                ]
            };
        }

        var chart1 = new ApexCharts(document.querySelector("#analysisChart4"), options1);
        chart1.render();

    }
}


function searchChart5() {
    $("#analysisChart5")[0].innerHTML = "<div class='analysis-chart' id=\"analysisChart5\"></div>";
    objJsonData.ITEM_NM = _fnToNull($("#CHART_TYPE5").val().replace(/\//gi, '.'));

    var rtnVal = _fnGetAjaxData("POST", "Statistic", "fnGetItemAnalyze", objJsonData);

    if (JSON.parse(rtnVal).Result[0].trxCode == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;
        var vTotal = 0;
        for (var i = 0; i < vResult.length; i++) {
            vTotal += _fnToZero(vResult[i]["PERF_DATA"]);
        }
        var vChartData = [];
        var vPortData = [];
        var j = 0;
        if (vResult.length == 0) {
            vChartData.push(0);
        } else {

            for (var i = 0; i < vResult.length; i++) {
                if (_fnToZero(vResult[j]["PERF_DATA"]) != 0 && (_fnToZero(vResult[j]["PERF_DATA"]) / vTotal) * 100 > 1) {
                    vChartData.push(Math.ceil(Number(vResult[j]["PERF_DATA"])));
                    vPortData.push(_fnToNull(vResult[j]["PORT_NM"]));
                    if ((j + 1) != vResult.length) {
                        j++;
                    }
                } else {
                    if ((j + 1) != vResult.length) {
                        j++;
                    }
                }
            }
        }
        if (matchMedia("screen and (max-width: 1600px)").matches) {
            var options1 = {
                series: vChartData,
                labels: vPortData,
                chart: {
                    type: 'donut',
                    height: 300,
                    width: '100%',
                },
                colors: ['#3c4d9f', '#5153a2', '#715ea6', '#97649b', '#bf6687', '#d4777d', '#b6c2d8', '#838e9f', '#4f5d77', '#4f5d77'],
                dataLabels: {
                    enabled:false,
                    dropShadow: {
                        enabled: false
                    },
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            offset: -25,
                        }
                    }
                },
                legend: {
                    position: 'bottom',
                    width: '100%',
                    height: 100
                },
                tooltip: { //호버 시 퍼센트 나타내기
                    enabled: true,
                    custom: function ({ series, seriesIndex, dataPointIndex, w }) {
                        let total = 0; for (let x of series) { total += x; } let selected = series[seriesIndex]
                        return w.config.labels[seriesIndex] + ":  " + (selected / total * 100).toFixed(2) + "%";
                    }
                },
                responsive: [
                    {
                        breakpoint: 1600,
                        options: {
                            chart: {
                                height: 300,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        },
                    },
                    {
                        breakpoint: 1024,
                        options: {
                            chart: {
                                height: 350,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        }
                    }
                ]
            };

        }
        else {

            var options1 = {
                series: vChartData,
                labels: vPortData,
                chart: {
                    type: 'donut',
                    height: '100%',
                },
                colors: ['#3c4d9f', '#5153a2', '#715ea6', '#97649b', '#bf6687', '#d4777d', '#b6c2d8', '#838e9f', '#4f5d77', '#4f5d77'],
                dataLabels: {
                    enabled:false,
                    dropShadow: {
                        enabled: false
                    },
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            offset: -25,
                        }
                    }
                },
                legend: {
                    width: 160,
                    height: '100%'
                },
                tooltip: { //호버 시 퍼센트 나타내기
                    enabled: true,
                    custom: function ({ series, seriesIndex, dataPointIndex, w }) {
                        let total = 0; for (let x of series) { total += x; } let selected = series[seriesIndex]
                        return w.config.labels[seriesIndex] + ":  " + (selected / total * 100).toFixed(2) + "%";
                    }
                },
                responsive: [
                    {
                        breakpoint: 1600,
                        options: {
                            chart: {
                                height: 300,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        },
                    },
                    {
                        breakpoint: 1024,
                        options: {
                            chart: {
                                height: 350,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        }
                    }
                ]
            };
        }
        var chart1 = new ApexCharts(document.querySelector("#analysisChart5"), options1);
        chart1.render();

    }
}


function searchChart6() {
    $("#analysisChart6")[0].innerHTML = "<div class='analysis-chart' id=\"analysisChart6\"></div>";
    objJsonData.ITEM_NM = _fnToNull($("#CHART_TYPE6").val().replace(/\//gi, '.'));

    var rtnVal = _fnGetAjaxData("POST", "Statistic", "fnGetItemAnalyze", objJsonData);

    if (JSON.parse(rtnVal).Result[0].trxCode == "Y") {
        var vResult = JSON.parse(rtnVal).Table1;
        var vTotal = 0;
        for (var i = 0; i < vResult.length; i++) {
            vTotal += _fnToZero(vResult[i]["PERF_DATA"]);
        }
        var vChartData = [];
        var vPortData = [];
        var j = 0;
        if (vResult.length == 0) {
            vChartData.push(0);
        } else {

            for (var i = 0; i < vResult.length; i++) {
                if (_fnToZero(vResult[j]["PERF_DATA"]) != 0 && (_fnToZero(vResult[j]["PERF_DATA"]) / vTotal) * 100 > 1) {
                    vChartData.push(Math.ceil(Number(vResult[j]["PERF_DATA"])));
                    vPortData.push(_fnToNull(vResult[j]["PORT_NM"]));
                    if ((j + 1) != vResult.length) {
                        j++;
                    }
                } else {
                    if ((j + 1) != vResult.length) {
                        j++;
                    }
                }
            }
        }
        if (matchMedia("screen and (max-width: 1600px)").matches) {
            var options1 = {
                series: vChartData,
                labels: vPortData,
                chart: {
                    type: 'donut',
                    height: 300,
                    width: '100%',
                },
                colors: ['#3c4d9f', '#5153a2', '#715ea6', '#97649b', '#bf6687', '#d4777d', '#b6c2d8', '#838e9f', '#4f5d77', '#4f5d77'],
                dataLabels: {
                    enabled:false,
                    dropShadow: {
                        enabled: false
                    },
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            offset: -25,
                        }
                    }
                },
                legend: {
                    position:'bottom',
                    width: '100%',
                    height: 100
                },
                tooltip: { //호버 시 퍼센트 나타내기
                    enabled: true,
                    custom: function ({ series, seriesIndex, dataPointIndex, w }) {
                        let total = 0; for (let x of series) { total += x; } let selected = series[seriesIndex]
                        return w.config.labels[seriesIndex] + ":  " + (selected / total * 100).toFixed(2) + "%";
                    }
                },
                responsive: [
                    {
                        breakpoint: 1600,
                        options: {
                            chart: {
                                height: 300,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        },
                    },
                    {
                        breakpoint: 1024,
                        options: {
                            chart: {
                                height: 350,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        }
                    }
                ]
            };

        }
        else {

            var options1 = {
                series: vChartData,
                labels: vPortData,
                chart: {
                    type: 'donut',
                    height: '100%',
                },
                colors: ['#3c4d9f', '#5153a2', '#715ea6', '#97649b', '#bf6687', '#d4777d', '#b6c2d8', '#838e9f', '#4f5d77', '#4f5d77'],
                dataLabels: {
                    enabled:false,
                    dropShadow: {
                        enabled: false
                    },
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            offset: -25,
                        }
                    }
                },
                legend: {
                    width: 160,
                    height: '100%'
                },
                tooltip: { //호버 시 퍼센트 나타내기
                    enabled: true,
                    custom: function ({ series, seriesIndex, dataPointIndex, w }) {
                        let total = 0; for (let x of series) { total += x; } let selected = series[seriesIndex]
                        return w.config.labels[seriesIndex] + ":  " + (selected / total * 100).toFixed(2) + "%";
                    }
                },
                responsive: [
                    {
                        breakpoint: 1600,
                        options: {
                            chart: {
                                height: 300,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        },
                    },
                    {
                        breakpoint: 1024,
                        options: {
                            chart: {
                                height: 350,
                                width: '100%',
                            },
                            legend: {
                                position: 'bottom',
                                width: '100%',
                                height: 100
                            }
                        }
                    }
                ]
            };
        }

        var chart1 = new ApexCharts(document.querySelector("#analysisChart6"), options1);
        chart1.render();

    }
}
/////////////////////function///////////////////////////////////

//////////////////////function makelist////////////////////////

/////////////////////API///////////////////////////////////////