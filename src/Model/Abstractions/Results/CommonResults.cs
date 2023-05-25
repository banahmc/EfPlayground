namespace Model.Abstractions.Results;

public record NotFound();

public record ValidationFailed(IEnumerable<string> Errors);
