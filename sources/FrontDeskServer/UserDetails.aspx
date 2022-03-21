<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserDetails.aspx.cs" Inherits="UserDetailsForm"
    MasterPageFile="~/FrontDeskMaster.master" %>

<%@ MasterType VirtualPath="~/FrontDeskMaster.master" %>
<asp:Content runat="server" ID="content" ContentPlaceHolderID="cphContent">
    <div class="c w100 red bpad10">
        <asp:Label runat="server" ID="lblBlockError" Text="User account is blocked."></asp:Label>
        <asp:Label runat="server" ID="lblLockedError" Text="User account is locked out."></asp:Label>
    </div>
    <div class="grid2col">
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblUserName" Text="User name" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="vldUserName" runat="server" Display="Dynamic" CssClass="error_message"
                ControlToValidate="txtUsername" ErrorMessage="User name is required"
                ValidationGroup="ObjectInfo" />
        </div>
        <asp:PlaceHolder ID="phPassword" runat="server">
            <div class="column left">
                <cc:FormLabel runat="server" ID="lblPassword" Text="Password" Mandatory="true"></cc:FormLabel>
            </div>
            <div class="column right">
                <asp:TextBox ID="txtPassword1" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="vldPassword1" runat="server" Display="Dynamic" CssClass="error_message"
                    ControlToValidate="txtPassword1" ErrorMessage="User password is required"
                    ValidationGroup="ObjectInfo" />
                <asp:RegularExpressionValidator ID="revNewPassword" runat="server" ControlToValidate="txtPassword1"
                    ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$" ErrorMessage="Password must contain one lowercase character, one uppercase character, one numeric character and should be at least 6 characters"
                    ValidationGroup="ObjectInfo"></asp:RegularExpressionValidator>
                <asp:CustomValidator runat="server" ID="cvldNewPassword" OnServerValidate="ValidationNewPassword"
                    ErrorMessage="Password can not contain your user name" Display="Static" 
                    ControlToValidate="txtPassword1" ValidationGroup="ObjectInfo"></asp:CustomValidator>
            </div>
            <div class="column left">
                <cc:FormLabel Text="Confirm password" runat="server" ID="lblConfirmPassword" Mandatory="true"></cc:FormLabel>
            </div>
            <div class="column right">
                <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="vldReqCmpPassword" runat="server" Display="Dynamic" CssClass="error_message"
                    ControlToValidate="txtPassword2" ErrorMessage="User confirm password is required"
                    ValidationGroup="ObjectInfo" />
                <asp:CompareValidator ID="vldCmpPassword" runat="server" CssClass="error_message"
                    Display="Dynamic" ControlToValidate="txtPassword2" ErrorMessage="Password and Confirmation not equal"
                    ControlToCompare="txtPassword1" SetFocusOnError="True" ValidationGroup="ObjectInfo">Password and confirm not equal</asp:CompareValidator>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phGroup" runat="server">
            <div class="column left">
                <cc:FormLabel runat="server" ID="lblGroup" Text="Group" Mandatory="true"></cc:FormLabel>
            </div>
            <div class="column right">
                <asp:DropDownList runat="server" ID="ddlGroup" DataSourceID="odsrRoles">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="vldGroup" runat="server" Display="Dynamic" CssClass="error_message"
                    ControlToValidate="ddlGroup" ErrorMessage="Group is required"
                    ValidationGroup="ObjectInfo" />
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phOwnerGroup" runat="server">
            <div class="column left">
                <cc:FormLabel runat="server" ID="FormLabel1" Text="Group"></cc:FormLabel>
            </div>
            <div class="column right">
                <asp:Label runat="server" ID="lblGroupValue"></asp:Label>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phBranchLocation" runat="server">
            <div class="column left">
                <cc:FormLabel runat="server" ID="lblBranch" Text="Branch location" Mandatory="true"></cc:FormLabel>
            </div>
            <div class="column right">
                <asp:DropDownList runat="server" ID="ddlBranch" DataValueField="BranchLocationID"
                    DataTextField="Name">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="vldBranch" runat="server" Display="Dynamic" CssClass="error_message"
                    ControlToValidate="ddlBranch" ErrorMessage="Branch location is required"
                    ValidationGroup="ObjectInfo" />
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phOwnerBranchLocation" runat="server">
            <div class="column left">
                <cc:FormLabel runat="server" ID="FormLabel3" Text="Branch Location"></cc:FormLabel>
            </div>
            <div class="column right">
                <asp:Label runat="server" ID="lblBranchValue"></asp:Label>
            </div>
        </asp:PlaceHolder>
        <div class="column left">
            <cc:FormLabel ID="lblFirstName" runat="server" Text="First name" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="128"></asp:TextBox>
            <asp:RequiredFieldValidator ID="vldFirstName" runat="server" Display="Dynamic" CssClass="error_message"
                ControlToValidate="txtFirstName" ErrorMessage="First name is required"
                ValidationGroup="ObjectInfo"></asp:RequiredFieldValidator>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblLastName" Text="Last name" Mandatory="true"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtLastName" runat="server" MaxLength="128"></asp:TextBox>
            <asp:RequiredFieldValidator ID="vldLastName" runat="server" Display="Dynamic" CssClass="error_message"
                ControlToValidate="txtLastName" ErrorMessage="Last name is required"
                ValidationGroup="ObjectInfo"></asp:RequiredFieldValidator>
        </div>
        <div class="column left">
            <cc:FormLabel ID="lblMiddleName" runat="server" Text="Middle name"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtMiddleName" runat="server" MaxLength="128"></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblMail" Text="E-mail" Mandatory="false"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtEmail" runat="server" MaxLength="128" />
