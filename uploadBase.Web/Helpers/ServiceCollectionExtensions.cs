using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Net;
using uploadBase.Shared.Models;

namespace uploadBase.Web.Helpers
{

    public static class ExceptionHandlerExtensions
    {
        //Simple handler
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    var exDetails = new ExceptionDetails((int)HttpStatusCode.InternalServerError, error?.Error.Message ?? "");

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = exDetails.StatusCode;
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.Response.Headers.Add("Application-Error", exDetails.Message);
                    context.Response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");

                    await context.Response.WriteAsync(exDetails.ToString());
                });
            });

            return app;
        }

        //custom handler with logging
        public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app)
            => app.UseMiddleware<ApiExceptionHandlingMiddleware>();
    }
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddCorsConfig(this IServiceCollection services, MD.CorsPolicySetting policy, string name="AllowAll")
        {


            services.AddCors(c => c.AddPolicy(name,
                options => options.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

            if (policy != null )
            {
                services.AddCors(c => c.AddPolicy(policy.Name, options => options.WithOrigins(policy.AllowOrigins).WithHeaders(policy.AllowHeaders).WithMethods(policy.AllowMethods)));
            }

            return services;
        }


        public static IServiceCollection AddCustomLocalization(this IServiceCollection services,params string[] langs)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            //without country code "-xx" suffix => culture invariant
            var supportedCultures = Enumerable.Range(0, langs.Length).Select(e => new CultureInfo(langs[e])).ToList(); // new List<CultureInfo> { new("en"), new("fa") };
            if(supportedCultures.Count == 0)
            {
                supportedCultures.Add(new("en-US"));
            }
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
                
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            return services;
        }
    }
}
