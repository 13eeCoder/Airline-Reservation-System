using System;
using System.Web.UI;

namespace FinalSDAProject
{
    public partial class Dashboard : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsUserLogin"] == null || !(bool)Session["IsUserLogin"])
            {
                Response.Redirect("~/Login.aspx");
            }

            if (Session["IsAdmin"] != null && (bool)Session["IsAdmin"])
            {
                Response.Redirect("~/AdminDashboard.aspx");
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Login.aspx");
        }
    }
}