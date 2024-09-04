var url = "Main";
var vLocation_Login = window.location.origin + "/Export";

//Enter 키 입력 시 다음 칸 혹은 해당 버튼 불러오는 로직
$(document).on("keyup", "input", function (e) {
    if (e.which == 13) {
        if (this.id == "FindPW_ConfirmKey") {
            $("#SendEmail_GetPW").click();
        } else if (this.id == "FindPW_Email") {
            $("#SendEmail_GetConfirmKey").click();
        }
    }
});

//핸드폰 입력 시 
$(document).on("keyup", "#FindID_HP", function () {
    if (!(event.keyCode >= 37 && event.keyCode <= 40)) {
        var inputVal = $(this).val();
        $(this).val(inputVal.replace(/[^0-9 | -]/gi, ''));

        vValue = $(this).val().trim();
        $(this).val(_fnMakePhoneForm(vValue));
    }
});

//사용자 아이디 찾기
$(document).on("click", "#SendEmail_GetID", function () {
    $("#FindID_Result")[0].innerHTML = "";
    fnGetID();
});

//비밀번호 찾기 - 이메일 인증
$(document).on("click", "#SendEmail_GetConfirmKey", function () {
    fnGetConfirmKey();
});

//사용자 비밀번호 변경
$(document).on("click", "#SendEmail_GetPW", function () {
    fnGetPW();
});



//엔터키 이벤트
$(document).keyup(function (e) {
    if (e.keyCode == 13) {//키가 13이면 실행 (엔터는 13)

        //온라인 접수 input 이동
        if ($(e.target).attr('data-index') != undefined) {
            if ($(e.target).attr('data-index').indexOf("findUser") > -1) {
                //indexOf 데이터를 지우고 +1
                var vIndex = $(e.target).attr('data-index').replace("findUser", "");

                if (vIndex == "2") {
                    fnGetID();
                }
                else {
                    $('[data-index="findUser' + (parseFloat(vIndex) + 1).toString() + '"]').focus();
                }
            }

            if ($(e.target).attr('data-index').indexOf("findPwd") > -1) {
                //indexOf 데이터를 지우고 +1
                var vIndex = $(e.target).attr('data-index').replace("findPwd", "");

                if (vIndex == "2") {
                    fnGetConfirmKey();
                }
                else {
                    $('[data-index="findPwd' + (parseFloat(vIndex) + 1).toString() + '"]').focus();
                }
            }
        }
    }
});


//이메일 인증 확인 버튼
function fnGetConfirmKey() {
    try {

       if ($("#FindPW_Email").val() == "") {
            $(".error.error_pw").html("이메일을 입력 해 주세요.");
            $(".error.error_pw").show();
            $("#FindPW_Email").focus();
        }
        else {
            //ajax
            var objCheck_INFO = new Object();
           objCheck_INFO.USR_ID = $("#FindPW_Email").val();


           var rtnVal = _fnGetAjaxData("POST", url, "fnLoginApi", objCheck_INFO);
           if (JSON.parse(rtnVal).Result[0]["trxCode"] == "Y") {
               var new_pswd = _fnGetAjaxData("POST", url, "fnGetPassword", objCheck_INFO);

               if (new_pswd != "") {
                   objCheck_INFO.PSWD = new_pswd;

                   var rtnVal = _fnGetAjaxData("POST", url, "SetPWApi", objCheck_INFO);
                   if (JSON.parse(rtnVal).Result[0].trxCode == "Y") {
                       $(".alert_cont .inner").html("임시비밀번호가 발급되었습니다.");
                       layerPopup('#alert01', "", false);
                       $("#alert_close").focus();
                       $('#alert_close').click(function () {
                           location.href = window.location.origin;
                       });
                       //fnSendEmail(new_pswd);
                   } else if (JSON.parse(rtnVal).Result[0].trxCode == "E") {
                       $("#pwd_error").html("메일전송에 실패하였습니다.");
                       $("#pwd_error").show();
                   }

               }
           } else {
               $(".alert_cont .inner").html("존재하지않는 이메일입니다.");
               layerPopup('#alert01', "", false);
               $("#alert_close").focus();
           }
        }
    } catch (err) {
        console.log(err.message);
    }
}



