$(document).ready(function(){
  $('.tooltip').tooltipster();

  $("#nav-wrapper").css('visibility', 'visible');
  $('#nav > li').after('<div class="nav-border"></div>');

 // adds svg arrow to list items

 $('<span class="li-arrow hidden-print">&nbsp;</span>').insertBefore('#p_content.part_text li');
 $('<span class="li-arrow hidden-print">&nbsp;</span>').insertBefore('#p_col1_bottom.part_text li');

 if (!$('html').hasClass("lt-ie10")) {

  $('<img id="li-arrow" class="svg" src="images/tm_arrow.svg"></img>').appendTo('span.li-arrow');
  $('<img id="header_arrow" class="svg" src="images/tm_cta_arrow.svg" ></img>').appendTo('a.communities-button span');
  $('<img id="tm_cta_arrow" class="svg" src="images/tm_cta_arrow.svg"></img>').appendTo('span.arrow-block');
  $('<img id="tm_hm-cta_arrow" class="svg" src="images/tm_cta_arrow.svg"></img>').appendTo('div.cta-item a span');

  $('<img id="tm_hm-cta_arrow" class="svg tester" src="images/tm_cta_arrow.svg"></img>').appendTo('#home #cta-contain div.cta div.cta-item a span');
 }

 else {
  $('<img id="li-arrow" src="images/tm_arrow_sm.svg"></img>').appendTo('span.li-arrow');
  $('<img id="block-arrow_sm" src="images/block-arrow_sm.svg"></img>').appendTo('span.arrow-block');
  $('<img id="block-arrow" src="images/block-arrow.svg"></img>').appendTo('div.cta-item a span');
  $('<img id="header_arrow" src="images/c-arrow.svg" ></img>').appendTo('a.communities-button span');
}

  $('.vid-container').fitVids();

  $('a.callout > p > img').unwrap();

   $('.sidebar > p > img').unwrap();

   $('.sidebar > div.part_text > p > img').unwrap();

  if (navigator.userAgent.match(/iemobile/i)) {
   $('div.tel').wrapInner( "<pre style='display:inline;'></pre>");

   $('span.win').wrapInner( "<pre style='display:inline; padding:0'></pre>");

   $('h1.location-name').wrapInner( "<pre style='display:inline;'></pre>");
  }


  switchToMobileLeads();
  removeBreakTags();

});


$(window).load(function(){
 /*
   * Replace all SVG images with inline SVG
 */
  jQuery('img.svg').each(function(){
      var $img = jQuery(this);
      var imgID = $img.attr('id');
      var imgClass = $img.attr('class');
      var imgURL = $img.attr('src');

      jQuery.get(imgURL, function(data) {
          // Get the SVG tag, ignore the rest
          var $svg = jQuery(data).find('svg');

          // Add replaced image's ID to the new SVG
          if(typeof imgID !== 'undefined') {
              $svg = $svg.attr('id', imgID);
          }
          // Add replaced image's classes to the new SVG
          if(typeof imgClass !== 'undefined') {
              $svg = $svg.attr('class', imgClass+' replaced-svg');
          }

          // Remove any invalid XML tags as per http://validator.w3.org
          $svg = $svg.removeAttr('xmlns:a');

          // Replace image with new SVG
          $img.replaceWith($svg);

      }, 'xml');

  });
});



