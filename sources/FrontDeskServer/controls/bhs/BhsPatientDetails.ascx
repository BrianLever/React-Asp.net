<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BhsPatientDetails.ascx.cs" Inherits="FrontDesk.Server.Web.Controls.BhsPatientDetailsControl" %>
 <div class="group noborder">
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