using System;
using System.Data;
using System.Diagnostics;
using System.Web.UI;

namespace FinalSDAProject
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        private string userEmail;
        private int userId;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnVerifyEmail_Click(object sender, EventArgs e)
        {
            userEmail = txtEmail.Text.Trim();

            try
            {
                DataRow user = GetUserByEmail(userEmail);
                if (user != null)
                {
                    userId = Convert.ToInt32(user["UserID"]);
                    emailSection.Visible = false;
                    passwordSection.Visible = true;
                    lblMessage.Visible = false;
                }
                else
                {
                    ShowErrorMessage("Email not found in our system");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
                Debug.WriteLine($"ForgotPassword Exception: {ex}");
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                ShowErrorMessage("Passwords do not match");
                return;
            }

            try
            {
                bool success = UpdateUserPassword(userId, txtNewPassword.Text);
                if (success)
                {
                    Session["PasswordResetSuccess"] = "Password reset successfully! Please login with your new password.";
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    ShowErrorMessage("Password reset failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
                Debug.WriteLine($"PasswordReset Exception: {ex}");
            }
        }

        private DataRow GetUserByEmail(string email)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                string query = "SELECT UserID FROM Users WHERE Email = @Email";
                using (var cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    conn.Open();

                    DataTable dt = new DataTable();
                    using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                    }
                }
            }
        }

        private bool UpdateUserPassword(int userId, string newPassword)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                string query = "UPDATE Users SET Password = @Password WHERE UserID = @UserID";
                using (var cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Password", newPassword);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private void ShowErrorMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
        }
    }
}