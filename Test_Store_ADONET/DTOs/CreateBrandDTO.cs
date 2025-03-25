using System.ComponentModel.DataAnnotations;
using Test_Store_ADONET.Common;

namespace Test_Store_ADONET.DTOs
{
    public class CreateBrandDTO
    {
        /// <summary>
        /// Represents the brand name with validation for required input, maximum length of 100 characters, and specific
        /// character restrictions.
        ///Ensures the name contains only letters, numbers, spaces, and hyphens.
        /// </summary>
        [Required(ErrorMessage = "The brand name is required.")]
        [MaxLength(100, ErrorMessage = "The brand name cannot exceed 100 characters.")]
        [RegularExpression(RegexPatterns.BrandName, ErrorMessage = "The brand name can only contain letters, numbers, spaces, and hyphens.")]
        public string Name { get; set; }
    }
}
