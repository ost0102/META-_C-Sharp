////////////////////전역 변수//////////////////////////
var swiper;
var swiper2;
var swiper3;
$("#today_dt").text(fnSetNowDate()); //현재 날짜 세팅
var tt = $("#today_dt").text().split('.')[1];
////////////////////jquery event///////////////////////
$(function () {

    //로그인 하지 않고 들어왔을때
    //if (_fnToNull($("#Session_USR_ID").val()) == "") {
    //    location.href = window.location.origin;
    //}

  swiper = new Swiper(".swiper-container", {
        //loop: true,
        navigation: {
            nextEl: ".swiper-button-next",
            prevEl: ".swiper-button-prev"
        },
        breakpoints: {
            1201: {
                centeredSlides: true,
                slidesPerView: 1.6,
                spaceBetween: 140,
            },
        },
    });

    // swiper2 = new Swiper(".swiper-container2", {
    //    //loop: true,
    //    navigation: {
    //        nextEl: ".swiper-button-next",
    //        prevEl: ".swiper-button-prev"
    //    },
    //    breakpoints: {
    //        1201: {
    //            centeredSlides: true,
    //            slidesPerView: 1.6,
    //            spaceBetween: 140,
    //        },
    //    },
    //});

    //swiper3 = new Swiper(".swiper-container3", {
    //    //loop: true,
    //    navigation: {
    //        nextEl: ".swiper-button-next",
    //        prevEl: ".swiper-button-prev"
    //    },
    //    breakpoints: {
    //        1201: {
    //            centeredSlides: true,
    //            slidesPerView: 1.6,
    //            spaceBetween: 140,
    //        },
    //    },
    //});


    swiper.slideTo(11, 500, false);
    swiper2.slideTo(3, 500, false);
    swiper3.slideTo(1, 500, false);
   

});

$(document).on("click", ".GetServicePrice", function () {
    fnSetTotalPrice();
});
 

function fnSetTotalPrice() {

    var vTotalPrice = 0;
    if ($("#chkEsti").is(":checked")) {
        vTotalPrice += 200000;
    };
    if ($("#chkExim").is(":checked")) {
        vTotalPrice += 100000;
    };
    if ($("#chkData").is(":checked")) {
        vTotalPrice += 100000;
    };

    $("#tot_prc").text(fnSetComma(vTotalPrice));
}
function fnSetNowDate() {
    try {
        var weekDate = _vSelectDate.getTime() + (24 * 60 * 60 * 1000);
        _vSelectDate.setTime(weekDate);

        var weekYear = _vSelectDate.getFullYear();
        var weekMonth = _vSelectDate.getMonth() + 1;
        var weekDay = _vSelectDate.getDate() - 1;

        var result = weekYear + "." + _pad(weekMonth, "2") + "." + _pad(weekDay, "2");
        return result;
    }
    catch (err) {
        console.log("[Error - fnSetNowDate]" + err.message);
    }
}
$("#month_tab").click(function (e) {
    $("#bangi").hide();
    $("#month").show();
    $("#month_child").attr('style', 'display:block');
    $("#bungi").hide();
    swiper = new Swiper(".swiper-container", {
        //loop: true,
        navigation: {
            nextEl: ".slide01 .swiper-button-next",
            prevEl: ".slide01 .swiper-button-prev"
        },
        breakpoints: {
            1201: {
                centeredSlides: true,
                slidesPerView: 1.6,
                spaceBetween: 140,
            },
        },
    });

    swiper.slideTo((parseInt(tt) - 1), 500, false);
});
$("#btnRequest").click(function (e) {
    $(".alert_cont .inner").html("서비스 변경 요청되었습니다.<br>");
    layerPopup('#alert01');
});

$("#bungi_tab").click(function (e) {
    $("#month").hide();
    $("#bungi").show();
    $("#bungi_child").attr('style','display:block');
    $("#bangi").hide();
    swiper2 = new Swiper(".swiper-container2", {
        //loop: true,
        navigation: {
            nextEl: ".slide02 .swiper-button-next",
            prevEl: ".slide02 .swiper-button-prev"
        },
        breakpoints: {
            1201: {
                centeredSlides: true,
                slidesPerView: 1.6,
                spaceBetween: 140,
            },
        },
    });

    swiper2.slideTo(3, 500, false);
});
$("#bangi_tab").click(function (e) {
    $("#month").hide();
    $("#bangi").show();
    $("#bangi_child").attr('style', 'display:block');
    $("#bungi").hide();
    swiper3 = new Swiper(".swiper-container3", {
        //loop: true,
        navigation: {
            nextEl: ".slide03 .swiper-button-next",
            prevEl: ".slide03 .swiper-button-prev"
        },
        breakpoints: {
            1201: {
                centeredSlides: true,
                slidesPerView: 1.6,
                spaceBetween: 140,
            },
        },
    });

    swiper3.slideTo(1, 500, false);
});

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

/////////////////////API///////////////////////////////////////