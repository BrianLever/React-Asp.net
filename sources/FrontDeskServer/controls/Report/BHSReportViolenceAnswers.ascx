<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BHSReportViolenceAnswers.ascx.cs"
    Inherits="FrontDesk.Server.Web.Controls.BHSReportViolenceAnswersControl" %>
<div class="group section hits">
    <div class="header">
        <%= this.ScreeningSectionInfo.ScreeningSectionName %>
    </div>
    <div class="list">
        <div class="main-question-group">
            <asp:Repeater runat="server" ID="mainQuestionsRepeater">
                <HeaderTemplate>
                    <table>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="">
                            <asp:Literal ID="ltrQuestion" runat="server" />
                        </td>
                        <td class="c5">
                            <span class="img">
                                <cc:YesNoLabel ID="ynl0" runat="server" />
                            </span>
                            <asp:Label runat="server" ID="ltr0" />
                        </td>
                        <td class="c6">
                            <span class="img">
                                <cc:YesNoLabel ID="ynl1" runat="server" />
                            </span>
                            <asp:Label runat="server" ID="ltr1" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody> </table>
                </FooterTemplate>

            </asp:Repeater>
        </div>
        <asp:Repeater runat="server" ID="rptSectionAnswers">
            <HeaderTemplate>
                <asp:Label runat="server" ID="lblPreamble" CssClass="preamble" />
                <table>
                    <tbody>
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
                        <span class="img">
                            <cc:YesNoLabel ID="ynl0" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr0" />
                    </td>
                    <td class="c3">
                        <span class="img">
                            <cc:YesNoLabel ID="ynl1" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr1" />
                    </td>
                    <td class="c4">
                        <span class="img">
                            <cc:YesNoLabel ID="ynl2" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr2" />
                    </td>
                    <td class="c5">
                        <span class="img">
                            <cc:YesNoLabel ID="ynl3" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr3" />
                    </td>
                    <td class="c6">
                        <span class="img">
                            <cc:YesNoLabel ID="ynl4" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr4" />
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
                        <span class="img">
                            <cc:YesNoLabel ID="ynl0" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr0" />
                    </td>
                    <td class="c3">
                        <span class="img">
                            <cc:YesNoLabel ID="ynl1" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr1" />
                    </td>
                    <td class="c4">
                        <span class="img">
                            <cc:YesNoLabel ID="ynl2" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr2" />
                    </td>
                    <td class="c5">
                        <span class="img">
                            <cc:YesNoLabel ID="ynl3" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr3" />
                    </td>
                    <td class="c6">
                        <span class="img">
                            <cc:YesNoLabel ID="ynl4" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr4" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="score">
        <table>
            <tr class="head">
                <td class="copy" rowspan="2">
                    <%= FrontDesk.Server.Resources.Copyrights.HITS_HTML %>
                </td>
                <td class="score">
                    <%= ScreeningSectionID %>
                    Score
                </td>
                <td class="risk">
                    <%= Resources.TextMessages.BHS_REPORT_INDICATES %>
                </td>
            </tr>
            <tr class="body">
                <td class="score"><% =ScreeningSectionResult.Score.ToString() %>
                </td>
                <td class="risk">
                    <% = ScreeningSectionResult.ScoreLevelDisplayHTML%>
                </td>
        </table>
    </div>
</div>

