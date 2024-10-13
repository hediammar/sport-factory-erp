using System.ComponentModel.DataAnnotations;

namespace SportFactoryApp
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)] // Length can be adjusted based on password hashing
        public string Password { get; set; }

        [Required]
        [MaxLength(50)] // Length can be adjusted based on password hashing
        public string role { get; set; }
    }
}
