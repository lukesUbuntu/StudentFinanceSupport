$(function() {
    //load metisMenu module
    $('#side-menu').metisMenu();

});
(function ($) {
    //jquery plugin by luke hardiman
    $.fn.alertBox = function (message, success) {
        return (
        success = (typeof success == "boolean" && success == true) ? "success" : success,
        success = (typeof success == "string") ? "alert-" + success : "alert-danger",
        this.text(message),
        this.removeClass("alert-danger alert-success"),
        this.addClass(success),
        this.show())
    }
}(jQuery));
//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
// Sets the min-height of #page-wrapper to window size
$(function() {
    $(window).bind("load resize", function() {
        topOffset = 50;
        width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.navbar-collapse').addClass('collapse');
            topOffset = 100; // 2-row-menu
        } else {
            $('div.navbar-collapse').removeClass('collapse');
        }

        height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;
        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            $("#page-wrapper").css("min-height", (height) + "px");
        }
    });

    var url = window.location;
    var element = $('ul.nav a').filter(function() {
        return this.href == url || url.href.indexOf(this.href) == 0;
    }).addClass('active').parent().parent().addClass('in').parent();
    if (element.is('li')) {
        element.addClass('active');
    }
});
