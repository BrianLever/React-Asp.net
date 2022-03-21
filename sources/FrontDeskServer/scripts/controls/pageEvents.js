; (function ($) {
    $.fn.extend({
        pageEvents: function (options) {
            var defaultOptions = {
                baseUrl: "api/",
                saveChangesButtonId: "",
                saveChangesEvent: "saveChangesClicked",
                idSeperator: "_"
            };

            var container = this;

            options = $.extend(defaultOptions, options);

            function getSaveChangesButtonSelector() {
                return "#" + options.saveChangesButtonId;
            }

            function saveChangesButtonClicked() {
                console.info("[Events] saveChangesEvent triggered");
                $(window).trigger(options.saveChangesEvent);
             
            }

            function init() {
                $(getSaveChangesButtonSelector()).on("click", saveChangesButtonClicked);
            }

            function pageLoaded() {
                init();
            }
            
            window.Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);
        }
    });

})(jQuery);