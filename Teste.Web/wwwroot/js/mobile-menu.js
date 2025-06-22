document.addEventListener('DOMContentLoaded', function () {
    const toggleButton = document.getElementById('menuToggle');
    const sidebar = document.querySelector('.sidebar');

    const icon = toggleButton.querySelector('i');

    toggleButton.addEventListener('click', function () {
        sidebar.classList.toggle('show');
        if (sidebar.classList.contains('show')) {
            icon.classList.replace('bi-chevron-double-right', 'bi-chevron-double-left');
        } else {
            icon.classList.replace('bi-chevron-double-left', 'bi-chevron-double-right');
        }
    });
});