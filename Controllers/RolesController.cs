using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Service;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("Create")]
        public async Task<ActionResult<bool>> AddRoles(string roleName)
        {
            try
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));

                var findRole = await _roleManager.FindByNameAsync(roleName);

                await _roleManager.AddClaimAsync(findRole, new Claim(CustomClaimType.Permission, Permissions.Dashboards.View));
                await _roleManager.AddClaimAsync(findRole, new Claim(CustomClaimType.Permission, Permissions.Dashboards.Create));
            }
            catch (Exception e)
            {

                throw;
            }


            return Ok(true);
        }


        [HttpGet("AssignRole")]
        public async Task<ActionResult<bool>> AssignRoles(string userName, string roleName)
        {
            var users = await _userManager.FindByNameAsync(userName);

            await _userManager.AddToRoleAsync(users, roleName);


            return Ok(true);
        }


    }
}
