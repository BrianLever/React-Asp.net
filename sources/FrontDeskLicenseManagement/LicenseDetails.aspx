<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true" CodeFile="LicenseDetails.aspx.cs" Inherits="LicenseDetails" %>
<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    
    <div class="grid2col">
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblSerialNumberTitle" Text="Serial Number" Mandatory="false"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label runat="server" ID="lblSerialNumber" Text=""></asp:Label> <asp:Label runat="server" CssClass="note" Text="<%$ Resources: TextMessages, SerialNumberNoteText  %>" />          
        </div>

        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel4" Text="Years Of Validity (1 to 15)" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
        <!-- TODO: apply skin for numeric only input -->
            <asp:TextBox ID="txtYears" runat="server" MaxLength="2" SkinID="NumbersOnly"></asp:TextBox>
            <asp:RequiredFieldValidator ID="vldYears" runat="server" Display="Dynamic" CssClass="error_message"
                ControlToValidate="txtYears" ErrorMessage="Validity period is required"
                ValidationGroup="ObjectInfo" />
            <asp:RangeValidator ID="vldYearsRange" runat="server" 
                CssClass="error_message" Display="Dynamic" ValidationGroup="ObjectInfo"
                Type="Integer" MinimumValue="1" MaximumValue="15"
                ControlToValidate="txtYears" ErrorMessage="Validity period must be from 1 to 15 years"
            />
            
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel5" Text="Max Allowed Branch Locations (1 to 4095)" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtMaxLocations" runat="server" MaxLength="4" SkinID="NumbersOnly"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                CssClass="error_message" Display="Dynamic" ValidationGroup="ObjectInfo"
                Type="Integer" MinimumValue="1" MaximumValue="4095"
                ControlToValidate="txtMaxLocations" ErrorMessage="Max Allowed Branch Locations must be from 1 to 4095"
            />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" CssClass="error_message"
                ControlToValidate="txtMaxLocations" ErrorMessage="Max Allowed Branch Locations are required"
                ValidationGroup="ObjectInfo" />
        </div>
        
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel6" Text="Max Allowed Kiosks (1 to 4095)" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtMaxKiosks" runat="server" MaxLength="4" SkinID="NumbersOnly"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator2" runat="server" 
                CssClass="error_message" Display="Dynamic" ValidationGroup="ObjectInfo"
                Type="Integer" MinimumValue="1" MaximumValue="4095"
                ControlToValidate="txtMaxKiosks" ErrorMessage="Max Allowed Kiosks must be from 1 to 4095"
            />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" CssClass="error_message"
                ControlToValidate="txtMaxKiosks" ErrorMessage="Max Allowed Kiosks are required"
                ValidationGroup="ObjectInfo" />
        </div>

        <asp:PlaceHolder ID="plcNew" runat="server">                       
        
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel1" Text="Quantity of Licenses to create" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
        <!-- TODO: assume no more than 999 licenses at once -->
            <asp:TextBox ID="txtNumLicenses" runat="server" MaxLength="3" SkinID="NumbersOnly"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" CssClass="error_message"
                ControlToValidate="txtNumLicenses" ErrorMessage="Quantity of Licenses is required"
                ValidationGroup="ObjectInfo" />
        </div>        
        
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="plcExisting" runat="server">                       

        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel8" Text="License Key" ></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label runat="server" ID="lblLicenseString" Text=""></asp:Label>
            &nbsp;
            <asp:LinkButton ID="btnCopyLicenseCode" runat="server" Text="Copy" CausesValidation="false" />
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel9" Text="Issued" ></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label runat="server" ID="lblIssued" Text=""></asp:Label>
        </div>
        
        </asp:PlaceHolder>                
        
        <asp:PlaceHolder ID="plcOwner" runat="server">                       

        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel10" Text="Registered To" ></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:HyperLink ID="hlnClientDetails" runat="server"></asp:HyperLink>
            <%--<asp:Label runat="server" ID="lblCompanyName" Text=""></asp:Label>--%>
        </div>
        
        </asp:PlaceHolder>
        
        <asp:PlaceHolder ID="plcActivation" runat="server">
        
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel2" Text="Activated" ></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label runat="server" ID="lblActivationDate" Text=""></asp:Label>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel3" Text="Activation Request" ></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label runat="server" ID="lblActivationRequest" Text=""></asp:Label>
            &nbsp;
            <asp:LinkButton ID="btnCopyActivationRequest" runat="server" Text="Copy" CausesValidation="false" />
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel7" Text="Activation Key" ></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label runat="server" ID="lblActivationKey" Text=""></asp:Label>
            &nbsp;
            <asp:LinkButton ID="btnCopyActivationKey" runat="server" Text="Copy" CausesValidation="false" />
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel11" Text="Expires" ></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label runat="server" ID="lblExpirationDate" Text=""></asp:Label>
        </div>
        
        </asp:PlaceHolder>
                
        <div class="column left">
            &nbsp;
        </div>
        <div class="column right">
            <asp:ValidationSummary runat="server" ID="vldSummary" ValidationGroup="ObjectInfo" />
        </div>
        
        <div class="column left">
            &nbsp;
        </div>
        <div class="column lmar10 tpad15 w350px r">
            <asp:Button runat="server" ID="btnCreate" Text="Create" 
                ValidationGroup="ObjectInfo" UseSubmitBehavior="false" onclick="btnCreate_Click"
                 />

            <asp:Button runat="server" ID="btnDelete" Text="Delete" ValidationGroup="ObjectInfo"
                UseSubmitBehavior="false"  
                OnClientClick="if(!confirm('Are you sure that you want to delete this License?')){return false;}" 
                onclick="btnDelete_Click" />
            
            <asp:Button runat="server" ID="btnBack" Text="Back" UseSubmitBehavior="false" OnClientClick="location.href='Licenses.aspx'; return false" />
        </div>
    </div>

</asp:Content>

