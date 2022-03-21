<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SystemSettings.aspx.cs" Inherits="SystemTools_SystemSettings"
    MasterPageFile="~/SystemTools/SystemTools.master" %>

<%@ MasterType VirtualPath="~/SystemTools/SystemTools.master" %>
<asp:Content runat="server" ID="content" ContentPlaceHolderID="cphSystemTools">
    <div class="grid2col">
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblPasswordRenewalPeriodDays" Text="Password renewal period days"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox runat="server" ID="txtPasswordRenewalPeriodDays" SkinID=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvRenewPwdPeriod" runat="server"
                ErrorMessage="Password renewal period is required"
                ControlToValidate="txtPasswordRenewalPeriodDays" ValidationGroup="UpdateSetting"
                Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="cvRenewPwdPeriod" runat="server" ControlToValidate="txtPasswordRenewalPeriodDays"
                ValueToCompare="0" Type="Integer" Operator="GreaterThan" Text="*" Display="Dynamic"
                ValidationGroup="UpdateSetting" ErrorMessage="<%$ Resources: TextMessages, RenewalPeriodCompareError  %>"></asp:CompareValidator>

        </div>
        <div class="column left">
            &nbsp;
        </div>
        <div class="column right">
            <asp:Button runat="server" ID="btnSave" Text="Save Changes" OnClick="btnSave_Click"
                ValidationGroup="UpdateSetting" />
        </div>
        <div class="column left">
            &nbsp;
        </div>
        <div class="column w350px l tpad10">
            <asp:ValidationSummary runat="server" ID="vs" ValidationGroup="UpdateSetting"
                DisplayMode="SingleParagraph" />
        </div>
    </div>


    <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="sec_log_settings">
                <h2>Security log settings</h2>
                <asp:Repeater ID="rptCategory" runat="server" DataSourceID="odsEvent" OnItemDataBound="OnCategoryBound">
                    <ItemTemplate>
                        <div class="cat_name"><%# Eval("CategoryName") %></div>
                        <asp:Repeater ID="rptEvent" runat="server">
                            <ItemTemplate>
                                <div class="event">
                                    <div class="descr">
                                        <%# Eval("Description") %>
                                    </div>
                                    <div class="enabled">
                                        <asp:CheckBox ID="cbEnabled" runat="server" Checked='<%# Eval("Enabled") %>' />
                                        <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("SecurityEventID") %>' />
                                    </div>
                                </div>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <div class="event_alternate">
                                    <div class="descr">
                                        <%# Eval("Description") %>
                                    </div>
                                    <div class="enabled">
                                        <asp:CheckBox ID="cbEnabled" runat="server" Checked='<%# Eval("Enabled") %>' />
                                        <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("SecurityEventID") %>' />
                                    </div>
                                </div>
                            </AlternatingItemTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                </asp:Repeater>
                <div class="buttons bmar10">
                    <asp:Button ID="btnSaveLog" runat="server" Text="Save" OnClick="OnSaveSecuritySettigns" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



    <asp:ObjectDataSource ID="odsEvent" runat="server" TypeName="FrontDesk.Server.Logging.SecurityLog"
        SelectMethod="GetCategoriesWithEvents"></asp:ObjectDataSource>

</asp:Content>
