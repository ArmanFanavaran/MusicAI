using MusicAI.Services.MusicService;

namespace MusicAI.Services.Registeration
{
    public class AnthemScoreServiceRegisteration
    {
        public static void AddAnthemScoreServiceRegisteration(IServiceCollection services)
        {
            services.AddScoped<AnthemScoreCMDService>();
        }
    }
}
