/* Public global function for validation */
function editPatientContact_ValidateZipCode(sender, args) {
    args.IsValid = (args.Value.length === 5);
}


; (function ($) {
    $.fn.extend({
        editPatientContactCtrl: function (options) {
            var defaultOptions = {
                baseUrl: "api/",
                changeMessageLocator: "#changeMessage",
                resetButtonLocator: "#resetButton",
                arrowCanvasLocator: "#canvasArrow",
                exportButtonLocator: "#exportButton",
                arrowFillColor: "#4059AD",
                birthdayDateFormat: "MM/dd/yyyy",
                data: null,
                dirtyMsgHtmlTemplate: "<div class=\"left\" id=\"dirty-{{propertyName}}-left\"></div><div class=\"right\" id=\"dirty-{{propertyName}}-right\"><span class=\"dirty-state-label\" >ScreenDox value: {{val}}</span></div>"

            };

            var container = this;
            options = $.extend(defaultOptions, options);

            options.data.Birthday = DateFormat.format.date(options.data.Birthday, options.birthdayDateFormat);

            
            var initialDataJsonString = JSON.stringify(options.data);

            console.debug("data: " + initialDataJsonString);
            var initialData = JSON.parse(initialDataJsonString);
          
            function drawArrow() {
                var canvas = $(options.arrowCanvasLocator);
                var context = canvas.get(0).getContext("2d");

                var size = {
                    width: canvas.width(),
                    height: canvas.height()
                };
                context.canvas.width = size.width;
                context.canvas.height = size.height;


                context.save();
                var fromx = 0;
                var fromy = size.height / 2;
                var tox = size.width;
                var toy = fromy;
                var headlen = 10;

                context.lineCap = "round";
                context.lineWidth = 1;
                context.strokeStyle = context.fillStyle = options.arrowFillColor;

                context.beginPath();
                context.moveTo(fromx, fromy);
                context.lineTo(tox, toy);
                context.closePath();
                context.stroke();

                context.beginPath();
                context.moveTo(tox, toy);
                context.lineTo(tox - headlen, toy - headlen / 2);
                context.lineTo(tox - headlen, toy + headlen / 2);
                context.closePath();
                context.fill();

                context.restore();
            }

            function getChangeMessageControl() {
                return $(options.changeMessageLocator, container);
            }

            function getResetButtonControl() {
                return $(options.resetButtonLocator, container);
            }

            function getNextButtonControl() {
                return $(options.NextButtonLocator, container);
            }

            function setCleanState() {

                console.debug("[editPatientContactCtrl] Setting clean state");
                getChangeMessageControl().css("visibility", "hidden");
                getResetButtonControl().css("display", "none");
            }

            function setDirtyState() {

                console.debug("[editPatientContactCtrl] Setting dirty state");

                getChangeMessageControl().css("visibility", "visible");
                getResetButtonControl().css("display", "block");

            }

            //render label with initial value before modification
            function setModifiedState(ctrl, propertyName) {
                var columnDiv = $(ctrl).closest("div.right");

                var html = options.dirtyMsgHtmlTemplate;
                html = html.replace(/{{propertyName}}/gi, propertyName);
                html = html.replace(/{{val}}/gi, initialData[propertyName]);

                columnDiv.after(html);
            }

            //render label with initial value before modification
            function cleanModifiedState(ctrl, propertyName) {
                $("div#dirty-" + propertyName + "-left").detach();
                $("div#dirty-" + propertyName + "-right").detach();
            }


            function readCurrentState(propertyName, ctrl) {
                var data = options.data;

                if (propertyName) {
                    updateViewModelProperty(propertyName, ctrl);
                }
                else {
                    for (var key in data) {
                        ctrl = $("[data-item=" + key + "]", container);
                        updateViewModelProperty(key);
                    }
                }

                console.debug("[editPatientContactCtrl] data:" + JSON.stringify(data));
            }

            function updateViewModelProperty(key, ctrl) {
                var data = options.data;

                var domEl = ctrl.get(0);
                if (!domEl) return;

                var tagName = domEl.tagName;

                if (tagName === "INPUT") {
                    if (key === "Birthday") {
                        data[key] = DateFormat.format.date(Date.parse(ctrl.val()), options.birthdayDateFormat);
                    }
                    else if (key === "Phone")
                    {
                        var phone = ctrl.val().replace(/\(\)\s+-\s+/gi, "");
                        console.log("phone: " + phone);
                        data[key] = phone;
                    }
                    else {
                        data[key] = ctrl.val();
                    }
                }
                else if (tagName === "SELECT") {
                    if (key === "Birthday") {
                        data[key] = DateFormat.format.date(Date.parse(ctrl.val()), options.birthdayDateFormat);
                    }
                    else {
                        data[key] = ctrl.val();
                    }
                }
            }

            function onDataChanged() {

                var ctrl = $(this);
                var property = ctrl.attr("data-item");

                readCurrentState(property, ctrl);

                if (JSON.stringify(options.data) === JSON.stringify(initialData)) {
                    //no changes
                    setCleanState();
                    cleanModifiedState(ctrl, property);
                }
                else {
                    setDirtyState();

                    cleanModifiedState(ctrl, property);
                    setModifiedState(ctrl, property);
                }

                return true;
            }


            function init() {
                pageLoaded();
            }

            function pageLoaded() {
                drawArrow();
                setCleanState();

                console.debug("[editPatientContactCtrl] pageLoaded event has occured");

                $("input[type='text']", container).on("change", onDataChanged);
                $("select", container).on("change", onDataChanged);
            }

            //init();

            window.Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);

        }
    });

})(jQuery);