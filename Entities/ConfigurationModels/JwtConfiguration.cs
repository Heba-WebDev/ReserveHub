namespace Entities.ConfigurationModels;

public class JwtConfiguration
{
    public string Section { get; set; } = "JwtSettings";
    public string? ValidIssuer { get; set; }
    public string? Validaudience { get; set; }
    public string? SecretKey { get; set; }
    public string? Expries { get; set; }
}
