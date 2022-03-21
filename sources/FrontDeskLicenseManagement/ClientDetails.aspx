<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="ClientDetails.aspx.cs" Inherits="ClientDetails" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="grid2col w80 c">
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel4" Text="Company name" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="128"></asp:TextBox>
            <asp:RequiredFieldValidator ID="vldCompanyName" runat="server" Display="Dynamic"
                CssClass="error_message" ControlToValidate="txtCompanyName" ErrorMessage="Company name is required"
                ValidationGroup="ObjectInfo" />
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" Text="State"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:DropDownList ID="ddlState" runat="server" AppendDataBoundItems="true" DataSourceID="odsStates"
                DataTextField="Name" DataValueField="StateCode" />
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" Text="City"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtCity" runat="server" MaxLength="128"></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel7" Text="Address line 1"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtAddress1" runat="server" MaxLength="128"></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel8" Text="Address line 2"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtAddress2" runat="server" MaxLength="128"></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel9" Text="Postal code"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtPostalCode" runat="server" MaxLength="24"></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel10" Text="E-mail"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtEmail" runat="server" MaxLength="128"></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel11" Text="Contact person"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtContactPerson" runat="server" MaxLength="128"></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel12" Text="Contact phone"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtContactPhone" runat="server" MaxLength="24"></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel13" Text="Notes"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtNotes" runat="server" MaxLength="1024" Height="100px" TextMode="MultiLine"></asp:TextBox>
        </div>
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
            <asp:Button runat="server" ID="btnAdd" Text="Add" ValidationGroup="ObjectInfo" UseSubmitBehavior="false"
                OnClick="btnAdd_Click" />
            <asp:Button runat="server" ID="btnUpdate" Text="Save Changes" ValidationGroup="ObjectInfo"
                UseSubmitBehavior="false" OnClick="btnUpdate_Click" />
            <asp:Button runat="server" ID="btnDelete" Text="Delete" ValidationGroup="ObjectInfo"
                UseSubmitBehavior="false" OnClientClick="if(!confirm('Are you sure that you want to delete this Client?')){return false;}"
                OnClick="btnDelete_Click" />
            <asp:Button runat="server" ID="btnBack" Text="Back" UseSubmitBehavior="false" OnClientClick="location.href='Clients.aspx'; return false" />
        </div>
    </div>
        
        <asp:PlaceHolder runat="server" ID="phClientLicense">
        
            <div class="w80 c tpad10 b bpad10 i" style="text-align:left;font-size:11pt">
                Client Licenses
            </div>
            <div class="w80 c">    
            
            <div class="l tmar10 fclear action">                
                <asp:LinkButton ID="lnbAssignLicense" runat="server" Text="Assign license"></asp:LinkButton>&nbsp;
                <asp:LinkButton ID="lnbtnCreateLicense" runat="server" Text="Get New license"></asp:LinkButton>
            </div>
            <div class="l fclear tmar10">
                <asp:GridView ID="gvLicenses" runat="server" DataSourceID="odsrLicense" AutoGenerateColumns="false"
                    AllowSorting="true">
                    <EmptyDataTemplate>
                        <div class="c w100">
                            No licenses found.</div>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:BoundField HeaderText="License Key" DataField="LicenseString" SortExpression="LicenseString" />
                        <asp:BoundField HeaderText="Issued On" DataField="Issued" SortExpression="Issued" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField HeaderText="Max. Locations" DataField="MaxBranchLocations"
                            SortExpression="MaxBranchLocations" />
                        <asp:BoundField HeaderText="Max. Kiosks" DataField="MaxKiosks" SortExpression="MaxKiosks" />
                        <asp:BoundField HeaderText="Duration (years)" DataField="Years" SortExpression="Years" />
                        
                        <asp:BoundField HeaderText="Activated On" DataField="Activated" SortExpression="Activated" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField HeaderText="Expired On" DataField="ExpirationDate" SortExpression="ExpirationDate" DataFormatString="{0:MM/dd/yyyy}" />
                        
                        <%--<asp:BoundField HeaderText="Serial Number" DataField="SerialNumber" SortExpression="SerialNumber" />--%>
                        <asp:HyperLinkField HeaderText="Details" HeaderStyle-CssClass="c w5" Text="Details"
                            DataNavigateUrlFormatString="~/LicenseDetails.aspx?id={0}" DataNavigateUrlFields="LicenseID" />
                    </Columns>
                </asp:GridView>
            </div>
            </div>
            
        </asp:PlaceHolder>
    
    <asp:ObjectDataSource runat="server" ID="odsStates" TypeName="FrontDesk.State" SelectMethod="GetList"
        EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="600" />
    <asp:ObjectDataSource runat="server" ID="odsrLicense" TypeName="FrontDesk.Server.Licensing.Management.LicenseEntityHelper"
        SelectMethod="GetForClient">
        <SelectParameters>
            <asp:QueryStringParameter Name="clientID" QueryStringField="id" DbType="Int32" />
        </SelectParameters>        
    </asp:ObjectDataSource>
</asp:Content>
