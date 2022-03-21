using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using AjaxControlToolkit;
using System.Reflection;

#region [ Resources ]

[assembly: System.Web.UI.WebResource("FrontDesk.Server.Web.Controls.Grid.HierarDynamicGridBehavior.js", "text/javascript")]

#endregion

namespace FrontDesk.Server.Web.Controls.Grid
{
    [ClientScriptResource("FrontDesk.Server.Web.Controls.HierarDynamicGridBehavior", "FrontDesk.Server.Web.Controls.Grid.HierarDynamicGridBehavior")]
    [TargetControlType(typeof(Control))]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]
    public class HierarDynamicGridExtender : ExtenderControlBase
    {
        [DefaultValue("~/images/collapse_grid.png")]
        [ExtenderControlProperty]
        [ClientPropertyName("expandedImageUrl")]
        public virtual string ExpandedImageUrl
        {
            get { return GetPropertyValue("ExpandedImageUrl", string.Empty); }
            set { SetPropertyValue("ExpandedImageUrl", value); }
        }
        [DefaultValue("~/images/expand_grid.png")]
        [ExtenderControlProperty]
        [ClientPropertyName("collapsedImageUrl")]
        public virtual string CollapsedImageUrl
        {
            get { return GetPropertyValue("CollapsedImageUrl", string.Empty); }
            set { SetPropertyValue("CollapsedImageUrl", value); }
        }
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("dataKey")]
        public virtual string DataKey
        {
            get { return GetPropertyValue("DataKey", string.Empty); }
            set { SetPropertyValue("DataKey", value); }
        }


        public HierarDynamicGridExtender()
        {
            CollapsedImageUrl = "~/images/expand_grid.png";
            ExpandedImageUrl = "~/images/collapse_grid.png";
        }
        

    }
}
