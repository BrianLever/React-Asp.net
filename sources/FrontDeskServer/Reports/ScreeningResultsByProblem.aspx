<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/Reports.master" AutoEventWireup="true"
    CodeFile="ScreeningResultsByProblem.aspx.cs" Inherits="ScreeningResultsByProblemAgePage" EnableEventValidation="false" %>

<%@ Register Src="~/controls/Report/BHIReportByAge.ascx" TagName="IndexReport" TagPrefix="uc" %>
<%@ MasterType VirtualPath="~/Reports/Reports.master" %>
<%@ Register TagPrefix="uc" TagName="GpraTimePeriod" Src="~/controls/UI/GPRAPeriodTimeRange.ascx" %>
<%@ Register TagPrefix="uc" TagName="ProblemScoreFilter" Src="~/controls/UI/ProblemScoreFilter.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphTop" runat="Server">
    <div>
        <div class="inline_filter bhi">
            <ul class="line">
                <li>
                    <ul class="filter-box line">
                        <li>
                            <cc:FormLabel runat="server" Text="Location"></cc:FormLabel>
                        </li>
                        <li>
                            <asp:DropDownList ID="ddlLocations" runat="server" DataSourceID="odsLocations" DataTextField="Name"
                                DataValueField="BranchLocationID" EnableViewState="false" AppendDataBoundItems="true"
                                AutoPostBack="False">
                                <asp:ListItem Text="<%$ Resources: TextMessages, DropDown_AllText %>" Value="" />
                            </asp:DropDownList>
                            <div>
                                <asp:Label ID="lblMyLocation" runat="server" />
                            </div>
                        </li>
                    </ul>
                </li>
                <li>
                    <ul class="filter-box line">
                        <li>
                            <cc:FormLabel runat="server" ID="lblPeriod" Text="Report Period"></cc:FormLabel>
                        </li>
                        <li>
                            <div class="date_range">
                                <uc:GpraTimePeriod runat="server" ID="dtrDateFilter" PeriodType="Custom" />
                            </div>
                        </li>
                    </ul>
                </li>

            </ul>
            <div class="score-filter-box">
                <asp:UpdatePanel runat="server" ID="updFilter" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <Triggers>
                    </Triggers>
                    <ContentTemplate>
                        <uc:ProblemScoreFilter runat="server" ID="ucScoreFilter" />

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="buttons">
                <asp:Button ID="btnApply" runat="server" Text="Apply" UseSubmitBehavior="false" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" UseSubmitBehavior="false" OnClick="btnClear_Click" />

                <asp:Button ID="btnPrint" runat="server" Text="Print" UseSubmitBehavior="false" OnClick="btnPrint_Click" />
            </div>
        </div>
        <div class="clearer">
        </div>
    </div>


    <asp:ObjectDataSource runat="server" ID="odsLocations" TypeName="FrontDesk.Server.Screening.Services.BranchLocationService"
        SelectMethod="GetAccesibleCheckInLocationsForCurrentUser" EnableCaching="false"></asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" ID="odsrYears" TypeName="FrontDesk.Server.Screening.ScreeningResultHelper"
        SelectMethod="GetDateListForReport" EnableCaching="false"></asp:ObjectDataSource>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphSubContent" runat="Server">
    <div class="bhi score-filter-container">
        <div class="indicator">
            <h1>Behavioral Health Indicator Report <span class="sub">Screening Results by Sort</span></h1>
        </div>

        <asp:UpdatePanel runat="server" ID="upd" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <div class="check-in">
                    <cc:HierarDynamicGrid runat="server" ID="grvList" DataSourceID="odsCheckInList" AllowPaging="true"
                        AllowSorting="true" AutoGenerateColumns="false" CellPadding="0" CssClass="gridView"
                        CellSpacing="0" BorderWidth="0px" PageSize="20" TemplateDataMode="Table" EnableViewState="false">
                        <EmptyDataTemplate>
                            <div class="c">
                                No records found
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
                                            <a href='<%# ResolveClientUrl(string.Format("~/EditPatientContact.aspx?id={0}", DataBinder.Eval(Container.DataItem, "ScreeningResultID"))) %>'>
                                                <asp:Image runat="server" ID="imgEdit" ImageUrl="~/images/page_edit.png" CssClass="datagrid document icon" AlternateText="Edit"></asp:Image>
                                            </a>
                                        </td>
                                        <td class="w65 l">
                                            <asp:HyperLink runat="server" Target="_blank" NavigateUrl='<%# string.Format("~/PatientCheckIn.aspx?id={0}", DataBinder.Eval(Container.DataItem, "ScreeningResultID")) %>'
                                                Text='<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "ScreeningName")) + " Report") %>'
                                                EnableViewState="false" CssClass='<%#DataBinder.Eval(Container.DataItem, "IsPositiveCssClass") %>'></asp:HyperLink>
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
        <asp:ObjectDataSource runat="server" ID="odsCheckInList" TypeName="FrontDesk.Server.Screening.ScreeningResultHelper"
            SelectMethod="GetUniquePatientCheckInsByProblem" EnablePaging="true" SelectCountMethod="GetUniquePatientCheckInsByProblemCount"
            SortParameterName="orderBy">
            <SelectParameters>
                <asp:Parameter Name="filter" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <div id="auto-refresh-button-container" class="buttons r">
            <asp:HyperLink ID="btnDisableAutoRefresh" runat="server" Text=""></asp:HyperLink>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            var btnPrintID = "<%=btnPrint.ClientID%>";

            function OnBeginRequest(sender, args) {
                var btnPrint = document.getElementById(btnPrintID);
                if (btnPrint != null) {
                    btnPrint.disabled = true;
                }
            }

            function OnEndRequest(sender, args) {
                var btnPrint = document.getElementById(btnPrintID);
                if (btnPrint != null) {
                    btnPrint.disabled = false;
                }
            }

            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(OnBeginRequest);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(OnEndRequest);



            $("#<%= upd.ClientID %>").autoRefreshUpdatePanel({
                triggerButtonId: "<%= btnApply.ClientID%>",
                onGridContentChangedEvent: "<%= HierarDynamicGrid.ChildItemLoadedJSEventName %>",
                turnOffButtonId: "<%= btnDisableAutoRefresh.ClientID %>",
                turnOffValueId: "<%= hdnDisableAutoRefresh.ClientID %>"
            });


        });


    </script>



</asp:Content>
