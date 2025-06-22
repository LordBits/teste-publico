document.addEventListener('DOMContentLoaded', function () {

    const form = document.getElementById('loginForm');
    const loginBtn = document.getElementById('loginButton');
    if (!loginBtn) return;

    form.addEventListener('submit', function (event) {
        event.preventDefault();  // Sempre previne submit tradicional

        // Desabilita o botão para evitar múltiplos cliques
        loginBtn.disabled = true;

        clearErrors();

        const email = document.getElementById('email').value.trim();
        const password = document.getElementById('password').value.trim();

        let hasError = false;

        // Validação email
        if (!email) {
            showFieldError('email', 'O campo Email é obrigatório.');
            hasError = true;
        } else if (!validateEmail(email)) {
            showFieldError('email', 'Informe um email válido.');
            hasError = true;
        }

        // Validação senha
        if (!password) {
            showFieldError('password', 'O campo Senha é obrigatório.');
            hasError = true;
        }

        // Só chama o controller JS se não tiver erro
        if (!hasError) {
            fazerLogin(email, password);
        }else{
            // habilita o button
            loginBtn.disabled = false;
        }
    });

    form.reset();
    document.getElementById('email').focus();
});

function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

function showFieldError(field, message) {
    const input = document.getElementById(field);
    const errorDiv = document.getElementById(field + 'Error');

    input.classList.add('is-invalid');
    errorDiv.textContent = message;
}

function clearErrors() {
    ['email', 'password'].forEach(field => {
        const input = document.getElementById(field);
        const errorDiv = document.getElementById(field + 'Error');

        input.classList.remove('is-invalid');
        errorDiv.textContent = '';
    });

    document.getElementById('formError').textContent = '';
}