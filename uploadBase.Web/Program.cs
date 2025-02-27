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

/*Bootstrap logger
 */

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    //WebRootPath = "wwwroot",
});

/*configure appsetting options
 */

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<FormOptions>(opt =>
{
    //opt.BufferBodyLengthLimit = 512 * 1024 * 1024;

    //it needs
    opt.MultipartBodyLengthLimit = 512 * 1024 * 1024;

});

builder.Services.Configure<IISServerOptions>(opt =>
{
    opt.MaxRequestBodySize = 512 * 1024 * 1024;

});


/*UseSerilog configuration
 */

/*Setup DB Context
 */
//builder.Services.AddDbContext<BFAContext>();


builder.Services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();

/*inject service
 */

/*setup cors policy
 */


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
    .AddDataAnnotationsLocalization();




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

app.UseRequestLocalization();

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

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//using attribute for routing
app.MapControllers();

app.Run();
