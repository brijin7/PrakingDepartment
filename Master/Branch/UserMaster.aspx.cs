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

public partial class Master_EmployeeMaster : System.Web.UI.Page
{
    IFormatProvider objEnglishDate = new System.Globalization.CultureInfo("en-GB", true);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null && Session["UserRole"] == null)
        {
            Session.Clear();
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
        }
        if (string.IsNullOrEmpty(Session["BranchId"] as string))
        {
            if (Session["UserRole"].ToString() == "SA")
            {
                Response.Redirect("~/Login/OwnerLogin.aspx");
            }
            else
            {
                Response.Redirect("~/Login/BranchLogin.aspx");
            }
        }
        if (!IsPostBack)
        {
            BindEmployeeMaster();

        }
        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }

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
                        + "userMaster?userRole=E&branchId=" + Session["branchId"].ToString().Trim();
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
                            var objResponse1 =
                                   JsonConvert.DeserializeObject<List<Address>>(rateInfo.ToString());
                            dtaddress = ConvertToDataTable(objResponse1);
                            if (dtaddress.Rows.Count > 0)
                            {
                                if (dtaddress.Rows[0]["addressId"].ToString() != "")
                                {
                                    dtadd.Rows.Add(dt.Rows[i]["userId"], dtaddress.Rows[0]["addressId"], dtaddress.Rows[0]["address"], dtaddress.Rows[0]["alternatePhoneNumber"],
                                   dtaddress.Rows[0]["district"], dtaddress.Rows[0]["state"], dtaddress.Rows[0]["city"], dtaddress.Rows[0]["pincode"]);

                                }
                            }
                            else
                            {
                                dtadd.Rows.Add(dt.Rows[i]["userId"], "", "", "",
                              "", "", "", "");
                            }

                        }

                        List<Address> address = new List<Address>();
                        address = (from DataRow dr in dtadd.Rows
                                   select new Address()
                                   {
                                       addressId = dr["addressId"].ToString(),
                                       address = dr["address"].ToString(),
                                       city = dr["city"].ToString(),
                                       district = dr["district"].ToString(),
                                       state = dr["state"].ToString(),
                                       alternatePhoneNumber = dr["alternatePhoneNumber"].ToString(),
                                       userId = dr["userId"].ToString(),
                                       pincode = dr["pincode"].ToString()
                                   }).ToList();
                        List<EmployeeMasterClass> employee = new List<EmployeeMasterClass>();

                        employee = (from DataRow dr in dt.Rows
                                    select new EmployeeMasterClass()
                                    {
                                        parkingOwnerId = dr["parkingOwnerId"].ToString(),
                                        branchId = dr["branchId"].ToString(),
                                        branchName = dr["branchName"].ToString(),
                                        emailId = dr["emailId"].ToString(),
                                        phoneNumber = dr["phoneNumber"].ToString(),
                                        userName = dr["userName"].ToString(),
                                        password = dr["password"].ToString(),
                                        userId = dr["userId"].ToString(),
                                        imageUrl = dr["imageUrl"].ToString(),
                                        userRole = dr["userRole"].ToString(),
                                        activeStatus = dr["activeStatus"].ToString(),
                                        employeeId = dr["employeeId"].ToString(),
                                    }).ToList();
                        var JoinUsingQS = (from emp in employee
                                           join add in address
                                           on emp.userId equals add.userId

                                           select new
                                           {
                                               parkingOwnerId = emp.parkingOwnerId,
                                               branchId = emp.branchId,
                                               userName = emp.userName,
                                               password = emp.password,
                                               emailId = emp.emailId,
                                               phoneNumber = emp.phoneNumber,
                                               alternatePhoneNumber = add.alternatePhoneNumber,
                                               address = add.address,
                                               city = add.city,
                                               district = add.district,
                                               state = add.state,
                                               pincode = add.pincode,
                                               imageUrl = emp.imageUrl,
                                               userRole = emp.userRole,
                                               activeStatus = emp.activeStatus,
                                               employeeId = emp.employeeId,
                                               addressId = add.addressId,
                                               userId = emp.userId,
                                               branchName = emp.branchName,

                                           }).ToList();
                        DataTable dtfinal = ConvertToDataTable(JoinUsingQS);
                        if (dtfinal.Rows.Count > 0)
                        {
                            gvEmpmaster.DataSource = dtfinal;
                            gvEmpmaster.DataBind();
                        }
                        else
                        {
                            spAddorEdit.InnerText = "Add ";
                            ADD();
                            gvEmpmaster.DataBind();
                        }
                    }
                    else
                    {
                        spAddorEdit.InnerText = "Add ";
                        ADD();
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

    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        spAddorEdit.InnerText = "Add ";
        ADD();
    }
    public void ADD()
    {
        divGv.Visible = false;
        divForm.Visible = true;
        togglecheck.Checked = false;
    }
    #endregion

    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            if (txtEmployeename.Text == "")
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Enter Employee Name');", true);
                txtEmployeename.Focus();
                return;
            }

            InsertEmployee();
        }
        else
        {
            UpdateEmployee();
        }
    }
    #endregion

    #region Insert Function
    public void InsertEmployee()
    {
        string file = hfImageUrl.Value;

        if (txtPincode.Text == "")
        {
            hfCity.Value = null;
            hfDistrict.Value = null;
            hfState.Value = null;
        }

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                EmployeeMasterClass Insert = new EmployeeMasterClass()
                {
                    parkingOwnerId = Session["ParkingOwnerId"].ToString() == "0" ? null : Session["ParkingOwnerId"].ToString(),
                    branchId = Session["branchId"].ToString() == "" ? null : Session["branchId"].ToString(),
                    userName = txtEmployeename.Text.Trim() == "" ? null : txtEmployeename.Text.Trim(),
                    password = txtPassword.Text.Trim() == "" ? null : txtPassword.Text.Trim(),
                    emailId = txtEmail.Text.Trim() == "" ? null : txtEmail.Text.Trim(),
                    phoneNumber = txtPhoneNo.Text.Trim() == "" ? null : txtPhoneNo.Text.Trim(),
                    alternatePhoneNumber = txtAlterPhoneNo.Text.Trim() == "" ? null : txtAlterPhoneNo.Text.Trim(),
                    address = txtAddress.Text.Trim() == "" ? null : txtAddress.Text.Trim(),
                    city = hfCity.Value.Trim() == "" ? null : hfCity.Value.Trim(),
                    district = hfDistrict.Value.Trim() == "" ? null : hfDistrict.Value.Trim(),
                    state = hfState.Value.Trim() == "" ? null : hfState.Value.Trim(),
                    pincode = txtPincode.Text.Trim() == "" ? "0" : txtPincode.Text.Trim(),
                    imageUrl = file.ToString().Trim() == "" ? null : file.ToString().Trim(),
                    userRole = "E",
                    activeStatus = "A",
                    approvalStatus = Session["approvalStatus"].ToString().Trim(),
                    createdBy = Session["UserId"].ToString() == "" ? null : Session["UserId"].ToString()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("userMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindEmployeeMaster();
                        Cancel();
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
    public void UpdateEmployee()
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
                    parkingOwnerId = Session["ParkingOwnerId"].ToString() == "0" ? null : Session["ParkingOwnerId"].ToString(),
                    userId = ViewState["userId"].ToString() == "" ? null : ViewState["userId"].ToString(),
                    branchId = Session["branchId"].ToString() == "" ? null : Session["branchId"].ToString(),

                    userName = txtEmployeename.Text.Trim() == "" ? null : txtEmployeename.Text.Trim(),
                    //password = txtPassword.Text.Trim() == "" ? null : txtPassword.Text.Trim(),
                    emailId = txtEmail.Text.Trim() == "" ? null : txtEmail.Text.Trim(),
                    phoneNumber = txtPhoneNo.Text.Trim() == "" ? null : txtPhoneNo.Text.Trim(),
                    alternatePhoneNumber = txtAlterPhoneNo.Text.Trim() == "" ? null : txtAlterPhoneNo.Text.Trim(),
                    address = txtAddress.Text.Trim() == "" ? null : txtAddress.Text.Trim(),
                    city = hfCity.Value.Trim() == "" ? null : hfCity.Value.Trim(),
                    district = hfDistrict.Value.Trim() == "" ? null : hfDistrict.Value.Trim(),
                    state = hfState.Value.Trim() == "" ? null : hfState.Value.Trim(),
                    pincode = txtPincode.Text.Trim() == "" ? null : txtPincode.Text.Trim(),
                    imageUrl = file.ToString().Trim() == "" ? null : file.ToString().Trim(),
                    userRole = "E",
                    activeStatus = "A",
                    updatedBy = Session["UserId"].ToString() == "" ? null : Session["UserId"].ToString(),
                    employeeId = ViewState["employeeId"].ToString() == "" ? null : ViewState["employeeId"].ToString(),
                    addressId = ViewState["addressId"].ToString() == "" ? null : ViewState["addressId"].ToString()

                };
                HttpResponseMessage response = client.PutAsJsonAsync("userMaster", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindEmployeeMaster();
                        btnSubmit.Text = "Submit";
                        Cancel();
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
    }
    public void Cancel()
    {
        spAddorEdit.InnerText = "";
        divGv.Visible = true;
        divForm.Visible = false;
        txtEmployeename.Text = string.Empty;
        txtPassword.Text = string.Empty;
        txtPassword.Attributes.Add("value", txtPassword.Text);
        txtEmail.Text = string.Empty;
        txtPhoneNo.Text = string.Empty;
        txtAlterPhoneNo.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtDistrict.Text = string.Empty;
        txtState.Text = string.Empty;
        txtPincode.Text = string.Empty;
        fupEmpLink.Dispose();
        imgEmpPhotoPrev.ImageUrl = "~/images/EmptyImage.png";
        btnSubmit.Text = "Submit";
        hfCity.Value = string.Empty;
        hfDistrict.Value = string.Empty;
        hfState.Value = string.Empty;
        hfPassword.Value = string.Empty;
        hfDOJ.Value = string.Empty;
        txtEmployeename.Enabled = true;
    }
    #endregion 

    #region Edit Click
    protected void LnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            togglecheck.Checked = false;
            spAddorEdit.InnerText = "Edit ";
            txtEmployeename.Enabled = false;
            txtPassword.Enabled = false;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvbranchId = (Label)gvrow.FindControl("lblgvbranchId");
            Label lblgvuserName = (Label)gvrow.FindControl("lblgvuserName");
            txtEmployeename.Text = lblgvuserName.Text.Trim();
            Label lblgvpassword = (Label)gvrow.FindControl("lblgvpassword");
            txtPassword.Text = lblgvpassword.Text.Trim();
            hfPassword.Value = txtPassword.Text.Trim();
            if (txtPassword.TextMode == TextBoxMode.Password)
            {
                txtPassword.Attributes.Add("value", txtPassword.Text);
            }
            Label lblgvemailId = (Label)gvrow.FindControl("lblgvemailId");
            txtEmail.Text = lblgvemailId.Text.Trim();
            Label lblgvphoneNumber = (Label)gvrow.FindControl("lblgvphoneNumber");
            txtPhoneNo.Text = lblgvphoneNumber.Text.Trim();
            Label lblgvuserRole = (Label)gvrow.FindControl("lblgvuserRole");
            Label lblgvemployeeId = (Label)gvrow.FindControl("lblgvemployeeId");
            Label lblgvaddressId = (Label)gvrow.FindControl("lblgvaddressId");
            Label lblgvaddress = (Label)gvrow.FindControl("lblgvaddress");
            txtAddress.Text = lblgvaddress.Text;
            Label lblgvalternatePhoneNumber = (Label)gvrow.FindControl("lblgvalternatePhoneNumber");
            txtAlterPhoneNo.Text = lblgvalternatePhoneNumber.Text;
            Label lblgvdistrict = (Label)gvrow.FindControl("lblgvdistrict");
            hfDistrict.Value = lblgvdistrict.Text;
            txtDistrict.Text = hfDistrict.Value;
            Label lblgvstate = (Label)gvrow.FindControl("lblgvstate");
            hfState.Value = lblgvstate.Text;
            txtState.Text = hfState.Value;
            Label lblgvcity = (Label)gvrow.FindControl("lblgvcity");
            hfCity.Value = lblgvcity.Text;
            txtCity.Text = hfCity.Value;
            Label lblgvpincode = (Label)gvrow.FindControl("lblgvpincode");
            txtPincode.Text = lblgvpincode.Text;
            Label lblgvparkingOwnerId = (Label)gvrow.FindControl("lblgvparkingOwnerId");
            Label lblgvuserId = (Label)gvrow.FindControl("lblgvuserId");
            Label lblPhotoLink = (Label)gvrow.FindControl("lblgvimageUrl");

            imgEmpPhotoPrev.ImageUrl = lblPhotoLink.Text.Trim();
            hfPrevImageLink.Value = lblPhotoLink.Text.Trim();

            if (imgEmpPhotoPrev.ImageUrl == "")
            {
                imgEmpPhotoPrev.ImageUrl = "~/images/EmptyImage.png";

            }

            hfImageUrl.Value = imgEmpPhotoPrev.ImageUrl;
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string employeeId = gvEmpmaster.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.Text.Trim().ToString() == "Active" ? "A" : "D";
            ViewState["employeeId"] = lblgvemployeeId.Text.Trim();
            ViewState["userId"] = lblgvuserId.Text;
            ViewState["addressId"] = lblgvaddressId.Text;
            btnSubmit.Text = "Update";
            ADD();
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
                Label lbluserId = (Label)gvrow.FindControl("lblgvuserId");
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "userMaster?userId=" + lbluserId.Text
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
                        BindEmployeeMaster();
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

    #region Employee Address Class
    public class EmployeeMasterClass
    {
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string blockId { get; set; }
        public string floorId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string emailId { get; set; }

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

    /// <summary>
    /// menu Access Rights 
    /// Created By Abhinaya K
    /// Created Date 02-AUG-2022
    /// </summary>
    #region menu Option Access
    public class menuOptionAccess
    {
        public String parkingOwnerId { get; set; }
        public String userId { get; set; }
        public String userName { get; set; }
        public String userRole { get; set; }
        public String moduleId { get; set; }

        public List<optionDetails> optionDetails { get; set; }

        public String createdBy { get; set; }
    }
    public class optionDetails
    {
        public string optionId { get; set; }
        public string optionName { get; set; }
        public string menuOptionActiveStatus { get; set; }
        public string MenuOptionAccessId { get; set; }
        public string MenuOptionAccessActiveStatus { get; set; }
        public string activeStatus { get; set; }
        public string AddRights { get; set; }
        public string EditRights { get; set; }
        public string ViewRights { get; set; }
        public string DeleteRights { get; set; }
    }


    #endregion
    #region Get Method
    public void BindMenuAccess()
    {

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                       + "menuOptionAccess?userId=" + Session["UserId"].ToString() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        List<menuOptionAccess> lst = JsonConvert.DeserializeObject<List<menuOptionAccess>>(ResponseMsg);
                        var firstItem = lst.ElementAt(0);
                        var lst1 = firstItem.optionDetails.ToList();
                        DataTable optionDetails = ConvertToDataTable(lst1);
                        var Option = lst1.Where(x => x.optionName == "userMaster" && x.MenuOptionAccessActiveStatus == "A")
                            .Select(x => new
                            {
                                optionName = x.optionName,
                                AddRights = x.AddRights,
                                EditRights = x.EditRights,
                                ViewRights = x.ViewRights,
                                DeleteRights = x.DeleteRights
                            }).ToList();
                        if (Option.Count > 0)
                        {
                            var Add = Option.Select(y => y.AddRights).ToList();
                            var Edit = Option.Select(y => y.EditRights).ToList();
                            var Delete = Option.Select(y => y.DeleteRights).ToList();
                            var View = Option.Select(y => y.ViewRights).ToList();
                            if (Add[0] == "True")
                            {
                                btnAdd.Visible = true;
                            }
                            else
                            {
                                btnAdd.Visible = false;
                            }
                            if (View[0] == "True")
                            {
                                divGridView.Visible = true;
                                if (Edit[0] == "True")
                                {
                                    gvEmpmaster.Columns[6].Visible = true;
                                }
                                else
                                {
                                    gvEmpmaster.Columns[6].Visible = false;
                                }
                                if (Delete[0] == "True")
                                {
                                    gvEmpmaster.Columns[7].Visible = true;
                                }
                                else
                                {
                                    gvEmpmaster.Columns[7].Visible = false;
                                }

                            }
                            else
                            {
                                divGridView.Visible = false;
                            }

                        }
                        else
                        {
                            divForm.Visible = false;
                            divGv.Visible = false;
                        }



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