$.fn.exists = function () {
    return this.length !== 0;
};

(function ($) {
    function defineMasks() {
        $(".claim8").mask("99999999");
        $(".pin6").mask("999999");
    };

    $(document).ready(function () {
        defineMasks();
    });
}(jQuery));