using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoursesOnline.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _config;

    public AuthController(UserManager<User> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        // Validación de modelo
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized(new { message = "Credenciales inválidas" });

        var valid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!valid)
            return Unauthorized(new { message = "Credenciales inválidas" });

        // Obtener roles del usuario
        var roles = await _userManager.GetRolesAsync(user);
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        // Agregar roles como claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddHours(24), // ← Aumentado a 24 horas
            claims: claims,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return Ok(new
        {
            tokenType = "Bearer",
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            expiresIn = 86400, // 24 horas en segundos
            user = new
            {
                id = user.Id,
                email = user.Email,
                userName = user.UserName,
                roles = roles
            }
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return BadRequest(new { message = "El email ya está registrado" });

        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                message = "Error al crear el usuario",
                errors = result.Errors.Select(e => e.Description)
            });
        }

        // Asignar rol por defecto
        await _userManager.AddToRoleAsync(user, "User");

        return Ok(new { message = "Usuario registrado exitosamente" });
    }
}