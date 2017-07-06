$(document).ready(function () {
    $('input#RUT').on('keyup', function () {
        var rut = $(this).val();
        rut = $.trim(rut);
        rut = rut.split('-').join('').split('.').join('');
        $(this).val(rut.toUpperCase());
    });

    $('input#Patente').on('keyup', function (e) {
        var patente = $(this).val();
        patente = $.trim(patente);
        //patente = patente.split('-').join('');
        $(this).val(patente.toUpperCase());

        if ((e.keyCode > 47 && e.keyCode < 58) || (e.keyCode < 106 && e.keyCode > 95)) {
            this.value = this.value.replace(/([A-Z]{2})\-?([A-Z]{2})\-?(\d{2})/g, '$1-$2-$3');
            return true;
        }
        this.value = this.value.replace(/[^\-0-9A-Z]/g, '');

    });
});