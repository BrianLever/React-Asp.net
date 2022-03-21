using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FrontDesk.Server.Web.Controls
{
    /// <summary>
    /// The USA phone number control
    /// </summary>
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class Phone: CompositeControl
    {
        /// <summary>
        /// Gets or sets the USA phone number
        /// Uses format (xxx)xxx-xxxx
        /// NOTE: NumbersOnly skin
        /// </summary>
        public string Value
        {
            get
            {
                EnsureChildControls();

                StringBuilder phone = new StringBuilder();
                phone.Append('(');
                phone.Append(tb1.Text);
                phone.Append(") ");
                phone.Append(tb2.Text);
                phone.Append('-');
                phone.Append(tb3.Text);

                //if phone is empty - return empty string

                Regex reEmptyPhone = new Regex(@"\(\)\s+-\s*");

                var result = reEmptyPhone.Replace(phone.ToString(), string.Empty);

                return result;
            }
            set
            { 
                EnsureChildControls();

                //remove all non digit characters
                StringBuilder phone = new StringBuilder(value);
                phone.Replace("(", String.Empty);
                phone.Replace(")", String.Empty);
                phone.Replace("-", String.Empty);
                phone.Replace(" ", String.Empty);

                string clearPhone = phone.ToString();

                if (!string.IsNullOrWhiteSpace(clearPhone))
                {
                    try
                    {
                        tb1.Text = clearPhone.Substring(0, 3);
                        tb2.Text = clearPhone.Substring(3, 3);
                        tb3.Text = clearPhone.Substring(6, 4);
                    }
                    catch (Exception exc)
                    {
                        throw new FormatException("Invalid format of phone number", exc);
                    }
                }
                else
                {
                    //missing patient address because of screening frequence feature
                    tb1.Text = string.Empty;
                    tb2.Text = string.Empty;
                    tb3.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets value indicates whether phone number is mandatory
        /// </summary>
        public bool IsRequired { get; set; }

        public string ValidationGroup { get; set; }

        /// <summary>
        /// Gets or sets a validator's error message 
        /// </summary>
        public string ValidationErrorMessage { get; set; }

        private TextBox tb1, tb2, tb3;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Add initial script
            string script = String.Format(@"
function ValidatePhone(sender, args){{
    var parts = [new PhonePart('{0}', 3), new PhonePart('{1}', 3), new PhonePart('{2}', 4)]
    var validator = new PhoneValidator(parts, {3});
    args.IsValid = validator.Validate();
}}", tb1.ClientID, tb2.ClientID, tb3.ClientID, IsRequired.ToString().ToLower());

            Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "__validate_method", script, true);


            string reference = Page.ClientScript.GetWebResourceUrl(this.GetType(), "FrontDesk.Server.Web.scripts.ClientValidator.js");
            Page.ClientScript.RegisterClientScriptInclude("__validator", reference);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            int width = 28;

            tb1 = new TextBox();
            tb1.Width = Unit.Pixel(width);
            tb1.MaxLength = 3;
            tb1.SkinID = "NumbersOnly";
            tb1.ApplyStyleSheetSkin(this.Page);
            tb1.CssClass = "textbox fleft";
            this.Controls.Add(tb1);

            AddSeparator("–");

            tb2 = new TextBox();
            tb2.MaxLength = 3;
            tb2.Width = Unit.Pixel(width);
            tb2.SkinID = "NumbersOnly";
            tb2.ApplyStyleSheetSkin(this.Page);
            tb2.CssClass = "textbox fleft";
            this.Controls.Add(tb2);

            AddSeparator("–");

            tb3 = new TextBox();
            tb3.MaxLength = 4;
            tb3.Width = Unit.Pixel(width + 10);
            tb3.SkinID = "NumbersOnly";
            tb3.ApplyStyleSheetSkin(this.Page);
            tb3.CssClass = "textbox fleft";
            this.Controls.Add(tb3);

            CustomValidator phoneValidator = new CustomValidator();
            phoneValidator.ValidationGroup = this.ValidationGroup;
            phoneValidator.Display = ValidatorDisplay.None;
            phoneValidator.ErrorMessage = this.ValidationErrorMessage;
            phoneValidator.ClientValidationFunction = "ValidatePhone";
            phoneValidator.ApplyStyleSheetSkin(this.Page);
            this.Controls.Add(phoneValidator);

            this.Controls.Add(new LiteralControl("<div class='fclear' style='height:0px'></div>"));
        }

        private void AddSeparator(string separator)
        {
            this.Controls.Add(new LiteralControl("<div style='float:left; padding:0px 3px;'>"));
            this.Controls.Add(new LiteralControl(separator));
            this.Controls.Add(new LiteralControl("</div>"));
        }
    }
}
