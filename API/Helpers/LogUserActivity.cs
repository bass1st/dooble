namespace API.Helpers;

public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();

        if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

        var userId = resultContext.HttpContext.User.GetUserId();
        // Access repository
        var unitOfWork = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
        // Get User object
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);

        user.LastActive = DateTime.UtcNow;

        await unitOfWork.Complete();
    }
}