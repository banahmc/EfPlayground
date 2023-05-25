using FluentValidation.Results;

namespace Model.Common.Results;

public record ValidationFailed
{
    public ValidationFailed(IDictionary<string, string[]> errors)
    {
        Errors = errors;
    }

    public ValidationFailed(IEnumerable<ValidationFailure> validationFailures)
    {
        Errors = validationFailures
            .GroupBy(x => x.PropertyName)
            .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage).ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
