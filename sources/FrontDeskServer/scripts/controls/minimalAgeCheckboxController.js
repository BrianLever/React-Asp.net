; (function ($) {
    $.fn.extend({
        minimalAgeCheckboxController: function (options) {
            var defaultOptions = {
                baseUrl: "api/",
                groups: [], /* typeof ScreeningMinimalAgeGroup: { PrimarySectionID: "id", AlternativeSectionID: "id"} */
                checkboxItemSelector: ":checkbox",
                sectionIdCtrlName: "hdnID"

            };
            options = $.extend(defaultOptions, options);
            var container = this;

            function getAllCheckboxes() {
                return $(options.checkboxItemSelector, container);
            }

            function onItemSelected() {
                var activeElement = this;
                if (!activeElement.checked) return;

                var el = $(this);
                var groupId = el.attr("data-group-id");
                if (!groupId) return;
                                
                getCheckboxesForGroup(groupId).each(function (index, element) {
                    if (element !== activeElement) {
                        console.debug("[minimalAgeCheckboxController] change element.checked: " + element.id);

                        if (element.checked) {
                            element.checked = false;

                            setTimeout(function () {
                                $(element).change();
                            }, 10);
                        }
                    }
                });


                if (!activeElement.checked) {
                    //getAllCheckboxes()[0].checked = true;
                }
            }

            function getCheckboxesForGroup(groupId) {
                return $(":checkbox[data-group-id=" + groupId + "]", container);
            }

            function findSectionIdForCheckbox(checkboxEl) {
                var selector = "input[id$='" + options.sectionIdCtrlName + "']";
                var hdnEl = $(checkboxEl).parents("tr").find(selector);
                return hdnEl.val();
            }


            function onInit() {
                getAllCheckboxes().each(function (index, el) {
                    jqueryEl = $(el);

                    //set section id property
                    var sectionId = findSectionIdForCheckbox(jqueryEl);
                    console.debug("[minimalAgeCheckboxController] section id:" + sectionId);
                    jqueryEl.attr("data-item", sectionId);

                    //set group property
                    $.each(options.groups, function (index, el) {
                        if (el.PrimarySectionID === sectionId ||
                            el.AlternativeSectionID === sectionId) {
                            jqueryEl.attr("data-group-id", index);
                            console.debug("[minimalAgeCheckboxController] group Id:" + index);
                        }
                    });

                    jqueryEl.change(onItemSelected);
                });

            }

            onInit();

        }
    });

})(jQuery);