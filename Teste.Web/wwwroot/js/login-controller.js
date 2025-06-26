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

function showBackendError(message, tipo = "danger") {
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

    let icone = "";
    switch (tipo) {
        case "success":
            icone = "<i class='bi bi-check-circle-fill me-2'></i>";
            break;
        case "warning":
            icone = "<i class='bi bi-exclamation-triangle-fill me-2'></i>";
            break;
        case "info":
            icone = "<i class='bi bi-info-circle-fill me-2'></i>";
            break;
        case "danger":
        default:
            icone = "<i class='bi bi-x-circle-fill me-2'></i>";
            break;
    }

    errorDiv.innerHTML = `${icone}${message}`;
    errorDiv.className = 'alert alert-' + tipo + ' mt-3 d-flex align-items-center';
    errorDiv.classList.remove('d-none');
};