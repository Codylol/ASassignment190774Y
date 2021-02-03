<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ASassignment190774Y.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            height: 33px;
        }
    </style>
    <script src ="https://www.google.com/recaptcha/api.js?render=1"></script> <%--site key--%>
</head>
<body>
    <script>
     grecaptcha.ready(function () {
         grecaptcha.execute('1', { action: 'Login' }).then(function (token) { //site key
        document.getElementById("g-recaptcha-response").value = token;
        });
     });
    </script>
    <form id="form1" runat="server">
        <input type ="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
        <div>
            <asp:Label ID="Label1" runat="server" Text="Login"></asp:Label>
        </div>
        <table class="auto-style1">
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="User ID/Email"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_logemail" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_logpw" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                
                </td>
                <td>
                   
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Login" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                  
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                   
                    <asp:Label ID="lbl_msg" runat="server" EnableViewState="False" Text="Error Msg"></asp:Label>
                </td>
                <td class="auto-style2">
                   
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_gScore" runat="server" Text="JSON Msg"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                 
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    </form>
    
</body>
</html>
