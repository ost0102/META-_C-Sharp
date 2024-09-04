////////////////////전역 변수//////////////////////////
//var _objJsonData = new Object();
var _vChkID;
var _vChkForCrn;
var url = "Main"
////////////////////jquery event///////////////////////
$(function () {

    //다음, 이전 버튼
    $(".btns.next").on("click", function () {
        if ($('.join-status__info.status1').hasClass('on')) {
            $('.join-status__info.status1').removeClass('on');
            $('.join-status__info.status2').addClass('on');
            $(".join-in__box.first").hide();
            /*$(".join-in__box.second.execution").show();*/
            $(".join-in__box.second.shipper").show();
        } else if ($('.join-status__info.status2').hasClass('on')) {
            $('.join-status__info.status2').removeClass('on');
            $('.join-status__info.status3').addClass('on');
            /*$(".join-in__box.second.execution").hide();*/
            $(".join-in__box.second.shipper").hide();
            $(".join-in__box.third").show();
        } else if ($('.join-status__info.status3').hasClass('on')) {
            $('.join-status__info.status3').removeClass('on');
            $('.join-status__info.status4').addClass('on');
            $(".join-in__box.third").hide();
            $(".join-in__box.fourth").show();
        }
    })
    $(".btns.prev").on("click", function () {
        if ($('.join-status__info.status4').hasClass('on')) {
            $('.join-status__info.status4').removeClass('on');
            $('.join-status__info.status3').addClass('on');
            $(".join-in__box.third").show();
            $(".join-in__box.fourth").hide();
        } else if ($('.join-status__info.status3').hasClass('on')) {
            $('.join-status__info.status3').removeClass('on');
            $('.join-status__info.status2').addClass('on');
            $(".join-in__box.third").hide();
            //$(".join-in__box.second.execution").show();
            $(".join-in__box.second.shipper").show();
        } else if ($('.join-status__info.status2').hasClass('on')) {
            $('.join-status__info.status2').removeClass('on');
            $('.join-status__info.status1').addClass('on');
            //$(".join-in__box.second.execution").hide();
            $(".join-in__box.second.shipper").hide();
            $(".join-in__box.first").show();
        }
    })



    //-찍어서 자동으로 나오게 세팅
    $("#Elvis_HP_NO").keyup(function (e) {
        $(this).val($(this).val().replace(/[^0-9]/gi, ""));
        $(this).val(_fnMakePhoneForm($(this).val()));
    });
});

$("input").focusout(function (e) {

    var $this = $(e.target);
    var vValue = "";

    if ($this.attr('id') == "RES_EMAIL") {
        vValue = $("#RES_EMAIL").val();

        if (vValue == "") {
            fnShowWarning("RES_EMAIL", "Email_Warning", "#f44336");
        } else if (_vEmail_Check == "true") {
            fnCheckID_RealTime($("#RES_EMAIL").val().trim());
        }
    }
    else if ($this.attr('id') == "RES_PWD") {
        vValue = $("#RES_PWD").val();

        if (vValue == "") {
            fnShowWarning("RES_PWD", "PW1_Empty", "#f44336");
        }
    }
    else if ($this.attr('id') == "RES_PWD2") {
        vValue = $("#RES_PWD2").val();

        if (vValue == "") {
            fnShowWarning("RES_PWD2", "PW2_Empty", "#f44336");
        }
    }
    else if ($this.attr('id') == "RES_NAME") {
        vValue = $("#RES_NAME").val();

        if (vValue == "") {
            fnShowWarning("RES_NAME", "NAME_Empty", "#f44336");
        }
    }
    else if ($this.attr('id') == "RES_TEL") {
        vValue = $("#RES_TEL").val();

        if (vValue == "") {
            fnShowWarning("RES_TEL", "TEL_Empty", "#f44336");
        }
    }
    else if ($this.attr('id') == "CUST_NM") {
        vValue = $("#CUST_NM").val();

        if (vValue == "") {
            fnShowWarning("CUST_NM", "CUST_EMPTY", "#f44336");
        }
    }
    else if ($this.attr('id') == "RES_CRN") {
        vValue = $("#RES_CRN").val();

        if (vValue == "") {
            fnShowWarning("RES_CRN", "CRN_NO", "#f44336");
        }
    }
});

