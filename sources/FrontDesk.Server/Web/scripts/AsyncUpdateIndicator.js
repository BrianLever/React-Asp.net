// JScript File
var prm = null;
function initAsyncHandlers() {
    prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
//    prm.add_endRequest(AsyncUpdateIndicator_EndRequest);
}

function InitializeRequest(sender, args) {
    //    if (prm.get_isInAsyncPostBack()) {
    //        args.set_cancel(true);
    //    }

    //set control in the center
    var progressBarCtrl = findUpdateProgressControl("DIV", "progressBar"); //document.getElementById('progressBar');
    var progressBarText = findUpdateProgressControl("DIV", "progressBarText"); //document.getElementById('progressBarText');
    
    var browserWindow = new BrowserWindow();
    var iLeft = Math.floor((browserWindow.ClientWidth - 400) / 2);
    var iTop = Math.floor((browserWindow.ClientHeight - 60) / 2);
    progressBarCtrl.style.left = iLeft + "px";
    progressBarCtrl.style.top = iTop + "px";

    //progressBarCtrl.style.visibility = 'visible';
}

function AsyncUpdateIndicator_EndRequest(sender, args) {
}

function findUpdateProgressControl(tagName, idPostfix) {
    var divs = document.getElementsByTagName(tagName);

    for (var i = 0; i < divs.length; i++) {
        var d = divs[i];
        var sID = new String(d.id);
        if (sID.endsWith(idPostfix)) {
            return d;
        }
    }
    return null;
}


