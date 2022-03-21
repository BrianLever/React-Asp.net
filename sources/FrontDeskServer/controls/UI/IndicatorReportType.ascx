<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IndicatorReportType.ascx.cs"
    Inherits="UI.IndicatorReportTypeControl" %>
<ul>
    <li class="label">
        <asp:RadioButton runat="server" ID="rbtUniquePatients" GroupName="reporttype" Checked="true" Text="Unique Patients" />

    </li>
    <li class="label">
        <asp:RadioButton runat="server" ID="rbtTotalPatients" GroupName="reporttype" Text="Total Reports" />
    </li>
</ul>
