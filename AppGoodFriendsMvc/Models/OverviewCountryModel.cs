using DbRepos;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services;

namespace AppGoodFriendsMvc.Models;

public class OverviewCountryModel
{

    public List<GstUsrInfoFriendsDto> Friends {get; set;}
    public List<GroupedFriendsDTO> GroupedFriends {get; set;}
    public List<GroupedFriendsDTO> UnknownFriends {get; set;}

    public GstUsrInfoAllDto _info {get; set;}

    public OverviewCountryModel()
    {
        
    }
}



