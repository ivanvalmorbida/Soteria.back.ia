# Sistema de Cadastro - Backend .NET

API REST para gerenciamento de cadastros de pessoas físicas e jurídicas, com autenticação JWT e autorização baseada em roles.

## 🚀 Tecnologias

- **.NET 10** — framework principal
- **ASP.NET Core Web API** — framework web
- **Dapper** — micro ORM para acesso a dados
- **MySQL** — banco de dados
- **JWT Bearer** — autenticação (HMAC-SHA256, expiração de 8 horas)
- **FluentValidation** — validação de DTOs
- **Swagger / OpenAPI** — documentação interativa

## 📋 Pré-requisitos

- .NET 10 SDK
- MySQL Server 8.0+
- IDE de sua preferência (Visual Studio, VS Code, Rider)

## 🔧 Configuração

### 1. Clonar o repositório

```bash
git clone <seu-repositorio>
cd Soteria.back.ia
```

### 2. Configurar a string de conexão e JWT

Edite `SistemaCadastro.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<host>;Database=soteriasoft;Uid=<user>;Pwd=<senha>;"
  },
  "JwtSettings": {
    "Secret": "ChaveSecretaComPeloMenos32Caracteres",
    "ExpirationHours": 8,
    "Issuer": "SistemaCadastro",
    "Audience": "SistemaCadastroAPI"
  }
}
```

### 3. Restaurar dependências e executar

```bash
cd SistemaCadastro.API
dotnet restore
dotnet run            # ou: dotnet watch run (hot reload)
```

A API ficará disponível em:

- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI (apenas em Development): `https://localhost:5001/swagger`

## 🔐 Autenticação e Autorização

A API usa JWT Bearer. Faça login em `POST /api/auth/login` e envie o token nas demais requisições no header:

```
Authorization: Bearer <token>
```

### Roles

Roles são inteiros armazenados no claim `ClaimTypes.Role` do JWT:

| Código | Role           |
| -----: | -------------- |
| `1`    | Administrador  |
| `2`    | Usuário        |
| `3`    | Convidado      |

### Políticas de autorização

Definidas em `Program.cs`:

- `ApenasAdministrador` — somente role `1`
- `UsuarioOuSuperior` — roles `1` e `2`
- `QualquerAutenticado` — qualquer usuário autenticado

Há também o atributo `[AutorizarTipoUsuario(1, 2)]` para autorização granular em controllers/actions.

## 📚 Endpoints

### Autenticação (`/api/auth`)

| Método | Rota                    | Auth        | Descrição                          |
| -----: | ----------------------- | ----------- | ---------------------------------- |
| POST   | `/api/auth/login`       | Anônimo     | Autentica e retorna JWT            |
| POST   | `/api/auth/registrar`   | Anônimo     | Cria novo usuário                  |
| POST   | `/api/auth/alterar-senha` | Autenticado | Altera senha do usuário logado     |
| GET    | `/api/auth/me`          | Autenticado | Dados do usuário logado            |
| GET    | `/api/auth/validar-token` | Autenticado | Verifica se o token continua válido |

#### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "usuario": "admin",
  "senha": "senha123"
}
```

Resposta:

```json
{
  "success": true,
  "message": "Login realizado com sucesso",
  "token": "eyJhbGciOi...",
  "usuario": {
    "codigo": 1,
    "usuario": "admin",
    "tipo": 1,
    "tipoDescricao": "Administrador",
    "pessoa": 10,
    "nomePessoa": "Fulano"
  }
}
```

### Pessoa Física (`/api/pessoafisica`)

| Método | Rota                              | Política             |
| -----: | --------------------------------- | -------------------- |
| GET    | `/api/pessoafisica`               | `QualquerAutenticado` |
| GET    | `/api/pessoafisica/{codigo}`      | `QualquerAutenticado` |
| GET    | `/api/pessoafisica/nome/{nome}`   | `QualquerAutenticado` |
| POST   | `/api/pessoafisica`               | `UsuarioOuSuperior`  |
| PUT    | `/api/pessoafisica/{codigo}`      | `UsuarioOuSuperior`  |

#### Criar pessoa física

```http
POST /api/pessoafisica
Authorization: Bearer <token>
Content-Type: application/json

