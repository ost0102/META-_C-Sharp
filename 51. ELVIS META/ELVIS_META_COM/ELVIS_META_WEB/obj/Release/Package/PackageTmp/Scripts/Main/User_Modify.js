
var url = "Main";
var vLocation_Login = "";
var chk = false; var chkpwd = false;

$(function () {

    $("#page_User_Modify").addClass("on");
   if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }
    //로그인 세션 & 쿠키를 가져옵니다.
    if (!_fnCheckLogin()) { return false }
    else { objLogin_Info = _fnCheckLogin() }

    try {
        var result = _fnGetAjaxData("POST", url, "fnLoginApi", objLogin_Info);
        if (JSON.parse(result).Result[0].trxCode == "Y") {

            objLogin_Info.CRN = JSON.parse(result).Table1[0].CRN;
            objLogin_Info.USR_NM = JSON.parse(result).Table1[0].USR_NM;
            objLogin_Info.HP_NO = JSON.parse(result).Table1[0].HP_NO;
            objLogin_Info.LOC_NM = JSON.parse(result).Table1[0].LOC_NM;
            objLogin_Info.LOC_ADDR = JSON.parse(result).Table1[0].LOC_ADDR;
            objLogin_Info.BIZCOND = JSON.parse(result).Table1[0].BIZCOND;
            objLogin_Info.BIZTYPE = JSON.parse(result).Table1[0].BIZTYPE;
            objLogin_Info.CEO = JSON.parse(result).Table1[0].CEO;
            objLogin_Info.TEL_NO = JSON.parse(result).Table1[0].TEL_NO;
            objLogin_Info.ENG_NM = JSON.parse(result).Table1[0].ENG_NM;
            objLogin_Info.ENG_ADDR = JSON.parse(result).Table1[0].ENG_ADDR;
            objLogin_Info.CTRY_CD = JSON.parse(result).Table1[0].CTRY_CD;
            objLogin_Info.MNGT_NO = JSON.parse(result).Table1[0].MNGT_NO;


            var result = _fnGetAjaxData("POST", url, "fnGetCrnFile", objLogin_Info);
            if (JSON.parse(result).Result[0].trxCode == "Y") {
                $("#NOW_FILE_NM").text(JSON.parse(result).Table1[0].FILE_NAME);
                $("#NOW_FILE_PATH").val(JSON.parse(result).Table1[0].FILE_PATH);
            }
        }
        else if (JSON.parse(result).Result[0].trxCode == "N") {
            _fnAlertMsg("회원 정보를 가져올 수 없습니다. 관리자에게 문의하세요.");
            location.href = vLocation_Login;
            return false;
        }
        else if (JSON.parse(result).Result[0].trxCode == "E") {
            _fnAlertMsg("오류가 발생 하였습니다. 관리자에게 문의 하세요");
            return false;
        }

        $("#NOW_EMAIL").text(objLogin_Info.USR_ID);
        $("#NOW_PSWD").text(objLogin_Info.USR_ID);
        $("#NOW_LOC_NM").text(objLogin_Info.LOC_NM);
        $("#NOW_CRN").val(objLogin_Info.CRN);
        $("#NOW_NAME").val(objLogin_Info.USR_NM);
        $("#NOW_TEL").val(objLogin_Info.HP_NO);
        $("#NOW_CRN").text(objLogin_Info.CRN);
        $("#NOW_LOC_ADDR").val(objLogin_Info.LOC_ADDR);
        $("#NOW_BIZCOND").val(objLogin_Info.BIZCOND);
        $("#NOW_BIZTYPE").val(objLogin_Info.BIZTYPE);
        $("#NOW_CEO").val(objLogin_Info.CEO);
        $("#NOW_TEL_NO").val(objLogin_Info.TEL_NO);
        $("#NOW_ENG_NM").val(objLogin_Info.ENG_NM);
        $("#NOW_ENG_ADDR").val(objLogin_Info.ENG_ADDR);
        $("#NOW_CTRY_CD").val(objLogin_Info.CTRY_CD);

    } catch (err) {
        console.log(err.message);
    }
});

