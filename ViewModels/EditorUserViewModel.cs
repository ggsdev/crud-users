using System.ComponentModel.DataAnnotations;

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

    }
}
