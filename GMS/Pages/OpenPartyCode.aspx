﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/CustomerServices.Master" AutoEventWireup="true"
    CodeBehind="OpenPartyCode.aspx.cs" Inherits="ATOZWEBGMS.Pages.OpenPartyCode" Title="New Party Open" %>

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

    <div id="DivMain" runat="server" align="center">
        <table class="style1">
            <thead>
                <tr>
                    <th colspan="3">New Party Code
                    </th>
                </tr>

            </thead>
        
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Group :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlGroup" runat="server" CssClass="cls text" Width="316px" Height="30px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged">
                        <asp:ListItem Value="0">-Select-</asp:ListItem>
                        <asp:ListItem Value="11">Loan A/C Liabilities (LLT)</asp:ListItem>
                        <asp:ListItem Value="12">Loan A/C Liabilities (STL)</asp:ListItem>
                        <asp:ListItem Value="13">Carrier/Currency Exchanger</asp:ListItem>
                        <asp:ListItem Value="14">Party Dubai</asp:ListItem>
                        <asp:ListItem Value="15">Party India</asp:ListItem>     
                        <asp:ListItem Value="16">Cash</asp:ListItem>
                        <asp:ListItem Value="17">Loan and Advance</asp:ListItem>
                        <asp:ListItem Value="18">Advance Employee</asp:ListItem> 
                        <asp:ListItem Value="19">Sundry Dealer</asp:ListItem>
                        <asp:ListItem Value="20">Sundry Customer</asp:ListItem>
                        <asp:ListItem Value="21">Inventory</asp:ListItem>
                        <asp:ListItem Value="22">Bank Accounts</asp:ListItem>
                        <asp:ListItem Value="51">Misc.</asp:ListItem>

                    </asp:DropDownList>
                </td>
                

            </tr>
             

            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Party Name:" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPartyName" runat="server" CssClass="cls text" Width="316px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);" autocomplete="off" ></asp:TextBox>
                </td>
            </tr>

              <tr>
                <td>
                    <asp:Label ID="lblAdd1" runat="server" Text="Address Line 1 :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                     <asp:TextBox ID="txtPartyAddressL1" runat="server" CssClass="cls text" Width="316px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);" autocomplete="off"></asp:TextBox>
                </td>
            </tr>

              <tr>
                <td>
                    <asp:Label ID="lblAdd2" runat="server" Text="Address Line 2 :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                     <asp:TextBox ID="txtPartyAddressL2" runat="server" CssClass="cls text" Width="316px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);" autocomplete="off"></asp:TextBox>
                </td>
            </tr>

              <tr>
                <td>
                    <asp:Label ID="lblAdd3" runat="server" Text="Address Line 3 :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                     <asp:TextBox ID="txtPartyAddressL3" runat="server" CssClass="cls text" Width="316px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);" autocomplete="off"></asp:TextBox>
                </td>
            </tr>


            <tr>
                <td>
                    <asp:Label ID="lblMobile" runat="server" Text="Mobile No. :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                      <asp:TextBox ID="txtMobileNo" runat="server" CssClass="cls text" Width="150px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                       onkeypress="return IsDecimalKey(event)"  Font-Size="Large" onkeydown="return (event.keyCode !=13);" autocomplete="off"></asp:TextBox>

                </td>
               
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblEmail" runat="server" Text="E-mail :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPartyEmail" runat="server" CssClass="cls text" Width="150px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);" autocomplete="off"></asp:TextBox>
                    &nbsp;</td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblCarringRate" runat="server" Text="Carring Rate :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCarringRate" runat="server" CssClass="cls text" Width="150px" Height="25px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" onkeydown="return (event.keyCode !=13);" autocomplete="off"></asp:TextBox>
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

    <div id="DivLedgerMaint" runat="server">
        <table style="width: 340px; height: 130px; background-color: #e9e9e9;">
            
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblGenPartyCode" runat="server" Text="Generated New Party Code  :" ForeColor="Red"></asp:Label>

                    <asp:Label ID="lblNewLPartyNo" runat="server" CssClass="cls text" Width="250px" Height="27px" BorderColor="#1293D1" BorderStyle="Ridge"
                                     Style="text-align: center" Font-Size="X-Large" ></asp:Label>

                </td>
            </tr>


            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblGenLdgCode" runat="server" Text="Generated New Ledger Code :" ForeColor="Red"></asp:Label>

                    <asp:Label ID="ctrlNewAccNo" runat="server" CssClass="cls text" Width="250px" Height="27px" BorderColor="#1293D1" BorderStyle="Ridge"
                                     Style="text-align: center" Font-Size="X-Large" ></asp:Label>

                </td>
            </tr>

            <tr>
                <td style="text-align: center">
                    <asp:Label ID="Label2" runat="server" Text=" Party Ledger Currency :" ForeColor="Red"></asp:Label>

                <%--</td>
                <td>--%>
                    <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="cls text" Width="250px" Height="30px" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large">
                        <asp:ListItem Value="0">-Select-</asp:ListItem>
                    </asp:DropDownList>

                </td>
                
            </tr>

            <tr>

                <td style="text-align: center">
                    <asp:Button ID="btnLedgerMaint" runat="server" Text="OKAY"
                        Font-Size="Large" ForeColor="White"
                        Font-Bold="True" CssClass="button green"
                        Height="27px"/>
                    
                </td>
            </tr>
        </table>
    </div>






    <asp:Label ID="lblLastLPartyNo" runat="server" Visible="False"></asp:Label>
  

     <asp:Label ID="hdnNewAccNo" runat="server" Visible="False"></asp:Label>
    
    

</asp:Content>
