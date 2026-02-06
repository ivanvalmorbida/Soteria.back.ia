# 🚀 Guia Rápido - Sistema de Cadastro

## 📁 Estrutura do Projeto

```
SistemaCadastro/
├── 📄 README.md                          # Documentação completa
├── 📄 .gitignore                         # Arquivos ignorados pelo Git
├── 📄 SistemaCadastro.sln                # Solução Visual Studio
├── 📄 api-integration.js                 # Integração JavaScript com API
│
└── SistemaCadastro.API/                  # Projeto da API
    ├── 📄 Program.cs                     # Configuração e startup
    ├── 📄 appsettings.json               # Configurações da aplicação
    ├── 📄 SistemaCadastro.API.csproj     # Arquivo de projeto .NET
    │
    ├── Configuration/                    # Configurações
    │   └── DatabaseConfig.cs
    │
    ├── Controllers/                      # Endpoints da API
    │   ├── PessoaFisicaController.cs
    │   ├── PessoaJuridicaController.cs
    │   ├── PessoaController.cs
    │   └── AuxiliaryControllers.cs
    │
    ├── DTOs/                             # Data Transfer Objects
    │   ├── PessoaFisicaDto.cs
    │   └── PessoaJuridicaDto.cs
    │
    ├── Models/                           # Modelos de domínio
    │   ├── Pessoa.cs
    │   ├── PessoaFisica.cs
    │   ├── PessoaJuridica.cs
    │   ├── PessoaEmail.cs
    │   ├── PessoaFone.cs
    │   └── AuxiliaryModels.cs
    │
    ├── Repositories/                     # Acesso a dados
    │   ├── Interfaces.cs
    │   ├── PessoaRepository.cs
    │   └── AuxiliaryRepositories.cs
    │
    └── Services/                         # Lógica de negócio
        ├── Interfaces.cs
        ├── PessoaFisicaService.cs
        ├── PessoaJuridicaService.cs
        └── PessoaService.cs
```

## ⚡ Início Rápido

### 1. Pré-requisitos
- .NET 8.0 SDK
- MySQL Server 8.0+
- Editor de código (VS Code, Visual Studio ou Rider)

### 2. Configurar Banco de Dados
```bash
# Execute o script SQL fornecido no MySQL
mysql -u root -p < script.sql
```

### 3. Configurar Connection String
Edite `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=sistema_cadastro;Uid=root;Pwd=sua_senha;"
  }
}
```

### 4. Executar a API
```bash
cd SistemaCadastro/SistemaCadastro.API
dotnet restore
dotnet run
```

Acesse: `https://localhost:5001/swagger`

## 🎯 Endpoints Principais

### Pessoa Física
- `GET /api/pessoafisica` - Listar todas
- `GET /api/pessoafisica/{id}` - Buscar por ID
- `POST /api/pessoafisica` - Criar nova
- `PUT /api/pessoafisica/{id}` - Atualizar

### Pessoa Jurídica
- `GET /api/pessoajuridica` - Listar todas
- `GET /api/pessoajuridica/{id}` - Buscar por ID
- `POST /api/pessoajuridica` - Criar nova
- `PUT /api/pessoajuridica/{id}` - Atualizar

### Consulta Geral
- `GET /api/pessoa` - Listar todas as pessoas
- `GET /api/pessoa/search?termo=x` - Pesquisar
- `DELETE /api/pessoa/{id}` - Excluir

### Dados Auxiliares
- `GET /api/estado` - Listar estados
- `GET /api/cidade` - Listar cidades
- `GET /api/cidade/estado/{id}` - Cidades por estado
- `GET /api/cep/{cep}` - Buscar CEP

## 💻 Integração Frontend

### Incluir o arquivo JavaScript
```html
<script src="api-integration.js"></script>
```

### Usar as funções
```javascript
// Criar pessoa física
const data = {
    nome: "João Silva",
    cpf: "12345678900",
    email: "joao@email.com"
};

const resultado = await window.API.createPessoaFisica(data);

// Listar pessoas
const pessoas = await window.API.getPessoas();

// Buscar por termo
const busca = await window.API.searchPessoas("João");
```

## 🏗️ Arquitetura em Camadas

```
┌─────────────────────────────────────────────┐
│          Controllers (API Layer)             │
│  - PessoaFisicaController                   │
│  - PessoaJuridicaController                 │
│  - PessoaController                         │
└─────────────────┬───────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────────┐
│         Services (Business Logic)            │
│  - PessoaFisicaService                      │
│  - PessoaJuridicaService                    │
│  - PessoaService                            │
└─────────────────┬───────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────────┐
│       Repositories (Data Access)             │
│  - PessoaRepository                         │
│  - PessoaFisicaRepository                   │
│  - PessoaJuridicaRepository                 │
└─────────────────┬───────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────────┐
│              MySQL Database                  │
│  - tb_pessoa                                │
│  - tb_pessoa_fisica                         │
│  - tb_pessoa_juridica                       │
│  - tb_pessoa_email                          │
│  - tb_pessoa_fone                           │
└─────────────────────────────────────────────┘
```

