﻿using System;
using System.Web;
using System.Web.UI;
using DataAccessLayer.DTO;
using System.Drawing;
using ATOZWEBGMS.WebSessionStore;
using DataAccessLayer.Utility;
using DataAccessLayer.DTO.HouseKeeping;
using System.Net.NetworkInformation;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using DataAccessLayer.DTO.CustomerServices;
using DataAccessLayer.BLL;
using Microsoft.Win32;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ATOZWEBGMS.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string hostip = string.Empty;
                string hostname = string.Empty;

                hostip = Request.UserHostAddress; //Encript(Request.LogonUserIdentity);



                if (Request.IsLocal)
                {
                    Pass.Visible = false;
                    main.Visible = true;
                    txtIdNo.Focus();
                }
                else
                {
                    main.Visible = false;
                    Pass.Visible = false;

                    string qry1 = "SELECT LicenseKye FROM A2ZCUBISWEBSECURITY";
                    DataTable dt1 = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry1, "A2ZHKGMS");
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in dt1.Rows)
                        {
                            var fldMacAddRec = dr1["LicenseKye"].ToString();
                            if (hostip == fldMacAddRec)
                            {
                                main.Visible = true;
                                txtIdNo.Focus();

                                Session["IdsNo"] = string.Empty;

                            }
                        }
                    }
                }

            }
        }

        private string Encript(string sPassword)
        {
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bs = System.Text.Encoding.UTF8.GetBytes(sPassword);
                bs = x.ComputeHash(bs);
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                foreach (byte b in bs)
                {
                    s.Append(b.ToString("x2").ToLower());
                }
                return s.ToString();
            }
            catch (Exception ex)
            {
                throw ex;

                // 
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {

                // AtoZUtility installData = new AtoZUtility();

                if (txtPassword.Text == string.Empty)
                {
                    String csname1 = "PopupScript";
                    Type cstype = GetType();
                    ClientScriptManager cs = Page.ClientScript;

                    txtPassword.Focus();

                    return;
                }


                AtoZUtility a2zUtility = new AtoZUtility();

                string orgPass = a2zUtility.GeneratePassword(OrgPass.Value, 1);

                if (orgPass != txtPassword.Text)
                {
                    String csname1 = "PopupScript";
                    Type cstype = GetType();
                    ClientScriptManager cs = Page.ClientScript;

                    if (!cs.IsStartupScriptRegistered(cstype, csname1))
                    {
                        String cstext1 = "alert('Wrong Password');";
                        cs.RegisterStartupScript(cstype, csname1, cstext1, true);

                        txtPassword.Focus();
                    }
                    return;
                }

                else
                {

                    bool isValid = true; //CheckSoftwareValidation();

                    if (isValid)
                    {
                        SessionStore.SaveToCustomStore(Params.SYS_USER_ID, txtIdNo.Text);
                        Session["IdsNo"] = txtIdNo.Text;
                        Session["LogOutFlag"] = "";
                        Response.Redirect("ERP.aspx", false);
                    }
                    else
                    {
                        Response.Write("<script>alert('Error in Login');</script>");
                    }

                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "scriptkey", "<script>alert('System Error.btnLogin_Click Problem');</script>");
            }
        }

        private bool CheckSoftwareValidation()
        {
            try
            {
                var url = "https://kaynat.info/ruban/lisense.html";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls ;

                WebRequest request = HttpWebRequest.Create(url);

               

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string responseText = reader.ReadToEnd();

                DateTime CurrentDate = DateTime.Now;

                DateTime ValidationDate = Convert.ToDateTime(responseText);


                if (CurrentDate > ValidationDate)
                {
                    return false;
                }

                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        protected void txtUserId_TextChanged(object sender, EventArgs e)
        {
            try
            {

                hdnIDFlag.Value = "0";

                int checkUser = CheckUserId();

                if (checkUser == 0)
                {

                    if (hdnIDFlag.Value == "0")
                    {
                        txtPassword.Focus();
                        btnLogin.Enabled = true;

                    }
                    else
                    {
                        txtIdNo.Text = string.Empty;
                        txtIdNo.Focus();
                        return;
                    }
                }
                else
                {
                    btnLogin.Enabled = false;

                    string msg = string.Empty;

                    switch (checkUser)
                    {
                        case 1:
                            msg = "User Id Not Available";
                            txtIdNo.Text = string.Empty;
                            break;
                        case 2:
                            msg = "Change Password - New Id was Created";

                            //DivchangePass.Visible = true;
                            break;
                        case 3:
                            msg = "User Id is using by other client or Abnormal Logout";
                            txtIdNo.Text = string.Empty;
                            break;
                        default:
                            msg = "Check User Id Information";
                            txtIdNo.Text = string.Empty;
                            break;
                    }

                    String csname1 = "PopupScript";
                    Type cstype = GetType();
                    ClientScriptManager cs = Page.ClientScript;

                    if (!cs.IsStartupScriptRegistered(cstype, csname1))
                    {
                        String cstext1 = "alert('" + msg + "');";
                        cs.RegisterStartupScript(cstype, csname1, cstext1, true);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        protected void VerifyUserId()
        {
            hdnIDFlag.Value = "0";
            string qry1 = "SELECT IdsNo,IdsName from A2ZSYSIDS WHERE IdsLogInFlag <> 0 AND IdsNo = '" + txtIdNo.Text + "'";
            DataTable dt1 = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry1, "A2ZACGMS");
            if (dt1.Rows.Count > 0)
            {
                hdnIDFlag.Value = "1";
                IDMSG1();
                return;
            }
            string qry2 = "SELECT IdsNo,IdsName from A2ZSYSIDS WHERE IdsLogInFlag <> 0 AND IdsNo = '" + txtIdNo.Text + "'";
            DataTable dt2 = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry2, "A2ZGLCUBS");
            if (dt2.Rows.Count > 0)
            {
                hdnIDFlag.Value = "1";
                IDMSG2();
                return;
            }
            string qry3 = "SELECT IdsNo,IdsName from A2ZSYSIDS WHERE IdsLogInFlag <> 0 AND IdsNo = '" + txtIdNo.Text + "'";
            DataTable dt3 = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry3, "A2ZHRCUBS");
            if (dt3.Rows.Count > 0)
            {
                hdnIDFlag.Value = "1";
                IDMSG3();
                return;
            }
            string qry4 = "SELECT IdsNo,IdsName from A2ZSYSIDS WHERE IdsLogInFlag <> 0 AND IdsNo = '" + txtIdNo.Text + "'";
            DataTable dt4 = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry4, "A2ZBTCUBS");
            if (dt4.Rows.Count > 0)
            {
                hdnIDFlag.Value = "1";
                IDMSG4();
                return;
            }
            string qry5 = "SELECT IdsNo,IdsName from A2ZSYSIDS WHERE IdsLogInFlag <> 0 AND IdsNo = '" + txtIdNo.Text + "'";
            DataTable dt5 = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry5, "A2ZOBTCUBS");
            if (dt5.Rows.Count > 0)
            {
                hdnIDFlag.Value = "1";
                IDMSG5();
                return;
            }
        }
        protected int CheckUserId()
        {
            // For Return Value of CheckUserId()
            //---------------------------------
            // 0 = Id is Available 
            // 1 = ID not in Table
            // 2 = Please Change Password - New Id was created
            // 3 = Id was not Initialize - Abnormal Logout
            //---------------------------------
            // End of For Return Value of CheckUserId()

            try
            {

                A2ZSYSIDSDTO idsDto = A2ZSYSIDSDTO.GetUserInformation(Converter.GetInteger(txtIdNo.Text), "A2ZHKGMS");

                if (idsDto.IdsNo == 0)
                {
                    String csname1 = "PopupScript";
                    Type cstype = GetType();
                    ClientScriptManager cs = Page.ClientScript;

                    if (!cs.IsStartupScriptRegistered(cstype, csname1))
                    {
                        String cstext1 = "alert('ID Not Found .');";
                        cs.RegisterStartupScript(cstype, csname1, cstext1, true);

                        txtIdNo.Text = string.Empty;
                    }
                    return 0;
                }

                OrgPass.Value = idsDto.IdsPass;

                if (idsDto.IdsPass == "XXXXXXXX")
                {

                    return 2;
                }



                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btChangePassword_Click(object sender, EventArgs e)
        {


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {



        }



        protected void MSG1()
        {
            String csname1 = "PopupScript";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('Old Password did not match.');";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }

            return;

        }


        protected void IDMSG1()
        {
            String csname1 = "PopupScript";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('ID Using in Customer Service Module');";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }

            return;

        }

        protected void IDMSG2()
        {
            String csname1 = "PopupScript";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('ID Using in General Ledger Module');";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }

            return;

        }

        protected void IDMSG3()
        {
            String csname1 = "PopupScript";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('ID Using in Human Resorce Module');";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }

            return;

        }
        protected void IDMSG4()
        {
            String csname1 = "PopupScript";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('ID Using in Booth Module');";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }

            return;

        }

        protected void IDMSG5()
        {
            String csname1 = "PopupScript";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('ID Using in Off Booth Module');";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }

            return;

        }
        protected void LicenseMSG()
        {
            String csname1 = "PopupScript";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('You Don't Have Permission');";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }
            return;
        }

        protected void MSG2()
        {
            String csname1 = "PopupScript";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('New Password did not match.');";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }

            return;

        }

        protected void MSG3()
        {
            String csname1 = "PopupScript";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('Data Saved Successfully.');";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }

            return;

        }
        protected void lockbutton()
        {
            txtIdNo.Enabled = false;
            txtPassword.Enabled = false;
            btnLogin.Enabled = false;
            //btnChangePassword.Enabled = false;
        }

        protected void unlockbutton()
        {
            txtIdNo.Enabled = true;
            txtPassword.Enabled = true;
            btnLogin.Enabled = true;
            //btnChangePassword.Enabled = true;
        }

        protected void btChangePassword_Click1(object sender, EventArgs e)
        {
            if (txtIdNo.Text != string.Empty)
            {
                main.Visible = false;
                Pass.Visible = true;
            }
            else
            {
                String csname1 = "PopupScript";
                Type cstype = GetType();
                ClientScriptManager cs = Page.ClientScript;

                if (!cs.IsStartupScriptRegistered(cstype, csname1))
                {
                    String cstext1 = "alert('Please Input Id No.');";
                    cs.RegisterStartupScript(cstype, csname1, cstext1, true);

                    return;
                }


            }


        }

        protected void btnPasswordChange_Click(object sender, EventArgs e)
        {
            try
            {
                AtoZUtility a2zUtility = new AtoZUtility();
                string orgPass = a2zUtility.GeneratePassword(OrgPass.Value, 1);

                if (orgPass != txtOldPassword.Text)
                {
                    MSG1();

                    //ShowMessage("Old Password did not match.", Color.Red);
                    // DivLogin.Visible = false;
                }
                else
                {
                    if (txtNewPassword.Text != this.txtConfirmPassword.Text)
                    {
                        MSG2();
                        //ShowMessage("New Password did not match.", Color.Red);
                        // DivLogin.Visible = false;
                    }
                    else
                    {

                        string newPass = a2zUtility.GeneratePassword(txtNewPassword.Text, 0);

                        A2ZSYSIDSDTO idsDto = new A2ZSYSIDSDTO();

                        idsDto.IdsNo = Converter.GetSmallInteger(txtIdNo.Text);
                        idsDto.IdsPass = newPass;

                        int rowEffiect = A2ZSYSIDSDTO.UpdateNewPassword(idsDto);

                        if (rowEffiect > 0)
                        {
                            MSG3();
                            //ShowMessage("Data saved successfully.", Color.Green);

                            main.Visible = true;
                            Pass.Visible = false;

                        }
                    }
                }



                txtIdNo.Text = string.Empty;
                //lblWelcome.Text = string.Empty;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            main.Visible = true;
            Pass.Visible = false;

            txtOldPassword.Text = string.Empty;
            txtPassword.Text = string.Empty;

            txtConfirmPassword.Text = string.Empty;


        }


    }
}