<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="BranchLocationDetails.aspx.cs" Inherits="management_BranchLocationDetails" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="grid2col">
        <div class="column left">
            &nbsp;</div>
        <div class="column right disabled">
            <asp:Label runat="server" ID="lblStatus"></asp:Label>&nbsp;
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblName" Text="Location name" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtName" runat="server" MaxLength="128"></asp:TextBox>
            <asp:RequiredFieldValidator ID="vldName" runat="server" Display="Dynamic" CssClass="error_message"
                ControlToValidate="txtName" ToolTip="Field is required" ErrorMessage="Location name is required"
                ValidationGroup="ObjectInfo" />
        </div>

         <div class="column left">
            <cc:FormLabel runat="server" ID="lblProfile" Text="Screen profile" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:DropDownList runat="server" ID="ddlScreeningProfile" DataValueField="ID"
                DataTextField="Name" DataSourceID="odsrScreeningProfile" AppendDataBoundItems="true">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="vldScreeningProfile" runat="server" Display="Dynamic" CssClass="error_message"
                ControlToValidate="ddlScreeningProfile" ErrorMessage="Screen profile is required" ValidationGroup="ObjectInfo" />
        </div>

        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel2" Text="Description"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtDescription" runat="server" Height="100px" TextMode="MultiLine"
                MaxLength="128"></asp:TextBox>
        </div>
        <div class="column left">
         
        </div>
        <div class="column right">
            <asp:ValidationSummary ID="valSummary" runat="server" ValidationGroup="ObjectInfo" />
        </div>
        <div class="column left">
            &nbsp;
        </div>
        <div class="column buttons right">
            <asp:Button runat="server" ID="btnSave" Text="Save changes" OnClick="btnSave_Click"
                ValidationGroup="ObjectInfo"/>
            <asp:Button runat="server" ID="btnDelete" Text="Delete" OnClick="btnDelete_Click"
                OnClientClick="if(!confirm('Are you sure that you want to delete this Branch Location?')){return false;}" />
            <asp:Button runat="server" ID="btnEnabled" CausesValidation="false" OnClick="btnEnabled_Click" />
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" />
        </div>
    </div>

    <asp:ObjectDataSource runat="server" ID="odsrScreeningProfile" TypeName="FrontDesk.Server.Screening.Services.ScreeningProfileService"
        EnableCaching="false" CacheExpirationPolicy="Absolute" SelectMethod="GetAll"></asp:ObjectDataSource>
</asp:Content>
