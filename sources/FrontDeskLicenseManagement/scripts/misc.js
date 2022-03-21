var bV = parseInt(navigator.appVersion);
var NS4 = (document.layers) ? true : false;
var IE4 = (document.all && bV >= 4) ? true : false;
var hasDOM = (document.getElementById) ? true : false;

///Alow intput only numeric characters
function IsValidIntegerKey(iKeyCode, EnableMinus) {
    var bCancel = true;
    if (IsValidSystemKey(iKeyCode) ||
        (iKeyCode >= 96 && iKeyCode <= 105) ||
        (iKeyCode >= 48 && iKeyCode <= 57)
        ) {
        bCancel = false;
    }

    if (EnableMinus && (iKeyCode == 109 || iKeyCode == 189)) bCancel = false;
    return bCancel;
}

function IsValidSystemKey(iKeyCode) {
    ///<sumary>Validate if system key was pressed. System key don't impact on the control text value and should be permitted.</summary>
    var bCancel = true;
    if (iKeyCode == 46 ||
        iKeyCode == 37 ||
        iKeyCode == 38 ||
        iKeyCode == 39 ||
        iKeyCode == 40 ||
        iKeyCode == 8 ||
        iKeyCode == 9 ||
        iKeyCode == 13) {
        bCancel = false;
    }
    return !bCancel;
};


function checkINTEGER(el, eventObj, EnableMinus) {
    var bCancel = true;
    if (EnableMinus == null) EnableMinus = false;
    eventObj = getEvent(eventObj);
    var iKeyCode = getKeyCode(eventObj);
    var bCtrlKey = eventObj.ctrlKey;

    if (bCtrlKey) bCancel = false
    else { bCancel = IsValidIntegerKey(iKeyCode, EnableMinus); }

    if (bCancel) cancelEvent(eventObj);
    return !bCancel;

}


function checkFLOAT(el, eventObj, EnableMinus) {
    if (EnableMinus == null) EnableMinus = false;
    var bCancel = true;

    eventObj = getEvent(eventObj);
    var iKeyCode = getKeyCode(eventObj);
    var IsCtrlKey = eventObj.ctrlKey;

    if (IsCtrlKey) bCancel = false;
    else {
        bCancel = IsValidIntegerKey(iKeyCode, EnableMinus);

        if ((iKeyCode == 190 || iKeyCode == 188 || iKeyCode == 110) &&
            el &&
            el.value && el.value.indexOf(".") < 0

        ) {
            bCancel = false;
        }

    }
    //alert(bCancel);
    if (bCancel) cancelEvent(eventObj);
    return !bCancel;
}


function checkFLOATLIMITED(el, eventObj, digits, EnableMinus) {

    eventObj = getEvent(eventObj);
    var iKeyCode = getKeyCode(eventObj);
    var IsCtrlKey = eventObj.ctrlKey;
    var bCancel = true;
    if (IsCtrlKey) bCancel = false;
    else {
        bCancel = !checkFLOAT(el, eventObj, EnableMinus);
        if (!bCancel && !IsValidSystemKey(iKeyCode)) {
            // if still valid check for how many digits after decimal point
            var curPos = getSelectionStart(el);
            var pointPos = el.value.indexOf(".");
            //check that digits number not more then specified in parameter
            if (pointPos >= 0) {
                var sDigits = el.value.substr(pointPos + 1);
                if (sDigits.length >= digits && curPos > pointPos) {
                    bCancel = true;
                }
            }
            if (bCancel) cancelEvent(eventObj);
        }
    }
    return !bCancel;
}

function onSearchFieldKeyPressed(el, eventObj, okBtnId) {
    var cancel = false;

    eventObj = getEvent(eventObj);
    var iKeyCode = getKeyCode(eventObj);
    var IsCtrlKey = eventObj.ctrlKey;
    var bCancel = true;
    if (!IsCtrlKey) {
        if (iKeyCode == 13) { //Enter
            //trigger okBtn
            var okBtn = $get(okBtnId);
            if (okBtn != null) {
                okBtn.click();
                cancel = true;
            }

        }
        else if (iKeyCode == 27) { //Esc
            el.value = "";
            cancel = true;
        }

    }

    return !cancel;
}