{
  "nome": "João Silva Santos",
  "cpf": "12345678900",
  "nascimento": "1990-01-15T00:00:00",
  "sexo": "M",
  "estadoCivil": 1,
  "nacionalidade": 1,
  "profissao": 1,
  "cidadeNasc": 1,
  "ufNasc": 24,
  "conjuge": null,
  "cep": "89200000",
  "estado": 24,
  "cidade": 1,
  "bairro": 5,
  "endereco": 12,
  "numero": "123",
  "complemento": "Apto 101",
  "telefones": [
    { "telefone": "(47) 99999-9999", "tipo": 1, "descricao": "Celular" }
  ],
  "enderecosEletronicos": [
    { "endereco": "joao@email.com", "tipo": 1, "descricao": "Pessoal" }
  ],
  "obs": "Cliente VIP"
}
```

> `bairro` e `endereco` são FKs (`int?`). Caso queira pesquisar/criar pelo nome, use `GET /api/bairro/nome/{nome}` e `GET /api/endereco/nome/{nome}` antes de informar o ID.

### Pessoa Jurídica (`/api/pessoajuridica`)

| Método | Rota                            | Política             |
| -----: | ------------------------------- | -------------------- |
| GET    | `/api/pessoajuridica`           | `QualquerAutenticado` |
| GET    | `/api/pessoajuridica/{codigo}`  | `QualquerAutenticado` |
| POST   | `/api/pessoajuridica`           | `UsuarioOuSuperior`  |
| PUT    | `/api/pessoajuridica/{codigo}`  | `UsuarioOuSuperior`  |

#### Criar pessoa jurídica

```http
POST /api/pessoajuridica
Authorization: Bearer <token>
Content-Type: application/json

{
  "razaoSocial": "Tech Solutions Ltda",
  "nome": "Tech Solutions",
  "cnpj": "12345678000190",
  "inscricaoEstadual": "123456789",
  "atividade": 1,
  "representante": 5,
  "cep": "89200000",
  "estado": 24,
  "cidade": 1,
  "bairro": 5,
  "endereco": 12,
  "numero": "456",
  "complemento": "Sala 10",
  "telefones": [
    { "telefone": "(47) 3333-3333", "tipo": 5, "descricao": "Comercial" }
  ],
  "enderecosEletronicos": [
    { "endereco": "contato@techsolutions.com.br", "tipo": 1, "descricao": "Comercial" }
  ],
  "obs": "Cliente corporativo"
}
```

> O CNPJ aceita formato alfanumérico (12 primeiros caracteres podem ser letras, com dígitos verificadores calculados via Módulo 11 da SERPRO — ver `Utilities/CnpjAlfanumericoValidator.cs`).

### Pessoa (operações comuns) (`/api/pessoa`)

| Método | Rota                          | Política              |
| -----: | ----------------------------- | --------------------- |
| GET    | `/api/pessoa`                 | `QualquerAutenticado` |
| GET    | `/api/pessoa/{codigo}`        | `QualquerAutenticado` |
| GET    | `/api/pessoa/search?termo=`   | `QualquerAutenticado` |
| DELETE | `/api/pessoa/{codigo}`        | `ApenasAdministrador` |

### Dados auxiliares (somente leitura)

| Rota                                            | Descrição                                  |
| ----------------------------------------------- | ------------------------------------------ |
| `GET /api/estado`                               | Lista estados                              |
| `GET /api/estado/{codigo}`                      | Estado por código                          |
| `GET /api/cidade`                               | Lista cidades                              |
| `GET /api/cidade/{codigo}`                      | Cidade por código                          |
| `GET /api/cidade/estado/{estadoId}`             | Cidades por estado                         |
| `GET /api/bairro/nome/{nome}`                   | Busca bairros por nome                     |
| `GET /api/endereco/nome/{nome}`                 | Busca endereços (logradouros) por nome     |
| `GET /api/cep/{cep}`                            | Consulta CEP                               |
| `GET /api/cbo`                                  | Lista CBOs (profissões)                    |
| `GET /api/cbo/{codigo}`                         | CBO por código                             |
| `GET /api/cbo/descricao/{descricao}`            | Busca CBO por descrição                    |
| `GET /api/nacionalidade`                        | Lista nacionalidades                       |
| `GET /api/nacionalidade/{codigo}`               | Nacionalidade por código                   |
| `GET /api/atividadeeconomica`                   | Lista atividades econômicas                |
| `GET /api/atividadeeconomica/{codigo}`          | Atividade por código                       |
| `GET /api/atividadeeconomica/setor/{setor}`     | Atividades por setor                       |
| `GET /api/atividadeeconomica/descricao/{desc}`  | Busca atividade por descrição              |
| `GET /api/atividadeeconomicasubsetor`           | Lista subsetores                           |
| `GET /api/atividadeeconomicasubsetor/{codigo}`  | Subsetor por código                        |
| `GET /api/estadocivil`                          | Lista estados civis                        |
| `GET /api/estadocivil/{codigo}`                 | Estado civil por código                    |
| `GET /api/tipotelefone`                         | Lista tipos de telefone                    |
| `GET /api/tipoenderecoeletronico`               | Lista tipos de endereço eletrônico         |

## 🏗️ Arquitetura

```
SistemaCadastro.API/
├── Configuration/        # JwtSettings, DatabaseConfig
├── Controllers/          # Endpoints HTTP
├── DTOs/                 # Contratos de entrada/saída
├── Models/               # Entidades de domínio
├── Repositories/         # Acesso a dados (Dapper + MySQL)
├── Services/             # Regras de negócio
├── Utilities/            # Helpers (ex.: validador de CNPJ alfanumérico)
├── Validators/           # Validações FluentValidation
└── Program.cs            # Bootstrap, DI, JWT, políticas
```

Fluxo: `Controllers` → `Services` → `Repositories` → MySQL.

### Modelagem

`Pessoa` (`tb_pessoa`) é a entidade base, com campo `tipo`:

- `'F'` — Física, complementada por `tb_pessoa_fisica`
- `'J'` — Jurídica, complementada por `tb_pessoa_juridica`

Tabelas auxiliares:

- `tb_bairro` e `tb_endereco` — resolvidas via `GetOrCreateAsync` (cria se não existir)
- `PessoaTelefone` e `PessoaEnderecoEletronico` — substituídos integralmente em update (delete + insert)

### Senhas

Hash SHA-256 (sem salt). `AuthService` cuida de login, registro, geração de token e troca de senha.

## 🧪 Testando a API

### Swagger UI

1. Inicie a aplicação (`dotnet run`).
2. Acesse `https://localhost:5001/swagger`.
3. Faça login via `POST /api/auth/login`.
4. Clique em **Authorize** e cole `Bearer <token>` (ou apenas o token).
5. Teste os demais endpoints.

