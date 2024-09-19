using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using apiTenis.Helpers;
using apiTenis.Models;

namespace apiTenis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost("Crear")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return await ConstruirToken(model);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo model)
        {
            var resultado = await _signInManager.PasswordSignInAsync(model.Email,
                model.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(model);
            }
            else
            {
                return BadRequest("Invalid login attempt");
            }
        }

        [HttpPost("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserToken>> Renovar()
        {
            var userName = HttpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("El nombre de usuario no está presente.");
            }
            var userInfo = new UserInfo
            {
                Email = userName
            };

            return await ConstruirToken(userInfo);
        }

        private async Task<UserToken> ConstruirToken(UserInfo userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email),
            };

            var identityUser = await _userManager.FindByEmailAsync(userInfo.Email);

            if (identityUser == null || string.IsNullOrEmpty(identityUser.Id))
            {
                throw new Exception("El usuario no existe o su ID es inválido.");
            }

            claims.Add(new Claim(ClaimTypes.NameIdentifier, identityUser.Id));

            var claimsDB = await _userManager.GetClaimsAsync(identityUser);

            claims.AddRange(claimsDB);

            var jwtKey = _configuration["JWT:key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("La clave JWT no está configurada correctamente.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };

        }

        [HttpGet("Usuarios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<UsuarioDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Users.AsQueryable();
            queryable = queryable.OrderBy(x => x.Email);
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<UsuarioDTO>>(entidades);
        }

        [HttpGet("Roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<string>>> GetRoles()
        {
            var roles = await context.Roles.Select(x => x.Name).ToListAsync();

            if (roles == null || roles.Count == 0)
                return NotFound("No se encontraron roles.");

            return Ok(roles);
        }

        [HttpPost("AsignarRol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> AsignarRol(EditarRolDTO editarRolDTO)
        {
            var user = await _userManager.FindByIdAsync(editarRolDTO.UsuarioId);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.NombreRol));
            return NoContent();
        }

        [HttpPost("RemoveRol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> RemoverRol(EditarRolDTO editarRolDTO)
        {
            var user = await _userManager.FindByIdAsync(editarRolDTO.UsuarioId);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.NombreRol));
            return NoContent();
        }

        [HttpPost("HacerAdmin")]
        //Comentado para poder crear facilmente administradores
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await _userManager.FindByEmailAsync(editarAdminDTO.Email);
            if (usuario == null) return NotFound($"No se encontró un usuario con el email {editarAdminDTO.Email}.");
            await _userManager.AddClaimAsync(usuario, new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin"));
            return NoContent();
        }
        [HttpPost("RemoverAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await _userManager.FindByEmailAsync(editarAdminDTO.Email);
            if (usuario == null) return NotFound($"No se encontró un usuario con el email {editarAdminDTO.Email}.");
            await _userManager.RemoveClaimAsync(usuario, new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin"));
            return NoContent();
        }
    }
}
