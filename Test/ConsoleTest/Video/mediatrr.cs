using EasyValidate.Abstractions;
using EasyValidate.Attributes;
using MediatR;

namespace ConsoleTest.Video;

public partial class CreateUser : IRequest<Guid>, IGenerate
{

    [NotEmpty, MaxLength(50)]
    public string Name { get; init; } = string.Empty;

    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [GreaterThan(0)]
    public int Age { get; init; }

    [NotEmpty]
    public string Address { get; init; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; init; } = string.Empty;

    public DateTime DateOfBirth { get; init; }

    public bool IsActive { get; init; }

    public List<string> Tags { get; init; } = [];
}

public sealed class ValidationBehavior<TRequest, TResponse>(IServiceProvider provider) :
 IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IServiceProvider _provider = provider;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct
    )
    {
        IValidationResult? result = null;
        
        // Check if the request implements IValidate or IAsyncValidate
        if (request is IValidate validate)
            result = validate.Validate((c) => c.SetServiceProvider(_provider));
        else if (request is IAsyncValidate validateAsync)
            result = await validateAsync.ValidateAsync((c) => c.SetServiceProvider(_provider));


        if (result != null && !result.IsValid())
        {
            throw new Exception(result.ToString())
            {
                Data = { ["Errors"] = result.Results }
            };
        }
        return await next(ct);
    }
}
