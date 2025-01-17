using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppGoodFriendsMvc.Models;
using Services;

namespace AppGoodFriendsMvc.Controllers;

public class OverviewController : Controller
{
    private readonly ILogger<OverviewController> _logger;
    private readonly IFriendsService _service;

    public OverviewController(ILogger<OverviewController> logger, IFriendsService service)
    {
        _logger = logger;
        _service = service;
    }

    #region Countries
    [HttpGet]
    public async Task<IActionResult> OverviewCountry()
    {
        var vwm = new OverviewCountryModel();

        //get db info overview
        vwm._info = await _service.InfoAsync();

        //get GstUsrInfoFriendsDto
        vwm.Friends = vwm._info.Friends;

        //grouping by country turning GstUsrInfoFriendsDto into GroupedFriendsDTO
        vwm.GroupedFriends = vwm.Friends.GroupBy(f => f.Country)
                                .Select(c => new GroupedFriendsDTO
                                {
                                    Country = c.Key,
                                    NrFriends = c.Where(f => f.City == null).Sum(f => f.NrFriends),
                                    NrCities = c.Where(f => f.City != null).Select(f => f.City)
                                                .Count() // counting number of cities in the country not including null values
                                })
                                .ToList();

        return View(vwm);
    }
    #endregion

    #region Cities

    [HttpGet]
    [Route("Overview/OverViewCities")]
    public async Task<IActionResult> OnGet(string country)
    {
        var vwm = new OverviewCitiesModel();
        //fetching country from query string
        vwm.Country = Request.Query["country"].ToString();

        //fetching db info from service
        vwm._info = await _service.InfoAsync();

        //if country is known find cities with linq
        //group the friends list by city and sum the friends 
        vwm.Friends = vwm._info.Friends.Where(f => f.Country == vwm.Country)
                                .GroupBy(c => c.City)
                                .Select(c => new CitySummaryDTO // extracting whats needed from the GstUsrInfoFriendsDto and putting it into the summary
                                {
                                    City = c.Key, // city taken from the Groupby key
                                    NrFriends = c.Sum(f => f.NrFriends), // summing number of friends in this city
                                    NrPets = 0 // 0 because it's friends list not pets
                                })
                                .ToList();


        vwm.Pets = vwm._info.Pets.Where(f => f.Country == vwm.Country && f.Country != null)
                                .GroupBy(c => c.City)
                                .Select(c => new CitySummaryDTO 
                                {
                                    City = c.Key,
                                    NrFriends = 0, 
                                    NrPets = c.Sum(p => p.NrPets)
                                })
                                .ToList();

        /* before the above 2 Linq statements _info.Friends and Pets were 2 different data types 
            now they are lists of CitySummaryDTO and can be combined into one list
        */ 
        
        vwm.FriendPetSummaries = vwm.Pets.Concat(vwm.Friends)
                            .GroupBy(c => c.City)
                            .Select(c => new CitySummaryDTO // combining number of friends and pets living in the same city 
                            {
                                City = c.Key,
                                NrPets = c.Sum(p => p.NrPets),
                                NrFriends = c.Sum(f => f.NrFriends)
                            })
                            .ToList();
    


        return View("OverviewCities", vwm);
    }

    #endregion


    #region List of friends
    [HttpGet]
    public async Task<IActionResult> ListOfFriends(string search, int pagenr)
    {
        var vwm = new ListOfFriendsModel(){ ThisPageNr = pagenr, SearchFilter = search };

        //Use the Service
        var resp = await _service.ReadFriendsAsync(vwm.UseSeeds, false, vwm.SearchFilter, vwm.ThisPageNr, vwm.PageSize);
        vwm.Friends = resp.PageItems;
        vwm.NrOfFriends = resp.DbItemsCount;

        //Pagination
        vwm.UpdatePagination(resp.DbItemsCount);
        

        return View("ListOfFriends", vwm);
    }

    [HttpPost]
    public async Task<IActionResult> Search(ListOfFriendsModel vwm)
    {
        //Use the Service
        var resp = await _service.ReadFriendsAsync(vwm.UseSeeds, false, vwm.SearchFilter, vwm.ThisPageNr, vwm.PageSize);
        vwm.Friends = resp.PageItems;
        vwm.NrOfFriends = resp.DbItemsCount;

        //Pagination
        vwm.UpdatePagination(resp.DbItemsCount);

        //Page is rendered as the postback is part of the form tag
        return View("ListOfFriends", vwm);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteFriend(Guid itemId, ListOfFriendsModel vwm)
    {
        await _service.DeleteFriendAsync(itemId);

        //Use the Service
        var resp = await _service.ReadFriendsAsync(vwm.UseSeeds, false, vwm.SearchFilter, vwm.ThisPageNr, vwm.PageSize);
        vwm.Friends = resp.PageItems;
        vwm.NrOfFriends = resp.DbItemsCount;

        //Pagination
        vwm.UpdatePagination(resp.DbItemsCount);

        return View("ListOfFriends", vwm);
    }

    #endregion

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
