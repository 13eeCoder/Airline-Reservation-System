<%@ Page Title="Register" Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="FinalSDAProject.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register - Flight Booking System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/Login.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-box">
            <h2>Create Account</h2>

            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control mb-3" placeholder="Username" required="required" />
            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control mb-3" placeholder="Email" required="required" />
            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control mb-3" placeholder="First Name" required="required" />
            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control mb-3" placeholder="Last Name" required="required" />
            
            <div class="password-container">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control mb-3" placeholder="Password" required="required" />
                <div class="show-password-container">
                    <input type="checkbox" id="chkShowPassword" onclick="togglePassword()" />
                    <label for="chkShowPassword">Show Password</label>
                </div>
            </div>

            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control mb-3" placeholder="Confirm Password" required="required" />

            <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn btn-primary w-100 mb-3" OnClick="btnRegister_Click" />
            
            <asp:Label ID="lblMessage" runat="server" CssClass="error-message mb-3" Visible="false" />

            <div class="text-center mt-2">
                <span>Already have an account? </span>
                <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="Login.aspx" CssClass="text-link">
                    Login here
                </asp:HyperLink>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript">
        function togglePassword() {
            var pwdField = document.getElementById('<%= txtPassword.ClientID %>');
            var confirmPwdField = document.getElementById('<%= txtConfirmPassword.ClientID %>');
            pwdField.type = (pwdField.type === "password") ? "text" : "password";
            confirmPwdField.type = (confirmPwdField.type === "password") ? "text" : "password";
        }
    </script>
</body>
</html>