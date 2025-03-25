using System.ComponentModel.DataAnnotations;
using Test_Store_ADONET.Common;

namespace Test_Store_ADONET.DTOs
{
    public class LoginDTO
    {
        /// <summary>
        /// Represents a username with validation rules. It must be provided, cannot exceed 50 characters, and can only
        /// include letters, numbers, spaces, and hyphens.
        /// </summary>
        [Required(ErrorMessage = "The username is required.")]
        [MaxLength(50, ErrorMessage = "The username cannot exceed 50 characters.")]
        [RegularExpression(RegexPatterns.BrandName, ErrorMessage = "The username can only contain letters, numbers, spaces, and hyphens.")]
        public string Username { get; set; }

        /// <summary>
        /// Represents a password with validation rules. It must be provided, cannot exceed 50 characters, and can only
        /// include letters, numbers, spaces, and hyphens.
        /// </summary>
        [Required(ErrorMessage = "The password is required.")]
        [MaxLength(50, ErrorMessage = "The password cannot exceed 50 characters.")]
        [RegularExpression(RegexPatterns.BrandName, ErrorMessage = "The password can only contain letters, numbers, spaces, and hyphens.")]
        public string Password { get; set; }
    }
}
