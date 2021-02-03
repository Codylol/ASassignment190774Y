<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="ASassignment190774Y.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="User Profile"></asp:Label>
            <br />
            <table class="auto-style1">
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Email"></asp:Label>
                        </td>
                    <td>
                        <asp:Label ID="lbl_email" runat="server" Text="email"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_msg" runat="server" Text="Message"></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btn_logout" runat="server" OnClick="btn_logout_Click" Text="Logout" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
