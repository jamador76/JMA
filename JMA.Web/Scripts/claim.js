(function ($) {
    function defineMasks() {
        $(".zip5").mask("99999");
        $(".phone").mask("(999) 999-9999");
    };

    function toggleAddress() {
        if ($('.isForeign').is(':checked')) {
            $(".foreign").show();
            $(".domestic").hide();
            $(':input', '.domestic')
                .val('')
                .removeAttr('checked')
                .removeAttr('selected');
        } else {
            $(".domestic").show();
            $(".foreign").hide();
            $(':input', '.foreign')
                .val('')
                .removeAttr('checked')
                .removeAttr('selected');
        }
    };

    $(document).ready(function () {
        defineMasks();
        toggleAddress();

        $('.isForeign').change(function () {
            toggleAddress();
        });

        $('#chkIAgree').change(function () {
            if ($('#chkIAgree').prop("checked")) {
                $("#SubmitClaim").removeAttr("disabled").removeClass("disabledfield");
            }
            else {
                $("#SubmitClaim").prop("disabled", "disabled").addClass("disabledfield");
            }
        });
    });
}(jQuery));