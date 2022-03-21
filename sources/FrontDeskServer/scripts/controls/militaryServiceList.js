; (function ($) {
    $.fn.extend({
        militaryServiceList: function (options) {
            var defaultOptions = {
                baseUrl: "api/",
                checkboxItemSelector: ":checkbox",
                allowedAdditionalItem: []
            };

            defaultOptions.allowedAdditionalItem[0] = null;
            defaultOptions.allowedAdditionalItem[1] = 3;
            defaultOptions.allowedAdditionalItem[2] = 3;
            defaultOptions.allowedAdditionalItem[3] = null;

            options = $.extend(defaultOptions, options);
            var container = this;

            function getAllCheckboxes() {
                return $(options.checkboxItemSelector, container);
            }

            function onItemSelected() {
                var activeElement = this;

                var currentSelectedItems = [];

                getAllCheckboxes().each(function (index, element) {
                    if (element.checked) {
                        var id = parseInt(element.id.substring(element.id.lastIndexOf("_")+1));
                        currentSelectedItems.push(id);
                    }
                });

                //console.log("currentSelectedItems: " + currentSelectedItems);

                if (currentSelectedItems.length > 1) {
                    var additionalItem = options.allowedAdditionalItem[currentSelectedItems[0]];

                    if (additionalItem !== currentSelectedItems[1]) {
                        getAllCheckboxes().each(function (index, element) {
                            if (element !== activeElement) {
                                element.checked = false;
                            }
                        });
                    }

                }
                
                if (currentSelectedItems.length === 0) {

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