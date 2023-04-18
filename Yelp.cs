using Chat.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Chat.Hubs;
using System;
using Yelp.Api; // Import Yelp API namespace
using System.Collections.Generic; // Import List

public class YelpAPI
{
  public async Task<List<Business>> SearchBusinessesAsync(string term)
  {
    var request = new Yelp.Api.Models.SearchRequest();
    request.Latitude = 49.282359695758885;
    request.Longitude = -123.1168886758965;
    request.Term = term;
    request.MaxResults = 40;
    request.OpenNow = true;
    DotNetEnv.Env.Load();
    var connectionString = Environment.GetEnvironmentVariable("YELP_API_KEY");
    var client = new Yelp.Api.Client(connectionString);
    var results = await client.SearchBusinessesAllAsync(request);
    
    // Create a list to hold the businesses
    var businesses = new List<Business>();
    
    // Loop through the results and add each business to the list
    foreach(var yelpBusiness in results.Businesses)
    {
       var business = new Business(
    yelpBusiness.Name,
    yelpBusiness.Location.Address1,
    yelpBusiness.Location.City,
    yelpBusiness.Location.State,
    yelpBusiness.Location.ZipCode,
    yelpBusiness.Rating,
    yelpBusiness.Price
);
        
        businesses.Add(business);
    }
    
    return businesses;
  }
}
