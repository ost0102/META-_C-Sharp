

//#region ★★★★★★전역 변수★★★★★★
var _vSelectDate = new Date();
var obj = new Object();
var mymap;
var vHtml = "";
var bindDate = "";
//#endregion



//#region ★★★★★★시작영역 ★★★★★★
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    $(".sub_esti").addClass("on");
    $(".sub_esti .sub_depth").addClass("on");
    $(".sub_esti .sub_depth li:first-child a").addClass("on");


    if (_fnToNull($("#QUOT_NO").val()) != "") {
        fnSearchReqQuot();
        $("#Prc_quot").hide();
    }
    else { //이전 화면으로 돌아가기
        location.href = window.location.origin + "/Estimate/Regist";
    }

    //$("#ProgressBar_Loading").show(); //프로그래스 바
    //setTimeout(function () {
    //    $("#ProgressBar_Loading").hide(); //프로그래스 바
    //}, 1000);

    

});

//#endregion


//#region ★★★★★ Comm ★★★★★

//엔터키 Event
$(document).keyup(function (e) {
    if (e.keyCode == 13) {
        if ($(e.target).attr('data-index') != undefined) {
            var vIndex = $(e.target).attr('data-index');
            //#region 날짜쪽 nextFocus
            if (parseFloat(vIndex) == 1 || parseFloat(vIndex) == 2 || parseFloat(vIndex) == 3) {
                bindDate = "";
                if ($(e.target).val() != "") {
                    if (!_fnValiDate($(e.target).val())) { //날짜가 아닐 때
                        if (!layerChek) {
                            _fnAlertMsg("존재하지 않는 월입니다.", $(e.target).attr('id'));
                        }
                        else {
                            $(e.target).val('');
                            $(e.target).siblings().eq(0).hide();
                            layerChek = false;
                        }
                    }
                    else {
                        $(e.target).val(bindDate);
                        $('[data-index="' + (parseFloat(vIndex) + 1).toString() + '"]').focus();
                    }
                }
                else {
                    $('[data-index="' + (parseFloat(vIndex) + 1).toString() + '"]').focus();
                }
            }
            //#endregion
            else {
                $('[data-index="' + (parseFloat(vIndex) + 1).toString() + '"]').focus();
            }


        }
        
    }

});

//#endregion

//#region ★★★★★ 디테일 입력 영역 이벤트 ★★★★★


//#region 키보드 이벤트
//숫자 입력란 (ETD/ETA/견적유효기간 ) keydown event validation 숫자만 되도록

//날짜 표기
const autoHyphen = (target) => {
    target.value = target.value
        .replace(/[^0-9]/g, '')
        .replace(/^(\d{2,4})(\d{2})(\d{2})$/, `$1\-$2\-$3`);
}



////#region 날짜 관련 input validation
//$(document).on("focusout", "#ETD", function () {
//    var dateVal = _fnValiDate($(this).val());
//    if ($(this).val().length != 0) {
        
//        if (dateVal == "") {
//            _fnAlertMsg("존재하지 않은 날짜 입니다.", "ETD");
//            $(this).siblings().eq(0).hide();
//        }
//    }
//    $(this).val(dateVal);
//});


//$(document).on("focusout", "#ETA", function () {
//    var dateVal = _fnValiDate($(this).val());
//    if ($(this).val().length != 0) {

//        if (dateVal == "") {
//            _fnAlertMsg("존재하지 않은 날짜 입니다.", "ETA");
//            $(this).siblings().eq(0).hide();
//        }
//    }
//    $(this).val(dateVal);
//});

//$(document).on("focusout", "#QUOT_DATE", function () {
//    var dateVal = _fnValiDate($(this).val());
//    if ($(this).val().length != 0) {

//        if (dateVal == "") {
//            _fnAlertMsg("존재하지 않은 날짜 입니다.", "QUOT_DATE");
//            $(this).siblings().eq(0).hide();
//        }
//    }
//    $(this).val(dateVal);
//});


