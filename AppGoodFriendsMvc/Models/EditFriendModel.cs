using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using AppGoodFriendsMvc.SeidoHelpers;
using Models;
using Newtonsoft.Json;
using Services;
using Models.DTO;
using DbRepos;

namespace AppGoodFriendsMvc.Models;

public class EditFriendModel
{
    [BindProperty]
    public FriendIM FriendInput { get; set; }

    [BindProperty]
    public string PageHeader { get; set; }

    //For Validation
    public ModelValidationResult ValidationResult { get; set; } = new ModelValidationResult(false, null, null);

    #region methods
    public async Task<IFriend> SaveAddress(IFriendsService _service)
    {

        //fetching friend and address from db
        var mg = await _service.ReadFriendAsync(FriendInput.FriendId, false);
        var model = mg.Address;

        //modified address
        var modifiedAddress = FriendInput.Address;
        
        

        //updating or creating address in db
        if(modifiedAddress.StatusIM == StatusIM.Inserted || modifiedAddress.AddressId == default)
        {
            var cuDto = modifiedAddress.CreateCUdto();
            cuDto.FriendsId = new List<Guid>();
            cuDto.FriendsId.Add(mg.FriendId);
            model = await _service.UpsertAddressAsync(cuDto);
        }
        else 
        {
            //updating model from modified 
            model = modifiedAddress.UpdateModel(model);
            await _service.UpdateAddressAsync(new AddressCUdto(model));
        }

        mg.Address = model;
        return mg;
    }
    #endregion

    #region Input Model 
    public enum StatusIM { Unknown, Unchanged, Inserted, Modified, Deleted}
    
    public class AddressIM
    {
        public StatusIM StatusIM { get; set; }

        public Guid AddressId { get; set; }

        [Required(ErrorMessage = "You must enter a street address")]
        public virtual string StreetAddress { get; set; }

        [Required(ErrorMessage = "You must enter a zip code")]
        public virtual int ZipCode { get; set; }

        [Required(ErrorMessage = "You must enter a city")]
        public virtual string City { get; set; }

        [Required(ErrorMessage = "You must enter a country")]
        public virtual string Country { get; set; }

        public AddressIM() { }
        public AddressIM(AddressIM original)
        {
            StatusIM = original.StatusIM;
            AddressId = original.AddressId;
            StreetAddress = original.StreetAddress;
            ZipCode = original.ZipCode;
            City = original.City;
            Country = original.Country;
        }
        public AddressIM(IAddress model)
        {
            StatusIM = StatusIM.Unchanged;
            AddressId = model.AddressId;
            StreetAddress = model.StreetAddress;
            ZipCode =  model.ZipCode;
            City =  model.City;
            Country =  model.Country;
        }
        
        //to update the model in database
        public IAddress UpdateModel(IAddress model)
        {
            model.AddressId = this.AddressId;
            model.StreetAddress = this.StreetAddress;
            model.ZipCode = this.ZipCode;
            model.City = this.City;
            model.Country = this.Country;
            return model;
        }

        //to create new album in the database
        public AddressCUdto CreateCUdto () => new AddressCUdto(){

            AddressId = null,
            StreetAddress = this.StreetAddress,
            ZipCode = this.ZipCode,
            City = this.City,
            Country = this.Country,
        };
    }
    public class FriendIM
    {
        public StatusIM StatusIM { get; set; }

        public Guid FriendId { get; set; }

        [Required(ErrorMessage = "You must provide a first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must provide a last name")]
        public string LastName { get; set; }

        public string? Email { get; set; }

        public DateTime? Birthday { get; set; }

        public AddressIM Address { get; set; }

        public FriendIM() {}
        public FriendIM(IFriend model)
        {
            StatusIM = StatusIM.Unchanged;
            FriendId = model.FriendId;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Email = model.Email;
            Birthday = model.Birthday;

            if(model.Address != null)
            {
                Address = new AddressIM(model.Address);
            }
            

        }

        //to update the model in database
        public IFriend UpdateModel(IFriend model)
        {
            model.FirstName = this.FirstName;
            model.LastName = this.LastName;
            model.Email = this.Email;
            model.Birthday = this.Birthday;
            return model;
        }

        //to create new music group in the database
        public FriendCUdto CreateCUdto () => new (){
            
            FriendId = null,
            FirstName = this.FirstName,
            LastName = this.LastName,
            Email = this.Email,
            Birthday = this.Birthday,
        };

              
    }
    #endregion

    public EditFriendModel()
    {
        
    }
}



