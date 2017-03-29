/*
 * Custom Progress Bar
 * https://ocias.com/blog/unity-webgl-custom-progress-bar/
 */

function init() {}

function UnityProgress (dom) {
	this.progress = 0.0;
	this.message = "";
	this.dom = dom;

	createjs.CSSPlugin.install(createjs.Tween);
	createjs.Ticker.setFPS(60);
	
	var parent = dom.parentNode;

	this.SetProgress = function (progress) {
		if (this.progress < progress)
			this.progress = progress;

		if (progress == 1) {
		  this.SetMessage("Preparing...");
		  document.getElementById("bgBar").style.display = "none";
		  document.getElementById("progressBar").style.display = "none";
		} 
		
		this.Update();
	}
	
	this.SetMessage = function (message) {
	    if (message.toLowerCase().indexOf("down") !== -1) {
	        // incoming: "Downloading Data... (XXXX/YYYYY)"

            // seperate numbers from all string
	        var splitted1 = message.split("(");
	        var lastPart = splitted1[splitted1.length-1].slice(0,-1);
	        var sizesArray = lastPart.split("/");

            // calculate percentage
	        var percentage = Math.round((sizesArray[0] / sizesArray[1]) * 100);

            // set custom message
	        this.message = "Downloading Data... " + percentage + "%";
	    } else {
	        this.message = message;
	    }
		this.Update();
	}
	
	this.Clear = function() {
		document.getElementById("loadingBox").style.display = "none";
	}
	
	this.Update = function() {
		var length = 200 * Math.min(this.progress, 1);
		bar = document.getElementById("progressBar")
		createjs.Tween.removeTweens(bar);
		createjs.Tween.get(bar).to({width: length}, 500, createjs.Ease.sineOut);
		document.getElementById("loadingInfo").innerHTML = this.message;
	}
	
	this.Update ();
}