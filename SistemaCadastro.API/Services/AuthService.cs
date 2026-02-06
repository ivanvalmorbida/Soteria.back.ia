using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SistemaCadastro.API.Configuration;
using SistemaCadastro.API.DTOs;
using SistemaCadastro.API.Repositories;

namespace SistemaCadastro.API.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<LoginResponseDto> RegistrarAsync(RegistrarUsuarioDto dto);
    Task<bool> AlterarSenhaAsync(int usuarioId, AlterarSenhaDto dto);
    Task<UsuarioDto?> GetUsuarioByIdAsync(int codigo);
    string GerarToken(int usuarioId, string usuario, int tipo);
}

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPessoaRepository _pessoaRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IPessoaRepository pessoaRepository,
        IOptions<JwtSettings> jwtSettings)
    {
        _usuarioRepository = usuarioRepository;
        _pessoaRepository = pessoaRepository;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        // Buscar usuário
        var usuario = await _usuarioRepository.GetByUsuarioAsync(loginDto.Usuario);
        
        if (usuario == null)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Usuário ou senha inválidos"
            };
        }

        // Verificar senha
        if (!VerificarSenha(loginDto.Senha, usuario.Senha ?? ""))
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Usuário ou senha inválidos"
            };
        }

        // Buscar nome da pessoa se existir
        string? nomePessoa = null;
        if (usuario.Pessoa.HasValue)
        {
            var pessoa = await _pessoaRepository.GetByIdAsync(usuario.Pessoa.Value);
            nomePessoa = pessoa?.Nome;
        }

        // Gerar token
        var token = GerarToken(usuario.Codigo, usuario.UsuarioLogin ?? "", usuario.Tipo ?? 0);

        return new LoginResponseDto
        {
            Success = true,
            Message = "Login realizado com sucesso",
            Token = token,
            Usuario = new UsuarioDto
            {
                Codigo = usuario.Codigo,
                Usuario = usuario.UsuarioLogin,
                Tipo = usuario.Tipo,
                Pessoa = usuario.Pessoa,
                NomePessoa = nomePessoa,
                Cadastro = usuario.Cadastro
            }
        };
    }

    public async Task<LoginResponseDto> RegistrarAsync(RegistrarUsuarioDto dto)
    {
        // Validar senhas
        if (dto.Senha != dto.ConfirmarSenha)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "As senhas não coincidem"
            };
        }

        // Verificar se usuário já existe
        var usuarioExistente = await _usuarioRepository.GetByUsuarioAsync(dto.Usuario);
        if (usuarioExistente != null)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Este usuário já está cadastrado"
            };
        }

        // Criar usuário
        var senhaHash = GerarHashSenha(dto.Senha);
        
        var usuario = new Models.Usuario
        {
            UsuarioLogin = dto.Usuario,
            Senha = senhaHash,
            Tipo = dto.Tipo,
            Pessoa = dto.Pessoa,
            Cadastro = DateTime.Now
        };

        var codigo = await _usuarioRepository.CreateAsync(usuario);

        // Gerar token
        var token = GerarToken(codigo, dto.Usuario, dto.Tipo);

        return new LoginResponseDto
        {
            Success = true,
            Message = "Usuário registrado com sucesso",
            Token = token,
            Usuario = new UsuarioDto
            {
                Codigo = codigo,
                Usuario = dto.Usuario,
                Tipo = dto.Tipo,
                Pessoa = dto.Pessoa,
                Cadastro = DateTime.Now
            }
        };
    }

    public async Task<bool> AlterarSenhaAsync(int usuarioId, AlterarSenhaDto dto)
    {
        // Validar senhas
        if (dto.NovaSenha != dto.ConfirmarNovaSenha)
        {
            return false;
        }

        // Buscar usuário
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
        if (usuario == null)
        {
            return false;
        }

        // Verificar senha atual
        if (!VerificarSenha(dto.SenhaAtual, usuario.Senha ?? ""))
        {
            return false;
        }

        // Atualizar senha
        var novaSenhaHash = GerarHashSenha(dto.NovaSenha);
        return await _usuarioRepository.AlterarSenhaAsync(usuarioId, novaSenhaHash);
    }

    public async Task<UsuarioDto?> GetUsuarioByIdAsync(int codigo)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(codigo);
        if (usuario == null)
            return null;

        string? nomePessoa = null;
        if (usuario.Pessoa.HasValue)
        {
            var pessoa = await _pessoaRepository.GetByIdAsync(usuario.Pessoa.Value);
            nomePessoa = pessoa?.Nome;
        }

        return new UsuarioDto
        {
            Codigo = usuario.Codigo,
            Usuario = usuario.UsuarioLogin,
            Tipo = usuario.Tipo,
            Pessoa = usuario.Pessoa,
            NomePessoa = nomePessoa,
            Cadastro = usuario.Cadastro
        };
    }

    public string GerarToken(int usuarioId, string usuario, int tipo)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString()),
                new Claim(ClaimTypes.Name, usuario),
                new Claim(ClaimTypes.Role, tipo.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GerarHashSenha(string senha)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(senha);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private bool VerificarSenha(string senha, string senhaHash)
    {
        var hashSenhaInformada = GerarHashSenha(senha);
        return hashSenhaInformada == senhaHash;
    }
}
