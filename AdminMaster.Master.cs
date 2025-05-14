using System;
using System.Web.UI;

namespace FinalSDAProject
{
    public partial class AdminMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is authenticated and is admin
            if (Session["IsUserLogin"] == null || !(bool)Session["IsUserLogin"] ||
                Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }
}