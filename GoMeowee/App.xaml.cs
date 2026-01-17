using GoMeowee.Services.Interfaces;
using GoMeowee.Shells;

namespace GoMeowee
{
    public partial class App : Application
    {
        private readonly IAuthState _authState;
        private readonly IServiceProvider _services;

        public App(IAuthState authState, IServiceProvider services)
        {
            InitializeComponent();

            _authState = authState;
            _services = services;

            _authState.AuthStateChanged += OnAuthStateChanged;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            _ = InitializeAuth();
            return new Window(CreateRootPage()) { Width = 480};
        }

        private Page CreateRootPage()
        {
            return _authState.IsAuthenticated
                ? _services.GetRequiredService<AppShell>()
                : _services.GetRequiredService<AuthShell>();
        }

        private void OnAuthStateChanged()
        {
            if (Windows.Count == 0)
                return;

            Windows[0].Page = CreateRootPage();
        }

        private async Task InitializeAuth()
        {
            var authService = _services.GetRequiredService<IAuthService>();
            await authService.RestoreAuthStateAsync();
        }
    }
}