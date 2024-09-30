namespace MusicAI.Infrastructure.Data.Configurations
{
    public class AnthemConfig
    {
        public static string anthemScorePath { set; get; }
        public static string outPutDir { set; get; }
        public static string outPutFormat { set; get; }
        public static string inPutDir { set; get; }
        public static string inPutFormat { set; get; }
        public static string appDataLocal { set; get; }
        public static int MaxCuncurrentRequestPerCore { set; get; }
        public static int MaxCoreParallelism { set; get; }
    }
}
