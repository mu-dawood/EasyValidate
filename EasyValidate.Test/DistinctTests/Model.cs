using EasyValidate.Abstraction.Attributes;
using System.Collections.Generic;

namespace EasyValidate.Test.DistinctTests
{
    public partial class DistinctModel
    {
        [Distinct<int>]
        public IEnumerable<int>? UniqueCollection { get; set; }
    }
}