## 🔑 Conceitos Importantes

### DTOs (Data Transfer Objects)
- `PessoaFisicaCreateDto` - Para criar
- `PessoaFisicaUpdateDto` - Para atualizar
- `PessoaFisicaDto` - Para retornar dados

### Dependency Injection
Todos os serviços e repositories são registrados no `Program.cs` e injetados via construtor.

### Dapper
Usado como micro-ORM para queries SQL eficientes sem a complexidade do Entity Framework.

### Async/Await
Todas as operações são assíncronas para melhor performance.

## 🧪 Testando a API

### Com cURL
```bash
# Listar pessoas físicas
curl -X GET "https://localhost:5001/api/pessoafisica" \
  -H "accept: application/json"

# Criar pessoa física
curl -X POST "https://localhost:5001/api/pessoafisica" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "cpf": "12345678900"
  }'
```

### Com Postman
1. Importe a URL base: `https://localhost:5001/api`
2. Crie requisições para cada endpoint
3. Configure headers: `Content-Type: application/json`

## 📊 Exemplo de Payload Completo

### Pessoa Física
```json
{
  "nome": "João Silva Santos",
  "cpf": "12345678900",
  "identidade": "123456789",
  "orgaoIdentidade": "SSP",
  "ufIdentidade": 25,
  "nascimento": "1990-01-15T00:00:00",
  "sexo": "M",
  "estadoCivil": 1,
  "nacionalidade": 1,
  "profissao": 1,
  "ctps": "12345678",
  "pis": "12345678901",
  "cidadeNasc": 1,
  "ufNasc": 24,
  "cep": "89200000",
  "estado": 24,
  "cidade": 1,
  "bairro": "Centro",
  "endereco": "Rua das Flores",
  "numero": "123",
  "complemento": "Apto 101",
  "telefones": [
    {
      "valor": "(47) 99999-9999",
      "descricao": "Celular"
    },
    {
      "valor": "(47) 3333-3333",
      "descricao": "Residencial"
    }
  ],
  "emails": [
    {
      "valor": "joao@email.com",
      "descricao": "Pessoal"
    }
  ],
  "obs": "Cliente VIP desde 2020"
}
```

### Pessoa Jurídica
```json
{
  "razaoSocial": "Tech Solutions Ltda",
  "nome": "Tech Solutions",
  "cnpj": "12345678000190",
  "inscricaoEstadual": "123456789",
  "atividade": 1,
  "homepage": "https://techsolutions.com.br",
  "representante": "João Silva",
  "cep": "89200000",
  "estado": 24,
  "cidade": 1,
  "bairro": "Centro",
  "endereco": "Rua Comercial",
  "numero": "456",
  "complemento": "Sala 10",
  "telefones": [
    {
      "valor": "(47) 3333-3333",
      "descricao": "Comercial"
    }
  ],
  "emails": [
    {
      "valor": "contato@techsolutions.com.br",
      "descricao": "Comercial"
    }
  ],
  "obs": "Cliente corporativo"
}
```

## 🛠️ Comandos Úteis

```bash
# Restaurar dependências
dotnet restore

# Compilar
dotnet build

# Executar
dotnet run

# Executar em watch mode (recarrega ao salvar)
dotnet watch run

# Publicar para produção
dotnet publish -c Release -o ./publish

# Limpar build
dotnet clean
```

## 🔐 Segurança (A Implementar)

- [ ] JWT Authentication
- [ ] HTTPS obrigatório em produção
- [ ] Rate limiting
- [ ] Input validation
- [ ] SQL injection protection (já implementado com Dapper)
- [ ] CORS restrito em produção

## 📝 Próximos Passos

1. Implementar autenticação JWT
2. Adicionar paginação nas listagens
3. Implementar filtros avançados
4. Adicionar testes unitários
5. Configurar CI/CD
6. Dockerizar a aplicação
7. Adicionar logging estruturado
8. Implementar cache

## 💡 Dicas

- Use o Swagger UI para explorar a API
- Mantenha as connection strings seguras
- Use variáveis de ambiente em produção
- Implemente logs para debug
- Faça backup regular do banco de dados

## 📞 Suporte

Para dúvidas ou problemas:
1. Verifique o README.md completo
2. Consulte a documentação do Swagger
3. Revise os logs da aplicação
4. Abra uma issue no repositório

---

**Desenvolvido com .NET 8.0 e ❤️**
