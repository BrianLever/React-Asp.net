<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="ErrorLog.aspx.cs" Inherits="ErrorLogForm" EnableSessionState="False"
    EnableViewState="false" EnableViewStateMac="false" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content runat="server" ID="conternt" ContentPlaceHolderID="cphContent">
    <div class="grid2col">
        <div class="column left">
            <cc:FormLabel runat="server" Text="ID" Mandatory="false"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label ID="lblID" runat="server" />
        </div>
         <div class="column left">
            <cc:FormLabel runat="server" Text="Created date" Mandatory="false"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label ID="lblCreatedDate" runat="server" />
        </div>
<%--        <div class="column left">
            <cc:FormLabel runat="server" Text="Kiosk" Mandatory="false"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label ID="lblKioskLabel" runat="server" />
        </div>--%>
        <div class="column left">
            <cc:FormLabel runat="server" Text="Message" Mandatory="false"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label ID="lblErrorMessage" runat="server" />
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" Text="Stack trace" Mandatory="false"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label ID="lblStackTrace" runat="server" />
        </div>
        <div class="column left"></div>
        <div class="column right">
            <asp:Button runat="server" ID="btnBack" OnClientClick="location.href='ErrorLogList.aspx'; return false;" Text="Back" />
        </div>
    </div>
    <div class="clearer"></div>
</asp:Content>
