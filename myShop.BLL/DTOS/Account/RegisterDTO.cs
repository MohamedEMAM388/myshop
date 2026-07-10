using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.DTOS.Account
{
    public class RegisterDTO
    {

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, MinimumLength = 5,
              ErrorMessage = "Address must be between 5 and 200 characters.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "City must be between 2 and 100 characters.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
