<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BHSReportAlcoholAnswers.ascx.cs"
    Inherits="FrontDesk.Server.Web.Controls.BHSReportAlcoholAnswersControl" %>
<div class="group section cage">
    <div class="header">
        <%= this.ScreeningSectionInfo.ScreeningSectionName%></div>
    <div class="list">
        <asp:Repeater runat="server" ID="rptSectionAnswers">
            <HeaderTemplate>
                <table>
                    <tbody>
                        <asp:Repeater runat="server" ID="mainQuestionsRepeater">
                            <ItemTemplate>
                                 <tr>
                                    <td class="c01" colspan="2">
                                        <asp:Literal ID="ltrQuestion" runat="server" />
                                    </td>
                                    <td class="c2">
                                        <span class="img"><cc:YesNoLabel ID="ynl0" runat="server" /></span>
                                        <asp:Label runat="server" ID="ltr0" />
                                    </td>
                                    <td class="c3">
                                        <span class="img"><cc:YesNoLabel ID="ynl1" runat="server" /></span>
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
                        <span class="img"><cc:YesNoLabel ID="ynl0" runat="server" /></span>
                        <asp:Label runat="server" ID="ltr0" />
                    </td>
                    <td class="c3">
                        <span class="img"><cc:YesNoLabel ID="ynl1" runat="server" /></span>
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
                        <span class="img"><cc:YesNoLabel ID="ynl0" runat="server" /></span>
                        <asp:Label runat="server" ID="ltr0" />
                    </td>
                    <td class="c3">
                        <span class="img"><cc:YesNoLabel ID="ynl1" runat="server" /></span>
                        <asp:Label runat="server" ID="ltr1" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </tbody> </table></FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="score">
        <table>
            <tr class="head">
                <td class="copy" rowspan="2">
                    <%= FrontDesk.Server.Resources.Copyrights.CAGE_HTML %>
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
            </tr>
        </table>
    </div>
</div>
