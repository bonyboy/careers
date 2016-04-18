$(document).ready(function() {
  // add class 'parent' to nav items that have subnav
  $(".nav li a").each(function() {
    if ($(this).next().length > 0) {
      $(this).addClass("parent");
    };
  })

  // bind click event for opening and closing mobile nav
  $(".toggleMenu").click(function(e) {
    e.preventDefault();
    $(this).toggleClass("active");
    $("#nav-wrapper").toggle();
  });

  // add span tags for adding down arrows at end of mobile nav links
  $("#nav li a.parent").wrapInner('<span class="down-arrow"></span>');

  // add span tags for making main nav menu full justified
  $("#nav-wrapper").append('<span class="justify"></span>');

  adjustMenu();
});

$(window).bind('resize orientationchange', function() {
  adjustMenu();
});

var adjustMenu = function() {
  if (Modernizr.mq('(max-width: 979px)')) {
    // display tablet/mobile nav
    $("#nav-border").hide();
    $("#shadow-bar").hide();
    $(".nav").removeClass("desktop-nav");
    $(".nav").addClass("mobile-nav");
    $(".toggleMenu").css("display", "inline-block");
    $(".mob-pay").css("display", "inline-block");
    $(".referfriend").css("display", "inline-block");
    $(".payol").css("display", "none");
    if (!$(".toggleMenu").hasClass("active")) {
      $("#nav-wrapper").hide();
    } else {
      $("#nav-wrapper").show();
    }
    $(".nav li").unbind('mouseenter mouseleave');
    $(".nav li a.parent").unbind('click').bind('click', function(e) {
      // must be attached to anchor element to prevent bubbling
      e.preventDefault();
      $(this).parent("li").siblings(".hover").removeClass("hover");
      $(this).parent("li").toggleClass("hover");
    });
  }


  else if(Modernizr.mq('all and (min-device-width: 981px)') && Modernizr.touch) {
    $(".toggleMenu").css("display", "none");
    $(".mob-pay").css("display", "none");
    $(".referfriend").css("display", "none");
    $(".payol").css("display", "inline-block");
    $(".nav").removeClass("mobile-nav");
    $(".nav").addClass("desktop-nav");
    $("#nav-wrapper").show();
    $("#nav-border").show();
    $("#shadow-bar").show();
    $(".nav li").removeClass("hover");
    $(".nav li a").unbind('click');

    $(".nav li a.parent").unbind('click').bind('click', function(e) {
      // must be attached to anchor element to prevent bubbling
      e.preventDefault();

    });
  }

  // else if (Modernizr.mq('(min-width: 1200px)') && Modernizr.touch) {
  //   console.log("nexus");


  //   $(".toggleMenu").css("display", "none");
  //   $(".mob-pay").css("display", "none");
  //   $(".referfriend").css("display", "none");
  //   $(".payol").css("display", "inline-block");
  //   $(".nav").removeClass("mobile-nav");
  //   $(".nav").addClass("desktop-nav");
  //   $(".nav li").removeClass("hover");
  //   $(".nav li a").unbind('click');

  //   $(".nav li a.parent").unbind('click').bind('click', function(e) {
  //     // must be attached to anchor element to prevent bubbling
  //     e.preventDefault();

  //   });
  // }
  
  else if (Modernizr.mq('(min-width: 980px)')) {
    // display desktop nav
    $(".toggleMenu").css("display", "none");
    $(".mob-pay").css("display", "none");
    $(".referfriend").css("display", "none");
    $(".payol").css("display", "inline-block");
    $(".nav").removeClass("mobile-nav");
    $(".nav").addClass("desktop-nav");
    $("#nav-wrapper").show();
    $("#nav-border").show();
    $("#shadow-bar").show();
    $(".nav li").removeClass("hover");
    $(".nav li a").unbind('click');

    $(".nav li").unbind('click').bind('click', function(e) {
        window.location = $(this).find('a').attr('href');
        e.stopPropagation();
        e.preventDefault();
        return false;
      });

    $(".nav li").unbind('mouseenter mouseleave').bind('mouseenter mouseleave', function() {
      // must be attached to li so that mouseleave is not triggered when hover over submenu
      $(this).toggleClass('hover');
    });
  }

  
  // move any last desktop drop-down menu to left to keep from going off right side of screen
  if ((Modernizr.mq('(min-width: 980px)')) && (Modernizr.mq('(max-width: 1319px)'))) {
    $("ul.desktop-nav li.last .level2").css("margin-left", "-188px");
  }
}

