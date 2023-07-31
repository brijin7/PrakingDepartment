using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    string BaseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].Trim();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string UserId = string.Empty;
            string parkingOwnerId = string.Empty;
            string branchId = string.Empty;
            string UserRole = string.Empty;


            ////Sadmin Role Login 

            //UserId = "92";
            //parkingOwnerId = "";
            //branchId = "";
            //UserRole = "SA"; //Sadmin
            //Session["ShowPage"] = "0";
            //Session["UserName"] = "Sadmin";

            ////Admin Role Login

            //UserId = "91";
            //parkingOwnerId = "54";
            //parkingOwnerId = "";
            //branchId = "";
            //UserRole = "A"; //Admin
            //Session["UserName"] = "Admin";
            //Session["ShowPage"] = "0";

            //// User Role Login

            //UserId = "6";
            //parkingOwnerId = "";
            //branchId = "45";
            //UserRole = "E"; //Employee

            //Response.Redirect("Index.aspx?qUserId=" + UserId.Trim() + "&qUserRole= " + UserRole.Trim() + "&qParkingOwnerId="
            //    + parkingOwnerId.Trim() + "&qBranchId=" + branchId.Trim() + "");
        }
    }
}