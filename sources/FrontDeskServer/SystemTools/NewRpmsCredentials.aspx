<%@ Page Title="" Language="C#" MasterPageFile="~/SystemTools/SystemTools.master"
    AutoEventWireup="true" CodeFile="NewRpmsCredentials.aspx.cs" Inherits="SystemTools_NewRpmsCredentials" %>

<%@ MasterType VirtualPath="~/SystemTools/SystemTools.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSystemTools" runat="Server">
    <div class="grid2col" id="rpmsCredentialsForm">
        <div class="column left">
            <cc:FormLabel runat="server" Text="Access Code" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox runat="server" ID="txtAccessCode"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="vldAccessCode" ControlToValidate="txtAccessCode"
                ErrorMessage="Access code is required" Display="Dynamic" ValidationGroup="ObjectInfo"></asp:RequiredFieldValidator>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" Text="Verify Code" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox runat="server" ID="txtVerifyCode"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="vldVerifyCode" ControlToValidate="txtVerifyCode"
                ErrorMessage="Verify code is required" Display="Dynamic" ValidationGroup="ObjectInfo"></asp:RequiredFieldValidator>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" Text="Expire On" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
              <cc:RichDatePicker ID="dtExpireOnDate" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="vldExpireOnDate" ControlToValidate="dtExpireOnDate"
                ErrorMessage="Expire On is required" Display="Dynamic" ValidationGroup="ObjectInfo"></asp:RequiredFieldValidator>
        </div>
        <div class="column w350px r lpad10">
            <asp:Button runat="server" ID="btnAdd" Text="Save" ValidationGroup="ObjectInfo"
                OnClick="btnAdd_Click" />
          
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                OnClientClick="location.href = 'RpmsCredentials.aspx'; return false" />
        </div>

    </div>



   
</asp:Content>
