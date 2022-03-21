using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Reflection;

namespace FrontDesk.Server.Web.Controls
{

    /// <summary>
    /// Summary description for PhoneTextBox
    /// </summary>
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class PhoneTextBox : CompositeControl, System.Web.UI.INamingContainer
    {

        private RequiredFieldValidator vldReqValdator = null;
        private RegularExpressionValidator vldRegValidator = null;
        private TextBox txtPhone = null;
        private TextBox txtExtension = null;

        private const string PhoneNumberValidationErrorText = "Please enter the phone number in the format '[+]C L XXX XX XX' where C is a country code (1-3 digits) and L is  an area code (1-3 digits)";
        private const string PhoneNumberMask = "999 999 999 999 999";
        private const string PhoneNumberValidationRequeredErrorText = "Phone is required";
        private const string PhoneNumberValidationExpression = @"^(\+)?\d{1,3}([ -]\d+)+$";
        //Error text for Fax
        private const string FaxNumberValidationErrorText = "Please enter the fax in the format '[+]C L XXX XX XX' where C is a country code (1-3 digits) and L is  an area code (1-3 digits)";
        private const string FaxNumberValidationRequeredErrorText = "Fax is required";
        
        // regexps for german phones
        private static Regex delimiters = new Regex(@"[\t\-\./]", RegexOptions.Compiled);
        private static Regex notPhoneChars = new Regex(@"[^\d\(\) ]", RegexOptions.Compiled);
        private static Regex notNumbers = new Regex(@"[^\d]", RegexOptions.Compiled);

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.txtPhone = new TextBox();
            this.txtPhone.ID = "txtPhone";
            this.txtPhone.SkinID = this.SkinID;
            this.txtPhone.CssClass = "textbox";
            this.txtPhone.TabIndex = this.TabIndex;
            this.txtPhone.ApplyStyleSheetSkin(this.Page);
            this.txtPhone.Width = this.Width;
            this.txtPhone.MaxLength = 24;
            this.Controls.Add(this.txtPhone);

            if (!WithExtension)
            {
                Label separator = new Label();
                separator.Text = " - ";
                this.Controls.Add(separator);

                txtExtension = new TextBox();
                txtExtension.CssClass = "textbox";
                txtExtension.MaxLength = 4;
                txtExtension.Width = Unit.Pixel(50);
                this.Controls.Add(txtExtension);

                Label note = new Label();
                note.Text = " (ext.)";
                this.Controls.Add(note);

            }


            this.vldReqValdator = new RequiredFieldValidator();
            this.vldReqValdator.SkinID = this.SkinID;
            this.vldReqValdator.ApplyStyleSheetSkin(this.Page);
            this.vldReqValdator.ValidationGroup = this.ValidationGroup;
            this.vldReqValdator.EnableClientScript = true;
            this.vldReqValdator.Enabled = false;

            this.vldRegValidator = new RegularExpressionValidator();
            this.vldRegValidator.SkinID = this.SkinID;
            this.vldRegValidator.ApplyStyleSheetSkin(this.Page);
            this.vldRegValidator.ValidationGroup = this.ValidationGroup;
            this.vldRegValidator.EnableClientScript = true;

            //var phone = new CityBoard.IMN.Membership.LocalizedPhoneNumber();

            this.vldReqValdator.ErrorMessage = IsPhone ? PhoneNumberValidationRequeredErrorText : FaxNumberValidationRequeredErrorText;
            this.vldReqValdator.Text = "*";
            this.vldReqValdator.ID = "vldreq";
            this.vldReqValdator.ControlToValidate = this.txtPhone.ID;
            this.vldReqValdator.Display = ValidatorDisplay.Dynamic;
            this.Controls.Add(this.vldReqValdator);

            this.vldRegValidator.ErrorMessage = IsPhone ? PhoneNumberValidationErrorText : FaxNumberValidationErrorText;
            this.vldRegValidator.Text = IsPhone ? PhoneNumberValidationErrorText : FaxNumberValidationErrorText;
            this.vldRegValidator.ID = "vld";
            this.vldRegValidator.ValidationExpression = PhoneNumberValidationExpression;
            this.vldRegValidator.ControlToValidate = this.txtPhone.ID;
            this.vldRegValidator.Display = ValidatorDisplay.Dynamic;

            this.Controls.Add(this.vldRegValidator);
        }

        public bool IsValid
        {
            get
            {
                EnsureChildControls();
                this.vldReqValdator.Validate();
                if (IsRequiredValidate)
                {
                    vldReqValdator.Validate();
                }
                return this.vldRegValidator.IsValid && IsRequiredValidate ? this.vldReqValdator.IsValid : true;
            }
        }
        private string _validationGroup;

        public string ValidationGroup
        {
            get
            {
                return _validationGroup;
            }
            set
            {
                EnsureChildControls();
                _validationGroup = value;
                this.vldRegValidator.ValidationGroup = value;
                this.vldReqValdator.ValidationGroup = value;
            }
        }

        public string Text
        {
            get
            {
                EnsureChildControls();
                return FormatInternationalPhoneNumber(txtPhone.Text);
                //return txtPhone.Text;
            }
            set
            {
                EnsureChildControls();
                var phoneString = FormatInternationalPhoneNumber(value);

                txtPhone.Text = phoneString;
            }
        }

        public string Extension
        {
            get
            {
                return txtExtension.Text;
            }
            set
            {
                txtExtension.Text = value;
            }
        }

        public bool IsRequiredValidate
        {
            get
            {
                EnsureChildControls();
                return this.vldReqValdator.Enabled;
            }
            set
            {
                EnsureChildControls();
                this.vldReqValdator.Enabled = value;
            }
        }

