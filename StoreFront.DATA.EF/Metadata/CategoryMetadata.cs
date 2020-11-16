using System.ComponentModel.DataAnnotations;

namespace StoreFront.DATA.EF
{
    class CategoryMetadata
    {
        [Display(Name = "Category")]
        [Required]
        [DisplayFormat(NullDisplayText = "* Category name is required")]
        public string CategoryName { get; set; }

        [StringLength(50, ErrorMessage = "* Description must be less than 50 characters.")]
        public string Description { get; set; }
    }

    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {

    }
}
