function confirmarDelete(id) {
    const meta = document.querySelector('meta[name="csrf-token"]');
    if (!meta) {
        console.error("Meta CSRF token not found!");
        Swal.fire('Erro!', 'Token CSRF não encontrado na página.', 'error');
        return;
    }
    const token = meta.getAttribute('content');

    Swal.fire({
        title: 'Deletar Produto',
        text: "Essa ação não pode ser desfeita! Deseja prosseguir com ela?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sim, deletar!',
        cancelButtonText: 'Não, Cancelar',
        reverseButtons: true,
        showLoaderOnConfirm: true, // ativa o spinner no botão
        preConfirm: () => {
            // Retorna uma Promise que o SweetAlert vai aguardar
            return $.ajax({
                url: '/produto/delete',
                type: 'DELETE',
                data: { id: id },
                headers: {
                    'RequestVerificationToken': token
                }
            }).then((data) => {
                if (data.success) {
                    return data; // resolve
                } else {
                    throw new Error(data.message || 'Erro ao deletar.');
                }
            }).catch((error) => {
                Swal.showValidationMessage(`Erro: ${error.message}`);
                throw error; // para manter o modal aberto
            });
        },
        allowOutsideClick: () => !Swal.isLoading() // impede clique fora enquanto carrega
    }).then((result) => {
        if (result.isConfirmed && result.value) {
            Swal.fire('Deletado!', result.value.message, 'success').then(() => {
                location.reload();
            });
        }
    });
};

function visualizarProduto(id) {
    $.get('/produto/visualizar', { id: id })
        .done(function (html) {
            $('#modalContent').html(html);
            var modal = new bootstrap.Modal(document.getElementById('modalProduto'));
            modal.show();
        })
        .fail(function () {
            Swal.fire('Erro!', 'Não foi possível carregar os dados do produto.', 'error');
        });
};