<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SystemStatusSummary.ascx.cs"
    Inherits="SystemStatusSummaryCtrl" EnableViewState="true" %>
<div id="system_summary">
    <h2>
        Total System Summary</h2>
    <ul>
        <li>
            <cc:FormLabel Text="Check-In record count" runat="server" />
            <asp:Label runat="server" ID="lblCheckInCount" />
        </li>
        <li>
            <cc:FormLabel Text="Branch Location count" runat="server" />
            <asp:Label runat="server" ID="lblLocationCount" />
        </li>
        <li>
            <cc:FormLabel Text="Kiosk count" runat="server" />
            <asp:Label runat="server" ID="lblKioskCout" />
        </li>
    </ul>
     <div class="clearer">
    </div>
</div>
