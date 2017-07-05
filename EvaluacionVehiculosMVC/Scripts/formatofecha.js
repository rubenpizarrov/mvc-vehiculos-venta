$(function () {
    $.datepicker.setDefaults($.datepicker.regional["es"]);
    $("input:text.fecha").datepicker({
        regional: 'es',
        dateFormat: 'dd-mm-yy',
        altField: '#actualDate',

        gotoCurrent: true,
        maxDate: '+1m +1w'
    });

    
    $.validator.addMethod('date',
    function (value, element, params) {
        if (this.optional(element)) {
            return true;
        }

        var ok = true;
        try {
            $.datepicker.parseDate('dd-mm-yy', value);
        }
        catch (err) {
            ok = false;
        }
        return ok;
    });
});