<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="Licenses.aspx.cs" EnableEventValidation="false" Inherits="Licenses" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<%@ Register Src="~/controls/SimpleFilter.ascx" TagPrefix="uc" TagName="Filter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="w90 c">
        <uc:Filter runat="server" ID="ctrlFilter" OnSearching="ucFilter_Searching">
            <FilterByValues>Filter by license key</FilterByValues>
        </uc:Filter>
        <div class="tmar10 l action">
            <asp:LinkButton ID="lnbNew" runat="server" Text="Create new license" OnClick="lnbNew_Click"></asp:LinkButton>
        </div>
        <div class="tmar10 c">
            <asp:UpdatePanel runat="server" ID="upnlLicenseList" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvLicenses" runat="server" DataSourceID="odsLicenses" AutoGenerateColumns="false"
                        AllowSorting="true" AllowPaging="true" PageSize="20" OnSorting="gvLicenses_Sorting">
                        <EmptyDataTemplate>
                            <div class="c w100">
                                No licenses found.</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField HeaderText="License Key" DataField="LicenseString" SortExpression="LicenseString" 
                                HeaderStyle-CssClass="w15"/>
                            <asp:BoundField HeaderText="Issued On" DataField="Issued" SortExpression="Issued" DataFormatString="{0:MM/dd/yyyy}" 
                                HeaderStyle-CssClass="w10"/>
                            <asp:HyperLinkField HeaderText="Registered To" DataNavigateUrlFormatString="~/ClientDetails.aspx?id={0}"
                                DataNavigateUrlFields="ClientID" DataTextField="CompanyName" SortExpression="CompanyName" 
                                HeaderStyle-CssClass="w10" />
                            <asp:BoundField HeaderText="Duration (years)" DataField="Years" SortExpression="Years" 
                                HeaderStyle-CssClass="w10" />
                            <asp:BoundField HeaderText="Max. Locations" DataField="MaxBranchLocations" SortExpression="MaxBranchLocations" 
                                HeaderStyle-CssClass="w10" />
                            <asp:BoundField HeaderText="Max. Kiosks" DataField="MaxKiosks" SortExpression="MaxKiosks" 
                                HeaderStyle-CssClass="w10" />
                            <asp:BoundField HeaderText="Serial Number" DataField="SerialNumber" SortExpression="SerialNumber" 
                                HeaderStyle-CssClass="w10" />
                            <asp:BoundField HeaderText="Activated On" DataField="Activated" SortExpression="Activated" DataFormatString="{0:MM/dd/yyyy}" 
                                HeaderStyle-CssClass="w10"/>    
                            <asp:BoundField HeaderText="Expired On" DataField="ExpirationDate" SortExpression="ExpirationDate" DataFormatString="{0:MM/dd/yyyy}" 
                                HeaderStyle-CssClass="w10"/>    
                            <asp:HyperLinkField HeaderText="Details" 
                                HeaderStyle-CssClass="c w5" Text="Details"
                                DataNavigateUrlFormatString="~/LicenseDetails.aspx?id={0}" DataNavigateUrlFields="LicenseID" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvLicenses" EventName="Sorting" />
                    <asp:AsyncPostBackTrigger ControlID="ctrlFilter" EventName="Searching" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:ObjectDataSource ID="odsLicenses" runat="server" TypeName="FrontDesk.Server.Licensing.Management.LicenseEntityHelper"
        SelectMethod="GetAllWithFilter" EnablePaging="true" SelectCountMethod="CountAll" SortParameterName="orderBy">
        <SelectParameters>
            <asp:Parameter Name="licenseKey" DbType="String" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
