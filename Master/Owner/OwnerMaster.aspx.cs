using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Master_OwnerMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null && Session["UserRole"] == null)
        {
            Session.Clear();
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
        }
        if (!IsPostBack)
        {
            BindGvOwner();
        }
    }

    #region Bind GridView
    public void BindGvOwner()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim() + "parkingOwnerMaster";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        List<OwnerMaster> OwnerMstr = JsonConvert.DeserializeObject<List<OwnerMaster>>(ResponseMsg);
                        var other = JsonConvert.DeserializeObject<dynamic>(ResponseMsg);
                        foreach (var item in other)
                        {
                            item.Property("addressDetails").Remove();
                        }
                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);
                        if (dt.Rows.Count > 0)
                        {
                            gvOwnerDetails.DataSource = dt;
                            gvOwnerDetails.DataBind();
                        }
                        else
                        {
                            gvOwnerDetails.DataBind();
                        }
                    }
                    else
                    {
                        Clear();
                        divGv.Visible = false;
                        divForm.Visible = true;
                        spAddorEdit.InnerText = "Add ";
                       // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    #endregion

    #region Submit_Click/Insert
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            InsertOwner();
        }
        else
        {
            UpdateOwner();
        }

    }
    public void InsertOwner()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                OwnerMaster Insert = new OwnerMaster()
                {
                    userName = txtUserName.Text.Trim(),
                    password = txtPassword.Text.Trim(),
                    parkingName = txtParkingName.Text.Trim(),
                    shortName = txtShortName.Text.Trim(),
                    emailId = txtEmailId.Text.Trim(),
                    phoneNumber = txtPhoneNumber.Text.Trim(),
                    activeStatus = "A",
                    founderName = "Southern Railways",
                    logoUrl = hfImageUrl.Value,
                    websiteUrl = "www.Prematix.com",
                    placeType = "Commercial",
                    createdBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("admin", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        Clear();
                        BindGvOwner();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region Add Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Clear();
        divGv.Visible = false;
        divForm.Visible = true;
        spAddorEdit.InnerText = "Add ";
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }
    public void Clear()
    {
        txtParkingName.Text = string.Empty;
        txtShortName.Text = string.Empty;
        txtUserName.Text = string.Empty;
        fupEmpLink.Dispose();
        hfImageUrl.Value = string.Empty;
        imgEmpPhotoPrev.ImageUrl = "~/images/emptylogo.png";
        divForm.Visible = false;
        divGv.Visible = true;
        txtUserName.Enabled = true;
        txtPassword.Enabled = true;
        btnSubmit.Text = "Submit";

        txtPassword.Text = "1";
        txtEmailId.Text = null;
        txtPhoneNumber.Text = null;

        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ClearPassword();", true);

    }
    #endregion

    #region Edit Click
    protected void LnkEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblGvParkingOwnerId = (Label)gvrow.FindControl("lblGvParkingOwnerId");
            Label lblGvUserId = (Label)gvrow.FindControl("lblGvUserId");
            Label lblGvParkingName = (Label)gvrow.FindControl("lblGvParkingName");
            Label lblGvShortName = (Label)gvrow.FindControl("lblGvShortName");
            Label lblGvFounderName = (Label)gvrow.FindControl("lblGvFounderName");
            //Label lblGvGSTNumber = (Label)gvrow.FindControl("lblGvGSTNumber");
            Label lblGvPlaceType = (Label)gvrow.FindControl("lblGvPlaceType");

            Label lblGvUserName = (Label)gvrow.FindControl("lblGvUserName");
            Label lblGvPassword = (Label)gvrow.FindControl("lblGvPassword");
            Label lblGvPhoneNo = (Label)gvrow.FindControl("lblGvPhoneNo");
            Label lblGvEmailId = (Label)gvrow.FindControl("lblGvEmailId");
            Label lblgvlogoUrl = (Label)gvrow.FindControl("lblgvlogoUrl");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string sgvparkingOwnerId = gvOwnerDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.Text == "Active" ? "A" : "D";
            ViewState["parkingOwnerId"] = sgvparkingOwnerId.ToString().Trim();
            ViewState["parkingOwnerUserId"] = lblGvUserId.Text;
            txtParkingName.Text = lblGvParkingName.Text;
            txtShortName.Text = lblGvShortName.Text;
            txtUserName.Text = lblGvUserName.Text;
            txtPassword.Text = lblGvPassword.Text;
            txtEmailId.Text = lblGvEmailId.Text;
            txtPhoneNumber.Text = lblGvPhoneNo.Text;

            hfPassword.Value = txtPassword.Text.Trim();
            if (txtPassword.TextMode == TextBoxMode.Password)
            {
                txtPassword.Attributes.Add("value", txtPassword.Text);
            }


            imgEmpPhotoPrev.ImageUrl = lblgvlogoUrl.Text.Trim();

            if (imgEmpPhotoPrev.ImageUrl == "")
            {
                imgEmpPhotoPrev.ImageUrl = "~/images/emptylogo.png";
            }

            hfImageUrl.Value = imgEmpPhotoPrev.ImageUrl;
            //upimgurl.FileName= hfImageUrl.Value;                               
            txtUserName.Enabled = false;
            txtPassword.Enabled = false;
            spAddorEdit.InnerText = "Edit ";
            divGv.Visible = false;
            divForm.Visible = true;
           
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    #endregion

    #region Update 
    public void UpdateOwner()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                OwnerMaster Update = new OwnerMaster()
                {
                    parkingOwnerId = ViewState["parkingOwnerId"].ToString().Trim(),
                    userId = ViewState["parkingOwnerUserId"].ToString().Trim(),
                    parkingName = txtParkingName.Text.Trim(),
                    shortName = txtShortName.Text.Trim(),
                    userName = txtUserName.Text.Trim(),
                    //password = txtPassword.Text.Trim(),
                    emailId = txtEmailId.Text.Trim(),
                    phoneNumber = txtPhoneNumber.Text.Trim(),
                    logoUrl = hfImageUrl.Value,
                    activeStatus = ViewState["ActiveStatus"].ToString().Trim(),
                    founderName = "Southern Railways",
                    websiteUrl = "www.Prematix.com",
                    placeType = "Commercial",
                    updatedBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("parkingOwnerMaster", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        Clear();
                        BindGvOwner();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region Delete Click
    protected void lnkActiveOrInactive_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                Label lblGvUserId = (Label)gvrow.FindControl("lblGvUserId");
                string sParkingOwnerId = gvOwnerDetails.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "parkingOwnerMaster?parkingOwnerId=" + sParkingOwnerId
                            + "&activeStatus=" + sActiveStatus + "&userId=" + lblGvUserId.Text.ToString();

                HttpResponseMessage response = client.DeleteAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindGvOwner();
                        Clear();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region Owner Master Class
    public class OwnerMaster
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string parkingOwnerId { get; set; }
        public string parkingName { get; set; }
        public string shortName { get; set; }
        public string founderName { get; set; }
        public string logoUrl { get; set; }
        public string websiteUrl { get; set; }
        public string gstNumber { get; set; }
        public string emailId { get; set; }
        public string phoneNumber { get; set; }
        public string placeType { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
    }
    #endregion
}
