using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Primal.Infrastructure
{
    public class EncounterIdRequired : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.HttpContext.RequestServices.GetRequiredService<UserContext>().EncounterId is null)
            {
                filterContext.Result = new UnauthorizedResult();
            }
        }
    }
}