<%--            <asp:RequiredFieldValidator ID="vldAddEmail" runat="server" ControlToValidate="txtEmail"
                Display="Static" ErrorMessage="Email is required" ValidationGroup="ObjectInfo"></asp:RequiredFieldValidator>
--%>            <asp:RegularExpressionValidator ID="vldRegEmail" runat="server" ErrorMessage="E-mail is not valid"
                ControlToValidate="txtEmail" CssClass="error_message" Display="Static" EnableClientScript="true"
                Text="E-mail is not valid" SetFocusOnError="True" ValidationGroup="ObjectInfo"></asp:RegularExpressionValidator>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblCity" Text="City"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtCity" runat="server" MaxLength="128"></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblState" Text="State"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:DropDownList runat="server" ID="ddlState" DataSourceID="odsState" DataTextField="Name"
                DataValueField="StateCode">
            </asp:DropDownList>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblAddress" Text="Address line 1"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtAddress1" runat="server" MaxLength="128"></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblAddress2" Text="Address line 2"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtAddress2" runat="server" MaxLength="128"></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblPostalCode" Text="Postal code"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtPostalCode" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator runat="server" ID="vldPostalCode" ControlToValidate="txtPostalCode"
                Display="Static" ValidationGroup="ObjectInfo" ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="lblPhone" Text="Contact phone"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox runat="server" ID="txtPhone" MaxLength="24"></asp:TextBox>
        </div>
        <div class="column left">
            <cc:FormLabel runat="server" ID="FormLabel2" Text="Comments"></cc:FormLabel>
        </div>
        <div class="column right">
            <asp:TextBox ID="txtComments" runat="server" Height="100px" TextMode="MultiLine"
                MaxLength="128"></asp:TextBox>
        </div>
        <div class="column left">
          
        </div>
        <div class="column right">
            <asp:ValidationSummary runat="server" ID="vldSummary" ValidationGroup="ObjectInfo" />
        </div>
        <div class="column left">
            &nbsp;
        </div>
        <div class="column buttons right">
            <asp:Button runat="server" ID="btnSave" Text="Add" ValidationGroup="ObjectInfo" UseSubmitBehavior="false"
                OnClick="btnAdd_Click" />
            <asp:Button runat="server" ID="btnUpdate" Text="Save Changes" ValidationGroup="ObjectInfo"
                UseSubmitBehavior="false" OnClick="btnUpdate_Click" />
            <asp:Button runat="server" ID="btnDelete" Text="Delete" ValidationGroup="ObjectInfo"
                UseSubmitBehavior="false" OnClick="btnDelete_Click" OnClientClick="if(!confirm('Are you sure that you want to delete this User?')){return false;}" />
            <asp:Button runat="server" ID="btnBlock" Text="Block" UseSubmitBehavior="false" OnClick="btnBlock_Click" />
            <asp:Button runat="server" ID="btnUnLock" Text="Unlock" UseSubmitBehavior="false"
                OnClick="btnUnlock_Click" />
            <asp:Button runat="server" ID="btnBack" Text="Back" UseSubmitBehavior="false" OnClientClick="location.href='UserList.aspx'; return false" />
        </div>
    </div>
    <%-- Get all state --%>
    <asp:ObjectDataSource runat="server" ID="odsState" TypeName="FrontDesk.State" SelectMethod="GetAllState"
        EnableCaching="true" CacheExpirationPolicy="Absolute" CacheDuration="600">
    </asp:ObjectDataSource>
    <%-- Get all branch --%>
    <asp:ObjectDataSource runat="server" ID="odsrBranch" TypeName="FrontDesk.Server.Screening.Services.BranchLocationService"
        EnableCaching="true" CacheDuration="60" CacheExpirationPolicy="Sliding" SelectMethod="GetAll">
    </asp:ObjectDataSource>
    <%-- Get branch for user--%>
    <asp:ObjectDataSource runat="server" ID="odsrUserBranch" TypeName="FrontDesk.Server.Screening.Services.BranchLocationService"
        EnableCaching="true" CacheExpirationPolicy="Absolute" CacheDuration="600" SelectMethod="GetForUserID">
        <SelectParameters>
            <asp:Parameter Name="userID"/>
        </SelectParameters>
    </asp:ObjectDataSource>
    <%-- Get all roles --%>
    <asp:ObjectDataSource runat="server" ID="odsrRoles" TypeName="FrontDesk.Server.FDUser"
         SelectMethod="GetAllRoles" EnableCaching="true" CacheExpirationPolicy="Absolute" CacheDuration="600">
    </asp:ObjectDataSource>
</asp:Content>
