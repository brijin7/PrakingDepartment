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

public partial class Master_MenuAccessRights : System.Web.UI.Page
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
            BindddlUser();
            BindMenuOptionAccess();
        }
        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuaccess();
        }

    }

    #region User
    public void BindddlUser()
    {
        try
        {
            ddlUserName.Items.Clear();
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
                        ddlUserName.DataSource = dtBlock;
                        ddlUserName.DataValueField = "userId";
                        ddlUserName.DataTextField = "userName";
                        ddlUserName.DataBind();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlUserName.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }


    #endregion
    #region Bind Menu Option
    public void BindMenuOption()
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
                        + "menuOptions?activeStatus=A";


                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow dr;
                                dr = dt.Rows[i];

                                if (Session["branchOptions"].ToString().Trim() == "Y")
                                {
                                    if (Session["blockOption"].ToString().Trim() == "N")
                                    {
                                        if (dt.Rows[i]["optionName"].ToString() == "blockMaster")
                                        {
                                            dr.Delete();
                                        }
                                    }
                                    if (Session["floorOption"].ToString().Trim() == "N")
                                    {
                                        if (dt.Rows[i]["optionName"].ToString() == "floorMaster")
                                        {
                                            dr.Delete();
                                        }

                                    }
                                    if (Session["slotsOption"].ToString().Trim() == "N")
                                    {
                                        if (dt.Rows[i]["optionName"].ToString() == "slot")
                                        {
                                            dr.Delete();
                                        }

                                    }
                                }

                            }
                            dt.AcceptChanges();

                            gvOption.DataSource = dt;
                            gvOption.DataBind();
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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

    #endregion
    #region Bind Menu Option Access
    public void BindMenuOptionAccess()
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
                        + "menuOptionAccess?branchId=" + Session["branchId"].ToString() + "";


                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        List<menuOptionAccess> Option = JsonConvert.DeserializeObject<List<menuOptionAccess>>(ResponseMsg);
                        List<menuOptionAccess> filteredOption = Option.Where<menuOptionAccess>(row => row.optionDetails != null).ToList();

                        if (filteredOption.Count > 0)
                        {
                            gvMenuAccess.DataSource = filteredOption;
                            gvMenuAccess.DataBind();
                            gvMenuAccess.Visible = true;
                        }
                        else
                        {
                            spAddorEdit.InnerText = "Add ";
                            BindddlUser();
                            ADD();
                            gvMenuAccess.DataSource = null;
                            gvMenuAccess.Visible = false;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
                        spAddorEdit.InnerText = "Add ";
                        BindddlUser();
                        ADD();
                        gvMenuAccess.DataSource = null;
                        gvMenuAccess.Visible = false;
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

    protected void gvMenuAccess_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != -1)
        {
            var Option = e.Row.DataItem as menuOptionAccess;
            var dataList = e.Row.FindControl("DataList1") as DataList;
            dataList.DataSource = Option.optionDetails;
            dataList.DataBind();
        }
    }

    #endregion
    #region Bind Menu Option Access user
    public void BindMenuOptionAccessuser()
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
                        + "menuOptionAccess?userId=" + ddlUserName.SelectedValue + "";


                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        List<menuOptionAccess> Options = JsonConvert.DeserializeObject<List<menuOptionAccess>>(ResponseMsg);
                        DataTable dt1 = ConvertToDataTable(Options);
                        if (dt1.Rows.Count > 0)
                        {
                            GridView1.DataSource = Options;
                            GridView1.DataBind();
                            for (int j = 0; j < GridView1.Rows.Count; j++)
                            {
                                Label lblgv1userId = GridView1.Rows[j].FindControl("lblgv1userId") as Label;
                                var dataList = GridView1.Rows[j].FindControl("DataList12") as DataList;
                                int count = dataList.Items.Count;
                                string optionId = string.Empty;
                                string MenuOptionAccessId = string.Empty;
                                string AddRights = string.Empty;
                                string ViewRights = string.Empty;
                                string EditRights = string.Empty;
                                string DeleteRights = string.Empty;
                                string Activestatus = string.Empty;
                                DataTable dt = new DataTable();
                                dt.Columns.Add("optionId");
                                dt.Columns.Add("optionName");
                                dt.Columns.Add("menuOptionActiveStatus");
                                dt.Columns.Add("MenuOptionAccessId");
                                dt.Columns.Add("MenuOptionAccessActiveStatus");
                                dt.Columns.Add("AddRights");
                                dt.Columns.Add("EditRights");
                                dt.Columns.Add("ViewRights");
                                dt.Columns.Add("DeleteRights");

                                for (int i = 0; i < count; i++)
                                {
                                    Label lbloptionId = dataList.Items[i].FindControl("lbloptionId1") as Label;
                                    Label lbloptionName = dataList.Items[i].FindControl("lbloptionName1") as Label;
                                    Label lblmenuOptionActiveStatus = dataList.Items[i].FindControl("lblmenuOptionActiveStatus1") as Label;
                                    Label lblMenuOptionAccessId = dataList.Items[i].FindControl("lblMenuOptionAccessId1") as Label;
                                    Label lblMenuOptionAccessActiveStatus = dataList.Items[i].FindControl("lblMenuOptionAccessActiveStatus1") as Label;
                                    Label lblopAddRights = dataList.Items[i].FindControl("lblopAddRights1") as Label;
                                    Label lblopEditRights = dataList.Items[i].FindControl("lblopEditRights1") as Label;
                                    Label lblopViewRights = dataList.Items[i].FindControl("lblopViewRights1") as Label;
                                    Label lblopDeleteRights = dataList.Items[i].FindControl("lblopDeleteRights1") as Label;
                                    dt.NewRow();
                                    dt.Rows.Add(lbloptionId.Text, lbloptionName.Text, lblmenuOptionActiveStatus.Text, lblMenuOptionAccessId.Text,
                                        lblMenuOptionAccessActiveStatus.Text,
                                        lblopAddRights.Text, lblopEditRights.Text, lblopViewRights.Text, lblopDeleteRights.Text);
                                    optionId += lbloptionId.Text.Trim() + ',';
                                    Activestatus += lblMenuOptionAccessActiveStatus.Text.Trim() + ',';
                                    MenuOptionAccessId += lblMenuOptionAccessId.Text.Trim() + ',';

                                    AddRights += lblopAddRights.Text + ',';
                                    ViewRights += lblopViewRights.Text + ',';
                                    EditRights += lblopEditRights.Text + ',';
                                    DeleteRights += lblopDeleteRights.Text + ',';
                                }
                                List<optionDetails> CategoryList = new List<optionDetails>();
                                CategoryList = (from DataRow dr in dt.Rows
                                                select new optionDetails()
                                                {
                                                    optionId = dr["optionId"].ToString(),
                                                    MenuOptionAccessActiveStatus = dr["MenuOptionAccessActiveStatus"].ToString(),

                                                }).ToList();

                                string[] sbBoatSvc = optionId.Split(',');

                                int bBoatSvcCount = sbBoatSvc.Count();

                                string[] AddRightss;
                                string[] EditRightss;
                                string[] ViewRightss;
                                string[] DeleteRightss;
                                //string[] FormRightss;
                                string FormRights = string.Empty;
                                AddRightss = AddRights.Split(',');
                                EditRightss = EditRights.Split(',');
                                ViewRightss = ViewRights.Split(',');
                                DeleteRightss = DeleteRights.Split(',');
                                for (int i = 0; i < gvOption.Rows.Count; i++)
                                {
                                    Label lbloptionId = gvOption.Rows[i].FindControl("lbloptionId") as Label;
                                    CheckBox CheckBox1 = gvOption.Rows[i].FindControl("CheckBox1") as CheckBox;
                                    if (sbBoatSvc.Contains(lbloptionId.Text))
                                    {
                                        var status = CategoryList.Where(x => x.optionId == lbloptionId.Text).Select(x => x.MenuOptionAccessActiveStatus).ToList();
                                        if (status[0].ToString() == "A")
                                        {
                                            FormRights += "A" + ',';
                                            CheckBox1.Checked = true;
                                        }
                                        else
                                        {
                                            FormRights += "D" + ',';
                                            CheckBox1.Checked = false;

                                        }
                                    }
                                    else
                                    {
                                        FormRights += "D" + ',';
                                        CheckBox1.Checked = false;

                                    }


                                    CheckBox AddCheckBox1 = gvOption.Rows[i].FindControl("AddCheckBox1") as CheckBox;
                                    if (AddRightss[i] == "True")
                                    {
                                        AddCheckBox1.Checked = true;
                                    }
                                    else
                                    {
                                        AddCheckBox1.Checked = false;
                                    }
                                    CheckBox ViewCheckBox1 = gvOption.Rows[i].FindControl("ViewCheckBox1") as CheckBox;
                                    if (ViewRightss[i] == "True")
                                    {
                                        ViewCheckBox1.Checked = true;
                                    }
                                    else
                                    {
                                        ViewCheckBox1.Checked = false;
                                    }
                                    CheckBox EditCheckBox1 = gvOption.Rows[i].FindControl("EditCheckBox1") as CheckBox;
                                    if (EditRightss[i] == "True")
                                    {

                                        EditCheckBox1.Checked = true;
                                    }
                                    else
                                    {
                                        EditCheckBox1.Checked = false;
                                    }
                                    CheckBox DeleteCheckBox1 = gvOption.Rows[i].FindControl("DeleteCheckBox1") as CheckBox;
                                    if (DeleteRightss[i] == "True")
                                    {

                                        DeleteCheckBox1.Checked = true;
                                    }
                                    else
                                    {
                                        DeleteCheckBox1.Checked = false;
                                    }
                                    if (CheckBox1.Checked == true)
                                    {
                                        ViewCheckBox1.Enabled = true;
                                        EditCheckBox1.Enabled = true;
                                        DeleteCheckBox1.Enabled = true;
                                        AddCheckBox1.Enabled = true;

                                    }
                                    else
                                    {

                                        ViewCheckBox1.Enabled = false;
                                        EditCheckBox1.Enabled = false;
                                        DeleteCheckBox1.Enabled = false;
                                        AddCheckBox1.Enabled = false;
                                    }

                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < gvOption.Rows.Count; i++)
                            {
                                CheckBox CheckBox1 = gvOption.Rows[i].FindControl("CheckBox1") as CheckBox;

                                CheckBox1.Checked = false;

                                CheckBox AddCheckBox1 = gvOption.Rows[i].FindControl("AddCheckBox1") as CheckBox;

                                AddCheckBox1.Checked = false;

                                CheckBox ViewCheckBox1 = gvOption.Rows[i].FindControl("ViewCheckBox1") as CheckBox;

                                ViewCheckBox1.Checked = false;

                                CheckBox EditCheckBox1 = gvOption.Rows[i].FindControl("EditCheckBox1") as CheckBox;
                                EditCheckBox1.Checked = false;

                                CheckBox DeleteCheckBox1 = gvOption.Rows[i].FindControl("DeleteCheckBox1") as CheckBox;
                                DeleteCheckBox1.Checked = false;

                            }


                        }
                    }
                    else
                    {
                        for (int i = 0; i < gvOption.Rows.Count; i++)
                        {
                            CheckBox CheckBox1 = gvOption.Rows[i].FindControl("CheckBox1") as CheckBox;

                            CheckBox1.Checked = false;

                            CheckBox AddCheckBox1 = gvOption.Rows[i].FindControl("AddCheckBox1") as CheckBox;

                            AddCheckBox1.Checked = false;

                            CheckBox ViewCheckBox1 = gvOption.Rows[i].FindControl("ViewCheckBox1") as CheckBox;

                            ViewCheckBox1.Checked = false;

                            CheckBox EditCheckBox1 = gvOption.Rows[i].FindControl("EditCheckBox1") as CheckBox;
                            EditCheckBox1.Checked = false;

                            CheckBox DeleteCheckBox1 = gvOption.Rows[i].FindControl("DeleteCheckBox1") as CheckBox;
                            DeleteCheckBox1.Checked = false;

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

    #endregion
    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        spAddorEdit.InnerText = "Add ";
        BindddlUser();
        ADD();

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
        string optionId = string.Empty;
        string Activestatus = string.Empty;
        string AddRights = string.Empty;
        string ViewRights = string.Empty;
        string EditRights = string.Empty;
        string DeleteRights = string.Empty;
        int Fi = 0, Ai = 0, Ei = 0, Di = 0, Vi = 0;
        foreach (GridViewRow item in gvOption.Rows)
        {
            Label lbloptionId = item.FindControl("lbloptionId") as Label;
            CheckBox CheckBox1 = item.FindControl("CheckBox1") as CheckBox;

            if (CheckBox1.Checked == true)
            {
                Fi = 1;
                Activestatus += "A" + ',';
            }
            else
            {
                Activestatus += "D" + ',';
            }
            CheckBox AddCheckBox1 = item.FindControl("AddCheckBox1") as CheckBox;
            if (AddCheckBox1.Checked == true)
            {
                Ai = 1;
                AddRights += "true" + ',';
            }
            else
            {
                AddRights += "false" + ',';
            }
            CheckBox ViewCheckBox1 = item.FindControl("ViewCheckBox1") as CheckBox;
            if (ViewCheckBox1.Checked == true)
            {
                Vi = 1;
                ViewRights += "true" + ',';
            }
            else
            {
                ViewRights += "false" + ',';
            }
            CheckBox EditCheckBox1 = item.FindControl("EditCheckBox1") as CheckBox;
            if (EditCheckBox1.Checked == true)
            {
                Ei = 1;
                EditRights += "true" + ',';
            }
            else
            {
                EditRights += "false" + ',';
            }
            CheckBox DeleteCheckBox1 = item.FindControl("DeleteCheckBox1") as CheckBox;
            if (DeleteCheckBox1.Checked == true)
            {
                Di = 1;
                DeleteRights += "true" + ',';
            }
            else
            {
                DeleteRights += "false" + ',';
            }
            optionId += lbloptionId.Text + ',';

        }
        if (Fi == 0)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Check Any One Form Rights ');", true);
            return;
        }
        //if (Ai == 0)
        //{
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Check  Add Rights ');", true);
        //    return;
        //}
        //if (Ei == 0)
        //{
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Check  Edit Rights ');", true);
        //    return;
        //}
        //if (Di == 0)
        //{
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Check  Delete Rights ');", true);
        //    return;
        //}
        //if (Vi == 0)
        //{
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Check  View Rights ');", true);
        //    return;
        //}
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                InsertmenuOptionAccess Insert = new InsertmenuOptionAccess()
                {
                    parkingOwnerId = Session["ParkingOwnerId"].ToString() == "0" ? null : Session["ParkingOwnerId"].ToString(),
                    userId = ddlUserName.SelectedValue.Trim() == "" ? null : ddlUserName.SelectedValue.Trim(),
                    moduleId = "0",
                    optionDetails = GetOptionDetails(optionId.ToString().TrimEnd(','), Activestatus.ToString().TrimEnd(','),
                     AddRights.ToString().TrimEnd(','), EditRights.ToString().TrimEnd(','), ViewRights.ToString().TrimEnd(','),
                      DeleteRights.ToString().TrimEnd(',')),
                    createdBy = Session["UserId"].ToString() == "" ? null : Session["UserId"].ToString()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("menuOptionAccess", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

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
    public static List<InsertoptionDetails> GetOptionDetails(string optionId, string activeStatus,
        string AddRights, string EditRights, string ViewRights, string DeleteRights)
    {
        string[] optionIds;
        string[] activeStatuss;
        string[] AddRightss;
        string[] EditRightss;
        string[] ViewRightss;
        string[] DeleteRightss;
        AddRightss = AddRights.Split(',');
        EditRightss = EditRights.Split(',');
        ViewRightss = ViewRights.Split(',');
        DeleteRightss = DeleteRights.Split(',');
        optionIds = optionId.Split(',');
        activeStatuss = activeStatus.Split(',');
        List<InsertoptionDetails> lst = new List<InsertoptionDetails>();
        for (int i = 0; i < optionIds.Count(); i++)
        {
            lst.AddRange(new List<InsertoptionDetails>
            {
                new InsertoptionDetails { optionId=optionIds[i] ,ViewRights=ViewRightss[i],AddRights=AddRightss[i],EditRights=EditRightss[i],
               DeleteRights=DeleteRightss[i] ,activeStatus=activeStatuss[i]}

            });
        }

        return lst;

    }
    #endregion
    #region Update Function
    public void UpdateEmployee()
    {
        string optionId = string.Empty;
        string Activestatus = string.Empty;
        string AddRights = string.Empty;
        string ViewRights = string.Empty;
        string EditRights = string.Empty;
        string DeleteRights = string.Empty;
        int Fi = 0, Ai = 0, Ei = 0, Di = 0, Vi = 0;
        string MenuOptionAccessId = string.Empty;

        foreach (GridViewRow item in gvOption.Rows)
        {
            Label lbloptionId = item.FindControl("lbloptionId") as Label;
            CheckBox CheckBox1 = item.FindControl("CheckBox1") as CheckBox;
            if (CheckBox1.Checked == true)
            {
                Fi = 1;
                Activestatus += "A" + ',';
            }
            else
            {
                Activestatus += "D" + ',';
            }
            CheckBox AddCheckBox1 = item.FindControl("AddCheckBox1") as CheckBox;
            if (AddCheckBox1.Checked == true)
            {
                Ai = 1;
                AddRights += "true" + ',';
            }
            else
            {
                AddRights += "false" + ',';
            }
            CheckBox ViewCheckBox1 = item.FindControl("ViewCheckBox1") as CheckBox;
            if (ViewCheckBox1.Checked == true)
            {
                Vi = 1;
                ViewRights += "true" + ',';
            }
            else
            {
                ViewRights += "false" + ',';
            }
            CheckBox EditCheckBox1 = item.FindControl("EditCheckBox1") as CheckBox;
            if (EditCheckBox1.Checked == true)
            {
                Ei = 1;
                EditRights += "true" + ',';
            }
            else
            {
                EditRights += "false" + ',';
            }
            CheckBox DeleteCheckBox1 = item.FindControl("DeleteCheckBox1") as CheckBox;
            if (DeleteCheckBox1.Checked == true)
            {
                Di = 1;
                DeleteRights += "true" + ',';
            }
            else
            {
                DeleteRights += "false" + ',';
            }
            optionId += lbloptionId.Text + ',';
        }
        if (Fi == 0)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Check Any One Form Rights ');", true);
            return;
        }

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                UpdatemenuOptionAccess Update = new UpdatemenuOptionAccess()
                {
                    menuOptionAccessDetails = GetUpdateOptionDetails(optionId.ToString().TrimEnd(','),
                    Activestatus.ToString().TrimEnd(','), ViewState["MenuOptionAccessId"].ToString().TrimEnd(','),
                    Session["UserId"].ToString(), AddRights.ToString().TrimEnd(','), ViewRights.ToString().TrimEnd(','),
                    EditRights.ToString().TrimEnd(','), DeleteRights.ToString().TrimEnd(',')),

                };
                HttpResponseMessage response = client.PutAsJsonAsync("menuOptionAccess", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        btnSubmit.Text = "Submit";
                        BindMenuOption();
                        BindMenuOptionAccess();
                        ViewState["MenuOptionAccessId"] = "";
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

    public static List<menuOptionAccessDetails> GetUpdateOptionDetails(string optionId, string activeStatus,
        string MenuOptionAccessId, string updatedBy, string Addrights, string ViewRights, string EditRights, string DeleteRights)
    {
        string[] optionIds;
        string[] activeStatuss;
        string[] Addrightss;
        string[] ViewRightss;
        string[] EditRightss;
        string[] DeleteRightss;
        string[] MenuOptionAccessIds;
        optionIds = optionId.Split(',');
        activeStatuss = activeStatus.Split(',');
        MenuOptionAccessIds = MenuOptionAccessId.Split(',');
        Addrightss = Addrights.Split(',');
        ViewRightss = ViewRights.Split(',');
        EditRightss = EditRights.Split(',');
        DeleteRightss = DeleteRights.Split(',');

        List<menuOptionAccessDetails> lsts = new List<menuOptionAccessDetails>();
        for (int i = 0; i < optionIds.Count(); i++)
        {
            lsts.AddRange(new List<menuOptionAccessDetails>
            {
                new menuOptionAccessDetails { optionId=optionIds[i],activeStatus=activeStatuss[i],
                    MenuOptionAccessId =MenuOptionAccessIds[i],
                    updatedBy=updatedBy,viewRights=ViewRightss[i],addRights=Addrightss[i],
                editRights=EditRightss[i],deleteRights=DeleteRightss[i]}  });
        }

        return lsts;

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
        BindMenuOptionAccess();
        spAddorEdit.InnerText = "";
        divGv.Visible = true;
        divForm.Visible = false;
        ddlUserName.ClearSelection();
        //ddlUsernamegrid.ClearSelection();
        ddlUserName.Enabled = true;
    }
    #endregion
    #region Add Click Fucntion
    public void ADD()
    {
        divGv.Visible = false;
        divForm.Visible = true;
        ddlUserName.Enabled = true;
        btnSubmit.Text = "Submit";
        BindMenuOption();
    }
    #endregion
    #region Edit Click
    protected void LnkEdit_Click(object sender, EventArgs e)
    {
        ViewState["MenuOptionAccessId"] = "";
        BindMenuOption();
        try
        {
            spAddorEdit.InnerText = "Edit ";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvuserId = gvrow.FindControl("lblgvuserId") as Label;
            BindddlUser();
            ddlUserName.SelectedValue = lblgvuserId.Text;
            ddlUserName.Enabled = false;
            var dataList = gvrow.FindControl("DataList1") as DataList;
            int count = dataList.Items.Count;
            string optionId = string.Empty;
            string MenuOptionAccessId = string.Empty;
            string AddRights = string.Empty;
            string ViewRights = string.Empty;
            string EditRights = string.Empty;
            string DeleteRights = string.Empty;
            string Activestatus = string.Empty;
            DataTable dt = new DataTable();
            dt.Columns.Add("optionId");
            dt.Columns.Add("optionName");
            dt.Columns.Add("menuOptionActiveStatus");
            dt.Columns.Add("MenuOptionAccessId");
            dt.Columns.Add("MenuOptionAccessActiveStatus");
            dt.Columns.Add("AddRights");
            dt.Columns.Add("EditRights");
            dt.Columns.Add("ViewRights");
            dt.Columns.Add("DeleteRights");
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Label lbloptionId = dataList.Items[i].FindControl("lbloptionId") as Label;
                    Label lbloptionName = dataList.Items[i].FindControl("lbloptionName") as Label;
                    Label lblmenuOptionActiveStatus = dataList.Items[i].FindControl("lblmenuOptionActiveStatus") as Label;
                    Label lblMenuOptionAccessId = dataList.Items[i].FindControl("lblMenuOptionAccessId") as Label;
                    Label lblMenuOptionAccessActiveStatus = dataList.Items[i].FindControl("lblMenuOptionAccessActiveStatus") as Label;
                    Label lblopAddRights = dataList.Items[i].FindControl("lblopAddRights") as Label;
                    Label lblopEditRights = dataList.Items[i].FindControl("lblopEditRights") as Label;
                    Label lblopViewRights = dataList.Items[i].FindControl("lblopViewRights") as Label;
                    Label lblopDeleteRights = dataList.Items[i].FindControl("lblopDeleteRights") as Label;
                    dt.NewRow();
                    dt.Rows.Add(lbloptionId.Text, lbloptionName.Text, lblmenuOptionActiveStatus.Text, lblMenuOptionAccessId.Text,
                        lblMenuOptionAccessActiveStatus.Text,
                        lblopAddRights.Text, lblopEditRights.Text, lblopViewRights.Text, lblopDeleteRights.Text);
                    optionId += lbloptionId.Text.Trim() + ',';
                    Activestatus += lblMenuOptionAccessActiveStatus.Text.Trim() + ',';
                    MenuOptionAccessId += lblMenuOptionAccessId.Text.Trim() + ',';
                    ViewState["MenuOptionAccessId"] = MenuOptionAccessId.ToString().Trim();
                    AddRights += lblopAddRights.Text + ',';
                    ViewRights += lblopViewRights.Text + ',';
                    EditRights += lblopEditRights.Text + ',';
                    DeleteRights += lblopDeleteRights.Text + ',';
                }
                List<optionDetails> CategoryList = new List<optionDetails>();
                CategoryList = (from DataRow dr in dt.Rows
                                select new optionDetails()
                                {
                                    optionId = dr["optionId"].ToString(),
                                    MenuOptionAccessActiveStatus = dr["MenuOptionAccessActiveStatus"].ToString(),

                                }).ToList();

                string[] sbBoatSvc = optionId.Split(',');

                int bBoatSvcCount = sbBoatSvc.Count();

                string[] AddRightss;
                string[] EditRightss;
                string[] ViewRightss;
                string[] DeleteRightss;
                //string[] FormRightss;
                string FormRights = string.Empty;
                AddRightss = AddRights.Split(',');
                EditRightss = EditRights.Split(',');
                ViewRightss = ViewRights.Split(',');
                DeleteRightss = DeleteRights.Split(',');
                for (int i = 0; i < gvOption.Rows.Count; i++)
                {
                    Label lbloptionId = gvOption.Rows[i].FindControl("lbloptionId") as Label;
                    CheckBox CheckBox1 = gvOption.Rows[i].FindControl("CheckBox1") as CheckBox;
                    if (sbBoatSvc.Contains(lbloptionId.Text))
                    {
                        var status = CategoryList.Where(x => x.optionId == lbloptionId.Text).Select(x => x.MenuOptionAccessActiveStatus).ToList();
                        if (status[0].ToString() == "A")
                        {
                            FormRights += "A" + ',';
                            CheckBox1.Checked = true;
                        }
                        else
                        {
                            FormRights += "D" + ',';
                            CheckBox1.Checked = false;

                        }
                    }
                    else
                    {
                        FormRights += "D" + ',';
                        CheckBox1.Checked = false;

                    }

                    CheckBox AddCheckBox1 = gvOption.Rows[i].FindControl("AddCheckBox1") as CheckBox;
                    if (AddRightss[i] == "True")
                    {
                        AddCheckBox1.Checked = true;
                    }
                    else
                    {
                        AddCheckBox1.Checked = false;
                    }
                    CheckBox ViewCheckBox1 = gvOption.Rows[i].FindControl("ViewCheckBox1") as CheckBox;
                    if (ViewRightss[i] == "True")
                    {
                        ViewCheckBox1.Checked = true;
                    }
                    else
                    {
                        ViewCheckBox1.Checked = false;
                    }
                    CheckBox EditCheckBox1 = gvOption.Rows[i].FindControl("EditCheckBox1") as CheckBox;
                    if (EditRightss[i] == "True")
                    {

                        EditCheckBox1.Checked = true;
                    }
                    else
                    {
                        EditCheckBox1.Checked = false;
                    }
                    CheckBox DeleteCheckBox1 = gvOption.Rows[i].FindControl("DeleteCheckBox1") as CheckBox;
                    if (DeleteRightss[i] == "True")
                    {

                        DeleteCheckBox1.Checked = true;
                    }
                    else
                    {
                        DeleteCheckBox1.Checked = false;
                    }
                    if (CheckBox1.Checked == true)
                    {
                        ViewCheckBox1.Enabled = true;
                        EditCheckBox1.Enabled = true;
                        DeleteCheckBox1.Enabled = true;
                        AddCheckBox1.Enabled = true;

                    }
                    else
                    {

                        ViewCheckBox1.Enabled = false;
                        EditCheckBox1.Enabled = false;
                        DeleteCheckBox1.Enabled = false;
                        AddCheckBox1.Enabled = false;
                    }




                }
            }


            btnSubmit.Text = "Update";
            divGv.Visible = false;
            divForm.Visible = true;
        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string excp = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + excp + "');", true);
        }
    }
    #endregion
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
    #region Insert menu Option Access
    public class InsertmenuOptionAccess
    {
        public String parkingOwnerId { get; set; }
        public String userId { get; set; }
        public String moduleId { get; set; }

        public List<InsertoptionDetails> optionDetails { get; set; }

        public String createdBy { get; set; }
    }
    public class InsertoptionDetails
    {
        public string optionId { get; set; }
        public string activeStatus { get; set; }
        public string AddRights { get; set; }
        public string EditRights { get; set; }
        public string ViewRights { get; set; }
        public string DeleteRights { get; set; }
    }


    #endregion
    #region Update menu Option Access
    public class UpdatemenuOptionAccess
    {
        //public String userId { get; set; }
        public List<menuOptionAccessDetails> menuOptionAccessDetails { get; set; }

        //public String updatedBy { get; set; }
    }
    public class menuOptionAccessDetails
    {
        public string MenuOptionAccessId { get; set; }
        public string optionId { get; set; }
        public string updatedBy { get; set; }
        public string viewRights { get; set; }
        public string addRights { get; set; }
        public string editRights { get; set; }
        public string deleteRights { get; set; }
        public string activeStatus { get; set; }
    }


    #endregion
    protected void ddlUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindMenuOptionAccessuser();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != -1)
        {
            var Options = e.Row.DataItem as menuOptionAccess;
            var dataList = e.Row.FindControl("DataList12") as DataList;
            dataList.DataSource = Options.optionDetails;
            dataList.DataBind();
        }
    }

    /// <summary>
    /// menu Access Rights 
    /// Created By Abhinaya K
    /// Created Date 02-AUG-2022
    /// </summary>

    #region Get Method
    public void BindMenuaccess()
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
                        var Option = lst1.Where(x => x.optionName == "menuAccessRights" && x.MenuOptionAccessActiveStatus == "A")
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
                                    gvMenuAccess.Columns[4].Visible = true;
                                }
                                else
                                {
                                    gvMenuAccess.Columns[4].Visible = false;
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