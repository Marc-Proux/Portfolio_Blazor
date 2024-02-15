namespace Portfolio.Model.Smtp;

public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool UseDefaultCredentials { get; set; }
    public bool EnableSsl { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string MyAddr {get; set;} = string.Empty;
    public string MyName {get; set;} = string.Empty;
    public string MyMessageDisplayName {get; set;} = string.Empty;
}