<%@ Page Title="" Language="C#" MasterPageFile="~/SystemTools/SystemTools.master"
	AutoEventWireup="true" CodeFile="ErrorLogList.aspx.cs" Inherits="ErrorLogListForm"
	EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/SystemTools/SystemTools.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSystemTools" runat="Server">
	<div class="content-right-margin">
		 <div class="tmar10 l action">
            <asp:HyperLink ID="lnkLinkToCentralLogging" runat="server" Text="Link to Central Logging" Target="_blank"></asp:HyperLink>
        </div>
        <div class="validation_error">
			<asp:ValidationSummary runat="server" />
		</div>
		<div class="inline_filter w100 c">
			<ul class="line">
				<li>
					<cc:FormLabel runat="server" Text="Start date"></cc:FormLabel>
				</li>
				<li>
					<cc:RichDatePicker ID="dtStartDate" runat="server" />
					<asp:CompareValidator runat="server" ID="vldStartDateType" ControlToValidate="dtStartDate"
						Display="Static" Operator="DataTypeCheck" Type="Date" EnableClientScript="true"
						Text="!" ErrorMessage="Start date field has invalid date value" />
				</li>
				<li>
					<cc:FormLabel ID="FormLabel1" runat="server" Text="End date"></cc:FormLabel>
				</li>
				<li>
					<cc:RichDatePicker ID="dtEndDate" runat="server" />
					<asp:CompareValidator runat="server" ID="vldEndDateType" ControlToValidate="dtEndDate"
						Display="Static" Operator="DataTypeCheck" Type="Date" EnableClientScript="true"
						Text="!" ErrorMessage="End date field has invalid date value" />
				</li>
				<li class="buttons">
					<asp:Button ID="btnSearch" runat="server" UseSubmitBehavior="true" Text="Search"
						EnableViewState="false" OnClick="Search_Click" CausesValidation="false" />
					<asp:Button ID="btnClear" runat="server" UseSubmitBehavior="false" Text="Clear" EnableViewState="false"
						OnClick="Clear_Click" CausesValidation="false" />
				</li>
			</ul>
		</div>
		<div class="clearer">
		</div>
       
		<div class="w100 c tmar10">
			<asp:UpdatePanel runat="server" ID="updItems" UpdateMode="Always">
				<ContentTemplate>
					<asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="false" AllowPaging="true"
						PageSize="20" DataSourceID="odsItems">
						<EmptyDataTemplate>
							<div class="c w100">
								Error log is empty
							</div>
						</EmptyDataTemplate>
						<Columns>
							<asp:BoundField HeaderText="Created date" DataField="CreatedDate" 
                                HeaderStyle-CssClass="w25 l" ItemStyle-CssClass="l" DataFormatString="{0:MM/dd/yyyy HH:mm:ss zzz}"
								HtmlEncode="false" />
							<asp:BoundField HeaderText="Kiosk" DataField="KioskLabel" SortExpression="KioskLabel"
								HeaderStyle-CssClass="w20 l" ItemStyle-CssClass="l" />
							<asp:BoundField HeaderText="Error message" DataField="ErrorMessage"
								HeaderStyle-CssClass="w50 l" ItemStyle-CssClass="l" />
							<asp:HyperLinkField HeaderText="Details" DataNavigateUrlFormatString="ErrorLog.aspx?id={0}"
								DataNavigateUrlFields="ErrorLogID" HeaderStyle-CssClass="w5 c" ItemStyle-CssClass="c"
								Text="Details" />
						</Columns>
					</asp:GridView>
				</ContentTemplate>
				<Triggers>
					<asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
					<asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
					<asp:AsyncPostBackTrigger ControlID="btnDeleteAll" EventName="Click" />
				</Triggers>
			</asp:UpdatePanel>
		</div>
		<div class="w100 c tmar10 bmar10">
			<div class="l">
				<asp:Button runat="server" ID="btnDeleteAll" CausesValidation="false" Text="Clear error log"
					UseSubmitBehavior="false" OnClientClick="if(!confirm('You are about to delete all records from Error log. You might want to export records to the excel before deleting.')){return false;}"
					OnClick="btnDeleteAll_Click" />
				<cc:ExcelExportButton runat="server" ID="btnExport" CausesValidation="false" Text="Export all to Excel"
					FileName="Error Log" UseSubmitBehavior="false" CssClass="btn" />
			</div>
		</div>
	</div>

	<asp:ObjectDataSource ID="odsItems" runat="server" TypeName="FrontDesk.Server.Logging.ErrorLog"
		SelectMethod="Get" SelectCountMethod="GetCount" EnablePaging="true">
		<SelectParameters>
			<asp:ControlParameter ControlID="dtStartDate" Name="startDate" DbType="DateTimeOffset"
				ConvertEmptyStringToNull="true" />
			<asp:ControlParameter ControlID="dtEndDate" Name="endDate" DbType="DateTimeOffset"
				ConvertEmptyStringToNull="true" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
