using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TravelRecordApp.Common;

namespace TravelRecordApp.Models
{
    public class Location
    {
        public string Address { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int Distance { get; set; }
        public string Cc { get; set; }
        public string Country { get; set; }
        public IList<string> FormattedAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CrossStreet { get; set; }
    }

    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PluralName { get; set; }
        public string ShortName { get; set; }
    }

    public class Venue
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public IList<Category> Categories { get; set; }

        public static async Task<List<Venue>> GetVenuesAsync(double latitude, double longitude)
        {
            var venues = new List<Venue>();

            var url = GenerateURL(latitude, longitude);
            using (var client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                string json = await responseMessage.Content.ReadAsStringAsync();

                var venueRoot = JsonConvert.DeserializeObject<VenueRoot>(json);
                venues = venueRoot.Response.Venues as List<Venue>;
            }
            return venues;
        }

        private static string GenerateURL(double latitude, double longitude)
        {
            return string.Format(Globals.FOURSQUARE_VENUES_URL, latitude, longitude, Globals.FOURSQUARE_CLIENT_ID, Globals.FOURSQUARE_CLIENT_SECRET, DateTime.Now.ToString("yyyyMMdd"));
        }
    }

    public class Response
    {
        public IList<Venue> Venues { get; set; }
    }

    public class VenueRoot
    {
        public Response Response { get; set; }
    }
}
