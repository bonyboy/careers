using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Touchmark;

partial class displayJobMessage : System.Web.UI.Page
{

    string BaseURL = JobUtils.BaseURL;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //HyperLinkURL.Text = "Touchmark.com";
            //HyperLinkURL.NavigateUrl = "http://www.touchmark.com";
            
            if (!string.IsNullOrEmpty(Request.QueryString["url"]))
            {
                try
                {
                    String s = ComUtilsG5.DecryptText(Request.QueryString["url"]);

                    //error message probably longer than 50
                    if (s.Length < 50)
                    {
                        //HyperLinkURL.Text = s;
                        //HyperLinkURL.NavigateUrl = "http://" + s;
                    }
                }

                catch (Exception ex)
                {
                    String exmessage = ex.Message.ToString();
                }
            }

            DivPreHire.Visible = false;
            DivNoPreHire.Visible = true;

            if (!string.IsNullOrEmpty(Request.QueryString["url2"]))
            {
                try
                {
                    String t = ComUtilsG5.DecryptText(Request.QueryString["url2"]);

                    if (!t.ToLower().Contains("invalid"))
                    {
                        HyperLinkPreHire.Text = "Click here for Touchmark Questionnaire";
                        HyperLinkPreHire.NavigateUrl = t;
                        DivPreHire.Visible = true;
                        DivNoPreHire.Visible = false;
                    }
                }

                catch (Exception ex)
                {
                    String exmessage = ex.Message.ToString();
                }
            }
        }
    }

}