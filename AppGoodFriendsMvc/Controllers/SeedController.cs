using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppGoodFriendsMvc.Models;
using Services;

namespace AppGoodFriendsMvc.Controllers;

public class SeedController : Controller
{
    private readonly ILogger<SeedController> _logger;
    private readonly IFriendsService _service;

    public SeedController(ILogger<SeedController> logger, IFriendsService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Seed()
    {
        var vwm = new SeedModel();
        var resp = await _service.ReadFriendsAsync(true, true, null, 0, 100);
        vwm.NrOfFriends = resp.DbItemsCount;
        return View(vwm);
    }

    [HttpPost]
    public async Task<IActionResult> Seeding(SeedModel vwm)
    {
        if (ModelState.IsValid)
        {
            if (vwm.RemoveSeeds)
            {
                await _service.RemoveSeedAsync(true);
                await _service.RemoveSeedAsync(false);
            }
            await _service.SeedAsync(vwm.NrOfItems);

            return RedirectToAction("ListOfFriends", "Overview");
        }
        return View(vwm);           
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
