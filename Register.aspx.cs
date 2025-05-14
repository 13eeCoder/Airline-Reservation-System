using System;
using System.Diagnostics;
using System.Web.UI;

namespace FinalSDAProject
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                ShowErrorMessage("Passwords do not match");
                return;
            }

            try
            {
                bool success = DatabaseHelper.AddUser(
                    txtUsername.Text.Trim(),
                    txtPassword.Text,
                    txtEmail.Text.Trim(),
                    txtFirstName.Text.Trim(),
                    txtLastName.Text.Trim(),
                    false); // isAdmin set to false for regular users

                if (success)
                {
                    // Redirect to login page with success message
                    Session["RegistrationSuccess"] = "Registration successful! Please login.";
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    ShowErrorMessage("Registration failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Registration error: {ex.Message}");
                Debug.WriteLine($"Registration Exception: {ex}");
            }
        }

        private void ShowErrorMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
        }
    }
}