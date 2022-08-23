﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/CustomerServices.Master" AutoEventWireup="true"
    CodeBehind="GoldStockDetailReport.aspx.cs" Inherits="ATOZWEBGMS.Pages.GoldStockDetailReport" Title="Stock Details Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="../Styles/structure.css" rel="stylesheet" />--%>
    <style type="text/css">
        body {
            background: url(../Images/PageBackGround.jpg)no-repeat;
            background-size: cover;
        }
    </style>

    <script language="javascript" type="text/javascript">
        $(function () {
            $("#<%= txtFromDate.ClientID %>").datepicker();
               $("#<%= txtToDate.ClientID %>").datepicker();

               var prm = Sys.WebForms.PageRequestManager.getInstance();

               prm.add_endRequest(function () {
                   $("#<%= txtFromDate.ClientID %>").datepicker();
                $("#<%= txtToDate.ClientID %>").datepicker();

            });

           });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
    <br />

    <div id="DivMain" runat="server" align="center">
        <br />

        <table class="style1" width="900px">
            <thead>
                <tr>
                    <th colspan="6">Gold Stock Details Report
                    </th>
                </tr>

            </thead>

            <tr>
                <td colspan="8">&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbtStckTotalDtl" runat="server" GroupName="RptOptPrm" Text="Total Stock Details" Style="font-weight: 700" Font-Italic="True" Checked="True" AutoPostBack="true" OnCheckedChanged="rbtStckTotalDtl_CheckedChanged" />

                </td>
            </tr>

            <tr>
                <td colspan="8">&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbtStckDetail1" runat="server" GroupName="RptOptPrm" Text="Code wise Gold Stock Details" Style="font-weight: 700" Font-Italic="True" AutoPostBack="true" OnCheckedChanged="rbtStckDetail1_CheckedChanged" />

                </td>
            </tr>


            <tr>
                <td colspan="8">&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbtStckDetail2" runat="server" GroupName="RptOptPrm" Text="Karat & Purity wise Gold Stock Details" Style="font-weight: 700" Font-Italic="True" AutoPostBack="true" OnCheckedChanged="rbtStckDetail2_CheckedChanged" />

                </td>
            </tr>

            <tr>
                <td colspan="8">&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbtStckDetail3" runat="server" GroupName="RptOptPrm" Text="Gross wise Gold Stock Details" Style="font-weight: 700" Font-Italic="True" AutoPostBack="true" OnCheckedChanged="rbtStckDetail3_CheckedChanged" />

                </td>
            </tr>

            <tr>
                <td></td>
                <td colspan="8">&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbtDetails" runat="server" GroupName="RptDtlOptPrm" Font-Size="Large" Text="Details" Style="font-weight: 700" Font-Italic="True" Checked="true" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rbtSummary" runat="server" GroupName="RptDtlOptPrm" Font-Size="Large" Text="Summary" Style="font-weight: 700" Font-Italic="True" />
                </td>
            </tr>



            <tr>
                <td></td>
                <td>
                    <asp:CheckBox ID="chkAllLocation" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllLocation_CheckedChanged" Font-Size="Large" ForeColor="Red" Text="   All" />
                    &nbsp;
                    <asp:Label ID="lblLocation" runat="server" Text="Location " Width="120px" Font-Size="Large" ForeColor="Red"></asp:Label>

                </td>
                <td>
                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="cls text" Width="130px" Height="30px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large">
                        <asp:ListItem Value="0">-Select-</asp:ListItem>
                        <asp:ListItem Value="2">Dubai Stock</asp:ListItem>
                        <asp:ListItem Value="1">Dhaka Stock</asp:ListItem>
                    </asp:DropDownList>

                </td>

                <td>
                    <asp:Label ID="Label4" runat="server" Text="" Width="150px" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>

                <td>
                    <asp:Label ID="Label5" runat="server" Text="" Width="150px" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="" Width="150px" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
            </tr>


            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="" Width="150px" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="From Date :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="cls text" Width="120px" Height="27px" Style="text-align: Right" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" TabIndex="6"></asp:TextBox>

                </td>

                <td>&nbsp;&nbsp;
                    <asp:Label ID="Label1" runat="server" Text="To Date :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="cls text" Width="120px" Height="27px" Style="text-align: Right" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" TabIndex="6"></asp:TextBox>

                </td>

                <td>
                    <asp:Label ID="Label3" runat="server" Text="" Width="150px" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
            </tr>


            <tr>
                <td></td>
                <td colspan="5">

                    <asp:Button ID="btnView" runat="server" Text="Preview/Print"
                        Font-Size="Large" ForeColor="White"
                        Font-Bold="True" CssClass="button blue"
                        OnClientClick="return ValidationBeforeSave()" Height="27px" OnClick="btnView_Click" />
                    &nbsp;
                    &nbsp;
                    <asp:Button ID="BtnExit" runat="server" Text="Exit" Font-Size="Large" ForeColor="#FFFFCC"
                        Font-Bold="True" CausesValidation="False"
                        CssClass="button red" OnClick="BtnExit_Click" Height="27px" />
                    <br />

                </td>
            </tr>
        </table>
    </div>





    <asp:Label ID="lblLastLPartyNo" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblProcessDate" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblNewLPartyNo" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="hdnNewAccNo" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="ctrlNewAccNo" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="CtrlVoucherNo" runat="server" Visible="False"></asp:Label>

    <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="CtrlProcDate" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblCurrencyCode" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="CtrlProgFlag" runat="server" Visible="False"></asp:Label>

    <asp:HiddenField ID="hPartCode" runat="server" />

</asp:Content>
