<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BHIDemographicsByAge.ascx.cs" Inherits="BHIDemographicsByAgeControl" %>

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
                        <asp:Label ID="ltrCategory" runat="server" />
                    </td>
                    <asp:PlaceHolder runat="server" ID="plhAgeValues" />
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="alt">
                    <td class="w50">
                        <asp:Label ID="ltrCategory" runat="server" />
                    </td>
                    <asp:PlaceHolder runat="server" ID="plhAgeValues" />
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </tbody> </table>
                    
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>
