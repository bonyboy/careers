function touchNavToggle(){
  $("ul.nav").on("click", "a", function(event){
    if(Modernizr.touch && Modernizr.mq('all and (min-width :979px)') || (navigator.userAgent.match(/iemobile/i))) {
      var dd = $(this).next('ul');
      if(dd.length == 1){
        if(dd.hasClass('hover')){
          dd.removeClass('hover');
        } else {
          dd.addClass('hover');
        }
      }
    }
  });
}

function switchToMobileLeads() {
  if((Modernizr.mq('all and (max-device-width: 767px)') && Modernizr.touch) || (navigator.userAgent.match(/iemobile/i))) {
    $('a[href*="/leads/"]').each(function(){
      url = this.href.split('/');
      url[4] = "mobile_" + url[4];
      this.href = url.join('/');
    });
  }
}

function scrollToContent() {
  if((Modernizr.mq('all and (max-width: 600px)') && Modernizr.touch) || (navigator.userAgent.match(/iemobile/i))) {
    $('nav[role=navigation] a[href]').attr('href', function(href) {
      var param = "&scrollto=true";
      return this.href + param;
    });
    var pathname = window.location.pathname;
    if (pathname.indexOf("scrollto") >= 0){
      $('html, body').animate({
        scrollTop: $("#content-top", "formWrapper").offset().top
      }, 500);
    }
  }
}

function removeBreakTags() {
  if((Modernizr.mq('all and (max-device-width: 960px)') && Modernizr.touch) || (navigator.userAgent.match(/iemobile/i))) {
    $("#callout-contain br").replaceWith(" ");
  }
}
