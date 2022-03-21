/* public functions to validate field */
;
var bhsVisitPageValidator= {
    "validateTreatmentDescription": function (source, args) {
        args.IsValid = true;

        var treatmentSelectId = "#" + source.id.replace("vldTreatmentActionDescription", "ddlTreatmentAction");
        var descriptionId = "#" + source.id.replace("vldTreatmentActionDescription", "txtTreatmentActionDescription");

        var treatmentActionCtrl = $(treatmentSelectId);
        var treatmentDescriptionCtrl = $(descriptionId);

        if (args.Value == "" && treatmentActionCtrl.val() != "") {
            args.IsValid = false;
            treatmentDescriptionCtrl.addClass("invalid");
        }
        else {
            treatmentDescriptionCtrl.removeClass("invalid");
        }
    },
    "validateVisitRecommendationDescription": function (source, args) {
        args.IsValid = true;

        var selectId = "#" + source.id.replace("vldNewVisitRecommendationDescription", "ddlNewVisitRecommendation");
        var descriptionId = "#" + source.id.replace("vldNewVisitRecommendationDescription", "txtNewVisitRecommendationDescription");

        var selectedOptionCtrl = $(selectId + " option:selected");
        var descriptionCtrl = $(descriptionId);

        var selectedOptionText = selectedOptionCtrl.text();

        if (args.Value == "" && selectedOptionText.indexOf("Other") >= 0) {
            args.IsValid = false;
            descriptionCtrl.addClass("invalid");

            console.debug("[validation] Control not valie: " + descriptionId);
        }
        else {
            descriptionCtrl.removeClass("invalid");
        }
    }
};