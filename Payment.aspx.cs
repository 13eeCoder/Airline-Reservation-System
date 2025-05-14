using System;
using System.Web.UI;

namespace FinalSDAProject
{
    public partial class Payment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                if (Session["BookingPrice"] != null)
                {
                    decimal price = Convert.ToDecimal(Session["BookingPrice"]);
                    lblPrice.Text = price.ToString("C");
                }
                else
                {
                    Response.Redirect("UserDashboard.aspx");
                }
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            // In a real application, you would process the payment here
            // For this example, we'll just simulate a successful payment

            // Get payment method
            string paymentMethod = "";
            if (rbMastercard.Checked) paymentMethod = "Mastercard";
            else if (rbVisa.Checked) paymentMethod = "Visa";
            else if (rbPaypak.Checked) paymentMethod = "Paypak";

            // Process the booking
            int userId = Convert.ToInt32(Session["UserID"]);
            int flightId = Convert.ToInt32(Session["BookingFlightId"]);

            try
            {
                bool success = DatabaseHelper.BookFlight(userId, flightId, 1); // 1 passenger
                if (success)
                {
                    // Clear booking session
                    Session.Remove("BookingFlightId");
                    Session.Remove("BookingPrice");

                    // Show success message and redirect
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "paymentSuccess",
                        "setTimeout(function() { window.location.href = 'UserDashboard.aspx?tab=bookings'; }, 3000);", true);

                    // Show success message
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showSuccess",
                        "alert('Payment successful! You will be redirected to your bookings.');", true);
                }
                else
                {
                    lblMessage.Text = "Error processing your booking. Please try again.";
                    lblMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.Visible = true;
            }
        }
    }
}