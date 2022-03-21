/// master page function - OnResize
appConfig = {};

function onResize() {
    var browserWindow = new BrowserWindow();
    var iHeight = browserWindow.ClientHeight;
    var iWidth = browserWindow.ClientWidth;
    var headerDiv = document.getElementById("header");
    var footerDiv = document.getElementById("footer");
    var contentDiv = document.getElementById("content");


    //    status = ("Header = " + headerDiv.offsetHeight +" & Footer = " + footerDiv.offsetHeight);
    //    status = ("iHeight = " + iHeight + " & Content = " + contentDiv.offsetHeight);
    try {

        iHeight = iHeight - headerDiv.offsetHeight - footerDiv.offsetHeight;
        if (iHeight > 0) {
            contentDiv.style.minHeight = iHeight + "px";
        }
        $(footerDiv).css("visibility", "visible");
    }
    catch (ex) {
        //do nothing

    }
}
function onResizePrivate() {
    var browserWindow = new BrowserWindow();
    var iHeight = browserWindow.ClientHeight;
    var iWidth = browserWindow.ClientWidth;
    var headerDiv = document.getElementById("HeaderDiv");
    var footerDiv = document.getElementById("FooterDiv");
    var contentDiv = document.getElementById("idContent");
    iHeight = iHeight - headerDiv.offsetHeight - footerDiv.offsetHeight - 1;
    contentDiv.style.height = iHeight + "px";
    contentDiv.style.width = iWidth + "px";
}

window.onkeydown = function (e) {
    if (window.event) return; //ignore for IE
    var keynum = e.which;
    var ammount = 500;
    if (keynum == 34) {//page down
        e.preventDefault();
        document.getElementById('idContent').scrollTop += ammount;
    }
    else if (keynum == 33) {//page up
        e.preventDefault();
        document.getElementById('idContent').scrollTop -= ammount;
    }
}


function BrowserWindow() {       // Function to be used for getting the dimensions of browser window.
    // will return correct size regardless of browser type/version.

    this.ClientWidth = 0;
    this.ClientHeight = 0;
    this.DocumentWidth = 0;
    this.DocumentHeight = 0;
    this.ScrollX = 0;
    this.ScrollY = 0;

    iClientWidth = 0;
    iClientHeight = 0;
    iDocumentWidth = 0;
    iDocumentHeight = 0;
    iScrollX = 0;
    iScrollY = 0;



    // Get the visible size of the browser window
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        iClientWidth = window.innerWidth;
        iClientHeight = window.innerHeight;
    }
    else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        iClientWidth = document.documentElement.clientWidth;
        iClientHeight = document.documentElement.clientHeight;
    }
    else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible
        iClientWidth = document.body.clientWidth;
        iClientHeight = document.body.clientHeight;
    }

    // Get the document size (NOTE: IE6 and Mozilla Strict only)
    {
        iDocumentWidth = document.body.clientWidth;
        iDocumentHeight = document.body.clientHeight;
    }

    // Get the scroll position 
    if (typeof (window.pageYOffset) == 'number') {
        //Netscape compliant
        iScrollX = window.pageXOffset;
        iScrollY = window.pageYOffset;
    }
    else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
        //DOM compliant
        iScrollX = document.body.scrollLeft;
        iScrollY = document.body.scrollTop;
    }
    else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
        //IE6 standards compliant mode
        iScrollX = document.documentElement.scrollLeft;
        iScrollY = document.documentElement.scrollTop;
    }

    this.ClientWidth = iClientWidth;
    this.ClientHeight = iClientHeight;
    this.DocumentWidth = iDocumentWidth;
    this.DocumentHeight = iDocumentHeight;
    this.ScrollX = iScrollX;
    this.ScrollY = iScrollY;
}



