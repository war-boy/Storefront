using System.ComponentModel.DataAnnotations;

namespace StoreFront.DATA.EF
{
    class ProductMetadata
    {
        [Required]
        [Display (Name = "Product Name")]
        [StringLength(25, ErrorMessage = "* Product Name must be less than 25 characters.")]
        [DisplayFormat(NullDisplayText = "* Product name is required")]
        public string ProductName { get; set; }

        [Required]
        [Display (Name = "Category")]
        public int CategoryID { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal? Price { get; set; }

        [Required]
        public string Condition { get; set; }

        [StringLength(200, ErrorMessage = "* Description must be less than 200 characters.")]
        [Required]
        [UIHint("MultilineText")]
        public string Description { get; set; }

       
        [Display(Name = "Prouct Image")]
        public string ProductImage { get; set; }

        [Display(Name = "In Stock")]
        public bool? InStock { get; set; }

        [Display(Name = "Units in Stock")]
        public int? UnitsInStock { get; set; }

        public string Source { get; set; }


    }

    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    { }
}
