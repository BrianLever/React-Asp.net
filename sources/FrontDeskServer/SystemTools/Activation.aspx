<%@ Page Title="" Language="C#" MasterPageFile="~/SystemTools/SystemTools.master"
    AutoEventWireup="true" CodeFile="Activation.aspx.cs" Inherits="SystemTools_Activation" %>

<%@ MasterType VirtualPath="~/SystemTools/SystemTools.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSystemTools" runat="Server">
    <div>
        <ul class="licenseKey">
            <li>
                <cc:FormLabel runat="server" Text="License Key" /></li>
            <li class="key">
                <asp:Label runat="server" ID="ltrLicenseKey" />
                <asp:LinkButton ID="btnCopyLicense" runat="server" Text="Copy" CausesValidation="false" />
            </li>
            <li>
                <cc:FormLabel runat="server" Text="Activation Request Key" /></li>
            <li class="key">
                <asp:UpdatePanel runat="server" ID="updRequset" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="ltrActivationRequest" />
                        <asp:LinkButton ID="btnCopyRequestCode" runat="server" Text="Copy" CausesValidation="false" />
                        <div class="operations">
                            <asp:LinkButton ID="btnGetRequestCode" runat="server" Text="Get" OnClick="btnGetRequestCode_Click"
                                CausesValidation="false" />
                            <asp:HyperLink ID="lnkSendRequestCode" runat="server" Text="Send by email" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGetRequestCode" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </li>
            <li>
                <cc:FormLabel ID="FormLabel1" runat="server" Text="Enter Activation Key" /></li>
            <li>
                <asp:TextBox runat="server" ID="txtActivationKey"></asp:TextBox>
                <asp:RequiredFieldValidator ID="vldActivationKey" runat="server" ControlToValidate="txtActivationKey"
                    ErrorMessage="" Text="*" EnableClientScript="true" Display="Static" />
            </li>
            <li>
                <asp:UpdatePanel runat="server" ID="updResult" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" CssClass="error" ID="lblValidationResult" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnActivate" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </li>
            <li>
                <asp:Button runat="server" ID="btnActivate" Text="Activate" OnClick="btnActivate_Click" />
            </li>
        </ul>
    </div>
</asp:Content>
