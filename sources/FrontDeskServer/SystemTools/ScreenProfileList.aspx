<%@ Page Title="" Language="C#" 
    MasterPageFile="~/SystemTools/SystemTools.master"
    AutoEventWireup="true"
    CodeFile="ScreenProfileList.aspx.cs" Inherits="management_ScreenProfile" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/SystemTools/SystemTools.master" %>

<%@ Register Src="~/controls/SimpleFilter.ascx" TagName="SimpleFilter" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSystemTools" runat="Server">
    <div class="w80 c">
        <div class="tmar10 l">
            <asp:LinkButton ID="lnbNew" runat="server" Text="New Screen Profile" OnClick="lnbNew_Click"></asp:LinkButton>
        </div>
        <div class="l bpad10 tpad10">
            <uc:SimpleFilter runat="server" ID="ucFilter" OnSearching="ucFilter_Searching">
                <FilterByValues>Filter by name</FilterByValues>
            </uc:SimpleFilter>
        </div>
        <div class="tmar10">
            <asp:UpdatePanel runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <asp:GridView ID="gvItems" runat="server" DataSourceID="odsItems"
                        AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" PageSize="20" >
                        <EmptyDataTemplate>
                            <div class="c w100">
                                No screen profile found.</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField HeaderText="Name" DataField="Name" SortExpression="Name" HeaderStyle-CssClass="w30 l"
                                ItemStyle-CssClass="l" />
                            <asp:BoundField HeaderText="Description" DataField="Description" SortExpression="Description"
                                HeaderStyle-CssClass="w65 l" ItemStyle-CssClass="l" />
                            <asp:HyperLinkField HeaderText="Details" DataNavigateUrlFormatString="~/systemtools/ScreenProfile.aspx?id={0}"
                                DataNavigateUrlFields="ID" Text="Details" HeaderStyle-CssClass="c w5"
                                ItemStyle-CssClass="c" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:ObjectDataSource ID="odsItems" runat="server" TypeName="FrontDesk.Server.Screening.Services.ScreenProfileService"
        EnablePaging="true" SelectMethod="GetAll" SelectCountMethod="CountAll" SortParameterName="orderBy" EnableCaching="false">
        <SelectParameters>
            <asp:Parameter Name="filterByName" Type="String" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
