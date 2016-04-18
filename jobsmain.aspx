<%@ Page Language="vb" debug="false" AutoEventWireup="false" CodeFile="jobsmain.aspx.vb" Inherits="jobsmain" %>

<!DOCTYPE html>
<html class="js no-touch wf-museoslab-i3-inactive wf-museoslab-i7-inactive wf-museoslab-n3-inactive wf-museoslab-n5-inactive wf-museoslab-n7-inactive wf-opensans-i4-inactive wf-opensans-i6-inactive wf-opensans-i7-inactive wf-opensans-n4-inactive wf-opensans-n6-inactive wf-opensans-n7-inactive wf-inactive" lang="en" style="">

<!--<![endif]-->

<head>

<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<meta name="format-detection" content="telephone=no">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<meta content="Touchmark offers rewarding careers with competitive benefits and salaries. Please view our website for current employment opportunities." name="description" />

<title>Career Benefits at Touchmark</title>

<!-- CSS Styles -->
<link rel="stylesheet" href="css/touchmarkcom.css" type="text/css" media="screen">
<link rel="stylesheet" href="css/responsiveslides.css" type="text/css" media="screen">
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/3.4.0/animate.min.css"> <!-- Optional -->
<link rel="stylesheet" href="css/liquid-slider.css">
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">

<link href='https://fonts.googleapis.com/css?family=Lato:100,300,400,700|PT+Serif' rel='stylesheet' type='text/css'>
<link href="images/favicon.png" rel="shortcut icon" type="image/png">
<link href="css/print_style.css" media="print" rel="stylesheet" type="text/css">

<!-- Javascript -->
<script src="js/jquery.js" type="text/javascript"></script>
<script src="js/touchmarkcom.js" type="text/javascript"></script>
<script src="js/modernizr.js" type="text/javascript"></script>
<script src="js/picture-tag.js" type="text/javascript"></script>
<script src="js/responsive-nav.js" type="text/javascript"></script>
<script src="js/jquery.fitvids.js" type="text/javascript"></script>
<script src="js/g5-responsive.js" type="text/javascript"></script>
<script src="js/scripts.js" type="text/javascript"></script>
<script src="js/jquery.tooltipster.js" type="text/javascript"></script>
<script src="js/responsiveslides.min.js" type="text/javascript"></script>
<script type="text/javascript">var ie8 = false; var ie9 = false;</script>
<!--[if IE 8]><script type="text/javascript">var ie8 = true;</script><![endif]-->
<!--[if IE 9]><script type="text/javascript">var ie9 = true;</script><![endif]-->


<script>
    $(document).ready(function() {
    "use strict";
    function pictureInit(callback) {
        if(ie9 === true){
        $('video').each(function() {
            $(this).children().unwrap();
        });
        } else if (ie8 === true) {
        $('#slideshow img[src*="tiny-"]').each(function() {
            var newSrc = $(this).attr('src');
            newSrc = newSrc.replace('tiny-','large-');
            $(this).attr('src', newSrc);
        });
        }
        callback();
    }

    pictureInit(function() {
        $('picture').pictureTag();
    });

    // set up drop-downs inside mobile nav menu
    touchNavToggle();

    });
</script>

<script>
  $(function() {
    $(".rslides").responsiveSlides({
    speed: 700,
    maxwidth: 1600           
    });
  });
</script>


</head>

<body class="corp interior">

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

      <!-- ResponsiveSlides GWM 2/11/16 -->
  <header>
    <div class="rslides_container">
      <ul class="rslides" id="slider1">
        <li><img src="images/careers-interior-benefits.jpg" alt=""></li>
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

  <div id="main_contain">
    <section id="main">
        <div id="editor-content">
            <div class="part_text" id="p_content">      
                <div id="home">
                    <h1>Touchmark Careers</h1>

                    <p>Touchmark is a progressive company that develops, owns, and operates award-winning full-service retirement communities in the United States and Canada.  More than 2,000 people work at Touchmark, and each of us is committed to a common mission:  To enrich people&rsquo;s lives.</p>

                    <p>Our team members have been enriching the lives of older adults and their families since 1980, when our CEO Werner G. Nistler, Jr founded the company, and we hire people with the same core values from which Touchmark was born – service, integrity, compassion, teamwork, and excellence.</p>

                    <p>Touchmark is an Equal Opportunity Employer.  Learn more about us at <a href="http://touchmark.com">Touchmark.com</a>.</p>
           
                    <div class="wrapper">
                        <div id="one">
                            <p>Where Are You Interested In Working?</p>

                                
                                <%--code--%>
                                <style>.aspNetHidden{display:none;}</style>
                                <form runat="server" id="f">
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" OnInit="SqlDataSource_Init" />
                                <asp:Repeater ID="Repeater3" runat="server" DataSourceID="SqlDataSource3">
	                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="CheckBoxEntity" Text='<%# "&nbsp;" & Eval("state") & ":&nbsp;" & Eval("city") & ",&nbsp;" & Eval("entitylong") %>' />
                                <asp:HiddenField runat="server" ID="HiddenFieldEntityId" Value='<%#Eval("id") %>' Visible="false" /><br />
                                </ItemTemplate>
                                </asp:Repeater>
                                <asp:Button runat="server" ID="ButtonSubmit" Text="Submit" OnClick="ButtonSubmit_Click" />
					            </form>
                                <%--endcode--%>

                        </div>
                        <div id="two">
                            <img alt="Map of Touchmark locations" src="images/map.png" />
                        </div>
                    </div>
                </div>
            </div>
        </div>               
    </section>
  </div> 
    
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