//input 실시간 - Validation
$("input").keyup(function (e) {
    try {
        var $this = $(e.target);
        var vValue = "";
        var vCompare = "";

        //Input => E-mail
        if ($this.attr('id') == "RES_EMAIL") {

            vValue = $("#RES_EMAIL").val().trim();
            //Color Error => #f44336 / Success => #4caf50

            //특수문자 체크
            if (fnCheckSC(vValue)) {
                fnShowWarning("RES_EMAIL", "Email_Warning", "#4caf50");
                fnOnOffWarning("Email_SCWarning", "false");
            } else {
                fnShowWarning("RES_EMAIL", "Email_Warning", "#f44336");
            }

            //공백 값 체크
            if (vValue == "") {
                fnShowWarning("RES_EMAIL", "Email_Empty", "#f44336");
                fnOnOffWarning("Email_Warning", "false");
                fnOnOffWarning("Email_CheckID", "false");
                fnOnOffWarning("Email_SCWarning", "false");
            }
            else {
                //fnShowWarning("RES_EMAIL", "Email_Empty", "#4caf50");
                fnOnOffWarning("Email_Empty", "false");
                fnOnOffWarning("Email_CheckID", "false");
                fnOnOffWarning("Email_SCWarning", "false");
            }

            var regExp = /^[!$^()-_0-9a-zA-Z!$^()-_]([-_.]?[!$^()-_0-9a-zA-Z!$^()-_])*@[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*/i;
            if (vValue != "") {

                var vMatch = "true";
                var vSC = "true";

                //null이면 경고 , null이 아니면 가능
                if (vValue.match(regExp) == null) {
                    fnShowWarning("RES_EMAIL", "Email_Warn#ng", "#f44336");
                    vMatch = "false";
                    _vEmail_Check = "false";
                } else {
                    fnShowWarning("RES_EMAIL", "Email_Warning", "#4caf50");
                    _vEmail_Check = "true";
                }

                //true면 경고 false면 가능
                if (fnCheckSC(vValue)) {
                    fnShowWarning("RES_EMAIL", "Email_SCWarning", "#f44336");
                    vSC = "false";
                } else {
                    fnShowWarning("RES_EMAIL", "Email_SCWarning", "#4caf50");
                }

                if (vMatch == "false" || vSC == "false") {
                    fnShowWarning("RES_EMAIL", "Email_Warning", "#f44336");
                } else {
                    fnShowWarning("RES_EMAIL", "Email_SCWarning", "#4caf50");
                }
            }


            //Check Warning - true false
            if ($("#Email_Empty").css("display") == "inline-block" || $("#Email_Warning").css("display") == "inline-block" || $("#Email_CheckID").css("display") == "inline-block") {
                $("#Email_Hidden").val("false");
            } else {
                $("#Email_Hidden").val("true");
            }

            if (e.keyCode == 13) {
                $("#RES_PWD").focus();
            }

        } //Res_Email End
        //RES_PWD start
        else if ($this.attr('id') == "RES_PWD") {

            vValue = $("#RES_PWD").val();
            vCompare = $("#RES_PWD2").val();

            var vBoolean_LessSix = "false";
            var vBoolean_Regular = "false";
            var vBoolean_Different = "false";

            //값 없을 시 경고메시지
            if (vValue == "") {
                fnShowWarning("RES_PWD", "PW1_Empty", "#f44336");
                fnOnOffWarning("PW1_OverSix", "false");
            }
            else {
                fnOnOffWarning("PW1_Empty", "false");

                if (vValue.length < 6) {
                    vBoolean_LessSix = "false";
                    fnOnOffWarning("PW1_OverSix", "true");
                } else {
                    vBoolean_LessSix = "true";
                    fnOnOffWarning("PW1_OverSix", "false");
                }

                //대문자 소문자 체크
                var vCheck = /^(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9])(?=.*[0-9]).{6,17}$/;
                if (!vCheck.test(vValue)) {
                    vBoolean_Regular = "false";
                    fnOnOffWarning("PW1_Regular", "true");
                } else {
                    vBoolean_Regular = "true";
                    fnOnOffWarning("PW1_Regular", "false");
                }
                //비교문 체크
                vBoolean_Different = fnPwCompare(vValue, vCompare);
            }

            //마지막 체크
            if (vBoolean_LessSix == "false" || vBoolean_Regular == "false" || vBoolean_Different == "false") {
                fnWarningBorder("RES_PWD", "#f44336");
                if (vBoolean_Different == "false" && $("#RES_PWD2").val() != "") {
                    fnWarningBorder("RES_PWD2", "#f44336");
                }
            } else {
                fnWarningBorder("RES_PWD", "#4caf50");
                //만약 비밀번호 확인이 오류면 변경 해주는 로직. (확인이 필요함)
                if ($("#RES_PWD2").css("border-top-color") == "rgb(244, 67, 54)") {
                    if ($("#PW2_Empty").css("display") != "inline-block" && $("#PW2_Compare").css("display") != "inline-block") {
                        $("#RES_PWD2").css("border-color", "#4caf50");
                    }
                }
            }

            //Check Warning - true false
            if ($("#PW1_Empty").css("display") == "inline-block" || $("#PW1_OverSix").css("display") == "inline-block" || $("#PW1_Regular").css("display") == "inline-block") {
                if ($("#PW2_Empty").css("display") == "inline-block" || $("#PW2_OverSix").css("display") == "inline-block" || $("#PW2_Compare").css("display") == "inline-block" || $("#PW2_Regular").css("display") == "inline-block") {
                    $("#PW1_Hidden").val("true");
                } else {
                    $("#PW1_Hidden").val("false");
                }
            } else {
                $("#PW1_Hidden").val("true");
                $("#PW2_Hidden").val("true");
            }
            if (e.keyCode == 13) {
                $("#RES_PWD2").focus();
            }
        } //RES_PWD end
        //RES_PWD2 start 
        else if ($this.attr('id') == "RES_PWD2") {

            vValue = $("#RES_PWD2").val();
            vCompare = $("#RES_PWD").val();

            var vBoolean_LessSix = "false";
            var vBoolean_Regular = "false";
            var vBoolean_Different = "false";

            //값 없을 시 경고메시지
            if (vValue == "") {
                fnShowWarning("RES_PWD2", "PW2_Empty", "#f44336");
                fnOnOffWarning("PW2_OverSix", "false");
            }
            else {
                fnOnOffWarning("PW2_Empty", "false");

                if (vValue.length < 6) {
                    vBoolean_LessSix = "false";
                    fnOnOffWarning("PW2_OverSix", "true");
                } else {
                    vBoolean_LessSix = "true";
                    fnOnOffWarning("PW2_OverSix", "false");
                }

                //대문자 소문자 체크
                var vCheck = /^(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9])(?=.*[0-9]).{6,17}$/;
                if (!vCheck.test(vValue)) {
                    vBoolean_Regular = "false";
                    fnOnOffWarning("PW2_Regular", "true");
                } else {
                    vBoolean_Regular = "true";
                    fnOnOffWarning("PW2_Regular", "false");
                }
                //비교문 체크
                vBoolean_Different = fnPwCompare(vValue, vCompare);
            }

            //마지막 체크
            if (vBoolean_LessSix == "false" || vBoolean_Regular == "false" || vBoolean_Different == "false") {
                fnWarningBorder("RES_PWD2", "#f44336");
                if (vBoolean_Different == "false" && $("#RES_PWD").val() != "") {
                    fnWarningBorder("RES_PWD", "#f44336");
                }
            } else {
                fnWarningBorder("RES_PWD2", "#4caf50");
                //만약 비밀번호 확인이 오류면 변경 해주는 로직. (확인이 필요함)
                if ($("#RES_PWD").css("border-top-color") == "rgb(244, 67, 54)") {
                    if ($("#PW1_Empty").css("display") != "inline-block") {
                        $("#RES_PWD").css("border-color", "#4caf50");
                    }
                }
            }

            //Check Warning - true false
            if ($("#PW2_Empty").css("display") == "inline-block" || $("#PW2_OverSix").css("display") == "inline-block" || $("#PW2_Compare").css("display") == "inline-block" || $("#PW2_Regular").css("display") == "inline-block") {
                $("#PW2_Hidden").val("false");
            } else {
                $("#PW2_Hidden").val("true");
            }
            if (e.keyCode == 13) {
                $("#RES_NAME").focus();
            }
        } //RES_PWD2 end
        else if ($this.attr('id') == "RES_NAME") {
            vValue = $("#RES_NAME").val();
            //var check = /[ㄱ-ㅎ|ㅏ-ㅣ]/; //한글 자음 체크
            var regExp = /[\{\}\[\]\/?.,;:|\)*~`!^\-_+<>@\#$%&\\\=\(\'\"]/gi;
            if (regExp.test(vValue)) {
                $("#RES_NAME").val(vValue.replace(regExp, ""));
            }
            //데이터 없을 때
            if (vValue == "") {
                fnShowWarning("RES_NAME", "NAME_Empty", "#f44336");
                fnOnOffWarning("NAME_OverTwo", "false");
            } else {
                fnShowWarning("RES_NAME", "NAME_Empty", "#4caf50");
            }

            //2개 이상
            if (vValue != "") {
                if (vValue.length < 2) {
                    fnShowWarning("RES_NAME", "NAME_OverTwo", "#f44336");
                } else {
                    fnShowWarning("RES_NAME", "NAME_OverTwo", "#4caf50");
                }
            }

            //Check Warning - true false
            if ($("#NAME_Empty").css("display") == "inline-block" || $("#NAME_OverTwo").css("display") == "inline-block" || $("#NAME_CheckKorean").css("display") == "inline-block") {
                $("#NAME_Hidden").val("false");
            } else {
                $("#NAME_Hidden").val("true");
            }
            if (e.keyCode == 13) {
                $("#RES_TEL").focus();
            }
        } else if ($this.attr('id') == "CUST_NM") {
            vValue = $("#CUST_NM").val();
            //var check = /[ㄱ-ㅎ|ㅏ-ㅣ]/; //한글 자음 체크
            //                var regExp = /[\{\}\[\]\/?.,;:|\)*~`!^\-_+<>@\#$%&\\\=\(\'\"]/gi;
            //                if (regExp.test(vValue)) {
            //                    $("#CUST_NM").val(vValue.replace(regExp, ""));
            //                }
            //데이터 없을 때
            if (vValue == "") {
                fnShowWarning("CUST_NM", "CUST_Empty", "#f44336");
            } else {
                fnShowWarning("CUST_NM", "CUST_Empty", "#4caf50");
            }


            //Check Warning - true false
            if ($("#CUST_Empty").css("display") == "inline-block") {
                $("#CustNAME_Hidden").val("false");
            } else {
                $("#CustNAME_Hidden").val("true");
            }
            if (e.keyCode == 13) {
                $("#RES_CRN").focus();
            }

        } //RES_NAME end
        //RES_TEL Strat
        else if ($this.attr('id') == "RES_TEL") {

            vValue = $("#RES_TEL").val().trim();
            var vKorCheck = /[ㄱ-ㅎ|ㅏ-ㅣ]/; //한글 자음 체크
            var vEngCheck = /[a-z | A-Z]/;

            //Phone 하이픈 넣기
            if (vValue != "") {
                $(this).val(_fnMakePhoneForm(vValue));
            }

            //값이 없을 때
            if (vValue == "") {
                fnShowWarning("RES_TEL", "TEL_Empty", "#f44336");
                fnOnOffWarning("TEL_NotNumber", "false");
            } else {
                fnShowWarning("RES_TEL", "TEL_Empty", "#4caf50");
            }

            //숫자가 아닐 때
            if (vKorCheck.test(vValue) || vEngCheck.test(vValue)) {
                fnShowWarning("RES_TEL", "TEL_NotNumber", "#f44336");
            } else {
                fnOnOffWarning("TEL_NotNumber", "false");
            }

            //Check Warning - true false
            if ($("#TEL_Empty").css("display") == "inline-block" || $("#TEL_NotNumber").css("display") == "inline-block") {
                $("#TEL_Hidden").val("false");
            } else {
                $("#TEL_Hidden").val("true");
            }
            if (e.keyCode == 13) {
                $("#CUST_NM").focus();
            }
        } //RES_TEL end
        //RES_CRN Start
        
        else if ($this.attr('id') == "RES_CRN") {

            vValue = $("#RES_CRN").val().trim();

            ////                //사업자 번호가 맞는지 체크
            if (vValue.replace(/[^0-9]/gi, '').length == 10 && vValue.length == 10) {
                    fnShowWarning("RES_CRN", "CRN_NO", "#4caf50");
            } else {
                //_fnAlertMsg("잘못 된 사업자 번호를 입력 하셨습니다.");
                fnShowWarning("RES_CRN", "CRN_NO", "#f44336");

                if (vValue.length == 0) {
                    fnOnOffWarning("CRN_NO", "true");
                } else {
                    fnOnOffWarning("CRN_NO", "false");
                }
                
            }
            if ($("#CRN_NO").css("display") == "inline-block") {
                $("#CRN_Hidden").val("false");
            } else {
                $("#CRN_Hidden").val("true");
            }

        } //RES_CRN end    
        else if ($this.attr('id') == "RES_BIZCOND") {
            if (e.keyCode == 13) {
                $("#RES_BIZTYPE").focus();
            }
        } //RES_BIZCOND end    
        else if ($this.attr('id') == "RES_BIZTYPE") {
            if (e.keyCode == 13) {
                $("#RES_CEO").focus();
            }
        } //RES_BIZCOND end    
        else if ($this.attr('id') == "RES_CEO") {
            if (e.keyCode == 13) {
                $("#RES_CUST_TEL").focus();
            }
        } //RES_BIZCOND end    
        else if ($this.attr('id') == "RES_CUST_TEL") {
            if (e.keyCode == 13) {
                $("#RES_ENG_NM").focus();
            }
        } //RES_BIZCOND end    
        else if ($this.attr('id') == "RES_ENG_NM") {
            if (e.keyCode == 13) {
                $("#RES_ENG_ADDR").focus();
            }
        } //RES_BIZCOND end    
        else if ($this.attr('id') == "RES_ENG_ADDR") {
            if (e.keyCode == 13) {
                $("#RES_CTRY_CD").focus();
            }
        } //RES_BIZCOND end    
    } catch (err) {
        console.log(err.message);
    }

});

