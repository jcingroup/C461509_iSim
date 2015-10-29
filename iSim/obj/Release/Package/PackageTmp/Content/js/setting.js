// 回頂端
$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('.goTop').fadeIn(500);
    } else {
        $('.goTop').fadeOut(500);
    }
});

// 點選後跳到 href 指向的位置
$('.scroll-y, .goTop').click(function () {
    $('html, body').animate({
        scrollTop: $($.attr(this, 'href')).offset().top
    }, 1000);
    return false;
});


// 行動裝置的主選單
// $(".menu-trigger").click(function (event){
//     $("body").toggleClass("toggled");
//     event.preventDefault();
// });

// // 行動裝置的產品左選單
// $(".submenu-trigger .btn").click(function (event){
//     $("#sidebar").slideToggle();
//     event.preventDefault();
// });

// include
$.get("_include/header.html",function(data) {
    $("#header").html(data);
});
$.get("_include/footer.html",function(data) {
    $("#footer").html(data);
});