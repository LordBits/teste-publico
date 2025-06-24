$(document).ready(function () {
    $('#Documento').on('input', function () {
        var val = $(this).val().replace(/\D/g, '');

        if (val.length <= 11) {
            $(this).val(formatarCPF(val));
        } else {
            $(this).val(formatarCNPJ(val));
        }
    });

    $('form').on('submit', function (e) {
        var documento = $('#Documento').val().replace(/\D/g, '');
        if (!validarDocumento(documento)) {
            e.preventDefault();
            showFieldError('Documento inválido! Insira um CPF ou CNPJ válido.');
        }
    });

    function showFieldError(message) {
        const errorDiv = document.getElementById('documentoError');
        errorDiv.textContent = message;
    };

    function formatarCPF(cpf) {
        return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
    };

    function formatarCNPJ(cnpj) {
        return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, "$1.$2.$3/$4-$5");
    };

    function validarDocumento(doc) {
        if (doc.length === 11) return validarCPF(doc);
        if (doc.length === 14) return validarCNPJ(doc);
        return false;
    };

    function validarCPF(cpf) {
        if (/^(\d)\1+$/.test(cpf)) return false;
        var soma = 0;
        for (var i = 0; i < 9; i++) soma += parseInt(cpf.charAt(i)) * (10 - i);
        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        soma = 0;
        for (var i = 0; i < 10; i++) soma += parseInt(cpf.charAt(i)) * (11 - i);
        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf.charAt(9) == digito1 && cpf.charAt(10) == digito2;
    };

    function validarCNPJ(cnpj) {
        if (/^(\d)\1+$/.test(cnpj)) return false;
        var tamanho = cnpj.length - 2;
        var numeros = cnpj.substring(0, tamanho);
        var digitos = cnpj.substring(tamanho);
        var soma = 0;
        var pos = tamanho - 7;
        for (var i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2) pos = 9;
        }
        var resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0)) return false;

        tamanho = tamanho + 1;
        numeros = cnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;
        for (var i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2) pos = 9;
        }
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        return resultado == digitos.charAt(1);
    };
});