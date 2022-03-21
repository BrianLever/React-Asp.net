using System;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections.Generic;

namespace FrontDesk.Server.Web
{
    /// <summary>
    /// Base class for web forms that manage some business entity
    /// I.E. Client Management, User Management etc,
    /// </summary>
    //[Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public abstract class BaseWebForm : BasePage
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseWebForm()
        {
            this.Init += new EventHandler(BaseWebForm_Init);
        }

        #region Accessors

        /// <summary>
        /// Virtual property and should be overrided in the derived page class
        /// </summary>
        public abstract bool IsNewInstance { get; }

        /// <summary>
        /// Return true if object info was retreived successfully from the database using FormObjectID value
        /// </summary>
        /// <remarks>OnFormObjectIDNotFound method not invoked</remarks>
        public abstract bool IsFormObjectInitialized { get; }

        /// <summary>
        /// Init form business object if object id was evaluated
        /// </summary>
        public abstract void EnsureFormObjectCreated();

        #endregion

        #region Page View/Edit mode Properties & Methods

        /// <summary>
        /// Set page mode : is edit enabled or not.
        /// If true - page in EDIT mode, If false - page in READONLY mode
        /// </summary>
        public bool IsEditEnabled = false;

        /// <summary>
        /// Turn page mode back Edit Mode
        /// </summary>
        public void RedirectToEditMode()
        {
            string newPath = string.Empty;
            try
            {
                string originalPath = Request.Url.ToString();
                newPath = originalPath;

                Regex regex = new Regex(EDIT_MODE_KEY + "=[^&]*");
                if (regex.IsMatch(originalPath, 0))
                {
                    newPath = regex.Replace(originalPath, EDIT_MODE_KEY + "=true");
                }
                else
                {
                    if (originalPath.IndexOf("?") > 0) originalPath = string.Concat(originalPath, "&");
                    else originalPath = string.Concat(originalPath, "?");
                    originalPath = string.Concat(originalPath, EDIT_MODE_KEY + "=true");

                    newPath = originalPath;
                }
                Response.Redirect(newPath);
            }
            catch (System.Threading.ThreadAbortException) { }
        }



        public static string GetRedirectToPageUrl(string url, IDictionary<string, object> parameters)
        {
            string newPath = string.Empty;

            string originalPath = url;
            newPath = originalPath;
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> kp in parameters)
                {
                    Regex regex = new Regex(kp.Key + "=[^&]*");
                    if (regex.IsMatch(originalPath, 0))
                    {
                        newPath = regex.Replace(newPath, string.Format("{0}={1}", kp.Key, kp.Value));
                    }
                    else
                    {
                        if (newPath.IndexOf("?") > 0) newPath = string.Concat(newPath, "&");
                        else newPath = string.Concat(newPath, "?");
                        newPath += string.Format("{0}={1}", kp.Key, kp.Value);
                    }
                }
            }
            return newPath;

        }

        public void RedirectToRefresh()
        {
            string originalPath = Request.Url.ToString();

            Response.Redirect(originalPath, false);
        }

        /// <summary>
        /// Turn page mode back to read/view mode from Edit Mode
        /// </summary>
        public void RedirectBackFromEditMode()
        {
            string newPath = string.Empty;
            try
            {
                string originalPath = Request.Url.ToString();
                if (!IsNewInstance)
                {
                    //if not Add mode
                    newPath = originalPath;

                    Regex regex = new Regex(EDIT_MODE_KEY + "=[^&]*");
                    if (regex.IsMatch(originalPath, 0))
                    {
                        newPath = regex.Replace(originalPath, EDIT_MODE_KEY + "=false");
                    }

                    //remove request edit param
                    regex = new Regex("[&]?" + EDIT_MODE_REQUEST_KEY + "=[^&]*");
                    if (regex.IsMatch(originalPath, 0))
                    {
                        newPath = regex.Replace(originalPath, string.Empty);
                    }


                }
                else
                {
                    newPath = this.BackPageUrl;
                }

                Response.Redirect(newPath);
            }
            catch (System.Threading.ThreadAbortException) { }
        }


        #endregion

        #region Page & Page Controls Event Handlers

        private const string EDIT_MODE_KEY = "edit_mode";
        /// <summary>
        /// Request GET parameter key nam for set on/off edit mode. Acceptable values: 1 - edit mode, any other - read only mode
        /// </summary>
        public virtual string EDIT_MODE_REQUEST_KEY { get { return "edit"; } }

        /// <summary>
        /// Apply Edit Mode init settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseWebForm_Init(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("BaseWebForm.Init event handler invoked");
            bool IsEditFound = false;

            if (!IsEditEnabled) //if Edit mode has not been overriden in SetPermissions method - get it value.
            {

                IsEditEnabled = this.GetPageIDValue<bool>(EDIT_MODE_KEY, out IsEditFound) && HasWritePermission;

                //try to find edit parameter with other name in querystring
                if (!IsEditFound && !IsPostBack)
                {
                    IsEditEnabled = (this.GetPageIDValue<int>(EDIT_MODE_REQUEST_KEY, PageIDSource.Request, out IsEditFound) > 0) && HasWritePermission;

                }
            }
        }




        /// <summary>
        /// Save Edit Mode state
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            //save mode parameter to ViewState
            this.ViewState.Add(EDIT_MODE_KEY, IsEditEnabled);

            base.OnPreRender(e);
        }

        #endregion

        #region Page Processing flow

        /// <summary>
        /// Bind data to list and gridviews if needed
        /// </summary>
        [Obfuscation(Feature = "renaming", Exclude = true)]
        protected virtual void BindPageListControls() { }




        #endregion
    }
}
