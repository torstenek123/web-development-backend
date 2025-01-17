using DbRepos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DTO;
using Services;

namespace AppGoodFriendsRazor.Pages;

public class OverviewCountryModel : PageModel
{

    public List<GstUsrInfoFriendsDto> Friends;
    public List<GroupedFriendsDTO> GroupedFriends;
    public List<GroupedFriendsDTO> UnknownFriends;
    private readonly ILogger<OverviewCountryModel> _logger;
    private GstUsrInfoAllDto _info;
    readonly  IFriendsService _service;

    public OverviewCountryModel(ILogger<OverviewCountryModel> logger, IFriendsService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> OnGet()
    {
        //get db info overview
        _info = await _service.InfoAsync();

        //get GstUsrInfoFriendsDto
        Friends = _info.Friends;

        //grouping by country turning GstUsrInfoFriendsDto into GroupedFriendsDTO
        GroupedFriends = Friends.GroupBy(f => f.Country)
                                .Select(c => new GroupedFriendsDTO
                                {
                                    Country = c.Key,
                                    NrFriends = c.Where(f => f.City == null).Sum(f => f.NrFriends),
                                    NrCities = c.Where(f => f.City != null).Select(f => f.City)
                                                .Count() // counting number of cities in the country not including null values
                                })
                                .ToList();

        return Page();
    }
}



