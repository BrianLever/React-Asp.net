/* public functions to validate field */
;
var bhsDemographicsPageValidator = {
    "validateTribe": function (source, args) {
        args.IsValid = true;

        var options = {
            AmericanIndianId: "1",
            AlaskaNativeId: "7"
        };

    
        var raceDropDownListCtrlId = "#" + source.id.replace("vldTribalAffiliation", "ddlRace");

        var raceEl = $(raceDropDownListCtrlId).children("option:selected");
        var selectedRaceId = raceEl.val();

        console.log("[bhsDemographicsPageValidator] selectedRaceId: " + selectedRaceId);
        console.log("[bhsDemographicsPageValidator] args.Value: " + args.Value);

        var isRequired = selectedRaceId === options.AmericanIndianId || selectedRaceId === options.AlaskaNativeId;

        if (args.Value === "" && isRequired) {
            args.IsValid = false;
            //treatmentDescriptionCtrl.addClass("invalid");
        }
        else {
            //treatmentDescriptionCtrl.removeClass("invalid");
        }
    }
};

/* Control widget's code */

; (function ($) {
    $.fn.extend({
        bhsDemographicCtrl: function (options) {
            var defaultOptions = {
                tribalAffiliationCtrlId: "",
                raceDropDownListCtrlId: "",
                AmericanIndianId: "1",
                AlaskaNativeId: "7"
            };
            options = $.extend(defaultOptions, options);
            var container = this;

            function getRaceListCtrl() {
                return $("#" + options.raceDropDownListCtrlId, container);
            }
            function getTribalFormLabelRequiredIndicatorCtrl() {
                return $("#" + options.tribalAffiliationCtrlId + " span.ast", container);
            }



            function onRaceChanged(el) {
                var selectedEl = $(el).children("option:selected");
                var selectedRaceId = selectedEl.val();

                console.log("[bhsDemographicCtrl] selectedRaceId: " + selectedRaceId);

                makeTribalRequired(selectedRaceId === options.AmericanIndianId || selectedRaceId === options.AlaskaNativeId);
            }

            function Subscribe() {
                getRaceListCtrl().on("change", function () { onRaceChanged(this); });
            }


            function makeTribalRequired(requiredFlag) {
                if (requiredFlag)
                    getTribalFormLabelRequiredIndicatorCtrl().show();
                else

                    getTribalFormLabelRequiredIndicatorCtrl().hide();
            }

            function onInit() {
                Subscribe();

                onRaceChanged(getRaceListCtrl());
            }

            onInit();
        }
    });

})(jQuery);