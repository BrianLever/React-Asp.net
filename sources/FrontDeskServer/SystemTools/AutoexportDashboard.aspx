<%@ Page Title="" Language="C#" MasterPageFile="~/SystemTools/SystemTools.master"
    AutoEventWireup="true" CodeFile="AutoexportDashboard.aspx.cs" Inherits="SystemTools_AutoexportDashboard" %>

<%@ Register Src="~/controls/SystemStatusSummary.ascx" TagName="SystemSummary" TagPrefix="uc" %>
<%@ MasterType VirtualPath="~/SystemTools/SystemTools.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSystemTools" runat="Server">
    <div class="content-right-margin">
        <div class="inline_filter w100 c">
            <ul class="line">
                <li>
                    <cc:FormLabel runat="server" Text="Export Start date"></cc:FormLabel>
                </li>
                <li>
                    <cc:RichDatePicker ID="dtStartDate" runat="server" />
                    <asp:CompareValidator runat="server" ID="vldStartDateType" ControlToValidate="dtStartDate"
                        Display="Static" Operator="DataTypeCheck" Type="Date" EnableClientScript="true"
                        Text="!" ErrorMessage="Start date field has invalid date value" />
                </li>
                <li>
                    <cc:FormLabel ID="FormLabel1" runat="server" Text="Export End date"></cc:FormLabel>
                </li>
                <li>
                    <cc:RichDatePicker ID="dtEndDate" runat="server" />
                    <asp:CompareValidator runat="server" ID="vldEndDateType" ControlToValidate="dtEndDate"
                        Display="Static" Operator="DataTypeCheck" Type="Date" EnableClientScript="true"
                        Text="!" ErrorMessage="Export End date field has invalid date value" />
                </li>
                <li>
                    <cc:FormLabel runat="server" Text="Name"></cc:FormLabel>
                </li>
                <li>
                    <asp:TextBox ID="txtNameFilter" runat="server" MaxLength="128" />
                </li>
                <li class="buttons">
                    <asp:Button ID="btnSearch" runat="server" UseSubmitBehavior="true" Text="Search"
                        EnableViewState="false" OnClick="Search_Click" CausesValidation="false" />
                    <asp:Button ID="btnClear" runat="server" UseSubmitBehavior="false" Text="Clear" EnableViewState="false"
                        OnClick="Clear_Click" CausesValidation="false" />
                </li>
            </ul>
        </div>

        <div class="tmar10 tpad10">
            <div id="system_summary">
                <h2>Auto-Export Statistics</h2>
                <asp:UpdatePanel runat="server" ID="updItems" UpdateMode="Always">
                    <ContentTemplate>
                        <ul>
                            <li>
                                <cc:FormLabel Text="Succeed exports" runat="server" />
                                <asp:Label runat="server" ID="lblSucceedCount" />
                            </li>
                            <li>
                                <cc:FormLabel Text="Failed exports" runat="server" />
                                <asp:Label runat="server" ID="lblFailedCount" />
                            </li>
                            <li>
                                <cc:FormLabel Text="Total exports" runat="server" />
                                <asp:Label runat="server" ID="lblTotal" />
                            </li>
                        </ul>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="clearer">
                </div>
            </div>
        </div>

        <div class="w100 c tmar10">
            <div id="system_summary">
                <h2 class="l">Patient Name Auto-Correction Log</h2>
                <asp:UpdatePanel runat="server" ID="updView" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                            PageSize="50" DataSourceID="odsItems">
                            <EmptyDataTemplate>
                                <div class="c w100">
                                    Log is empty
                                </div>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:BoundField HeaderText="Date" DataField="CreatedDate"
                                    HeaderStyle-CssClass="w15 l" ItemStyle-CssClass="l" DataFormatString="{0:MM/dd/yyyy HH:mm zzz}"
                                    HtmlEncode="false" />
                                <asp:BoundField HeaderText="Name (EHR)" DataField="CorrectedPatientName"
                                    HeaderStyle-CssClass="w15 l" ItemStyle-CssClass="l" SortExpression="CorrectedPatientName" />
                                <asp:BoundField HeaderText="Birthday (EHR)" DataField="CorrectedBirthday"
                                    HeaderStyle-CssClass="w10 l" ItemStyle-CssClass="l" DataFormatString="{0:MM/dd/yyyy}"
                                    HtmlEncode="false" />
                                <asp:BoundField HeaderText="Initial Name" DataField="OriginalPatientName"
                                    HeaderStyle-CssClass="w20 l" ItemStyle-CssClass="l" />
                                <asp:BoundField HeaderText="Initial Birthday" DataField="OriginalBirthday"
                                    HeaderStyle-CssClass="w10 l" ItemStyle-CssClass="l" DataFormatString="{0:MM/dd/yyyy}"
                                    HtmlEncode="false" />
                                <asp:BoundField HeaderText="Comments" DataField="Comments"
                                    HeaderStyle-CssClass="w30 l" ItemStyle-CssClass="l" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>
    <asp:ObjectDataSource ID="odsLicences" runat="server" TypeName="FrontDesk.Server.Licensing.Services.LicenseService"
        SelectMethod="GetAllProductLicensesForDisplay"></asp:ObjectDataSource>

    <asp:ObjectDataSource ID="odsItems" runat="server" TypeName="ScreenDox.EHR.Common.PatientNameCorrectionLogService"
        SelectMethod="Get"
        SelectCountMethod="GetCount"
        EnablePaging="true"
        EnableCaching="false"
        SortParameterName="orderBy">
        <SelectParameters>
            <asp:ControlParameter ControlID="dtStartDate" Name="startDate" DbType="DateTimeOffset"
                ConvertEmptyStringToNull="true" />
            <asp:ControlParameter ControlID="dtEndDate" Name="endDate" DbType="DateTimeOffset"
                ConvertEmptyStringToNull="true" />
            <asp:ControlParameter ControlID="txtNameFilter" Name="nameFilter" DbType="String"
                ConvertEmptyStringToNull="true" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
