namespace TravelAgency.Model
{
    public class Location
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public Location()
        {
            Id = -1;
            Country = "";
            City = "";
        }

        public Location(int id, string country, string city)
        {
            Id = id;
            Country = country;
            City = city;
        }
    }
}
