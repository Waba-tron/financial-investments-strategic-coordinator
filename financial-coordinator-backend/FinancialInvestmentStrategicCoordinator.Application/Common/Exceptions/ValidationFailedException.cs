namespace FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;

public class ValidationFailedException : AppException
{
    /// <summary>PascalCase property name -> list of error messages.</summary>
    public IReadOnlyDictionary<string, string[]> ValidationErrors { get; }

    public ValidationFailedException(IReadOnlyDictionary<string, string[]> validationErrors)
        : base("One or more validation errors occurred.", "VALIDATION_FAILED", 422)
    {
        ValidationErrors = validationErrors;
    }
}
