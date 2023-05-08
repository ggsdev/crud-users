using Newtonsoft.Json;

namespace ANPCentral.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string? Complement { get; set; }
        public User User { get; set; }
    }
}
