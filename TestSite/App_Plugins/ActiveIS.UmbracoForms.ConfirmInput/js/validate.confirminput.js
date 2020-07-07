jQuery.validator.addMethod("validateconfirminput",
    function (value, element, params) {
        var inputId;
        var input;
        var confirmId;
        var confirmInput;
        var inputVal;
        var confirmInputVal;

        if (element.id.includes("_confirm")) {
            inputId = element.id.split("_")[0];
            input = jQuery("#" + inputId);
            inputVal = input.val();

            confirmId = element.id;
            confirmInput = jQuery("#" + confirmId);
            confirmInputVal = confirmInput.val();

            if (inputVal !== confirmInputVal) {
                input.addClass("input-validation-error");
                return false;
            }
        } else {
            inputId = element.id;
            input = jQuery("#" + inputId);
            inputVal = input.val();

            confirmId = element.id;
            confirmInput = jQuery("#" + confirmId + "_confirm");
            confirmInputVal = confirmInput.val();

            if (inputVal !== confirmInputVal) {
                confirmInput.addClass("input-validation-error");
                return false;
            }
        }

        input.removeClass("input-validation-error");
        confirmInput.removeClass("input-validation-error");
        return true;
    });
jQuery.validator.unobtrusive.adapters.addBool("validateconfirminput");