////#endregion

//txt 입력란 (비고 (db문제되는 문자 replace))

//VSL , Cntr 입력란 (영문, 숫자만)
const autoEnNum = (target) => {
    target.value = target.value
        .replace(/[^a-zA-Z0-9]/g, '')
}




//숫자 값들 입력 란
const onlyNum = (target) => {
    target.value = target.value
        .replace(/[^0-9\.]/g, '')
        .replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
}



//#endregion

//#endregion

//#region ★★★★★ 운임 입력 영역 이벤트 ★★★★★

function setSmaeValue(Pagetype) {
    if (Pagetype == "PC") { // pc에서 할때

    }

    if (Pagetype = "MO") { //모바일에서 할때

    }
}

//pc운임 추가 버튼
$(document).on('click', '.frt_btn.off', function () {
    $(this).parents('.esti_div').addClass('addrow');
    $(this).removeClass('off');
    $(this).addClass('on');
    var newFrt = $('.esti_div:first').clone();
    //newFrt.find('input').val('');
    //newFrt.find('.btns.icon.delete').hide();
    $(".esti_input__div").append(newFrt);
    $(this).parents('.esti_div').find('input').val('');
    $(this).parents('.esti_div').removeClass('addrow');
    $(this).parents('.esti_div').find('.btns.icon.delete').hide();
    $(this).addClass('off');
    $(this).removeClass('on');


    calcTotAmt();
    
});


//운임 삭제 버튼
$(document).on('click', '.frt_btn.on', function () {
    $(this).closest('.esti_div').remove();
    calcTotAmt();
});

//#endregion

//저장 버튼
$(document).on('click', "#btnRegist", function () {
    fnSaveQuot();
});

//#region ★★★★★  함수 영역 ★★★★★

function calcTotAmt() {

    
        var total_amt = 0;

        $(".amount_total").empty();

        var cnt = $(".addrow").length;

        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {
                total_amt += parseInt($(".addrow").eq(i).find(".tot_amt").val());
            }
        }
    
    

    $(".amount_total").text(total_amt);



}


//상단 메인 요청 데이터 조회
function fnSearchReqQuot() {
    //#region 파라미터 생성
    obj = new Object();

    obj.QUOT_NO = _fnToNull($("#QUOT_NO").val());
    //obj.CRN = _fnToNull($("#Session_CRN").val());
    obj.AUTH_KEY = $("#Session_AUTH_KEY").val();
    //#endRegion


    $.ajax({
        type: "POST",
        url: "/Estimate/fnGetQuotationList",
        async: true,
        dataType: "json",
        data: { "vJsonData": _fnMakeJson(obj) },
        success: function (rtnVal) {

            if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") { // 데이터 있을 때
                var DataList = JSON.parse(rtnVal).Table1;

                fnSettingHeader(DataList);
                $(".amount_total").text(0);
                //vHtml = fnMakeQuotList(DataList);
            }
        },
        beforeSend: function() {
            $("#ProgressBar_Loading").show(); //프로그래스 바
        },
        complete: function () {
            $("#ProgressBar_Loading").hide(); //프로그래스 바
        }
    });

}


