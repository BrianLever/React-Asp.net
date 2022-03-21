<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FollowUpList.aspx.cs" Inherits="BhsFollowUpListForm"
    MasterPageFile="~/FrontDeskMaster.master" EnableSessionState="True" EnableViewState="true" 
    EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<%@ Register Src="~/controls/bhs/FollowUpList.ascx" TagName="List"
    TagPrefix="uc" %>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <div>
         <uc:List ID="ucList" runat="server" />
    </div>
</asp:Content>
