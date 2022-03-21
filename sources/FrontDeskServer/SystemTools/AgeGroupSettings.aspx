<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgeGroupSettings.aspx.cs" Inherits="SystemTools_AgeGroupSettings"
    MasterPageFile="~/SystemTools/SystemTools.master" %>

<%@ MasterType VirtualPath="~/SystemTools/SystemTools.master" %>
<asp:Content runat="server" ID="content" ContentPlaceHolderID="cphSystemTools">
    <div class="grid2col">
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblAgeSettings" Text="Age groups for Indicator Reports"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox runat="server" ID="txtAgeSettings" SkinID=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvAgeSettings" runat="server"
                ErrorMessage="Age groups value is required"
                ControlToValidate="txtAgeSettings" ValidationGroup="UpdateSetting"
                Display="Dynamic"></asp:RequiredFieldValidator>
          
            
        </div>
        <div class="column left">
            &nbsp;
        </div>
        <div class="column right">
            <span class="reset-button"><a href="javascript:void(0)">Reset to default</a></span> <br />
             <asp:CustomValidator ID="rgvAgeSettings" runat="server" Display="Dynamic"
                ValidationGroup="UpdateSetting" ControlToValidate="txtAgeSettings" 
                 ClientValidationFunction="rgvAgeSettings_Validate"
                 OnServerValidate="rgvAgeSettings_ServerValidate"
                 ></asp:CustomValidator>
                     
        </div>
        <div class="column left">
            &nbsp;
        </div>
        <div class="column right">
            <asp:Button runat="server" ID="btnSave" Text="Save Changes" OnClick="btnSave_Click"
                ValidationGroup="UpdateSetting" />
        </div>
        <div class="column left">
            &nbsp;
        </div>
        <div class="column w350px l tpad10">
            <asp:ValidationSummary runat="server" ID="vs" ValidationGroup="UpdateSetting"
                DisplayMode="SingleParagraph" />
        </div>
         <div class="column left">
            <cc:FormLabel runat="server" ID="lblPreviewLabel" Text="Preview Age Report groups"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:Label runat="server" ID="lblPreview" SkinID="" ></asp:Label>
        </div>
    </div>
    <script type="text/javascript">

        var opts = {
            "Default": "<%= DefaultAgeGroupValues %>",
            "Regex": "<%= ClientRegexExpression%>"
        };

        function rgvAgeSettings_Validate(sender, args) {
            var re = eval(opts.Regex);
            args.IsValid = re.test(args.Value);
        }

        $(document).ready(function () {
            $(".reset-button a").click(function () {

                $("#<%= txtAgeSettings.ClientID %>").val(opts.Default);

                return false;
            });
        });

    </script>


</asp:Content>
