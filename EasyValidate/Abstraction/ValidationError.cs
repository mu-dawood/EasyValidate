namespace EasyValidate.Abstraction
{
    public class ValidationError
    {
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public object?[] Args { get; set; } = [];
        public string AttributeName { get; set; } = string.Empty;
        public string FormattedMessage { get; internal set; } = string.Empty;
    }
}