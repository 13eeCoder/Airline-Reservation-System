using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalSDAProject
{
    public partial class AdminDashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mvDashboard.ActiveViewIndex = 0;
                SetActiveLink(btnDashboard);

                // Load dropdown data
                LoadAirlineDropdown();
                LoadAirportDropdowns();
                LoadUserDropdown();
                LoadFlightDropdown();
            }
        }

        private void LoadAirlineDropdown()
        {
            DataTable dt = DatabaseHelper.GetAllAirlines();
            ddlAirlineID.DataSource = dt;
            ddlAirlineID.DataBind();
        }

        private void LoadAirportDropdowns()
        {
            DataTable dt = DatabaseHelper.GetAirports();
            ddlSourceAirportID.DataSource = dt;
            ddlSourceAirportID.DataBind();
            ddlDestinationAirportID.DataSource = dt.Copy();
            ddlDestinationAirportID.DataBind();
        }

        private void LoadUserDropdown()
        {
            DataTable dt = DatabaseHelper.GetAllUsers();
            ddlUserID.DataSource = dt;
            ddlUserID.DataBind();
        }

        private void LoadFlightDropdown()
        {
            DataTable dt = DatabaseHelper.GetAllFlights();
            ddlFlightID.DataSource = dt;
            ddlFlightID.DataBind();
        }

        protected void btnDashboard_Click(object sender, EventArgs e)
        {
            mvDashboard.ActiveViewIndex = 0;
            SetActiveLink(btnDashboard);
        }

        protected void btnUsers_Click(object sender, EventArgs e)
        {
            mvDashboard.ActiveViewIndex = 1;
            SetActiveLink(btnUsers);
        }

        protected void btnFlights_Click(object sender, EventArgs e)
        {
            mvDashboard.ActiveViewIndex = 2;
            SetActiveLink(btnFlights);
        }

        protected void btnBookings_Click(object sender, EventArgs e)
        {
            mvDashboard.ActiveViewIndex = 3;
            SetActiveLink(btnBookings);
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                bool success = DatabaseHelper.AddUser(
                    txtUsername.Text.Trim(),
                    txtPassword.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtFirstName.Text.Trim(),
                    txtLastName.Text.Trim(),
                    chkIsAdmin.Checked
                );

                if (success)
                {
                    lblUserMessage.Text = $"User '{txtUsername.Text}' added successfully!";
                    lblUserMessage.CssClass = "text-success fw-bold";
                    // Clear all fields
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    txtEmail.Text = "";
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    chkIsAdmin.Checked = false;

                    // Refresh user dropdown
                    LoadUserDropdown();
                }
            }
            catch (Exception ex)
            {
                lblUserMessage.Text = $"Error adding user: {ex.Message}";
                lblUserMessage.CssClass = "text-danger fw-bold";
            }
        }

        protected void btnEditUser_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEditUsername.Text))
            {
                try
                {
                    string username = txtEditUsername.Text.Trim();
                    string password = txtEditPassword.Text.Trim();
                    string firstName = txtEditFirstName.Text.Trim();
                    string lastName = txtEditLastName.Text.Trim();
                    string email = txtEditEmail.Text.Trim();
                    bool isAdmin = chkEditIsAdmin.Checked;

                    if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName) &&
                        string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password) && !isAdmin)
                    {
                        lblUserMessage.Text = "Please enter at least one field to update";
                        lblUserMessage.CssClass = "text-danger fw-bold";
                        return;
                    }

                    bool success = DatabaseHelper.UpdateUser(username, password, firstName, lastName, email, isAdmin);

                    if (success)
                    {
                        lblUserMessage.Text = $"User '{username}' updated successfully!";
                        lblUserMessage.CssClass = "text-success fw-bold";
                        txtEditUsername.Text = "";
                        txtEditPassword.Text = "";
                        txtEditFirstName.Text = "";
                        txtEditLastName.Text = "";
                        txtEditEmail.Text = "";
                        chkEditIsAdmin.Checked = false;
                        LoadAllUsers();
                    }
                    else
                    {
                        lblUserMessage.Text = $"User '{username}' not found!";
                        lblUserMessage.CssClass = "text-danger fw-bold";
                    }
                }
                catch (Exception ex)
                {
                    lblUserMessage.Text = $"Error updating user: {ex.Message}";
                    lblUserMessage.CssClass = "text-danger fw-bold";
                }
            }
            else
            {
                lblUserMessage.Text = "Please enter a Username to update";
                lblUserMessage.CssClass = "text-danger fw-bold";
            }
        }
        protected void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDeleteUserId.Text))
            {
                try
                {
                    int userId = int.Parse(txtDeleteUserId.Text);
                    bool success = DatabaseHelper.DeleteUser(userId);

                    if (success)
                    {
                        lblUserMessage.Text = $"User with ID {userId} deleted successfully!";
                        lblUserMessage.CssClass = "text-success fw-bold";
                        txtDeleteUserId.Text = "";
                        LoadAllUsers();
                        LoadUserDropdown();
                    }
                    else
                    {
                        lblUserMessage.Text = $"User with ID {userId} not found!";
                        lblUserMessage.CssClass = "text-danger fw-bold";
                    }
                }
                catch (Exception ex)
                {
                    lblUserMessage.Text = $"Error deleting user: {ex.Message}";
                    lblUserMessage.CssClass = "text-danger fw-bold";
                }
            }
            else
            {
                lblUserMessage.Text = "Please enter a User ID to delete";
                lblUserMessage.CssClass = "text-danger fw-bold";
            }
        }

        protected void btnViewUsers_Click(object sender, EventArgs e)
        {
            lblLoadingUsers.Text = "Loading All Users...";
            lblLoadingUsers.Visible = true;
            gvUsers.Visible = false;

            ScriptManager.RegisterStartupScript(this, GetType(), "loadUsers",
                "setTimeout(function() { " +
                "   var btn = document.getElementById('" + btnHiddenLoadUsers.ClientID + "'); " +
                "   btn.click(); " +
                "}, 3000);", true);
        }

        protected void btnAddFlight_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime departureTime = DateTime.Parse(txtDepartureTime.Text);
                DateTime arrivalTime = DateTime.Parse(txtArrivalTime.Text);
                int duration = (int)(arrivalTime - departureTime).TotalMinutes;

                bool success = DatabaseHelper.AddFlight(
                    txtFlightNumber.Text.Trim(),
                    int.Parse(ddlAirlineID.SelectedValue),
                    int.Parse(ddlSourceAirportID.SelectedValue),
                    int.Parse(ddlDestinationAirportID.SelectedValue),
                    departureTime,
                    arrivalTime,
                    decimal.Parse(txtPrice.Text),
                    int.Parse(txtTotalSeats.Text),
                    int.Parse(txtAvailableSeats.Text),
                    duration
                );

                if (success)
                {
                    lblFlightMessage.Text = $"Flight '{txtFlightNumber.Text}' added successfully!";
                    lblFlightMessage.CssClass = "text-success fw-bold";
                    // Clear all fields
                    txtFlightNumber.Text = "";
                    txtDepartureTime.Text = "";
                    txtArrivalTime.Text = "";
                    txtPrice.Text = "";
                    txtTotalSeats.Text = "";
                    txtAvailableSeats.Text = "";
                    txtDuration.Text = "";

                    // Refresh flight dropdown
                    LoadFlightDropdown();
                }
            }
            catch (Exception ex)
            {
                lblFlightMessage.Text = $"Error adding flight: {ex.Message}";
                lblFlightMessage.CssClass = "text-danger fw-bold";
            }
        }

        protected void btnDeleteFlight_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDeleteFlightId.Text))
            {
                try
                {
                    int flightId = int.Parse(txtDeleteFlightId.Text);
                    bool success = DatabaseHelper.DeleteFlight(flightId);

                    if (success)
                    {
                        lblFlightMessage.Text = $"Flight with ID {flightId} deleted successfully!";
                        lblFlightMessage.CssClass = "text-success fw-bold";
                        txtDeleteFlightId.Text = "";
                        LoadAllFlights();
                        LoadFlightDropdown();
                    }
                    else
                    {
                        lblFlightMessage.Text = $"Flight with ID {flightId} not found!";
                        lblFlightMessage.CssClass = "text-danger fw-bold";
                    }
                }
                catch (Exception ex)
                {
                    lblFlightMessage.Text = $"Error deleting flight: {ex.Message}";
                    lblFlightMessage.CssClass = "text-danger fw-bold";
                }
            }
            else
            {
                lblFlightMessage.Text = "Please enter a Flight ID to delete";
                lblFlightMessage.CssClass = "text-danger fw-bold";
            }
        }

        protected void btnViewFlights_Click(object sender, EventArgs e)
        {
            lblLoadingFlights.Text = "Loading Flights...";
            lblLoadingFlights.Visible = true;
            gvFlights.Visible = false;

            ScriptManager.RegisterStartupScript(this, GetType(), "loadFlights",
                "setTimeout(function() { " +
                "   var btn = document.getElementById('" + btnHiddenLoadFlights.ClientID + "'); " +
                "   btn.click(); " +
                "}, 3000);", true);
        }

        protected void btnAddBooking_Click(object sender, EventArgs e)
        {
            try
            {
                bool success = DatabaseHelper.AddBooking(
                    int.Parse(ddlUserID.SelectedValue),
                    int.Parse(ddlFlightID.SelectedValue),
                    DateTime.Parse(txtBookingDate.Text),
                    int.Parse(txtPassengers.Text),
                    decimal.Parse(txtTotalPrice.Text),
                    ddlStatus.SelectedValue
                );

                if (success)
                {
                    lblBookingMessage.Text = "Booking added successfully!";
                    lblBookingMessage.CssClass = "text-success fw-bold";
                    // Clear fields
                    txtPassengers.Text = "";
                    txtTotalPrice.Text = "";
                    txtBookingDate.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblBookingMessage.Text = $"Error adding booking: {ex.Message}";
                lblBookingMessage.CssClass = "text-danger fw-bold";
            }
        }

        protected void btnSearchBooking_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBookingID.Text))
            {
                lblBookingMessage.Text = $"Showing details for booking ID: {txtBookingID.Text}";
                lblBookingMessage.CssClass = "text-info fw-bold";
            }
        }

        protected void btnDeleteBooking_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDeleteBookingId.Text))
            {
                try
                {
                    int bookingId = int.Parse(txtDeleteBookingId.Text);
                    bool success = DatabaseHelper.DeleteBooking(bookingId);

                    if (success)
                    {
                        lblBookingMessage.Text = $"Booking with ID {bookingId} deleted successfully!";
                        lblBookingMessage.CssClass = "text-success fw-bold";
                        txtDeleteBookingId.Text = "";
                        LoadAllBookings();
                    }
                    else
                    {
                        lblBookingMessage.Text = $"Booking with ID {bookingId} not found!";
                        lblBookingMessage.CssClass = "text-danger fw-bold";
                    }
                }
                catch (Exception ex)
                {
                    lblBookingMessage.Text = $"Error deleting booking: {ex.Message}";
                    lblBookingMessage.CssClass = "text-danger fw-bold";
                }
            }
            else
            {
                lblBookingMessage.Text = "Please enter a Booking ID to delete";
                lblBookingMessage.CssClass = "text-danger fw-bold";
            }
        }

        protected void btnViewBookings_Click(object sender, EventArgs e)
        {
            lblLoadingBookings.Text = "Loading Bookings...";
            lblLoadingBookings.Visible = true;
            gvBookings.Visible = false;

            ScriptManager.RegisterStartupScript(this, GetType(), "loadBookings",
                "setTimeout(function() { " +
                "   var btn = document.getElementById('" + btnHiddenLoadBookings.ClientID + "'); " +
                "   btn.click(); " +
                "}, 3000);", true);
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        private void SetActiveLink(LinkButton activeButton)
        {
            btnDashboard.CssClass = "nav-link";
            btnUsers.CssClass = "nav-link";
            btnFlights.CssClass = "nav-link";
            btnBookings.CssClass = "nav-link";

            activeButton.CssClass = "nav-link active";
        }

        protected void LoadAllUsers(object sender, EventArgs e)
        {
            DataTable dt = DatabaseHelper.GetAllUsers();
            gvUsers.DataSource = dt;
            gvUsers.DataBind();
            lblUserMessage.Text = $"Loaded {dt.Rows.Count} users";
            lblUserMessage.CssClass = "text-info fw-bold";
            lblLoadingUsers.Visible = false;
            gvUsers.Visible = true;
        }

        protected void LoadAllFlights(object sender, EventArgs e)
        {
            DataTable dt = DatabaseHelper.GetAllFlights();
            gvFlights.DataSource = dt;
            gvFlights.DataBind();
            lblFlightMessage.Text = $"Loaded {dt.Rows.Count} flights";
            lblFlightMessage.CssClass = "text-info fw-bold";
            lblLoadingFlights.Visible = false;
            gvFlights.Visible = true;
        }

        protected void LoadAllBookings(object sender, EventArgs e)
        {
            DataTable dt = DatabaseHelper.GetAllBookings();
            gvBookings.DataSource = dt;
            gvBookings.DataBind();
            lblBookingMessage.Text = $"Loaded {dt.Rows.Count} bookings";
            lblBookingMessage.CssClass = "text-info fw-bold";
            lblLoadingBookings.Visible = false;
            gvBookings.Visible = true;
        }

        protected void LoadAllUsers()
        {
            LoadAllUsers(null, null);
        }

        protected void LoadAllFlights()
        {
            LoadAllFlights(null, null);
        }

        protected void LoadAllBookings()
        {
            LoadAllBookings(null, null);
        }
    }
}