using ANPCentral.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ANPCentral.ViewModels
{
    public class EditorUserViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public AddressViewModel? Address { get; set; }

    }
    public class AddressViewModel
    {
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        public string? Complement { get; set; } = null;
    }

}
