using System;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;
using Newtonsoft.Json;

namespace Services;

public class FriendsServiceWapi : IFriendsService
{
    private readonly ILogger<FriendsServiceWapi> _logger;
    private readonly HttpClient _httpClient;

    //To ensure Json deserializern is using the class implementations instead of interfaces 
    private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
    {
        Converters = {
            new AbstractConverter<csFriend, IFriend>(),
            new AbstractConverter<Address, IAddress>(),
            new AbstractConverter<Quote, IQuote>(),
            new AbstractConverter<Pet, IPet>()
        },
    };


    #region constructors
    public FriendsServiceWapi(IHttpClientFactory httpClientFactory, ILogger<FriendsServiceWapi> logger)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(name: "FriendsWebApi");
    }
    #endregion

    #region Admin Services

    public async Task<GstUsrInfoAllDto> SeedAsync(int nrOfItems) 
    {
        string uri = $"admin/seed?count={nrOfItems}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var info = JsonConvert.DeserializeObject<GstUsrInfoAllDto>(s);
        return info;
    }
    public async Task<GstUsrInfoAllDto> RemoveSeedAsync(bool seeded)
    {
        string uri = $"admin/removeseed?seeded={seeded}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var info = JsonConvert.DeserializeObject<GstUsrInfoAllDto>(s);
        return info;
    }

    #endregion

    #region Guest Services
    public async Task<GstUsrInfoAllDto> InfoAsync()
    {
        string uri = $"Guest/Info";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var info = JsonConvert.DeserializeObject<GstUsrInfoAllDto>(s);
        return info;

    }

    #endregion

    #region Friend CRUD
    public async Task<ResponsePageDto<IFriend>> ReadFriendsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        string uri = $"Friends/read?seeded={seeded}&flat={flat}&filter={filter}&pagenr={pageNumber}&pagesize={pageSize}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponsePageDto<IFriend>>(s, _jsonSettings);
        return resp;
    }

    public async Task<IFriend> ReadFriendAsync(Guid id, bool flat)
    {
        string uri = $"Friends/ReadItem?id={id}&flat={flat}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IFriend>(s, _jsonSettings);
        return resp;
    }

    public async Task<IFriend> DeleteFriendAsync(Guid id)
    {
        string uri = $"Friends/DeleteItem/{id}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IFriend>(s, _jsonSettings);
        return resp;
    }

    public async Task<IFriend> UpdateFriendAsync(FriendCUdto item)
    {
        string uri = $"Friends/UpdateItem/{item.FriendId}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IFriend>(s, _jsonSettings);
        return resp;
    }

    public async Task<IFriend> CreateFriendAsync(FriendCUdto item)
    {
        string uri = $"Friends/CreateItem/";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, item);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IFriend>(s, _jsonSettings);
        return resp;
    }

    #endregion

    #region Address CRUD
    public async Task<ResponsePageDto<IAddress>> ReadAddressesAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        string uri = $"Addresses/read?seeded={seeded}&flat={flat}&filter={filter}&pagenr={pageNumber}&pagesize={pageSize}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponsePageDto<IAddress>>(s, _jsonSettings);
        return resp;
    }

    public async Task<IAddress> ReadAddressAsync(Guid id, bool flat)
    {
        string uri = $"Addresses/ReadItem?id={id}&flat={flat}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IAddress>(s, _jsonSettings);
        return resp;
    }

    public async Task<IAddress> DeleteAddressAsync(Guid id)
    {
        string uri = $"Addresses/DeleteItem/{id}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IAddress>(s, _jsonSettings);
        return resp;
    }

    public async Task<IAddress> UpdateAddressAsync(AddressCUdto item)
    {
        string uri = $"Addresses/UpdateItem/{item.AddressId}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IAddress>(s, _jsonSettings);
        return resp;
    }

    public async Task<IAddress> CreateAddressAsync(AddressCUdto item)
    {
        string uri = $"Addresses/CreateItem/";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, item);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IAddress>(s, _jsonSettings);
        return resp;
    }

    public async Task<IAddress> UpsertAddressAsync(AddressCUdto item)
    {
        //temp list which will hold all addresses
        List<IAddress> Addresses = new List<IAddress>();

        // request to get DbItemsCount property so all addresses can be fetched at once  
        var infoResponse = await ReadAddressesAsync(item.Seeded, false, null, 0, 10);

        // request to fetch all Addresses with the known DbItemsCount
        var AddressesResponse = await ReadAddressesAsync(item.Seeded, false, null, 0, infoResponse.DbItemsCount);
        

        // adding Addresses to the temp list
        Addresses.AddRange(AddressesResponse.PageItems);

        // request to fetch all Addresses with the known DbItemsCount for opposite seeded
        AddressesResponse = await ReadAddressesAsync(!item.Seeded, false, null, 0, infoResponse.DbItemsCount);
        
        // adding Addresses to the temp list
        Addresses.AddRange(AddressesResponse.PageItems);

        // check if a address exists
        var address = Addresses.Where(a => (a.StreetAddress == item.StreetAddress) && 
                                        (a.ZipCode == item.ZipCode)&&
                                        (a.City == item.City)&&
                                        (a.Country == item.Country))
                                        .FirstOrDefault();



        if(address != null)
        {   
            item.AddressId = address.AddressId;
            return await UpdateAddressAsync(item);
        }
        else
        {
            //Create and insert a new Artist
            item.Seeded = false; 
            return await CreateAddressAsync(item);
        }

        
    }
    #endregion

    #region Quote CRUD
    public async Task<ResponsePageDto<IQuote>> ReadQuotesAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        string uri = $"Quotes/read?seeded={seeded}&flat={flat}&filter={filter}&pagenr={pageNumber}&pagesize={pageSize}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponsePageDto<IQuote>>(s, _jsonSettings);
        return resp;
    }

    public async Task<IQuote> ReadQuoteAsync(Guid id, bool flat)
    {
        string uri = $"Quotes/ReadItem?id={id}&flat={flat}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IQuote>(s, _jsonSettings);
        return resp;
    }

    public async Task<IQuote> DeleteQuoteAsync(Guid id)
    {
        string uri = $"Quotes/DeleteItem/{id}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IQuote>(s, _jsonSettings);
        return resp;
    }

    public async Task<IQuote> UpdateQuoteAsync(QuoteCUdto item)
    {
        string uri = $"Quotes/UpdateItem/{item.QuoteId}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IQuote>(s, _jsonSettings);
        return resp;
    }

    public async Task<IQuote> CreateQuoteAsync(QuoteCUdto item)
    {
        string uri = $"Quotes/CreateItem/";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, item);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IQuote>(s, _jsonSettings);
        return resp;
    }
    #endregion

    #region Pet CRUD
    public async Task<ResponsePageDto<IPet>> ReadPetsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        string uri = $"Pets/read?seeded={seeded}&flat={flat}&filter={filter}&pagenr={pageNumber}&pagesize={pageSize}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponsePageDto<IPet>>(s, _jsonSettings);
        return resp;
    }

    public async Task<IPet> ReadPetAsync(Guid id, bool flat)
    {
        string uri = $"Pets/ReadItem?id={id}&flat={flat}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IPet>(s, _jsonSettings);
        return resp;
    }

    public async Task<IPet> DeletePetAsync(Guid id)
    {
        string uri = $"Pets/DeleteItem/{id}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IPet>(s, _jsonSettings);
        return resp;
    }

    public async Task<IPet> UpdatePetAsync(PetCUdto item)
    {
        string uri = $"Pets/UpdateItem/{item.PetId}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, item);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IPet>(s, _jsonSettings);
        return resp;
    }

    public async Task<IPet> CreatePetAsync(PetCUdto item)
    {
        string uri = $"Quotes/CreateItem/";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, item);

        //Throw an exception if the response is not successful
        response.EnsureSuccessStatusCode();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<IPet>(s, _jsonSettings);
        return resp;
    }
    #endregion





}
public class AbstractConverter<TReal, TAbstract> : JsonConverter where TReal : TAbstract
{
    public override Boolean CanConvert(Type objectType)
        => objectType == typeof(TAbstract);

    public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        => serializer.Deserialize<TReal>(reader);

    public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        => serializer.Serialize(writer, value);
}

