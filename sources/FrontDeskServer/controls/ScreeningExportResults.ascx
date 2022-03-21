<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ScreeningExportResults.ascx.cs"
    Inherits="ScreeningExportResultsControl" EnableViewState="false" %>
<div class="export-result clearfix">
    <asp:PlaceHolder runat="server" ID="phUnhandledException" Visible="False">
        <h2 class="AllFailed">The error has occured during export operation. Please check the export status below and try it again if not succeeded. <br/> Error message: <%= UnhandledExceptionMessage %></h2>
    </asp:PlaceHolder>

    <asp:Repeater ID="rptResults" runat="server">
        <HeaderTemplate>
            <h2 class="<%= ExportStatus.ToString() %>">
                <%= Header %></h2>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li class="<%# (Convert.ToBoolean(Eval("IsSuccessful"))? "ok" : "failed") %>"><span
                class="row-number">
                <%# Container.ItemIndex + 1 %>.</span> <span class="name">
                    <%# Eval("ActionName") %></span> <span class="status">
                        <%# (Convert.ToBoolean(Eval("IsSuccessful"))? "OK" : "FAILED") %></span>
                <div class="error-details" style='<%# (Eval("Fault") != null? "": "display:none") %>'>
                    <span class="info">
                        <%# (Eval("Fault") != null? Eval("Fault.InfoMessage") : null)%></span> <span class="errorDetails">
                            <%# (Eval("Fault") != null? Eval("Fault.ErrorMessage"): null)%></span>
                </div>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
