using EasyValidate.Abstraction.Attributes;
using System.Collections.Generic;

namespace EasyValidate.Test.NoDuplicatesTests
{
    public partial class NoDuplicatesModel
    {
        [Distinct]
        public IEnumerable<int>? UniqueCollection { get; set; }
    }
}
