// Exemplo de JavaScript inicial para desativar envios de formulário, se houver campos inválidos.
(function() {
    'use strict';
    window.addEventListener('load', function() {
    // Pega todos os formulários que nós queremos aplicar estilos de validação Bootstrap personalizados.
    var forms = document.getElementsByClassName('needs-validation');
    // Faz um loop neles e evita o envio
    var validation = Array.prototype.filter.call(forms, function(form) {
        form.addEventListener('submit', function (event) {
            if (form.checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });
  }, false);
})();

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();

    if ($(".maskInscricao").length)
        $(".maskInscricao").mask("999.999.999.999")

    if ($(".maskCPF").length)
        $(".maskCPF").mask("999.999.999-99")

    if ($(".maskCNPJ").length)
        $(".maskCNPJ").mask("99.999.999/9999-99")

    if ($(".maskPhone").length) {
        var SPMaskBehavior = function (val) {
            return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
        },
            spOptions = {
                onKeyPress: function (val, e, field, options) {
                    field.mask(SPMaskBehavior.apply({}, arguments), options);
                }
            };

        $('.maskPhone').mask(SPMaskBehavior, spOptions);

    }

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
        if ($(".toggler").hasClass("hidden"))
            $('.toggler').removeClass('hidden');
        else
            $('.toggler').addClass('hidden');
    });

    if ($('.dataPicker').length > 0) {
        $('.dataPicker').datetimepicker({
            "allowInputToggle": true,
            "showClose": true,
            "showClear": true,
            "showTodayButton": true,
            "format": "DD/MM/YYYY",
        });
    }

    $("#seleciona-tudo").on('click', function () {
        if ($(this).is(':checked')) {
            $('input:checkbox').prop("checked", true);
        } else {
            $('input:checkbox').prop("checked", false);
        }
    });

});

function setCamposCadastro() {
    if ($("#PessoaFisica").is(":checked")) {
        $("#CamposGenero").removeClass("hidden");
        $("#DataNascimento").prop("required", true);
        $("#Genero").prop("required", true);
        $("#CpfCnpj").unmask().mask("999.999.999-99");
    }
    else {
        $("#CamposGenero").addClass("hidden");
        $("#DataNascimento").prop("required", false);
        $("#Genero").prop("required", false);
        $("#CpfCnpj").unmask().mask("99.999.999/9999-99");
    }
}

function setIsencao() {
    if ($("#Isento").is(":checked")) {
        $("#RgIe").prop("required", false);
        $("#RgIe").prop("readonly", true);
        $("#RgIe").val("");
    }
    else {
        $("#RgIe").prop("required", true);
        $("#RgIe").prop("readonly", false);
    }
}