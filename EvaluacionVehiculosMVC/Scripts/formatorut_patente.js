$(document).ready(function () {
    $('input#RUT').on('keyup', function () {
        var rut = $(this).val();
        rut = $.trim(rut);
        rut = rut.split('-').join('').split('.').join('');
        if ((rut.charAt(i) != '0') && (rut.charAt(i) != '1') && (rut.charAt(i) != '2') && (rut.charAt(i) != '3') && (rut.charAt(i) != '4') && (rut.charAt(i) != '5') && (rut.charAt(i) != '6') && (rut.charAt(i) != '7') && (rut.charAt(i) != '8') && (rut.charAt(i) != '9') && (rut.charAt(i) != 'k') && (rut.charAt(i) != 'K')) {
                alert("El valor ingresado no corresponde a un RUT valido.");
                return false;
            
        }
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