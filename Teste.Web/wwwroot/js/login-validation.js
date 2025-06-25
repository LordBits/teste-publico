document.addEventListener('DOMContentLoaded', function () {
    const emailInput = document.getElementById('email');
    const passwordInput = document.getElementById('password');
    const rememberMeCheckbox = document.getElementById('rememberMe');

    // Quando a página carrega, tenta preencher os campos
    if (localStorage.getItem('rememberMe') === 'true') {
        emailInput.value = localStorage.getItem('savedEmail') || '';
        passwordInput.value = localStorage.getItem('savedPassword') || '';
        rememberMeCheckbox.checked = true;
    }

    const togglePassword = document.getElementById('togglePassword');

    togglePassword.addEventListener('click', function () {
        const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordInput.setAttribute('type', type);

        // Troca o ícone
        if (type === 'text') {
            togglePassword.classList.remove('bi-eye');
            togglePassword.classList.add('bi-eye-slash');
        } else {
            togglePassword.classList.remove('bi-eye-slash');
            togglePassword.classList.add('bi-eye');
        }
    });

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
        const isChecked = document.getElementById('rememberMe').checked;

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
            fazerLogin(email, password, isChecked);
        }else{
            // habilita o button
            loginBtn.disabled = false;
        }
    });

    document.getElementById('email').focus();
});

function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
};

function showFieldError(field, message) {
    const input = document.getElementById(field);
    const errorDiv = document.getElementById(field + 'Error');

    input.classList.add('is-invalid');
    errorDiv.textContent = message;
};

function clearErrors() {
    ['email', 'password'].forEach(field => {
        const input = document.getElementById(field);
        const errorDiv = document.getElementById(field + 'Error');

        input.classList.remove('is-invalid');
        errorDiv.className = 'd-none';
        errorDiv.textContent = '';
    });

    document.getElementById('formError').className = 'd-none';
    document.getElementById('formError').textContent = '';
};