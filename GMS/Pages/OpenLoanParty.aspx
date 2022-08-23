﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/CustomerServices.Master" AutoEventWireup="true"
    CodeBehind="OpenLoanParty.aspx.cs" Inherits="ATOZWEBGMS.Pages.OpenLoanPartyOpen" Title="New Party Open" %>

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
                    <th colspan="3">New Loan Party Code
                    </th>
                </tr>

            </thead>
        

             

            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Party Name:" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPartyName" runat="server" CssClass="cls text" Width="316px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);" ></asp:TextBox>
                </td>
            </tr>

              <tr>
                <td>
                    <asp:Label ID="Label11" runat="server" Text="Address Line 1 :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                     <asp:TextBox ID="txtPartyAddressL1" runat="server" CssClass="cls text" Width="316px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);"></asp:TextBox>
                </td>
            </tr>

              <tr>
                <td>
                    <asp:Label ID="Label12" runat="server" Text="Address Line 2 :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                     <asp:TextBox ID="txtPartyAddressL2" runat="server" CssClass="cls text" Width="316px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);" ></asp:TextBox>
                </td>
            </tr>

              <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Address Line 3 :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                     <asp:TextBox ID="txtPartyAddressL3" runat="server" CssClass="cls text" Width="316px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);"></asp:TextBox>
                </td>
            </tr>


            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Mobile No. :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                      <asp:TextBox ID="txtMobileNo" runat="server" CssClass="cls text" Width="150px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                       onkeypress="return IsDecimalKey(event)"  Font-Size="Large" onkeydown="return (event.keyCode !=13);"></asp:TextBox>

                </td>
               
            </tr>

            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="E-mail :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPartyEmail" runat="server" CssClass="cls text" Width="150px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);"></asp:TextBox>
                    &nbsp;</td>
            </tr>

      
          
            <tr>
               <td>

               </td>
                <td>
                  
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






    <asp:Label ID="lblLastLPartyNo" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblNewLPartyNo" runat="server" Visible="False"></asp:Label>
</asp:Content>
