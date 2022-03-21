<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ScreenTimeLogReportSection.ascx.cs" Inherits="ScreenTimeLogReportSectionControl" %>

<div class="group noborder section">
    <div class="indicatorReport">
        <asp:Repeater runat="server" ID="rptReportSection">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                            <th class="w55"><asp:Localize ID="l1" runat="server" Text='<%$ Resources: Labels, ScreenTimeLog_ScreeningMeasure_Label %>' /></th>
                            <th class="w15"><asp:Localize ID="l2" runat="server" Text='<%$ Resources: Labels, ScreenTimeLog_NumberOfReports_Label %>' /></th>
                            <th class="w15"><asp:Localize ID="l3" runat="server" Text='<%$ Resources: Labels, ScreenTimeLog_TotalTime_Label %>' /></th>
                            <th class="w15"><asp:Localize ID="l4" runat="server" Text='<%$ Resources: Labels, ScreenTimeLog_AverageTime_Label %>' /></th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label ID="ltrCategory" runat="server" />
                    </td>
                    <td class="w15">
                        <asp:Label ID="lblCount" runat="server" />
                    </td>
                    <td class="w15">
                        <asp:Label ID="lblTotalTime" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblAvgTime" runat="server" />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="alt">
                    <td>
                        <asp:Label ID="ltrCategory" runat="server" />
                    </td>
                    <td class="w15">
                        <asp:Label ID="lblCount" runat="server" />
                    </td>
                    <td class="w15">
                        <asp:Label ID="lblTotalTime" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblAvgTime" runat="server" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </tbody>
                <tfoot>
                    <tr class="summary-footer">
                        <td><asp:Localize ID="l5" runat="server" Text='<%$ Resources: Labels, ScreenTimeLog_Totals_Label %>' /></td>
                        <td class="w15">
                            <asp:Label ID="lblCount" runat="server" />
                        </td>
                        <td class="w15">
                            <asp:Label ID="lblTotalTime" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblAvgTime" runat="server" />
                        </td>
                    </tr>
                </tfoot>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>