//견적 저장
function fnSaveQuot() {
    //#region 파라미터 생성
    var obj1 = new Object();
    var objMain = new Object();
    var objFrt = new Object();
    var objCntr = new Object();
    var frtArray = [];
    var formData = new FormData();

    var objQoutData = new Object();
    var quot_mngt = _fnMakeMngt("CREATEQUOT"); //관리번호 채번

    //#region ☆☆☆메인 테이블☆☆☆

    //#region 헤더 데이터
    
    objMain.QUOT_NO = _fnToNull($("#QUOT_NO").val());//
    objMain.AUTH_KEY = $("#Session_AUTH_KEY").val();//
    objMain.MNGT_NO = quot_mngt;
    objMain.CRN = _fnToNull($("#Session_CRN").val());//
    objMain.USR_ID = _fnToNull($("#Session_USR_ID").val());
    objMain.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());

    objMain.FWD_OFFICE_NM = _fnToNull($("#Session_CUST_NM").val());//
    objMain.FWD_PIC_NM = _fnToNull($("#Session_CUST_NM").val());//
    objMain.FWD_PIC_TEL = _fnToNull($("#Session_HP_NO").val());//
    objMain.FWD_PIC_EMAIL = _fnToNull($("#Session_USR_ID").val());//


    objMain.POL_CD = _fnToNull($("#POL_CD").val());//
    objMain.POL_NM = _fnToNull($("#POL_NM").text());//
    objMain.POD_CD = _fnToNull($("#POD_CD").val()); // 
    objMain.POD_NM = _fnToNull($("#POD_NM").text());//
    objMain.REQ_YMD = _fnToNull($("#req_ymd").text()).replace(/\./gi, "");
    objMain.ITEM_NM = _fnToNull($("#ITEM_NM").text());//
    objMain.CUST_NM = _fnToNull($("#OFFICE_NM").val());//
    objMain.CUST_PIC_NM = _fnToNull($("#PIC_NM").text());//
    objMain.CUST_PIC_TEL = _fnToNull($("#PIC_TEL").val());//
    objMain.CUST_PIC_EMAIL = _fnToNull($("#PIC_MAIL").val());//

    if (_fnToNull($("#POL_CD").val()).substring(0, 2) != "KR") {
        objMain.SVC_TYPE = "IMPORT";
    } else {
        objMain.SVC_TYPE = "EXPORT";
    }

    //#endregion

    //#region 디테일
    
    objMain.ETD = _fnToNull($("#ETD").val()).replace(/-/gi, "");//
    objMain.ETA = _fnToNull($("#ETA").val()).replace(/-/gi, "");//
    objMain.VLDT_YMD = _fnToNull($("#VLDT").val()).replace(/-/gi, "");//
    objMain.REQ_SVC = _fnToNull($("#REQ option:selected").val());//
    objMain.CNTR_TYPE = String(_fnToNull($("#Cntr_Type option:selected").val())).toUpperCase();
    objMain.VSL = String(_fnToNull($("#VSL").val())).toUpperCase();//
    objMain.RMK = _fnToNull($("#RMK").val());//
    objMain.PKG = _fnToNull($("#PKG").val()).replace(/\,/gi, '');//
    objMain.GRS_WGT = _fnToNull($("#WGT").val()).replace(/\,/gi, '');//
    objMain.CBM = _fnToNull($("#CBM").val()).replace(/\,/gi, '');//
    objMain.CNTR_NO = _fnToNull($("#CNTR_NO").val()).replace(/\,/gi, '');//
    


    //#endregion

    //메인 데이터

    
    objQoutData.MAIN = JSON.parse(_fnMakeJson(objMain));
    //FormData.append("MAIN", JSON.parse(_fnMakeJson(objMain)));
    //#endregion 

    //#region ☆☆☆운임☆☆☆

    //운임 배열화 하여 json 처리
    var tr = $(".addrow").length;
    if (tr != 0) {
        //헤더
        var tblhdr = $(".addrow").eq(0).find('p').map(function () {
            return $(this).attr('value');

        }).get();
        //로우
        obj1 = "";
        var tbl = $(".addrow").map(function (idx, el) {

            obj1 = { id: idx + 1 };
            for (var i = 0; i < tblhdr.length; i++) {
                const td = $(el).find('.frt_txt').eq(i).val();
                obj1[tblhdr[i]] = td;
            }

            frtArray.push(obj1);
            return frtArray;
        }).get();

        objFrt = JSON.parse(JSON.stringify(frtArray));
        objQoutData.FRT = JSON.parse(_fnMakeJson(objFrt));

        //FormData.append("FRT", JSON.parse(_fnMakeJson(objFrt)));
    }


    //#endregion


    //#region ☆☆☆Cntr☆☆☆
    

    objCntr.CNTR_TYPE = _fnToNull($("#CNTR_NO").val());
    objQoutData.CNTR = JSON.parse(_fnMakeJson(objCntr));


    //FormData.append("CNTR", JSON.parse(_fnMakeJson(objCntr)));
    //#endregion

    formData.append("REQVAL", JSON.stringify(objQoutData));


    //전송
    //var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnFwdSendQuot", objQoutData);

    //if (rtnVal == "Y") { //요청 성공

    //}

    var request = new XMLHttpRequest();
    request.open("POST", _ApiUrl + "api/Quotation/SetFwdMakeQuotation")
    request.setRequestHeader("Authorization-Type", "Y");
    request.setRequestHeader("Authorization-Token", _fnToNull($("#Session_AUTH_KEY").val()));
    request.send(formData);
    request.onload = function (e) {
        if (this.status == 200) {
            console.log('response', this.statusText);
            //$("#QUOT_NO").val(this.responseText.replace(/\"/gi, ''));
            location.href = window.location.origin + "/Estimate/Inquiry";
            //fnSendForwarderList(forwardCnt); // 포워더 정보 전송
        }
    };

}

