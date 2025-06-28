using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Strings.CreditCard
{
    public partial class CreditCardModel
    {
        [CreditCard]
        public string CardNumber { get; set; } = string.Empty;

        [CreditCard]
        public string PaymentCard { get; set; } = string.Empty;

        [Optional, CreditCard]
        public string? OptionalCard { get; set; }
    }

    public partial class CreditCardNestedModel
    {
        [CreditCard]
        public string PrimaryCard { get; set; } = string.Empty;

        public CreditCardModel? NestedCards { get; set; }
    }
}
