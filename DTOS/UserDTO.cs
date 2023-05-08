namespace ANPCentral.DTOS
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public AddressDto? Address { get; set; }
    }

    public class AddressDto
    {
        public Guid AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string? Complement { get; set; }
    }
}