////*****************Dev Function Area***********************//
//이메일 아이디 확인 함수
function fnGetID() {
    try {
        var objCheck_INFO = new Object();
        if ($("#FindID_Name").val().length < 2) {
            $("#id_error").html("두자리 이상 입력하세요");
            $('[data-index="findUser1"]').focus();
            $("#id_error").show();
            $(".id-result").hide();
            return false;
        }

        if ($("#FindID_HP").val().length == 0) {
            $("#id_error").html("휴대폰번호를 입력하세요");
            $("#id_error").show();
            $(".id-result").hide();
            return false;
        }
        objCheck_INFO.USR_NM = $("#FindID_Name").val();
        objCheck_INFO.HP_NO = $("#FindID_HP").val().replace(/-/gi,"");

        var rtnVal = _fnGetAjaxData("POST", url, "GetFindIDApi", objCheck_INFO);
        if (JSON.parse(rtnVal).Result[0].trxCode == "Y") {
            $(".error").hide();
            //innerHTML
            var vHTML = "";
            vHTML += "ID: ";
            $.each(JSON.parse(rtnVal).Table1, function (i) {
                if (0 < i) {
                    vHTML += " , " + fnSetEmailHidden(JSON.parse(rtnVal).Table1[i].USR_ID);
                } else {
                    vHTML += fnSetEmailHidden(JSON.parse(rtnVal).Table1[i].USR_ID);
                }

            });
            vHTML += " 입니다";
            $(".id-result").show();
            $("#FindID_Result")[0].innerHTML = vHTML;
        }
        else if (JSON.parse(rtnVal).Result[0].trxCode == "N") {
            $(".id-result").show();
            $("#FindID_Result")[0].innerHTML = "아이디가 존재하지 않습니다.";
        } else if (JSON.parse(rtnVal).Result[0].trxCode == "E") {
            $(".error").html("오류가 발생 하였습니다. 관리자에게 문의 하세요.");
            $(".error").show();
        }
    } catch (err) {
        console.log(err.message);
    }
}

//새로운 비밀번호 확인 함수
function fnGetPW() {
    try {
        var objCheck_INFO = new Object();
        objCheck_INFO.LOC_NM = $("#FindPW_Name").val().trim();
        objCheck_INFO.USR_ID = $("#FindPW_Email").val().trim();
        objCheck_INFO.APP_KEY = $("#FindPW_ConfirmKey").val().trim();
        objCheck_INFO.DOMAIN = _vDOMAIN;
        objCheck_INFO.OFFICE_CD = offie_code;

        var rtnVal = _fnGetAjaxData("POST", url, "GetFindPW", objCheck_INFO);
        if (rtnVal.Table[0].trxCode == "Y") {
            if (fnSendEmail(rtnVal, "B") == "Y") {
            }

            $(".alert_cont .inner").html("임시비밀번호가 발급되었습니다.");
            layerPopup('#alert01', "", false);
            $("#alert_close").focus();
            $('#alert_close').click(function () {
                location.href = window.location.origin;
            });
        }
        else if (rtnVal.Result[0].trxCode == "N") {
            $(".error").html("인증번호가 맞지 않습니다. 확인하시고 다시 재발급 요청 해주시기 바랍니다.");
            $(".error").show();
        }
    } catch (err) {
        $(".error").html("인증번호가 맞지 않습니다. 확인하시고 다시 재발급 요청 해주시기 바랍니다.");
        $(".error").show();
    }
}


//이메일 텍스트 hidden (value => 텍스트 값 / hiddenNum => 얼마나 가릴지) 
function fnSetEmailHidden(value) {

    if (value.indexOf("@") != -1) {
        var vValue = value.split("@");

        if (vValue[0].length < 4) {
            var vResult = vValue[0];
            vResult = vResult.substring(0, vResult.length - 1);
            vResult += "＊@";
            vResult += vValue[1];

            return vResult;
        }
        else {
            var vResult = vValue[0];
            vResult = vResult.substring(0, vResult.length - 3);
            vResult += "＊＊＊@";
            vResult += vValue[1];

            return vResult;
        }
    }
    else {
        _fnAlertMsg("이메일을 찾을 수 없습니다.\n@누락");
        console.log("@가 없습니다.");
    }
}

