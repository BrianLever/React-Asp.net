; (function ($) {
    $.fn.extend({
        multipleSelectCheckbox: function (options) {
            var defaultOptions = {
                baseUrl: "api/",
                checkboxItemSelector: ":checkbox",
                clearAllOptionIdTrail: "_all"

            };
            options = $.extend(defaultOptions, options);
            var container = this;

            function getAllCheckboxes() {
                return $(options.checkboxItemSelector, container);
            }

            function setSelectStatusForClearAllElement(checked) {
                $(options.checkboxItemSelector + "[id$=" + options.clearAllOptionIdTrail + "]", container).prop("checked", checked);
            }

            function onItemSelected() {
                var activeElement = this;

                var id = $(activeElement).prop("id");
                var checked = activeElement.checked;


                if (id.endsWith(options.clearAllOptionIdTrail)) {
                    //clear all other options

                    getAllCheckboxes().each(function (index, element) {
                        if (element !== activeElement) {
                            element.checked = false;
                        }
                    });


                    if (!activeElement.checked) {
                        getAllCheckboxes()[0].checked = true;
                    }

                }
                else if (!checked) { /* if this was the only selected element */

                    var selectedElementExists = false;
                    getAllCheckboxes().each(function (index, element) {
                        if (element.checked) {
                            selectedElementExists = true;
                        }
                    });
                    
                    if (!selectedElementExists) {
                        setSelectStatusForClearAllElement(true);
                    }
                }
                else
                {
                    setSelectStatusForClearAllElement(false);
                }
            }

            function onInit() {
                getAllCheckboxes().change(onItemSelected);
            }

            onInit();
        }
    });

})(jQuery);