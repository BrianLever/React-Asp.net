<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="BranchLocationList.aspx.cs" Inherits="management_BranchLocation" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>

<%@ Register Src="~/controls/SimpleFilter.ascx" TagName="SimpleFilter" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="w80 c">
        <div class="tmar10 l">
            <asp:LinkButton ID="lnbNew" runat="server" Text="New Branch Location" OnClick="lnbNew_Click"></asp:LinkButton>
        </div>

        <asp:UpdatePanel runat="server" ID="upd" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
            </Triggers>
            <ContentTemplate>

                <div class="l bpad10 tpad10">
                    <div class="inline_filter">
                        <ul class="line2col check-in">
                            <li>
                                <cc:FormLabel runat="server" ID="lblLocationName" Text="Location name"></cc:FormLabel>
                                <asp:TextBox runat="server" ID="txtLocationNameFilter" IE:Width="200px" />
                            </li>
                            <li>
                                <cc:FormLabel runat="server" ID="lblScreeningProfile" Text="Screen Profile"></cc:FormLabel>
                                <asp:DropDownList ID="ddlScreeningProfile" runat="server" EnableViewState="false" IE:Width="206px"
                                    DataSourceID="odsScreeningProfiles" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </li>
                            <li>
                                <cc:FormLabel runat="server" ID="lblDisplayHidden" Text="Visibility"></cc:FormLabel>
                                <asp:DropDownList ID="ddlHideDisabled" runat="server" EnableViewState="false" IE:Width="206px">
                                    <asp:ListItem Value="0">Show active</asp:ListItem>
                                    <asp:ListItem Value="1">Show all</asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <li class="buttons">
                                <asp:Button runat="server" ID="btnSearch" UseSubmitBehavior="false" Text="Search" OnClick="btnSearch_Click" />
                                <asp:Button runat="server" ID="btnClear" UseSubmitBehavior="false" Text="Clear" OnClick="btnClear_Click" />

                            </li>
                        </ul>
                    </div>
                </div>
                <div class="tmar10">

                    <asp:GridView ID="gvBranchLocations" runat="server" DataSourceID="odsBranchLocations"
                        AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" PageSize="20">
                        <EmptyDataTemplate>
                            <div class="c w100">
                                No branch locations found.
                            </div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField HeaderText="Name" DataField="Name" SortExpression="Name" HeaderStyle-CssClass="w30 l"
                                ItemStyle-CssClass="l" />
                            <asp:BoundField HeaderText="Description" DataField="Description" SortExpression="Description"
                                HeaderStyle-CssClass="w40 l" ItemStyle-CssClass="l" />
                            <asp:BoundField HeaderText="Screen Profile / Department" DataField="ScreeningProfileName" SortExpression="ScreeningProfileName"
                                HeaderStyle-CssClass="w25 l" ItemStyle-CssClass="l" />
                            <asp:HyperLinkField HeaderText="Details" DataNavigateUrlFormatString="~/management/BranchLocationDetails.aspx?id={0}"
                                DataNavigateUrlFields="BranchLocationID" Text="Details" HeaderStyle-CssClass="c w5"
                                ItemStyle-CssClass="c" />
                        </Columns>
                    </asp:GridView>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:ObjectDataSource ID="odsBranchLocations" runat="server" TypeName="FrontDesk.Server.Screening.Services.BranchLocationService"
        EnablePaging="true" SelectMethod="GetAll" SelectCountMethod="CountAll" SortParameterName="orderBy" EnableCaching="false">
        <SelectParameters>
            <asp:Parameter Name="filterByName" Type="String" DefaultValue="" />
            <asp:Parameter Name="showDisabled" Type="Boolean" DefaultValue="false" />
            <asp:Parameter Name="screeningProfileId" Type="Int32" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:ObjectDataSource runat="server" ID="odsScreeningProfiles" TypeName="FrontDesk.Server.Screening.Services.ScreeningProfileService"
        SelectMethod="GetAll" EnableCaching="false"></asp:ObjectDataSource>

</asp:Content>
