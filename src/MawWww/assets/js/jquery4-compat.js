// jquery-validation-unobtrusive (4.0.0, still its latest release) calls two helpers jQuery 4
// removed, so validation messages throw "parseJSON is not a function" when they show.  put
// back just those two, as jQuery 3 defined them, and only where they are missing.  load after
// jquery and before jquery.validate.unobtrusive
(function ($) {
    if (typeof $.parseJSON !== "function") {
        $.parseJSON = JSON.parse;
    }

    if (typeof $.isFunction !== "function") {
        $.isFunction = function (obj) {
            return typeof obj === "function" && typeof obj.nodeType !== "number";
        };
    }
})(jQuery);
