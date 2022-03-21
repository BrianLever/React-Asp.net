; (function ($) {
    $.fn.extend({
        findPatientAddressCtrl: function (options) {
            var defaultOptions = {
                baseUrl: "api/",
                patientAddressCtrlId: "",
                patientSearchResultCtrlId: "",
                patientEhrCtrlId: "hdnPatientEhr",
                phoneNumberCtlId: "ccPhone",
                streetAddressCtrlId: "txtAddress",
                cityCtrlId: "txtCity",
                stateCtrlId: "ddlState",
                zipCodeCtrlId: "txtZipCode",
                jsonDataHiddenId: "hdnItemData",
                selectBtnId: "btnSelect",
                onItemsUpdatedEvent: "onItemsUpdated",
                idSeperator: "_"
            };

            var container = this;
            var patientListContainer = $(getPatientSearchContainerSelector());
            var patientFormContainer = $(getPatientAddressCtrlSelector());


            options = $.extend(defaultOptions, options);

            function getSelectButtonSelector()
            {
                return ":button[id*=" + options.selectBtnId + "]";
            }
            function getJsonFieldSelector(lineId) {
                return "input[type=hidden][id$='" + options.jsonDataHiddenId + options.idSeperator + lineId + "']";
            }

            function getPatientSearchContainerSelector()
            {
                return "#" + options.patientSearchResultCtrlId;
            }
            function getPatientAddressCtrlSelector() {
                return "#" + options.patientAddressCtrlId;
            }

            function setPhone(value)
            {
                var regex = /\(?(\d{3})\)?[-\s]?(\d{3})[ -]?(\d+)/i;
                //value format "760 741-5766"
                
                console.debug("patient phone: " + value);
                var matches = (value || "").match(regex);
                console.debug("patient match groups: " + matches);

                
                matches = (matches || new Array(4));
                for (var index = 0; index < 3; index++) {
                    var el = $("input[type=text][id$=" + options.phoneNumberCtlId + options.idSeperator + "ctl0" + index + "]", patientFormContainer);

                    console.debug("selected phone element id: " + el.prop("id"));
                    console.debug("patient phone group " + index + ": " + matches[index + 1]);

                    el.val(matches[index+1] || "");
                }
            }

            function setPatientTextFieldValue(id, value) {
                 $("input[type=text][id$=" + id + "]", patientFormContainer).val(value);
            }

            function setStateValue(value) {
                $("select[id$=" + options.stateCtrlId + "]", patientFormContainer).val(value);
            }

            function setEhr(value)
            {
                $("input[type=hidden][id$=" + options.patientEhrCtrlId + "]", patientFormContainer).val(value);
            }
           
                      
            function onPatientSelected()
            {
                var element = $(this);
              
                var id = element.prop("id");
                var lineId = id.substring(id.lastIndexOf("_") + 1);

                var jsonPayload = $(getJsonFieldSelector(lineId), patientListContainer).val();

                console.info("selected patient: " + jsonPayload);

                populatePatientFields($.parseJSON(jsonPayload));
            }

            function populatePatientFields(jsonData)
            {
                setPhone(jsonData.PhoneHome);
                setPatientTextFieldValue(options.streetAddressCtrlId, jsonData.StreetAddress);
                setPatientTextFieldValue(options.cityCtrlId, jsonData.City);
                setPatientTextFieldValue(options.zipCodeCtrlId, jsonData.ZipCode);
                setStateValue(jsonData.StateID);
                setEhr(jsonData.EHR);
            }
            

            function init() {
                pageLoaded();
            }

            function pageLoaded() {
                $(getSelectButtonSelector(), patientListContainer).click(onPatientSelected);
            }
            
            
            
            init();

            window.Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);

        }
    });

})(jQuery);