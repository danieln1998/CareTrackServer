using AutoMapper;
using CareTrack.API.CustomActionFilters;
using CareTrack.API.Models.Domain;
using CareTrack.API.Models.DTO;
using CareTrack.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace CareTrack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly IMapper mapper;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository ,IMapper mapper)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]
        [ValidateModel]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                //Add Roles to this user
                
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                                                
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        var userId = await userManager.GetUserIdAsync(identityUser);
                        return Ok(new { userId });

                    }

                }

            }
            return BadRequest("Not Registerd!");

        }

        [HttpPost]
        [Route("Login")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.UserName);

            if (user != null)
            {
                var resultAccessFailed = user.AccessFailedCount;
                if (resultAccessFailed > 3)
                {

                    return BadRequest("Your account has been locked, contact the admin for help");

                }
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    user.AccessFailedCount = 0;
                    await userManager.UpdateAsync(user);
                    
                    //Get Roles for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        //Create Token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken

                        };

                        return Ok(response);
                    }

                }
                else
                {
                    await userManager.AccessFailedAsync(user);
                    
                }
            }

            return BadRequest("UserName or Password incorrect.");

        }

        [HttpDelete]
        [Route("Delete/{id:Guid}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                return Ok();
            }

            return NotFound("User Not Found");
        }

        [HttpGet]
        [Route("GetById/{id:Guid}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                var userDto = mapper.Map<UserDto>(user);
                userDto.Roles = roles;
                return Ok(userDto);
            }

            return NotFound("User Not Found");
        }

        [HttpGet]
        [Route("GetUsersByRole")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetUsersByRole([FromQuery] string role)
        {
            var users = await userManager.GetUsersInRoleAsync(role);
        
                return Ok(mapper.Map<List<UserDto>>(users));
            
       
        }

        [HttpPut]
        [Route("UpdateUserName")]
        [ValidateModel]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> UpdateUserName([FromBody] UpdateUserNameDto updateUserDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("Error while identifying user");
            }
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            var existingUser = await userManager.FindByNameAsync(updateUserDto.UserName);
            if (existingUser != null && existingUser.Id != userId)
            {
                return BadRequest("Username already taken by another user");
            }

            user.UserName = updateUserDto.UserName;
            user.Email = updateUserDto.UserName;
            await userManager.UpdateAsync(user);
            

            return Ok(mapper.Map<UserDto>(user));


        }

        [HttpPut]
        [Route("UpdatePassword")]
        [ValidateModel]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("Error while identifying user");
            }
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, updatePasswordDto.CurrentPassword, updatePasswordDto.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return BadRequest("Error updating password");
            }

            return Ok();

        }

        [HttpPut]
        [Route("UpdateRoles/{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateRoles([FromRoute] Guid id, [FromBody] UpdateRolesDto updateRolesDto)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound("User Not Found");
                
            }

            if (updateRolesDto.Roles != null && updateRolesDto.Roles.Any())
            {
                var roles = await userManager.GetRolesAsync(user);
                var resultRemove = await userManager.RemoveFromRolesAsync(user, roles.ToArray());

                if (resultRemove.Succeeded)
                {
                    var result = await userManager.AddToRolesAsync(user, updateRolesDto.Roles);

                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                }
                
            }


            return BadRequest("There was a problem when updating the roles");

            

        }

        [HttpPut]
        [Route("UnlockAccount/{id:Guid}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UnlockAccount([FromRoute] Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound("User Not Found");

            }

            user.AccessFailedCount = 0;
            await userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpGet]
        [Route("IsAuthorized")]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public IActionResult IsAuthorized([FromQuery] string roleRequest)
        {

           if (!User.IsInRole(roleRequest))
            {
                return Unauthorized("Unauthorized user");
            }

            return Ok(true);

        }


    }

}
