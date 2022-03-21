<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/Reports.master" AutoEventWireup="true"
    CodeFile="ExportToExcel.aspx.cs" Inherits="ExportToExcelPage" EnableEventValidation="false" %>

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
                <asp:Button ID="btnPrint" runat="server" Text="Export" UseSubmitBehavior="false" OnClick="btnPrint_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" UseSubmitBehavior="false" OnClick="btnClear_Click" />
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

        <div class="w70">
            <div class="grid2col">
                <div class="column left">
                    <cc:FormLabel runat="server" ID="lblScreening" Text="Screening Results" AssociatedControdivD="chkScreening"></cc:FormLabel>
                </div>
                <div class="column right">
                    <asp:CheckBox runat="server" ID="chkScreening"></asp:CheckBox>
                </div>
            </div>
            <div class="grid2col">
                <div class="column left">
                    <cc:FormLabel runat="server" ID="lblDemographics" Text="Demographics Report" AssociatedControdivD="chkDemographics"></cc:FormLabel>
                </div>
                <div class="column right">
                    <asp:CheckBox runat="server" ID="chkDemographics"></asp:CheckBox>
                </div>
            </div>
            <div class="grid2col">
                <div class="column left">
                    <cc:FormLabel runat="server" ID="lblVisit" Text="Visit Report" AssociatedControdivD="chkVisit"></cc:FormLabel>
                </div>
                <div class="column right">
                    <asp:CheckBox runat="server" ID="chkVisit"></asp:CheckBox>
                </div>
            </div>
            <div class="grid2col">
                <div class="column left">
                    <cc:FormLabel runat="server" ID="lbl" Text="Follow-Up Report" AssociatedControdivD="chkFollowUp"></cc:FormLabel>
                </div>
                <div class="column right">
                    <asp:CheckBox runat="server" ID="chkFollowUp"></asp:CheckBox>
                </div>
            </div>
             <div class="grid2col">
                <div class="column left">
                    <cc:FormLabel runat="server" ID="lblDrugs" Text="Drug Use Results" AssociatedControlID="chkDrugsOfChoice"></cc:FormLabel>
                </div>
                <div class="column right">
                    <asp:CheckBox runat="server" ID="chkDrugsOfChoice"></asp:CheckBox>
                </div>
            </div>
             <div class="grid2col">
                <div class="column left">
                    <cc:FormLabel runat="server" ID="lblCombined" Text="Combined Report (Screening, Drug Use, and Demographics)" AssociatedControlID="chkCombined"></cc:FormLabel>
                </div>
                <div class="column right">
                    <asp:CheckBox runat="server" ID="chkCombined"></asp:CheckBox>
                </div>
            </div>
            
            <div class="grid2col c">
                <div class="column joined tmar10 bmar10">
                    Check each box for data to be exported to Excel.
                    <br />
                    Select the location and report period. Click the “Export” button.
                </div>
            </div>
            <div class="grid2col">
                <div class="column left l">
                </div>
                <div class="column right">
                </div>
            </div>
        </div>


    </div>

    <script type="text/javascript">
    </script>



</asp:Content>
