////////////////////전역 변수//////////////////////////
var _vSelectDate = new Date();
var obj = new Object();
var mymap;
var url = "Format"
var addYn = false;
////////////////////jquery event///////////////////////
$(function () {

    //로그인 하지 않고 들어왔을때
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        location.href = window.location.origin;
    }

    $(".sub_format").addClass("on");
    $(".sub_format .sub_depth").addClass("on");
    $(".sub_format .sub_depth li:nth-child(1) a").addClass("on");

    if (_fnToNull($("#View_TMP_DTL").val()) != "") {
        _vTmpDtl = _fnToNull($("#View_TMP_DTL").val());
        fnMakeDetail(_vTmpDtl);
    }



});


$(document).ready(function () {
    var fileTarget = $('.int_box #formatFile');

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
})

$("#DocumentAdd").click(function (e) {

    formData = new FormData(); //Form 초기화
    const files = $("input[name=files]")[0].files;

    formData.append("files", files[0]);
    $.ajax({
        type: "POST",
        url: "/Main/Upload_Files",
        dataType: "json",
        async: false,
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: formData,
        success: function (result, status, xhr) {
            $("#iframe_test").attr("src", "/web/viewer.html?file=/Files/TEMP/" + $("input[name=files]")[0].files[0].name);
            addYn = true;
        },
        error: function (xhr, status, error) {
            _fnAlertMsg("[Error]관리자에게 문의 해 주세요. " + status);
            vReturn = false;
        }
    });

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

$("#RMK").keyup(function (e) {
    var regExp = /{/gi;
    if (regExp.test($(this).val())) {
        $(this).val($(this).val().substring(0, $(this).val().length - 1));
    }

    var regExp = /}/gi;
    if (regExp.test($(this).val())) {
        $(this).val($(this).val().substring(0, $(this).val().length - 1));
    }
});

$("#btnRegist").click(function (e) {
    if (addYn) {
        if (_fnToNull($("#DOC_NAME").val()) != "") {
            forms = new FormData();
            if ($("input[name=files]")[0].files.length > 0) {
                const files = $("input[name=files]")[0].files;
                var test = $("#Session_AUTH_KEY").val();

                forms.append("files", files[0]);
                forms.append("MNGT_NO", $("#Session_MNGT_NO").val());
                forms.append("CRN", $("#Session_CRN").val());
                forms.append("TMPLT_NAME", $("#DOC_NAME").val());
                forms.append("TMPLT_TYPE", $("#DOC_TYPE option:selected").val());
                forms.append("RMK", $("#RMK").val());

                forms.append("USR_ID", $("#Session_USR_ID").val());
                forms.append("USR_NM", $("#Session_USR_NM").val());
                forms.append("HP_NO", $("#Session_HP_NO").val());
                forms.append("LOC_NM", $("#Session_CUST_NM").val());

                var request = new XMLHttpRequest();
                request.open("POST", _ApiUrl + "api/Template/TemplateUpload");
                request.setRequestHeader("Authorization-Type", "Y");
                request.setRequestHeader("Authorization-Token", $("#Session_AUTH_KEY").val());
                request.send(forms);
                request.onload = function (e) {
                    if (this.status == 200) {
                        console.log('response', this.statusText);
                        _fnAlertMsg("등록이 완료되었습니다. 승인 확인 후 사용 가능 합니다.");
                        layerPopup('#alert01', "", false);
                        $("#alert_close").focus();
                        $('#alert_close').click(function () {
                            controllerToLink("Index", "Format", "");
                        });

                    }
                };
            }
        } else {
            _fnAlertMsg("문서명을 입력해주세요.");
        }
    } else {
        _fnAlertMsg("서식을 추가해주세요.");
    }

});


function fnMakeDetail(rtnval) {
    const arr = rtnval.split("_^_");
    $('#DOC_TYPE').val(arr[1]).prop("selected", true);
    $('#DOC_TYPE').attr("disabled", true);
    $('#DOC_NAME').attr("disabled", true);
    $('#RMK').attr("disabled", true);
    $('#DOC_NAME').val(arr[2]).prop("selected", true);
    $('#FILE_NM').val(arr[3]).prop("selected", true);
    $('#RMK').val(arr[4]).prop("selected", true);

    TmpFileDown(arr[5], arr[3]);
    $("#btnRegist").hide();
    $("#DocumentAdd").hide();
    $("#formatFile").attr("disabled", true);
}


function TmpFileDown(file_path , file_nm) {
    var objJsonData = new Object();
    objJsonData.FILE_PATH = file_path;
    objJsonData.FILE_NM = file_nm;

    $.ajax({
        type: "POST",
        url: "/Main/FileDownload",
        async: true,
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {

            $("#iframe_test").attr("src", "/web/viewer.html?file=/Content/TempFiles/" + file_nm);

        }, error: function (xhr) {
            console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
            console.log(xhr);
            return;
        }
    });

};


$("#btnPrev").click(function (e) {
    controllerToLink("Index", "Format", "");

});
/////////////////////function///////////////////////////////////

//////////////////////function makelist////////////////////////

/////////////////////////////API///////////////////////////////
