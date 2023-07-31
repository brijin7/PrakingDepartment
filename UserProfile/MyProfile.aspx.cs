using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Globalization;

public partial class Master_MyProfile : System.Web.UI.Page
{
    IFormatProvider objEnglishDate = new System.Globalization.CultureInfo("en-GB", true);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindEmployeeMaster();
        }
        txtEmployeename.Attributes.Add("ReadOnly", "ReadOnly");
        txtPassword.Attributes.Add("ReadOnly", "ReadOnly");
    }

    #region Bind Employee  Master 
    public void BindEmployeeMaster()
    {
        try
        {
            DataTable dtaddress = new DataTable();
            DataTable dtadd = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                    + "userMaster?userId=" + Session["UserId"].ToString() + "";

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    var Response = JObject.Parse(SmartParkingList)["response"].ToArray();

                    dtadd.Columns.Add("userId");
                    dtadd.Columns.Add("addressId");
                    dtadd.Columns.Add("address");
                    dtadd.Columns.Add("alternatePhoneNumber");
                    dtadd.Columns.Add("district");
                    dtadd.Columns.Add("state");
                    dtadd.Columns.Add("city");
                    dtadd.Columns.Add("pincode");

                    if (StatusCode == 1)
                    {
                        var other = JsonConvert.DeserializeObject<dynamic>(ResponseMsg);

                        foreach (var item in other)
                        {
                            if (item.addressDetails.Count == 0)
                            {
                                item.Property("addressDetails").Remove();
                            }

                        }

                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var rateInfo = JObject.Parse(SmartParkingList)["response"][i]["addressDetails"];
                            var objResponse1 = JsonConvert.DeserializeObject<List<Address>>(rateInfo.ToString());
                            dtaddress = ConvertToDataTable(objResponse1);
                            if (dtaddress.Rows.Count > 0)
                            {
                                if (dtaddress.Rows[0]["addressId"].ToString() != "")
                                {
                                    dtadd.Rows.Add(dt.Rows[i]["userId"], dtaddress.Rows[0]["alternatePhoneNumber"],
                                    dtaddress.Rows[0]["addressId"], dtaddress.Rows[0]["address"], dtaddress.Rows[0]["city"],
                                    dtaddress.Rows[0]["district"], dtaddress.Rows[0]["state"],
                                    dtaddress.Rows[0]["pincode"]);

                                    ViewState["addressId"] = dtaddress.Rows[0]["addressId"].ToString();

                                    txtAddress.Text = dtaddress.Rows[0]["address"].ToString();
                                    txtCity.Text = dtaddress.Rows[0]["city"].ToString();
                                    txtDistrict.Text = dtaddress.Rows[0]["district"].ToString();
                                    txtState.Text = dtaddress.Rows[0]["state"].ToString();
                                    txtPincode.Text = dtaddress.Rows[0]["pincode"].ToString();
                                }
                            }
                            else
                            {
                                dtadd.Rows.Add(dt.Rows[i]["userId"], "", "", "",
                              "", "", "", "");

                                divUserDetails.Visible = false;

                            }
                        }


                        if (dt.Rows.Count > 0)
                        {
                            //ViewState["branchId"] = dt.Rows[0]["branchId"].ToString();                          
                            ViewState["userId"] = dt.Rows[0]["userId"].ToString();
                            // ViewState["employeeId"]  = dt.Rows[0]["employeeId"].ToString();
                            txtEmployeename.Text = dt.Rows[0]["userName"].ToString();
                            txtPassword.Text = dt.Rows[0]["password"].ToString();
                            hfPassword.Value = txtPassword.Text.Trim();
                            if (txtPassword.TextMode == TextBoxMode.Password)
                            {
                                txtPassword.Attributes.Add("value", txtPassword.Text);
                            }

                            txtPhoneNo.Text = dt.Rows[0]["phoneNumber"].ToString();

                            imgEmpPhotoPrev.ImageUrl = dt.Rows[0]["imageUrl"].ToString();
                            hfPrevImageLink.Value = imgEmpPhotoPrev.ImageUrl;

                            if (imgEmpPhotoPrev.ImageUrl == "")
                            {
                                imgEmpPhotoPrev.ImageUrl = "~/images/EmptyImage.png";

                            }
                            hfImageUrl.Value = imgEmpPhotoPrev.ImageUrl;
                            txtEmail.Text = dt.Rows[0]["emailId"].ToString();
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    DataTable ConvertToDataTable<TSource>(IEnumerable<TSource> source)
    {
        var props = typeof(TSource).GetProperties();

        var dt = new DataTable();
        dt.Columns.AddRange(
          props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray()
        );

        source.ToList().ForEach(
          i => dt.Rows.Add(props.Select(p => p.GetValue(i, null)).ToArray())
        );

        return dt;
    }
    #endregion

    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
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
                var Update = new EmployeeMasterClass()
                {
                    //parkingOwnerId = Session["ParkingOwnerId"].ToString() == "0" ? null : Session["ParkingOwnerId"].ToString(),
                    userId = ViewState["userId"].ToString() == "" ? null : ViewState["userId"].ToString(),
                    //branchId = Session["branchId"].ToString() == "" ? null : Session["branchId"].ToString(),

                    userName = txtEmployeename.Text.Trim(),
                    //password = txtPassword.Text.Trim(),
                    emailId = txtEmail.Text.Trim() == "" ? null : txtEmail.Text.Trim(),
                    phoneNumber = txtPhoneNo.Text.Trim() == "" ? null : txtPhoneNo.Text.Trim(),
                    //alternatePhoneNumber = txtAlterPhoneNo.Text.Trim() == "" ? null : txtAlterPhoneNo.Text.Trim(),
                    address = txtAddress.Text.Trim() == "" ? null : txtAddress.Text.Trim(),
                    city = hfCity.Value.Trim() == "" ? null : hfCity.Value.Trim(),
                    district = hfDistrict.Value.Trim() == "" ? null : hfDistrict.Value.Trim(),
                    state = hfState.Value.Trim() == "" ? null : hfState.Value.Trim(),
                    pincode = txtPincode.Text.Trim() == "" ? null : txtPincode.Text.Trim(),
                    imageUrl = file.ToString().Trim() == "" ? null : file.ToString().Trim(),
                    updatedBy = Session["UserId"].ToString() == "" ? null : Session["UserId"].ToString()

                };
                HttpResponseMessage response = client.PutAsJsonAsync("userMaster", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        Session["UserName"] = txtEmployeename.Text.Trim();
                        BindEmployeeMaster();
                        Cancel();
                        ClientScript.RegisterStartupScript(this.GetType(), "Redirect",
                            "window.onload=function(){ successalert('" + ResponseMsg.ToString().Trim() + "'); window.location.href = '../DashBoard.aspx';}", true);
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

    #region Reset Click
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Cancel();
        Response.Redirect("~/DashBoard.aspx", false);
    }
    #endregion

    #region Cancel Click Fucntion
    public void Cancel()
    {
        txtEmployeename.Text = string.Empty;
        txtPassword.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtPhoneNo.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtDistrict.Text = string.Empty;
        txtState.Text = string.Empty;
        txtPincode.Text = string.Empty;
        fupEmpLink.Dispose();
        imgEmpPhotoPrev.ImageUrl = "~/images/EmptyImage.png";
        hfCity.Value = null;
        hfDistrict.Value = null;
        hfState.Value = null;
    }
    #endregion

    #region Employee Address Class
    public class EmployeeMasterClass
    {
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string blockId { get; set; }
        public string floorId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string emailId
        { get; set; }

        public string phoneNumber { get; set; }
        public string alternatePhoneNumber { get; set; }
        public string mainContactName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string imageUrl { get; set; }
        public string DOJ { get; set; }
        public string empType { get; set; }
        public string userRole { get; set; }
        public string empDesignation { get; set; }
        public string activeStatus { get; set; }
        public string walletAmt { get; set; }
        public string loyaltyPoints { get; set; }
        public string otp { get; set; }
        public string approvalStatus { get; set; }
        public string shiftId { get; set; }
        public string updatedBy { get; set; }
        public string createdBy { get; set; }
        public string employeeId { get; set; }
        public string addressId { get; set; }
        public string userId { get; set; }
        public string branchName { get; set; }
        public string blockName { get; set; }
        public string floorName { get; set; }
        public string shiftName { get; set; }
        public string empDesignationName { get; set; }
        public string empTypeName { get; set; }
        public List<Address> addressDetails { get; set; }


    }
    public class Address
    {
        public string userId { get; set; }
        public string addressId { get; set; }
        public string address { get; set; }
        public string alternatePhoneNumber { get; set; }
        public string district { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
    }

    #endregion

}