$("#RES_TEL").keyup(function (event) {
    if (!(event.keyCode >= 37 && event.keyCode <= 40)) {
        var inputVal = $(this).val();
        $(this).val(inputVal.replace(/[^0-9 | -]/gi, ''));
    }
});

$("#RES_CRN").keyup(function (event) {
    if (!(event.keyCode >= 37 && event.keyCode <= 40)) {
        var inputVal = $(this).val();
        $(this).val(inputVal.replace(/[^0-9]/gi, ''));
    }
});


//실시간 ID 중복 체크
function fnCheckID_RealTime(value) {
    try {
        var objCHECK_INFO = new Object();
        objCHECK_INFO.USR_ID = value;

        if (value.lastIndexOf(".") + 1 == value.length) {
            return;
        }
        else {
            var rtnVal = _fnGetAjaxData("POST", url, "ChkEmailApi", objCHECK_INFO);


            if (JSON.parse(rtnVal).Result[0].trxCode == "N") {
                return;
            } else if (JSON.parse(rtnVal).Result[0].trxCode == "Y") {
                if (JSON.parse(rtnVal).Table1[0].APV_YN == "D") {
                    $("#Email_CheckID").hide();
                    fnShowWarning("RES_EMAIL", "Email_CheckID", "#f44336");
                    $("#Email_Hidden").val("false");
                } else {
                    $("#Email_CheckID").hide();
                    fnShowWarning("RES_EMAIL", "Email_CheckID", "#f44336");
                    $("#Email_Hidden").val("false");
                }
                //        $("#RES_EMAIL").val("");
                //        $("#RES_EMAIL").focus();
            } else if (JSON.parse(rtnVal).Result[0].trxCode == "E") {
                _fnAlertMsg("회원 정보를 가지고 올 수 없습니다.");
                $("#Email_Hidden").val("false");
                return;
            }
        }
    } catch (err) {
        console.log(err.message);
    }
}


//사업자 번호가 DB에 있는 것인지 여부 체크
function fnGetComCodeInfo(CRN) {
    try {
        var vResult = ""; //결과값

        //Json 데이터 한번 감싸기

        var objCHECK_INFO = new Object();
        objCHECK_INFO.CRN = CRN;
        objCHECK_INFO.DOMAIN = _vDOMAIN;

        var rtnVal = _fnGetAjaxData("POST", url, "ChkCRNApi", objCHECK_INFO);

        if (rtnVal.Result[0].trxCode == "Y") {

            $("#CUST_CD").val(rtnVal.Table[0].CUST_CD);
            $("#CUST_NM").val(rtnVal.Table[0].CUST_NM);
            $("#CUST_NM").attr("disabled", true);
            fnShowWarning("CUST_NM", "CUST_Empty", "#4caf50");

            vResult = true;
        } else if (rtnVal.Result[0].trxCode == "N") {
            //            _fnAlertMsg(rtnVal.Result[0].trxMsg);
            //            $("#RES_CRN").val("");
            vResult = false;
        }

        return vResult;
    } catch (err) {
        console.log(err.message);
    }
}

