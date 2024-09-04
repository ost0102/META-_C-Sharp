////////////////////전역 변수//////////////////////////
////////////////////jquery event///////////////////////
//$(function () {
//
//});



//모바일 화물추적 버튼 이벤트
$(document).on("click", "#mo_Layer_SearchTracking", function () {
    $("#Layer_S_BL_NO").val("");

    if (_fnToNull($("#mo_Layer_T_HBL_NO").val()) == "") {
        _fnAlertMsg("MBL , HBL , CntrNo 중 하나를 입력 해 주세요.");
        return false;
    } else {
        $("#Layer_S_BL_NO").val($("#mo_Layer_T_HBL_NO").val().replace(/ /gi, ""));
        fnGetLayerTrkData($("#mo_Layer_T_HBL_NO").val().replace(/ /gi,""));
    }
});

//모바일 공통 화물 추적 엔터키 이벤트
$(document).on("keyup", "#mo_Layer_T_HBL_NO", function (e) {
    if (e.keyCode == 13) {
        if (_fnToNull($("#mo_Layer_T_HBL_NO").val()) == "") {
            _fnAlertMsg("MBL , HBL , CntrNo 중 하나를 입력 해 주세요.");
            return false;
        } else {
            $("#Layer_S_BL_NO").val($("#mo_Layer_T_HBL_NO").val().replace(/ /gi, ""));
            fnGetLayerTrkData($("#mo_Layer_T_HBL_NO").val().replace(/ /gi, ""));
        }
    }
});

//공통 화물 추적 버튼 이벤트
$(document).on("click", "#Layer_SearchTracking", function () {
    $("#Layer_S_BL_NO").val("");

    if (_fnToNull($("#Layer_T_HBL_NO").val()) == "") {
        _fnAlertMsg("MBL , HBL , CntrNo 중 하나를 입력 해 주세요.");
        return false;
    } else {
        $("#Layer_S_BL_NO").val($("#Layer_T_HBL_NO").val().replace(/ /gi, ""));
        fnGetLayerTrkData($("#Layer_T_HBL_NO").val().replace(/ /gi, ""));
    }
});


//공통 화물 추적 엔터키 이벤트
$(document).on("keyup", "#Layer_T_HBL_NO", function (e) {
    if (e.keyCode == 13) {
        if (_fnToNull($("#Layer_T_HBL_NO").val()) == "") {
            _fnAlertMsg("MBL , HBL , CntrNo 중 하나를 입력 해 주세요.");
            return false;
        } else {
            $("#Layer_S_BL_NO").val($("#Layer_T_HBL_NO").val().replace(/ /gi, ""));
            fnGetLayerTrkData($("#Layer_T_HBL_NO").val().replace(/ /gi, ""));
        }
    }
});

//레이어 안 공통 화물 추적 버튼 이벤트
$(document).on("click", "#btn_layerTrkList", function () {

    if (_fnToNull($("#Layer_S_BL_NO").val()) == "") {
        _fnAlertMsg("MBL , HBL , CntrNo 중 하나를 입력 해 주세요.");
        return false;
    } else {
        fnGetLayerTrkData($("#Layer_S_BL_NO").val().replace(/ /gi, ""));
    }
});

//레이어 안 공통 화물 추적 엔터 키 이벤트
$(document).on("keyup", "#Layer_S_BL_NO", function (e) {

    if (e.keyCode == 13) {
        if (_fnToNull($("#Layer_S_BL_NO").val()) == "") {
            _fnAlertMsg("MBL , HBL , CntrNo 중 하나를 입력 해 주세요.");
            return false;
        } else {
            fnGetLayerTrkData($("#Layer_S_BL_NO").val().replace(/ /gi, ""));
        }
    }

});

$(document).on("click", ".nav_toggle", function () {
    if ($("#header.sub").hasClass("close")) {
        sessionStorage.setItem("key1", "N");
    } else {
        sessionStorage.setItem("key1", "Y");
    }
});

//로그아웃 버튼 이벤트
$(document).on("click", "#login_btn", function () {    
    _fnLogout();
});

