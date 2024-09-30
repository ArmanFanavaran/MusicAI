using Microsoft.AspNetCore.Mvc;
using MusicAI.Services;
using MusicAI.Services.MusicService;

namespace MusicAI.Controllers.Transcription
{
    public class TranscriptionController : Controller
    {
       
        // wave output
        public IActionResult Wav()
        {
            return View("~/Pages/Transcription/Index-wav.cshtml");
        }
        // webm output with new update
        //public IActionResult Mp3()
        //{
        //    return View("~/Pages/Transcription/Index-mp3.cshtml");
        //}

        // wave output
        //public IActionResult Wav1()
        //{
        //    return View("~/Pages/Transcription/Index-wav1.cshtml");
        //}

        //public IActionResult OnsetWave()
        //{
        //    return View("~/Pages/Transcription/OnsetWave.cshtml");
        //}

       

        [HttpGet]
        public async Task TestMultipleProcessV3()
        {
            var service = new AnthemScoreCmdServiceTest();
            await service.TestMultipleProcessV3();
        }

        [HttpGet]
        public async Task TestMultipleProcessV2()
        {
            var service = new AnthemScoreCmdServiceTest();
            await service.TestMultipleProcessV2();
        }
    }
}
