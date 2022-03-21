<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MatchedPatientList.ascx.cs"
	Inherits="MatchedPatientList" %>
<asp:UpdatePanel runat="server" ID="updRoot" ChildrenAsTriggers="true" UpdateMode="Conditional">
	<ContentTemplate>
		<asp:Repeater ID="rptMatchedPatient" runat="server" OnItemDataBound="rptMatchedPatient_Bound">
			<HeaderTemplate>
				<div>
			</HeaderTemplate>
			<ItemTemplate>
				<div class="EHRItem <%# Container.ItemIndex % 2 == 0 ? "" : "alt" %>">
					<div class="fleft tpad3" style="width: 40px">
						&nbsp;
                        <asp:Image ID="imgApplyIcon" runat="server" ImageUrl="~/images/check.png" Visible='<%# Convert.ToInt32(Eval("ID")) == SelectedItemID %>' />
					</div>
					<div class="fleft">
						<div>
							HRN: <%# Eval("EHR")%>
                            <asp:HiddenField runat="server" ClientIDMode="Predictable" ID="hdnItemData" />
						</div>
						<div>
							<div class="fleft b">
								<asp:Label ID="txtFirstName" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
							</div>
							<div class="fleft lpad5 b">
								<asp:Label ID="txtMiddleName" runat="server" Text='<%# Eval("MiddleName") %>'></asp:Label>
							</div>
							<div class="fleft lpad5 b">
								<asp:Label ID="txtLastName" runat="server" Text='<%# Eval("LastName") %>'></asp:Label>
							</div>
						</div>
						<div class="fclear lpad20">
							<asp:Label ID="txtBirthday" runat="server" Text='<%# string.Format("{0:MM/dd/yyyy}", Eval("DateOfBirth"))%>'></asp:Label>
						</div>
						<div class="lpad20">
							<asp:Label ID="txtPhone" runat="server" Text='<%# Eval("PhoneHome")%>'></asp:Label>
						</div>
						<div class="lpad20">
							<asp:Label ID="txtStreet" runat="server" Text='<%# Eval("StreetAddress")%>'></asp:Label>
						</div>
						<div class="lpad20">
							<div class="fleft">
								<asp:Label ID="txtCity" runat="server" Text='<%# Eval("City")%>'></asp:Label>,
							</div>
							<div class="fleft lpad5">
								<asp:Label runat="server" ID="txtState" Text='<%# Eval("StateID")%>'></asp:Label>,
							</div>
							<div class="fleft lpad5">
								<asp:Label runat="server" ID="txtZipCode" Text='<%# Eval("ZipCode") %>'></asp:Label>
							</div>
						</div>
						<div class="fclear" style="height: 0px">
						</div>
						<div class="tmar10 bpad10">
							<asp:Button runat="server" ID="btnSelect" ClientIDMode="Predictable" Text='<%# SelectItemButtonText %>' UseSubmitBehavior="false"
								CausesValidation="false" CommandArgument='<%# string.Format("{0}|{1}|{2}|{3}", Eval("ID"), Eval("LastName"), Eval("FirstName"),Eval("MiddleName")) %>' OnClick="btnSelect_click" Width="150px" />
						</div>


					</div>

				</div>
			</ItemTemplate>
			<FooterTemplate>
				</div>
			</FooterTemplate>
		</asp:Repeater>
		<asp:PlaceHolder ID="phPaging" runat="server">
			<div class="fleft pad10 ">
				<cc:Pager runat="server" ID="ctrlPaging" PageSize="5" OnNavigate="ccPaging_Navigate" />
			</div>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="phNoMatches" runat="server" Visible="false">
			<div class="nomatch">
				<asp:Literal runat="server" Text="<%$ Resources: TextMessages, NoMatchesForPatient %>" />
			</div>
		</asp:PlaceHolder>
	</ContentTemplate>
	<Triggers>
		<asp:AsyncPostBackTrigger ControlID="ctrlPaging" EventName="Navigate" />
	</Triggers>
</asp:UpdatePanel>
