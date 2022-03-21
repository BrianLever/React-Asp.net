; (function ($) {
    $.fn.extend({
        nextVisitSchedule: function (options) {
            var defaultOptions = {
                baseUrl: "api/",
                newDateCtrlId: "dtNewVisitDate",
                followUpCtlId: "ddlThirtyDatyFollowUpFlag",
                followUpDateCtrlId: "dtFollowUpDate",
                saveChangesEvent: "saveChangesClicked",
                followUpDateContainerId: "follow-up-date",
                resetFollowUpDateToDefaultSelector: "span.reset-button a",
                idSeperator: "_",
                defaultFollowUpDateInDays: 30,
                isCompleted: false
            };

            var container = this;
            var currentFollowUpFlagState = false;
            var disableFollowUpChange = false;
           
            options = $.extend(defaultOptions, options);

            function getFollowUpFlagSelector() {
                return "select[id$=" + options.followUpCtlId + "]";
            }
            function getNewDateSelector() {
                return "input[type=text][id$=" + options.newDateCtrlId + "]";
            }
            function getFollowUpDateSelector() {
                return "input[type=text][id$=" + options.followUpDateCtrlId + "]";
            }
            function getResetButtonElement() {
                return $(getFollowUpDateContainerSelector() + " " + options.resetFollowUpDateToDefaultSelector);
            }

            function getFollowUpDateContainerSelector() {
                return "#" + options.followUpDateContainerId;
            }

            function onNewVisitDataChanged() {

                if (disableFollowUpChange) { return; } //no changes when form is complete;
                var element = $(this);

                var value = element.val();
                console.debug("[schedule] date changes. Value: " + value);

                if (value === "") {
                    disableFollowUp();
                }
                else {
                    enableFollowUp();
                }
            }

            function onFollowUpFlagChanged() {

                //if completed, does not allow to make any changes
               
                var element = $(this);
                console.debug("[schedule] onFollowUpFlagChanged event triggered");

                var value = element.val();
                console.debug("[schedule] follow up flag changed. Value: " + value);

                if (value == 1) {
                    if (!currentFollowUpFlagState) {
                        //previosly it was set to false and now we set to defaut
                        setFollowUpDateToDefault();
                    }

                    toggleFollowUpDateVisibility(true);
                    currentFollowUpFlagState = true;
                }
                else { //set no follow up
                    toggleFollowUpDateVisibility(false);
                    currentFollowUpFlagState = false;
                }
            }

            function setFollowUpDateToDefault() {
                console.debug("[schedule] called setFollowUpDateToDefault");
                var visitDate = $(getNewDateSelector()).val();
                setFollowUpDateValue(visitDate);
            }


            function setFollowUpDateToEmpty() {
                console.debug("[schedule] called setFollowUpDateToEmpty");
                $(getFollowUpDateSelector()).val("");
            }

            function setFollowUpDateValue(visitDate) {
                console.debug("[schedule] callied setFollowUpDateValue with " + visitDate);

                if (visitDate === "") {
                    console.warn("[schedule] new visit date is empty.");

                    return;
                }

                var visitDateVal = new Date(visitDate);
                console.debug("[schedule] visitDateVal " + visitDateVal);

                if (!visitDateVal) {
                    console.warn("[schedule] failed to calculate follow up date. visit date is not correct date value: " + visitDate);
                }

                var followUpDateVal = new Date(visitDateVal);
                followUpDateVal.setDate(followUpDateVal.getDate() + parseInt(options.defaultFollowUpDateInDays));
                console.debug("[schedule] followUpDateVal: " + followUpDateVal);

                var followUpDateString = (followUpDateVal.getMonth() + 1) + "/" + followUpDateVal.getDate() + "/" + followUpDateVal.getFullYear();
                var followUpDateEl = $(getFollowUpDateSelector()).val(followUpDateString);

            }

            function toggleFollowUpDateVisibility(visible) {
                var el = $(getFollowUpDateContainerSelector());

                console.debug("[schedule] toggleFollowUpDateVisibility. id: " + el.prop("id"));
                if (visible) {
                    el.removeClass("hidden");
                    console.debug("[schedule] show follow up date");
                }
                else {
                    el.addClass("hidden");
                    console.debug("[schedule] hide follow up date");
                }
            }

            function initFollowUp() {
                console.debug("[schedule] initializing schedule");

                var dateEl = $(getNewDateSelector());
                var followUpFlagEl = $(getFollowUpFlagSelector());
                currentFollowUpFlagState = followUpFlagEl.val() == 1;

                disableFollowUpChange = options.isCompleted && followUpFlagEl.val() == 1;

                dateEl.change(onNewVisitDataChanged);
                
                console.debug("[schedule] disableFollowUpChange: " + disableFollowUpChange);

                if (!disableFollowUpChange) {
                    $(getFollowUpFlagSelector()).change(onFollowUpFlagChanged);
                }

                getResetButtonElement().click(setFollowUpDateToDefault);
                
                followUpFlagEl.prop("disabled", disableFollowUpChange || dateEl.val() == "");
                //init follow up date control visibility
                onFollowUpFlagChanged.call(followUpFlagEl[0]);
            }

            function enableFollowUp() {
                console.debug("[schedule] enabling schedule");
                var el = $(getFollowUpFlagSelector());

                var curDisabledState = el.prop("disabled");
                if (curDisabledState === true) {
                    setFollowUpDateToEmpty();
                }

                el.prop("disabled", false);
                
            }

            function disableFollowUp() {
                console.debug("[schedule] disabling schedule");
                var el = $(getFollowUpFlagSelector()).val(0);
                el.val(0);
                el.prop("disabled", true);
            }


            function init() {
                console.debug("[schedule] is complete: " + options.isCompleted);

                initFollowUp();
            }

            function pageLoaded() {
                init();
            }

            window.Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);

        }
    });

})(jQuery);