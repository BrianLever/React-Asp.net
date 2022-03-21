; (function ($) {
    $.fn.extend({
        drugOfChoiceMultistep: function (options) {
            var defaultOptions = {
                baseUrl: "api/",
                selectItemSelector: ".drug-of-choice select",
                Dast10Level: 1 /* if Dast10Level is 0, disable selecting drug of choice */
            };


            options = $.extend(defaultOptions, options);
            var container = this;

            function getAllControls() {
                return $(options.selectItemSelector, container);
            }

            function getPrimaryCtrl() {
                return getAllControls().filter('[id$="Primary"]');
            }
            function getSecondaryCtrl() {
                return getAllControls().filter('[id$="Secondary"]');
            }
            function getTertiaryCtrl() {
                return getAllControls().filter('[id$="Tertiary"]');
            }


            function setControlEnabled(ctrl, enabled) {

                /* override enable state if Dast-10 is negative */
                if (!checkAllowEditMode()) enabled = false;


                ctrl.prop("disabled", !enabled);

                if (!enabled) {
                    ctrl.val(0);
                }
            }

            function setPrimaryEnabled(enabled) {
                setControlEnabled(getPrimaryCtrl(), enabled);
            }

            function setSecondaryEnabled(enabled) {
                setControlEnabled(getSecondaryCtrl(), enabled);
            }

            function setTertiaryEnabled(enabled) {
                setControlEnabled(getTertiaryCtrl(), enabled);
            }

            function getControlValue(ctrl) {
                return ctrl.val() || 0;
            }

            function getPrimaryValue() {
                return getPrimaryCtrl().val() || 0;
            }
            function getSecondaryValue() {
                return getControlValue(getSecondaryCtrl());
            }


            function onItemSelected() {
                var el = $(this);

                let isPrimary = el.prop("id").endsWith("Primary");
                let isSecondary = el.prop("id").endsWith("Secondary");

                if (isPrimary) {
                    onPrimaryChanged();
                }
                else if (isSecondary) {
                    onSecondaryChanged();
                }
            }

            function checkAllowEditMode() {
                return options.Dast10Level > 0;
            }

            function onPrimaryChanged() {
                if (getPrimaryValue() === "0") {
                    setSecondaryEnabled(false);
                    setTertiaryEnabled(false);
                }
                else {
                    setSecondaryEnabled(true);
                    onSecondaryChanged();
                }

                filterControlValues();

            }

            function onSecondaryChanged() {
                if (getSecondaryValue() === "0") {
                    setTertiaryEnabled(false);
                }
                else {
                    setTertiaryEnabled(true);
                    filterControlValues();
                }
                
            }

            function filterControlValues() {
                console.debug("[drugOfChoiceMultistep] filterControlValues called.");

                var controls = [getPrimaryCtrl(), getSecondaryCtrl(), getTertiaryCtrl()];
                var selectedItems = [];

                
                controls.forEach(function (el) {
                    let value = el.val();

                    //enable all options
                    $("option", $(el)).removeAttr("disabled", "disabled");

                    //disable previously selected options
                    selectedItems.forEach(function (id) {
                        $("option[value=" + id + "]", $(el)).attr("disabled", "disabled");

                        if (value === id) {
                            console.log("reseting control");
                            el.val(0);
                            value = 0;
                        }
                    });

                    //add current value to the list
                    if (value > 0) {
                        selectedItems.push(value);
                    }

                });

                //move item #3 to #2 if #2 was reset
                if (getSecondaryCtrl().val() === "0" && getTertiaryCtrl().val() !== "0") {
                    getSecondaryCtrl().val(getTertiaryCtrl().val());
                    setTertiaryEnabled(false);
                    console.debug("[drugOfChoiceMultistep] swapping #3 and #2.");
                }

            }

        

            function onInit() {

                if (!checkAllowEditMode()) {
                    setPrimaryEnabled(false);
                    setSecondaryEnabled(false);
                    setTertiaryEnabled(false);

                }
                else {
                    getAllControls().change(onItemSelected);
                    onPrimaryChanged();
                }
            }

            onInit();
        }
    });

})(jQuery);