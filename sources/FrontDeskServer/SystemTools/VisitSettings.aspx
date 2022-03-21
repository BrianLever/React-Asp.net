<%@ Page Title="" Language="C#" MasterPageFile="~/SystemTools/SystemTools.master"
    AutoEventWireup="true" CodeFile="VisitSettings.aspx.cs" Inherits="VisitSettingsForm" %>

<%@ MasterType VirtualPath="~/SystemTools/SystemTools.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphSystemTools" runat="Server">
    <div class="content-right-margin">
        <link rel="Stylesheet" href='<%= ResolveClientUrl("~/styles/iphonecheckbox.css")%>' type="text/css" media="screen" />

        <div class="validation_error">
            <asp:ValidationSummary ID="vldSummary" runat="server" ValidationGroup="form" EnableClientScript="true" />
        </div>
        <div class="c tmar10">
            <asp:Repeater runat="server" ID="rptAgeParams">
                <HeaderTemplate>
                    <table class="gridView visitSettingsSelection">
                        <thead>
                            <tr>
                                <th class="c1">Measure Tool
                                </th>
                                <th class="c2">Minumum Cut Score
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
                            <%# (Server.HtmlEncode(Convert.ToString(Eval("Name")))) %>
                            <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("Id") %>' />
                        </td>
                        <td class="c2">
                            <table class="grid2col">
                                <tr>
                                    <td class="left">
                                        <asp:Label runat="server" ID="lblCutScore" CssClass="hint" />
                                    </td>
                                    <td class="right">
                                        <asp:TextBox runat="server" ID="txtCutScore" Value='<%# Eval("CutScore") %>' />

                                        <asp:RequiredFieldValidator ID="vldCutScore" runat="server" ControlToValidate="txtCutScore"
                                            Display="None" ValidationGroup="form" />
                                        <asp:RangeValidator ID="vldCutScoreRange" runat="server" ControlToValidate="txtCutScore" Display="None"
                                            MinimumValue="1" MaximumValue="30" Type="Integer" ValidationGroup="form" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="c3">
                            <asp:CheckBox ID="chkEnabled" runat="server" Checked='<%# Eval("IsEnabled")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="alt">
                        <td class="c1">
                            <%# (Server.HtmlEncode(Convert.ToString(Eval("Name")))) %>
                            <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("Id") %>' />

                            <td class="c2">
                                <table class="grid2col">
                                    <tr>
                                        <td class="left">
                                            <asp:Label runat="server" ID="lblCutScore" CssClass="hint" />
                                        </td>
                                        <td class="right">
                                            <asp:TextBox runat="server" ID="txtCutScore" Value='<%# Eval("CutScore") %>' />


                                            <asp:RequiredFieldValidator ID="vldCutScore" runat="server" ControlToValidate="txtCutScore"
                                                Display="None" ValidationGroup="form" />
                                            <asp:RangeValidator ID="vldCutScoreRange" runat="server" ControlToValidate="txtCutScore" Display="None"
                                                MinimumValue="1" MaximumValue="30" Type="Integer" ValidationGroup="form" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        <td class="c3">
                            <asp:CheckBox ID="chkEnabled" runat="server" Checked='<%# Eval("IsEnabled")%>' />
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </div>
        <div class="l note tmar5">
            <asp:Localize ID="ltrNote" runat="server" Text="<%$ Resources: TextMessages, VisitSettingsForm_Notes %>"></asp:Localize>
        </div>
        <div class="c tmar10">
            <div class="l">
                <asp:Button runat="server" ID="btnSave" CausesValidation="true" Text="Save changes"
                    UseSubmitBehavior="false" OnClientClick="if(!confirm('You are about to update Visit settings. Changes will be applied to all connected kiosks. Proceed?')){return false;}" ValidationGroup="form" />
                <asp:Button runat="server" ID="btnReset" CausesValidation="false" Text="Reset" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(".visitSettingsSelection").find(':checkbox').iphoneStyle();
    </script>
</asp:Content>
