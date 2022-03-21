<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GPRAPeriodTimeRange.ascx.cs"
    Inherits="UI.GPRAPeriodTimeRangeControl" %>
<ul>
    <li class="label">
        <asp:RadioButton runat="server" ID="rbtCustom" GroupName="grpa" Checked="true" Text="Custom" />
    </li>
    <li>
        <cc:RichDatePicker runat="server" ID="dpStartDate" /><span>&nbsp;to&nbsp;</span><cc:RichDatePicker
            runat="server" ID="dpEndDate" />
    </li>
    <li class="label">
        <asp:RadioButton runat="server" ID="rbtGpra" GroupName="grpa" Text="GPRA Reporting Period" />
    </li>
    <li>
        <asp:DropDownList runat="server" ID="cmbGpraPeriods" />
    </li>
</ul>
