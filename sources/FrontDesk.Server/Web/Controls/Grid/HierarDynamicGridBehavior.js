/// <reference name="MicrosoftAjax.js"/>

Type.registerNamespace("FrontDesk");
Type.registerNamespace("FrontDesk.Server");
Type.registerNamespace("FrontDesk.Server.Web");
Type.registerNamespace("FrontDesk.Server.Web.Controls");

FrontDesk.Server.Web.Controls.HierarDynamicGridBehavior = function (elemenent) {
    FrontDesk.Server.Web.Controls.HierarDynamicGridBehavior.initializeBase(this, [elemenent]);


    this._collapsedImageUrl = null;
    this._expandedImageUrl = null;
    this._dataKey = null; //parent row data key value

    this._element$delegates = {
        click: Function.createDelegate(this, this._element_onclick)
    };

};

FrontDesk.Server.Web.Controls.HierarDynamicGridBehavior.prototype = {
    initialize: function() {
        FrontDesk.Server.Web.Controls.HierarDynamicGridBehavior.callBaseMethod(this, 'initialize');

        var elt = this.get_element();
        $addHandlers(elt, this._element$delegates);

        var _row = this._getParentElementByTagName(elt, "TR");
        if (_row != null) {
            $addHandlers(_row, this._element$delegates);
        }

    },
    expand: function() {
        this._toggleRow();
    },
    collapse: function() {
        this._toggleRow();
    },
    get_collapsedImageUrl: function() {
        return this._collapsedImageUrl;
    },
    set_collapsedImageUrl: function(value) {
        this._collapsedImageUrl = value;
    },
    get_expandedImageUrl: function() {
        return this._expandedImageUrl;
    },
    set_expandedImageUrl: function(value) {
        this._expandedImageUrl = value;
    },
    get_dataKey: function() {
        return this._dataKey;
    },
    set_dataKey: function(value) {
        this._dataKey = value;
    },
    dispose: function() {
        var elt = this.get_element();
        $clearHandlers(elt);

        var _row = this._getParentElementByTagName(elt, "TR");
        if (_row != null) {
            $clearHandlers(_row);
        }
        FrontDesk.Server.Web.Controls.HierarDynamicGridBehavior.callBaseMethod(this, 'dispose');
    },
    _toggleRow: function() {
        var elt = this.get_element();
        if (elt == null)
            return;

        var state = 1;
        var mainTableTr = this._getParentElementByTagName(elt, "TR");
        //if the hidden row has not already been generated, clone the panel into a new row
        var existingRow = $get(elt.id + "showRow");
        if (existingRow == null) {

            //call server callback method to get the data
            GetRelatedDataFromServer(this.get_dataKey(), this.get_id());
        }
        else {
            var existingRowDiv = existingRow.cells[0].childNodes[0];
            if (existingRowDiv != null) {


                if (existingRowDiv.style.display == "none") {
                    existingRowDiv.style.display = "block";
                    elt.src = this.get_expandedImageUrl();
                    this._addCssClass(mainTableTr, "expanded");
                    state = 1;
                }
                else {

                    existingRowDiv.style.display = "none";
                    elt.src = this.get_collapsedImageUrl();
                    this._removeCssClass(mainTableTr, "expanded");
                    state = 0;

                }
            }
        }
        this._changeRowState(elt, state);
    },
    _changeRowState: function(sender, state) {
        var table = this._getParentElementByTagName(sender, "TABLE");
        if (table != null) {
            var hiddenfield = table.ExpandedClientIDsName;
            var rowStates = document.getElementsByName(hiddenfield)[0].value;

            if (state == 1) {
                if (rowStates.indexOf(sender.id) == -1)
                    rowStates += ", " + sender.id;
            }
            else if (state == 0)
                rowStates = rowStates.replace(sender.id, "");

            document.getElementsByName(hiddenfield)[0].value = rowStates;
        }
    },
    _getParentElementByTagName: function(element, tagName) {

        while (element.tagName != tagName) {
            if (element.parentNode == null) return null;

            element = element.parentNode;

        }
        return element;
    },
    _element_onclick: function(e) {
        /// <summary> 
        /// Handles the click event of the asociated show/hide image
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
        e.preventDefault();
        e.stopPropagation();
        this._toggleRow();
    },
    renderNestedContent: function(respose) {

        var elt = this.get_element();
        if (elt == null)
            return;

        //getting a reference to the table
        var table = this._getParentElementByTagName(elt, "TABLE");

        var mainRowTr = this._getParentElementByTagName(elt, "TR");

        var index = mainRowTr.sectionRowIndex + 1;
        //concatenate name of hidden panel => replace "Icon" from elt.id with "Panel"\n

        var newId = new String(elt.id).replace("Icon", "Panel");

        rowDivName = newId;  //HierarGrid_ReplaceStr(elt.id, "Icon", "Panel");
        //var rowDiv = window.document.getElementById(rowDivName);

        //adding new row to table
        var newRow = table.insertRow(index);
        newRow.id = elt.id + "showRow";
        //adding new cell to row
        var newTD = document.createElement("TD");
        //set the colspan to the number of columns
        var columnCount = table.ColumnCount;
        newTD.colSpan = columnCount;

        var myTD = newRow.appendChild(newTD);
        myTD.className = "details_container";
        //clone Panel into new cell
        var div = document.createElement("div");

        var copy = div; //rowDiv.cloneNode(false);
        copy.innerHTML = respose;


        copy.childNodes[0].style.display = "block";

        myTD.innerHTML = copy.innerHTML;

        elt.src = this.get_expandedImageUrl();
        this._addCssClass(mainRowTr, "expanded");

        state = 1;



        this._changeRowState(elt, state);
    },
    _addCssClass: function(element, className) {
        var currentClassName = new String(element.className);
        var newClassName = "";

        if (currentClassName.indexOf(className, 0) >= 0) {
            return; //class already exists
        }
        else {
            element.className = currentClassName + " " + className;
        }
    },
    _removeCssClass: function(element, className) {
        var currentClassName = new String(element.className);
        var newClassName = "";

        if (currentClassName.indexOf(className, 0) < 0) {
            return; //class does not xists
        }
        else {
            element.className = currentClassName.replace(className, "");
        }
    }
}
FrontDesk.Server.Web.Controls.HierarDynamicGridBehavior.registerClass('FrontDesk.Server.Web.Controls.HierarDynamicGridBehavior', Sys.UI.Behavior);

//if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
