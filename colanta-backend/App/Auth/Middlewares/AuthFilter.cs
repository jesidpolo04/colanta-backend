using colanta_backend.App.Auth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace colanta_backend.App.Auth.Middlewares{
    public class AuthFilter : IActionFilter
{
    private JWTService service;

    public AuthFilter(IConfiguration configuration){
        this.service = new JWTService(configuration);
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorization);
        var authorizationArray = authorization.ToString().Split(" ");
        var token = authorizationArray.GetValue(1).ToString();
        if(!this.service.validToken(token)){
            context.Result = new UnauthorizedResult();
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Do something after the action executes.
    }
}
}

