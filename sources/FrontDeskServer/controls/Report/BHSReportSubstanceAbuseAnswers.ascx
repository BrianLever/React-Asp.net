<%@ control language="C#" autoeventwireup="true" codefile="BHSReportSubstanceAbuseAnswers.ascx.cs"
    inherits="FrontDesk.Server.Web.Controls.BHSReportSubstanceAbuseAnswersControl" %>
<div class="group section tcc">
    <div class="header">
        <%= this.ScreeningSectionInfo.ScreeningSectionName%>
    </div>
    <div class="list">
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
                            <cc:yesnolabel id="ynl0" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr0" />
                    </td>
                    <td class="c3">
                        <span class="img">
                            <cc:yesnolabel id="ynl1" runat="server" />
                        </span>
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
                        <span class="img">
                            <cc:yesnolabel id="ynl0" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr0" />
                    </td>
                    <td class="c3">
                        <span class="img">
                            <cc:yesnolabel id="ynl1" runat="server" />
                        </span>
                        <asp:Label runat="server" ID="ltr1" />
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
                    <%= global::FrontDesk.Server.Resources.Copyrights.DAST10_HTML %>
                </td>
                <td class="score">
                    <%= ScreeningInfo.FindSectionByID(ScreeningSectionID).ScreeningSectionShortName %>
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
