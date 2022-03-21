<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/Reports.master" AutoEventWireup="true"
    CodeFile="IndexReportBhsVisitOutcomes.aspx.cs" Inherits="IndexReportBhsVisitOutcomesPage" EnableEventValidation="false" %>

<%@ Register Src="~/controls/Report/BHIBhsVisitOutcomes.ascx" TagName="IndexReport" TagPrefix="uc" %>
<%@ MasterType VirtualPath="~/Reports/Reports.master" %>
<%@ Register TagPrefix="uc" TagName="GpraTimePeriod" Src="~/controls/UI/GPRAPeriodTimeRange.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReportType" Src="~/controls/UI/IndicatorReportType.ascx" %>

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
                                <uc:GpraTimePeriod runat="server" ID="dtrDateFilter" />
                            </div>
                        </li>
                    </ul>
                </li>
                <li>
                    <ul class="filter-box line">
                        <li>
                            <cc:FormLabel runat="server" Text="Report Type"></cc:FormLabel>
                        </li>
                        <li>
                             <uc:ReportType runat="server" ID="ucReportType" />
                        </li>
                    </ul>
                </li>
            </ul>
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

    <div>
        <asp:UpdatePanel runat="server" ID="upd" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <uc:IndexReport runat="server" ID="ucReport" />

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">



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



    </script>



</asp:Content>
