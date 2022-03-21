<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"
    MasterPageFile="~/FrontDeskMaster.master" EnableSessionState="True" EnableViewState="true" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<%@ Register Src="~/controls/CheckInList.ascx" TagName="CheckInList"
    TagPrefix="uc" %>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <div>
         <uc:CheckInList ID="ucCheckInList" runat="server" />
    </div>
</asp:Content>
