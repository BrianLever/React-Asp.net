<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BHSReportTobacoAnswers.ascx.cs"
    Inherits="FrontDesk.Server.Web.Controls.BHSReportTobaccoAnswersControl" %>
<div class="group section tcc">
    <div class="header">
        <%= this.ScreeningSectionInfo.ScreeningSectionName %>
    </div>
    <div class="list">
        <asp:Repeater runat="server" ID="rptSectionAnswers">
            <HeaderTemplate>
                <table>
                    <tbody>
                        <asp:Repeater runat="server" ID="mainQuestionsRepeater">
                            <ItemTemplate>
                                <tr>
                                    <td class="c1" colspan="2">
                                        <asp:Literal ID="ltrQuestion" runat="server" />
                                    </td>
                                    <td class="c2">
                                        <cc:YesNoLabel ID="ynl0" runat="server" />
                                        <asp:Label runat="server" ID="ltr0" />
                                    </td>
                                    <td class="c3">
                                        <cc:YesNoLabel ID="ynl1" runat="server" />
                                        <asp:Label runat="server" ID="ltr1" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="c0 details">
                        <asp:Literal ID="ltrNo" runat="server" />
                    </td>
                    <td class="c1">
                        <asp:Literal ID="ltrQuestion" runat="server" />
                    </td>
                    <td class="c2">
                        <cc:YesNoLabel ID="ynl0" runat="server" />
                        <asp:Label runat="server" ID="ltr0" />
                    </td>
                    <td class="c3">
                        <cc:YesNoLabel ID="ynl1" runat="server" />
                        <asp:Label runat="server" ID="ltr1" />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="alt">
                    <td class="c0 details">
                        <asp:Literal ID="ltrNo" runat="server" />
                    </td>
                    <td class="c1">
                        <asp:Literal ID="ltrQuestion" runat="server" />
                    </td>
                    <td class="c2">
                        <cc:YesNoLabel ID="ynl0" runat="server" />
                        <asp:Label runat="server" ID="ltr0" />
                    </td>
                    <td class="c3">
                        <cc:YesNoLabel ID="ynl1" runat="server" />
                        <asp:Label runat="server" ID="ltr1" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>
