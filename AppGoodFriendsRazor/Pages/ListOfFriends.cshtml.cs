using DbRepos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DTO;
using Services;

namespace AppGoodFriendsRazor.Pages;

public class ListOfFriendsModel : PageModel
{

    private readonly ILogger<ListOfFriendsModel> _logger;
    private GstUsrInfoAllDto _info;
    private  IFriendsService _service;
    public List<IFriend> Friends { get; set; }

    [BindProperty]
    public bool UseSeeds { get; set; } = true;

    public int NrOfFriends { get; set; }

    //Pagination
    public int NrOfPages { get; set; }
    public int PageSize { get; } = 10;

    public int ThisPageNr { get; set; } = 0;
    public int PrevPageNr { get; set; } = 0;
    public int NextPageNr { get; set; } = 0;
    public int NrVisiblePages { get; set; } = 0;

    //ModelBinding for the form
    [BindProperty]
    public string SearchFilter { get; set; } = null;


    public ListOfFriendsModel(ILogger<ListOfFriendsModel> logger, IFriendsService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> OnGet()
    {
        //Read a QueryParameters
        if (int.TryParse(Request.Query["pagenr"], out int pagenr))
        {
            ThisPageNr = pagenr;
        }

        SearchFilter = Request.Query["search"];

        //Use the Service
        var resp = await _service.ReadFriendsAsync(UseSeeds, false, SearchFilter, ThisPageNr, PageSize);
        Friends = resp.PageItems;
        NrOfFriends = resp.DbItemsCount;

        //Pagination
        UpdatePagination(resp.DbItemsCount);
        

        return Page();
    }
    public async Task<IActionResult> OnPostSearch()
    {
        //Use the Service
        var resp = await _service.ReadFriendsAsync(UseSeeds, false, SearchFilter, ThisPageNr, PageSize);
        Friends = resp.PageItems;
        NrOfFriends = resp.DbItemsCount;

        //Pagination
        UpdatePagination(resp.DbItemsCount);

        //Page is rendered as the postback is part of the form tag
        return Page();
    }
    public async Task<IActionResult> OnPostDeleteFriend(Guid friendId)
    {
        await _service.DeleteFriendAsync(friendId);

        //Use the Service
        var resp = await _service.ReadFriendsAsync(UseSeeds, false, SearchFilter, ThisPageNr, PageSize);
        Friends = resp.PageItems;
        NrOfFriends = resp.DbItemsCount;

        //Pagination
        UpdatePagination(resp.DbItemsCount);

        return Page();
    }

    private void UpdatePagination(int nrOfItems)
    {
        //Pagination
        NrOfPages = (int)Math.Ceiling((double)nrOfItems / PageSize);
        PrevPageNr = Math.Max(0, ThisPageNr - 1);
        NextPageNr = Math.Min(NrOfPages - 1, ThisPageNr + 1);
        NrVisiblePages = Math.Min(10, NrOfPages);
    }
}



