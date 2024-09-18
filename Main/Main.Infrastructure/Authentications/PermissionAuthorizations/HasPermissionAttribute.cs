using Microsoft.AspNetCore.Authorization;

namespace Main.Infrastructure.Authentications.PermissionAuthorizations
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission) : base(policy: permission.ToString())
        {

        }
    }
}
