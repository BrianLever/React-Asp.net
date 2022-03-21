using System;
using FrontDesk.Server.LicenseManagerWeb;

public partial class Clients : LMBasePage
{

    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Method name stays readable: unable to determine obfuscated name.
        // Method names and parameters may stay in .aspx.
        odsClients.TypeName = typeof(FrontDesk.Server.Licensing.Management.Client).FullName;

        RegisterGridViewForCustomPaging(gvClients);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Clients";

        gvClients.PageIndex = 0;
        gvClients.DataBind();
        //odsClients.SelectMethod = "GetAll";
    }

    protected void lnbNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ClientDetails.aspx");
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        gvClients.DataBind();
    }

}
