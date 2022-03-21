<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true" CodeFile="Activation.aspx.cs" Inherits="Activation" %>
<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <asp:UpdatePanel ID="up" runat="server" RenderMode="Block">
        <ContentTemplate>
    <div class="grid2col w80 c">
        <div class="column left">
            <cc:FormLabel ID="FormLabel1" runat="server" Text="Activation Request" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtActivationRequest" runat="server" MaxLength="128"></asp:TextBox>
            <asp:RequiredFieldValidator ID="vldRequest" runat="server" Display="Dynamic"
                CssClass="error_message" ControlToValidate="txtActivationRequest" ErrorMessage="Activation request is required"
                ValidationGroup="ObjectInfo" />
        </div>
        
        <asp:PlaceHolder ID="phLicenseDetails" runat="server" Visible="false">
            
            <div class="column left header">
                <div>
                    License details
                </div>
            </div>
            <div class="column right header">
                <div>&nbsp; </div>
            </div>
        
            <div class="column left">
                Client:
            </div>
            <div class="column right linked">
                <asp:PlaceHolder ID="phExistClient" runat="server">
                    <asp:HyperLink ID="hlClient" runat="server" Target="_blank" NavigateUrl="~/ClientDetails.aspx"></asp:HyperLink>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="phNoClient" runat="server">
                    <asp:Literal ID="lblNoClient"   runat="server" Mode="PassThrough" />
                </asp:PlaceHolder>
            </div>            
        
            <div class="column left">
                Serial Number:
            </div>
            <div class="column right">
                <asp:Label runat="server" ID="lblSerialNumber" Text=""></asp:Label>
            </div>
        
            <div class="column left">
                Years of Validity:
            </div>
            <div class="column right">
                <asp:Label runat="server" ID="lblYearsOfValidity" Text=""></asp:Label>
            </div>
        

            <div class="column left">
                Number of Branch Locations:
            </div>
            <div class="column right">
                <asp:Label runat="server" ID="lblBrNum" Text=""></asp:Label>
            </div>

            <div class="column left">
                Number of Kiosks:
            </div>
            <div class="column right">
                <asp:Label runat="server" ID="lblKiosksNum" Text=""></asp:Label>
            </div>

            <div class="column left">
                License Key:
            </div>
            <div class="column right">
                <asp:Label runat="server" ID="lblLicense" Text=""></asp:Label>
                &nbsp;
                <asp:LinkButton ID="btnCopyLicenseKey" runat="server" Text="Copy" CausesValidation="false" />
            </div>                                    
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phExpDate" Visible="false" runat="server">
        
            <div class="column joined header">
                <div>
                    Define the date of license expiration
                </div>
            </div>

            <div class="column left">
                The effective date of the license:
            </div>
            <div class="column right">
                <div>
                    <asp:RadioButton ID="rbnTomorrow" SkinID="" runat="server" Checked="true" AutoPostBack="true"
                        OnCheckedChanged="OnTommorowSelected" Text="Tomorrow" GroupName="EffectiveDate" />
                </div>
                <div class="fclear">
                    <div class="fleft">
                        <asp:RadioButton ID="rbtnOtherDate" runat="server" Text="Other date" AutoPostBack="true"
                            OnCheckedChanged="OnOtherDateSelected" GroupName="EffectiveDate" />
                    </div>
                    <div class="fleft lmar5">
                        <cc:RichDatePicker ID="rdpOtherDate" runat="server" Enabled="false"></cc:RichDatePicker>
                        <asp:RequiredFieldValidator ID="rfvOtherDate" runat="server"
                            ControlToValidate="rdpOtherDate" Enabled="false"
                            ValidationGroup="ObjectInfo" ErrorMessage="Date is required">&nbsp;*</asp:RequiredFieldValidator>
                    </div>
                    <div class="fleft lmar5">
                        <asp:Button ID="lbnCalculate" runat="server" Text="Calculate" 
                            OnClick="CalculateOther_Click"  ValidationGroup="ObjectInfo" Enabled="false"></asp:Button>
                    </div>
                </div>
            </div>
            <div class="column left">
                License expiration date:
            </div>
            <div class="column right">
                <asp:Label ID="lblExpirationDate" runat="server"></asp:Label>
            </div>
            
        </asp:PlaceHolder>
        
        <asp:PlaceHolder ID="phErr" runat="server" Visible="false">
            <div class="column joined">
                <asp:Label ID="lblErr" runat="server" CssClass="error_message" 
                    Text=""></asp:Label>
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
            <asp:Button runat="server" ID="btnContinue" Text="Continue" 
                ValidationGroup="ObjectInfo" UseSubmitBehavior="false" OnClick="btnContinue_Click" />
                            
            <asp:Button runat="server" ID="btnActivate" Text="Activate" Visible="false"
                ValidationGroup="ObjectInfo" UseSubmitBehavior="false" OnClick="btnActivate_Click" />
            
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" UseSubmitBehavior="false" />
        </div>       
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

