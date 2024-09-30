namespace MusicAI.Models
{
    public class MusicNote
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string ChunkOutPutAddress { get; set; }
        public int ChunkNumber { get; set; }
        public string MainFileAddress { get; set; }
    }
}
