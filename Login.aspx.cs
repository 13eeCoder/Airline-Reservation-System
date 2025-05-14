using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI;

namespace FinalSDAProject
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // ... existing connection string code ...

                // Show registration success message if redirected from registration
                if (Session["RegistrationSuccess"] != null)
                {
                    lblMessage.Text = Session["RegistrationSuccess"].ToString();
                    lblMessage.Visible = true;
                    lblMessage.CssClass = "success-message mb-3";
                    Session.Remove("RegistrationSuccess");
                }

                // Show password reset success message if redirected from password reset
                if (Session["PasswordResetSuccess"] != null)
                {
                    lblMessage.Text = Session["PasswordResetSuccess"].ToString();
                    lblMessage.Visible = true;
                    lblMessage.CssClass = "success-message mb-3";
                    Session.Remove("PasswordResetSuccess");
                }
            }
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            try
            {
                Debug.WriteLine($"Attempting login for: {username}");

                if (DatabaseHelper.ValidateUser(username, password))
                {
                    DataRow userDetails = DatabaseHelper.GetUserDetails(username);
                    if (userDetails != null)
                    {
                        Session["IsUserLogin"] = true;
                        Session["UserID"] = userDetails["UserID"];
                        Session["Username"] = userDetails["Username"];
                        Session["FirstName"] = userDetails["FirstName"];
                        Session["IsAdmin"] = Convert.ToBoolean(userDetails["IsAdmin"]);

                        Debug.WriteLine($"Login successful for: {username}");
                        RedirectBasedOnUserRole();
                    }
                }
                else
                {
                    ShowErrorMessage("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                // Show the actual error
                ShowErrorMessage($"Login error: {ex.Message}");
                Debug.WriteLine($"Login Exception: {ex.ToString()}");
            }
        }
        private void RedirectBasedOnUserRole()
        {
            if (Session["IsAdmin"] != null && (bool)Session["IsAdmin"])
            {
                Response.Redirect("AdminDashboard.aspx");
            }
            else
            {
                Response.Redirect("UserDashboard.aspx");
            }
        }

        private void ShowErrorMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
        }
    }
}