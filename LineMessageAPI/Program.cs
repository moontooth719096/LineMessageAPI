using LineMessageAPI.Interfaces;
using LineMessageAPI.Middlewares;
using LineMessageAPI.Models.LineMessage;
using LineMessageAPI.Services;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(p =>
{
    var config = new LineMessageOption();
    var section = configuration.GetSection("LineMessageAPI");
    section.Bind(config);
    return config;
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddScoped<RequestIDService>();
builder.Services.AddSingleton<ILogService, LocalFileService>();
builder.Services.AddSingleton<APIHelper>();


builder.Services.AddSingleton<ILineMessageService, LineMessageService>();

var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();//�������ҥ~
app.UseAuthorization();

app.MapControllers();

app.Run();
