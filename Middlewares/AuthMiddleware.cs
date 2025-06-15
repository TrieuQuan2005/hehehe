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

            // Bỏ qua kiểm tra cho trang login, static files
            if (path.StartsWith("/auth/login") || path.StartsWith("/css") || path.StartsWith("/js"))
            {
                await _next(context);
                return;
            }

            // Kiểm tra nếu chưa đăng nhập (session không có MaNhapHoc)
            if (context.Session.GetString("MaNhapHoc") == null)
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }

            await _next(context);
        }
    }

    // Extension method để gọi từ Program.cs
    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}