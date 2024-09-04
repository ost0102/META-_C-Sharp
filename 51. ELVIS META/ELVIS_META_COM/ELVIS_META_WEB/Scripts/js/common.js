cssVars({ }); // CSS 사용자 정의 속성지원을 위한 스크립트 선언 css-vars-ponyfill.min.js
(function (window,$){
	var PRIME = null;
	PRIME = {
		init : function(){
			var funcThis = this,
				$header = $("#header"),
				$window = $(window),
				$navigation = $(".navigation");
			funcThis.pluginFunc(),
			funcThis.bind();

			$window.on('scroll', function() {
				if($window.scrollTop() > 0){
					$header.addClass("scroll");
					$navigation.addClass("scroll");
			  	}else{
					$header.removeClass("scroll");
					$navigation.removeClass("scroll");
			  	}
			});


			$window.resize(function(e){
				if(matchMedia("screen and (min-width: 1025px)").matches){
					if($('#hamberger').hasClass('show')){
						$('#hamberger').removeClass('show');
						$('.total_menu').attr('style', '');
					}
				}
				
				if($('.select_pop').length && $('.select_pop').css("display") == 'block'){
					$('.select_pop').attr('style', '');
					$('.select_pop').hide();
				}
			});
		},
		pluginFunc : function(){ 
			if($('.scrollbar-outer').length) {
				$('.scrollbar-outer').scrollbar();
			}
			
			// 달력플러그인 Type1 - 단독
			var calDate = $(".cal_date");
			if(calDate.length > 0) {
				calDate.each(function (index, item) {
					var $this = $(this);
					$this.datetimepicker({
						timepicker:false,
						format:'Y-m-d',
						onSelectDate:function(dp,$input){
					        var str = $input.val();
					        var m = str.substr(0,10);
					        
					        $this.find(".date").val(m);
						 }
					});
				});
			}
			
			// 달력플러그인 Type2 - 시작일~종료일
			// 달력플러그인
			var sDate = $(".start_date");
			if(sDate.length > 0) {
				sDate.datetimepicker({
					timepicker:false,
					format:'Y-m-d',
					onShow:function( ct ){
					   this.setOptions({
					    	maxDate: eDate.find(".date").val()? eDate.find(".date").val():false
					 	});
					},
					/*startDate:'2018.02.01',*/
					onSelectDate:function(dp,$input){
				        var str = $input.val();
				        var m = str.substr(0,10);
				        sDate.find(".date").val(m);
				   }
				});
			}
			var eDate = $(".end_date");
			if(eDate.length > 0) {
				eDate.datetimepicker({
					timepicker:false,
					format:'Y-m-d',
					 onShow:function( ct ){
					   this.setOptions({
					    minDate:sDate.find(".date").val()?sDate.find(".date").val():false
					   });
					 },
					/*startDate:'2018.02.01',*/
					onSelectDate:function(dp,$input){
				        var str = $input.val();
				        var m = str.substr(0,10);
				        eDate.find(".date").val(m);
				    }
				});
			}
		},
        bind : function(){ // 이벤트 바인딩
        	$(document).on('click', '.tbl_type1 .view', function (e) {
        		if($(this).css('display') != 'block'){
	        		var target = $(e.target),
					     targetparent = $(e.target.parentNode);
					if (target.is("a") || target.is("button") || targetparent.is("button")) {
					    return false;
					}
					var $this = $(this),
					     $relatedInfo = $("#"+$(this).data("row"));
					 if($relatedInfo.length > 0){
					     $(this).toggleClass("active");
					     $relatedInfo.toggle();
				     }
			     }
			}).on('click', '#hamberger, .total_menu', function (e) {
				var $hamberger = $('#hamberger');
				if (!$hamberger.hasClass("show")) {
					$('body').addClass("menu");
					$hamberger.addClass('show');
					$('.total_menu').fadeIn(200);
					$('.total_menu').addClass('on');
				}else{
					if($(e.target).parents(".menu_in").length>0){
						return;
					}
					$hamberger.removeClass('show');
					$('body').removeClass("menu");
					$('.total_menu').fadeOut(200, function(){
						$(this).attr('style', '');
					});
					$('.total_menu').removeClass('on');
				}
			}).on('click', '.delivery_status .btn_close', function (e) {
				if($('.delivery_status').hasClass('hide')){
					$('.delivery_status').removeClass('hide');
					$(this).text('CLOSE');
				}else{
					$('.delivery_status').addClass('hide');
					$(this).text('OPEN');
				}
			}).on('click', '.tab li', function (e) {
				if ($('.tab_panel').length > 0) {
					var $tab = $(this).closest('.tab'),
						$tabLi = $(this).closest('li'),
						$panel = $('.tab_panel .panel').eq($tabLi.index());
					if ($(this).closest('.tab_area').parent().attr('class') != "req-info__seq" && $(this).closest('.tab_area').parent().attr('class') != "dash-tab") {
						$('.tab_panel .panel').hide();
						$panel.show();
					}
					$tab.find('li').removeClass("on");
					$tabLi.addClass("on");
				}
			}).on('click', '.del', function (e) {
				var intBox = $(this).closest(".int_box");
				intBox.find("input[type='text']").val('').focus();
				intBox.find(".del").remove();
			}).on('click', '.file_view .file', function (e) {
				var $file = $(this).closest('.file_view'),
					 $layer = $file.find('.file_layer');
				$('.file_view').not($file).find('.file_layer').hide();
				if($layer.is(':visible')){
					$layer.hide();
				}else{
					$layer.show();
					if($file.find('.scrollbar-outer').length) {
						$file.find('.scrollbar-outer').scrollbar();
					}
					
					$(document).mouseup(function(e){
					    if (!$file.is(e.target) &&$file.has(e.target).length === 0) {
					    	$layer.hide();
					    }
					});
				}
			}).on('click', '#header .nav_toggle', function (e) {
				$('#header.sub').toggleClass('close');
			}).on('click', '#header .nav_toggle2', function (e) {
				$('#header.sub').toggleClass('close');
			});
        	
        	$(".int_box input[type='text']").bind("change keyup input", function(e) {
				if($(e.target).parents(".main_search").length){
					var intBox = $(this).closest(".int_box");
					if(!intBox.find('.del').length){
						$(this).after('<button type="submit" class="btns del"><span class="blind">삭제</span></button>');
					}
					intBox.find(".del").toggle(Boolean($(this).val()));
				}
			});
        }
	};
	window.PRIME = PRIME;
})(window, jQuery);	