//hides all the select menus that overlap the given layer
function HideOverlappingSelects(oLayer) {
    //if this is IE, hide selects that overlap the progress bar
    if ((document.all || (Browser.OS == OS_MAC && Browser.UserAgent == BROWSER_GECKO)) && oLayer) {
        var i;
        var arrSelects = document.getElementsByTagName('SELECT');
        var arrSelectsIgnore = oLayer.getElementsByTagName('SELECT');

        //prevents selects inside the div from being hidden
        for (i = 0; i < arrSelectsIgnore.length; i++) {
            arrSelectsIgnore[i].overlap_ignore = true;
        }

        for (i = 0; i < arrSelects.length; i++) {
            //if the select overlaps the progress bar, and is visible, hide it and mark it
            if (DoesOverlap(oLayer, arrSelects[i]) && !arrSelects[i].overlap_ignore) {
                if (IsObjectVisible(arrSelects[i])) {
                    arrSelects[i].style.visibility = 'hidden';
                    arrSelects[i].overlap_hidden = true; //set expando value
                    arrSelects[i].overlap_hidden_layer = oLayer.id;

                    if (Browser.OS == OS_MAC && Browser.UserAgent == BROWSER_GECKO) {
                        arrSelects[i].style.display = 'none';
                    } else {
                        CreateTempSelectDiv(arrSelects[i]);
                    }

                }
            }
        }
    }
}

//shows the selects hidden by calling HideOverlappingSelects with the same layer
function ShowOverlappingSelects(oLayer) {
    if ((document.all || (Browser.OS == OS_MAC && Browser.UserAgent == BROWSER_GECKO)) && oLayer) {
        var arrSelects = document.getElementsByTagName('SELECT');
        var arrSelectsIgnore = oLayer.getElementsByTagName('SELECT');
        for (var i = 0; i < arrSelects.length; i++) {
            if (arrSelects[i].overlap_hidden) { //read expando value
                if (arrSelects[i].overlap_hidden_layer == oLayer.id) {
                    arrSelects[i].style.visibility = 'visible';
                    if (Browser.OS == OS_MAC && Browser.UserAgent == BROWSER_GECKO) {
                        arrSelects[i].style.display = 'block';
                    } else {
                        RemoveTempSelectDiv(arrSelects[i]);
                    }
                }
            }
        }

        //prevents selects inside the div from being hidden
        for (i = 0; i < arrSelectsIgnore.length; i++) {
            arrSelectsIgnore[i].overlap_ignore = false;
        }
    }
}

//creates a div that looks just like the select object
function CreateTempSelectDiv(oSelect) {
    var oCoords = new ObjCoordinates(oSelect);
    var oTempDiv = document.createElement('<div id="' + oSelect.name + '_tempDiv" style="position:absolute; left:0px; top:0px; width:0px; height:0px; z-index:1"></div>');
    oTempDiv.style.width = oCoords.Width;
    oTempDiv.style.height = oCoords.Height;
    oTempDiv.style.top = oCoords.Top;
    oTempDiv.style.left = oCoords.Left;
    oTempDiv.style.border = "2px inset";
    oTempDiv.style.paddingLeft = "3px";
    oTempDiv.style.paddingTop = "0px";
    oTempDiv.style.fontFamily = 'Arial';

    if (oSelect.style.fontSize == '') {
        oTempDiv.style.fontSize = '11px';
    } else {
        oTempDiv.style.fontSize = parseInt(oSelect.style.fontSize) - 1;
    }

    oTempDiv.style.overflowY = 'scroll';

    if (oSelect.className == '') {
        oTempDiv.style.backgroundColor = "#FFFFFF";
    }

    if (oSelect.multiple) {
        oTempDiv.className = oSelect.className;
        oTempDiv.disabled = true;

        oTempDiv.style.overflowX = 'auto';
        oTempDiv.style.height = oCoords.Height;
        oTempDiv.style.width = oCoords.Width;
        for (var i = 0; i < oSelect.options.length; i++) {
            oTempDiv.innerHTML += oSelect.options[i].text + '<br>';
            if (i >= 5) {
                break;
            }
        }
    } else {

        if (oSelect.selectedIndex >= 0) {
            oTempDiv.innerHTML = oSelect.options[oSelect.selectedIndex].innerHTML;
        }
    }
    document.body.appendChild(oTempDiv);
}

