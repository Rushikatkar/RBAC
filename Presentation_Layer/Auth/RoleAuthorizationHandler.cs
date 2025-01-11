using Microsoft.AspNetCore.Authorization;

namespace Presentation_Layer.Auth
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            // Get the role claim from the user's token
            var roleClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            if (roleClaim != null && int.TryParse(roleClaim.Value, out int userRoleId))
            {
                // If the user's RoleId matches the required RoleId, mark the requirement as fulfilled
                if (userRoleId == requirement.RoleId)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
