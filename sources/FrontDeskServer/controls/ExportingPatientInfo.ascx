<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ExportingPatientInfo.ascx.cs"
    Inherits="ExportingPatientInfo" %>
<div id="patient-export-preview">
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
        <li>
            <asp:Label ID="lblEhrPhoneHome" runat="server" />
        </li>
        <li>
            <asp:Label ID="lblEhrPhoneOffice" runat="server" />
        </li>
        <li>
            <asp:Label ID="lblEhrStreetAddress" runat="server" />
        </li>
        <li>
            <asp:Label ID="lblEhrCity" runat="server" />,&nbsp;<asp:Label ID="lblEhrStateID"
                runat="server" />,&nbsp;<asp:Label ID="lblEhrZipCode" runat="server" />
        </li>
    </ul>
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
        <li class="fd-phone">
            <asp:Label ID="lblFdPhone" runat="server" />
        </li>
        <li class="fd-address">
            <asp:Label ID="lblFdAddress" runat="server" />
        </li>
        <li class="fd-city">
            <asp:Label ID="lblFdCity" runat="server" />,&nbsp;<asp:Label ID="lblFdStateID"
                runat="server" />,&nbsp;<asp:Label ID="lblFdZipCode" runat="server" />
        </li>
    </ul>
</div>
