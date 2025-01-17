using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AppGoodFriendsRazor.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace AppGoodFriendsRazor.Pages
{
    public class SeedModel : PageModel
    {

        readonly IFriendsService _service = null;
        readonly ILogger<SeedModel> _logger = null;

        public int NrOfGroups => _nrOfGroups().Result;
        private async Task<int> _nrOfGroups()
        {
            var resp = await _service.ReadFriendsAsync(true, true, null, 0, 100);
            return resp.DbItemsCount;
        }

        [BindProperty]
        [Required (ErrorMessage = "You must enter nr of items to seed")]
        public int NrOfItems { get; set; } = 100;

        [BindProperty]
        public bool RemoveSeeds { get; set; } = true;

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (RemoveSeeds)
                {
                    await _service.RemoveSeedAsync(true);
                    await _service.RemoveSeedAsync(false);
                }
                await _service.SeedAsync(NrOfItems);

                return Redirect($"~/ListOfFriends");
            }
            return Page();           
        }

        //Inject services just like in WebApi
        public SeedModel(IFriendsService service, ILogger<SeedModel> logger)
        {
            _service = service;
            _logger = logger;
        }
    }
}
