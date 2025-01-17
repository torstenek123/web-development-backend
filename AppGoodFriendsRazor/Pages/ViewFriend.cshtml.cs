using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DTO;
using Services;

namespace AppMusicRazor.Pages
{
    public class ModalData
    {
        public string postdata { get; set; } 
    }
	public class ViewFriendModel : PageModel
    {
        //Just like for WebApi
        readonly IFriendsService _service = null;
        readonly ILogger<ViewFriendModel> _logger = null;

        public IFriend Friend  { get; set; }

        [BindProperty]
        public Guid FriendId { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Guid _friendId = Guid.Parse(Request.Query["id"]);
            Friend = await _service.ReadFriendAsync(_friendId, false);
            FriendId = _friendId;

            return Page();
        }
        public async Task<IActionResult> OnPostDeletePet(Guid itemId)
        {
            await _service.DeletePetAsync(itemId);
            Friend = await _service.ReadFriendAsync(FriendId, false);

            return Page();
        }
        public async Task<IActionResult> OnPostDeleteQuote(Guid itemId)
        {
            Friend = await _service.ReadFriendAsync(FriendId, false);
            Friend.Quotes = Friend.Quotes.Where(x => x.QuoteId != itemId).ToList();
            await _service.UpdateFriendAsync(new FriendCUdto(Friend));
            Friend = await _service.ReadFriendAsync(FriendId, false);

            return Page();
        }

        //Inject services just like in WebApi
        public ViewFriendModel(IFriendsService service, ILogger<ViewFriendModel> logger)
        {
            _service = service;
            _logger = logger;
        }
    }
}
