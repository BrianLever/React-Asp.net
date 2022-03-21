<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModifyPassword.ascx.cs"
    Inherits="FrontDesk.Control.ModifyPasswordCtrl" %>
<div class="grid2col">
    <asp:PlaceHolder ID="phOldPasswordRequired" runat="server">
        <div class="column left">
            <cc:FormLabel ID="lblPwd" runat="server" Text="Password" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="400px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="vldPassword" runat="server" ControlToValidate="txtPassword"
                ErrorMessage="Password is required" Display="Dynamic" CssClass="lpad5" />
        </div>
    </asp:PlaceHolder>
    <div class="column left">
        <cc:FormLabel ID="lblNewPwd" runat="server" Text="New Password" Mandatory="true"></cc:FormLabel>
    </div>
    <div class="column right">
        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" Width="400px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="vldNewPassword" runat="server" ControlToValidate="txtNewPassword"
            ErrorMessage="New Password is required" Display="Dynamic" />
        <asp:RegularExpressionValidator ID="revNewPassword" runat="server" ControlToValidate="txtNewPassword"
            ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$" ErrorMessage="Password must contain one lowercase character, one uppercase character, one numeric character and should be at least 6 characters"></asp:RegularExpressionValidator>
        <asp:CustomValidator runat="server" ID="cvldNewPassword" OnServerValidate="ValidationNewPassword"
            ErrorMessage="Password can not contain your user name" Display="Static" ControlToValidate="txtNewPassword"></asp:CustomValidator>
    </div>
    <div class="column left">
        <cc:FormLabel ID="lblConfirmNewPwd" runat="server" Text="Confirm New Password" Mandatory="true"></cc:FormLabel>
    </div>
    <div class="column right">
        <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="Password" Width="400px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvConfirmNewPassword" runat="server" ControlToValidate="txtConfirmNewPassword"
            ErrorMessage="New Password confirmation is required" Display="Dynamic" />
        <asp:CompareValidator ID="vldCmpPassword" runat="server" ControlToValidate="txtConfirmNewPassword"
            ControlToCompare="txtNewPassword" SetFocusOnError="True" ErrorMessage="New Password and New Password confirmation must match"></asp:CompareValidator>
    </div>
</div>
