
namespace uploadBase.Shared.Models;
public class AuthSetting
{
    public string SecretKey { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;

}

public class CorsPolicySetting
{
    public string Name { get; set; }
    public string[] AllowHeaders { get; set; }
    public string[] AllowMethods { get; set; }

    public string[] AllowOrigins { get; set; }

}

public class PathSetting
{
    public string Share { get; set; }
    public string Upload { get; set; }
    public string Stream { get; set; }

    public string Template { get; set; }

    public string Base { get; set; }
}


public class TemplateSetting
{
    public string User { get; set; }

}