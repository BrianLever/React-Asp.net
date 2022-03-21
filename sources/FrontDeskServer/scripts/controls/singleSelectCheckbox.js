; (function ($) {
    $.fn.extend({
        singleSelectCheckbox: function (options) {
            var defaultOptions = {
                baseUrl: "api/",
                checkboxItemSelector: ":checkbox"

            };
            options = $.extend(defaultOptions, options);
            var container = this;

            function getAllCheckboxes() {
                return $(options.checkboxItemSelector, container);
            }

            function onItemSelected() {
                var activeElement = this;

                getAllCheckboxes().each(function (index, element) {
                    if (element != activeElement) {
                        element.checked = false;
                    }
                });


                if (!activeElement.checked) {
                    getAllCheckboxes()[0].checked = true;
                }
            }

            function onInit() {
                getAllCheckboxes().change(onItemSelected);
            }

            onInit();
        }
    });

})(jQuery);