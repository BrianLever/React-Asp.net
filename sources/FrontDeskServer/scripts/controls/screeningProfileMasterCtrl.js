/* Control widget's code */

; (function ($) {
    $.fn.extend({
        screeningProfileMasterCtrl: function (options) {
            var defaultOptions = {
                formObjectId: null,
                menuCtrlLocation: ".AspNet-Menu",
                idTemplatePattern: /\.aspx.*/,
                hideOnEmptyObject: true
            };
            options = $.extend(defaultOptions, options);
            var container = this;

            function getMenuCtrl() {
                return $(options.menuCtrlLocation, container);
            }

            function initMenuItemIds() {
                var menuCrl = getMenuCtrl();

                $("a.AspNet-Menu-Link", menuCrl).each(function (index) {
                    var self = $(this);

                    var href = self.attr("href");
                    href = href.replace(options.idTemplatePattern, ".aspx?id=" + options.formObjectId);
                    self.prop("href", href);
                });
            }

            function hideMenuItems() {
                var menuCrl = getMenuCtrl();
                $("a.AspNet-Menu-Link", menuCrl).hide();
            }

            function onInit() {
                if (options.hideOnEmptyObject) {
                    if (!options.formObjectId || options.formObjectId === "0") {

                        console.debug("[screeningProfileMasterCtrl] calling hideMenuItems...");
                        hideMenuItems();

                        return;
                    }
                }
                initMenuItemIds();
            }

            onInit();
        }
    });

})(jQuery);