        protected bool _isPhone = true;
        public bool IsPhone
        {
            get { return _isPhone; }
            set { _isPhone = value;}
        }

        public int MaxLength
        {
            get
            {
                EnsureChildControls();
                return txtPhone.MaxLength;
            }
            set
            {
                EnsureChildControls();
                txtPhone.MaxLength = value;
            }
        }
        /// <summary>
        /// Get phone number text box
        /// </summary>
        public TextBox PhoneNumberTextBox
        {
            get
            {
                EnsureChildControls();
                return this.txtPhone;
            }
        }

        public bool WithExtension { get; set; }

        public PhoneTextBox()
        {
            WithExtension = true;
        }

        public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
        {
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            //base.RenderBeginTag(writer);
        }

        /// <summary>
        /// Formats the phone number according to german rules.
        /// </summary>
        /// <param name="value">Phone number</param>
        /// <returns>Formatted phone number</returns>
        public static string FormatGermanPhoneNumber(string value)
        {
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(value))
            {
                // change all delimiters to spaces
                string s = delimiters.Replace(value, " ");

                // remove all invalid symbols
                s = notPhoneChars.Replace(s, string.Empty);

                StringBuilder sb = new StringBuilder(s);

                // remove double spaces                
                while (sb.ToString().IndexOf("  ") != -1)
                {
                    sb.Replace("  ", " ");
                }

                // ensure no spaces near braces
                sb.Replace("( ", "(");
                sb.Replace(" )", ")");

                sb = new StringBuilder(sb.ToString().Trim());

                // drop country code
                if (sb.ToString().StartsWith("+49"))
                {
                    sb.Remove(0, 3);
                }
                else if (sb.ToString().StartsWith("(+49)"))
                {
                    sb.Remove(0, 5);
                }
                //else if (sb.ToString().StartsWith("+(49)"))
                //{
                //    sb.Remove(0, 5);
                //}

                // Here, we have prefix and number with single spaces in random places,
                // prefix may be in braces.
                // From invalid input stings, we also may have '(', ')' and '+' at random places.
                string rawNumber = sb.ToString().Trim();

                // Group prefix and number digits with proper spacing

                string prefix = string.Empty;   // 2 to 5 chars without leading 0
                string number = string.Empty;   // 3 to 7 chars

                // try to parse prefix in first braces
                int prefixEnd = rawNumber.IndexOf(")");
                if (prefixEnd != -1)
                {
                    // have prefix in braces. Note we will drop anything before braces.
                    int start = rawNumber.IndexOf("(") + 1;
                    prefix = notNumbers.Replace(rawNumber.Substring(start, prefixEnd - start), string.Empty);
                    number = notNumbers.Replace(rawNumber.Substring(prefixEnd + 1), string.Empty);
                }
                else
                {
                    // no braces, assume 1st group is a prefix
                    prefixEnd = rawNumber.IndexOf(" ");
                    if (prefixEnd != -1)
                    {
                        prefix = notNumbers.Replace(rawNumber.Substring(0, prefixEnd), string.Empty);
                        number = notNumbers.Replace(rawNumber.Substring(prefixEnd + 1), string.Empty);
                    }
                    else
                    {
                        // no spaces - assume no prefix at all
                        number = notNumbers.Replace(rawNumber, string.Empty);
                    }
                }

                // split number by N digits, leaving no more than N*2-1 in the first group
                int n = 2;
                int shift = number.Length % n;
                for (int i = 0; i < number.Length / n; i++) // note integer division
                {
                    if (i > 0)
                    {
                        result.Append(" ");
                    }
                    result.Append(number.Substring(i * n + shift, n));
                }
                // add remaining leading digits
                result.Insert(0, number.Substring(0, shift));

                if (prefix.Length > 0)
                {
                    // add leading 0 to prefix
                    if (prefix[0] != '0')
                    {
                        prefix = '0' + prefix;
                    }

                    // split prefix, 0XXX + rest
                    if (prefix.Length > 4)
                    {
                        prefix = prefix.Substring(0, 4) + ' ' + prefix.Substring(4);
                    }

                    result.Insert(0, " ");
                    result.Insert(0, prefix);
                }
            }

            return result.ToString();
        }


        public static string FormatInternationalPhoneNumber(string value)
        {

            return value;
            /*
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(value))
            {
                // change all delimiters to spaces
                string s = delimiters.Replace(value, " ");

                // remove all invalid symbols
                s = notPhoneChars.Replace(s, string.Empty);

                StringBuilder sb = new StringBuilder(s);

                // remove double spaces                
                while (sb.ToString().IndexOf("  ") != -1)
                {
                    sb.Replace("  ", " ");
                }

                // ensure no spaces near braces
                sb.Replace("( ", "(");
                sb.Replace(" )", ")");

                sb = new StringBuilder(sb.ToString().Trim());

                
                // Here, we have prefix and number with single spaces in random places,
                // prefix may be in braces.
                // From invalid input stings, we also may have '(', ')' and '+' at random places.
                string rawNumber = sb.ToString().Trim();

                // Group prefix and number digits with proper spacing

                string country = string.Empty; // 1 to 3 chars with optional '+'
                string area = string.Empty;   // 1 to 5 chars
                string number = string.Empty;   // 3 to 7 chars


                // try to parse prefix in first braces
                int prefixEnd = rawNumber.IndexOfAny(new char[]{')', ' '});

                
            }

            return result.ToString();
            */
        }


        public void Clear()
        {
            this.Text = String.Empty;
            this.Extension = String.Empty;
        }
    }
}