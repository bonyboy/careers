//inject script after jquery include

jQuery(function ($) {
    $('#carousel1').carousel({
        interval: false
    });
});


$(function () {
//    $('li').hover(function () {
//        $(this).addClass('highlight');
//    }, function () {
//        $(this).removeClass('highlight');
//    });

    $('.nav li').click(function () {
        $('.nav li').removeClass('active');
        $(this).addClass('active');
    });
});