////////////////////전역 변수//////////////////////////
////////////////////jquery event///////////////////////

$(function () {
    if (_fnToNull($("#Session_USR_ID").val()) != "") {
        location.href = window.location.origin + "/Info/Dashboard";
    }

    var userInputId = _fnGetCookie("Prime_CK_USR_ID_REMEMBER_ELVISBIG");
    if (_fnToNull(userInputId) != "") {
        $("#email").val(userInputId);
        $("#login_keep").replaceWith("<input type='checkbox' id='login_keep' name='login_keep' checked/>");
    }

    var classes = ["main_img01"];
    $(".main_visual").addClass(classes[~~(Math.random() * classes.length)]);

    $("#lnb").on('mouseenter focusin', function () {
        if (!matchMedia("screen and (max-width: 1024px)").matches) {
            $(".navi_bg").show();
            $(".sub_depth").show();
        }
    })
    $(".navi_bg").on('mouseleave focusout', function () {
        if (!matchMedia("screen and (max-width: 1024px)").matches) {
            $(".navi_bg").hide();
            $(".sub_depth").hide();
        }
    });

});
//로그인 버튼 이벤트
$(document).on("click", "#Login_btn", function () {
    //fnLogin();
    fnLoginApi();
});

$(document).on("keyup", "#email", function (e) {
    if (e.keyCode == 13) {
        if ($("#email").val() == "") {
            $("#Password_Warning").hide();
            $("#Email_Warning").show();
            $("#email").focus();
            $(".search_type1 #email").css("border-bottom", "1px solid #a9b0c7");
        } else {
            $("#Email_Warning").hide();
            $("#password").focus();
            $(".search_type1 #email").css("border-bottom", "unset");
        }
    }
});

$(document).on("keyup", "#password", function (e) {
    if (e.keyCode == 13) {
        if ($("#password").val() == "") {
            $("#Email_Warning").hide();
            $("#Password_Warning").show();
            $("#password").focus();
        } else {
            $("#Password_Warning").hide();
            $("#Login_btn").blur();
            $("#Login_btn").click();
        }
    }
});

