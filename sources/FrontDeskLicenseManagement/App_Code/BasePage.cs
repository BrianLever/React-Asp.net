using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Membership;

namespace FrontDesk.Server.LicenseManagerWeb
{

    /// <summary>
    /// Page access permissions
    /// </summary>
    [Flags]
    public enum AccessPermissions
    {
        None = 0x0,
        Read = 0x1,
        Write = 0x2,
        Delete = 0x4,
        Print = 0x8
    }

    /// <summary>
    /// The source if the searched collection
    /// </summary>
    [Flags]
    public enum PageIDSource
    {
        None = 0,
        QueryString = 1,
        Request = 2,
        Session = 4,
        ViewState = 8,
        Form = 16,
        /// <summary>
        /// Search in all sources
        /// </summary>
        Any = QueryString | Request | Session | ViewState | Form,
        /// <summary>
        /// Search in source that couldn't be modified by user
        /// </summary>
        NonUserInteractive = Session | ViewState,
        /// <summary>
        /// Search in source that could be modified by user
        /// </summary>
        UserIteractive = QueryString | Request | Form
    }

    /// <summary>
    /// Base Class for all pages
    /// </summary>
    public class LMBasePage : System.Web.UI.Page
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public LMBasePage()
        {
            //set defaut permissions
            WritePermission = false;
            ReadPermission = false;
            DeletePermission = false;
            PrintPermission = false;

            this.Init += new EventHandler(BasePage_Init);
        }

        void BasePage_Init(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("BasePage.Init event handler invoked");
            // set page permissions
            SetPagePermissions();
            BasePage_InitComplete(sender, e);
        }

        #region Page access permissions

        /// <summary>
        /// page access permissions
        /// Read by default
        /// </summary>
        private AccessPermissions grantedPagePermissions = AccessPermissions.Read;

        /// <summary>
        /// True if current user has write permission on page. True by default
        /// </summary>
        public bool HasWritePermission
        {
            get { return ((this.grantedPagePermissions & AccessPermissions.Write) > 0); }
        }
        /// <summary>
        /// True if current user has read permission on page. True by default
        /// </summary>
        public bool HasReadPermission
        {
            get { return ((this.grantedPagePermissions & AccessPermissions.Read) > 0); }
        }
        /// <summary>
        /// True if current user has delete permission on page. True by default
        /// </summary>

        public bool HasDeletePermission
        {
            get { return ((this.grantedPagePermissions & AccessPermissions.Delete) > 0); }
        }
        /// <summary>
        /// True if current user has ptint permission on page. True by default
        /// </summary>

        public bool HasPrintPermission
        {
            get { return ((this.grantedPagePermissions & AccessPermissions.Print) > 0); }
        }
        /// <summary>
        /// Set or remove write permission to the page
        /// </summary>
        public bool WritePermission
        {
            set
            {
                if (value) this.grantedPagePermissions |= AccessPermissions.Write;
                else if (HasWritePermission) { this.grantedPagePermissions = (AccessPermissions)(this.grantedPagePermissions ^ AccessPermissions.Write); }
            }
        }
        /// <summary>
        /// Set or remove read permission to the page
        /// </summary>
        public bool ReadPermission
        {
            set
            {
                if (value) this.grantedPagePermissions |= AccessPermissions.Read;
                else if (HasReadPermission) this.grantedPagePermissions = (AccessPermissions)(this.grantedPagePermissions ^ AccessPermissions.Read);
            }
        }
        /// <summary>
        /// Set or remove delete permission to the page
        /// </summary>
        public bool DeletePermission
        {
            set
            {
                if (value) this.grantedPagePermissions |= AccessPermissions.Delete;
                else if (HasDeletePermission) this.grantedPagePermissions = (AccessPermissions)(this.grantedPagePermissions ^ AccessPermissions.Delete);
            }
        }

        /// <summary>
        /// Set or remove print permission to the page
        /// </summary>
        public bool PrintPermission
        {
            set
            {
                if (value) this.grantedPagePermissions |= AccessPermissions.Print;
                else if (HasPrintPermission) this.grantedPagePermissions = (AccessPermissions)(this.grantedPagePermissions ^ AccessPermissions.Print);
            }
        }

        /// <summary>
        /// Set page permissions
        /// </summary>
        [Obfuscation(Feature = "renaming", Exclude = true)]
        protected virtual void SetPagePermissions() { }

