using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MusicAI.Controllers
{
    [ApiController]

    // with bellow data anotation the default return type of each action is application/json
    [Produces("application/json")]

    //define the path of the versioned controller
    [Route("[controller]/[action]")]
    public class MyController : ControllerBase
    {
        [HttpGet]
        public ActionResult ProcessAudio()
        {
            Process process = new Process();
            process.StartInfo.FileName = @"D:\Programs\AutoIt3\Au3Info_x64.exe";
            process.StartInfo.Arguments = @"D:\ArmanFanavaranParsRayaneh\Projects\bigProject\MusicAi\WebApp\MusicAI\MusicAI\wwwroot\config\your_autoit_script.au3";
            process.Start();
            process.WaitForExit();
            return Ok();
        }
    }
}
