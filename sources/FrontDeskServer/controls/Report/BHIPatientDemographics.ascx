<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BHIPatientDemographics.ascx.cs" Inherits="FrontDesk.Server.Web.Controls.BHIPatientDemographics" %>

<%@ Register Src="~/controls/Report/BHIDemographicsByAge.ascx" TagName="IndexReportSection" TagPrefix="uc" %>

<div class="bhsreport indicator">
    <h1>Patient Demographics Report <span class="sub">Visits</span></h1>
    <div class="group noborder section">
        <div class="header">
            <table>
                <tbody>
                    <tr>
                        <td class="c0">Created Date:
                        </td>
                        <td class="c1">
                            <asp:Literal runat="server" ID="ltrCreatedDate" />
                        </td>
                    </tr>
                    <tr>
                        <td class="c0">Report Period:
                        </td>
                        <td class="c1">
                            <asp:Literal runat="server" ID="ltrReportPeriod" />
                        </td>
                    </tr>
                    <tr>
                        <td class="c0">Branch Locations:
                        </td>
                        <td class="c1">
                            <asp:Literal runat="server" ID="ltrBranchLocation" />
                        </td>
                    </tr>
                    <tr>
                        <td class="c0">Report Type:
                        </td>
                        <td class="c1">
                            <asp:Literal runat="server" ID="ltrReportType" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <uc:IndexReportSection runat="server" ID="ctrlRaceSection" />
    <uc:IndexReportSection runat="server" ID="ctrlGenderSection" />
    <uc:IndexReportSection runat="server" ID="ctrlSexualOrientationSection" />
    <uc:IndexReportSection runat="server" ID="ctrlMaritalStatus" />
    <uc:IndexReportSection runat="server" ID="ctrlEducationLevel" />
    <uc:IndexReportSection runat="server" ID="ctrlLeavingOnOrOffReservation" />
    <uc:IndexReportSection runat="server" ID="ctrlMilitaryExperience" />

    
</div>
