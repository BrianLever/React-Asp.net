using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using AjaxControlToolkit;
using System.Globalization;
using FrontDesk.Web.Helpers;
using FrontDesk.Server.Web;
using System.Reflection;

namespace FrontDesk.Server.Web.Controls
{
    /// <summary>
    /// Date time picker
    /// </summary>
    [ToolboxItem(true)]
    [Themeable(true)]
    [ToolboxData("<{0}:RichDatePicker runat=\"Server\" id=\"datePicker\"  />")]
    [AspNetHostingPermission(SecurityAction.Demand,
     Level = AspNetHostingPermissionLevel.Minimal)]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed - need for datasources on pages
    public class RichDatePicker : TextBox, INamingContainer
    {
        //private TextBox txtRichDatePicker = null;
        private CalendarExtender ctrlCalendarExtender = null;
        private LinkButton btnRichDatePicker = null;
        private Image imgRichDatePicker = null;

        #region Image proterties

        /// <summary>
        /// calendar image url
        /// </summary>
        private string _imageUrl;
        /// <summary>
        /// Calendar pick image button
        /// </summary>
        [Category("Misc")]
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                try
                {
                    _imageUrl = ((BasePage)Page).GetVirtualPath(value);
                }
                catch (Exception)
                {
                    //if we not hosted on BasePage derived page class
                    _imageUrl = value;
                }
            }
        }

        /// <summary>
        /// calendar image title
        /// </summary>
        private string _imageTitle = "Click to open calendar popup";
        [Category("Misc")]
        public string Title
        {
            get { return _imageTitle; }
            set { _imageTitle = value; }
        }

        #endregion

        #region Data & Text Properties

        /// <summary>
        /// date format
        /// </summary>
        private string _format = "mm/dd/yyyy";
        /// <summary>
        /// Custom date format
        /// </summary>
        [Category("Misc")]
        public string Format
        {
            get { return _format; }
            set
            {
                _format = value;
                if (_useUserCulture)
                {
                    CultureInfo oCulture = CultureInfo.CurrentCulture;
                    string sDelimiter = oCulture.DateTimeFormat.DateSeparator;
                    if (_format.IndexOf("/") > 0)
                    {
                        _format = _format.Replace("/", sDelimiter);
                    }
                }
            }
        }

        private bool _useUserCulture = true;
        /// <summary>
        /// Use page CultureInfo for adjust date delimiters
        /// </summary>
        [Category("Misc"),
        DefaultValue("true"),
        Themeable(true)
        ]
        public bool UseUserCulture
        {
            get { return _useUserCulture; }
            set
            {
                _useUserCulture = value;
            }
        }

        [Category("Misc")]
        [Obfuscation(Feature = "renaming", Exclude = true)] // used in data binding by name
        public DateTimeOffset? SelectedDateTimeOffset
        {
            set
            {
                if (!value.HasValue)
                {
                    SelectedDate = null;
                }
                else
                {
                    SelectedDate = value.Value.DateTime;
                }
            }
            get
            {
                var date = SelectedDate;

                if(!date.HasValue)
                {
                    return null;
                }

                return new DateTimeOffset(date.Value);
            }
        }

        /// <summary>
        /// Set/Get date value
        /// </summary>
        [Category("Misc")]
        [Obfuscation(Feature = "renaming", Exclude = true)] // used in data binding by name
        public DateTime? SelectedDate
        {
            get
            {
                if (this.Text.Length == 0)
                {
                    return null;
                }
                else
                {
                    try
                    {
                        IFormatProvider culture = _useUserCulture ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
                        return DateTime.Parse(this.Text, culture);
                    }
                    catch (FormatException)
                    {
                        return null;
                    }
                }

            }
            set
            {
                EnsureChildControls();
                if (!value.HasValue)
                {
                    this.Text = "";
                }
                else
                {
                    IFormatProvider culture = _useUserCulture ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
                    this.Text = value.Value.ToString("d", culture);
                }
            }
        }
        /// <summary>
        /// Get date text box value
        /// </summary>
        public override string Text
        {
            get
            {
                EnsureChildControls();
                return base.Text;
            }
        }

        #endregion

        #region Style Properties

        protected string _customContainerClass = string.Empty;


        public override string CssClass
        {
            get
            {
                return _customContainerClass;
            }
            set
            {
                _customContainerClass = value;
            }
        }

        protected string _customTextboxCssClass = string.Empty;
        [Themeable(true)]
        public string TextboxCssClass
        {
            get
            {
                return base.CssClass;
            }
            set
            {
                base.CssClass = value;
            }
        }

        protected string _customIconCssClass = string.Empty;
        [Themeable(true)]
        public string IconCssClass
        {
            get
            {
                return _customIconCssClass;
            }
            set
            {
                _customIconCssClass = value;
            }
        }

        protected string _popupCalendarCssClass = string.Empty;
        [Themeable(true)]
        public string PopupCalendarCssClass
        {
            get
            {
                return _popupCalendarCssClass;
            }
            set
            {
                _popupCalendarCssClass = value;
            }
        }

        #endregion

        #region Control Properties

        public LinkButton TriggerButton
        {
            get
            {
                EnsureChildControls();
                return btnRichDatePicker;
            }
        }
        public string OnClientDateSelectionChanged { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public RichDatePicker()
        {
            _format = "mm/dd/yyyy";
            _imageTitle = "Select Date";
            this.CssClass = "picker";
            this.TextboxCssClass = "textbox";
            this.PopupCalendarCssClass = "popup_calendar ajax__calendar";
        }


        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (_imageUrl == null)
            {
                this._imageUrl = Page.ClientScript.GetWebResourceUrl(typeof(RichDatePicker), "FrontDesk.Server.Web.images.cal_ico.png");
            }

            string cssResourceURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "FrontDesk.Server.Web.Styles.DatePicker.css");
            WebUIHelper.RegisterCssStyle(this, "datePicker", cssResourceURL);
        }


        protected override void CreateChildControls()
        {
            this.MaxLength = 10;
          
            imgRichDatePicker = new Image();
            imgRichDatePicker.ID = "imgRichDatePicker";
            imgRichDatePicker.AlternateText = "calendar";
            imgRichDatePicker.EnableViewState = false;
            imgRichDatePicker.CssClass = "cal-img";
            imgRichDatePicker.ImageUrl = ImageUrl;


            btnRichDatePicker = new LinkButton();
            btnRichDatePicker.ID = "btnPicker";
            btnRichDatePicker.CausesValidation = false;
            btnRichDatePicker.Controls.Add(imgRichDatePicker);

            ctrlCalendarExtender = new CalendarExtender();
            ctrlCalendarExtender.ID = "calExt";
            ctrlCalendarExtender.Animated = true;
            ctrlCalendarExtender.PopupButtonID = btnRichDatePicker.ID;
            ctrlCalendarExtender.OnClientDateSelectionChanged = OnClientDateSelectionChanged;
            
            ctrlCalendarExtender.TargetControlID = this.ID;
            //ctrlCalendarExtender.PopupPosition = CalendarPosition.Left;


            Controls.Add(btnRichDatePicker);
            Controls.Add(ctrlCalendarExtender);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ctrlCalendarExtender.SelectedDate = this.SelectedDate;
            ctrlCalendarExtender.CssClass = this.PopupCalendarCssClass;

            

        }


        protected override void Render(HtmlTextWriter writer)
        {


            //css classes
            string containerClass = "datePicker_container";
            if (this.ReadOnly)
            {
                containerClass = FrontDesk.Web.Helpers.WebUIHelper.AppendClass(containerClass, "readonly");
            }
            
            if (!String.IsNullOrEmpty(this.CssClass))
                containerClass = FrontDesk.Web.Helpers.WebUIHelper.AppendClass(containerClass, this.CssClass);

            string iconCssClass = "icon";
            if (!String.IsNullOrEmpty(this.IconCssClass))
                iconCssClass = FrontDesk.Web.Helpers.WebUIHelper.AppendClass(iconCssClass, this.IconCssClass);



            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + IdSeparator + "_container");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, containerClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Div); //container div

            writer.RenderBeginTag(HtmlTextWriterTag.Div); //txt div
            base.Render(writer);

           


            ctrlCalendarExtender.RenderControl(writer);
            writer.RenderEndTag(); //end txt div


            writer.AddAttribute(HtmlTextWriterAttribute.Class, iconCssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            btnRichDatePicker.RenderControl(writer);
            writer.RenderEndTag(); //div
            writer.RenderEndTag(); //div
        }
    }
}
