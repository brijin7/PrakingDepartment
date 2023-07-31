﻿<%@ Page Title="Branch Login" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="BranchLogin.aspx.cs" Inherits="Master_Login_BranchLogin" %>

<asp:Content ID="frmBranchlogin" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        label {
            font-weight: 900;
            color: #2196f3;
            font-size: 1.5rem;
        }

        .card {
            box-shadow: 0 6px 8px 0 rgba(0, 0, 0, 0.38);
            transition: 0.3s;
            width: 156px;
            height: 170px;
            background-color: var(--white);
            padding-left: 16px;
            border-radius: 1.25rem;
        }

            .card:hover {
                box-shadow: 10px 8px 16px 0 rgba(0,0,0,0.2);
            }

        .cardIn {
            box-shadow: 0 6px 8px 0 rgba(0, 0, 0, 0.38);
            transition: 0.3s;
            width: 258px;
            height: 112px;
            background-color: var(--white);
            padding-left: 16px;
        }

        .container {
            padding: 2px 16px;
        }

        .gradient4 {
            /*background: rgba(102, 122, 102, 0.6);*/
            background-color: white;
            align-content: center;
        }

        .wrap {
            white-space: nowrap;
        }

        .content-wrapper {
            font-size: 1.1em;
            margin-bottom: 44px;
        }

            .content-wrapper:last-child {
                margin-bottom: 0;
            }

        .link {
            color: #121943;
        }

            .link:focus {
                box-shadow: 0px 0px 0px 2px #121943;
            }

        .input-wrapper {
            display: flex;
            flex-direction: column;
        }

            .input-wrapper .label {
                align-items: baseline;
                display: flex;
                font-weight: 700;
                justify-content: space-between;
                margin-bottom: 8px;
            }

            .input-wrapper .optional {
                color: #5a72b5;
                font-size: 0.9em;
            }

            .input-wrapper .input {
                border: 1px solid #5a72b5;
                border-radius: 4px;
                height: 40px;
                padding: 8px;
            }

        code {
            background: #e5efe9;
            border: 1px solid #5a72b5;
            border-radius: 4px;
            padding: 2px 4px;
        }

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

        #modal {
            opacity: 0;
            transition: opacity 0.25s ease-in-out;
            visibility: hidden;
        }

            #modal:target {
                opacity: 1;
                visibility: visible;
            }

                #modal:target .modal-body {
                    opacity: 1;
                    transform: translateY(1);
                }

            #modal .modal-body {
                max-width: 500px;
                opacity: 0;
                transform: translateY(-100px);
                transition: opacity 0.25s ease-in-out;
                width: 100%;
                z-index: 1;
            }

        .cardmodal {
            background: #fff;
            background-image: linear-gradient(48deg, #fff 0%, #e5efe9 100%);
            border-top-right-radius: 16px;
            border-bottom-left-radius: 16px;
            box-shadow: -20px 20px 35px 1px rgba(10, 49, 86, 0.18);
            display: flex;
            flex-direction: column;
            padding: 32px;
            margin-left: 285px;
            margin-top: 203px;
            max-width: 757px;
            width: 100%;
        }

        .branchimage {
            Height: 120px !important;
            Width: 120px;
        }

        .row {
            display: flex;
            flex-wrap: wrap;
            padding-left: 18px;
        }

        .BranchList {
            margin-inline: auto;
            overflow: hidden;
        }

        .lblbrnchName {
            max-width: 130px !important;
            margin-top: -10px;
            width: 100% !important;
            white-space: pre-line;
            text-align-last: center;
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
                <asp:Label ID="lblNavFirst" runat="server" CssClass="pageRoutecol" Text="Branch Login"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Branch <span class="Card-title-second">Login </span></h4>
                </div>
            </div>
            <div class="row justify-content-center">
                <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12" style="text-align-last: center;">
                    <asp:Label
                        ID="lblbranch"
                        runat="server"
                        Text="List Of Branches" Style="color: #1cb7fd; font-size: 24px; font-weight: bold;">                      
                    </asp:Label>
                    <hr style="margin-top: 1rem !important; margin-bottom: 1rem !important; border: 0 !important; border-top: 1px solid rgb(28 183 253 / 27%) !important; width: 350px !important; margin-inline: auto !important;" />
                </div>

            </div>
            <br />
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="wrap">
                    <asp:DataList ID="gvBranchList" runat="server" RepeatColumns="4" Visible="true"
                        CssClass="BranchList table-responsive section" Style="overflow: auto;">
                        <ItemTemplate>
                            <div class="card">
                                <div class="row">
                                    <asp:ImageButton runat="server" CssClass="branchimage" ID="lnkBtnBranch"
                                        ImageUrl="~/images/Branch/Parking320210506134648305140.jpg" OnClick="lnkBtnBranch_Click"></asp:ImageButton>
                                    <asp:Label align="left" ID="lblbranchName" CssClass="lblbrnchName" runat="server" Text='<%# Bind("branchName") %>' Font-Bold="true"></asp:Label>
                                    <asp:Label align="left" ID="lblbranchId" runat="server" Text='<%# Bind("branchId") %>' Visible="false" Font-Bold="true"></asp:Label>
                                    <asp:Label align="left" ID="lblslotExist" runat="server" Text='<%# Bind("slotExist") %>' Visible="false" Font-Bold="true"></asp:Label>
                                    <asp:Label align="left" ID="lblbranchOptions" runat="server" Text='<%# Bind("branchOptions") %>' Visible="false" Font-Bold="true"></asp:Label>

                                </div>
                            </div>
                            <br />
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <table style="height: 4px; width: 25px;">
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </SeparatorTemplate>
                    </asp:DataList>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

