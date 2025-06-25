function fazerLogin(email, password, isChecked) {
    if (isChecked) {
        localStorage.setItem('savedEmail', email);
        localStorage.setItem('savedPassword', password);
        localStorage.setItem('rememberMe', 'true');
    } else {
        localStorage.removeItem('savedEmail');
        localStorage.removeItem('savedPassword');
        localStorage.removeItem('rememberMe');
    }
    fetch('/account/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: `email=${encodeURIComponent(email)}&password=${encodeURIComponent(password)}&rememberMe=${isChecked}`
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                window.location.href = data.redirectUrl; // Agora, rota controlada pelo backend
            } else {
                showBackendError(data.message);
            }
        });
};

function showBackendError(message) {
    const loginBtn = document.getElementById('loginButton');
    loginBtn.disabled = false;

    $('.alert').remove(); // remove alertas antigos

    let errorDiv = document.getElementById('formError');

    //Recria e insere no DOM, caso não exista o formDiv
    if (!errorDiv) {
        const container = document.getElementById('loginForm');

        errorDiv = document.createElement('div');
        errorDiv.id = 'formError';
        container.appendChild(errorDiv);
    }

    errorDiv.textContent = message;
    errorDiv.classList.remove('d-none');
    errorDiv.classList.add('alert', 'alert-danger', 'mt-3');
};