<%@ Page Title="" Language="C#" 
    MasterPageFile="~/SystemTools/SystemTools.master"
    AutoEventWireup="true" CodeFile="RpmsCredentials.aspx.cs" Inherits="SystemTools_RpmsCredentials" %>

<%@ MasterType VirtualPath="~/SystemTools/SystemTools.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSystemTools" runat="Server">
    <div class="ui-helper-clearfix content-right-margin">
        <div class="tmar10 l action">
            <asp:HyperLink ID="lnkNew" runat="server" Text="Enter new EHR credentials" NavigateUrl="NewRpmsCredentials.aspx" />
        </div>
        <div class="info">
            <asp:Localize runat="server" Text="<%$ Resources: TextMessages, RpmsCredentials_MoreThanOneMessage %>">
        
            </asp:Localize>
        </div>
        <div class="tmar10 c">
            <asp:UpdatePanel runat="server" ID="updList" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grvItems" runat="server" DataKeyNames="Id" AutoGenerateColumns="false"
                        AllowSorting="false" AllowPaging="false">
                        <EmptyDataTemplate>
                            <div class="c w100">
                                You have not entered any credential.</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField HeaderText="Access Code" HeaderStyle-CssClass="w20" />
                            <asp:BoundField HeaderText="Verify Code" HeaderStyle-CssClass="w15" />
                            <asp:BoundField HeaderText="Expired On" HeaderStyle-CssClass="w10" />
                            <asp:TemplateField HeaderStyle-CssClass="w5 c">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnDelete" CommandName="remove" CausesValidation="false"
                                        Text="[ X ]" OnClientClick="if(!confirm('You are about to remove credential. Proceed?')){return false;}" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        
    </div>
</asp:Content>