### cURL

```bash
# Login
TOKEN=$(curl -sX POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"usuario":"admin","senha":"senha123"}' | jq -r .token)

# Listar pessoas físicas
curl -X GET https://localhost:5001/api/pessoafisica \
  -H "Authorization: Bearer $TOKEN"

# Buscar por código
curl -X GET https://localhost:5001/api/pessoafisica/1 \
  -H "Authorization: Bearer $TOKEN"
```

## 📦 Build e Deploy

```bash
dotnet build -c Release
dotnet publish -c Release -o ./publish
```

### Docker (exemplo)

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["SistemaCadastro.API/SistemaCadastro.API.csproj", "SistemaCadastro.API/"]
RUN dotnet restore "SistemaCadastro.API/SistemaCadastro.API.csproj"
COPY . .
WORKDIR "/src/SistemaCadastro.API"
RUN dotnet publish "SistemaCadastro.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SistemaCadastro.API.dll"]
```

```bash
docker build -t sistema-cadastro-api .
docker run -p 5000:80 sistema-cadastro-api
```

## 🔒 CORS

A API usa a política `AllowAll` (qualquer origem/método/header). Para produção, restrinja em `Program.cs`:

```csharp
options.AddPolicy("Production", builder =>
    builder.WithOrigins("https://seudominio.com")
           .AllowAnyMethod()
           .AllowAnyHeader());
```

## 🛠️ Melhorias Futuras

- [ ] Paginação nas listagens
- [ ] Cache (ex.: Redis) para dados auxiliares
- [ ] Logs estruturados (Serilog)
- [ ] Testes automatizados (unitários e de integração)
- [ ] Health checks
- [ ] Rate limiting
- [ ] Versionamento da API
- [ ] Hash de senha com salt (ex.: BCrypt/Argon2) substituindo SHA-256

## 📄 Licença

Este projeto está sob a licença MIT.

## 🤝 Contribuindo

1. Faça um fork do projeto.
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`).
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`).
4. Push para a branch (`git push origin feature/MinhaFeature`).
5. Abra um Pull Request.
