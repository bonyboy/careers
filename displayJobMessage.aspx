<%@ Page Language="c#" debug="false" AutoEventWireup="true" CodeFile="displayJobMessage.aspx.cs" Inherits="displayJobMessage" %>

<!DOCTYPE html>
<html class="js no-touch wf-museoslab-i3-inactive wf-museoslab-i7-inactive wf-museoslab-n3-inactive wf-museoslab-n5-inactive wf-museoslab-n7-inactive wf-opensans-i4-inactive wf-opensans-i6-inactive wf-opensans-i7-inactive wf-opensans-n4-inactive wf-opensans-n6-inactive wf-opensans-n7-inactive wf-inactive" lang="en" style="">

<!--<![endif]-->

<head>

<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<meta content="Touchmark offers rewarding careers with competitive benefits and salaries. Please view our website for current employment opportunities." name="description" />

<title>Careers at Touchmark</title>

<!-- CSS Styles -->
<link rel="stylesheet" href="https://touchmark.net/careers/css/touchmarkcom.css" type="text/css" media="screen">
<link rel="stylesheet" href="https://touchmark.net/careers/css/responsiveslides.css" type="text/css" media="screen">

<link href='https://fonts.googleapis.com/css?family=Lato:100,300,400,700|PT+Serif' rel='stylesheet' type='text/css'>
<link href="https://touchmark.net/careers/images/favicon.png" rel="shortcut icon" type="image/png">

<!-- Javascript -->
<script src="https://touchmark.net/careers/js/jquery.js" type="text/javascript"></script>
<script src="https://touchmark.net/careers/js/touchmarkcom.js" type="text/javascript"></script>
<script src="https://touchmark.net/careers/js/modernizr.js" type="text/javascript"></script>
<script src="https://touchmark.net/careers/js/responsive-nav.js" type="text/javascript"></script>
<script src="https://touchmark.net/careers/js/g5-responsive.js" type="text/javascript"></script>
<script src="https://touchmark.net/careers/js/scripts.js" type="text/javascript"></script>
<script src="https://touchmark.net/careers/js/jquery.tooltipster.js" type="text/javascript"></script>
<script src="https://touchmark.net/careers/js/responsiveslides.min.js" type="text/javascript"></script>
<!--[if IE 8]><script type="text/javascript">var ie8 = true;</script><![endif]-->
<!--[if IE 9]><script type="text/javascript">var ie9 = true;</script><![endif]-->

</head>

<body class="corp home">

  <!--[if lt IE 9]>
    <div id="legacy_banner">

  <p>This website will not render properly with your current browser version. Please upgrade to a more recent version.</p>
  <p id="legacy_browsers">
    <a href="http://windows.microsoft.com/en-us/internet-explorer/download-ie" target="_blank">Internet Explorer</a>
    <span>|</span>
    <a href="https://www.mozilla.org/en-US/firefox/new/" target="_blank">Firefox</a>
    <span>|</span>
    <a href="https://www.google.com/chrome/browser/" target="_blank">Chrome</a>
</p>
</div>
  <![endif]-->

  <!--[if IE 9]>
    <div id="legacy_banner">

  <p>This website will not render properly with your current browser version. Please upgrade to a more recent version.</p>
  <p id="legacy_browsers">
    <a href="http://windows.microsoft.com/en-us/internet-explorer/download-ie" target="_blank">Internet Explorer</a>
    <span>|</span>
    <a href="https://www.mozilla.org/en-US/firefox/new/" target="_blank">Firefox</a>
    <span>|</span>
    <a href="https://www.google.com/chrome/browser/" target="_blank">Chrome</a>
