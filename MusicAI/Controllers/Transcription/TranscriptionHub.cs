using Microsoft.AspNetCore.SignalR;
using MusicAI.Contracts.Enums;
using MusicAI.Services.Logs;
using MusicAI.Services.MusicService;

namespace MusicalAI2.microService.TranscriptionService.API.Controllers
{
    public class TranscriptionHub : Hub
    {
        public async Task SendAudioChunkBase64(string audioChunkBase64,string roomID,long timeSlotId,byte instrumentId)
        {
            byte[] audioChunk = Convert.FromBase64String(audioChunkBase64);
            // Process the audio chunk
            string outputFile = await new AnthemScoreCMDService().ProcessAudio(audioChunk,roomID,timeSlotId,instrumentId);

            // Send the result back to the client
            await Clients.Caller.SendAsync("ReceiveTranscription", outputFile);
        }

        public async Task SendAudioChunkBinary(byte[] audioChunk,string roomID,long timeSlotId,byte instrumentId)
        {
            // Process the audio chunk
            string outputFile = await new AnthemScoreCMDService().ProcessAudio(audioChunk,roomID,timeSlotId,instrumentId);

            // Send the result back to the client
            await Clients.Caller.SendAsync("ReceiveTranscription", outputFile);
        }
    }
}
