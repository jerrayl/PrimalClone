using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Primal.Infrastructure
{
    public class ScenarioIdRequired : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.HttpContext.RequestServices.GetRequiredService<UserContext>().ScenarioId is null)
            {
                filterContext.Result = new UnauthorizedResult();
            }
        }
    }
}