/**
 * API Service - Integração com Backend .NET
 * 
 * Configure a URL base da API no início do arquivo
 */

const API_BASE_URL = 'https://localhost:5001/api';

// Configuração de headers padrão
const getHeaders = () => ({
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    // Adicione aqui o token JWT quando implementar autenticação
    // 'Authorization': `Bearer ${getToken()}`
});

// Funções auxiliares
const handleResponse = async (response) => {
    if (!response.ok) {
        const error = await response.json().catch(() => ({ message: 'Erro desconhecido' }));
        throw new Error(error.message || `HTTP Error ${response.status}`);
    }
    return response.json().catch(() => null);
};

// ========== PESSOA FÍSICA ==========

/**
 * Lista todas as pessoas físicas
 */
async function getPessoasFisicas() {
    try {
        const response = await fetch(`${API_BASE_URL}/pessoafisica`, {
            method: 'GET',
            headers: getHeaders()
        });
        return await handleResponse(response);
    } catch (error) {
        console.error('Erro ao buscar pessoas físicas:', error);
        throw error;
    }
}

/**
 * Busca uma pessoa física por código
 */
async function getPessoaFisicaById(codigo) {
    try {
        const response = await fetch(`${API_BASE_URL}/pessoafisica/${codigo}`, {
            method: 'GET',
            headers: getHeaders()
        });
        return await handleResponse(response);
    } catch (error) {
        console.error(`Erro ao buscar pessoa física ${codigo}:`, error);
        throw error;
    }
}

/**
 * Cria uma nova pessoa física
 */
async function createPessoaFisica(data) {
    try {
        const response = await fetch(`${API_BASE_URL}/pessoafisica`, {
            method: 'POST',
            headers: getHeaders(),
            body: JSON.stringify(data)
        });
        return await handleResponse(response);
    } catch (error) {
        console.error('Erro ao criar pessoa física:', error);
        throw error;
    }
}

/**
 * Atualiza uma pessoa física
 */
async function updatePessoaFisica(codigo, data) {
    try {
        const response = await fetch(`${API_BASE_URL}/pessoafisica/${codigo}`, {
            method: 'PUT',
            headers: getHeaders(),
            body: JSON.stringify({ ...data, codigo })
        });
        return await handleResponse(response);
    } catch (error) {
        console.error(`Erro ao atualizar pessoa física ${codigo}:`, error);
        throw error;
    }
}

// ========== PESSOA JURÍDICA ==========

/**
 * Lista todas as pessoas jurídicas
 */
async function getPessoasJuridicas() {
    try {
        const response = await fetch(`${API_BASE_URL}/pessoajuridica`, {
            method: 'GET',
            headers: getHeaders()
        });
        return await handleResponse(response);
    } catch (error) {
        console.error('Erro ao buscar pessoas jurídicas:', error);
        throw error;
    }
}

/**
 * Busca uma pessoa jurídica por código
 */
async function getPessoaJuridicaById(codigo) {
    try {
        const response = await fetch(`${API_BASE_URL}/pessoajuridica/${codigo}`, {
            method: 'GET',
            headers: getHeaders()
        });
        return await handleResponse(response);
    } catch (error) {
        console.error(`Erro ao buscar pessoa jurídica ${codigo}:`, error);
        throw error;
    }
}

/**
 * Cria uma nova pessoa jurídica
 */
async function createPessoaJuridica(data) {
    try {
        const response = await fetch(`${API_BASE_URL}/pessoajuridica`, {
            method: 'POST',
            headers: getHeaders(),
            body: JSON.stringify(data)
        });
        return await handleResponse(response);
    } catch (error) {
        console.error('Erro ao criar pessoa jurídica:', error);
        throw error;
    }
}

/**
 * Atualiza uma pessoa jurídica
 */
async function updatePessoaJuridica(codigo, data) {
    try {
        const response = await fetch(`${API_BASE_URL}/pessoajuridica/${codigo}`, {
            method: 'PUT',
            headers: getHeaders(),
            body: JSON.stringify({ ...data, codigo })
        });
        return await handleResponse(response);
    } catch (error) {
        console.error(`Erro ao atualizar pessoa jurídica ${codigo}:`, error);
        throw error;
    }
}

// ========== PESSOA (CONSULTA GERAL) ==========

/**
 * Lista todas as pessoas
 */
async function getPessoas() {
    try {
        const response = await fetch(`${API_BASE_URL}/pessoa`, {
            method: 'GET',
            headers: getHeaders()
        });
        return await handleResponse(response);
    } catch (error) {
        console.error('Erro ao buscar pessoas:', error);
        throw error;
    }
}

/**
 * Busca pessoa por código
 */
