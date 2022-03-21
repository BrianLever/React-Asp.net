<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FollowUpList.ascx.cs" Inherits="controls_FollowUpList"
    EnableViewState="true" %>
<%@ Register TagPrefix="uc" TagName="GpraTimePeriod" Src="~/controls/UI/GPRAPeriodTimeRange.ascx" %>
<div class="inline_filter">
    <ul class="line2col check-in">
        <li>
            <cc:FormLabel runat="server" ID="lblFirstName" Text="First name"></cc:FormLabel>
            <asp:TextBox runat="server" ID="txFirstNameFilter" IE:Width="200px" />
        </li>
        <li>
            <cc:FormLabel runat="server" ID="lblLastName" Text="Last name"></cc:FormLabel>
            <asp:TextBox runat="server" ID="txtLastNameFilter" IE:Width="200px" />
        </li>
        <li>
            <cc:FormLabel runat="server" ID="lblScreendoxId" Text="ScreenDox ID"></cc:FormLabel>
            <asp:TextBox runat="server" ID="txtScreendoxIdFilter" IE:Width="200px" SkinID="NumbersOnly" />
        </li>
        <li>
            <cc:FormLabel runat="server" ID="FormLabel1" Text="Location"></cc:FormLabel>
            <asp:DropDownList ID="ddlLocations" runat="server" DataSourceID="odsLocations" DataTextField="Name"
                DataValueField="BranchLocationID" EnableViewState="false" AppendDataBoundItems="true"
                IE:Width="206px">
            </asp:DropDownList>
        </li>
        <li>
            <cc:FormLabel runat="server" ID="lblDateRange" Text="Follow-up date"></cc:FormLabel>
            <div class="date_range">
                <uc:GpraTimePeriod runat="server" ID="dtrDateFilter" />
            </div>
        </li>
        <li>
            <cc:FormLabel runat="server" ID="lblReportType" Text="Report type"></cc:FormLabel>
            <div class="reporttype-filter">
                <asp:RadioButton runat="server" ID="rbtAllReports" GroupName="reportType" Text="All Reports" Checked="true" />
                <asp:RadioButton runat="server" ID="rbtCompletedReports" GroupName="reportType" Text="Completed Reports" />
                <asp:RadioButton runat="server" ID="rbtIncompleteReports" GroupName="reportType" Text="Incomplete Reports" />
            </div>

        </li>
        <li class="buttons">
            <asp:Button runat="server" ID="btnSearch" UseSubmitBehavior="false" Text="Search"
                OnClick="btnSearch_Click" />
            <asp:Button runat="server" ID="btnClear" UseSubmitBehavior="false" Text="Clear" OnClick="btnClear_Click" />
            <asp:Button runat="server" ID="btnPrint" UseSubmitBehavior="false" Text="Print" OnClick="btnPrint_Click" />
        </li>
    </ul>
