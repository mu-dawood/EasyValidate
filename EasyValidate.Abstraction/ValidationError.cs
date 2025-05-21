using System.Collections.Generic;

namespace EasyValidate.Abstraction
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}