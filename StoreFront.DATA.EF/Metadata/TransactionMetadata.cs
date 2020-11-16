using System.ComponentModel.DataAnnotations;

namespace StoreFront.DATA.EF.Metadata
{
    //TODO 1: add metadata
    class TransactionMetadata
    {
    }

    [MetadataType(typeof(TransactionMetadata))]
    public partial class Transaction
    { }
}
