//#region ★★★★★★전역 변수★★★★★★
var _vSelectDate = new Date();
var obj = new Object();
var mymap;
var vHtml = "";
var objJsonData = new Object();
//#endregion



//#region ★★★★★★시작영역 ★★★★★★
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "" || _fnToNull($("#MNGT_NO").val()) == "") {
        location.href = window.location.origin;
    }
    

    $(".sub_esti").addClass("on");
    $(".sub_esti .sub_depth").addClass("on");
    $(".sub_esti .sub_depth li:nth-child(2) a").addClass("on");


  

    //$("#ProgressBar_Loading").show(); //프로그래스 바
    //setTimeout(function () {
    //    $("#ProgressBar_Loading").hide(); //프로그래스 바
    //}, 1000);

    fnSearchData();

});


//#endregion



//#region ★★★★★★조회 영역 이벤트★★★★★★

//목록 버튼 클릭
$(document).on("click", "#back_list",function () {
    location.href = window.location.origin + "/Estimate/Inquiry";
});


//#endregion



//#region ★★★★★★Func ★★★★★

//자료 조회
function fnSearchData() {

    objJsonData = new Object();

    objJsonData.AUTH_KEY = _fnToNull($("#Session_AUTH_KEY").val());
    objJsonData.CRN = _fnToNull($("#Session_CRN").val());
    objJsonData.MNGT_NO = _fnToNull($("#MNGT_NO").val());

    $.ajax({
        type: "POST",
        url: "/Estimate/fnGetFwdQuotData",
        async: true,
        dataType: "json",
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (rtnVal) {
            if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
                bindData(rtnVal);
            }
        }

    });
}

//#endregion


