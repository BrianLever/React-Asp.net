<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModifySequrityQuestion.ascx.cs"
    Inherits="ModifySequrityQuestionCtrl" %>
    
    
<div class="grid2col">
    <div class="column left fclear">
        <cc:FormLabel runat="server" ID="lblQuestion" Text="Security Question" Mandatory="true"></cc:FormLabel>
    </div>
    <div class="column right">
        <asp:DropDownList runat="server" ID="ddlSecurityQuestion" DataSourceID="odsrSecurityQuestion"
            Width="405px" OnSelectedIndexChanged="ddlSecurityQuestion_Select" AppendDataBoundItems="true" AutoPostBack="true">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="vldSecurityQuestion" runat="server" ControlToValidate="ddlSecurityQuestion"
            ErrorMessage="Security question is required"/>
    </div>
    <asp:UpdatePanel ID="upnlOwnQuestion" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder runat="server" ID="phOwnQuestion" Visible="false">
                <div class="column left fclear">
                    &nbsp;
                </div>
                <div class="column right">
                    <asp:TextBox runat="server" ID="txtOwnQuestion" Width="400px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="vldOwnQuestion" runat="server" ControlToValidate="txtOwnQuestion"
                        ErrorMessage="Your security question is required"/>
                </div>
            </asp:PlaceHolder>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlSecurityQuestion" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="column left fclear">
        <cc:FormLabel runat="server" ID="FormLabel3" Text="Security Answer" Mandatory="true"></cc:FormLabel>
    </div>
    <div class="column right">
        <asp:TextBox runat="server" ID="txtSecurityAnswer" Width="400px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="vldSecurityAnswer" runat="server" ControlToValidate="txtSecurityAnswer"
            ErrorMessage="Security answer is required"/>
    </div>
</div>

<asp:ObjectDataSource runat="server" ID="odsrSecurityQuestion" TypeName="FrontDesk.Server.SecurityQuestion"
    SelectMethod="GetQuestions"></asp:ObjectDataSource>
