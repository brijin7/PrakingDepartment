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

public partial class Master_BlockMaster : System.Web.UI.Page
{
    #region Page Load
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
            BindBlockMaster();            
        }
        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }

        if (Session["UserRole"].ToString().Trim() == "SA")
        {
            gvBlockmaster.Columns[6].Visible = true;
            gvBlockmaster.Columns[7].Visible = true;
            gvBlockmaster.Columns[8].Visible = true;
          

        }
        else if (Session["UserRole"].ToString().Trim() == "A")
        {
            gvBlockmaster.Columns[6].Visible = true;
            gvBlockmaster.Columns[7].Visible = false;
            gvBlockmaster.Columns[8].Visible = false;
        }
    }
    #endregion
    #region Cancel Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    #endregion
    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            InsertBlock();
        }
        else
        {
            UpdateBlockMaster();
        }
    }
    #endregion
    #region Cancel Click Fucntion
    public void Cancel()
    {
        modalCard.Visible = false;
        txtReason.Text = "";
        divCancellationReason.Visible = false;
        divsubmit.Visible = false;
        divGv.Visible = true;
        divForm.Visible = false;
        btnSubmitText();
        txtBlockName.Text = "";
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
    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        spAddorEdit.InnerText = "Add ";
        ADD();
    }
    #endregion
    #region Insert Function
    public void InsertBlock()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new BlockMasterClass()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    blockName = txtBlockName.Text,
                    activeStatus = "A",
                    approvalStatus = Session["approvalStatus"].ToString().Trim(),
                    createdBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("blockMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindBlockMaster();
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
    public void UpdateBlockMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new BlockMasterClass()
                {
                    branchId = Session["branchId"].ToString(),
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    blockName = txtBlockName.Text,
                    blockId = hfBlockId.Value,
                    approvalStatus = ViewState["approvalStatus"].ToString().Trim(),
                    updatedBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("blockMaster", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        btnSubmitText();
                        BindBlockMaster();
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
    #region Block  Master Class
    public class BlockMasterClass
    {
        public String parkingOwnerId { get; set; }
        public String activeStatus { get; set; }
        public String branchId { get; set; }
        public String blockName { get; set; }
        public String blockId { get; set; }
        public String approvalStatus { get; set; }
        public String createdBy { get; set; }
        public String updatedBy { get; set; }

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
    #region Edit Click
    protected void LnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            spAddorEdit.InnerText = "Edit ";
            btnUpdateText();
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvblockName = (Label)gvrow.FindControl("lblgvblockName");
            Label lblBlockId = (Label)gvrow.FindControl("lblgvblockId");
            Label lblgvbranchId = (Label)gvrow.FindControl("lblgvbranchId");
            Label lblgvparkingOwnerId = (Label)gvrow.FindControl("lblgvparkingOwnerId");
            Label lblgvapprovalStatus = (Label)gvrow.FindControl("lblgvapprovalStatus");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string sgvBlockmaster = gvBlockmaster.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.Text.ToString() == "Active" ? "A" : "D";
            ViewState["approvalStatus"] = lblgvapprovalStatus.Text.Trim();
            ViewState["blockId"] = sgvBlockmaster.ToString().Trim();
            txtBlockName.Text = lblgvblockName.Text;
            hfBlockId.Value = lblBlockId.Text;
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
                string sBlockId = gvBlockmaster.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "blockMaster?blockId=" + sBlockId
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
                        BindBlockMaster();
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
    #region Bind Block  Master 
    public void BindBlockMaster()
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
                      + "blockMaster?branchId=";
                sUrl += Session["branchId"].ToString().Trim();

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
                            gvBlockmaster.DataSource = dt;
                            gvBlockmaster.DataBind();
                        }
                        else
                        {
                            spAddorEdit.InnerText = "Add ";
                            ADD();
                            gvBlockmaster.DataSource = null;
                            gvBlockmaster.DataBind();
                        }

                    }
                    else
                    {
                        spAddorEdit.InnerText = "Add ";
                        ADD();
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
                        var Option = lst1.Where(x => x.optionName == "blockMaster" && x.MenuOptionAccessActiveStatus == "A")
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
                                    gvBlockmaster.Columns[6].Visible = true;
                                }
                                else
                                {
                                    gvBlockmaster.Columns[6].Visible = false;
                                }
                                if (Delete[0] == "True")
                                {
                                    gvBlockmaster.Columns[7].Visible = true;
                                }
                                else
                                {
                                    gvBlockmaster.Columns[7].Visible = false;
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
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('You donot have Access right to this Form');", true);
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

    #region Update Approval Status
    protected void ImgClosed_Click(object sender, ImageClickEventArgs e)
    {
        modalCard.Visible = false;
        txtReason.Text = "";
        divCancellationReason.Visible = false;
        divsubmit.Visible = false;
    }

    protected void btnSubmitPopup_Click(object sender, EventArgs e)
    {
        ChangeApprovalStatus(ViewState["ApproveBranchId"].ToString(), "N");
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        modalCard.Visible = false;
        txtReason.Text = "";
        divCancellationReason.Visible = false;
        divsubmit.Visible = false;
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        divCancellationReason.Visible = false;
        divsubmit.Visible = false;
        ChangeApprovalStatus(ViewState["ApproveBlockId"].ToString(), "A");
    }

    protected void btnCancelModal_Click(object sender, EventArgs e)
    {
        divCancellationReason.Visible = true;
        divsubmit.Visible = true;
    }
    protected void lblapprovalStatus_Click(object sender, EventArgs e)
    {
        modalCard.Visible = true;
        divCancellationReason.Visible = false;
        divsubmit.Visible = false;
        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        LinkButton lblapprovalStatus = (LinkButton)gvrow.FindControl("lblapprovalStatus");
        Label lblgvblockId = (Label)gvrow.FindControl("lblgvblockId");
        string sActiveStatus = lblapprovalStatus.Text.Trim() == "Waiting List" ? "W" : "A";
        ViewState["ApproveBlockId"] = lblgvblockId.Text;
        ViewState["ApproveStatus"] = sActiveStatus.Trim();

    }

    public void ChangeApprovalStatus(string blockId, string ApprovalStatus)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim() + "approvalStatus?blockId=" + blockId + "&approvalStatus=" + ApprovalStatus + "";
                sUrl += " cancellationReason = " + txtReason.Text == "" ? null : txtReason.Text + "";
                var Insert = new Approval()
                {
                    blockId = blockId,
                    approvalStatus = ApprovalStatus,
                    cancellationReason = txtReason.Text == "" ? null : txtReason.Text
                };

                HttpResponseMessage response = client.PutAsJsonAsync(sUrl, Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        Cancel();
                        BindBlockMaster();
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
    public class Approval
    {
        public string userId { get; set; }
        public string blockId { get; set; }
        public string approvalStatus { get; set; }
        public string cancellationReason { get; set; }

    }


    #endregion
}