using ANPCentral.Data;

namespace ANPCentral.Middlewares
{
    public class VerifyIfUserExistsMiddleware
    {
        private readonly RequestDelegate _next;

        public VerifyIfUserExistsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserDataContext dbContext)
        {
            if (context.Request.Path.StartsWithSegments("/users"))
            {
                // Check if the id parameter is present in the route
                if (context.Request.RouteValues.TryGetValue("id", out object idObj) && Guid.TryParse(idObj.ToString(), out Guid id))
                {
                    var user = await dbContext.Users.FindAsync(id);
                    if (user == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsync("User not found");
                        return;
                    }
                    context.Items["user"] = user;
                }
            }

            await _next(context);
        }
    }
}
