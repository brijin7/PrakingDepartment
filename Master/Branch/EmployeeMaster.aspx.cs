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

    #region User
    public void BindddlUser()
    {
        try
        {
            ddlUser.Items.Clear();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                        + "userMaster?userRole=E&activeStatus=A&branchId=" + Session["branchId"].ToString().Trim() + "&type=D";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtBlock = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ddlUser.DataSource = dtBlock;
                        ddlUser.DataValueField = "employeeId";
                        ddlUser.DataTextField = "userName";
                        ddlUser.DataBind();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlUser.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    public void BindeditddlUser()
    {
        try
        {
            ddlUser.Items.Clear();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                        + "userMaster?userRole=E&activeStatus=A&branchId=" + Session["branchId"].ToString().Trim();
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtBlock = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ddlUser.DataSource = dtBlock;
                        ddlUser.DataValueField = "employeeId";
                        ddlUser.DataTextField = "userName";
                        ddlUser.DataBind();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlUser.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region Block
    public void BindDdlBlock()
    {
        try
        {
            ddlBlock.Items.Clear();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "blockMaster?branchId=" + Session["branchId"].ToString()
                            + "&activeStatus=A&approvalStatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtBlock = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ddlBlock.DataSource = dtBlock;
                        ddlBlock.DataValueField = "blockId";
                        ddlBlock.DataTextField = "blockName";
                        ddlBlock.DataBind();
                        if (dtBlock.Rows.Count == 1)
                        {
                            ddlBlock.SelectedValue = dtBlock.Rows[0]["blockId"].ToString();
                            BindFloor();
                            ViewState["BlockCount"] = "1";
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlBlock.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    protected void ddlBlock_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBlock.SelectedIndex != 0)
        {
            BindFloor();
        }
        else
        {
            ddlFloor.SelectedIndex = 0;
            ddlFloor.Items.Clear();
        }
    }
    #endregion

    #region Bind Floor 
    public void BindFloor()
    {
        try
        {
            ddlFloor.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim() + "floorMaster?activeStatus=A&blockId=";
                sUrl += ddlBlock.SelectedValue.Trim();

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        var other = JsonConvert.DeserializeObject<dynamic>(ResponseMsg);
                        foreach (var item in other)
                        {
                            item.Property("floorVehicleDetails").Remove();
                            item.Property("floorFeaturesDetails").Remove();
                        }
                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);

                        if (dt.Rows.Count > 0)
                        {
                            ddlFloor.DataSource = dt;
                            ddlFloor.DataValueField = "floorId";
                            ddlFloor.DataTextField = "floorName";
                            ddlFloor.DataBind();

                            if (dt.Rows.Count == 1)
                            {
                                ddlFloor.SelectedValue = dt.Rows[0]["floorId"].ToString();
                                ViewState["FloorCount"] = "1";
                            }
                        }
                        else
                        {
                            ddlFloor.DataBind();
                        }
                        ddlFloor.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlFloor.ClearSelection();
                    }
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region Shift
    public void BindDdlShift()
    {
        try
        {
            using (var client = new HttpClient())
            {
                ddlShift.Items.Clear();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "shiftMaster?branchId=" + Session["branchId"].ToString() + ""
                            + "&activestatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtShift = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ddlShift.DataSource = dtShift;
                        ddlShift.DataValueField = "shiftId";
                        ddlShift.DataTextField = "shiftConfigName";
                        ddlShift.DataBind();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlShift.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region Designation
    public void BindDdlDesignation()
    {
        try
        {
            using (var client = new HttpClient())
            {
                ddlDesignation.Items.Clear();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "configMaster?configTypename=EmployeeDesignation&activestatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtDesignation = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ddlDesignation.DataSource = dtDesignation;
                        ddlDesignation.DataValueField = "configId";
                        ddlDesignation.DataTextField = "configName";
                        ddlDesignation.DataBind();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlDesignation.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region EmployeeType
    public void BindDdlEmployeeType()
    {
        try
        {
            using (var client = new HttpClient())
            {
                ddlEmpType.Items.Clear();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "configMaster?configTypename=EmployeeType&activestatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtDEmployeeType = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ddlEmpType.DataSource = dtDEmployeeType;
                        ddlEmpType.DataValueField = "configId";
                        ddlEmpType.DataTextField = "configName";
                        ddlEmpType.DataBind();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlEmpType.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

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
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dt.Rows[i];
                            if (dr["blockId"].ToString() == "" || dr["blockId"] == null)
                            {
                                dr.Delete();

                            }

                        }
                        dt.AcceptChanges();



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
                                        blockId = dr["blockId"].ToString(),
                                        blockName = dr["blockName"].ToString(),
                                        floorName = dr["floorName"].ToString(),
                                        floorId = dr["floorId"].ToString(),
                                        emailId = dr["emailId"].ToString(),
                                        phoneNumber = dr["phoneNumber"].ToString(),
                                        userName = dr["userName"].ToString(),
                                        password = dr["password"].ToString(),
                                        userId = dr["userId"].ToString(),
                                        imageUrl = dr["imageUrl"].ToString(),
                                        DOJ = dr["DOJ"].ToString(),
                                        empType = dr["empType"].ToString(),
                                        empTypeName = dr["empTypeName"].ToString(),
                                        userRole = dr["userRole"].ToString(),
                                        empDesignation = dr["empDesignation"].ToString(),
                                        empDesignationName = dr["empDesignationName"].ToString(),
                                        activeStatus = dr["activeStatus"].ToString(),
                                        shiftName = dr["shiftName"].ToString(),
                                        shiftId = dr["shiftId"].ToString(),
                                        employeeId = dr["employeeId"].ToString(),
                                    }).ToList();
                        var JoinUsingQS = (from emp in employee
                                           join add in address
                                           on emp.userId equals add.userId

                                           select new
                                           {
                                               parkingOwnerId = emp.parkingOwnerId,
                                               branchId = emp.branchId,
                                               blockId = emp.blockId,
                                               floorId = emp.floorId,
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
                                               DOJ = emp.DOJ,
                                               empType = emp.empType,
                                               userRole = emp.userRole,
                                               empDesignation = emp.empDesignation,
                                               activeStatus = emp.activeStatus,
                                               shiftId = emp.shiftId,
                                               employeeId = emp.employeeId,
                                               addressId = add.addressId,
                                               userId = emp.userId,
                                               branchName = emp.branchName,
                                               blockName = emp.blockName,
                                               floorName = emp.floorName,
                                               shiftName = emp.shiftName,
                                               empTypeName = emp.empTypeName,
                                               empDesignationName = emp.empDesignationName

                                           }).ToList();
                        DataTable dtfinal = ConvertToDataTable(JoinUsingQS);
                        if (dtfinal.Rows.Count > 0)
                        {
                            gvEmpmaster.DataSource = dtfinal;
                            gvEmpmaster.DataBind();
                        }
                        else
                        {
                            AddMethod();
                            gvEmpmaster.DataBind();
                          //  ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
                        AddMethod();
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
        AddMethod();
    }
    public void AddMethod()
    {

        ViewState["BlockCount"] = "0";
        ViewState["FloorCount"] = "0";
        spAddorEdit.InnerText = "Add ";
        ADD();
        BindddlUser();
        BindDdlBlock();
        BindDdlShift();
        BindDdlDesignation();
        BindDdlEmployeeType();
        ddlUser.Enabled = true;
        if (ViewState["BlockCount"].ToString() == "1" && ViewState["FloorCount"].ToString() == "1")
        {
            ddlBlock.Enabled = false;
            ddlFloor.Enabled = false;
        }
        else
        {
            ddlBlock.Enabled = true;
            ddlFloor.Enabled = true;
        }
    }

    #endregion

    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (btnSubmit.Text == "Submit")
        {
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
        string date1;
        string times;
        string times1;
        string[] date2;
        DateTime dates = Convert.ToDateTime(txtDOJ.Text.Trim());
        date1 = dates.ToString("yyyy-MM-dd");
        DateTimeOffset utcTime = DateTimeOffset.UtcNow;
        times = utcTime.ToString("yyyy'-'MM'-'dd'T 'hh':'mm':'ss'.'fff'Z'");
        date2 = times.Split(' ');
        times1 = date1 + "T" + date2[1];
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
                    branchId = Session["branchId"].ToString() == "" ? null : Session["branchId"].ToString(),
                    blockId = ddlBlock.SelectedValue.Trim() == "" ? null : ddlBlock.SelectedValue.Trim(),
                    floorId = ddlFloor.SelectedValue.Trim() == "" ? null : ddlFloor.SelectedValue.Trim(),
                    employeeId = ddlUser.SelectedValue.Trim() == "" ? null : ddlUser.SelectedValue.Trim(),
                    DOJ = times1 == "" ? null : times1,
                    empType = ddlEmpType.SelectedValue.Trim() == "" ? null : ddlEmpType.SelectedValue.Trim(),
                    empDesignation = ddlDesignation.SelectedValue.Trim() == "" ? null : ddlDesignation.SelectedValue.Trim(),
                    shiftId = ddlShift.SelectedValue.Trim() == "" ? null : ddlShift.SelectedValue.Trim(),
                    updatedBy = Session["UserId"].ToString() == "" ? null : Session["UserId"].ToString()
                };

                HttpResponseMessage response = client.PutAsJsonAsync("employeeMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlFloor.Items.Clear();
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
        if (txtDOJ.Text == "")
        {
            txtDOJ.Text = hfDOJ.Value;
        }

        string date1;
        string times;
        string times1;
        string[] date2;
        DateTime dates = Convert.ToDateTime(txtDOJ.Text.Trim());
        date1 = dates.ToString("yyyy-MM-dd");
        DateTimeOffset utcTime = DateTimeOffset.UtcNow;
        times = utcTime.ToString("yyyy'-'MM'-'dd'T 'hh':'mm':'ss'.'fff'Z'");
        date2 = times.Split(' ');
        times1 = date1 + "T" + date2[1];
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
                    branchId = Session["branchId"].ToString() == "" ? null : Session["branchId"].ToString(),
                    blockId = ddlBlock.SelectedValue.Trim() == "" ? null : ddlBlock.SelectedValue.Trim(),
                    floorId = ddlFloor.SelectedValue.Trim() == "" ? null : ddlFloor.SelectedValue.Trim(),
                    employeeId = ddlUser.SelectedValue.Trim() == "" ? null : ddlUser.SelectedValue.Trim(),
                    DOJ = times1 == "" ? null : times1,
                    empType = ddlEmpType.SelectedValue.Trim() == "" ? null : ddlEmpType.SelectedValue.Trim(),
                    empDesignation = ddlDesignation.SelectedValue.Trim() == "" ? null : ddlDesignation.SelectedValue.Trim(),
                    updatedBy = Session["UserId"].ToString() == "" ? null : Session["UserId"].ToString(),
                    shiftId = ddlShift.SelectedValue.Trim() == "" ? null : ddlShift.SelectedValue.Trim()

                };
                HttpResponseMessage response = client.PutAsJsonAsync("employeeMaster", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindEmployeeMaster();
                        ddlFloor.Items.Clear();
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

    #region Reset Click
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    #endregion

    #region Cancel Click Fucntion
    public void Cancel()
    {
        divGv.Visible = true;
        divForm.Visible = false;
        ddlBlock.ClearSelection();
        ddlFloor.ClearSelection();
        ddlFloor.Items.Clear();
        txtDOJ.Text = string.Empty;
        ddlEmpType.ClearSelection();
        ddlDesignation.ClearSelection();
        ddlShift.ClearSelection();
        btnSubmit.Text = "Submit";
        ddlUser.Enabled = true;
        spAddorEdit.InnerText = "";
    }
    #endregion

    #region Add Click Fucntion
    public void ADD()
    {
        divGv.Visible = false;
        divForm.Visible = true;
    }
    #endregion

    #region Edit Click
    protected void LnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["BlockCount"] = "0";
            ViewState["FloorCount"] = "0";
            spAddorEdit.InnerText = "Edit ";
            ddlUser.Enabled = false;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            BindDdlBlock();
            Label lblgvbranchId = (Label)gvrow.FindControl("lblgvbranchId");
            Label lblgvblockId = (Label)gvrow.FindControl("lblgvblockId");
            if (lblgvblockId.Text.Trim() != "")
            {
                ddlBlock.SelectedValue = lblgvblockId.Text.Trim();
            }
            BindFloor();
            Label lblgvfloorId = (Label)gvrow.FindControl("lblgvfloorId");
            if (lblgvfloorId.Text.Trim() != "")
            {
                ddlFloor.SelectedValue = lblgvfloorId.Text.Trim();
            }
            BindDdlShift();
            Label lblgvshiftId = (Label)gvrow.FindControl("lblgvshiftId");
            if (lblgvshiftId.Text.Trim() != "")
            {
                ddlShift.SelectedValue = lblgvshiftId.Text.Trim();
            }

            Label lblgvuserName = (Label)gvrow.FindControl("lblgvuserName");

            Label lblgvpassword = (Label)gvrow.FindControl("lblgvpassword");

            Label lblgvemailId = (Label)gvrow.FindControl("lblgvemailId");

            Label lblgvphoneNumber = (Label)gvrow.FindControl("lblgvphoneNumber");

            Label lblgvDOJ = (Label)gvrow.FindControl("lblgvDOJ");
            hfDOJ.Value = lblgvDOJ.Text.Trim();
            DateTime Date = Convert.ToDateTime(hfDOJ.Value);
            txtDOJ.Text = Date.ToString("yyyy-MM-dd").Trim();
            hfDOJ.Value = Date.ToString("yyyy-MM-dd").Trim();
            if (txtDOJ.TextMode == TextBoxMode.Date)
            {
                txtDOJ.Attributes.Add("value", txtDOJ.Text);
            }

            BindDdlEmployeeType();
            Label lblgvempType = (Label)gvrow.FindControl("lblgvempType");
            if (lblgvempType.Text.Trim() != "")
            {
                ddlEmpType.SelectedValue = lblgvempType.Text.Trim();
            }

            BindDdlDesignation();
            Label lblgvempDesignation = (Label)gvrow.FindControl("lblgvempDesignation");
            if (lblgvempDesignation.Text.Trim() != "")
            {
                ddlDesignation.SelectedValue = lblgvempDesignation.Text;
            }

            Label lblgvuserRole = (Label)gvrow.FindControl("lblgvuserRole");
            Label lblgvemployeeId = (Label)gvrow.FindControl("lblgvemployeeId");
            Label lblgvaddressId = (Label)gvrow.FindControl("lblgvaddressId");
            Label lblgvaddress = (Label)gvrow.FindControl("lblgvaddress");
            Label lblgvalternatePhoneNumber = (Label)gvrow.FindControl("lblgvalternatePhoneNumber");
            Label lblgvdistrict = (Label)gvrow.FindControl("lblgvdistrict");
            hfDistrict.Value = lblgvdistrict.Text;
            Label lblgvstate = (Label)gvrow.FindControl("lblgvstate");
            hfState.Value = lblgvstate.Text;
            Label lblgvcity = (Label)gvrow.FindControl("lblgvcity");
            hfCity.Value = lblgvcity.Text;
            Label lblgvpincode = (Label)gvrow.FindControl("lblgvpincode");
            Label lblgvparkingOwnerId = (Label)gvrow.FindControl("lblgvparkingOwnerId");
            Label lblgvuserId = (Label)gvrow.FindControl("lblgvuserId");
            BindeditddlUser();
            ddlUser.SelectedValue = lblgvemployeeId.Text;
            Label lblPhotoLink = (Label)gvrow.FindControl("lblgvimageUrl");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string employeeId = gvEmpmaster.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.Text.Trim().ToString() == "Active" ? "A" : "D";
            ViewState["employeeId"] = lblgvemployeeId.Text.Trim();
            ViewState["userId"] = lblgvuserId.Text;
            ViewState["addressId"] = lblgvaddressId.Text;
            btnSubmit.Text = "Update";
            ADD();


            if (ViewState["BlockCount"].ToString() == "0" && ViewState["FloorCount"].ToString() == "0")
            {
                ddlBlock.Enabled = true;
                ddlFloor.Enabled = true;
            }
            else
            {
                ddlBlock.Enabled = false;
                ddlFloor.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string excp = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + excp + "');", true);

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
                        var Option = lst1.Where(x => x.optionName == "employeeMaster" && x.MenuOptionAccessActiveStatus == "A")
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
                                    gvEmpmaster.Columns[8].Visible = true;
                                }
                                else
                                {
                                    gvEmpmaster.Columns[8].Visible = false;
                                }
                                if (Delete[0] == "True")
                                {
                                    gvEmpmaster.Columns[9].Visible = true;
                                }
                                else
                                {
                                    gvEmpmaster.Columns[9].Visible = false;
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