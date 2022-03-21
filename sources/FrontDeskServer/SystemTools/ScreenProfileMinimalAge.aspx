<%@ Page Title="" Language="C#" MasterPageFile="~/systemtools/ScreeningProfile.master"
    AutoEventWireup="true" CodeFile="ScreenProfileMinimalAge.aspx.cs" Inherits="ScreenProfileMinimalAgeForm" %>

<%@ MasterType VirtualPath="~/systemtools/ScreeningProfile.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphSubContent" runat="Server">
    <div class="content-right-margin">
        <link rel="Stylesheet" href='<%= ResolveClientUrl("~/styles/iphonecheckbox.css")%>' type="text/css" media="screen" />

        <div class="validation_error">
            <asp:ValidationSummary ID="vldSummary" runat="server" ValidationGroup="form" EnableClientScript="true" />
        </div>
        <div class="c tmar10">
            <asp:Repeater runat="server" ID="rptAgeParams">
                <HeaderTemplate>
                    <table class="gridView ageSelection">
                        <thead>
                            <tr>
                                <th class="c1">Screening Measure
                                </th>
                                <th class="c2">Minimum Age
                                </th>
                                <th class="c3">Turn On/Off
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <FooterTemplate>
                    </tbody></table>
                </FooterTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="c1">
                            <%# (Server.HtmlEncode(Convert.ToString(Eval("ScreeningSectionLabel")))) %>
                            <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ScreeningSectionID") %>' />
                        </td>
                        <td class="c2">
                            <asp:TextBox ID="txtAge" runat="server" SkinID="age" Text='<%# Eval("MinimalAge")%>' Visible='<%# Eval("IsAgeEditControlVisible")%>' />
                            <asp:RequiredFieldValidator ID="vldAge" runat="server" ControlToValidate="txtAge"
                                Display="None" ErrorMessage="<%$ Resources : TextMessages, ScreeningMinimalAgeForm_EmptyValueError %>" ValidationGroup="form" />
                            <asp:RangeValidator ID="vldAgeRange" runat="server" ControlToValidate="txtAge" Display="None"
                                ErrorMessage="<%$ Resources : TextMessages, ScreeningMinimalAgeForm_RangeError %>"
                                MinimumValue="0" MaximumValue="200" Type="Integer" ValidationGroup="form" />
                        </td>
                        <td class="c3">
                            <asp:CheckBox ID="chkEnabled" runat="server" Checked='<%# Eval("IsEnabled")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="alt">
                        <td class="c1">
                            <%# (Server.HtmlEncode(Convert.ToString(Eval("ScreeningSectionLabel")))) %>
                            <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ScreeningSectionID") %>' />
                        </td>
                        <td class="c2">
                            <asp:TextBox ID="txtAge" runat="server" SkinID="age" Text='<%# Eval("MinimalAge")%>' Visible='<%# Eval("IsAgeEditControlVisible")%>' />
                            <asp:RequiredFieldValidator ID="vldAge" runat="server" ControlToValidate="txtAge"
                                Display="None" ErrorMessage="<%$ Resources : TextMessages, ScreeningMinimalAgeForm_EmptyValueError %>" ValidationGroup="form" />
                            <asp:RangeValidator ID="vldAgeRange" runat="server" ControlToValidate="txtAge" Display="None"
                                ErrorMessage="<%$ Resources : TextMessages, ScreeningMinimalAgeForm_RangeError %>"
                                MinimumValue="0" MaximumValue="200" Type="Integer" ValidationGroup="form" />
                        </td>
                        <td class="c3">
                            <asp:CheckBox ID="chkEnabled" runat="server" Checked='<%# Eval("IsEnabled")%>' />
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </div>
        <div class="l note tmar5">
            <asp:Localize ID="ltrNote" runat="server" Text="<%$ Resources: TextMessages, ScreeningMinimalAgeForm_Notes %>"></asp:Localize>
        </div>
        <div class="c tmar10">
            <div class="l">
                <asp:Button runat="server" ID="btnSave" CausesValidation="true" Text="Save changes"
                    UseSubmitBehavior="false" OnClientClick="if(!confirm('You are about to update minimum age settings for measures. Changes will be applied to all connected kiosks. Proceed?')){return false;}" ValidationGroup="form" />
                <asp:Button runat="server" ID="btnReset" CausesValidation="false" Text="Reset" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(".ageSelection").find(':checkbox').iphoneStyle();

        $(".ageSelection").minimalAgeCheckboxController({
            groups: <%= GetScreeningSectionGroupsAsJson()%>
        });


    </script>
</asp:Content>
