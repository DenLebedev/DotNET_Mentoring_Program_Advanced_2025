using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CartingService.Middleware
{
    public class TokenLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                var jwtHandler = new JwtSecurityTokenHandler();

                if (jwtHandler.CanReadToken(token))
                {
                    var jwtToken = jwtHandler.ReadJwtToken(token);
                    var subject = jwtToken.Subject;
                    var roles = jwtToken.Claims.Where(c => c.Type == "role").Select(c => c.Value);
                    var permissions = jwtToken.Claims.Where(c => c.Type == "permission").Select(c => c.Value);

                    Console.WriteLine($"[Token] Subject: {subject}");
                    Console.WriteLine($"[Token] Roles: {string.Join(", ", roles)}");
                    Console.WriteLine($"[Token] Permissions: {string.Join(", ", permissions)}");
                }
            }

            await _next(context);
        }
    }
}
