using System;
using System.Collections.Generic;

namespace EasyValidate.Abstraction
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public object[] Args { get; set; }
        public string AttributeName { get; set; }
        public string FormattedMessage { get; internal set; }
    }
}