/////////////////////function///////////////////////////////////
function fnLogin() {
    try {
        //로그인 체크
        if ($("#email").val() == "") {
            $("#Password_Warning").hide();
            $("#Email_Warning").show();
            $("#email").focus();
            $(".search_type1 #email").css("border-bottom", "1px solid #a9b0c7");
            return false;
        }
        else {
            $("#Email_Warning").hide();
            $(".search_type1 #email").css("border-bottom", "unset");
        }
        if ($("#password").val() == "") {
            $("#Email_Warning").hide();
            $("#Password_Warning").show();
            $("#password").focus();
            return false;
        }
        else {
            $("#Password_Warning").hide();
        }

        var objJsonData = new Object();
        objJsonData.ID = $("#email").val();
        objJsonData.PSWD = $("#password").val();

        $.ajax({
            type: "POST",
            url: "/Main/fnLogin",
            async: true,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                if (JSON.parse(result).Result[0]["trxCode"] == "Y") {

                    if (JSON.parse(result).Table[0].APV_YN == "Y") {
                        //아이디 저장 체크 일 경우 쿠키에 저장
                        if ($('input[name=login_keep]')[0].checked) {
                            _fnSetCookie("Prime_CK_USR_ID_REMEMBER_ELVISBIG", JSON.parse(result).Table[0].USR_ID, "168");
                        } else {
                            _fnDelCookie("Prime_CK_USR_ID_REMEMBER_ELVISBIG");
                        }                        

                        $.ajax({
                            type: "POST",
                            url: "/Main/SaveLogin",
                            async: true,
                            data: { "vJsonData": _fnMakeJson(JSON.parse(result)) },
                            success: function (result, status, xhr) {
                               // if (JSON.parse(result).Table[0].A)
                                location.href = window.location.origin + "/Main";                                
                            }
                        });
                    }
                    else if (JSON.parse(result).Table[0].APV_YN == "N") {
                        _fnAlertMsg("승인이 되지 않았습니다. 담당자에게 문의 하세요.");
                    }
                    else if (JSON.parse(result).Table[0].APV_YN == "D") {
                        _fnAlertMsg("가입 승인이 거절 되었습니다.<br/><br/>거절사유 : " + JSON.parse(result).Table[0].EMAIL_MSG);
                    }
                    else if (JSON.parse(result).Table[0].APV_YN == "S") {
                        _fnAlertMsg("아이디가 정지 되었습니다. 담당자에게 문의 하세요.");
                    }
                }
                else if (JSON.parse(result).Result[0]["trxCode"] == "N") {
                    _fnAlertMsg("아이디 혹은 비밀번호가 틀렸습니다");                    
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
    } catch (err) {
        console.log("[Error - fnLogin]"+err.message);
    }
}
//////////////////////API 테스트////////////////////////

$(document).on("click", "#test", function (e) {

    var objJsonData = new Object();
    objJsonData.USR_ID = "TEST";
    $.ajax({
        type: "POST",
        url: "/Main/fnTest",
        async: true,
        dataType: "json",
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            if (JSON.parse(result).Result[0]["trxCode"] == "Y") {


            }

        }, error: function (xhr) {
            console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
            console.log(xhr);
            return;
        }, beforeSend: function () {

        },
        complete: function () {
        }
    });

});

function fnLoginApi() {

    try {
        //로그인 체크
        if ($("#email").val() == "") {
            $("#Password_Warning").hide();
            $("#Email_Warning").show();
            $("#email").focus();
            $(".search_type1 #email").css("border-bottom", "1px solid #a9b0c7");
            $(".search_type1 #email").css("border-radius", "4px");
            $(".search_type1 #password").css("border-radius", "4px");
            return false;
        }
        else {
            $("#Email_Warning").hide();
            $(".search_type1 #email").css("border-bottom", "unset");
            $(".search_type1 #email").css("border-radius", "4px 4px 0 0");
            $(".search_type1 #password").css("border-radius", "0 0 4px 4px");
        }
        if ($("#password").val() == "") {
            $("#Email_Warning").hide();
            $("#Password_Warning").show();
            $("#password").focus();
            return false;
        }
        else {
            $("#Password_Warning").hide();
        }

        var objJsonData = new Object();
        objJsonData.USR_ID = $("#email").val();
        objJsonData.PSWD = $("#password").val();

        $.ajax({
            type: "POST",
            url: "/Main/fnLoginApi",
            async: true,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                if (JSON.parse(result).Result[0]["trxCode"] == "Y") {

                    if (JSON.parse(result).Table1[0].APV_YN == "Y") {
                        //아이디 저장 체크 일 경우 쿠키에 저장
                        if ($('input[name=login_keep]')[0].checked) {
                            _fnSetCookie("Prime_CK_USR_ID_REMEMBER_ELVISBIG", JSON.parse(result).Table1[0].USR_ID, "168");
                        } else {
                            _fnDelCookie("Prime_CK_USR_ID_REMEMBER_ELVISBIG");
                        }
                        var test = JSON.parse(result).Table1[0].USR_ID;
                        if (JSON.parse(result).Table1[0].PSWD == CryptoJS.MD5($("#password").val()).toString()) {
                            $.ajax({
                                type: "POST",
                                url: "/Main/SaveLogin",
                                async: true,
                                data: { "vJsonData": _fnMakeJson(JSON.parse(result)) },
                                success: function (result, status, xhr) {
                                    if (test == "prime@yjit.co.kr") {
                                        location.href = window.location.origin + "/Info/DashboardForwarder";
                                    } else {
                                        location.href = window.location.origin + "/Info/Dashboard";
                                    }
                                }
                            });
                        } else {
                            _fnAlertMsg("아이디 혹은 비밀번호가 틀렸습니다");
                        }
                    }
                    else if (JSON.parse(result).Table1[0].APV_YN == "N") {
                        _fnAlertMsg("승인이 되지 않았습니다. 담당자에게 문의 하세요.");
                    }
                    else if (JSON.parse(result).Table1[0].APV_YN == "D") {
                        _fnAlertMsg("가입 승인이 거절 되었습니다.<br/><br/>거절사유 : " + JSON.parse(result).Table1[0].EMAIL_MSG);
                    }
                    else if (JSON.parse(result).Table1[0].APV_YN == "S") {
                        _fnAlertMsg("아이디가 정지 되었습니다. 담당자에게 문의 하세요.");
                    }
                }
                else if (JSON.parse(result).Result[0]["trxCode"] == "N") {
                    _fnAlertMsg("아이디 혹은 비밀번호가 틀렸습니다");
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
    } catch (err) {
        console.log("[Error - fnLogin]" + err.message);
    }
}
/////////////////////API///////////////////////////////////////
