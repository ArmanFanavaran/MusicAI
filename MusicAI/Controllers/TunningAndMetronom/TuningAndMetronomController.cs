using Microsoft.AspNetCore.Mvc;

namespace MusicAI.Controllers.TunningAndMetronom
{
    public class TuningAndMetronomController : Controller
    {
        public IActionResult tune()
        {
            return View("~/Pages/TuningAndMetronon/tune.cshtml");
        }
        public IActionResult tune2()
        {
            return View("~/Pages/TuningAndMetronon/tune2.cshtml");
        }
        public IActionResult metronome()
        {
            return View("~/Pages/TuningAndMetronon/metronome.cshtml");
        }
        public IActionResult multiRecorder()
        {
            return View("~/Pages/TuningAndMetronon/multiRecorder.cshtml");
        }
    }
}
