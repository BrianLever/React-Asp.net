<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BHSReportAnxietyAnswers.ascx.cs"
    Inherits="FrontDesk.Server.Web.Controls.BHSReportAnxietyAnswersControl" %>
<div class="group section phq9">
    <div class="header">
        <%= _anxietyScreeningResult.ScreeningSectionName %></div>
    <div class="list">
        <asp:Repeater runat="server" ID="rptSectionAnswers">
            <HeaderTemplate>
             <asp:Label runat="server" ID="lblPreamble" CssClass="preamble" />
                <table>
                    <tbody> 
                        
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="<%# string.Format("r{0}",AnswerListIndex) %>">
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
                    <td class="c4">
                        <span class="img"><cc:YesNoLabel ID="ynl2" runat="server" /></span>
                        <asp:Label runat="server" ID="ltr2" />
                    </td>
                    <td class="c5">
                        <span class="img"><cc:YesNoLabel ID="ynl3" runat="server" /></span>
                        <asp:Label runat="server" ID="ltr3" />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="<%# string.Format("r{0}",AnswerListIndex) %>, alt">
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
                    <td class="c4">
                        <span class="img"><cc:YesNoLabel ID="ynl2" runat="server" /></span>
                        <asp:Label runat="server" ID="ltr2" />
                    </td>
                    <td class="c5">
                        <span class="img"><cc:YesNoLabel ID="ynl3" runat="server" /></span>
                        <asp:Label runat="server" ID="ltr3" />
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
                    <%= FrontDesk.Server.Resources.Copyrights.GAD7_HTML%>
                </td>
                <td class="score">
                    <%= _anxietyScreeningResult.ScreeningSectionShortName %>
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
     <!-- Last question -->
    <div class="list difficulty">
         <asp:Repeater runat="server" ID="rptDifficulty" >
            <HeaderTemplate>
                <table>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
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
                    <td class="c4">
                        <span class="img"><cc:YesNoLabel ID="ynl2" runat="server" /></span>
                        <asp:Label runat="server" ID="ltr2" />
                    </td>
                    <td class="c5">
                        <span class="img"><cc:YesNoLabel ID="ynl3" runat="server" /></span>
                        <asp:Label runat="server" ID="ltr3" />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr >
                     <td class="c0">
                        <asp:Literal ID="ltrNo" runat="server" />
                    </td>
                    <td class="c1">
                        <asp:Literal ID="ltrQuestion" runat="server" />
                    </td>
                    <td class="c2">
                        <table><tr><td><cc:YesNoLabel ID="ynl0" runat="server" /></td><td>
                        <asp:Label runat="server" ID="ltr0" /></td></tr></table>
                    </td>
                    <td class="c3">
                         <table><tr><td><cc:YesNoLabel ID="ynl1" runat="server" /></td><td>
                        <asp:Label runat="server" ID="ltr1" /></td></tr></table>
                    </td>
                    <td class="c4">
                         <table><tr><td><cc:YesNoLabel ID="ynl2" runat="server" /></td><td>
                        <asp:Label runat="server" ID="ltr2" /></td></tr></table>
                    </td>
                    <td class="c5">
                          <table><tr><td><cc:YesNoLabel ID="ynl3" runat="server" /></td><td>
                        <asp:Label runat="server" ID="ltr3" /></td></tr></table>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </tbody> </table></FooterTemplate>
        </asp:Repeater>
    </div>
</div>
