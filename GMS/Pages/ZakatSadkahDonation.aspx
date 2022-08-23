﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/CustomerServices.Master" AutoEventWireup="true"
    CodeBehind="ZakatSadkahDonation.aspx.cs" Inherits="ATOZWEBGMS.Pages.ZakatSadkahDonation" Title="Differ Balance" %>

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
            $("#<%= txtDate.ClientID %>").datepicker();
         

            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $("#<%= txtDate.ClientID %>").datepicker();
                  
               });

        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
    <br />

    <div align="center">
        <br />

        <table class="style1" width="800px">
            <thead>
                <tr>
                    <th colspan="6">Zakat Sadkah Donation
                    </th>
                </tr>

            </thead>
                      

            <tr>

                <td>
                    <asp:Label ID="Label2" runat="server" Text="" Width="150px" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Date On :" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    &nbsp;
                    <asp:TextBox ID="txtDate" runat="server" CssClass="cls text" Width="110px" Height="27px" Style="text-align: Right" BorderColor="#1293D1" BorderStyle="Ridge"
                        Font-Size="Large" TabIndex="6" Enabled="False"></asp:TextBox>

                </td>

                
                <td>
                    <asp:Label ID="Label3" runat="server" Text="" Width="150px" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>

            </tr>


            <tr>
                <td></td>
                <td colspan="5">

                    <asp:Button ID="btnView" runat="server" Text="Donation"
                        Font-Size="Large" ForeColor="White"
                        Font-Bold="True" CssClass="button blue"
                        OnClientClick="return ValidationBeforeSave()" Height="27px" OnClick="btnView_Click" Width="162px" />
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

    <asp:Label ID="LastVoucherNo" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="LastConfirmVchNo" runat="server" Visible="False"></asp:Label>

    <asp:HiddenField ID="hPartCode" runat="server" />

</asp:Content>
