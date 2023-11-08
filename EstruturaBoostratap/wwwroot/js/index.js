﻿// Exemplo de JavaScript inicial para desativar envios de formulário, se houver campos inválidos.
(function () {
    'use strict';
    window.addEventListener('load', function () {
        // Pega todos os formulários que nós queremos aplicar estilos de validação Bootstrap personalizados.
        var forms = document.getElementsByClassName('needs-validation');
        // Faz um loop neles e evita o envio
        var validation = Array.prototype.filter.call(forms, function (form) {
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

    if ($(".maskRG").length)
        $(".maskRG").mask("99.999.999-9")

    if ($(".maskCPF").length)
        $(".maskCPF").mask("999.999.999-99")

    if ($(".maskCNPJ").length)
        $(".maskCNPJ").mask("99.999.999/9999-99")

    if ($(".maskCEP").length)
        $(".maskCEP").mask("99999-999")

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

    if ($('.dataPicker').length > 0) {
        $('.dataPicker').datetimepicker({
            "allowInputToggle": true,
            "showClose": true,
            "showClear": true,
            "showTodayButton": true,
            "format": "DD/MM/YYYY",
        });
    }

    if ($(".fancyboxIframe").length > 0) {
        $(".fancyboxIframe").fancybox({
            fitToView: false,
            width: '50%',
            height: '70%',
            autoSize: false,
            closeClick: false,
            openEffect: 'none',
            closeEffect: 'none',
            iframe: {
                scrolling: 'auto',
                preload: true
            }
        });
    }

    $("#seleciona-tudo").on('click', function () {
        if ($(this).is(':checked')) {
            $('input:checkbox').prop("checked", true);
        } else {
            $('input:checkbox').prop("checked", false);
        }
    });

    $(".btn-delete").click(function () {
        var TelefoneID = $(this).data("delete");

        $.ajax({
            url: "/Devedores/DeleteTelefone",
            type: "POST",
            data: {
                id: TelefoneID
            },
            success: function (retorno) {
                if (retorno == true)
                    $("#dados_" + TelefoneID).remove();
            }
        });
    });

    $(".btn-savar").click(function () {
        var TelefoneID = $(this).data("save");
        var Telefone = $("#Tele_" + TelefoneID).val();
        var Descricao = $("#Desc_" + TelefoneID).val();

        $.ajax({
            url: "/Devedores/AlterarTelefone",
            type: "POST",
            data: {
                id: TelefoneID,
                telefone: Telefone,
                descricao: Descricao,
            },
            success: function (retorno) {
                if (retorno == true) {
                    $("#Retorno_" + TelefoneID).html('<p class="text-success">Dados Alterados!</p>');
                    setTimeout(function () {
                        $("#Retorno_" + TelefoneID).empty();
                    }, 2000);
                }
            }
        });
    });

    $(".btn-pagar").click(function () {
        var Pagamento = $(this).data("page");

        $.ajax({
            url: "/Devedores/PagamentoParcela",
            type: "POST",
            data: {
                id: Pagamento
            },
            success: function (retorno) {
                $("#Pago_" + Pagamento).html(retorno.dataAtual);
                $("#Desc_" + Pagamento).html(retorno.descricao);
            }
        });
    });

    $("#Modalidade").change(function () {
        var ModalidadeSelecionada = $(this).val();
        if (ModalidadeSelecionada == 1) {
            $("#QTDParcela").prop("disabled", false);
        } else {
            $("#QTDParcela").prop("disabled", true);
            $("#QTDParcela").val("");
        }
    });

    $("#adicionar-campo").click(function () {
        var novoCampo = $(".input-container:first").clone();
        novoCampo.find("input").val("");
        novoCampo.find("input.maskPhone").unmask();
        novoCampo.find("input.maskPhone").mask("(00) 0000-00009");

        $(".input-container:last").after(novoCampo);
    });

    $("body").on("click", ".remover-campo", function () {
        $(this).closest(".input-container").remove();
    });

    $("#SimularOpcao").click(function () {

        var DevedorID = $("#DevedorID").val();
        var ContratoID = $("#ContratoID").val();
        var DataPagamento = $("#data-pagamento").val();
        var Modalidade = $("#Modalidade").val();
        var QTDParcela = $("#QTDParcela").val();

        if (DataPagamento == "") {
            alert("Informe Uma Data Para Pagamento!");
            return false;
        }

        if (Modalidade == "") {
            alert("Selecione Uma Modalidade de Pagamento!");
            return false;
        }

        if (Modalidade == 1) {
            if (QTDParcela == "") {
                alert("Informe a Quantidade de Parcelas!");
                return false;
            }
        }

        $.ajax({
            url: "/Devedores/RealizarSimulacao",
            type: "POST",
            data: {
                id: DevedorID,
                contratoid: ContratoID,
                datapagamento: DataPagamento,
                modalidade: Modalidade,
                qtdparcela: QTDParcela
            },
            success: function (retorno) {
                console.log(retorno);
                var tabela = $('<table class="table table-striped table-hover">');
                var cabecalho = $('<thead class="table-light">');
                var cabecalhoRow = $("<tr>");
                cabecalhoRow.append($("<th>").text("CPF"));
                cabecalhoRow.append($("<th>").text("Contrato"));
                cabecalhoRow.append($("<th>").text("Data de Pagamento"));
                cabecalhoRow.append($("<th>").text("Valor Principal"));
                cabecalhoRow.append($("<th>").text("Valor Atualizado"));
                cabecalhoRow.append($("<th>").text("Valor Modalidade"));
                cabecalho.append(cabecalhoRow);

                var corpoTabela = $("<tbody>");
                var linha1 = $("<tr>");
                linha1.append($("<td>").text(retorno.listaParcelas[0].cpf));
                linha1.append($("<td>").text(retorno.listaParcelas[0].contrato));
                linha1.append($("<td>").text(moment(retorno.listaParcelas[0].dataPagamento).format("DD/MM/YYYY")));
                linha1.append($("<td>").text(retorno.listaParcelas[0].valorPrincipal));
                linha1.append($("<td>").text(retorno.listaParcelas[0].valorAtualizado));
                linha1.append($("<td>").text(retorno.listaParcelas[0].valorFinal));


                corpoTabela.append(linha1);
                tabela.append(cabecalho);
                tabela.append(corpoTabela);

                $("#RetornoCalculo").append(tabela);
            }
        });
    });

});

function VerificaCPF(element_id, CPF) {

    var CPF = CPF.replace(/[.\-\/]/g, "");
    erro = 0;

    if ($("#" + element_id).val().length == 0) {
        return false;
    }

    if (!validaCpf(CPF)) {
        alert("CPF INV\u00c1LIDO!");

        $("#" + element_id).val('');
        $("#" + element_id).focus();
    }
    else {
        $("#" + element_id).css("color", "#0ad008");
    }

}

function validaCpf(cpf) {

    if (cpf.length != 11 || cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" || cpf == "33333333333" || cpf == "44444444444" || cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" || cpf == "99999999999") {
        return false;
    }

    add = 0;
    for (i = 0; i < 9; i++)
        add += parseInt(cpf.charAt(i)) * (10 - i);
    rev = 11 - (add % 11);

    if (rev == 10 || rev == 11)
        rev = 0;

    if (rev != parseInt(cpf.charAt(9)))
        return false;

    add = 0;
    for (i = 0; i < 10; i++)
        add += parseInt(cpf.charAt(i)) * (11 - i);
    rev = 11 - (add % 11);
    if (rev == 10 || rev == 11)
        rev = 0;
    if (rev != parseInt(cpf.charAt(10)))
        return false;

    return true;
}

function BuscarCep(cepOriginal) {
    var cep = cepOriginal.replace('-', '');

    if (cep == "")
        return false;

    $(".mask-loading").removeClass("hidden");

    $.ajax({
        url: "/BuscaCep/BuscaEnderecos",
        type: "POST",
        data: {
            cep: cep
        },
        success: function (retorno) {
            if (retorno.erro == true) {
                alert("Cep Não Encontrado!");
            }
            else {
                $("#Endereco").val(retorno.logradouro);
                $("#Bairro").val(retorno.bairro);
                $("#Cidade").val(retorno.localidade);
                $("#Estado").val(retorno.uf);
                $("#IBGE").val(retorno.ibge);
                $("#Numero").focus();
            }

            $(".mask-loading").addClass("hidden");
        }
    });
}