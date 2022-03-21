<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RpmsCredentialsExpiratonNotificationMessage.ascx.cs"
    Inherits="RpmsCredentialsExpiratonNotificationMessageCtrl" EnableViewState="false" %>

<asp:Panel CssClass="notification-box" ID="pnlNotification" runat="server">
    <asp:Image ID="imgWarn" runat="server" CssClass="warn" AlternateText="Notification"  ImageUrl="~/images/warning-icon.png" />
    <div>
        <asp:Label ID="lblRpmsCredentialsExpirationAlert" runat="server" CssClass="text"></asp:Label>
    </div>
</asp:Panel>
