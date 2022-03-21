<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="ChangeSecurityQuestion.aspx.cs" Inherits="ChangeSecurityQuestion" %>

<%@ Register Src="~/controls/ModifySequrityQuestion.ascx" TagPrefix="uc" TagName="ChangeSecurityQuestion" %>
<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content ID="cnt" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="grid2col">
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblPsssword" Text="Password"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox runat="server" ID="txtPassword" CssClass="w400px" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="vldPassword" runat="server" ControlToValidate="txtPassword"
                ErrorMessage="Password is required" Display="Dynamic" ValidationGroup="ChangeQuestion" />
        </div>
    </div>
    <uc:ChangeSecurityQuestion ID="ucChangeQuestion" runat="server" ValidationGroup="ChangeQuestion"
        OnChangeFailed="ucChangeQuestion_ChangeFailed" OnPasswordChanged="ucChangeQuestion_PasswordChanged" />
    <div class="grid2col">
        <asp:UpdatePanel runat="server" ID="upnlChange">
            <ContentTemplate>
                <div class="column left">
                    &nbsp;
                </div>
                <div class="column right">
                    <asp:ValidationSummary ID="vsChangePassowrd" runat="server" ValidationGroup="ChangeQuestion" />
                    <asp:Label runat="server" ID="lblError" CssClass="error_message"></asp:Label>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnChange" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="column left">
            &nbsp;
        </div>
        <div class="column right">
            <asp:Button ID="btnChange" runat="server" Text="Save" ValidationGroup="ChangeQuestion"
                OnClick="btnChange_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="location.href='UserProfile.aspx'; return false;"
                UseSubmitBehavior="false" />
        </div>
    </div>
</asp:Content>
