using MusicAI.Infrastructure.Data.Configurations.Registeration;
using MusicAI.Services.Registeration;

namespace MusicalAI2.microService.TranscriptionService.Application.RegisterationFacad
{
    public static class TranscriptionserviceRegisterationFacad
    {
        public static void AddTranscriptionserviceRegisterationFacad(this IServiceCollection services, IConfiguration _Configuration)
        {
            TranscriptionServiceConfigRegisteration.AddTranscriptionServiceConfig(services,_Configuration);
            AnthemScoreServiceRegisteration.AddAnthemScoreServiceRegisteration(services);
        }
    }
}
