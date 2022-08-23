﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/CustomerServices.Master" AutoEventWireup="true"
    CodeBehind="AddItemGroup.aspx.cs" Inherits="ATOZWEBGMS.Pages.AddItemGroup  " Title="Item Group Add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="../Styles/structure.css" rel="stylesheet" />--%>
    <style type="text/css">
        body {
            background: url(../Images/PageBackGround.jpg)no-repeat;
            background-size: cover;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
    <br />

    <div align="center">
        <table class="style1">
            <thead>
                <tr>
                    <th colspan="3">Add Item Group
                    </th>
                </tr>

            </thead>
        

             

            <%--<tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Group Code :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtGroupCode" runat="server" CssClass="cls text" Width="150px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);" AutoPostBack="true" OnTextChanged="txtGroupCode_TextChanged"></asp:TextBox>
                </td>
            </tr>--%>

              <tr>
                <td>
                    <asp:Label ID="Label11" runat="server" Text="Group Name :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                     <asp:TextBox ID="txtGroupName" runat="server" CssClass="cls text" Width="150px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);"></asp:TextBox>
                </td>
            </tr>

              <tr>
                <td>
                    <asp:Label ID="Label12" runat="server" Text="Making Range From :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                     <asp:TextBox ID="txtMRangeFrom" runat="server" CssClass="cls text" Width="150px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);" ></asp:TextBox>
                </td>
            </tr>

              <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Making Range To :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                     <asp:TextBox ID="txtMRangeTo" runat="server" CssClass="cls text" Width="150px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);"></asp:TextBox>
                </td>
            </tr>


            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Purity 22k :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                      <asp:TextBox ID="txtPurity22" runat="server" CssClass="cls text" Width="150px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                       onkeypress="return IsDecimalKey(event)"  Font-Size="Large" onkeydown="return (event.keyCode !=13);"></asp:TextBox>
                </td>
               
            </tr>

            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Purity 21k :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPurity21" runat="server" CssClass="cls text" Width="150px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);"></asp:TextBox>
                    &nbsp;</td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Purity 18k :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPurity18" runat="server" CssClass="cls text" Width="150px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);"></asp:TextBox>
                    &nbsp;</td>
            </tr>
      
          
            <tr>
               <td colspan="2">

              
                  
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit"
                        Font-Size="Large" ForeColor="White"
                        Font-Bold="True" CssClass="button green"
                        OnClientClick="return ValidationBeforeSave()" Height="27px" OnClick="btnSubmit_Click" />
                     &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel"
                        Font-Size="Large" ForeColor="White"
                        Font-Bold="True" CssClass="button blue"
                        OnClick="btnCancel_Click" Height="27px" />
                    &nbsp;
                    <asp:Button ID="BtnExit" runat="server" Text="Exit" Font-Size="Large" ForeColor="#FFFFCC"
                        Font-Bold="True" CausesValidation="False"
                        CssClass="button red" OnClick="BtnExit_Click" Height="27px" />
                    <br />

                </td>
            </tr>
        </table>
    </div>






    <asp:Label ID="lblLastCPartyNo" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblNewCPartyNo" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblNewItemGroup" runat="server" Visible="False"></asp:Label>
</asp:Content>
