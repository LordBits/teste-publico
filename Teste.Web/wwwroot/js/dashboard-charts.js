let chart;

function carregarDadosGrafico(agrupamento = 'mes') {
    fetch(`/home/obter-dados-grafico?agrupamento=${agrupamento}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json'
        },
        credentials: 'include'
    })
        .then(response => {
            if (!response.ok) throw new Error('Erro ao carregar dados do gráfico');
            return response.json();
        })
        .then(data => {
            atualizarGrafico(data, agrupamento);
        })
        .catch(error => {
            console.error(error);
            alert('Erro ao buscar dados do gráfico!');
        });
}

function atualizarGrafico(data, tipo) {
    if (chart) chart.destroy();

    const ctx = document.getElementById('graficoCadastro').getContext('2d');

    chart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: data.labels,
            datasets: [
                {
                    label: 'Clientes',
                    backgroundColor: '#0d6efd',
                    data: data.clientes
                },
                {
                    label: 'Produtos',
                    backgroundColor: '#198754',
                    data: data.produtos
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: `Cadastros por ${tipo === 'ano' ? 'Ano' : 'Mês'}`
                },
                legend: {
                    position: 'bottom'
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: { precision: 0 }
                }
            }
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    const tipoSelect = document.getElementById('tipoVisualizacao');

    carregarDadosGrafico(tipoSelect.value);

    tipoSelect.addEventListener('change', () => {
        carregarDadosGrafico(tipoSelect.value);
    });
});