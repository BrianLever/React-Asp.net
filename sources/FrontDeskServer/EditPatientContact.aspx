<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditPatientContact.aspx.cs"
    Inherits="EditPatientContact" MasterPageFile="~/FrontDeskMaster.master" %>

<%@ Register TagPrefix="ctrl" TagName="MatchedPatientList" Src="~/controls/MatchedPatientList.ascx" %>
<%@ Register Src="~/controls/UI/RpmsCredentialsExpiratonNotificationMessage.ascx" TagPrefix="ctrl" TagName="RpmsCredentialsExpirationAlert" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>

<asp:Content ID="cntNotification" runat="server" ContentPlaceHolderID="cphNotificaionArea">
    <ctrl:RpmsCredentialsExpirationAlert runat="server" ID="credentialsNotification" ClientIDMode="Static" />
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphContent">
    <div id="patient-details-root">
        <asp:UpdatePanel runat="server" ID="updRoot" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <div id="PatientDetails">
                    <h2><%= Resources.TextMessages.PatientInfoEdit_FDTitle %></h2>
                    <div class="container">
                        <canvas id="canvasArrow"></canvas>
                        <div class="frame"></div>
                    </div>

                    <%-- First Name  --%>
                    <div class="left">
                        <cc:FormLabel ID="lblFirstName" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, FirstName %>"></cc:FormLabel>
                    </div>
                    <div class="right">
                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="128" data-item="FirstName"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="vldFirstName" runat="server" Display="None"
                            ControlToValidate="txtFirstName" ErrorMessage="First name is required" ValidationGroup="ObjectInfo" />
                    </div>
                    <%-- Last Name  --%>
                    <div class="left">
                        <cc:FormLabel ID="lblLastName" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, LastName %>"></cc:FormLabel>
                    </div>
                    <div class="right">
                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="128" data-item="LastName"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="vldLastName" runat="server" Display="None"
                            ControlToValidate="txtLastName" ErrorMessage="Last name is required" ValidationGroup="ObjectInfo" />
                    </div>
                    <%-- Middle Name  --%>
                    <div class="left">
                        <cc:FormLabel ID="lblMiddleName" runat="server" Mandatory="false" Text="<%$ Resources: FormTexts, MiddleName %>"></cc:FormLabel>
                    </div>
                    <div class="right">
                        <asp:TextBox ID="txtMiddleName" runat="server" MaxLength="128" data-item="MiddleName"></asp:TextBox>
                    </div>
                    <%-- Date of Birth  --%>
                    <div class="left">
                        <cc:FormLabel ID="lblBirthday" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, Birthday %>"></cc:FormLabel>
                    </div>
                    <div class="right">
                        <cc:RichDatePicker ID="txtBirthday" runat="server" data-item="Birthday"></cc:RichDatePicker>
                        <asp:RequiredFieldValidator ID="vldBirthday" runat="server" Display="None"
                            ControlToValidate="txtBirthday" ErrorMessage="Date of birth is required" ValidationGroup="ObjectInfo" />
                        <asp:CustomValidator ID="c_vldBirthday" runat="server" Display="Dynamic" Text="Date of birth can not be more than today"
                            ControlToValidate="txtBirthday" ErrorMessage="Date of birth can not be more than today"
                            ValidationGroup="ObjectInfo" OnServerValidate="BirthdayValidate"></asp:CustomValidator>
                    </div>

                    <asp:PlaceHolder ID="phAddressDetails" runat="server">

                        <%-- Phone   --%>
                        <div class="left">
                            <cc:FormLabel ID="lblPhone" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, Phone %>"></cc:FormLabel>
                        </div>
                        <div class="right">
                            <cc:Phone ID="ccPhone" runat="server" IsRequired="true" ValidationGroup="ObjectInfo"
                                ValidationErrorMessage="Please enter the phone number in format xxx-xxx-xxxx" data-item="Phone" />
                        </div>
                        <%-- Address   --%>
                        <div class="left">
                            <cc:FormLabel ID="lblAddress" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, Address %>"></cc:FormLabel>
                        </div>
                        <div class="right">
                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="512" data-item="StreetAddress"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vldAddress" runat="server" Display="None"
                                ControlToValidate="txtAddress" ErrorMessage="Address is required" ValidationGroup="ObjectInfo" />
                        </div>
                        <%-- City   --%>
                        <div class="left">
                            <cc:FormLabel ID="lblCity" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, City %>"></cc:FormLabel>
                        </div>
                        <div class="right">
                            <asp:TextBox ID="txtCity" runat="server" MaxLength="255" data-item="City"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vldCity" runat="server" Display="None"
                                ControlToValidate="txtCity" ErrorMessage="City is required" ValidationGroup="ObjectInfo" />
                        </div>
                        <%-- State   --%>
                        <div class="left">
                            <cc:FormLabel ID="lblState" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, State %>"></cc:FormLabel>
                        </div>
                        <div class="right">
                            <asp:DropDownList runat="server" ID="ddlState" DataSourceID="odsState" DataTextField="Name"
                                DataValueField="StateCode" data-item="StateID" data-item-text="StateName">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vldState" runat="server" Display="Dynamic"
                                ControlToValidate="ddlState" ErrorMessage="State is required" ValidationGroup="ObjectInfo" />
                        </div>
                        <%-- Zip Code   --%>
                        <div class="left">
                            <cc:FormLabel ID="lblZipCode" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, ZipCode %>"></cc:FormLabel>
                        </div>
                        <div class="right">
                            <asp:TextBox ID="txtZipCode" runat="server" MaxLength="5" SkinID="ZipCode" data-item="ZipCode"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vldZipCode" runat="server" Display="None"
                                ControlToValidate="txtZipCode" ErrorMessage="Zip Code is required"
                                ValidationGroup="ObjectInfo" />
                            <asp:CustomValidator ID="vldCustZipCode" runat="server" Display="None"
                                ControlToValidate="txtZipCode" ErrorMessage="Please enter a 5-digit zip code value"
                                ClientValidationFunction="editPatientContact_ValidateZipCode"
                                ValidationGroup="ObjectInfo" OnServerValidate="ZipCodeValidate"></asp:CustomValidator>
                        </div>
                    </asp:PlaceHolder>

                    <div class="left">
                    </div>
                    <div class="right">
                        <asp:ValidationSummary runat="server" ID="vldSummary" ValidationGroup="ObjectInfo" />
                    </div>
                    <div class="fclear" id="changeMessage" style="visibility: hidden">
                        <asp:Label runat="server" CssClass="error_message" ID="lblChangeMessage" Text="<%$ Resources: TextMessages, EditPatient_DataChanged %>"></asp:Label>
                    </div>
                    <div class="left">
                    </div>
                    <div class="right buttonset nopadding">
                        <div class="fright">
                            <asp:Button ID="btnSave" runat="server" Text="Save to ScreenDox" ValidationGroup="ObjectInfo"
                                UseSubmitBehavior="false" OnClick="btnSave_Click" Width="150px" />
                        </div>

                        <div id="resetButton" class="fright rpad5 resetbtn" style="display: none">
                            <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="true"
                                UseSubmitBehavior="false" Width="70px" OnClick="btnReset_Click" />
                        </div>

                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="PatientMatched">
            <h2><%= Resources.TextMessages.PatientInfoEdit_RpmsTitle %></h2>
            <ctrl:MatchedPatientList runat="server" ID="ctrlMatchedPatientList" />
        </div>



        <div class="fclear patientDetails note l">
            <p>
                The phone number and address in the ScreenDox box will change in the EHR Record. 
                <br />
                If the ScreenDox Record is different than the EHR Record, the text
                in the EHR Record will appear in <span class="red">RED</span>.
                <br />
                If the EHR Record is correct, 
                change the information in the box to match the EHR Record 
                and click "Save to ScreenDox",
                click "Select EHR Record" for the correct EHR Record, and click "Next &gt;".
 
            </p>

            <p>
                NOTE: If the phone number, street address, city, state, and ZIP code fields are blank in the ScreenDox box, this means that contact information is not collected, per system settings.<br />
                These blank fields will not replace the information in the EHR record. Click the “Select EHR Record” button and then click the “Next &gt;” button.<br />
            </p>

        </div>

        <asp:Panel runat="server" ID="pnlExportEligibilityNotes" CssClass="fclear note ">
            <div class="fright rpad5">
                <p>
                    <%= GetAlreadyExportedWarning() %>
                </p>
            </div>
        </asp:Panel>

        <div class="fclear">
            <div class="fright">
                <div id="exportButton" class="fright rpad5">
                    <asp:Button ID="btnExport" runat="server" Text="Next >" UseSubmitBehavior="false" CausesValidation="false"
                        OnClick="btnExport_Click" Width="70px" />
                </div>
                <div class="fright rpad5">
                    <asp:Button runat="server" ID="btnBack" Text="< Previous" UseSubmitBehavior="false"
                        Width="70px" />
                </div>
            </div>
        </div>
    </div>

    <%-- Get all states --%>
    <asp:ObjectDataSource runat="server" ID="odsState" TypeName="FrontDesk.State" SelectMethod="GetAllState"
        EnableCaching="true" CacheExpirationPolicy="Absolute" CacheDuration="600"></asp:ObjectDataSource>

    <script type="text/javascript">

        $("#patient-details-root").editPatientContactCtrl({
            changeMessageLocator: "#changeMessage",
            resetButtonLocator: "#resetButton",
            exportButtonLocator: "#exportButton",
            data: <%= GetPatientInfoAsJson() %>
        });



    </script>
</asp:Content>
