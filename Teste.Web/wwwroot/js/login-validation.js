document.getElementById('loginForm').addEventListener('submit', function(event) {
  event.preventDefault();

  // limpar erros anteriores
  clearErrors();

  const email = document.getElementById('email').value.trim();
  const password = document.getElementById('password').value.trim();

  let hasError = false;

  if (!email) {
    showError('email', 'Insira seu e-mail.');
    hasError = true;
  } else if (!validateEmail(email)) {
    showError('email', 'Insira um endereço de e-mail válido.');
    hasError = true;
  }

  if (!password) {
    showError('password', 'Insira sua senha.');
    hasError = true;
  }

  if (hasError) {
    return;
  }

  // tudo ok
  this.submit();
});

function validateEmail(email) {
  const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return re.test(email);
};

function showError(field, message) {
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
    errorDiv.textContent = '';
  });
};

window.onload = function() {
    document.getElementById("loginForm").reset();

    // Dá foco no campo de email
    document.getElementById("email").focus();
};