$("#NOW_FILE_NM").click(function (e) {

    var objJsonData = new Object();
    objJsonData.FILE_PATH = $("#NOW_FILE_PATH").val();
    objJsonData.FILE_NM = $("#NOW_FILE_NM").text();
    
    $.ajax({
        type: "POST",
        url: "/Main/FileDownload",
        async: true,
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
          
                window.location = "/MAIN/DownloadFile?FILE_NM=" + objJsonData.FILE_NM;
         
        }, error: function (xhr) {
            console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
            console.log(xhr);
            return;
        }
    });

});

$(".chk").click(function () {
        layerPopup('#privacy_pop');
});

//로그아웃 버튼 이벤트
$(document).on("click", "#Logout_Btn", function () {
    _fnLogout();
});

$("#NOW_FILE_NM").click(function (e) {
    
});

function fnCheckID_RealTime(value) {
    try {   
        if (rtnVal.Result[0].trxCode == "Y") {
            fnOnOffWarning("PWD_Same", "true");
            fnWarningBorder("NEW_PWD", "#f44336");
            chkpwd = true;
        } else if (rtnVal.Result[0].trxCode == "N") {
            fnOnOffWarning("PWD_Same", "false");
            chkpwd = false;
            return;
        } else if (rtnVal.Result[0].trxCode == "E") {
            _fnAlertMsg("회원 정보를 가지고 올 수 없습니다.");
            $("#PWD_Same").val("false");
            return;
        }
    } catch (err) {
        console.log(err.message);
    }
}

$(document).on("focusout", "input", function (e) {
   //var $this = $(e.target);
   // if ($this.attr('id') == "NEW_PWD") {
   //     fnCheckID_RealTime($("#NEW_PWD").val().trim());
   // }
});
//Keyup관련 내용 추가.

$(document).on("keyup", "input", function (e) {
    try{
        var $this = $(e.target);
        var vValue = "";
        var vCompare = "";

        if ($this.attr('id') == "NEW_PWD") {
            vValue = $("#NEW_PWD").val();
            vCompare = $("#NEW_PWD2").val();

            //값이 없을 경우
            if (_fnToNull($("#NEW_PWD").val()) == "") {
                $("#NEW_PWD").css("border-color", "");
                fnOnOffWarning("PW1_OverSix", "false");
                fnOnOffWarning("PW1_Regular", "false");
                return false;
            }

            var vBoolean_LessSix = "false";
            var vBoolean_Regular = "false";
            var vBoolean_Different = "false";

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

            if (vBoolean_LessSix == "false" || vBoolean_Regular == "false") {
                fnWarningBorder("NEW_PWD", "#f44336");
                if ($("#PW2_Compare").css("display") == "inline-block") {
                    fnWarningBorder("NEW_PWD2", "#f44336");
                }
            } else {
                fnWarningBorder("NEW_PWD", "");
                //만약 비밀번호 확인이 오류면 변경 해주는 로직. (확인이 필요함)
                if ($("#NEW_PWD2").css("border-top-color") == "rgb(244, 67, 54)") {
                    $("#NEW_PWD2").css("border-color", "");
                }
            }
        }
        //새로운 비밀번호 확인
        else if ($this.attr('id') == "NEW_PWD2") {
            vValue = $("#NEW_PWD2").val();
            vCompare = $("#NEW_PWD").val();

            //값이 없을 경우
            if (_fnToNull($("#NEW_PWD2").val()) == "") {
                $("#NEW_PWD2").css("border-color", "");
                fnOnOffWarning("PW2_OverSix", "false");
                fnOnOffWarning("PW2_Regular", "false");
                fnOnOffWarning("PW2_Compare", "false");
                return false;
            }


            var vBoolean_LessSix = "false";
            var vBoolean_Regular = "false";
            var vBoolean_Different = "false";

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

            //마지막 체크
            if (vBoolean_LessSix == "false" || vBoolean_Regular == "false" || vBoolean_Different == "false") {
                fnWarningBorder("NEW_PWD2", "#f44336");

                if (vBoolean_Different == "false" && $("#NEW_PWD").val() != "") {
                    fnWarningBorder("NEW_PWD2", "#f44336");
                }

            } else {
                fnWarningBorder("NEW_PWD2", "");
                //만약 비밀번호 확인이 오류면 변경 해주는 로직. (확인이 필요함)
                if ($("#NEW_PWD").css("border-top-color") == "rgb(244, 67, 54)") {
                    if (!chkpwd) {
                        $("#NEW_PWD").css("border-color", "");
                    }
                }
            }
        }
        else if ($this.attr('id') == "NOW_TEL") {
            vValue = $("#NOW_TEL").val().trim();
            var vKorCheck = /[ㄱ-ㅎ|ㅏ-ㅣ]/; //한글 자음 체크
            var vEngCheck = /[a-z | A-Z]/;

            //Phone 하이픈 넣기
            $(this).val(_fnMakePhoneForm(vValue));
        }
    } catch (err) {
        console.log(err.message);
    }
});

