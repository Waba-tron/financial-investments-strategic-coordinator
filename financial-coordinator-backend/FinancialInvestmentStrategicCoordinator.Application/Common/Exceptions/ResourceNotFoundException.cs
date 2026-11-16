namespace FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;

public class ResourceNotFoundException : AppException
{

    public ResourceNotFoundException(string resourceName, object key)
        : base($"{resourceName} with key '{key}' was not found.", "RESOURCE_NOT_FOUND", 404)
    {
    }
}
