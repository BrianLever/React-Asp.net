<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword"
	MasterPageFile="~/FrontDeskMaster.master" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>

<%@ Register TagPrefix="uc" TagName="ChangePassword" Src="~/controls/ModifyPassword.ascx" %>

<asp:Content runat="server" ID="cnt" ContentPlaceHolderID="cphContent">
	<div id="password-recovery" class="wizard">
		<asp:UpdatePanel runat="server" ID="upnlWizard">
			<ContentTemplate>
				<asp:Wizard runat="server" ID="wizardForgotPassword" CancelButtonText="Cancel" CellPadding="0"
					CellSpacing="0" DisplaySideBar="false" Width="100%" EnableViewState="true" OnNextButtonClick="OnNextButtonClick">
					<WizardSteps>
						<asp:WizardStep ID="wstUN" runat="server" StepType="Start" Title="Enter your user name">
							<asp:Panel ID="pnl" runat="server" DefaultButton="btnUserNameContinue">
								<div class="grid2col">
									<div class="column left">
										<cc:FormLabel runat="server" ID="lblUserName" Text="User Name" Mandatory="true"></cc:FormLabel>
									</div>
									<div class="column right">
										<asp:TextBox runat="server" ID="txtUserName"></asp:TextBox>
										<asp:RequiredFieldValidator ID="vldUserName" runat="server" ControlToValidate="txtUserName"
											ToolTip="User name is required" ErrorMessage="User name is required" ValidationGroup="EnterUserName"></asp:RequiredFieldValidator>
									</div>
									<div class="column left">
										&nbsp;
									</div>
									<div class="column right">
										<asp:ValidationSummary ID="vs" runat="server" CssClass="error_summary" ValidationGroup="EnterUserName" />
										<asp:Label runat="server" ID="lblNotValidUserName" CssClass="error_message"></asp:Label>
									</div>
									<div class="column left">
									</div>
									<div class="column right">
										<asp:Button runat="server" ID="btnUserNameContinue" CommandName="MoveNext" Text="Continue"
											Width="100px" ValidationGroup="EnterUserName" UseSubmitBehavior="true" />
									</div>
								</div>
							</asp:Panel>
						</asp:WizardStep>
						<asp:WizardStep ID="wstSecQuestion" runat="server" StepType="Step" Title="Answer security question">
							<asp:Panel ID="pnl1" runat="server" DefaultButton="btnMoveToChangePwd">
								<div class="grid2col">
									<div class="column left">
										<cc:FormLabel runat="server" ID="lblQuestion1" Text="Question"></cc:FormLabel>
									</div>
									<div class="column right">
										<asp:Label ID="lblQuestion" runat="server"></asp:Label>
									</div>
									<div class="column left">
										<cc:FormLabel runat="server" ID="lblAnswer1" Text="Answer" Mandatory="true"></cc:FormLabel>
									</div>
									<div class="column right">
										<asp:TextBox ID="txtAnswer" runat="server"></asp:TextBox>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAnswer"
											ToolTip="Answer is required" Display="Dynamic" ErrorMessage="Answer is required"
											ValidationGroup="SecurityQuestion"></asp:RequiredFieldValidator>
									</div>
									<div class="column left">
										&nbsp;
									</div>
									<div class="column right">
										<asp:ValidationSummary ID="vs1" runat="server" CssClass="error_summary" ValidationGroup="SecurityQuestion" />
										<asp:Label runat="server" ID="lblNotValidSecurityQuestion" CssClass="error_message"></asp:Label>
									</div>
									<div class="column left">
									</div>
									<div class="column right">
										<asp:Button runat="server" ID="btnMoveToChangePwd" CommandName="MoveNext" Text="Continue"
											Width="100px" ValidationGroup="SecurityQuestion" UseSubmitBehavior="true" />
									</div>
								</div>
							</asp:Panel>
						</asp:WizardStep>
						<asp:WizardStep ID="wstSetPwd" runat="server" StepType="Step" Title="Setup new Password">
							<asp:Panel ID="Panel1" runat="server" DefaultButton="btnMoveToChangePwd">
								<uc:ChangePassword runat="server" ID="ucResetPassword" IsOldPasswordRequired="false"
									ValidationGroup="SetupNewPassword" OnChangeFailed="ucResetPassword_ChangeFailed"
									OnPasswordChanged="ucResetPassword_Changed" />
								<div class="grid2col">
									<div class="column left">
									</div>
									<div class="column right">
										<asp:ValidationSummary ID="vs2" runat="server" ValidationGroup="SetupNewPassword" />
										<asp:Label runat="server" ID="lblInvalidPassword" CssClass="error_message"></asp:Label>
									</div>

									<div class="column left">
									</div>
									<div class="column right">
										<asp:Button runat="server" ID="btnFinish" OnClick="OnComplete" Text="Complete" Width="100px"
											ValidationGroup="SetupNewPassword" />
									</div>
								</div>
							</asp:Panel>
						</asp:WizardStep>
					</WizardSteps>
					<HeaderTemplate>
						<div class="wizard-sidebar clearfix bmar10">
							<ul class="">
								<% =RenderHeaderStepsHeaders()%>
							</ul>

						</div>
					</HeaderTemplate>
					<HeaderStyle />
					<StartNavigationTemplate>
					</StartNavigationTemplate>
					<StepNavigationTemplate>
					</StepNavigationTemplate>
					<FinishNavigationTemplate>
					</FinishNavigationTemplate>
				</asp:Wizard>
			</ContentTemplate>
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="wizardForgotPassword" EventName="NextButtonClick" />
			</Triggers>
		</asp:UpdatePanel>
	</div>
</asp:Content>
