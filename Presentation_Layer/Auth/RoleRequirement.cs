using Microsoft.AspNetCore.Authorization;

namespace Presentation_Layer.Auth
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public int RoleId { get; }

        public RoleRequirement(int roleId)
        {
            RoleId = roleId;
        }
    }
}
