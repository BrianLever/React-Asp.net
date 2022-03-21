<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true" CodeFile="AssignLicense.aspx.cs" Inherits="AssignLicense" %>
<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">

    <div class="grid2col w80 c">

        <div class="column joined linked">
            <div class="title">
                Assign new license to 
                <asp:HyperLink ID="hlClient" runat="server" Target="_blank" NavigateUrl="~/ClientDetails.aspx"></asp:HyperLink>.
            </div>
        </div>


        <div class="column left">
            <cc:FormLabel runat="server" ID="lblLicKey" Text="License Key" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtLicenseKey" runat="server" MaxLength="128"></asp:TextBox>   
            <asp:RequiredFieldValidator ID="vldLicense" runat="server" Display="Dynamic"
                CssClass="error_message" ControlToValidate="txtLicenseKey" ErrorMessage="License Key is required"
                ValidationGroup="ObjectInfo" />              
        </div>
    
        <div class="column left">
            &nbsp;
        </div>
        <div class="column right">
            <asp:ValidationSummary runat="server" ID="vldSummary" ValidationGroup="ObjectInfo" />
        </div>    

        <div class="column left">
            &nbsp;
        </div>
        <div class="column lmar10 tpad15 w350px r">
            <asp:Button runat="server" ID="btnAssign" Text="Assign" ValidationGroup="ObjectInfo" 
                UseSubmitBehavior="false" onclick="btnAssign_Click" />
                        
            <asp:Button runat="server" ID="btnBack" Text="Back" UseSubmitBehavior="false" OnClientClick="location.href='Clients.aspx'; return false" />
        </div>
    
    </div>

</asp:Content>

