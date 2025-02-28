//section 1
//logger

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Text.Json.Serialization;
using uploadBase.Shared.Tools;
using uploadBase.Shared;
using uploadBase.Web.Helpers;
using static uploadBase.Shared.Interfaces;
using Serilog;
using Serilog.Templates;
using static uploadBase.Shared.Constants;
using uploadBase.Web.Resources;

/*Bootstrap logger
 */
Log.Logger = new LoggerConfiguration().MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    //WebRootPath = "wwwroot",
});



/*configure appsetting options
 */
var pathsetting = builder.Configuration.GetSection(Setting.PathSetting);
pathsetting[nameof(MD.PathSetting.BasePath)] = Directory.GetCurrentDirectory();
pathsetting = pathsetting.RevertPathSlash<MD.PathSetting>();
var baseurl = builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey);
if (!string.IsNullOrEmpty(baseurl))
{
    var uri = new Uri(baseurl);

    pathsetting[nameof(MD.PathSetting.BaseUrl)] = uri.AbsolutePath;
}

builder.Services.Configure<MD.PathSetting>(pathsetting);
var CorsPolicy = builder.Configuration.GetSection(Setting.CorsPolicySetting).Get<MD.CorsPolicySetting>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<FormOptions>(opt =>
{

    opt.BufferBodyLengthLimit = 512 * 1024 * 1024;

    //it needs
    opt.MultipartBodyLengthLimit = 512 * 1024 * 1024;

});

builder.Services.Configure<IISServerOptions>(opt =>
{
    opt.MaxRequestBodySize = 512 * 1024 * 1024;

});

/*UseSerilog configuration
 */
builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
.ReadFrom.Configuration(context.Configuration)
.ReadFrom.Services(services)
//.WriteTo.Console(new ExpressionTemplate(
//    // Include trace and span ids when present.
//    "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}"))
//
);


/*Setup DB Context
 */
//builder.Services.AddDbContext<BFAContext>();


builder.Services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();

/*inject service
 */

/*setup cors policy
 */
builder.Services.AddCorsConfig(CorsPolicy!);


/*setup signalr
 */
//builder.Services.AddSignalR(hubOptions =>
//{
//    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
//    hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(15);
//    hubOptions.EnableDetailedErrors = true;
//});

/*setup controller
 */
builder.Services.AddCustomLocalization("en-US", "zh-HK");
builder.Services.AddControllersWithViews()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options => {
        //using same resource for data annotation for multiple classes
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        factory.Create(typeof(SharedResource));
    });




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/*add compression
 */

builder.Services.AddSwaggerGen(option =>
{

});

/*add authentication
 */

/*Try to add session
 */
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".uploadBase.Session";
});

/*add hub gateway (observable for creating subscriber through hub responding to signalr client's request=subscribing method)
 */
//builder.Services.AddSingleton(typeof(IResultGateway<>), typeof(ModelResultGateway<>));


/*add mediatr request routing
 */
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Interfaces>());

//builder.Services.AddHsts(options =>
//{
//    options.Preload = true;
//    options.IncludeSubDomains = true;
//    options.MaxAge = TimeSpan.FromDays(60);
//    options.ExcludedHosts.Add("example.com");
//    options.ExcludedHosts.Add("www.example.com");
//});

//builder.Services.AddHttpsRedirection(options =>
//{
//    options.RedirectStatusCode =307; //temporaryredirect
//    options.HttpsPort = 5001;
//});


var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    ApplyCurrentCultureToResponseHeaders = true,
    CultureInfoUseUserOverride = false,
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseApiExceptionHandling();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{

    app.UseExceptionHandler("/Home/Error");
    //add notice for browser to use strict transport security
    app.UseHsts();
}
/*Use Cors
 */

app.UseCors(CorsPolicy!.Name);

/*Use SerilogRequestLogging
 */
app.UseSerilogRequestLogging(option =>
{
    option.EnrichDiagnosticContext = (diagnostic, http) =>
    {
        diagnostic.Set("LocalTime", DateTime.Now.ToString("yyyyMMdd+HHmmss"));

    };
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

//using attribute for routing
app.MapControllers();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
