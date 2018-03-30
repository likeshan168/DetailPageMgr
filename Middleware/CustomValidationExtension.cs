using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;

namespace DetailPage.Middleware
{
    public static class CustomValidationExtension
    {
        public static IApplicationBuilder UseCustomValidation(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof (app));
            return app.UseMiddleware<CustomValidationMiddleware>();
        }
    }
}