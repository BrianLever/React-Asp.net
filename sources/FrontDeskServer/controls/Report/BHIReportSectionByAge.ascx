<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BHIReportSectionByAge.ascx.cs" Inherits="BHIReportSectionByAge" %>

<div class="group noborder section">
    <div class="indicatorReport">
        <asp:Repeater runat="server" ID="rptReportSection">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                            <th >
                                <asp:Literal runat="server" ID="lblHeader"></asp:Literal>
                            </th>
                            <%# GetAgesColumnHeaders() %>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="w50">
                        <asp:Label ID="ltrQuestion" runat="server" />
                        <p class="indicat">
                            <asp:Literal ID="ltrScoreIndicates" runat="server"></asp:Literal>
                        </p>
                    </td>
                    <asp:PlaceHolder runat="server" ID="plhAgeValues" />
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="alt">
                    <td>
                        <asp:Label ID="ltrQuestion" runat="server" />
                        <p class="indicat">
                            <asp:Literal ID="ltrScoreIndicates" runat="server"></asp:Literal>
                        </p>
                    </td>
                    <asp:PlaceHolder runat="server" ID="plhAgeValues" />
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </tbody> </table>
                    
            </FooterTemplate>
        </asp:Repeater>

        <div class="questionOnFocus">
            <asp:Repeater runat="server" ID="rptQuestionOnFocus">
                <HeaderTemplate>
                    <table>
                        <thead>
                            <tr>
                                <th class="w50">
                                    <asp:Label runat="server" CssClass="preamble" ID="lblPreamble"></asp:Label>
                                    <asp:Label runat="server" ID="lblQuestion"></asp:Label>
                                </th>
                                <%# GetAgesColumnHeaders() %>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label ID="ltrQuestion" runat="server" />
                            <p class="indicat">
                                <asp:Literal ID="ltrScoreIndicates" runat="server"></asp:Literal>
                            </p>
                        </td>
                        <asp:PlaceHolder runat="server" ID="plhAgeValues" />
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="alt">
                        <td>
                            <asp:Label ID="ltrQuestion" runat="server" />
                            <p class="indicat">
                                <asp:Literal ID="ltrScoreIndicates" runat="server"></asp:Literal>
                            </p>
                        </td>
                        <asp:PlaceHolder runat="server" ID="plhAgeValues" />
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </tbody> </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div class="copyright">
            <asp:Literal ID="ltrCopyright" runat="server"></asp:Literal>
        </div>
    </div>
</div>