/*--- CARET position ---------------------------*/
function getSelectionStart(o) {
    ///<summary>Get textbox selection start - current cursor position</summary>
    ///<paramerter>Textbox object</parameter>
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}

function getSelectionEnd(o) {
    ///<summary>Get textbox selection end - current cursor position</summary>
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveStart('character', -o.value.length)
        return r.text.length
    } else return o.selectionEnd
}




//Input only float numbers with letters
function IsValidAlphaIntegerKey(eventObj, EnableMinus) {
    var bCancel = true;
    var iKeyCode = getKeyCode(eventObj);
    var IsCtrlKey = eventObj.ctrlKey;
    var IsShiftKey = eventObj.shiftKey;


    //alert(iKeyCode);
    if (IsCtrlKey) bCancel = false;
    //else if(IsShiftKey)  bCancel = false;    
    else if (
        (iKeyCode >= 96 && iKeyCode <= 105) ||
        (iKeyCode >= 48 && iKeyCode <= 57) ||
        (iKeyCode >= 65 && iKeyCode <= 90) ||
        iKeyCode == 46 ||
        iKeyCode == 48 ||
        iKeyCode == 37 ||
        iKeyCode == 39 ||
        iKeyCode == 8 ||
        iKeyCode == 9 ||
        iKeyCode == 13) {
        bCancel = false;
    }

    if (EnableMinus && (iKeyCode == 109 || iKeyCode == 189)) bCancel = false;
    return bCancel;
}

function checkAlphaFloat(el, eventObj, EnableMinus) {
    if (EnableMinus == null) EnableMinus = false;
    var bCancel = true;

    eventObj = getEvent(eventObj);
    var iKeyCode = getKeyCode(eventObj);
    var IsCtrlKey = eventObj.ctrlKey;
    var IsShiftKey = eventObj.shiftKey;

    if (IsCtrlKey) bCancel = false;
    //else if(IsShiftKey)  bCancel = false;    
    else {
        bCancel = IsValidAlphaIntegerKey(eventObj, EnableMinus);
        if ((iKeyCode == 190 || iKeyCode == 188 || iKeyCode == 110) &&
            el &&
            el.value && el.value.indexOf(".") < 0
        ) {
            bCancel = false;
            //            alert(iKeyCode);
        }

    }
    //alert(bCancel);
    if (bCancel) cancelEvent(eventObj);
    return !bCancel;
}

function getEvent(e) {
    if (window.event)
        return window.event;
    else
        return e;
}

function cancelEvent(evt) {
    if (evt.preventDefault)
        evt.preventDefault();
    if (evt.stopPropagation)
        evt.stopPropagation();
    if (window.event)
        window.event.returnValue = false;

    return false;
}

function getKeyCode(e) {
    var code = 0;
    if (window.event)
        code = window.event.keyCode;
    else if (e) {
        if (e.which > 0) {
            code = e.which;
        }
        else if (e.keyCode != 0) { code = e.keyCode; }
        else { code = e.charCode; }
    }
    return code;
}


//set focus on form element
function setFocus(sControlID, sType) {
    var i = 0;
    var ctrlElement;
    for (i = 0; i < document.forms[0].elements.length; i++) {
        ctrlElement = document.forms[0].elements[i];
        if (ctrlElement.tagName == "INPUT" && ctrlElement.type == sType && ctrlElement.id.indexOf(sControlID) > 0) {
            ctrlElement.focus();
            break;
        }
    }
}

// Filter Enter Key
// Ignore Enter Key
function disableEnterKey(el, eventObj) {
    eventObj = getEvent(eventObj);

    var bCancel = (eventObj.keyCode == 13) ? true : false;

    if (bCancel) cancelEvent(eventObj);
    return !bCancel;
}




function openPopupWindow(width, height, title, pagename, id, bCloseOnClick) {

    var left = (screen.availWidth - width) / 2;
    var top = (screen.availHeight - height) / 2;
    if (bCloseOnClick == null) bCloseOnClick = false;

    var sOptions = "height=" + height + ",width=" + width + ",top=" + top + ",left=" + left + ",status=no,toolbar=no,menubar=no,location=no"
    var sLink = pagename;
    if (sLink.indexOf("?") > 0) sLink += "&";
    else sLink += "?";

    sLink += "id=" + id;
}





