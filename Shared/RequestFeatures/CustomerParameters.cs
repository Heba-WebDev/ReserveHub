namespace Shared.RequestFeatures;

public class CustomerParameters : RequestParameters
{
    public CustomerParameters() => OrderBy = "FirstName";
    public string? SearchTerm { get; set; }
}
