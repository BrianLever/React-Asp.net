<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ScreenTimeLog.ascx.cs" Inherits="FrontDesk.Server.Web.Controls.ScreenTimeLogControl" %>

<%@ Register Src="~/controls/Report/ScreenTimeLogReportSection.ascx" TagName="IndexReportSection" TagPrefix="uc" %>

<div class="bhsreport indicator">
    <h1>
        Behavioral Health Indicator Report <span class="sub">Screen Time Log</span></h1>
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
                </tbody>
            </table>
        </div>
    </div>

    <uc:IndexReportSection runat="server" ID="ctrlTimeMeasures" />

</div>