</div>
<asp:UpdatePanel runat="server" ID="upd" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <div class="check-in follow-up">
            <cc:HierarDynamicGrid runat="server" ID="grvList" DataSourceID="odsList" AllowPaging="true"
                AllowSorting="true" AutoGenerateColumns="false" CellPadding="0" CssClass="gridView"
                CellSpacing="0" BorderWidth="0px" PageSize="20"
                TemplateDataMode="Table">
                <EmptyDataTemplate>
                    <div class="c">
                        No Follow-Up records found
                    </div>
                </EmptyDataTemplate>
                <AlternatingRowStyle CssClass="alt" />
                <RowStyle />
                <PagerSettings PageButtonCount="10" Position="Bottom" NextPageText="Next" PreviousPageText="Prev" />
                <EditRowStyle BackColor="#bbbbbb" BorderColor="#000000" BorderStyle="Solid" BorderWidth="1px" />
                <SelectedRowStyle BackColor="#CCCCCC" />
                <Columns>
                    <cc:HierarColumn HeaderText="" DataKeyName="ScreeningResultID">
                        <ItemStyle CssClass="icon" />
                    </cc:HierarColumn>
                    <asp:TemplateField HeaderText="Patient Name" SortExpression="FullName">
                        <ItemTemplate>
                            <div>
                                <%# Eval("FullName")%>
                            </div>
                        </ItemTemplate>
                        <HeaderStyle CssClass="l" />
                        <ItemStyle CssClass="l pname" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Date of Birth" DataField="Birthday" SortExpression="Birthday"
                        DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false">
                        <HeaderStyle CssClass="w10 l" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Follow-Up Date" SortExpression="LastFollowDate">
                        <HeaderStyle CssClass="w15 r" />
                        <ItemStyle CssClass="r" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Visit Date" SortExpression="LastVisitDate">
                        <HeaderStyle CssClass="w15 r" />
                        <ItemStyle CssClass="r" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="File Completion Date" SortExpression="LastCompleteDate">
                        <HeaderStyle CssClass="w15 r" />
                        <ItemStyle CssClass="r" />
                    </asp:BoundField>
                </Columns>
                <NestedTableTemplate>
                    <asp:Repeater runat="server" ID="rpt" DataSource="<%# Container.DataSource %>" EnableViewState="false"
                        OnItemDataBound="rpt_RowDataBound">
                        <HeaderTemplate>
                            <table class="details_table">
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="w5 l">
                                    <a href='<%# DataBinder.Eval(Container.DataItem, "DetailsPageUrl") %>'>
                                        <asp:Image runat="server" ID="imgEdit" ImageUrl="~/images/page_edit.png" CssClass="datagrid document icon" AlternateText="Edit"></asp:Image>
                                    </a>
                                </td>
                                <td class="w50 l">
                                    <asp:HyperLink runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "DetailsPageUrl") %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "ReportName") %>'
                                        EnableViewState="false"></asp:HyperLink>
                                </td>

                                <td class="r w15">
                                    <asp:Literal runat="server" ID="ltrDate" Text='<%# DataBinder.Eval(Container.DataItem, "FollowUpDateLabel")%>' />
                                </td>
                                <td class="r w15">
                                    <asp:Literal runat="server" ID="ltrVisitDate" Text='<%# DataBinder.Eval(Container.DataItem, "ScheduledVisitDateLabel")%>' />
                                </td>
                                <td class="r w15">
                                    <asp:Literal runat="server" ID="ltrCompleteDate" Text='<%# DataBinder.Eval(Container.DataItem, "CompleteDateLabel")%>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody></table>
                        </FooterTemplate>
                    </asp:Repeater>
                </NestedTableTemplate>
                <PagerSettings PageButtonCount="5" LastPageText="Last" FirstPageText="First" />
                <PagerTemplate>
                    <asp:PlaceHolder runat="server" ID="phPager"></asp:PlaceHolder>
                </PagerTemplate>
            </cc:HierarDynamicGrid>
            <asp:HiddenField ID="hdnDisableAutoRefresh" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div id="auto-refresh-button-container" class="buttons r">
    <asp:HyperLink ID="btnDisableAutoRefresh" runat="server" Text=""></asp:HyperLink>
</div>

<asp:ObjectDataSource runat="server" ID="odsList" SelectMethod="GetUniqueVisitsForDataSource" EnablePaging="true"
    SelectCountMethod="GetUniqueVisitsCountForDataSource"
    SortParameterName="orderBy">
    <SelectParameters>
        <asp:Parameter Name="firstNameFilter" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="lastNameFilter" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="locationFilter" Type="Int32" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="startDateFilter" Type="DateTime" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="endDateFilter" Type="DateTime" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="reportTypeFilter" Type="Int32" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="screeningResultIdFilter" Type="String" ConvertEmptyStringToNull="true" />
    </SelectParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource runat="server" ID="odsLocations" TypeName="FrontDesk.Server.Screening.Services.BranchLocationService"
    SelectMethod="GetAccesibleCheckInLocationsForCurrentUser" EnableCaching="false"></asp:ObjectDataSource>
<script type="text/javascript">
    $(document).ready(function () {
        $("#<%= upd.ClientID %>").autoRefreshUpdatePanel({
            triggerButtonId: "<%= btnSearch.ClientID%>",
            onGridContentChangedEvent: "<%= HierarDynamicGrid.ChildItemLoadedJSEventName %>",
            turnOffButtonId: "<%= btnDisableAutoRefresh.ClientID %>",
            turnOffValueId: "<%= hdnDisableAutoRefresh.ClientID %>"
        });
    });
</script>
