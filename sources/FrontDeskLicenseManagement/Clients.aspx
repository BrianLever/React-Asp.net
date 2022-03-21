<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDeskMaster.master" AutoEventWireup="true" 
    CodeFile="Clients.aspx.cs" Inherits="Clients" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
        
    <div class="w80 c">
    
<%--    <asp:Panel ID="pnlFilter" runat="server">
    
        <div class="inline_filter c" >
            <ul class="line">
                <li>
                    <cc:FormLabel ID="FormLabel1" runat="server" Text="Branch Location"></cc:FormLabel>
                </li>
                <li >
                    <asp:DropDownList runat="server" ID="ddlBranchLocation" DataSourceID="odsBranchLocation" 
                        DataTextField="Name" DataValueField="BranchLocationID" 
                        AutoPostBack="true" 
                        onselectedindexchanged="ddlBranchLocation_SelectedIndexChanged">
                    </asp:DropDownList>
                </li>
            </ul>
        </div>
        
        <div class="clearer"></div>
    
    </asp:Panel>
--%>    
    <div class="tmar10 l action">
        <asp:LinkButton ID="lnbNew" runat="server" Text="Add new client" 
            onclick="lnbNew_Click" ></asp:LinkButton>
    </div>
    
    <div class="tmar10 c">
    <asp:UpdatePanel runat="server" ID="upnlClientList" ChildrenAsTriggers="true" UpdateMode="Conditional"  >
            <ContentTemplate>
    
    <asp:GridView ID="gvClients" runat="server" 
        DataSourceID="odsClients" AutoGenerateColumns="false"
        AllowSorting="true" AllowPaging="true" PageSize="20" >
        <EmptyDataTemplate>
            <div class="c w100">
                No clients found.</div>
        </EmptyDataTemplate>
        <Columns>
            <asp:BoundField HeaderText="Company Name" DataField="CompanyName" SortExpression="CompanyName"
                HeaderStyle-CssClass="w40 l" ItemStyle-CssClass="l" />
            <asp:BoundField HeaderText="Email" DataField="Email" SortExpression="Email"
                HeaderStyle-CssClass="w35 l" ItemStyle-CssClass="l"/>
            <asp:BoundField HeaderText="Contact Phone" DataField="ContactPhone" SortExpression="ContactPhone"
                HeaderStyle-CssClass="w20 l" ItemStyle-CssClass="l"/>
            <asp:TemplateField HeaderText="Details" HeaderStyle-CssClass="c w5" ItemStyle-CssClass="c">
                <ItemTemplate>
                    <div class="c">
                        <asp:HyperLink ID="hlDetails" runat="server" Text="Details"
                            NavigateUrl='<%# String.Format("~/ClientDetails.aspx?id={0}", Eval("ClientID")) %>'>
                        </asp:HyperLink>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvClients" EventName="Sorting" />
    </Triggers>
    </asp:UpdatePanel>
    
    </div>
    
    </div>    
    
    <asp:ObjectDataSource ID="odsClients" runat="server"
        SelectMethod="GetAllWithPaging" SelectCountMethod="GetCount" 
        SortParameterName="orderBy"  EnablePaging="true">
    </asp:ObjectDataSource>

</asp:Content>

