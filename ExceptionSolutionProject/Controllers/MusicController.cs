using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using System.Threading.Tasks;

public class MusicController : Controller
{
    private readonly SpotifyService _spotifyService;

    public MusicController(SpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            ModelState.AddModelError(string.Empty, "Query parameter is required.");
            return View();
        }

        var result = await _spotifyService.SearchMusicAsync(query);
        ViewBag.Result = result;
        return View();
    }

}