        /// <summary>
        /// Confugure page content according to the page access permissions
        /// </summary>
        [Obfuscation(Feature = "renaming", Exclude = true)]
        protected virtual void ApplyPagePermissions() { }


        #endregion
        /// <summary>
        /// Email Regular expression
        /// </summary>
        public static string EmailRegularExpression
        {
            get { return FrontDesk.Common.Mailing.EmailManager.ValidationExpression; }
        }

        /// <summary>
        /// Strong Type current user info 
        /// </summary>
        public FDMembershipUser MembershipUser
        {
            get { return (FDMembershipUser)System.Web.Security.Membership.GetUser(); }
        }

        #region Alerts



        #region Obsolete methods
        /// <summary>
        /// Display message as alert that is preserved through redirect (session required)
        /// </summary>
        /// <param name="sText">added text</param>
        [Obsolete("Use SetRedirectAlert instead of SetAlert.")]
        public void SetAlert(string sText)
        {
            string sAlert = "";

            if (Session["ALERT"] != null)
            {
                sAlert = Session["ALERT"].ToString();
            }
            else
            {
                sAlert = "";
            }
            if (sAlert.Length > 0)
            {
                sAlert += "\\n";
            }
            sAlert += sText.Replace("'", @"\'");
            Session["ALERT"] = sAlert;
        }

        /// <summary>
        /// Return alert message
        /// </summary>
        /// <param name="bClearAlert">Need clear alert text after read</param>
        /// <returns></returns>
        [Obsolete("Use GetRedirectAlert instead of GetAlert.")]
        public string GetAlert(bool bClearAlert)
        {
            string sAlert = null;
            try
            {
                if (Session["ALERT"] != null)
                {
                    sAlert = Session["ALERT"].ToString();

                    if (bClearAlert)
                    {
                        Session["ALERT"] = null;
                    }
                }
            }
            catch (Exception) { }
            return sAlert;
        }

        /// <summary>
        /// Return alert message and clear alert text after read
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use GetRedirectAlert instead of GetAlert.")]
        public string GetAlert()
        {
            return GetAlert(true);
        }

        #endregion

        /// <summary>
        /// Display message as alert that is preserved through redirect (session required)
        /// </summary>
        /// <param name="sText">added text</param>
        public void SetRedirectAlert(string sText)
        {
            string sAlert = "";

            if (Session["ALERT"] != null)
            {
                sAlert = Session["ALERT"].ToString();
            }
            else
            {
                sAlert = "";
            }
            if (sAlert.Length > 0)
            {
                sAlert += "\\n";
            }
            sAlert += sText.Replace("'", @"\'");
            Session["ALERT"] = sAlert;
        }

        /// <summary>
        /// Return alert message
        /// </summary>
        /// <param name="bClearAlert">Need clear alert text after read</param>
        /// <returns></returns>
        public string GetRedirectAlert(bool bClearAlert)
        {
            string sAlert = null;
            try
            {
                if (Session["ALERT"] != null)
                {
                    sAlert = Session["ALERT"].ToString();

                    if (bClearAlert)
                    {
                        Session["ALERT"] = null;
                    }
                }
            }
            catch (Exception) { }
            return sAlert;
        }


        /// <summary>
        /// Return alert message and clear alert text after read
        /// </summary>
        /// <returns></returns>
        public string GetRedirectAlert()
        {
            return GetRedirectAlert(true);
        }


        /// <summary>
        /// Return javascript code to show alert message
        /// </summary>
        /// <returns></returns>
        public string GetJsAlert()
        {
            string sJs = "";
            string sAlert = GetRedirectAlert();

            if (sAlert != null)
            {
                sAlert = sAlert.Replace("\r", "\\r");
                sAlert = sAlert.Replace("\\n", "\n");
                sAlert = sAlert.Replace("\n", "\\n");
                //sJs = string.Format("<script language=\"javascript\" type=\"text/javascript\">alert(\"{0}\");</script>", sAlert);
                sJs = string.Format("alert(\"{0}\");", sAlert);

            }
            return sJs;
        }
        #endregion

