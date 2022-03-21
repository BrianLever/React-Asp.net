<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PatientScheduledVisits.ascx.cs"
    Inherits="PatientScheduledVisits" %>
<input type="hidden" runat="server" id="hdnSelectedVisitID" />
<asp:UpdatePanel runat="server" ID="updRoot" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Repeater ID="rptVisits" runat="server" >
            <HeaderTemplate>
                <div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="EHRItem <%# Container.ItemIndex % 2 == 0 ? "" : "alt" %> <%# (Convert.ToDateTime(Eval("Date"))==ScreeningDate)? "preferred" : "" %>">
                    <div class="fleft tpad3" style="width: 40px">
                        &nbsp;
                        <asp:Image ID="imgApplyIcon" runat="server" ImageUrl="~/images/check.png" Visible='<%# Convert.ToInt32(Eval("ID")) == SelectedItemID %>' />
                    </div>
                    <div class="fleft">
                        <div>
                            <%# string.Format("{0:MM'/'dd'/'yyyy' 'HH':'mm}", Eval("Date"))%>
                        </div>
                        <div class="fclear">
                            <%# Eval("Location.Name")%>
                        </div>
                         <div class="fclear">
                            <%# Eval("ServiceCategory")%>
                        </div>
                        <br class="clearer"/>
                    </div>
                    <div class="fright pad5">
                        <asp:Button runat="server" ID="btnSelect" Text="Select" UseSubmitBehavior="false"
                            CausesValidation="false" CommandArgument='<%# Eval("ID") %>' OnClick="btnSelect_click"
                            Width="70px" />
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
        <asp:PlaceHolder ID="phPaging" runat="server">
            <div class="fleft pad10 ">
                <cc:Pager runat="server" ID="ctrlPaging" PageSize="5" OnNavigate="ccPaging_Navigate" />
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phNoMatches" runat="server" Visible="false">
            <div class="nomatch">
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: TextMessages, NoVisitsForPatient %>" />
            </div>
        </asp:PlaceHolder>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ctrlPaging" EventName="Navigate" />
    </Triggers>
</asp:UpdatePanel>
