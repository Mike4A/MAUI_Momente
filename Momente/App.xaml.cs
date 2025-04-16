namespace Momente
{
    public partial class App : Application
    {
        public App()
        {
            Application.Current!.UserAppTheme = AppTheme.Dark; //Application.Current.RequestedTheme;
            InitializeComponent();            
            MainPage = new AppShell();           
        }
    }
}