        #region Page & Page Controls Event Handlers
        /// <summary>
        /// Overriden OnInit method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
           
           
        }

       
        /// <summary>
        /// Overriden OnLoad method.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {

            string sAlert = GetJsAlert();
            if (sAlert != null && sAlert.Length > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert_script", sAlert, true);
            }

            //_3SI2.Common.Debugging.DebugLogger.TraceException(null, "BasePage.OnLoad");
            base.OnLoad(e);
            //_3SI2.Common.Debugging.DebugLogger.TraceException(null, "BasePage.OnLoad pre-end");
            
            
        }

        /// <summary>
        /// Overriden onPreRender method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            SetControlsEnabledState();

            //apply page permissions
            ApplyPagePermissions();
            //set tab index for all visible or enabled elements
            SetTabIndex();
            base.OnPreRender(e);

           

            this.FlushAjaxScriptBlock();
            this.FlushAjaxStartupScriptBlock();



        }



        /// <summary>
        /// On sort handler for sorting Grid View by 2 or more columns
        /// </summary>
        public virtual void OnGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            GridView gvView = null;

            if (sender is GridView) gvView = sender as GridView;

            if (gvView != null)
            {

                int i = 0;
                string SortExpr = e.SortExpression;

                if (gvView.SortExpression.IndexOf(" ASC") > -1 || gvView.SortExpression.IndexOf(" DESC") > -1)
                {
                    string prevSorting = gvView.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
                    if (SortExpr.Equals(prevSorting))
                    {
                        e.SortDirection = e.SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                    }
                }

                string[] columns = SortExpr.Split(new char[] { ',' });
                StringBuilder sNewExpr = new StringBuilder();
                for (i = 0; i < columns.Length; i++)
                {
                    sNewExpr.AppendFormat("{0} {1},", columns[i], e.SortDirection == SortDirection.Ascending ? "ASC" : "DESC");
                }
                if (i > 0) sNewExpr.Length -= 5;
                e.SortExpression = sNewExpr.ToString();
            }

        }

        /// <summary>
        /// Link to redirect on Back button click event
        /// </summary>
        public string BackPageUrl = String.Empty;


        #endregion

        /// <summary>
        /// Override this method to set tabl index to page controls which are in visible state
        /// </summary>
        public virtual void SetTabIndex() { }

        #region Application Virtual Path
        /// <summary>
        /// Get application path
        /// </summary>
        /// <param name="bWithSlashAtEnd"></param>
        /// <returns></returns>
        public string GetApplicationPath(bool bWithSlashAtEnd)
        {
            StringBuilder sPath = new StringBuilder(Request.ApplicationPath);
            if (bWithSlashAtEnd)
            {
                if (sPath[sPath.Length - 1] != '/')
                {
                    sPath.Append("/");
                }
            }
            else
            {
                ///remove / at the ent
                if (sPath[sPath.Length - 1] == '/')
                {
                    sPath.Length--;
                }
            }
            return sPath.ToString();
        }

        /// <summary>
        /// Replace ~ with Application path
        /// </summary>
        /// <param name="sResourcePath"></param>
        /// <returns></returns>
        public string GetVirtualPath(string sResourcePath)
        {
            return sResourcePath.Replace("~", GetApplicationPath(false));
        }

        #endregion

        /// <summary>
        /// Set Controls Enabled/Disabled status
        /// </summary>
        [Obfuscation(Feature = "renaming", Exclude = true)]
        protected virtual void SetControlsEnabledState() { }

        #region Form Object ID

        /// <summary>
        /// Get key value from page parameter source
        /// </summary>
        /// <typeparam name="T">Key value type</typeparam>
        /// <param name="sKey">Key name</param>
        /// <param name="source">Source where key will be searched in</param>
        /// <param name="IsFound">True, if key with not null value was found. Otherwise false.</param>
        /// <returns>Returns key value. If key was not found or key value is null or empty string, returns default type value</returns>
        /// <remarks>
        /// Key searched in source in sequence:
        /// 1. QueryString
        /// 2. ViewState
        /// 3. Session
        /// 4. Form
        /// 5. Request
        /// </remarks>
        public T GetPageIDValue<T>(string sKey, PageIDSource source, out bool IsFound)
        {
            T iID = default(T);

            IsFound = false;
            try
            {
                if ((source & PageIDSource.QueryString) > 0 && !String.IsNullOrEmpty(Request.QueryString[sKey]))
                {
                    iID = ConvertToType<T>(Request.QueryString[sKey]);
                    IsFound = true;
                }
                else if ((source & PageIDSource.ViewState) > 0 && ViewState[sKey] != null)
                {
                    iID = ConvertToType<T>(ViewState[sKey]);
                    IsFound = true;
                }
                else if ((source & PageIDSource.Session) > 0 && Session != null && Session[sKey] != null)
                {
                    iID = ConvertToType<T>(Session[sKey]);
                    IsFound = true;
                }
                else if ((source & PageIDSource.Form) > 0 && Request.Form[sKey] != null)
                {
                    iID = ConvertToType<T>(Request.Form[sKey]);
                    IsFound = true;
                }
                else if ((source & PageIDSource.Request) > 0 && !String.IsNullOrEmpty(Request[sKey]))
                {
                    iID = ConvertToType<T>(Request[sKey]);
                    IsFound = true;
                }
            }
            catch (Exception ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.DebugException(ex);
            }
            return iID;

        }

        /// <summary>
        /// Get key value from page parameter source.
        /// </summary>
        /// <typeparam name="T">Key value type</typeparam>
        /// <param name="sKey">Key name</param>
        /// <param name="source">Source where key will be searched in</param>
        /// <param name="idValue">Key value. If key was not found or key value is null or empty string, returns default type value</param>
        /// <returns>True, if key with not null value was found. Otherwise false.</returns>
        /// <remarks>
        /// Key searched in source in sequence:
        /// 1. QueryString
        /// 2. ViewState
        /// 3. Session
        /// 4. Form
        /// 5. Request
        /// </remarks>
        public bool TryGetPageIDValue<T>(string sKey, PageIDSource source, out T idValue)
        {
            idValue = default(T);

            bool IsFound = false;
            try
            {
                if ((source & PageIDSource.QueryString) > 0 && !String.IsNullOrEmpty(Request.QueryString[sKey]))
                {
                    idValue = ConvertToType<T>(Request.QueryString[sKey]);
                    IsFound = true;
                }
                else if ((source & PageIDSource.ViewState) > 0 && ViewState[sKey] != null)
                {
                    idValue = ConvertToType<T>(ViewState[sKey]);
                    IsFound = true;
                }
                else if ((source & PageIDSource.Session) > 0 && Session != null && Session[sKey] != null)
                {
                    idValue = ConvertToType<T>(Session[sKey]);
                    IsFound = true;
                }
                else if ((source & PageIDSource.Form) > 0 && Request.Form[sKey] != null)
                {
                    idValue = ConvertToType<T>(Request.Form[sKey]);
                    IsFound = true;
                }
                else if ((source & PageIDSource.Request) > 0 && !String.IsNullOrEmpty(Request[sKey]))
                {
                    idValue = ConvertToType<T>(Request[sKey]);
                    IsFound = true;
                }
            }
            catch (Exception ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.DebugException(ex);
            }
            return IsFound;

        }
        
        /// <summary>
        /// Retrive key value in all sources
        /// </summary>
        /// <typeparam name="T">Key value type</typeparam>
        /// <param name="sKey">Key name</param>
        /// <param name="IsFound">True, if key with not null value was found. Otherwise false</param>
        /// <returns>Returns key value.</returns>
        public T GetPageIDValue<T>(string sKey, out bool IsFound)
        {
            return GetPageIDValue<T>(sKey, PageIDSource.Any, out IsFound);
        }

        /// <summary>
        /// Search key value in all sources
        /// </summary>
        public bool TryGetPageIDValue<T>(string sKey, out T idValue)
        {
            return TryGetPageIDValue<T>(sKey, PageIDSource.Any, out idValue);
        }
        /// <summary>
        /// Convert data types with known exception handling
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        internal T ConvertToType<T>(object value)
        {
            T converted = default(T);
            if (value != null)
            {
                if (typeof(T) == typeof(Guid))
                {
                    converted = (T)Convert.ChangeType(new Guid(Convert.ToString(value)), typeof(T));
                }
                else
                {
                    converted = (T)Convert.ChangeType(value, typeof(T));
                }
            }
            return converted;
        }

        #region Nullable Types

        /// <summary>
        /// Convert data types with known exception handling and null return value on enable to convert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        internal Nullable<T> ConvertToNullableType<T>(object value) where T : struct
        {
            Nullable<T> converted = null;
            if (value != null)
            {
                if (typeof(T) == typeof(Guid))
                {
                    converted = (T)Convert.ChangeType(new Guid(Convert.ToString(value)), typeof(T));
                }
                else
                {
                    converted = (T)Convert.ChangeType(value, typeof(T));
                }
            }
            return converted;
        }

             /// <summary>
        /// Get key value from page parameter source
        /// </summary>
        /// <typeparam name="T">Key value type</typeparam>
        /// <param name="sKey">Key name</param>
        /// <param name="source">Source where key will be searched in</param>
        /// <param name="IsFound">True, if key with not null value was found. Otherwise false.</param>
        /// <returns>Returns key value. If key was not found or key value is null or empty string, returns default type value</returns>
        /// <remarks>
        /// Key searched in source in sequence:
        /// 1. QueryString
        /// 2. ViewState
        /// 3. Session
        /// 4. Form
        /// 5. Request
        /// </remarks>
        public Nullable<T> GetPageIDValue<T>(string sKey, PageIDSource source)
            where T : struct
        {
            Nullable<T> iID = null;

            try
            {
                if ((source & PageIDSource.QueryString) > 0 && !String.IsNullOrEmpty(Request.QueryString[sKey]))
                {
                    iID = ConvertToType<T>(Request.QueryString[sKey]);
                }
                else if ((source & PageIDSource.ViewState) > 0 && ViewState[sKey] != null)
                {
                    iID = ConvertToType<T>(ViewState[sKey]);
                }
                else if ((source & PageIDSource.Session) > 0 && Session != null && Session[sKey] != null)
                {
                    iID = ConvertToType<T>(Session[sKey]);
                }
                else if ((source & PageIDSource.Form) > 0 && Request.Form[sKey] != null)
                {
                    iID = ConvertToType<T>(Request.Form[sKey]);
                }
                else if ((source & PageIDSource.Request) > 0 && !String.IsNullOrEmpty(Request[sKey]))
                {
                    iID = ConvertToType<T>(Request[sKey]);
                }
            }
            catch (Exception ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.DebugException(ex);
            }
            return iID;

        }


        /// <summary>
        /// Retrive key value in all sources
        /// </summary>
        /// <typeparam name="T">Key value type</typeparam>
        /// <param name="sKey">Key name</param>
        /// <param name="IsFound">True, if key with not null value was found. Otherwise false</param>
        /// <returns>Returns key value.</returns>
        public Nullable<T> GetPageIDValue<T>(string sKey) where T : struct
        {
            return GetPageIDValue<T>(sKey, PageIDSource.Any);
        }

    
        #endregion

        /// <summary>
        /// Get page ID value as string
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="IsFound"></param>
        /// <returns></returns>
        public string GetPageIDValueAsString(string sKey, out bool IsFound)
        {
            string sID = String.Empty;
            IsFound = false;
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString[sKey]))
                {
                    sID = Request.QueryString[sKey];
                    IsFound = true;
                }
                else if (ViewState[sKey] != null)
                {
                    sID = Convert.ToString(ViewState[sKey]);
                    IsFound = true;
                }
                else if (Session[sKey] != null)
                {
                    sID = Convert.ToString(Session[sKey]);
                    IsFound = true;
                }


                else if (Request.Form[sKey] != null)
                {
                    sID = Request.Form[sKey];
                    IsFound = true;
                }
                else if (!String.IsNullOrEmpty(Request[sKey]))
                {
                    sID = Request[sKey];
                    IsFound = true;
                }
            }
            catch (Exception ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.DebugException(ex);
            }
            return sID;

        }


        #endregion


        #region AJAX Scripts
        /// <summary>
        /// Set Error Alert using AJAX ScriptManager object
        /// </summary>
        /// <param name="sText"></param>
        /// <param name="oType"></param>
        public void SetErrorAlert(string sText, Type oType)
        {
            SetErrorAlert(sText, oType, this.IsInAjaxPostBack);
        }
        /// <summary>
        /// Display error message as alert message. 
        /// Error message wouldn't preserved through redirect (uses Ajax feature for display)
        /// </summary>
        /// <param name="sText"></param>
        /// <param name="oType"></param>
        /// <param name="useAjaxMethod">If True - ScriptManager object is used to register control.
        /// Otherwise Page's ClientScript is used</param>
        public void SetErrorAlert(string sText, Type oType, bool useAjaxMethod)
        {
            sText = PrepearJavaScriptAlertStript(sText);
            if (!useAjaxMethod)
            {
                ClientScript.RegisterStartupScript(oType, "_error",
              string.Format("alert('{0}');", sText),
              true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, oType, "_error",
                    string.Format("alert('{0}');", sText/*.Replace("'", @"\'").Replace("\n", "\\n")*/),
                    true);
            }
        }

        /// <summary>
        /// Display custom message as alert message.
        /// Error message wouldn't preserved through redirect (uses Ajax feature for display)
        /// This methods uses Ajax ScriptManager class to resiter client script
        /// </summary>
        /// <param name="sText"></param>
        /// <param name="oType"></param>
        /// <param name="sKey"></param>
        public void SetAjaxAlert(string sText, Type oType, string sKey)
        {
            sText = PrepearJavaScriptAlertStript(sText);
            ScriptManager.RegisterClientScriptBlock(this, oType, sKey,
                string.Format("alert('{0}');", sText/*.Replace("'", @"\'").Replace("\n", "\\n")*/),
                true);
        }

        public void CallAjaxScript(string sScript, string sKey)
        {
            if (IsInAjaxPostBack)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), sKey, sScript, true);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), sKey, sScript, true);
            }
        }
        /// <summary>
        /// Add AJAX Client Script to the page output, that will be execute on page load event
        /// </summary>
        /// <param name="sScript"></param>
        /// <param name="sKey"></param>
        public void CallAjaxStartupScript(string sScript, string sKey)
        {
            if (IsInAjaxPostBack)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), sKey, sScript, true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), sKey, sScript, true);
            }
        }
        /// <summary>
        /// Replace special character for strings that uses in the alert scripts
        /// </summary>
        /// <param name="script"></param>
        /// <param name="replaceNewLine"></param>
        /// <returns></returns>
        public static string PrepearJavaScriptAlertStript(string script)
        {

            Regex regex = new Regex(@"(?<a>[^\\])(')"); //replace one ' to \'
            script = regex.Replace(script, @"${a}\'");

            regex = new Regex(@"([ ]?\r?\n)"); //replace one \n to \\n
            script = regex.Replace(script, "\\n");


            return script;
        }

        private StringBuilder ajaxScriptBlock = null;
        private StringBuilder ajaxStartupScriptBlock = null;

        /// <summary>
        /// Add script statement to the script block
        /// </summary>
        public void AddAjaxScriptStatement(string scriptStatement)
        {

            if (this.ajaxScriptBlock == null) this.ajaxScriptBlock = new StringBuilder();
            this.ajaxScriptBlock.Append(scriptStatement);

        }


        /// <summary>
        /// Add script statement to the script block that will be executed at startup
        /// </summary>
        public void AddAjaxStartupScriptStatement(string scriptStatement)
        {

            if (this.ajaxStartupScriptBlock == null) this.ajaxStartupScriptBlock = new StringBuilder();
            this.ajaxStartupScriptBlock.Append(scriptStatement);

        }
        /// <summary>
        /// Flush added script statement to the page with default script key
        /// </summary>
        public void FlushAjaxScriptBlock()
        {
            this.FlushAjaxScriptBlock("_ajax_script_block");
        }
        /// <summary>
        /// Flush added script statement to the page with custom script key
        /// </summary>
        /// <param name="keyName">Custom unique key for script block</param>
        public void FlushAjaxScriptBlock(string keyName)
        {
            if (this.ajaxScriptBlock != null && this.ajaxScriptBlock.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), keyName, this.ajaxScriptBlock.ToString(), true);
                this.ajaxScriptBlock.Length = 0;
            }
        }

        /// <summary>
        /// Flush added script statement to the page with default script key
        /// </summary>
        public void FlushAjaxStartupScriptBlock()
        {
            this.FlushAjaxStartupScriptBlock("_ajax_startup_script_block");
        }
        /// <summary>
        /// Flush added script statement to the page with custom script key
        /// </summary>
        /// <param name="keyName">Custom unique key for script block</param>
        public void FlushAjaxStartupScriptBlock(string keyName)
        {
            if (this.ajaxStartupScriptBlock != null && this.ajaxStartupScriptBlock.Length > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), keyName, this.ajaxStartupScriptBlock.ToString(), true);
                this.ajaxStartupScriptBlock.Length = 0;
            }
        }

        /// <summary>
        /// Get script statement for assign initialValue to html control
        /// </summary>
        /// <param name="controlID">HTML control ID (full name)</param>
        /// <param name="controlType">Conrol type</param>
        /// <param name="initialValue">Value to assign</param>
        /// <returns>Javascript statement</returns>
        public string GetControlClearStateScript(string controlID, HTMLControlType controlType, string initialValue)
        {
            StringBuilder statement = new StringBuilder();
            statement.AppendFormat("$get('{0}').", controlID);

            switch (controlType)
            {
                case HTMLControlType.Textbox:
                    statement.AppendFormat("value='{0}';", initialValue);
                    break;
                case HTMLControlType.Checkbox:
                    statement.AppendFormat("checked={0};", initialValue);
                    break;
                case HTMLControlType.Combobox:
                    statement.AppendFormat("selectedIndex={0};", initialValue);
                    break;
                default:
                    statement.Length = 0;
                    break;
            }
            return statement.ToString();
        }

        #endregion

        #region App Version
        //Removed because this method returns library dll verions instead of calling application
        /// <summary>
        /// Current Application Name - exec assembly version
        /// </summary>
        public virtual string CurrentAppVersion
        {
            get
            {
                return FrontDesk.Common.Versioning.AppVersion.CurrentAppVersion;
            }
        }

        #endregion

        #region Control Callback Ajax State
        /// <summary>
        /// Get if page currently processed in Ajax Async mode
        /// </summary>
        public bool IsInAjaxPostBack
        {
            get
            {
                var ajaxPostBackEnabled = false;
                var manager = ScriptManager.GetCurrent(this);
                if (manager != null)
                {
                    ajaxPostBackEnabled = manager.IsInAsyncPostBack;
                }
                return ajaxPostBackEnabled;
            }
        }


        #endregion

        #region Cache

        /// <summary>
        /// Set HTTP Header to Disable cache
        /// </summary>
        public void DisableCache()
        {
            //disable cache
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();


        }

        #endregion

        
        #region Error Handling
        /// <summary>
        /// Path to the error page
        /// </summary>
        [Obfuscation(Feature = "renaming", Exclude = true)]
        protected virtual string ErrorPagePath { get { return "~/Error.aspx"; } }

        /// <summary>
        /// Redirect to error page
        /// </summary>
        /// <param name="errorMessage"></param>
        public virtual void RedirectToErrorPage(string errorMessage)
        {
            Server.Transfer(string.Format("{0}?msg={1}", ErrorPagePath, Server.UrlEncode(errorMessage)));
        }

        public virtual void RedirectToErrorPage()
        {
            Server.Transfer(string.Format("{0}?msg={1}", ErrorPagePath, Server.UrlEncode(CustomError.GetInternalErrorMessage())));
        }
       

        #endregion


        #region Paging - Bind custom paging template for GridView
        /// <summary>
        /// Register grid to use custom paging.
        /// Requirement: GridView PagingTemplate must have Placeholder with id "phPager"
        /// </summary>
        /// <param name="gvGrid"></param>
        public void RegisterGridViewForCustomPaging(GridView gvGrid)
        {
            gvGrid.DataBound += new EventHandler(gvGrid_PagingDataBound);

           

        }
        private void gvGrid_PagingDataBound(object sender, EventArgs e)
        {
            SetPaging((GridView)sender);
        }

        private void SetPaging(GridView gvGrid)
        {
            GridViewRow row = gvGrid.BottomPagerRow;
            Label span = null;
            HyperLink linkBtn = null;

            int startPage, endPage, pageCount;
            if (gvGrid.Rows.Count > 0)
            {
                PlaceHolder place = row.FindControl("phPager") as PlaceHolder;
                if (place != null)
                {
                    Panel div = new Panel();
                    place.Controls.Add(div);
                    div.CssClass = "pager";
                    div.EnableViewState = true;

                    int currentPage = gvGrid.PageIndex;
                    int currentPageRange = currentPage / gvGrid.PagerSettings.PageButtonCount;
                    pageCount = gvGrid.PageCount;


                    startPage = currentPageRange * gvGrid.PagerSettings.PageButtonCount;
                    endPage = startPage + gvGrid.PagerSettings.PageButtonCount;

                    if (endPage >= pageCount) endPage = pageCount;

                    Label pageLabel = new Label();
                    pageLabel.CssClass = "cur_page";
                    pageLabel.Text = string.Format("Page {0} of {1}", currentPage + 1, gvGrid.PageCount);
                    div.Controls.Add(pageLabel);


                    //render Previous and ... link before page navigation if needed

                    if (currentPageRange > 0)
                    {
                        //render First link
                        span = new Label();
                        span.CssClass = "first";
                        div.Controls.Add(span);

                        linkBtn = new HyperLink();
                        linkBtn.NavigateUrl = GetPagingNavigationUrl(gvGrid, 0);
                        //linkBtn.CommandArgument = (1).ToString();
                        linkBtn.Text = gvGrid.PagerSettings.FirstPageText;
                        span.Controls.Add(linkBtn);
                    }

                    if (currentPage > 0)
                    { //render Previous link

                        span = new Label();
                        span.CssClass = "previous";
                        div.Controls.Add(span);

                        linkBtn = new HyperLink();
                        linkBtn.NavigateUrl = GetPagingNavigationUrl(gvGrid, currentPage -1);
                        //linkBtn.CommandArgument = (currentPage).ToString();
                        linkBtn.Text = gvGrid.PagerSettings.PreviousPageText;
                        span.Controls.Add(linkBtn);
                    }
                    if (currentPageRange > 0)
                    {

                        //render ... link
                        span = new Label();
                        span.CssClass = "previous_range";
                        div.Controls.Add(span);

                        linkBtn = new HyperLink();
                        linkBtn.NavigateUrl = GetPagingNavigationUrl(gvGrid, startPage - gvGrid.PagerSettings.PageButtonCount);
                        //linkBtn.CommandArgument = (startPage - gvGrid.PagerSettings.PageButtonCount + 1).ToString();
                        linkBtn.Text = "...";
                        span.Controls.Add(linkBtn);

                    }


                    for (int i = startPage; i < endPage; i++)
                    {
                        Label pageLink = new Label();
                        pageLink.CssClass = "page_link";
                        div.Controls.Add(pageLink);

                        HyperLink btn = new HyperLink();
                        btn.NavigateUrl = GetPagingNavigationUrl(gvGrid, i);
                        //btn.CommandName = "Page";
                        //btn.CommandArgument = (i + 1).ToString();

                        if (i == gvGrid.PageIndex)
                        {
                            pageLink.CssClass += " selected";
                        }

                        btn.Text = (i + 1).ToString();
                        //btn.ToolTip = "Page " + (i + 1).ToString();


                        pageLink.Controls.Add(btn);

                        //Label lbl = new Label();
                        //lbl.Text = " ";
                        //place.Controls.Add(lbl);
                    }

                    if (pageCount > endPage) //show "..." and "Next" link if we have more pages
                    {
                        //render ... button
                        span = new Label();
                        span.CssClass = "next_range";
                        div.Controls.Add(span);

                        linkBtn = new HyperLink();
                        linkBtn.NavigateUrl = GetPagingNavigationUrl(gvGrid, endPage);
                        //linkBtn.CommandArgument = (endPage + 1).ToString();
                        linkBtn.Text = "...";
                        span.Controls.Add(linkBtn);



                    }

                    if (currentPage < pageCount - 1)
                    {
                        span = new Label();
                        span.CssClass = "next";
                        div.Controls.Add(span);

                        linkBtn = new HyperLink();
                        linkBtn.NavigateUrl = GetPagingNavigationUrl(gvGrid, currentPage + 1);
                        //linkBtn.CommandArgument = (currentPage + 2).ToString();
                        linkBtn.Text = gvGrid.PagerSettings.NextPageText;
                        span.Controls.Add(linkBtn);
                    }

                    if (currentPage != pageCount - 1) //render last page
                    {
                        span = new Label();
                        span.CssClass = "last";
                        div.Controls.Add(span);

                        linkBtn = new HyperLink();
                        linkBtn.NavigateUrl = GetPagingNavigationUrl(gvGrid, pageCount - 1);
                        
                        //linkBtn.CommandArgument = (pageCount).ToString();
                        linkBtn.Text = gvGrid.PagerSettings.LastPageText;
                        span.Controls.Add(linkBtn);
                    }
                }

            }
        }
        /// <summary>
        /// Get command link to particular page.
        /// </summary>
        /// <param name="gvGrid">Navigating grid view</param>
        /// <param name="pageIndex">Zero based page index</param>
        /// <returns></returns>
        private string GetPagingNavigationUrl(GridView gvGrid, int pageIndex)
        {
            return string.Format("javascript:__doPostBack('{0}','{1}${2}')", gvGrid.UniqueID, "Page", pageIndex + 1);
        }

        #endregion

        #region LicenseKey and Activation Validation

        void BasePage_InitComplete(object sender, EventArgs e)
        {
        }

        #endregion

       
    }

    /// <summary>
    /// HTML control type for AJAX script methods
    /// </summary>
    public enum HTMLControlType
    {
        /// <summary>
        /// None - default
        /// </summary>
        None,
        /// <summary>
        /// Textbox or any control that has 'value' property for keep value
        /// </summary>
        Textbox,
        /// <summary>
        /// Drop-down list
        /// </summary>
        Combobox,
        /// <summary>
        /// Checkbox - has checked boolean property
        /// </summary>
        Checkbox
    }

}