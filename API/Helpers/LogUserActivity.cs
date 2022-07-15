using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();
            // Access repository
            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            // Get User object
            var user = await repo.GetUserByIdAsync(userId);

            user.LastActive = DateTime.Now;

            await repo.SaveAllAsync();
        }
    }
}
