using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace hehehe.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLower();

            // Bỏ qua Auth, static files, favicon...
            if (path.StartsWith("/auth") || path.StartsWith("/css") || path.StartsWith("/js") ||
                path.StartsWith("/images") || path.Contains(".") || path == "/")
            {
                await _next(context);
                return;
            }

            if (context.Session.GetString("MaNhapHoc") == null)
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }

            await _next(context);
        }
    }

    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}