async function getPessoaById(codigo) {
    try {
        const response = await fetch(`${API_BASE_URL}/pessoa/${codigo}`, {
            method: 'GET',
            headers: getHeaders()
        });
        return await handleResponse(response);
    } catch (error) {
        console.error(`Erro ao buscar pessoa ${codigo}:`, error);
        throw error;
    }
}

/**
 * Pesquisa pessoas por termo
 */
async function searchPessoas(termo) {
    try {
        const response = await fetch(`${API_BASE_URL}/pessoa/search?termo=${encodeURIComponent(termo)}`, {
            method: 'GET',
            headers: getHeaders()
        });
        return await handleResponse(response);
    } catch (error) {
        console.error('Erro ao pesquisar pessoas:', error);
        throw error;
    }
}

/**
 * Exclui uma pessoa
 */
async function deletePessoa(codigo) {
    try {
        const response = await fetch(`${API_BASE_URL}/pessoa/${codigo}`, {
            method: 'DELETE',
            headers: getHeaders()
        });
        return await handleResponse(response);
    } catch (error) {
        console.error(`Erro ao excluir pessoa ${codigo}:`, error);
        throw error;
    }
}

// ========== DADOS AUXILIARES ==========

/**
 * Lista todos os estados
 */
async function getEstados() {
    try {
        const response = await fetch(`${API_BASE_URL}/estado`, {
            method: 'GET',
            headers: getHeaders()
        });
        return await handleResponse(response);
    } catch (error) {
        console.error('Erro ao buscar estados:', error);
        throw error;
    }
}

/**
 * Lista todas as cidades
 */
async function getCidades() {
    try {
        const response = await fetch(`${API_BASE_URL}/cidade`, {
            method: 'GET',
            headers: getHeaders()
        });
        return await handleResponse(response);
    } catch (error) {
        console.error('Erro ao buscar cidades:', error);
        throw error;
    }
}

/**
 * Lista cidades por estado
 */
async function getCidadesByEstado(estadoId) {
    try {
        const response = await fetch(`${API_BASE_URL}/cidade/estado/${estadoId}`, {
            method: 'GET',
            headers: getHeaders()
        });
        return await handleResponse(response);
    } catch (error) {
        console.error(`Erro ao buscar cidades do estado ${estadoId}:`, error);
        throw error;
    }
}

/**
 * Busca informações de CEP
 */
async function getCep(cep) {
    try {
        const cepLimpo = cep.replace(/\D/g, '');
        const response = await fetch(`${API_BASE_URL}/cep/${cepLimpo}`, {
            method: 'GET',
            headers: getHeaders()
        });
        return await handleResponse(response);
    } catch (error) {
        console.error(`Erro ao buscar CEP ${cep}:`, error);
        throw error;
    }
}

// ========== EXEMPLO DE USO ==========

/**
 * Exemplo de como usar a API no frontend
 */
async function exemploDeUso() {
    try {
        // Criar pessoa física
        const novaPessoa = {
            nome: "João Silva Santos",
            cpf: "12345678900",
            sexo: "M",
            nascimento: "1990-01-15T00:00:00",
            cep: "89200000",
            estado: 24,
            cidade: 1,
            bairro: "Centro",
            endereco: "Rua das Flores",
            numero: "123",
            telefones: [
                { valor: "(47) 99999-9999", descricao: "Celular" }
            ],
            emails: [
                { valor: "joao@email.com", descricao: "Pessoal" }
            ]
        };

        const resultado = await createPessoaFisica(novaPessoa);
        console.log('Pessoa criada:', resultado);

        // Listar todas as pessoas físicas
        const pessoas = await getPessoasFisicas();
        console.log('Pessoas físicas:', pessoas);

        // Buscar por termo
        const busca = await searchPessoas("João");
        console.log('Resultado da busca:', busca);

    } catch (error) {
        console.error('Erro:', error.message);
    }
}

// ========== INTEGRAÇÃO COM O FORMULÁRIO HTML ==========

/**
 * Submete o formulário de pessoa física
 */
