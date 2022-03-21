<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ExportParametersAtGlance.ascx.cs"
    Inherits="ExportParametersAtGlance" %>
<div class="export-parameter-glance clearfix">
    <h2>ScreenDox patient record:</h2>
    <ul class="frontdesk-patient-info">
        <li>
            Record No.: <asp:Label ID="lblScreeningId" runat="server" />
        </li>
        <li>
            Screened on: <asp:Label ID="lblScreeningDate" runat="server" />
        </li>
        <li class="fd-name">
            <asp:Label ID="lblFdName" runat="server" />
        </li>
        <li class="fd-bd">
            <asp:Label ID="lblFdBirthday" runat="server" />
        </li>
    </ul>
    <h2>EHR patient record:</h2>
    <ul class="ehr-patient-info">
        <li>
            HRN: <asp:Label ID="lblEhrHRN" runat="server" />
        </li>
        <li>
            <asp:Label ID="lblEhrName" runat="server" />
        </li>
        <li>
            <asp:Label ID="lblEhrBirthday" runat="server" />
        </li>
    </ul>
    <h2>Visit:</h2>
    <ul class="inline">
        <li>
            Visit: <asp:Label ID="lblVisitDate" runat="server" />
        </li>
        <li>
            Category: <asp:Label ID="lblVisitCategory" runat="server" />
        </li>
        
        <li>
            Location: <asp:Label ID="lblVisitLocation" runat="server" />
        </li>
    </ul>
</div>
