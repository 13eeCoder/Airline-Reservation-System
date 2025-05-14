using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalSDAProject
{
    public partial class UserDashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                // Load airports dropdowns
                DataTable airports = DatabaseHelper.GetAirports();
                ddlSource.DataSource = airports;
                ddlSource.DataBind();
                ddlDestination.DataSource = airports.Copy();
                ddlDestination.DataBind();

                txtDepartureDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                mvUserDashboard.ActiveViewIndex = 0;
                SetActiveLink(btnSearchTab);

                // Show all available flights by default
                LoadAllFlights();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void btnSearchTab_Click(object sender, EventArgs e)
        {
            mvUserDashboard.ActiveViewIndex = 0;
            SetActiveLink(btnSearchTab);
        }

        protected void btnBookingsTab_Click(object sender, EventArgs e)
        {
            mvUserDashboard.ActiveViewIndex = 1;
            SetActiveLink(btnBookingsTab);
            LoadUserBookings();
        }

        protected void btnEditProfileTab_Click(object sender, EventArgs e)
        {
            mvUserDashboard.ActiveViewIndex = 2;
            SetActiveLink(btnEditProfileTab);
            LoadUserProfile();
        }

        private void SetActiveLink(LinkButton activeButton)
        {
            btnSearchTab.CssClass = "nav-link";
            btnBookingsTab.CssClass = "nav-link";
            btnEditProfileTab.CssClass = "nav-link";
            activeButton.CssClass = "nav-link active";
        }

        private void LoadAllFlights()
        {
            DataTable dt = DatabaseHelper.GetFlights(null, null, null);
            gvFlights.DataSource = dt;
            gvFlights.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime? departureDate = null;
            if (!string.IsNullOrEmpty(txtDepartureDate.Text))
            {
                departureDate = DateTime.Parse(txtDepartureDate.Text);
            }

            string source = ddlSource.SelectedItem.Text;
            string destination = ddlDestination.SelectedItem.Text;

            DataTable dt = DatabaseHelper.GetFlights(source, destination, departureDate);

            gvFlights.DataSource = dt;
            gvFlights.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "searchExecuted",
                "toastr.success('Found " + dt.Rows.Count + " flights');", true);
        }

        private void LoadUserBookings()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            DataTable dt = DatabaseHelper.GetUserBookings(userId);
            gvBookings.DataSource = dt;
            gvBookings.DataBind();
        }

        private void LoadUserProfile()
        {
            DataRow userDetails = DatabaseHelper.GetUserDetails(Session["Username"].ToString());
            if (userDetails != null)
            {
                txtEditFirstName.Text = userDetails["FirstName"].ToString();
                txtEditLastName.Text = userDetails["LastName"].ToString();
                txtEditEmail.Text = userDetails["Email"].ToString();
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            string username = Session["Username"].ToString();
            string password = txtEditPassword.Text;
            string firstName = txtEditFirstName.Text;
            string lastName = txtEditLastName.Text;
            string email = txtEditEmail.Text;

            bool success = DatabaseHelper.UpdateUser(username, password, firstName, lastName, email, false);

            if (success)
            {
                lblProfileMessage.Text = "Profile updated successfully!";
                lblProfileMessage.Visible = true;
                Session["FirstName"] = firstName; // Update session with new first name
            }
            else
            {
                lblProfileMessage.Text = "Error updating profile. Please try again.";
                lblProfileMessage.CssClass = "text-danger";
                lblProfileMessage.Visible = true;
            }
        }

        protected void gvFlights_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "BookFlight")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int flightId = Convert.ToInt32(gvFlights.DataKeys[rowIndex].Value);
                decimal price = Convert.ToDecimal(gvFlights.DataKeys[rowIndex].Values["Price"]);

                // Store flight details in session for payment page
                Session["BookingFlightId"] = flightId;
                Session["BookingPrice"] = price;

                // Redirect to payment page
                Response.Redirect("Payment.aspx");
            }
        }

        protected void gvBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelBooking")
            {
                int bookingId = Convert.ToInt32(e.CommandArgument);

                try
                {
                    bool success = DatabaseHelper.CancelBooking(bookingId);
                    if (success)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "cancelSuccess",
                            "toastr.success('Booking cancelled successfully!');", true);
                        LoadUserBookings();
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "cancelError",
                        $"toastr.error('Error: {ex.Message}');", true);
                }
            }
        }
    }
}