$(function(){
	PRIME.init();

	$('.btn_top').on("click", function () {
		$('html, body').animate({ scrollTop: 0 }, 400);
		return false;
	});
	$("#lnb > li > a").on("click", function () {
		if (!$(this).closest("li").hasClass("on")) {
			$("#lnb > li").removeClass("on");
			$("#lnb .sub_depth").removeClass("on");
			$("#lnb .sub_depth").slideUp(100);
			$(this).closest("li").addClass("on");
			$(this).closest("li").find(".sub_depth").slideDown(100);
			$(this).closest("li").find(".sub_depth").addClass("on");
		}
		else {
			$("#lnb > li").removeClass("on");
			$("#lnb .sub_depth").removeClass("on");
			$("#lnb .sub_depth").slideUp(100);
        }
    })
});

//리스트 트래킹 버튼
$(document).on('click', '.btn_listOpen', function () {
	var $par = $(this).closest('.wanna_open');
	var inx = $par.index();
	if ($par.hasClass('open') == true) {
		$('.wanna_open:eq(' + inx + ')').find('.tracking_box').stop().slideUp();
		$par.removeClass('open');
	}
	else {
		if ($('.wanna_open').hasClass('open')) {
			$('.wanna_open').removeClass('open');
			$('.wanna_open').find('.tracking_box').slideUp();
		}
		$('.wanna_open:eq(' + inx + ')').addClass('open');
		$par.find('.tracking_box').slideDown();
	}
})

