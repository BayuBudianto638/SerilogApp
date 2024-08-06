using Microsoft.AspNetCore.Http.Features;
using Serilog;
using Serilog.Events;
using System.Diagnostics;

namespace SerilogApp.Diagnostics
{
    public class SerilogMiddleware
    {
        private readonly RequestDelegate _next;

        public SerilogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                Log.Information("Response: {StatusCode} {Body}", context.Response.StatusCode, responseBodyText);

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}
