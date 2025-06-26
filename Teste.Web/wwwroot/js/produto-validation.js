String.prototype.reverse = function(){
    return this.split('').reverse().join(''); 
};
function mascaraMoeda(campo) {
    var valor = campo.value.replace(/\D/g, "").split("").reverse().join("");
    var resultado = "";
    var mascara = "##.###.###,##".split("").reverse().join("");

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

function mascaraPeso(pesoInput) {
    let valor = pesoInput.value.replace(/\D/g, ""); // Remove tudo que não for número

    // Remove zeros à esquerda
    valor = valor.replace(/^0+/, "");

    if (valor.length === 0) {
        pesoInput.value = "";
        return;
    }

    while (valor.length < 4) {
        valor = "0" + valor; // Garante ao menos 4 dígitos para formar decimal com 3 casas
    }

    const parteInteira = valor.slice(0, valor.length - 3);
    const parteDecimal = valor.slice(-3);

    pesoInput.value = `${parteInteira},${parteDecimal}`;
};

// Validador de Código de Barras EAN-13
function validarCodigoBarras(input) {
    const codigo = input.value.trim();
    const mensagemErro = input.nextElementSibling;

    mensagemErro.textContent = "";

    if (!/^[0-9]{13}$/.test(codigo)) {
        mensagemErro.textContent = "O código de barras deve conter 13 dígitos.";
        return;
    }

    // Cálculo do dígito verificador EAN-13
    let soma = 0;
    for (let i = 0; i < 12; i++) {
        const num = parseInt(codigo[i]);
        soma += i % 2 === 0 ? num : num * 3;
    }
    const digitoCalculado = (10 - (soma % 10)) % 10;
    const digitoInformado = parseInt(codigo[12]);

    if (digitoCalculado !== digitoInformado) {
        mensagemErro.textContent = "Código de barras inválido (dígito verificador incorreto).";
    }
};

document.addEventListener("DOMContentLoaded", function () {
    const valorVenda = document.querySelector('input[name="ValorVenda"]');
    const pesos = document.querySelectorAll('input[name="PesoBruto"], input[name="PesoLiquido"]');
    const codigoBarra = document.querySelector('input[name="CodigoBarra"]');

    if (valorVenda) {
        valorVenda.addEventListener('input', function () {
            mascaraMoeda(this);
        });
    }

    pesos.forEach(p => {
        p.addEventListener('input', function () {
            mascaraPeso(this);
        });
    });

    if (codigoBarra) {
        codigoBarra.addEventListener('blur', function () {
            validarCodigoBarras(this);
        });
    }
});