/////////////////////function///////////////////////////////////
////로그아웃 (세션 , 쿠키 삭제)
function _fnLogout() {
    ////로그아웃 (세션 , 쿠키 삭제)
    $.ajax({
        type: "POST",
        url: "/Main/fnLogOut",
        async: false,
        success: function (result, status, xhr) {
            location.href = window.location.origin;
        }
    });
}

//레이어 팝업 화물추적 함수
function fnGetLayerTrkData(vBLNO) {
    var vHtml = "";
    try {
        var objJsonData = new Object();
        objJsonData.AUTH_KEY = $("#Session_AUTH_KEY").val();
        objJsonData.MNGT_NO = vBLNO;

        var rtnVal = _fnGetAjaxData("POST", "Estimate", "fnGetTrackingList", objJsonData);
        if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
            var vResult = JSON.parse(rtnVal).Table1;
            $(".trk_list").empty();
            vHtml += "		<div class='trk_box wanna_open'>			";
            vHtml += "	        <div class='trk_cover'>			";
            vHtml += "	            <div style='display:none'>" + _fnToNull(vResult[0].HBL_NO) + "</div>			";
            vHtml += "	            <div style='display:none'>" + _fnToNull(vResult[0].CNTR_NO) + "</div>			";
            vHtml += "	            <div class='trk_info'>			";
            vHtml += "	                <p>Master B/L No.</p>			";
            vHtml += "	                <p class='trk_cont'>" + _fnToNull(vResult[0].MBL_NO) + "</p>			";
            vHtml += "	            </div>			";
            vHtml += "	            <div class='trk_info'>			";
            vHtml += "	                <p>Container No.</p>			";
            vHtml += "	                <p class='trk_cont'>" + _fnToNull(vResult[0].CNTR_NO) + "</p>			";
            vHtml += "	            </div>			";
            vHtml += "	            <div class='trk_info'>			";
            vHtml += "	                <p>Last location</p>			";
            vHtml += "	                <p class='trk_cont'>- " + _fnToNull(vResult[0].LAST_EVENT_NM) + "</p>			";
            vHtml += "	            </div>			";
            //vHtml += "	            <button class='btn_open btn_listOpen' id='" + 0 + "'></button>			";
            vHtml += "	        </div>			";

            vHtml += "          <div class='trcaking_bg'>"

            
            vHtml += "              <div class='tracking_process'>";
            var max_len = vResult.length -1 
            $.each(vResult, function (j) {
                vHtml += "<div class='tracking_process_list'>"
                if (_fnToNull(vResult[j].EVENT_NM) == _fnToNull(vResult[0].LAST_EVENT_NM)) {
                    vHtml += "  <div class='trk_process_box on'>" + _fnToNull(vResult[j].EVENT_NM) + "</div>";
                }
                else {
                    if (vResult[j].REQ_SVC == "SEA" && vResult[j].EX_IM_TYPE == "I" ) { //해운 수입 건일 경우
                        if (j == 2 || j == 3) {
                            var event_txt1 = _fnToNull(vResult[j].EVENT_NM).slice(0, 2);
                            var event_txt2 = _fnToNull(vResult[j].EVENT_NM).slice(2, _fnToNull(vResult[j].EVENT_NM).length);
                            vHtml += "  <div class='trk_process_box'>" + event_txt1 +"</br>" + event_txt2 + "</div>";
                        }
                        else {
                            vHtml += "  <div class='trk_process_box'>" + _fnToNull(vResult[j].EVENT_NM) + "</div>";
                        }
                    }
                    else {
                        vHtml += "  <div class='trk_process_box'>" + _fnToNull(vResult[j].EVENT_NM) + "</div>";
                    }
                    
                }
                
                vHtml += "</div>"
                if (j != max_len) {
                    vHtml += "                  <img src='/Images/step.png'>";
                }
            });
            //#region 기존 직접 그림

            //vHtml += "                  <div class='tracking_process_list'>";
            //vHtml += "	                    <div class='trk_process_box'>공컨반출</div>";                
            //vHtml += "	                </div>			";
            //vHtml += "                  <img src='/Images/step.png'>";
            //vHtml += "                  <div class='tracking_process_list'>";
            //vHtml += "	                    <div class='trk_process_box'>반입</div>";
            //vHtml += "	                </div>			";
            //vHtml += "                  <img src='/Images/step.png'>";
            //vHtml += "                  <div class='tracking_process_list'>";
            //vHtml += "	                    <div class='trk_process_box'>선적</div>";
            //vHtml += "	                </div>			";
            //vHtml += "                  <img src='/Images/step.png'>";
            //vHtml += "                  <div class='tracking_process_list'>";
            //vHtml += "	                    <div class='trk_process_box'>출항</div>";
            //vHtml += "	                </div>			";
            //vHtml += "                  <img src='/Images/step.png'>";
            //vHtml += "                  <div class='tracking_process_list'>";
            //vHtml += "	                    <div class='trk_process_box'>입항</div>";
            //vHtml += "	                </div>			";

            //#endregion
            vHtml += "	            </div>			";
            


            vHtml += "	            <div class='tracking_box' id='layer_trk_list'>			";
            $.each(vResult, function (i) {
            if (_fnToNull(vResult[i].EVENT_CD) == _fnToNull(vResult[i].LAST_EVENT_CD)) {
                if (_fnToNull(vResult[i].REQ_SVC) == "SEA") {
                    if (_fnToNull(vResult[i].EX_IM_TYPE) == "E") {
                        if (i == 4) {
                            vHtml += "	            <div class='track_stat last on'>			";
                        } else {
                            vHtml += "	            <div class='track_stat on'>			";
                        }
                    } else {
                        if (i == 5) {
                            vHtml += "	            <div class='track_stat import last on'>			";
                        } else {
                            vHtml += "	            <div class='track_stat import on'>			";
                        }
                    }
                } else {
                    if (_fnToNull(vResult[i].EX_IM_TYPE) == "E") {
                        if (i == 1) {
                            vHtml += "	            <div class='track_stat air last on'>			";
                        } else {
                            vHtml += "	            <div class='track_stat air on'>			";
                        }
                    } else {
                        if (i == 1) {
                            vHtml += "	            <div class='track_stat import air last on'>			";
                        } else {
                            vHtml += "	            <div class='track_stat import air on'>			";
                        }
                    }
                }
                
            } else {
                if (_fnToNull(vResult[i].EX_IM_TYPE) == "E") {
                    if (_fnToNull(vResult[i].REQ_SVC) == "SEA") {
                        vHtml += "	            <div class='track_stat yet'>			";
                    } else {
                        vHtml += "	            <div class='track_stat air yet'>			";
                    }
                } else {
                    if (_fnToNull(vResult[i].REQ_SVC) == "SEA") {
                        vHtml += "	            <div class='track_stat import yet'>			";
                    } else {
                        vHtml += "	            <div class='track_stat import air yet'>			";
                    }
                }
            }
            vHtml += "	                <div class='track_process'>			";
            vHtml += "	                    <div class='track_inner'>			";
            vHtml += "                          <div class='track title'>";
            vHtml += "	                            <div class='track_cell'>			";
            vHtml += "	                                <p class='track_event_nm'>" + _fnToNull(vResult[i].EVENT_NM) + "</p>			";
            vHtml += "	                            </div>			";
            vHtml += "	                        </div>			";
            vHtml += "	                        <div class='track img'>			";
            vHtml += "	                            <div class='track_cell'>			";
            vHtml += "	                            </div>			";
            vHtml += "	                        </div>			";
            vHtml += "	                        <div class='track loc'>			";
            vHtml += "	                            <div class='track_cell'>			";
            vHtml += "	                                <p class='title'>LOCATION</p>			";
            vHtml += "	                                <p>" + _fnToNull(vResult[i].ACT_LOC_NM) + "</p>			";
            vHtml += "	                            </div>			";
            vHtml += "	                        </div>			";
            vHtml += "	                        <div class='track dnt'>			";
            vHtml += "	                            <div class='track_cell'>			";
            vHtml += "	                                <p class='title'>DATE AND TIME</p>			";
            vHtml += "	                                <p>" + String(_fnToNull(vResult[i]["ACT_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " " + _fnFormatTime(_fnToNull(vResult[i]["ACT_HM"]).substring("0", "4")) + "</p>			";
            vHtml += "	                            </div>			";
            vHtml += "	                        </div>			";
            //vHtml += "	                        <div class='track etc'>			";
            //vHtml += "	                            <div class='track_cell'>			";
            //vHtml += "	                            </div>			";
            //vHtml += "	                        </div>			";
            vHtml += "	                    </div>			";
            vHtml += "	                </div>			";
            vHtml += "	            </div>			";
            });
            vHtml += "	            </div>			";
            vHtml += "	        </div>			";
            vHtml += "	    </div>			";


            $(".trk_list").append(vHtml);
            $(".btn_listOpen").click();
            layerPopup("#trackingList");
        }

        //$.ajax({
        //    type: "POST",
        //    url: "/Sub/fnGetTracking",
        //    async: false,
        //    dataType: "json",
        //    data: { "vJsonData": _fnMakeJson(objJsonData) },
        //    success: function (result) {

        //        if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
        //            $("#layer_trk_list").empty();
        //            fnMakeLayerTrkData(JSON.parse(result));
        //            layerPopup('#trackingList');
        //        }
        //        else if (JSON.parse(result).Result[0]["trxCode"] != "Y") {
        //            _fnAlertMsg("화물 추적 할 수 없는 데이터 입니다.");
        //        }
                
        //    }, error: function (xhr, status, error) {
        //        _fnAlertMsg("담당자에게 문의 하세요.");
        //        console.log(error);
        //    }
        //});

    }
    catch (err) {
        console.log("[Error - fnGetLayerTrkData]" + err.message);
    }
}

//////////////////////function makelist////////////////////////
//트레킹 데이터 그리기
function fnMakeLayerTrkData(rtnJson) {
    try {
        var apdVal = "";
        var rtnTbl = rtnJson.Table1;

        var vChkOn = false;

        if (_fnToNull(rtnTbl) != "") {
            $(rtnTbl).each(function (i) {

                if (rtnTbl[i].EX_IM_TYPE == "E") { //수출
                    if (_fnToNull(rtnTbl[i].SEQ) == _fnToNull(rtnTbl[i].ROW_COUNT)) {
                        //apdVal += "	<div class='track_stat on import'> ";
                        if (_fnToNull(rtnTbl[i].SEQ) == _fnToNull(rtnTbl[i].MAX_SEQ)) {
                            apdVal += "	<div class='track_stat on last export'> ";
                        } else {
                            apdVal += "	<div class='track_stat on export'> ";
                        }
                        vChkOn = true

                    } else {
                        if (vChkOn) {
                            if (_fnToNull(rtnTbl[i].ROW_COUNT) == _fnToNull(rtnTbl[i].MAX_SEQ)) {
                                apdVal += "	<div class='track_stat last export'> ";
                            } else {
                                apdVal += "	<div class='track_stat yet export'> ";
                            }
                        } else {
                            apdVal += "	<div class='track_stat export'> ";
                        }
                    }
                    apdVal += "            <div class='track_proc'> ";
                    apdVal += "                <div class='track_cell'> ";
                    apdVal += "                    <p>" + _fnToNull(rtnTbl[i].EVENT_NM) + "</p> ";
                    apdVal += "                </div> ";
                    apdVal += "            </div> ";
                    apdVal += "            <div class='track_process'> ";
                    apdVal += "                <div class='track_inner'> ";
                    apdVal += "                    <div class='track img'> ";
                    apdVal += "                        <div class='track_cell'> ";
                    apdVal += "                        </div> ";
                    apdVal += "                    </div> ";
                    apdVal += "                    <div class='track loc'> ";
                    apdVal += "                        <div class='track_cell'> ";
                    apdVal += "                            <p class='title'>LOCATION</p> ";
                    apdVal += "                            <p>" + _fnToNull(rtnTbl[i].ACT_LOC_NM) + "</p> ";
                    apdVal += "                        </div> ";
                    apdVal += "                    </div> ";
                    apdVal += "                    <div class='track dnt'> ";
                    apdVal += "                        <div class='track_cell'> ";
                    apdVal += "                            <p class='title'>DATE AND TIME</p> ";
                    apdVal += "                            <p>" + _fnDateFormat(_fnToNull(rtnTbl[i].ACT_YMD)) + "<br />" + _fnDateFormat(_fnToNull(rtnTbl[i].ACT_HM)) + "</p> ";
                    apdVal += "                        </div> ";
                    apdVal += "                    </div> ";
                    apdVal += "                    <div class='track etc'> ";
                    apdVal += "                        <div class='track_cell'> ";
                    //if (i == 2) {
                    //    apdVal += "                        <button type='button' class='btn_track' name='layerTerminal' id='layerTerminal'>터미널 정보</button> ";
                    //    apdVal += "                       <p style='display:none'>" + _fnToNull(rtnTbl[i].VSL) + "</p> ";
                    //} else if (i == 3) {
                    //    apdVal += "                       <button type='button' class='btn_track' name='layerLocation' id='layerLocation'>실시간 위치정보</button> ";
                    //    apdVal += "                            <p style='display:none'> </p> ";
                    //    apdVal += "                            <p style='display:none'>" + _fnToNull(rtnTbl[i].HBL_NO) + "</p> ";
                    //    //apdVal += "                            <p style='display:none'>" + _fnToNull(rtnTbl[i].VSL) + "</p> ";
                    //    //apdVal += "                            <p style='display:none'>" + _fnToNull(rtnTbl[i].POL_CD) + "</p> ";
                    //    //apdVal += "                            <p style='display:none'>" + _fnToNull(rtnTbl[i].POD_CD) + "</p> ";
                    //} else if (i == 4) {
                    //    apdVal += "                       <button type='button' class='btn_track' name='layerUnipass' id='layerUnipass' >UNIPASS</button>";
                    //    apdVal += "                       <p style='display:none'>" + _fnToNull(rtnTbl[i].HBL_NO) + "</p> ";
                    //    apdVal += "                       <p style='display:none'>" + _fnToNull(rtnTbl[i].ETD) + "</p> ";
                    //}
                    apdVal += "                        </div> ";
                    apdVal += "                    </div> ";
                    apdVal += "                </div> ";
                    apdVal += "            </div> ";
                    apdVal += "        </div>";
                }
                else if (rtnTbl[i].EX_IM_TYPE == "I") { //수입
                    if (_fnToNull(rtnTbl[i].SEQ) == _fnToNull(rtnTbl[i].ROW_COUNT)) {
                        //apdVal += "	<div class='track_stat on import'> ";
                        if (_fnToNull(rtnTbl[i].SEQ) == _fnToNull(rtnTbl[i].MAX_SEQ)) {
                            apdVal += "	<div class='track_stat on last import'> ";
                        } else {
                            apdVal += "	<div class='track_stat on import'> ";
                        }
                        vChkOn = true

                    } else {
                        if (vChkOn) {
                            if (_fnToNull(rtnTbl[i].ROW_COUNT) == _fnToNull(rtnTbl[i].MAX_SEQ)) {
                                apdVal += "	<div class='track_stat last import'> ";
                            } else {
                                apdVal += "	<div class='track_stat yet import'> ";
                            }
                        } else {
                            apdVal += "	<div class='track_stat import'> ";
                        }
                    }
                    //if (_fnToNull(rtnTbl[i].SEQ) == (i + 3)) {
                    //    //apdVal += "	<div class='track_stat on import'> ";
                    //    if (rtnTbl.length == (i + 1)) {
                    //        apdVal += "	<div class='track_stat on last import'> ";
                    //    } else {
                    //        apdVal += "	<div class='track_stat on import'> ";
                    //    }
                    //    vChkOn = true
                    //
                    //} else {
                    //    if (vChkOn) {
                    //        if (rtnTbl.length == (i + 1)) {
                    //            apdVal += "	<div class='track_stat last import'> ";
                    //        } else {
                    //            apdVal += "	<div class='track_stat yet import'> ";
                    //        }
                    //    } else {
                    //        apdVal += "	<div class='track_stat import'> ";
                    //    }
                    //}

                    apdVal += "            <div class='track_proc'> ";
                    apdVal += "                <div class='track_cell'> ";
                    apdVal += "                    <p>" + _fnToNull(rtnTbl[i].EVENT_NM) + "</p> ";
                    apdVal += "                </div> ";
                    apdVal += "            </div> ";
                    apdVal += "            <div class='track_process'> ";
                    apdVal += "                <div class='track_inner'> ";
                    apdVal += "                    <div class='track img'> ";
                    apdVal += "                        <div class='track_cell'> ";
                    apdVal += "                        </div> ";
                    apdVal += "                    </div> ";
                    apdVal += "                    <div class='track loc'> ";
                    apdVal += "                        <div class='track_cell'> ";
                    apdVal += "                            <p class='title'>LOCATION</p> ";
                    apdVal += "                            <p>" + _fnToNull(rtnTbl[i].ACT_LOC_NM) + "</p> ";
                    apdVal += "                        </div> ";
                    apdVal += "                    </div> ";
                    apdVal += "                    <div class='track dnt'> ";
                    apdVal += "                        <div class='track_cell'> ";
                    apdVal += "                            <p class='title'>DATE AND TIME</p> ";
                    apdVal += "                            <p>" + _fnDateFormat(_fnToNull(rtnTbl[i].ACT_YMD)) + "<br />" + _fnDateFormat(_fnToNull(rtnTbl[i].ACT_HM)) + "</p> ";
                    apdVal += "                        </div> ";
                    apdVal += "                    </div> ";
                    apdVal += "                    <div class='track etc'> ";
                    apdVal += "                        <div class='track_cell'> ";
                    //if (i == 3) {
                    //    apdVal += "                        <button type='button' class='btn_track' name='layerTerminal'id='layerTerminal'>터미널 정보</button> ";
                    //    apdVal += "                       <p style='display:none'>" + _fnToNull(rtnTbl[i].VSL) + "</p> ";
                    //} else if (i == 0) {
                    //    apdVal += "                       <button type='button' class='btn_track' name='layerLocation'id='layerLocation'>실시간 위치정보</button> ";
                    //    apdVal += "                            <p style='display:none'> </p> ";
                    //    apdVal += "                            <p style='display:none'>" + _fnToNull(rtnTbl[i].HBL_NO) + "</p> ";
                    //    //apdVal += "                            <p style='display:none'>" + _fnToNull(rtnTbl[i].VSL) + "</p> ";
                    //    //apdVal += "                            <p style='display:none'>" + _fnToNull(rtnTbl[i].POL_CD) + "</p> ";
                    //    //apdVal += "                            <p style='display:none'>" + _fnToNull(rtnTbl[i].POD_CD) + "</p> ";
                    //} else if (i == 2) {
                    //    apdVal += "                       <button type='button' class='btn_track' name='layerUnipass' id='layerUnipass' >UNIPASS</button>";
                    //    apdVal += "                       <p style='display:none'>" + _fnToNull(rtnTbl[i].HBL_NO) + "</p> ";
                    //    apdVal += "                       <p style='display:none'>" + _fnToNull(rtnTbl[i].ETD) + "</p> ";
                    //}
                    apdVal += "                        </div> ";
                    apdVal += "                    </div> ";
                    apdVal += "                </div> ";
                    apdVal += "            </div> ";
                    apdVal += "        </div>";
                }
            });
            $("#layer_trk_list").append(apdVal);
            //drawingLayerNodata();
        } else {
            apdVal += "	<div class='client' id='no_data'> ";
            apdVal += "        <div class='no_data'> ";
            apdVal += "            <div class='no_sorry'> ";
            apdVal += "                <h3> ";
            apdVal += "                    Sorry<br /> ";
            apdVal += "                    <span>검색 결과가 없습니다.</span> ";
            apdVal += "                </h3> ";
            apdVal += "            </div> ";
            apdVal += "        </div> ";
            apdVal += "    </div>";
            $("#layer_trk_list").append(apdVal);
        }
    }
    catch (err) {
        console.log("[Error - fnMakeLayerTrkData]" + err.message);
    }
}
/////////////////////////////API///////////////////////////////