//회원가입 DB 통신
function fnUser_Register(vObj_User) {
    try {
        var rtnVal = _fnGetAjaxData("POST", url, "PostRegist", vObj_User);
        if (rtnVal.Result[0].trxCode == "Y") {
            $(".alert_cont .inner").html("회원가입 신청이 완료 되었습니다. <br />담당자의 승인 확인 후 사용 가능 합니다.");
            layerPopup('#alert01', "", false);
            $("#alert_close").focus();
            $('#alert_close').click(function () {
                location.href = window.location.origin;
            });
            var pushObj = new Object();
            pushObj.JOB_TYPE = "USR";
            pushObj.MSG = "회원가입 승인 해 주세요.";
            pushObj.REF1 = rtnVal.Table1[0].USR_ID;
            pushObj.REF2 = "";
            pushObj.REF3 = "";
            pushObj.REF4 = "";
            pushObj.REF5 = "";
            pushObj.USR_ID = rtnVal.Table1[0].USR_ID;

            Chathub_Push_Message(pushObj);
        }
        else if (rtnVal.Result[0].trxCode == "N") {
            _fnAlertMsg("회원가입이 되지 않았습니다. 관리자에게 문의 하세요.");
        } else if (rtnVal.Result[0].trxCode == "E") {
            _fnAlertMsg(rtnVal.Result[0].trxMsg);
        }
    } catch (err) {
        console.log(err.message);
    }
}

//Validation
//회원가입 폼 공백 확인
function fnCheckData(vObj_User) {
    try {
        //Pw Check 

        var vPW = $("#RES_PWD").val();
        var vPW2 = $("#RES_PWD2").val();
        //value
        var vEmail = $('#RES_EMAIL').val().trim();
        //    var vEmail_yn = $('input[name=email_yn]')[0].checked;
        var vPwd = $('#RES_PWD').val();
        var vName = $('#RES_NAME').val();
        var vTel = $('#RES_TEL').val();
        var vCustNm = $('#CUST_NM').val();
        var vCrn = $('#RES_CRN').val();
        var vCrnFile = $('#Elvis_Crn_FileName').val();
        var vOfficeAddr = $('#RES_OFFICE_ADDR').val();

        if (vEmail == "") {
            fnShowWarning("RES_EMAIL", "Email_Empty", "#f44336");
        }

        if (vPW == "") {
            fnShowWarning("RES_PWD", "PW1_Empty", "#f44336");
        }

        if (vPW2 == "") {
            fnShowWarning("RES_PWD2", "PW2_Empty", "#f44336");
        }

        if (vName == "") {
            fnShowWarning("RES_NAME", "NAME_Empty", "#f44336");
        }
        if (vTel == "") {
            fnShowWarning("RES_TEL", "TEL_Empty", "#f44336");
        }
        if (vCustNm == "") {
            fnShowWarning("CUST_NM", "CUST_Empty", "#f44336");
        }

        if (vCrnFile == "") {
            fnShowWarning("Elvis_Crn_FileName", "crnFile_empty", "#f44336");
        }

        if (vOfficeAddr == "") {
            fnShowWarning("RES_OFFICE_ADDR", "office_addr_empty", "#f44336");
        }
        if (vEmail == "" || vPW == "" || vPW2 == "" || vName == "" || vTel == "" || vCustNm == "" || vCrnFile == "" || vOfficeAddr == "") {
            return false;
        }
        vObj_User.USR_ID = vEmail;
        vObj_User.CRN = vCrn;
        vObj_User.LICENSE = vLicense;
        vObj_User.EMAIL = vEmail;
        vObj_User.EMAIL_YN = "Y";
        vObj_User.PSWD = vPwd;
        vObj_User.CHAR_PSWD = fnSetPwdHidden(vPwd);
        vObj_User.LOC_NM = vName;
        vObj_User.HP_NO = vTel;
        vObj_User.CUST_CD = $("#CUST_CD").val();
        vObj_User.CUST_NM = $("#CUST_NM").val();
        vObj_User.DOMAIN = _vDOMAIN;
        var rtnVal = _fnGetAjaxData("POST", url, "GetOfficeCode", vObj_User);
        if (rtnVal.Result[0].trxCode == "Y") {
            vObj_User.OFFICE_CD = rtnVal.Table1[0].OFFICE_CD;
        } else {
            vObj_User.OFFICE_CD = "";
        }
        //하드코딩
        vObj_User.USE_YN = "Y";
        vObj_User.USR_TYPE = ""; //화주

        return vObj_User;
    } catch (err) {
        console.log(err.message);
    }
}



//사업자 등록 번호 체크
function fnCheckCRN(value) {
    try {
        var vCrnCode = value;
        if (vCrnCode.length == 10) {
            //            if (fnCheckBizID(vCrnCode)) {
            //_fnAlertMsg("사업자 번호가 맞습니다.");
            return true;
            //            } else {
            //                //_fnAlertMsg("잘못된 사업자 번호를 입력하셨습니다.");            
            //            }
        } else {
            _fnAlertMsg("사업자 번호 자리수가 맞지 않습니다.");
            return false;
        }
    } catch (err) {
        console.log(err.message);
    }
}



//Warning 메시지 보여주는 부분
function fnShowWarning(InputID, SpanID, Color) {
    var vColor = Color;

    $("#" + InputID).css("border-color", Color);

    if (vColor == "#f44336") {
        $("#" + SpanID).show();
    }
    else if (vColor == "#4caf50") {
        $("#" + SpanID).hide();
    }
}

//Border를 초록 혹은 빨강으로 변경 시켜주는 함수
function fnWarningBorder(InputID, Color) {
    $("#" + InputID).css("border-color", Color);
}


//true => show / false => hide
function fnOnOffWarning(SpanID, IsCheck) {
    var vIsCheck = IsCheck;

    if (vIsCheck == "true") {
        $("#" + SpanID).show();
    }
    else if (vIsCheck == "false") {
        $("#" + SpanID).hide();
    }
}

//패스워드 & 패스워드 확인 비교
function fnPwCompare(value1, value2) {
    var vPw1 = value1;
    var vPw2 = value2;

    //1번 값이 둘다 다를 경우.
    //2번 값이 똑같을 경우.    

    if (vPw1 != "" && vPw2 != "") {
        if (vPw1 != vPw2) {
            fnOnOffWarning("PW2_Compare", "true");

            return "false";
        } else if (vPw1 == vPw2 && 5 < vPw2.length) {
            fnOnOffWarning("PW2_Compare", "false");
            return "true";
        }
    }
    return false;
}





//사업자등록번호 체크
function fnCheckBizID(bizID) {
    try {
        // bizID는 숫자만 10자리로 해서 문자열로 넘긴다.
        var checkID = new Array(1, 3, 7, 1, 3, 7, 1, 3, 5, 1);
        var tmpBizID, i, chkSum = 0, c2, remander;
        var result;

        bizID = bizID.replace(/-/gi, '');

        for (i = 0; i <= 7; i++) {
            chkSum += checkID[i] * bizID.charAt(i);
        }

        c2 = "0" + (checkID[8] * bizID.charAt(8));
        c2 = c2.substring(c2.length - 2, c2.length);
        chkSum += Math.floor(c2.charAt(0)) + Math.floor(c2.charAt(1));
        remander = (10 - (chkSum % 10)) % 10;

        if (Math.floor(bizID.charAt(9)) == remander) {
            result = true; // OK!

        } else {
            result = false;
        }
        return result;
    } catch (err) {
        console.log(err.message);
    }
}

