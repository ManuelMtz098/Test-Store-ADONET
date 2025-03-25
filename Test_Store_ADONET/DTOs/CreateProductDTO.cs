using System.ComponentModel.DataAnnotations;
using Test_Store_ADONET.Common;

namespace Test_Store_ADONET.DTOs
{
    public class CreateProductDTO
    {
        /// <summary>
        /// Represents the product name with validation for required input, maximum length, and allowed characters.
        /// Ensures the name is a valid format.
        /// </summary>
        [Required(ErrorMessage = "The product name is required.")]
        [MaxLength(100, ErrorMessage = "The product name cannot exceed 255 characters.")]
        [RegularExpression(RegexPatterns.BrandName, ErrorMessage = "The product name can only contain letters, numbers, spaces, and hyphens.")]
        public string Name { get; set; }

        /// <summary>
        /// Represents the product description, which is required and must not exceed 1000 characters. It can only
        /// contain letters, numbers, spaces, and hyphens.
        /// </summary>
        [Required(ErrorMessage = "Theproduct description is required.")]
        [MaxLength(100, ErrorMessage = "The product description cannot exceed 1000 characters.")]
        [RegularExpression(RegexPatterns.BrandName, ErrorMessage = "The product description can only contain letters, numbers, spaces, and hyphens.")]
        public string Description { get; set; }

        /// <summary>
        /// Represents the unique identifier for a brand. It is a required field that must be provided.
        /// </summary>
        [Required(ErrorMessage = "The brand ID is required.")]
        public Guid IdBrand { get; set; }
    }
}
