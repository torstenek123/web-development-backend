using DbRepos;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services;

namespace AppGoodFriendsMvc.Models;

public class ListOfFriendsModel
{

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

    public ListOfFriendsModel()
    {
        
    }
    public void UpdatePagination(int nrOfItems)
    {
        //Pagination
        NrOfPages = (int)Math.Ceiling((double)nrOfItems / PageSize);
        PrevPageNr = Math.Max(0, ThisPageNr - 1);
        NextPageNr = Math.Min(NrOfPages - 1, ThisPageNr + 1);
        NrVisiblePages = Math.Min(10, NrOfPages);
    }
}



