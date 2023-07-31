using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Master_Configuration_ChargePinConfig : System.Web.UI.Page
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
            BindVehicleConfigName();
        }
    }
    #region BindGridView
    public void BindVehicleConfigName()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("chargePinConfig").Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            gvVehicleConfigType.DataSource = dt;
                            gvVehicleConfigType.DataBind();
                            divGridView.Visible = true;
                        }
                        else
                        {
                            gvVehicleConfigType.DataBind();
                        }
                    }
                    else
                    {
                        Action CallFnOnAdd = new Action(divFormVisible);
                        CallFnOnAdd.Invoke();
                        divForm.Visible = true;
                        divGridView.Visible = false;
                        divGv.Visible = false;
                        spAddorEdit.InnerText = "Add ";
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
    #region Add Details
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Action CallFnOnAdd = new Action(divFormVisible);
        CallFnOnAdd.Invoke();
        divForm.Visible = true;
        divGridView.Visible = false;
        divGv.Visible = false;
        spAddorEdit.InnerText = "Add ";
    }
    #endregion
    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetInputField();
        Action CallFnOnCancel = new Action(divGvVisible);
        CallFnOnCancel.Invoke();
    }
    public void ResetInputField()
    {
        spAddorEdit.InnerText = "";
        divForm.Visible = false;
        divGridView.Visible = true;
        divGv.Visible = true;
        txtChargePinType.Text = string.Empty;
        string file = string.Empty;
        btnSubmit.Text = "Submit";
        fupVehicleImage.Dispose();
        imgEmpPhotoPrev.ImageUrl = "~/images/ChargePin2.jpg";
        hfImageUrl.Value = "";
        BindVehicleConfigName();
    }

    #endregion
    #region divFormVisible
    public void divFormVisible()
    {
        divForm.Visible = true;
        divGv.Visible = false;
    }
    #endregion
    #region divGvVisible
    public void divGvVisible()
    {
        divForm.Visible = false;
        divGv.Visible = true;
    }
    #endregion
    #region Smart Parking Class
    public class chargePinConfigMaster
    {
        public string chargePinId { get; set; }
        public string chargePinConfig { get; set; }
        public string chargePinImageUrl { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }

    }
    #endregion
    #region Insert Function
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (hfImageUrl.Value != null && hfImageUrl.Value != "" && hfImageUrl.Value != "~/images/ChargePin2.jpg")
        {
            if (btnSubmit.Text == "Submit")
            {
                Insert();
            }
            else
            {
                Update();
            }
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Please Upload Charge Pin Image');", true);
        }

    }
    public void Insert()
    {
        string file = hfImageUrl.Value;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new chargePinConfigMaster()
                {
                    chargePinConfig = txtChargePinType.Text.Trim(),
                    chargePinImageUrl = hfImageUrl.Value,
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("chargePinConfig", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ResetInputField();
                        btnSubmit.Visible = true;

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
        finally
        {


        }
    }
    #endregion
    #region Edit
    protected void LnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            spAddorEdit.InnerText = "Edit ";
            divForm.Visible = true;
            divGv.Visible = false;
            divGridView.Visible = false;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblchargePinConfig = (Label)gvrow.FindControl("lblchargePinConfig");
            LinkButton lblActiveStatus = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            Label lblPhotoLink = (Label)gvrow.FindControl("lblPhotoLink");
            string sChargePinId = gvVehicleConfigType.DataKeys[gvrow.RowIndex].Value.ToString();
            imgEmpPhotoPrev.ImageUrl = lblPhotoLink.Text.Trim();
            hfPrevImageLink.Value = lblPhotoLink.Text.Trim();

            if (imgEmpPhotoPrev.ImageUrl == "")
            {
                imgEmpPhotoPrev.ImageUrl = "~/images/ChargePin2.jpg";

            }

            hfImageUrl.Value = imgEmpPhotoPrev.ImageUrl;

            txtChargePinType.Text = lblchargePinConfig.Text;
            ViewState["configId"] = sChargePinId.ToString().Trim();
            ViewState["ActiveStatus"] = lblActiveStatus.Text.ToString() == "Active" ? "A" : "D";
            btnSubmit.Text = "Update";

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Delete
    protected void lnkActiveOrInactive_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sChargePinId = gvVehicleConfigType.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "chargePinConfig?chargePinId=" + sChargePinId
                            + "&activeStatus="
                            + sActiveStatus;

                HttpResponseMessage response = client.DeleteAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindVehicleConfigName();
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
    #region Update Function
    public void Update()
    {
        string file = hfImageUrl.Value;

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new chargePinConfigMaster()
                {
                    chargePinId = ViewState["configId"].ToString(),
                    chargePinConfig = txtChargePinType.Text.Trim(),
                    chargePinImageUrl = hfImageUrl.Value,
                    updatedBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("chargePinConfig", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ResetInputField();
                        btnSubmit.Visible = true;

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

}