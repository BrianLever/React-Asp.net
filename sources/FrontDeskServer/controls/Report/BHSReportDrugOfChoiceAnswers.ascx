<%@ control language="C#" autoeventwireup="true" codefile="BHSReportDrugOfChoiceAnswers.ascx.cs"
    inherits="FrontDesk.Server.Web.Controls.BHSReportDrugOfChoiceAnswersControl" %>
<div class="group section drug-of-choice">
    <div class="header">
        <%= this.ScreeningSectionInfo.ScreeningSectionName%>
    </div>
    <div class="list">
        <asp:Repeater runat="server" ID="rptSectionAnswers">
            <HeaderTemplate>
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
                        <asp:Label runat="server" ID="ltr" />
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
                        
                        <asp:Label runat="server" ID="ltr" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>
