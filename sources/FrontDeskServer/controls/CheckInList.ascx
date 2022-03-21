<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CheckInList.ascx.cs" Inherits="controls_CheckInList"
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
            <cc:FormLabel runat="server" ID="lblDateRange" Text="Created date"></cc:FormLabel>
            <div class="date_range">
                <uc:GpraTimePeriod runat="server" ID="dtrDateFilter" />
            </div>
        </li>
        <li class="buttons">
            <asp:Button runat="server" ID="btnSearch" UseSubmitBehavior="false" Text="Search"
                OnClick="btnSearch_Click" />
            <asp:Button runat="server" ID="btnClear" UseSubmitBehavior="false" Text="Clear" OnClick="btnClear_Click" />

        </li>
    </ul>
</div>
<asp:UpdatePanel runat="server" ID="upd" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <div class="check-in">
            <cc:HierarDynamicGrid runat="server" ID="grvList" DataSourceID="odsCheckInList" AllowPaging="true"
                AllowSorting="true" AutoGenerateColumns="false" CellPadding="0" CssClass="gridView"
                CellSpacing="0" BorderWidth="0px" PageSize="20" TemplateDataMode="Table">
                <EmptyDataTemplate>
                    <div class="c">
                        No Patient Check-In records found
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
                        <HeaderStyle CssClass="l w58" />
                        <ItemStyle CssClass="l pname" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Date of Birth" DataField="Birthday" SortExpression="Birthday"
                        DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false">
                        <HeaderStyle CssClass="w10 l" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Export Date" SortExpression="ExportDate">
                        <ItemTemplate>
                        </ItemTemplate>
                        <HeaderStyle CssClass="w15 c" />
                        <ItemStyle CssClass="c" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Check-in Time" SortExpression="LastCheckinDate">
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
                                    <a href='<%# string.Format("EditPatientContact.aspx?id={0}", DataBinder.Eval(Container.DataItem, "ScreeningResultID")) %>'>
                                        <asp:Image runat="server" ID="imgEdit" ImageUrl="~/images/page_edit.png" CssClass="datagrid document icon" AlternateText="Edit"></asp:Image>
                                    </a>
                                </td>
                                <td class="l">
                                    <asp:HyperLink runat="server" NavigateUrl='<%# string.Format("~/PatientCheckIn.aspx?id={0}", DataBinder.Eval(Container.DataItem, "ScreeningResultID")) %>'
                                        Text='<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "ScreeningName")) + " Report") %>'
                                        EnableViewState="false" CssClass='<%#DataBinder.Eval(Container.DataItem, "IsPositiveCssClass") %>'></asp:HyperLink>
                                </td>
                                <td class="c w15">
                                    <asp:PlaceHolder runat="server" ID="phExportDate"></asp:PlaceHolder>
                                </td>
                                <td class="r w15">
                                    <asp:Literal runat="server" ID="ltrDate" Text='<%# DataBinder.Eval(Container.DataItem, "CreatedDateLabel")%>' />
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

<asp:ObjectDataSource runat="server" ID="odsCheckInList" TypeName="FrontDesk.Server.Screening.ScreeningResultHelper"
    SelectMethod="GetUniquePatientCheckIns" EnablePaging="true" SelectCountMethod="GetUniquePatientCheckInsCount"
    SortParameterName="orderBy">
    <SelectParameters>
        <asp:Parameter Name="firstNameFilter" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="lastNameFilter" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="screeningResultIdFilter" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="locationFilter" Type="Int32" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="startDateFilter" Type="DateTime" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="endDateFilter" Type="DateTime" ConvertEmptyStringToNull="true" />
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
