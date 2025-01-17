using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AppGoodFriendsMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace AppGoodFriendsMvc.Models;

public class SeedModel
{

    public int NrOfFriends { get; set; }

    [BindProperty]
    [Required (ErrorMessage = "You must enter nr of items to seed")]
    public int NrOfItems { get; set; } = 100;

    [BindProperty]
    public bool RemoveSeeds { get; set; } = true;

    public SeedModel()
    {
        
    }
}



