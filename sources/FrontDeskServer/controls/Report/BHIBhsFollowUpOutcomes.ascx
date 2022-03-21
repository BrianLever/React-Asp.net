<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BHIBhsFollowUpOutcomes.ascx.cs" Inherits="FrontDesk.Server.Web.Controls.BHIBhsFollowUpOutcomesControl" %>

<%@ Register Src="~/controls/Report/BHIDemographicsByAge.ascx" TagName="IndexReportSection" TagPrefix="uc" %>

<div class="bhsreport indicator">
    <h1>Follow-Up Outcomes</h1>
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

    <uc:IndexReportSection runat="server" ID="ctrlPatientAttendedVisit" />
    <uc:IndexReportSection runat="server" ID="ctrlFollowUpContactOutcome" />
    <uc:IndexReportSection runat="server" ID="ctrlNewVisitReferralRecommendation" />
    <uc:IndexReportSection runat="server" ID="ctrlNewVisitReferralRecommendationAccepted" />
    <uc:IndexReportSection runat="server" ID="ctrlReasonNewVisitReferralRecommendationNotAccepted" />
    <uc:IndexReportSection runat="server" ID="ctrlDischarged" />
    <uc:IndexReportSection runat="server" ID="ctrlThirtyDatyFollowUpFlag" />



</div>