function openPopupDialog(width, height, title, pagename, id, bCloseOnClick, closePopupText) {
    var left = (document.body.clientWidth - width) / 2;
    var top = (document.body.clientHeight - height) / 2;
    if (bCloseOnClick == null) bCloseOnClick = false;

    //var sOptions = "height=" + height + ",width=" + width + ",top=" + top + ",left=" + left + ",status=no,toolbar=no,menubar=no,location=no"

    var sLink = pagename;
    if (id != null && id > 0) {
        if (sLink.indexOf("?") > 0) sLink += "&";
        else sLink += "?";

        sLink += "id=" + id;
    }


    var oDiv;

    oDiv = document.getElementById("popupDivID");
    if (oDiv != null) oDiv.removeNode(true);
    //create new div

    oDiv = document.createElement("Div");
    oDiv.id = "popupDivID";
    oDiv.style.height = height + 30 + "px";
    oDiv.style.width = width + "px";
    oDiv.style.left = left + "px";
    oDiv.style.top = top + "px";
    oDiv.style.backgroundColor = "#FFFFFF";
    oDiv.style.position = "absolute";
    oDiv.style.zIndex = 2000;
    oDiv.style.textAlign = "center";
    oDiv.className = "border pad5";
    oDiv.onclick = function() {
        this.parentNode.removeChild(this);
    }

    var dtDate = new Date();
    sLink += "&" + dtDate.valueOf();
    //alert(sLink);

    var oImg = document.createElement("img");
    oImg.id = "popupDivID_Image";
    //oImg.src = "images/about.jpg";
    oImg.src = sLink;
    oImg.alt = "";
    if (closePopupText != null) oImg.title = closePopupText;

    var oSpan = document.createElement("span");
    var sDivContent = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:100%\">" + "<tr>";
    sDivContent += "<td class=\"tpad10 bold\" align=\"center\">";

    var closeLinkText = 'Close';
    if (CityBoard.IMN.Web.Strings != null) {
        var strings = new CityBoard.IMN.Web.Strings();
        closeLinkText = strings.Labels['Pupup_CloseWindowText'];
    }

    sDivContent += String.format("<a class='link' href=\"javascript:void(0)\" onclick=\"var oDiv=document.getElementById('popupDivID'); oDiv.parentNode.removeChild(oDiv); return false;\">[{0}]</a>", closeLinkText);
    sDivContent += "</td>";
    sDivContent += "</tr>";
    sDivContent += "</table>";

    //oDiv.innerHTML = sDivContent;
    oSpan.innerHTML = sDivContent;

    oDiv.appendChild(oImg);
    oDiv.appendChild(oSpan);
    document.body.appendChild(oDiv);
    var sCode = 'var obj = document.getElementById("popupDivID_Image"); if(obj!= null){obj.src="' + sLink + '"}';
    //alert(sCode)
    setTimeout(sCode, 500);


}



function convertNameToClientID(clientName) {
    var i;
    var clientID = '';
    for (i = 0; i < clientName.length; i++) {
        if (clientName.charAt(i) == '$') clientID += '_';
        else clientID += clientName.charAt(i);
    }
    return clientID
}
////////////////////////////////////////////////////
// Cookie
////////////////////////////////////////////////////
function setCookie(name, value, expDate) {
    if (expDate == -1)
        expDate = new Date(-1);
    var expires = (expDate != null ? "; expires=" + expDate.toGMTString() : "");
    document.cookie = name + "=" + value + expires + "; path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ')
            c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0)
            return c.substring(nameEQ.length, c.length);
    }
    return null;
}

// ksa 0/16/2008
// limit max number of characters and lines in the text area
function limitTextAreaLength(el, eventObj, maxTextLen, maxLines) {

    var bCancel = false;

    eventObj = getEvent(eventObj);
    var code = getKeyCode(eventObj);

    if (code > 9) {
        var text = new String(el.value);
        var textLen = text.length;
        if (textLen + 1 > maxTextLen) bCancel = true;
        else { //check max lines. Calculate number of \n
            for (var i = 0, newLineCount = 0; i < textLen; i++) {
                if (text.charCodeAt(i) == 13) newLineCount++;
            }
            if (newLineCount > maxLines || (code == 13 && newLineCount >= maxLines)) bCancel = true;
        }
    }
    if (bCancel) cancelEvent(eventObj);
    return !bCancel;
}
