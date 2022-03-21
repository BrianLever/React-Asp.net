<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="PatientCheckIn.aspx.cs" Inherits="PatientCheckIn" EnableSessionState="False"
    EnableViewState="false" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<%@ Register Src="~/controls/Report/BHSReport.ascx" TagName="BHSReport" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="bhsreport_details">
        <p>
            <cc:FormLabel runat="server" ID="lblRecordIDLabel" Text="Record No." />
            <asp:Literal runat="server" ID="lblRecordID" />
            <cc:FormLabel runat="server" ID="lblCreatedDateLabel" Text="Created Date" />
            <asp:Literal runat="server" ID="lblCreatedDate" />
            <cc:FormLabel runat="server" ID="lblLocationLabel" Text="Branch Location" />
            <asp:Literal runat="server" ID="lblLocation" />
        </p>
        <p id="pnlExportInfo" runat="server" visible="false">

            <cc:FormLabel runat="server" ID="lblExportDateLabel" Text="Exported At" />
            <asp:Literal runat="server" ID="lblExportDate" />
            <cc:FormLabel runat="server" ID="lblHRNLabel" Text="HRN" />
            <asp:Literal runat="server" ID="lblHRN" />
            <cc:FormLabel runat="server" ID="lblLinkedVisitTimeLabel" Text="Visit Date" />
            <asp:Literal runat="server" ID="lblLinkedVisitTime" />
            <cc:FormLabel runat="server" ID="lblLinkedVisitLocationLabel" Text="Visit Location" />
            <asp:Literal runat="server" ID="lblLinkedVisitLocation" />
            <cc:FormLabel runat="server" ID="lblExportedByLabel" Text="Exported By" />
            <asp:Literal runat="server" ID="lblExportedBy" />
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
                <asp:Button ID="btnEdit" runat="server" Text="Edit" UseSubmitBehavior="false" OnClick="btnEdit_Click" />
                <span class="create-bhs-visit">
                    <asp:Button ID="btnCreateBhsVisit" runat="server" Text="Create Visit" UseSubmitBehavior="false" OnClick="btnCreateBhsVisit_Click" />
                </span>
                <span class="create-bhs-visit">
                    <asp:Button ID="btnOpenPatientDemographics" runat="server" Text="Open Patient Demographics Report" UseSubmitBehavior="false" OnClientClick="return false;" CausesValidation="false" />
                </span>
            </div>
            <div class="delete fright w40">
                <asp:Button ID="btnDelete" runat="server" Text="Delete Screening Report" OnClick="btnDelete_Click"
                    UseSubmitBehavior="false" Visible="false" />
            </div>
        </div>
    </div>
    <div>
        <uc:BHSReport ID="ucReport" runat="server" Visible="true" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var demographicsId = <%= PatientDemographicsID??0%>;


            var demographicsBtn = $("#<%= btnOpenPatientDemographics .ClientID%>");

            if (demographicsId > 0) {
                demographicsBtn.click(function () {
                    window.open('<% =PatientDemographicsUrl %>', '_blank');
                    return false;
                });
            } else {
                demographicsBtn.hide();
            }
        });

    </script>
</asp:Content>
