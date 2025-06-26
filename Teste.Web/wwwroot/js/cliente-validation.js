String.prototype.reverse = function () {
    return this.split('').reverse().join('');
};
function mascaraCpfCnpj(campo) {
    if (!campo) return;

    var valor = campo.value.replace(/\D/g, "").split("").reverse().join("");
    var resultado = "";

    if (valor.length <= 11) {
        var mascara = "###.###.###-##".split("").reverse().join("");
    } else {
        var mascara = "##.###.###/####-##".split("").reverse().join("");
    }

    for (var x = 0, y = 0; x < mascara.length && y < valor.length;) {
        if (mascara.charAt(x) !== '#') {
            resultado += mascara.charAt(x);
            x++;
        } else {
            resultado += valor.charAt(y);
            y++;
            x++;
        }
    }

    campo.value = resultado.reverse();
};

function showFieldError(message) {
    const errorDiv = document.getElementById('documentoError');
    errorDiv.textContent = message;
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

document.addEventListener("DOMContentLoaded", function () {
    const documento = document.querySelector('input[name="Documento"]');
    const form = document.querySelector('form[name="formCliente"]');

    if (documento) {
        mascaraCpfCnpj(documento);
    }

    form.addEventListener('submit', function (e) {
        const valorSemMascara = documento.value.replace(/\D/g, '');
        console.log("Tentando validar:", valorSemMascara);

        if (!validarDocumento(valorSemMascara)) {
            e.preventDefault();
            console.log("Documento inválido");
            showFieldError('Documento inválido! Insira um CPF ou CNPJ válido.');
        }
    });
});