//Check 경고 메시지가 있는지 없는지 검증
function fnIsWarningMSG() {
    var vEmail_Hidden = $("#Email_Hidden").val();
    var vPW1_Hidden = $("#PW1_Hidden").val();
    var vPW2_Hidden = $("#PW2_Hidden").val();
    var NAME_Hidden = $("#NAME_Hidden").val();
    var TEL_Hidden = $("#TEL_Hidden").val();
    var CustName_Hidden = $("#CustNAME_Hidden").val();
    var CRN_Hidden = $("#CRN_Hidden").val();
    //    var vCheck = $('input[name=iden_yn]')[0].checked;
    //    var vEmailCheck = $('input[name=email_yn]')[0].checked;
    //var COMPANY_Hidden = $("#COMPANY_Hidden").val();

    //    if (vEmail_Hidden == "true" && vPW1_Hidden == "true" && vPW2_Hidden == "true" && NAME_Hidden == "true" && TEL_Hidden == "true" && CRN_Hidden == "true") {
    if (vEmail_Hidden == "true" && vPW1_Hidden == "true" && vPW2_Hidden == "true" && NAME_Hidden == "true" && TEL_Hidden == "true" && CustName_Hidden == "true" && CRN_Hidden == "true") {
        return true;
    } else {
        return false;
    }
}

//특수 문자가 있는지 확인
function fnCheckSC(value) {

    var vValue = value;
    var vObj_SC = new Object();

    //특수문자 아스키 코드

    vObj_SC.Asterisk = String.fromCharCode("42"); // *
    vObj_SC.PercentSign = String.fromCharCode("37"); //%
    vObj_SC.Ampersand = String.fromCharCode("38"); //&
    vObj_SC.Plus = String.fromCharCode("43"); // +
    vObj_SC.BackSlash = String.fromCharCode("92");  //\
    vObj_SC.Colon = String.fromCharCode("58"); // :
    vObj_SC.Grave = String.fromCharCode("96"); // '
    vObj_SC.LAngle = String.fromCharCode("60"); // <
    vObj_SC.RAngle = String.fromCharCode("62"); // >
    vObj_SC.Slash = String.fromCharCode("47"); // /

    if (vValue.indexOf(vObj_SC.Asterisk) != -1 || vValue.indexOf(vObj_SC.PercentSign) != -1 || vValue.indexOf(vObj_SC.Ampersand) != -1 || vValue.indexOf(vObj_SC.Plus) != -1 || vValue.indexOf(vObj_SC.BackSlash) != -1 || vValue.indexOf(vObj_SC.Colon) != -1 || vValue.indexOf(vObj_SC.Grave) != -1 || vValue.indexOf(vObj_SC.LAngle) != -1 || vValue.indexOf(vObj_SC.RAngle) != -1 || vValue.indexOf(vObj_SC.Slash) != -1) {
        return true;
    } else {
        return false;
    }
}


//비밀번호 텍스트 hidden (비밀번호 정보 보여주기 위함 ) 
function fnSetPwdHidden(value) {

    var vValue = value;
    var vResult = vValue.substr(0, Math.floor(vValue.length / 5));
    for (var i = 0; i < vValue.length - Math.floor(vValue.length / 5); i++) {
        vResult += "*";
    }

    return vResult;
}


$(document).ready(function () {
    var fileTarget = $('.int_box #businessCopy');
    var fileTarget2 = $('.int_box #CompanyIntro');

    fileTarget.on('change', function () {  // 값이 변경되면
        if (window.FileReader) {  // modern browser
            var filename = $(this)[0].files[0].name;
        }
        else {  // old IE
            var filename = $(this).val().split('/').pop().split('\\').pop();  // 파일명만 추출
        }

        // 추출한 파일명 삽입
        $(this).siblings('.upload-name').val(filename);
    });

    fileTarget2.on('change', function () {  // 값이 변경되면
        if (window.FileReader) {  // modern browser
            var filename = $(this)[0].files[0].name;
        }
        else {  // old IE
            var filename = $(this).val().split('/').pop().split('\\').pop();  // 파일명만 추출
        }

        // 추출한 파일명 삽입
        $(this).siblings('.upload-name').val(filename);
    });
})

//비밀번호 1번 key 입력 이벤트
$(document).on("keyup", "#RES_PWD", function (e) {

    var vPw1 = $(this).val();
    var vPw2 = $("#RES_PWD2").val();

    //6개 이상
    if (vPw1 == "") {
        $("#Pw1_OverSix").hide();
    } else {
        if (vPw1.length < 6) {
            $("#Pw1_OverSix").show();
        } else {
            $("#Pw1_OverSix").hide();
        }
    }

    //6개 이상
    if (vPw1 == "") {
        $("#Pw2_Compare").hide();
    } else {
        if (vPw1 != "" && vPw2 != "") {
            if (vPw1 != vPw2) {
                //데이터를 입력 해 주세요.
                $("#Pw2_Compare").show();
            } else if (vPw1 == vPw2) {
                $("#Pw2_Compare").hide();
            }
        }
    }

});

//비밀번호 2번 key 입력 이벤트
$(document).on("keyup", "#RES_PWD2", function (e) {
    var vPw1 = $("#RES_PWD").val();
    var vPw2 = $(this).val();

    //6개 이상
    if (vPw2 == "") {
        $("#Pw2_OverSix").hide();
    } else {
        if (vPw2.length < 6) {
            $("#Pw2_OverSix").show();
        } else {
            $("#Pw2_OverSix").hide();
        }
    }

    if (vPw2 == "") {
        $("#Pw2_Compare").hide();
    } else {
        if (vPw1 != "" && vPw2 != "") {
            if (vPw1 != vPw2) {
                //데이터를 입력 해 주세요.
                $("#Pw2_Compare").show();
            } else if (vPw1 == vPw2) {
                $("#Pw2_Compare").hide();
            }
        }
    }
});

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

//아이디 key 체크
$(document).on("keyup", "#Elvis_ID", function (e) {
    if (e.keyCode != 13) {
        _vChkID = "";
    }
});

//포워더 사업자등록번호 key 체크
$(document).on("keyup", "#Elvis_ForCRN", function (e) {
    if (e.keyCode != 13) {
        _vChkForCrn = "";
    }
});

//주소 찾기 버튼 이벤트
$(document).on("click", "#Elvis_Find_Addr", function () {

    //기존 데이터 초기화
    $("#Elvis_Juso_keyword").val("");
    $("#Elvis_JusoDetail_roadAddrPart2").val("");
    $(".Elvis_JusoList_Area").hide();

    layerPopup('#Find_Address');
});


//기본주소를 입력 해 주세요. input 클릭 시 주소 찾기 팝업창 띄우기
$(document).on("click", "#Elvis_OFFICE_ADDR, #Elvis_OFFICE_CODE", function () {

    //기존 데이터 초기화
    $("#Elvis_Juso_keyword").val("");
    $("#Elvis_JusoDetail_roadAddrPart2").val("");
    $(".Elvis_JusoList_Area").hide();

    layerPopup('#Find_Address');
});

//상세건물 보기 이벤트
$(document).on("click", "#Elvis_JusoList_DataList tr td[name='detBdNmList']", function () {
    if (_fnToNull($(this).text().trim()) != "") {
        if ($(this).text() == "닫기") {
            $(this).text("상세건물 보기");
            $(this).parent().find("td").eq(1).find("p[name=detBdNmList_display]").hide();
        } else {
            $(this).text("닫기");
            $(this).parent().find("td").eq(1).find("p[name=detBdNmList_display]").show();
        }
    }
});

