namespace ChatApp.Application.Common.Settings;

/// <summary>
/// Settings for the Cloudinary service.
/// </summary>
public class CloudinarySettings
{
    /// <summary>
    /// Gets or sets the cloud name.
    /// </summary>
    public string CloudName { get; set; }


    /// <summary>
    /// Gets or sets the api key.
    /// </summary>
    public string ApiKey { get; set; }


    /// <summary>
    /// Gets or sets the api secret.
    /// </summary>
    public string ApiSecret { get; set; }
}