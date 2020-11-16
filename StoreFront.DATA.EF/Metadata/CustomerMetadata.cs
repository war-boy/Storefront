using System.ComponentModel.DataAnnotations;

namespace StoreFront.DATA.EF.Metadata
{
    class CustomerMetadata
    {

    }

    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    { }
}