//주소 버튼 클릭 시 상세 주소 데이터 입력 창 띄우기
$(document).on("click", "#Elvis_JusoList_DataList tr td[name='roadAddr']", function () {

    //기존 검색창 hide and show
    $(".Elvis_JusoDetail_Area").show();
    $(".Elvis_JusoList_Area").hide();
    //초기화도 같이 해주자.

    //init
    $("#Elvis_JusoDetail_roadAddr1").text("");
    $("#Elvis_JusoDetail_roadAddr2").text("");
    $("#Elvis_JusoDetail_roadAddrPart1").val("");
    $("#Elvis_JusoDetail_zipNo").val("");

    //도로명 주소
    $("#Elvis_JusoDetail_roadAddr1").text($(this).find("input[name='roadAddrPart1']").val());

    //상세주소 입력
    $("#Elvis_JusoDetail_roadAddr2").text($(this).find("input[name='roadAddrPart2']").val());

    //우편번호 , 기본주소 값
    $("#Elvis_JusoDetail_roadAddrPart1").val($(this).find("input[name='roadAddrPart1']").val());
    $("#Elvis_JusoDetail_zipNo").val($(this).find("input[name='zipNo']").val());

});

//주소 찾기 - 상세주소입력 엔터키 이벤트
$(document).on("keyup", "#Elvis_JusoDetail_roadAddrPart2", function (i) {
    if (i.keyCode == 13) {
        fnSetAddrData();
    }
});

//주소 찾기 디테일 "주소입력"
$(document).on("click", "#Elvis_JusoDetail_BtnArea label", function () {
    fnSetAddrData();
});


//submit - 등록 버튼 이벤트
$(document).on("click", ".btn_Regester", function () {
    fnRegister();
});

//가입 완료 후 확인 버튼 이벤트
$(document).on("click", "#btn_RegComplete", function () {    
    location.href = window.location.origin;
});

////////////////////////function///////////////////////
//아이디 중복 확인
function fnChkID() {
    try {

        if (_fnToNull($("#Elvis_ID").val().trim()) == "") {
            _fnAlertMsg("아이디를 입력 해 주세요.", "Elvis_ID");
            return false;
        }

        var objJsonData = new Object();
        objJsonData.ID = $("#Elvis_ID").val();

        $.ajax({
            type: "POST",
            url: "/Main/fnCheckID",
            async: true,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                if (result == null) {
                    _fnAlertMsg("오류가 발생 하였습니다. 담당자에게 문의 해주세요.");
                } else {
                    if (JSON.parse(result).Result[0]["trxCode"] == "N") {
                        _fnAlertMsg("이미 사용중인 아이디 입니다.");
                    }
                    else if (JSON.parse(result).Result[0]["trxCode"] == "E") {
                        _fnAlertMsg("담당자에게 문의 하세요.");
                        console.log(JSON.parse(result).Result[0]["trxMsg"]);
                    } else if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                        _fnAlertMsg("가입 가능한 아이디 입니다.");
                        _vChkID = $("#Elvis_ID").val();
                    }
                }
            },
            error: function (xhr, status, error) {
                _fnAlertMsg("담당자에게 문의 하세요.");
                console.log(error);
                $("#ProgressBar_Loading").hide();
            },
            beforeSend: function () {
                $("#ProgressBar_Loading").show(); //프로그래스 바
            },
            complete: function () {
                $("#ProgressBar_Loading").hide(); //프로그래스 바
            }
        });
    }
    catch (e) {
        console.log("[Error - fnChkID]" + e.message);
    }
}

//사업자 번호 확인 함수
function fnChkForCrn() {
    try {
        var regex = /[^0-9]/g;
        if ($("#Elvis_ForCRN").val().length == 10) {

            if (regex.test(_fnToNull($("#Elvis_ForCRN").val()))) {
                _fnAlertMsg("숫자만 입력 해 주세요.");
                $("#Elvis_ForCRN").focus();
            }
            else {
                var objJsonData = new Object();
                objJsonData.CRN = $("#Elvis_ForCRN").val().replace(/-/gi, "-").trim();

                $.ajax({
                    type: "POST",
                    url: "/Main/fnCheckCRN",
                    async: true,
                    dataType: "json",
                    data: { "vJsonData": _fnMakeJson(objJsonData) },
                    success: function (result) {
                        if (result == null) {
                            _fnAlertMsg("오류가 발생 하였습니다. 담당자에게 문의 해주세요.");
                        } else {
                            if (JSON.parse(result).Result[0]["trxCode"] == "N") {
                                _fnAlertMsg("ELVIS-BIG을 사용하지 않는 사업자 번호입니다.");
                            }
                            else if (JSON.parse(result).Result[0]["trxCode"] == "E") {
                                _fnAlertMsg("담당자에게 문의 하세요.");
                                console.log(JSON.parse(result).Result[0]["trxMsg"]);
                            } else if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                                _fnAlertMsg("가입 가능한 사업자 번호 입니다.");
                                _vChkForCrn = $("#Elvis_ForCRN").val();
                            }
                        }
                    },
                    error: function (xhr, status, error) {
                        _fnAlertMsg("담당자에게 문의 하세요.");
                        console.log(error);
                        $("#ProgressBar_Loading").hide();
                    },
                    beforeSend: function () {
                        $("#ProgressBar_Loading").show(); //프로그래스 바
                    },
                    complete: function () {
                        $("#ProgressBar_Loading").hide(); //프로그래스 바
                    }
                });
            }
        } else {
            if ($("#Elvis_ForCRN").val().length < 10) {
                _fnAlertMsg("사업자 등록 번호 10자리를 입력 해 주세요.");
            }
        }
    }
    catch (e) {
        console.log("[Error - fnChkForCrn]"+e.message);
    }
}

//동의하고 가입하기 함수
function fnRegister() {
    try {
        if (fnValidation()) {

            if (!fnIsWarningMSG()) return false;

            var objJsonData = new Object();

            objJsonData.USR_ID = $("#RES_EMAIL").val();
            objJsonData.PSWD = $("#RES_PWD").val();
            objJsonData.USR_NM = $("#RES_NAME").val();
            objJsonData.HP_NO = $("#RES_TEL").val().replace(/-/gi, "");
            objJsonData.CRN = $("#RES_CRN").val().replace(/-/gi, "");
            objJsonData.LOC_NM = $("#CUST_NM").val();
            objJsonData.LOC_ADDR = $("#RES_OFFICE_ADDR").val();
            objJsonData.BIZCOND = $("#RES_BIZCOND").val();
            objJsonData.BIZTYPE = $("#RES_BIZTYPE").val();
            objJsonData.CEO = $("#RES_CEO").val();
            objJsonData.TEL_NO = $("#RES_CUST_TEL").val();
            objJsonData.ENG_NM = $("#RES_ENG_NM").val();
            objJsonData.ENG_ADDR = $("#RES_ENG_ADDR").val();
            objJsonData.CTRY_CD = $("#RES_CTRY_CD").val();

            //MNGT_NO만 넣어주기



            $.ajax({
                type: "POST",
                url: "/Main/fnRegisterApi",
                async: true,
                dataType: "json",
                data: { "vJsonData": _fnMakeJson(objJsonData) },
                success: function (result) {
                    if (result == null) {
                        _fnAlertMsg("오류가 발생 하였습니다. 담당자에게 문의 해주세요.");
                    } else {
                        if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                            uploadFile(JSON.parse(result).Table1[0]["MNGT_NO"]);
                            layerPopup("#layer_RegComplete");
                            //_fnAlertMsg("가입 신청이 완료 되었습니다.");
                        }
                        else if (JSON.parse(result).Result[0]["trxCode"] == "N") {
                            _fnAlertMsg(JSON.parse(result).Result[0]["trxMsg"]);
                            console.log(JSON.parse(result).Result[0]["trxMsg"]);
                        }
                        else if (JSON.parse(result).Result[0]["trxCode"] == "E") {
                            _fnAlertMsg("담당자에게 문의 하세요.");
                            console.log(JSON.parse(result).Result[0]["trxMsg"]);
                        }
                    }
                }, error: function (xhr, status, error) {
                    _fnAlertMsg("담당자에게 문의 하세요.");
                    console.log(error);
                    $("#ProgressBar_Loading").hide();
                },
                beforeSend: function () {
                    $("#ProgressBar_Loading").show(); //프로그래스 바
                },
                complete: function () {
                    $("#ProgressBar_Loading").hide(); //프로그래스 바
                }
            });


        }
    }
    catch (err) {
        console.log("[Error - fnRegister]" + err.message);
    }
}


