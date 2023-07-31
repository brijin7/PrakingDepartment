using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["BaseUrl"] = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].Trim();
            Session["ImageUrl"] = System.Configuration.ConfigurationManager.AppSettings["ImageUrl"].Trim();
            Session["SmsUrl"] = System.Configuration.ConfigurationManager.AppSettings["SmsUrl"].Trim();

            Session["Branch"] = "0";
            Session["UserId"] = Request.QueryString["qUserId"].ToString().Trim();
            Session["UserRole"] = Request.QueryString["qUserRole"].ToString().Trim();
            Session["UserName"] = Request.QueryString["qUserName"].ToString().Trim();
            Session["parkingOwnerId"] = Request.QueryString["qParkingOwnerId"].ToString().Trim();
            //Session["branchId"] = Request.QueryString["qBranchId"].ToString().Trim();
            Session["ShowPage"] = Request.QueryString["qShowPage"].ToString().Trim(); ; 
             Session["Bookingnav"] = Request.QueryString["qBookingnav"].ToString().Trim();

            if (Session["UserRole"].ToString().Trim() == "SA")
            {               
                Response.Redirect("~/DashBoard.aspx", false);
            }
            else if (Session["UserRole"].ToString().Trim() == "A")
            {              
                Response.Redirect("~/Login/BranchLogin.aspx", false);
            }
            else if (Session["UserRole"].ToString().Trim() == "E")
            {
                Session["ShowPage"] = "1";
                Response.Redirect("~/DashBoard.aspx", false);
            }

        }
    }
}
