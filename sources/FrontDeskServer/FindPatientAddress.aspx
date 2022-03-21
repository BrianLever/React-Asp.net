<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FindPatientAddress.aspx.cs"
	Inherits="FindPatientContact" MasterPageFile="~/FrontDeskMaster.master" %>

<%@ Register TagPrefix="ctrl" TagName="MatchedPatientList" Src="~/controls/MatchedPatientList.ascx" %>
<%@ Register Src="~/controls/UI/RpmsCredentialsExpiratonNotificationMessage.ascx" TagPrefix="ctrl" TagName="RpmsCredentialsExpirationAlert" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>


<asp:Content ID="cntNotification" runat="server" ContentPlaceHolderID="cphNotificaionArea">
	<ctrl:RpmsCredentialsExpirationAlert runat="server" ID="credentialsNotification" ClientIDMode="Static" />
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphContent">
	<div id="patient-details-root">
		<div id="PatientDetails">
            <asp:HiddenField ID="hdnPatientEhr" runat="server" />
			<h2><%= Resources.TextMessages.PatientInfoEdit_FDTitle %></h2>
			<div class="container">
				<canvas id="canvasArrow"></canvas>
				<div class="frame"></div>
			</div>
			<%-- First Name  --%>
			<div class="left">
				<cc:FormLabel ID="lblFirstName" runat="server" Mandatory="false" Text="<%$ Resources: FormTexts, FirstName %>"></cc:FormLabel>
			</div>
			<div class="right">
				<asp:TextBox ID="txtFirstName" runat="server" ReadOnly="true" />
			</div>
			<%-- Last Name  --%>
			<div class="left">
				<cc:FormLabel ID="lblLastName" runat="server" Mandatory="false" Text="<%$ Resources: FormTexts, LastName %>"></cc:FormLabel>
			</div>
			<div class="right">
				<asp:TextBox ID="txtLastName" runat="server" ReadOnly="true" />
			</div>
			<%-- Middle Name  --%>
			<div class="left">
				<cc:FormLabel ID="lblMiddleName" runat="server" Mandatory="false" Text="<%$ Resources: FormTexts, MiddleName %>"></cc:FormLabel>
			</div>
			<div class="right">
				<asp:TextBox ID="txtMiddleName" runat="server" ReadOnly="true" MaxLength="128"></asp:TextBox>
			</div>
			<%-- Date of Birth  --%>
			<div class="left">
				<cc:FormLabel ID="lblBirthday" runat="server" Text="<%$ Resources: FormTexts, Birthday %>"></cc:FormLabel>
			</div>
			<div class="right">
				<cc:RichDatePicker ID="txtBirthday" runat="server" ReadOnly="true"></cc:RichDatePicker>
			</div>

			<asp:PlaceHolder ID="phAddressDetails" runat="server">

				<%-- Phone   --%>
				<div class="left">
					<cc:FormLabel ID="lblPhone" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, Phone %>"></cc:FormLabel>
				</div>
				<div class="right">
					<cc:Phone ID="ccPhone" runat="server" IsRequired="true" ValidationGroup="ObjectInfo"
						ValidationErrorMessage="Please enter the phone number in format xxx-xxx-xxxx" />
				</div>
				<%-- Address   --%>
				<div class="left">
					<cc:FormLabel ID="lblAddress" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, Address %>"></cc:FormLabel>
				</div>
				<div class="right">
					<asp:TextBox ID="txtAddress" runat="server" MaxLength="512"></asp:TextBox>
					<asp:RequiredFieldValidator ID="vldAddress" runat="server" Display="None"
						ControlToValidate="txtAddress" ErrorMessage="Address is required" ValidationGroup="ObjectInfo" />
				</div>
				<%-- City   --%>
				<div class="left">
					<cc:FormLabel ID="lblCity" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, City %>"></cc:FormLabel>
				</div>
				<div class="right">
					<asp:TextBox ID="txtCity" runat="server" MaxLength="255"></asp:TextBox>
					<asp:RequiredFieldValidator ID="vldCity" runat="server" Display="None"
						ControlToValidate="txtCity" ErrorMessage="City is required" ValidationGroup="ObjectInfo" />
				</div>
				<%-- State   --%>
				<div class="left">
					<cc:FormLabel ID="lblState" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, State %>"></cc:FormLabel>
				</div>
				<div class="right">
					<asp:DropDownList runat="server" ID="ddlState" DataSourceID="odsState" DataTextField="Name"
						DataValueField="StateCode">
					</asp:DropDownList>
					<asp:RequiredFieldValidator ID="vldState" runat="server" Display="Dynamic"
						ControlToValidate="ddlState" ErrorMessage="State is required" ValidationGroup="ObjectInfo" />
				</div>
				<%-- Zip Code   --%>
				<div class="left">
					<cc:FormLabel ID="lblZipCode" runat="server" Mandatory="true" Text="<%$ Resources: FormTexts, ZipCode %>"></cc:FormLabel>
				</div>
				<div class="right">
					<asp:TextBox ID="txtZipCode" runat="server" MaxLength="5" SkinID="ZipCode"></asp:TextBox>
					<asp:RequiredFieldValidator ID="vldZipCode" runat="server" Display="None"
						ControlToValidate="txtZipCode" ErrorMessage="Zip Code is required"
						ValidationGroup="ObjectInfo" />
					<asp:CustomValidator ID="vldCustZipCode" runat="server" Display="None"
						ControlToValidate="txtZipCode" ErrorMessage="Please enter a 5-digit zip code value"
						ClientValidationFunction="ValidateZipCode"
						ValidationGroup="ObjectInfo" OnServerValidate="ZipCodeValidate"></asp:CustomValidator>
				</div>
			</asp:PlaceHolder>

			<div class="left">
			</div>
			<div class="right">
				<asp:ValidationSummary runat="server" ID="vldSummary" ValidationGroup="ObjectInfo" />
			</div>
			
			<div class="left">
			</div>
			<div class="right buttonset nopadding">
				<div class="fright">
					<asp:Button ID="btnSave" runat="server" Text="Save to ScreenDox" ValidationGroup="ObjectInfo"
						UseSubmitBehavior="false" OnClick="btnSave_Click" Width="150px" />
				</div>

			</div>


		</div>
		<div id="PatientMatched">
			<h2><%= Resources.TextMessages.PatientInfoEdit_RpmsTitle %></h2>
			<ctrl:MatchedPatientList runat="server" ID="ctrlMatchedPatientList" AllowDeselect="false" />
		</div>
	</div>

	<%-- Get all states --%>
	<asp:ObjectDataSource runat="server" ID="odsState" TypeName="FrontDesk.State" SelectMethod="GetAllState"
		EnableCaching="true" CacheExpirationPolicy="Absolute" CacheDuration="600"></asp:ObjectDataSource>
    
	<script type="text/javascript">
        
		function ValidateZipCode(sender, args) {
			args.IsValid = (args.Value.length === 5);
		}

		function drawArrow() {
			var canvas = $("#canvasArrow");
			var context = canvas.get(0).getContext("2d");

			var size = {
				width: canvas.width(),
				height: canvas.height()
			};
			context.canvas.width = size.width;
			context.canvas.height = size.height;


			context.save();
			var fromx = 0;
			var fromy = size.height / 2;
			var tox = size.width;
			var toy = fromy;
			var headlen = 10;

			context.lineCap = "round";
			context.lineWidth = 1;
			context.strokeStyle = context.fillStyle = "#4059AD";

			context.beginPath();
			context.moveTo(fromx, fromy);
			context.lineTo(tox, toy);
			context.closePath();
			context.stroke();

			context.beginPath();
			context.moveTo(tox, toy);
			context.lineTo(tox - headlen, toy - headlen / 2);
			context.lineTo(tox - headlen, toy + headlen / 2);
			context.closePath();
			context.fill();

			context.restore();
		}

        $(document).ready(function () {

            drawArrow();

            $("#patient-details-root").findPatientAddressCtrl({
                patientAddressCtrlId: 'PatientDetails',
                patientSearchResultCtrlId: 'PatientMatched'

            });
        });
        

	</script>

</asp:Content>
