using DuitKu.Middleware;

namespace DuitKu.Bootstrap
{
    public static class RouteServiceProvider
    {
        public static WebApplication Boot(this WebApplication app)
        {
            app.MapControllers();

            return app;
        }

        public static WebApplication Middleware(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}