</p>
</div>
  <![endif]-->

  <header>
      <!-- ResponsiveSlides GWM 2/11/16 -->
    <div class="rslides_container">
      <ul class="rslides" id="slider1">
        <li><img src="images/careers-header-videos-1600.jpg" alt="Find your rewarding career at Touchmark"></li>
      </ul>
    </div>
     <div id="header-content">
      <a class="toggleMenu" href="http://www.touchmark.com/careers" style="display: none;">Menu</a>
      <div id="header-info-contain">
        <div id="logo-container">
          <a href="/careers/index.html"><img alt="Touchmark" id="logo" src="images/touchmark_logo_300.png"></a>
        </div>
        <div id="nav-wrapper" style="visibility: visible;">
            <ul class="nav level1 desktop-nav" id="nav">
              <li><a class="top-level" href="videos.html"><span>Videos</span></a></li>
              <div class="nav-border"></div>  
              <li><a class="top-level" href="benefits.html"><span>Benefits</span></a></li>
              <div class="nav-border"></div>  
              <li class="selectedSearch"><a class="top-level selectedLink" href="jobsmain.aspx"><span>Search Jobs</span></a></li>
              <div class="nav-border"></div>  
              <li class="last selectedApply" style="background-color:#bb1f1f;"><a class="top-level" href="jobsmain.aspx"><span>Apply Now</span></a></li>    
  			  <div class="nav-border"></div>
			</ul>
        <span class="justify"></span>
        </div>
      </div>
    </div>
  </header>

  <section id="center">
    <div id="home">


        <%---TEMPLATE---%>
        <div class="wrapper">
            <div id="one">

                <div id="formWrapper">

                    <style type="text/css">body {display:none;}</style>

                    <form runat="server" id="f">

                    <div runat="server" id="DivNoPreHire" visible="false">
                    <br />
                    <h2>Your information has been received.</h2>
                    <br /><br />
                    Thank you for your interest in employment with Touchmark.
                    If we determine that your qualifications are an appropriate match for the position or if we
                    desire more information from you, we will contact you within 2 weeks. If you have not heard
                    from us by the end of that period, please be assured that your records will be retained for one year.
                    Should a position become open that matches your qualifications we will contact you.
                    <br /><br />
                    Touchmark Human Resources
                    </div>

                    <div runat="server" id="DivPreHire" visible="false">
                    <br />
                    <h2>Your information has been received.</h2>
                    <br /><br />
                    Thank you for your interest in employment with Touchmark.
                    In order for your resume to be reviewed, please click on the link below and complete the questionnaire,
                    which takes about 30 minutes to complete. We just sent you an email with the link, so if you are unable
                    to complete it at this time, please complete it at your earliest convenience. Your resume submission
                    will then be complete and your resume will be reviewed at that time.
                    <br /><br />
                    Once we have received your completed questionnaire, if we determine that you might be an appropriate match for
                    the position we will contact you within two weeks. If you have not heard from us by then, please be assured that
                    your resume will be retained for one year. Should a position become open that matches your qualifications we
                    will contact you.
                    <br /><br />
                    Thank you!
                    <br /><br />
                    <h3><asp:Hyperlink ID="HyperLinkPreHire" runat="server" /></h3>
                    </div>
                    
                    <script type="text/javascript">
                        if (self == top) {
                            document.getElementsByTagName("body")[0].style.display = 'block';
                        }
                        else {
                            top.location = self.location;
                        }
                    </script>

                    <!-- Begin INDEED conversion code -->
                    <script type="text/javascript">
                    // <![CDATA[
                        var indeed_conversion_id = '3970927210099154';
                        var indeed_conversion_label = '';
                    // ]]>
                    </script>
                    <%--<script type="text/javascript" src="//conv.indeed.com/pagead/conversion.js"></script>
                    <noscript>
                    <img height=1 width=1 border=0 src="//conv.indeed.com/pagead/conv/3970927210099154/?script=0">
                    </noscript>--%>
                    <!-- End INDEED conversion code -->

					</form>
                </div>

            </div>
        </div>
        <%---/TEMPLATE---%>
                       
    </div>
  </section> 
    
  <section id="bottom">
    <div id="home-content">
      <div>
        <h2>“I love my job! To make a difference in someone’s life on a daily basis by such simple things like a handshake, a smile, a compliment &hellip; it’s the best.”&nbsp;&nbsp;&nbsp;</h2>
        <div class="footerQuoteAuthor">
          <span>- Jonathan Ferns, Resident Care Coordinator, Touchmark at Mt. Bachelor Village</span>
        </div>
      </div>
    </div>
  </section>

  <footer>
    <div id="footer-container">
      &copy; <a href="http://touchmark.com/careers">2016 Touchmark, LLC, all rights reserved.</a>
      <a href="http://www.touchmark.com/privacy" class="privacy_link">Privacy policy</a>
      <img alt="Footer_symbols" class="footer_symbols" src="images/footer_symbols.png">
    </div>
  </footer>

</body>
</html>