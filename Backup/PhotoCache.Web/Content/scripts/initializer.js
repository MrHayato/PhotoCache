window.App = {
    Views: {},
    Models: {},
    Collections: {},
    Templates: {}
};

var _gaq = _gaq || [];
_gaq.push(["_setAccount", "UA-34594774-1"]);
_gaq.push(["_trackPageview"]);

Bootstrap.load([
    "backbone",
    "bootstrap.dropdown",
    "@" + ("https:" === document.location.protocol ? "https://ssl" : "http://www") + ".google-analytics.com/ga.js"
]);