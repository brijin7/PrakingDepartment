﻿using System;
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

public partial class Master_PrintingInsSA : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null && Session["UserRole"] == null)
        {
            Session.Clear();
              Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
        }
        if (!IsPostBack)
        {
            BindPrintingInstructions();
        }
        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }
    }
    #endregion

    #region Bind Printing Instructions
    public void BindPrintingInstructions()
    {
        try
        {
            string sUrl = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                sUrl = Session["BaseUrl"].ToString().Trim() + "printingInstructionsConfig?type=SA";

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
                            gvPrintingInstructions.DataSource = dt;
                            gvPrintingInstructions.DataBind();
                        }
                        else
                        {
                            gvPrintingInstructions.DataSource = null;
                            gvPrintingInstructions.DataBind();
                        }
                    }
                    else
                    {
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
    #endregion

    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            InsertPrintingInstructions();
        }
        else
        {
            UpdatePrintingInstructions();
        }
    }
    #endregion

    #region Insert Function
    public void InsertPrintingInstructions()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new PrintingInstructionsClass()
                {
                    instructionType = rbtnlinstructionType.SelectedValue,
                    instructions = txtinstructions.Text,
                    createdBy = Session["UserId"].ToString(),
                };
               
                HttpResponseMessage response = client.PostAsJsonAsync("printingInstructionsConfig", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindPrintingInstructions();
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
    public void UpdatePrintingInstructions()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new PrintingInstructionsClass()
                {
                    instructionType = rbtnlinstructionType.SelectedValue,
                    instructions = txtinstructions.Text,
                    uniqueId = hfuniqueId.Value,                   
                    updatedBy = Session["UserId"].ToString()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("printingInstructionsConfig", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        btnSubmitText();
                        BindPrintingInstructions();
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

    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ADD();
    }
    #endregion

    #region Add Click Fucntion
    public void ADD()
    {
        divGv.Visible = false;
        divForm.Visible = true;
        spAddorEdit.InnerText = "Add ";
        rbtnlinstructionType.Enabled = true;
    }
    #endregion

    #region Update Text
    public void btnUpdateText()
    {
        btnSubmit.Text = "Update";
    }
    #endregion

    #region Submit Text
    public void btnSubmitText()
    {
        btnSubmit.Text = "Submit";
    }
    #endregion    

    #region Cancel Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    #endregion

    #region Cancel Click Fucntion
    public void Cancel()
    {
        divGv.Visible = true;
        divForm.Visible = false;
        txtinstructions.Text = "";
        rbtnlinstructionType.ClearSelection();
        btnSubmitText();
        spAddorEdit.InnerText = "";
        rbtnlinstructionType.Enabled = true;
    }
    #endregion

    #region Edit Click 
    protected void LnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            btnUpdateText();
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvinstructionType = (Label)gvrow.FindControl("lblgvinstructionType");
            Label lblgvinstructions = (Label)gvrow.FindControl("lblgvinstructions");
            Label lblgvuniqueId = (Label)gvrow.FindControl("lblgvuniqueId");
            string sgvPrintingInstructions = gvPrintingInstructions.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["instructionType"] = lblgvinstructionType.Text == "Pass" ? "P" : "R";
            ViewState["uniqueId"] = sgvPrintingInstructions.ToString().Trim();
            rbtnlinstructionType.SelectedValue = ViewState["instructionType"].ToString();
            txtinstructions.Text = lblgvinstructions.Text;
            hfuniqueId.Value = lblgvuniqueId.Text;
            ADD();
            spAddorEdit.InnerText = "Edit ";
            rbtnlinstructionType.Enabled = false;
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    #endregion

    #region Printing Instructions  Class
    public class PrintingInstructionsClass
    {      
        public String activeStatus { get; set; }      
        public String instructionType { get; set; }
        public String instructions { get; set; }
        public String uniqueId { get; set; }
        public String createdBy { get; set; }
        public String updatedBy { get; set; }

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
                Label lbluniqueId = (Label)gvrow.FindControl("lblgvuniqueId");
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "printingInstructionsConfig?uniqueId=" + lbluniqueId.Text
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
                        BindPrintingInstructions();
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
                        var Option = lst1.Where(x => x.optionName == "printingInstructions" && x.MenuOptionAccessActiveStatus == "A")
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
                                    gvPrintingInstructions.Columns[7].Visible = true;
                                }
                                else
                                {
                                    gvPrintingInstructions.Columns[7].Visible = false;
                                }
                                if (Delete[0] == "True")
                                {
                                    gvPrintingInstructions.Columns[8].Visible = true;
                                }
                                else
                                {
                                    gvPrintingInstructions.Columns[8].Visible = false;
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
}