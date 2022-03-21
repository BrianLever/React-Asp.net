<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BHIDrugOfChoiceReportByAge.ascx.cs" Inherits="FrontDesk.Server.Web.Controls.BHIDrugOfChoiceReportByAgeControl" %>

<%@ Register Src="~/controls/Report/BHIReportSectionByAge.ascx" TagName="IndexReportSection" TagPrefix="uc" %>

<div class="bhsreport indicator">
    <h1>
        Behavioral Health Indicator Report <span class="sub">Drug Use Results by Age</span></h1>
    <div class="group noborder section">
        <div class="header">
            <table>
                <tbody>
                    <tr>
                        <td class="c0">
                            Created Date:
                        </td>
                        <td class="c1">
                            <asp:Literal runat="server" ID="ltrCreatedDate" />
                        </td>
                    </tr>
                    <tr>
                        <td class="c0">
                            Report Period:
                        </td>
                        <td class="c1">
                            <asp:Literal runat="server" ID="ltrReportPeriod" />
                        </td>
                    </tr>
                    <tr>
                        <td class="c0">
                            Branch Locations:
                        </td>
                        <td class="c1">
                            <asp:Literal runat="server" ID="ltrBranchLocation" />
                        </td>
                    </tr>
                    <tr>
                        <td class="c0">
                            Report Type:
                        </td>
                        <td class="c1">
                            <asp:Literal runat="server" ID="ltrReportType" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
  
    <uc:IndexReportSection runat="server" ID="ctrlDrugOfChoiceSectionPrimary" ApplyPaddingForItemsStartingFromLine="0" />
    <uc:IndexReportSection runat="server" ID="ctrlDrugOfChoiceSectionSecondary" ApplyPaddingForItemsStartingFromLine="0" />
    <uc:IndexReportSection runat="server" ID="ctrlDrugOfChoiceSectionTertiary" ApplyPaddingForItemsStartingFromLine="0" />

    <div class="footer">
        <div class="logo">
            </div>
        <div class="version">
            V.3.10</div>
    </div>
</div>
