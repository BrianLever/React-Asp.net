<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FollowUp.ascx.cs"
    Inherits="FrontDesk.Server.Web.Controls.FollowUpControl"
    EnableViewState="false" %>

<%@ Register Src="~/controls/bhs/BhsPatientDetails.ascx" TagName="BhsPatientDetails" TagPrefix="uc" %>


<div id="followup-form" class="bhsvisit bhs">
    <h1>Follow-Up Report</h1>

    <uc:BhsPatientDetails ID="ucPatientDetails" runat="server" />
    <div class="group noborder">
        <div class="grid2col">
            <div class="column left">
                <cc:FormLabel runat="server" Text="Visit/referral recommendation" />
            </div>
            <div class="column right">
                <asp:TextBox ID="txtVisitReferralRecommendation" runat="server" ReadOnly="true" />
            </div>
        </div>
        <div class="grid2col">
            <div class="column left">
                <cc:FormLabel runat="server" Text="Scheduled visit date" />
            </div>
            <div class="column right">
                <asp:TextBox ID="txtVisitDate" runat="server" ReadOnly="true" />
            </div>
        </div>
        <div class="grid2col">
            <div class="column left">
                <cc:FormLabel runat="server" Text="Scheduled follow-up date" />
            </div>
            <div class="column right">
                <asp:TextBox ID="txtFollowUpDate" runat="server" ReadOnly="true" />
            </div>
        </div>
    </div>

    <asp:PlaceHolder runat="server" ID="phSecuritySection">
        <div class="group noborder fields">
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Patient attended visit" Mandatory="true" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlPatientAttendedVisit" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsPatientAttednedVisit" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vldPatientAttendedVisit" runat="server" Display="Dynamic"
                        CssClass="error_message"
                        ControlToValidate="ddlPatientAttendedVisit" ErrorMessage="'Patient attended visit' is required" ValidationGroup="ObjectInfo" />
                </div>
            </div>
            <div class="grid2col date">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Follow-up contact date" Mandatory="false" />
                </div>
                <div class="column right">
                    <cc:RichDatePicker runat="server" ID="dtFollowUpContactDate" />
                </div>
            </div>
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Follow-up contact outcome" Mandatory="false" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlFollowUpContactOutcome" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsFollowUpContactOutcome" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vldFollowUpContactOutcome" runat="server" Display="Dynamic"
                        CssClass="error_message"
                        ControlToValidate="ddlFollowUpContactOutcome" ErrorMessage="'Follow-up contact outcome' is required" ValidationGroup="ObjectInfo" />
                </div>
            </div>
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="New visit/referral recommendation" Mandatory="false" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlNewVisitRecommendation" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsNewVisitRecommendation" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </div>

            </div>
            <div class="grid2col droplist notes">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="New visit/referral recommendation (description)" Mandatory="false" />
                </div>
                <div class="column right">
                    <asp:TextBox ID="txtNewVisitRecommendationDescription" TextMode="MultiLine" Rows="3" runat="server" />
                </div>
            </div>
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="New visit/referral recommendation accepted" Mandatory="false" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlNewVisitReferralRecommendationAccepted" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsNewVisitReferralRecommendationAccepted" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Reason recommendation NOT accepted" Mandatory="false" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlReasonNewVisitReferralRecommendationNotAccepted" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsReasonNewVisitReferralRecommendationNotAccepted" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="New visit date" />
                </div>
                <div class="column right">
                    <cc:RichDatePicker runat="server" ID="dtNewVisitDate" />
                    <asp:RequiredFieldValidator ID="vldNewVisitDate" runat="server" Display="Dynamic" Enabled="false"
                        CssClass="error_message"
                        ControlToValidate="dtNewVisitDate" ErrorMessage="'New visit date' cannot be empty when follow-up exists." ValidationGroup="ObjectInfo" />

                </div>
            </div>
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Discharged" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlDischarged" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsDischarged" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="grid2col">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Create follow-up?" />
                </div>
                <div class="column right">
                    <asp:DropDownList ID="ddlThirtyDatyFollowUpFlag" runat="server">
                        <asp:ListItem Value="0" Text="No" />
                        <asp:ListItem Value="1" Text="Yes" />

                    </asp:DropDownList>
                </div>
            </div>
            <div class="grid2col hidden" id="follow-up-date">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Follow-up date" />

                </div>
                <div class="column right">
                    <cc:RichDatePicker runat="server" ID="dtFollowUpDate" />
                    <span class="reset-button"><a href="javascript:void(0)">Reset follow-up date to default 30 days</a></span>

                </div>
            </div>
            <div class="grid2col notes">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Notes" />
                </div>
                <div class="column right">
                    <asp:HiddenField ID="txtNotes" runat="server" />
                    <div id="editor-notes">
                    </div>
                </div>
            </div>

        </div>
        <div class="group noborder">
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


</div>
<asp:ObjectDataSource runat="server" ID="odsPatientAttednedVisit" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetPatientAttendedVisit" />
<asp:ObjectDataSource runat="server" ID="odsFollowUpContactOutcome" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetFollowUpContactOutcome" />
<asp:ObjectDataSource runat="server" ID="odsNewVisitRecommendation" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetNewVisitRecommendation" />
<asp:ObjectDataSource runat="server" ID="odsNewVisitReferralRecommendationAccepted" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetNewVisitReferralRecommendationAccepted" />
<asp:ObjectDataSource runat="server" ID="odsReasonNewVisitReferralRecommendationNotAccepted" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetReasonNewVisitReferralRecommendationNotAccepted" />
<asp:ObjectDataSource runat="server" ID="odsDischarged" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetDischarged" />

<script type="text/javascript">
    $(document).ready(function () {
        $("#followup-form").nextVisitSchedule({
            "isCompleted":<%= Page.CurFormObject.IsCompleted? "true":"false" %>
        });

        $("#visit-form").richTextNotes({
            "notesTextBoxId": "<%= txtNotes.ClientID %>"
        });
    });
</script>


