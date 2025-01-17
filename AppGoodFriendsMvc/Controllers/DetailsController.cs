using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppGoodFriendsMvc.Models;
using Models.DTO;
using Services;
using static AppGoodFriendsMvc.Models.EditFriendModel;
using AppGoodFriendsMvc.SeidoHelpers;
namespace AppGoodFriendsMvc.Controllers;

public class DetailsController : Controller
{
    private readonly ILogger<DetailsController> _logger;
    private readonly IFriendsService _service;

    public DetailsController(ILogger<DetailsController> logger, IFriendsService service)
    {
        _logger = logger;
        _service = service;
    }
    #region View details
    [HttpGet]
    public async Task<IActionResult> ViewFriend(string id)
    {
        var vwm = new ViewFriendModel();
        Guid _friendId = Guid.Parse(id);
        vwm.Friend = await _service.ReadFriendAsync(_friendId, false);
        vwm.FriendId = _friendId;

        return View("ViewFriend", vwm);
    }
    [HttpPost]
    public async Task<IActionResult> DeletePet(ViewFriendModel vwm, Guid itemId)
    {
        await _service.DeletePetAsync(itemId);
        vwm.Friend = await _service.ReadFriendAsync(vwm.FriendId, false);

        return View("ViewFriend", vwm);
    }
    public async Task<IActionResult> DeleteQuote(ViewFriendModel vwm, Guid itemId)
    {
        vwm.Friend = await _service.ReadFriendAsync(vwm.FriendId, false);
        vwm.Friend.Quotes = vwm.Friend.Quotes.Where(x => x.QuoteId != itemId).ToList();
        await _service.UpdateFriendAsync(new FriendCUdto(vwm.Friend));
        vwm.Friend = await _service.ReadFriendAsync(vwm.FriendId, false);

        return View("ViewFriend", vwm);
    }

    #endregion

    #region Edit details

    [HttpGet]
    public async Task<IActionResult> EditFriend()
    {
        var vwm = new EditFriendModel();
        if (Guid.TryParse(Request.Query["id"], out Guid _groupId))
        {
            //Read a music group from 
            var mg = await _service.ReadFriendAsync(_groupId, false);

            //Populate the InputModel from the music group
            vwm.FriendInput = new FriendIM(mg);
            vwm.PageHeader = "Edit details of a friend";

        }
        else
        {
            //Create an empty music group
            vwm.FriendInput = new FriendIM();
            vwm.FriendInput.StatusIM = StatusIM.Inserted;
            vwm.FriendInput.FirstName = null;
            vwm.FriendInput.LastName = null;
            vwm.FriendInput.Email = null;
            vwm.FriendInput.Birthday = null;
            vwm.FriendInput.Address = new AddressIM();
            vwm.FriendInput.Address.StatusIM = StatusIM.Inserted;

            vwm.PageHeader = "Create a new a friend";
        }

        return View("EditFriend", vwm);
    }
    [HttpPost]
    public async Task<IActionResult> Save(EditFriendModel vwm)
    {
        string[] keys = { "FriendInput.FirstName",
                            "FriendInput.LastName",
                            "FriendInput.Address.ZipCode",
                            "FriendInput.Address.StreetAddress",
                            "FriendInput.Address.City",
                            "FriendInput.Address.Country",};

        if (!ModelState.IsValidPartially(out ModelValidationResult validationResult, keys))
        {
            vwm.ValidationResult = validationResult;
            return View("EditFriend", vwm);
        }

        //This is where the music plays
        //First, are we creating a new Music group or editing another
        if (vwm.FriendInput.StatusIM == StatusIM.Inserted)
        {
            var newMg = await _service.CreateFriendAsync(vwm.FriendInput.CreateCUdto());
            //get the newly created FriendId
            vwm.FriendInput.FriendId = newMg.FriendId;
        }

        // Do update address
        var mg = await vwm.SaveAddress(_service);

        // Update the Friend itself
        mg = vwm.FriendInput.UpdateModel(mg);
        await _service.UpdateFriendAsync(new FriendCUdto(mg));

        if (vwm.FriendInput.StatusIM == StatusIM.Inserted)
        {
            return RedirectToAction("ListOfFriends", "Overview");
        }

        return Redirect($"ViewFriend?id={vwm.FriendInput.FriendId}");
    }
    

    #endregion

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
