using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using MvcCore.Models;

namespace MvcCore.Authorization
{
    public class ClientIsOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Client>
    {
        UserManager<IdentityUser> _userManager;

        public ClientIsOwnerAuthorizationHandler(UserManager<IdentityUser>
            userManager)
        {
            _userManager = userManager;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   Client resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            // If not asking for CRUD permission, return.

            //if (requirement.Name != Constants.CreateOperationName &&
            //    requirement.Name != Constants.ReadOperationName &&
            //    requirement.Name != Constants.UpdateOperationName &&
            //    requirement.Name != Constants.DeleteOperationName)
            //{
            //    return Task.CompletedTask;
            //}

            if (resource.UserName == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
