<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SimpleFilter.ascx.cs"
    Inherits="SimpleFilterCtrl" %>
<div class="simple_filter">
    <asp:DropDownList runat="server" ID="ddlFilter" EnableViewState="false" AccessKey="Y" ToolTip="Use Alt + Y to set focus on this list">
    </asp:DropDownList>
    <cc:FormLabel runat="server" ID="lblFilter" Text="Filter"></cc:FormLabel>
    <asp:TextBox ID="txtValue" runat="server" EnableViewState="false"  AccessKey="F"  ToolTip="Use Alt + F to set focus on this field"></asp:TextBox>
    <asp:Button ID="btnSearch" runat="server" UseSubmitBehavior="false" Text="Search"
        EnableViewState="false" OnClick="Search_Click"  CausesValidation="false" Width="70px" />
    <asp:Button ID="btnClear" runat="server" UseSubmitBehavior="false" Text="Clear" EnableViewState="false"
        OnClick="Clear_Click" CausesValidation="false" Width="70px"/>
</div>
<div class="clearer"></div>
