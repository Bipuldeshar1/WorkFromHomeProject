using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using WFH.Api.dto;
using WFH.Api.DTO;
using WFH.infrastructure.Repository;
using WorkFromHome.Application.Common.Repository;
using WorkFromHome.Domain.models;
using WorkFromHome.Infrastructure;

namespace WFH.Api.Controllers
{


    [Authorize(AuthenticationSchemes = "Okta,Custom", Policy = "ManagerEmployee")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        private readonly WorkFromHomeDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IToken _token;
        private readonly IEmailService _emailService;

        public LoginController(WorkFromHomeDbContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration,
            IEmployeeRepository employeeRepository, IToken token, IEmailService emailService)
        {
            this._context = context;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._configuration = configuration;
            this._employeeRepository = employeeRepository;
            this._token = token;
            this._emailService = emailService;
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Login(LoginDto login, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(login.Email);
                if (user == null || user.IsDeleted == true)
                {
                    return NotFound("user not found");
                }

                var emp = await _employeeRepository.GetSingleEmployeeByEmailAsync(user.Email!, cancellationToken);
                if (emp == null || emp.IsDeleted == true)
                {
                    return NotFound("user not found");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

                if (result.Succeeded)
                {
                    if (user.FirstLogin == true)
                    {
                        var redirectUrl = Url.Action(nameof(UpdatePassword), "Login");
                        return StatusCode(302);
                    }


                    var claims = new List<Claim>
                    {
                  
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim("EmployeeId",emp.Id.ToString() ),
                    new Claim("Email",emp.Email.ToString()),
                    new Claim(ClaimTypes.Email,user.Email!),
                    
                    };

                    var userRoles = await _userManager.GetRolesAsync(user);

                    foreach (var role in userRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                    }

                    var accessToken = _token.GenerateAccessToken(claims);

                    var refreshToken = _token.GenerateRefreshToken();


                    user.UpdateRefreshToken(refreshToken, DateTime.Now.AddDays(2));

                    await _userManager.UpdateAsync(user);

               

                    var tokens = new TokenDto {
                    accessToken=accessToken,
                    refreshToken=refreshToken,
                    };
                    return Ok(tokens);
                }
                return BadRequest("incorrect password ");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Refresh")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Refresh(TokenDto token)
        {
            try
            {
                if (string.IsNullOrEmpty(token.accessToken) || string.IsNullOrEmpty(token.refreshToken))
                {
                    return BadRequest("Invalid token request");
                }

                string accessToken = token.accessToken;
                string refreshToken = token.refreshToken;

                
                var principal = _token.GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                {
                    return BadRequest("Invalid access token");
                }

                var userId = principal.FindFirst("Email")?.Value;
                var user = await _userManager.FindByEmailAsync(userId!.ToString());

                //if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                //{
                //    return BadRequest("Invalid refresh token");
                //}

              
                var newAccessToken = _token.GenerateAccessToken(principal.Claims);

              
                var newRefreshToken = _token.GenerateRefreshToken();

                user.UpdateRefreshToken(newRefreshToken,  DateTime.Now.AddDays(7) );
                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    accessToken = newAccessToken,
                    refreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("LogOut")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(GetEmployeeEmailCliam());
                if (user == null)
                {
                    return NotFound();
                }
                user.UpdateRefreshToken(null, DateTime.MinValue);

                var result = await _userManager.UpdateAsync(user);
                await _signInManager.SignOutAsync();

                if (!result.Succeeded)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("GetCurrentUser")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCurentUser(CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetSingleEmployeeByEmailAsync(GetEmployeeEmailCliam(), cancellationToken);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //1st login
        [AllowAnonymous]
        [HttpPost("updatePassword")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdatePassword(updatePasswordDto updatePasswordDto, CancellationToken cancellationToken)
        {
            try
            {
                if (updatePasswordDto.oldPassword == updatePasswordDto.newPassword)
                {
                    return BadRequest();
                }
                var user = await _userManager.FindByEmailAsync(updatePasswordDto.Email);
                if (user == null)
                {
                    return NotFound();
                }
                var result = await _userManager.ChangePasswordAsync(user, updatePasswordDto.oldPassword, updatePasswordDto.newPassword);
                if (result.Succeeded)
                {
                    user.UpdateFirstLogin(false);
                    await _userManager.UpdateAsync(user);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ChangePassword")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangePassword(PasswordDto passwordDto)
        {
            try
            {
                if (passwordDto.oldPassword == passwordDto.newPassword)
                {
                    return BadRequest("old and new password cannot be same");
                }
                var user = await _userManager.FindByEmailAsync(GetEmployeeEmailCliam());
                if (user == null)
                {
                    return NotFound("user not found");
                }
                var result = await _userManager.ChangePasswordAsync(user, passwordDto.oldPassword, passwordDto.newPassword);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}