function uploadFile(mngt_no) {
    var formData = new FormData();
    if ($("input[name=files]")[0].files.length > 0) {
        const files = $("input[name=files]")[0].files;
        var i = 0;

        formData.append("files", files[0]);
        formData.append("MNGT_NO", mngt_no);
        formData.append("CRN", $('#RES_CRN').val());

        var request = new XMLHttpRequest();
        request.open("POST", _ApiUrl + "api/UserData/LicenseUpload")
        request.setRequestHeader("Authorization-Type", "N");
        request.send(formData);
        request.onload = function (e) {
            if (this.status == 200) {
                console.log('response', this.statusText);
            }
        };
    }
}

//동의하고 가입하기 벨리데이션 체크
function fnValidation() {

    /********************************************필수 값 체크********************************************/
    //이메일 값 체크
    if (_fnToNull($("#RES_EMAIL").val().replace(/ /gi, "")) == "") {
        _fnAlertMsg("이메일을 입력 해 주세요.", "RES_EMAIL");
        return false;
    }

    //비밀번호 값 체크
    if (_fnToNull($("#RES_PWD").val()) == "") {
        _fnAlertMsg("비밀번호를 입력 해 주세요.", "RES_PWD");
        return false;
    }

    //비밀번호 확인 체크
    if (_fnToNull($("#RES_PWD2").val()) == "") {
        _fnAlertMsg("비밀번호를 입력 해 주세요.", "RES_PWD2");
        return false;
    }

    //담당자 명 값 체크 
    if (_fnToNull($("#RES_NAME").val().replace(/ /gi, "")) == "") {
        _fnAlertMsg("담당자 명을 입력 해 주세요.", "RES_NAME");
        return false;
    }


    //휴대전화 값 체크 
    if (_fnToNull($("#RES_TEL").val().replace(/ /gi, "")) == "") {
        _fnAlertMsg("휴대전화 번호를 입력 해 주세요.", "RES_TEL");
        return false;
    }

    //사업자명 값 체크 
    if (_fnToNull($("#CUST_NM").val().replace(/ /gi, "")) == "") {
        _fnAlertMsg("사업자 번호를 입력 해 주세요.", "CUST_NM");
        return false;
    }

    //사업자등록번호 값 체크 
    if (_fnToNull($("#RES_CRN").val().replace(/ /gi, "")) == "") {
        _fnAlertMsg("사업자 번호를 입력 해 주세요.", "RES_CRN");
        return false;
    }

    ////사업자등록번호 값 체크 
    if ($("input[name=files]")[0].files.length == 0) {
        _fnAlertMsg("사업자등록증 사본을 선택해 주세요.", "Elvis_Crn_FileName");
        return false;
    }

    
    ////사업자등록번호 값 체크 
    if (_fnToNull($("#RES_OFFICE_ADDR").val().replace(/ /gi, "")) == "") {
        _fnAlertMsg("주소를 입력 해 주세요.", "RES_LOC_ADDR");
        return false;
    }
    
    /********************************************필수 값 체크********************************************/

    //비밀번호 체크 로직 넣기 
    //var vCheck = /^(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9])(?=.*[0-9]).{6,17}$/;
    //!vCheck.test(vValue)

    if (_fnToNull($("#RES_PWD").val()) != _fnToNull($("#RES_PWD2").val())) {
        _fnAlertMsg("비밀번호가 동일 하지 않습니다. 다시 입력 해 주세요.", "RES_PWD2");
        return false;
    }

    //번호 이외의 문자가 들어간 것 체크
    var regex = /[^0-9]/g;

    //사업자 번호 숫자 체크
    if (regex.test(_fnToNull($("#RES_CRN").val().replace(/-/gi, "").replace(/ /gi, "")))) {
        _fnAlertMsg("숫자만 입력 해 주세요.", "RES_CRN");
        return false;
    }


    //휴대전화 숫자 체크
    if (regex.test(_fnToNull($("#RES_TEL").val().replace(/-/gi, "").replace(/ /gi, "")))) {
        _fnAlertMsg("숫자만 입력 해 주세요.", "RES_TEL");
        return false;
    }    


    return true;
}

//주소 검색 함수
function fngetAddr(vPage) {

    //페이징 value 바꾸기
    $("#Elvis_Juso_currentPage").val(vPage);

    // 적용예 (api 호출 전에 검색어 체크) 	
    if (!fnCheckSearchedWord($("#Elvis_Juso_keyword").val())) {
        return;
    }

    $.ajax({
        url: "http://www.juso.go.kr/addrlink/addrLinkApiJsonp.do"  //인터넷망
        , type: "post"
        , data: $("#juso_form").serialize()
        , dataType: "jsonp"
        , crossDomain: true
        , success: function (jsonStr) {
            //$("#list").html("");
            var errCode = jsonStr.results.common.errorCode;
            var errDesc = jsonStr.results.common.errorMessage;
            if (errCode != "0") {
                //alert(errCode + "=" + errDesc);
                _fnAlertMsg(errDesc);
            } else {
                if (jsonStr != null) {
                    //기존 검색창 hide and show
                    $(".Elvis_JusoDetail_Area").hide();
                    $(".Elvis_JusoList_Area").show();

                    fnMakeAddrList(jsonStr);
                    fnAddrPaging(jsonStr["results"]["common"]["totalCount"], 5, 10, vPage)
                    //alert(jsonStr);                    
                }
            }
        }, error: function (xhr, status, error) {
            _fnAlertMsg("에러발생");
        }
    });
}

