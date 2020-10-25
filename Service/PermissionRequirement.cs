using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Service
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }

        public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly UserManager<IdentityUser> _userManager;

            public PermissionAuthorizationHandler(UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }


            protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                PermissionRequirement requirement)
            {
                if (context.User == null)
                {
                    return;
                }

                var user = await _userManager.GetUserAsync(context.User);

                if (user == null)
                {
                    return;
                }

                var userRoleNames = await _userManager.GetRolesAsync(user);

                var userRoles = _roleManager.Roles.Where(a => userRoleNames.Contains(a.Name));

                foreach(var role in userRoles)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);

                    var permissions = roleClaims.Where(x => x.Type == CustomClaimType.Permission && x.Value == requirement.Permission && x.Issuer == "LOCAL AUTHORITY").Select(x => x.Value);

                    if (permissions.Any())
                    {
                        context.Succeed(requirement);
                        return;
                    }

                }

                
            }
        }
    }
}
