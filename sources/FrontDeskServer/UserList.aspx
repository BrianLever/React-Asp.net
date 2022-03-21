<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true"
    CodeFile="UserList.aspx.cs" Inherits="management_UserList" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="w80 c">
        <asp:Panel ID="pnlFilter" runat="server">
            <div class="inline_filter c">
                <ul class="line">
                    <li>
                        <cc:FormLabel ID="FormLabel1" runat="server" Text="Branch Location"></cc:FormLabel>
                    </li>
                    <li>
                        <asp:DropDownList runat="server" ID="ddlBranchLocation" DataSourceID="odsBranchLocation"
                            DataTextField="Name" DataValueField="BranchLocationID" AutoPostBack="true" OnSelectedIndexChanged="ddlBranchLocation_SelectedIndexChanged">
                        </asp:DropDownList>
                    </li>
                </ul>
            </div>
            <div class="clearer">
            </div>
        </asp:Panel>
        <div class="tmar10 l action">
            <asp:LinkButton ID="lnbNew" runat="server" Text="Add New User" OnClick="lnbNew_Click"></asp:LinkButton>
        </div>
        <div class="tmar10 c">
            <asp:UpdatePanel runat="server" ID="upnlUserList" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvUsers" runat="server" DataSourceID="odsUsers" AutoGenerateColumns="false"
                        AllowSorting="true" 
                        AllowPaging="true" >
                        <EmptyDataTemplate>
                            <div class="c w100">
                                No users found.</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField HeaderText="First Name" DataField="FirstName" SortExpression="FirstName" HeaderStyle-CssClass="w15" />
                            <asp:BoundField HeaderText="Last Name" DataField="LastName" SortExpression="LastName" HeaderStyle-CssClass="w15" />
                            <asp:BoundField HeaderText="Login" DataField="Username" SortExpression="Username" HeaderStyle-CssClass="w10" />
                            <asp:BoundField HeaderText="Email" DataField="Email" SortExpression="Email" HeaderStyle-CssClass="w15"/>
                            <asp:BoundField HeaderText="Phone" DataField="ContactPhone" SortExpression="ContactPhone" HeaderStyle-CssClass="w15" />
                            <asp:BoundField HeaderText="Branch Location" DataField="BranchLocationName" SortExpression="BranchLocationName" HeaderStyle-CssClass="w15" />
                            <asp:HyperLinkField HeaderText="Details" HeaderStyle-CssClass="c w5" ItemStyle-CssClass="c" DataNavigateUrlFormatString="~/UserDetails.aspx?id={0}" DataNavigateUrlFields="UserID" Text="Details"  />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlBranchLocation" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="gvUsers" EventName="Sorting" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:ObjectDataSource ID="odsUsers" runat="server" TypeName="FrontDesk.Server.Membership.FDMembershipUser"
        SelectMethod="GetListWithBranchLocation">
        <SelectParameters>
            <asp:Parameter Name="branchLocationID" ConvertEmptyStringToNull="true" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsBranchLocation" runat="server" TypeName="FrontDesk.Server.Screening.Services.BranchLocationService"
        SelectMethod="GetAllForDisplay"></asp:ObjectDataSource>
</asp:Content>
