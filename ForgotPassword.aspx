<%@ Page Title="Forgot Password" Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="FinalSDAProject.ForgotPassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password - Flight Booking System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/Login.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-box">
            <h2>Reset Password</h2>

            <div id="emailSection" runat="server">
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control mb-3" placeholder="Enter your email" required="required" />
                <asp:Button ID="btnVerifyEmail" runat="server" Text="Verify Email" CssClass="btn btn-primary w-100 mb-3" OnClick="btnVerifyEmail_Click" />
            </div>

            <div id="passwordSection" runat="server" visible="false">
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control mb-3" placeholder="New Password" required="required" />
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control mb-3" placeholder="Confirm Password" required="required" />
                <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" CssClass="btn btn-primary w-100 mb-3" OnClick="btnResetPassword_Click" />
            </div>
            
            <asp:Label ID="lblMessage" runat="server" CssClass="error-message mb-3" Visible="false" />

            <div class="text-center mt-2">
                <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="Login.aspx" CssClass="text-link">
                    Back to Login
                </asp:HyperLink>
            </div>
        </div>
    </form>
</body>
</html>