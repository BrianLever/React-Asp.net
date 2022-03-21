<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="ExportWizard.aspx.cs" Inherits="ExportWizard" EnableSessionState="ReadOnly" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<%@ Register Src="~/controls/ExportingPatientInfo.ascx" TagPrefix="ctrl" TagName="PatientRecord" %>
<%@ Register Src="~/controls/PatientScheduledVisits.ascx" TagPrefix="ctrl" TagName="ScheduledVisits" %>
<%@ Register Src="~/controls/PreviewScreeningExport.ascx" TagPrefix="ctrl" TagName="PreviewExportResults" %>
<%@ Register Src="~/controls/ExportParametersAtGlance.ascx" TagPrefix="ctrl" TagName="ExportParametersAtGlance" %>
<%@ Register Src="~/controls/ScreeningExportResults.ascx" TagPrefix="ctrl" TagName="ScreeningExportResults" %>

<%@ Register Src="~/controls/UI/RpmsCredentialsExpiratonNotificationMessage.ascx" TagPrefix="ctrl" TagName="RpmsCredentialsExpirationAlert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <ctrl:RpmsCredentialsExpirationAlert runat="server" ID="credentialsNotification" ClientIDMode="Static" />
    <asp:Wizard runat="server" ID="exportWizard" ClientIDMode="Static" ActiveStepIndex="1"
        DisplayCancelButton="true" CancelDestinationPageUrl="~/PatientCheckIn.aspx">
        <LayoutTemplate>
            <div class="wizard">
                <div class="wizard-sidebar clearfix">
                    <ul>
                        <asp:PlaceHolder ID="sideBarPlaceHolder" runat="server" />
                    </ul>
                </div>
                <div class="wizard-header">
                    <asp:PlaceHolder ID="headerPlaceHolder" runat="server" Visible="false" />
                </div>
                <div class="wizard-content clearfix">
                    <asp:PlaceHolder ID="WizardStepPlaceHolder" runat="server" />
                </div>
                <div class="wizard-buttons">
                    <asp:PlaceHolder ID="navigationPlaceHolder" runat="server" />
                </div>
            </div>
        </LayoutTemplate>
        <SideBarTemplate>
            <asp:ListView ID="sideBarList" runat="server">
                <LayoutTemplate>
                    <ul id="ItemPlaceHolder" runat="server" class="clearfix" />
                </LayoutTemplate>
                <ItemTemplate>
                    <li>
                        <asp:LinkButton ID="sideBarButton" runat="server" Text="Button" />
                    </li>
                </ItemTemplate>
                <SelectedItemTemplate>
                    <li class="selected">
                        <asp:LinkButton ID="sideBarButton" runat="server" Text="Button" />
                    </li>
                </SelectedItemTemplate>
            </asp:ListView>
        </SideBarTemplate>
        <StartNavigationTemplate>
            <asp:Button ID="btnNext" CommandName="MoveNext" UseSubmitBehavior="false" runat="server"
                Text="Next >" />
        </StartNavigationTemplate>
        <StepNavigationTemplate>
            <asp:Button ID="btnPrev" UseSubmitBehavior="false" CommandName="MovePrevious" runat="server"
                Text="< Previous" />
            <asp:Button ID="btnNext" UseSubmitBehavior="false" CommandName="MoveNext" runat="server"
                Text="Next >" />
        </StepNavigationTemplate>
        <FinishNavigationTemplate>
            <asp:Button ID="btnPrev" UseSubmitBehavior="false" CommandName="MovePrevious" runat="server"
                Text="< Previous" />
            <asp:Button ID="btnFinish" UseSubmitBehavior="false" CommandName="MoveComplete" runat="server"
                Text="Start Export" />
        </FinishNavigationTemplate>
        <HeaderTemplate>
            <%= GetStepDescription() %>
        </HeaderTemplate>
        <WizardSteps>
            <asp:WizardStep ID="patientRecordStep" runat="server" Title="Find Patient Record">
            </asp:WizardStep>
            <asp:WizardStep ID="visitStep" Title="Find Visit" StepType="Finish">
                <div>
                    <div class="fleft w40">
                        <ctrl:PatientRecord runat="server" ID="ctrlPatientRecord" />
                    </div>
                    <div class="fright w50">
                        <ctrl:ScheduledVisits runat="server" ID="ctrlVisits" />
                    </div>
                </div>
            </asp:WizardStep>
            <asp:WizardStep ID="resultStep" runat="server" StepType="Complete" Title="Results">
                <ctrl:ScreeningExportResults ID="ctrlScreeningExportResults" runat="server" />
                <div id="export-preview-page">
                    <ctrl:PreviewExportResults ID="ctrlPreviewExportResult" runat="server" />

                    <asp:PlaceHolder ID="plhSucceed" runat="server" Visible="False">
                        <script type="text/javascript">

                            function closeButtonClick() {
                                $("#<%= btnClose.ClientID %>").click();
                            }

                            setTimeout(closeButtonClick, 5000);
                        </script>
                    </asp:PlaceHolder>



                    <div class="r">
                        <asp:Button ID="btnPreviousOnError" runat="server" Text="< Previous" OnClick="btnPreviousOnError_Click" Visible="False" />
                        <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" />
                    </div>
                </div>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>
