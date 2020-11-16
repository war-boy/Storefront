using System.ComponentModel.DataAnnotations;

namespace StoreFront.DATA.EF.Metadata
{
    class EmployeeMetadata
    {
    }

    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    { }
}
