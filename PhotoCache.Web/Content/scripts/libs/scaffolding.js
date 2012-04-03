/// <reference path="backbone-0.9.2.js" />
/// <reference path="jquery-1.7.2.js" />
/// <reference path="backbone.modelbinding-0.5.0.js" />
/// <reference path="bootstrap-2.0.2.js" />
/// <reference path="underscore-1.3.1.js" />
// Need to load the above libraries first

(function($) {
    "use strict";

    var setNavigation = function() {
        var location = document.location.href;
        var sectionArray = location.split("/");
        var section = sectionArray[sectionArray.length - 1];
        var $link = $(".navbar .nav").find("a[rel=" + section + "]").parent("li");
        if ($link.length == 0)
            $link = $(".navbar .nav").find("li:first-child");
        $link.addClass("active");
    };

    $(document).ready(function() {
        setNavigation();
    });
})(jQuery);