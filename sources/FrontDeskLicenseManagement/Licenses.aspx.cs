using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.LicenseManagerWeb;

public partial class Licenses : LMBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Method name stays readable: unable to determine obfuscated name.
        // Method names and parameters may stay in .aspx.
        odsLicenses.TypeName = typeof(FrontDesk.Server.Licensing.Management.LicenseEntityHelper).FullName;

        RegisterGridViewForCustomPaging(gvLicenses);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Licenses";
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        gvLicenses.DataBind();
    }

    protected void lnbNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/LicenseDetails.aspx");
    }

    protected void gvLicenses_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.OnGridViewSorting(sender, e);
    }

    protected void ucFilter_Searching(object sender, FilterSearchingEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.FilterBy))
        {
            odsLicenses.SelectParameters["licenseKey"].DefaultValue = e.Value.ToString();
        }
        else
        {
            odsLicenses.SelectParameters["licenseKey"].DefaultValue = String.Empty;
        }

        gvLicenses.PageIndex = 0;
        gvLicenses.DataBind();
    }
}
