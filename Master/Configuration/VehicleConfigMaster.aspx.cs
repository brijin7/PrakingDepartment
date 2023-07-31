using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Master_Configuration_VehicleConfigMaster : System.Web.UI.Page
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
                HttpResponseMessage response = client.GetAsync("vehicleConfigMaster").Result;
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
                        spAddorEdit.InnerText = "Add ";
                        Action CallFnOnAdd = new Action(divFormVisible);
                        CallFnOnAdd.Invoke();
                        divForm.Visible = true;
                        divGridView.Visible = false;
                        divGv.Visible = false;
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
    #region Add Details
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        spAddorEdit.InnerText = "Add ";
        Action CallFnOnAdd = new Action(divFormVisible);
        CallFnOnAdd.Invoke();
        divForm.Visible = true;
        divGridView.Visible = false;
        divGv.Visible = false;

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
        BindVehicleConfigName();
        txtVehicleName.Text = string.Empty;
        txtVehicleKeyName.Text = string.Empty;
        btnSubmit.Text = "Submit";
        fupVehicleImage.Dispose();
        fupVehiclePHImage.Dispose();
        imgVehPhotoPrev.ImageUrl = "~/images/vehicle-icon.png";
        imgVehPHPhotoPrev.ImageUrl = "~/images/vehicle-icon.png";
        hfImageUrl.Value = "";
        hfPHImageUrl.Value = "";
        rbtnVehicleNo.SelectedValue = "Y";
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

    #region Insert Function
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (hfPHImageUrl.Value != null && hfPHImageUrl.Value != "" && hfPHImageUrl.Value != "~/images/vehicle-icon.png")
        {
            if (hfImageUrl.Value != null && hfImageUrl.Value != "" && hfImageUrl.Value != "~/images/vehicle-icon.png")
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
                if (imgVehPHPhotoPrev.ImageUrl == "" || hfPHImageUrl.Value == "")
                {
                    imgVehPHPhotoPrev.ImageUrl = "~/images/vehicle-icon.png";
                }
                else { imgVehPHPhotoPrev.ImageUrl = hfPHImageUrl.Value; }
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Please Upload Vehicle Image');", true);
            }
        }
        else
        {
            if (imgVehPhotoPrev.ImageUrl == "" || hfImageUrl.Value == "")
            {
                imgVehPhotoPrev.ImageUrl = "~/images/vehicle-icon.png";
            }
            else { imgVehPhotoPrev.ImageUrl = hfImageUrl.Value; }

            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Please Upload Vehicle Place Holder Image');", true);
        }
    }
    public void Insert()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new VehicleConfigMaster()
                {
                    vehicleName = txtVehicleName.Text.Trim(),
                    vehicleKeyName = txtVehicleKeyName.Text.Trim(),
                    vehicleImageUrl = hfImageUrl.Value,
                    isvehicleNumberRequired = rbtnVehicleNo.SelectedValue,
                    vehiclePlaceHolderImageUrl = hfPHImageUrl.Value,
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("vehicleConfigMaster", Insert).Result;
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
            string sVehConfigId = gvVehicleConfigType.DataKeys[gvrow.RowIndex].Value.ToString();

            Label lblvehicleName = (Label)gvrow.FindControl("lblvehicleName");
            Label lblvehicleKeyName = (Label)gvrow.FindControl("lblvehicleKeyName");
            LinkButton lblActiveStatus = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            ImageButton lblVehicleImage = (ImageButton)gvrow.FindControl("lblVehicleImage");
            Label lblVehicleNo = (Label)gvrow.FindControl("lblVehicleNo");
            Label lblPhotoLink = (Label)gvrow.FindControl("lblPhotoLink");
            imgVehPhotoPrev.ImageUrl = lblPhotoLink.Text.Trim();

            if (imgVehPhotoPrev.ImageUrl == "")
            {
                imgVehPhotoPrev.ImageUrl = "~/images/vehicle-icon.png";

            }

            hfImageUrl.Value = imgVehPhotoPrev.ImageUrl;
            Label lblVehPHUrl = (Label)gvrow.FindControl("lblVehPHUrl");
            imgVehPHPhotoPrev.ImageUrl = lblVehPHUrl.Text.Trim();

            if (imgVehPHPhotoPrev.ImageUrl == "")
            {
                imgVehPHPhotoPrev.ImageUrl = "~/images/vehicle-icon.png";

            }

            hfPHImageUrl.Value = imgVehPHPhotoPrev.ImageUrl;

            txtVehicleName.Text = lblvehicleName.Text;
            txtVehicleKeyName.Text = lblvehicleKeyName.Text;
            rbtnVehicleNo.SelectedValue = lblVehicleNo.Text == "Yes" ? "Y" : "N";
            ViewState["configId"] = sVehConfigId.ToString().Trim();
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
                string sVehConfigId = gvVehicleConfigType.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "vehicleConfigMaster?vehicleConfigId=" + sVehConfigId
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
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new VehicleConfigMaster()
                {
                    vehicleConfigId = ViewState["configId"].ToString(),
                    vehicleName = txtVehicleName.Text.Trim(),
                    vehicleKeyName = txtVehicleKeyName.Text.Trim(),
                    vehicleImageUrl = hfImageUrl.Value,
                    isvehicleNumberRequired = rbtnVehicleNo.SelectedValue,
                    vehiclePlaceHolderImageUrl = hfPHImageUrl.Value,
                    updatedBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("vehicleConfigMaster", Update).Result;
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

    #region Vehicle Config Class
    public class VehicleConfigMaster
    {
        public string vehicleName { get; set; }
        public string vehicleImageUrl { get; set; }
        public string vehiclePlaceHolderImageUrl { get; set; }
        public string vehicleKeyName { get; set; }
        public string isvehicleNumberRequired { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public string vehicleConfigId { get; set; }
    }
    #endregion
}