<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="BhsDemographics.aspx.cs" Inherits="BhsDemographicsForm" EnableSessionState="True"
    EnableViewState="false" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<%@ Register Src="~/controls/Bhs/PatientDemographics.ascx" TagName="PatientDemographics" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="bhsreport_details bhsvisit">
        <p>
            <cc:FormLabel runat="server" ID="lblRecordIDLabel" Text="Record No." />
            <asp:Literal runat="server" ID="lblRecordID" />
            <cc:FormLabel runat="server" ID="lblCreatedDateLabel" Text="Created Date" />
            <asp:Literal runat="server" ID="lblCreatedDate" />
            <cc:FormLabel runat="server" ID="lblLocationLabel" Text="Branch Location" />
            <asp:Literal runat="server" ID="lblLocation" />
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
                <asp:Button ID="btnFindAddress" runat="server" Text="Find Address" UseSubmitBehavior="false" OnClick="btnFindAddress_Click" />

            </div>
        </div>
    </div>
    <div>
        <uc:PatientDemographics ID="ucDemographics" runat="server" Visible="true" />
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

    
</asp:Content>
