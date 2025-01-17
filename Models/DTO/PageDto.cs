//purpose of DTO is to combine number of friends and pets living in the same city in OverviewCitiesModel
public class CitySummaryDTO
{
    public string City {get; set;}
    public int NrFriends {get; set;}
    public int NrPets {get; set;}
}

//purpose of DTO is to combine number of friends living in the same country in OverviewCountryModel
public class GroupedFriendsDTO
{
    public string Country { get; set; }
    public int NrFriends { get; set; }
    public int NrCities {get; set; }
}