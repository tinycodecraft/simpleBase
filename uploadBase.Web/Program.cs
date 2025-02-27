//section 1
//logger

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Text.Json.Serialization;
using uploadBase.Web.Helpers;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    //WebRootPath = "wwwroot",
});


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

//Setup DB Context
//builder.Services.AddDbContext<BFAContext>();


builder.Services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();


// Add services to the container.
builder.Services.AddCustomLocalization("en-US", "zh-HK");
builder.Services.AddControllersWithViews()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{

});

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
