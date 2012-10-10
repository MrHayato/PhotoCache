$(function () {
	function onDeviceReady() {
		pictureSource = navigator.camera.PictureSourceType;
		destinationType = navigator.camera.DestinationType;
		//$("#camera").after("<span>Camera Ready.</span>");
	}
	
	function setImage(image) {
		alert("Rawr");
		$("#camera").attr("src", image);
	}
	
	function onCaptureSuccess(imageData) {
		setImage("data:image/jpeg;base64," + imageData);
	}

	function onNavigateSuccess(imageURI) {
		setImage(imageURI);
	}

	function onCaptureFail(message) {
		alert('Failed because: ' + message);
	}

	function capturePhoto() {
		// Take picture using device camera and retrieve image as base64-encoded string
		navigator.camera.getPicture(onCaptureSuccess, onCaptureFail, { quality : 30, destinationType: destinationType.DATA_URL });
	}
	
	function findPhoto() {
	    navigator.camera.getPicture(onNavigateSuccess, onCaptureFail, { quality: 30, destinationType: destinationType.FILE_URI, sourceType: pictureSource.SAVEDPHOTOALBUM });
	}
	
    $(document).ready(function () {
	    document.addEventListener('deviceready', onDeviceReady, false);
    
		$("#camera").click(capturePhoto);
		$("#library").click(findPhoto);
	});
});
