using DbRepos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DTO;
using Services;

namespace AppGoodFriendsRazor.Pages;

public class OverviewCitiesModel : PageModel
{

    public List<CitySummaryDTO> Friends;
    public List<CitySummaryDTO> Pets;
    public List<CitySummaryDTO> FriendPetSummaries = new List<CitySummaryDTO>();
    public string Country = null;
    private readonly ILogger<OverviewCitiesModel> _logger;
    private GstUsrInfoAllDto _info;
    private  IFriendsService _service;

    public OverviewCitiesModel(ILogger<OverviewCitiesModel> logger, IFriendsService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> OnGet()
    {
        //fetching country from query string
        Country = Request.Query["country"].ToString();

        //fetching db info from service
        _info = await _service.InfoAsync();

        //if country is known find cities with linq
        //group the friends list by city and sum the friends 
        Friends = _info.Friends.Where(f => f.Country == Country)
                                .GroupBy(c => c.City)
                                .Select(c => new CitySummaryDTO // extracting whats needed from the GstUsrInfoFriendsDto and putting it into the summary
                                {
                                    City = c.Key, // city taken from the Groupby key
                                    NrFriends = c.Sum(f => f.NrFriends), // summing number of friends in this city
                                    NrPets = 0 // 0 because it's friends list not pets
                                })
                                .ToList();


        Pets = _info.Pets.Where(f => f.Country == Country && f.Country != null)
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
        
        FriendPetSummaries = Pets.Concat(Friends)
                            .GroupBy(c => c.City)
                            .Select(c => new CitySummaryDTO // combining number of friends and pets living in the same city 
                            {
                                City = c.Key,
                                NrPets = c.Sum(p => p.NrPets),
                                NrFriends = c.Sum(f => f.NrFriends)
                            })
                            .ToList();
    


        return Page();
    }
}




