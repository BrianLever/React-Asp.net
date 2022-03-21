<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword"
    MasterPageFile="~/FrontDeskMaster.master" %>

<%@ Register TagPrefix="uc" TagName="ChangePassword" Src="~/controls/ModifyPassword.ascx" %>
<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content runat="server" ID="cnt" ContentPlaceHolderID="cphContent">
    <uc:ChangePassword ID="ucChangePassword" runat="server" ValidationGroup="vldChangePassword"
        IsOldPasswordRequired="true" OnPasswordChanged="ucChangePassword_PasswordChanged"
        OnChangeFailed="ucChangePassword_ChangeFailed" />
    <div class="grid2col">
        <asp:UpdatePanel runat="server" ID="upnlChange">
            <ContentTemplate>
                <div class="column left">
                    &nbsp;
                </div>
                <div class="column right">
                    <asp:ValidationSummary ID="vsChangePassowrd" runat="server" ValidationGroup="vldChangePassword" />
                    <asp:Label runat="server" ID="lblError" CssClass="error_message"></asp:Label>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnChangePassword" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="column left">
            &nbsp;
        </div>
        <div class="column right w400px">
            <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" ValidationGroup="vldChangePassword"
                OnClick="btnChangePassword_Click" UseSubmitBehavior="true" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="location.href='UserProfile.aspx'; return false;"
                UseSubmitBehavior="false" />
        </div>
    </div>
</asp:Content>