//removes the cooresponding "fake select" div
function RemoveTempSelectDiv(oSelect) {
    var oTemp = document.getElementById(oSelect.name + '_tempDiv');
    if (oTemp) {
        oTemp.removeNode(true);
    }
}

//wrapper object which makes it easy to access the coordinates of an HTML element
function ObjCoordinates(oObj) {
    if (!oObj) {
        return false;
    }

    this.Obj = oObj;

    this.Left = GetIeX(oObj);
    this.Top = GetIeY(oObj);
    this.Right = this.Left + parseInt(oObj.offsetWidth);
    this.Bottom = this.Top + parseInt(oObj.offsetHeight);

    this.Height = this.Bottom - this.Top;
    this.Width = this.Right - this.Left;
}

function GetIeX(element) {
    var xPos = GetOffsetLeft(element);
    var tempElement = GetOffsetParent(element);

    while (tempElement != null) {
        xPos += GetOffsetLeft(tempElement);

        if (tempElement.tagName != "BODY") {
            xPos -= tempElement.scrollLeft;

        }

        tempElement = GetOffsetParent(tempElement);
    }

    return xPos;
}

function GetIeY(element) {
    var yPos = GetOffsetTop(element);
    var tempElement = GetOffsetParent(element);

    while (tempElement != null) {
        yPos += GetOffsetTop(tempElement);

        if (tempElement.tagName != "BODY") {
            yPos -= tempElement.scrollTop;
        }
        tempElement = GetOffsetParent(tempElement);
    }
    return yPos;
}


function ValidateEmail(sEmail, bGetMessage) {
    var regExpObj = /^([a-zA-Z0-9_\+\-\.\']+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/  //'
    if (sEmail.search(regExpObj) != 0) {
        return bGetMessage ? ERR_MSG_VALIDATE_EMAIL : false;
    }
    return bGetMessage ? "" : true;
}

function ValidateMultiEmail(sEmailList, bGetMessage) {
    var arrEmails
    if (sEmailList.indexOf(',') != -1) {
        arrEmails = sEmailList.split(',');
    } else if (sEmailList.indexOf(';') != -1) {
        arrEmails = sEmailList.split(';');
    } else {
        arrEmails = sEmailList.split(',');
    }

    for (var i = 0; i < arrEmails.length; i++) {
        if (!ValidateEmail(arrEmails[i].trim())) {
            return bGetMessage ? ERR_MSG_VALIDATE_EMAIL.replace('#value#', arrEmails[i].trim()) : false;
        }
    }

    return bGetMessage ? "" : true;
}




function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    }
    else {
        window.onload = function () {
            oldonload();
            func();
        }
    }
}

function addEvent(object, func, eventName) {
    var oldfunc = object[eventName];
    if (typeof object[eventName] != 'function') {
        object[eventName] = func;
    }
    else {
        object[eventName] = function () {
            oldfunc();
            return func();
        }
    }
}

function addResizeEvent(func) {
    var oldonresize = window.onresize;
    if (typeof window.onresize != 'function') {
        window.onresize = func;
    }
    else {
        window.onresize = function () {
            oldonresize();
            func();
        }
    }
}


// open window

function openFullSizedWindow(sUrl) {
    var browserWindow = new BrowserWindow();
    var iLeft = Math.ceil((screen.availWidth - browserWindow.ClientWidth) / 2);
    var iTop = Math.ceil((screen.availHeight - browserWindow.ClientHeight) / 2);

    var sFeatures = "height=" + browserWindow.ClientHeight + ", width=" + browserWindow.ClientWidth + ",left=" + iLeft + ",top=" + iTop + ",location=0,resizable =1, scrollbars=0, toolbar=0";
    var oNewWindow = window.open(sUrl, "", sFeatures, false);
    return oNewWindow;
}

/// find child control by TAG
function findDHTMLControl(oParentNode, sChildTagName) {
    var oTargetControl = null;
    if (oParentNode.childNodes != null && oParentNode.childNodes.length > 0) {
        for (var i = 0; i < oParentNode.childNodes.length; i++) {
            if (oParentNode.childNodes[i].tagName == sChildTagName) {
                oTargetControl = oParentNode.childNodes[i];
                break;
            }
        }
    }
    return oTargetControl;
}