//특수문자, 특정문자열(sql예약어의 앞뒤공백포함) 제거
function fnCheckSearchedWord(obj) {
    if (obj.length > 0) {
        //특수문자 제거
        var expText = /[%=><]/;
        if (expText.test(obj) == true) {
            _fnAlertMsg("특수문자를 입력 할수 없습니다.");
            obj = obj.split(expText).join("");
            return false;
        }

        //특정문자열(sql예약어의 앞뒤공백포함) 제거
        var sqlArray = new Array(
            //sql 예약어
            "OR", "SELECT", "INSERT", "DELETE", "UPDATE", "CREATE", "DROP", "EXEC",
            "UNION", "FETCH", "DECLARE", "TRUNCATE"
        );

        var regex;
        for (var i = 0; i < sqlArray.length; i++) {
            regex = new RegExp(sqlArray[i], "gi");

            if (regex.test(obj)) {
                _fnAlertMsg("\"" + sqlArray[i] + "\"와(과) 같은 특정문자로 검색할 수 없습니다.");
                obj = obj.replace(regex, "");
                return false;
            }
        }
    }
    return true;
}

//페이징 함수
function fnAddrgoPage(vPage) {
    fngetAddr(vPage);
}

//totalData = 총 데이터 count
//dataPerPage = 한페이지에 나타낼 데이터 수
//pageCount = 한화면에 나타낼 페이지 수
//currentPage = 선택한 페이지 
//주소 찾기 페이징
function fnAddrPaging(totalData, dataPerPage, pageCount, currentPage) {

    var totalPage = Math.ceil(totalData / dataPerPage);    // 총 페이지 수
    var pageGroup = Math.ceil(currentPage / pageCount);    // 페이지 그룹
    if (pageCount > totalPage) pageCount = totalPage;
    var last = pageGroup * pageCount;    // 화면에 보여질 마지막 페이지 번호
    if (last > totalPage) last = totalPage;
    var first = last - (pageCount - 1);    // 화면에 보여질 첫번째 페이지 번호
    var next = last + 1;
    var prev = first - 1;

    $("#Elvis_JusoList_Paging").empty();

    var prevPage;
    var nextPage;
    if (currentPage - 1 < 1) { prevPage = 1; } else { prevPage = currentPage - 1; }
    if (last < totalPage) { nextPage = currentPage + 1; } else { nextPage = last; }

    var html = "";

    html += "<span onclick='fnAddrgoPage(1)'><<</span>";
    html += "<span onclick='fnAddrgoPage(" + prevPage + ")'><</span>";

    for (var i = first; i <= last; i++) {

        if (i == currentPage) {
            html += "<span class=\"active\">" + i + "</span>";
        } else {
            html += "<span onclick='fnAddrgoPage(" + i + ")'>" + i + "</span>";
        }
    }

    html += "<span onclick='fnAddrgoPage(" + nextPage + ")'>></span>";
    html += "<span onclick='fnAddrgoPage(" + totalPage + ")'>>></span>";

    $("#Elvis_JusoList_Paging").append(html);    // 페이지 목록 생성		
}


//검색 엔터 서치
function fnEnterSearch() {
    var evt_code = (window.netscape) ? ev.which : event.keyCode;
    if (evt_code == 13) {
        event.keyCode = 0;
        fngetAddr(1);
    }
}

//주소 찾기 - 레이어 팝업 끄기
function fnAddrCloseLayer() {
    $(".Elvis_JusoDetail_Area").hide();
    $(".Elvis_JusoList_Area").hide();
    layerClose('#Find_Address');
}

//주소 팝업에서 엔터키
function fnSetAddrData() {
    //우편번호
    $("#Elvis_OFFICE_CODE").val($("#Elvis_JusoDetail_zipNo").val());

    //기본주소
    $("#RES_OFFICE_ADDR").val($("#Elvis_JusoDetail_roadAddrPart1").val() + " " + $("#Elvis_JusoDetail_roadAddrPart2").val());

    

    //close 레이어
    fnAddrCloseLayer();
}

//회원가입 Content 만들기
function fnMakeContent() {

    try {

        var vHTML = "";

        vHTML += "  아이디 : " + $("#Elvis_ID").val();
        vHTML += "  이메일 : " + $("#Elvis_EMAIL").val();
        vHTML += "  사업자등록번호 : " + $("#Elvis_CRN").val();

        if (_fnToNull($("#Elvis_OfficeNM").val()) != "") {
            vHTML += "  상호 : " + $("#Elvis_OfficeNM").val();
        }

        vHTML += "  포워더 연계사 사업자등록번호 : " + $("#Elvis_ForCRN").val();
        vHTML += "  휴대전화 : " + $("#Elvis_HP_NO").val();

        if (_fnToNull($("#Elvis_OFFICE_ADDR").val()) != "") {
            vHTML += "  주소 : " + $("#Elvis_OFFICE_CODE").val() + " " + $("#Elvis_OFFICE_ADDR").val() + " " + $("#Elvis_OFFICE_ADDR2").val();
        }
        
        return vHTML;

    }
    catch (err) {
        console.log("[Error - fnMakeContent]" + err.message);
    }

}
/////////////////function MakeList/////////////////////
//주소 리스트 찍어주는 함수
function fnMakeAddrList(vJsonData) {

    var vHTML = "";
    var vCommon = vJsonData["results"]["common"];
    var vResult = vJsonData["results"]["juso"];

    $("#Elvis_JusoList_Total")[0].innerHTML = "＊도로명주소 검색 결과 (" + vCommon["totalCount"] + "건)";

    $.each(vResult, function (i) {

        if ((i % 2) == 0) {
            vHTML += "<tr style=\"background-color:#f2f2f2\">";
        } else {
            vHTML += "<tr>";
        }

        vHTML += " <td> ";
        vHTML += ((parseInt(vCommon["countPerPage"]) * (parseInt(vCommon["currentPage"]) - 1))) + (i + 1);
        vHTML += " </td> ";

        vHTML += "   <td name=\"roadAddr\"> ";
        vHTML += "   	<strong>" + vResult[i]["roadAddr"] + "</strong><br /> ";
        vHTML += "   	<p>[지번] " + vResult[i]["jibunAddr"] + "</p> ";

        if (_fnToNull(vResult[i]["detBdNmList"]) != "") {
            vHTML += "   	<p name=\"detBdNmList_display\" style=\"display:none\">[상세건물명]" + vResult[i]["detBdNmList"] + "</p> ";
        }

        //여기다가 숨겨두자
        vHTML += " <input type=\"hidden\" name=\"roadAddrPart1\" value=\"" + vResult[i]["roadAddrPart1"] + "\" /> ";
        vHTML += " <input type=\"hidden\" name=\"roadAddrPart2\" value=\"" + vResult[i]["roadAddrPart2"] + "\" /> ";
        vHTML += " <input type=\"hidden\" name=\"zipNo\" value=\"" + vResult[i]["zipNo"] + "\" /> ";

        vHTML += "   </td> ";
        vHTML += "   <td name=\"detBdNmList\"> ";

        if (_fnToNull(vResult[i]["detBdNmList"]) != "") {
            vHTML += "   	상세건물 보기 ";
        }

        vHTML += "   </td> ";
        vHTML += "   <td>" + vResult[i]["zipNo"] + "</td> ";
        vHTML += "</tr>";

    });

    $("#Elvis_JusoList_DataList")[0].innerHTML = vHTML;
}
$(document).on('click', '.join-type__box.join_type', function () {
    $('.join-type__box.join_type').removeClass('on');
    $(this).addClass('on');
})
$(document).on('click', '.join-type__box', function () {
    $(this).addClass('on');
})
$(document).on('click', '.join-type__box.on', function () {
    $(this).removeClass('on');
})
//////////////////////////API////////////////////////////
