<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BHSVisitReport.ascx.cs" Inherits="FrontDesk.Server.Web.Controls.BHSVisitReportControl"
    EnableViewState="false" %>

<div id="visit-form" class="bhsvisit">
    <h1>Visit Report</h1>
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
                    <asp:HyperLink runat="server" ID="lblRecordNo" Target="_blank" />
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

        <div id="screening-result" class="group noborder ">
            <div class="grid2col scorelevel">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Screening Date" />
                </div>
                <div class="column right">
                    <asp:TextBox ID="txtScreeningDate" runat="server" ReadOnly="true" />
                </div>
            </div>
            <div class="grid2col scorelevel">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Tobacco exposure (smoker in the home)" />
                </div>
                <div class="column right">
                    <asp:CheckBox ID="chkTobacoExposureSmokerInHomeFlag" runat="server" Enabled="false" />
                </div>
            </div>
            <div class="grid2col scorelevel">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Tobacco use (ceremony)" />
                </div>
                <div class="column right">
                    <asp:CheckBox ID="chkTobacoExposureCeremonyUseFlag" runat="server" Enabled="false" />
                </div>
            </div>
            <div class="grid2col scorelevel">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Tobacco use (smoking)" />
                </div>
                <div class="column right">
                    <asp:CheckBox ID="chkTobacoExposureSmokingFlag" runat="server" Enabled="false" />
                </div>
            </div>
            <div class="grid2col scorelevel">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Tobacco use (smokeless)" />
                </div>
                <div class="column right">
                    <asp:CheckBox ID="chkTobacoExposureSmoklessFlag" runat="server" Enabled="false" />
                </div>
            </div>
            <div class="grid3col scorelevel">
                <div class="column first">
                    <cc:FormLabel runat="server" Text="Alcohol drug use (CAGE)" />
                </div>
                <div class="column second">
                    <asp:TextBox ID="txtAlcoholUseFlagScoreLevel" runat="server" ReadOnly="true" />
                </div>
                <div class="column third">
                    <asp:TextBox ID="txtAlcoholUseFlagScoreLevelLabel" runat="server" ReadOnly="true" />

                </div>
            </div>
            <div class="grid3col scorelevel">
                <div class="column first">
                    <cc:FormLabel runat="server" Text="Non-medical drug use (DAST-10)" />
                </div>
                <div class="column second">
                    <asp:TextBox ID="txtSubstanceAbuseFlagScoreLevel" runat="server" ReadOnly="true" />
                </div>
                <div class="column third">
                    <asp:TextBox ID="txtSubstanceAbuseFlagScoreLevelLabel" runat="server" ReadOnly="true" />
                </div>
            </div>
            <div class="grid2col scorelevel">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Drug Use" />
                </div>
                <div class="column right">
                </div>

            </div>
            <div class="grid2col scorelevel drug-of-choice">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Primary" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlDrugOfChoicePrimary" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsDrugOfChoice">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="grid2col scorelevel drug-of-choice">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Secondary" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlDrugOfChoiceSecondary" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsDrugOfChoice">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="grid2col scorelevel drug-of-choice bmar20">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Tertiary" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlDrugOfChoiceTertiary" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsDrugOfChoice">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="grid3col scorelevel">
                <div class="column first">
                    <cc:FormLabel runat="server" Text="Anxiety (GAD-7)" />
                </div>
                <div class="column second">
                    <asp:TextBox ID="txtAnxietyFlagScoreLevel" runat="server" ReadOnly="true" />
                </div>
                <div class="column third">
                    <asp:TextBox ID="txtAnxietyFlagScoreLevelLabel" runat="server" ReadOnly="true" />
                </div>
            </div>

            <div class="grid3col scorelevel">
                <div class="column first">
                    <cc:FormLabel runat="server" Text="Depression (PHQ-9)" />
                </div>
                <div class="column second">
                    <asp:TextBox ID="txtDepressionFlagScoreLevel" runat="server" ReadOnly="true" />
                </div>
                <div class="column third">
                    <asp:TextBox ID="txtDepressionFlagScoreLevelLabel" runat="server" ReadOnly="true" />
                </div>
            </div>
            <div class="grid2col">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Suicidal ideation (PHQ-9)" />
                </div>
                <div class="column right">
                    <asp:TextBox ID="txtDepressionThinkOfDeathAnswer" runat="server" ReadOnly="true" />
                </div>
            </div>
            <div class="grid3col scorelevel">
                <div class="column first">
                    <cc:FormLabel runat="server" Text="Domestic/intimate partner violence (HITS)" />
                </div>
                <div class="column second">
                    <asp:TextBox ID="txtPartnerViolenceFlagScoreLevel" runat="server" ReadOnly="true" />
                </div>
                <div class="column third">
                    <asp:TextBox ID="txtPartnerViolenceFlagScoreLevelLabel" runat="server" ReadOnly="true" />
                </div>
            </div>
            <div class="grid3col scorelevel">
                <div class="column first">
                    <cc:FormLabel runat="server" Text="Problem gambling (BBGS)" />
                </div>
                <div class="column second">
                    <asp:TextBox ID="txtProblemGamblingFlagScoreLevel" runat="server" ReadOnly="true" />
                </div>
                <div class="column third">
                    <asp:TextBox ID="txtProblemGamblingFlagScoreLevelLabel" runat="server" ReadOnly="true" />
                </div>
            </div>

        </div>

        <div class="group noborder ">
            <asp:Repeater ID="rptOtherTools" runat="server">
                <HeaderTemplate>
                    <div class="grid3col">
                        <div class="column first">
                        </div>
                        <div class="column second">
                            Score or Result
                        </div>
                        <div class="column third">
                            Name of Tool
                        </div>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="grid3col">
                        <div class="column first">
                            <cc:FormLabel runat="server" ID="lblTool" Text="Other screening tool" />
                        </div>
                        <div class="column second">
                            <asp:TextBox ID="txtScoreOrResult" TextMode="MultiLine" runat="server" />
                        </div>
                        <div class="column third">
                            <asp:TextBox ID="txtNameOfTool" TextMode="MultiLine" runat="server" />

                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:Repeater>
        </div>

        <div class="group noborder ">
            <asp:Repeater ID="rptTreatmentAction" runat="server">
                <HeaderTemplate>
                    <div class="grid3col">
                        <div class="column first">
                        </div>
                        <div class="column second">
                        </div>
                        <div class="column third">
                            Description
                        </div>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="grid3col">
                        <div class="column first">
                            <cc:FormLabel runat="server" ID="lblTool" Text="Treatment action 1 (delivered)" />
                        </div>
                        <div class="column second">
                            <asp:DropDownList runat="server" ID="ddlTreatmentAction" DataValueField="ID"
                                DataTextField="Name" DataSourceID="odsTreatmentAction">
                            </asp:DropDownList>
                        </div>
                        <div class="column third">
                            <asp:TextBox ID="txtTreatmentActionDescription" runat="server" />
                            <asp:CustomValidator ID="vldTreatmentActionDescription"
                                runat="server" Display="Dynamic" Enabled="true" ValidateEmptyText="true"
                                CssClass="error_message" ClientValidationFunction="bhsVisitPageValidator.validateTreatmentDescription"
                                Text="Description is required when Treatment action is selected."
                                ControlToValidate="txtTreatmentActionDescription" ValidationGroup="ObjectInfo" />
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:Repeater>
        </div>

        <div class="group noborder fields">
            <div class="grid3col droplist-with-description">
                <div class="column first">
                    <cc:FormLabel runat="server" Text="New visit/referral recommendation" Mandatory="true" />
                </div>
                <div class="column second">
                    <asp:DropDownList runat="server" ID="ddlNewVisitRecommendation" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsNewVisitRecommendation" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vldNewVisitRecommendation" runat="server" Display="Dynamic"
                        CssClass="error_message"
                        ControlToValidate="ddlNewVisitRecommendation" ErrorMessage="'New visit/referral recommendation' is required" ValidationGroup="ObjectInfo" />
                </div>
                <div class="column third">
                    <asp:TextBox ID="txtNewVisitRecommendationDescription" runat="server" />
                    <asp:CustomValidator ID="vldNewVisitRecommendationDescription"
                        runat="server" Display="Dynamic" Enabled="true" ValidateEmptyText="true"
                        CssClass="error_message" ClientValidationFunction="bhsVisitPageValidator.validateVisitRecommendationDescription"
                        Text="Description is required when 'Other' option is selected."
                        ControlToValidate="txtNewVisitRecommendationDescription" ValidationGroup="ObjectInfo" />
                </div>
            </div>
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="New visit/referral recommendation accepted" Mandatory="true" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlNewVisitReferralRecommendationAccepted" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsNewVisitReferralRecommendationAccepted" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vldNewVisitReferralRecommendationAccepted" runat="server" Display="Dynamic"
                        CssClass="error_message"
                        ControlToValidate="ddlNewVisitReferralRecommendationAccepted" ErrorMessage="'New visit/referral recommendation accepted' is required" ValidationGroup="ObjectInfo" />
                </div>
            </div>
            <div class="grid2col droplist">
                <div class="column left">
                    <cc:FormLabel runat="server" Text="Reason recommendation NOT accepted" Mandatory="true" />
                </div>
                <div class="column right">
                    <asp:DropDownList runat="server" ID="ddlReasonNewVisitReferralRecommendationNotAccepted" DataValueField="ID"
                        DataTextField="Name" DataSourceID="odsReasonNewVisitReferralRecommendationNotAccepted" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="vldReasonNewVisitReferralRecommendationNotAccepted" runat="server" Display="Dynamic"
                        CssClass="error_message"
                        ControlToValidate="ddlReasonNewVisitReferralRecommendationNotAccepted" ErrorMessage="'Reason recommendation NOT accepted' is required" ValidationGroup="ObjectInfo" />
                </div>
            </div>
            <div class="grid2col">
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
        <div class="group noborder ">
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
<asp:ObjectDataSource runat="server" ID="odsNewVisitRecommendation" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetNewVisitRecommendation" />
<asp:ObjectDataSource runat="server" ID="odsNewVisitReferralRecommendationAccepted" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetNewVisitReferralRecommendationAccepted" />
<asp:ObjectDataSource runat="server" ID="odsReasonNewVisitReferralRecommendationNotAccepted" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetReasonNewVisitReferralRecommendationNotAccepted" />
<asp:ObjectDataSource runat="server" ID="odsDischarged" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetDischarged" />
<asp:ObjectDataSource runat="server" ID="odsTreatmentAction" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetTreatmentAction" />
<asp:ObjectDataSource runat="server" ID="odsDrugOfChoice" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
    EnableCaching="true" CacheDuration="300" CacheExpirationPolicy="Absolute" SelectMethod="GetDrugOfChoice" />



<script type="text/javascript">


    $(document).ready(function () {
        $("#visit-form").nextVisitSchedule({
            "isCompleted":<%= Page.CurFormObject.IsCompleted? "true":"false" %>
        });

        $("#visit-form").richTextNotes({
            "notesTextBoxId": "<%= txtNotes.ClientID %>"
        });

        $("#screening-result").drugOfChoiceMultistep({
            "Dast10Level": <%= Page.CurFormObject.SubstanceAbuseFlag != null? Page.CurFormObject.SubstanceAbuseFlag.ScoreLevel: 0 %>
            
        });

    });






</script>
