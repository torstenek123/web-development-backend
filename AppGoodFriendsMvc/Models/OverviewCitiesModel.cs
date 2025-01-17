using DbRepos;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services;

namespace AppGoodFriendsMvc.Models;

public class OverviewCitiesModel
{

    public List<CitySummaryDTO> Friends {get; set; }
    public List<CitySummaryDTO> Pets {get; set; }
    public List<CitySummaryDTO> FriendPetSummaries {get; set; } = new List<CitySummaryDTO>();
    public string Country {get; set; }

    public GstUsrInfoAllDto _info {get; set; }

    public OverviewCitiesModel()
    {
        
    }
}



