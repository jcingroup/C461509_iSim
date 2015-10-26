// 回頂端
$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('.goTop').fadeIn();
    } else {
        $('.goTop').fadeOut();
    }
});
$('.goTop').click(function (event) {
    $('html, body').animate({
        scrollTop: $($.attr(this, 'href')).offset().top
    }, 1000);
    event.preventDefault();
});

// 行動裝置的主選單
$(".menu-trigger").click(function (event){
    $("body").toggleClass("toggled");
    event.preventDefault();
});

// 行動裝置的產品左選單
$(".submenu-trigger .btn").click(function (event){
    $("#sidebar").slideToggle();
    event.preventDefault();
});

// include
$(document).ready(function(){
    $.get("header.html",function(data) {
        $("#header").html(data);
    });
    $.get("footer.html",function(data) {
        $("#footer").html(data);
    });
});
