<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PatientDemographics.ascx.cs"
    Inherits="FrontDesk.Server.Web.Controls.PatientDemographicsControl"
    EnableViewState="false" %>

<div class="bhsreport bhsvisit">
    <h1>Patient Demographics</h1>
    <div class="group noborder bhs">
        <table class="patient">
            <tr class="head">
                <td class="col1">Patient Last Name
                </td>
                <td class="col2">First Name
                </td>
                <td class="col3">Middle Name
                </td>
                <td class="col4">Date of Birth
                </td>
                <td class="col5">Record Number
                </td>
            </tr>
            <tr class="body">
                <td class="col1">
                    <asp:Literal runat="server" ID="lblLastname" />
                </td>
                <td class="col2">
                    <asp:Literal runat="server" ID="lblFirstname" />
                </td>
                <td class="col3">
                    <asp:Literal runat="server" ID="lblMiddlename" />
                </td>
                <td class="col4">
                    <asp:Literal runat="server" ID="lblBirthday" />
                </td>
                <td class="col5">
                    <asp:Literal runat="server" ID="lblRecordNo" />
                </td>
            </tr>
            <tr class="head">
                <td class="col1">Mailing Address
                </td>
                <td class="col2">City
                </td>
                <td class="col3">State
                </td>
                <td class="col4">ZIP Code
                </td>
                <td class="col5">Primary Phone Number
                </td>
            </tr>
            <tr class="body">
                <td class="col1">
                    <asp:Literal runat="server" ID="lblStreetAddress" />
                </td>
                <td class="col2">
                    <asp:Literal runat="server" ID="lblCity" />
                </td>
                <td class="col3">
                    <asp:Literal runat="server" ID="lblState" />
                </td>
                <td class="col4">
                    <asp:Literal runat="server" ID="lblZipCode" />
                </td>
                <td class="col5">
                    <asp:Literal runat="server" ID="lblPhone" />
                </td>
            </tr>
        </table>
    </div>
    <asp:PlaceHolder runat="server" ID="phSecuritySection">
        <div class="group noborder">
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Race" Mandatory="true" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlRace" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsRace" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vldRace" runat="server" Display="Dynamic"
                        CssClass="error_message"
                        ControlToValidate="ddlRace" ErrorMessage="Race is required" ValidationGroup="ObjectInfo" />
                </div>
            </div>
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Gender" Mandatory="true" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlGender" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsGender" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vldGender" runat="server" Display="Dynamic"
                        CssClass="error_message"
                        ControlToValidate="ddlGender" ErrorMessage="Gender is required" ValidationGroup="ObjectInfo" />
                </div>
            </div>
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Sexual Orientation" Mandatory="true" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlSexualOrientation" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsSexualOrientation" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vldSexualOrientation" runat="server" Display="Dynamic"
                        CssClass="error_message"
                        ControlToValidate="ddlSexualOrientation" ErrorMessage="Sexual Orientation is required" ValidationGroup="ObjectInfo" />
                </div>
            </div>

            <div class="grid2col notes">
                <div class="column left">
                    <cc:FormLabel runat="server" ID="lblTribalAffiliation" Text="Tribal affiliation" Mandatory="true" />
                </div>
                <div class="column right">
                    <asp:TextBox ID="txtTribalAffiliation" MaxLength="128" runat="server" />
                    <asp:CustomValidator ID="vldTribalAffiliation" runat="server" Display="Dynamic"
                        CssClass="error_message" ValidateEmptyText="true"
                        ControlToValidate="txtTribalAffiliation" ErrorMessage="Tribal affiliation is required" ValidationGroup="ObjectInfo"
                        EnableClientScript="true" ClientValidationFunction="bhsDemographicsPageValidator.validateTribe"
                        />

                </div>
            </div>

            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Marital Status" Mandatory="true" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlMaritalStatus" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsMaritalStatus" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vldMaritalStatus" runat="server" Display="Dynamic"
                        CssClass="error_message"
                        ControlToValidate="ddlMaritalStatus" ErrorMessage="Marital Status is required" ValidationGroup="ObjectInfo" />
                </div>
            </div>
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Education Level" Mandatory="true" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlEducationLevel" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsEducationLevel" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vldEducationLevel" runat="server" Display="Dynamic"
                        CssClass="error_message"
                        ControlToValidate="ddlEducationLevel" ErrorMessage="Education Level is required" ValidationGroup="ObjectInfo" />
                </div>
            </div>
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Living &quot;on&quot; or &quot;off&quot; reservation" Mandatory="true" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlLivingOnReservation" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsLivingOnReservation" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vldLivingOnReservation" runat="server" Display="Dynamic"
                        CssClass="error_message"
                        ControlToValidate="ddlLivingOnReservation" ErrorMessage="'Living &quot;on&quot; or &quot;off&quot; reservation' is required" ValidationGroup="ObjectInfo" />
                </div>
            </div>

            <div class="grid2col notes">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="County of residence" Mandatory="true" />
                </div>
                <div class="column right">
                    <asp:TextBox ID="txtCountyofResidence" MaxLength="128" runat="server" />
                    <asp:RequiredFieldValidator ID="vldCountyofResidence" runat="server" Display="Dynamic"
                        CssClass="error_message"
                        ControlToValidate="txtCountyofResidence" ErrorMessage="County of residence is required" ValidationGroup="ObjectInfo" />

                </div>
            </div>
            <div class="grid2col notes">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Military experience" Mandatory="true" />
                </div>
                <div class="column right">
                    <asp:CheckBoxList ID="chlMilitaryExperience" DataSourceID="odsMilitaryExperience" RepeatColumns="2"
                        DataTextField="Name" DataValueField="Id"
                        runat="server" />

                </div>
            </div>

            <div class="grid2col">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Staff name" />
                </div>
                <div class="column right">
                    <asp:TextBox ID="txtStaffName" ReadOnly="true" runat="server" />
                </div>
            </div>
            <div class="grid2col">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Complete date" />
                </div>
                <div class="column right">
                    <asp:TextBox ID="txtCompleteDate" ReadOnly="true" runat="server" />
                </div>
            </div>
        </div>
    </asp:PlaceHolder>

    <script>
        $(document).ready(function () {
            $("#<%=chlMilitaryExperience.ClientID%>").militaryServiceList({

             });

             $("#<%=txtTribalAffiliation.ClientID%>").typeahead(
                 {
                     source: function (query, process) {
                         return $.getJSON("<%=ResolveUrl("~/api/typeahead.ashx")%>", {
                             "list": "tribe", "q": query
                         },
                             function (data) {
                                 return process(data);
                             });
                     },
                     minLength: 2,
                     delay: 1
                 });

             $("#<%=txtCountyofResidence.ClientID%>").typeahead(
                 {
                     source: function (query, process) {
                         return $.getJSON("<%=ResolveUrl("~/api/typeahead.ashx")%>", {
                            "list": "county", "q": query
                        },
                            function (data) {
                                return process(data);
                            });
                    },
                    minLength: 2,
                    delay: 1
                });

              $(".bhsreport.bhsvisit").bhsDemographicCtrl(
                 {
                     tribalAffiliationCtrlId: "<%= lblTribalAffiliation.ClientID %>",
                     raceDropDownListCtrlId: "<%= ddlRace.ClientID %>"
                 });
        });

    </script>

</div>
<asp:ObjectDataSource runat="server" ID="odsRace" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetRace" />
<asp:ObjectDataSource runat="server" ID="odsGender" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetGender" />
<asp:ObjectDataSource runat="server" ID="odsSexualOrientation" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetSexualOrientation" />
<asp:ObjectDataSource runat="server" ID="odsMaritalStatus" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetMaritalStatus" />
<asp:ObjectDataSource runat="server" ID="odsEducationLevel" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetEducationLevel" />
<asp:ObjectDataSource runat="server" ID="odsLivingOnReservation" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetLivingOnReservation" />
<asp:ObjectDataSource runat="server" ID="odsMilitaryExperience" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetMilitaryExperience" />




