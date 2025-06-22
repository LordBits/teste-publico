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
    const errorDiv = document.getElementById('formError');
    const loginBtn = document.getElementById('loginButton');
    errorDiv.textContent = message;
    errorDiv.classList.add('text-danger', 'alert', 'alert-danger');
    //habilita o button
    loginBtn.disabled = false;
};