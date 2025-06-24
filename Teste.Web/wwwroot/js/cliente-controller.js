function confirmarDelete(id) {
    const meta = document.querySelector('meta[name="csrf-token"]');
    if (!meta) {
        console.error("Meta CSRF token not found!");
        Swal.fire('Erro!', 'Token CSRF não encontrado na página.', 'error');
        return;
    }
    const token = meta.getAttribute('content');

    Swal.fire({
        title: 'Deletar Cliente',
        text: "Essa ação não pode ser desfeita! Deseja prosseguir com ela?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sim, deletar!',
        cancelButtonText: 'Não, Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/cliente/delete',
                type: 'DELETE',
                data: { id: id },
                headers: {
                    'RequestVerificationToken': token
                },
                success: function (data) {
                    if (data.success) {
                        Swal.fire('Deletado!', data.message, 'success').then(() => {
                            location.reload();
                        });
                    } else {
                        Swal.fire('Erro!', data.message || 'Erro ao deletar.', 'error');
                    }
                },
                error: function () {
                    Swal.fire('Erro!', 'Não foi possível conectar ao servidor.', 'error');
                }
            });
        }
    });
};

function visualizarCliente(id) {
    $.get('/cliente/visualizar', { id: id })
        .done(function (html) {
            $('#modalContent').html(html);
            var modal = new bootstrap.Modal(document.getElementById('modalCliente'));
            modal.show();
        })
        .fail(function () {
            Swal.fire('Erro!', 'Não foi possível carregar os dados do cliente.', 'error');
        });
};