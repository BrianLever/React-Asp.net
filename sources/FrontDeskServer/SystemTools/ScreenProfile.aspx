<%@ Page Title="" Language="C#" MasterPageFile="~/systemtools/ScreeningProfile.master" AutoEventWireup="true"
    CodeFile="ScreenProfile.aspx.cs" Inherits="management_ScreenProfilePage" %>

<%@ MasterType VirtualPath="~/systemtools/ScreeningProfile.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSubContent" runat="Server">
    <div class="grid2col">
        <div class="column left">
            &nbsp;</div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblName" Text="Screens Profile Name" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtName" runat="server" MaxLength="128"></asp:TextBox>
            <asp:RequiredFieldValidator ID="vldName" runat="server" Display="Dynamic" CssClass="error_message"
                ControlToValidate="txtName" ToolTip="Field is required" ErrorMessage="Screen Profile Name is required"
                ValidationGroup="ObjectInfo" />
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="frmDescription" Text="Description"></cc:FormLabel>
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
            <asp:Button runat="server" ID="btnDelete" Text="Delete" OnClick="btnDelete_Click" />
            <asp:Button runat="server" ID="btnCancel" Text="Back to List" OnClick="btnCancel_Click" />
        </div>
    </div>
</asp:Content>
