<%@ Page Title="Login" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FinalSDAProject.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login - Flight Booking System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/Login.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-box">
            <h2>Login</h2>

            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control mb-3" placeholder="Username" required="required" />
            
            <div class="password-container">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control mb-3" placeholder="Password" required="required" />
                <div class="show-password-container">
                    <input type="checkbox" id="chkShowPassword" onclick="togglePassword()" />
                    <label for="chkShowPassword">Show Password</label>
                </div>
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary w-100 mb-3" OnClick="btnLogin_Click" />
            
            <asp:Label ID="lblMessage" runat="server" CssClass="error-message mb-3" Visible="false" />

            <div class="text-center">
                <asp:HyperLink ID="lnkForgotPassword" runat="server" NavigateUrl="ForgotPassword.aspx" CssClass="text-link">
                    Forgot Password?
                </asp:HyperLink>
            </div>

            <div class="text-center mt-2">
                <span>Don't have an account? </span>
                <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="Register.aspx" CssClass="text-link">
                    Register here
                </asp:HyperLink>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript">
        function togglePassword() {
            var pwdField = document.getElementById('<%= txtPassword.ClientID %>');
            pwdField.type = (pwdField.type === "password") ? "text" : "password";
        }
    </script>
    

</body>
</html>