/* 레이어팝업 */
var layerPopup = function(obj){
	var $laybtn = $(obj),
		$glayer_zone = $(".layer_zone");
	if($glayer_zone.length===0){return;}
	//$glayer_zone.hide();
	$("body").addClass("layer_on");   
	$laybtn.fadeIn(200);
	
	$glayer_zone.on("click",".close",function(e){
		var $this = $(this),
			 $t_layer = $this.parents(".layer_zone");
		$("body").removeClass("layer_on");   
		$t_layer.fadeOut(300);
	});
};

/* 레이어팝업 닫기 */
var layerClose = function(obj){
	var $laybtn = $(obj);
	$("body").removeClass("layer_on");  
	$("#" + closeVar).focus();
	$laybtn.hide();
};

var selectOpen = function(obj){
	var $obj = $(obj);
	$obj.show();
	
	if($obj.find('.scrollbar-outer').length) {
		$obj.find('.scrollbar-outer').scrollbar();
	}
	
	$(document).mouseup(function(e){
	    if (!$obj.is(e.target) &&$obj.has(e.target).length === 0) {
	    	$obj.hide();
	    }
	});
};
var selectClose = function(obj1, obj2){
	$(obj1).closest('.int_box').find('input').val('').focus();
	$(obj2).hide();
};

var selectPopOpen = function(obj){
	var $obj = $(obj),
	     $intBox = $(obj).closest(".int_box");
	    
	if($obj.css("display") == 'block'){
		$obj.hide();
	}else{
		if(matchMedia("screen and (min-width: 1025px)").matches){
			var wrapWidth = $("#wrap").outerWidth(),
			     intBoxLeft = $intBox.offset().left,
			     objWidth = $obj.outerWidth(true);
			if ((wrapWidth - intBoxLeft) < objWidth) {
				var left = (wrapWidth - intBoxLeft)-objWidth;
				$obj.css('left', left);
			}
		}else{
			$obj.attr('style', '');
		}
				
		$obj.fadeIn(100);
		
		if($obj.find('.scrollbar-outer').length) {
			$obj.find('.scrollbar-outer').scrollbar();
		}
	}
	
	$(document).mouseup(function(e){
	    if (!$obj.is(e.target) &&$obj.has(e.target).length === 0 && !$intBox.is(e.target) && $intBox.has(e.target).length === 0) {
	    	$obj.hide();
	    }
	});
	
	$(window).resize(function(e){
		if($obj.css("display") == 'block'){
			$obj.attr('style', '');
			$obj.hide();
		}
	});
};
var selectPopClose = function(obj){
	$(obj).hide();
};

var showRadioPanel = function(obj1, obj2){
	 $(obj1).show();
	 $(obj2).hide();
};

function fnMovePage(vId) {

	var offset = $("#" + vId).offset();

	//클릭시 window width가 몇인지 체크
	var windowWidth = $(window).width();
	var vHeaderHeight = $("#header").height();

	if (windowWidth < 1025) {
		$('html, body').animate({ scrollTop: offset.top - vHeaderHeight }, 350);
		$('.total_menu, .dim').fadeOut(300, function () {
			$('.total_menu, .dim').attr('style', '');
		});
	}
	else {
		$('html, body').animate({ scrollTop: offset.top - 71 }, 350);
	}

}

$(document).ready(function () {
	// 페이지 로딩 시 실행
	checkWidthAndToggleClass();

	// 창 크기가 변할 때마다 실행
	$(window).resize(function () {
		checkWidthAndToggleClass();
	});

	// 창 크기에 따라 클래스를 토글하는 함수
	function checkWidthAndToggleClass() {
		if ($(window).width() <= 1300) {
			$('#header').removeClass('sub');
		} else {
			$('#header').addClass('sub');
		}
	}
});