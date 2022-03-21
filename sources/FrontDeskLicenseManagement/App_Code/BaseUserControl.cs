﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace FrontDesk.Server.LicenseManagerWeb.Controls
{
    /// <summary>
    /// Base class for all user controls
    /// </summary>
    public abstract class LMBaseUserControl : System.Web.UI.UserControl
    {
        public new LMBasePage Page
        {
            get
            {
                return (LMBasePage)base.Page;
            }
            set
            {
                base.Page = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        /// <summary>
        /// Impement this method to include your control into page cross tab indexing
        /// </summary>
        /// <param name="startTabIndex"></param>
        public abstract void ApplyTabIndexToControl(ref short startTabIndex);



        public T GetIDValue<T>(string sKey, PageIDSource source, out bool IsFound)
        {
            T iID = default(T);

            IsFound = false;
            try
            {


                if ((source & PageIDSource.QueryString) > 0 && !String.IsNullOrEmpty(Request.QueryString[sKey]))
                {
                    iID = Page.ConvertToType<T>(Request.QueryString[sKey]);
                    IsFound = true;
                }
                else if ((source & PageIDSource.ViewState) > 0 && ViewState[sKey] != null)
                {
                    iID = Page.ConvertToType<T>(ViewState[sKey]);
                    IsFound = true;
                }
                else if ((source & PageIDSource.Session) > 0 && Session[sKey] != null)
                {
                    iID = Page.ConvertToType<T>(Session[sKey]);
                    IsFound = true;
                }
                else if ((source & PageIDSource.Form) > 0 && Request.Form[sKey] != null)
                {
                    iID = Page.ConvertToType<T>(Request.Form[sKey]);
                    IsFound = true;
                }
                else if ((source & PageIDSource.Request) > 0 && !String.IsNullOrEmpty(Request[sKey]))
                {
                    iID = Page.ConvertToType<T>(Request[sKey]);
                    IsFound = true;
                }

            }
            catch (Exception ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.DebugException(ex);
            }
            return iID;

        }

        public T GetIDValue<T>(string sKey,  out bool IsFound)
        {
            return GetIDValue<T>(sKey, PageIDSource.Any, out IsFound);

        }
    }
}
