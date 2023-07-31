<%@ Page Title="Block Master" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="BlockMaster.aspx.cs" Inherits="Master_BlockMaster" %>

<asp:Content ID="frmBlockMaster" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
                .modal-header {
            align-items: baseline;
            display: flex;
            justify-content: space-between;
        }

        .close {
            background: none;
            border: none;
            cursor: pointer;
            display: flex;
            height: 16px;
            text-decoration: none;
            width: 16px;
            box-sizing: unset;
        }

            .close svg {
                width: 16px;
                box-sizing: unset;
            }

        .modal-wrapper {
            align-items: center;
            background: rgba(0, 0, 0, 0.7);
            bottom: 0;
            display: flex;
            justify-content: center;
            left: 0;
            position: fixed;
            right: 0;
            top: 0;
        }

        .modal-body {
            opacity: 1 !important;
            transform: translateY(1);
        }

        .model {
            opacity: 1;
            visibility: visible;
            z-index: 5;
        }


        .modal-body {
            max-width: 500px;
            opacity: 0;
            transform: translateY(-100px);
            transition: opacity 0.25s ease-in-out;
            width: 100%;
            z-index: 5;
        }

        .cardModal {
            background: #fff;
            background-image: linear-gradient(48deg, #fff 0%, #e5efe9 100%);
            border-top-right-radius: 16px;
            border-bottom-left-radius: 16px;
            box-shadow: -20px 20px 35px 1px rgba(10, 49, 86, 0.18);
            display: flex;
            flex-direction: column;
            padding: 32px;
            margin: 234px;
            max-width: 616px;
            width: 100%;
            height:auto;

        }
    </style>
    <div class="container-fluid containerBg">
        <div class="row PageRoutePos">
            <div>
                <asp:Label ID="lblMainPage" CssClass="pageRoutecol" runat="server" Text="Home"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavFirst" runat="server" CssClass="pageRoutecol" Text="Master"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Block Master"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Block <span class="Card-title-second">Master </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                  <asp:Label runat="server" CssClass="form-check-label"
                        Text="Block Name"></asp:label><span class="spanStar">*</span>                   
                    <asp:TextBox ID="txtBlockName" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfdBlockName" ValidationGroup="BlockMaster" ControlToValidate="txtBlockName" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Block Name">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="BlockMaster" CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
                <div>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>

        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Block <span class="Card-title-second">Master </span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                    <%-- <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="add +" CssClass="pure-material-button-contained btnBgColorAdd" />--%>
                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvBlockmaster"
                        runat="server"
                        AllowPaging="True"
                        DataKeyNames="blockId"
                        CssClass="gvv display"
                        AutoGenerateColumns="false"
                        BorderStyle="None"
                        PageSize="100">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Parking Owner" Visible="false" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvparkingOwnerId"
                                        runat="server"
                                        Text='<%#Bind("parkingOwnerId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Id" Visible="false" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvbranchId"
                                        runat="server"
                                        Text='<%#Bind("branchId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Block Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvblockName"
                                        runat="server"
                                        Text='<%#Bind("blockName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Block ID" Visible="false" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvblockId"
                                        runat="server"
                                        Text='<%#Bind("blockId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ApprovalStatus" Visible="false" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvapprovalStatus"
                                        runat="server"
                                        Text='<%#Bind("approvalStatus") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="LnkEdit"
                                        runat="server"
                                        src="../../images/edit-icon.png" alt="image"
                                        Text="Edit"
                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' OnClick="LnkEdit_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:LinkButton
                                        ID="lnkActiveOrInactive"
                                        runat="server"
                                        CssClass='<%#Eval("activeStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                        Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>'
                                        OnClick="lnkActiveOrInactive_Click"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approval Status" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton
                                            ID="lblapprovalStatus"
                                            runat="server"
                                            CssClass='<%#Eval("approvalStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                            Text='<%#Eval("approvalStatus").ToString() == "A" ? "Approved" : "Waiting List" %>'
                                            Enabled='<%#Eval("approvalStatus").ToString() == "A" ? false : true %>'
                                            OnClick="lblapprovalStatus_Click">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
       <div class="modal-wrapper model" id="modalCard" runat="server" visible="false">
        <div class="modal-body cardModal">
            <div class="modal-header">
                <h4 class="card-title" style="font-weight: bold">Approval Status</h4>
                <asp:ImageButton ID="ImgClosed" runat="server"
                    OnClick="ImgClosed_Click" ImageUrl="~/images/Close.svg" Width="25px"></asp:ImageButton>
            </div>

            <div class="row" style="margin-left: -1px !important; margin-top: 10px;">
                <div>
                    <asp:Button
                        ID="btnApprove"
                        runat="server"
                        Text="Approve"
                        OnClick="btnApprove_Click"
                        CausesValidation="false"
                        CssClass="pure-material-button-contained btnBgColorAdd" />
                    <asp:Button
                        ID="btnCancelModal"
                        runat="server"
                        Text="Cancel"
                        OnClick="btnCancelModal_Click"
                        CausesValidation="false"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="background-color: red" />
                </div>


            </div>
            <div class="row" style="margin-left: -1px !important; margin-top: 10px;">
                <div id="divCancellationReason" runat="server" class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" style="margin-top: 20px;">
                    <asp:Label
                        ID="lblcancel"
                        runat="server"
                        Text="Cancellation Reason" CssClass="form-check-label">
                    </asp:Label>

                    <asp:TextBox ID="txtReason" TextMode="MultiLine"
                        Height="50px" runat="server" CssClass="form-control section labels"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Visible="false" ControlToValidate="txttc" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter About Offer">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row justify-content-end" id="divsubmit" runat="server" visible="false">
                <div class="mr-3">
                    <asp:Button ID="btnSubmitPopup" CssClass="pure-material-button-contained btnBgColorAdd"
                        Text="Submit" ValidationGroup="Check" runat="server" OnClick="btnSubmitPopup_Click" />
                </div>
                <div>
                    <asp:Button ID="btnCancelPopup" CssClass="pure-material-button-contained btnBgColorCancel"
                        Text="Cancel" CausesValidation="false" OnClick="btnCancelPopup_Click"
                        runat="server" />
                </div>
            </div>


        </div>
    </div>
    <asp:HiddenField ID="hfBlockId" runat="server" />
</asp:Content>

