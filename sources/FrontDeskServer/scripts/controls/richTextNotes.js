; (function ($) {
    $.fn.extend({
        richTextNotes: function (options) {
            var defaultOptions = {
                baseUrl: "api/",
                notesTextBoxId: "",
                richTextTemplateId: "editor-notes",
                saveChangesEvent: "saveChangesClicked",
                idSeperator: "_",
                deferRequestBy: 300
            };

            var container = this;
            container.onChangeTimeout = null;

            var quill = null;

            options = $.extend(defaultOptions, options);

            function getNotesTextBoxSelector() {
                return "#" + options.notesTextBoxId;
            }
            function getRichTextTemplateIdSelector() {
                return "#" + options.richTextTemplateId;
            }

            function onSaveChangesEvent() {
                console.info("[RichTextNotes] onSaveChangesEvent occured.")

                var notesEl = $(getNotesTextBoxSelector());
                var notesText = JSON.stringify(quill.getContents());
                var notesHtml = quill.root.innerHTML;
                notesEl.val(notesText);

                console.debug("[RichTextNotes] Content: " + notesEl.val())
                console.debug("[RichTextNotes] notesHtml: " + notesHtml)
                
            }

            function initControl() {
                quill = new Quill(getRichTextTemplateIdSelector(), {
                    theme: 'snow',
                    formats: [
                        'bold',
                        'italic',
                        'underline',
                        'strike',
                        'link',
                        'list',
                        'indent',
                        'color',
                        'background'
                    ],
                    modules: {
                        toolbar: [
                            ['bold', 'italic', 'underline', 'strike'],
                            [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                            [{ 'color': [] }, { 'background': [] }],
                            ['link'],
                            ['clean'] 
                        ]
                    }
                });

                quill.on('text-change', function (delta, oldDelta, source) {
                    clearTimeout(container.onChangeTimeout);

                    container.onChangeTimeout = setTimeout(function () {
                        onSaveChangesEvent();
                    }, options.deferRequestBy);

                   
                });


                var notesEl = $(getNotesTextBoxSelector());
                var notesText = notesEl.val();
                quill.setContents(JSON.parse(notesText || "[]"));
            }


            $(window).on(options.saveChangesEvent, onSaveChangesEvent);



            function init() {
                initControl();
            }

            init();

            //window.Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);

        }
    });

})(jQuery);