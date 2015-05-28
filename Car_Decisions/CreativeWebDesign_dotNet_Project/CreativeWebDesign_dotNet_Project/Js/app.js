var app = angular.module('app', ['ngSanitize']);
//
app.controller('mainController', ['$scope', function($scope){
	$scope.modalTitle ="";
	$scope.modalContent="";
	$scope.closeButton ="";
	$scope.infoImage ="../Images/info_Transparent.png";
	$scope.minusImage ="../Images/minus_new.jpg";
	$scope.plusImage ="../Images/plus_new.jpg";
	$scope.sliderImage ="../Images/slider.jpg";
	$scope.helpText ="";
	$scope.test ="tests";
	$scope.sliderValues = {"s1" : 0, "s2" : 0, "s3" : 0, "s4" : 0, "s5" : 0, "s6" : 0, "s7" : 0, "s8" : 0, "s9" : 0, "s10" : 0, "s11" : 0, "s12" : 0, 
	"s_vol8" : 0, "s_14" : 0, "s_15": 0, "s_16" : 0};
	$scope.currentSlider;
	$scope.val8 = 0;
	$scope.val12 = 0;
	$scope.val4 = 0;
	$scope.val16 = 0;
	$scope.Links = [];
	var uaObj = {
        userActivity:[]
	};
	var windowStartTime;
	var windowStopTime;
	var siteStartTime = getTime();
	var siteStopTime;
	var finalChoice = "";
	var windowIndex;
	var windowWasOpened= false;
	

	var linkNum = 0;
    //figure out which browser is being used
	var isOpera = !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
    // Opera 8.0+ (UA detection to detect Blink/v8-powered Opera)
	var isFirefox = typeof InstallTrigger !== 'undefined';   // Firefox 1.0+
	var isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0;
    // At least Safari 3+: "[object HTMLElementConstructor]"
	var isChrome = !!window.chrome && !isOpera;              // Chrome 1+
	var isIE = /*@cc_on!@*/false || !!document.documentMode;

	//add browser specific style
	
    //ie
  
	if(isIE){
		$('.rangeButton').css('margin-bottom', '20px');
		//$('.pwLeftArrowBorder').css('margin-left', '166px');
		$('.pwLeftArrow').css('margin-left', '163px');
		$('.pwLeftArrow').css('margin-top', '48.5px');
		$('.pwTopArrow').css('margin-left', '69px');
		$('.ssArrow').css('margin-left', '49px');
		$('.ssArrow').css('margin-top', '21px');
		$('td').css('padding', '0');
		$('qm').css('transform', 'translate(-18.5px, -.5px)');
	}
	if(isSafari){
		
		  $('.range').css('margin-bottom', '5px').change();
		  $('.range').css('background-color', '#125eab').change();
		  $('.range').css('border: 1px', 'solid white').change();
		  $('.range').css('border-radius', '4px').change();
		  $('.range').css('background-color', '#125eab').change();
		  $('.range').css('height', '10px');
		  $('.pw').css('top', '15px');
		  $('.pwTopArrow').css('margin-top', '136px');
		  $('.pwTopArrowBorder').css('margin-top', '137px');
		  $('.pwLeftArrow').css('margin-top', '49px');
		  $('.pwTopArrow').css('margin-left', '69px');
		  $('.pwTopArrow').css('margin-top', '127px!important');
	    /*border: 1px solid white;
	    border-radius: 3px;
	    height: 7px;*/
	}
	if(isChrome){
		$('.pwLeftArrow').css('margin-top', '49px');
		$('.pwTopArrow').css('margin-left', '69px');
		$('.pwTopArrow').css('margin-top', '127px!important');

	}
	//variables
	var modalClicked = false;
	var modalOpen = false;
	var buttonVal = "";
	var buttons = $("button");
	var modalDisableOverlay = $('<div class="modal-disable-overlay">&nbsp;</div>');

	$('input').val(0).change();


	$(".info").on("click", function (e) {
	    //add to user activity
	    windowWasOpened = true;
	    var windowClicked = $(this).data("window");
	    addInfoOpen(windowClicked);
	    

        $("#fader10").data("id", windowClicked)
        $scope.currentSlider = $(this).data('slider');
        
		$scope.$apply(function(){
			$scope.helpText = $(this).data('help');
		});
		
		$scope.modalTitle = "Info";
		$("#modal").show();
		buttonVal = $(this).data('info');
		//var thisClass = $(this).attr('class');
		$('helpWindow').hide();
		$('#infoWindow').show();
		$('#backButton').hide();
		$('#filter').addClass('filter');
		$('#modalInfo').append(buttonVal);
		/*$scope.$apply(function(){

			$scope.modalContent = buttonVal;

		
		});

		/*Slider*/
		var value =$scope.sliderValues[$scope.currentSlider];
		$('#fader10').val($scope.sliderValues[$scope.currentSlider]).change();
		document.querySelector('#modalOutput').value = $scope.sliderValues[$scope.currentSlider];
		
		setTimeout(function () { modalOpen = true; }, 300);
		windowWasOpened = false;
		createSSLink();
	});
	//Slider

	var val10 = 0;
								
	function outputUpdate(vol) {
	document.querySelector('#modalOutput').value = vol;
	}

	$scope.sliderMinus = function (num){
			if(num > 0)
			{
				
				num--;
				
				/*
				$('#fader10').val($scope.sliderValues[$scope.currentSlider]).change();
				document.querySelector('#modalOutput').value = $scope.sliderValues[$scope.currentSlider];*/
			}
			return num;
			
		};

	$scope.sliderPlus =function (num){
			if(num < 10)
			{
				num++;
				
			}
			
			return num;
		};
		
	$('#helpButton').hover(function(){
		$('#helpW').show();
	},function(){
		$('#helpW').hide();
	});


	$('#closeButton').on('click', function(){
		$('#filter').removeClass('filter');
		$('#helpWindow').hide();
		$('#modal').hide();
		$('#backButton').hide();
		modalOpen = false;
		$('#modalInfo').empty();
		addInfoClose();
		/*$scope.$apply(function(){
			$scope.modalContent = "";
		});*/
		
	});

	//close modal if user clicks outside modal
	$('#modal').on('click', function(){
		modalClicked = true;
	});

	$('html').on('click', function(){
	    

	    if(modalOpen === true)
		{
			
	        if (modalClicked === false) {
	           
	            addInfoClose();
	            
				$('#filter').removeClass('filter');
				$('#helpWindow').hide();
				$('#modal').hide();
				$('#backButton').hide();
				modalOpen = false;
				$('#modalInfo').empty();/*
				$scope.$apply(function(){
					$scope.modalContent = "";
				});*/


			}
		}
		
		modalClicked = false;
	});
	
    /*
	$('#backButton').on('click', function(){
		$('#infoWindow').show();
		$('#helpWindow').hide();
		$('#helpButton').show();
		$(this).hide();

		$scope.$apply(function(){

			$scope.modalContent = buttonVal;
		
		});
	})*/

	
	//Checkboxes
	
	var box = "";
	$(":checkbox").prop("checked", false);
	$(":checkbox").attr('disabled', false);
	
	$(":checkbox").on("click", function(){
		
		
		box = $(this).attr('id');
		$(":checkbox").prop("checked", false);
		document.getElementById(box).checked = true;
	});

	/*Pop up triggers*/
	$('.cat').hover(function(){
		var pwID = "#" + $(this).data('window');
		$(pwID).show();

	}, function(){
		var pwID = "#" + $(this).data('window');
		$(pwID).hide();
	});

	
	
	//Superscript link function
	function createSSLink(){
		$('.ssLink').each(function(){
			$(this).append("<sup class='sl' data-window=" + "'link" + linkNum.toString() + "'>" + $(this).data('ss')  + "</sup>" + "<div data-test='test' style='-webkit-transform: translate(-65px, -37px); transform: translate(-65px, -37px);' class='ssPW'  id='"
				+ 'link' + linkNum.toString() + "'>"
				+ "<div class='ssArrowBorder'></div>"
				+	"<div class='ssArrow'></div>"
				+	"<a target='blank' href='blank'>" + $(this).data('link') + "</a> </div>"
			);
			
			//$(this).attr('data-window', 'link' + linkNum.toString());
			
			linkNum++;
		});
		$('.sl').hover(function(){
			var pwID;
		 	pwID = "#" + $(this).data('window');
			$(pwID).css("display", "inline");
		
		}, function(){
			var pwID = "#" + $(this).data('window');
			
			$(pwID).hide();

		});

		
	}

    //add user activity
	$("input").change(function () {
	    
	    if ($(this).prop("type") === "checkbox") {
	        AddInput($(this).data("id"), $(this).data("value"), "checkBox");
	        finalChoice = $(this).data("value");
	    }
	        
	    else {
	        
	        if (windowWasOpened === false) {
	        AddInput($(this).data("id"), $(this).val(), "range");
	        }
	    }
	    
	});

	function AddInput(inputClicked, value, type) {
	    var element = {timeSpentOnElement: "", elementClicked: "", orderClicked: "", elementType: "", elementValue: "" };
	    element.orderClicked = uaObj.userActivity.length + 1;
	    element.elementClicked = inputClicked;
	    element.elementType = type;
	    element.elementValue = value;
	    uaObj.userActivity.push(element);
	}
    
    //add info window
	function addInfoOpen(windowClicked) {
	    windowStartTime = getTime();
	    var window = { timeSpentOnElement: "", elementClicked: "", orderClicked: "", elementType: "", elementValue: "" }
	    window.orderClicked = uaObj.userActivity.length + 1;
	    window.elementClicked = windowClicked;
	    window.elementType = "window";
	    uaObj.userActivity.push(window);
	    windowIndex = uaObj.userActivity.length - 1;
	}

	function addInfoClose() {
	    windowStopTime = getTime();
	    
	    uaObj.userActivity[windowIndex].timeSpentOnElement = (windowStopTime - windowStartTime) / 1000;
	    windowIndex = "X";
	}
    
	function getTime() {
	    var day = new Date();
	    return day.getTime();
	}
    /*
	$("#sendUA").on("click", function () {
	    $.ajax()({
	        method: "POST",
	        url: "http://localhost:61376/Home/PostDemographics",
	        data: {ua : userActivity}
	    });
	});*/

	

	$("#sendUA").on("click", function (e) {
	    e.preventDefault();
	    var timeOnSite = ((getTime() - siteStartTime)/1000)/60;
	    var action = this.action;
	    var uID = $("#userID").data("value");
	    var uaString = JSON.stringify(uaObj); //.replace(/"/g, "'");
	    $("#sendUA").hide();
	    var matrix = $("matrix").data("matrix");
		$.ajax({
			crossDomain: true,
	        headers: { 'Access-Control-Allow-Origin': '*' },
	        type: "POST",
	        url: action,
	        data: { userActivity: uaString, userID: uID, timeOnSurvey: timeOnSite, finalChoice: finalChoice, matrix: matrix},
	        success: function () {
	            
	            window.location = "http://cardecisionssite.azurewebsites.net/home/finishedsurvey";
	            
	        },
	        error: function() {
	            alert("survery failed to post");
	        }
	    });
		
	    
	  
	});



	
}]);

