namespace FrontEnd.Models;

public class LoginForm
{
    public string? Usn { get; set; }
    public string? Login { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ReturnUrl { get; set; }
    public bool? RememberMe { get; set; }
}