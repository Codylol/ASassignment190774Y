<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="ASassignment190774Y.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
    <style type="text/css">
        .auto-style1 {
            height: 32px;
        }
    </style>
    <script type="text/javascript">
        function pwvalidation() { //validating pw function
            var str = document.getElementById('<%=tb_pw.ClientID %>').value; //Extracting the data from the tb

            if (str.length < 8) { //checking if the lenght of the input is less than 8
                document.getElementById("lbl_pwchecker").innerHTML = "Length of Password must be at least 8 characters";
                document.getElementById("lbl_pwchecker").style.color = "Red";
                return ("too short")
            }
            else if (str.search(/[0-9]/) == -1) { //validating the pw if it has at least 1 numeral
                document.getElementById("lbl_pwchecker").innerHTML = "Password requires at least 1 number";
                document.getElementById("lbl_pwchecker").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[a-z]/) == -1) { //validating the pw if it has at least one lowercase character
                document.getElementById("lbl_pwchecker").innerHTML = "Password requires a lowercase character";
                document.getElementById("lbl_pwchecker").style.color = "Red";
                return ("no_lowercase");
            }
            else if (str.search(/[A-Z]/) == -1) { //validating the pw if it has at least one uppercase character
                document.getElementById("lbl_pwchecker").innerHTML = "Password requires a uppercase character";
                document.getElementById("lbl_pwchecker").style.color = "Red";
                return ("no_uppercase");
            }            else if (str.search(/[^a-zA-Z0-9]/) == -1) { //validating the pw if it has at least one special character, how it checks is it ^ negates the lower/upper case characters and numbers
                document.getElementById("lbl_pwchecker").innerHTML = "Password requires a special character";
                document.getElementById("lbl_pwchecker").style.color = "Red";
                return ("no_specialcharacter");
            }
            document.getElementById("lbl_pwchecker").innerHTML = "Strong Password"; //if condition is fulfilled, this will be shown
            document.getElementById("lbl_pwchecker").style.color = "Green";
        }
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Registration"></asp:Label>
        </div>
        <table class="auto-style1">
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label3" runat="server" Text="First Name"></asp:Label>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="tb_fname" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style1"></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Last Name"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_lname" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Credit Card number"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_cc" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
             <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Month of Expiry"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_expiry" runat="server" TextMode="Date"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
             <tr>
                <td>
                    <asp:Label ID="Label9" runat="server" Text="CVV"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_cvv" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Email address"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_email" runat="server" TextMode="Email"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_pw" runat="server" onkeyup ="javascript:pwvalidation()" TextMode="Password"></asp:TextBox> <%--implemting the function into the tb--%>
                    <asp:Button ID="Button2" runat="server" OnClick="btn_checkpw" Text="Check Password" />
                </td>
                <td>
                    <asp:Label ID="lbl_pwchecker" runat="server" Text="pwchecker"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Date of Birth"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_dob" runat="server" TextMode="Date"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Sign Up" OnClick="Button1_Click" />
                    <asp:Label ID="lbl_success" runat="server" Text="Label"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="btn_login" runat="server" OnClick="btn_login_Click" Text="Click here to Login" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            
        </table>
    </form>
</body>
</html>
