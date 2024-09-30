using MusicAI.Infrastructure.Data.Configurations;

namespace MusicAI.Infrastructure.Data.Configurations.Registeration
{
    public class TranscriptionServiceConfigRegisteration
    {
        public static void AddTranscriptionServiceConfig(IServiceCollection services, IConfiguration _Configuration)
        {

            AnthemConfig.anthemScorePath = _Configuration.GetValue<string>("AnthemConfig:anthemScorePath");
            AnthemConfig.outPutDir = _Configuration.GetValue<string>("AnthemConfig:outPutDir");
            AnthemConfig.outPutFormat = _Configuration.GetValue<string>("AnthemConfig:outPutFormat");
            AnthemConfig.inPutDir = _Configuration.GetValue<string>("AnthemConfig:inPutDir");
            AnthemConfig.inPutFormat = _Configuration.GetValue<string>("AnthemConfig:inPutFormat");
            AnthemConfig.appDataLocal = _Configuration.GetValue<string>("AnthemConfig:appDataLocal");
            AnthemConfig.MaxCuncurrentRequestPerCore = _Configuration.GetValue<int>("AnthemConfig:MaxCuncurrentRequestPerCore");
            AnthemConfig.MaxCoreParallelism = _Configuration.GetValue<int>("AnthemConfig:MaxCoreParallelism");
        }
    }
}
