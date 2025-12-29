using GoMeowee.Services.Interfaces;
using System.Windows.Input;

namespace GoMeowee.ViewModels;

public class LoginViewModel : BaseViewModel
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string? ErrorMessage { get; private set; }
    public bool HasError => ErrorMessage is not null;

    public LoginViewModel()
    {

    }
}
