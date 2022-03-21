<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PreviewScreeningExport.ascx.cs"
    Inherits="PreviewScreeningExportControl" %>
<%@ Import Namespace="EhrInterface"  %>
<%@ Import Namespace="RPMS.Common.Models" %>

<div class="export-preview clearfix">
    <asp:Repeater ID="rptErrors" runat="server">
        <HeaderTemplate>
            <div class="errors">
                <h2>
                    Export Error:</h2>
                <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <%# Container.DataItem %></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul> </div>
        </FooterTemplate>
    </asp:Repeater>
    
     <h2 class="tpad10">Screening Results:</h2>

    <asp:Repeater ID="rptPatientRecordChanges" runat="server">
        <HeaderTemplate>
            <h2>
                Patient's record updates:</h2>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li><span class="row-number">
                <%# Container.ItemIndex + 1 %>.</span><span class="name">
                    <%# Eval("Field") %></span> <span class="before"><em>From:</em><span>
                        <%# Eval("CurrentValue") %></span></span> <span class="after"><em>To:</em><span>
                            <%# Eval("UpdateWithValue")%></span></span></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptHealthFactors" runat="server">
        <HeaderTemplate>
            <h3>
                Health Factors:</h3>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li><span class="row-number">
                <%# Container.ItemIndex + 1 %>.</span> <span class="name">
                    <%# Eval("Factor") %>
                    (<%# Eval("Code") %>)</span> <span class="comments"><em>Comments:</em><%# Eval("Comment") %>
                    </span></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptExams" runat="server">
        <HeaderTemplate>
            <h3>
                Exams:</h3>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li><span class="row-number">
                <%# Container.ItemIndex + 1 %>.</span> <span class="name">
                    <%# Eval("ExamName")%>
                    (<%# Eval("Code") %>)</span> <span class="result"><em>Result:</em><%# Eval("ResultLabel")%>
                    </span><span class="comments"><em>Comments:</em><%# Eval("Comment") %>
                    </span></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptCrisisAlerts" runat="server">
        <HeaderTemplate>
            <h3>
                Crisis Alerts:</h3>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li>Item 1</li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptSections" runat="server">
        <HeaderTemplate>
            <h3>
                Problems:</h3>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li><span class="row-number">
                <%# Container.ItemIndex + 1 %>.</span> <span class="name">
                    <%# ((ExportScreeningSectionPreview)Container.DataItem).GetName() %>, Score Level: <%# Eval("ScoreLevelLabel") %></span>
                    </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
