<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BHSReport.ascx.cs" Inherits="FrontDesk.Server.Web.Controls.BHSReportControl"
    EnableViewState="false" %>
<%@ Register Src="~/controls/Report/BHSReportTobacoAnswers.ascx" TagName="Tobacco"
    TagPrefix="uc" %>
<%@ Register Src="~/controls/Report/BHSReportViolenceAnswers.ascx" TagName="Violence"
    TagPrefix="uc" %>
<%@ Register Src="~/controls/Report/BHSReportAlcoholAnswers.ascx" TagName="Alcohol"
    TagPrefix="uc" %>
<%@ Register Src="~/controls/Report/BHSReportDepressionAnswers.ascx" TagName="Depression"
    TagPrefix="uc" %>
<%@ Register Src="~/controls/Report/BHSReportSubstanceAbuseAnswers.ascx" TagName="Drugs"
    TagPrefix="uc" %>
<%@ Register Src="~/controls/Report/BHSReportDrugOfChoiceAnswers.ascx" TagName="DrugsOfChoice"
    TagPrefix="uc" %>
<%@ Register Src="~/controls/Report/BHSReportAnxietyAnswers.ascx" TagName="Anxiety"
    TagPrefix="uc" %>
<%@ Register Src="~/controls/Report/BHSReportProblemGamblingAnswers.ascx" TagName="Gambling"
    TagPrefix="uc" %>

<div class="bhsreport">
    <h1>Behavioral Health Screening Report</h1>
    <div class="group noborder bhs">
        <table class="patient">
            <tr class="head">
                <td class="col1">Patient Last Name
                </td>
                <td class="col2">First Name
                </td>
                <td class="col3">Middle Name
                </td>
                <td class="col4">Date of Birth
                </td>
                <td class="col5">Record Number
                </td>
            </tr>
            <tr class="body">
                <td class="col1">
                    <asp:Literal runat="server" ID="lblLastname" />
                </td>
                <td class="col2">
                    <asp:Literal runat="server" ID="lblFirstname" />
                </td>
                <td class="col3">
                    <asp:Literal runat="server" ID="lblMiddlename" />
                </td>
                <td class="col4">
                    <asp:Literal runat="server" ID="lblBirthday" />
                </td>
                <td class="col5">
                    <asp:Literal runat="server" ID="lblRecordNo" />
                </td>
            </tr>
            <tr class="head">
                <td class="col1">Mailing Address
                </td>
                <td class="col2">City
                </td>
                <td class="col3">State
                </td>
                <td class="col4">ZIP Code
                </td>
                <td class="col5">Primary Phone Number
                </td>
            </tr>
            <tr class="body">
                <td class="col1">
                    <asp:Literal runat="server" ID="lblStreetAddress" />
                </td>
                <td class="col2">
                    <asp:Literal runat="server" ID="lblCity" />
                </td>
                <td class="col3">
                    <asp:Literal runat="server" ID="lblState" />
                </td>
                <td class="col4">
                    <asp:Literal runat="server" ID="lblZipCode" />
                </td>
                <td class="col5">
                    <asp:Literal runat="server" ID="lblPhone" />
                </td>
            </tr>
        </table>
    </div>
    <asp:PlaceHolder runat="server" ID="phSecuritySectrion">


        <uc:Tobacco runat="server" ID="ucTobacco" />
        <uc:Alcohol runat="server" ID="ucAlcohol" />
        <uc:Drugs runat="server" ID="ucDrugs" />
        <uc:DrugsOfChoice runat="server" ID="ucDrugsOfChoice" />

        <uc:Anxiety runat="server" ID="ucAnxiety" />

        <uc:Depression runat="server" ID="ucDepression" />
        <uc:Violence runat="server" ID="ucViolence" />
        <uc:Gambling runat="server" ID="ucGambling" />
    </asp:PlaceHolder>

    <div class="footer">
        <div class="logo">
        </div>
        <div class="date">
            <asp:Label runat="server" ID="lblCreateDate"></asp:Label>
        </div>
        <div class="version">
            V.3.10
        </div>
    </div>
</div>