//엔터 쳤을 경우 다음 input
$(document).on("keydown", "input", function (e) {
    if (e.which == 13) {
        var $this = $(e.target);
        var index = parseFloat($this.attr('data-index'));
        $('[data-index="' + (index + 1).toString() + '"]').focus();
    }
});

$("#btnCancel").click(function (e) {
    if ($("#Session_USR_ID").val() == "prime@yjit.co.kr") {
        controllerToLink("DashboardForwarder", "Info");
    } else {
        controllerToLink("Dashboard", "Info");
    }

});
//수정 버튼
$(document).on("click", "#btn_Modify", function () {
    try {
        if (fnValidation()) {

            objLogin_Info.HP_NO = $("#NOW_TEL").val().replace(/-/gi,'');
            objLogin_Info.USR_NM = $("#NOW_NAME").val();
            objLogin_Info.LOC_ADDR = $("#NOW_LOC_ADDR").val();
            objLogin_Info.BIZCOND = $("#NOW_BIZCOND").val();
            objLogin_Info.BIZTYPE = $("#NOW_BIZTYPE").val();
            objLogin_Info.CEO = $("#NOW_CEO").val();
            objLogin_Info.TEL_NO = $("#NOW_TEL_NO").val();
            objLogin_Info.ENG_NM = $("#NOW_ENG_NM").val();
            objLogin_Info.ENG_ADDR = $("#NOW_ENG_ADDR").val();
            objLogin_Info.CTRY_CD = $("#NOW_CTRY_CD").val();
            //비밀번호 변경 값이 있을 경우.
            if (_fnToNull($("#NEW_PWD").val()) != "") {
                objLogin_Info.PSWD = $("#NEW_PWD").val();
            }

            //유저 업데이트
            var result = _fnGetAjaxData("POST", url, "UserInfoUpdateApi", objLogin_Info);
            if (JSON.parse(result).Result[0].trxCode == "Y") {
                $(".alert_cont .inner").html("회원정보가 수정되었습니다.");
                layerPopup('#alert01', "", false);
                $("#alert_close").focus();
                $('#alert_close').click(function () {
                    loginSet();
                });
            }
            else if (JSON.parse(result).Result[0].trxCode == "N") {
                _fnAlertMsg(result.Result[0].trxMsg);
                console.log("[회원 수정]수정 버튼" + result.Result[0].trxMsg);
            }
            else if (JSON.parse(result).Result[0].trxCode == "E") {
                _fnAlertMsg("오류가 발생 하였습니다. 관리자에게 문의 하세요");
                console.log("[회원 수정]수정 버튼" + result.Result[0].trxMsg);
            }

        }
    } catch (err) {
        console.log(err.message);
    }
});
function loginSet() {
    var objJsonData = new Object();
    objJsonData.USR_ID = $("#NOW_EMAIL").text();
    if (_fnToNull($("#NEW_PWD").val()) != "") {
        objJsonData.PSWD = $("#NEW_PWD").val();
    } else {
        objJsonData.PSWD = $("#NOW_PWD").val();
    }
    

    $.ajax({
        type: "POST",
        url: "/Main/fnLoginApi",
        async: true,
        dataType: "json",
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                if (JSON.parse(result).Table1[0].APV_YN == "Y") {
                    if (JSON.parse(result).Table1[0].PSWD == CryptoJS.MD5(objJsonData.PSWD)) {
                        $.ajax({
                            type: "POST",
                            url: "/Main/SaveLogin",
                            async: true,
                            data: { "vJsonData": _fnMakeJson(JSON.parse(result)) },
                            success: function (result, status, xhr) {
                                if ($("#Session_USR_ID").val() == "prime@yjit.co.kr") {
                                    location.href = window.location.origin + "/Info/DashboardForwarder";
                                } else {
                                    location.href = window.location.origin + "/Info/Dashboard";
                                }
                            }
                        });
                    } else {
                        if ($("#Session_USR_ID").val() == "prime@yjit.co.kr") {
                            location.href = window.location.origin + "/Info/DashboardForwarder";
                        } else {
                            location.href = window.location.origin + "/Info/Dashboard";
                        }
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

}

//*****************Dev Function Area***********************//

//검사
function fnValidation() {
 try{
    
    var vNewPwd = $("#NEW_PWD").val();
    var vCheckNewPwd = $("#NEW_PWD2").val();
    var vCheck = /^(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9])(?=.*[0-9]).{6,17}$/; //비밀번호 유효성 검사


    //필수값 - 이름
    if (_fnToNull($("#NOW_NAME").val()) == "") {
        _fnAlertMsg("이름을 입력 해 주세요.","NOW_NAME" );
        $("#NOW_NAME").focus();
        return false;
    }

    if (_fnToNull($("#NOW_TEL").val()) == "") {
        _fnAlertMsg("휴대폰번호를 입력 해 주세요.", "NOW_TEL");
        $("#NOW_TEL").focus();
        return false;
    }

    //비밀번호 관련 벨리데이션 체크
    if (_fnToNull(vNewPwd) != "" || _fnToNull(vCheckNewPwd) != "") {
        //벨리데이션 
        if (vNewPwd != vCheckNewPwd) {
            _fnAlertMsg("새 비밀번호와 확인 비밀번호가 같지 않습니다.");
            return false;
        }
        if (vNewPwd.length < 6) {
            _fnAlertMsg("비밀번호의 자리수는 6자리 이상 16자리 이하로 입력 해 주세요.");
            return false;
        }
        if (!vCheck.test(vNewPwd)) {
            _fnAlertMsg("비밀번호는 영문,숫자,특수문자를 포함하여 입력 해 주세요.");
            return false;
        }

        //if (vNowPwd == vNewPwd) {
        //    _fnAlertMsg("현재 비밀번호과 동일합니다. 확인하고 다시 입력 해 주세요.");
        //    return false;
        //}
    }

    return true;
} catch (err) {
    console.log(err.message);
}
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

//Border 값 변경 시켜주느 함수
function fnWarningBorder(InputID, Color) {
    $("#" + InputID).css("border-color", Color);
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
    $("#NOW_LOC_ADDR").val($("#Elvis_JusoDetail_roadAddrPart1").val() + " " + $("#Elvis_JusoDetail_roadAddrPart2").val());



    //close 레이어
    fnAddrCloseLayer();
}

//주소 찾기 디테일 "주소입력"
$(document).on("click", "#Elvis_JusoDetail_BtnArea label", function () {
    fnSetAddrData();
});



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

//주소 찾기 버튼 이벤트
$(document).on("click", "#Elvis_Find_Addr", function () {

    //기존 데이터 초기화
    $("#Elvis_Juso_keyword").val("");
    $("#Elvis_JusoDetail_roadAddrPart2").val("");
    $(".Elvis_JusoList_Area").hide();

    layerPopup('#Find_Address');
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

$(document).on('click', '.list-info > p', function () {
    $('.join-in__box.fourth').hide();
    /*$('.join-in__box.second.shipper').show();*/
    $('.join-in__box.second.execution').show();
})

$(document).on('click', '.join-type__box', function () {
    $(this).addClass('on');
})
$(document).on('click', '.join-type__box.on', function () {
    $(this).removeClass('on');
})
$(document).on('click', '[name=prevpage]', function () {
    window.history.back();
})
$(document).on('click', '[name=mypage]', function () {
    $('.join-in__box.second.shipper, .join-in__box.second.execution').hide();
    $('.join-in__box.fourth').show();
})