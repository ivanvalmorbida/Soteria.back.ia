# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# From SistemaCadastro.API/
dotnet restore
dotnet build
dotnet run
dotnet watch run          # hot reload on save
dotnet publish -c Release -o ./publish
dotnet clean
```

The API runs at `http://localhost:5000` / `https://localhost:5001`. Swagger UI is at `https://localhost:5001/swagger` (development only).

There are no automated tests in this project.

## Configuration

Connection string and JWT settings live in `SistemaCadastro.API/appsettings.json`. The database is MySQL at `172.16.172.199`, database `soteriasoft`. JWT tokens expire in 8 hours and use HMAC-SHA256.

## Architecture

**Stack**: .NET 10, ASP.NET Core Web API, Dapper (micro-ORM), MySQL, FluentValidation, JWT Bearer auth.

**Layer flow**: `Controllers` → `Services` → `Repositories` → MySQL. All interfaces are declared in `Services/Interfaces.cs` and `Repositories/Interfaces.cs`. All DI registrations are in `Program.cs`.

### Data model

`Pessoa` (`tb_pessoa`) is the base entity for both person types, discriminated by `tipo = 'F'` (física) or `'J'` (jurídica). Each has a corresponding extension table joined by the `pessoa` FK:

- `tb_pessoa_fisica` — CPF, identity docs, birth info, marital status, profession
- `tb_pessoa_juridica` — CNPJ, razão social, economic activity, representative

`Bairro` and `Endereco` are shared lookup tables used via `GetOrCreateAsync` — they resolve a string name to an ID, inserting if not found.

`PessoaTelefone` and `PessoaEnderecoEletronico` are replaced wholesale on update: all records for a person are deleted, then re-inserted from the DTO list.

### Authorization

Roles are integers stored as JWT claims (`ClaimTypes.Role`): `1` = Administrador, `2` = Usuário, `3` = Convidado. Three authorization policies are defined in `Program.cs`: `ApenasAdministrador`, `UsuarioOuSuperior`, `QualquerAutenticado`. A custom `[AutorizarTipoUsuario(1, 2)]` attribute is also available for fine-grained controller/action authorization.

Passwords are hashed with SHA-256 (no salt). `AuthService` handles login, registration, token generation, and password change.

### Key utilities

`Utilities/CnpjAlfanumericoValidator.cs` — validates and formats alphanumeric CNPJs using the SERPRO Módulo 11 algorithm. Supports the new format where the first 12 characters can be alphanumeric (letters map to values 17–42 per SERPRO table) and the last 2 must be digits.

### Auxiliary data

`AuxiliaryControllers.cs` and `AuxiliaryRepositories.cs` serve read-only lookup endpoints: estados, cidades (filterable by estado), bairros, CEP lookup, CBO (occupations), nacionalidades, atividades econômicas, estado civil.
