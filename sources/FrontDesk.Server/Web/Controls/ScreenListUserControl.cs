using FrontDesk.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Web.Controls
{
    public abstract class ScreenListUserControl: BaseUserControl
    {
        #region Filter properties

        protected string FirstnameFilter
        {
            get
            {
                bool isFound;
                string value = this.GetIDValue<string>("FirstnameFilter", out isFound);
                if (!isFound)
                    return String.Empty;
                else
                {
                    return value;
                }
            }
            set
            {
                ViewState["FirstnameFilter"] = value;
                Session["FirstnameFilter"] = value;
            }
        }
        protected string LastnameFilter
        {
            get
            {
                bool isFound;
                string value = this.GetIDValue<string>("LastnameFilter", out isFound);
                if (!isFound)
                    return String.Empty;
                else
                {
                    return value;
                }
            }
            set
            {
                ViewState["LastnameFilter"] = value;
                Session["LastnameFilter"] = value;
            }
        }

        protected long? ScreenDoxIDFilter
        {
            get
            {
                bool isFound;
                string value = this.GetIDValue<string>("ScreenDoxIDFilter", out isFound);
                if (!isFound)
                {
                    return null;
                }
                else
                {
                    return value.ParseTo<long>();
                }
            }
            set
            {
                ViewState["ScreenDoxIDFilter"] = value;
                Session["ScreenDoxIDFilter"] = value;
            }
        }
        protected int? LocationFilter
        {
            get
            {
                bool isFound;
                int value = this.GetIDValue<int>("LocationFilter", out isFound);
                if (!isFound)
                    return null;
                else
                {
                    return (int)value;
                }
            }
            set
            {
                ViewState["LocationFilter"] = value;
                Session["LocationFilter"] = value;
            }
        }

        protected bool AutoRefreshEnabled
        {
            get
            {
                bool isFound;
                bool value = GetIDValue<bool>("CheckIn_AutoRefreshEnabled", out isFound);
                if (!isFound)
                    return true;
                else
                {
                    return value;
                }
            }
            set
            {
                ViewState["CheckIn_AutoRefreshEnabled"] = value;
                Session["CheckIn_AutoRefreshEnabled"] = value;
            }
        }

        #endregion

        protected bool _forceClearSearchParams = false; //true when clear button has been pressed

    }
}
