using FFMpegCore;
using Microsoft.AspNetCore.SignalR;
using MusicAI.Middleware;
using MusicAI.Middleware.ExceptionHandling;
using MusicalAI2.microService.TranscriptionService.API.Controllers;
using MusicalAI2.microService.TranscriptionService.Application.RegisterationFacad;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddRazorPages();
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024; // 1 MB (adjust as needed)
    options.EnableDetailedErrors = true;
    options.AddFilter<LoggingHubFilter>();
});
builder.Services.AddTranscriptionserviceRegisterationFacad(builder.Configuration);
var app = builder.Build();
app.UseCors(options =>
{
  options.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .SetIsOriginAllowed(origin => true);
});
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CustomLoggingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages();
    endpoints.MapHub<TranscriptionHub>("/transcriptionHub");
    //endpoints.MapHub<VoiceHub>("/voiceHub");
});

app.Run();
