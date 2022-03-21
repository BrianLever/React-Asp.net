<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="KioskList.aspx.cs" Inherits="KioskManagement_KioskList" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<%@ Register Src="~/controls/SimpleFilter.ascx" TagName="SimpleFilter" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="w80 c">
        <div class="tmar10 l bpad10">
            <a href="KioskDetails.aspx">Add New Kiosk</a>
        </div>
        <asp:UpdatePanel runat="server" ID="upnlKioskList" ChildrenAsTriggers="true" UpdateMode="Conditional">
            <ContentTemplate>


                <div class="l bpad10">
                    <div class="inline_filter">
                        <ul class="line2col check-in">
                            <li>
                                <cc:FormLabel runat="server" ID="lblName" Text="Location name or Key"></cc:FormLabel>
                                <asp:TextBox runat="server" ID="txtNameFilter" IE:Width="200px" />
                            </li>
                            <li>
                                <cc:FormLabel runat="server" ID="lblScreeningProfile" Text="Screen Profile"></cc:FormLabel>
                                <asp:DropDownList ID="ddlScreeningProfile" runat="server" EnableViewState="false" IE:Width="206px"
                                    DataSourceID="odsScreeningProfiles" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </li>
                            <li>
                                <cc:FormLabel runat="server" ID="lblLocationName" Text="Branch Location"></cc:FormLabel>
                                <asp:DropDownList ID="ddlBranchLocation" runat="server" EnableViewState="false" IE:Width="206px"
                                    DataSourceID="odsBranchLocations" DataTextField="Name" DataValueField="BranchLocationID" AppendDataBoundItems="true">
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
                                <asp:Button runat="server" ID="btnSearch" UseSubmitBehavior="true" Text="Search" OnClick="btnSearch_Click" />
                                <asp:Button runat="server" ID="btnClear" UseSubmitBehavior="false" Text="Clear" OnClick="btnClear_Click" />

                            </li>
                        </ul>
                    </div>
                </div>
                <div class="tmar10">
                    <asp:GridView ID="gvKiosks" runat="server" DataSourceID="odsKiosk" AutoGenerateColumns="false"
                        AllowSorting="true" DataKeyNames="KioskID" AllowPaging="true"
                        OnSorting="gvKiosks_Sorting">
                        <EmptyDataTemplate>
                            <div class="c w100">
                                No kiosks found.
                            </div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField HeaderText="Key" SortExpression="KioskID" HeaderStyle-CssClass="w15" />
                            <asp:BoundField HeaderText="Kiosk Name" DataField="KioskName" SortExpression="KioskName"
                                HeaderStyle-CssClass="w20" />
                            <asp:BoundField HeaderText="Branch Location" DataField="BranchLocationName" SortExpression="BranchLocationName"
                                HeaderStyle-CssClass="w15" />
                            <asp:BoundField HeaderText="Screen Profile" DataField="ScreeningProfileName" SortExpression="ScreeningProfileName"
                                HeaderStyle-CssClass="w15" />

                            <asp:BoundField HeaderText="Last Activity Time" DataField="LastActivityDate" DataFormatString="{0:MM/dd/yyyy HH:mm zzz}"
                                SortExpression="LastActivityDate" HeaderStyle-CssClass="w15" />
                            <asp:BoundField HeaderText="IP Address" DataField="IpAddress" SortExpression="IpAddress"
                                HeaderStyle-CssClass="w15" />
                            <asp:BoundField HeaderText="Version" DataField="KioskAppVersion" SortExpression="KioskAppVersion"
                                HeaderStyle-CssClass="w5" />

                            <asp:TemplateField HeaderText="Details" HeaderStyle-CssClass="r w10">
                                <ItemTemplate>
                                    <div class="r">
                                        <asp:HyperLink ID="hlDetails" runat="server" Text="Details" NavigateUrl='<%# String.Format("KioskDetails.aspx?id={0}", Eval("KioskID")) %>'>
                                        </asp:HyperLink>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvKiosks" EventName="Sorting" />
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
    <asp:ObjectDataSource runat="server" ID="odsKiosk"
        SelectCountMethod="GetKioskCount" EnablePaging="true" SelectMethod="GetAllWithFiltering"
        SortParameterName="orderBy">
        <SelectParameters>
            <asp:Parameter Name="kioskID" Type="Int16" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="filterByName" Type="String" DefaultValue="" />
            <asp:Parameter Name="branchLocationID" Type="Int32" DefaultValue="" />
            <asp:Parameter Name="screeningProfileID" Type="Int32" DefaultValue="" />
            <asp:Parameter Name="showDisabled" Type="Boolean" DefaultValue="false" />
            <asp:Parameter Name="userID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" ID="odsScreeningProfiles" TypeName="FrontDesk.Server.Screening.Services.ScreeningProfileService"
        SelectMethod="GetAll" EnableCaching="false"></asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" ID="odsBranchLocations" TypeName="FrontDesk.Server.Screening.Services.BranchLocationService"
        SelectMethod="GetAll" EnableCaching="true" CacheExpirationPolicy="Absolute"></asp:ObjectDataSource>
</asp:Content>
