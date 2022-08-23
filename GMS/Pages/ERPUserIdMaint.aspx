﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/CustomerServices.Master" AutoEventWireup="true"
    CodeBehind="ERPUserIdMaint.aspx.cs" Inherits="ATOZWEBGMS.Pages.ERPUserIdMaint"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Styles/TableStyle1.css" rel="stylesheet" type="text/css" />

    <script src="../scripts/validation.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function Userid() {

            var ddlIdLevel = document.getElementById('<%=ddlIdLevel.ClientID%>');
            var txtIdNo = document.getElementById('<%=txtIdNo.ClientID%>').value;
         <%--  var txtPerNo = document.getElementById('<%=txtPerNo.ClientID%>');--%>




            if (ddlIdLevel.selectedIndex == '' || ddlIdLevel.length == 0)
                alert('Please Select Lavel Name ');

            else if (txtIdNo == '' || txtIdNo.length == 0)
                alert('Please Input Id No.');

            else if (txtPerNo == '' || txtPerNo.length == 0)
                alert('Please Input PER No.');
            else
                return confirm('Are you sure you want save the data');
            return false;
        }
    </script>

    <style type="text/css">
        .style1 {
            height: 31px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
    <br />

    <div id="DivMain" runat="server" align="center">
        <table class="style1">

            <thead>
                <tr>
                    <th colspan="3">Add User Id
                    </th>
                </tr>

            </thead>


            <tr>

                <td>
                    <asp:Label ID="Label1" runat="server" Text="ID Level :" Font-Size="Large"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlIdLevel" runat="server" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlIdLevel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>

                <td>
                    <asp:Label ID="Label2" runat="server" Text="ID No. :" Font-Size="Large"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtIdNo" runat="server" Height="25px" CssClass="cls text" BorderColor="#1293D1" BorderStyle="Ridge"
                        onkeypress="return IsNumberKey(event)" AutoPostBack="True" OnTextChanged="txtIdNo_TextChanged"
                        MaxLength="4"></asp:TextBox>
                </td>
            </tr>
            <tr>

                <td>
                    <asp:Label ID="lblUserName" runat="server" Text="User Name :" Font-Size="Large"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" Height="25px" CssClass="cls text" Width="350px" BorderColor="#1293D1" BorderStyle="Ridge"></asp:TextBox>
                    <%--<asp:DropDownList ID="ddlPerNo" runat="server" BorderColor="#1293D1" BorderStyle="Ridge"
                                    AutoPostBack="True" Width="248px" OnSelectedIndexChanged="ddlPerNo_SelectedIndexChanged">
                                </asp:DropDownList>
                    
                    <asp:Label ID="lblIdsName" runat="server" Text="" Visible="false"></asp:Label>--%>



                    <%--          <asp:Label ID="lblFatherName" runat="server" Text="Father's Name"></asp:Label>
                    <asp:TextBox ID="txtFatherName" runat="server" BorderColor="#1293D1" BorderStyle="Ridge"
                        Enabled="False"></asp:TextBox>--%>
                </td>
            </tr>



            <%--<tr>
                   
                    <td>
                        <asp:Label ID="lblGlCashCode" runat="server" Text="GL Cash Code :" Font-Size="Large"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtGlCashCode" runat="server" Height="25px" CssClass="cls text" BorderColor="#1293D1" BorderStyle="Ridge" AutoPostBack="True" OnTextChanged="txtGlCashCode_TextChanged"></asp:TextBox>
                        <asp:DropDownList ID="ddlGLCashCode" runat="server" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlGLCashCode_SelectedIndexChanged" Width="248px">
                        </asp:DropDownList>

                    </td>
                </tr>--%>

            <tr>

                <td>
                    <asp:Label ID="lblSODfag" runat="server" Text="SOD Permission :" Font-Size="Large"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="ChkSODflag" runat="server" Font-Size="Large" ForeColor="Red" />

                </td>
            </tr>

            <tr>

                <td>
                    <asp:Label ID="lblVchPrintfag" runat="server" Text="Voucher Print :" Font-Size="Large"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="ChkVPrintflag" runat="server" Font-Size="Large" ForeColor="Red" />

                </td>
            </tr>

            <tr>

                <td>
                    <asp:Label ID="lblMobileflag" runat="server" Text="Mobile Permission :" Font-Size="Large"></asp:Label>
                </td>
                <td>
                <asp:CheckBox ID="ChkMobileflag" runat="server" Font-Size="Large" ForeColor="Red" />

                </td>
            </tr>


            <%--<tr>
                    
                    <td>
                        <asp:Label ID="lblGenAutoVch" runat="server" Text="Generate Auto Voucher No. :" Font-Size="Large"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkGenAutoVch" runat="server" Font-Size="Large" ForeColor="Red" />

                    </td>
                </tr>--%>

            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Transaction Limit Accessibility ..........." Font-Size="Large"
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLIdsCashCredit" runat="server" Text="Limit Cash Credit Transaction Amt.   :" Font-Size="Large"
                        ForeColor="Black"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLIdsCashCredit" runat="server" CssClass="cls text" Width="200px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Height="25px" Font-Size="Large" onkeypress="return IsDecimalKey(event)" onFocus="javascript:this.select();" MaxLength="14"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblLIdsCashDebit" runat="server" Text="Limit Cash Debit Transaction Amt.      :" Font-Size="Large"
                        ForeColor="Black"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLIdsCashDebit" runat="server" CssClass="cls text" Width="200px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Height="25px" Font-Size="Large" onkeypress="return IsDecimalKey(event)" onFocus="javascript:this.select();" MaxLength="14"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblLIdsTrfCredit" runat="server" Text="Limit Transfer Credit Transaction Amt. :" Font-Size="Large"
                        ForeColor="Black"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLIdsTrfCredit" runat="server" CssClass="cls text" Width="200px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Height="25px" Font-Size="Large" onkeypress="return IsDecimalKey(event)" onFocus="javascript:this.select();" MaxLength="14"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblLIdsTrfDebit" runat="server" Text="Limit Transfer Debit Transaction Amt.    :" Font-Size="Large"
                        ForeColor="Black"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLIdsTrfDebit" runat="server" CssClass="cls text" Width="200px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Height="25px" Font-Size="Large" onkeypress="return IsDecimalKey(event)" onFocus="javascript:this.select();" MaxLength="14"></asp:TextBox>
                </td>
            </tr>



            <%--<tr>
                <th>
                    <asp:Label ID="lblManagment" runat="server" Text="Managment Name"></asp:Label>
                </th>
                <td>
                </td>
                <td>
                    <asp:TextBox ID="txtManagment" runat="server" BorderColor="#1293D1" BorderStyle="Ridge"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td></td>
                <td colspan="2">
                    <asp:Button ID="btnAdd" runat="server" Text="Add User Id" OnClientClick="return Userid(event,this)"
                        CssClass="button green size-120" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnShow" runat="server" Text="Show Id Info"
                        CssClass="button blue size-100" OnClick="btnShow_Click" />
                    <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="button red size-100"
                        OnClick="btnExit_Click" />
                </td>
            </tr>
        </table>
    </div>

    <div id="DivMessage" runat="server" align="center">
        <table class="style1">
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
                <td>
                    <asp:Button ID="btnHideMessageDiv" runat="server" Text="OK" CssClass="button blue size-100"
                        OnClick="btnHideMessageDiv_Click" />
                </td>
            </tr>
        </table>
    </div>

    <div id="DivGridView" runat="server" align="center" visible="False">
        <table class="style1">
            <thead>
                <tr>
                    <th>User Id Information
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <asp:GridView ID="gvUserIdInfromation" runat="server" BorderColor="#1293D1"
                            BorderStyle="Ridge">
                        </asp:GridView>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="temp">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtDp" runat="server" Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtDn" runat="server" Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtloc" runat="server" Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtsec" runat="server" Visible="False"></asp:TextBox>
                    <asp:Label ID="lblPrmUnitFlag" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblPrmUnitNo" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
    </div>



</asp:Content>
