<%@ Page Language="c#" MasterPageFile="~/FrontDeskMaster.master" CodeFile="Error.aspx.cs"
    AutoEventWireup="true" Inherits="ErrorForm" Title="Error handler" EnableViewState="False"
    EnableSessionState="false"  %>

<asp:Content ContentPlaceHolderID="cphContent" runat="server">
 
    <div class="errorPage c pad10">
        <asp:Label ID="lblErrorMessage" runat="server"  />
    </div>
</asp:Content>
