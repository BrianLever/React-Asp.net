<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="FollowUp.aspx.cs" Inherits="BhsFollowUpForm" EnableSessionState="True"
    EnableViewState="false" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<%@ Register Src="~/controls/Bhs/FollowUp.ascx" TagName="FollowUp" TagPrefix="uc" %>
<asp:Content ID="cntCss" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="bhsreport_details bhsvisit">
        <p>
            <cc:FormLabel runat="server" ID="lblRecordIDLabel" Text="Record No." />
            <asp:Literal runat="server" ID="lblRecordID" />
            <cc:FormLabel runat="server" ID="lblCreatedDateLabel" Text="Visit Date" />
            <asp:Literal runat="server" ID="lblCreatedDate" />
            <cc:FormLabel runat="server" ID="lblLocationLabel" Text="Branch Location" />
            <asp:Literal runat="server" ID="lblLocation" />
            <cc:FormLabel runat="server" ID="FormLabel1" Text="Patient Screening" />
            <asp:HyperLink runat="server" ID="lnkScreening" />
             <cc:FormLabel runat="server" ID="lblVisit" Text="Visit" />
            <asp:HyperLink runat="server" ID="lnkVisit"  />
        </p>
        <asp:PlaceHolder runat="server" ID="phValidationErrors" Visible="false">
            <p class="errors">
                <asp:Literal runat="server" ID="lblValdationErrors" />
            </p>
        </asp:PlaceHolder>
        <div class="clearer">
        </div>
        <div class="buttons">
            <div class="fleft w50">
                <asp:Button ID="btnBack" runat="server" Text="Cancel" UseSubmitBehavior="false" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" UseSubmitBehavior="false" OnClick="btnPrint_Click" />
            </div>
        </div>
    </div>
    <div>
        <uc:FollowUp ID="ucDetails" runat="server" Visible="true" />
    </div>
    <div class="grid2col">
        <div class="column left"></div>
        <div class="column right">
            <asp:ValidationSummary runat="server" ID="vldSummary" ValidationGroup="ObjectInfo" />
        </div>
    </div>
    <div class="buttons grid2col">
        <div class="column left"></div>
        <div class="column right">
            <asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" UseSubmitBehavior="false" OnClick="btnSaveChanges_Click" ValidationGroup="ObjectInfo" />
            <asp:Button ID="btnReturn" runat="server" Text="Return to List" UseSubmitBehavior="false" />
        </div>
    </div>
     <script>
       $(document).ready(function () {
           $("form").pageEvents({
               "saveChangesButtonId": "<%= btnSaveChanges.ClientID %>"
           });

       });

   </script>
</asp:Content>
