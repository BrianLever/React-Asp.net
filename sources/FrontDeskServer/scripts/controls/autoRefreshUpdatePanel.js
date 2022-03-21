; (function ($) {
    $.fn.extend({
        autoRefreshUpdatePanel: function (options) {
            var defaultOptions = {
                loggingFlag: "autorefresh",
                idSeperator: "_",
                onAutoRefreshEvent: "onAutoRefresh",
                onGridContentChangedEvent: "HierarDynamicGrid.Loaded",
                turnOffButtonId: null,
                turnOffValueId: null,

                toggleOffButtonTextPrefix: "Turn Auto-refresh ",
                timerInterval: 30000,
                enableAutoRefresh: true
            };


            var container = this;
            var lostFocusTime = null;

            options = $.extend(defaultOptions, options);
            configLogSource(options.loggingFlag, false);


            function getTriggerButtonSelector() {
                return "#" + options.triggerButtonId;
            }

            function getTurnOffButtonSelector() {
                return "#" + options.turnOffButtonId;
            }

            function getTurnOffValueIdSelector() {
                return "#" + options.turnOffValueId;
            }

            function toggleAutoRefreshState() {
                logDebug("Calling toggleAutoRefreshState.", options.loggingFlag);

                options.enableAutoRefresh = !options.enableAutoRefresh;

                setAutoRefreshState();
                setAutoRefreshButtonText();
            }

            function getAutoRefreshState() {
                var state = parseInt($(getTurnOffValueIdSelector()).val());
                state = isNaN(state) ? true : state;

                return state === 0 ? false : true;
            }

            function setAutoRefreshState() {
                $(getTurnOffValueIdSelector()).val(options.enableAutoRefresh ? "1" : "0");
            }

            function setAutoRefreshButtonText() {
                var text = options.toggleOffButtonTextPrefix + (options.enableAutoRefresh ? "Off" : "On");

                logDebug("Calling setAutoRefreshButtonText. text: " + text, options.loggingFlag);

                $(getTurnOffButtonSelector()).text(text);
            }

            function init() {

                options.enableAutoRefresh = getAutoRefreshState();
                setAutoRefreshButtonText();
                logDebug("Init enableAutoRefresh: " + options.enableAutoRefresh, options.loggingFlag);

                pageLoaded();

                $(getTurnOffButtonSelector()).on("click", toggleAutoRefreshState);
            }

            //window lifecycle events
            function windowHasFocus() {

                logDebug("window received focus. Refresh and resumer timer.", options.loggingFlag);

                var nowDate = new Date();
                var timeFromLostFocus = (nowDate - lostFocusTime);

                if (timeFromLostFocus > options.timerInterval * 0.7) {
                    $(window).trigger(options.onAutoRefreshEvent);
                }
                else {
                    logDebug("no refresh after window focus return. time spent: " + timeFromLostFocus / 1000 + "sec", options.loggingFlag);
                }

            }

            function windowLostFocus() {
                logDebug("window lost focus. Stop timer.", options.loggingFlag);
                lostFocusTime = new Date();
                cancelAutoUpdate();
            }

            //refresh lifecycle events
            function pageLoaded() {

                logDebug("pageLoaded. Timer: " + container.timer, options.loggingFlag);

                resetTimer();
            }

            function onAjaxComplete() {
                resetTimer();
            }

            function resetTimer() {
                cancelAutoUpdate();

                container.timer = setTimeout(refresh, options.timerInterval);
                //logDebug("restarted timer. Timer: " + container.timer, options.loggingFlag);
            }

            function refresh() {
                if (options.enableAutoRefresh) {
                    logDebug("triggering grid refresh. Timer: " + container.timer, options.loggingFlag);

                    $(getTriggerButtonSelector()).click();
                }
                else {
                    logDebug("Autorefresh is disabled. no action.", options.loggingFlag);
                }
            }

            function cancelAutoUpdate() {
                if (container.timer) {

                    clearInterval(container.timer);

                    //logDebug("timer stopped. Timer:" + container.timer, options.loggingFlag);

                }
            }

            init();

            window.Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);
            $(window).on(options.onGridContentChangedEvent, onAjaxComplete);
            $(window).on(options.onAutoRefreshEvent, refresh);
            //$(window).on("focus", windowHasFocus);
            //$(window).on("blur", windowLostFocus);
            $(window).on("click", resetTimer);
            $(container).on("mousemove", resetTimer);
            $(window).on("keypress", resetTimer);
        }
    });

})(jQuery);