/// 
function searchDHTMLControl(oParentNode, sChildTagName) {
    ///<summary>Find child control by TAG.
    ///Search for DHTML control inside oParentNode child elementes. Search using recurcive algorithm.
    ///Returns first elements that is meets the conditions 
    ///</summary>

    var oTargetControl = null;
    if (oParentNode.childNodes != null && oParentNode.childNodes.length > 0) {

        for (var i = 0; i < oParentNode.childNodes.length; i++) {
            if (oParentNode.childNodes[i].tagName == sChildTagName) {
                oTargetControl = oParentNode.childNodes[i];
                break;
            }
            else {
                oTargetControl = searchDHTMLControl(oParentNode.childNodes[i], sChildTagName);
                if (oTargetControl != null) break;
            }
        }
    }
    return oTargetControl;
}




// select gridview item checkbox when user typed quantity into text box
function CheckedIfNonEmpty(sender) {
    var td = sender.parentNode;
    var tr = td.parentNode;
    td = tr.cells[0];

    var span = findDHTMLControl(td, "SPAN");

    if (span != null) {
        var oCheckBox = findDHTMLControl(span, "INPUT");
        if (oCheckBox != null) {
            if (sender.value.length > 0) oCheckBox.checked = true;
            else oCheckBox.checked = false;
        }
    }
}



/* Get max z-index on the page */
///Get max z-index for all child elements
function getMaxChildZIndexes(element) {
    var maxZIndex = 0;
    var elementZIndex = 0;

    if (element.childNodes != null && element.childNodes.length > 0) {
        for (var i = 0; i < element.childNodes.length; i++) {
            elementZIndex = getMaxChildZIndexes(element.childNodes[i]);
            maxZIndex = Math.max(maxZIndex, elementZIndex);
        }
        if (element.style != null)
            maxZIndex = Math.max(new Number(element.style.zIndex), maxZIndex);
    }
    else if (element.style != null) maxZIndex = new Number(element.style.zIndex);

    return maxZIndex;
}

function getMaxZIndex() {
    return getMaxChildZIndexes(document.documentElement);
}


function setElementInnerTextProperty(control, value) {
    ///<summary>Set inner text property that works for IE and Firefox</summary>

    if (document.all) {
        control.innerText = value;
    } else {
        control.textContent = value;
    }

}

function setComboboxSelectedValue(combobox, value) {
    ///<summary>Select combobox option by value. If value not found - select first option</summary>
    ///<parameter name="value" type="string">Value to be searched for</parameter>
    var i, selectedIndex = 0;
    for (i = 0; i < combobox.options.length; i++) {
        if (combobox.options[i].value == value) {
            selectedIndex = i;
            break;
        }
    }
    combobox.selectedIndex = selectedIndex;
}

// ksa 0/16/2008
// limit max number of characters and lines in the text area
function limitTextAreaLength(el, eventObj, maxTextLen, maxLines) {

    var bCancel = false;

    eventObj = getEvent(eventObj);
    var code = getKeyCode(eventObj);

    if (code > 9
        && code != 46// delete
        && code != 27 //esc
        && code != 144 //Num Lock
        && code != 37 // left arrow
        && code != 36 //Home
        && code != 35 // End
    ) {
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

    if (el.value.length > maxTextLen) {
        var newText = el.value.substring(0, maxTextLen);
        el.value = newText;
    }
    return !bCancel;
}

// ksa 0/16/2008
// limit max number of characters and lines in the text area
function limitTextAreaLengthOnMousePaste(el, eventObj, maxTextLen, maxLines) {
    if (el.value.length > maxTextLen) {
        var newText = el.value.substring(0, maxTextLen);
        el.value = newText;
    }
}

function copyToClipboard(el) {
    navigator.clipboard.writeText(el.innerText)
        .then(function () {
            console.log('Async: Copying to clipboard was successful!');
        }, function (err) {
            console.error('Async: Could not copy text: ', err);
            alert("Sorry, but your browser doesn't support copy to clipboard feature. Please select text and press CTRL+C.");

        });

}