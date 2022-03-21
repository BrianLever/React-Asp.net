<%@ Page Title="" Language="C#" MasterPageFile="~/systemtools/ScreeningProfile.master"
	AutoEventWireup="true" CodeFile="ScreenProfileFrequencySettings.aspx.cs" Inherits="ScreenProfileFrequencySettingsForm" %>

<%@ MasterType VirtualPath="~/systemtools/ScreeningProfile.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSubContent" runat="Server">
	<div class="content-right-margin">
		<div class="validation_error">
			<asp:ValidationSummary ID="vldSummary" runat="server" ValidationGroup="form" EnableClientScript="true" />
		</div>
		<div id="screeningFrequency" class="c tmar10">
			<asp:Repeater runat="server" ID="rptAgeParams">
				<HeaderTemplate>
					<table class="gridView screeningFrequency">
						<thead>
							<tr>
								<th class="c1">Measure Tool
								</th>
								<th class="c2">Frequency per GPRA Period
								</th>
							</tr>
						</thead>
						<tbody>
				</HeaderTemplate>
				<FooterTemplate>
					</tbody></table>
				</FooterTemplate>
				<ItemTemplate>
					<tr>
						<td class="c1">
							<%# (Server.HtmlEncode(Convert.ToString(Eval("Name")))) %>
							<asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ID") %>' />
						</td>
						<td class="c2">
							<asp:DropDownList runat="server" ID="cmbFrequency" ClientIDMode="Static" />
						</td>
					</tr>
				</ItemTemplate>
				<AlternatingItemTemplate>
					<tr class="alt">
						<td class="c1">
							<%# (Server.HtmlEncode(Convert.ToString(Eval("Name")))) %>
							<asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ID") %>' />
						</td>
						<td class="c2">
							<asp:DropDownList runat="server" ID="cmbFrequency" ClientIDMode="Static" />
						</td>
					</tr>
				</AlternatingItemTemplate>
			</asp:Repeater>
		</div>
		<div class="l note tmar5">
			<asp:Localize ID="ltrNote" runat="server" Text="<%$ Resources: TextMessages, ScreeningFrequency_Notes %>"></asp:Localize>
		</div>
		<div class="c tmar10">
			<div class="l">
				<asp:Button runat="server" ID="btnSave" CausesValidation="true" Text="Save changes"
					UseSubmitBehavior="false" OnClientClick="if(!confirm('You are about to update screening frequency settings for measures. Changes will be applied to all connected kiosks. Proceed?')){return false;}" ValidationGroup="form" />
				<asp:Button runat="server" ID="btnReset" CausesValidation="false" Text="Reset" UseSubmitBehavior="false" />
			</div>
		</div>
	</div>

</asp:Content>
