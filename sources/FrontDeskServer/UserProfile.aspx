<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="UserProfile"
    MasterPageFile="~/FrontDeskMaster.master" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content runat="server" ID="content" ContentPlaceHolderID="cphContent">
    <div class="w100">
        <div class="fleft w20 w200px leftMenu">
            <asp:Menu runat="server" id="menuLeft" DataSourceID="smdsLeftMenu" SkinID="LeftMenu"></asp:Menu>
            <asp:SiteMapDataSource ID="smdsLeftMenu" runat="server" StartingNodeUrl="~/UserProfile.aspx"
                ShowStartingNode="false" />
        </div>
        <div class="fleft w60">
            <div class="grid2col">
                <asp:UpdatePanel runat="server" ID="upnlProfile" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="column left">
                            <cc:FormLabel runat="server" ID="lblUserName" Text="User name"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:TextBox ID="txtUsername" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="column left">
                            <cc:FormLabel runat="server" ID="lblGroup" Text="Group"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:Label runat="server" ID="lblGroupValue"></asp:Label>
                        </div>
                        <div class="column left">
                            <cc:FormLabel runat="server" ID="lblBranchLocation" Text="Branch location"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:Label runat="server" ID="lblBranchLocationValue"></asp:Label>
                        </div>
                        <div class="column left">
                            <cc:FormLabel ID="lblFirstName" runat="server" Text="First name" Mandatory="true"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="128"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vldFirstName" runat="server" Display="Dynamic" CssClass="error_message"
                                ControlToValidate="txtFirstName" ErrorMessage="First name is required"
                                ValidationGroup="ObjectInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="column left">
                            <cc:FormLabel runat="server" ID="lblLastName" Text="Last name" Mandatory="true"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:TextBox ID="txtLastName" runat="server" MaxLength="128"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vldLastName" runat="server" Display="Dynamic" CssClass="error_message"
                                ControlToValidate="txtLastName" ErrorMessage="Last name is required"
                                ValidationGroup="ObjectInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="column left">
                            <cc:FormLabel ID="lblMiddleName" runat="server" Text="Middle name"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:TextBox ID="txtMiddleName" runat="server" MaxLength="128"></asp:TextBox>
                        </div>
                        <div class="column left">
                            <cc:FormLabel runat="server" ID="lblMail" Text="E-mail" Mandatory="false"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="128" />
<%--                            <asp:RequiredFieldValidator ID="vldAddEmail" runat="server" ControlToValidate="txtEmail"
                                Display="Static" ValidatText="*" ionGroup="ObjectInfo" ErrorMessage="Email is required"></asp:RequiredFieldValidator>
--%>                            <asp:RegularExpressionValidator ID="vldRegEmail" runat="server" ErrorMessage="E-mail is not valid"
                                ControlToValidate="txtEmail" CssClass="error_message" Display="Static" EnableClientScript="true"
                                Text="E-mail is not valid" SetFocusOnError="True" ValidationGroup="ObjectInfo"></asp:RegularExpressionValidator>
                        </div>
                        <div class="column left">
                            <cc:FormLabel runat="server" ID="lblCity" Text="City"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:TextBox ID="txtCity" runat="server" MaxLength="128"></asp:TextBox>
                        </div>
                        <div class="column left">
                            <cc:FormLabel runat="server" ID="lblState" Text="State"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:DropDownList runat="server" ID="ddlState" DataSourceID="odsState" DataTextField="Name"
                                DataValueField="StateCode">
                            </asp:DropDownList>
                        </div>
                        <div class="column left">
                            <cc:FormLabel runat="server" ID="lblAddress" Text="Address line 1"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:TextBox ID="txtAddress1" runat="server" MaxLength="128"></asp:TextBox>
                        </div>
                        <div class="column left">
                            <cc:FormLabel runat="server" ID="lblAddress2" Text="Address line 2" MaxLength="128"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:TextBox ID="txtAddress2" runat="server"></asp:TextBox>
                        </div>
                        <div class="column left">
                            <cc:FormLabel runat="server" ID="lblPostalCode" Text="Postal code" MaxLength="24"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:TextBox ID="txtPostalCode" runat="server"></asp:TextBox>
                            <asp:RegularExpressionValidator runat="server" ID="vldPostalCode" ControlToValidate="txtPostalCode"
                                Display="Static" ValidationGroup="ObjectInfo" ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>
                        </div>
                        <div class="column left">
                            <cc:FormLabel runat="server" ID="lblPhone" Text="Contact phone" ></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:TextBox runat="server" ID="txtPhone" MaxLength="24"></asp:TextBox>
                        </div>
                        <div class="column left">
                            <cc:FormLabel runat="server" ID="FormLabel2" Text="Comments"></cc:FormLabel>
                        </div>
                        <div class="column right">
                            <asp:TextBox ID="txtComments" runat="server" Height="100px" TextMode="MultiLine"
                                MaxLength="128"></asp:TextBox>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="column left">
                  
                </div>
                <div class="column right">
                    <asp:ValidationSummary runat="server" ID="vldSummary" ValidationGroup="ObjectInfo" />
                </div>
                <div class="column left">
                    &nbsp;
                </div>
                <div class="column right buttons w350px">
                    <asp:Button runat="server" ID="btnSave" Text="Save Changes" ValidationGroup="ObjectInfo"
                        UseSubmitBehavior="false" OnClick="btnSave_Click" Width="110px" />
                    <asp:Button runat="server" ID="btnBack" Text="Back" UseSubmitBehavior="false" OnClientClick="location.href='Default.aspx'; return false" />
                </div>
            </div>
        </div>
    </div>
    <%-- Get all state --%>
    <asp:ObjectDataSource runat="server" ID="odsState" TypeName="FrontDesk.State" SelectMethod="GetAllState"
        EnableCaching="true" CacheExpirationPolicy="Absolute" CacheDuration="600"></asp:ObjectDataSource>
</asp:Content>
