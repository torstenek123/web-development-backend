using DbRepos;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services;

namespace AppGoodFriendsMvc.Models;

public class ViewFriendModel
{

    public IFriend Friend  { get; set; }

    [BindProperty]
    public Guid FriendId { get; set; }

    public ViewFriendModel()
    {
        
    }
}



