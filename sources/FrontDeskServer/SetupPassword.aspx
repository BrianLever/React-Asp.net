<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetupPassword.aspx.cs" Inherits="SetupPassword"
    MasterPageFile="~/FrontDeskMaster.master" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>

<%@ Register TagPrefix="uc" TagName="ChangePassword" Src="~/controls/ModifyPassword.ascx" %>
<%@ Register TagPrefix="uc" TagName="ChangeSequrity" Src="~/controls/ModifySequrityQuestion.ascx" %>

<asp:Content runat="server" ID="cnt" ContentPlaceHolderID="cphContent">
    <uc:ChangePassword runat="server" ID="ucChangePassword" IsOldPasswordRequired="true"
        ValidationGroup="PasswordSetup" OnPasswordChanged="ucChangePassword_PasswordChanged"
        OnChangeFailed="ucChangePassword_ChangeFailed" />
    <uc:ChangeSequrity runat="server" ID="ucChangeQuestion" ValidationGroup="PasswordSetup"
        OnChangeFailed="ucChangeQuestion_ChangeFailed" OnPasswordChanged="ucChangeQuestion_Changed" />
    <div class="grid2col">
        <asp:UpdatePanel runat="server" ID="upnlChange" >
            <ContentTemplate>
                <div class="column left">
                    &nbsp;
                </div>
                <div class="column right">
                    <asp:ValidationSummary ID="vsChangePassowrd" runat="server" ValidationGroup="PasswordSetup" />
                    <asp:Label runat="server" ID="lblError" CssClass="error_message"></asp:Label>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="column left">
            &nbsp;
        </div>
        <div class="column lpad20 tpad15 w400px r">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" ValidationGroup="PasswordSetup" />
        </div>
    </div>
</asp:Content>
