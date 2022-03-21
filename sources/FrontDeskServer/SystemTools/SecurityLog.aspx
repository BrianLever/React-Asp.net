<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SecurityLog.aspx.cs" Inherits="SystemTools_SecurityLog" 
    MasterPageFile="~/SystemTools/SystemTools.master" EnableEventValidation="false" %>
    <%@ MasterType VirtualPath="~/SystemTools/SystemTools.master" %>
    
<asp:Content ID="cnt" runat="server" ContentPlaceHolderID="cphSystemTools">
	<div class="content-right-margin">
    <asp:UpdatePanel ID="upFilter" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <ul class="input_form sec_rep">
                <li>
                    <cc:FormLabel runat="server" ID="lblCaterory" Text="Category"></cc:FormLabel>
                    <asp:DropDownList ID="ddlCategory" runat="server" Width="180px" DataTextField="Description" DataValueField="ID" AppendDataBoundItems="true"></asp:DropDownList>
                </li>
                <li>
                    <cc:FormLabel runat="server" ID="lblEvent" Text="Event"></cc:FormLabel>
                    <asp:DropDownList ID="ddlEvent" runat="server" Width="300px" DataTextField="Description" DataValueField="ID" AppendDataBoundItems="false"></asp:DropDownList>
                </li>
                <li>
                    <cc:FormLabel runat="server" ID="lblDateRange" Text="Date"></cc:FormLabel>
                    <div class="date_range">
                        <cc:RichDatePicker runat="server" ID="dpStartDate" /><span>to</span>
                        <cc:RichDatePicker runat="server" ID="dpEndDate" />
                    </div>
                </li>
                <li class="buttons">
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnClear" UseSubmitBehavior="false" Text="Clear" OnClick="btnClear_Click" />
                </li>
            </ul>
        </ContentTemplate>
    </asp:UpdatePanel>                    

    <asp:UpdatePanel ID="up" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:GridView ID="gvLog" runat="server" DataSourceID="odsLog" EnableViewState="true"
                AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" PageSize="15" >
                <EmptyDataTemplate>
                    <div class="c w100">No security log items found.</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:HyperLinkField HeaderText="User" DataNavigateUrlFormatString="~/UserDetails.aspx?id={0}" Target="_blank"
                        DataNavigateUrlFields="PKID" HeaderStyle-CssClass="l w25" ItemStyle-CssClass="l linked" DataTextField="FullName" SortExpression="FullName" />
                        
                    <asp:BoundField HeaderText="Date" DataField="LogDate" SortExpression="LogDate" HeaderStyle-CssClass="w25 l"
                        ItemStyle-CssClass="l" DataFormatString="{0:MM/dd/yyyy HH:mm:ss zzz}" />
                                          
                    <asp:BoundField HeaderText="Event" DataField="HTMLDescription" SortExpression="se.Description"
                        HeaderStyle-CssClass="w50 l" ItemStyle-CssClass="l linked" HtmlEncode="false" />
                </Columns>
            </asp:GridView>
            
            <div class="tmar10 l w100 bpad15">
                <cc:ExcelExportButton runat="server" ID="btnExport" CausesValidation="false" Text="Export to Excel" 
                    UseSubmitBehavior="false" FileName="Security Log" CssClass="btn" OnClick="Export_Click"  />
             </div>
            
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    
    <asp:ObjectDataSource ID="odsLog" runat="server" TypeName="FrontDesk.Server.Logging.SecurityLog"
        EnablePaging="true" SelectMethod="GetReport" SelectCountMethod="GetReportItemsCount" SortParameterName="orderBy">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlCategory" Type="Int32" PropertyName="SelectedValue" Name="categoryID" />
            <asp:ControlParameter ControlID="ddlEvent" Type="Int32" PropertyName="SelectedValue" Name="eventID" />
            <asp:ControlParameter ControlID="dpStartDate" Type="DateTime" PropertyName="SelectedDate" ConvertEmptyStringToNull="true" Name="startDate" />
            <asp:ControlParameter ControlID="dpEndDate" Type="DateTime" PropertyName="SelectedDate" ConvertEmptyStringToNull="true" Name="endDate" />
            <asp:Parameter Name="isSA" Type="Boolean" DefaultValue="false" />
        </SelectParameters>
    </asp:ObjectDataSource>
</div>
<script type="text/javascript">

    var ddlEvents = null;

    function PopulateEvents(target) {

        ddlEvents = document.getElementById("<%= ddlEvent.ClientID %>");
        
        ddlEvents.disabled = true;
        
        if (target.selectedIndex <= 0) {
            ddlEvents.options.length = 0;
            ddlEvents.options[0] = new Option("<%= Resources.TextMessages.DropDown_NotSelectedFilter %>", "0");
            return;
        }
        
        ddlEvents.options.length = 0;
        ddlEvents.options[0] = new Option("Loading...");

        PageMethods.GetEvents(target.options[target.selectedIndex].value, OnEventsLoadingComplete, OnEvenetsLoadingError);
    }

    function OnEventsLoadingComplete(result) {
        ddlEvents.options.length = 0;
        ddlEvents.options[0] = new Option("<%= Resources.TextMessages.DropDown_NotSelectedFilter %>", "0");
        for (var i = 0; i < result.length; i++) {
            ddlEvents.options[i + 1] = new Option(result[i].Description, result[i].ID);
        }

        ddlEvents.disabled = false;
    }

    function OnEvenetsLoadingError() {
        ddlEvents.options[0] = new Option("<%= Resources.TextMessages.DropDown_NotSelectedFilter %>", "0");
        alert("<%= FrontDesk.Common.Messages.CustomError.GetInternalErrorMessage() %>");
    }

</script>


</asp:Content>