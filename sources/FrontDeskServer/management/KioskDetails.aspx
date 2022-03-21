<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="KioskDetails.aspx.cs" Inherits="KioskManagement_KioskDetails" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="grid2col">
        <div class="column left">
            &nbsp;</div>
        <div class="column right disabled">
            <asp:Label runat="server" ID="lblStatus"></asp:Label>
        </div>
        <asp:PlaceHolder runat="server" ID="phKioskKey">
            <div class="column left">
                <cc:FormLabel runat="server" ID="lblKey" Text="Kiosk key"></cc:FormLabel>
            </div>
            <div class="column right">
                <asp:Label runat="server" ID="lblKeyValue"></asp:Label><span class="note lmar10"><asp:Localize runat="server" Text="<%$ Resources: TextMessages, KioskKeyLabelNotes %>" /></span>
            </div>
        </asp:PlaceHolder>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblLocation" Text="Branch location" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:DropDownList runat="server" ID="ddlBranch" DataValueField="BranchLocationID"
                DataTextField="Name" DataSourceID="odsrBranch">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="vldBranch" runat="server" Display="Dynamic" CssClass="error_message"
                ControlToValidate="ddlBranch" ErrorMessage="Branch location is required" ValidationGroup="ObjectInfo" />
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblName" Text="Kiosk name" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox runat="server" ID="txtName"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="vldName" ControlToValidate="txtName"
                ErrorMessage="Kiosk name is required" Display="Dynamic" ValidationGroup="ObjectInfo"></asp:RequiredFieldValidator>
        </div>
         <div class="column left">
            <cc:FormLabel runat="server" ID="lblScreeningProfile" Text="Screen Profile"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label runat="server" ID="lblScreeningProfileName" ></asp:Label>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblSecret" Text="Secret Key"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox runat="server" ID="txtSecret" MaxLength="64" ></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblDescription" Text="Description"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Height="100"></asp:TextBox>
        </div>
        <div class="column left">
          </div>
        <div class="column w350px l tpad10">
            <asp:ValidationSummary runat="server" ID="vldSummary" ValidationGroup="ObjectInfo" />
        </div>
        <div class="column left">
            &nbsp;</div>
        <div class="column right buttons">
            <asp:Button runat="server" ID="btnAdd" Text="Create" ValidationGroup="ObjectInfo"
                OnClick="btnAdd_Click" />
            <asp:Button runat="server" ID="btnSave" Text="Save Changes" ValidationGroup="ObjectInfo"
                OnClick="btnUpdate_Click" />
            <asp:Button runat="server" ID="btnDelete" Text="Delete" CausesValidation="false"
                OnClick="btnDelete_Click" OnClientClick="if(!confirm('Are you sure that you want to delete this kiosk?')){return false;}" />
            <asp:Button runat="server" ID="btnEnabled" CausesValidation="false" OnClick="btnEnabled_Click" />
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                OnClientClick="location.href = 'KioskList.aspx'; return false" />
        </div>
    </div>
    <%-- Get all branch --%>
    <asp:ObjectDataSource runat="server" ID="odsrBranch" TypeName="FrontDesk.Server.Screening.Services.BranchLocationService"
        EnableCaching="false" CacheExpirationPolicy="Absolute" SelectMethod="GetAll"></asp:ObjectDataSource>
</asp:Content>
