// 2. This code loads the IFrame Player API code asynchronously.
			var tag = document.createElement('script');
	      tag.src = "https://www.youtube.com/iframe_api";
	      var firstScriptTag = document.getElementsByTagName('script')[0];
	      firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

			// 3. This function creates an <iframe> (and YouTube player)
	      //    after the API code downloads.
	      
	      var player;
	      function loadVideo(){ //onYouTubeIframeAPIReady() {
	        player = new YT.Player('player', {
	          height: '390',
	          width: '640',
	          videoId: '7ov6Zq_bp6c',
	          playerVars: {
	          	'showinfo' :0,
	          	'controls': 0
	          },
	          events: {
	            'onReady': onPlayerReady,
	            'onStateChange' : stateChange
	          }
	        });
	        $("#startVideo").hide();
	      }
	  

	      // 4. The API will call this function when the video player is ready.
	      function onPlayerReady(event) {
	        event.target.playVideo();

	        
	      }

	      function stateChange(event){
	      	if(event.target.getPlayerState() === 0)
		      {
		      	$("#player").hide();
		      	$("#videoCompleted").show();
		      }

	      	if(event.target.getPlayerState() === 2)
	      	{
	      		event.target.playVideo();
	      	}
	      }