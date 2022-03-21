<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="LoginForm"
	MasterPageFile="~/FrontDeskMaster.master" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content ContentPlaceHolderID="cphContentRaw" ID="content1" runat="server">
	<div id="login-outer" style="visibility: collapse">
			<asp:Login ID="UserLogin" Width="350px" runat="server" DisplayRememberMe="false"
				OnAuthenticate="Login_Authenticate" CssClass="block-hcenter login-form">
				<LayoutTemplate>
					<div>
						<div class="c b panelHeader pad5">
							Login
						</div>
					</div>
					<div>
						<div class="fleft r w30 tpad10" style="line-height: 22px;">
							<cc:FormLabel runat="server" ID="lblUserName" CssClass="nowrap b" Text="User Name"></cc:FormLabel>
						</div>
						<div class="fleft l w60 lpad5 tpad10" style="line-height: 22px;">
							<asp:TextBox ID="UserName" runat="server" Width="150px"></asp:TextBox>
							<asp:RequiredFieldValidator ID="vldUserName" runat="server" ControlToValidate="UserName"
								SetFocusOnError="true"></asp:RequiredFieldValidator>
						</div>
						<div class="fleft r w30 tpad10" style="line-height: 22px;">
							<cc:FormLabel runat="server" ID="FormLabel1" CssClass="nowrap b" Text="Password"></cc:FormLabel>
						</div>
						<div class="fleft l w60 lpad5 tpad10" style="line-height: 22px;">
							<asp:TextBox ID="Password" runat="server" TextMode="Password" Width="150px"></asp:TextBox>
							<asp:RequiredFieldValidator ID="vldPass" runat="server" ControlToValidate="Password"
								SetFocusOnError="true"></asp:RequiredFieldValidator>
						</div>
						<div class="fclear w100 c tpad15 error_message">
							<asp:Label ID="lblError" runat="server" />
						</div>
						<div class="fclear fleft w30">&nbsp;</div>
						<div class="fleft l w60 lpad5 ">
							<asp:HyperLink ID="hlForgotPassword" runat="server" EnableViewState="false"
								NavigateUrl="~/ForgotPassword.aspx" Text="Forgot your password?"></asp:HyperLink>
						</div>
						<div class="fclear fleft w30">&nbsp;</div>
						<div class="fleft l w60 lpad5 tpad10">
							<asp:Button ID="btnLogin" CommandName="Login" runat="server" Text="Login"
								Width="100px"></asp:Button>
						</div>
						<div class="clearer">
							<div>
							</div>
				</LayoutTemplate>
			</asp:Login>
	</div>
</asp:Content>
