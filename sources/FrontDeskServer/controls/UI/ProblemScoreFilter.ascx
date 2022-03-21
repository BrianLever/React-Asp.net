<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProblemScoreFilter.ascx.cs"
    Inherits="UI.ProblemScoreFilterControl" %>


<div class="problem-score-filter">
    <ul class="line">
        <li>
            <ul class="filter-box line">
                <li>
                    <cc:FormLabel runat="server" Text="Tobacco"></cc:FormLabel>
                </li>
                <%
/*                    
  Tobacco questions:
  QuestionID    Question Text
  1             Do you use tobacco for ceremony?
  2             Do you smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?
  3         	Do you use smokeless tobacco?
  4             Do you use tobacco? (main question)
                    
*/                  %>

                <li>
                    <ul id="filter-sort-tobacco">
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_TCC_all" Text="None" Checked="true" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_TCC_1" Text="Tobacco Exposure (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_TCC2_2" Text="Tobacco Use (Smoking) (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_TCC3_3" Text="Tobacco Use (Smokeless) (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_TCC1_1" Text="Tobacco Use (Ceremony) (0)" />
                        </li>
                    </ul>
                </li>
            </ul>
        </li>
    </ul>
    <ul class="line">
        <li>
            <ul class="filter-box line">
                <li>
                    <cc:FormLabel runat="server" Text="Alcohol Use (CAGE)" />
                </li>
                <li>
                    <ul id="filter-sort-cage">
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_CAGE_all" Text="None" Checked="true" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_CAGE_1" Text="At Risk (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_CAGE_2" Text="Current Problem (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_CAGE_3" Text="Dependence (0)" />
                        </li>
                    </ul>
                </li>
            </ul>
        </li>
    </ul>
    <ul class="line">
        <li>
            <ul class="filter-box line">
                <li>
                    <cc:FormLabel runat="server" Text="Non-Medical Drug Use (DAST-10)" />
                </li>
                <li>
                    <ul id="filter-sort-dast">
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_DAST_all" Text="None" Checked="true" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_DAST_1" Text="Low (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_DAST_2" Text="Moderate (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_DAST_3" Text="Substantial (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_DAST_4" Text="Severe (0)" />
                        </li>

                    </ul>
                </li>
                <li>
                    <ul id="filter-sort-doch">
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_DOCH_all" Text="None" Checked="true" />
                        </li>
                        <li class="label">
                            <ul id="filter-sort-doch-items">
                                <asp:DataList ID="dtlDochItems" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" RepeatLayout="Flow" 
                                    OnItemDataBound="dtlDochItems_ItemDataBound"
                                   DataSourceID="odsDrugsOfChoice"
                                    >
                                    <ItemTemplate>
                                        <li class="label">
                                            <asp:CheckBox runat="server" ClientIDMode="Predictable"  ID="chk" Text='<%# string.Concat(DataBinder.Eval(Container.DataItem, "Name"), " (0)")%>' />
                                        </li>
                                    </ItemTemplate>

                                </asp:DataList>
                            </ul>
                        </li>
                    </ul>
                </li>
            </ul>
        </li>
    </ul>
    <ul class="line">
        <li>
            <ul class="filter-box line">
                <li>
                    <cc:FormLabel runat="server" Text="Anxiety (GAD-2 / GAD-7)" />
                </li>
                <li>
                    <ul id="filter-sort-gad">
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_GAD7A_all" Text="None" Checked="true" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_GAD7A_1" Text="Mild (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_GAD7A_2" Text="Moderate (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_GAD7A_3" Text="Severe (0)" />
                        </li>

                    </ul>
                </li>
            </ul>
        </li>
    </ul>
    <ul class="line">
        <li>
            <ul class="filter-box line">
                <li>
                    <cc:FormLabel runat="server" Text="Depression (PHQ-2 / PHQ-9)" />
                </li>
                <li>
                    <ul id="filter-sort-phq">
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_PHQ1_all" Text="None" Checked="true" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_PHQ1_2" Text="Mild (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_PHQ1_3" Text="Moderate (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_PHQ1_4" Text="Moderate-Severe (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_PHQ1_5" Text="Severe (0)" />
                        </li>

                    </ul>
                </li>
            </ul>
        </li>
    </ul>
    <ul class="line">
        <li>
            <ul class="filter-box line">
                <li>
                    <cc:FormLabel runat="server" Text="Suicidal Ideation (PHQ-9)" />
                </li>
                <li>
                    <ul id="filter-sort-suicidal">
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_PHQ2_all" Text="None" Checked="true" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_PHQ2_1" Text="Several Days (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_PHQ2_2" Text="More Than Half the Days (0)" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_PHQ2_3" Text="Nearly Every Day (0)" />
                        </li>
                    </ul>
                </li>
            </ul>
        </li>
    </ul>
    <ul class="line">
        <li>
            <ul class="filter-box line">
                <li>
                    <cc:FormLabel runat="server" Text="Domestic/Intimate Partner Violence (HITS)" />
                </li>
                <li>
                    <ul id="filter-sort-hits">
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_HITS_all" Text="None" Checked="true" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_HITS_1" Text="Current problem (0)" />
                        </li>

                    </ul>
                </li>
            </ul>
        </li>
    </ul>
     <ul class="line">
        <li>
            <ul class="filter-box line">
                <li>
                    <cc:FormLabel runat="server" Text="Problem Gambling (BBGS)" />
                </li>
                <li>
                    <ul id="filter-sort-bbgs">
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_BBGS_all" Text="None" Checked="true" />
                        </li>
                        <li class="label">
                            <asp:CheckBox runat="server" ID="rbt_BBGS_1" Text="Evidence of PROBLEM GAMBLING (0)" />
                        </li>

                    </ul>
                </li>
            </ul>
        </li>
    </ul>
</div>

    <asp:ObjectDataSource runat="server" ID="odsDrugsOfChoice" TypeName="FrontDesk.Server.Data.BhsVisits.LookupListsDataSource"
        SelectMethod="GetDrugOfChoice" EnableCaching="true" CacheDuration="30" OnSelected="odsDrugsOfChoice_Selected"></asp:ObjectDataSource>
<script type="text/javascript">

    function initBehavior() {
        $("#filter-sort-tobacco").multipleSelectCheckbox({});
        $("#filter-sort-cage").multipleSelectCheckbox({});
        $("#filter-sort-dast").multipleSelectCheckbox({});
        $("#filter-sort-gad").multipleSelectCheckbox({});
        $("#filter-sort-phq").multipleSelectCheckbox({});
        $("#filter-sort-suicidal").multipleSelectCheckbox({});
        $("#filter-sort-hits").multipleSelectCheckbox({});
        $("#filter-sort-doch").multipleSelectCheckbox({});
        $("#filter-sort-bbgs").multipleSelectCheckbox({});
    }

    $(document).ready(initBehavior);
    window.Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(initBehavior);
</script>