async function submitPessoaFisicaForm(event) {
    event.preventDefault();
    
    const form = event.target;
    const formData = new FormData(form);
    
    // Coletar telefones
    const telefones = [];
    const fones = formData.getAll('fone[]');
    const foneDescs = formData.getAll('fone_desc[]');
    
    for (let i = 0; i < fones.length; i++) {
        if (fones[i]) {
            telefones.push({
                valor: fones[i],
                descricao: foneDescs[i] || ''
            });
        }
    }
    
    // Coletar emails
    const emails = [];
    const emailVals = formData.getAll('email[]');
    const emailDescs = formData.getAll('email_desc[]');
    
    for (let i = 0; i < emailVals.length; i++) {
        if (emailVals[i]) {
            emails.push({
                valor: emailVals[i],
                descricao: emailDescs[i] || ''
            });
        }
    }
    
    // Montar objeto
    const data = {
        nome: formData.get('nome'),
        cpf: formData.get('cpf')?.replace(/\D/g, ''),
        identidade: formData.get('identidade'),
        orgaoIdentidade: formData.get('orgaoidentidade'),
        ufIdentidade: formData.get('ufidentidade') ? parseInt(formData.get('ufidentidade')) : null,
        nascimento: formData.get('nascimento') || null,
        sexo: formData.get('sexo') || null,
        estadoCivil: formData.get('estadocivil') ? parseInt(formData.get('estadocivil')) : null,
        nacionalidade: formData.get('nacionalidade') ? parseInt(formData.get('nacionalidade')) : null,
        profissao: formData.get('profissao') ? parseInt(formData.get('profissao')) : null,
        ctps: formData.get('ctps'),
        pis: formData.get('pis')?.replace(/\D/g, ''),
        cep: formData.get('cep')?.replace(/\D/g, ''),
        estado: formData.get('estado') ? parseInt(formData.get('estado')) : null,
        cidade: formData.get('cidade') ? parseInt(formData.get('cidade')) : null,
        bairro: formData.get('bairro'),
        endereco: formData.get('endereco'),
        numero: formData.get('numero'),
        complemento: formData.get('complemento'),
        telefones,
        emails,
        obs: formData.get('obs')
    };
    
    try {
        const resultado = await createPessoaFisica(data);
        showToast('Cadastro realizado com sucesso!', 'success');
        form.reset();
        return resultado;
    } catch (error) {
        showToast('Erro ao salvar cadastro: ' + error.message, 'error');
        throw error;
    }
}

/**
 * Carrega a lista de pessoas na tabela
 */
async function carregarTabelaPessoas() {
    try {
        const pessoas = await getPessoas();
        const tbody = document.querySelector('#tabelaConsulta tbody');
        
        if (!tbody) return;
        
        tbody.innerHTML = '';
        
        pessoas.forEach(pessoa => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${pessoa.codigo}</td>
                <td>${pessoa.nome}</td>
                <td>${pessoa.tipo === 'F' ? 'Física' : 'Jurídica'}</td>
                <td>${pessoa.cidadeNome || '-'} - ${pessoa.estadoNome || '-'}</td>
                <td>${new Date(pessoa.cadastro).toLocaleString('pt-BR')}</td>
                <td>
                    <div class="action-buttons">
                        <button class="btn-icon" onclick="editarPessoa(${pessoa.codigo})" title="Editar">✏️</button>
                        <button class="btn-icon" onclick="visualizarPessoa(${pessoa.codigo})" title="Visualizar">👁️</button>
                        <button class="btn-icon" onclick="confirmarExclusao(${pessoa.codigo})" title="Excluir">🗑️</button>
                    </div>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (error) {
        console.error('Erro ao carregar tabela:', error);
        showToast('Erro ao carregar dados', 'error');
    }
}

/**
 * Carrega estados no select
 */
async function carregarEstados() {
    try {
        const estados = await getEstados();
        const selects = document.querySelectorAll('select[name="estado"]');
        
        selects.forEach(select => {
            select.innerHTML = '<option value="">Selecione...</option>';
            estados.forEach(estado => {
                const option = document.createElement('option');
                option.value = estado.codigo;
                option.textContent = `${estado.sigla} - ${estado.nome}`;
                select.appendChild(option);
            });
        });
    } catch (error) {
        console.error('Erro ao carregar estados:', error);
    }
}

// Inicialização quando a página carregar
document.addEventListener('DOMContentLoaded', function() {
    // Carregar estados
    carregarEstados();
    
    // Se estiver na aba de consulta, carregar a tabela
    if (document.querySelector('#tabelaConsulta')) {
        carregarTabelaPessoas();
    }
    
    // Integrar formulários
    const formPF = document.getElementById('formPessoaFisica');
    if (formPF) {
        formPF.addEventListener('submit', submitPessoaFisicaForm);
    }
});

// Exportar funções para uso global
window.API = {
    // Pessoa Física
    getPessoasFisicas,
    getPessoaFisicaById,
    createPessoaFisica,
    updatePessoaFisica,
    
    // Pessoa Jurídica
    getPessoasJuridicas,
    getPessoaJuridicaById,
    createPessoaJuridica,
    updatePessoaJuridica,
    
    // Pessoa (Geral)
    getPessoas,
    getPessoaById,
    searchPessoas,
    deletePessoa,
    
    // Auxiliares
    getEstados,
    getCidades,
    getCidadesByEstado,
    getCep,
    
    // Utilidades
    carregarTabelaPessoas,
    carregarEstados
};