//날짜 벨리데이션
function _fnValiDate(strDate) {
    var sDate = String(strDate).replace(/-/gi, '');
    var formate = /^(19[7-9][0-9]|20\d{2})(0[0-9]|1[0-2])(0[1-9]|[1-2][0-9]|3[0-1])$/;
    var year = new Date().getFullYear();
    if (sDate.length == 8) { // 다입력시
        sDate = sDate;
    }
    if (sDate.length == 6) { // 10년도 연 + 월 일 
        sDate = String(year).substring(0, 2) + sDate;
    }
    if (sDate.length == 4) { // 월 일 
        sDate = String(year) + sDate;
    }

    if (!formate.test(sDate)) {
        return false;
    }
    else {
        bindDate = sDate.replace(/(\d{4})(\d{2})(\d{2})/, '$1\-$2\-$3');
        return true;
    }
}



//헤더 리스트 셋팅
function fnSettingHeader(vJson) {
    var _hHtml = "";
    var vResult = vJson;
    $("#estiList").empty();

    _hHtml += "<div class='esti-cont registdetail'>";
    _hHtml += " <div class='esti-cont__info'>";

    //pol
    _hHtml += "     <div class='esti-cont__box inquiry'>";
    _hHtml += "         <div class='esti-cont__inner'>";
    _hHtml += "             <div class='esti-cont__flex'>";
    _hHtml += "                 <div class='esti-cont__desc'><p id='POL_NM'>" + _fnToNull(vResult[0]["POL_NM"]) + "</p></div>";
    _hHtml += "                     <input type='hidden' id='POL_CD' value='" + _fnToNull(vResult[0]["POL_CD"]) + "'>";
    if (_fnToNull(vResult[0]["ETD"]) != "") {
        _hHtml += "                 <div class='esti-cont__desc2'><p>" + String(_fnToNull(vResult[0]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "(" + (_fnGetWhatDay_Eng(_fnToNull(vResult[0]["ETD"]).replace(/\./gi, ""))) + ")</p></div>";
    }
    else {
        _hHtml += "                 <div class='esti-cont__desc2'><p style='visibility:hidden'>-</p></div>";
    }
    _hHtml += "             </div>";
    _hHtml += "         </div>";
    _hHtml += "     </div>";
    //화살표
    _hHtml += "     <div class='esti-cont__box inquiry'>";
    _hHtml += "         <div class='esti-cont__inner'>";
    _hHtml += "             <p class='esti-cont__progress inquiry'><img src='/Images/icn_progress.png'></p>";
    _hHtml += "         </div>";
    _hHtml += "     </div>";
    //pod
    _hHtml += "     <div class='esti-cont__box inquiry'>";
    _hHtml += "         <div class='esti-cont__inner'>";
    _hHtml += "             <div class='esti-cont__flex'>";
    _hHtml += "                 <div class='esti-cont__desc'><p id='POD_NM'>" + _fnToNull(vResult[0]["POD_NM"]) + "</p></div>";
    _hHtml += "                     <input type='hidden' id='POD_CD' value='" + _fnToNull(vResult[0]["POD_CD"]) + "'>";
    if (_fnToNull(vResult[0]["ETA"]) != "") {
        _hHtml += "                 <div class='esti-cont__desc2'><p>" + String(_fnToNull(vResult[0]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "(" + (_fnGetWhatDay_Eng(_fnToNull(vResult[0]["ETA"]).replace(/\./gi, ""))) + ")</p></div>";
    }
    else {
        _hHtml += "                 <div class='esti-cont__desc2'><p style='visibility:hidden'>-</p></div>";
    }
    _hHtml += "             </div>";
    _hHtml += "         </div>";
    _hHtml += "     </div>";
    //헤더세부정보
    _hHtml += "     <div class='esti-cont__box inquiry'>";
    _hHtml += "         <div class='esti-cont__inner inner-flex'>";
    //견적일
    _hHtml += "             <div class='esti-cont__date flex-column'>";

    _hHtml += "                 <div class='esti-cont__date_start'>";
    _hHtml += "                     <span class='esti-cont__date_title'>견적요청일</span>";
    _hHtml += "                     <span class='esti-cont__date_cont' id='req_ymd'>" + String(_fnToNull(vResult[0]["REQ_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/,'$1.$2.$3')+"</span>";
    _hHtml += "                 </div>";

    _hHtml += "                 <div class='esti-cont__date_end'>";
    _hHtml += "                     <span class='esti-cont__date_title'>최종 견적일</span>";
    _hHtml += "                     <span class='esti-cont__date_cont' style='visibility:hidden'></span>";
    _hHtml += "                 </div>";

    _hHtml += "             </div>";
    //상품명,담당자명
    _hHtml += "             <div class='esti-cont__etc'>";

    _hHtml += "                 <div class='esti-cont__etc_item'>";
    _hHtml += "                     <span class='esti-cont__etc_title'>품목명</span>";
    _hHtml += "                     <span class='esti-cont__etc_cont' id='ITEM_NM'>"+_fnToNull(vResult[0]["ITEM_NM"])+"</span>";
    _hHtml += "                 </div>";

    _hHtml += "                 <div class='esti-cont__etc_item'>";
    _hHtml += "                     <span class='esti-cont__etc_title'>담당자</span>";
    _hHtml += "                     <span class='esti-cont__etc_cont' id='PIC_NM'>" + _fnToNull(vResult[0]["PIC_NM"]) + "</span>";
    _hHtml += "                     <input type='hidden' id='PIC_TEL' value='" + _fnToNull(vResult[0]["PIC_TEL"]) + "'>";
    _hHtml += "                     <input type='hidden' id='PIC_MAIL' value='" + _fnToNull(vResult[0]["PIC_MAIL"]) + "'>";
    _hHtml += "                 </div>";

    _hHtml += "             </div>";
    _hHtml += "         </div>";
    _hHtml += "     </div>";

    _hHtml += "     <div class='esti-cont__box inquiry'>";
    _hHtml += "         <div class='esti-cont__inner' id='Prc_quot' style='display:none'></div>";
    _hHtml += "     </div>";

    _hHtml += " </div>";
    _hHtml += "</div>";

    $("#estiList").append(_hHtml);

    $("#ETD").val(String(_fnToNull(vResult[0]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1-$2-$3'));
    $("#ETA").val(String(_fnToNull(vResult[0]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1-$2-$3'));
}




//#endregion