function bindData(obj) {

    //#region MainData Bind
    var HdObj = new Object();
    HdObj = JSON.parse(obj).HEADER[0];
    vHtml = "";

    $(".registdetail").empty();

    //#region Header
    vHtml += " <div class='esti-cont__info'>";
    //pol
    vHtml += "  <div class='esti-cont__box inquiry'>";
    vHtml += "      <div class='esti-cont__inner'>"
    vHtml += "          <div class='esti-cont__flex'>";
    vHtml += "              <div class='esti-cont__desc'><p>" + _fnToNull(HdObj["POL_NM"]) + "</p></div>";
    if (_fnToNull(HdObj["ETD"]) != "") {
        vHtml += "              <div class='esti-cont__desc2'><p>"+ String(_fnToNull(HdObj["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "(" + (_fnGetWhatDay_Eng(_fnToNull(HdObj["ETD"]).replace(/\./gi, ""))) + ")" +"</p></div>";
    }
    else {
        vHtml += "              <div class='esti-cont__desc2'><p style='visibility:hidden'>-</p></div>";
    }
    
    vHtml += "          </div>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    //화살표
    vHtml += "  <div class='esti-cont__box inquiry'>";
    vHtml += "      <div class='esti-cont__inner'>"
    vHtml += "          <p class='esti-cont__progress inquiry'><img src='/Images/icn_progress.png'></p>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    //pod
    vHtml += "  <div class='esti-cont__box inquiry'>";
    vHtml += "      <div class='esti-cont__inner'>"
    vHtml += "          <div class='esti-cont__flex'>";
    vHtml += "              <div class='esti-cont__desc'><p>" + _fnToNull(HdObj["POD_NM"]) + "</p></div>";
    if (_fnToNull(HdObj["ETD"]) != "") {
        vHtml += "              <div class='esti-cont__desc2'><p>" + String(_fnToNull(HdObj["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "(" + (_fnGetWhatDay_Eng(_fnToNull(HdObj["ETA"]).replace(/\./gi, ""))) + ")" + "</p></div>";
    }
    else {
        vHtml += "              <div class='esti-cont__desc2'><p style='visibility:hidden'>-</p></div>";
    }

    vHtml += "          </div>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    //세부정보
    vHtml += "  <div class='esti-cont__box inquiry'>";
    vHtml += "      <div class='esti-cont__inner inner-flex'>"
    vHtml += "          <div class='esti-cont__date flex-column'>";
    vHtml += "              <div class='esti-cont__date_start'>";
    vHtml += "                  <span class='esti-cont__date_title'>견적요청일</span>";
    vHtml += "                  <span class='esti-cont__date_cont'>" + String(_fnToNull(HdObj["REQ_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3')+"</span>";
    vHtml += "              </div>";
    vHtml += "              <div class='esti-cont__date_end'>";
    vHtml += "                  <span class='esti-cont__date_title'>최종요청일</span>";
    vHtml += "                  <span class='esti-cont__date_cont'>" + String(_fnToNull(HdObj["QUOT_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</span>";
    vHtml += "              </div>";
    vHtml += "          </div>";
    vHtml += "          <div class='esti-cont__etc'>";
    vHtml += "              <div class='esti-cont__etc_item'>";
    vHtml += "                  <span class='esti-cont__etc_title'>품목명</span>";
    vHtml += "                  <span class='esti-cont__etc_cont'>" + _fnToNull(HdObj["ITEM_NM"])+"</span>";
    vHtml += "              </div>";
    vHtml += "              <div class='esti-cont__etc_item'>";
    vHtml += "                  <span class='esti-cont__etc_title'>담당자</span>";
    vHtml += "                  <span class='esti-cont__etc_cont'>" + _fnToNull(HdObj["CUST_PIC_NM"]) + "</span>";
    vHtml += "              </div>";
    vHtml += "          </div>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    //견적금액
    vHtml += "  <div class='esti-cont__box inquiry'>";
    vHtml += "      <div class='esti-cont__inner'>"
    vHtml += "          <div class='esti-cont__etc'>";
    vHtml += "              <div class='esti-cont__etc_item'>";
    vHtml += "                  <span class='esti-cont__etc_title'>견적금액</span>";
    vHtml += "                  <span class='esti-cont__etc_cont amount'>"
    var curr;
    var loc_amt;
    var amt;
    if (_fnToNull(HdObj["CURR_CD"]) != "") {
        curr = String(_fnToNull(HdObj["CURR_CD"])).split(",");
        loc_amt = String(_fnToNull(HdObj["FARE_LOC_AMT"])).split(",");
        amt = String(_fnToNull(HdObj["FARE_AMT"])).split(",");
        for (var k = 0; k < curr.length; k++) {
            if (k == 0) {
                if (curr[k] == "KRW") {
                    vHtml += _fnToNull(curr[k]) + ":" + _fnToNull(_fnGetNumber(parseInt(loc_amt[k]),"sum"));
                }
                else {
                    vHtml += _fnToNull(curr[k]) + ":" + _fnToNull(_fnGetNumber(parseInt(amt[k]), "sum"));
                }
            }
            else
            {
                vHtml += "&nbsp; |&nbsp;";
                if (curr[k] == "KRW") {
                    vHtml += _fnToNull(curr[k]) + ":" + _fnToNull(_fnGetNumber(parseInt(loc_amt[k]), "sum"));
                }
                else {
                    vHtml += _fnToNull(curr[k]) + ":" + _fnToNull(_fnGetNumber(parseInt(amt[k]), "sum"));
                }
                
            }
        }
    }

    vHtml += "                  </span>";
    vHtml += "              </div>";
    vHtml += "          </div>";
    vHtml += "      </div>";
    vHtml += "  </div>";

    vHtml += " </div>"
    //#endregion
    

    $(".registdetail").append(vHtml);
    //#endregion

    //#region Detail Bind

    var DtlObj = new Object();

    DtlObj = JSON.parse(obj).DETAIL[0];
    
    vHtml = "";

    $("#detail_data").empty();

    //#rregion 상세견적내역
    vHtml += "<div class='esti-input flex4'>";
    vHtml += "  <div class='esti-input__col'>";
    vHtml += "      <p class='esti-input__tit'>ETD</p>";
    vHtml += "      <div class='int_box'>";
    vHtml += "          <input type='text' value='" + String(_fnToNull(DtlObj["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/gi,'$1.$2.$3')+"' disabled>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    vHtml += "  <div class='esti-input__col'>";
    vHtml += "      <p class='esti-input__tit'>ETA</p>";
    vHtml += "      <div class='int_box'>";
    vHtml += "          <input type='text' class value='" + String(_fnToNull(DtlObj["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/gi, '$1.$2.$3')+"' disabled>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    vHtml += "  <div class='esti-input__col'>";
    vHtml += "      <p class='esti-input__tit'>견적유효기간</p>";
    vHtml += "      <div class='int_box'>"
    vHtml += "          <input type='text' value='" + String(_fnToNull(DtlObj["VLDT_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/gi, '$1.$2.$3')+"' disabled>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    vHtml += "  <div class='esti-input__col'>";
    vHtml += "      <p class='esti-input__tit'>REMARK</p>";
    vHtml += "      <div class='int_box'>";
    vHtml += "          <input type='text' value='"+_fnToNull(DtlObj["RMK"])+"' disabled>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    vHtml += "</div>";

    vHtml += "<div class='esti-input flex3'>";
    vHtml += "  <div class='esti-input__col'>";
    vHtml += "      <p class='esti-input__tit'>서비스<p>";
    vHtml += "      <div class='int_box'>";
    vHtml += "          <input type='text' value='" + _fnToNull(DtlObj["REQ_SVC"]) + "' disabled>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    vHtml += "  <div class='esti-input__col'>";
    vHtml += "      <p class='esti-input__tit'>VESSEL NO</p>";
    vHtml += "      <div class='int_box'>";
    vHtml += "          <input type='text' value='"+_fnToNull(DtlObj["VSL"])+"' disabled>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    vHtml += "  <div class='esti-input__col'>";
    var cntr_type;
    if (_fnToNull(DtlObj["CNTR_TYPE"]) == "F") {
        cntr_type = "FCL";
    }
    else {
        cntr_type = "LCL";
    }

    vHtml += "      <p class='esti-input__tit'> FCL/LCL</p>";
    vHtml += "      <div class='int_box'>";
    vHtml += "          <input type='text' value='"+cntr_type+"' disabled>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    vHtml += "  <div class='esti-input__col' style='visibility:hidden;'>";
    vHtml += "      <p class='esti-input__tit'></p>";
    vHtml += "      <div class='int_box'>";
    vHtml += "          <input type='text' disabled>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    vHtml += "</div>";

    vHtml += "<div class='esti-input flex4'>";
    vHtml += "  <div class='esti-input__col'>";
    vHtml += "      <p class='esti-input__tit'>PKG</p>";
    vHtml += "      <div class='int_box'>";
    vHtml += "          <input type='text' value='"+_fnToNull(DtlObj["PKG"])+"' disabled>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    vHtml += "  <div class='esti-input__col'>";
    vHtml += "      <p class='esti-input__tit'>WEIGHT</p>";
    vHtml += "      <div class='int_box'>";
    vHtml += "          <input type='text' value='"+_fnToNull(DtlObj["GRS_WGT"])+"' disabled>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    vHtml += "  <div class='esti-input__col'>";
    vHtml += "      <p class='esti-input__tit'>CBM</p>";
    vHtml += "      <div class='int_box'>";
    vHtml += "          <input type='text' value='"+_fnToNull(DtlObj["CBM"])+"' disabled>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    vHtml += "  <div class='esti-input__col'>";
    vHtml += "      <p class='esti-input__tit'>Container</p>";
    vHtml += "      <div class='int_box'>";
    vHtml += "          <input type='text' value='"+_fnToNull(DtlObj["CNTR_NO"])+"' disabled>";
    vHtml += "      </div>";
    vHtml += "  </div>";
    vHtml += "</div>";


    //#endregion


    $("#detail_data").append(vHtml);


    //#endregion

    //#region Frat Bind

    vHtml = "";
    $("#frt_list").empty();

    var frtObj = new Object();
    frtObj = JSON.parse(obj).FRT
    var tot_price = 0;
    

    $.each(frtObj, function (i) {
        vHtml += "<div class='esti_div exinquirydetail'>";

        vHtml += "  <div class='esti-input_col'>";
        vHtml += "      <p class='esti-input_tit' value='FARE_CD'>운임</p>";
        vHtml += "      <div class='int_box'>";
        vHtml += "          <input type='text' class='frt_txt' value='" + frtObj[i]["FARE_CD"]+"'>";
        vHtml += "      </div>";
        vHtml += "  </div>";
        vHtml += "  <div class='esti-input_col'>";
        vHtml += "      <p class='esti-input_tit' value='FARE_NM'>운임명</p>";
        vHtml += "      <div class='int_box'>";
        vHtml += "          <input type='text' class='frt_txt' value='" + frtObj[i]["FARE_NM"] + "'>";
        vHtml += "      </div>";
        vHtml += "  </div>";
        vHtml += "  <div class='esti-input_col'>";
        vHtml += "      <p class='esti-input_tit' value='CUR_CD'>통화</P>";
        vHtml += "      <div class='int_box'>";
        vHtml += "          <input type='text' class='frt_txt' value='" + frtObj[i]["CURR_CD"] + "'>";
        vHtml += "      </div>";
        vHtml += "  </div>";
        vHtml += "  <div class='esti-input_col'>";
        vHtml += "      <p class='esti-input_tit' value='PKG_UNIT'>단위</P>";
        vHtml += "      <div class='int_box'>";
        vHtml += "          <input type='text' class='frt_txt' value='" + frtObj[i]["PKG_UNIT"] + "'>";
        vHtml += "      </div>";
        vHtml += "  </div>";
        vHtml += "  <div class='esti-input_col'>";
        vHtml += "      <p class='esti-input_tit' value='PKG'>Qty</P>";
        vHtml += "      <div class='int_box'>";
        vHtml += "          <input type='text' class='frt_txt' value='" + frtObj[i]["PKG"] + "'>";
        vHtml += "      </div>";
        vHtml += "  </div>";
        vHtml += "  <div class='esti-input_col'>";
        vHtml += "      <p class='esti-input_tit' value='UNIT_PRC'>단가</P>";
        vHtml += "      <div class='int_box'>";
        vHtml += "          <input type='text' class='frt_txt' value='" + frtObj[i]["UNIT_PRC"].toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",") + "'>";
        vHtml += "      </div>";
        vHtml += "  </div>";
        vHtml += "  <div class='esti-input_col'>";
        vHtml += "      <p class='esti-input_tit' value='FARE_AMT'>금액</P>";
        vHtml += "      <div class='int_box'>";
        vHtml += "          <input type='text' class='frt_txt' value='" + frtObj[i]["FARE_AMT"].toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",") + "'>";
        vHtml += "      </div>";
        vHtml += "  </div>";
        vHtml += "  <div class='esti-input_col'>";
        vHtml += "      <p class='esti-input_tit' value='FARE_LOC_AMT'>원화금액</P>";
        vHtml += "      <div class='int_box'>";
        vHtml += "          <input type='text' class='frt_txt' value='" + frtObj[i]["FARE_LOC_AMT"].toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",") + "'>";
        vHtml += "      </div>";
        vHtml += "  </div>";
        vHtml += "  <div class='esti-input_col'>";
        vHtml += "      <p class='esti-input_tit' value='FARE_VAT_AMT'>세액</P>";
        vHtml += "      <div class='int_box'>";
        vHtml += "          <input type='text' class='frt_txt' value='" + frtObj[i]["FARE_VAT_AMT"].toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",") + "'>";
        vHtml += "      </div>";
        vHtml += "  </div>";
        vHtml += "  <div class='esti-input_col'>";
        vHtml += "      <p class='esti-input_tit' value='TOT_AMT'>총금액</P>";
        vHtml += "      <div class='int_box'>";
        vHtml += "          <input type='text' class='frt_txt' value='" + frtObj[i]["TOT_AMT"].toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",") + "'>";
        vHtml += "      </div>";
        vHtml += "  </div>";


        vHtml += "</div>";
        tot_price += parseInt(frtObj[i]["TOT_AMT"]);
    });

    $("#frt_list").append(vHtml);

    $(".amount_total").empty();
    $(".amount_total").append(tot_price.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",") );

    //#endregion

}