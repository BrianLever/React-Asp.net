<%@ Page Title="" Language="C#" MasterPageFile="~/SystemTools/SystemTools.master"
    AutoEventWireup="true" CodeFile="LicenseManagement.aspx.cs" Inherits="SystemTools_LicenseManagement" %>

<%@ Register Src="~/controls/SystemStatusSummary.ascx" TagName="SystemSummary" TagPrefix="uc" %>
<%@ MasterType VirtualPath="~/SystemTools/SystemTools.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSystemTools" runat="Server">
    <div class="content-right-margin">
        <div class="tmar10 l action">
            <asp:HyperLink ID="lnkNew" runat="server" Text="Enter new license key" NavigateUrl="NewLicenseKey.aspx" />
        </div>
        <div class="info">
            <asp:Localize runat="server" Text="<%$ Resources: TextMessages, LicenseKey_MoreThanOneLisenceRuleInfo %>">
        
            </asp:Localize>
        </div>
        <div class="tmar10 c">
            <asp:UpdatePanel runat="server" ID="updList" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grvItems" runat="server" DataSourceID="odsLicences" AutoGenerateColumns="false"
                        AllowSorting="false" AllowPaging="false">
                        <EmptyDataTemplate>
                            <div class="c w100">
                                You have not entered any license key.</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField HeaderText="License Key" HeaderStyle-CssClass="w20" />
                            <asp:BoundField HeaderText="Registered Date" HeaderStyle-CssClass="w15" />
                            <asp:BoundField HeaderText="Max. Locations" HeaderStyle-CssClass="w15" />
                            <asp:BoundField HeaderText="Max. Kiosks" HeaderStyle-CssClass="w10" />
                            <asp:BoundField HeaderText="Duration (years)" HeaderStyle-CssClass="w15" />
                            <asp:BoundField HeaderText="Activated On" HeaderStyle-CssClass="w10" />
                            <asp:BoundField HeaderText="Expired On" HeaderStyle-CssClass="w10" />
                            <asp:TemplateField HeaderStyle-CssClass="w5 c">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnDelete" CommandName="remove" CausesValidation="false"
                                        Text="[ X ]" OnClientClick="if(!confirm('You are about to remove License Key. Proceed?')){return false;}" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="tmar10 tpad10">
            <uc:SystemSummary runat="server" ID="ucSummary" />
        </div>
    </div>
    <div class="clearer">
    </div>
    <asp:ObjectDataSource ID="odsLicences" runat="server" TypeName="FrontDesk.Server.Licensing.Services.LicenseService"
        SelectMethod="GetAllProductLicensesForDisplay" EnableCaching="false"></asp:ObjectDataSource>
</asp:Content>
