function fazerLogin(email, password) {
    fetch('/Account/Login', {
        method: 'POST',
            headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: `email=${encodeURIComponent(email)}&password=${encodeURIComponent(password)}`
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
    errorDiv.textContent = message;
    errorDiv.classList.add('text-danger');
};