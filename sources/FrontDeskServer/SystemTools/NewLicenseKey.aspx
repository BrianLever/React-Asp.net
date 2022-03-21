<%@ Page Title="" Language="C#" MasterPageFile="~/SystemTools/SystemTools.master"
    AutoEventWireup="true" CodeFile="NewLicenseKey.aspx.cs" Inherits="SystemTools_NewLicenseKey" %>

<%@ MasterType VirtualPath="~/SystemTools/SystemTools.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSystemTools" runat="Server">
    <div>
        <ul class="licenseKey">
            <li>
                <cc:FormLabel runat="server" Text="Enter License Key" /></li>
            <li>
                <asp:TextBox runat="server" ID="txtLicenseKey"></asp:TextBox>
                <asp:RequiredFieldValidator ID="vldLicenseKey" runat="server" ControlToValidate="txtLicenseKey"
                    ErrorMessage="" Text="*" EnableClientScript="true" Display="Static" />
            </li>
            <li>
                <asp:UpdatePanel runat="server" ID="updResult" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Label runat="server" CssClass="error" ID="lblValidationResult" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRegister" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </li>
            <li>
                <asp:Button runat="server" ID="btnRegister" Text="Register" OnClick="btnRegister_Click" />
            </li>
        </ul>
    </div>
</asp:Content>
