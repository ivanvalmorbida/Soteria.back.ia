# Sistema de Cadastro - Backend .NET C#

API REST completa para gerenciamento de cadastros de pessoas físicas e jurídicas.

## 🚀 Tecnologias

- **.NET 8.0** - Framework principal
- **ASP.NET Core Web API** - Framework web
- **Dapper** - Micro ORM para acesso a dados
- **MySQL** - Banco de dados
- **FluentValidation** - Validação de modelos
- **Swagger/OpenAPI** - Documentação da API

## 📋 Pré-requisitos

- .NET 8.0 SDK
- MySQL Server 8.0+
- IDE (Visual Studio, VS Code ou Rider)

## 🔧 Configuração

### 1. Clone o repositório

```bash
git clone <seu-repositorio>
cd SistemaCadastro
```

### 2. Configure o banco de dados

Execute o script SQL fornecido para criar as tabelas no MySQL.

### 3. Configure a string de conexão

Edite o arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=sistema_cadastro;Uid=root;Pwd=sua_senha;"
  }
}
```

### 4. Restaure as dependências

```bash
cd SistemaCadastro.API
dotnet restore
```

### 5. Execute a aplicação

```bash
dotnet run
```

A API estará disponível em:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

## 📚 Endpoints da API

### Pessoa Física

#### Listar todas as pessoas físicas
```http
GET /api/pessoafisica
```

#### Buscar pessoa física por código
```http
GET /api/pessoafisica/{codigo}
```

#### Criar nova pessoa física
```http
POST /api/pessoafisica
Content-Type: application/json

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
    }
  ],
  "emails": [
    {
      "valor": "joao@email.com",
      "descricao": "Pessoal"
    }
  ],
  "obs": "Cliente VIP"
}
```

#### Atualizar pessoa física
```http
PUT /api/pessoafisica/{codigo}
Content-Type: application/json

{
  "codigo": 1,
  "nome": "João Silva Santos",
  // ... demais campos
}
```

### Pessoa Jurídica

#### Listar todas as pessoas jurídicas
```http
GET /api/pessoajuridica
```

#### Buscar pessoa jurídica por código
```http
GET /api/pessoajuridica/{codigo}
```

#### Criar nova pessoa jurídica
```http
POST /api/pessoajuridica
Content-Type: application/json

{
  "razaoSocial": "Tech Solutions Ltda",
  "nome": "Tech Solutions",
  "cnpj": "12345678000190",
  "inscricaoEstadual": "123456789",
  "atividade": 1,
  "homepage": "https://techsolutions.com.br",
  "representante": 5,
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

#### Atualizar pessoa jurídica
```http
PUT /api/pessoajuridica/{codigo}
Content-Type: application/json

{
  "codigo": 1,
  "razaoSocial": "Tech Solutions Ltda",
  // ... demais campos
}
```

### Consultas Gerais

#### Listar todas as pessoas
```http
GET /api/pessoa
```

#### Buscar pessoa por código
```http
GET /api/pessoa/{codigo}
```

#### Pesquisar pessoas
```http
GET /api/pessoa/search?termo=joão
```

#### Excluir pessoa
```http
DELETE /api/pessoa/{codigo}
```

### Dados Auxiliares

#### Listar estados
```http
GET /api/estado
```

#### Buscar estado por código
```http
GET /api/estado/{codigo}
```

#### Listar cidades
```http
GET /api/cidade
```

#### Listar cidades por estado
```http
GET /api/cidade/estado/{estadoId}
```

#### Buscar CEP
```http
GET /api/cep/{cep}
```

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas:

```
SistemaCadastro.API/
├── Configuration/          # Configurações da aplicação
├── Controllers/            # Endpoints da API
├── DTOs/                   # Data Transfer Objects
├── Models/                 # Modelos de domínio
├── Repositories/           # Camada de acesso a dados
├── Services/               # Lógica de negócio
└── Program.cs              # Configuração e startup
```

### Camadas

- **Controllers**: Recebem requisições HTTP e retornam respostas
- **Services**: Contêm a lógica de negócio
- **Repositories**: Realizam operações no banco de dados
- **DTOs**: Objetos para transferência de dados entre camadas
- **Models**: Representam as entidades do banco de dados

## 🔒 CORS

A API está configurada para aceitar requisições de qualquer origem em desenvolvimento. Para produção, configure adequadamente no `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production",
        builder =>
        {
            builder.WithOrigins("https://seudominio.com")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
```

## 🧪 Testando a API

### Usando Swagger UI

1. Acesse `https://localhost:5001/swagger`
2. Expanda um endpoint
3. Clique em "Try it out"
4. Preencha os parâmetros
5. Clique em "Execute"

### Usando cURL

```bash
# Criar pessoa física
curl -X POST "https://localhost:5001/api/pessoafisica" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "cpf": "12345678900",
    "email": "joao@email.com"
  }'

# Listar pessoas físicas
curl -X GET "https://localhost:5001/api/pessoafisica"

# Buscar por código
curl -X GET "https://localhost:5001/api/pessoafisica/1"
```

## 📦 Build e Deploy

### Build

```bash
dotnet build --configuration Release
```

### Publicar

```bash
dotnet publish --configuration Release --output ./publish
```

### Docker (opcional)

Crie um `Dockerfile`:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SistemaCadastro.API/SistemaCadastro.API.csproj", "SistemaCadastro.API/"]
RUN dotnet restore "SistemaCadastro.API/SistemaCadastro.API.csproj"
COPY . .
WORKDIR "/src/SistemaCadastro.API"
RUN dotnet build "SistemaCadastro.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SistemaCadastro.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SistemaCadastro.API.dll"]
```

Build e execute:

```bash
docker build -t sistema-cadastro-api .
docker run -p 5000:80 sistema-cadastro-api
```

## 🛠️ Melhorias Futuras

- [ ] Autenticação JWT
- [ ] Autorização baseada em roles
- [ ] Paginação nas listagens
- [ ] Cache com Redis
- [ ] Logs estruturados com Serilog
- [ ] Testes unitários e de integração
- [ ] Health checks
- [ ] Rate limiting
- [ ] Versionamento da API
- [ ] API Gateway

## 📄 Licença

Este projeto está sob a licença MIT.

## 👨‍💻 Autor

Desenvolvido para demonstração de arquitetura .NET Core.

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📞 Suporte

Para suporte, abra uma issue no repositório ou entre em contato através do email.
