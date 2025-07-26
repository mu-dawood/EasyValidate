using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Strings.CreditCard
{
    public partial class CreditCardModel
 : IValidate
    {
        [CreditCard]
        public string CardNumber { get; set; } = string.Empty;

        [CreditCard]
        public string PaymentCard { get; set; } = string.Empty;

        [Optional, CreditCard]
        public string? OptionalCard { get; set; }
    }

    public partial class CreditCardNestedModel
 : IValidate
    {
        [CreditCard]
        public string PrimaryCard { get; set; } = string.Empty;

        public CreditCardModel? NestedCards { get; set; }
    }
}
