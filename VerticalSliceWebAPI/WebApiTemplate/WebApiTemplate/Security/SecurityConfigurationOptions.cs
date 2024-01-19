namespace WebApiTemplate.Security;

public class SecurityConfigurationOptions
{
    public const string ConfigurationSectionName = "Security";

    public CorsOptions Cors { get; set; } = new();
}

public class CorsOptions
{
    public List<string> Origins { get; set; } = [];

    public override string ToString() => string.Join(',', Origins);
}
